<script setup lang="ts">
//#region 引入
import { reactive, defineAsyncComponent, watch } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useRouter, useRoute } from "vue-router";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import {
  addActivityEmployeePipeline,
  getBasicManagerContacter,
  GetBasicCompany,
  getBasicManagerCompanyLocation,
} from "@/services";
import type {
  MbsCrmActHttpAddActivityEmployeePipelineReqMdl,
  MbsCrmActHttpAddActivityEmployeePipelineRspMdl,
} from "@/services/pms-http/crm/activity/crmActivityHttpFormat";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import {
  MbsBscHttpGetManagerCompanyLocationReqMdl,
  MbsBscHttpGetManagerCompanyReqMdl,
  MbsSysCttHttpGetManagerContacterReqMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { getCityLabel } from "@/utils/getCityLabel";
import { getManagerCompanyStatusLabel } from "@/utils/getManagerCompanyStatusLabel";
import { getCustomerGradeLabel } from "@/utils/getCustomerGradeLabel";
import { getManagerContacterStatusLabel } from "@/utils/getManagerContacterStatusLabel";
// import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyContacterComboBox from "@/components/feature/search-bar/GetManyContacterComboBox.vue";
const GetManyManagerCompanyComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue")
);
const GetManyManagerCompanyLocationComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyLocationComboBox.vue")
);
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
/** 路由參數取得 */
const route = useRoute();
//#endregion

//#region 型別定義
/** Crm-活動名單-新增-頁面模型 */
interface CrmActivityEmployeePipelineAddViewMdl {
  managerActivityID: number | null;
  managerCompanyID: number;
  managerCompanyLocationID: number | null;
  managerCompanyLocationName: string | null;
  managerContacterID: number | null;
  atomPipelineStatus: DbAtomPipelineStatusEnum;
}
/** Crm-活動名單-新增-驗證模型 */
interface CrmActivityEmployeePipelineAddValidMdl {
  managerCompanyID: boolean;
  managerCompanyLocationID: boolean;
  managerContacterID: boolean;
}

/** 系統設定-客戶-檢視-頁面模型 */
interface SysCompanyDetailViewMdl {
  managerCompanyUnifiedNumber: string | null;
  managerCompanyName: string | null;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  atomSecurityGrade: DbAtomSecurityGradeEnum | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  managerCompanyLocationID: number | null;
  managerCompanyLocationName: string | null;
  atomCity: DbAtomCityEnum | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 系統設定-窗口-檢視-頁面模型 */
interface SysContacterDetailViewMdl {
  managerContacterID: number;
  managerCompanyID: number;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterTitle: string | null;
  managerContacterTelephone: string | null;
  atomManagerContacterStatus: DbAtomManagerContacterStatusEnum | null;
  managerContacterIsAgreeSurvey: boolean | null;
  atomManagerContacterRatingKind: number | null;
  managerContacterEmployeeID: number | null;
  managerContacterRemark: string | null;
}

//#endregion

//#region 頁面物件
/** 目錄:客戶 */
const companyMenu = DbAtomMenuEnum.SystemCompany;
/** 目錄:窗口 */
const contacterMenu = DbAtomMenuEnum.SystemContacter;
/** 活動ID */
const managerActivityID = Number(route.params.activityId);

/** Crm-活動名單-新增-頁面物件 */
const crmActivityEmployeePipelineAddViewObj = reactive<CrmActivityEmployeePipelineAddViewMdl>({
  managerActivityID: null,
  managerCompanyID: 0,
  managerCompanyLocationID: null,
  managerCompanyLocationName: null,
  managerContacterID: null,
  atomPipelineStatus: DbAtomPipelineStatusEnum.Hyphen,
});

/** Crm-活動名單-新增-驗證物件 */
const crmActivityEmployeePipelineAddValidObj = reactive<CrmActivityEmployeePipelineAddValidMdl>({
  managerCompanyID: true,
  managerCompanyLocationID: true,
  managerContacterID: true,
});

/** 系統-公司-檢視-頁面物件 */
const sysCompanyDetailViewObj = reactive<SysCompanyDetailViewMdl>({
  managerCompanyUnifiedNumber: null,
  managerCompanyName: null,
  atomManagerCompanyStatus: null,
  managerCompanyMainKindName: null,
  managerCompanySubKindName: null,
  atomCustomerGrade: null,
  atomSecurityGrade: null,
  atomEmployeeRange: null,
  managerCompanyLocationID: null,
  managerCompanyLocationName: null,
  atomCity: null,
  managerCompanyLocationAddress: null,
  managerCompanyLocationTelephone: null,
  managerCompanyLocationStatus: null,
});

/** 系統-窗口-檢視-頁面物件 */
const sysContacterDetailViewObj = reactive<SysContacterDetailViewMdl>({
  managerContacterID: 0,
  managerCompanyID: 0,
  managerContacterName: null,
  managerContacterEmail: null,
  managerContacterPhone: null,
  managerContacterDepartment: null,
  managerContacterTitle: null,
  managerContacterTelephone: null,
  atomManagerContacterStatus: null,
  managerContacterIsAgreeSurvey: null,
  atomManagerContacterRatingKind: null,
  managerContacterEmployeeID: null,
  managerContacterRemark: null,
});
//#endregion

//#region 驗證相關
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;
  // 公司
  if (!crmActivityEmployeePipelineAddViewObj.managerCompanyID) {
    crmActivityEmployeePipelineAddValidObj.managerCompanyID = false;
    isValid = false;
  } else {
    crmActivityEmployeePipelineAddValidObj.managerCompanyID = true;
  }
  // 營業地點
  if (!crmActivityEmployeePipelineAddViewObj.managerCompanyLocationID) {
    crmActivityEmployeePipelineAddValidObj.managerCompanyLocationID = false;
    isValid = false;
  } else {
    crmActivityEmployeePipelineAddValidObj.managerCompanyLocationID = true;
  }
  // 窗口
  if (!crmActivityEmployeePipelineAddViewObj.managerContacterID) {
    crmActivityEmployeePipelineAddValidObj.managerContacterID = false;
    isValid = false;
  } else {
    crmActivityEmployeePipelineAddValidObj.managerContacterID = true;
  }
  return isValid;
};
//#endregion

//#region API / 資料流程
/** 取得公司資料 */
const getCompanyData = async (selectedID: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsBscHttpGetManagerCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: selectedID,
  };
  const responseData = await GetBasicCompany(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定公司詳細資料
  sysCompanyDetailViewObj.managerCompanyUnifiedNumber = responseData.managerCompanyUnifiedNumber;
  sysCompanyDetailViewObj.managerCompanyName = responseData.managerCompanyName;
  sysCompanyDetailViewObj.atomManagerCompanyStatus = responseData.atomManagerCompanyStatus;
  sysCompanyDetailViewObj.managerCompanyMainKindName = responseData.managerCompanyMainKindName;
  sysCompanyDetailViewObj.managerCompanySubKindName = responseData.managerCompanySubKindName;
  sysCompanyDetailViewObj.atomCustomerGrade = responseData.atomCustomerGrade;
  sysCompanyDetailViewObj.atomSecurityGrade = responseData.atomSecurityGrade;
  sysCompanyDetailViewObj.atomEmployeeRange = responseData.atomEmployeeRange;
};

/** 取得公司營業地點資料 */
const getCompanyLocationData = async (selectedID: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsBscHttpGetManagerCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyLocationID: selectedID,
    managerCompanyLocationIsDeleted: false,
  };
  const responseData = await getBasicManagerCompanyLocation(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定公司營業地點詳細資料
  sysCompanyDetailViewObj.managerCompanyLocationID = responseData.managerCompanyLocationID;
  sysCompanyDetailViewObj.managerCompanyLocationName = responseData.managerCompanyLocationName;
  sysCompanyDetailViewObj.atomCity = responseData.atomCity;
  sysCompanyDetailViewObj.managerCompanyLocationAddress =
    responseData.managerCompanyLocationAddress;
  sysCompanyDetailViewObj.managerCompanyLocationTelephone =
    responseData.managerCompanyLocationTelephone;
  sysCompanyDetailViewObj.managerCompanyLocationStatus = responseData.managerCompanyLocationStatus;
};

/** 取得公司窗口資料 */
const getCompanyContacterData = async (selectedID: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsSysCttHttpGetManagerContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID: selectedID,
  };
  const responseData = await getBasicManagerContacter(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定公司窗口詳細資料
  sysContacterDetailViewObj.managerContacterID = responseData.managerContacterID;
  sysContacterDetailViewObj.managerCompanyID = responseData.managerCompanyID;
  sysContacterDetailViewObj.managerContacterName = responseData.managerContacterName;
  sysContacterDetailViewObj.managerContacterEmail = responseData.managerContacterEmail;
  sysContacterDetailViewObj.managerContacterPhone = responseData.managerContacterPhone;
  sysContacterDetailViewObj.managerContacterDepartment = responseData.managerContacterDepartment;
  sysContacterDetailViewObj.managerContacterTitle = responseData.managerContacterJobTitle;
  sysContacterDetailViewObj.managerContacterTelephone = responseData.managerContacterTelephone;
  sysContacterDetailViewObj.atomManagerContacterStatus = responseData.atomManagerContacterStatus;
  sysContacterDetailViewObj.managerContacterIsAgreeSurvey =
    responseData.managerContacterIsAgreeSurvey;
  sysContacterDetailViewObj.atomManagerContacterRatingKind =
    responseData.atomManagerContacterRatingKind;
  sysContacterDetailViewObj.managerContacterEmployeeID = responseData.managerContacterEmployeeID;
  sysContacterDetailViewObj.managerContacterRemark = responseData.managerContacterRemark;
};
//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push(`/crm/activity/activity/detail/${managerActivityID}/pipeline`);
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  router.push(`/crm/activity/activity/detail/${managerActivityID}/pipeline`);
};

/** 點擊【前往新增客戶】 */
const clicktoAddCustomer = () => {
  const url = router.resolve("/system/company/add");
  window.open(url.href, "_blank");
};

/** 點擊【前往新增窗口】 */
const clicktoAddContacter = () => {
  const url = router.resolve("/system/contacter/add");
  window.open(url.href, "_blank");
};

/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateField()) return;

  // 呼叫 api
  const requestData: MbsCrmActHttpAddActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: managerActivityID,
    managerCompanyID: crmActivityEmployeePipelineAddViewObj.managerCompanyID!,
    managerCompanyLocationID: crmActivityEmployeePipelineAddViewObj.managerCompanyLocationID!,
    managerContacterID: crmActivityEmployeePipelineAddViewObj.managerContacterID!,
    atomPipelineStatus: DbAtomPipelineStatusEnum.Hyphen,
  };
  const responseData: MbsCrmActHttpAddActivityEmployeePipelineRspMdl | null =
    await addActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增名單成功！");

  // 導向詳細頁
  setTimeout(() => {
    router.push(
      `/crm/activity/activity/detail/${managerActivityID}/pipeline/detail/${
        responseData.employeePipelineID
      }`
    );
  }, 1500);
};

//#endregion

//#region 監聽器
// 監聽公司變更，取得公司詳細資料
watch(
  () => crmActivityEmployeePipelineAddViewObj.managerCompanyID,
  (newVal) => {
    if (newVal !== null) {
      getCompanyData(newVal);
    }
  }
);

// 監聽營業地點變更，取得營業地點詳細資料
watch(
  () => crmActivityEmployeePipelineAddViewObj.managerCompanyLocationID,
  (newVal) => {
    if (newVal !== null) {
      getCompanyLocationData(newVal);
    }
  }
);

// 監聽窗口變更，取得窗口詳細資料
watch(
  () => crmActivityEmployeePipelineAddViewObj.managerContacterID,
  (newVal) => {
    if (newVal !== null) {
      getCompanyContacterData(newVal);
    }
  }
);
//#endregion

</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <div class="flex items-center gap-4">
      <!-- 返回按鈕 -->
      <button class="btn-back flex items-center" @click="clickBackBtn">
        <svg
          class="w-4 h-4 inline-block mr-1"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M15 19l-7-7 7-7"
          />
        </svg>
        <span>返回</span>
      </button>
    </div>

    <!-- 基本資訊 區塊-->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="text-xl font-bold text-brand-100">基本資訊</div>
      <!-- 狀態 -->
      <div class="mb-4 flex flex-col gap-2">
        <label class="form-label">狀態 <span class="required-label">*</span></label>
        <select
          v-model="crmActivityEmployeePipelineAddViewObj.atomPipelineStatus"
          class="select-box"
          disabled
        >
          <option :value="DbAtomPipelineStatusEnum.Hyphen">-</option>
        </select>
      </div>
    </div>

    <!-- 客戶 區塊 -->
    <div class="flex flex-col gap-4 p-8 bg-white rounded-lg">
      <div class="flex flex-row justify-between">
        <div class="text-xl font-bold text-brand-100">客戶</div>
        <button
          v-if="employeeInfoStore.hasEveryonePermission(companyMenu, 'create')"
          class="btn-add"
          @click="clicktoAddCustomer()"
        >
          新增客戶
        </button>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 客戶 -->
        <div class="flex-1">
          <label class="form-label">客戶 <span class="required-label">*</span></label>
          <GetManyManagerCompanyComboBox
            v-model:managerCompanyID="crmActivityEmployeePipelineAddViewObj.managerCompanyID"
            :disabled="false"
          />
          <div class="error-wrapper">
            <p v-if="!crmActivityEmployeePipelineAddValidObj.managerCompanyID" class="error-tip">
              不可為空
            </p>
          </div>
        </div>

        <!-- 營業地點 -->
        <div class="flex-1">
          <label class="form-label">營業地點 <span class="required-label">*</span></label>
          <GetManyManagerCompanyLocationComboBox
            v-model:managerCompanyID="crmActivityEmployeePipelineAddViewObj.managerCompanyID"
            v-model:managerCompanyLocationID="
              crmActivityEmployeePipelineAddViewObj.managerCompanyLocationID
            "
            v-model:managerCompanyLocationName="
              crmActivityEmployeePipelineAddViewObj.managerCompanyLocationName
            "
            :disabled="!crmActivityEmployeePipelineAddViewObj.managerCompanyID"
          />
          <div class="error-wrapper">
            <p
              v-if="!crmActivityEmployeePipelineAddValidObj.managerCompanyLocationID"
              class="error-tip"
            >
              不可為空
            </p>
          </div>
        </div>
      </div>

      <!-- 客戶詳細資訊 區塊 -->
      <div class="p-8 bg-gray-100 rounded-md text-sm text-gray-700">
        <div class="grid grid-cols-2 gap-x-6 gap-y-2">
          <!-- 統編 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">統編</span>
              <span class="display-value truncate">
                {{ sysCompanyDetailViewObj.managerCompanyUnifiedNumber || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 客戶全名 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶全名</span>
              <span class="display-value truncate">
                {{ sysCompanyDetailViewObj.managerCompanyName || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 人員規模 -->
          <!-- <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">人員規模</span>
              <span class="display-value truncate">
                {{ getEmployeeRangeLabel(sysCompanyDetailViewObj.atomEmployeeRange) || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div> -->

          <!-- 客戶分級 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶分級</span>
              <span class="display-value truncate">
                {{ getCustomerGradeLabel(sysCompanyDetailViewObj.atomCustomerGrade) || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 客戶主分類 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶主分類</span>
              <span class="display-value truncate">
                {{ sysCompanyDetailViewObj.managerCompanyMainKindName || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 客戶子分類 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">客戶子分類</span>
              <span class="display-value truncate">
                {{ sysCompanyDetailViewObj.managerCompanySubKindName || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 縣市 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">縣市</span>
              <span class="display-value truncate">
                {{ getCityLabel(sysCompanyDetailViewObj.atomCity) || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 地址 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">地址</span>
              <span class="display-value truncate">
                {{ sysCompanyDetailViewObj.managerCompanyLocationAddress || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 電話 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電話</span>
              <span class="display-value truncate">
                {{ sysCompanyDetailViewObj.managerCompanyLocationTelephone || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 狀態 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">狀態</span>
              <span class="display-value truncate">
                {{
                  getManagerCompanyStatusLabel(
                    sysCompanyDetailViewObj.managerCompanyLocationStatus
                  ) || "-"
                }}
              </span>
            </p>
            <hr class="my-2" />
          </div>
        </div>
      </div>
    </div>

    <!-- 窗口 區塊 -->
    <div class="flex flex-col gap-4 p-8 bg-white rounded-lg">
      <div class="flex flex-row justify-between">
        <div class="text-xl font-bold text-brand-100">窗口</div>
        <button
          v-if="employeeInfoStore.hasEveryonePermission(contacterMenu, 'create')"
          class="btn-add"
          @click="clicktoAddContacter()"
        >
          新增窗口
        </button>
      </div>

      <!-- 窗口 -->
      <div>
        <label class="form-label">姓名/電子信箱 <span class="required-label">*</span></label>
        <GetManyContacterComboBox
          v-model:managerContacterID="crmActivityEmployeePipelineAddViewObj.managerContacterID"
          :managerContacterCompanyID="crmActivityEmployeePipelineAddViewObj.managerCompanyID"
          :disabled="!crmActivityEmployeePipelineAddViewObj.managerCompanyID"
        />
        <div class="error-wrapper">
          <p v-if="!crmActivityEmployeePipelineAddValidObj.managerContacterID" class="error-tip">
            不可為空
          </p>
        </div>
      </div>

      <!-- 窗口詳細資訊 區塊 -->
      <div class="p-8 bg-gray-100 rounded-md text-sm text-gray-700">
        <div class="grid grid-cols-2 gap-x-6 gap-y-2">
          <!-- 姓名 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">姓名</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterName || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 電子信箱 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電子信箱</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterEmail || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 手機 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">手機</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterTelephone || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 部門 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">部門</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterDepartment || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 職稱 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">職稱</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterTitle || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 電話#分機 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電話#分機</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterTelephone || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 狀態 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">狀態</span>
              <span class="display-value truncate">
                {{
                  getManagerContacterStatusLabel(
                    sysContacterDetailViewObj.atomManagerContacterStatus
                  ) || "-"
                }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 個資同意 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">個資同意</span>
              <span
                v-if="sysContacterDetailViewObj.managerContacterIsAgreeSurvey !== null"
                class="display-value truncate"
              >
                {{ sysContacterDetailViewObj.managerContacterIsAgreeSurvey ? "同意" : "不同意" }}
              </span>
              <span v-else class="display-value truncate">-</span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 開發評等 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">開發評等</span>
              <span class="display-value truncate">
                {{
                  getManagerContacterRatingLabel(
                    sysContacterDetailViewObj.atomManagerContacterRatingKind
                  ) || "-"
                }}
              </span>
            </p>
            <hr class="my-2" />
          </div>

          <!-- 備註 -->
          <div>
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">備註</span>
              <span class="display-value truncate">
                {{ sysContacterDetailViewObj.managerContacterRemark || "-" }}
              </span>
            </p>
            <hr class="my-2" />
          </div>
        </div>
      </div>
    </div>

    <!-- 按鈕列 -->
    <div class="flex justify-center gap-2 mt-4">
      <button class="btn-cancel" @click="clickCancelBtn">取消</button>
      <button class="btn-submit" @click="clickSubmitBtn">送出</button>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
