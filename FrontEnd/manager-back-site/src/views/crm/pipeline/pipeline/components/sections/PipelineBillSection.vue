<script setup lang="ts">
import { ref } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
import { formatDate } from "@/utils/timeFormatter";
import { getEmployeePipelineBillStatusLabel } from "@/utils/getEmployeePipelineBillStatusLabel";
import { formatCurrency } from "@/utils/currencyFormatter";

const VAT_RATE = 0.05;
const toTaxIncluded = (amount: number) => Math.round(amount * (1 + VAT_RATE));

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
const isExpanded = ref(true);
// -----------------------------------------------------------------
/** 發票記錄項目 */
interface BillItem {
  /** 商機發票紀錄ID */
  employeePipelineBillID: number | null;
  /** 發票號碼 */
  employeePipelineBillBillNumber: string | null;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 發票種類：先開發票/後執行 */
  employeePipelineBillIsPreIssued: boolean;
  /** 預計執行日 */
  employeePipelineBillExecuteDate: string | null;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string | null;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}

interface Props {
  /** 發票記錄列表 */
  billList: BillItem[];
  /** 總金額（來自產品成交價總和） */
  totalAmount: number;
  /** 分期期數 */
  periodCount: number;
  /** 是否為唯讀模式 */
  readonly?: boolean;
  /** 是否允許編輯多筆 */
  canEditMultipleBills: boolean;
  /** 是否顯示每列「確認開立」按鈕 */
  showConfirmButton: boolean;
  /** 是否顯示每列「通知開立」按鈕 */
  showNotifyButton: boolean;
}

interface Emits {
  /** 編輯多筆發票記錄 */
  (e: "edit-multiple-bills"): void;
  /** 通知開立發票（未結案狀態） */
  (e: "notify-bill-issue", bill: BillItem, index: number): void;
  /** 確認開立發票（處理中狀態） */
  (e: "confirm-bill-issue", bill: BillItem, index: number): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: true,
  canEditMultipleBills: false,
  showConfirmButton: false,
  showNotifyButton: false,
});

const emit = defineEmits<Emits>();

/** 取得發票狀態樣式 */
const getBillStatusClass = (status: DbAtomEmployeePipelineBillStatusEnum): string => {
  switch (status) {
    case DbAtomEmployeePipelineBillStatusEnum.InProgress:
      return "bg-yellow-100 text-yellow-800";
    case DbAtomEmployeePipelineBillStatusEnum.Completed:
      return "bg-green-100 text-green-800";
    case DbAtomEmployeePipelineBillStatusEnum.NotCompleted:
      return "bg-gray-100 text-gray-800";
    default:
      return "bg-gray-100 text-gray-800";
  }
};
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex items-center justify-between">
      <div class="h-8 flex items-center gap-2 cursor-pointer" @click="isExpanded = !isExpanded">
        <h2 class="subtitle">發票記錄</h2>
        <!-- 展開/收合圖示 -->
        <svg
          class="w-5 h-5 transition-transform"
          :class="{ 'rotate-180': !isExpanded }"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M19 9l-7 7-7-7"
          />
        </svg>
      </div>

      <span
        v-if="
          props.canEditMultipleBills &&
          !props.readonly &&
          employeeInfoStore.hasPermission(menu, 'edit') &&
          isExpanded
        "
      ></span>
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded" class="flex flex-col gap-4">
      <!-- 總金額與分期期數卡片 -->
      <div class="grid grid-cols-4 gap-4">
        <div class="p-4 bg-blue-50 rounded-lg border border-blue-200">
          <div class="text-sm text-blue-600 mb-1">總金額（含稅）</div>
          <div class="text-2xl font-bold text-blue-900">
            {{ formatCurrency(toTaxIncluded(totalAmount)) }}
          </div>
        </div>
        <div class="p-4 bg-green-50 rounded-lg border border-green-200">
          <div class="text-sm text-green-600 mb-1">分期期數</div>
          <div class="text-2xl font-bold text-green-900">{{ periodCount }} 期</div>
        </div>
      </div>

      <div v-if="!props.billList.length"></div>

      <div v-else>
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-center w-24">期數</th>
              <th class="text-center w-36">發票日期</th>
              <th class="text-center w-24">發票種類</th>
              <th class="text-center w-36">預計執行日</th>
              <th class="text-start w-36">發票號碼</th>
              <th class="text-end w-36">含稅金額</th>
              <th class="text-start">備註</th>
              <th class="text-center w-28">狀態</th>
              <th v-if="showConfirmButton || showNotifyButton" class="text-center w-40">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(bill, index) in billList"
              :key="bill.employeePipelineBillID ?? index"
              class="border border-gray-300"
            >
              <td class="text-center">第 {{ bill.employeePipelineBillPeriodNumber }} 期</td>
              <td class="text-center">
                {{ formatDate(bill.employeePipelineBillBillTime) || "-" }}
              </td>
              <td class="text-center">
                {{ bill.employeePipelineBillIsPreIssued ? "先開發票" : "-" }}
              </td>
              <td class="text-center">
                {{
                  bill.employeePipelineBillIsPreIssued
                    ? formatDate(bill.employeePipelineBillExecuteDate || "") || "-"
                    : "-"
                }}
              </td>
              <td class="text-start">{{ bill.employeePipelineBillBillNumber || "-" }}</td>
              <td class="text-end">
                {{ formatCurrency(toTaxIncluded(bill.employeePipelineBillNoTaxAmount)) }}
              </td>
              <td class="text-start truncate">{{ bill.employeePipelineBillRemark || "-" }}</td>
              <td class="text-center">
                <span
                  class="px-2 py-1 text-sm rounded-md"
                  :class="getBillStatusClass(bill.employeePipelineBillStatus)"
                >
                  {{ getEmployeePipelineBillStatusLabel(bill.employeePipelineBillStatus) }}
                </span>
              </td>
              <td v-if="showConfirmButton || showNotifyButton" class="text-center">
                <div class="flex gap-2 justify-center">
                  <button
                    v-if="
                      bill.employeePipelineBillStatus ===
                        DbAtomEmployeePipelineBillStatusEnum.InProgress &&
                      showConfirmButton &&
                      employeeInfoStore.hasPermission(menu, 'edit')
                    "
                    class="btn-update text-sm px-2 py-1"
                    @click="emit('confirm-bill-issue', bill, index)"
                  >
                    確認開立
                  </button>
                  <button
                    v-if="
                      bill.employeePipelineBillStatus ===
                        DbAtomEmployeePipelineBillStatusEnum.NotCompleted &&
                      showNotifyButton &&
                      employeeInfoStore.hasPermission(menu, 'edit')
                    "
                    class="btn-update text-sm px-2 py-1"
                    @click="emit('notify-bill-issue', bill, index)"
                  >
                    通知開立
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <button
        v-if="
          props.canEditMultipleBills &&
          !props.readonly &&
          employeeInfoStore.hasPermission(menu, 'edit')
        "
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
        @click="emit('edit-multiple-bills')"
      >
        新增發票紀錄
      </button>
    </div>
  </div>
</template>
