<script setup lang="ts">
import { onMounted, reactive, ref, computed } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { getExpense, updateExpense } from "@/services/pms-http/work/project/workProjectHttpService";
import type {
    MbsWrkPrjHttpGetExpenseReqMdl,
    MbsWrkPrjHttpGetExpenseRspMdl,
    MbsWrkPrjHttpUpdateExpenseReqMdl,
    MbsWrkPrjHttpUpdateExpenseRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import { formatDateTime, formatToServerDateStartISO8 } from "@/utils/timeFormatter";

//-------------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();
//-------------------------------------------------------------
const props = defineProps<{
    employeeProjectID: number;
    employeeProjectExpenseID: number;
}>();

const emit = defineEmits<{
    (e: "close"): void;
    (e: "submit"): void;
}>();
//-------------------------------------------------------------
/** 更新-工作-專案管理-支出-模型 */
interface UpdateWorkProjectExpenseMdl {
    employeeProjectExpenseName: string;
    employeeProjectExpensePredictAmount: number | null;
    employeeProjectExpenseActualAmount: number | null;
    employeeProjectExpenseBillNumber: string;
    employeeProjectExpenseBillTime: string;
    employeeProjectExpenseRemark: string;
}

/** 更新-工作-專案管理-支出-驗證模型 */
interface UpdateWorkProjectExpenseValidateMdl {
    employeeProjectExpenseName: boolean;
    employeeProjectExpensePredictAmount: boolean;
}

//-------------------------------------------------------------
/** 更新-工作-專案管理-支出-物件 */
const updateWorkProjectExpenseObj = reactive<UpdateWorkProjectExpenseMdl>({
    employeeProjectExpenseName: "",
    employeeProjectExpensePredictAmount: null,
    employeeProjectExpenseActualAmount: null,
    employeeProjectExpenseBillNumber: "",
    employeeProjectExpenseBillTime: "",
    employeeProjectExpenseRemark: "",
});

/** 更新-工作-專案管理-支出-原始物件 (用於比對變更) */
const updateWorkProjectExpenseOriginalObj = reactive<UpdateWorkProjectExpenseMdl>({
    employeeProjectExpenseName: "",
    employeeProjectExpensePredictAmount: null,
    employeeProjectExpenseActualAmount: null,
    employeeProjectExpenseBillNumber: "",
    employeeProjectExpenseBillTime: "",
    employeeProjectExpenseRemark: "",
});

/** 更新-工作-專案管理-支出-驗證物件 */
const updateWorkProjectExpenseValidateObj = reactive<UpdateWorkProjectExpenseValidateMdl>({
    employeeProjectExpenseName: true,
    employeeProjectExpensePredictAmount: true,
});

const isLoading = ref(true);

/** 發票號碼有值就不能修改 */
const isBillNumberDisabled = computed(() => {
    return !!updateWorkProjectExpenseOriginalObj.employeeProjectExpenseBillNumber;
});

//-------------------------------------------------------------
/** 取得【專案支出】資料 */
const getExpenseData = async () => {
    if (!requireToken()) return;

    const requestData: MbsWrkPrjHttpGetExpenseReqMdl = {
        employeeLoginToken: tokenStore.token!,
        employeeProjectExpenseID: props.employeeProjectExpenseID,
    };

    const responseData: MbsWrkPrjHttpGetExpenseRspMdl | null = await getExpense(requestData);

    if (!responseData) {
        showError.value = true;
        errorMessage.value = "系統錯誤，請稍後再試";
        return;
    }

    const isSuccess = handleErrorCode(responseData.errorCode);
    if (!isSuccess) return;

    // 設定資料
    updateWorkProjectExpenseObj.employeeProjectExpenseName = responseData.employeeProjectExpenseName;
    updateWorkProjectExpenseObj.employeeProjectExpensePredictAmount = responseData.employeeProjectExpensePredictAmount;
    updateWorkProjectExpenseObj.employeeProjectExpenseActualAmount = responseData.employeeProjectExpenseActualAmount;
    updateWorkProjectExpenseObj.employeeProjectExpenseBillNumber = responseData.employeeProjectExpenseBillNumber || "";
    updateWorkProjectExpenseObj.employeeProjectExpenseBillTime = responseData.employeeProjectExpenseBillTime
        ? responseData.employeeProjectExpenseBillTime.split('T')[0]
        : "";
    updateWorkProjectExpenseObj.employeeProjectExpenseRemark = responseData.employeeProjectExpenseRemark || "";

    Object.assign(updateWorkProjectExpenseOriginalObj, updateWorkProjectExpenseObj);

    isLoading.value = false;
};

//-------------------------------------------------------------
/** 取得變更的欄位 */
const getChangedFields = () => {
    const changes: Partial<MbsWrkPrjHttpUpdateExpenseReqMdl> = {
        employeeLoginToken: tokenStore.token!,
        employeeProjectExpenseID: props.employeeProjectExpenseID,
        employeeProjectExpenseName: updateWorkProjectExpenseObj.employeeProjectExpenseName,
        employeeProjectExpensePredictAmount: updateWorkProjectExpenseObj.employeeProjectExpensePredictAmount ?? 0,
    };

    if (updateWorkProjectExpenseObj.employeeProjectExpenseActualAmount !== updateWorkProjectExpenseOriginalObj.employeeProjectExpenseActualAmount) {
        changes.employeeProjectExpenseActualAmount = updateWorkProjectExpenseObj.employeeProjectExpenseActualAmount;
    }

    if (updateWorkProjectExpenseObj.employeeProjectExpenseBillNumber !== updateWorkProjectExpenseOriginalObj.employeeProjectExpenseBillNumber) {
        changes.employeeProjectExpenseBillNumber = updateWorkProjectExpenseObj.employeeProjectExpenseBillNumber || null;
    }

    if (updateWorkProjectExpenseObj.employeeProjectExpenseBillTime !== updateWorkProjectExpenseOriginalObj.employeeProjectExpenseBillTime) {
        changes.employeeProjectExpenseBillTime = updateWorkProjectExpenseObj.employeeProjectExpenseBillTime
            ? formatToServerDateStartISO8(updateWorkProjectExpenseObj.employeeProjectExpenseBillTime)
            : null;
    }

    if (updateWorkProjectExpenseObj.employeeProjectExpenseRemark !== updateWorkProjectExpenseOriginalObj.employeeProjectExpenseRemark) {
        changes.employeeProjectExpenseRemark = updateWorkProjectExpenseObj.employeeProjectExpenseRemark || null;
    }

    return changes;
};

//-------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
    let isValid = true;

    // 支出項目名稱
    const nameValid =
        !!updateWorkProjectExpenseObj.employeeProjectExpenseName &&
        updateWorkProjectExpenseObj.employeeProjectExpenseName.length <= 50;
    updateWorkProjectExpenseValidateObj.employeeProjectExpenseName = nameValid;
    if (!nameValid) isValid = false;

    // 預估金額
    const predictAmountValid = updateWorkProjectExpenseObj.employeeProjectExpensePredictAmount !== null;
    updateWorkProjectExpenseValidateObj.employeeProjectExpensePredictAmount = predictAmountValid;
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
    const isAddingBillNumber =
        !updateWorkProjectExpenseOriginalObj.employeeProjectExpenseBillNumber &&
        updateWorkProjectExpenseObj.employeeProjectExpenseBillNumber;

    if (isAddingBillNumber) {
        const confirmed = window.confirm("請確認發票號碼是否輸入正確，送出後將無法修改！");
        if (!confirmed) {
            return;
        }
    }

    // 取得變更欄位
    const changedFields = getChangedFields();

    // 呼叫 api
    const responseData: MbsWrkPrjHttpUpdateExpenseRspMdl | null = await updateExpense(changedFields as MbsWrkPrjHttpUpdateExpenseReqMdl);

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

onMounted(() => {
    getExpenseData();
});
</script>

<template>
    <div class="modal-overlay">
        <div class="w-[620px] bg-white rounded-lg shadow-lg">
            <!-- 標題列 -->
            <div class="flex items-center justify-between p-4 border-b">
                <h2 class="text-lg font-bold text-gray-800">編輯支出項目</h2>
                <button class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
                    @click="$emit('close')">
                    ×
                </button>
            </div>

            <!-- 內容 -->
            <div class="p-6 space-y-4 max-h-[70vh] overflow-y-auto">
                <!-- 載入中 -->
                <div v-if="isLoading" class="flex justify-center items-center py-12">
                    <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-brand-100"></div>
                </div>

                <!-- 表單內容 -->
                <template v-else>
                    <!-- 錯誤提示 -->
                    <p v-if="showError" class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded">
                        {{ errorMessage }}
                    </p>

                    <!-- 支出項目 -->
                    <div class="flex flex-col gap-2 flex-1">
                        <label class="form-label">支出項目 <span class="required-label">*</span></label>
                        <input v-model="updateWorkProjectExpenseObj.employeeProjectExpenseName" class="input-box"
                            placeholder="請輸入文字" @input="resetError" />
                        <div class="h-2">
                            <p v-if="!updateWorkProjectExpenseValidateObj.employeeProjectExpenseName" class="error-tip">
                                不可為空，長度不可超過 50 個字
                            </p>
                        </div>
                    </div>

                    <!-- 預估金額 -->
                    <div class="flex flex-col gap-2 flex-1">
                        <label class="form-label">預估金額 <span class="required-label">*</span></label>
                        <input v-model="updateWorkProjectExpenseObj.employeeProjectExpensePredictAmount" type="number"
                            class="input-box" placeholder="請輸入數字" @input="resetError" />
                        <div class="h-2">
                            <p v-if="!updateWorkProjectExpenseValidateObj.employeeProjectExpensePredictAmount"
                                class="error-tip">
                                不可為空
                            </p>
                        </div>
                    </div>

                    <!-- 實際金額 -->
                    <div class="flex flex-col gap-2 flex-1">
                        <label class="form-label">實際金額</label>
                        <input v-model="updateWorkProjectExpenseObj.employeeProjectExpenseActualAmount" type="number"
                            class="input-box" placeholder="請輸入數字" />
                        <div class="h-2"></div>
                    </div>

                    <!-- 發票資訊 -->
                    <div class="flex gap-4 w-full">
                        <div class="flex flex-col gap-2 flex-1">
                            <label class="form-label">發票號碼</label>
                            <input v-model="updateWorkProjectExpenseObj.employeeProjectExpenseBillNumber"
                                :disabled="isBillNumberDisabled" class="input-box"
                                :class="{ 'bg-gray-100 cursor-not-allowed': isBillNumberDisabled }"
                                placeholder="請輸入發票號碼" />
                            <div class="h-2">
                                <p v-if="isBillNumberDisabled" class="text-xs text-gray-500">
                                </p>
                            </div>
                        </div>
                        <div class="flex flex-col gap-2 flex-1">
                            <label class="form-label">發票日期</label>
                            <input v-model="updateWorkProjectExpenseObj.employeeProjectExpenseBillTime" type="date"
                                class="input-box" />
                            <div class="h-2"></div>
                        </div>
                    </div>

                    <!-- 備註 -->
                    <div class="flex flex-col gap-2 flex-1">
                        <label class="form-label">備註</label>
                        <textarea v-model="updateWorkProjectExpenseObj.employeeProjectExpenseRemark"
                            class="input-box h-24 resize-none" placeholder="請輸入備註"></textarea>
                    </div>
                </template>
            </div>

            <!-- 底部 -->
            <div class="flex justify-end gap-2 p-4 border-t">
                <button class="btn-cancel" @click="$emit('close')">取消</button>
                <button class="btn-submit" :disabled="isLoading" @click="clickSubmitBtn">送出</button>
            </div>
        </div>
    </div>
</template>
