<script setup lang="ts">
import { ref, watch } from "vue";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
import { formatDate } from "@/utils/timeFormatter";

interface BillItem {
  /** 商機發票紀錄ID */
  employeePipelineBillID: number | null;
  /** 發票號碼 */
  employeePipelineBillBillNumber: string | null;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string | null;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}

interface Props {
  show: boolean;
  /** 當前發票資料 */
  bill: BillItem | null;
}

interface Emits {
  (
    e: "confirm",
    data: {
      employeePipelineBillID: number;
      employeePipelineBillNumber: string;
      employeePipelineBillRemark: string;
    }
  ): void;
  (e: "cancel"): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

/** 發票號碼 */
const employeePipelineBillNumber = ref("");

/** 備註 */
const employeePipelineBillRemark = ref("");

/** 格式化金額顯示 */
const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat("zh-TW", {
    style: "currency",
    currency: "TWD",
    minimumFractionDigits: 0,
  }).format(amount);
};

/** 載入資料 */
const loadData = () => {
  if (props.bill) {
    employeePipelineBillNumber.value = props.bill.employeePipelineBillBillNumber || "";
    employeePipelineBillRemark.value = props.bill.employeePipelineBillRemark || "";
  }
};

/** 重置表單 */
const resetForm = () => {
  employeePipelineBillNumber.value = "";
  employeePipelineBillRemark.value = "";
};

/** 監聽 show 和 bill 變化 */
watch(
  () => [props.show, props.bill],
  ([newShow]) => {
    if (newShow) {
      loadData();
    } else {
      resetForm();
    }
  },
  { immediate: true }
);

/** 處理確認 */
const handleConfirm = () => {
  if (!props.bill?.employeePipelineBillID) {
    alert("發票資料錯誤");
    return;
  }

  emit("confirm", {
    employeePipelineBillID: props.bill.employeePipelineBillID,
    employeePipelineBillNumber: employeePipelineBillNumber.value,
    employeePipelineBillRemark: employeePipelineBillRemark.value,
  });
};

/** 處理取消 */
const handleCancel = () => {
  emit("cancel");
};
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">確認開立發票</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-6 overflow-y-auto flex-1">
        <div v-if="bill" class="space-y-4">
          <!-- 唯讀資訊區塊 -->
          <div class="bg-gray-50 p-4 rounded-lg border border-gray-200 space-y-3">
            <div class="flex justify-between items-center">
              <span class="text-sm text-gray-500">發票期數</span>
              <span class="font-bold text-gray-900"
                >第 {{ bill.employeePipelineBillPeriodNumber }} 期</span
              >
            </div>
            <div class="flex justify-between items-center">
              <span class="text-sm text-gray-500">發票日期</span>
              <span class="font-medium text-gray-900">{{
                formatDate(bill.employeePipelineBillBillTime)
              }}</span>
            </div>
            <div class="flex justify-between items-center">
              <span class="text-sm text-gray-500">未稅金額</span>
              <span class="font-medium text-gray-900">{{
                formatCurrency(bill.employeePipelineBillNoTaxAmount)
              }}</span>
            </div>
          </div>

          <!-- 發票號碼 -->
          <div class="flex flex-col gap-2">
            <label class="form-label">發票號碼</label>
            <input
              v-model="employeePipelineBillNumber"
              type="text"
              class="input-box"
              placeholder="請輸入發票號碼"
            />
          </div>

          <!-- 備註 -->
          <div class="flex flex-col gap-2">
            <label class="form-label">備註</label>
            <textarea
              v-model="employeePipelineBillRemark"
              rows="4"
              class="textarea-box resize-none"
              placeholder="請輸入備註..."
            ></textarea>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleConfirm">確定</button>
        </div>
      </div>
    </div>
  </div>
</template>
