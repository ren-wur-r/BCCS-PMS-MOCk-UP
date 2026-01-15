<script setup lang="ts">
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";
import { formatDateTime } from "@/utils/timeFormatter";
import { getEmployeePipelineSalerStatusLabel } from "@/utils/getEmployeePipelineSalerStatusLabel";

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
// -----------------------------------------------------------------
/** 待處理業務指派項目 */
interface PendingEmployeePipelineSalerItem {
  /** 商機業務ID */
  employeePipelineSalerID: number | null;
  /** 商機業務-業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum | null;
  /** 商機業務-建立員工名稱(指派人員) */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-是否有拒絕權限 */
  hasRejectPermissions: boolean;
  /** 商機業務-是否有接受權限 */
  hasAcceptPermissions: boolean;
  /** 商機業務-是否有轉指派權限 */
  hasReassignPermissions: boolean;
}

/** 業務指派記錄項目 */
interface EmployeePipelineSalerItem {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 商機業務-業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-業務回覆時間 */
  employeePipelineSalerReplyTime: string | null;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立員工名稱(指派人員) */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-備註 */
  employeePipelineSalerRemark: string;
}

interface Props {
  /** 待處理業務指派 */
  pendingEmployeePipelineSaler: PendingEmployeePipelineSalerItem | null;
  /** 業務指派記錄列表 */
  employeePipelineSalerList: EmployeePipelineSalerItem[];
  /** 是否為唯讀模式 */
  readonly?: boolean;
}

interface Emits {
  /** 接受指派 */
  (e: "acceptAssignment"): void;
  /** 拒絕指派 */
  (e: "rejectAssignment"): void;
  /** 轉指派業務 */
  (e: "reassignSaler"): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: false,
});

const emit = defineEmits<Emits>();

/** 取得業務狀態樣式 */
const getEmployeePipelineSalerStatusClass = (
  status: DbAtomEmployeePipelineSalerStatusEnum
): string => {
  switch (status) {
    case DbAtomEmployeePipelineSalerStatusEnum.Pending:
      return "bg-yellow-100 text-yellow-800 px-2 py-1 text-xs rounded-md";
    case DbAtomEmployeePipelineSalerStatusEnum.Accepted:
      return "bg-green-100 text-green-800 px-2 py-1 text-xs rounded-md";
    case DbAtomEmployeePipelineSalerStatusEnum.Rejected:
      return "bg-red-100 text-red-800 px-2 py-1 text-xs rounded-md";
    case DbAtomEmployeePipelineSalerStatusEnum.Reassigned:
      return "bg-blue-100 text-blue-800 px-2 py-1 text-xs rounded-md";
    default:
      return "bg-gray-100 text-gray-800 px-2 py-1 text-xs rounded-md";
  }
};
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex justify-between items-center">
      <h2 class="subtitle">指派業務記錄</h2>
    </div>

    <!-- 待處理業務指派 -->
    <div v-if="pendingEmployeePipelineSaler" class="mb-6">
      <div class="border border-yellow-300 rounded-lg bg-yellow-50 p-4">
        <div class="flex items-center justify-between gap-4">
          <div class="flex justify-center items-center gap-4">
            <div>
              <span class="bg-yellow-100 text-yellow-800 px-2 py-1 text-sm rounded-md">
                {{
                  getEmployeePipelineSalerStatusLabel(
                    pendingEmployeePipelineSaler.employeePipelineSalerStatus
                  )
                }}
              </span>
            </div>

            <div class="flex gap-12 text-sm">
              <!-- 指派時間 -->
              <div>
                <span class="text-gray-400">指派時間：</span>
                <span class="text-black">
                  {{
                    formatDateTime(pendingEmployeePipelineSaler.employeePipelineSalerCreateTime) ||
                    "-"
                  }}
                </span>
              </div>
              <!-- 指派業務 -->
              <div>
                <span class="text-gray-400">指派業務：</span>
                <span class="text-black">
                  {{ pendingEmployeePipelineSaler.employeePipelineSalerEmployeeName || "-" }}
                </span>
              </div>
              <!-- 指派人員 -->
              <div>
                <span class="text-gray-400">指派人員：</span>
                <span class="text-black">
                  {{ pendingEmployeePipelineSaler.employeePipelineSalerCreateEmployeeName || "-" }}
                </span>
              </div>
            </div>
          </div>

          <!-- 操作按鈕 -->
          <div v-if="!props.readonly" class="flex gap-2">
            <button
              v-if="
                pendingEmployeePipelineSaler.hasRejectPermissions &&
                employeeInfoStore.hasPermission(menu, 'edit')
              "
              class="btn px-4 py-2 bg-red-500 hover:bg-red-600 text-white"
              @click="emit('rejectAssignment')"
            >
              拒絕
            </button>
            <button
              v-if="
                pendingEmployeePipelineSaler.hasAcceptPermissions &&
                employeeInfoStore.hasPermission(menu, 'edit')
              "
              class="btn px-4 py-2 bg-green-500 hover:bg-green-600 text-white"
              @click="emit('acceptAssignment')"
            >
              接受
            </button>
            <button
              v-if="
                pendingEmployeePipelineSaler.hasReassignPermissions &&
                employeeInfoStore.hasPermission(menu, 'edit')
              "
              class="btn-update px-4 py-2"
              @click="emit('reassignSaler')"
            >
              轉指派業務
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- 指派業務歷史記錄 -->
    <div>
      <div class="overflow-y-auto max-h-48">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white sticky top-0">
            <tr>
              <th class="text-start">指派時間</th>
              <th class="text-start">回覆時間</th>
              <th class="text-start">業務人員</th>
              <th class="text-start">指派人員</th>
              <th class="text-center">狀態</th>
              <th class="text-start">備註</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in employeePipelineSalerList"
              :key="item.employeePipelineSalerID || index"
              class="border border-gray-300"
            >
              <td class="text-start">
                {{ formatDateTime(item.employeePipelineSalerCreateTime) }}
              </td>
              <td class="text-start">
                {{
                  item.employeePipelineSalerReplyTime
                    ? formatDateTime(item.employeePipelineSalerReplyTime)
                    : "-"
                }}
              </td>
              <td class="text-start">{{ item.employeePipelineSalerEmployeeName }}</td>
              <td class="text-start">{{ item.employeePipelineSalerCreateEmployeeName }}</td>
              <td class="text-center">
                <span
                  :class="getEmployeePipelineSalerStatusClass(item.employeePipelineSalerStatus)"
                >
                  {{ getEmployeePipelineSalerStatusLabel(item.employeePipelineSalerStatus) || "-" }}
                </span>
              </td>
              <td class="text-start">{{ item.employeePipelineSalerRemark || "-" }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
