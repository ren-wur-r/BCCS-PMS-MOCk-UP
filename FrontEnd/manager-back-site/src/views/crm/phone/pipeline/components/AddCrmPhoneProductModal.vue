<script setup lang="ts">
import { reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import type {} from "@/services/pms-http/system/product/systemProductHttpFormat";
import { addPhoneEmployeePipelineProduct } from "@/services";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
import GetManyManagerProductComboBox, {
  GetManyManagerProductComboBoxSelectedData,
} from "@/components/feature/search-bar/GetManyManagerProductComboBox.vue";
import GetManyManagerProductSpecificationComboBox from "@/components/feature/search-bar/GetManyManagerProductSpecificationComboBox.vue";
import {
  MbsCrmPhnHttpAddEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpAddEmployeePipelineProductRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
const props = defineProps<{
  employeePipelineID: number;
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();

/** 選擇產品後的處理 */
const onProductSelected = (product: GetManyManagerProductComboBoxSelectedData) => {
  addCrmPhoneProductObj.managerProductMainKindID = product.managerProductMainKindID;
  addCrmPhoneProductObj.managerProductMainKindName = product.managerProductMainKindName;

  addCrmPhoneProductObj.managerProductSubKindID = product.managerProductSubKindID;
  addCrmPhoneProductObj.managerProductSubKindName = product.managerProductSubKindName;
};

//----------------------------------------------------------------------------
/** 新增-電銷-產品-模型 */
interface AddCrmPhoneProductMdl {
  employeePipelineID: number;
  managerProductID: number;
  managerProductSpecificationID: number;
  managerProductMainKindID: number;
  managerProductSubKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerProductSpecificationName: string;
}

/** 新增-電銷-產品-驗證模型 */
interface AddCrmPhoneProductValidateMdl {
  managerProductMainKindID: boolean;
  managerProductSubKindID: boolean;
  managerProductID: boolean;
  managerProductSpecificationID: boolean;
}
//----------------------------------------------------------------------------
/** 新增-電銷-產品-物件 */
const addCrmPhoneProductObj = reactive<AddCrmPhoneProductMdl>({
  employeePipelineID: 0,
  managerProductID: 0,
  managerProductSpecificationID: 0,
  managerProductMainKindID: 0,
  managerProductSubKindID: 0,
  managerProductMainKindName: "",
  managerProductSubKindName: "",
  managerProductSpecificationName: "",
});

/** 新增-電銷-產品-驗證物件 */
const addCrmPhoneProductValidObj = reactive<AddCrmPhoneProductValidateMdl>({
  managerProductMainKindID: true,
  managerProductSubKindID: true,
  managerProductID: true,
  managerProductSpecificationID: true,
});
//----------------------------------------------------------------------------
/** 驗證方法 */
const validateField = () => {
  let isValid = true;

  // 產品名稱
  if (!addCrmPhoneProductObj.managerProductID) {
    addCrmPhoneProductValidObj.managerProductID = false;
    isValid = false;
  } else {
    addCrmPhoneProductValidObj.managerProductID = true;
  }

  // 規格
  if (!addCrmPhoneProductObj.managerProductSpecificationID) {
    addCrmPhoneProductValidObj.managerProductSpecificationID = false;
    isValid = false;
  } else {
    addCrmPhoneProductValidObj.managerProductSpecificationID = true;
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

  // 驗證
  if (!validateField()) return;

  // 呼叫API
  const requestData: MbsCrmPhnHttpAddEmployeePipelineProductReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: props.employeePipelineID,
    managerProductID: addCrmPhoneProductObj.managerProductID,
    managerProductSpecificationID: addCrmPhoneProductObj.managerProductSpecificationID,
  };

  const responseData: MbsCrmPhnHttpAddEmployeePipelineProductRspMdl | null =
    await addPhoneEmployeePipelineProduct(requestData);

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
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">附加產品</h2>
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
              v-model:managerProductMainKindID="addCrmPhoneProductObj.managerProductMainKindID"
              v-model:managerProductMainKindName="addCrmPhoneProductObj.managerProductMainKindName"
              :disabled="false"
              placeholder="請選擇主分類"
            />
          </div>

          <!-- 產品子分類 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">產品子分類</label>
            <GetManyManagerProductSubKindComboBox
              v-model:managerProductSubKindID="addCrmPhoneProductObj.managerProductSubKindID"
              v-model:managerProductSubKindName="addCrmPhoneProductObj.managerProductSubKindName"
              :managerProductMainKindID="addCrmPhoneProductObj.managerProductMainKindID"
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
              v-model:managerProductID="addCrmPhoneProductObj.managerProductID"
              v-model:managerProductMainKindID="addCrmPhoneProductObj.managerProductMainKindID"
              v-model:managerProductSubKindID="addCrmPhoneProductObj.managerProductSubKindID"
              :disabled="false"
              :preventAutoReset="true"
              @product-selected="onProductSelected"
            />
            <div class="error-wrapper">
              <span v-if="!addCrmPhoneProductValidObj.managerProductID" class="error-tip">不可為空</span>
            </div>
          </div>

          <!-- 產品規格 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">產品規格 <span class="required-label">*</span></label>
            <GetManyManagerProductSpecificationComboBox
              v-model:managerProductSpecificationID="
                addCrmPhoneProductObj.managerProductSpecificationID
              "
              v-model:managerProductSpecificationName="
                addCrmPhoneProductObj.managerProductSpecificationName
              "
              :managerProductID="addCrmPhoneProductObj.managerProductID"
              :disabled="false"
            />
            <div class="error-wrapper">
              <span v-if="!addCrmPhoneProductValidObj.managerProductSpecificationID" class="error-tip">
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
