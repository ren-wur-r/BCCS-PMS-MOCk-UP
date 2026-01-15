<script setup lang="ts">
import { reactive, watch } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { addProjectWork } from "@/services";
import type {
  MbsWrkPrjHttpAddWorkReqMdl,
  MbsWrkPrjHttpAddWorkRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";

const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();

//-------------------------------------------------------------
const props = defineProps<{
  employeeProjectID: number;
  employeeProjectWorkUrl: string | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//-------------------------------------------------------------
/** 編輯-工作-專案管理-工作計劃書-頁面模型 */
interface UpdateWorkProjectWorkObj {
  employeeProjectWorkUrl: string;
}

/** 編輯-工作-專案管理-工作計劃書-驗證模型 */
interface UpdateWorkProjectWorkValidateMdl {
  employeeProjectWorkUrl: boolean;
}
//-------------------------------------------------------------
/** 編輯-工作-專案管理-工作計劃書-頁面物件 */
const updateWorkProjectWorkObj = reactive<UpdateWorkProjectWorkObj>({
  employeeProjectWorkUrl: "",
});

/** 編輯-工作-專案管理-工作計劃書-驗證物件 */
const updateWorkProjectWorkValidateObj = reactive<UpdateWorkProjectWorkValidateMdl>({
  employeeProjectWorkUrl: true,
});

//-------------------------------------------------------------
/** 驗證【工作計劃書】欄位 */
const validateWorkField = () => {
  let isValid = true;

  const url = updateWorkProjectWorkObj.employeeProjectWorkUrl.trim();

  // 不可為空
  updateWorkProjectWorkValidateObj.employeeProjectWorkUrl = !!url;
  if (!updateWorkProjectWorkValidateObj.employeeProjectWorkUrl) {
    isValid = false;
    return isValid;
  }

  // 必須 http:// or https://
  const urlPattern = /^(https?:\/\/)([\w-]+\.)+[\w-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$/;

  updateWorkProjectWorkValidateObj.employeeProjectWorkUrl = urlPattern.test(url);

  if (!updateWorkProjectWorkValidateObj.employeeProjectWorkUrl) {
    isValid = false;
  }

  return isValid;
};

//-------------------------------------------------------------
//監聽網址 編輯時自動回填
watch(
  () => props.employeeProjectWorkUrl,
  (val) => {
    updateWorkProjectWorkObj.employeeProjectWorkUrl = val ?? "";
  },
  { immediate: true }
);

//-------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;
  // 驗證
  if (!validateWorkField()) return;
  const requestData: MbsWrkPrjHttpAddWorkReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: props.employeeProjectID!,
    employeeProjectWorkUrl: updateWorkProjectWorkObj.employeeProjectWorkUrl,
  };

  const responseData: MbsWrkPrjHttpAddWorkRspMdl | null = await addProjectWork(requestData);
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
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">修改工作計劃書</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-6 space-y-4">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
        >
          {{ errorMessage }}
        </p>

        <div class="mb-4 flex flex-col gap-2 flex-1">
          <label class="form-label">工作計劃書 <span class="required-label">*</span></label>
          <input
            v-model="updateWorkProjectWorkObj.employeeProjectWorkUrl"
            type="url"
            placeholder="請輸入網址"
            class="input-box"
          />
          <div class="h-2">
            <p v-if="!updateWorkProjectWorkValidateObj.employeeProjectWorkUrl" class="error-tip">
              工作計劃書網址不可為空，也請輸入正確的網址格式 (http:// 或 https:// 開頭)
            </p>
          </div>
        </div>
      </div>

      <!-- 底部按鈕 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="$emit('close')">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>
</template>
