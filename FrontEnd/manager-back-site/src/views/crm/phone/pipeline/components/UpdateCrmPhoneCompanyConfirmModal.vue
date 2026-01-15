<script setup lang="ts">
import { reactive, watch, onMounted } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import GetManyManagerCompanyComboBox from "@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue";
import GetManyManagerCompanyLocationComboBox from "@/components/feature/search-bar/GetManyManagerCompanyLocationComboBox.vue";
import {
  MbsCrmPhnHttpGetEmployeePipelineCompanyReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineCompanyReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import {
  GetBasicCompany,
  getPhoneEmployeePipelineCompany,
  updatePhoneEmployeePipelineCompany,
  getBasicManagerCompanyLocation,
} from "@/services";
import {
  MbsBscHttpGetManagerCompanyReqMdl,
  MbsBscHttpGetManagerCompanyRspMdl,
  MbsBscHttpGetManagerCompanyLocationReqMdl,
  MbsBscHttpGetManagerCompanyLocationRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { getCityLabel } from "@/utils/getCityLabel";
import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { getManagerCompanyStatusLabel } from "@/utils/getManagerCompanyStatusLabel";
import { getCustomerGradeLabel } from "@/utils/getCustomerGradeLabel";
//----------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();
//----------------------------------------------------------
const props = defineProps<{
  employeePipelineID: number;
  hasCompany: boolean; // 是否已有公司

  originalCompany?: {
    managerCompanyName: string | null;
    atomEmployeeRange?: string | null;
    managerCompanyMainKindName?: string | null;
    managerCompanySubKindName?: string | null;
    managerCompanyLocationAddress?: string | null;
    managerCompanyLocationTelephone?: string | null;
  };
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//----------------------------------------------------------
/** 編輯-電銷-公司-更正-模型 */
interface UpdateCrmPhoneCompanyConfirmMdl {
  managerCompanyID: number | null;
  managerCompanyName: string | null;
  managerCompanyLocationID: number | null;
  managerCompanyLocationName: string | null;
  managerCompanyUnifiedNumber: string | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  managerCompanyCityName: string | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerCompanyStatusName: string | null;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 編輯-電銷-公司-更正-驗證模型 */
interface UpdateCrmPhoneCompanyValidateMdl {
  managerCompanyID: boolean;
  managerCompanyLocationID: boolean;
}
//--------------------------------------------------------------------------
/** 編輯-電銷-公司-更正-物件 */
const updatePhoneCompanyObj = reactive<UpdateCrmPhoneCompanyConfirmMdl>({
  managerCompanyID: null,
  managerCompanyName: null,
  managerCompanyLocationID: null,
  managerCompanyLocationName: null,
  managerCompanyUnifiedNumber: null,
  atomEmployeeRange: null,
  atomCustomerGrade: null,
  managerCompanyMainKindName: null,
  managerCompanySubKindName: null,
  managerCompanyCityName: null,
  managerCompanyLocationAddress: null,
  managerCompanyLocationTelephone: null,
  managerCompanyStatusName: null,
  atomManagerCompanyStatus: null,
});

/** 編輯-電銷-公司-更正-驗證物件 */
const updatePhoneCompanyValidateObj = reactive<UpdateCrmPhoneCompanyValidateMdl>({
  managerCompanyID: true,
  managerCompanyLocationID: true,
});
//----------------------------------------------------------
/** 取得【公司】資料 */
const getCompanyData = async () => {
  if (!requireToken()) return;

  const requestData: MbsCrmPhnHttpGetEmployeePipelineCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: props.employeePipelineID,
  };

  const responseData = await getPhoneEmployeePipelineCompany(requestData);

  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  if (responseData.company) {
    Object.assign(updatePhoneCompanyObj, {
      managerCompanyUnifiedNumber: responseData.company.managerCompanyUnifiedNumber,
      managerCompanyName: responseData.company.managerCompanyName,
      atomEmployeeRange: responseData.company.atomEmployeeRange,
      atomCustomerGrade: responseData.company.atomCustomerGrade,
      managerCompanyMainKindName: responseData.company.managerCompanyMainKindName,
      managerCompanySubKindName: responseData.company.managerCompanySubKindName,
      managerCompanyCityName: getCityLabel(responseData.company.atomCity),
      managerCompanyLocationName: responseData.company.managerCompanyLocationAddress,
      managerCompanyLocationAddress: responseData.company.managerCompanyLocationAddress,
      managerCompanyLocationTelephone: responseData.company.managerCompanyLocationTelephone,
      managerCompanyStatusName: responseData.company.managerCompanyLocationStatus,
    });
  }
};

/** 取得【管理者公司】資料 */
const getManagerCompanyData = async (managerCompanyID: number) => {
  if (!requireToken()) return;

  const requestData: MbsBscHttpGetManagerCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: managerCompanyID,
  };

  const responseData: MbsBscHttpGetManagerCompanyRspMdl | null = await GetBasicCompany(requestData);
  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  updatePhoneCompanyObj.managerCompanyID = responseData.managerCompanyID;
  updatePhoneCompanyObj.managerCompanyUnifiedNumber = responseData.managerCompanyUnifiedNumber;
  updatePhoneCompanyObj.managerCompanyName = responseData.managerCompanyName;
  updatePhoneCompanyObj.atomManagerCompanyStatus = responseData.atomManagerCompanyStatus;
  updatePhoneCompanyObj.managerCompanyMainKindName = responseData.managerCompanyMainKindName;
  updatePhoneCompanyObj.managerCompanySubKindName = responseData.managerCompanySubKindName;
  updatePhoneCompanyObj.atomCustomerGrade = responseData.atomCustomerGrade;
  updatePhoneCompanyObj.atomEmployeeRange = responseData.atomEmployeeRange;
};

/** 取得【管理者公司營業地點】資料 */
const getManagerCompanyLocationData = async (managerCompanyLocationID: number) => {
  if (!requireToken()) return;

  const requestData: MbsBscHttpGetManagerCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyLocationID: managerCompanyLocationID,
    managerCompanyLocationIsDeleted: false,
  };

  const responseData: MbsBscHttpGetManagerCompanyLocationRspMdl | null =
    await getBasicManagerCompanyLocation(requestData);
  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  updatePhoneCompanyObj.managerCompanyLocationName = responseData.managerCompanyLocationName;
  updatePhoneCompanyObj.managerCompanyCityName = getCityLabel(responseData.atomCity);
  updatePhoneCompanyObj.managerCompanyLocationAddress = responseData.managerCompanyLocationAddress;
  updatePhoneCompanyObj.managerCompanyLocationTelephone =
    responseData.managerCompanyLocationTelephone;
  updatePhoneCompanyObj.managerCompanyStatusName = getManagerCompanyStatusLabel(
    responseData.managerCompanyLocationStatus
  );
};
//-----------------------------------------------------------
/** 驗證方法 */
const validateField = (): boolean => {
  let isValid = true;

  // 公司
  if (!updatePhoneCompanyObj.managerCompanyID) {
    updatePhoneCompanyValidateObj.managerCompanyID = false;
    isValid = false;
  } else {
    updatePhoneCompanyValidateObj.managerCompanyID = true;
  }

  // 營業地點
  if (!updatePhoneCompanyObj.managerCompanyLocationID) {
    updatePhoneCompanyValidateObj.managerCompanyLocationID = false;
    isValid = false;
  } else {
    updatePhoneCompanyValidateObj.managerCompanyLocationID = true;
  }

  return isValid;
};
//----------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  if (!requireToken()) return;

  if (!validateField()) {
    return;
  }

  if (!updatePhoneCompanyObj.managerCompanyID || !updatePhoneCompanyObj.managerCompanyLocationID) {
    showError.value = true;
    errorMessage.value = "請選擇完整的客戶與營業地點";
    return;
  }

  const requestData: MbsCrmPhnHttpUpdateEmployeePipelineCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: props.employeePipelineID,
    managerCompanyID: updatePhoneCompanyObj.managerCompanyID!,
    managerCompanyLocationID: updatePhoneCompanyObj.managerCompanyLocationID!,
  };

  const responseData: MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl | null =
    await updatePhoneEmployeePipelineCompany(requestData);

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

/** 重置錯誤提示 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};

// 當選擇公司變更時，清空地點
watch(
  () => updatePhoneCompanyObj.managerCompanyID,
  async (newVal) => {
    // 清空營業地點ID與名稱
    updatePhoneCompanyObj.managerCompanyLocationID = null;
    updatePhoneCompanyObj.managerCompanyLocationName = null;

    // 清空營業地點相關顯示資訊
    updatePhoneCompanyObj.managerCompanyCityName = null;
    updatePhoneCompanyObj.managerCompanyLocationAddress = null;
    updatePhoneCompanyObj.managerCompanyLocationTelephone = null;
    updatePhoneCompanyObj.managerCompanyStatusName = null;

    if (newVal) {
      await getManagerCompanyData(newVal);
    }
  }
);

// 當選擇地點變更時，取得地點資料
watch(
  () => updatePhoneCompanyObj.managerCompanyLocationID,
  async (newVal) => {
    if (newVal) {
      await getManagerCompanyLocationData(newVal);
    }
  }
);

onMounted(async () => {
  if (props.hasCompany) {
    // 已有公司 進入編輯模式
    await getCompanyData();
  }
  if (updatePhoneCompanyObj.managerCompanyID) {
    await getManagerCompanyData(updatePhoneCompanyObj.managerCompanyID);
  }
});
</script>

<template>
  <div class="modal-overlay">
    <div class="max-h-[90vh] w-[900px] max-w-[95vw] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">更正客戶</h2>
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

        <!--查詢-->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 w-full">
          <!-- 客戶 -->
          <div>
            <label class="form-label">客戶<span class="required-label">*</span></label>
            <GetManyManagerCompanyComboBox
              v-model:managerCompanyID="updatePhoneCompanyObj.managerCompanyID"
              v-model:managerCompanyName="updatePhoneCompanyObj.managerCompanyName"
              :disabled="false"
              @change="resetError"
            />
            <div class="error-wrapper">
              <span v-if="!updatePhoneCompanyValidateObj.managerCompanyID" class="error-tip">
                不可為空
              </span>
            </div>
          </div>
          <!-- 營業地點 -->
          <div>
            <label class="form-label">營業地點 <span class="required-label">*</span></label>
            <GetManyManagerCompanyLocationComboBox
              v-model:managerCompanyLocationID="updatePhoneCompanyObj.managerCompanyLocationID"
              v-model:managerCompanyLocationName="updatePhoneCompanyObj.managerCompanyLocationName"
              :managerCompanyID="updatePhoneCompanyObj.managerCompanyID"
              :disabled="!updatePhoneCompanyObj.managerCompanyID"
              @change="resetError"
            />
            <div class="error-wrapper">
              <span
                v-if="!updatePhoneCompanyValidateObj.managerCompanyLocationID"
                class="error-tip"
              >
                不可為空
              </span>
            </div>
          </div>
        </div>

        <!-- 內容區:左右兩區分 -->
        <div class="flex gap-4 h-[280px]">
          <!-- 左邊：原始客戶 -->
          <div class="flex-1 w-1/2 bg-white rounded-lg border border-gray-300">
            <!-- 標題 -->
            <div class="bg-gray-100 p-1 rounded-t-lg">
              <h3 class="text-brand-201 font-semibold ml-2 text-sm p-1">原始客戶</h3>
            </div>

            <!-- 內容 -->
            <div class="p-4">
              <template v-if="props.originalCompany">
                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶全名</span>
                  <span class="display-value break-words">{{
                    props.originalCompany.managerCompanyName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">人員規模</span>
                  <span class="display-value break-words">{{
                    props.originalCompany.atomEmployeeRange || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶主分類</span>
                  <span class="display-value break-words">{{
                    props.originalCompany.managerCompanyMainKindName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶子分類</span>
                  <span class="display-value break-words">{{
                    props.originalCompany.managerCompanySubKindName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">地址</span>
                  <span class="display-value break-words">{{
                    props.originalCompany.managerCompanyLocationAddress || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電話</span>
                  <span class="display-value break-words">{{
                    props.originalCompany.managerCompanyLocationTelephone || "-"
                  }}</span>
                </p>
              </template>

              <template v-else>
                <div class="text-center text-gray-400 py-6">無原始客戶資料</div>
              </template>
            </div>
          </div>

          <!-- 右邊：確認客戶 -->
          <div class="flex-1 w-1/2 bg-white rounded-lg border border-gray-300">
            <!-- 標題 -->
            <div class="bg-gray-100 p-1 rounded-t-lg">
              <h3 class="text-brand-201 font-semibold ml-2 text-sm p-1">確認客戶</h3>
            </div>

            <!-- 內容 -->
            <div class="p-3 flex-1 overflow-y-auto h-[240px]">
              <template v-if="!updatePhoneCompanyObj.managerCompanyID">
                <div
                  class="flex flex-col items-center justify-center text-center text-gray-400 h-full"
                >
                  <div class="text-4xl mb-2">!</div>
                  <p class="mb-4 text-sm">請先查詢客戶資料</p>
                </div>
              </template>

              <template v-else>
                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">統編</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanyUnifiedNumber || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶全名</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanyName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">人員規模</span>
                  <span class="display-value break-words">{{
                    getEmployeeRangeLabel(updatePhoneCompanyObj.atomEmployeeRange) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶分級</span>
                  <span class="display-value break-words">{{
                    getCustomerGradeLabel(updatePhoneCompanyObj.atomCustomerGrade) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶主分類</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanyMainKindName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶子分類</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanySubKindName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">縣市</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanyCityName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">地址</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanyLocationAddress || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電話</span>
                  <span class="display-value break-words">{{
                    updatePhoneCompanyObj.managerCompanyLocationTelephone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">狀態</span>
                  <span class="display-value break-words">{{
                    getManagerCompanyStatusLabel(updatePhoneCompanyObj.atomManagerCompanyStatus) ||
                    "-"
                  }}</span>
                </p>
                <hr class="my-2" />
              </template>
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
