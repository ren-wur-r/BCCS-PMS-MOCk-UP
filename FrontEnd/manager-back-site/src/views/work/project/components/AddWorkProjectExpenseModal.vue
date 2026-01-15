<script setup lang="ts">
import { reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { addExpense } from "@/services/pms-http/work/project/workProjectHttpService";
import type {
  MbsWrkPrjHttpAddExpenseReqMdl,
  MbsWrkPrjHttpAddExpenseRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import { formatToServerDateStartISO8 } from "@/utils/timeFormatter";

//-------------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();
//-------------------------------------------------------------
const props = defineProps<{
  employeeProjectID: number;
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//-------------------------------------------------------------
/** 新增-工作-專案管理-支出-模型 */
interface AddWorkProjectExpenseMdl {
  employeeProjectExpenseName: string;
  employeeProjectExpensePredictAmount: number | null;
  employeeProjectExpenseActualAmount: number | null;
  employeeProjectExpenseBillNumber: string;
  employeeProjectExpenseBillTime: string;
  employeeProjectExpenseRemark: string;
}

/** 新增-工作-專案管理-支出-驗證模型 */
interface AddWorkProjectExpenseValidateMdl {
  employeeProjectExpenseName: boolean;
  employeeProjectExpensePredictAmount: boolean;
}

//-------------------------------------------------------------
/** 新增-工作-專案管理-支出-物件 */
const addWorkProjectExpenseObj = reactive<AddWorkProjectExpenseMdl>({
  employeeProjectExpenseName: "",
  employeeProjectExpensePredictAmount: null,
  employeeProjectExpenseActualAmount: null,
  employeeProjectExpenseBillNumber: "",
  employeeProjectExpenseBillTime: "",
  employeeProjectExpenseRemark: "",
});

/** 新增-工作-專案管理-支出-驗證物件 */
const addWorkProjectExpenseValidateObj = reactive<AddWorkProjectExpenseValidateMdl>({
  employeeProjectExpenseName: true,
  employeeProjectExpensePredictAmount: true,
});

//-------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 支出項目名稱
  const nameValid =
    !!addWorkProjectExpenseObj.employeeProjectExpenseName &&
    addWorkProjectExpenseObj.employeeProjectExpenseName.length <= 50;
  addWorkProjectExpenseValidateObj.employeeProjectExpenseName = nameValid;
  if (!nameValid) isValid = false;

  // 預估金額（必填）
  const predictAmountValid = addWorkProjectExpenseObj.employeeProjectExpensePredictAmount !== null;
  addWorkProjectExpenseValidateObj.employeeProjectExpensePredictAmount = predictAmountValid;
  if (!predictAmountValid) isValid = false;

  return isValid;
};

//-------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 驗證 Token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateField()) {
    return;
  }

  // 新增發票號碼二次確認
  if (addWorkProjectExpenseObj.employeeProjectExpenseBillNumber) {
    const confirmed = window.confirm("請確認發票號碼是否輸入正確，送出後將無法修改！");
    if (!confirmed) {
      return;
    }
  }

  // 呼叫 api
  const requestData: MbsWrkPrjHttpAddExpenseReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: props.employeeProjectID!,
    employeeProjectExpenseName: addWorkProjectExpenseObj.employeeProjectExpenseName,
    employeeProjectExpensePredictAmount:
      addWorkProjectExpenseObj.employeeProjectExpensePredictAmount ?? 0,
    employeeProjectExpenseActualAmount: addWorkProjectExpenseObj.employeeProjectExpenseActualAmount,
    employeeProjectExpenseBillNumber:
      addWorkProjectExpenseObj.employeeProjectExpenseBillNumber || null,
    employeeProjectExpenseBillTime: addWorkProjectExpenseObj.employeeProjectExpenseBillTime
      ? formatToServerDateStartISO8(addWorkProjectExpenseObj.employeeProjectExpenseBillTime)
      : null,
    employeeProjectExpenseRemark: addWorkProjectExpenseObj.employeeProjectExpenseRemark || null,
  };

  const responseData: MbsWrkPrjHttpAddExpenseRspMdl | null = await addExpense(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  emit("submit");
  emit("close");
};

//-------------------------------------------------------------
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[620px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-3 border-b">
        <h2 class="text-lg font-bold text-gray-800">新增支出項目</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <!-- 內容 -->
      <div class="p-6 space-y-4 max-h-[70vh] overflow-y-auto">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
        >
          {{ errorMessage }}
        </p>

        <!-- 支出項目 -->
        <div class="flex-1">
          <label class="form-label">支出項目 <span class="required-label">*</span></label>
          <input
            v-model="addWorkProjectExpenseObj.employeeProjectExpenseName"
            class="input-box"
            placeholder="請輸入支出項目"
            @input="resetError"
          />
          <div class="error-wrapper">
            <p
              v-if="!addWorkProjectExpenseValidateObj.employeeProjectExpenseName"
              class="error-tip"
            >
              不可為空，長度不可超過 50 個字
            </p>
          </div>
        </div>

        <!-- 預估金額 -->
        <div class="flex-1">
          <label class="form-label">預估金額 <span class="required-label">*</span></label>
          <input
            v-model="addWorkProjectExpenseObj.employeeProjectExpensePredictAmount"
            type="number"
            class="input-box"
            placeholder="請輸入預估金額"
            @input="resetError"
          />
          <div class="error-wrapper">
            <p
              v-if="!addWorkProjectExpenseValidateObj.employeeProjectExpensePredictAmount"
              class="error-tip"
            >
              不可為空
            </p>
          </div>
        </div>

        <!-- 實際金額 -->
        <div class="flex-1">
          <label class="form-label">實際金額</label>
          <input
            v-model="addWorkProjectExpenseObj.employeeProjectExpenseActualAmount"
            type="number"
            class="input-box"
            placeholder="請輸入實際金額"
          />
          <div class="error-wrapper"></div>
        </div>

        <!-- 發票資訊 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">發票號碼</label>
            <input
              v-model="addWorkProjectExpenseObj.employeeProjectExpenseBillNumber"
              class="input-box"
              placeholder="請輸入發票號碼"
            />
            <div class="error-wrapper"></div>
          </div>
          <div class="flex-1">
            <label class="form-label">發票日期</label>
            <input
              v-model="addWorkProjectExpenseObj.employeeProjectExpenseBillTime"
              type="date"
              class="input-box"
            />
            <div class="error-wrapper"></div>
          </div>
        </div>

        <!-- 備註 -->
        <div class="flex flex-col gap-2 flex-1">
          <label class="form-label">備註</label>
          <textarea
            v-model="addWorkProjectExpenseObj.employeeProjectExpenseRemark"
            class="input-box h-24 resize-none"
            placeholder="請輸入備註"
          ></textarea>
        </div>
      </div>

      <!-- 底部 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="$emit('close')">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>
</template>
