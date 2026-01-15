<script setup lang="ts">
import { reactive, watch } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import GetManyManagerCompanyComboBox from "@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue";
import GetManyManagerCompanyLocationComboBox from "@/components/feature/search-bar/GetManyManagerCompanyLocationComboBox.vue";
import { GetBasicCompany, getBasicManagerCompanyLocation } from "@/services";
import {
  MbsBscHttpGetManagerCompanyReqMdl,
  MbsBscHttpGetManagerCompanyRspMdl,
  MbsBscHttpGetManagerCompanyLocationReqMdl,
  MbsBscHttpGetManagerCompanyLocationRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { getCityLabel } from "@/utils/getCityLabel";
import { getManagerCompanyStatusLabel } from "@/utils/getManagerCompanyStatusLabel";
// import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { getCustomerGradeLabel } from "@/utils/getCustomerGradeLabel";

//----------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();

//----------------------------------------------------------
/** 客戶資訊項目 */
interface CompanyInfo {
  managerCompanyUnifiedNumber: string;
  managerCompanyID: number | null;
  managerCompanyName: string;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  managerCompanyMainKindName: string;
  managerCompanySubKindName: string;
  atomCity: DbAtomCityEnum | null;
  managerCompanyLocationID: number | null;
  managerCompanyLocationName: string;
  managerCompanyLocationAddress: string;
  managerCompanyLocationTelephone: string;
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

const props = defineProps<{
  show: boolean;
  currentCompany?: CompanyInfo | null;
}>();

const emit = defineEmits<{
  (e: "confirm", data: CompanyInfo): void;
  (e: "cancel"): void;
}>();

//----------------------------------------------------------
/** 附加客戶模型 */
interface AddCompanyMdl {
  managerCompanyID: number | null;
  managerCompanyName: string | null;
  managerCompanyLocationID: number | null;
  managerCompanyLocationName: string | null;
  managerCompanyUnifiedNumber: string | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  atomCity: DbAtomCityEnum | null;
  managerCompanyCityName: string | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerCompanyStatusName: string | null;
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 附加客戶驗證模型 */
interface AddCompanyValidateMdl {
  managerCompanyID: boolean;
  managerCompanyLocationID: boolean;
}

//--------------------------------------------------------------------------
/** 附加客戶物件 */
const addCompanyObj = reactive<AddCompanyMdl>({
  managerCompanyID: null,
  managerCompanyName: null,
  managerCompanyLocationID: null,
  managerCompanyLocationName: null,
  managerCompanyUnifiedNumber: null,
  atomEmployeeRange: null,
  atomCustomerGrade: null,
  managerCompanyMainKindName: null,
  managerCompanySubKindName: null,
  atomCity: null,
  managerCompanyCityName: null,
  managerCompanyLocationAddress: null,
  managerCompanyLocationTelephone: null,
  managerCompanyStatusName: null,
  managerCompanyLocationStatus: null,
});

/** 附加客戶驗證物件 */
const addCompanyValidateObj = reactive<AddCompanyValidateMdl>({
  managerCompanyID: true,
  managerCompanyLocationID: true,
});

//----------------------------------------------------------
/** 取得【管理者公司】資料 */
const getManagerCompanyData = async (managerCompanyID: number) => {
  if (!requireToken()) return;

  const requestData: MbsBscHttpGetManagerCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: managerCompanyID,
  };

  const responseData: MbsBscHttpGetManagerCompanyRspMdl | null = await GetBasicCompany(requestData);
  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  addCompanyObj.managerCompanyID = responseData.managerCompanyID;
  addCompanyObj.managerCompanyUnifiedNumber = responseData.managerCompanyUnifiedNumber;
  addCompanyObj.managerCompanyName = responseData.managerCompanyName;
  addCompanyObj.managerCompanyMainKindName = responseData.managerCompanyMainKindName;
  addCompanyObj.managerCompanySubKindName = responseData.managerCompanySubKindName;
  addCompanyObj.atomCustomerGrade = responseData.atomCustomerGrade;
  addCompanyObj.atomEmployeeRange = responseData.atomEmployeeRange;
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

  addCompanyObj.managerCompanyLocationName = responseData.managerCompanyLocationName;
  addCompanyObj.atomCity = responseData.atomCity;
  addCompanyObj.managerCompanyCityName = getCityLabel(responseData.atomCity);
  addCompanyObj.managerCompanyLocationAddress = responseData.managerCompanyLocationAddress;
  addCompanyObj.managerCompanyLocationTelephone = responseData.managerCompanyLocationTelephone;
  addCompanyObj.managerCompanyLocationStatus = responseData.managerCompanyLocationStatus;
  addCompanyObj.managerCompanyStatusName = getManagerCompanyStatusLabel(
    responseData.managerCompanyLocationStatus
  );
};

//-----------------------------------------------------------
/** 驗證方法 */
const validateField = (): boolean => {
  let isValid = true;

  // 公司
  if (!addCompanyObj.managerCompanyID) {
    addCompanyValidateObj.managerCompanyID = false;
    isValid = false;
  } else {
    addCompanyValidateObj.managerCompanyID = true;
  }

  // 營業地點
  if (!addCompanyObj.managerCompanyLocationID) {
    addCompanyValidateObj.managerCompanyLocationID = false;
    isValid = false;
  } else {
    addCompanyValidateObj.managerCompanyLocationID = true;
  }

  return isValid;
};

//----------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = () => {
  if (!validateField()) {
    return;
  }

  if (!addCompanyObj.managerCompanyID || !addCompanyObj.managerCompanyLocationID) {
    showError.value = true;
    errorMessage.value = "請選擇完整的客戶與營業地點";
    return;
  }

  const companyData: CompanyInfo = {
    managerCompanyUnifiedNumber: addCompanyObj.managerCompanyUnifiedNumber || "",
    managerCompanyID: addCompanyObj.managerCompanyID,
    managerCompanyName: addCompanyObj.managerCompanyName || "",
    atomEmployeeRange: addCompanyObj.atomEmployeeRange,
    atomCustomerGrade: addCompanyObj.atomCustomerGrade,
    managerCompanyMainKindName: addCompanyObj.managerCompanyMainKindName || "",
    managerCompanySubKindName: addCompanyObj.managerCompanySubKindName || "",
    atomCity: addCompanyObj.atomCity,
    managerCompanyLocationID: addCompanyObj.managerCompanyLocationID,
    managerCompanyLocationName: addCompanyObj.managerCompanyLocationName || "",
    managerCompanyLocationAddress: addCompanyObj.managerCompanyLocationAddress || "",
    managerCompanyLocationTelephone: addCompanyObj.managerCompanyLocationTelephone || "",
    managerCompanyLocationStatus: addCompanyObj.managerCompanyLocationStatus,
  };

  emit("confirm", companyData);
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  emit("cancel");
};

/** 重置錯誤提示 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};

/** 重置表單 */
const resetForm = () => {
  addCompanyObj.managerCompanyID = null;
  addCompanyObj.managerCompanyName = null;
  addCompanyObj.managerCompanyLocationID = null;
  addCompanyObj.managerCompanyLocationName = null;
  addCompanyObj.managerCompanyUnifiedNumber = null;
  addCompanyObj.atomEmployeeRange = null;
  addCompanyObj.atomCustomerGrade = null;
  addCompanyObj.managerCompanyMainKindName = null;
  addCompanyObj.managerCompanySubKindName = null;
  addCompanyObj.atomCity = null;
  addCompanyObj.managerCompanyCityName = null;
  addCompanyObj.managerCompanyLocationAddress = null;
  addCompanyObj.managerCompanyLocationTelephone = null;
  addCompanyObj.managerCompanyStatusName = null;
  addCompanyObj.managerCompanyLocationStatus = null;

  addCompanyValidateObj.managerCompanyID = true;
  addCompanyValidateObj.managerCompanyLocationID = true;

  resetError();
};

// 當選擇公司變更時，清空地點
watch(
  () => addCompanyObj.managerCompanyID,
  async (newVal) => {
    if (
      props.currentCompany?.managerCompanyLocationID &&
      newVal === props.currentCompany?.managerCompanyID
    ) {
      // 編輯狀態且公司未變更時，不清空地點
      return;
    } else {
      // 清空營業地點ID與名稱
      addCompanyObj.managerCompanyLocationID = null;
      addCompanyObj.managerCompanyLocationName = null;

      // 清空營業地點相關顯示資訊
      addCompanyObj.atomCity = null;
      addCompanyObj.managerCompanyCityName = null;
      addCompanyObj.managerCompanyLocationAddress = null;
      addCompanyObj.managerCompanyLocationTelephone = null;
      addCompanyObj.managerCompanyStatusName = null;
      addCompanyObj.managerCompanyLocationStatus = null;
    }

    if (newVal) {
      await getManagerCompanyData(newVal);
    }
  }
);

// 當選擇地點變更時，取得地點資料
watch(
  () => addCompanyObj.managerCompanyLocationID,
  async (newVal) => {
    if (newVal) {
      await getManagerCompanyLocationData(newVal);
    }
  }
);

// 監聽彈窗顯示狀態
watch(
  () => props.show,
  (newVal) => {
    if (newVal) {
      // 彈窗開啟時，如果有現有客戶資料則載入
      if (props.currentCompany) {
        addCompanyObj.managerCompanyID = props.currentCompany.managerCompanyID;
        addCompanyObj.managerCompanyName = props.currentCompany.managerCompanyName;
        addCompanyObj.managerCompanyLocationID = props.currentCompany.managerCompanyLocationID;
        addCompanyObj.managerCompanyLocationName = props.currentCompany.managerCompanyLocationName;
        addCompanyObj.managerCompanyUnifiedNumber =
          props.currentCompany.managerCompanyUnifiedNumber;
        addCompanyObj.atomEmployeeRange = props.currentCompany.atomEmployeeRange;
        addCompanyObj.atomCustomerGrade = props.currentCompany.atomCustomerGrade;
        addCompanyObj.managerCompanyMainKindName = props.currentCompany.managerCompanyMainKindName;
        addCompanyObj.managerCompanySubKindName = props.currentCompany.managerCompanySubKindName;
        addCompanyObj.atomCity = props.currentCompany.atomCity;
        addCompanyObj.managerCompanyCityName = getCityLabel(props.currentCompany.atomCity);
        addCompanyObj.managerCompanyLocationAddress =
          props.currentCompany.managerCompanyLocationAddress;
        addCompanyObj.managerCompanyLocationTelephone =
          props.currentCompany.managerCompanyLocationTelephone;
        addCompanyObj.managerCompanyLocationStatus =
          props.currentCompany.managerCompanyLocationStatus;
        addCompanyObj.managerCompanyStatusName = getManagerCompanyStatusLabel(
          props.currentCompany.managerCompanyLocationStatus
        );
      }
    } else {
      // 彈窗關閉時重置表單
      resetForm();
    }
  }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div class="w-[630px] max-w-full max-h-[90vh] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">
          {{ currentCompany ? "編輯客戶" : "附加客戶" }}
        </h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="clickCancelBtn"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="flex flex-col gap-6 p-6">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded mb-3"
        >
          {{ errorMessage }}
        </p>

        <!-- 查詢區 -->
        <div class="flex gap-4 w-full">
          <!-- 客戶 -->
          <div >
            <label class="form-label">客戶<span class="required-label">*</span></label>
            <GetManyManagerCompanyComboBox
              v-model:managerCompanyID="addCompanyObj.managerCompanyID"
              v-model:managerCompanyName="addCompanyObj.managerCompanyName"
              :disabled="false"
              @change="resetError"
            />
            <div class="h-2">
              <p v-if="!addCompanyValidateObj.managerCompanyID" class="error-tip">不可為空</p>
            </div>
          </div>

          <!-- 營業地點 -->
          <div >
            <label class="form-label">營業地點 <span class="required-label">*</span></label>
            <GetManyManagerCompanyLocationComboBox
              v-model:managerCompanyLocationID="addCompanyObj.managerCompanyLocationID"
              v-model:managerCompanyLocationName="addCompanyObj.managerCompanyLocationName"
              :managerCompanyID="addCompanyObj.managerCompanyID"
              :disabled="!addCompanyObj.managerCompanyID"
              @change="resetError"
            />
            <div class="h-2">
              <p v-if="!addCompanyValidateObj.managerCompanyLocationID" class="error-tip">
                不可為空
              </p>
            </div>
          </div>
        </div>

        <!-- 客戶資訊顯示區 -->
        <div class="w-full">
          <div class="bg-white rounded-lg border border-gray-300 flex flex-col h-64">
            

            <!-- 內容 -->
            <div class="p-6 flex-1 overflow-y-auto">
              <template v-if="!addCompanyObj.managerCompanyID">
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
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyUnifiedNumber || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶全名</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <!-- <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">人員規模</span>
                  <span class="display-value">{{
                    getEmployeeRangeLabel(addCompanyObj.atomEmployeeRange) || "-"
                  }}</span>
                </p>
                <hr class="my-2" /> -->

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶分級</span>
                  <span class="display-value">{{
                    getCustomerGradeLabel(addCompanyObj.atomCustomerGrade) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶主分類</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyMainKindName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">客戶子分類</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanySubKindName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">縣市</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyCityName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">地址</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyLocationAddress || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電話</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyLocationTelephone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">狀態</span>
                  <span class="display-value">{{
                    addCompanyObj.managerCompanyStatusName || "-"
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
        <button class="btn-cancel" @click="clickCancelBtn">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>
</template>
