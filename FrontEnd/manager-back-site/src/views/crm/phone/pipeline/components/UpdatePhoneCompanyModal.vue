<script setup lang="ts">
import { reactive, watch, onMounted } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import GetManyManagerCompanyComboBox from "@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue";
import GetManyManagerCompanyLocationComboBox from "@/components/feature/search-bar/GetManyManagerCompanyLocationComboBox.vue";
import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { getCustomerGradeLabel } from "@/utils/getCustomerGradeLabel";
import {
  MbsCrmPhnHttpGetEmployeePipelineCompanyReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineCompanyReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineCompanyRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import {
  getBasicManagerCompanyLocation,
  GetBasicCompany,
  getPhoneEmployeePipelineCompany,
  updatePhoneEmployeePipelineCompany,
} from "@/services";
import { getManagerCompanyStatusLabel } from "@/utils/getManagerCompanyStatusLabel";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { getCityLabel } from "@/utils/getCityLabel";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import {
  MbsBscHttpGetManagerCompanyLocationReqMdl,
  MbsBscHttpGetManagerCompanyLocationRspMdl,
  MbsBscHttpGetManagerCompanyReqMdl,
  MbsBscHttpGetManagerCompanyRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
//----------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();
//----------------------------------------------------------
const props = defineProps<{
  employeePipelineID: number;
  hasCompany: boolean; // 是否已有公司
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//----------------------------------------------------------
/** 編輯-電銷-公司-模型 */
interface UpdatePhoneCompanyMdl {
  managerCompanyID: number | null;
  managerCompanyName: string | null;
  managerCompanyLocationID: number | null;
  managerCompanyLocationName: string | null;
  managerCompanyUnifiedNumber: string | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  managerCompanyCityName: DbAtomCityEnum | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerCompanyStatusName: DbAtomManagerCompanyStatusEnum | null;
}

/** 編輯-電銷-公司-驗證模型 */
interface UpdatePhoneCompanyValidMdl {
  managerCompanyID: boolean;
  managerCompanyLocationID: boolean;
}

/** 編輯-電銷-公司-物件 */
const updatePhoneCompanyObj = reactive<UpdatePhoneCompanyMdl>({
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
});

/** 編輯-電銷-公司-驗證物件 */
const updatePhoneCompanyValidObj = reactive<UpdatePhoneCompanyValidMdl>({
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
      managerCompanyCityName: responseData.company.atomCity,
      managerCompanyLocationName: responseData.company.managerCompanyLocationName,
      managerCompanyLocationAddress: responseData.company.managerCompanyLocationAddress,
      managerCompanyLocationTelephone: responseData.company.managerCompanyLocationTelephone,
      managerCompanyStatusName: responseData.company.managerCompanyLocationStatus,
    } satisfies Partial<UpdatePhoneCompanyMdl>);
  }
};

/** 取得【營業地點】資料(顯示下方資料) */
const getLocationData = async (locationID: number) => {
  const requestData: MbsBscHttpGetManagerCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyLocationID: locationID,
    managerCompanyLocationIsDeleted: false,
  };

  const responseData: MbsBscHttpGetManagerCompanyLocationRspMdl | null =
    await getBasicManagerCompanyLocation(requestData);

  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  if (responseData) {
    Object.assign(updatePhoneCompanyObj, {
      managerCompanyLocationName: responseData.managerCompanyLocationName,
      managerCompanyCityName: responseData.atomCity,
      managerCompanyLocationAddress: responseData.managerCompanyLocationAddress,
      managerCompanyLocationTelephone: responseData.managerCompanyLocationTelephone,
      managerCompanyStatusName: responseData.managerCompanyLocationStatus,
    } satisfies Partial<UpdatePhoneCompanyMdl>);
  }
};

/** 取得【公司】資料(顯示下方資料) */
const GetBasicCompanyData = async (companyID: number) => {
  const requestData: MbsBscHttpGetManagerCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: companyID,
  };

  const responseData: MbsBscHttpGetManagerCompanyRspMdl | null = await GetBasicCompany(requestData);

  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  if (responseData) {
    Object.assign(updatePhoneCompanyObj, {
      managerCompanyUnifiedNumber: responseData.managerCompanyUnifiedNumber,
      managerCompanyName: responseData.managerCompanyName,
      atomCustomerGrade: responseData.atomCustomerGrade,
      managerCompanyMainKindName: responseData.managerCompanyMainKindName,
      managerCompanySubKindName: responseData.managerCompanySubKindName,
    } satisfies Partial<UpdatePhoneCompanyMdl>);
  }
};
//----------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  if (!requireToken()) return;
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
  () => {
    updatePhoneCompanyObj.managerCompanyLocationID = null;
    updatePhoneCompanyObj.managerCompanyLocationName = null;
    updatePhoneCompanyObj.managerCompanyCityName = null;
    updatePhoneCompanyObj.managerCompanyLocationAddress = null;
    updatePhoneCompanyObj.managerCompanyLocationTelephone = null;
    updatePhoneCompanyObj.managerCompanyStatusName = null;
  }
);
// 當選擇地點變更時，取得地點資料
watch(
  () => updatePhoneCompanyObj.managerCompanyLocationID,
  async (newVal) => {
    if (!newVal || !updatePhoneCompanyObj.managerCompanyID) return;
    getLocationData(newVal);
  }
);
// 當選擇公司變更時，取得公司資料
watch(
  () => updatePhoneCompanyObj.managerCompanyID,
  async (newVal) => {
    if (!newVal || !updatePhoneCompanyObj.managerCompanyID) return;
    GetBasicCompanyData(newVal);
  }
);

onMounted(async () => {
  if (props.hasCompany) {
    // 已有公司 進入編輯模式
    await getCompanyData();
  }
});
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯客戶</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 flex-1">
        <div class="space-y-4">
          <!-- 後端錯誤訊息 -->
          <p
            v-if="showError"
            class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
          >
            {{ errorMessage }}
          </p>

          <div class="flex gap-4 w-full">
            <!-- 客戶 -->
            <div class="flex flex-col gap-2 flex-1">
              <label class="form-label">客戶 <span class="required-label">*</span></label>
              <GetManyManagerCompanyComboBox
                v-model:managerCompanyID="updatePhoneCompanyObj.managerCompanyID"
                v-model:managerCompanyName="updatePhoneCompanyObj.managerCompanyName"
                :disabled="false"
                @change="resetError"
              />
              <p class="error-wrapper">
                <span v-if="!updatePhoneCompanyValidObj.managerCompanyID" class="error-tip">
                  不可為空
                </span>
              </p>
            </div>

            <!-- 營業地點 -->
            <div class="flex flex-col gap-2 flex-1">
              <label class="form-label">營業地點 <span class="required-label">*</span></label>
              <GetManyManagerCompanyLocationComboBox
                v-model:managerCompanyLocationID="updatePhoneCompanyObj.managerCompanyLocationID"
                v-model:managerCompanyLocationName="
                  updatePhoneCompanyObj.managerCompanyLocationName
                "
                :managerCompanyID="updatePhoneCompanyObj.managerCompanyID"
                :disabled="!updatePhoneCompanyObj.managerCompanyID"
                @change="resetError"
              />
              <p class="error-wrapper">
                <span v-if="!updatePhoneCompanyValidObj.managerCompanyLocationID" class="error-tip">
                  不可為空
                </span>
              </p>
            </div>
          </div>

          <!-- 內容顯示 -->
          <div
            class="bg-gray-50 border border-gray-200 rounded-md p-6 text-gray-700 overflow-y-auto max-h-80"
          >
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">統編</span>
              <span class="display-value truncate">{{
                updatePhoneCompanyObj.managerCompanyUnifiedNumber || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <!-- 客戶全名 -->
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶全名</span>
              <span class="display-value truncate">{{
                updatePhoneCompanyObj.managerCompanyName || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <!-- 人員規模 -->
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">人員規模</span>
              <span class="display-value truncate">{{
                getEmployeeRangeLabel(updatePhoneCompanyObj.atomEmployeeRange) || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <!-- 客戶分級 -->
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶分級</span>
              <span class="display-value truncate">{{
                getCustomerGradeLabel(updatePhoneCompanyObj.atomCustomerGrade) || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <!-- 客戶主分類 -->
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶主分類</span>
              <span class="display-value truncate">{{
                updatePhoneCompanyObj.managerCompanyMainKindName || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <!-- 客戶子分類 -->
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶子分類</span>
              <span class="display-value truncate">{{
                updatePhoneCompanyObj.managerCompanySubKindName || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <!-- 縣市 -->
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">縣市</span>
              <span class="display-value truncate">{{
                getCityLabel(updatePhoneCompanyObj.managerCompanyCityName) || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">地址</span>
              <span class="display-value truncate">{{
                updatePhoneCompanyObj.managerCompanyLocationAddress || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電話</span>
              <span class="display-value truncate">{{
                updatePhoneCompanyObj.managerCompanyLocationTelephone || "-"
              }}</span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">狀態</span>
              <span class="display-value truncate">{{
                getManagerCompanyStatusLabel(updatePhoneCompanyObj.managerCompanyStatusName) || "-"
              }}</span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
