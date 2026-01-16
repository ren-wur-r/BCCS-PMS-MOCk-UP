<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, computed, watch, onBeforeUnmount } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
import {
  getManyPhoneEmployeePipelineContacter,
  getManyPhoneEmployeePipelinePhone,
  getManyPhoneEmployeePipelineProduct,
  getManyPhoneEmployeePipelineSaler,
  getPhoneActivityEmployeePipeline,
  getPhoneOriginalEmployeePipelineContacter,
  removePhoneEmployeePipelineContacter,
  removePhoneEmployeePipelineProduct,
  updatePhoneActivityEmployeePipelineStatus,
} from "@/services";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import {
  MbsCrmPhnHttpGetActivityEmployeePipelineReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelinePhoneReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineProductReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineProductRspMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineSalerReqMdl,
  MbsCrmPhnHttpGetManyEmployeePipelineSalerRspMdl,
  MbsCrmPhnHttpGetOriginalEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpGetOriginalEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpRemoveEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpRemoveEmployeePipelineContacterRspMdl,
  MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusReqMdl,
  MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { getManagerContacterStatusLabel } from "@/utils/getManagerContacterStatusLabel";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";
// import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { getEmployeePipelineSalerStatusLabel } from "@/utils/getEmployeePipelineSalerStatusLabel";
import { formatDateTime } from "@/utils/timeFormatter";
import { formatCurrency } from "@/utils/currencyFormatter";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import UpdatePhoneCompanyModal from "./components/UpdatePhoneCompanyModal.vue";
import AddCrmPhoneContacterModal from "./components/AddCrmPhoneContacterModal.vue";
import UpdateCrmPhoneContacterModal from "./components/UpdateCrmPhoneContacterModal.vue";
import AddCrmPhoneProductModal from "./components/AddCrmPhoneProductModal.vue";
import AddCrmPhonePhoneModal from "./components/AddCrmPhonePhoneModal.vue";
import UpdateCrmPhoneProductModal from "./components/UpdateCrmPhoneProductModal.vue";
import AddCrmPhoneSalerModal from "./components/AddCrmPhoneSalerModal.vue";
import ActivityDetailTabs from "@/components/feature/activity/ActivityDetailTabs.vue";
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作 */
const router = useRouter();
/** 路由參數 */
const route = useRoute();
//#endregion

//#region 型別定義
/** 名單頁籤種類列舉 */
enum PipelineTabEnum {
  BasicData = "basicData",
  Survey = "survey",
}

/** Crm-活動名單-檢視-頁面模型 */
interface CrmActivityPipelineDetailViewMdl {
  managerCompanyName: string;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterTelephone: string | null;
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  managerContacterIsConsent: boolean;
  managerActivityName: string | null;
  originalCompanyList: CrmPhonePipelineCompanyItemMdl | null;
  companyList: CrmPhonePipelineCompanyItemMdl | null;
  originalContacterList: CrmPhonePipelineOriginalContacterItemMdl | null;
  contacterList: CrmPhonePipelineContacterItemMdl[];
  productList: CrmPhonePipelineProductItemMdl[];
  phoneList: CrmPhonePipelinePhoneItemMdl[];
  salerList: CrmPhonePipelineSalerItemMdl[];
}

/** CRM-電銷管理-名單-公司-項目模型 */
interface CrmPhonePipelineCompanyItemMdl {
  managerCompanyID: number;
  managerCompanyUnifiedNumber: string | null;
  managerCompanyName: string | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum;
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  atomCity: DbAtomCityEnum | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum;
}

/** CRM-電銷管理-名單-原始窗口-項目模型 */
interface CrmPhonePipelineOriginalContacterItemMdl {
  managerContacterID: number;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterTelephone: string | null;
  managerContacterIsConsent: boolean;
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  employeePipelineContacterIsPrimary: boolean;
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  managerContacterRemark: string | null;
}

/** CRM-電銷管理-名單-窗口-項目模型 */
interface CrmPhonePipelineContacterItemMdl {
  employeePipelineContacterID: number;
  managerContacterID: number;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterTelephone: string | null;
  managerContacterIsConsent: boolean;
  managerContacterStatus: DbAtomManagerContacterStatusEnum | null;
  employeePipelineContacterIsPrimary: boolean;
  atomRatingKind: DbAtomManagerContacterRatingKindEnum | null;
  managerContacterRemark: string | null;
}

/** CRM-電銷管理-名單-產品-項目模型 */
interface CrmPhonePipelineProductItemMdl {
  employeePipelineProductID: number;
  managerProductID: number;
  managerProductName: string | null;
  managerProductMainKindID: number;
  managerProductMainKindName: string | null;
  managerProductSubKindID: number;
  managerProductSubKindName: string | null;
  managerProductSpecificationID: number;
  managerProductSpecificationName: string | null;
  managerProductSpecificationSellPrice: number;
}

/** CRM-電銷管理-名單-電銷紀錄-項目模型 */
interface CrmPhonePipelinePhoneItemMdl {
  employeePipelinePhoneID: number;
  employeePipelinePhoneRecordTime: string | null;
  managerContacterName: string | null;
  employeePipelinePhoneTitle: string | null;
  employeePipelinePhoneRemark: string | null;
  employeePipelinePhoneCreateEmployeeName: string | null;
}

/** CRM-電銷管理-名單-指派業務-項目模型 */
interface CrmPhonePipelineSalerItemMdl {
  employeePipelineSalerID: number;
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum | null;
  employeePipelineSalerCreateTime: string | null;
  employeePipelineSalerCreateEmployeeName: string | null;
  employeePipelineSalerReplyTime: string | null;
  employeePipelineSalerEmployeeName: string | null;
  employeePipelineSalerRemark: string | null;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmPhone;
/** 目錄:窗口 */
const contacterMenu = DbAtomMenuEnum.SystemContacter;
/** 目錄:客戶 */
const companyMenu = DbAtomMenuEnum.SystemCompany;
/** 名單ID(路由參數取得) */
const managerActivityEmployeePipelineID = Number(route.params.pipelineId);
/** 活動ID(路由參數取得) */
const managerActivityID = Number(route.params.activityId);
/** 作用中的Tab */
const activeTab = ref<PipelineTabEnum>(PipelineTabEnum.BasicData);
/** 窗口ID */
const selectedContacterID = ref<number | null>(null);
/** 商機窗口ID */
const selectedEmployeePipelineContacterID = ref<number | null>(null);
/** 是否為主要窗口 */
const employeePipelineContacterIsPrimary = ref<boolean>(false);
/** 是否顯示新增附加產品視窗 */
const isShowAddProductModal = ref(false);
/** 是否顯示編輯附加產品視窗 */
const isShowUpdateProductModal = ref(false);
/** 選擇中的產品ID */
const selectedProductId = ref<number>(0);
/** 是否顯示新增電銷記錄視窗 */
const isShowAddPhoneModal = ref(false);
/** 是否顯示新增指派業務視窗 */
const isShowAddSalerModal = ref(false);
/** 是否顯示更新客戶視窗 */
const isShowUpdateCompanyModal = ref(false);
/** 是否顯示附加窗口視窗 */
const isShowAddContacterModal = ref(false);
/** 是否顯示更新窗口視窗 */
const isShowUpdateContacterModal = ref(false);
/** 原始客戶區塊是否展開 */
const isOriCompanyOpen = ref(true);
/** 客戶區塊是否展開 */
const isCompanyOpen = ref(true);
/** 窗口區塊是否展開 */
const isContacterOpen = ref(true);

/** CRM-活動名單-檢視-頁面物件 */
const crmActivityPipelineDetailViewObj = reactive<CrmActivityPipelineDetailViewMdl>({
  managerCompanyName: "",
  atomEmployeeRange: DbAtomEmployeeRangeEnum.NotSelected,
  managerCompanyMainKindName: "",
  managerCompanySubKindName: "",
  managerCompanyLocationAddress: "",
  managerCompanyLocationTelephone: "",
  managerContacterName: "",
  managerContacterEmail: "",
  managerContacterPhone: "",
  managerContacterDepartment: "",
  managerContacterJobTitle: "",
  managerContacterTelephone: "",
  atomPipelineStatus: DbAtomPipelineStatusEnum.Hyphen,
  managerContacterIsConsent: false,
  managerActivityName: "",
  originalCompanyList: null,
  companyList: null,
  originalContacterList: null,
  contacterList: [],
  productList: [],
  phoneList: [],
  salerList: [],
});

/** CRM-電銷管理-名單-原始窗口-項目物件 */
const crmPhonePipelineOriginalContacterItemObj = reactive<CrmPhonePipelineOriginalContacterItemMdl>(
  {
    managerContacterID: 0,
    managerContacterName: "",
    managerContacterEmail: "",
    managerContacterPhone: "",
    managerContacterDepartment: "",
    managerContacterJobTitle: "",
    managerContacterTelephone: "",
    managerContacterIsConsent: false,
    employeePipelineContacterIsPrimary: false,
    managerContacterStatus: DbAtomManagerContacterStatusEnum.Unknown,
    atomRatingKind: DbAtomManagerContacterRatingKindEnum.NotSelected,
    managerContacterRemark: null,
  }
);

/** 控制各窗口的開關 */
const contacterOpenMap = reactive<Record<number, boolean>>({});

/** 最新一筆指派業務 */
const latestSaler = computed(() => {
  return crmActivityPipelineDetailViewObj.salerList?.[0] || null;
});

/** 是否有待回覆的指派業務 */
const isLatestPending = computed(() => {
  if (!latestSaler.value) return false;

  return (
    latestSaler.value.employeePipelineSalerStatus === DbAtomEmployeePipelineSalerStatusEnum.Pending
  );
});
/** 是否有接收的指派業務 */
const isLatestAccepted = computed(() => {
  if (!latestSaler.value) return false;

  return (
    latestSaler.value.employeePipelineSalerStatus === DbAtomEmployeePipelineSalerStatusEnum.Accepted
  );
});

//#endregion

//#region UI行為
/** 切換Tab */
const changeTab = (tab: PipelineTabEnum) => {
  activeTab.value = tab;
  if (tab === PipelineTabEnum.BasicData) {
    getOriginalEmployeePipelineContacterData();
  }
};

/** 切換窗口開關 */
const toggleContacter = (id: number) => {
  contacterOpenMap[id] = !contacterOpenMap[id];
};

/** 取得指派業務狀態樣式 */
const getStatusClass = (status: DbAtomEmployeePipelineSalerStatusEnum | null) => {
  switch (status) {
    case DbAtomEmployeePipelineSalerStatusEnum.Accepted:
      return "bg-green-200 text-green-800";
    case DbAtomEmployeePipelineSalerStatusEnum.Pending:
      return "bg-yellow-200 text-yellow-800";
    case DbAtomEmployeePipelineSalerStatusEnum.Rejected:
      return "bg-red-200 text-red-800";
    default:
      return "bg-gray-200 text-gray-800";
  }
};
//#endregion

//#region API / 資料流程
/** 取得【活動名單】資料 */
const getPipelineData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData = await getPhoneActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.atomPipelineStatus = responseData.atomPipelineStatus;
  // 原始客戶
  crmActivityPipelineDetailViewObj.originalCompanyList = responseData.originalCompany
    ? {
      managerCompanyID: 0,
      managerCompanyUnifiedNumber: responseData.originalCompany.managerCompanyUnifiedNumber,
      managerCompanyName: responseData.originalCompany.managerCompanyName,
      atomEmployeeRange: responseData.originalCompany.atomEmployeeRange,
      atomCustomerGrade: responseData.originalCompany.atomCustomerGrade,
      managerCompanyMainKindName: responseData.originalCompany.managerCompanyMainKindName,
      managerCompanySubKindName: responseData.originalCompany.managerCompanySubKindName,
      atomCity: responseData.originalCompany.atomCity,
      managerCompanyLocationAddress: responseData.originalCompany.managerCompanyLocationAddress,
      managerCompanyLocationTelephone:
        responseData.originalCompany.managerCompanyLocationTelephone,
      managerCompanyLocationStatus: responseData.originalCompany.managerCompanyLocationStatus,
    }
    : null;
  // 目前客戶
  crmActivityPipelineDetailViewObj.companyList = responseData.company
    ? {
      managerCompanyID: responseData.company.managerCompanyID,
      managerCompanyUnifiedNumber: responseData.company.managerCompanyUnifiedNumber,
      managerCompanyName: responseData.company.managerCompanyName,
      atomEmployeeRange: responseData.company.atomEmployeeRange,
      atomCustomerGrade: responseData.company.atomCustomerGrade,
      managerCompanyMainKindName: responseData.company.managerCompanyMainKindName,
      managerCompanySubKindName: responseData.company.managerCompanySubKindName,
      atomCity: responseData.company.atomCity,
      managerCompanyLocationAddress: responseData.company.managerCompanyLocationAddress,
      managerCompanyLocationTelephone: responseData.company.managerCompanyLocationTelephone,
      managerCompanyLocationStatus: responseData.company.managerCompanyLocationStatus,
    }
    : null;
  //  原始窗口
  crmActivityPipelineDetailViewObj.originalContacterList = responseData.originalContacter
    ? {
      managerContacterID: 0,
      managerContacterName: responseData.originalContacter.managerContacterName,
      managerContacterEmail: responseData.originalContacter.managerContacterEmail,
      managerContacterPhone: responseData.originalContacter.managerContacterPhone,
      managerContacterDepartment: responseData.originalContacter.managerContacterDepartment,
      managerContacterJobTitle: responseData.originalContacter.managerContacterJobTitle,
      managerContacterTelephone: responseData.originalContacter.managerContacterTelephone,
      managerContacterIsConsent: responseData.originalContacter.managerContacterIsConsent,
      managerContacterStatus: responseData.originalContacter.managerContacterStatus,
      atomRatingKind: responseData.originalContacter.atomRatingKind,
      employeePipelineContacterIsPrimary: false,
      managerContacterRemark: null,
    }
    : null;
};

/** 取得【原始名單窗口】資料 */
const getOriginalEmployeePipelineContacterData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetOriginalEmployeePipelineContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmPhnHttpGetOriginalEmployeePipelineContacterRspMdl | null =
    await getPhoneOriginalEmployeePipelineContacter(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 處理資料
  crmPhonePipelineOriginalContacterItemObj.managerContacterName = responseData.managerContacterName;
  crmPhonePipelineOriginalContacterItemObj.managerContacterEmail =
    responseData.managerContacterEmail;
  crmPhonePipelineOriginalContacterItemObj.managerContacterPhone =
    responseData.managerContacterPhone;
  crmPhonePipelineOriginalContacterItemObj.managerContacterDepartment =
    responseData.managerContacterDepartment;
  crmPhonePipelineOriginalContacterItemObj.managerContacterJobTitle =
    responseData.managerContacterJobTitle;
  crmPhonePipelineOriginalContacterItemObj.managerContacterTelephone =
    responseData.managerContacterTelephone;
  crmPhonePipelineOriginalContacterItemObj.managerContacterIsConsent =
    responseData.managerContacterIsConsent;
  crmPhonePipelineOriginalContacterItemObj.managerContacterStatus =
    responseData.managerContacterStatus;
  crmPhonePipelineOriginalContacterItemObj.atomRatingKind = responseData.atomRatingKind;
};

/** 取得【窗口】列表 */
const getContacterList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetManyEmployeePipelineContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmPhnHttpGetManyEmployeePipelineContacterRspMdl | null =
    await getManyPhoneEmployeePipelineContacter(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 驗證是否有窗口資料
  if (responseData.employeePipelineContacterList === null) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.contacterList = responseData.employeePipelineContacterList.map(
    (item) => ({
      employeePipelineContacterID: item.employeePipelineContacterID,
      managerContacterID: item.managerContacterID,
      employeePipelineContacterIsPrimary: item.employeePipelineContacterIsPrimary,
      managerContacterName: item.managerContacterName,
      managerContacterEmail: item.managerContacterEmail,
      managerContacterPhone: item.managerContacterPhone,
      managerContacterDepartment: item.managerContacterDepartment,
      managerContacterJobTitle: item.managerContacterJobTitle,
      managerContacterTelephone: item.managerContacterTelephone,
      managerContacterIsConsent: item.managerContacterIsConsent,
      managerContacterStatus: item.managerContacterStatus,
      atomRatingKind: item.atomRatingKind,
      managerContacterRemark: item.managerContacterRemark,

      _open: false,
    })
  );
};

/** 取得【指派業務】列表 */
const getEmployeePipelineSalerList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetManyEmployeePipelineSalerReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmPhnHttpGetManyEmployeePipelineSalerRspMdl | null =
    await getManyPhoneEmployeePipelineSaler(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 驗證是否有指派業務資料
  if (responseData.employeePipelineSalerList === null) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.salerList = responseData.employeePipelineSalerList.map(
    (item) => ({
      employeePipelineSalerID: item.employeePipelineSalerID,
      employeePipelineSalerStatus: item.employeePipelineSalerStatus,
      employeePipelineSalerCreateTime: item.employeePipelineSalerCreateTime,
      employeePipelineSalerCreateEmployeeName: item.employeePipelineSalerCreateEmployeeName,
      employeePipelineSalerReplyTime: item.employeePipelineSalerReplyTime,
      employeePipelineSalerEmployeeName: item.employeePipelineSalerEmployeeName,
      employeePipelineSalerRemark: item.employeePipelineSalerRemark,
    })
  );
};

/** 取得【電銷記錄】列表 */
const getEmployeePipelinePhoneList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetManyEmployeePipelinePhoneReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmPhnHttpGetManyEmployeePipelinePhoneRspMdl | null =
    await getManyPhoneEmployeePipelinePhone(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 驗證是否有電銷記錄資料
  if (responseData.employeePipelinePhoneList === null) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.phoneList = responseData.employeePipelinePhoneList.map(
    (item) => ({
      employeePipelinePhoneID: item.employeePipelinePhoneID,
      employeePipelinePhoneRecordTime: item.employeePipelinePhoneRecordTime,
      managerContacterName: item.managerContacterName,
      employeePipelinePhoneTitle: item.employeePipelinePhoneTitle,
      employeePipelinePhoneCreateEmployeeName: item.employeePipelinePhoneCreateEmployeeName,
      employeePipelinePhoneRemark: item.employeePipelinePhoneRemark,
    })
  );
};

/** 取得【產品】列表 */
const getProductList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetManyEmployeePipelineProductReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmPhnHttpGetManyEmployeePipelineProductRspMdl | null =
    await getManyPhoneEmployeePipelineProduct(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 驗證是否有產品資料
  if (responseData.employeePipelineProductList === null) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.productList = responseData.employeePipelineProductList.map(
    (item) => ({
      employeePipelineProductID: item.employeePipelineProductID,
      managerProductID: item.managerProductID,
      managerProductName: item.managerProductName,
      managerProductMainKindID: item.managerProductMainKindID,
      managerProductMainKindName: item.managerProductMainKindName,
      managerProductSubKindID: item.managerProductSubKindID,
      managerProductSubKindName: item.managerProductSubKindName,
      managerProductSpecificationID: item.managerProductSpecificationID,
      managerProductSpecificationName: item.managerProductSpecificationName,
      managerProductSpecificationSellPrice: item.managerProductSpecificationSellPrice,
    })
  );
};

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push(`/crm/phone/activity/detail/${managerActivityID}/pipeline`);
};

/** 點擊【編輯窗口】按鈕 */
const clickUpdateContacterBtn = (contacterId: number) => {
  selectedContacterID.value = contacterId;
  selectedEmployeePipelineContacterID.value =
    crmActivityPipelineDetailViewObj?.contacterList.find(
      (item) => item.managerContacterID === contacterId
    )?.employeePipelineContacterID || null;
  employeePipelineContacterIsPrimary.value =
    crmActivityPipelineDetailViewObj?.contacterList.find(
      (item) => item.managerContacterID === contacterId
    )?.employeePipelineContacterIsPrimary || false;
  isShowUpdateContacterModal.value = true;
};

/** 點擊【刪除】窗口按鈕 */
const clickRemoveContacterBtn = async (contacterId: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpRemoveEmployeePipelineContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineContacterID: contacterId,
  };
  const responseData: MbsCrmPhnHttpRemoveEmployeePipelineContacterRspMdl | null =
    await removePhoneEmployeePipelineContacter(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除窗口成功！");

  // 重新取得列表資料
  getContacterList();
};

/** 點擊【編輯產品】按鈕 */
const clickUpdateProductBtn = (productId: number) => {
  selectedProductId.value = productId;
  isShowUpdateProductModal.value = true;
};

/** 點擊【刪除產品】按鈕 */
const clickRemoveProductBtn = async (productId: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineProductID: productId,
  };
  const responseData = await removePhoneEmployeePipelineProduct(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除產品成功！");

  // 清空目前顯示資料
  Object.assign(crmActivityPipelineDetailViewObj.productList, {
    employeePipelineProductID: null,
    employeePipelineID: null,
    managerCompanyID: null,
    managerProductID: null,
    managerProductName: "",
    managerProductMainKindID: null,
    managerProductMainKindName: "",
    managerProductSubKindID: null,
    managerProductSubKindName: "",
    managerProductSpecificationID: null,
    managerProductSpecificationName: "",
  });

  // 重新取得產品列表資料
  getProductList();
};

/** 點擊【附加窗口】按鈕 */
const clickAddContacterBtn = () => {
  if (!crmActivityPipelineDetailViewObj.companyList?.managerCompanyID) {
    displayError("請先確認上方的『客戶』資料");
    return;
  }

  isShowAddContacterModal.value = true;
};

/** 點擊【附加電銷】按鈕 */
const clickAddPhoneBtn = () => {
  selectedContacterID.value =
    crmActivityPipelineDetailViewObj?.contacterList.find(
      (item) => item.employeePipelineContacterIsPrimary
    )?.managerContacterID || null;
  if (!selectedContacterID.value) {
    displayError("請先確認上方的『窗口』資料");
    return;
  }

  isShowAddPhoneModal.value = true;
};

/** 點擊【指派業務】按鈕 */
const clickAddSalerBtn = () => {
  selectedContacterID.value =
    crmActivityPipelineDetailViewObj?.contacterList.find(
      (item) => item.employeePipelineContacterIsPrimary
    )?.managerContacterID || null;
  if (!selectedContacterID.value || !crmActivityPipelineDetailViewObj.companyList) {
    displayError("請先確認上方的『客戶』及『窗口』資料正確再進行指派業務");
    return;
  }

  isShowAddSalerModal.value = true;
};

/** 點擊【新增客戶】按鈕 */
const clickCompanyAddBtn = () => {
  window.open("/ManagerBackSite/system/company/add", "_blank");
};

/** 點擊【新增窗口】按鈕 */
const clickContacterAddBtn = () => {
  window.open("/ManagerBackSite/system/contacter/add", "_blank");
};

/** 點擊【更改名單狀態為已完成電銷】按鈕 */
const clickChangeAtomPipelineStatusBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmPhnHttpUpdateActivityEmployeePipelineStatusRspMdl | null =
    await updatePhoneActivityEmployeePipelineStatus(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("已完成電銷！");

  // 更新目前狀態
  crmActivityPipelineDetailViewObj.atomPipelineStatus = DbAtomPipelineStatusEnum.CompletedSales;
};
//#endregion

//#region 生命週期
onMounted(() => {
  getPipelineData();
  getProductList();
  getContacterList();
  getEmployeePipelineSalerList();
  getEmployeePipelinePhoneList();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<PipelineTabEnum, string> = {
      [PipelineTabEnum.BasicData]: "基本資料",
      [PipelineTabEnum.Survey]: "問卷",
    };
    setModuleTitle(titleMap[tab] ?? "基本資料");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});
//#endregion

</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <!-- 標題列 -->
    <div class="flex items-center flex-row gap-2 justify-between">
      <div class="flex flex-row gap-2">
        <div class="flex items-center gap-4">
          <button class="btn-back flex items-center" @click="clickBackBtn">
            <svg class="w-4 h-4 inline-block mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
            </svg>
            <span>返回</span>
          </button>
        </div>
        <div class="h1 mb-3">{{ crmActivityPipelineDetailViewObj.managerActivityName }}</div>
      </div>
    </div>

    <ActivityDetailTabs :activity-id="managerActivityID" base-path="/crm/phone/activity" />

    <div>
      <!-- Tabs 選單 -->
      <div class="flex mb-0 gap-y-4">
        <button :class="['tab-btn', { active: activeTab === PipelineTabEnum.BasicData }]"
          @click="changeTab(PipelineTabEnum.BasicData)">
          基本資料
        </button>
        <!-- <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.Survey }]"
        @click="changeTab(PipelineTabEnum.Survey)"
      >
        問卷填寫
      </button> -->
      </div>

      <!-- Tabs 內容 -->
      <div class="tab-content flex-1 overflow-hidden">
        <!-- 基本資料 內容 -->
        <div v-if="activeTab === PipelineTabEnum.BasicData" class="flex flex-col gap-4">
          <!-- 基本資訊 區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
            <div class="text-xl font-bold text-brand-100">基本資訊</div>
            <div class="flex items-center">
              <!-- 狀態 -->
              <div class="mb-4 flex flex-col flex-1 gap-2">
                <label class="form-label">狀態</label>
                <select v-model="crmActivityPipelineDetailViewObj.atomPipelineStatus" class="select-box" disabled>
                  <option :value="DbAtomPipelineStatusEnum.Hyphen">-</option>
                  <option :value="DbAtomPipelineStatusEnum.Opened">已開啟</option>
                  <option :value="DbAtomPipelineStatusEnum.Clicked">已點擊</option>
                  <option :value="DbAtomPipelineStatusEnum.TransferredToSales">已轉電銷</option>
                  <option :value="DbAtomPipelineStatusEnum.CompletedSales">已完成電銷</option>
                  <option :value="DbAtomPipelineStatusEnum.TransferredToBusiness">已轉業務</option>
                  <option :value="DbAtomPipelineStatusEnum.Business10Percent">商機10%</option>
                  <option :value="DbAtomPipelineStatusEnum.Business30Percent">商機30%</option>
                  <option :value="DbAtomPipelineStatusEnum.Business70Percent">商機70%</option>
                  <option :value="DbAtomPipelineStatusEnum.Business100Percent">商機100%</option>
                  <option :value="DbAtomPipelineStatusEnum.BusinessFailed">商機失敗</option>
                  <option :value="DbAtomPipelineStatusEnum.TransferredToProject">已轉專案</option>
                </select>
              </div>
              <button v-if="
                crmActivityPipelineDetailViewObj.atomPipelineStatus ===
                DbAtomPipelineStatusEnum.TransferredToSales
              " class="btn ml-2 bg-brand-200 text-white" @click="clickChangeAtomPipelineStatusBtn">
                已完成電銷
              </button>
            </div>
          </div>

          <!-- 客戶 區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="flex justify-between items-center">
              <h2 class="subtitle">客戶</h2>
              <button v-if="employeeInfoStore.hasPermission(companyMenu, 'create')" class="btn-add"
                @click="clickCompanyAddBtn">
                新增客戶
              </button>
            </div>

            <div class="flex flex-row gap-3">
              <!-- 原始客戶 區塊 -->
              <div class="rounded-lg flex-1">
                <!-- 標題列 -->
                <button class="w-full h-12 flex justify-between items-center p-3 bg-gray-100 rounded-t-lg" :class="[
                  'text-sm text-gray-700 border border-gray-300 bg-gray-50',
                  isOriCompanyOpen
                    ? 'border-b-0 rounded-0 ' //打開
                    : 'border-b-1 rounded-b-lg', // 關閉
                ]" @click="isOriCompanyOpen = !isOriCompanyOpen">
                  <span class="text-brand-201 font-semibold">原始客戶</span>
                  <svg :class="[
                    'w-4 h-4 transform transition-transform',
                    isOriCompanyOpen ? 'rotate-180' : 'rotate-0',
                  ]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </button>

                <!-- 原始公司內容區塊 -->
                <div v-show="isOriCompanyOpen" :class="[
                  'p-8 rounded-b-lg text-sm text-gray-700 ',
                  isOriCompanyOpen ? 'border border-gray-300' : 'border-t-0',
                ]">
                  <div v-if="crmActivityPipelineDetailViewObj.originalCompanyList"
                    class="grid grid-cols-1 gap-x-6 gap-y-2">
                    <!-- 客戶全名 -->
                    <div>
                      <p class="flex flex-row gap-5">
                        <span class="display-label w-20 shrink-0">客戶全名</span>
                        <span class="display-value truncate">
                          {{
                            crmActivityPipelineDetailViewObj.originalCompanyList
                              .managerCompanyName || "-"
                          }}
                        </span>
                      </p>
                      <hr class="my-2" />
                    </div>

                    <!-- 人員規模 -->
                    <!-- <div>
                      <p class="flex flex-row gap-5">
                        <span class="display-label w-20 shrink-0">人員規模</span>
                        <span class="display-value truncate">
                          {{
                            getEmployeeRangeLabel(
                              crmActivityPipelineDetailViewObj.originalCompanyList.atomEmployeeRange
                            ) || "-"
                          }}
                        </span>
                      </p>
                      <hr class="my-2" />
                    </div> -->

                    <!-- 客戶主分類 -->
                    <div>
                      <p class="flex flex-row gap-5">
                        <span class="display-label w-20 shrink-0">客戶主分類</span>
                        <span class="display-value truncate">
                          {{
                            crmActivityPipelineDetailViewObj.originalCompanyList
                              .managerCompanyMainKindName || "-"
                          }}
                        </span>
                      </p>
                      <hr class="my-2" />
                    </div>

                    <!-- 客戶子分類 -->
                    <div>
                      <p class="flex flex-row gap-5">
                        <span class="display-label w-20 shrink-0">客戶子分類</span>
                        <span class="display-value truncate">
                          {{
                            crmActivityPipelineDetailViewObj.originalCompanyList
                              .managerCompanySubKindName || "-"
                          }}
                        </span>
                      </p>
                      <hr class="my-2" />
                    </div>

                    <!-- 地址 -->
                    <div>
                      <p class="flex flex-row gap-5">
                        <span class="display-label w-20 shrink-0">地址</span>
                        <span class="display-value truncate">
                          {{
                            crmActivityPipelineDetailViewObj.originalCompanyList
                              .managerCompanyLocationAddress || "-"
                          }}
                        </span>
                      </p>
                      <hr class="my-2" />
                    </div>

                    <!-- 電話 -->
                    <div>
                      <p class="flex flex-row gap-5">
                        <span class="display-label w-20 shrink-0">電話</span>
                        <span class="display-value truncate">
                          {{
                            crmActivityPipelineDetailViewObj.originalCompanyList
                              .managerCompanyLocationTelephone || "-"
                          }}
                        </span>
                      </p>
                      <hr class="my-2" />
                    </div>
                  </div>

                  <!-- 無資料時 -->
                  <div v-else class="text-gray-400 text-center py-2">無公司資料</div>
                </div>
              </div>

              <!-- 確認客戶 區塊 -->
              <div class="rounded-lg flex-1">
                <!-- 標題列 -->
                <button
                  class="w-full h-12 flex justify-between items-center px-3 text-sm text-gray-700 bg-gray-50 border border-gray-300"
                  :class="[isCompanyOpen ? 'rounded-t-lg border-b-1' : 'rounded-lg']"
                  @click="isCompanyOpen = !isCompanyOpen">
                  <div class="flex items-center gap-2">
                    <span class="text-brand-201 font-semibold">確認客戶</span>

                    <button v-if="
                      crmActivityPipelineDetailViewObj.companyList &&
                      employeeInfoStore.hasPermission(menu, 'edit')
                    " class="btn-detail" @click.stop="isShowUpdateCompanyModal = true">
                      重新選擇
                    </button>
                  </div>

                  <svg :class="[
                    'w-4 h-4 transform transition-transform',
                    isCompanyOpen ? 'rotate-180' : 'rotate-0',
                  ]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </button>

                <!-- 內容區塊 -->
                <div v-show="isCompanyOpen"
                  class="p-8 text-sm text-gray-700 border border-gray-300 border-t-0 rounded-b-lg">
                  <!-- 有資料 -->
                  <div v-if="crmActivityPipelineDetailViewObj.companyList" class="grid grid-cols-1 gap-x-6 gap-y-2">
                    <div v-if="crmActivityPipelineDetailViewObj.companyList" class="grid grid-cols-1 gap-x-6 gap-y-2">
                      <!-- 客戶全名 -->
                      <div>
                        <p class="flex flex-row gap-5">
                          <span class="display-label w-20 shrink-0">客戶全名</span>
                          <span class="display-value truncate">
                            {{
                              crmActivityPipelineDetailViewObj.companyList.managerCompanyName || "-"
                            }}
                          </span>
                        </p>
                        <hr class="my-2" />
                      </div>
                      <!-- 人員規模 -->
                      <!-- <div>
                        <p class="flex flex-row gap-5">
                          <span class="display-label w-20 shrink-0">人員規模</span>
                          <span class="display-value truncate">
                            {{
                              getEmployeeRangeLabel(
                                crmActivityPipelineDetailViewObj.companyList.atomEmployeeRange
                              ) || "-"
                            }}
                          </span>
                        </p>
                        <hr class="my-2" />
                      </div> -->
                      <!-- 客戶主分類 -->
                      <div>
                        <p class="flex flex-row gap-5">
                          <span class="display-label w-20 shrink-0">客戶主分類</span>
                          <span class="display-value truncate">
                            {{
                              crmActivityPipelineDetailViewObj.companyList
                                .managerCompanyMainKindName || "-"
                            }}
                          </span>
                        </p>
                        <hr class="my-2" />
                      </div>
                      <!-- 客戶子分類 -->
                      <div>
                        <p class="flex flex-row gap-5">
                          <span class="display-label w-20 shrink-0">客戶子分類</span>
                          <span class="display-value truncate">
                            {{
                              crmActivityPipelineDetailViewObj.companyList
                                .managerCompanySubKindName || "-"
                            }}
                          </span>
                        </p>
                        <hr class="my-2" />
                      </div>
                      <!-- 地址 -->
                      <div>
                        <p class="flex flex-row gap-5">
                          <span class="display-label w-20 shrink-0">地址</span>
                          <span class="display-value truncate">
                            {{
                              crmActivityPipelineDetailViewObj.companyList
                                .managerCompanyLocationAddress || "-"
                            }}
                          </span>
                        </p>
                        <hr class="my-2" />
                      </div>
                      <!-- 電話 -->
                      <div>
                        <p class="flex flex-row gap-5">
                          <span class="display-label w-20 shrink-0">電話</span>
                          <span class="display-value truncate">
                            {{
                              crmActivityPipelineDetailViewObj.companyList
                                .managerCompanyLocationTelephone || "-"
                            }}
                          </span>
                        </p>
                        <hr class="my-2" />
                      </div>
                    </div>
                  </div>

                  <!-- 無資料 -->
                  <div v-else class="flex flex-col items-center justify-center text-center h-full">
                    <div class="text-4xl text-gray-400 mb-2">!</div>
                    <p class="text-gray-500 mb-4">尚未確認客戶資料</p>

                    <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')" class="btn-add"
                      @click="isShowUpdateCompanyModal = true">
                      選擇客戶
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- 窗口 區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="flex justify-between items-center">
              <h2 class="subtitle">窗口</h2>
              <button v-if="employeeInfoStore.hasEveryonePermission(contacterMenu, 'create')" class="btn-add"
                @click="clickContacterAddBtn">
                新增窗口
              </button>
            </div>

            <!-- 原始窗口 -->
            <div>
              <button class="w-full flex justify-between items-center p-3 bg-gray-100 rounded-t-lg" :class="[
                'text-sm text-gray-700 border border-gray-300 bg-gray-50',
                isContacterOpen ? 'border-b-0' : 'border-b-1 rounded-b-lg',
              ]" @click="isContacterOpen = !isContacterOpen">
                <span class="text-brand-201 font-semibold">原始窗口</span>
                <svg :class="[
                  'w-4 h-4 transform transition-transform',
                  isOriCompanyOpen ? 'rotate-180' : 'rotate-0',
                ]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </button>
              <div v-show="isContacterOpen" :class="[
                'p-8 rounded-b-lg text-sm text-gray-700 transition-all duration-200',
                isContacterOpen ? 'border border-gray-300' : 'border-t-0',
              ]">
                <div v-if="crmActivityPipelineDetailViewObj.originalContacterList"
                  class="grid grid-cols-2 gap-x-6 gap-y-2">
                  <!-- 姓名 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">姓名</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterName || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 電子信箱 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">電子信箱</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterEmail || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 手機 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">手機</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterPhone || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 部門 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">部門</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterDepartment || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 職稱 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">職稱</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterJobTitle || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 電話#分機 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">電話#分機</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterTelephone || "-"
                        }}
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
                            crmActivityPipelineDetailViewObj.originalContacterList
                              .managerContacterStatus
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
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.originalContacterList
                            .managerContacterIsConsent
                            ? "同意"
                            : "不同意"
                        }}
                      </span>
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
                            crmActivityPipelineDetailViewObj.originalContacterList.atomRatingKind
                          ) || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>
                </div>

                <!-- 無資料 -->
                <div v-else class="text-gray-400 text-center py-2">無窗口資料</div>
              </div>
            </div>

            <hr />

            <!-- 確認窗口 區塊 -->
            <div class="flex flex-col gap-4">
              <div class="flex justify-between items-center">
                <h3 class="subtitle">確認窗口</h3>
                <!-- 附加窗口 按鈕 -->
                <button
                  v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
                  class="rounded-lg border border-dashed px-4 py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
                  style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                  @click="clickAddContacterBtn"
                >
                  附加窗口
                </button>
              </div>
              <div class="grid grid-cols-1 gap-2">
                <div v-for="item in [...crmActivityPipelineDetailViewObj.contacterList].sort(
                  (a, b) =>
                    Number(b.employeePipelineContacterIsPrimary) -
                    Number(a.employeePipelineContacterIsPrimary)
                )" :key="item.employeePipelineContacterID">
                  <div>
                    <button class="w-full flex justify-between items-center p-3 bg-gray-100 rounded-t-lg" :class="[
                      'p-3  text-sm text-gray-700 transition-all duration-200 border border-gray-300 bg-gray-50 ',
                      contacterOpenMap[item.employeePipelineContacterID]
                        ? 'border-b-0'
                        : 'border-b-1  rounded-b-lg',
                    ]" @click="toggleContacter(item.employeePipelineContacterID)">
                      <div class="flex flex-row items-center gap-4">
                        <span :class="item.employeePipelineContacterIsPrimary
                            ? 'badge-contacter-primary'
                            : 'badge-contacter-secondary'
                          ">
                          {{ item.employeePipelineContacterIsPrimary ? "主要窗口" : "次要窗口" }}
                        </span>
                        <span class="flex flex-col text-left">
                          <span>{{ item.managerContacterName }}</span>
                        </span>
                      </div>

                      <div class="flex flex-row justify-center items-center gap-4">
                        <!-- 展開箭頭 -->
                        <svg :class="[
                          'w-4 h-4 transition-transform duration-200',
                          contacterOpenMap[item.employeePipelineContacterID]
                            ? 'rotate-180'
                            : 'rotate-0',
                        ]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                        </svg>
                        <div class="flex flex-row gap-1">
                          <button
                            v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                            class="rounded-lg border border-dashed px-3 py-1 text-xs font-medium text-[#082F49] hover:text-[#061F30]"
                            style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                            @click="clickUpdateContacterBtn(item.managerContacterID)"
                          >
                            編輯
                          </button>
                          <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'delete')" class="btn-delete"
                            @click="clickRemoveContacterBtn(item.employeePipelineContacterID)">
                            刪除
                          </button>
                        </div>
                      </div>
                    </button>

                    <!-- 明細 -->
                    <div v-show="contacterOpenMap[item.employeePipelineContacterID]" :class="[
                      'p-8  rounded-b-lg text-sm text-gray-700 transition-all duration-200',
                      contacterOpenMap[item.employeePipelineContacterID]
                        ? 'border border-gray-300'
                        : '',
                    ]">
                      <div class="grid grid-cols-2 gap-x-6 gap-y-2">
                        <!-- 姓名 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">姓名</span>
                            <span class="display-value truncate">{{
                              item.managerContacterName || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 電子信箱 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">電子信箱</span>
                            <span class="display-value truncate">{{
                              item.managerContacterEmail || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 手機 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">手機</span>
                            <span class="display-value truncate">{{
                              item.managerContacterPhone || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 部門 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">部門</span>
                            <span class="display-value truncate">{{
                              item.managerContacterDepartment || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 職稱 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">職稱</span>
                            <span class="display-value truncate">{{
                              item.managerContacterJobTitle || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 電話#分機 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">電話#分機</span>
                            <span class="display-value truncate">{{
                              item.managerContacterTelephone || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 狀態 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">狀態</span>
                            <span class="display-value truncate">{{
                              getManagerContacterStatusLabel(item.managerContacterStatus) || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 個資同意 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">個資同意</span>
                            <span class="display-value truncate">
                              {{ item.managerContacterIsConsent ? "同意" : "不同意" }}
                            </span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 開發評等 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">開發評等</span>
                            <span class="display-value truncate">{{
                              getManagerContacterRatingLabel(item.atomRatingKind) || "-"
                              }}</span>
                          </p>
                          <hr class="my-2" />
                        </div>

                        <!-- 備註 -->
                        <div>
                          <p class="flex flex-row gap-5">
                            <span class="display-label w-20 shrink-0">備註</span>
                            <span class="display-value truncate">
                              {{ item.managerContacterRemark || "-" }}
                            </span>
                          </p>
                          <hr class="my-2" />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- 無資料顯示 -->
              <div v-if="crmActivityPipelineDetailViewObj.contacterList.length === 0"
                class="text-center py-8 text-gray-500">
                尚未附加任何窗口
              </div>
            </div>
          </div>

          <!-- 產品 區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="text-xl font-bold text-brand-100">產品</div>

            <!-- 產品列表表格 -->
            <div v-if="crmActivityPipelineDetailViewObj.productList.length > 0">
              <table class="table-base table-fixed table-sticky w-full">
                <thead class="bg-gray-800 text-white">
                  <tr>
                    <th class="text-start">產品名稱</th>
                    <th class="text-start">產品主分類</th>
                    <th class="text-start">產品子分類</th>
                    <th class="text-start">規格</th>
                    <th class="text-end w-32">售價</th>
                    <th class="text-center w-40">操作</th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="item in crmActivityPipelineDetailViewObj.productList"
                    :key="item.employeePipelineProductID">
                    <td class="text-start">
                      {{ item.managerProductName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerProductMainKindName }}
                    </td>
                    <td class="text-start">{{ item.managerProductSubKindName }}</td>
                    <td class="text-start">
                      {{ item.managerProductSpecificationName }}
                    </td>
                    <td class="text-end">
                      {{ formatCurrency(item.managerProductSpecificationSellPrice) }}
                    </td>
                    <td class="text-center">
                      <button
                        v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                        class="me-1 rounded-lg border border-dashed px-3 py-1 text-xs font-medium text-[#082F49] hover:text-[#061F30]"
                        style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                        @click="clickUpdateProductBtn(item.employeePipelineProductID)"
                      >
                        編輯
                      </button>
                      <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'delete')" class="btn-delete"
                        @click="clickRemoveProductBtn(item.employeePipelineProductID)">
                        刪除
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>

            <!-- 無資料顯示 -->
            <div v-else class="text-center py-8 text-gray-500">尚未選擇任何產品</div>

            <button
              v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
              class="rounded-lg border border-dashed px-4 py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
              style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
              @click="isShowAddProductModal = true"
            >
              附加產品
            </button>
          </div>

          <!-- 電銷記錄 區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="text-xl font-bold text-brand-100">電銷記錄</div>

            <!-- 電銷記錄列表表格 -->
            <div v-if="crmActivityPipelineDetailViewObj.phoneList.length > 0">
              <table class="table-base table-fixed table-sticky w-full">
                <thead class="bg-gray-800 text-white">
                  <tr>
                    <th class="text-start w-40">時間</th>
                    <th class="text-start w-24">窗口</th>
                    <th class="text-start w-48">電銷主題</th>
                    <th class="text-start w-24">電銷人員</th>
                    <th class="text-start">備註</th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="item in crmActivityPipelineDetailViewObj.phoneList" :key="item.employeePipelinePhoneID">
                    <td class="text-start">
                      {{ formatDateTime(item.employeePipelinePhoneRecordTime) }}
                    </td>
                    <td class="text-start">
                      {{ item.managerContacterName }}
                    </td>
                    <td class="text-start">{{ item.employeePipelinePhoneTitle }}</td>
                    <td class="text-start">
                      {{ item.employeePipelinePhoneCreateEmployeeName }}
                    </td>
                    <td class="text-start">{{ item.employeePipelinePhoneRemark }}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <!-- 無資料顯示 -->
            <div v-else class="text-center py-8 text-gray-500">尚未附加任何電銷記錄</div>

            <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')" class="btn-add"
              @click="clickAddPhoneBtn">
              附加電銷記錄
            </button>
          </div>

          <!-- 指派業務記錄 區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="text-xl font-bold text-brand-100">指派業務記錄</div>
            <p class="text-sm text-gray-400 mb-2">
              ⚠ 提醒 : 請先確認上方的『客戶』與『窗口』資訊正確後，才能進行業務指派
              並且需等待上一位業務回覆後才可再次指派
            </p>

            <!-- 指派業務記錄列表表格 -->
            <div v-if="crmActivityPipelineDetailViewObj.salerList.length > 0">
              <table class="table-base table-fixed table-sticky w-full">
                <thead class="bg-gray-800 text-white">
                  <tr>
                    <th class="text-start">狀態</th>
                    <th class="text-start">指派時間</th>
                    <th class="text-start">指派人員</th>
                    <th class="text-start">回覆時間</th>
                    <th class="text-start">回覆業務人員</th>
                    <th class="text-start">備註</th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="item in crmActivityPipelineDetailViewObj.salerList" :key="item.employeePipelineSalerID">
                    <td>
                      <span class="p-1 text-nowrap rounded-lg text-sm"
                        :class="getStatusClass(item.employeePipelineSalerStatus)">
                        {{ getEmployeePipelineSalerStatusLabel(item.employeePipelineSalerStatus) }}
                      </span>
                    </td>
                    <td class="text-start">
                      {{ formatDateTime(item.employeePipelineSalerCreateTime) }}
                    </td>
                    <td class="text-start">{{ item.employeePipelineSalerCreateEmployeeName }}</td>
                    <td class="text-start">
                      {{ formatDateTime(item.employeePipelineSalerReplyTime) }}
                    </td>
                    <td class="text-start">
                      {{ item.employeePipelineSalerEmployeeName }}
                    </td>
                    <td class="text-start">{{ item.employeePipelineSalerRemark }}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <!-- 無資料顯示 -->
            <div v-else class="text-center py-8 text-gray-500">尚未指派任何業務</div>

            <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')" class="btn-add"
              :disabled="isLatestPending || isLatestAccepted" @click="clickAddSalerBtn">
              指派業務
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- 錯誤訊息 彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 編輯客戶 彈跳視窗 -->
    <UpdatePhoneCompanyModal v-if="isShowUpdateCompanyModal" :employeePipelineID="managerActivityEmployeePipelineID"
      :hasCompany="!!crmActivityPipelineDetailViewObj.companyList" @close="isShowUpdateCompanyModal = false"
      @submit="getPipelineData" />

    <!-- 新增窗口 彈跳視窗 -->
    <AddCrmPhoneContacterModal v-if="isShowAddContacterModal" :managerContacterCompanyID="crmActivityPipelineDetailViewObj.companyList?.managerCompanyID ?? 0
      " :employeePipelineContacterID="managerActivityEmployeePipelineID"
      :contacterList="crmActivityPipelineDetailViewObj.contacterList" @close="isShowAddContacterModal = false"
      @submit="getContacterList" />

    <!-- 編輯窗口 彈跳視窗 -->
    <UpdateCrmPhoneContacterModal v-if="isShowUpdateContacterModal" :managerContacterCompanyID="crmActivityPipelineDetailViewObj.companyList?.managerCompanyID ?? 0
      " :employeePipelineContacterIsPrimary="employeePipelineContacterIsPrimary"
      :employeePipelineContacterID="selectedEmployeePipelineContacterID ?? 0"
      :employeePipelineID="managerActivityEmployeePipelineID" :managerContacterID="crmActivityPipelineDetailViewObj.contacterList.find(
        (item) => item.employeePipelineContacterID === selectedEmployeePipelineContacterID
      )?.managerContacterID || 0
        " :contacterList="crmActivityPipelineDetailViewObj.contacterList" @close="isShowUpdateContacterModal = false"
      @submit="getContacterList" />

    <!-- 新增產品 彈跳視窗 -->
    <AddCrmPhoneProductModal v-if="isShowAddProductModal" :employeePipelineID="managerActivityEmployeePipelineID"
      @close="isShowAddProductModal = false" @submit="getProductList" />

    <!-- 編輯產品 彈跳視窗 -->
    <UpdateCrmPhoneProductModal v-if="isShowUpdateProductModal" :employeePipelineProductID="selectedProductId"
      @close="isShowUpdateProductModal = false" @submit="getProductList" />

    <!-- 新增電銷記錄 彈跳視窗 -->
    <AddCrmPhonePhoneModal v-if="isShowAddPhoneModal" :employeePipelineID="managerActivityEmployeePipelineID"
      :managerContacterCompanyID="crmActivityPipelineDetailViewObj.companyList?.managerCompanyID ?? 0
        " :contacterList="crmActivityPipelineDetailViewObj.contacterList" @close="isShowAddPhoneModal = false"
      @submit="getEmployeePipelinePhoneList" />

    <!-- 指派業務 彈跳視窗 -->
    <AddCrmPhoneSalerModal v-if="isShowAddSalerModal" :employeePipelineID="managerActivityEmployeePipelineID"
      @close="isShowAddSalerModal = false" @submit="getEmployeePipelineSalerList()" />
  </div>
</template>
