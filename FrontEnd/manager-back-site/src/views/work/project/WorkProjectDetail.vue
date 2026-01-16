<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, onUnmounted, defineAsyncComponent, computed, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
// Enums / 常數
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { orgDepartmentDirectory } from "@/constants/orgDepartmentDirectory";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
// Stores
import { useTokenStore } from "@/stores/token";
import { useProjectTimeInfoStore } from "@/stores/projectTimeInfo";
import { useProjectMemberRoleStore } from "@/stores/projectMemberRole";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
// Services
import type {
  MbsWrkPrjHttpGetManyMemberRoleReqMdl,
  MbsWrkPrjHttpGetManyMemberRoleRspMdl,
  MbsWrkPrjHttpGetManyProjectStoneReqMdl,
  MbsWrkPrjHttpGetManyProjectStoneRspMdl,
  MbsWrkPrjHttpGetManyProjectStoneTaskReqMdl,
  MbsWrkPrjHttpGetManyProjectStoneTaskRspMdl,
  MbsWrkPrjHttpGetProjectReqMdl,
  MbsWrkPrjHttpGetProjectRspMdl,
  MbsWrkPrjHttpRemoveMemberReqMdl,
  MbsWrkPrjHttpRemoveMemberRspMdl,
  MbsWrkPrjHttpRemoveProjectStoneReqMdl,
  MbsWrkPrjHttpRemoveProjectStoneRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl,
  MbsWrkPrjHttpGetManyExpenseReqMdl,
  MbsWrkPrjHttpGetManyExpenseRspMdl,
  MbsWrkPrjHttpUpdateProjectReqMdl,
  MbsWrkPrjHttpUpdateProjectRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import {
  getManyProjectMemberRole,
  getManyProjectStone,
  getManyProjectStoneTask,
  getProject,
  removeProjectMember,
  removeProjectStone,
  updateProjectStoneTaskBucket,
  getManyExpense,
  updateProject,
} from "@/services";
// Utils
import { formatDate } from "@/utils/timeFormatter";
import { formatCurrency } from "@/utils/currencyFormatter";
import { getEmployeeProjectMemberRoleLabel } from "@/utils/getEmployeeProjectMemberRoleLabel";
import { getEmployeeProjectStatusLabel } from "@/utils/getEmployeeProjectStatusLabel";
// Components
import AddWorkProjectEmployeeModal from "./components/AddWorkProjectEmployeeModal.vue";
import AddWorkProjectExpenseModal from "./components/AddWorkProjectExpenseModal.vue";
import UpdateWorkProjectExpenseModal from "./components/UpdateWorkProjectExpenseModal.vue";
import ProjectRolePermissionInfo from "@/components/global/info/ProjectRolePermissionInfo.vue";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);
const SuccessToast = defineAsyncComponent(
  () => import("@/components/global/feedback/SuccessToast.vue")
);
const AddWorkProjectStoneModal = defineAsyncComponent(
  () => import("@/views/work/project/components/AddWorkProjectStoneModal.vue")
);
const UpdateWorkProjectStoneModal = defineAsyncComponent(
  () => import("@/views/work/project/components/UpdateWorkProjectStoneModal.vue")
);
const UpdateWorkProjectTaskModal = defineAsyncComponent(
  () => import("./components/UpdateWorkProjectTaskModal.vue")
);
const ConfirmDialog = defineAsyncComponent(
  () => import("@/components/global/feedback/ConfirmDialog.vue")
);
import {
  loadProjectTemplateSettings,
  getStageTemplatesByServiceItems,
} from "@/stores/projectTemplateSettings";
//#endregion

//#region 外部依賴
/** 專案角色權限說明 ref */
const projectRolePermissionInfoRef = ref<InstanceType<typeof ProjectRolePermissionInfo> | null>(
  null
);
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
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
/** 路由物件（用於取得路由參數） */
const route = useRoute();
/** 專案時間資訊儲存 */
const projectTimeInfoStore = useProjectTimeInfoStore();
/** 專案成員角色儲存 */
const projectMemberRoleStore = useProjectMemberRoleStore();
//#endregion

//#region 型別定義
enum PipelineTabEnum {
  Project = "Project",
  Template = "Template",
  ProjectStone = "ProjectStone",
  ProjectStoneTask = "ProjectStoneTask",
  Poc = "Poc",
  Expense = "Expense",
  Risk = "Risk",
}

/** 工作-專案管理-檢視-頁面模型 */
interface WorkProjectDetailViewMdl {
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum | null;
  employeeProjectCode: string;
  employeeProjectContractUrl: string | null;
  employeeProjectContractCreateTime: string;
  employeeProjectWorkUrl: string | null;
  employeeProjectWorkCreateTime: string;
  employeeProjectMemberList: WorkPrjListItemMdl[];
  employeeProjectName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
  managerCompanyName: string;
}

/** 工作-專案管理-檢視-項目模型 */
interface WorkPrjListItemMdl {
  employeeProjectMemberID: number;
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  managerRegionName: string;
  managerDepartmentName: string;
  employeeName: string;
  employeeID?: number;
}

interface PocBranchFormMdl {
  durationWeeks: string;
  result: "order" | "no_order" | "";
  rejectReason: string;
  rejectReasonNote: string;
  projectId: number;
  pipelineId: number | null;
}

/** 工作-專案管理-更新-驗證模型 */
interface WorkProjectUpdateValidateMdl {
  employeeProjectName: boolean;
  employeeProjectCode: boolean;
}

/** 工作-專案管理-更新-原始模型 */
interface WorkProjectUpdateOriginalMdl {
  employeeProjectName: string;
  employeeProjectCode: string;
}
interface StageOutputStateMdl {
  outputId: number;
  updatedAt: string;
  files: StageOutputFileMdl[];
}

interface StageOutputFileMdl {
  fileId: number;
  fileName: string;
  fileSize: number;
  fileType: string;
  uploadedAt: string;
}

interface StageWorkLogEntryMdl {
  logId: number;
  logDate: string;
  logContent: string;
  createdAt: string;
}

interface StageTemplateOutputMdl {
  outputId: number;
  outputName: string;
  required: boolean;
}

interface StageTemplateStageMdl {
  stageId: number;
  stageName: string;
  requiredOutputs: StageTemplateOutputMdl[];
}

type ReviewStatus = "not_sent" | "waiting" | "pending" | "approved" | "rejected" | "not_required";

interface StageOutputReviewItemMdl {
  reviewId: number;
  projectId: number;
  projectName: string;
  stageId: number;
  stageName: string;
  outputId: number;
  outputName: string;
  fileName: string;
  fileUrl: string;
  uploadedBy: string;
  uploadedAt: string;
  assistantStatus: ReviewStatus;
  managerStatus: ReviewStatus;
  overallStatus: ReviewStatus;
  assistantFeedback?: string;
  managerFeedback?: string;
  lastFeedback?: string;
  updatedAt: string;
  reviewTarget: "assistant" | "manager";
}

interface StageWorkLogGroupMdl {
  stageLogs: StageWorkLogEntryMdl[];
  dailyLogs: StageWorkLogEntryMdl[];
}
//------------------------------------------------------------
/** 工作-專案管理-里程碑-頁面模型 */
interface WorkProjectStoneListViewMdl {
  projectStoneItemList: WorkProjectStoneItemMdl[];
}

/** 工作-專案管理-里程碑-項目模型 */
interface WorkProjectStoneItemMdl {
  /** 里程碑 ID */
  employeeProjectStoneID: number;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string;
  /** 里程碑前 N 日通知 */
  employeeProjectStonePreNotifyDay: number;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項列表 */
  employeeProjectStoneTaskList: EmployeeProjectStoneTaskItemMdl[];
  /** 是否展開 */
  isOpen: boolean;
}
/** 里程碑工項列表 */
interface EmployeeProjectStoneTaskItemMdl {
  /** 里程碑工項 ID */
  employeeProjectStoneTaskID: number;
  /** 里程碑工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 里程碑工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 里程碑工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 里程碑工項執行者筆數 */
  employeeProjectStoneTaskExecutorCount: number;
}
//---------------------------------------------------------------------
/** 工作-專案管理-工項-頁面模型 */
interface WorkProjectTaskStoneListViewMdl {
  /** 里程碑列表 */
  employeeProjectStoneList: WorkProjectTaskListViewMdl[];
}
/** 工作-專案管理-工項-頁面模型 */
interface WorkProjectTaskListViewMdl {
  /** 里程碑 ID */
  employeeProjectStoneID: number;
  /** 里程碑名稱 */
  employeeProjectStoneName: string;
  /** 里程碑前 N 日通知 */
  employeeProjectStonePreNotifyDay: number;
  /** 里程碑開始時間 */
  employeeProjectStoneStartTime: string;
  /** 里程碑結束時間 */
  employeeProjectStoneEndTime: string;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum | null;
  /** 里程碑工項列表 */
  employeeProjectStoneTaskList: WorkProjectTaskItemMdl[];
}

/** * 工作-專案管理-工項-項目模型 */
export interface WorkProjectTaskItemMdl {
  /** 工項 ID */
  employeeProjectStoneTaskID: number;
  /** 工項名稱 */
  employeeProjectStoneTaskName: string;
  /** 工項開始時間 */
  employeeProjectStoneTaskStartTime: string;
  /** 工項結束時間 */
  employeeProjectStoneTaskEndTime: string;
  /** 工項工時 */
  employeeProjectStoneTaskWorkHour: number;
  /** 專案狀態 */
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  /** 工項執行者筆數 */
  employeeProjectStoneTaskExecutorCount: number;
  /** 工項清單列表 */
  employeeProjectStoneTaskBucketList: WorkProjectTaskItemMdlBucketItemMdl[];
}

/** 工項清單列表 */
export interface WorkProjectTaskItemMdlBucketItemMdl {
  /** 清單 ID */
  employeeProjectStoneTaskBucketID: number;
  /** 清單名稱 */
  employeeProjectStoneTaskBucketName: string;
  /** 是否完成 */
  employeeProjectStoneTaskBucketIsFinish: boolean;
}

/** 工作-專案管理-收支-頁面模型 */
export interface WorkProjectExpenseListViewMdl {
  /** 專案收支列表 */
  projectExpenseList: WorkProjectExpenseItemMdl[];
  /** Eip專案支出列表 */
  eipProjectExpenseList: WorkEipProjectExpenseItemMdl[];
  /** Eip專案旅費列表 */
  eipProjectTravelExpenseList: WorkEipProjectTravelExpenseItemMdl[];
}

/** 專案收支項目模型 */
export interface WorkProjectExpenseItemMdl {
  /** 專案支出 ID */
  employeeProjectExpenseID: number;
  /** 名稱 */
  employeeProjectExpenseName: string;
  /** 預估金額 */
  employeeProjectExpensePredictAmount: number;
  /** 實際金額 */
  employeeProjectExpenseActualAmount: number | null;
  /** 發票號碼 */
  employeeProjectExpenseBillNumber: string | null;
  /** 發票日期 */
  employeeProjectExpenseBillTime: string | null;
  /** 備註 */
  employeeProjectExpenseRemark: string | null;
}

/** Eip專案支出項目模型 */
export interface WorkEipProjectExpenseItemMdl {
  /** 簽核狀態 */
  projectExpenseApprovalStatus: string;
  /** 申請人員 */
  projectExpenseApplicant: string;
  /** 日期 */
  projectExpenseDate: string;
  /** 事由 */
  projectExpenseReason: string;
  /** 參與人員 */
  projectExpenseParticipants: string;
  /** 金額 */
  projectExpenseAmount: number;
}

/** Eip專案旅費項目模型 */
export interface WorkEipProjectTravelExpenseItemMdl {
  /** 簽核狀態 */
  projectTravelExpenseApprovalStatus: string;
  /** 申請人員 */
  projectTravelExpenseApplicant: string;
  /** 日期 */
  projectTravelExpenseTravelDate: string;
  /** 起訖地點 */
  projectTravelExpenseTravelRoute: string;
  /** 工作記要 */
  projectTravelExpenseWorkDescription: string;
  /** 交通工具 */
  projectTravelExpenseTransportationTool: string;
  /** 交通費金額 */
  projectTravelExpenseTransportationAmount: number | null;
  /** 里程 */
  projectTravelExpenseMileage: number | null;
  /** 膳宿費 */
  projectTravelExpenseAccommodationAmount: number | null;
  /** 特別費事由 */
  projectTravelExpenseSpecialExpenseReason: string | null;
  /** 特別費金額 */
  projectTravelExpenseSpecialExpenseAmount: number | null;
  /** 合計 */
  projectTravelExpenseTotalAmount: number | null;
}

interface WorkProjectChangeRequestItem {
  requestId: number;
  projectId: number;
  projectName: string;
  companyName: string;
  requesterName: string;
  requesterRole: string;
  requesterDepartment: string;
  requesterRegion: string;
  requestType: "member_adjust" | "task_adjust" | "cross_support";
  targetRegion?: string;
  targetDepartment?: string;
  targetStageName?: string;
  estimatedHours?: number | null;
  onsiteHours?: number | null;
  offsiteHours?: number | null;
  reason: string;
  status: "pending" | "approved" | "rejected";
  createdAt: string;
  updatedAt?: string;
  reviewedAt?: string;
  reviewedBy?: string;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.WorkProject;
/** 當前選中的Tab */
const activeTab = ref<PipelineTabEnum>(PipelineTabEnum.Project);
/** 專案ID */
const projectID = ref(Number(route.params.id) || 0);
const changeRequestStorageKey = "cache.work.project.changeRequests";
const changeRequestEventName = "work-project-change-requests-updated";
const changeRequests = ref<WorkProjectChangeRequestItem[]>([]);
const showChangeRequestForm = ref(false);
const changeRequestForm = reactive({
  requestType: null as WorkProjectChangeRequestItem["requestType"] | null,
  targetRegion: "",
  targetDepartment: "",
  targetStageName: "",
  estimatedHours: "",
  onsiteHours: "",
  offsiteHours: "",
  reason: "",
});
const changeRequestTypes = [
  { value: "member_adjust", label: "人員調整" },
  { value: "task_adjust", label: "工項異動" },
  { value: "cross_support", label: "跨部門/跨區支援" },
];
const pocBranchForm = reactive<PocBranchFormMdl>({
  durationWeeks: "",
  result: "",
  rejectReason: "",
  rejectReasonNote: "",
  projectId: projectID.value,
  pipelineId: null,
});
const pocBranchAssignments = ref<
  { id: number; departmentName: string; employeeName: string; progress: string; todo: string }[]
>([]);
const pocReasons = [
  "工程師技術不到位",
  "業務溝通問題",
  "產品功能不符合需求",
  "其他",
];
const hasPocBranch = computed(() => pocBranchAssignments.value.length > 0);
const projectChangeRequests = computed(() =>
  changeRequests.value.filter((item) => item.projectId === projectID.value)
);
const projectChangeRequestLastUpdated = computed(() => {
  if (projectChangeRequests.value.length === 0) return "";
  const latest = projectChangeRequests.value.reduce((acc, item) => {
    const current = new Date(item.updatedAt || item.createdAt).getTime();
    return current > acc ? current : acc;
  }, 0);
  return latest ? formatDate(new Date(latest).toISOString()) : "";
});

const persistPocBranch = () => {
  const formPayload = {
    durationWeeks: pocBranchForm.durationWeeks,
    result: pocBranchForm.result,
    rejectReason: pocBranchForm.rejectReason,
    rejectReasonNote: pocBranchForm.rejectReasonNote,
    projectId: pocBranchForm.projectId,
  };
  localStorage.setItem(
    `cache.work.project.poc.${projectID.value}`,
    JSON.stringify({
      pipelineId: pocBranchForm.pipelineId,
      form: formPayload,
      assignments: pocBranchAssignments.value,
    })
  );
  if (pocBranchForm.pipelineId) {
    localStorage.setItem(
      `cache.crm.pipeline.poc.${pocBranchForm.pipelineId}`,
      JSON.stringify({
        form: formPayload,
        assignments: pocBranchAssignments.value,
      })
    );
  }
};

const hydratePocBranch = () => {
  const raw = localStorage.getItem(`cache.work.project.poc.${projectID.value}`);
  if (!raw) return;
  try {
    const parsed = JSON.parse(raw);
    if (parsed?.form) {
      Object.assign(pocBranchForm, parsed.form);
    }
    if (Array.isArray(parsed?.assignments)) {
      pocBranchAssignments.value = parsed.assignments;
    }
    if (parsed?.pipelineId) {
      pocBranchForm.pipelineId = parsed.pipelineId;
    }
  } catch {
    return;
  }
};
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 新增人員彈跳視窗 */
const isShowAddMemberModal = ref(false);
/** 是否顯示【新增專案支出】彈跳視窗 */
const showAddExpenseModal = ref(false);
/** 是否顯示【編輯專案支出】彈跳視窗 */
const showUpdateExpenseModal = ref(false);
/** 選取的專案支出 ID (送至元件)*/
const selectedExpenseID = ref<number>(0);
/** 是否顯示刪除專案人員確認視窗 */
const showConfirmProjectMemberDialog = ref(false);
/** 暫存欲刪除的專案人員 ID */
const deletingProjectMemberID = ref<number | null>(null);
/** 是否顯示【新增里程碑】彈跳視窗 */
const isAddProjectStoneShow = ref(false);
/** 是否顯示【更新里程碑】彈跳視窗 */
const isUpdateProjectStoneShow = ref(false);
/** 選取的里程碑 ID (送至元件)*/
const selectedProjectStoneID = ref<number>(0);
/** 是否顯示刪除里程碑確認視窗 */
const showDeleteProjectStoneConfirm = ref(false);
/** 刪除的里程碑 ID */
const deletingProjectStoneID = ref<number | null>(null);
/** 是否顯示【更新工項】彈跳視窗 */
const isUpdateProjectTaskShow = ref(false);
/** 選取的工項 ID (送至元件)*/
const selectedProjectTaskID = ref<number>(0);
/** 操作選單(編輯/刪除) */
const showActionMenu = ref<number | null>(null);
/** 是否為權限錯誤 */
const isPermissionError = ref(false);

/** 工作-專案管理-列表-頁面物件 */
const workProjectDetailViewObj = reactive<WorkProjectDetailViewMdl>({
  atomEmployeeProjectStatus: null,
  employeeProjectCode: "",
  employeeProjectContractUrl: null,
  employeeProjectContractCreateTime: "",
  employeeProjectWorkUrl: null,
  employeeProjectWorkCreateTime: "",
  employeeProjectMemberList: [],
  employeeProjectName: "",
  employeeProjectStartTime: "",
  employeeProjectEndTime: "",
  managerCompanyName: "",
});
const templateSettings = ref(loadProjectTemplateSettings());
const projectTypeTemplates = computed(() => templateSettings.value.projectTypes);
const serviceItemTemplates = computed(() => templateSettings.value.serviceItems);
const reminderRule = computed(() => templateSettings.value.reminderRule);
const selectedProjectTypeId = ref<number | null>(
  projectTypeTemplates.value.length > 0 ? projectTypeTemplates.value[0].projectTypeId : null
);
const selectedServiceItemIds = ref<number[]>([]);
const selectedProjectType = computed(
  () =>
    projectTypeTemplates.value.find(
      (item) => item.projectTypeId === selectedProjectTypeId.value
    ) ?? null
);
const availableServiceItems = computed(() => {
  if (!selectedProjectType.value) return [];
  const base = serviceItemTemplates.value.filter((item) =>
    selectedProjectType.value?.serviceItemIds.includes(item.serviceItemId)
  );
  const selected = serviceItemTemplates.value.filter((item) =>
    selectedServiceItemIds.value.includes(item.serviceItemId)
  );
  const merged = [...base];
  for (const item of selected) {
    if (!merged.some((existing) => existing.serviceItemId === item.serviceItemId)) {
      merged.push(item);
    }
  }
  return merged;
});
const selectedServiceItems = computed(() =>
  availableServiceItems.value.filter((item) =>
    selectedServiceItemIds.value.includes(item.serviceItemId)
  )
);
const selectedServiceProductIds = ref<Record<number, number[]>>({});
const formatProductNames = (
  products: { productName?: string | null; productId: number }[]
): string => {
  const names = products.map((product) => product.productName).filter((name) => Boolean(name));
  if (names.length === 0) return "";
  if (names.length <= 3) return names.join("、");
  return `${names.slice(0, 3).join("、")} 等${names.length}項`;
};
const stageOwnerMap = ref<Record<number, string>>({});
const showAllStages = ref(false);
const stageOwnerStorageKey = computed(
  () => `cache.work.project.stageOwners.${projectID.value}`
);
const viewRoleName = computed(() => employeeInfoStore.effectiveRoleName || "");
const isConsultantView = computed(() => viewRoleName.value.includes("顧問"));
const visibleServiceItems = computed(() => {
  const roleName = viewRoleName.value;
  if (roleName.includes("工程")) {
    return selectedServiceItems.value.filter((item) => item.serviceItemName === "聯防設備");
  }
  if (roleName.includes("顧問")) {
    return selectedServiceItems.value.filter((item) => item.serviceItemName.includes("顧問"));
  }
  return selectedServiceItems.value;
});
const selectedServiceProducts = computed(() =>
  visibleServiceItems.value.map((service) => {
    const productIds = selectedServiceProductIds.value[service.serviceItemId];
    if (!productIds || productIds.length === 0) {
      return { service, products: [] };
    }
    return {
      service,
      products: service.products.filter((product) => productIds.includes(product.productId)),
    };
  })
);
const viewEngineerMember = computed(() => {
  if (!viewRoleName.value.includes("工程")) return null;
  return (
    workProjectDetailViewObj.employeeProjectMemberList.find((item) =>
      item.managerDepartmentName?.includes("工程部")
    ) ?? null
  );
});
const currentEmployeeName = computed(
  () => viewEngineerMember.value?.employeeName || employeeInfoStore.employeeName || ""
);
const primaryMemberRoles = new Set([
  DbEmployeeProjectMemberRoleEnum.Saler,
  DbEmployeeProjectMemberRoleEnum.ProjectManager,
  DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
]);
const primaryProjectMembers = computed(() =>
  workProjectDetailViewObj.employeeProjectMemberList.filter((item) =>
    primaryMemberRoles.has(item.employeeProjectMemberRole ?? DbEmployeeProjectMemberRoleEnum.Undefined) &&
    item.employeeProjectMemberRole !== DbEmployeeProjectMemberRoleEnum.Boss
  )
);
const secondaryProjectMembers = computed(() =>
  workProjectDetailViewObj.employeeProjectMemberList.filter(
    (item) =>
      !primaryMemberRoles.has(item.employeeProjectMemberRole ?? DbEmployeeProjectMemberRoleEnum.Undefined) &&
      item.employeeProjectMemberRole !== DbEmployeeProjectMemberRoleEnum.Boss
  )
);
const stageDepartmentAllowlist: Record<string, string[]> = {
  雲安維運: ["鑑識部", "監控部", "雲安維護監控中心"],
  風險檢測: ["風險稽核處", "安全實驗室"],
  顧問諮詢: ["管理顧問處", "北區顧問部", "中區顧問部", "南區顧問部"],
  聯防設備: ["工程部", "北區工程部", "中區工程部", "南區工程部", "高雄工程部", "台南工程部"],
  資安學苑: ["漢昕資安學苑"],
  內部軟體開發: ["研發部", "AI智能部", "系統開發部"],
  外部軟體開發: ["研發部", "AI智能部", "系統開發部"],
};
const getPrimaryRoleLabel = (role: DbEmployeeProjectMemberRoleEnum | null | undefined) => {
  switch (role) {
    case DbEmployeeProjectMemberRoleEnum.Saler:
      return "負責業務";
    case DbEmployeeProjectMemberRoleEnum.ProjectManager:
      return "建立專案人";
    default:
      return getEmployeeProjectMemberRoleLabel(role ?? DbEmployeeProjectMemberRoleEnum.Undefined);
  }
};
const getPrimaryMemberName = (item: WorkPrjListItemMdl) => {
  if (
    item.employeeProjectMemberRole === DbEmployeeProjectMemberRoleEnum.Saler &&
    (!item.employeeName || item.employeeName === "業務")
  ) {
    const byDepartment = orgMemberDirectory.filter(
      (member) => member.departmentName === item.managerDepartmentName || member.departmentName.includes("業務")
    );
    const regionFiltered =
      item.managerRegionName && item.managerRegionName !== "全區"
        ? byDepartment.filter((member) => member.regionName.startsWith(item.managerRegionName))
        : byDepartment;
    const picked = regionFiltered[0] ?? byDepartment[0];
    return picked?.name ?? "未指派";
  }
  return item.employeeName || "未指派";
};
const stageOwnerByService = (serviceName: string): string => {
  const memberList = workProjectDetailViewObj.employeeProjectMemberList;
  const normalized = serviceName.trim();
  const allowlist = stageDepartmentAllowlist[normalized] ?? [];
  const byDepartment = memberList.find((item) =>
    allowlist.some((dept) => item.managerDepartmentName?.includes(dept))
  );
  if (byDepartment?.employeeName) return byDepartment.employeeName;
  const fallback = memberList.find(
    (item) => item.employeeProjectMemberRole !== DbEmployeeProjectMemberRoleEnum.Boss
  );
  return fallback?.employeeName || "未指派";
};
const buildStageOwnerMap = () => {
  const stored = localStorage.getItem(stageOwnerStorageKey.value);
  if (stored) {
    try {
      stageOwnerMap.value = JSON.parse(stored);
      return;
    } catch {
      stageOwnerMap.value = {};
    }
  }
  const next: Record<number, string> = {};
  selectedServiceProducts.value.forEach((serviceEntry) => {
    const ownerName = stageOwnerByService(serviceEntry.service.serviceItemName);
    const stageTemplateIds = serviceEntry.products.flatMap((product) => product.stageTemplateIds);
    const templates = selectedStageTemplates.value.filter((template) =>
      stageTemplateIds.includes(template.stageTemplateId)
    );
    templates.forEach((template) => {
      template.stages.forEach((stage) => {
        if (!next[stage.stageId]) {
          next[stage.stageId] = ownerName;
        }
      });
    });
  });
  stageOwnerMap.value = next;
  localStorage.setItem(stageOwnerStorageKey.value, JSON.stringify(next));
};
const isStageVisible = (stageId: number): boolean => {
  if (showAllStages.value) return true;
  if (isConsultantView.value) return true;
  if (viewRoleName.value.includes("工程")) {
    const member = viewEngineerMember.value;
    const serviceNames = stageServiceMap.value.get(stageId) ?? [];
    if (!member?.managerDepartmentName) {
      return serviceNames.some((serviceName) => serviceName.trim() === "聯防設備");
    }
    return serviceNames.some((serviceName) => {
      const allowlist = stageDepartmentAllowlist[serviceName.trim()] ?? [];
      return allowlist.some((dept) => member.managerDepartmentName?.includes(dept));
    });
  }
  const owner = stageOwnerMap.value[stageId];
  if (!owner || !currentEmployeeName.value) return false;
  return owner === currentEmployeeName.value;
};
const getStageOwnerName = (stageId: number): string =>
  stageOwnerMap.value[stageId] || "未指派";
const getVisibleStagesForTemplate = (
  template: (typeof selectedStageTemplates.value)[number]
) => template.stages.filter((stage) => isStageVisible(stage.stageId));
const hasVisibleStagesForService = (serviceEntry: {
  service: { serviceItemName: string; serviceItemId: number };
  products: { productId: number; stageTemplateIds: number[] }[];
}): boolean => {
  return serviceEntry.products.some((product) =>
    selectedStageTemplates.value
      .filter((template) => product.stageTemplateIds.includes(template.stageTemplateId))
      .some((template) => getVisibleStagesForTemplate(template).length > 0)
  );
};
const stageServiceMap = computed(() => {
  const map = new Map<number, string[]>();
  selectedServiceProducts.value.forEach((serviceEntry) => {
    const stageTemplateIds = serviceEntry.products.flatMap((product) => product.stageTemplateIds);
    const templates = selectedStageTemplates.value.filter((template) =>
      stageTemplateIds.includes(template.stageTemplateId)
    );
    templates.forEach((template) => {
      template.stages.forEach((stage) => {
        const existing = map.get(stage.stageId) ?? [];
        if (!existing.includes(serviceEntry.service.serviceItemName)) {
          map.set(stage.stageId, [...existing, serviceEntry.service.serviceItemName]);
        }
      });
    });
  });
  return map;
});
const expandedStageIds = ref<Set<number>>(new Set());
const isStageExpanded = (stageId: number) => expandedStageIds.value.has(stageId);
const toggleStageExpanded = (stageId: number) => {
  const next = new Set(expandedStageIds.value);
  if (next.has(stageId)) {
    next.delete(stageId);
  } else {
    next.add(stageId);
  }
  expandedStageIds.value = next;
};
const selectedStageTemplates = computed(() =>
  getStageTemplatesByServiceItems(templateSettings.value, selectedServiceItemIds.value)
);
const outputStateMap = ref<Record<number, StageOutputStateMdl>>({});
const workLogMap = ref<Record<number, StageWorkLogGroupMdl>>({});
const logTabMap = ref<Record<number, "stage" | "daily">>({});
const activeLogStageId = ref<number | null>(null);
const workLogForm = reactive({
  logDate: "",
  logContent: "",
});
const getProjectTypeStorageKey = () => `workProjectProjectType:${projectID.value}`;
const getServiceItemsStorageKey = () => `workProjectServiceItems:${projectID.value}`;
const getServiceProductsStorageKey = () => `workProjectServiceProducts:${projectID.value}`;
const ensureServiceItemSelection = () => {
  if (!selectedProjectType.value) return;
  const availableIds = availableServiceItems.value.map((item) => item.serviceItemId);
  const filtered = selectedServiceItemIds.value.filter((id) => availableIds.includes(id));
  if (filtered.length > 0) {
    if (filtered.length !== selectedServiceItemIds.value.length) {
      selectedServiceItemIds.value = filtered;
    }
    return;
  }
  if (selectedProjectType.value.category === "single") {
    const firstServiceItem = availableServiceItems.value[0];
    selectedServiceItemIds.value = firstServiceItem ? [firstServiceItem.serviceItemId] : [];
  } else {
    selectedServiceItemIds.value = availableIds;
  }
};

/** 工作-專案管理-里程碑-頁面模型 */
const workProjectStoneListViewObj = reactive<WorkProjectStoneListViewMdl>({
  projectStoneItemList: [],
});

/** 工作-專案管理-工項-頁面物件 */
const workProjectTaskListViewObj = reactive<WorkProjectTaskStoneListViewMdl>({
  employeeProjectStoneList: [],
});

/** 工作-專案管理-收支-頁面物件 */
const workProjectExpenseListViewObj = reactive<WorkProjectExpenseListViewMdl>({
  projectExpenseList: [],
  eipProjectExpenseList: [],
  eipProjectTravelExpenseList: [],
});

type ProjectRiskItem = {
  id: number;
  title: string;
  note: string;
  createdAt: string;
  createdBy: string;
};

const projectRiskList = ref<ProjectRiskItem[]>([]);
const projectRiskDraft = reactive({
  title: "",
  note: "",
});

/** 工作-專案管理-更新-驗證物件 */
const wrkProjectUpdateValidateObj = reactive<WorkProjectUpdateValidateMdl>({
  employeeProjectName: true,
  employeeProjectCode: true,
});

/** 工作-專案管理-更新-原始物件 */
const wrkProjectUpdateOriginalObj = reactive<WorkProjectUpdateOriginalMdl>({
  employeeProjectName: "",
  employeeProjectCode: "",
});
//#endregion

//#region UI行為 / 畫面處理
/** 切換 Tab */
const changeTab = (tab: PipelineTabEnum) => {
  activeTab.value = tab;
  if (tab === PipelineTabEnum.Project) {
    getWorkProjectData();
  } else if (tab === PipelineTabEnum.Template) {
    templateSettings.value = loadProjectTemplateSettings();
  } else if (tab === PipelineTabEnum.ProjectStone) {
    // 里程碑改為依標準階段衍生，不需額外 API
  } else if (tab === PipelineTabEnum.ProjectStoneTask) {
    getManyProjectTaskList(projectID.value);
  } else if (tab === PipelineTabEnum.Poc) {
    hydratePocBranch();
  } else if (tab === PipelineTabEnum.Expense) {
    getExpenseData();
  } else if (tab === PipelineTabEnum.Risk) {
    // 風險管理暫為前端草稿清單
  }
};

const addProjectRisk = () => {
  const title = projectRiskDraft.title.trim();
  if (!title) {
    displayError("請輸入風險事項");
    return;
  }
  projectRiskList.value.unshift({
    id: Date.now(),
    title,
    note: projectRiskDraft.note.trim() || "-",
    createdAt: new Date().toISOString(),
    createdBy: employeeInfoStore.employeeName || "未登入使用者",
  });
  projectRiskDraft.title = "";
  projectRiskDraft.note = "";
  displaySuccess("已新增風險");
};

const toggleServiceItem = (serviceItemId: number) => {
  if (!selectedProjectType.value) return;
  if (selectedProjectType.value.category === "single") {
    selectedServiceItemIds.value = [serviceItemId];
    return;
  }
  const set = new Set(selectedServiceItemIds.value);
  if (set.has(serviceItemId)) {
    set.delete(serviceItemId);
  } else {
    set.add(serviceItemId);
  }
  selectedServiceItemIds.value = Array.from(set);
};

/** 計算總支出費用 (僅計算"簽核完成"的費用) */
const totalExpense = computed(() => {
  let total = 0;

  // 專案收支(僅計算"實際金額"的費用)
  if (workProjectExpenseListViewObj.projectExpenseList) {
    workProjectExpenseListViewObj.projectExpenseList.forEach((item) => {
      total += item.employeeProjectExpenseActualAmount || 0;
    });
  }

  // 專案支出
  if (workProjectExpenseListViewObj.eipProjectExpenseList) {
    workProjectExpenseListViewObj.eipProjectExpenseList.forEach((item) => {
      if (item.projectExpenseApprovalStatus === "簽核完成") {
        total += item.projectExpenseAmount;
      }
    });
  }

  // 專案旅費
  if (workProjectExpenseListViewObj.eipProjectTravelExpenseList) {
    workProjectExpenseListViewObj.eipProjectTravelExpenseList.forEach((item) => {
      if (item.projectTravelExpenseApprovalStatus === "簽核完成") {
        total += item.projectTravelExpenseTotalAmount || 0;
      }
    });
  }

  return total;
});

/** 已存在的專案成員清單 (送至新增成員元件用於判斷是否重複新增) */
const existMemberList = computed(() => {
  return workProjectDetailViewObj.employeeProjectMemberList.map((m) => ({
    employeeID: m.employeeID ?? 0,
    employeeProjectMemberRole: m.employeeProjectMemberRole,
  }));
});

/** 取得【前N日通知】日期顯示頁面 */
const getNotifyDate = (endTime: string, preDays: number) => {
  if (!endTime) return "-";

  const date = new Date(endTime);
  date.setDate(date.getDate() - (preDays ?? 0));

  return formatDate(date.toISOString());
};

/** 取得指派狀態樣式 */
const getStatusClass = (status: DbAtomEmployeeProjectStatusEnum | null) => {
  switch (status) {
    case DbAtomEmployeeProjectStatusEnum.Completed:
      return "bg-blue-200 text-blue-800";
    case DbAtomEmployeeProjectStatusEnum.OnSchedule:
      return "bg-green-200 text-green-800";
    case DbAtomEmployeeProjectStatusEnum.AtRisk:
      return "bg-yellow-200 text-yellow-800";
    case DbAtomEmployeeProjectStatusEnum.Delayed:
      return "bg-red-400 text-red-800";
    default:
      return "bg-gray-400 text-white";
  }
};

const loadChangeRequests = () => {
  const raw = localStorage.getItem(changeRequestStorageKey);
  if (!raw) {
    changeRequests.value = [];
    return;
  }
  try {
    const parsed = JSON.parse(raw);
    changeRequests.value = Array.isArray(parsed) ? parsed : [];
  } catch {
    changeRequests.value = [];
  }
};

const writeChangeRequests = (nextList: WorkProjectChangeRequestItem[]) => {
  changeRequests.value = nextList;
  localStorage.setItem(changeRequestStorageKey, JSON.stringify(nextList));
  window.dispatchEvent(new CustomEvent(changeRequestEventName));
};

const getChangeRequestTypeLabel = (type: WorkProjectChangeRequestItem["requestType"]) =>
  changeRequestTypes.find((item) => item.value === type)?.label || "其他";

const getChangeRequestStatusLabel = (status: WorkProjectChangeRequestItem["status"]) =>
  status === "approved" ? "已核可" : status === "rejected" ? "已退回" : "待核可";

const getChangeRequestStatusClass = (status: WorkProjectChangeRequestItem["status"]) =>
  status === "approved"
    ? "bg-emerald-50 text-emerald-700"
    : status === "rejected"
      ? "bg-rose-50 text-rose-700"
      : "bg-amber-50 text-amber-700";

const openChangeRequestForm = () => {
  showChangeRequestForm.value = true;
  if (!changeRequestForm.targetRegion) {
    changeRequestForm.targetRegion = employeeInfoStore.managerRegionName || "北區";
  }
};

const setChangeRequestType = (value: WorkProjectChangeRequestItem["requestType"]) => {
  changeRequestForm.requestType =
    changeRequestForm.requestType === value ? null : value;
  if (changeRequestForm.requestType !== "cross_support") {
    changeRequestForm.targetRegion = "";
    changeRequestForm.targetDepartment = "";
  } else if (!changeRequestForm.targetRegion) {
    changeRequestForm.targetRegion = employeeInfoStore.managerRegionName || "北區";
  }
  if (changeRequestForm.requestType !== "task_adjust") {
    changeRequestForm.targetStageName = "";
    changeRequestForm.estimatedHours = "";
    changeRequestForm.onsiteHours = "";
    changeRequestForm.offsiteHours = "";
  }
};

const setChangeRequestTargetRegion = (value: string) => {
  const nextValue = changeRequestForm.targetRegion === value ? "" : value;
  changeRequestForm.targetRegion = nextValue;
  changeRequestForm.targetDepartment = "";
};

const changeRequestDepartmentOptions = computed(() => {
  if (!changeRequestForm.targetRegion || changeRequestForm.targetRegion === "跨區") {
    return orgDepartmentDirectory;
  }
  const regionPrefix = changeRequestForm.targetRegion.startsWith("北")
    ? "北"
    : changeRequestForm.targetRegion.startsWith("中")
      ? "中"
      : "南";
  const departments = orgMemberDirectory
    .filter((item) => item.regionName.startsWith(regionPrefix))
    .map((item) => item.departmentName);
  const unique = Array.from(new Set(departments));
  return unique.filter((name) => orgDepartmentDirectory.includes(name));
});

const submitChangeRequest = () => {
  if (!changeRequestForm.requestType) {
    displayError("請選擇異動類型");
    return;
  }
  if (
    changeRequestForm.requestType === "task_adjust" &&
    !changeRequestForm.targetStageName.trim()
  ) {
    displayError("請填寫欲異動的階段或工項名稱");
    return;
  }
  if (!changeRequestForm.reason.trim()) {
    displayError("請填寫異動原因/說明");
    return;
  }
  if (
    changeRequestForm.requestType === "cross_support" &&
    (!changeRequestForm.targetRegion || !changeRequestForm.targetDepartment)
  ) {
    displayError("跨部門/跨區支援需填寫目標區域與部門");
    return;
  }

  const parseHour = (value: string) => {
    if (!value) return null;
    const parsed = Number(value);
    return Number.isFinite(parsed) ? parsed : null;
  };
  const nextItem: WorkProjectChangeRequestItem = {
    requestId: Date.now(),
    projectId: projectID.value,
    projectName: workProjectDetailViewObj.employeeProjectName || "未命名專案",
    companyName: workProjectDetailViewObj.managerCompanyName || "",
    requesterName: employeeInfoStore.employeeName || "未登入使用者",
    requesterRole: employeeInfoStore.effectiveRoleName || "未指定角色",
    requesterDepartment: employeeInfoStore.managerDepartmentName || "未指定部門",
    requesterRegion: employeeInfoStore.managerRegionName || "未指定區域",
    requestType: changeRequestForm.requestType,
    targetRegion:
      changeRequestForm.requestType === "cross_support"
        ? changeRequestForm.targetRegion
        : undefined,
    targetDepartment:
      changeRequestForm.requestType === "cross_support"
        ? changeRequestForm.targetDepartment.trim()
        : undefined,
    targetStageName:
      changeRequestForm.requestType === "task_adjust"
        ? changeRequestForm.targetStageName.trim()
        : undefined,
    estimatedHours:
      changeRequestForm.requestType === "task_adjust"
        ? parseHour(changeRequestForm.estimatedHours)
        : undefined,
    onsiteHours:
      changeRequestForm.requestType === "task_adjust"
        ? parseHour(changeRequestForm.onsiteHours)
        : undefined,
    offsiteHours:
      changeRequestForm.requestType === "task_adjust"
        ? parseHour(changeRequestForm.offsiteHours)
        : undefined,
    reason: changeRequestForm.reason.trim(),
    status: "pending",
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  };
  writeChangeRequests([nextItem, ...changeRequests.value]);
  changeRequestForm.reason = "";
  changeRequestForm.requestType = null;
  changeRequestForm.targetRegion = "";
  changeRequestForm.targetDepartment = "";
  changeRequestForm.targetStageName = "";
  changeRequestForm.estimatedHours = "";
  changeRequestForm.onsiteHours = "";
  changeRequestForm.offsiteHours = "";
  displaySuccess("已送出異動申請，等待主管核可");
};

const canEditStageOutputs = computed(() =>
  projectMemberRoleStore.canShow([
    DbEmployeeProjectMemberRoleEnum.ProjectManager,
    DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
  ])
);

type CheckInLog = {
  id: number;
  checkedAt: string;
  stageName: string;
  taskName: string;
  location: string;
  transport: string;
  projectName?: string;
  companyName?: string;
};

const checkInStorageKey = computed(() => `cache.work.project.checkins.v2.${projectID.value}`);
const checkInLogs = ref<CheckInLog[]>([]);
const showCheckInForm = ref(false);
const checkInForm = reactive({
  checkedAt: "",
  stageName: "",
  taskName: "",
  location: "",
  transport: "",
});

const checkInStageOptions = computed(() =>
  derivedStageMilestones.value.map((stage) => stage.stageName)
);

const buildDefaultCheckIns = (): CheckInLog[] => [];

const loadCheckInLogs = () => {
  const raw = localStorage.getItem(checkInStorageKey.value);
  if (raw) {
    try {
      const parsed = JSON.parse(raw);
      checkInLogs.value = Array.isArray(parsed) ? parsed : [];
      return;
    } catch {
      checkInLogs.value = [];
    }
  }
  const projectName = workProjectDetailViewObj.employeeProjectName.trim();
  const companyName = workProjectDetailViewObj.managerCompanyName?.trim() || "";
  if (projectName) {
    const matchedLogs: CheckInLog[] = [];
    for (let i = 0; i < localStorage.length; i += 1) {
      const key = localStorage.key(i);
      if (!key || !key.startsWith("cache.work.project.checkins.v2.")) continue;
      const value = localStorage.getItem(key);
      if (!value) continue;
      try {
        const parsed = JSON.parse(value);
        if (!Array.isArray(parsed)) continue;
        for (const log of parsed) {
          if (log?.projectName === projectName) {
            matchedLogs.push(log as CheckInLog);
          }
        }
      } catch {
        continue;
      }
    }
    if (matchedLogs.length > 0) {
      checkInLogs.value = matchedLogs;
      return;
    }
  }
  if (companyName) {
    const matchedLogs: CheckInLog[] = [];
    for (let i = 0; i < localStorage.length; i += 1) {
      const key = localStorage.key(i);
      if (!key || !key.startsWith("cache.work.project.checkins.v2.")) continue;
      const value = localStorage.getItem(key);
      if (!value) continue;
      try {
        const parsed = JSON.parse(value);
        if (!Array.isArray(parsed)) continue;
        for (const log of parsed) {
          if (log?.companyName === companyName) {
            matchedLogs.push(log as CheckInLog);
          }
        }
      } catch {
        continue;
      }
    }
    if (matchedLogs.length > 0) {
      checkInLogs.value = matchedLogs;
      return;
    }
  }
  checkInLogs.value = buildDefaultCheckIns();
};

const persistCheckInLogs = () => {
  localStorage.setItem(checkInStorageKey.value, JSON.stringify(checkInLogs.value));
};

const openCheckInForm = () => {
  showCheckInForm.value = true;
  if (!checkInForm.checkedAt) {
    const now = new Date();
    const iso = now.toISOString().slice(0, 16).replace("T", " ");
    checkInForm.checkedAt = iso;
  }
  if (!checkInForm.stageName && checkInStageOptions.value.length > 0) {
    checkInForm.stageName = checkInStageOptions.value[0];
  }
};

const resetCheckInForm = () => {
  checkInForm.checkedAt = "";
  checkInForm.stageName = "";
  checkInForm.taskName = "";
  checkInForm.location = "";
  checkInForm.transport = "";
};

const cancelCheckInForm = () => {
  showCheckInForm.value = false;
  resetCheckInForm();
};

const saveCheckIn = () => {
  if (!checkInForm.checkedAt) return;
  const next: CheckInLog = {
    id: Date.now(),
    checkedAt: checkInForm.checkedAt,
    stageName: checkInForm.stageName || "-",
    taskName: checkInForm.taskName || "-",
    location: checkInForm.location || "-",
    transport: checkInForm.transport || "-",
  };
  checkInLogs.value = [next, ...checkInLogs.value];
  persistCheckInLogs();
  showCheckInForm.value = false;
  resetCheckInForm();
};

const handleCheckInUpdated = () => {
  loadCheckInLogs();
};

const handleFileReviewEvent = () => {
  loadFileReviews();
};

const handleCheckInStorage = (event: StorageEvent) => {
  if (!event.key) return;
  if (event.key.startsWith("cache.work.project.checkins.v2.")) {
    loadCheckInLogs();
  }
};

const derivedStageMilestones = computed(() => {
  const milestones: { stageId: number; stageName: string; status: string }[] = [];
  const seen = new Set<number>();
  for (const template of selectedStageTemplates.value) {
    for (const stage of template.stages) {
      if (seen.has(stage.stageId)) continue;
      seen.add(stage.stageId);
      const requiredOutputs = stage.requiredOutputs.filter((item) => item.required);
      const hasUploads = stage.requiredOutputs.some(
        (item) => getOutputState(item.outputId).files.length > 0
      );
      const allRequiredUploaded =
        requiredOutputs.length > 0 &&
        requiredOutputs.every((item) => getOutputState(item.outputId).files.length > 0);
      const status = allRequiredUploaded ? "完成" : hasUploads ? "準備開始" : "未開始";
      milestones.push({ stageId: stage.stageId, stageName: stage.stageName, status });
    }
  }
  return milestones;
});
const milestoneTimeline = computed(() => {
  const total = derivedStageMilestones.value.length;
  return derivedStageMilestones.value.map((milestone, index) => {
    const widthPercent = total > 0 ? 100 / total : 100;
    const startPercent = total > 0 ? (index / total) * 100 : 0;
    return {
      ...milestone,
      startPercent,
      widthPercent,
      isLast: index === total - 1,
    };
  });
});
const currentStageId = computed(() => {
  const milestones = derivedStageMilestones.value;
  if (milestones.length === 0) return null;
  const active = milestones.find((milestone) => milestone.status !== "完成");
  return (active || milestones[milestones.length - 1]).stageId;
});
const currentStageName = computed(() => {
  const milestones = derivedStageMilestones.value;
  if (milestones.length === 0) return "-";
  const active = milestones.find((milestone) => milestone.status !== "完成");
  return (active || milestones[milestones.length - 1]).stageName || "-";
});
const getMilestoneStatusClass = (status: string) => {
  if (status === "完成") return "bg-emerald-500";
  if (status === "準備開始") return "bg-amber-400";
  return "bg-gray-300";
};
const getMilestoneTextClass = (status: string) => {
  if (status === "完成") return "text-emerald-600";
  if (status === "準備開始") return "text-amber-600";
  return "text-gray-500";
};

const getOutputState = (outputId: number) => {
  if (!outputStateMap.value[outputId]) {
    outputStateMap.value[outputId] = {
      outputId,
      updatedAt: "",
      files: [],
    };
  }
  return outputStateMap.value[outputId];
};

const handleOutputFileChange = (outputId: number, files: FileList | null) => {
  if (!files || files.length === 0) return;
  const state = getOutputState(outputId);
  const nextId =
    Math.max(0, ...state.files.map((file) => file.fileId)) + 1;
  const now = new Date().toISOString();

  Array.from(files).forEach((file, index) => {
    state.files.unshift({
      fileId: nextId + index,
      fileName: file.name,
      fileSize: file.size,
      fileType: file.type || "unknown",
      uploadedAt: now,
    });
  });
  state.updatedAt = now;
};

const removeOutputFile = (outputId: number, fileId: number) => {
  const state = getOutputState(outputId);
  state.files = state.files.filter((file) => file.fileId !== fileId);
};

const getOutputStatusLabel = (output: StageOutputStateMdl, required: boolean) => {
  if (output.files.length > 0) return "已上傳";
  return required ? "未上傳" : "選填";
};

const getOutputStatusClass = (output: StageOutputStateMdl, required: boolean) => {
  if (output.files.length > 0) return "bg-green-100 text-green-700";
  return required ? "bg-amber-100 text-amber-700" : "bg-gray-100 text-gray-600";
};

const fileReviewStorageKey = "cache.work.project.fileReviews";
const fileReviewEventName = "work-project-file-reviews-updated";
const fileReviews = ref<StageOutputReviewItemMdl[]>([]);

const normalizeFileReview = (item: any): StageOutputReviewItemMdl => {
  const updatedAt = item.updatedAt || item.reviewedAt || item.uploadedAt || new Date().toISOString();
  if (item.assistantStatus && item.managerStatus) {
    return { ...item, updatedAt } as StageOutputReviewItemMdl;
  }
  return {
    reviewId: item.reviewId || Date.now(),
    projectId: item.projectId || 0,
    projectName: item.projectName || "未命名專案",
    stageId: item.stageId || 0,
    stageName: item.stageName || "",
    outputId: item.outputId || 0,
    outputName: item.outputName || item.fileName || "",
    fileName: item.fileName || "",
    fileUrl: item.fileUrl || "",
    uploadedBy: item.uploadedBy || "專案成員",
    uploadedAt: item.uploadedAt || updatedAt,
    assistantStatus: "not_required",
    managerStatus: item.status || "pending",
    overallStatus: item.status || "pending",
    updatedAt,
    reviewTarget: "manager",
  };
};

const readFileReviews = (): StageOutputReviewItemMdl[] => {
  const raw = localStorage.getItem(fileReviewStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed.map(normalizeFileReview) : [];
  } catch {
    return [];
  }
};

const persistFileReviews = (nextList: StageOutputReviewItemMdl[]) => {
  fileReviews.value = nextList;
  localStorage.setItem(fileReviewStorageKey, JSON.stringify(nextList));
  window.dispatchEvent(new CustomEvent(fileReviewEventName));
};

const loadFileReviews = () => {
  fileReviews.value = readFileReviews();
};

const getLatestReviewForOutput = (outputId: number) => {
  const items = fileReviews.value.filter(
    (item) => item.projectId === projectID.value && item.outputId === outputId
  );
  if (items.length === 0) return null;
  return items.sort(
    (a, b) => new Date(b.updatedAt).getTime() - new Date(a.updatedAt).getTime()
  )[0];
};

const getReviewStatusLabel = (status: ReviewStatus) => {
  switch (status) {
    case "pending":
      return "待審";
    case "waiting":
      return "待助理";
    case "approved":
      return "已核可";
    case "rejected":
      return "已退回";
    case "not_required":
      return "不需審";
    default:
      return "未送審";
  }
};

const getReviewStatusClass = (status: ReviewStatus) => {
  switch (status) {
    case "pending":
      return "bg-amber-50 text-amber-700";
    case "waiting":
      return "bg-sky-50 text-sky-700";
    case "approved":
      return "bg-emerald-50 text-emerald-700";
    case "rejected":
      return "bg-rose-50 text-rose-700";
    case "not_required":
      return "bg-gray-100 text-gray-600";
    default:
      return "bg-gray-100 text-gray-600";
  }
};

const getOutputReviewState = (outputId: number) => {
  const item = getLatestReviewForOutput(outputId);
  if (!item) {
    return {
      assistantStatus: "not_sent" as ReviewStatus,
      managerStatus: "not_sent" as ReviewStatus,
      overallStatus: "not_sent" as ReviewStatus,
      lastFeedback: "",
    };
  }
  return {
    assistantStatus: item.assistantStatus,
    managerStatus: item.managerStatus,
    overallStatus: item.overallStatus,
    lastFeedback: item.lastFeedback || item.managerFeedback || item.assistantFeedback || "",
  };
};

const summarizeStatuses = (statuses: ReviewStatus[]) => {
  if (statuses.length === 0) return "not_sent" as ReviewStatus;
  if (statuses.includes("rejected")) return "rejected" as ReviewStatus;
  if (statuses.includes("pending") || statuses.includes("waiting")) return "pending" as ReviewStatus;
  const allApproved = statuses.every((status) => ["approved", "not_required"].includes(status));
  if (allApproved) return "approved" as ReviewStatus;
  const allNotRequired = statuses.every((status) => status === "not_required");
  if (allNotRequired) return "not_required" as ReviewStatus;
  return "not_sent" as ReviewStatus;
};

const getStageReviewSummary = (stage: StageTemplateStageMdl) => {
  const reviewStates = stage.requiredOutputs.map((output) =>
    getOutputReviewState(output.outputId)
  );
  return {
    assistantStatus: summarizeStatuses(reviewStates.map((state) => state.assistantStatus)),
    managerStatus: summarizeStatuses(reviewStates.map((state) => state.managerStatus)),
    overallStatus: summarizeStatuses(reviewStates.map((state) => state.overallStatus)),
  };
};

const canSendOutputReview = (outputId: number) => {
  const reviewState = getOutputReviewState(outputId);
  return (
    getOutputState(outputId).files.length > 0 &&
    (reviewState.overallStatus === "not_sent" || reviewState.overallStatus === "rejected")
  );
};

const sendOutputReview = (
  output: StageTemplateOutputMdl,
  stage: StageTemplateStageMdl,
  target: "assistant" | "manager"
) => {
  const latestFile = getOutputState(output.outputId).files[0];
  const now = new Date().toISOString();
  const nextItem: StageOutputReviewItemMdl = {
    reviewId: Date.now(),
    projectId: projectID.value,
    projectName: workProjectDetailViewObj.employeeProjectName || "未命名專案",
    stageId: stage.stageId,
    stageName: stage.stageName,
    outputId: output.outputId,
    outputName: output.outputName,
    fileName: latestFile?.fileName || output.outputName,
    fileUrl: "",
    uploadedBy: employeeInfoStore.employeeName || "專案成員",
    uploadedAt: latestFile?.uploadedAt || now,
    assistantStatus: target === "assistant" ? "pending" : "not_required",
    managerStatus: target === "assistant" ? "waiting" : "pending",
    overallStatus: "pending",
    assistantFeedback: "",
    managerFeedback: "",
    lastFeedback: "",
    updatedAt: now,
    reviewTarget: target,
  };
  const filtered = fileReviews.value.filter(
    (item) => !(item.projectId === projectID.value && item.outputId === output.outputId)
  );
  persistFileReviews([...filtered, nextItem]);
};

const getActiveLogTab = (stageId: number) => logTabMap.value[stageId] ?? "stage";

const setActiveLogTab = (stageId: number, tab: "stage" | "daily") => {
  logTabMap.value = { ...logTabMap.value, [stageId]: tab };
};

const getWorkLogList = (stageId: number, tab: "stage" | "daily") => {
  const group = workLogMap.value[stageId];
  if (!group) return [];
  return tab === "stage" ? group.stageLogs : group.dailyLogs;
};

const ensureWorkLogGroup = (stageId: number) => {
  if (!workLogMap.value[stageId]) {
    workLogMap.value[stageId] = { stageLogs: [], dailyLogs: [] };
  }
  return workLogMap.value[stageId];
};

const openWorkLogForm = (stageId: number) => {
  activeLogStageId.value = stageId;
  workLogForm.logDate = new Date().toISOString().slice(0, 10);
  workLogForm.logContent = "";
};

const cancelWorkLogForm = () => {
  activeLogStageId.value = null;
  workLogForm.logDate = "";
  workLogForm.logContent = "";
};

const saveWorkLog = (stageId: number) => {
  const content = workLogForm.logContent.trim();
  if (!content) return;
  const logTab = getActiveLogTab(stageId);
  const group = ensureWorkLogGroup(stageId);
  const logList = logTab === "stage" ? group.stageLogs : group.dailyLogs;
  const newId = Math.max(0, ...logList.map((log) => log.logId)) + 1;
  logList.unshift({
    logId: newId,
    logDate: workLogForm.logDate || new Date().toISOString().slice(0, 10),
    logContent: content,
    createdAt: new Date().toISOString(),
  });
  if (logTab === "stage") {
    group.stageLogs = logList;
  } else {
    group.dailyLogs = logList;
  }
  workLogMap.value[stageId] = group;
  cancelWorkLogForm();
};

/** 打開 URL 連結 */
const openUrl = (url: string | null) => {
  if (url) {
    window.open(url, "_blank");
  }
};
//#endregion

//#region 驗證相關
/** 驗證【更新專案】欄位 */
const validateUpdateProjectField = () => {
  let isValid = true;

  // 專案名稱（必填、50字內）
  const name = workProjectDetailViewObj.employeeProjectName.trim();

  wrkProjectUpdateValidateObj.employeeProjectName = name.length > 0 && name.length <= 50;

  if (!wrkProjectUpdateValidateObj.employeeProjectName) {
    isValid = false;
  }

  // 專案代碼（必填、50字內）
  const code = workProjectDetailViewObj.employeeProjectCode.trim();

  wrkProjectUpdateValidateObj.employeeProjectCode = code.length > 0 && code.length <= 50;

  if (!wrkProjectUpdateValidateObj.employeeProjectCode) {
    isValid = false;
  }

  return isValid;
};

/** 重設【專案資訊】驗證狀態 */
const resetProjectUpdateValidate = () => {
  wrkProjectUpdateValidateObj.employeeProjectName = true;
  wrkProjectUpdateValidateObj.employeeProjectCode = true;
};
//#endregion

//#region API / 資料流程
/** 取得【專案管理】資料 */
const getWorkProjectData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsWrkPrjHttpGetProjectReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: projectID.value,
  };
  const responseData: MbsWrkPrjHttpGetProjectRspMdl | null = await getProject(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    if (responseData.errorCode === MbsErrorCodeEnum.PermissionDenied) {
      isPermissionError.value = true;
    }
    return;
  }

  //設定資料
  workProjectDetailViewObj.atomEmployeeProjectStatus = responseData.atomEmployeeProjectStatus;
  workProjectDetailViewObj.employeeProjectCode = responseData.employeeProjectCode;
  workProjectDetailViewObj.employeeProjectContractUrl = responseData.employeeProjectContractUrl;
  workProjectDetailViewObj.employeeProjectContractCreateTime =
    responseData.employeeProjectContractCreateTime;
  workProjectDetailViewObj.employeeProjectWorkUrl = responseData.employeeProjectWorkUrl;
  workProjectDetailViewObj.employeeProjectWorkCreateTime =
    responseData.employeeProjectWorkCreateTime;
  workProjectDetailViewObj.employeeProjectName = responseData.employeeProjectName;
  workProjectDetailViewObj.employeeProjectStartTime = formatDate(
    responseData.employeeProjectStartTime
  );
  workProjectDetailViewObj.employeeProjectEndTime = formatDate(responseData.employeeProjectEndTime);
  workProjectDetailViewObj.managerCompanyName = responseData.managerCompanyName;
  const mappedMembers = responseData.employeeProjectMemberList.map(
    (item) =>
      ({
        employeeProjectMemberID: item.employeeProjectMemberID,
        employeeProjectMemberRole: item.employeeProjectMemberRole,
        managerRegionName: item.managerRegionName,
        managerDepartmentName: item.managerDepartmentName,
        employeeName: item.employeeName,
        employeeID: item.employeeID,
      }) satisfies WorkPrjListItemMdl
  );

  const hasSales = mappedMembers.some(
    (item) => item.employeeProjectMemberRole === DbEmployeeProjectMemberRoleEnum.Saler
  );
  if (!hasSales) {
    mappedMembers.unshift({
      employeeProjectMemberID: -2,
      employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum.Saler,
      managerRegionName: "全區",
      managerDepartmentName: "業務部",
      employeeName: "業務",
      employeeID: 0,
    });
  }

  workProjectDetailViewObj.employeeProjectMemberList = mappedMembers.sort(
    (a, b) => (a.employeeProjectMemberRole ?? 0) - (b.employeeProjectMemberRole ?? 0)
  );

  //儲存專案起始時間結束時間 到 store
  projectTimeInfoStore.setProjectTime({
    employeeProjectName: responseData.employeeProjectName,
    employeeProjectStartTime: responseData.employeeProjectStartTime,
    employeeProjectEndTime: responseData.employeeProjectEndTime,
  });

  buildStageOwnerMap();
};

/** 取得【專案里程碑】列表 */
const getManyProjectStoneList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsWrkPrjHttpGetManyProjectStoneReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: projectID.value,
  };
  const responseData: MbsWrkPrjHttpGetManyProjectStoneRspMdl | null =
    await getManyProjectStone(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  //設定資料
  workProjectStoneListViewObj.projectStoneItemList = responseData.employeeProjectStoneList.map(
    (item) => ({
      //里程碑
      employeeProjectStoneID: item.employeeProjectStoneID,
      employeeProjectStoneName: item.employeeProjectStoneName,
      employeeProjectStoneStartTime: item.employeeProjectStoneStartTime,
      employeeProjectStoneEndTime: item.employeeProjectStoneEndTime,
      employeeProjectStonePreNotifyDay: item.employeeProjectStonePreNotifyDay,
      atomEmployeeProjectStatus: item.atomEmployeeProjectStatus,
      isOpen: false,
      // 工項列表
      employeeProjectStoneTaskList: item.employeeProjectStoneTaskList.map((task) => ({
        employeeProjectStoneTaskID: task.employeeProjectStoneTaskID,
        employeeProjectStoneTaskName: task.employeeProjectStoneTaskName,
        employeeProjectStoneTaskStartTime: task.employeeProjectStoneTaskStartTime,
        employeeProjectStoneTaskEndTime: task.employeeProjectStoneTaskEndTime,
        atomEmployeeProjectStatus: task.atomEmployeeProjectStatus,
        employeeProjectStoneTaskExecutorCount: task.employeeProjectStoneTaskExecutorCount,
      })),
    })
  );
};

/** 取得【專案工項】列表 */
const getManyProjectTaskList = async (employeeProjectID: number) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsWrkPrjHttpGetManyProjectStoneTaskReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: employeeProjectID,
  };
  const responseData: MbsWrkPrjHttpGetManyProjectStoneTaskRspMdl | null =
    await getManyProjectStoneTask(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  //設定資料
  workProjectTaskListViewObj.employeeProjectStoneList = responseData.employeeProjectStoneList.map(
    (item) => ({
      employeeProjectStoneID: item.employeeProjectStoneID,
      employeeProjectStoneName: item.employeeProjectStoneName,
      employeeProjectStonePreNotifyDay: item.employeeProjectStonePreNotifyDay,
      employeeProjectStoneStartTime: item.employeeProjectStoneStartTime,
      employeeProjectStoneEndTime: item.employeeProjectStoneEndTime,
      atomEmployeeProjectStatus: item.atomEmployeeProjectStatus,
      employeeProjectStoneTaskList: item.employeeProjectStoneTaskList.map((task) => ({
        employeeProjectStoneTaskID: task.employeeProjectStoneTaskID,
        employeeProjectStoneTaskName: task.employeeProjectStoneTaskName,
        employeeProjectStoneTaskStartTime: task.employeeProjectStoneTaskStartTime,
        employeeProjectStoneTaskEndTime: task.employeeProjectStoneTaskEndTime,
        employeeProjectStoneTaskWorkHour: task.employeeProjectStoneTaskWorkHour,
        atomEmployeeProjectStatus: task.atomEmployeeProjectStatus,
        employeeProjectStoneTaskExecutorCount: task.employeeProjectStoneTaskExecutorCount,
        employeeProjectStoneTaskBucketList: task.employeeProjectStoneTaskBucketList.map(
          (bucket) => ({
            employeeProjectStoneTaskBucketID: bucket.employeeProjectStoneTaskBucketID,
            employeeProjectStoneTaskBucketName: bucket.employeeProjectStoneTaskBucketName,
            employeeProjectStoneTaskBucketIsFinish: bucket.employeeProjectStoneTaskBucketIsFinish,
          })
        ),
      })),
    })
  );
};

/** 取得多筆專案成員角色(判斷頁面顯示) */
const getManyProjectMemberRoleList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsWrkPrjHttpGetManyMemberRoleReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: projectID.value,
  };
  const responseData: MbsWrkPrjHttpGetManyMemberRoleRspMdl | null =
    await getManyProjectMemberRole(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  //儲存資料至 store
  projectMemberRoleStore.setMemberRoleList(responseData.employeeProjectMemberList);
};

/** 取得【專案收支】資料 */
const getExpenseData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 檢查專案 ID
  if (!projectID.value) return;

  // 呼叫 api
  const requestData: MbsWrkPrjHttpGetManyExpenseReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: projectID.value,
  };
  const responseData: MbsWrkPrjHttpGetManyExpenseRspMdl | null = await getManyExpense(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 更新頁面物件
  workProjectExpenseListViewObj.projectExpenseList = responseData.employeeProjectExpenseList || [];
  workProjectExpenseListViewObj.eipProjectExpenseList = responseData.eipProjectExpenseList || [];
  workProjectExpenseListViewObj.eipProjectTravelExpenseList =
    responseData.eipProjectTravelExpenseList || [];
};

/** 取得【專案】變更欄位 */
const getProjectChangedFields = (): Partial<MbsWrkPrjHttpUpdateProjectReqMdl> => {
  const changes: Partial<MbsWrkPrjHttpUpdateProjectReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: projectID.value,
  };

  // 專案名稱
  if (
    workProjectDetailViewObj.employeeProjectName.trim() !==
    wrkProjectUpdateOriginalObj.employeeProjectName
  ) {
    changes.employeeProjectName = workProjectDetailViewObj.employeeProjectName.trim();
  }

  // 專案代碼
  if (
    workProjectDetailViewObj.employeeProjectCode.trim() !==
    wrkProjectUpdateOriginalObj.employeeProjectCode
  ) {
    changes.employeeProjectCode = workProjectDetailViewObj.employeeProjectCode.trim();
  }

  return changes;
};

/** 驗證【專案人員角色完整性】 */
const validateMemberRoleCompletenessAfterDelete = (memberIDToDelete: number): boolean => {
  const currentMembers = workProjectDetailViewObj.employeeProjectMemberList;

  const remainingMembers = currentMembers.filter(
    (m) => m.employeeProjectMemberID !== memberIDToDelete
  );
  const requiredRoles = [
    DbEmployeeProjectMemberRoleEnum.Saler,
    DbEmployeeProjectMemberRoleEnum.ProjectManager,
    DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
    DbEmployeeProjectMemberRoleEnum.Executor,
    DbEmployeeProjectMemberRoleEnum.Assistant,
  ];

  return requiredRoles.every((role) =>
    remainingMembers.some((m) => m.employeeProjectMemberRole === role)
  );
};

/** 處理【移除專案成員】 */
const handleRemoveProjectMember = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 檢查欲刪除的專案成員 ID
  if (!deletingProjectMemberID.value) return;

  // 驗證角色完整性
  if (!validateMemberRoleCompletenessAfterDelete(deletingProjectMemberID.value)) {
    displayError("每個專案角色至少需要保留一位人員！");
    // 關閉 Dialog
    showConfirmProjectMemberDialog.value = false;
    deletingProjectMemberID.value = null;
    return;
  }

  // 呼叫 api
  const requestData: MbsWrkPrjHttpRemoveMemberReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectMemberID: deletingProjectMemberID.value,
  };
  const responseData: MbsWrkPrjHttpRemoveMemberRspMdl | null =
    await removeProjectMember(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉 Dialog
  showConfirmProjectMemberDialog.value = false;
  deletingProjectMemberID.value = null;

  // 顯示成功訊息
  displaySuccess("移除專案成員成功！");

  // 重新取得專案資料
  getWorkProjectData();
};

/** 處理【刪除里程碑】 */
const handleRemoveProjectStone = async () => {
  showActionMenu.value = null;

  // 驗證 token
  if (!requireToken()) return;

  // 檢查欲刪除的里程碑 ID
  if (!deletingProjectStoneID.value) return;

  // 呼叫 api
  const requestData: MbsWrkPrjHttpRemoveProjectStoneReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneID: deletingProjectStoneID.value,
  };
  const responseData: MbsWrkPrjHttpRemoveProjectStoneRspMdl | null =
    await removeProjectStone(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除里程碑成功！");

  // 關閉 Dialog
  showDeleteProjectStoneConfirm.value = false;

  // 重設刪除里程碑 ID
  deletingProjectStoneID.value = null;

  //重新取得專案里程碑資料
  getManyProjectStoneList();
};

/** 勾選子項目清單 */
const toggleBucket = async (bucket: WorkProjectTaskItemMdlBucketItemMdl) => {
  // 驗證 token
  if (!requireToken()) return;

  // 檢查 ID
  if (bucket.employeeProjectStoneTaskBucketID < 0) return;

  //自己先反轉本地值
  bucket.employeeProjectStoneTaskBucketIsFinish = !bucket.employeeProjectStoneTaskBucketIsFinish;

  // 呼叫 API
  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneTaskBucketID: bucket.employeeProjectStoneTaskBucketID,
    employeeProjectStoneTaskBucketIsFinish: bucket.employeeProjectStoneTaskBucketIsFinish,
  };
  const responseData: MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl | null =
    await updateProjectStoneTaskBucket(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新工項清單狀態成功！");

  // 重新取得專案工項資料
  getManyProjectTaskList(projectID.value);
};
//#endregion

//#region 按鈕事件
/** 點擊返回按鈕 */
const clickBackBtn = () => {
  router.push("/work/project");
};

/** 點擊【編輯】按鈕 */
const clickUpdateBtn = () => {
  // 重設驗證狀態
  resetProjectUpdateValidate();
  // 開啟編輯模式
  isEditMode.value = true;
};

/** 點擊【取消編輯】按鈕 */
const clickCancelUpdateBtn = () => {
  // 關閉編輯模式
  isEditMode.value = false;
  // 重設驗證狀態
  resetProjectUpdateValidate();
  // 重新取得專案資料
  getWorkProjectData();
};

/** 點擊提交【專案】按鈕 */
const clickSubmitProjectBtn = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateUpdateProjectField()) return;

  // 取得變更欄位
  const changedFields = getProjectChangedFields();
  if (Object.keys(changedFields).length <= 2) {
    isEditMode.value = false;
    return;
  }

  // 呼叫 api
  const responseData: MbsWrkPrjHttpUpdateProjectRspMdl | null = await updateProject(
    changedFields as MbsWrkPrjHttpUpdateProjectReqMdl
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
  displaySuccess("更新專案資訊成功！");

  // 重新取得資料
  getWorkProjectData();
};

/** 點擊【編輯里程碑】按鈕 */
const clickUpdateProjectStoneBtn = (employeeProjectStoneID: number) => {
  showActionMenu.value = null;
  selectedProjectStoneID.value = employeeProjectStoneID;
  isUpdateProjectStoneShow.value = true;
};

/** 點擊【刪除里程碑】按鈕 */
const clickRemoveProjectStoneBtn = (employeeProjectStoneID: number) => {
  showActionMenu.value = null;
  deletingProjectStoneID.value = employeeProjectStoneID;
  showDeleteProjectStoneConfirm.value = true;
};

/** 點擊【編輯工項】按鈕 */
const clickUpdateProjectTaskBtn = (employeeProjectTaskID: number) => {
  selectedProjectTaskID.value = employeeProjectTaskID;
  isUpdateProjectTaskShow.value = true;
};

/** 點擊操作按鈕 */
const clickActionBtn = (id: number) => {
  showActionMenu.value = showActionMenu.value === id ? null : id;
};

/** 關閉操作選單 */
const closeActionMenu = () => {
  showActionMenu.value = null;
};

/** 處理關閉錯誤訊息 */
const handleCloseError = () => {
  closeError();
  // 權限錯誤則返回列表頁
  if (isPermissionError.value) {
    isPermissionError.value = false;
    router.push("/work/project");
  }
};

//#endregion

//#region 彈跳視窗事件
/** 開啟刪除專案人員確認視窗 */
const openRemoveProjectMemberConfirm = (employeeProjectMemberID: number) => {
  deletingProjectMemberID.value = employeeProjectMemberID;
  showConfirmProjectMemberDialog.value = true;
};

/** 取消刪除專案人員確認視窗 */
const handleCancelRemoveProjectMember = () => {
  showConfirmProjectMemberDialog.value = false;
  deletingProjectMemberID.value = null;
};

/** 處理【取消刪除里程碑】 */
const handleCancelRemoveProjectStone = () => {
  showDeleteProjectStoneConfirm.value = false;
  deletingProjectStoneID.value = null;
};

//#endregion

//#region 生命週期
const loadProjectContext = () => {
  getWorkProjectData();
  getManyProjectMemberRoleList();

  templateSettings.value = loadProjectTemplateSettings();

  const storedProjectTypeId = Number(localStorage.getItem(getProjectTypeStorageKey()));
  if (Number.isFinite(storedProjectTypeId) && storedProjectTypeId > 0) {
    selectedProjectTypeId.value = storedProjectTypeId;
  }

  const storedServiceItemIds = localStorage.getItem(getServiceItemsStorageKey());
  if (storedServiceItemIds) {
    try {
      const parsed = JSON.parse(storedServiceItemIds) as number[];
      selectedServiceItemIds.value = Array.isArray(parsed) ? parsed : [];
    } catch {
      selectedServiceItemIds.value = [];
    }
  }
  const storedServiceProducts = localStorage.getItem(getServiceProductsStorageKey());
  if (storedServiceProducts) {
    try {
      const parsed = JSON.parse(storedServiceProducts) as Record<number, number[]>;
      selectedServiceProductIds.value = parsed && typeof parsed === "object" ? parsed : {};
    } catch {
      selectedServiceProductIds.value = {};
    }
  }

  if (!selectedProjectTypeId.value && projectTypeTemplates.value.length > 0) {
    selectedProjectTypeId.value = projectTypeTemplates.value[0].projectTypeId;
  }
  ensureServiceItemSelection();
  pocBranchForm.projectId = projectID.value;
  hydratePocBranch();
  loadCheckInLogs();
};

const applyTabFromQuery = () => {
  const tab = route.query.tab;
  const pipelineIdParam = Number(route.query.pipelineId);
  if (Number.isFinite(pipelineIdParam) && pipelineIdParam > 0) {
    pocBranchForm.pipelineId = pipelineIdParam;
  }
};

onMounted(() => {
  loadProjectContext();
  loadChangeRequests();
  loadFileReviews();
  applyTabFromQuery();
  window.addEventListener("pwa-checkin-updated", handleCheckInUpdated);
  window.addEventListener("storage", handleCheckInStorage);
  window.addEventListener(fileReviewEventName, handleFileReviewEvent as EventListener);
});

watch(
  () => route.params.id,
  (nextId) => {
    const parsed = Number(nextId);
    if (!Number.isFinite(parsed)) return;
    projectID.value = parsed;
    selectedProjectTypeId.value = null;
    selectedServiceItemIds.value = [];
    selectedServiceProductIds.value = {};
    loadProjectContext();
    loadChangeRequests();
    loadFileReviews();
    applyTabFromQuery();
  }
);

watch(
  () => route.query.tab,
  () => {
    applyTabFromQuery();
  }
);

onMounted(() => {
  document.addEventListener("click", closeActionMenu);
});

onUnmounted(() => {
  window.removeEventListener("pwa-checkin-updated", handleCheckInUpdated);
  window.removeEventListener("storage", handleCheckInStorage);
  window.removeEventListener(fileReviewEventName, handleFileReviewEvent as EventListener);
  document.removeEventListener("click", closeActionMenu);
  clearModuleTitle();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<PipelineTabEnum, string> = {
      [PipelineTabEnum.Project]: "專案資訊",
      [PipelineTabEnum.Template]: "標準階段與產出",
      [PipelineTabEnum.ProjectStone]: "里程碑",
      [PipelineTabEnum.ProjectStoneTask]: "里程碑工項",
      [PipelineTabEnum.Poc]: "POC",
      [PipelineTabEnum.Expense]: "收支",
      [PipelineTabEnum.Risk]: "風險管理",
    };
    setModuleTitle(titleMap[tab] ?? "專案資訊");
  },
  { immediate: true }
);

watch(
  () => [pocBranchForm, pocBranchAssignments],
  () => {
    persistPocBranch();
  },
  { deep: true }
);

watch(
  () => selectedProjectTypeId.value,
  (nextValue) => {
    if (nextValue) {
      localStorage.setItem(getProjectTypeStorageKey(), String(nextValue));
    } else {
      localStorage.removeItem(getProjectTypeStorageKey());
    }

    if (!selectedProjectType.value) {
      selectedServiceItemIds.value = [];
    } else {
      ensureServiceItemSelection();
    }
  }
);

watch(
  () => selectedServiceItemIds.value,
  (nextValue) => {
    if (selectedProjectType.value) {
      ensureServiceItemSelection();
    }
    localStorage.setItem(getServiceItemsStorageKey(), JSON.stringify(nextValue));
  },
  { deep: true }
);

watch(
  () => selectedStageTemplates.value,
  (templates) => {
    const next = new Set<number>();
    templates.forEach((template) => {
      template.stages.forEach((stage) => {
        next.add(stage.stageId);
      });
    });
    expandedStageIds.value = next;
    buildStageOwnerMap();
  },
  { immediate: true }
);
//#endregion
</script>

<template>
  <div class="flex flex-col p-4">
    <!-- 標題列 -->
    <div class="flex items-center flex-row gap-2 justify-between overflow-hidden mb-3">
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

    <!-- Tabs選單 -->
    <div class="flex mb-0 gap-y-4">
      <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.Project }]"
        @click="changeTab(PipelineTabEnum.Project)"
      >
        專案資訊
      </button>
      <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.Template }]"
        @click="changeTab(PipelineTabEnum.Template)"
      >
        標準階段與產出
      </button>
      <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.ProjectStone }]"
        @click="changeTab(PipelineTabEnum.ProjectStone)"
      >
        里程碑
      </button>
      <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.Expense }]"
        @click="changeTab(PipelineTabEnum.Expense)"
      >
        收支
      </button>
      <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.Risk }]"
        @click="changeTab(PipelineTabEnum.Risk)"
      >
        風險管理
      </button>
    </div>

    <!-- Tabs內容 -->
    <div class="tab-content flex-1">
      <!-- 專案資訊 -->
      <div
        v-if="activeTab === PipelineTabEnum.Project"
        class="tab h-full flex flex-col gap-4"
        data-annotation-scope="work-project-detail:project"
      >
        <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
          <div class="flex justify-between">
            <div class="subtitle">專案資訊</div>
            <div></div>
          </div>

          <div class="flex gap-4 w-full">
            <div class="mb-3 flex flex-col gap-2 flex-1">
              <label class="form-label">專案狀態 <span class="required-label">*</span></label>
              <div class="mt-2">
                <span
                  class="p-2 px-3 text-nowrap rounded-full text-sm"
                  :class="getStatusClass(workProjectDetailViewObj.atomEmployeeProjectStatus)"
                >
                  {{
                    getEmployeeProjectStatusLabel(
                      workProjectDetailViewObj.atomEmployeeProjectStatus
                    )
                  }}
                </span>
              </div>
            </div>
            <div class="mb-3 flex flex-col gap-2 flex-1">
              <label class="form-label">專案代碼 <span class="required-label">*</span></label>
              <input
                v-model="workProjectDetailViewObj.employeeProjectCode"
                type="text"
                class="input-box"
                placeholder="請輸入專案代碼"
                disabled
              />
              <p class="error-wrapper">
                <span v-if="!wrkProjectUpdateValidateObj.employeeProjectCode" class="error-tip">
                  不可為空，且長度不可超過50字
                </span>
              </p>
            </div>
          </div>
          <div class="flex gap-4 w-full">
            <div class="mb-3 flex flex-col gap-2 flex-1">
              <label class="form-label">專案名稱 <span class="required-label">*</span></label>
              <input
                v-model="workProjectDetailViewObj.employeeProjectName"
                type="text"
                class="input-box"
                placeholder="請輸入專案名稱"
                disabled
              />
              <p class="error-wrapper">
                <span v-if="!wrkProjectUpdateValidateObj.employeeProjectName" class="error-tip">
                  不可為空
                </span>
              </p>
            </div>

            <div class="mb-3 flex flex-col gap-2 flex-1">
              <label class="form-label">客戶 <span class="required-label">*</span></label>
              <input
                v-model="workProjectDetailViewObj.managerCompanyName"
                type="text"
                class="input-box"
                disabled
              />
            </div>
          </div>

          <!--專案起訖時間-->
          <div class="flex gap-4 w-full">
            <div class="mb-3 flex flex-col gap-2 flex-1">
              <label class="form-label">開始日期 <span class="required-label">*</span></label>
              <input
                v-model="workProjectDetailViewObj.employeeProjectStartTime"
                type="date"
                class="input-box"
                disabled
              />
            </div>

            <div class="mb-3 flex flex-col gap-2 flex-1">
              <label class="form-label">結束日期 <span class="required-label">*</span></label>
              <input
                v-model="workProjectDetailViewObj.employeeProjectEndTime"
                type="date"
                class="input-box"
                disabled
              />
            </div>
          </div>

        </div>

        <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="subtitle">打卡紀錄</h2>
              <p class="text-xs text-gray-500 mt-1">
                打卡完成後可於差旅/費用模組補填交通、住宿與餐費。
              </p>
            </div>
          </div>

          <div class="overflow-x-auto">
            <table class="table-base table-fixed table-sticky w-full min-w-[720px]">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-36">打卡時間</th>
                  <th class="text-start w-32">階段</th>
                  <th class="text-start w-40">工項</th>
                  <th class="text-start w-32">地點</th>
                  <th class="text-start w-28">交通方式</th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="checkInLogs.length === 0">
                  <td colspan="5" class="text-center text-gray-400 py-6">尚無打卡紀錄</td>
                </tr>
                <tr v-for="log in checkInLogs" :key="log.id" class="border border-gray-300">
                  <td class="text-start">{{ log.checkedAt }}</td>
                  <td class="text-start">{{ log.stageName }}</td>
                  <td class="text-start">{{ log.taskName }}</td>
                  <td class="text-start">{{ log.location }}</td>
                  <td class="text-start">{{ log.transport }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div v-if="showCheckInForm" class="space-y-3">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="form-label">打卡時間</label>
                <input v-model="checkInForm.checkedAt" type="text" class="input-box" placeholder="YYYY-MM-DD HH:mm" />
              </div>
              <div>
                <label class="form-label">階段</label>
                <select v-model="checkInForm.stageName" class="select-box">
                  <option value="">請選擇</option>
                  <option v-for="stageName in checkInStageOptions" :key="stageName" :value="stageName">
                    {{ stageName }}
                  </option>
                </select>
              </div>
              <div>
                <label class="form-label">工項</label>
                <input v-model="checkInForm.taskName" type="text" class="input-box" placeholder="輸入工項" />
              </div>
              <div>
                <label class="form-label">地點</label>
                <input v-model="checkInForm.location" type="text" class="input-box" placeholder="輸入地點" />
              </div>
              <div>
                <label class="form-label">交通方式</label>
                <select v-model="checkInForm.transport" class="select-box">
                  <option value="">不指定</option>
                  <option value="大眾運輸">大眾運輸</option>
                  <option value="計程車">計程車</option>
                  <option value="自行開車">自行開車</option>
                  <option value="其他">其他</option>
                </select>
              </div>
            </div>
            <div class="flex justify-end gap-2">
              <button class="btn-cancel" @click="cancelCheckInForm">取消</button>
              <button class="btn-submit" @click="saveCheckIn">儲存</button>
            </div>
          </div>
        </div>

        <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
          <div class="flex justify-between items-center">
            <div class="flex items-center gap-2">
              <h2 class="subtitle">主要人員</h2>
              <button
                class="text-gray-500 hover:text-brand-100 transition-colors"
                title="查看角色權限說明"
                @click="projectRolePermissionInfoRef?.openModal()"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                  />
                </svg>
              </button>
            </div>
          </div>

          <table class="table-base table-fixed table-sticky w-full">
            <thead class="bg-gray-800 text-white">
              <tr>
                <th class="text-start">專案角色</th>
                <th class="text-start">部門</th>
                <th class="text-start">人員</th>
              </tr>
            </thead>

            <tbody>
              <template v-if="primaryProjectMembers.length === 0">
                <tr class="text-center">
                  <td colspan="3">無資料</td>
                </tr>
              </template>
              <template v-else>
                <tr
                  v-for="item in primaryProjectMembers"
                  :key="item.employeeProjectMemberID"
                >
                  <td class="text-start">
                    {{ getPrimaryRoleLabel(item.employeeProjectMemberRole) }}
                  </td>
                  <td class="text-start">{{ item.managerDepartmentName }}</td>
                  <td class="text-start">
                    {{ getPrimaryMemberName(item) }}
                  </td>
                </tr>
              </template>
            </tbody>
          </table>

        </div>

        <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
          <div class="flex items-center gap-2">
            <h2 class="subtitle">專案成員</h2>
          </div>

          <table class="table-base table-fixed table-sticky w-full">
            <thead class="bg-gray-800 text-white">
              <tr>
                <th class="text-start">專案角色</th>
                <th class="text-start">部門</th>
                <th class="text-start">人員</th>
              </tr>
            </thead>

            <tbody>
              <template v-if="secondaryProjectMembers.length === 0">
                <tr class="text-center">
                  <td colspan="3">無資料</td>
                </tr>
              </template>
              <template v-else>
                <tr
                  v-for="item in secondaryProjectMembers"
                  :key="item.employeeProjectMemberID"
                >
                  <td class="text-start">
                    {{ getEmployeeProjectMemberRoleLabel(item.employeeProjectMemberRole) }}
                  </td>
                  <td class="text-start">{{ item.managerDepartmentName }}</td>
                  <td class="text-start">
                    {{ item.employeeName }}
                  </td>
                </tr>
              </template>
            </tbody>
          </table>

          <div class="flex justify-center py-4 w-full">
            <button
              class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
              type="button"
              style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
              @click="isShowAddMemberModal = true"
            >
              新增專案成員
            </button>
          </div>
        </div>

        <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="subtitle">異動申請</h2>
              <p class="text-xs text-gray-500 mt-1">
                工程師需提出異動申請，由 PM 或部門主管核可；跨部門/跨區支援會送交目標部門主管確認。
              </p>
            </div>
          </div>

          <div v-if="!showChangeRequestForm" class="flex justify-center py-4 w-full">
            <button
              class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
              type="button"
              style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
              @click="openChangeRequestForm"
            >
              申請異動
            </button>
          </div>

          <div v-else class="space-y-4">
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <label class="form-label">異動類型</label>
                <div class="flex flex-wrap gap-2">
                  <button
                    v-for="option in changeRequestTypes"
                    :key="option.value"
                    type="button"
                    class="rounded-md border px-3 py-2 text-sm font-semibold transition"
                    :class="
                      changeRequestForm.requestType === option.value
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                    "
                    @click="setChangeRequestType(option.value)"
                  >
                    {{ option.label }}
                  </button>
                </div>
              </div>
              <div v-if="changeRequestForm.requestType === 'cross_support'">
                <label class="form-label">目標區域</label>
                <div class="flex flex-wrap gap-2">
                  <button
                    type="button"
                    class="rounded-md border px-3 py-2 text-sm font-semibold transition"
                    :class="
                      changeRequestForm.targetRegion === '北區'
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                    "
                    @click="setChangeRequestTargetRegion('北區')"
                  >
                    北區
                  </button>
                  <button
                    type="button"
                    class="rounded-md border px-3 py-2 text-sm font-semibold transition"
                    :class="
                      changeRequestForm.targetRegion === '中區'
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                    "
                    @click="setChangeRequestTargetRegion('中區')"
                  >
                    中區
                  </button>
                  <button
                    type="button"
                    class="rounded-md border px-3 py-2 text-sm font-semibold transition"
                    :class="
                      changeRequestForm.targetRegion === '南區'
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                    "
                    @click="setChangeRequestTargetRegion('南區')"
                  >
                    南區
                  </button>
                  <button
                    type="button"
                    class="rounded-md border px-3 py-2 text-sm font-semibold transition"
                    :class="
                      changeRequestForm.targetRegion === '跨區'
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                    "
                    @click="setChangeRequestTargetRegion('跨區')"
                  >
                    跨區
                  </button>
                </div>
              </div>
              <div v-if="changeRequestForm.requestType === 'cross_support'">
                <label class="form-label">目標部門</label>
                <select v-model="changeRequestForm.targetDepartment" class="select-box">
                  <option value="">請選擇</option>
                  <option v-if="changeRequestDepartmentOptions.length === 0" value="" disabled>
                    無可用部門
                  </option>
                  <option
                    v-for="option in changeRequestDepartmentOptions"
                    :key="option"
                    :value="option"
                  >
                    {{ option }}
                  </option>
                </select>
              </div>
            </div>

            <div
              v-if="changeRequestForm.requestType === 'task_adjust'"
              class="grid grid-cols-1 md:grid-cols-4 gap-4"
            >
              <div class="md:col-span-2">
                <label class="form-label">階段/工項名稱</label>
                <input
                  v-model="changeRequestForm.targetStageName"
                  class="input-box"
                  placeholder="例如：需求訪談 / 需求確認"
                />
              </div>
              <div>
                <label class="form-label">預估時間</label>
                <input
                  v-model="changeRequestForm.estimatedHours"
                  type="number"
                  min="0"
                  class="input-box"
                  placeholder="輸入時數"
                />
              </div>
              <div>
                <label class="form-label">OnSite 時數</label>
                <input
                  v-model="changeRequestForm.onsiteHours"
                  type="number"
                  min="0"
                  class="input-box"
                  placeholder="輸入時數"
                />
              </div>
              <div>
                <label class="form-label">OffSite 時數</label>
                <input
                  v-model="changeRequestForm.offsiteHours"
                  type="number"
                  min="0"
                  class="input-box"
                  placeholder="輸入時數"
                />
              </div>
            </div>

            <div>
              <label class="form-label">異動原因/說明</label>
              <textarea
                v-model="changeRequestForm.reason"
                class="input-box min-h-[100px]"
                placeholder="說明人員調整、工項異動或跨部門支援需求"
              />
            </div>

            <div class="flex justify-end">
              <button class="btn-submit" type="button" @click="submitChangeRequest">
                送出申請
              </button>
            </div>
          </div>

          <div class="border-t border-gray-200 pt-4">
            <div class="flex items-center justify-between mb-3">
              <h3 class="text-sm font-semibold text-gray-700">本專案異動紀錄</h3>
              <div class="flex items-center gap-3 text-xs text-gray-500">
                <span>最後更新：{{ projectChangeRequestLastUpdated || "無" }}</span>
                <span>{{ projectChangeRequests.length }} 筆</span>
              </div>
            </div>
            <div v-if="projectChangeRequests.length === 0" class="text-sm text-gray-500">
              尚未提出異動申請。
            </div>
            <div v-else class="overflow-x-auto">
              <table class="table-base table-fixed table-sticky w-full">
                <thead class="bg-gray-800 text-white">
                  <tr>
                    <th class="text-start w-28">類型</th>
                    <th class="text-start w-32">申請人</th>
                    <th class="text-start w-32">所屬</th>
                    <th class="text-start">說明</th>
                    <th class="text-start w-28">狀態</th>
                    <th class="text-start w-32">申請時間</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in projectChangeRequests" :key="item.requestId">
                    <td class="text-start">{{ getChangeRequestTypeLabel(item.requestType) }}</td>
                    <td class="text-start">{{ item.requesterName }}</td>
                    <td class="text-start">
                      {{ item.requesterDepartment }} / {{ item.requesterRegion }}
                    </td>
                    <td class="text-start">
                      <p class="text-sm text-gray-700">{{ item.reason }}</p>
                      <p
                        v-if="item.requestType === 'cross_support'"
                        class="text-xs text-gray-500 mt-1"
                      >
                        目標：{{ item.targetDepartment || "-" }} / {{ item.targetRegion || "-" }}
                      </p>
                      <p
                        v-if="item.requestType === 'task_adjust'"
                        class="text-xs text-gray-500 mt-1"
                      >
                        調整：{{ item.targetStageName || "-" }}
                        <span class="mx-1 text-gray-300">|</span>
                        預估時間：{{ item.estimatedHours ?? "-" }}
                        <span class="mx-1 text-gray-300">|</span>
                        OnSite：{{ item.onsiteHours ?? "-" }}
                        <span class="mx-1 text-gray-300">|</span>
                        OffSite：{{ item.offsiteHours ?? "-" }}
                      </p>
                    </td>
                    <td class="text-start">
                      <span
                        class="inline-flex min-w-[72px] items-center justify-center rounded-md px-2 py-1 text-xs"
                        :class="getChangeRequestStatusClass(item.status)"
                      >
                        {{ getChangeRequestStatusLabel(item.status) }}
                      </span>
                    </td>
                    <td class="text-start">{{ formatDate(item.createdAt) || "-" }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <!-- 標準階段與產出 -->
      <div
        v-if="activeTab === PipelineTabEnum.Template"
        class="tab h-full flex flex-col gap-4"
        data-annotation-scope="work-project-detail:template"
      >
        <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
          <div class="flex items-center justify-end">
            <label class="flex items-center gap-2 text-sm text-gray-600">
              <input v-model="showAllStages" type="checkbox" class="w-4 h-4" />
              顯示全部階段
            </label>
          </div>

          <div class="space-y-4">
            <div
              v-for="serviceEntry in selectedServiceProducts"
              :key="serviceEntry.service.serviceItemId"
              class="rounded-lg border border-gray-200 bg-white p-4"
            >
              <div class="flex items-center justify-between">
                <p class="font-semibold text-gray-700">
                  {{ serviceEntry.service.serviceItemName }}
                </p>
              </div>

              <div class="space-y-4 mt-3">
                <p v-if="serviceEntry.products.length === 0" class="text-xs text-gray-500">
                  尚未選擇產品
                </p>
                <p
                  v-else-if="
                    serviceEntry.products.every((product) => product.stageTemplateIds.length === 0)
                  "
                  class="text-xs text-gray-500"
                >
                  尚未設定標準階段模板
                </p>
                <p
                  v-else-if="!showAllStages && !hasVisibleStagesForService(serviceEntry)"
                  class="text-xs text-gray-500"
                >
                  目前沒有分派給你的標準階段
                </p>
                <template v-for="product in serviceEntry.products" :key="product.productId">
                  <template
                    v-for="template in selectedStageTemplates.filter((t) =>
                      product.stageTemplateIds.includes(t.stageTemplateId)
                    )"
                    :key="template.stageTemplateId"
                  >
                    <div
                      v-for="stage in getVisibleStagesForTemplate(template)"
                      :key="stage.stageId"
                      class="rounded-lg border border-gray-200 bg-white p-4 mt-3"
                    >
                      <div class="flex items-center justify-between">
                        <div>
                          <p class="font-semibold text-gray-700">{{ stage.stageName }}</p>
                          <p class="text-xs text-gray-500">
                            負責人：{{ getStageOwnerName(stage.stageId) }}
                          </p>
                        </div>
                        <div class="flex gap-2">
                          <span
                            class="rounded-full bg-cyan-50 px-2 py-0.5 text-xs text-cyan-700"
                            v-if="stage.requiresWorkLog"
                          >
                            需工作日誌
                          </span>
                          <span
                            class="rounded-full bg-amber-50 px-2 py-0.5 text-xs text-amber-700"
                            v-if="stage.requiresOnsiteLog"
                          >
                            需駐點打卡
                          </span>
                          <span
                            v-if="Number.isFinite(stage.onsiteHours)"
                            class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                          >
                            OnSite：{{ stage.onsiteHours }}
                          </span>
                          <span
                            v-if="Number.isFinite(stage.offsiteHours)"
                            class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                          >
                            OffSite：{{ stage.offsiteHours }}
                          </span>
                          <span
                            class="rounded-full px-2 py-0.5 text-xs"
                            :class="getReviewStatusClass(getStageReviewSummary(stage).assistantStatus)"
                          >
                            助理：{{ getReviewStatusLabel(getStageReviewSummary(stage).assistantStatus) }}
                          </span>
                          <span
                            class="rounded-full px-2 py-0.5 text-xs"
                            :class="getReviewStatusClass(getStageReviewSummary(stage).managerStatus)"
                          >
                            經理：{{ getReviewStatusLabel(getStageReviewSummary(stage).managerStatus) }}
                          </span>
                          <span
                            class="rounded-full px-2 py-0.5 text-xs"
                            :class="getReviewStatusClass(getStageReviewSummary(stage).overallStatus)"
                          >
                            總體：{{ getReviewStatusLabel(getStageReviewSummary(stage).overallStatus) }}
                          </span>
                          <button
                            type="button"
                            class="btn-update"
                            :title="isStageExpanded(stage.stageId) ? '收合' : '展開'"
                            @click="toggleStageExpanded(stage.stageId)"
                          >
                            <svg
                              class="w-4 h-4"
                              viewBox="0 0 20 20"
                              fill="currentColor"
                              :class="isStageExpanded(stage.stageId) ? '' : 'rotate-180'"
                            >
                              <path
                                fill-rule="evenodd"
                                d="M5.23 12.21a.75.75 0 0 0 1.06.02L10 8.56l3.71 3.67a.75.75 0 1 0 1.05-1.07l-4.24-4.2a.75.75 0 0 0-1.05 0l-4.24 4.2a.75.75 0 0 0 .02 1.05Z"
                                clip-rule="evenodd"
                              />
                            </svg>
                          </button>
                        </div>
                      </div>

                      <div v-if="isStageExpanded(stage.stageId)" class="mt-3 space-y-3">
                        <div class="flex items-center justify-between">
                          <p class="text-sm font-semibold text-gray-600">應產文件與紀錄</p>
                        </div>
                        <div v-if="stage.requiredOutputs.length === 0" class="text-sm text-gray-500">
                          尚未設定產出
                        </div>
                        <div
                          v-for="output in stage.requiredOutputs"
                          :key="output.outputId"
                          class="rounded-md border border-gray-200 bg-white p-3"
                        >
                          <div class="flex items-center justify-between">
                            <div>
                              <p class="text-sm font-semibold text-gray-700">{{ output.outputName }}</p>
                            </div>
                          <div class="flex items-center gap-2">
                              <span
                                class="text-xs px-2 py-1 rounded-full text-nowrap"
                                :class="
                                  getOutputStatusClass(
                                    getOutputState(output.outputId),
                                    output.required
                                  )
                                "
                              >
                                {{
                                  getOutputStatusLabel(
                                    getOutputState(output.outputId),
                                    output.required
                                  )
                                }}
                              </span>
                              <span class="text-xs border py-1 px-3 rounded-full text-nowrap">
                                上次編輯日期 :
                                {{ formatDate(getOutputState(output.outputId).updatedAt) || "無" }}
                              </span>
                            </div>
                          </div>
                          <div class="mt-2 flex flex-wrap items-center gap-2 text-xs">
                            <span class="text-gray-500">審核</span>
                            <span
                              class="rounded-full px-2 py-0.5"
                              :class="getReviewStatusClass(getOutputReviewState(output.outputId).assistantStatus)"
                            >
                              助理：{{ getReviewStatusLabel(getOutputReviewState(output.outputId).assistantStatus) }}
                            </span>
                            <span
                              class="rounded-full px-2 py-0.5"
                              :class="getReviewStatusClass(getOutputReviewState(output.outputId).managerStatus)"
                            >
                              經理：{{ getReviewStatusLabel(getOutputReviewState(output.outputId).managerStatus) }}
                            </span>
                            <span
                              class="rounded-full px-2 py-0.5"
                              :class="getReviewStatusClass(getOutputReviewState(output.outputId).overallStatus)"
                            >
                              總體：{{ getReviewStatusLabel(getOutputReviewState(output.outputId).overallStatus) }}
                            </span>
                          </div>
                          <p
                            v-if="getOutputReviewState(output.outputId).lastFeedback"
                            class="mt-1 text-xs text-rose-600"
                          >
                            回饋：{{ getOutputReviewState(output.outputId).lastFeedback }}
                          </p>
                          <div class="mt-2 flex flex-row gap-2 items-center">
                            <label
                              class="btn-update cursor-pointer"
                              :class="canEditStageOutputs ? '' : 'opacity-50 cursor-not-allowed'"
                              title="支援多檔上傳，實際儲存位置依 NAS/SharePoint 設定"
                            >
                              上傳文件
                              <input
                                type="file"
                                class="hidden"
                                :disabled="!canEditStageOutputs"
                                @change="
                                  handleOutputFileChange(
                                    output.outputId,
                                    ($event.target as HTMLInputElement).files
                                  )
                                "
                              />
                            </label>
                            <button
                              v-if="canSendOutputReview(output.outputId)"
                              type="button"
                              class="btn-update"
                              @click="sendOutputReview(output, stage, 'assistant')"
                            >
                              送助理
                            </button>
                            <button
                              v-if="canSendOutputReview(output.outputId)"
                              type="button"
                              class="btn-update"
                              @click="sendOutputReview(output, stage, 'manager')"
                            >
                              送經理
                            </button>
                          </div>
                          <div
                            v-if="getOutputState(output.outputId).files.length > 0"
                            class="mt-2 space-y-2"
                          >
                            <div
                              v-for="file in getOutputState(output.outputId).files"
                              :key="file.fileId"
                              class="flex items-center justify-between rounded-md border border-gray-200 bg-gray-50 px-3 py-2"
                            >
                              <div>
                                <p class="text-sm text-gray-700">{{ file.fileName }}</p>
                                <p class="text-xs text-gray-500">
                                  {{ (file.fileSize / 1024 / 1024).toFixed(2) }} MB ｜ 上傳時間
                                  {{ formatDate(file.uploadedAt) || "無" }}
                                </p>
                              </div>
                              <button
                                v-if="canEditStageOutputs"
                                class="btn-delete"
                                @click="removeOutputFile(output.outputId, file.fileId)"
                              >
                                移除
                              </button>
                            </div>
                          </div>
                        </div>
                      </div>

                      <div v-if="isStageExpanded(stage.stageId)" class="mt-4">
                        <div class="flex items-center justify-between">
                          <p class="text-sm font-semibold text-gray-600">工作日誌</p>
                          <div class="flex gap-2">
                            <button
                              type="button"
                              class="tab-btn"
                              :class="{ active: getActiveLogTab(stage.stageId) === 'stage' }"
                              @click="setActiveLogTab(stage.stageId, 'stage')"
                            >
                              階段日誌
                            </button>
                            <button
                              type="button"
                              class="tab-btn"
                              :class="{ active: getActiveLogTab(stage.stageId) === 'daily' }"
                              @click="setActiveLogTab(stage.stageId, 'daily')"
                            >
                              日常日誌
                            </button>
                          </div>
                        </div>

                        <div
                          v-if="getWorkLogList(stage.stageId, getActiveLogTab(stage.stageId)).length > 0"
                          class="mt-2 space-y-2"
                        >
                          <div
                            v-for="log in getWorkLogList(stage.stageId, getActiveLogTab(stage.stageId))"
                            :key="log.logId"
                            class="rounded-md border border-gray-200 bg-white p-3"
                          >
                            <div class="flex items-center justify-between">
                              <p class="text-sm font-semibold text-gray-700">{{ log.logDate }}</p>
                              <span class="text-xs text-gray-500">
                                {{ formatDate(log.createdAt) || "無" }}
                              </span>
                            </div>
                            <p class="text-sm text-gray-600 mt-1">{{ log.logContent }}</p>
                          </div>
                        </div>
                        <p v-else class="text-sm text-gray-500 mt-2">
                          尚未有{{ getActiveLogTab(stage.stageId) === 'stage' ? '階段' : '日常' }}日誌
                        </p>

                        <div v-if="activeLogStageId === stage.stageId" class="mt-3 space-y-2">
                          <div class="grid grid-cols-2 gap-4">
                            <div>
                              <label class="form-label">日期</label>
                              <input v-model="workLogForm.logDate" type="date" class="input-box" />
                            </div>
                          </div>
                          <div>
                            <label class="form-label">內容</label>
                            <textarea
                              v-model="workLogForm.logContent"
                              class="input-box min-h-[80px]"
                              placeholder="請輸入工作日誌內容"
                            />
                          </div>
                          <button
                            v-if="canEditStageOutputs"
                            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
                            type="button"
                            style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                            @click="openWorkLogForm(stage.stageId)"
                          >
                            新增{{ getActiveLogTab(stage.stageId) === 'stage' ? '階段' : '日常' }}日誌
                          </button>
                          <div class="flex justify-end gap-2">
                            <button class="btn-cancel" @click="cancelWorkLogForm">取消</button>
                            <button class="btn-submit" @click="saveWorkLog(stage.stageId)">儲存</button>
                          </div>
                        </div>
                      </div>
                    </div>
                  </template>
                </template>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- 里程碑 -->
      <div
        v-if="activeTab === PipelineTabEnum.ProjectStone"
        class="tab h-full"
        data-annotation-scope="work-project-detail:milestone"
      >
        <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
          <div v-if="derivedStageMilestones.length === 0" class="p-10 text-center text-gray-500">
            無里程碑資料
          </div>
          <div v-else class="space-y-4">
            <div class="flex flex-wrap items-center justify-between gap-2 text-xs text-gray-500">
              <span>專案期間：{{ formatDate(workProjectDetailViewObj.employeeProjectStartTime) }}</span>
              <span>目前階段：{{ currentStageName }}</span>
              <span>{{ formatDate(workProjectDetailViewObj.employeeProjectEndTime) }}</span>
            </div>
            <div
              v-for="milestone in milestoneTimeline"
              :key="milestone.stageId"
              class="relative pl-6"
            >
              <div
                v-if="!milestone.isLast"
                class="absolute left-[9px] top-4 h-full w-px bg-gray-200"
              ></div>
              <div class="flex items-start gap-3">
                <span
                  class="mt-1 h-2.5 w-2.5 rounded-full"
                  :class="getMilestoneStatusClass(milestone.status)"
                ></span>
                <div class="flex-1">
                  <div class="flex items-center justify-between">
                    <p class="text-sm font-semibold text-gray-700">{{ milestone.stageName }}</p>
                    <p class="text-xs text-gray-500">
                      {{ milestone.stageId === currentStageId ? currentStageName : "-" }}
                    </p>
                  </div>
                  <div class="mt-2 h-2 rounded-full bg-gray-100 relative overflow-hidden">
                    <div
                      class="absolute inset-y-0 rounded-full"
                      :class="getMilestoneStatusClass(milestone.status)"
                      :style="{
                        left: milestone.startPercent + '%',
                        width: milestone.widthPercent + '%',
                      }"
                    ></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <!--工項-->
      <div
        v-if="activeTab === PipelineTabEnum.ProjectStoneTask"
        class="tab h-full"
        data-annotation-scope="work-project-detail:milestone-task"
      >
        <div class="flex flex-col bg-white rounded-lg p-8 rounded-tl-none">
          <!-- 整體無資料 -->
          <div
            v-if="workProjectTaskListViewObj.employeeProjectStoneList.length === 0"
            class="p-10 text-center text-gray-500"
          >
            無工項資料
          </div>

          <div
            v-else
            class="overflow-x-auto scrollbar-thin scrollbar-thumb-gray-400 scrollbar-track-gray-100"
          >
            <div class="flex gap-4 mb-3">
              <!-- 每個里程碑變成一欄 -->
              <div
                v-for="stone in workProjectTaskListViewObj.employeeProjectStoneList"
                :key="stone.employeeProjectStoneID"
                class="flex flex-col gap-4"
              >
                <div class="rounded-lg border border-gray-200 bg-gray-50 w-[320px]">
                  <div class="border-b border-gray-200 p-3">
                    <div class="flex justify-between items-center">
                      <!-- 里程碑名稱 -->
                      <p class="font-bold text-md text-gray-700 truncate">
                        {{ stone.employeeProjectStoneName }}
                      </p>
                      <!-- 總預估工時 -->
                      <p class="text-xs px-2 py-1 text-gray-600">
                        預估
                        {{
                          stone.employeeProjectStoneTaskList.reduce(
                            (sum, t) => sum + (t.employeeProjectStoneTaskWorkHour ?? 0),
                            0
                          )
                        }}
                        小時
                      </p>
                    </div>
                  </div>

                  <div class="p-4">
                    <!-- 里程碑資訊 -->
                    <div class="flex flex-col gap-1 mb-3">
                      <p class="text-sm">
                        開始日期 : {{ formatDate(stone.employeeProjectStoneStartTime) }}
                      </p>
                      <p class="text-sm">
                        結束日期 : {{ formatDate(stone.employeeProjectStoneEndTime) }}
                      </p>
                      <p class="text-sm">
                        前N日通知 : {{ stone.employeeProjectStonePreNotifyDay }}日 (
                        {{
                          getNotifyDate(
                            stone.employeeProjectStoneEndTime,
                            stone.employeeProjectStonePreNotifyDay
                          )
                        }}
                        )
                      </p>
                    </div>

                    <!-- 卡片（里程碑任務） -->
                    <div
                      v-for="task in stone.employeeProjectStoneTaskList"
                      :key="task.employeeProjectStoneTaskID"
                      class="bg-white rounded-lg border border-gray-200 p-4 flex flex-col gap-3 mt-3 hover:bg-gray-100 cursor-pointer"
                      @click="clickUpdateProjectTaskBtn(task.employeeProjectStoneTaskID)"
                    >
                      <!-- 卡片抬頭 -->
                      <div
                        class="flex flex-row justify-between cursor-pointer"
                        @click="clickUpdateProjectTaskBtn(task.employeeProjectStoneTaskID)"
                      >
                        <span
                          class="p-1 px-3 text-nowrap rounded-full text-xs"
                          :class="getStatusClass(task.atomEmployeeProjectStatus)"
                        >
                          {{ getEmployeeProjectStatusLabel(task.atomEmployeeProjectStatus) }}
                        </span>
                        <span
                          class="p-1 px-2 text-nowrap rounded-full text-xs border"
                          :class="
                            task.employeeProjectStoneTaskExecutorCount > 0
                              ? ' border-blue-300 text-blue-500 '
                              : 'border-gray-400 text-gray-600'
                          "
                        >
                          {{
                            task.employeeProjectStoneTaskExecutorCount > 0
                              ? task.employeeProjectStoneTaskExecutorCount + " 位執行者"
                              : "無執行者"
                          }}
                        </span>
                      </div>
                      <div class="flex justify-between items-center">
                        <p class="font-bold">{{ task.employeeProjectStoneTaskName }}</p>
                      </div>

                      <div class="flex flex-col gap-3 cursor-pointer">
                        <!-- Bucket 清單 -->
                        <div class="flex flex-col ml-1 gap-1">
                          <label
                            v-for="bucket in task.employeeProjectStoneTaskBucketList"
                            :key="bucket.employeeProjectStoneTaskBucketID"
                            class="flex items-center gap-2"
                            @click.stop
                          >
                            <input
                              type="checkbox"
                              :checked="bucket.employeeProjectStoneTaskBucketIsFinish"
                              class="rounded border-gray-400 cursor-pointer"
                              @change="toggleBucket(bucket)"
                              @click.stop
                            />
                            <span class="cursor-pointer text-sm">{{
                              bucket.employeeProjectStoneTaskBucketName
                            }}</span>
                          </label>
                        </div>

                        <!-- 下排資訊：時間 & 小時數 -->
                        <div class="flex justify-between text-xs text-gray-600 mt-1">
                          <div class="flex flex-row gap-1 border px-2 py-1 rounded-md items-center">
                            <span>
                              <svg
                                width="14"
                                height="14"
                                viewBox="0 0 14 14"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <path
                                  fill-rule="evenodd"
                                  clip-rule="evenodd"
                                  d="M7.06667 6.33533L9.66067 8.92933L9.09533 9.49533L6.26667 6.66667V2.66667H7.06667V6.33533ZM6.66667 13.3333C2.98467 13.3333 0 10.3487 0 6.66667C0 2.98467 2.98467 0 6.66667 0C10.3487 0 13.3333 2.98467 13.3333 6.66667C13.3333 10.3487 10.3487 13.3333 6.66667 13.3333ZM6.66667 12.5333C8.2226 12.5333 9.71481 11.9152 10.815 10.815C11.9152 9.71481 12.5333 8.2226 12.5333 6.66667C12.5333 5.11073 11.9152 3.61852 10.815 2.51831C9.71481 1.41809 8.2226 0.8 6.66667 0.8C5.11073 0.8 3.61852 1.41809 2.51831 2.51831C1.41809 3.61852 0.8 5.11073 0.8 6.66667C0.8 8.2226 1.41809 9.71481 2.51831 10.815C3.61852 11.9152 5.11073 12.5333 6.66667 12.5333Z"
                                  fill="#99A1AF"
                                />
                              </svg>
                            </span>
                            <span>
                              {{ formatDate(task.employeeProjectStoneTaskStartTime) }} ~
                              {{ formatDate(task.employeeProjectStoneTaskEndTime) }}
                            </span>
                          </div>
                          <div class="items-center flex">
                            <span>{{ task.employeeProjectStoneTaskWorkHour }} 小時</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!--收支-->
      <div
        v-if="activeTab === PipelineTabEnum.Expense"
        class="tab h-full"
        data-annotation-scope="work-project-detail:expense"
      >
        <div
          class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none h-full overflow-y-auto"
        >
          <!-- 總支出費用 -->
          <div class="flex justify-between w-full mb-4">
            <div class="title">收支</div>
            <h2 class="text-gray-700">總支出費用 : {{ formatCurrency(totalExpense) }}</h2>
          </div>

          <!-- 專案支出 -->
          <div class="flex flex-col gap-2">
            <div class="flex flex-row justify-between items-center">
              <h3 class="subtitle">專案支出</h3>
              <button
                v-if="
                  projectMemberRoleStore.canShow([
                    DbEmployeeProjectMemberRoleEnum.ProjectManager,
                    DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
                    DbEmployeeProjectMemberRoleEnum.Saler,
                  ])
                "
                class="btn-add"
                @click="showAddExpenseModal = true"
              >
                新增專案支出
              </button>
            </div>
            <div class="overflow-x-auto">
              <table class="table-base">
                <thead class="text-white uppercase">
                  <tr>
                    <th scope="col" class="text-nowrap">支出項目</th>
                    <th scope="col" class="text-nowrap">預估金額</th>
                    <th scope="col" class="text-nowrap">實際金額</th>
                    <th scope="col" class="text-center text-nowrap">發票日期</th>
                    <th scope="col" class="text-center text-nowrap">發票號碼</th>
                    <th scope="col" class="text-center">備註</th>
                    <th scope="col" class="text-center text-nowrap">操作</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="workProjectExpenseListViewObj.projectExpenseList.length === 0">
                    <td colspan="7" class="text-center">無資料</td>
                  </tr>
                  <tr
                    v-for="(item, index) in workProjectExpenseListViewObj.projectExpenseList"
                    :key="index"
                  >
                    <td class="text-center">
                      {{ item.employeeProjectExpenseName }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.employeeProjectExpensePredictAmount) }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.employeeProjectExpenseActualAmount || 0) }}
                    </td>
                    <td class="text-center">
                      {{ formatDate(item.employeeProjectExpenseBillTime) }}
                    </td>
                    <td class="text-center">
                      {{ item.employeeProjectExpenseBillNumber || "-" }}
                    </td>
                    <td class="text-center">
                      {{ item.employeeProjectExpenseRemark || "-" }}
                    </td>
                    <td class="text-center">
                      <button
                        v-if="
                          projectMemberRoleStore.canShow([
                            DbEmployeeProjectMemberRoleEnum.ProjectManager,
                            DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
                            DbEmployeeProjectMemberRoleEnum.Saler,
                          ])
                        "
                        class="btn-update"
                        @click="
                          () => {
                            selectedExpenseID = item.employeeProjectExpenseID;
                            showUpdateExpenseModal = true;
                          }
                        "
                      >
                        編輯
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <!-- EIP專案支出 -->
          <div class="flex flex-col gap-2 mt-6">
            <div class="flex flex-row justify-between items-center">
              <h3 class="subtitle">EIP專案支出</h3>
            </div>
            <div class="overflow-x-auto">
              <table class="table-base">
                <thead class="uppercase">
                  <tr>
                    <th scope="col" class="text-center text-nowrap">簽核狀態</th>
                    <th scope="col" class="text-center text-nowrap">申請人員</th>
                    <th scope="col" class="text-center text-nowrap">日期</th>
                    <th scope="col" class="text-center text-nowrap">支出項目</th>
                    <th scope="col" class="text-center text-nowrap">參與人員</th>
                    <th scope="col" class="text-center text-nowrap">總金額</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="workProjectExpenseListViewObj.eipProjectExpenseList.length === 0">
                    <td colspan="7" class="text-center">無資料</td>
                  </tr>
                  <tr
                    v-for="(item, index) in workProjectExpenseListViewObj.eipProjectExpenseList"
                    :key="index"
                    class="bg-white border-b hover:bg-gray-50"
                  >
                    <td class="text-center">
                      {{ item.projectExpenseApprovalStatus || "-" }}
                    </td>
                    <td class="text-center">
                      {{ item.projectExpenseApplicant || "-" }}
                    </td>
                    <td class="text-center">
                      {{ formatDate(item.projectExpenseDate) }}
                    </td>
                    <td class="font-medium text-gray-900 text-center">
                      {{ item.projectExpenseReason || "-" }}
                    </td>
                    <td class="text-center">
                      {{ item.projectExpenseParticipants || "-" }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.projectExpenseAmount) }}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <!-- EIP專案旅費 -->
          <div class="flex flex-col gap-2 mt-6">
            <div class="flex flex-row justify-between items-center">
              <h3 class="subtitle">EIP專案旅費</h3>
            </div>
            <div class="overflow-x-auto">
              <table class="table-base">
                <thead class="uppercase">
                  <tr>
                    <th scope="col" class="text-center text-nowrap">簽核狀態</th>
                    <th scope="col" class="text-center text-nowrap">申請人員</th>
                    <th scope="col" class="text-center text-nowrap">日期</th>
                    <th scope="col" class="text-center text-nowrap">起訖地點</th>
                    <th scope="col" class="text-center text-nowrap">工作記要</th>
                    <th scope="col" class="text-center text-nowrap">交通工具</th>
                    <th scope="col" class="text-center text-nowrap">交通費</th>
                    <th scope="col" class="text-center text-nowrap">里程</th>
                    <th scope="col" class="text-center text-nowrap">膳宿費</th>
                    <th scope="col" class="text-center text-nowrap">特別費事由</th>
                    <th scope="col" class="text-center text-nowrap">特別費</th>
                    <th scope="col" class="text-center text-nowrap">合計</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="workProjectExpenseListViewObj.eipProjectTravelExpenseList.length === 0">
                    <td colspan="12" class="text-center">無資料</td>
                  </tr>
                  <tr
                    v-for="(
                      item, index
                    ) in workProjectExpenseListViewObj.eipProjectTravelExpenseList"
                    :key="index"
                  >
                    <td class="text-center">
                      {{ item.projectTravelExpenseApprovalStatus || "-" }}
                    </td>
                    <td class="text-center">
                      {{ item.projectTravelExpenseApplicant || "-" }}
                    </td>
                    <td class="text-center">
                      {{ formatDate(item.projectTravelExpenseTravelDate) }}
                    </td>
                    <td class="text-center">
                      {{ item.projectTravelExpenseTravelRoute || "-" }}
                    </td>
                    <td class="font-medium text-gray-900 text-center">
                      {{ item.projectTravelExpenseWorkDescription || "-" }}
                    </td>
                    <td class="text-center">
                      {{ item.projectTravelExpenseTransportationTool || "-" }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.projectTravelExpenseTransportationAmount || 0) }}
                    </td>
                    <td class="text-center">
                      {{ item.projectTravelExpenseMileage || 0 }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.projectTravelExpenseAccommodationAmount || 0) }}
                    </td>
                    <td class="text-center">
                      {{ item.projectTravelExpenseSpecialExpenseReason || "-" }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.projectTravelExpenseSpecialExpenseAmount || 0) }}
                    </td>
                    <td class="text-center">
                      {{ formatCurrency(item.projectTravelExpenseTotalAmount || 0) }}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <!-- 風險管理 -->
      <div
        v-if="activeTab === PipelineTabEnum.Risk"
        class="tab h-full"
        data-annotation-scope="work-project-detail:risk"
      >
        <div
          class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none h-full overflow-y-auto"
        >
          <div class="flex items-center justify-between">
            <div>
              <div class="subtitle">風險管理</div>
              <p class="text-xs text-gray-500 mt-1">
                記錄可能風險，不影響專案風險等級，供部門主管與總經理檢視。
              </p>
            </div>
          </div>

          <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
            <div>
              <label class="form-label">風險事項</label>
              <input
                v-model="projectRiskDraft.title"
                type="text"
                class="input-box"
                placeholder="輸入風險事項"
              />
            </div>
            <div>
              <label class="form-label">說明</label>
              <input
                v-model="projectRiskDraft.note"
                type="text"
                class="input-box"
                placeholder="補充風險細節或影響"
              />
            </div>
          </div>

          <button
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
            style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
            type="button"
            @click="addProjectRisk"
          >
            新增風險
          </button>

          <div v-if="projectRiskList.length === 0" class="text-gray-400 text-center py-4">
            尚無風險紀錄
          </div>
          <div v-else class="overflow-x-auto">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-40">風險事項</th>
                  <th class="text-start">說明</th>
                  <th class="text-start w-32">提出人</th>
                  <th class="text-start w-28">日期</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in projectRiskList" :key="item.id" class="border border-gray-300">
                  <td class="text-start">{{ item.title }}</td>
                  <td class="text-start">{{ item.note }}</td>
                  <td class="text-start">{{ item.createdBy }}</td>
                  <td class="text-start">{{ formatDate(item.createdAt) || "-" }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="handleCloseError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!--附加人員彈跳視窗-->
    <AddWorkProjectEmployeeModal
      v-if="isShowAddMemberModal"
      :employeeProjectID="projectID"
      :existMemberList="existMemberList"
      @close="isShowAddMemberModal = false"
      @submit="getWorkProjectData"
    />

    <!--新增里程碑彈跳視窗-->
    <AddWorkProjectStoneModal
      v-if="isAddProjectStoneShow"
      :employeeProjectID="projectID"
      @close="isAddProjectStoneShow = false"
      @submit="getManyProjectStoneList"
    />

    <!--新增專案支出彈跳視窗-->
    <AddWorkProjectExpenseModal
      v-if="showAddExpenseModal"
      :employeeProjectID="projectID"
      @close="showAddExpenseModal = false"
      @submit="getExpenseData"
    />

    <!--更新專案支出彈跳視窗-->
    <UpdateWorkProjectExpenseModal
      v-if="showUpdateExpenseModal"
      :employeeProjectExpenseID="selectedExpenseID"
      :employeeProjectID="projectID"
      @close="showUpdateExpenseModal = false"
      @submit="getExpenseData"
    />

    <!--更新里程碑彈跳視窗-->
    <UpdateWorkProjectStoneModal
      v-if="isUpdateProjectStoneShow"
      :employeeProjectStoneID="selectedProjectStoneID"
      :employeeProjectID="projectID"
      @close="isUpdateProjectStoneShow = false"
      @submit="getManyProjectStoneList"
    />

    <!--更新工項彈跳視窗-->
    <UpdateWorkProjectTaskModal
      v-if="isUpdateProjectTaskShow"
      :employeeProjectStoneTaskID="selectedProjectTaskID"
      :employeeProjectID="projectID"
      @close="isUpdateProjectTaskShow = false"
      @submit="getManyProjectTaskList(projectID)"
      @check-bucket="getManyProjectTaskList(projectID)"
    />

    <!--確認刪除彈跳視窗-->
    <ConfirmDialog
      :show="showDeleteProjectStoneConfirm"
      title="確認刪除"
      message="確定要刪除此里程碑嗎？刪除後將無法復原。"
      @confirm="handleRemoveProjectStone"
      @cancel="handleCancelRemoveProjectStone"
    />

    <!-- 確認刪除彈跳視窗  -->
    <ConfirmDialog
      :show="showConfirmProjectMemberDialog"
      title="確認刪除"
      message="確定要刪除人員嗎？"
      confirm-text="確定"
      cancel-text="取消"
      @confirm="handleRemoveProjectMember"
      @cancel="handleCancelRemoveProjectMember"
    />

    <!-- 專案角色權限說明 -->
    <ProjectRolePermissionInfo ref="projectRolePermissionInfoRef" />
  </div>
</template>
