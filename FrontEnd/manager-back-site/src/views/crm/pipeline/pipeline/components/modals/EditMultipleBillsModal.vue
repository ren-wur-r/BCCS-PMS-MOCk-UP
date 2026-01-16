<script setup lang="ts">
import { ref, watch, computed } from "vue";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
import { formatToServerDateStartISO8 } from "@/utils/timeFormatter";

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
  show: boolean;
  /** 總金額（唯讀） */
  totalAmount: number;
  /** 當前分期期數 */
  currentPeriodCount: number;
  /** 當前發票列表 */
  currentBills: BillItem[];
}

interface Emits {
  (
    e: "confirm",
    data: {
      periodCount: number;
      billList: BillItem[];
    }
  ): void;
  (e: "cancel"): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

/** 分期期數 */
const periodCount = ref<number>(0);

/** 發票列表 */
const billList = ref<BillItem[]>([]);

/** 是否顯示計算確認對話框 */
const showCalculateConfirm = ref(false);

/** 將日期轉換為 YYYY-MM-DD 格式 */
const formatToDateInput = (date: Date): string => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};

/** 計算後的金額分配 */
const calculateBills = (): BillItem[] => {
  if (periodCount.value <= 0) return [];

  const baseAmount = Math.floor(props.totalAmount / periodCount.value);
  const remainder = props.totalAmount % periodCount.value;
  const startDate = new Date();

  const bills: BillItem[] = [];

  for (let i = 0; i < periodCount.value; i++) {
    const billDate = new Date(startDate);
    billDate.setMonth(billDate.getMonth() + i);

    bills.push({
      employeePipelineBillPeriodNumber: i + 1,
      employeePipelineBillBillTime: formatToDateInput(billDate),
      employeePipelineBillIsPreIssued: false,
      employeePipelineBillExecuteDate: null,
      employeePipelineBillNoTaxAmount: i === 0 ? baseAmount + remainder : baseAmount,
      employeePipelineBillRemark: "",
      employeePipelineBillID: null,
      employeePipelineBillBillNumber: null,
      employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum.NotCompleted,
    });
  }

  return bills;
};

/** 處理計算按鈕點擊 */
const handleCalculateClick = () => {
  if (periodCount.value <= 0) {
    alert("請輸入有效的分期期數（1-99）");
    return;
  }
  if (periodCount.value > 99) {
    alert("分期期數不可超過 99 期");
    return;
  }
  showCalculateConfirm.value = true;
};

/** 確認重新計算 */
const confirmCalculate = () => {
  billList.value = calculateBills();
  showCalculateConfirm.value = false;
};

/** 取消重新計算 */
const cancelCalculate = () => {
  showCalculateConfirm.value = false;
};

/** 格式化金額顯示 */
const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat("zh-TW", {
    style: "currency",
    currency: "TWD",
    minimumFractionDigits: 0,
  }).format(amount);
};

/** 計算總金額是否正確 */
const totalBillAmount = computed(() => {
  return billList.value.reduce((sum, bill) => sum + bill.employeePipelineBillNoTaxAmount, 0);
});

/** 驗證總金額是否一致 */
const isTotalAmountValid = computed(() => {
  return totalBillAmount.value === props.totalAmount;
});

/** 將伺服器日期時間轉換為 date 格式 (YYYY-MM-DD) */
const convertToDate = (dateTimeString: string): string => {
  if (!dateTimeString) return "";

  const date = new Date(dateTimeString);

  if (isNaN(date.getTime())) {
    return ""; // 無效日期
  }

  return formatToDateInput(date);
};

/** 載入資料 */
const loadData = () => {
  periodCount.value = props.currentPeriodCount;
  billList.value = props.currentBills.map((bill) => ({
    ...bill,
    employeePipelineBillBillTime: convertToDate(bill.employeePipelineBillBillTime),
    employeePipelineBillIsPreIssued: bill.employeePipelineBillIsPreIssued ?? false,
    employeePipelineBillExecuteDate: bill.employeePipelineBillExecuteDate
      ? convertToDate(bill.employeePipelineBillExecuteDate)
      : null,
  }));
};

/** 重置表單 */
const resetForm = () => {
  periodCount.value = 0;
  billList.value = [];
  showCalculateConfirm.value = false;
};

/** 監聽 show 變化 */
watch(
  () => props.show,
  (newVal) => {
    if (newVal) {
      loadData();
    } else {
      resetForm();
    }
  }
);

/** 處理確認 */
const handleConfirm = () => {
  if (!isTotalAmountValid.value) {
    alert(
      `發票總金額 ${formatCurrency(totalBillAmount.value)} 與產品總金額 ${formatCurrency(props.totalAmount)} 不一致`
    );
    return;
  }

  if (billList.value.length === 0) {
    alert("請先計算發票分期");
    return;
  }

  const hasMissingExecuteDate = billList.value.some(
    (bill) => bill.employeePipelineBillIsPreIssued && !bill.employeePipelineBillExecuteDate
  );
  if (hasMissingExecuteDate) {
    alert("已勾選先開發票的項目需填寫預計執行日");
    return;
  }

  // Date → DateTimeOffset 轉換
  const submitBillList = billList.value.map((bill) => ({
    ...bill,
    employeePipelineBillBillTime: formatToServerDateStartISO8(bill.employeePipelineBillBillTime),
    employeePipelineBillExecuteDate: bill.employeePipelineBillExecuteDate
      ? formatToServerDateStartISO8(bill.employeePipelineBillExecuteDate)
      : null,
  }));

  emit("confirm", {
    periodCount: periodCount.value,
    billList: submitBillList,
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
      class="max-w-5xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3">
        <h2 class="modal-title">編輯發票記錄</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          x
        </button>
      </div>

      <hr />

      <!-- 內容區域 -->
      <div class="p-6 overflow-y-auto flex-1">
        <div class="space-y-4">
          <div class="grid grid-cols-2 gap-4">
            <!-- 總金額顯示 -->
            <div class="p-4 bg-gray-50 rounded-lg">
              <div class="text-sm text-gray-600 mb-1">產品總成交價</div>
              <div class="text-2xl font-bold text-gray-900">{{ formatCurrency(totalAmount) }}</div>
            </div>

            <!-- 分期期數與計算按鈕 -->
            <div class="p-4 bg-gray-50 rounded-lg">
              <div class="flex gap-4 items-end">
                <div class="flex-1">
                  <label class="form-label"> 分期期數 <span class="required-label">*</span> </label>
                  <input
                    v-model.number="periodCount"
                    type="number"
                    min="0"
                    max="99"
                    class="input-box"
                    placeholder="請輸入 0-99"
                  />
                </div>
                <button type="button" class="btn-submit px-6" @click="handleCalculateClick">
                  計算
                </button>
              </div>
            </div>
          </div>

          <!-- 發票列表 -->
          <div v-if="billList.length > 0">
            <div class="d-flex justify-between items-center mb-4">
              <div v-if="!isTotalAmountValid" class="text-red-700 text-sm">
                ⚠️ 發票總金額 {{ formatCurrency(totalBillAmount) }} 與產品總成交價不一致，請調整
              </div>
              <div v-else class="text-green-700 text-sm">✓ 發票總金額正確</div>
            </div>

            <div class="overflow-y-auto">
              <table class="table-base table-fixed w-full">
                <thead class="bg-gray-800 text-white sticky top-0">
                  <tr>
                    <th class="text-center w-24">期數</th>
                    <th class="text-center w-40">發票日期</th>
                    <th class="text-center w-32">發票種類</th>
                    <th class="text-center w-40">預計執行日</th>
                    <th class="text-end w-40">未稅發票金額</th>
                    <th class="text-start">備註</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(bill, index) in billList" :key="index" class="border border-gray-300">
                    <td class="text-center">第 {{ bill.employeePipelineBillPeriodNumber }} 期</td>
                    <td class="text-center">
                      <input
                        v-model="bill.employeePipelineBillBillTime"
                        type="date"
                        class="input-box w-full"
                      />
                    </td>
                    <td class="text-center">
                      <label class="inline-flex items-center gap-2 text-sm text-gray-700">
                        <input
                          v-model="bill.employeePipelineBillIsPreIssued"
                          type="checkbox"
                          class="h-4 w-4"
                          @change="
                            bill.employeePipelineBillIsPreIssued
                              ? null
                              : (bill.employeePipelineBillExecuteDate = null)
                          "
                        />
                        先開發票
                      </label>
                    </td>
                    <td class="text-center">
                      <input
                        v-model="bill.employeePipelineBillExecuteDate"
                        type="date"
                        class="input-box w-full"
                        :disabled="!bill.employeePipelineBillIsPreIssued"
                      />
                    </td>
                    <td class="text-end">
                      <input
                        v-model.number="bill.employeePipelineBillNoTaxAmount"
                        type="number"
                        min="0"
                        class="input-box w-full text-right"
                      />
                    </td>
                    <td>
                      <textarea
                        v-model="bill.employeePipelineBillRemark"
                        rows="2"
                        class="input-box w-full resize-none"
                        placeholder="請輸入備註"
                      ></textarea>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <div v-else class="p-8 text-center text-gray-400">
            請輸入分期期數並點擊「計算」按鈕生成發票明細
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button
            class="btn-submit"
            :disabled="!isTotalAmountValid || billList.length === 0"
            @click="handleConfirm"
          >
            確定
          </button>
        </div>
      </div>
    </div>

    <!-- 計算確認對話框 -->
    <div
      v-if="showCalculateConfirm"
      class="fixed inset-0 z-[60] flex items-center justify-center bg-black bg-opacity-50"
    >
      <div class="w-full max-w-md rounded-lg bg-white p-6 shadow-xl">
        <h3 class="mb-4 text-lg font-bold">確認重新計算</h3>
        <p class="mb-6 text-gray-600">重新計算將會覆蓋目前的發票明細，確定要繼續嗎？</p>
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="cancelCalculate">取消</button>
          <button class="btn-submit" @click="confirmCalculate">確定</button>
        </div>
      </div>
    </div>
  </div>
</template>
