<script setup lang="ts">
import { reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import {
  MbsCrmPhnHttpAddEmployeePipelineSalerReqMdl,
  MbsCrmPhnHttpAddEmployeePipelineSalerRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import { addPhoneEmployeePipelineSaler } from "@/services";
import GetManyEmployeeComboBox from "@/components/feature/search-bar/GetManyEmployeeComboBox.vue";
import GetManyManagerRoleComboBox from "@/components/feature/search-bar/GetManyManagerRoleComboBox.vue";
//----------------------------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();

//-------------------------------------------------------------
const props = defineProps<{
  employeePipelineID: number;
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//-------------------------------------------------------------
/** 新增-電銷-指派業務-模型 */
interface AddCrmPhoneSalerMdl {
  employeePipelineID: number;
  managerProductName: string;
  managerRoleID: number;
  managerRegionID: number;
  managerDepartmentName: string;
  managerRoleName: string;
  managerRegionName: string;
  employeePipelineSalerEmployeeID: number;
  managerEmployeeName: string;
}

/** 新增-電銷-指派業務-驗證模型 */
interface AddCrmPhoneSalerValidateMdl {
  managerRegionID: boolean;
  managerRoleID: boolean;
  employeePipelineSalerEmployeeID: boolean;
}
//-------------------------------------------------------------------
/** 新增-電銷-指派業務-物件 */
const addCrmPhoneSalerObj = reactive<AddCrmPhoneSalerMdl>({
  employeePipelineID: 0,
  employeePipelineSalerEmployeeID: 0,
  managerProductName: "",
  managerRoleID: 0,
  managerRegionID: 0,
  managerDepartmentName: "",
  managerRoleName: "",
  managerRegionName: "",
  managerEmployeeName: "",
});

/** 新增-電銷-指派業務-驗證物件 */
const addCrmPhoneSalerValidObj = reactive<AddCrmPhoneSalerValidateMdl>({
  managerRegionID: true,
  managerRoleID: true,
  employeePipelineSalerEmployeeID: true,
});
//-------------------------------------------------------------
/** 驗證方法 */
const validateField = () => {
  let isValid = true;
  // 業務
  if (!addCrmPhoneSalerObj.employeePipelineSalerEmployeeID) {
    addCrmPhoneSalerValidObj.employeePipelineSalerEmployeeID = false;
    isValid = false;
  } else addCrmPhoneSalerValidObj.employeePipelineSalerEmployeeID = true;

  return isValid;
};
//--------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 驗證
  if (!validateField()) return;

  // 呼叫api
  const requestData: MbsCrmPhnHttpAddEmployeePipelineSalerReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: props.employeePipelineID!,
    employeePipelineSalerEmployeeID: addCrmPhoneSalerObj.employeePipelineSalerEmployeeID!,
  };

  const responseData: MbsCrmPhnHttpAddEmployeePipelineSalerRspMdl | null =
    await addPhoneEmployeePipelineSaler(requestData);

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
/** 重置錯誤 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">指派業務</h2>
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
          <!-- 地區 -->
          <!-- <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">地區</label>
            <GetManyManagerRegionComboBox
              v-model:managerRegionID="addCrmPhoneSalerObj.managerRegionID"
              v-model:managerRegionName="addCrmPhoneSalerObj.managerRegionName"
              :disabled="false"
              placeholder="請選擇地區"
              @change="resetError"
            />
          </div> -->

          <!-- 角色 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">角色</label>
            <GetManyManagerRoleComboBox
              v-model:managerRoleID="addCrmPhoneSalerObj.managerRoleID"
              v-model:managerRoleName="addCrmPhoneSalerObj.managerRoleName"
              v-model:managerDepartmentName="addCrmPhoneSalerObj.managerDepartmentName"
              v-model:managerRegionID="addCrmPhoneSalerObj.managerRegionID"
              v-model:managerRegionName="addCrmPhoneSalerObj.managerRegionName"
              :disabled="false"
              @change="resetError"
            />
          </div>
        </div>

        <hr class="my-6" />

        <!-- 業務 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">業務 <span class="required-label">*</span></label>
          <GetManyEmployeeComboBox
            v-model:managerEmployeeID="addCrmPhoneSalerObj.employeePipelineSalerEmployeeID"
            v-model:managerEmployeeName="addCrmPhoneSalerObj.managerEmployeeName"
            :managerRoleID="addCrmPhoneSalerObj.managerRoleID"
            :disabled="false"
            @change="resetError"
          />
          <div class="error-wrapper">
            <p v-if="!addCrmPhoneSalerValidObj.employeePipelineSalerEmployeeID" class="error-tip">
              不可為空
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
