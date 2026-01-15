<script setup lang="ts">
//#region 引入
import { reactive, onMounted, ref, defineAsyncComponent } from "vue";
import { useRouter, useRoute } from "vue-router";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import {
  getCompany,
  updateCompany,
  getManyCompanyLocation,
  getCompanyLocation,
  addCompanyLocation,
  updateCompanyLocation,
  getManyCompanyAffiliate,
  addCompanyAffiliate,
  updateCompanyAffiliate,
} from "@/services";
import type {
  MbsSysComHttpGetCompanyReqMdl,
  MbsSysComHttpGetCompanyRspMdl,
  MbsSysComHttpUpdateCompanyReqMdl,
  MbsSysComHttpUpdateCompanyRspMdl,
  MbsSysComHttpGetManyCompanyLocationReqMdl,
  MbsSysComHttpGetManyCompanyLocationRspMdl,
  MbsSysComHttpGetManyCompanyLocationRspItemMdl,
  MbsSysComHttpGetCompanyLocationReqMdl,
  MbsSysComHttpGetCompanyLocationRspMdl,
  MbsSysComHttpAddCompanyLocationReqMdl,
  MbsSysComHttpUpdateCompanyLocationReqMdl,
  MbsSysComHttpUpdateCompanyLocationRspMdl,
  MbsSysComHttpGetManyCompanyAffiliateReqMdl,
  MbsSysComHttpGetManyCompanyAffiliateRspMdl,
  MbsSysComHttpGetManyCompanyAffiliateRspItemMdl,
  MbsSysComHttpAddCompanyAffiliateReqMdl,
  MbsSysComHttpUpdateCompanyAffiliateReqMdl,
  MbsSysComHttpUpdateCompanyAffiliateRspMdl,
  MbsSysComHttpAddCompanyAffiliateRspMdl,
  MbsSysComHttpAddCompanyLocationRspMdl,
} from "@/services/pms-http/system/company/systemCompanyHttpFormat";
import { getManagerCompanyStatusLabel } from "@/utils/getManagerCompanyStatusLabel";
import { getCityLabel } from "@/utils/getCityLabel";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyCityComboBox from "@/components/feature/search-bar/GetManyCityComboBox.vue";
const GetManyManagerCompanyMainKindComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyMainKindComboBox.vue")
);
const GetManyManagerCompanySubKindComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanySubKindComboBox.vue")
);
const ConfirmDialog = defineAsyncComponent(
  () => import("@/components/global/feedback/ConfirmDialog.vue")
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
/** 路由參數物件（用於取得路由參數） */
const route = useRoute();
//#endregion

//#region 型別定義
/** 系統設定-客戶-檢視-頁面模型 */
interface SysCompanyDetailViewMdl {
  managerCompanyUnifiedNumber: string | null;
  managerCompanyName: string | null;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindID: number;
  managerCompanySubKindName: string | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  atomSecurityGrade: DbAtomSecurityGradeEnum;
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
}

/** 系統設定-客戶-檢視-驗證模型 */
interface SysCompanyDetailValidMdl {
  managerCompanyUnifiedNumber: boolean;
  managerCompanyName: boolean;
  managerCompanyMainKindID: boolean;
  managerCompanySubKindID: boolean;
  atomManagerCompanyStatus: boolean;
}

/** 系統設定-客戶-檢視-原始模型 */
interface SysComDetailOriginalMdl {
  managerCompanyUnifiedNumber: string | null;
  managerCompanyName: string | null;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
  managerCompanyMainKindID: number;
  managerCompanySubKindID: number;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  atomSecurityGrade: DbAtomSecurityGradeEnum | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
}
//--------------------------------------------
/** 系統設定-客戶-營業地點-列表-項目模型 */
interface SysComLocationListItemMdl {
  managerCompanyLocationID: number;
  managerCompanyLocationName: string;
  atomCity: DbAtomCityEnum;
  managerCompanyLocationAddress: string;
  managerCompanyLocationTelephone: string;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
  managerCompanyLocationIsDeleted?: boolean;
}

/** 系統設定-客戶-營業地點-列表-項目原始模型 */
interface SysComLocationListItemOriginalMdl {
  managerCompanyLocationName: string | null;
  atomCity: number;
  managerCompanyLocationAddress: string;
  managerCompanyLocationTelephone: string;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum;
}

/** 系統設定-客戶-營業地點-驗證模型 */
interface SysComLocationValidMdl {
  managerCompanyLocationName: boolean;
  managerCompanyLocationTelephone: boolean;
  atomCity: boolean;
  managerCompanyLocationAddress: boolean;
  atomManagerCompanyStatus: boolean;
}
//--------------------------------------------
/** 系統設定-客戶-關係客戶-列表-項目模型 */
interface SysComAffiliateListItemMdl {
  managerCompanyAffiliateID: number;
  managerCompanyAffiliateUnifiedNumber: string;
  managerCompanyAffiliateName: string;
}

/** 系統設定-客戶-關係客戶-列表-項目原始模型 */
interface SysComAffiliateListItemOriginalMdl {
  managerCompanyAffiliateID: number;
  managerCompanyAffiliateUnifiedNumber: string;
  managerCompanyAffiliateName: string | null;
}

/** 系統設定-客戶-公司關係-驗證模型 */
interface SysComAffiliateValidMdl {
  managerCompanyAffiliateUnifiedNumber: boolean;
  managerCompanyAffiliateName: boolean;
}
//--------------------------------------------

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemCompany;
/** 公司ID(從路由參數取得) */
const managerCompanyID = Number(route.params.id);
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 營業地點列表 */
const locationList = ref<SysComLocationListItemMdl[]>([]);
/** 營業地點列表原始資料 */
const locationListOriginal = ref<SysComLocationListItemMdl[]>([]);
/** 是否顯示【新增營業地點】Modal */
const sysComLocationModalAddShow = ref(false);
/** 是否顯示【編輯營業地點】Modal */
const sysComLocationModalUpdateShow = ref(false);
/** 是否顯示刪除確認視窗 */
const showDeleteLocationConfirm = ref(false);
/** 關係客戶列表 */
const affiliateList = ref<SysComAffiliateListItemMdl[]>([]);
/** 關係客戶原始列表 */
const affiliateListOriginal = ref<SysComAffiliateListItemMdl[]>([]);
/** 是否顯示【新增關係客戶】Modal */
const sysComAffiliateModalAddShow = ref(false);
/** 是否顯示【編輯關係客戶】Modal */
const sysComAffiliateModalUpdateShow = ref(false);
/** 刪除的營業地點 ID */
const deletingManagerCompanyLocationID = ref<number | null>(null);
/** 是否顯示刪除確認視窗 */
const showDeleteAffiliateConfirm = ref(false);
/** 刪除的關係客戶 ID */
const deletingManagerCompanyAffiliateID = ref<number | null>(null);

/** 系統設定-客戶-檢視-頁面物件 */
const sysCompanyDetailViewObj = reactive<SysCompanyDetailViewMdl>({
  managerCompanyUnifiedNumber: "",
  managerCompanyName: "",
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum.NotSelected,
  managerCompanyMainKindID: 0,
  managerCompanyMainKindName: null,
  managerCompanySubKindID: 0,
  managerCompanySubKindName: null,
  atomCustomerGrade: DbAtomCustomerGradeEnum.NotSelected,
  atomSecurityGrade: DbAtomSecurityGradeEnum.NotSelected,
  atomEmployeeRange: DbAtomEmployeeRangeEnum.NotSelected,
});
/** 系統設定-客戶-檢視-驗證物件 */
const sysCompanyDetailValidObj = reactive<SysCompanyDetailValidMdl>({
  managerCompanyUnifiedNumber: true,
  managerCompanyName: true,
  managerCompanyMainKindID: true,
  managerCompanySubKindID: true,
  atomManagerCompanyStatus: true,
});

/** 系統設定-客戶-檢視-原始模型 */
const sysComDetailOriginalObj = reactive<SysComDetailOriginalMdl>({
  managerCompanyUnifiedNumber: "",
  managerCompanyName: "",
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum.NotSelected,
  managerCompanyMainKindID: 0,
  managerCompanySubKindID: 0,
  atomCustomerGrade: DbAtomCustomerGradeEnum.NotSelected,
  atomSecurityGrade: DbAtomSecurityGradeEnum.NotSelected,
  atomEmployeeRange: DbAtomEmployeeRangeEnum.NotSelected,
});
//--------------------------------------------
/** 系統設定-客戶-營業地點-列表-項目物件 */
const sysComLocationListItemObj = reactive<SysComLocationListItemMdl>({
  managerCompanyLocationID: 0,
  managerCompanyLocationName: "",
  atomCity: DbAtomCityEnum.NotSelected,
  managerCompanyLocationAddress: "",
  managerCompanyLocationTelephone: "",
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum.NotSelected,
  managerCompanyLocationIsDeleted: false,
});

/** 系統設定-客戶-營業地點-列表-項目原始模型 */
const sysComLocationListItemOriginalObj = reactive<SysComLocationListItemOriginalMdl>({
  managerCompanyLocationName: "",
  atomCity: 0,
  managerCompanyLocationAddress: "",
  managerCompanyLocationTelephone: "",
  atomManagerCompanyStatus: 0,
});

/** 系統設定-客戶-營業地點-驗證物件 */
const sysComLocationValidObj = reactive<SysComLocationValidMdl>({
  managerCompanyLocationName: true,
  atomCity: true,
  managerCompanyLocationAddress: true,
  managerCompanyLocationTelephone: true,
  atomManagerCompanyStatus: true,
});
//--------------------------------------------
/** 系統設定-客戶-關係客戶-列表-項目模型 */
const sysComAffiliateListItemObj = reactive<SysComAffiliateListItemMdl>({
  managerCompanyAffiliateID: 0,
  managerCompanyAffiliateUnifiedNumber: "",
  managerCompanyAffiliateName: "",
});

/** 系統設定-客戶-關係客戶-列表-項目原始模型 */
const sysComAffiliateListItemOriginalObj = reactive<SysComAffiliateListItemOriginalMdl>({
  managerCompanyAffiliateID: 0,
  managerCompanyAffiliateUnifiedNumber: "",
  managerCompanyAffiliateName: null,
});

/** 系統設定-客戶-公司關係-驗證物件 */
const sysComAffiliateValidObj = reactive<SysComAffiliateValidMdl>({
  managerCompanyAffiliateUnifiedNumber: true,
  managerCompanyAffiliateName: true,
});

//#endregion

//#region 驗證相關
/** 驗證台灣統一編號 */
const validateTaiwanUnifiedNumber = (unifiedNumber: string): boolean => {
  // 格式檢查：必須是8位數字
  if (!/^\d{8}$/.test(unifiedNumber)) {
    return false;
  }

  const multipliers = [1, 2, 1, 2, 1, 2, 4, 1];
  const digits: number[] = [];

  for (let i = 0; i < 8; i++) {
    digits[i] = parseInt(unifiedNumber[i]);
  }

  // 計算乘積並拆分為個位數相加
  let digitSum = 0;
  for (let i = 0; i < 8; i++) {
    let product = digits[i] * multipliers[i];
    // 將乘積的十位和個位分開相加
    while (product > 0) {
      digitSum += product % 10;
      product = Math.floor(product / 10);
    }
  }

  // 第7位（索引6，倒數第二位）不是7的情況
  if (digits[6] !== 7) {
    return digitSum % 5 === 0;
  }

  // 第7位是7的情況：需要計算Z1和Z2
  // 計算前7位的和
  let sumExceptLast = 0;
  for (let i = 0; i < 7; i++) {
    let product = digits[i] * multipliers[i];
    while (product > 0) {
      sumExceptLast += product % 10;
      product = Math.floor(product / 10);
    }
  }

  // 最後一位的乘積（digits[7] * 1 = digits[7]）
  const lastProduct = digits[7] * multipliers[7];
  const lastTens = Math.floor(lastProduct / 10);

  // Z1: 最後一位個位數取1
  const z1 = sumExceptLast + lastTens + 1;

  // Z2: 最後一位個位數取0
  const z2 = sumExceptLast + lastTens + 0;

  return z1 % 5 === 0 || z2 % 5 === 0;
};

/** 驗證【公司】欄位 */
const validateCompanyField = () => {
  let isValid = true;

  // 統一編號
  const unifiedNumber = sysCompanyDetailViewObj.managerCompanyUnifiedNumber?.trim() || "";
  if (
    typeof sysCompanyDetailViewObj.managerCompanyUnifiedNumber !== "string" ||
    !unifiedNumber ||
    !validateTaiwanUnifiedNumber(unifiedNumber)
  ) {
    sysCompanyDetailValidObj.managerCompanyUnifiedNumber = false;
    isValid = false;
  } else {
    sysCompanyDetailValidObj.managerCompanyUnifiedNumber = true;
  }

  // 公司名稱
  if (
    typeof sysCompanyDetailViewObj.managerCompanyName !== "string" ||
    !sysCompanyDetailViewObj.managerCompanyName.trim() ||
    sysCompanyDetailViewObj.managerCompanyName.trim().length > 300
  ) {
    sysCompanyDetailValidObj.managerCompanyName = false;
    isValid = false;
  } else {
    sysCompanyDetailValidObj.managerCompanyName = true;
  }

  // 主分類
  if (
    typeof sysCompanyDetailViewObj.managerCompanyMainKindID !== "number" ||
    sysCompanyDetailViewObj.managerCompanyMainKindID <= 0
  ) {
    sysCompanyDetailValidObj.managerCompanyMainKindID = false;
    isValid = false;
  } else {
    sysCompanyDetailValidObj.managerCompanyMainKindID = true;
  }

  // 子分類
  if (
    typeof sysCompanyDetailViewObj.managerCompanySubKindID !== "number" ||
    sysCompanyDetailViewObj.managerCompanySubKindID <= 0
  ) {
    sysCompanyDetailValidObj.managerCompanySubKindID = false;
    isValid = false;
  } else {
    sysCompanyDetailValidObj.managerCompanySubKindID = true;
  }

  // 狀態
  if (
    sysCompanyDetailViewObj.atomManagerCompanyStatus === null ||
    sysCompanyDetailViewObj.atomManagerCompanyStatus === undefined ||
    sysCompanyDetailViewObj.atomManagerCompanyStatus === DbAtomManagerCompanyStatusEnum.NotSelected
  ) {
    sysCompanyDetailValidObj.atomManagerCompanyStatus = false;
    isValid = false;
  } else {
    sysCompanyDetailValidObj.atomManagerCompanyStatus = true;
  }

  return isValid;
};

/** 重設【公司】驗證狀態 */
const resetCompanyValidation = () => {
  Object.assign(sysCompanyDetailValidObj, {
    managerCompanyUnifiedNumber: true,
    managerCompanyName: true,
    managerCompanyMainKindID: true,
    managerCompanySubKindID: true,
    atomManagerCompanyStatus: true,
  });
};

/** 驗證【營業地點】欄位 */
const validateLocationField = () => {
  let isValid = true;

  // 名稱：不可空、長度 ≤ 100
  if (
    typeof sysComLocationListItemObj.managerCompanyLocationName !== "string" ||
    !sysComLocationListItemObj.managerCompanyLocationName.trim() ||
    sysComLocationListItemObj.managerCompanyLocationName.trim().length > 100
  ) {
    sysComLocationValidObj.managerCompanyLocationName = false;
    isValid = false;
  } else {
    sysComLocationValidObj.managerCompanyLocationName = true;
  }

  // 電話：可空，但若有輸入要符合格式 (02-123-4567 或 02-1234-5678)
  const tel = sysComLocationListItemObj.managerCompanyLocationTelephone?.trim();
  const phonePattern = /^(0\d{1,2}-\d{3,4}-\d{3,4})$/;
  if (tel && !phonePattern.test(tel)) {
    sysComLocationValidObj.managerCompanyLocationTelephone = false;
    isValid = false;
  } else {
    sysComLocationValidObj.managerCompanyLocationTelephone = true;
  }

  // 縣市：不可空
  if (
    sysComLocationListItemObj.atomCity === null ||
    sysComLocationListItemObj.atomCity === undefined ||
    sysComLocationListItemObj.atomCity === 0 ||
    sysComLocationListItemObj.atomCity === DbAtomCityEnum.NotSelected
  ) {
    sysComLocationValidObj.atomCity = false;
    isValid = false;
  } else {
    sysComLocationValidObj.atomCity = true;
  }

  // 地址：不可空、長度 ≤ 100
  if (
    typeof sysComLocationListItemObj.managerCompanyLocationAddress !== "string" ||
    !sysComLocationListItemObj.managerCompanyLocationAddress.trim() ||
    sysComLocationListItemObj.managerCompanyLocationAddress.trim().length > 100
  ) {
    sysComLocationValidObj.managerCompanyLocationAddress = false;
    isValid = false;
  } else {
    sysComLocationValidObj.managerCompanyLocationAddress = true;
  }

  // 狀態：可空（這裡預設通過）
  sysComLocationValidObj.atomManagerCompanyStatus = true;

  return isValid;
};

/** 重設【營業地點】驗證狀態 */
const resetLocationValidation = () => {
  Object.assign(sysComLocationValidObj, {
    managerCompanyLocationName: true,
    managerCompanyLocationTelephone: true,
    atomCity: true,
    managerCompanyLocationAddress: true,
    atomManagerCompanyStatus: true,
  });
};

/** 驗證【關係客戶】欄位 */
const validateAffiliateField = () => {
  let isValid = true;

  // 統編：不可為空，長度 ≤ 8
  const affiliateUnifiedNumber =
    sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber?.trim() || "";
  if (
    typeof sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber !== "string" ||
    !affiliateUnifiedNumber ||
    !validateTaiwanUnifiedNumber(affiliateUnifiedNumber)
  ) {
    sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber = false;
    isValid = false;
  } else {
    sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber = true;
  }

  // 名稱：不可為空，長度 ≤ 300
  if (
    typeof sysComAffiliateListItemObj.managerCompanyAffiliateName !== "string" ||
    !sysComAffiliateListItemObj.managerCompanyAffiliateName.trim() ||
    sysComAffiliateListItemObj.managerCompanyAffiliateName.trim().length > 300
  ) {
    sysComAffiliateValidObj.managerCompanyAffiliateName = false;
    isValid = false;
  } else {
    sysComAffiliateValidObj.managerCompanyAffiliateName = true;
  }

  return isValid;
};

/** 重設【關係客戶】驗證狀態 */
const resetAffiliateValidation = () => {
  Object.assign(sysComAffiliateValidObj, {
    managerCompanyAffiliateUnifiedNumber: true,
    managerCompanyAffiliateName: true,
  });
};

//#endregion

//#region API / 資料流程
/** 取得【公司】資料 */
const getCompanyData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫API
  const requestData: MbsSysComHttpGetCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID,
  };
  const responseData: MbsSysComHttpGetCompanyRspMdl | null = await getCompany(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  sysCompanyDetailViewObj.managerCompanyUnifiedNumber = responseData.managerCompanyUnifiedNumber;
  sysCompanyDetailViewObj.managerCompanyName = responseData.managerCompanyName;
  sysCompanyDetailViewObj.atomManagerCompanyStatus = responseData.atomManagerCompanyStatus;
  sysCompanyDetailViewObj.managerCompanyMainKindID = responseData.managerCompanyMainKindID;
  sysCompanyDetailViewObj.managerCompanyMainKindName = responseData.managerCompanyMainKindName;
  sysCompanyDetailViewObj.managerCompanySubKindID = responseData.managerCompanySubKindID;
  sysCompanyDetailViewObj.managerCompanySubKindName = responseData.managerCompanySubKindName;
  sysCompanyDetailViewObj.atomCustomerGrade = responseData.atomCustomerGrade;
  sysCompanyDetailViewObj.atomSecurityGrade = responseData.atomSecurityGrade;
  sysCompanyDetailViewObj.atomEmployeeRange = responseData.atomEmployeeRange;

  // 保存原始值
  sysComDetailOriginalObj.managerCompanyUnifiedNumber = responseData.managerCompanyUnifiedNumber;
  sysComDetailOriginalObj.managerCompanyName = responseData.managerCompanyName;
  sysComDetailOriginalObj.atomManagerCompanyStatus = responseData.atomManagerCompanyStatus;
  sysComDetailOriginalObj.managerCompanyMainKindID = responseData.managerCompanyMainKindID;
  sysComDetailOriginalObj.managerCompanySubKindID = responseData.managerCompanySubKindID;
  sysComDetailOriginalObj.atomCustomerGrade = responseData.atomCustomerGrade;
  sysComDetailOriginalObj.atomSecurityGrade = responseData.atomSecurityGrade;
  sysComDetailOriginalObj.atomEmployeeRange = responseData.atomEmployeeRange;
};

/** 取得【營業地點】列表 */
const getLocationList = async () => {
  // 驗證 token
  if (!requireToken()) return;
  // 呼叫 API
  const requestData: MbsSysComHttpGetManyCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID,
  };
  const responseData: MbsSysComHttpGetManyCompanyLocationRspMdl | null =
    await getManyCompanyLocation(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  locationList.value = responseData.managerCompanyLocationList?.map(
    (item: MbsSysComHttpGetManyCompanyLocationRspItemMdl) => ({
      managerCompanyLocationID: item.managerCompanyLocationID,
      managerCompanyLocationName: item.managerCompanyLocationName,
      atomCity: item.atomCity,
      managerCompanyLocationAddress: item.managerCompanyLocationAddress,
      managerCompanyLocationTelephone: item.managerCompanyLocationTelephone,
      atomManagerCompanyStatus: item.atomManagerCompanyStatus,
    })
  );

  // 保存原始資料
  locationListOriginal.value = responseData.managerCompanyLocationList?.map(
    (item: MbsSysComHttpGetManyCompanyLocationRspItemMdl) => ({
      managerCompanyLocationID: item.managerCompanyLocationID,
      managerCompanyLocationName: item.managerCompanyLocationName,
      atomCity: item.atomCity,
      managerCompanyLocationAddress: item.managerCompanyLocationAddress,
      managerCompanyLocationTelephone: item.managerCompanyLocationTelephone,
      atomManagerCompanyStatus: item.atomManagerCompanyStatus,
    })
  );
};

/** 取得【關係客戶】列表 */
const getAffiliateList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsSysComHttpGetManyCompanyAffiliateReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID,
  };
  const responseData: MbsSysComHttpGetManyCompanyAffiliateRspMdl | null =
    await getManyCompanyAffiliate(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  // 設定資料
  affiliateList.value = responseData.managerCompanyAffiliateList?.map(
    (item: MbsSysComHttpGetManyCompanyAffiliateRspItemMdl) => ({
      managerCompanyAffiliateID: item.managerCompanyAffiliateID,
      managerCompanyAffiliateUnifiedNumber: item.managerCompanyAffiliateUnifiedNumber,
      managerCompanyAffiliateName: item.managerCompanyAffiliateName,
    })
  );

  // 保存原始資料
  affiliateListOriginal.value = responseData.managerCompanyAffiliateList?.map(
    (item: MbsSysComHttpGetManyCompanyAffiliateRspItemMdl) => ({
      managerCompanyAffiliateID: item.managerCompanyAffiliateID,
      managerCompanyAffiliateUnifiedNumber: item.managerCompanyAffiliateUnifiedNumber,
      managerCompanyAffiliateName: item.managerCompanyAffiliateName,
    })
  );
};

/** 取得【公司】變更欄位 */
const getChangedFields = (): Partial<MbsSysComHttpUpdateCompanyReqMdl> => {
  const changes: Partial<MbsSysComHttpUpdateCompanyReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: managerCompanyID,
  };

  if (
    sysCompanyDetailViewObj.managerCompanyUnifiedNumber !==
    sysComDetailOriginalObj.managerCompanyUnifiedNumber
  ) {
    changes.managerCompanyUnifiedNumber = sysCompanyDetailViewObj.managerCompanyUnifiedNumber;
  }

  if (sysCompanyDetailViewObj.managerCompanyName !== sysComDetailOriginalObj.managerCompanyName) {
    changes.managerCompanyName = sysCompanyDetailViewObj.managerCompanyName;
  }

  if (
    sysCompanyDetailViewObj.atomManagerCompanyStatus !==
    sysComDetailOriginalObj.atomManagerCompanyStatus
  ) {
    changes.atomManagerCompanyStatus = sysCompanyDetailViewObj.atomManagerCompanyStatus!;
  }

  if (
    sysCompanyDetailViewObj.managerCompanyMainKindID !==
    sysComDetailOriginalObj.managerCompanyMainKindID
  ) {
    changes.managerCompanyMainKindID = sysCompanyDetailViewObj.managerCompanyMainKindID;
  }

  if (
    sysCompanyDetailViewObj.managerCompanySubKindID !==
    sysComDetailOriginalObj.managerCompanySubKindID
  ) {
    changes.managerCompanySubKindID = sysCompanyDetailViewObj.managerCompanySubKindID;
  }

  if (sysCompanyDetailViewObj.atomCustomerGrade !== sysComDetailOriginalObj.atomCustomerGrade) {
    changes.atomCustomerGrade = sysCompanyDetailViewObj.atomCustomerGrade!;
  }

  if (sysCompanyDetailViewObj.atomSecurityGrade !== sysComDetailOriginalObj.atomSecurityGrade) {
    changes.atomSecurityGrade = sysCompanyDetailViewObj.atomSecurityGrade!;
  }

  if (sysCompanyDetailViewObj.atomEmployeeRange !== sysComDetailOriginalObj.atomEmployeeRange) {
    changes.atomEmployeeRange = sysCompanyDetailViewObj.atomEmployeeRange!;
  }
  return changes;
};

/** 處理刪除【營業地點】 */
const handleRemoveLocation = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: Partial<MbsSysComHttpUpdateCompanyLocationReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyLocationID: deletingManagerCompanyLocationID.value!,
    managerCompanyLocationIsDeleted: true,
  };
  const responseData: MbsSysComHttpUpdateCompanyLocationRspMdl | null = await updateCompanyLocation(
    requestData as MbsSysComHttpUpdateCompanyLocationReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除營業地點成功！");

  // 更新列表
  await getLocationList();

  // 關閉刪除確認視窗
  showDeleteLocationConfirm.value = false;
};

/** 取得【關係客戶】變更欄位 */
const getAffiliateChangedFields = (): Partial<MbsSysComHttpUpdateCompanyAffiliateReqMdl> => {
  const changes: Partial<MbsSysComHttpUpdateCompanyAffiliateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyAffiliateID: sysComAffiliateListItemObj.managerCompanyAffiliateID,
  };

  // 統編
  if (
    sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber !==
    sysComAffiliateListItemOriginalObj.managerCompanyAffiliateUnifiedNumber
  ) {
    changes.managerCompanyAffiliateUnifiedNumber =
      sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber;
  }

  // 名稱
  if (
    sysComAffiliateListItemObj.managerCompanyAffiliateName !==
    sysComAffiliateListItemOriginalObj.managerCompanyAffiliateName
  ) {
    changes.managerCompanyAffiliateName = sysComAffiliateListItemObj.managerCompanyAffiliateName;
  }

  return changes;
};

/** 處理【刪除關係客戶】 */
const handleRemoveAffiliate = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: Partial<MbsSysComHttpUpdateCompanyAffiliateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyAffiliateID: deletingManagerCompanyAffiliateID.value!,
    managerCompanyAffiliateIsDeleted: true,
  };
  const responseData: MbsSysComHttpUpdateCompanyAffiliateRspMdl | null =
    await updateCompanyAffiliate(requestData as MbsSysComHttpUpdateCompanyAffiliateReqMdl);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除關係客戶成功！");

  // 更新列表
  await getAffiliateList();

  // 關閉刪除確認視窗
  showDeleteAffiliateConfirm.value = false;
};

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/company");
};

/** 點擊【編輯(客戶資訊)】按鈕 */
const clickUpdateBtn = () => {
  isEditMode.value = true;
};

/** 點擊提交【公司】按鈕 */
const clickSubmitCompanyBtn = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateCompanyField()) return;

  // 取得變更欄位
  const changedFields = getChangedFields();
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 呼叫api
  const responseData: MbsSysComHttpUpdateCompanyRspMdl | null = await updateCompany(
    changedFields as MbsSysComHttpUpdateCompanyReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 編輯模式關閉
  isEditMode.value = false;

  // 顯示成功訊息
  displaySuccess("更新客戶資訊成功！");

  // 重新取得資料
  getCompanyData();
};

/** 點擊取消編輯【公司】按鈕 */
const clickCancelUpdateCompanyBtn = () => {
  // 關閉編輯模式
  isEditMode.value = false;

  // 重設資料驗證狀態
  resetCompanyValidation();

  // 重新取得資料
  getCompanyData();
};

/** 取得【營業地點】變更欄位 */
const getLocationChangedFields = (): Partial<MbsSysComHttpUpdateCompanyLocationReqMdl> => {
  const changes: Partial<MbsSysComHttpUpdateCompanyLocationReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyLocationID: sysComLocationListItemObj.managerCompanyLocationID,
  };
  //名稱
  if (
    sysComLocationListItemObj.managerCompanyLocationName !==
    sysComLocationListItemOriginalObj.managerCompanyLocationName
  ) {
    changes.managerCompanyLocationName = sysComLocationListItemObj.managerCompanyLocationName;
  }

  //電話
  if (
    sysComLocationListItemObj.managerCompanyLocationTelephone !==
    sysComLocationListItemOriginalObj.managerCompanyLocationTelephone
  ) {
    changes.managerCompanyLocationTelephone =
      sysComLocationListItemObj.managerCompanyLocationTelephone;
  }

  //縣市
  if (sysComLocationListItemObj.atomCity !== sysComLocationListItemOriginalObj.atomCity) {
    changes.atomCity = sysComLocationListItemObj.atomCity;
  }

  //地址
  if (
    sysComLocationListItemObj.managerCompanyLocationAddress !==
    sysComLocationListItemOriginalObj.managerCompanyLocationAddress
  ) {
    changes.managerCompanyLocationAddress = sysComLocationListItemObj.managerCompanyLocationAddress;
  }

  //狀態
  if (
    sysComLocationListItemObj.atomManagerCompanyStatus !==
    sysComLocationListItemOriginalObj.atomManagerCompanyStatus
  ) {
    changes.atomManagerCompanyStatus = sysComLocationListItemObj.atomManagerCompanyStatus;
  }
  return changes;
};

/** 點擊打開【新增營業地點】Modal 按鈕 */
const clickOpenAddLocationModalBtn = () => {
  resetLocationValidation();
  Object.assign(sysComLocationListItemObj, {
    managerCompanyLocationID: 0,
    managerCompanyLocationName: "",
    atomCity: DbAtomCityEnum.NotSelected,
    managerCompanyLocationAddress: "",
    managerCompanyLocationTelephone: "",
    atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum.NotSelected,
  });

  sysComLocationModalAddShow.value = true;
};

/** 點擊送出【新增營業地點】 */
const clickSubmitAddLocationBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateLocationField()) return;

  // 呼叫 API
  const requestData: MbsSysComHttpAddCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID,
    managerCompanyLocationName: sysComLocationListItemObj.managerCompanyLocationName,
    atomCity: sysComLocationListItemObj.atomCity,
    managerCompanyLocationAddress: sysComLocationListItemObj.managerCompanyLocationAddress,
    managerCompanyLocationTelephone: sysComLocationListItemObj.managerCompanyLocationTelephone,
    atomManagerCompanyStatus: sysComLocationListItemObj.atomManagerCompanyStatus,
  };
  const responseData: MbsSysComHttpAddCompanyLocationRspMdl | null =
    await addCompanyLocation(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增營業地點成功！");

  // 更新列表
  await getLocationList();

  // 關閉 Modal
  sysComLocationModalAddShow.value = false;
};

/** 點擊打開【編輯營業地點】Modal 按鈕 */
const clickOpenUpdateLocationModalBtn = async (managerCompanyLocationID: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 重設驗證狀態
  resetLocationValidation();

  // 呼叫 API
  const requestData: MbsSysComHttpGetCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyLocationID,
  };
  const responseData: MbsSysComHttpGetCompanyLocationRspMdl | null =
    await getCompanyLocation(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  sysComLocationListItemObj.managerCompanyLocationID = responseData.managerCompanyLocationID;
  sysComLocationListItemObj.managerCompanyLocationName = responseData.managerCompanyLocationName;
  sysComLocationListItemObj.atomCity = responseData.atomCity;
  sysComLocationListItemObj.managerCompanyLocationAddress =
    responseData.managerCompanyLocationAddress;
  sysComLocationListItemObj.managerCompanyLocationTelephone =
    responseData.managerCompanyLocationTelephone;
  sysComLocationListItemObj.atomManagerCompanyStatus =
    responseData.atomManagerCompanyStatus ?? null;

  sysComLocationModalUpdateShow.value = true;

  // 記錄該筆原始資料
  const original = locationListOriginal.value.find(
    (item) => item.managerCompanyLocationID === managerCompanyLocationID
  );
  if (original) {
    Object.assign(sysComLocationListItemOriginalObj, JSON.parse(JSON.stringify(original)));
  }
};

/** 點擊送出【編輯營業地點】 */
const clickSubmitUpdateLocationBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateLocationField()) return;

  // 取得變更欄位
  const changedFields = getLocationChangedFields();
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 呼叫 API
  const responseData: MbsSysComHttpUpdateCompanyLocationRspMdl | null = await updateCompanyLocation(
    changedFields as MbsSysComHttpUpdateCompanyLocationReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新營業地點成功！");

  // 更新列表
  await getLocationList();

  // 關閉 Modal
  sysComLocationModalUpdateShow.value = false;
};

/** 點擊【刪除營業地點】按鈕 */
const clickRemoveProjectStoneBtn = (managerCompanyLocationID: number) => {
  deletingManagerCompanyLocationID.value = managerCompanyLocationID;
  showDeleteLocationConfirm.value = true;
};

/** 點擊【取消刪除營業地點】按鈕 */
const cancelRemoveLocation = () => {
  showDeleteLocationConfirm.value = false;
  deletingManagerCompanyLocationID.value = null;
};

/** 點擊新增【關係客戶】按鈕 */
const clickAddAffiliateBtn = () => {
  resetAffiliateValidation();
  Object.assign(sysComAffiliateListItemObj, {
    managerCompanyAffiliateUnifiedNumber: "",
    managerCompanyAffiliateName: "",
  });
  sysComAffiliateModalAddShow.value = true;
};

/** 點擊送出【新增關係客戶】按鈕 */
const clickSubmitAddAffiliateBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateAffiliateField()) return;

  // 呼叫 API
  const requestData: MbsSysComHttpAddCompanyAffiliateReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID,
    managerCompanyAffiliateUnifiedNumber:
      sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber,
    managerCompanyAffiliateName: sysComAffiliateListItemObj.managerCompanyAffiliateName,
  };
  const responseData: MbsSysComHttpAddCompanyAffiliateRspMdl | null =
    await addCompanyAffiliate(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增關係客戶成功！");

  // 更新列表
  await getAffiliateList();

  // 關閉 Modal
  sysComAffiliateModalAddShow.value = false;
};

/** 點擊打開【編輯關係客戶】Modal 按鈕 */
const clickOpenUpdateAffiliateModalBtn = async (managerCompanyAffiliateID: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 重設驗證狀態
  resetAffiliateValidation();

  // 呼叫 API
  const requestData: MbsSysComHttpGetManyCompanyAffiliateReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: managerCompanyID,
  };
  const responseData: MbsSysComHttpGetManyCompanyAffiliateRspMdl | null =
    await getManyCompanyAffiliate(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  const selected = responseData.managerCompanyAffiliateList?.find(
    (item) => item.managerCompanyAffiliateID === managerCompanyAffiliateID
  );
  if (!selected) return;

  Object.assign(sysComAffiliateListItemObj, selected);

  // 記錄原始資料
  Object.assign(sysComAffiliateListItemOriginalObj, JSON.parse(JSON.stringify(selected)));

  // 開啟 Modal
  sysComAffiliateModalUpdateShow.value = true;
};

/** 點擊送出【編輯關係客戶】按鈕 */
const clickSubmitUpdateAffiliateBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateAffiliateField()) return;

  // 取得變更欄位
  const changedFields = getAffiliateChangedFields();
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 呼叫 API
  const responseData: MbsSysComHttpUpdateCompanyAffiliateRspMdl | null =
    await updateCompanyAffiliate(changedFields as MbsSysComHttpUpdateCompanyAffiliateReqMdl);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新關係客戶成功！");

  // 更新列表
  await getAffiliateList();

  // 關閉 Modal
  sysComAffiliateModalUpdateShow.value = false;

  // 重新取得資料
  getAffiliateList();

  // 關閉錯誤訊息
  closeError();
};

/** 點擊【刪除營業地點】按鈕 */
const clickRemoveAffiliateBtn = (managerCompanyLocationID: number) => {
  deletingManagerCompanyAffiliateID.value = managerCompanyLocationID;
  showDeleteAffiliateConfirm.value = true;
};

/** 點擊【取消刪除關係客戶】按鈕 */
const cancelRemoveAffiliate = () => {
  showDeleteAffiliateConfirm.value = false;
  deletingManagerCompanyAffiliateID.value = null;
};

//#endregion

//#region 生命週期
onMounted(() => {
  // 取得客戶資訊資料
  getCompanyData();
  // 取得營業地點列表
  getLocationList();
  // 取得關係客戶列表
  getAffiliateList();
});
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <div class="flex items-center justify-between">
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

    <!-- 客戶資訊區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between">
        <div class="subtitle">客戶資訊</div>
        <!--編輯模式-->
        <template v-if="isEditMode">
          <div class="flex gap-2">
            <button class="btn-cancel" @click="clickCancelUpdateCompanyBtn">取消編輯</button>
            <button class="btn-submit" @click="clickSubmitCompanyBtn">儲存</button>
          </div>
        </template>
        <!--檢視模式-->
        <template v-else>
          <button
            v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
            class="btn-update"
            @click="clickUpdateBtn"
          >
            編輯
          </button>
        </template>
      </div>

      <form class="grid grid-cols-2 gap-4">
        <div>
          <label class="form-label">統編 <span class="required-label">*</span></label>
          <input
            v-model="sysCompanyDetailViewObj.managerCompanyUnifiedNumber"
            class="input-box"
            :disabled="!isEditMode"
            placeholder="請輸入統編"
          />
          <p class="error-wrapper">
            <span v-if="!sysCompanyDetailValidObj.managerCompanyUnifiedNumber" class="error-tip">
              請輸入正確的統一編號，不可為空，長度不可超過 8 個字
            </span>
          </p>
        </div>

        <!-- 客戶全名 -->
        <div>
          <label class="form-label">客戶全名 <span class="required-label">*</span></label>
          <input
            v-model="sysCompanyDetailViewObj.managerCompanyName"
            class="input-box"
            :disabled="!isEditMode"
            placeholder="請輸入客戶全名"
          />
          <div class="h-2">
            <p v-if="!sysCompanyDetailValidObj.managerCompanyName" class="error-tip">
              不可為空，長度不可超過 300 個字
            </p>
          </div>
        </div>

        <div>
          <label class="form-label">客戶主分類 <span class="required-label">*</span></label>
          <div v-if="isEditMode">
            <GetManyManagerCompanyMainKindComboBox
              v-model:managerCompanyMainKindID="sysCompanyDetailViewObj.managerCompanyMainKindID"
              v-model:managerCompanyMainKindName="
                sysCompanyDetailViewObj.managerCompanyMainKindName
              "
              :disabled="!isEditMode"
            />
          </div>
          <div v-else>
            <input
              v-model="sysCompanyDetailViewObj.managerCompanyMainKindName"
              class="input-box"
              :disabled="!isEditMode"
              placeholder="請輸入客戶主分類"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!sysCompanyDetailValidObj.managerCompanyMainKindID" class="error-tip">
              不可為空
            </span>
          </p>
        </div>

        <div>
          <label class="form-label">客戶子分類 <span class="required-label">*</span></label>
          <div v-if="isEditMode">
            <GetManyManagerCompanySubKindComboBox
              v-model:managerCompanyMainKindID="sysCompanyDetailViewObj.managerCompanyMainKindID"
              v-model:managerCompanySubKindID="sysCompanyDetailViewObj.managerCompanySubKindID"
              v-model:managerCompanySubKindName="sysCompanyDetailViewObj.managerCompanySubKindName"
              :disabled="!isEditMode"
            />
          </div>
          <div v-else>
            <input
              v-model="sysCompanyDetailViewObj.managerCompanySubKindName"
              class="input-box"
              placeholder="請輸入客戶子分類"
              :disabled="!isEditMode"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!sysCompanyDetailValidObj.managerCompanySubKindID" class="error-tip">
              不可為空
            </span>
          </p>
        </div>

        <!-- 狀態 -->
        <div class="mb-4 flex flex-col gap-2">
          <label class="form-label"> 狀態 <span class="required-label">*</span> </label>
          <div class="select-wrapper">
            <select
              v-model="sysCompanyDetailViewObj.atomManagerCompanyStatus"
              class="select-box"
              :disabled="!isEditMode"
              placeholder="請選擇狀態"
            >
              <option :value="DbAtomManagerCompanyStatusEnum.NotSelected">請選擇狀態</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Operating">營運中</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Closed">停業</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Unclear">不清楚</option>
            </select>
          </div>
          <p class="error-wrapper">
            <span v-if="!sysCompanyDetailValidObj.atomManagerCompanyStatus" class="error-tip">
              不可為空
            </span>
          </p>
        </div>

        <!-- 客戶分級 -->
        <div class="mb-4 flex flex-col gap-2">
          <label class="form-label">客戶分級</label>
          <div class="select-wrapper">
            <select
              v-model="sysCompanyDetailViewObj.atomCustomerGrade"
              class="select-box"
              :disabled="!isEditMode"
              placeholder="請選擇客戶分級"
            >
              <option :value="DbAtomCustomerGradeEnum.NotSelected">請選擇客戶分級</option>
              <option :value="DbAtomCustomerGradeEnum.A">A</option>
              <option :value="DbAtomCustomerGradeEnum.B">B</option>
              <option :value="DbAtomCustomerGradeEnum.C">C</option>
            </select>
          </div>
        </div>

        <!-- 資安責任等級 -->
        <div class="mb-4 flex flex-col gap-2">
          <label class="form-label">資安責任等級</label>
          <div class="select-wrapper">
            <select
              v-model="sysCompanyDetailViewObj.atomSecurityGrade"
              class="select-box"
              :disabled="!isEditMode"
              placeholder="請選擇資安責任等級"
            >
              <option :value="DbAtomSecurityGradeEnum.NotSelected">請選擇資安責任等級</option>
              <option :value="DbAtomSecurityGradeEnum.A">A</option>
              <option :value="DbAtomSecurityGradeEnum.B">B</option>
              <option :value="DbAtomSecurityGradeEnum.C">C</option>
              <option :value="DbAtomSecurityGradeEnum.D">D</option>
            </select>
          </div>
        </div>
      </form>
    </div>

    <!-- 營業地點區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between items-center">
        <div class="subtitle">營業地點</div>
        <button
          v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-add"
          @click="clickOpenAddLocationModalBtn"
        >
          新增營業地點
        </button>
      </div>
      <div class="overflow-x-auto">
        <div class="flex-1 overflow-y-auto table-scrollable">
          <div v-if="locationList.length > 0">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-32">營業地點名稱</th>
                  <th class="text-start w-32">電話</th>
                  <th class="text-start w-32">縣市</th>
                  <th class="text-start w-32">地址</th>
                  <th class="text-center w-24">狀態</th>
                  <th
                    v-if="
                      (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')) ||
                      (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'delete'))
                    "
                    class="text-center w-40"
                  >
                    操作
                  </th>
                </tr>
              </thead>
              <tbody>
                <template v-if="locationList.length > 0">
                  <tr v-for="item in locationList" :key="item.managerCompanyLocationID">
                    <td class="text-start">{{ item.managerCompanyLocationName }}</td>
                    <td class="text-start">{{ item.managerCompanyLocationTelephone || "-" }}</td>
                    <td class="text-start">{{ getCityLabel(item.atomCity) }}</td>
                    <td class="text-start">{{ item.managerCompanyLocationAddress }}</td>
                    <td class="text-center">
                      {{ getManagerCompanyStatusLabel(item.atomManagerCompanyStatus) }}
                    </td>
                    <td
                      v-if="
                        (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')) ||
                        (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'delete'))
                      "
                      class="text-center"
                    >
                      <button
                        v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                        class="btn-update me-1"
                        @click="clickOpenUpdateLocationModalBtn(item.managerCompanyLocationID)"
                      >
                        編輯
                      </button>
                      <button
                        v-if="
                          !isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'delete')
                        "
                        class="btn-delete"
                        @click="clickRemoveProjectStoneBtn(item.managerCompanyLocationID)"
                      >
                        刪除
                      </button>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>
          <div v-else class="text-center py-4">無資料</div>
        </div>
      </div>
    </div>

    <!-- 關係客戶區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between items-center">
        <div class="subtitle">關係客戶</div>
        <button
          v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-add"
          @click="clickAddAffiliateBtn"
        >
          新增關係客戶
        </button>
      </div>

      <div class="flex-1 overflow-y-auto table-scrollable">
        <div v-if="affiliateList.length > 0">
          <table class="table-base table-fixed table-sticky w-full">
            <thead class="sticky top-0 bg-white z-10">
              <tr>
                <th class="text-start w-40">統編</th>
                <th class="text-start">關係客戶名稱</th>
                <th
                  v-if="
                    (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')) ||
                    (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'delete'))
                  "
                  class="text-center w-40"
                >
                  操作
                </th>
              </tr>
            </thead>
            <tbody>
              <template v-if="affiliateList.length > 0">
                <tr
                  v-for="item in affiliateList"
                  :key="item.managerCompanyAffiliateID"
                  class="border"
                >
                  <td class="text-start">{{ item.managerCompanyAffiliateUnifiedNumber }}</td>
                  <td class="text-start">{{ item.managerCompanyAffiliateName }}</td>
                  <td
                    v-if="
                      (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')) ||
                      (!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'delete'))
                    "
                    class="text-center"
                  >
                    <button
                      v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                      class="btn-update me-1"
                      @click="clickOpenUpdateAffiliateModalBtn(item.managerCompanyAffiliateID)"
                    >
                      編輯
                    </button>
                    <button
                      v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'delete')"
                      class="btn-delete"
                      @click="clickRemoveAffiliateBtn(item.managerCompanyAffiliateID)"
                    >
                      刪除
                    </button>
                  </td>
                </tr>
              </template>
            </tbody>
          </table>
        </div>
        <div v-else class="text-center py-4">無資料</div>
      </div>
    </div>
  </div>

  <!-- 新增營業地點 Modal -->
  <div v-if="sysComLocationModalAddShow" class="modal-overlay">
    <div class="w-[480px] bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col">
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">新增營業地點</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysComLocationModalAddShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-2">
          <!-- 營業地點名稱 -->
          <div>
            <label class="form-label">營業地點名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComLocationListItemObj.managerCompanyLocationName"
              class="input-box"
              type="text"
              placeholder="請輸入營業地點名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComLocationValidObj.managerCompanyLocationName" class="error-tip">
                不可為空，長度不可超過100個字
              </span>
            </p>
          </div>

          <!-- 電話 -->
          <div>
            <label class="form-label">電話</label>
            <input
              v-model="sysComLocationListItemObj.managerCompanyLocationTelephone"
              class="input-box"
              placeholder="請輸入電話"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComLocationValidObj.managerCompanyLocationTelephone"
                class="error-tip"
              >
                格式不符 02-123-4567 或 02-1234-5678
              </span>
            </p>
          </div>

          <!-- 縣市和地址 -->
          <div class="flex gap-4">
            <!-- 縣市 -->
            <div class="w-40">
              <label class="form-label">縣市 <span class="required-label">*</span></label>
              <GetManyCityComboBox v-model:atom-city="sysComLocationListItemObj.atomCity" />
              <p class="error-wrapper">
                <span v-if="!sysComLocationValidObj.atomCity" class="error-tip">不可為空</span>
              </p>
            </div>

            <!-- 地址 -->
            <div class="flex-1">
              <label class="form-label">地址 <span class="required-label">*</span></label>
              <input
                v-model="sysComLocationListItemObj.managerCompanyLocationAddress"
                class="input-box"
                placeholder="請輸入地址"
              />
              <p class="error-wrapper">
                <span
                  v-if="!sysComLocationValidObj.managerCompanyLocationAddress"
                  class="error-tip"
                >
                  不可為空，長度不可超過100個字
                </span>
              </p>
            </div>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態</label>
            <select v-model="sysComLocationListItemObj.atomManagerCompanyStatus" class="select-box">
              <option :value="DbAtomManagerCompanyStatusEnum.NotSelected">請選擇狀態</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Operating">營運中</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Closed">停業</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Unclear">不清楚</option>
            </select>
            <p class="error-wrapper"></p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button
            class="btn-cancel"
            @click="
              sysComLocationModalAddShow = false;
              resetLocationValidation();
            "
          >
            取消
          </button>
          <button class="btn-submit" @click="clickSubmitAddLocationBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 編輯營業地點 Modal -->
  <div v-if="sysComLocationModalUpdateShow" class="modal-overlay">
    <div class="w-[480px] bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col">
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯營業地點</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="
            sysComLocationModalUpdateShow = false;
            closeError();
          "
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-2">
          <!-- 名稱 -->
          <div>
            <label class="form-label">名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComLocationListItemObj.managerCompanyLocationName"
              class="input-box"
              type="text"
              placeholder="請輸入名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComLocationValidObj.managerCompanyLocationName" class="error-tip">
                不可為空，長度不可超過100個字
              </span>
            </p>
          </div>

          <!-- 電話 -->
          <div>
            <label class="form-label">電話</label>
            <input
              v-model="sysComLocationListItemObj.managerCompanyLocationTelephone"
              class="input-box"
              placeholder="請輸入電話"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComLocationValidObj.managerCompanyLocationTelephone"
                class="error-tip"
              >
                格式不符 02-123-4567 或 02-1234-5678
              </span>
            </p>
          </div>

          <!-- 縣市和地址 -->
          <div class="flex gap-4">
            <!-- 縣市 -->
            <div class="w-40">
              <label class="form-label">縣市 <span class="required-label">*</span></label>
              <GetManyCityComboBox v-model:atom-city="sysComLocationListItemObj.atomCity" />
              <p class="error-wrapper">
                <span v-if="!sysComLocationValidObj.atomCity" class="error-tip">不可為空</span>
              </p>
            </div>

            <!-- 地址 -->
            <div class="flex-1">
              <label class="form-label">地址 <span class="required-label">*</span></label>
              <input
                v-model="sysComLocationListItemObj.managerCompanyLocationAddress"
                class="input-box"
                placeholder="請輸入地址"
              />
              <p class="error-wrapper">
                <span
                  v-if="!sysComLocationValidObj.managerCompanyLocationAddress"
                  class="error-tip"
                >
                  不可為空，長度不可超過100個字
                </span>
              </p>
            </div>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態</label>
            <select v-model="sysComLocationListItemObj.atomManagerCompanyStatus" class="select-box">
              <option :value="DbAtomManagerCompanyStatusEnum.NotSelected">請選擇狀態</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Operating">營運中</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Closed">停業</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Unclear">不清楚</option>
            </select>
            <p class="error-wrapper"></p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button
            class="btn-cancel"
            @click="
              sysComLocationModalUpdateShow = false;
              resetLocationValidation();
            "
          >
            取消
          </button>
          <button class="btn-submit" @click="clickSubmitUpdateLocationBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 新增關係客戶 Modal -->
  <div v-if="sysComAffiliateModalAddShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">新增關係客戶</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="
            sysComAffiliateModalAddShow = false;
            closeError();
          "
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-2">
          <!-- 統編 -->
          <div>
            <label class="form-label">統編 <span class="required-label">*</span></label>
            <input
              v-model="sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber"
              class="input-box"
              type="text"
              placeholder="請輸入統編"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber"
                class="error-tip"
              >
                不可為空，長度不可超過 8 個字
              </span>
            </p>
          </div>

          <!-- 關係客戶名稱 -->
          <div>
            <label class="form-label">關係客戶名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComAffiliateListItemObj.managerCompanyAffiliateName"
              class="input-box"
              type="text"
              placeholder="請輸入關係客戶名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComAffiliateValidObj.managerCompanyAffiliateName" class="error-tip">
                不可為空，長度不可超過 300 個字
              </span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button
            class="btn-cancel"
            @click="
              sysComAffiliateModalAddShow = false;
              resetAffiliateValidation();
              closeError();
            "
          >
            取消
          </button>
          <button class="btn-submit" @click="clickSubmitAddAffiliateBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 編輯關係客戶 Modal -->
  <div v-if="sysComAffiliateModalUpdateShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯關係客戶</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="
            sysComAffiliateModalUpdateShow = false;
            closeError();
          "
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-2">
          <!-- 統編 -->
          <div>
            <label class="form-label">統編 <span class="required-label">*</span></label>
            <input
              v-model="sysComAffiliateListItemObj.managerCompanyAffiliateUnifiedNumber"
              class="input-box"
              type="text"
              placeholder="請輸入統編"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber"
                class="error-tip"
              >
                不可為空，長度不可超過 8 個字
              </span>
            </p>
          </div>

          <!-- 關係客戶名稱 -->
          <div>
            <label class="form-label">關係客戶名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComAffiliateListItemObj.managerCompanyAffiliateName"
              class="input-box"
              type="text"
              placeholder="請輸入關係客戶名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComAffiliateValidObj.managerCompanyAffiliateName" class="error-tip">
                不可為空，長度不可超過 300 個字
              </span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button
            class="btn-cancel"
            @click="
              sysComAffiliateModalUpdateShow = false;
              resetAffiliateValidation();
            "
          >
            取消
          </button>
          <button class="btn-submit" @click="clickSubmitUpdateAffiliateBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

  <!--確認刪除【營業地點】彈跳視窗-->
  <ConfirmDialog
    :show="showDeleteLocationConfirm"
    title="確認刪除"
    message="確定要刪除此營業地點嗎？刪除後將無法復原。"
    @confirm="handleRemoveLocation"
    @cancel="cancelRemoveLocation"
  />

  <!--確認刪除【關係客戶】彈跳視窗-->
  <ConfirmDialog
    :show="showDeleteAffiliateConfirm"
    title="確認刪除"
    message="確定要刪除此關係客戶嗎？刪除後將無法復原。"
    @confirm="handleRemoveAffiliate"
    @cancel="cancelRemoveAffiliate"
  />
</template>
