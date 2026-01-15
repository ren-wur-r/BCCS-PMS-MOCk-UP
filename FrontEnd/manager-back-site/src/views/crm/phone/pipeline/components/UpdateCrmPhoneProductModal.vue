<script setup lang="ts">
import { onMounted, reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import type {} from "@/services/pms-http/system/product/systemProductHttpFormat";
import { getPhoneEmployeePipelineProduct, updatePhoneEmployeePipelineProduct } from "@/services";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
import GetManyManagerProductComboBox from "@/components/feature/search-bar/GetManyManagerProductComboBox.vue";
import GetManyManagerProductSpecificationComboBox from "@/components/feature/search-bar/GetManyManagerProductSpecificationComboBox.vue";
import {
  MbsCrmPhnHttpGetEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineProductRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
const props = defineProps<{
  employeePipelineProductID: number;
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//----------------------------------------------------------------------------
/** 更新-電銷-產品-模型 */
interface UpdateCrmPhoneProductMdl {
  employeePipelineID: number;
  managerProductID: number;
  managerProductName: string | null;
  managerProductSpecificationID: number;
  managerProductMainKindID: number;
  managerProductSubKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerProductSpecificationName: string | null;
}

/** 更新-電銷-產品-驗證模型 */
interface UpdateCrmPhoneProductValidateMdl {
  managerProductMainKindID: boolean;
  managerProductSubKindID: boolean;
  managerProductID: boolean;
  managerProductSpecificationID: boolean;
}

/** 更新-電銷-產品-物件 */
const updateCrmPhoneProductObj = reactive<UpdateCrmPhoneProductMdl>({
  employeePipelineID: 0,
  managerProductID: 0,
  managerProductName: "",
  managerProductSpecificationID: 0,
  managerProductMainKindID: 0,
  managerProductSubKindID: 0,
  managerProductMainKindName: "",
  managerProductSubKindName: "",
  managerProductSpecificationName: "",
});

/** 更新-電銷-產品-驗證物件 */
const updateCrmPhoneProductValidObj = reactive<UpdateCrmPhoneProductValidateMdl>({
  managerProductMainKindID: true,
  managerProductSubKindID: true,
  managerProductID: true,
  managerProductSpecificationID: true,
});
//----------------------------------------------------------------------------
/** 取得【電銷】資料 */
const getPhoneEmployeePipelineProductData = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  const requestData: MbsCrmPhnHttpGetEmployeePipelineProductReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineProductID: props.employeePipelineProductID,
  };
  const responseData = await getPhoneEmployeePipelineProduct(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }
  updateCrmPhoneProductObj.employeePipelineID = responseData.employeePipelineID;
  updateCrmPhoneProductObj.managerProductMainKindID = responseData.managerProductMainKindID;
  updateCrmPhoneProductObj.managerProductMainKindName = responseData.managerProductMainKindName;
  updateCrmPhoneProductObj.managerProductSubKindID = responseData.managerProductSubKindID;
  updateCrmPhoneProductObj.managerProductSubKindName = responseData.managerProductSubKindName;
  updateCrmPhoneProductObj.managerProductID = responseData.managerProductID;
  updateCrmPhoneProductObj.managerProductSpecificationID =
    responseData.managerProductSpecificationID;
  updateCrmPhoneProductObj.managerProductSpecificationName =
    responseData.managerProductSpecificationName;
  updateCrmPhoneProductObj.managerProductName = responseData.managerProductName;
};
//----------------------------------------------------------------------------
/** 驗證方法 */
const validateField = () => {
  let isValid = true;

  // 主分類
  if (!updateCrmPhoneProductObj.managerProductMainKindID) {
    updateCrmPhoneProductValidObj.managerProductMainKindID = false;
    isValid = false;
  } else {
    updateCrmPhoneProductValidObj.managerProductMainKindID = true;
  }

  // 子分類
  if (!updateCrmPhoneProductObj.managerProductSubKindID) {
    updateCrmPhoneProductValidObj.managerProductSubKindID = false;
    isValid = false;
  } else {
    updateCrmPhoneProductValidObj.managerProductSubKindID = true;
  }

  // 產品名稱
  if (!updateCrmPhoneProductObj.managerProductID) {
    updateCrmPhoneProductValidObj.managerProductID = false;
    isValid = false;
  } else {
    updateCrmPhoneProductValidObj.managerProductID = true;
  }

  // 規格
  if (!updateCrmPhoneProductObj.managerProductSpecificationID) {
    updateCrmPhoneProductValidObj.managerProductSpecificationID = false;
    isValid = false;
  } else {
    updateCrmPhoneProductValidObj.managerProductSpecificationID = true;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 欄位驗證
  if (!validateField()) {
    return;
  }
  // 呼叫api
  const requestData: MbsCrmPhnHttpUpdateEmployeePipelineProductReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineProductID: props.employeePipelineProductID,
    managerProductID: updateCrmPhoneProductObj.managerProductID,
    managerProductSpecificationID: updateCrmPhoneProductObj.managerProductSpecificationID,
  };

  const responseData: MbsCrmPhnHttpUpdateEmployeePipelineProductRspMdl | null =
    await updatePhoneEmployeePipelineProduct(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }
  emit("submit");
  emit("close");
};

onMounted(() => {
  getPhoneEmployeePipelineProductData();
});
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">編輯產品</h2>
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

        <div class="flex gap-4 w-full mt-3">
          <!-- 產品主分類 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">產品主分類</label>
            <GetManyManagerProductMainKindComboBox
              v-model:managerProductMainKindID="updateCrmPhoneProductObj.managerProductMainKindID"
              v-model:managerProductMainKindName="
                updateCrmPhoneProductObj.managerProductMainKindName
              "
              :disabled="false"
              placeholder="請選擇主分類"
            />
          </div>

          <!-- 產品子分類 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">產品子分類</label>
            <GetManyManagerProductSubKindComboBox
              v-model:managerProductSubKindID="updateCrmPhoneProductObj.managerProductSubKindID"
              v-model:managerProductSubKindName="updateCrmPhoneProductObj.managerProductSubKindName"
              :managerProductMainKindID="updateCrmPhoneProductObj.managerProductMainKindID"
              :disabled="false"
              placeholder="請選擇子分類"
            />
          </div>
        </div>

        <hr class="my-6" />

        <div class="flex gap-4 w-full">
          <!-- 產品名稱 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">產品名稱 <span class="required-label">*</span></label>
            <GetManyManagerProductComboBox
              v-model:managerProductID="updateCrmPhoneProductObj.managerProductID"
              v-model:managerProductName="updateCrmPhoneProductObj.managerProductName"
              :managerProductMainKindID="updateCrmPhoneProductObj.managerProductMainKindID"
              :managerProductSubKindID="updateCrmPhoneProductObj.managerProductSubKindID"
              :disabled="false"
              :preventAutoReset="true"
              placeholder="請選擇產品名稱"
            />
            <div class="error-wrapper">
              <span v-if="!updateCrmPhoneProductValidObj.managerProductID" class="error-tip">
                不可為空
              </span>
            </div>
          </div>

          <!-- 產品規格 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">產品規格 <span class="required-label">*</span></label>
            <GetManyManagerProductSpecificationComboBox
              v-model:managerProductSpecificationID="
                updateCrmPhoneProductObj.managerProductSpecificationID
              "
              v-model:managerProductSpecificationName="
                updateCrmPhoneProductObj.managerProductSpecificationName
              "
              :managerProductID="updateCrmPhoneProductObj.managerProductID"
              :disabled="false"
            />
            <div class="error-wrapper">
              <span
                v-if="!updateCrmPhoneProductValidObj.managerProductSpecificationID"
                class="error-tip"
              >
                不可為空
              </span>
            </div>
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
