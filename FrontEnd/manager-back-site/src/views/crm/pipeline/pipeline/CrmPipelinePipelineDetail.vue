<script setup lang="ts">
//#region 引入
// Vue
import { ref, reactive, onMounted, computed, watch } from "vue";
// Enums / 常數
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbEmployeePipelineStageCheckEnum, pipelineStageCheckOptions } from "@/constants/DbEmployeePipelineStageCheckEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomEmployeePipelineSalerStatusEnum } from "@/constants/DbAtomEmployeePipelineSalerStatusEnum";
import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";
import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { orgDepartmentDirectory } from "@/constants/orgDepartmentDirectory";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
// Stores
import { useTokenStore } from "@/stores/token";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import { useRouteParamId } from "@/composables/useRouteParamId";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
// Services
import {
  MbsCrmPplHttpGetEmployeePipelineReqMdl,
  MbsCrmPplHttpGetEmployeePipelineRspMdl,
  MbsCrmPplHttpGetEmployeePipelineRspProductItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspSalerTrackingItemMdl,
  MbsCrmPplHttpGetEmployeePipelineRspBillItemMdl,
  MbsCrmPplHttpUpdatePipelineStatusReqMdl,
  MbsCrmPplHttpUpdatePipelineStatusRspMdl,
  MbsCrmPplHttpTransferPipelineToProjectReqMdl,
  MbsCrmPplHttpTransferPipelineToProjectRspMdl,
  MbsCrmPplHttpTransferPipelineToProjectReqItemMdl,
  MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl,
  MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl,
  MbsCrmPplHttpAddEmployeePipelineSalerTrackingReqMdl,
  MbsCrmPplHttpAddEmployeePipelineSalerTrackingRspMdl,
  MbsCrmPplHttpAddEmployeePipelineDueReqMdl,
  MbsCrmPplHttpAddEmployeePipelineDueRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineDueReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineDueRspMdl,
  MbsCrmPplHttpRemoveEmployeePipelineDueReqMdl,
  MbsCrmPplHttpRemoveEmployeePipelineDueRspMdl,
  MbsCrmPplHttpAddEmployeePipelineProductReqMdl,
  MbsCrmPplHttpAddEmployeePipelineProductRspMdl,
  MbsCrmPplHttpUpdateEmployeePipelineProductReqMdl,
  MbsCrmPplHttpUpdateEmployeePipelineProductRspMdl,
  MbsCrmPplHttpRemoveEmployeePipelineProductReqMdl,
  MbsCrmPplHttpRemoveEmployeePipelineProductRspMdl,
  MbsCrmPplHttpUpdateManyEmployeePipelineBillReqMdl,
  MbsCrmPplHttpUpdateManyEmployeePipelineBillReqItemMdl,
  MbsCrmPplHttpUpdateManyEmployeePipelineBillRspMdl,
  MbsCrmPplHttpNotifyBillIssueReqMdl,
  MbsCrmPplHttpNotifyBillIssueRspMdl,
  MbsCrmPplHttpConfirmBillIssueReqMdl,
  MbsCrmPplHttpConfirmBillIssueRspMdl,
} from "@/services/pms-http/crm/pipeline/crmPipelineHttpFormat";
import {
  getEmployeePipeline,
  updatePipelineStatus,
  transferPipelineToProject,
  handleEmployeePipelineSaler,
  addEmployeePipelineSalerTracking,
  addEmployeePipelineDue,
  updateEmployeePipelineDue,
  removeEmployeePipelineDue,
  addEmployeePipelineProduct,
  updateEmployeePipelineProduct,
  removeEmployeePipelineProduct,
  updateManyEmployeePipelineBill,
  notifyBillIssue,
  confirmBillIssue,
} from "@/services/pms-http/crm/pipeline/crmPipelineHttpService";
// Utils
import {
  formatToServerDatetime,
  formatToServerDateStartISO8,
  formatToServerDateEndISO8,
} from "@/utils/timeFormatter";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
// Components
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import ConfirmDialog from "@/components/global/feedback/ConfirmDialog.vue";
import PipelineBasicInfoSection from "./components/sections/PipelineBasicInfoSection.vue";
import PipelineCompanySection from "./components/sections/PipelineCompanySection.vue";
import PipelineContacterSection from "./components/sections/PipelineContacterSection.vue";
import PipelineTrackingSection from "./components/sections/PipelineTrackingSection.vue";
import PipelineProductSection from "./components/sections/PipelineProductSection.vue";
import PipelineBillSection from "./components/sections/PipelineBillSection.vue";
import PipelineDueSection from "./components/sections/PipelineDueSection.vue";
import PipelineSalerSection from "./components/sections/PipelineSalerSection.vue";
import TransferProjectModal from "./components/modals/TransferProjectModal.vue";
import RejectAssignmentModal from "./components/modals/RejectAssignmentModal.vue";
import ReassignSalerModal from "./components/modals/ReassignSalerModal.vue";
import EditMultipleBillsModal from "./components/modals/EditMultipleBillsModal.vue";
import ConfirmBillIssueModal from "./components/modals/ConfirmBillIssueModal.vue";
import PipelineProductModal from "./components/modals/PipelineProductModal.vue";
import PipelineDueModal from "./components/modals/PipelineDueModal.vue";
import TrackingModal from "./components/modals/TrackingModal.vue";
// Router
import router from "@/router";
//#endregion

//#region 外部依賴
/** 令牌儲存 */
const tokenStore = useTokenStore();
const employeeInfoStore = useEmployeeInfoStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
//#endregion

//#region 型別定義
/** CRM-商機管理-檢視名單-頁面模型 */
interface CrmPipelinePipelineDetailViewMdl {
  /** 資料庫-原子-商機-狀態-列舉 */
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  /** 商機-承辦業務員工ID */
  employeePipelineSalerEmployeeID: number | null;
  /** 商機-承辦業務員工名稱 */
  employeePipelineSalerEmployeeName: string | null;
  /** Booking code */
  bookingCode: string | null;
  /** 客戶資訊 */
  company: CrmPipelinePipelineDetailCompanyItemMdl;
  /** 窗口資訊列表 */
  contacterList: CrmPipelinePipelineDetailContacterItemMdl[];
  /** 尚未回覆業務紀錄 */
  pendingEmployeePipelineSaler: CrmPipelinePipelineDetailPendingEmployeePipelineSalerItemMdl | null;
  /** 業務紀錄列表 */
  employeePipelineSalerList: CrmPipelinePipelineDetailSalerItemMdl[];
  /** 業務商機開發紀錄列表 */
  employeePipelineSalerTrackingList: CrmPipelinePipelineDetailSalerTrackingItemMdl[];
  /** 商機產品列表 */
  employeePipelineProductList: CrmPipelinePipelineDetailProductItemMdl[];
  /** 履約期限列表 */
  employeePipelineDueList: CrmPipelinePipelineDetailDueItemMdl[];
  /** 發票紀錄列表 */
  employeePipelineBillList: CrmPipelinePipelineDetailBillItemMdl[];
  /** 承辦業務地區ID */
  employeePipelineSalerRegionID: number | null;
  /** 承辦業務地區名稱 */
  employeePipelineSalerRegionName: string | null;
  /** 承辦業務部門ID */
  employeePipelineSalerDepartmentID: number | null;
  /** 承辦業務部門名稱 */
  employeePipelineSalerDepartmentName: string | null;
}

/** CRM-商機管理-檢視名單-客戶資訊項目-模型 */
interface CrmPipelinePipelineDetailCompanyItemMdl {
  /** 公司統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 客戶公司ID */
  managerCompanyID: number | null;
  /** 客戶公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 公司營業地點ID */
  managerCompanyLocationID: number | null;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** CRM-商機管理-檢視名單-窗口資訊項目-模型 */
interface CrmPipelinePipelineDetailContacterItemMdl {
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口-是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 窗口姓名 */
  managerContacterName: string;
  /** 窗口Email */
  managerContacterEmail: string;
  /** 窗口手機 */
  managerContacterPhone: string;
  /** 窗口部門 */
  managerContacterDepartment: string;
  /** 窗口職稱 */
  managerContacterJobTitle: string;
  /** 窗口電話(市話) */
  managerContacterTelephone: string;
  /** 窗口是否個資同意 */
  managerContacterIsConsent: boolean;
  /** 窗口在職狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等ID */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  managerContacterRemark: string;
}

/** CRM-商機管理-檢視名單-指派業務紀錄項目-模型 */
interface CrmPipelinePipelineDetailPendingEmployeePipelineSalerItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number | null;
  /** 商機業務-業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum | null;
  /** 商機業務-建立員工名稱(指派人員) */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-是否有拒絕權限 */
  hasRejectPermissions: boolean;
  /** 商機業務-是否有接受權限 */
  hasAcceptPermissions: boolean;
  /** 商機業務-是否有轉指派權限 */
  hasReassignPermissions: boolean;
}

/** CRM-商機管理-檢視名單-業務項目-模型 */
interface CrmPipelinePipelineDetailSalerItemMdl {
  /** 商機業務ID */
  employeePipelineSalerID: number;
  /** 商機業務-業務員工名稱 */
  employeePipelineSalerEmployeeName: string;
  /** 商機業務-業務回覆時間 */
  employeePipelineSalerReplyTime: string | null;
  /** 商機業務-建立時間(指派時間) */
  employeePipelineSalerCreateTime: string;
  /** 商機業務-狀態 */
  employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum;
  /** 商機業務-建立員工名稱(指派人員) */
  employeePipelineSalerCreateEmployeeName: string;
  /** 商機業務-備註 */
  employeePipelineSalerRemark: string;
}

/** CRM-商機管理-檢視名單-業務商機開發紀錄項目-模型 */
interface CrmPipelinePipelineDetailSalerTrackingItemMdl {
  /** 商機業務開發紀錄ID */
  employeePipelineSalerTrackingID: number;
  /** 開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 窗口ID */
  managerContacterID: number;
  /** 窗口名稱 */
  managerContacterName: string;
  /** 備註 */
  employeePipelineSalerTrackingRemark: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  employeePipelineSalerTrackingCreateEmployeeName: string;
}

/** CRM-商機管理-檢視名單-商機產品項目-模型 */
interface CrmPipelinePipelineDetailProductItemMdl {
  /** 商機產品ID */
  employeePipelineProductID: number | null;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 管理者產品名稱 */
  managerProductName: string;
  /** 管理者產品主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品子分類名稱 */
  managerProductSubKindName: string;
  /** 管理者產品規格名稱 */
  managerProductSpecificationName: string;
  /** 商機產品售價 */
  employeePipelineProductSellPrice: number;
  /** 商機產品成交價 */
  employeePipelineProductClosingPrice: number;
  /** 商機產品成本 */
  employeePipelineProductCostPrice: number;
  /** 商機產品毛利 */
  employeePipelineProductGrossProfit: number;
  /** 商機產品數量 */
  employeePipelineProductCount: number;
  /** 商機產品新購/續約 */
  employeePipelineProductPurchaseKind: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品採購方式 */
  employeePipelineProductContractKind: DbAtomEmployeePipelineProductContractKindEnum;
  /** 業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 採購方式文字（當選擇「其他」時） */
  employeePipelineProductContractText?: string;
}

/** CRM-商機管理-檢視名單-履約期限項目-模型 */
interface CrmPipelinePipelineDetailDueItemMdl {
  /** 商機履約通知ID */
  employeePipelineDueID: number | null;
  /** 履約日期 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}

/** CRM-商機管理-檢視名單-發票記錄項目-模型 */
interface CrmPipelinePipelineDetailBillItemMdl {
  /** 商機發票紀錄ID */
  employeePipelineBillID: number | null;
  /** 發票號碼 */
  employeePipelineBillBillNumber: string | null;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string | null;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}

/** 專案成員項目-模型 */
interface CrmPipelinePipelineDetailProjectMemberItemMdl {
  employeeID: number | null;
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  managerRegionID: number | null;
  managerRegionName: string | null;
  managerDepartmentID: number | null;
  managerDepartmentName: string | null;
  employeeName: string | null;
  isBoss?: boolean; // 標記是否為總經理固定成員
}

/** 轉專案資料-模型 */
interface CrmPipelinePipelineDetailTransferProjectMdl {
  employeeProjectCode: string;
  employeeProjectName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
  employeeProjectContractUrl: string;
  employeeProjectWorkUrl: string;
  employeeProjectMemberEmployeeList: CrmPipelinePipelineDetailProjectMemberItemMdl[];
}

type StageStatusFieldKey =
  | "businessNeedStatus"
  | "businessTimelineStatus"
  | "businessBudgetStatus"
  | "businessPresentationStatus"
  | "businessQuotationStatus"
  | "businessNegotiationStatus";

interface StageStatusFormMdl {
  businessNeedStatus: DbEmployeePipelineStageCheckEnum | null;
  businessTimelineStatus: DbEmployeePipelineStageCheckEnum | null;
  businessBudgetStatus: DbEmployeePipelineStageCheckEnum | null;
  businessPresentationStatus: DbEmployeePipelineStageCheckEnum | null;
  businessQuotationStatus: DbEmployeePipelineStageCheckEnum | null;
  businessNegotiationStatus: DbEmployeePipelineStageCheckEnum | null;
  businessStageRemark: string;
}

interface StageStatusNoteMdl {
  businessNeedStatus: string;
  businessTimelineStatus: string;
  businessBudgetStatus: string;
  businessPresentationStatus: string;
  businessQuotationStatus: string;
  businessNegotiationStatus: string;
}

interface PocFormMdl {
  startDate: string;
  endDate: string;
  durationWeeks: string;
  result: "order" | "no_order" | "";
  rejectReason: string;
  rejectReasonNote: string;
  projectId: number | null;
}

interface StageStatusFieldDefinition {
  key: StageStatusFieldKey;
  label: string;
  description: string;
  requiredFor: DbAtomPipelineStatusEnum[];
}

//#endregion

//#region 頁面物件
/** 商機ID  */
const pipelineId = useRouteParamId("pipelineId");
/** 是否顯示【轉指派業務】彈跳視窗 */
const showReassignSalerModal = ref(false);
/** 是否顯示【拒絕指派】彈跳視窗 */
const showRejectAssignmentModal = ref(false);
/** CRM-商機管理-檢視名單-檢視-頁面物件 */
const crmPipelinePipelineDetailViewObj = reactive<CrmPipelinePipelineDetailViewMdl>({
  atomPipelineStatus: null,
  employeePipelineSalerEmployeeID: null,
  employeePipelineSalerEmployeeName: null,
  bookingCode: null,
  company: {
    managerCompanyUnifiedNumber: "",
    managerCompanyID: null,
    managerCompanyName: "",
    atomEmployeeRange: null,
    atomCustomerGrade: null,
    managerCompanyMainKindName: "",
    managerCompanySubKindName: "",
    atomCity: null,
    managerCompanyLocationID: null,
    managerCompanyLocationAddress: "",
    managerCompanyLocationTelephone: "",
    managerCompanyLocationStatus: null,
  },
  contacterList: [],
  pendingEmployeePipelineSaler: {
    employeePipelineSalerID: null,
    employeePipelineSalerEmployeeName: "",
    employeePipelineSalerCreateTime: "",
    employeePipelineSalerStatus: null,
    employeePipelineSalerCreateEmployeeName: "",
    hasRejectPermissions: false,
    hasAcceptPermissions: false,
    hasReassignPermissions: false,
  },
  employeePipelineSalerList: [],
  employeePipelineSalerTrackingList: [],
  employeePipelineProductList: [],
  employeePipelineDueList: [],
  employeePipelineBillList: [],
  employeePipelineSalerRegionID: null,
  employeePipelineSalerRegionName: null,
  employeePipelineSalerDepartmentID: null,
  employeePipelineSalerDepartmentName: null,
});

const stageStatusForm = reactive<StageStatusFormMdl>({
  businessNeedStatus: null,
  businessTimelineStatus: null,
  businessBudgetStatus: null,
  businessPresentationStatus: null,
  businessQuotationStatus: null,
  businessNegotiationStatus: null,
  businessStageRemark: "",
});

const stageStatusNotes = reactive<StageStatusNoteMdl>({
  businessNeedStatus: "",
  businessTimelineStatus: "",
  businessBudgetStatus: "",
  businessPresentationStatus: "",
  businessQuotationStatus: "",
  businessNegotiationStatus: "",
});
const presentationDetail = reactive({
  manager: "",
  date: "",
  timeSlot: "",
  summary: "",
});
const presentationTimeSlots = ["上午", "下午", "晚上"];
const lastUpdatedAt = ref<Date | null>(null);
const updateTrackingEnabled = ref(false);
const markUpdated = () => {
  if (!updateTrackingEnabled.value) return;
  lastUpdatedAt.value = new Date();
};
const lastUpdatedLabel = computed(() =>
  lastUpdatedAt.value
    ? lastUpdatedAt.value.toLocaleString("zh-TW", { hour12: false })
    : "尚未更新"
);

const pocForm = reactive<PocFormMdl>({
  startDate: "",
  endDate: "",
  result: "",
  rejectReason: "",
  rejectReasonNote: "",
  durationWeeks: "",
  projectId: null,
});
const pocAssignments = ref<
  {
    id: number;
    regionCode: string;
    departmentName: string;
    employeeName: string;
    progress: string;
    todo: string;
  }[]
>([]);
const getPocDepartmentOptions = (regionCode: string) => {
  if (!regionCode || regionCode === "X") return orgDepartmentDirectory;
  const regionPrefix = regionCode === "A" ? "北" : regionCode === "B" ? "中" : "南";
  const departments = orgMemberDirectory
    .filter((item) => item.regionName.startsWith(regionPrefix))
    .map((item) => item.departmentName);
  return Array.from(new Set(departments));
};
const getPocEmployeeOptions = (regionCode: string, departmentName: string) => {
  if (!departmentName) return [];
  const candidates = orgMemberDirectory.filter(
    (item) => item.departmentName === departmentName
  );
  const regionFiltered =
    regionCode && regionCode !== "X"
      ? candidates.filter((item) => item.regionName.startsWith(regionCode === "A" ? "北" : regionCode === "B" ? "中" : "南"))
      : candidates;
  return Array.from(new Set(regionFiltered.map((item) => item.name)));
};
const addPocAssignment = () => {
  pocAssignments.value = [
    {
    id: Date.now(),
    regionCode: "",
    departmentName: "",
    employeeName: "",
    progress: "",
    todo: "",
    },
  ];
};
const clearPocAssignments = () => {
  if (pocForm.projectId) {
    localStorage.removeItem(`cache.work.project.poc.${pocForm.projectId}`);
  }
  if (pipelineId.value) {
    localStorage.removeItem(`cache.crm.pipeline.poc.${pipelineId.value}`);
  }
  pocAssignments.value = [];
  pocForm.startDate = "";
  pocForm.endDate = "";
  pocForm.durationWeeks = "";
  pocForm.projectId = null;
  pocForm.result = "";
  pocForm.rejectReason = "";
  pocForm.rejectReasonNote = "";
  window.dispatchEvent(new CustomEvent("crm-poc-updated"));
  pocSavedFlag.value = false;
};
const handleUpdateBookingCode = (value: string) => {
  crmPipelinePipelineDetailViewObj.bookingCode = value;
  const detailKey = `cache.crm.pipeline.detail.${pipelineId}`;
  const detailRaw = localStorage.getItem(detailKey);
  if (!detailRaw) return;
  try {
    const detail = JSON.parse(detailRaw) as Record<string, unknown>;
    detail.bookingCode = value;
    localStorage.setItem(detailKey, JSON.stringify(detail));
  } catch (error) {
    console.warn("[CRM Pipeline] Failed to update booking code", error);
  }
  markUpdated();
};

watch(
  () => [stageStatusForm, stageStatusNotes, pocAssignments],
  () => {
    markUpdated();
  },
  { deep: true }
);

watch(
  () => presentationDetail,
  () => {
    stageStatusNotes.businessPresentationStatus = JSON.stringify({
      manager: presentationDetail.manager,
      date: presentationDetail.date,
      timeSlot: presentationDetail.timeSlot,
      summary: presentationDetail.summary,
    });
  },
  { deep: true }
);

watch(
  () => [pocForm, pocAssignments],
  () => {
    persistPocForm();
    markUpdated();
  },
  { deep: true }
);

const stageStatusFields: StageStatusFieldDefinition[] = [
  {
    key: "businessNeedStatus",
    label: "需求",
    description: "確認客戶有明確的合作需求與方向。",
    requiredFor: [
      DbAtomPipelineStatusEnum.Business10Percent,
      DbAtomPipelineStatusEnum.Business30Percent,
      DbAtomPipelineStatusEnum.Business70Percent,
    ],
  },
  {
    key: "businessTimelineStatus",
    label: "時程",
    description: "已了解客戶預計啟動與完成的時程安排。",
    requiredFor: [DbAtomPipelineStatusEnum.Business30Percent, DbAtomPipelineStatusEnum.Business70Percent],
  },
  {
    key: "businessBudgetStatus",
    label: "預算",
    description: "已確認客戶有對應預算並做好初步核准。",
    requiredFor: [DbAtomPipelineStatusEnum.Business30Percent, DbAtomPipelineStatusEnum.Business70Percent],
  },
  {
    key: "businessPresentationStatus",
    label: "簡報",
    description: "已安排簡報或說明會，並取得客戶初步回饋。",
    requiredFor: [
      DbAtomPipelineStatusEnum.Business30Percent,
      DbAtomPipelineStatusEnum.Business70Percent,
    ],
  },
  {
    key: "businessQuotationStatus",
    label: "報價",
    description: "已提供正式報價單或提案文件予客戶。",
    requiredFor: [DbAtomPipelineStatusEnum.Business70Percent],
  },
  {
    key: "businessNegotiationStatus",
    label: "議價",
    description: "目前與客戶進入議價或合約協商階段。",
    requiredFor: [DbAtomPipelineStatusEnum.Business70Percent],
  },
];

const confirmedLabelOverrides: Record<string, string> = {
  businessBudgetStatus: "有預算",
  businessPresentationStatus: "已簡報",
  businessQuotationStatus: "已報價",
  businessNegotiationStatus: "議價中",
};
const getStageOptionLabel = (fieldKey: string, optionLabel: string) => {
  if (optionLabel !== "已確認") return optionLabel;
  return confirmedLabelOverrides[fieldKey] ?? optionLabel;
};

const stageRuleSummaries = [
  {
    status: DbAtomPipelineStatusEnum.Business10Percent,
    title: "10% ＝ 有需求",
    description: "客戶明確提出需求方向，但時間與預算尚待確認。",
  },
  {
    status: DbAtomPipelineStatusEnum.Business30Percent,
    title: "30% ＝ 有需求、有時間、有預算、已簡報",
    description: "完成需求確認並掌握預算及時程，簡報也已完成。",
  },
  {
    status: DbAtomPipelineStatusEnum.Business70Percent,
    title: "70% ＝ 已簡報、已報價、議價中",
    description: "進入報價與議價階段，需同步記錄客戶期望與回饋。",
  },
];

const getFieldRequiredLabel = (field: StageStatusFieldDefinition): string => {
  if (!field.requiredFor.length) {
    return "選填";
  }
  const labels = field.requiredFor.map((status) => getPipelineStatusLabel(status));
  return `需達到：${labels.join(" / ")}`;
};

const isStageFieldConfirmed = (value: DbEmployeePipelineStageCheckEnum | null): boolean =>
  value === DbEmployeePipelineStageCheckEnum.Confirmed;

const autoPipelineStatus = computed(() => {
  const current = crmPipelinePipelineDetailViewObj.atomPipelineStatus;
  if (
    current === DbAtomPipelineStatusEnum.BusinessFailed ||
    current === DbAtomPipelineStatusEnum.TransferredToProject
  ) {
    return current;
  }
  const hasAnySelection = [
    stageStatusForm.businessNeedStatus,
    stageStatusForm.businessTimelineStatus,
    stageStatusForm.businessBudgetStatus,
    stageStatusForm.businessPresentationStatus,
    stageStatusForm.businessQuotationStatus,
    stageStatusForm.businessNegotiationStatus,
  ].some((value) => value !== null);
  if (!hasAnySelection) {
    return null;
  }
  const hasNeed = isStageFieldConfirmed(stageStatusForm.businessNeedStatus);
  const hasTimeline = isStageFieldConfirmed(stageStatusForm.businessTimelineStatus);
  const hasBudget = isStageFieldConfirmed(stageStatusForm.businessBudgetStatus);
  const hasPresentation = isStageFieldConfirmed(stageStatusForm.businessPresentationStatus);
  const hasQuotation = isStageFieldConfirmed(stageStatusForm.businessQuotationStatus);
  const hasNegotiation = isStageFieldConfirmed(stageStatusForm.businessNegotiationStatus);

  if (hasNeed && hasTimeline && hasBudget && hasPresentation && hasQuotation && hasNegotiation) {
    return DbAtomPipelineStatusEnum.Business70Percent;
  }
  if (hasNeed && hasTimeline && hasBudget && hasPresentation) {
    return DbAtomPipelineStatusEnum.Business30Percent;
  }
  if (hasNeed) {
    return DbAtomPipelineStatusEnum.Business10Percent;
  }
  return null;
});

const pipelineTab = ref<"potential" | "formal" | "customer">("potential");
const isPipelineLocked = computed(() =>
  [
    DbAtomPipelineStatusEnum.Business100Percent,
    DbAtomPipelineStatusEnum.TransferredToProject,
  ].includes(crmPipelinePipelineDetailViewObj.atomPipelineStatus ?? DbAtomPipelineStatusEnum.Undefined)
);

const isPotentialPipeline = computed(() =>
  [
    DbAtomPipelineStatusEnum.TransferredToBusiness,
    DbAtomPipelineStatusEnum.Business10Percent,
    DbAtomPipelineStatusEnum.Business30Percent,
    DbAtomPipelineStatusEnum.Business70Percent,
  ].includes(crmPipelinePipelineDetailViewObj.atomPipelineStatus ?? DbAtomPipelineStatusEnum.Undefined)
);

const syncPipelineTab = () => {
  if (pipelineTab.value === "customer") return;
  pipelineTab.value = isPotentialPipeline.value ? "potential" : "formal";
};

const shouldSyncAutoStatus = (nextStatus: DbAtomPipelineStatusEnum | null) => {
  if (!nextStatus) return false;
  if (
    crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
      DbAtomPipelineStatusEnum.TransferredToProject ||
    crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
      DbAtomPipelineStatusEnum.BusinessFailed
  ) {
    return false;
  }
  if (nextStatus === crmPipelinePipelineDetailViewObj.atomPipelineStatus) return false;
  return [
    DbAtomPipelineStatusEnum.Business10Percent,
    DbAtomPipelineStatusEnum.Business30Percent,
    DbAtomPipelineStatusEnum.Business70Percent,
  ].includes(nextStatus);
};

const validateStageStatusBeforeUpdate = (targetStatus: DbAtomPipelineStatusEnum | null): boolean => {
  if (!targetStatus) return false;
  if (targetStatus === DbAtomPipelineStatusEnum.Business10Percent) {
    if (!isStageFieldConfirmed(stageStatusForm.businessNeedStatus)) {
      displayError("商機10%需先將「需求確認」標記為「已確認」。");
      return false;
    }
    const hasTimelineConfirmed = isStageFieldConfirmed(stageStatusForm.businessTimelineStatus);
    const hasBudgetConfirmed = isStageFieldConfirmed(stageStatusForm.businessBudgetStatus);
    if (hasTimelineConfirmed || hasBudgetConfirmed) {
      displayError("商機10%需保持「時程確認」與「預算確認」為待確認狀態。");
      return false;
    }
  }
  const requiredFields = stageStatusFields.filter((field) =>
    field.requiredFor.includes(targetStatus)
  );
  const missingFields = requiredFields.filter(
    (field) => !isStageFieldConfirmed(stageStatusForm[field.key])
  );
  if (missingFields.length > 0) {
    displayError(
      `請先在「商機經營紀錄」中將「${missingFields
        .map((field) => field.label)
        .join("、")}」標記為「已確認」。`
    );
    return false;
  }
  return true;
};

const buildStageStatusRequestPayload = () => ({
  businessNeedStatus: stageStatusForm.businessNeedStatus,
  businessTimelineStatus: stageStatusForm.businessTimelineStatus,
  businessBudgetStatus: stageStatusForm.businessBudgetStatus,
  businessPresentationStatus: stageStatusForm.businessPresentationStatus,
  businessQuotationStatus: stageStatusForm.businessQuotationStatus,
  businessNegotiationStatus: stageStatusForm.businessNegotiationStatus,
  businessStageRemark: stageStatusForm.businessStageRemark || null,
});

const pocReasons = [
  "工程師技術不到位",
  "業務溝通問題",
  "產品功能不符合需求",
  "其他",
];

const isDepartmentManager = computed(() => {
  const role = employeeInfoStore.effectiveRoleName || "";
  return (
    role.includes("各部門經理") ||
    role.includes("各處處長") ||
    role.includes("Admin")
  );
});

const goToPocProject = () => {
  if (!pocForm.projectId) return;
  router.push(`/work/project/detail/${pocForm.projectId}?tab=poc&pipelineId=${pipelineId.value}`);
};

const savePoc = () => {
  persistPocForm();
  if (pocForm.projectId) {
    persistPocToProject();
  }
  displaySuccess("POC 已儲存。");
  window.dispatchEvent(new CustomEvent("crm-poc-updated"));
  pocSavedFlag.value = true;
};

const cancelPocDraft = () => {
  clearPocAssignments();
};

const pocSavedFlag = ref(false);
const isPocSaved = computed(() => pocSavedFlag.value);

const shouldShowPocDraft = computed(
  () => pocAssignments.value.length > 0 && !isPocSaved.value
);

const persistStageNotes = () => {
  if (!pipelineId.value) return;
  localStorage.setItem(
    `cache.crm.pipeline.stageNotes.${pipelineId.value}`,
    JSON.stringify(stageStatusNotes)
  );
};

const getPipelineProjectLinkKey = () => `cache.crm.pipeline.projectLink.${pipelineId.value}`;

const persistPocForm = () => {
  if (!pipelineId.value) return;
  localStorage.setItem(
    `cache.crm.pipeline.poc.${pipelineId.value}`,
    JSON.stringify({
      form: pocForm,
      assignments: pocAssignments.value,
    })
  );
};

const persistPocToProject = () => {
  if (!pipelineId.value) return;
  if (!pocForm.projectId) return;
  localStorage.setItem(
    `cache.work.project.poc.${pocForm.projectId}`,
    JSON.stringify({
      pipelineId: pipelineId.value,
      form: pocForm,
      assignments: pocAssignments.value,
    })
  );
};

const hydrateStageNotes = () => {
  if (!pipelineId.value) return;
  const raw = localStorage.getItem(`cache.crm.pipeline.stageNotes.${pipelineId.value}`);
  if (!raw) return;
  try {
    Object.assign(stageStatusNotes, JSON.parse(raw));
    const presentationRaw = stageStatusNotes.businessPresentationStatus;
    if (presentationRaw) {
      try {
        const parsed = JSON.parse(presentationRaw);
        if (parsed && typeof parsed === "object") {
          presentationDetail.manager = parsed.manager || "";
          presentationDetail.date = parsed.date || "";
          presentationDetail.timeSlot = parsed.timeSlot || "";
          presentationDetail.summary = parsed.summary || "";
        } else {
          presentationDetail.summary = presentationRaw;
        }
      } catch {
        presentationDetail.summary = presentationRaw;
      }
    }
  } catch {
    return;
  }
};

const hydratePocForm = () => {
  if (!pipelineId.value) return;
  const linkedRaw = localStorage.getItem(getPipelineProjectLinkKey());
  const linkedId = linkedRaw ? Number(linkedRaw) : null;
  if (linkedId && !pocForm.projectId) {
    pocForm.projectId = linkedId;
  }
  const raw = localStorage.getItem(`cache.crm.pipeline.poc.${pipelineId.value}`);
  if (!raw) return;
  try {
    const parsed = JSON.parse(raw);
    if (parsed?.form) {
      Object.assign(pocForm, parsed.form);
    }
    if (Array.isArray(parsed?.assignments)) {
      pocAssignments.value = parsed.assignments;
    }
    pocSavedFlag.value = true;
  } catch {
    return;
  }
};

//#endregion

//#region 確認彈跳視窗狀態與方法
/** 確認彈跳視窗狀態模型 */
interface ConfirmDialogState {
  show: boolean;
  title: string;
  message: string;
  onConfirm: (() => void | Promise<void>) | null;
}

/** 確認彈跳視窗狀態物件 */
const confirmDialog = reactive<ConfirmDialogState>({
  show: false,
  title: "",
  message: "",
  onConfirm: null,
});

/** 顯示確認彈跳視窗 */
const showConfirmDialog = (options: {
  title: string;
  message: string;
  onConfirm: () => void | Promise<void>;
}) => {
  confirmDialog.title = options.title;
  confirmDialog.message = options.message;
  confirmDialog.onConfirm = options.onConfirm;
  confirmDialog.show = true;
};

/** 處理確認彈跳視窗確認 */
const handleConfirmDialog = async () => {
  confirmDialog.show = false;
  if (confirmDialog.onConfirm) {
    await confirmDialog.onConfirm();
  }
  // 重置
  confirmDialog.onConfirm = null;
};

/** 處理確認彈跳視窗取消 */
const handleCancelDialog = () => {
  confirmDialog.show = false;
  confirmDialog.onConfirm = null;
};

//#endregion

//#region 計算屬性 & 監聽器
/** 計算產品總成交價 */
const totalProductClosingPrice = computed(() => {
  return crmPipelinePipelineDetailViewObj.employeePipelineProductList.reduce(
    (sum, product) =>
      sum + product.employeePipelineProductClosingPrice * product.employeePipelineProductCount,
    0
  );
});

/** 計算發票分期期數 */
const billPeriodCount = computed(() => {
  const bills = crmPipelinePipelineDetailViewObj.employeePipelineBillList;
  if (bills.length === 0) return 0;
  return Math.max(...bills.map((bill) => bill.employeePipelineBillPeriodNumber));
});

/** 判斷是否顯示發票記錄區塊 */
const shouldShowBillSection = computed(() => {
  const status = crmPipelinePipelineDetailViewObj.atomPipelineStatus;
  return (
    status === DbAtomPipelineStatusEnum.Business10Percent ||
    status === DbAtomPipelineStatusEnum.Business30Percent ||
    status === DbAtomPipelineStatusEnum.Business70Percent ||
    status === DbAtomPipelineStatusEnum.Business100Percent ||
    status === DbAtomPipelineStatusEnum.TransferredToProject ||
    status === DbAtomPipelineStatusEnum.BusinessFailed
  );
});

/** 是否顯示【履約期限通知】區塊 */
const showDueSection = computed(() => {
  const status = crmPipelinePipelineDetailViewObj.atomPipelineStatus;
  return (
    status === DbAtomPipelineStatusEnum.Business10Percent ||
    status === DbAtomPipelineStatusEnum.Business30Percent ||
    status === DbAtomPipelineStatusEnum.Business70Percent ||
    status === DbAtomPipelineStatusEnum.Business100Percent ||
    status === DbAtomPipelineStatusEnum.TransferredToProject ||
    status === DbAtomPipelineStatusEnum.BusinessFailed
  );
});

//#endregion

//#region 資料操作
/** 取得資料 */
const getData = async () => {
  if (!requireToken()) return;

  if (!pipelineId || isNaN(pipelineId) || pipelineId <= 0) {
    displayError("找不到此商機");
    router.push("/crm/pipeline/pipeline");
  }

  const requestData: MbsCrmPplHttpGetEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId!,
  };

  const responseData: MbsCrmPplHttpGetEmployeePipelineRspMdl | null =
    await getEmployeePipeline(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  crmPipelinePipelineDetailViewObj.atomPipelineStatus = responseData.atomPipelineStatus;
  crmPipelinePipelineDetailViewObj.employeePipelineSalerEmployeeID =
    responseData.employeePipelineSalerEmployeeID;
  crmPipelinePipelineDetailViewObj.employeePipelineSalerEmployeeName =
    responseData.employeePipelineSalerEmployeeName;
  crmPipelinePipelineDetailViewObj.company = responseData.company;
  const localDetailRaw = localStorage.getItem(`cache.crm.pipeline.detail.${pipelineId}`);
  if (localDetailRaw) {
    try {
      const localDetail = JSON.parse(localDetailRaw) as { bookingCode?: string | null };
      crmPipelinePipelineDetailViewObj.bookingCode = localDetail.bookingCode ?? null;
    } catch (error) {
      console.warn("[CRM Pipeline] Failed to parse local booking code", error);
    }
  }
  crmPipelinePipelineDetailViewObj.contacterList = responseData.contacterList;
  crmPipelinePipelineDetailViewObj.pendingEmployeePipelineSaler =
    responseData.pendingEmployeePipelineSaler;

  crmPipelinePipelineDetailViewObj.employeePipelineSalerList =
    responseData.employeePipelineSalerList;
  // 包裝資料
  crmPipelinePipelineDetailViewObj.employeePipelineProductList =
    responseData.employeePipelineProductList?.map(
      (item: MbsCrmPplHttpGetEmployeePipelineRspProductItemMdl) =>
        ({
          employeePipelineProductID: item.employeePipelineProductID,
          managerProductName: item.managerProductName,
          managerProductID: item.managerProductID,
          managerProductSpecificationID: item.managerProductSpecificationID,
          managerProductMainKindName: item.managerProductMainKindName,
          managerProductSubKindName: item.managerProductSubKindName,
          managerProductSpecificationName: item.managerProductSpecificationName,
          employeePipelineProductSellPrice: item.employeePipelineProductSellPrice,
          employeePipelineProductClosingPrice: item.employeePipelineProductClosingPrice,
          employeePipelineProductCostPrice: item.employeePipelineProductCostPrice,
          employeePipelineProductCount: item.employeePipelineProductCount,
          employeePipelineProductPurchaseKind: item.employeePipelineProductPurchaseKind,
          employeePipelineProductContractKind: item.employeePipelineProductContractKind,
          managerProductMainKindCommissionRate: item.managerProductMainKindCommissionRate,
          employeePipelineProductContractText: item.employeePipelineProductContractText,
          employeePipelineProductGrossProfit:
            item.employeePipelineProductClosingPrice - item.employeePipelineProductCostPrice,
        }) satisfies CrmPipelinePipelineDetailProductItemMdl
    ) ?? [];
  crmPipelinePipelineDetailViewObj.employeePipelineSalerTrackingList =
    responseData.employeePipelineSalerTrackingList?.map(
      (item: MbsCrmPplHttpGetEmployeePipelineRspSalerTrackingItemMdl) =>
        ({
          employeePipelineSalerTrackingID: item.employeePipelineSalerTrackingID,
          employeePipelineSalerTrackingTime: item.employeePipelineSalerTrackingTime,
          managerContacterID: 0,
          managerContacterName: item.managerContacterName,
          employeePipelineSalerTrackingRemark: item.employeePipelineSalerTrackingRemark,
          employeePipelineSalerTrackingCreateEmployeeName:
            item.employeePipelineSalerTrackingCreateEmployeeName,
        }) satisfies CrmPipelinePipelineDetailSalerTrackingItemMdl
    ) ?? [];

  crmPipelinePipelineDetailViewObj.employeePipelineBillList =
    responseData.employeePipelineBillList?.map(
      (item: MbsCrmPplHttpGetEmployeePipelineRspBillItemMdl) =>
        ({
          employeePipelineBillID: item.employeePipelineBillID,
          employeePipelineBillPeriodNumber: item.employeePipelineBillPeriodNumber,
          employeePipelineBillBillTime: item.employeePipelineBillBillTime,
          employeePipelineBillBillNumber: item.employeePipelineBillBillNumber,
          employeePipelineBillNoTaxAmount: item.employeePipelineBillNoTaxAmount,
          employeePipelineBillRemark: item.employeePipelineBillRemark,
          employeePipelineBillStatus: item.employeePipelineBillStatus,
        }) satisfies CrmPipelinePipelineDetailBillItemMdl
    ) ?? [];
  crmPipelinePipelineDetailViewObj.employeePipelineBillList = responseData.employeePipelineBillList;
  crmPipelinePipelineDetailViewObj.employeePipelineDueList = responseData.employeePipelineDueList;
  crmPipelinePipelineDetailViewObj.employeePipelineSalerRegionID =
    responseData.employeePipelineSalerRegionID;
  crmPipelinePipelineDetailViewObj.employeePipelineSalerRegionName =
    responseData.employeePipelineSalerRegionName;
  crmPipelinePipelineDetailViewObj.employeePipelineSalerDepartmentID =
    responseData.employeePipelineSalerDepartmentID;
  crmPipelinePipelineDetailViewObj.employeePipelineSalerDepartmentName =
    responseData.employeePipelineSalerDepartmentName;

  stageStatusForm.businessNeedStatus = responseData.stageStatus?.businessNeedStatus ?? null;
  stageStatusForm.businessTimelineStatus = responseData.stageStatus?.businessTimelineStatus ?? null;
  stageStatusForm.businessBudgetStatus = responseData.stageStatus?.businessBudgetStatus ?? null;
  stageStatusForm.businessPresentationStatus =
    responseData.stageStatus?.businessPresentationStatus ?? null;
  stageStatusForm.businessQuotationStatus =
    responseData.stageStatus?.businessQuotationStatus ?? null;
  stageStatusForm.businessNegotiationStatus =
    responseData.stageStatus?.businessNegotiationStatus ?? null;
  stageStatusForm.businessStageRemark = responseData.stageStatus?.businessStageRemark || "";
  updateTrackingEnabled.value = true;
};

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/crm/pipeline/pipeline");
};

//#endregion

//#region 【指派業務記錄】區塊事件處理
/** 顯示[拒絕指派]彈跳視窗 */
const handleRejectAssignment = () => {
  showRejectAssignmentModal.value = true;
};

/** 顯示[轉指派業務]彈跳視窗 */
const handleReassignSaler = () => {
  showReassignSalerModal.value = true;
};

/** 處理[拒絕指派]確認 */
const handleRejectAssignmentConfirm = async (rejectReason: string) => {
  if (
    !requireToken() ||
    !crmPipelinePipelineDetailViewObj.pendingEmployeePipelineSaler?.employeePipelineSalerID
  ) {
    return;
  }

  const requestData: MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId!,
    employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum.Rejected,
    employeePipelineSalerRemark: rejectReason,
    employeePipelineSalerEmployeeID: null, // 拒絕指派不需要轉派
  };

  const responseData: MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl | null =
    await handleEmployeePipelineSaler(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showRejectAssignmentModal.value = false;

    showSuccess.value = true;
    successMessage.value = "已拒絕此商機，將返回商機管理列表頁";

    router.push("/crm/pipeline/pipeline");
  }
};

/** 處理[接受指派] */
const handleAcceptAssignment = async () => {
  if (
    !requireToken() ||
    !crmPipelinePipelineDetailViewObj.pendingEmployeePipelineSaler?.employeePipelineSalerID
  ) {
    return;
  }

  const requestData: MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId!,
    employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum.Accepted,
    employeePipelineSalerRemark: "", // 接受指派不需要備註
    employeePipelineSalerEmployeeID: null, // 接受指派不需要轉派
  };

  const responseData: MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl | null =
    await handleEmployeePipelineSaler(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showSuccess.value = true;
    successMessage.value = "已接受指派!";

    await getData();
  }
};

/** 處理[拒絕指派]取消 */
const handleRejectAssignmentCancel = () => {
  showRejectAssignmentModal.value = false;
};

/** 處理[轉指派業務]確認 */
const handleReassignSalerConfirm = async (data: { employeeID: number; employeeName: string }) => {
  if (!requireToken()) {
    return;
  }

  const requestData: MbsCrmPplHttpHandleEmployeePipelineSalerReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId!,
    employeePipelineSalerStatus: DbAtomEmployeePipelineSalerStatusEnum.Reassigned,
    employeePipelineSalerRemark: null,
    employeePipelineSalerEmployeeID: data.employeeID,
  };

  const responseData: MbsCrmPplHttpHandleEmployeePipelineSalerRspMdl | null =
    await handleEmployeePipelineSaler(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showReassignSalerModal.value = false;
    await getData();
  }
};

/** 處理[轉指派業務]取消 */
const handleReassignSalerCancel = () => {
  showReassignSalerModal.value = false;
};
//#endregion

//#region 【基本資訊】區塊事件處理
/** 是否顯示【新增專案】彈跳視窗 */
const showTransferProjectModal = ref(false);

/** 處理商機狀態更新 */
const handleBasicInfoUpdateStatus = () => {
  const currentStatus = crmPipelinePipelineDetailViewObj.atomPipelineStatus;
  const nextStatus = getNextPipelineStatus(currentStatus);
  const nextStatusText = getPipelineButtonText(currentStatus);

  if (!nextStatus || !nextStatusText) return;
  if (!validateStageStatusBeforeUpdate(nextStatus)) return;

  showConfirmDialog({
    title: "確認商機狀態變更",
    message: `確定要將商機狀態更新為「${nextStatusText}」嗎？`,
    onConfirm: () => handleUpdatePipelineStatus(nextStatus),
  });
};

/** 顯示[新增專案]彈跳視窗 */
const handleBasicInfoTransferProject = () => {
  showTransferProjectModal.value = true;
};

/** 處理[商機失敗]按鈕 */
const handleBasicInfoFailPipeline = () => {
  showConfirmDialog({
    title: "確認商機失敗",
    message: "確定要將此商機標記為「商機失敗」嗎？此操作將結束商機流程。",
    onConfirm: handleFailPipeline,
  });
};

/** 處理基本資訊區塊的轉指派業務 */
const handleBasicInfoReassignSaler = () => {
  showReassignSalerModal.value = true;
};

/** 處理[新增專案]確認 */
const handleTransferProjectDialogConfirm = async (
  formData: CrmPipelinePipelineDetailTransferProjectMdl
) => {
  if (!requireToken() || !pipelineId) return;

  // 過濾掉沒有選擇員工的成員，只送出實際選擇的成員
  const validMembers = formData.employeeProjectMemberEmployeeList
    .filter((member) => member.employeeID)
    .map(
      (member) =>
        ({
          employeeID: member.employeeID!,
          employeeProjectMemberRole: member.employeeProjectMemberRole,
        }) as MbsCrmPplHttpTransferPipelineToProjectReqItemMdl
    );

  const requestData: MbsCrmPplHttpTransferPipelineToProjectReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId,
    employeeProjectCode: formData.employeeProjectCode,
    employeeProjectName: formData.employeeProjectName,
    employeeProjectStartTime: formatToServerDateStartISO8(formData.employeeProjectStartTime),
    employeeProjectEndTime: formatToServerDateEndISO8(formData.employeeProjectEndTime),
    employeeProjectContractUrl: formData.employeeProjectContractUrl,
    employeeProjectWorkUrl: formData.employeeProjectWorkUrl,
    employeeProjectMemberEmployeeList: validMembers,
  };

  const responseData: MbsCrmPplHttpTransferPipelineToProjectRspMdl | null =
    await transferPipelineToProject(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    // 更新本地狀態為已轉專案
    crmPipelinePipelineDetailViewObj.atomPipelineStatus =
      DbAtomPipelineStatusEnum.TransferredToProject;
    showTransferProjectModal.value = false;
    if (responseData.employeeProjectID) {
      pocForm.projectId = responseData.employeeProjectID;
      localStorage.setItem(getPipelineProjectLinkKey(), String(responseData.employeeProjectID));
    }

    // 顯示成功提示
    displaySuccess(`專案「${formData.employeeProjectName}」已成功建立！`);
  }
};

/** 取得商機按鈕文字 - 需要在主頁面中使用 */
const getPipelineButtonText = (currentStatus: DbAtomPipelineStatusEnum | null): string => {
  if (!currentStatus) return "";

  switch (currentStatus) {
    case DbAtomPipelineStatusEnum.TransferredToBusiness:
      return "商機10%";
    case DbAtomPipelineStatusEnum.Business10Percent:
      return "商機30%";
    case DbAtomPipelineStatusEnum.Business30Percent:
      return "商機70%";
    case DbAtomPipelineStatusEnum.Business70Percent:
      return "商機100%";
    case DbAtomPipelineStatusEnum.Business100Percent:
      return "轉成專案";
    default:
      return "";
  }
};

/** 取得下一個商機狀態 */
const getNextPipelineStatus = (
  currentStatus: DbAtomPipelineStatusEnum | null
): DbAtomPipelineStatusEnum | null => {
  if (!currentStatus) return null;

  switch (currentStatus) {
    case DbAtomPipelineStatusEnum.TransferredToBusiness:
      return DbAtomPipelineStatusEnum.Business10Percent;
    case DbAtomPipelineStatusEnum.Business10Percent:
      return DbAtomPipelineStatusEnum.Business30Percent;
    case DbAtomPipelineStatusEnum.Business30Percent:
      return DbAtomPipelineStatusEnum.Business70Percent;
    case DbAtomPipelineStatusEnum.Business70Percent:
      return DbAtomPipelineStatusEnum.Business100Percent;
    default:
      return null;
  }
};

/** 處理商機狀態更新 */
const handleUpdatePipelineStatus = async (targetStatus?: DbAtomPipelineStatusEnum) => {
  if (!requireToken() || !pipelineId) return;

  const currentStatus = crmPipelinePipelineDetailViewObj.atomPipelineStatus;
  const nextStatus = targetStatus ?? getNextPipelineStatus(currentStatus);

  if (!nextStatus) {
    showError.value = true;
    errorMessage.value = "無法更新商機狀態";
    return;
  }

  const stagePayload = buildStageStatusRequestPayload();

  const requestData: MbsCrmPplHttpUpdatePipelineStatusReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId,
    atomPipelineStatus: nextStatus,
    ...stagePayload,
  };

  const responseData: MbsCrmPplHttpUpdatePipelineStatusRspMdl | null =
    await updatePipelineStatus(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    // 更新本地狀態
    crmPipelinePipelineDetailViewObj.atomPipelineStatus = nextStatus;
    if (nextStatus === DbAtomPipelineStatusEnum.Business100Percent) {
      showTransferProjectModal.value = true;
    }
  }
};

const syncAutoPipelineStatus = async (nextStatus: DbAtomPipelineStatusEnum) => {
  if (!requireToken() || !pipelineId) return;
  const stagePayload = buildStageStatusRequestPayload();
  const requestData: MbsCrmPplHttpUpdatePipelineStatusReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId,
    atomPipelineStatus: nextStatus,
    ...stagePayload,
  };
  const responseData: MbsCrmPplHttpUpdatePipelineStatusRspMdl | null =
    await updatePipelineStatus(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (isSuccess) {
    crmPipelinePipelineDetailViewObj.atomPipelineStatus = nextStatus;
  }
};

/** 處理商機失敗 */
const handleFailPipeline = async () => {
  if (!requireToken() || !pipelineId) return;

  const stagePayload = buildStageStatusRequestPayload();

  const requestData: MbsCrmPplHttpUpdatePipelineStatusReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId,
    atomPipelineStatus: DbAtomPipelineStatusEnum.BusinessFailed,
    ...stagePayload,
  };

  const responseData: MbsCrmPplHttpUpdatePipelineStatusRspMdl | null =
    await updatePipelineStatus(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 更新本地狀態
  crmPipelinePipelineDetailViewObj.atomPipelineStatus = DbAtomPipelineStatusEnum.BusinessFailed;

  // 顯示成功訊息
  displaySuccess("商機已標記為失敗");
};

//#endregion

//#region 【商機開發記錄】區塊事件處理
/** 是否顯示【商機開發記錄】彈跳視窗 */
const showTrackingModal = ref(false);

/** 處理新增開發記錄 */
const handleAddTracking = () => {
  showTrackingModal.value = true;
};

/** 處理新增開發記錄確認 */
const handleTrackingModalConfirm = async (data: {
  employeePipelineSalerTrackingTime: string;
  managerContacterID: number;
  employeePipelineSalerTrackingRemark: string;
}) => {
  if (!requireToken() || !pipelineId) {
    return;
  }

  const requestData: MbsCrmPplHttpAddEmployeePipelineSalerTrackingReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId,
    employeePipelineSalerTrackingTime: formatToServerDatetime(
      data.employeePipelineSalerTrackingTime
    ),
    managerContacterID: data.managerContacterID,
    employeePipelineSalerTrackingRemark: data.employeePipelineSalerTrackingRemark,
  };

  const responseData: MbsCrmPplHttpAddEmployeePipelineSalerTrackingRspMdl | null =
    await addEmployeePipelineSalerTracking(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showTrackingModal.value = false;
    // 重新載入資料以更新列表
    await getData();
  }
};

/** 處理新增開發記錄取消 */
const handleTrackingModalCancel = () => {
  showTrackingModal.value = false;
};
//#endregion

//#region 【產品】區塊事件處理
/** 是否顯示【新增產品】彈跳視窗 */
const showProductModal = ref(false);
/** 當前編輯的產品 */
const currentEditProduct = ref<CrmPipelinePipelineDetailProductItemMdl | null>(null);
/** 當前刪除的產品 ID */
const currentDeleteProductId = ref<number | null>(null);

/** 處理新增產品 */
const handleAddProduct = () => {
  currentEditProduct.value = null;
  showProductModal.value = true;
};

/** 處理編輯產品 */
const handleEditProduct = (product: CrmPipelinePipelineDetailProductItemMdl) => {
  currentEditProduct.value = { ...product };
  showProductModal.value = true;
};

/** 處理刪除產品 */
const handleDeleteProduct = (productId: number | null) => {
  if (!productId) {
    showError.value = true;
    errorMessage.value = "無效的產品ID";
    return;
  }
  currentDeleteProductId.value = productId;
  showConfirmDialog({
    title: "確認刪除",
    message: "確定要刪除此產品嗎？",
    onConfirm: async () => {
      await handleDeleteProductConfirm();
    },
  });
};

/** 處理產品彈跳視窗確認 */
const handleProductModalConfirm = async (data: {
  managerProductID: number;
  managerProductSpecificationID: number;
  employeePipelineProductSellPrice: number;
  employeePipelineProductClosingPrice: number;
  employeePipelineProductCostPrice: number;
  employeePipelineProductCount: number;
  employeePipelineProductPurchaseKindID: DbAtomEmployeePipelineProductPurchaseKindEnum;
  employeePipelineProductContractKindID: DbAtomEmployeePipelineProductContractKindEnum;
  employeePipelineProductContractText: string;
}) => {
  if (!requireToken() || !pipelineId) {
    return;
  }

  // 判斷是編輯模式還是新增模式
  const isEditMode = currentEditProduct.value?.employeePipelineProductID;

  if (isEditMode) {
    // 編輯模式：更新現有產品
    const requestData: MbsCrmPplHttpUpdateEmployeePipelineProductReqMdl = {
      employeeLoginToken: tokenStore.token!,
      employeePipelineProductID: currentEditProduct.value!.employeePipelineProductID!,
      managerProductID: data.managerProductID,
      managerProductSpecificationID: data.managerProductSpecificationID,
      employeePipelineProductSellPrice: data.employeePipelineProductSellPrice,
      employeePipelineProductClosingPrice: data.employeePipelineProductClosingPrice,
      employeePipelineProductCostPrice: data.employeePipelineProductCostPrice,
      employeePipelineProductCount: data.employeePipelineProductCount,
      employeePipelineProductPurchaseKindID: data.employeePipelineProductPurchaseKindID,
      employeePipelineProductContractKindID: data.employeePipelineProductContractKindID,
      employeePipelineProductContractText: data.employeePipelineProductContractText,
    };

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineProductRspMdl | null =
      await updateEmployeePipelineProduct(requestData);

    if (!responseData) {
      showError.value = true;
      errorMessage.value = "系統錯誤，請稍後再試";
      return;
    }

    const isSuccess = handleErrorCode(responseData.errorCode);

    if (isSuccess) {
      showProductModal.value = false;
      currentEditProduct.value = null;
      await getData();
    }
  } else {
    // 新增模式：新增產品
    const requestData: MbsCrmPplHttpAddEmployeePipelineProductReqMdl = {
      employeeLoginToken: tokenStore.token!,
      employeePipelineID: pipelineId,
      managerProductID: data.managerProductID,
      managerProductSpecificationID: data.managerProductSpecificationID,
      employeePipelineProductSellPrice: data.employeePipelineProductSellPrice,
      employeePipelineProductClosingPrice: data.employeePipelineProductClosingPrice,
      employeePipelineProductCostPrice: data.employeePipelineProductCostPrice,
      employeePipelineProductCount: data.employeePipelineProductCount,
      employeePipelineProductPurchaseKindID: data.employeePipelineProductPurchaseKindID,
      employeePipelineProductContractKindID: data.employeePipelineProductContractKindID,
      employeePipelineProductContractText: data.employeePipelineProductContractText,
    };

    const responseData: MbsCrmPplHttpAddEmployeePipelineProductRspMdl | null =
      await addEmployeePipelineProduct(requestData);

    if (!responseData) {
      showError.value = true;
      errorMessage.value = "系統錯誤，請稍後再試";
      return;
    }

    const isSuccess = handleErrorCode(responseData.errorCode);

    if (isSuccess) {
      showProductModal.value = false;
      currentEditProduct.value = null;
      await getData();
    }
  }
};

/** 處理產品彈跳視窗取消 */
const handleProductModalCancel = () => {
  showProductModal.value = false;
  currentEditProduct.value = null;
};

/** 處理刪除產品確認 */
const handleDeleteProductConfirm = async () => {
  if (!requireToken() || !currentDeleteProductId.value) {
    return;
  }

  const requestData: MbsCrmPplHttpRemoveEmployeePipelineProductReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineProductID: currentDeleteProductId.value,
  };

  const responseData: MbsCrmPplHttpRemoveEmployeePipelineProductRspMdl | null =
    await removeEmployeePipelineProduct(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    currentDeleteProductId.value = null;
    await getData();
  }
};

//#endregion

//#region 【發票記錄】區塊事件處理
/** 是否顯示【編輯多筆發票記錄】彈跳視窗 */
const showEditMultipleBillsModal = ref(false);
/** 是否顯示【確認開立發票】彈跳視窗 */
const showConfirmBillIssueModal = ref(false);
/** 當前處理的發票 */
const currentBill = ref<CrmPipelinePipelineDetailBillItemMdl | null>(null);

/** 處理編輯多筆發票記錄 */
const handleEditMultipleBills = () => {
  showEditMultipleBillsModal.value = true;
};

/** 處理編輯多筆發票記錄確認 */
const handleEditMultipleBillsConfirm = async (data: {
  periodCount: number;
  billList: CrmPipelinePipelineDetailBillItemMdl[];
}) => {
  if (!requireToken() || !pipelineId) {
    return;
  }

  const requestData: MbsCrmPplHttpUpdateManyEmployeePipelineBillReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: pipelineId,
    employeePipelineBillList: data.billList.map(
      (bill) =>
        ({
          employeePipelineBillPeriodNumber: bill.employeePipelineBillPeriodNumber,
          employeePipelineBillBillTime: formatToServerDateStartISO8(
            bill.employeePipelineBillBillTime
          ),
          employeePipelineBillNoTaxAmount: bill.employeePipelineBillNoTaxAmount,
          employeePipelineBillRemark: bill.employeePipelineBillRemark ?? null,
        }) satisfies MbsCrmPplHttpUpdateManyEmployeePipelineBillReqItemMdl
    ),
  };

  const responseData: MbsCrmPplHttpUpdateManyEmployeePipelineBillRspMdl | null =
    await updateManyEmployeePipelineBill(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showEditMultipleBillsModal.value = false;
    await getData();
  }
};

/** 處理編輯多筆發票記錄取消 */
const handleEditMultipleBillsCancel = () => {
  showEditMultipleBillsModal.value = false;
};

/** 處理通知開立發票 */
const handleNotifyBillIssue = (bill: CrmPipelinePipelineDetailBillItemMdl) => {
  if (bill.employeePipelineBillStatus !== DbAtomEmployeePipelineBillStatusEnum.NotCompleted) {
    showError.value = true;
    errorMessage.value = "僅可通知開立狀態為「待通知開立」的發票";
    return;
  }

  if (!bill.employeePipelineBillID) {
    showError.value = true;
    errorMessage.value = "無效的發票ID";
    return;
  }

  showConfirmDialog({
    title: "確認通知開立發票",
    message: "確定要通知開立發票嗎？",
    onConfirm: async () => {
      await handleNotifyBillConfirm(bill.employeePipelineBillID);
    },
  });
};

/** 處理通知開立發票確認 */
const handleNotifyBillConfirm = async (billID: number | null) => {
  const requestData: MbsCrmPplHttpNotifyBillIssueReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineBillID: billID!,
  };

  const responseData: MbsCrmPplHttpNotifyBillIssueRspMdl | null =
    await notifyBillIssue(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showConfirmBillIssueModal.value = false;
    showSuccess.value = true;
    successMessage.value = "通知開立發票成功";
    await getData();
  }
};

/** 處理確認開立發票 */
const handleConfirmBillIssue = (bill: CrmPipelinePipelineDetailBillItemMdl) => {
  showConfirmBillIssueModal.value = true;
  currentBill.value = { ...bill };
};

/** 處理確認開立發票確認 */
const handleConfirmBillIssueConfirm = async (data: {
  employeePipelineBillID: number;
  employeePipelineBillNumber: string;
  employeePipelineBillRemark: string;
}) => {
  if (!requireToken() || !pipelineId) {
    return;
  }

  const requestData: MbsCrmPplHttpConfirmBillIssueReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineBillID: data.employeePipelineBillID,
    employeePipelineBillNumber: data.employeePipelineBillNumber,
    employeePipelineBillRemark: data.employeePipelineBillRemark,
  };

  const responseData: MbsCrmPplHttpConfirmBillIssueRspMdl | null =
    await confirmBillIssue(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess) {
    showConfirmBillIssueModal.value = false;
    showSuccess.value = true;
    successMessage.value = "確認開立發票成功";
    await getData();
  }
};

/** 處理確認開立發票取消 */
const handleConfirmBillIssueCancel = () => {
  showConfirmBillIssueModal.value = false;
  currentBill.value = null;
};
//#endregion

//#region 【履約期限通知】區塊事件處理
/** 是否顯示【履約期限通知】彈跳視窗 */
const showDueModal = ref(false);
/** 當前編輯的履約期限通知 */
const currentEditDue = ref<CrmPipelinePipelineDetailDueItemMdl | null>(null);
/** 當前編輯的履約期限通知索引 */
const currentEditDueIndex = ref<number | null>(null);

/** 處理新增履約期限通知 */
const handleAddDue = () => {
  currentEditDueIndex.value = null;
  currentEditDue.value = null;
  showDueModal.value = true;
};

/** 處理編輯履約期限通知 */
const handleEditDue = (due: CrmPipelinePipelineDetailDueItemMdl, index: number) => {
  currentEditDue.value = { ...due };
  currentEditDueIndex.value = index;
  showDueModal.value = true;
};

/** 處理刪除履約期限通知 */
const handleDeleteDue = (index: number) => {
  const due = crmPipelinePipelineDetailViewObj.employeePipelineDueList[index];
  if (!due?.employeePipelineDueID) {
    showError.value = true;
    errorMessage.value = "無效的履約期限通知";
    return;
  }

  showConfirmDialog({
    title: "確認刪除",
    message: "確定要刪除此履約期限通知嗎？",
    onConfirm: async () => {
      if (!requireToken()) return;

      const requestData: MbsCrmPplHttpRemoveEmployeePipelineDueReqMdl = {
        employeeLoginToken: tokenStore.token!,
        employeePipelineDueID: due.employeePipelineDueID!,
      };

      const responseData: MbsCrmPplHttpRemoveEmployeePipelineDueRspMdl | null =
        await removeEmployeePipelineDue(requestData);

      if (!responseData) {
        showError.value = true;
        errorMessage.value = "系統錯誤，請稍後再試";
        return;
      }

      const isSuccess = handleErrorCode(responseData.errorCode);

      if (isSuccess) {
        await getData();
      }
    },
  });
};

/** 處理履約期限通知確認 */
const handleDueModalConfirm = async (data: {
  employeePipelineDueTime: string;
  employeePipelineDueRemark: string;
}) => {
  if (!requireToken() || !pipelineId) {
    return;
  }

  // 如果是編輯模式
  if (currentEditDue.value?.employeePipelineDueID) {
    const requestData: MbsCrmPplHttpUpdateEmployeePipelineDueReqMdl = {
      employeeLoginToken: tokenStore.token!,
      employeePipelineDueID: currentEditDue.value.employeePipelineDueID,
      employeePipelineDueTime: formatToServerDatetime(data.employeePipelineDueTime),
      employeePipelineDueRemark: data.employeePipelineDueRemark,
    };

    const responseData: MbsCrmPplHttpUpdateEmployeePipelineDueRspMdl | null =
      await updateEmployeePipelineDue(requestData);

    if (!responseData) {
      showError.value = true;
      errorMessage.value = "系統錯誤，請稍後再試";
      return;
    }

    const isSuccess = handleErrorCode(responseData.errorCode);

    if (isSuccess) {
      showDueModal.value = false;
      currentEditDue.value = null;
      await getData();
    }
  } else {
    // 新增模式
    const requestData: MbsCrmPplHttpAddEmployeePipelineDueReqMdl = {
      employeeLoginToken: tokenStore.token!,
      employeePipelineID: pipelineId,
      employeePipelineDueTime: formatToServerDatetime(data.employeePipelineDueTime),
      employeePipelineDueRemark: data.employeePipelineDueRemark,
    };

    const responseData: MbsCrmPplHttpAddEmployeePipelineDueRspMdl | null =
      await addEmployeePipelineDue(requestData);

    if (!responseData) {
      showError.value = true;
      errorMessage.value = "系統錯誤，請稍後再試";
      return;
    }

    const isSuccess = handleErrorCode(responseData.errorCode);

    if (isSuccess) {
      showDueModal.value = false;
      await getData();
    }
  }
};

/** 處理履約期限通知取消 */
const handleDueModalCancel = () => {
  showDueModal.value = false;
  currentEditDueIndex.value = null;
  currentEditDue.value = null;
};
//#endregion

//#region 生命週期
onMounted(() => {
  getData();
  hydrateStageNotes();
  hydratePocForm();
  syncPipelineTab();
});

watch(
  () => ({ ...stageStatusNotes }),
  () => {
    persistStageNotes();
  },
  { deep: true }
);

watch(
  () => ({ ...pocForm }),
  () => {
    persistPocForm();
  },
  { deep: true }
);

watch(
  () => autoPipelineStatus.value,
  (nextStatus) => {
    if (!shouldSyncAutoStatus(nextStatus)) return;
    syncAutoPipelineStatus(nextStatus as DbAtomPipelineStatusEnum);
  }
);

watch(
  () => crmPipelinePipelineDetailViewObj.atomPipelineStatus,
  () => {
    syncPipelineTab();
  }
);

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

    <div class="flex flex-col gap-4">
      <div class="flex gap-3">
        <button
          class="tab-btn"
          :class="{ active: pipelineTab === 'potential' }"
          @click="pipelineTab = 'potential'"
        >
          潛在商機
        </button>
        <button
          class="tab-btn"
          :class="{ active: pipelineTab === 'formal' }"
          @click="pipelineTab = 'formal'"
        >
          正式商機
        </button>
        <button
          class="tab-btn"
          :class="{ active: pipelineTab === 'customer' }"
          @click="pipelineTab = 'customer'"
        >
          客戶名稱
        </button>
      </div>
      <!-- 【指派業務記錄】區塊 -->
      <PipelineSalerSection
        v-if="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
          DbAtomPipelineStatusEnum.TransferredToBusiness
        "
        :pending-employee-pipeline-saler="
          crmPipelinePipelineDetailViewObj.pendingEmployeePipelineSaler
        "
        :employee-pipeline-saler-list="crmPipelinePipelineDetailViewObj.employeePipelineSalerList"
        @accept-assignment="handleAcceptAssignment"
        @reject-assignment="handleRejectAssignment"
        @reassign-saler="handleReassignSaler"
      />

      <!-- 【基本資訊】區塊 -->
      <PipelineBasicInfoSection
        :atom-pipeline-status="crmPipelinePipelineDetailViewObj.atomPipelineStatus"
        :display-pipeline-status="autoPipelineStatus"
        :company="crmPipelinePipelineDetailViewObj.company"
        :booking-code="crmPipelinePipelineDetailViewObj.bookingCode"
        :employee-pipeline-saler-employee-name="
          crmPipelinePipelineDetailViewObj.employeePipelineSalerEmployeeName
        "
        :readonly="isPipelineLocked"
        @update-status="handleBasicInfoUpdateStatus"
        @transfer-project="handleBasicInfoTransferProject"
        @fail-pipeline="handleBasicInfoFailPipeline"
        @reassign-saler="handleBasicInfoReassignSaler"
        @update-booking-code="handleUpdateBookingCode"
      />

      <div v-if="pipelineTab === 'potential'" class="bg-white rounded-lg p-6 flex flex-col gap-4">
        <div class="flex items-center justify-between">
          <h2 class="subtitle">商機經營紀錄</h2>
          <span class="text-xs text-gray-500">最後更新：{{ lastUpdatedLabel }}</span>
        </div>
        <details class="group rounded-lg border border-dashed border-gray-300 bg-gray-50 px-4 py-3">
          <summary class="flex cursor-pointer items-center justify-between text-sm font-semibold text-blue-700">
            <span>10% / 30% / 70% 條件說明</span>
            <svg
              class="h-4 w-4 transition-transform group-open:rotate-180"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
            </svg>
          </summary>
          <ul class="mt-3 flex flex-col gap-2">
            <li
              v-for="rule in stageRuleSummaries"
              :key="rule.status"
              class="flex items-start gap-3 rounded-lg border border-dashed border-gray-300 bg-white p-3"
            >
              <div class="mt-0.5 h-2 w-2 rounded-full bg-blue-500"></div>
              <div>
                <div class="text-sm font-semibold text-blue-700">{{ rule.title }}</div>
                <p class="text-xs text-gray-600 mt-1 leading-relaxed">
                  {{ rule.description }}
                </p>
              </div>
            </li>
          </ul>
        </details>
        <div>
          <table class="table-base table-fixed table-sticky w-full min-w-[720px]">
            <thead class="bg-gray-800 text-white">
              <tr>
                <th class="text-start w-16">條件</th>
                <th class="text-start w-44">狀態</th>
                <th class="text-start w-56">備註</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="field in stageStatusFields"
                :key="field.key"
                class="border border-gray-300"
              >
                <td class="text-start font-semibold text-gray-700">{{ field.label }}</td>
                <td class="text-start">
                  <div class="flex flex-wrap gap-2">
                    <button
                      v-for="option in pipelineStageCheckOptions"
                      :key="option.value"
                      type="button"
                      class="rounded-md border px-2 py-1 text-xs font-semibold transition"
                      :class="
                        stageStatusForm[field.key] === option.value
                          ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                          : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                      "
                      :disabled="isPipelineLocked"
                      @click="
                        isPipelineLocked
                          ? null
                          : (stageStatusForm[field.key] =
                              stageStatusForm[field.key] === option.value ? null : option.value)
                      "
                    >
                      {{ getStageOptionLabel(field.key, option.label) }}
                    </button>
                  </div>
                </td>
                <td class="text-start">
                  <template v-if="field.key === 'businessPresentationStatus'">
                    <div
                      v-if="stageStatusForm[field.key] === DbEmployeePipelineStageCheckEnum.Confirmed"
                      class="grid grid-cols-1 gap-2 md:grid-cols-2"
                    >
                      <div class="md:col-span-2">
                        <label class="form-label">產品經理</label>
                        <select
                          v-model="presentationDetail.manager"
                          class="select-box"
                          :disabled="isPipelineLocked"
                        >
                          <option value="">請選擇</option>
                          <option value="李子涵">李子涵</option>
                          <option value="盧彥伶">盧彥伶</option>
                          <option value="施祥倫">施祥倫</option>
                        </select>
                      </div>
                      <div>
                        <label class="form-label">簡報日期</label>
                        <input
                          v-model="presentationDetail.date"
                          type="date"
                          class="input-box"
                          :disabled="isPipelineLocked"
                        />
                      </div>
                      <div>
                        <label class="form-label">時段</label>
                        <select
                          v-model="presentationDetail.timeSlot"
                          class="select-box"
                          :disabled="isPipelineLocked"
                        >
                          <option value="">請選擇</option>
                          <option v-for="slot in presentationTimeSlots" :key="slot" :value="slot">
                            {{ slot }}
                          </option>
                        </select>
                      </div>
                      <div class="md:col-span-2">
                        <label class="form-label">簡報內容</label>
                        <input
                          v-model="presentationDetail.summary"
                          type="text"
                          class="input-box"
                          placeholder="輸入簡報重點"
                          :disabled="isPipelineLocked"
                        />
                      </div>
                    </div>
                    <input
                      v-else
                      v-model="stageStatusNotes[field.key]"
                      type="text"
                      class="input-box"
                      placeholder="請輸入備註"
                      :disabled="isPipelineLocked"
                    />
                  </template>
                  <input
                    v-else
                    v-model="stageStatusNotes[field.key]"
                    type="text"
                    class="input-box"
                    placeholder="請輸入備註"
                    :disabled="isPipelineLocked"
                  />
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="border border-gray-200 rounded-xl p-4 flex flex-col gap-3">
          <div class="flex items-center justify-between">
            <div class="flex flex-col">
              <h2 class="subtitle">POC</h2>
              <span class="text-[11px] text-gray-500">商機若需要poc請底見新增POC</span>
            </div>
            <div class="flex items-center gap-2">
              <button
                v-if="pocForm.projectId"
                type="button"
                class="btn-submit"
                @click="goToPocProject"
              >
                前往 POC
              </button>
            </div>
          </div>
          <div v-if="shouldShowPocDraft" class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label class="form-label">POC 週期</label>
              <div class="flex flex-wrap gap-2">
                <button
                  type="button"
                  class="rounded-md border px-3 py-1.5 text-sm font-semibold transition"
                  :class="pocForm.durationWeeks === '2' ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                  :disabled="isPipelineLocked"
                  @click="pocForm.durationWeeks = '2'"
                >
                  2 週
                </button>
                <button
                  type="button"
                  class="rounded-md border px-3 py-1.5 text-sm font-semibold transition"
                  :class="pocForm.durationWeeks === '3' ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                  :disabled="isPipelineLocked"
                  @click="pocForm.durationWeeks = '3'"
                >
                  3 週
                </button>
                <button
                  type="button"
                  class="rounded-md border px-3 py-1.5 text-sm font-semibold transition"
                  :class="pocForm.durationWeeks === '4' ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                  :disabled="isPipelineLocked"
                  @click="pocForm.durationWeeks = '4'"
                >
                  4 週
                </button>
              </div>
            </div>
            <div>
              <label class="form-label">POC 起訖時間</label>
              <div class="grid grid-cols-2 gap-2">
                <input
                  v-model="pocForm.startDate"
                  type="date"
                  class="input-box"
                  :disabled="isPipelineLocked"
                />
                <input
                  v-model="pocForm.endDate"
                  type="date"
                  class="input-box"
                  :disabled="isPipelineLocked"
                />
              </div>
            </div>
          </div>
          <div
            v-if="pocAssignments.length === 0 && !isPipelineLocked"
            class="flex justify-center py-4 w-full"
          >
            <button
              class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
              style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
              type="button"
              @click="addPocAssignment"
            >
              新增 POC
            </button>
          </div>
          <div v-else-if="shouldShowPocDraft" class="space-y-4">
            <table class="table-base table-fixed table-sticky w-full min-w-[820px]">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-24">區域</th>
                  <th class="text-start w-32">部門</th>
                  <th class="text-start w-36">指派人員</th>
                  <th class="text-start w-48">備註</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in pocAssignments" :key="item.id" class="border border-gray-300">
                  <td class="text-start">
                    <select v-model="item.regionCode" class="select-box" :disabled="isPipelineLocked">
                      <option value="">請選擇</option>
                      <option value="A">北區</option>
                      <option value="B">中區</option>
                      <option value="C">南區</option>
                      <option value="X">跨區</option>
                    </select>
                  </td>
                  <td class="text-start">
                    <select v-model="item.departmentName" class="select-box" :disabled="isPipelineLocked">
                      <option value="">請選擇</option>
                      <option
                        v-for="department in getPocDepartmentOptions(item.regionCode)"
                        :key="department"
                        :value="department"
                      >
                        {{ department }}
                      </option>
                    </select>
                  </td>
                  <td class="text-start">
                    <select
                      v-if="isDepartmentManager"
                      v-model="item.employeeName"
                      class="select-box"
                      :disabled="isPipelineLocked"
                    >
                      <option value="">請選擇</option>
                      <option
                        v-for="employee in getPocEmployeeOptions(item.regionCode, item.departmentName)"
                        :key="employee"
                        :value="employee"
                      >
                        {{ employee }}
                      </option>
                    </select>
                    <span v-else class="text-sm text-gray-600">
                      {{ item.employeeName || "待部門經理指派" }}
                    </span>
                  </td>
                  <td class="text-start">
                    <input
                      v-model="item.progress"
                      type="text"
                      class="input-box"
                      placeholder="給部門經理的備註"
                      :disabled="isPipelineLocked"
                    />
                  </td>
                </tr>
              </tbody>
            </table>

            <div v-if="shouldShowPocDraft" class="flex justify-center gap-2 pt-2">
              <button
                v-if="pocAssignments.length > 0 && !isPipelineLocked"
                class="btn-cancel"
                type="button"
                @click="cancelPocDraft"
              >
                取消建立
              </button>
              <button
                v-if="pocAssignments.length > 0 && !isPipelineLocked"
                class="btn-submit"
                type="button"
                @click="savePoc"
              >
                建立 POC
              </button>
            </div>
          </div>
          <div v-else-if="isPocSaved" class="space-y-4">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
              <div>
                <div class="text-xs text-gray-500">POC 週期</div>
                <div class="text-sm font-semibold text-gray-800">
                  {{ pocForm.durationWeeks ? `${pocForm.durationWeeks} 週` : "-" }}
                </div>
              </div>
              <div>
                <div class="text-xs text-gray-500">POC 起訖時間</div>
                <div class="text-sm font-semibold text-gray-800">
                  {{ pocForm.startDate || "-" }} ~ {{ pocForm.endDate || "-" }}
                </div>
              </div>
            </div>
            <table class="table-base table-fixed table-sticky w-full min-w-[820px]">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-24">區域</th>
                  <th class="text-start w-32">部門</th>
                  <th class="text-start w-36">指派人員</th>
                  <th class="text-start w-48">備註</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in pocAssignments" :key="`${item.id}-readonly`" class="border border-gray-300">
                  <td class="text-start">{{ item.regionCode || "-" }}</td>
                  <td class="text-start">{{ item.departmentName || "-" }}</td>
                  <td class="text-start">{{ item.employeeName || "待部門經理指派" }}</td>
                  <td class="text-start">{{ item.progress || "-" }}</td>
                </tr>
              </tbody>
            </table>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
              <div>
                <label class="form-label">POC 結果</label>
                <select v-model="pocForm.result" class="select-box" :disabled="isPipelineLocked">
                  <option value="">請選擇</option>
                  <option value="order">會下單</option>
                  <option value="no_order">不會下單</option>
                </select>
              </div>
              <div v-if="pocForm.result === 'no_order'">
                <label class="form-label">不會下單原因</label>
                <select v-model="pocForm.rejectReason" class="select-box" :disabled="isPipelineLocked">
                  <option value="">請選擇</option>
                  <option v-for="reason in pocReasons" :key="reason" :value="reason">
                    {{ reason }}
                  </option>
                </select>
              </div>
            </div>
            <div v-if="pocForm.result === 'no_order' && pocForm.rejectReason === '其他'">
              <label class="form-label">原因補充</label>
              <input
                v-model="pocForm.rejectReasonNote"
                type="text"
                class="input-box"
                placeholder="輸入原因"
                :disabled="isPipelineLocked"
              />
            </div>
            <div class="flex justify-center gap-2 pt-2">
              <button class="btn-submit" type="button" @click="savePoc">
                儲存結果
              </button>
              <button class="btn-cancel" type="button" @click="clearPocAssignments">
                刪除 POC
              </button>
            </div>
          </div>
        </div>
        <div v-if="pocAssignments.length > 0" class="flex flex-col gap-2">
          <label class="form-label">備註</label>
          <textarea
            v-model="stageStatusForm.businessStageRemark"
            class="input-box h-24"
            placeholder="可記錄進度、客戶額外需求或重要紀錄"
          ></textarea>
        </div>
      </div>

      <!-- 【客戶】區塊 -->
      <PipelineCompanySection
        v-if="pipelineTab === 'customer'"
        :company="crmPipelinePipelineDetailViewObj.company"
        :readonly="true"
      />

      <!-- 【窗口】區塊 -->
      <PipelineContacterSection
        v-if="pipelineTab === 'formal'"
        :contacter-list="crmPipelinePipelineDetailViewObj.contacterList"
        :readonly="true"
      />

      <!-- 【產品】區塊 -->
      <PipelineProductSection
        v-if="pipelineTab === 'formal'"
        :product-list="crmPipelinePipelineDetailViewObj.employeePipelineProductList"
        :readonly="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.TransferredToBusiness ||
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.TransferredToProject ||
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.BusinessFailed
        "
        @add-product="handleAddProduct"
        @edit-product="handleEditProduct"
        @delete-product="handleDeleteProduct"
      />

      <!-- 【商機開發記錄】區塊 -->
      <!-- 【發票記錄】區塊 -->
      <PipelineBillSection
        v-if="pipelineTab === 'formal' && shouldShowBillSection"
        :bill-list="crmPipelinePipelineDetailViewObj.employeePipelineBillList"
        :total-amount="totalProductClosingPrice"
        :period-count="billPeriodCount"
        :can-edit-multiple-bills="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus !==
          DbAtomPipelineStatusEnum.TransferredToProject
        "
        :show-confirm-button="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
          DbAtomPipelineStatusEnum.TransferredToProject
        "
        :show-notify-button="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
          DbAtomPipelineStatusEnum.TransferredToProject
        "
        :readonly="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.TransferredToProject ||
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.BusinessFailed
        "
        @edit-multiple-bills="handleEditMultipleBills"
        @notify-bill-issue="handleNotifyBillIssue"
        @confirm-bill-issue="handleConfirmBillIssue"
      />

      <!-- 【履約期限通知】區塊 -->
      <PipelineDueSection
        v-if="pipelineTab === 'formal' && showDueSection"
        :due-list="crmPipelinePipelineDetailViewObj.employeePipelineDueList"
        :readonly="
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.TransferredToBusiness ||
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.TransferredToProject ||
          crmPipelinePipelineDetailViewObj.atomPipelineStatus ===
            DbAtomPipelineStatusEnum.BusinessFailed
        "
        @add-due="handleAddDue"
        @edit-due="handleEditDue"
        @delete-due="handleDeleteDue"
      />
    </div>

    <!-- 成功提示 Toast -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 確認彈跳視窗 -->
    <ConfirmDialog
      :show="confirmDialog.show"
      :title="confirmDialog.title"
      :message="confirmDialog.message"
      @confirm="handleConfirmDialog"
      @cancel="handleCancelDialog"
    />

    <!-- 指派業務記錄-【拒絕指派】彈跳視窗 -->
    <RejectAssignmentModal
      :show="showRejectAssignmentModal"
      @confirm="handleRejectAssignmentConfirm"
      @cancel="handleRejectAssignmentCancel"
    />

    <!-- 基本資訊/指派業務記錄-【轉指派業務】彈跳視窗 -->
    <ReassignSalerModal
      :show="showReassignSalerModal"
      @confirm="handleReassignSalerConfirm"
      @cancel="handleReassignSalerCancel"
    />

    <!-- 基本資訊-【新增專案】彈跳視窗 -->
    <TransferProjectModal
      :show="showTransferProjectModal"
      :saler-employee-i-d="crmPipelinePipelineDetailViewObj.employeePipelineSalerEmployeeID"
      :saler-employee-name="crmPipelinePipelineDetailViewObj.employeePipelineSalerEmployeeName"
      :saler-region-i-d="crmPipelinePipelineDetailViewObj.employeePipelineSalerRegionID"
      :saler-region-name="crmPipelinePipelineDetailViewObj.employeePipelineSalerRegionName"
      :saler-department-i-d="crmPipelinePipelineDetailViewObj.employeePipelineSalerDepartmentID"
      :saler-department-name="crmPipelinePipelineDetailViewObj.employeePipelineSalerDepartmentName"
      @confirm="handleTransferProjectDialogConfirm"
      @cancel="showTransferProjectModal = false"
    />

    <!-- 產品-【附加/編輯產品】彈跳視窗 -->
    <PipelineProductModal
      :show="showProductModal"
      :product="currentEditProduct"
      @confirm="handleProductModalConfirm"
      @cancel="handleProductModalCancel"
    />

    <!-- 商機開發記錄-【附加/編輯商機開發記錄】彈跳視窗 -->
    <TrackingModal
      :show="showTrackingModal"
      :contacter-list="
        crmPipelinePipelineDetailViewObj.contacterList.map((c) => ({
          managerContacterID: c.managerContacterID,
          managerContacterName: c.managerContacterName,
        }))
      "
      @confirm="handleTrackingModalConfirm"
      @cancel="handleTrackingModalCancel"
    />

    <!-- 發票記錄-【編輯多筆發票紀錄】彈跳視窗 -->
    <EditMultipleBillsModal
      :show="showEditMultipleBillsModal"
      :total-amount="totalProductClosingPrice"
      :current-period-count="billPeriodCount"
      :current-bills="crmPipelinePipelineDetailViewObj.employeePipelineBillList"
      @confirm="handleEditMultipleBillsConfirm"
      @cancel="handleEditMultipleBillsCancel"
    />

    <!-- 發票記錄-【確認開立發票】彈跳視窗 -->
    <ConfirmBillIssueModal
      :show="showConfirmBillIssueModal"
      :bill="currentBill"
      @confirm="handleConfirmBillIssueConfirm"
      @cancel="handleConfirmBillIssueCancel"
    />

    <!-- 履約期限通知-【新增/編輯履約期限通知】彈跳視窗 -->
    <PipelineDueModal
      :show="showDueModal"
      :due="currentEditDue"
      @confirm="handleDueModalConfirm"
      @cancel="handleDueModalCancel"
    />
  </div>
</template>
