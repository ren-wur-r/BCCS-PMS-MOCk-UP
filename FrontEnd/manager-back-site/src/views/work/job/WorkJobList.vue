<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, defineAsyncComponent } from "vue";
import VueDatePicker from "@vuepic/vue-datepicker";
// Enums / 常數
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
// Stores
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
// Composables
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
// Services
import { WorkJobRecordOnlyAddPayloadMdl } from "./components/WorkJobRecordOnlyAdd.vue";
import {
  MbsWrkJobHttpAddProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpRemoveProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpRemoveProjectStoneJobWorkRspMdl,
} from "@/services/pms-http/work/job/workJobHttpFormat";
import {
  UpdateWorkJobPayloadMdl,
  UpdateWorkRecordPayloadMdl,
} from "./components/WorkJobRecordUpdate.vue";
import {
  addProjectStoneJobWork,
  getManyProjectStoneJob,
  getManyProjectStoneJobWork,
  removeProjectStoneJobWork,
  updateProjectStoneJobWork,
} from "@/services";
// Utils
import {
  formatDate,
  formatTime,
  formatToServerDateEndISO8,
  formatToServerDateStartISO8,
} from "@/utils/timeFormatter";
import { getEmployeeProjectStatusLabel } from "@/utils/getEmployeeProjectStatusLabel";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
// Components
import GetManyEmployeeProjectComboBox from "@/components/feature/search-bar/GetManyEmployeeProjectComboBox.vue";
import GetManyEmployeeProjectStoneComboBox from "@/components/feature/search-bar/GetManyEmployeeProjectStoneComboBox.vue";
import GetManyEmployeeProjectStoneJobComboBox from "@/components/feature/search-bar/GetManyEmployeeProjectStoneJobComboBox.vue";
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);
const SuccessToast = defineAsyncComponent(
  () => import("@/components/global/feedback/SuccessToast.vue")
);
const Pagination = defineAsyncComponent(
  () => import("@/components/global/pagination/Pagination.vue")
);
const WorkJobRecordOnlyAdd = defineAsyncComponent(
  () => import("./components/WorkJobRecordOnlyAdd.vue")
);
const ConfirmDialog = defineAsyncComponent(
  () => import("@/components/global/feedback/ConfirmDialog.vue")
);
const WorkJobRecordUpdate = defineAsyncComponent(
  () => import("./components/WorkJobRecordUpdate.vue")
);
const WorkJobRecordDetail = defineAsyncComponent(
  () => import("./components/WorkJobRecordDetail.vue")
);
// Router
import router from "@/router";
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, closeError } = useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
//#endregion

//#region 型別定義
enum TabEnum {
  WorkJob = "WorkJob",
  WorkRecord = "WorkRecord",
}

/** 工作-工項管理-查詢模型 */
interface WorkJobQueryMdl {
  employeeProjectID: number | null;
  employeeProjectName: string | null;
  employeeProjectStoneID: number | null;
  employeeProjectStoneName: string | null;
  employeeProjectStoneJobID: number | null;
  employeeProjectStoneJobName: string | null;
  employeeProjectStoneJobStatus: DbAtomEmployeeProjectStatusEnum | null;
  startEmployeeProjectStoneJobEndTime: string | null;
  endEmployeeProjectStoneJobEndTime: string | null;
  pageIndex: number;
  pageSize: number;
}

/** 工作-工項管理-列表-項目模型 */
interface WorkJobListItemMdl {
  employeeProjectStoneJobID: number;
  employeeProjectName: string;
  employeeProjectStoneName: string;
  employeeProjectStoneJobName: string;
  employeeProjectStoneJobStatus: DbAtomEmployeeProjectStatusEnum | null;
  employeeProjectStoneJobStartTime: string;
  employeeProjectStoneJobEndTime: string;
  estimatedHours?: number | null;
  monthTotalHours?: number | null;
  onsiteHours?: number | null;
  offsiteHours?: number | null;
  employeeProjectStoneJobExecutorList: {
    employeeProjectStoneJobExecutorName: string;
  }[];
}

/** 工作-工項管理-列表-頁面模型 */
interface WorkJobListViewMdl {
  query: WorkJobQueryMdl;
  employeeProjectJobList: WorkJobListItemMdl[];
  totalCount: number;
}

/** 工作-工作紀錄-查詢模型 */
interface WorkJobRecordQueryMdl {
  employeeProjectID: number | null;
  employeeProjectName: string | null;
  employeeProjectStoneID: number | null;
  employeeProjectStoneName: string | null;
  employeeID: number | null;
  employeeProjectStoneJobID: number | null;
  employeeProjectStoneJobName: string | null;
  employeeProjectStoneJobWorkStartTime: string | null;
  employeeProjectStoneJobWorkEndTime: string | null;
  pageIndex: number;
  pageSize: number;
}

/** 工作-工項管理-列表-項目模型 */
interface WorkJobRecordListItemMdl {
  employeeProjectStoneJobWorkID: number;
  employeeProjectStoneJobWorkStartTime: string;
  employeeProjectStoneJobWorkEndTime: string;
  employeeProjectName: string;
  employeeProjectStoneName: string;
  employeeProjectStoneJobName: string;
  employeeName: string;
  employeeProjectStoneJobWorkRemark: string;
}

/** 工作-工作紀錄-列表-頁面模型 */
interface WorkRecordListViewMdl {
  query: WorkJobRecordQueryMdl;
  employeeProjectJobRecordList: WorkJobRecordListItemMdl[];
  totalCount: number;
}
//#endregion

//#region 頁面物件
/** 當前作用中的tab */
const activeTab = ref<TabEnum>(TabEnum.WorkJob);
/** 日期範圍 */
const activityTimeRange = ref<[Date, Date] | null>(null);
/** 當前目錄 */
const menu = DbAtomMenuEnum.WorkJob;

// 是否顯示確認彈窗
const showConfirmWorkRecordDialog = ref(false);
// 準備要刪除的工作記錄 ID
const confirmWorkRecordID = ref<number | null>(null);
/** 是否顯示【編輯工作紀錄】彈跳視窗 */
const showUpdateWorkJobRecordModal = ref(false);
/** 目前編輯中的工作紀錄 */
const currentWorkRecord = ref<WorkJobRecordListItemMdl | null>(null);
/** 工作紀錄模式型別 全部(工項內容+工作紀錄)/只有工作紀錄 */
type WorkJobRecordMode = "full" | "record-only";
/** 工作紀錄編輯模式 */
const workJobRecordUpdateMode = ref<WorkJobRecordMode>("record-only");
/** 是否顯示【檢視工作紀錄】彈跳視窗 */
const showWorkJobRecordDetailModal = ref(false);
/** 工作紀錄檢視模式 */
const workJobRecordDetailMode = ref<WorkJobRecordMode>("record-only");
/** 是否顯示【快速新增工作紀錄】彈跳視窗 */
const showOnlyAddWorkJobRecordModal = ref(false);
/** 操作選單顯示控制 */
const showActionMenu = ref<number | null>(null);
/** 操作選單位置 */
const actionMenuPosition = ref({ top: "0px", left: "0px" });

/** 工作-工項管理-列表-頁面物件 */
const workJobListViewObj = reactive<WorkJobListViewMdl>({
  query: {
    employeeProjectID: null,
    employeeProjectName: null,
    employeeProjectStoneID: null,
    employeeProjectStoneName: null,
    employeeProjectStoneJobID: null,
    employeeProjectStoneJobName: null,
    employeeProjectStoneJobStatus: null,
    startEmployeeProjectStoneJobEndTime: null,
    endEmployeeProjectStoneJobEndTime: null,
    pageIndex: 1,
    pageSize: 10,
  },
  employeeProjectJobList: [],
  totalCount: 0,
});

/** 工作-工作紀錄-列表-頁面物件 */
const workRecordListViewObj = reactive<WorkRecordListViewMdl>({
  query: {
    employeeProjectID: null,
    employeeProjectName: null,
    employeeProjectStoneID: null,
    employeeProjectStoneName: null,
    employeeID: null,
    employeeProjectStoneJobID: null,
    employeeProjectStoneJobName: null,
    employeeProjectStoneJobWorkStartTime: null,
    employeeProjectStoneJobWorkEndTime: null,
    pageIndex: 1,
    pageSize: 10,
  },
  employeeProjectJobRecordList: [],
  totalCount: 0,
});
//#endregion

//#region UI行為 / 畫面處理
/** 切換tab */
const changeTab = (tab: TabEnum) => {
  activeTab.value = tab;
  if (tab === TabEnum.WorkJob) {
    getWorkJobList();
  } else {
    getWorkRecordList();
  }
};

/** 取得狀態樣式 */
const getStatusClass = (status: number | null) => {
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
const buildJobHourStats = () => {
  const now = new Date();
  const targetYear = now.getFullYear();
  const targetMonth = now.getMonth();
  const stats = new Map<number, { monthTotal: number; onsite: number; offsite: number }>();
  mockDataSets.workJobRecords.forEach((record) => {
    const start = new Date(record.startTime);
    const end = new Date(record.endTime);
    if (Number.isNaN(start.getTime()) || Number.isNaN(end.getTime())) return;
    if (start.getFullYear() !== targetYear || start.getMonth() !== targetMonth) return;
    const hours = Math.max(0, (end.getTime() - start.getTime()) / (1000 * 60 * 60));
    const entry = stats.get(record.jobId) ?? { monthTotal: 0, onsite: 0, offsite: 0 };
    entry.monthTotal += hours;
    if (record.workMode === "OnSite") {
      entry.onsite += hours;
    } else {
      entry.offsite += hours;
    }
    stats.set(record.jobId, entry);
  });
  return stats;
};
const formatHours = (value?: number | null) => {
  if (typeof value !== "number" || Number.isNaN(value)) return "-";
  return value % 1 === 0 ? value.toFixed(0) : value.toFixed(1);
};
//#endregion

//#region API / 資料流程
/** 取得【工項管理】列表 */
const getWorkJobList = async () => {
  // 驗證token
  if (!requireToken()) return;

  // 取得日期範圍
  const startDate = activityTimeRange.value ? activityTimeRange.value[0] : null;
  const endDate = activityTimeRange.value ? activityTimeRange.value[1] : null;
  // 確保日期格式正確（ISO 8601 格式）
  let formattedStartDate = null;
  let formattedEndDate = null;
  if (startDate) {
    formattedStartDate = formatToServerDateStartISO8(startDate);
  }
  if (endDate) {
    formattedEndDate = formatToServerDateEndISO8(endDate);
  }

  // 呼叫api
  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: workJobListViewObj.query.employeeProjectID,
    employeeProjectStoneID: workJobListViewObj.query.employeeProjectStoneID,
    employeeProjectStoneJobID: workJobListViewObj.query.employeeProjectStoneJobID,
    employeeProjectStoneJobName: workJobListViewObj.query.employeeProjectStoneJobName,
    employeeProjectStoneJobStatus: workJobListViewObj.query.employeeProjectStoneJobStatus,
    startEmployeeProjectStoneJobEndTime: formattedStartDate,
    endEmployeeProjectStoneJobEndTime: formattedEndDate,
    pageIndex: workJobListViewObj.query.pageIndex,
    pageSize: workJobListViewObj.query.pageSize,
  };
  const responseData = await getManyProjectStoneJob(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  const hourStats = buildJobHourStats();
  const estimatedHourMap = new Map(
    mockDataSets.workJobs.map((job) => [job.id, job.estimatedHours ?? null])
  );
  workJobListViewObj.employeeProjectJobList = responseData.employeeProjectStoneJobList?.map(
    (item) => ({
      employeeProjectStoneJobID: item.employeeProjectStoneJobID,
      employeeProjectName: item.employeeProjectName,
      employeeProjectStoneName: item.employeeProjectStoneName,
      employeeProjectStoneJobName: item.employeeProjectStoneJobName,
      employeeProjectStoneJobStatus: item.employeeProjectStoneJobStatus,
      employeeProjectStoneJobStartTime: item.employeeProjectStoneJobStartTime,
      employeeProjectStoneJobEndTime: item.employeeProjectStoneJobEndTime,
      estimatedHours: estimatedHourMap.get(item.employeeProjectStoneJobID) ?? null,
      monthTotalHours: hourStats.get(item.employeeProjectStoneJobID)?.monthTotal ?? null,
      onsiteHours: hourStats.get(item.employeeProjectStoneJobID)?.onsite ?? null,
      offsiteHours: hourStats.get(item.employeeProjectStoneJobID)?.offsite ?? null,
      employeeProjectStoneJobExecutorList:
        item.employeeProjectStoneJobExecutorList?.map((executor) => ({
          employeeProjectStoneJobExecutorName: executor.employeeProjectStoneJobExecutorName,
        })) ?? [],
    })
  );

  workJobListViewObj.totalCount = responseData.totalCount;
};

/** 取得【工作紀錄】列表 */
const getWorkRecordList = async () => {
  if (!requireToken()) return;

  const startDate = activityTimeRange.value ? activityTimeRange.value[0] : null;
  const endDate = activityTimeRange.value ? activityTimeRange.value[1] : null;

  // 確保日期格式正確（ISO 8601 格式）
  let formattedStartDate = null;
  let formattedEndDate = null;

  if (startDate) {
    formattedStartDate = formatToServerDateStartISO8(startDate);
  }

  if (endDate) {
    formattedEndDate = formatToServerDateEndISO8(endDate);
  }
  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: workRecordListViewObj.query.employeeProjectID,
    employeeProjectStoneID: workRecordListViewObj.query.employeeProjectStoneID,
    employeeProjectStoneJobID: workRecordListViewObj.query.employeeProjectStoneJobID,
    employeeID: workRecordListViewObj.query.employeeID,
    employeeProjectStoneJobWorkStartTime: formattedStartDate,
    employeeProjectStoneJobWorkEndTime: formattedEndDate,
    pageIndex: workRecordListViewObj.query.pageIndex,
    pageSize: workRecordListViewObj.query.pageSize,
  };

  const responseData = await getManyProjectStoneJobWork(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  workRecordListViewObj.employeeProjectJobRecordList =
    responseData.employeeProjectStoneJobWorkList?.map(
      (item) =>
        ({
          employeeProjectStoneJobWorkID: item.employeeProjectStoneJobWorkID,
          employeeProjectStoneJobWorkStartTime: item.employeeProjectStoneJobWorkStartTime,
          employeeProjectStoneJobWorkEndTime: item.employeeProjectStoneJobWorkEndTime,
          employeeProjectName: item.employeeProjectName,
          employeeProjectStoneName: item.employeeProjectStoneName,
          employeeProjectStoneJobName: item.employeeProjectStoneJobName,
          employeeName: item.employeeName,
          employeeProjectStoneJobWorkRemark: item.employeeProjectStoneJobWorkRemark,
        }) satisfies WorkJobRecordListItemMdl
    ) ?? [];

  workRecordListViewObj.totalCount = responseData.totalCount;
};

/** 確認刪除工作記錄 */
const handleRemoveWorkRecord = async () => {
  // 驗證token
  if (!requireToken()) return;

  // 確認有工作記錄ID
  if (!confirmWorkRecordID.value) return;

  // 呼叫API
  const requestData: MbsWrkJobHttpRemoveProjectStoneJobWorkReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobWorkID: confirmWorkRecordID.value,
  };
  const responseData: MbsWrkJobHttpRemoveProjectStoneJobWorkRspMdl | null =
    await removeProjectStoneJobWork(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "刪除工作記錄失敗，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉 dialog
  showConfirmWorkRecordDialog.value = false;
  confirmWorkRecordID.value = null;

  // 顯示成功訊息
  displaySuccess("刪除工作記錄成功!");

  // 重新抓列表
  await getWorkRecordList();
};

/** 點擊【編輯送出工作記錄】按鈕 */
const clickUpdateSubmitJobWorkBtn = async (payload: {
  employeeProjectStoneJobID: number | null;
  updateJob: UpdateWorkJobPayloadMdl | null;
  updateRecordWork: UpdateWorkRecordPayloadMdl | null;
}) => {
  // 驗證token
  if (!requireToken()) return;

  // 呼叫API
  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobID: payload.employeeProjectStoneJobID ?? null,
    employeeProjectStoneJobRemark: payload.updateJob?.employeeProjectStoneTaskRemark ?? null,
    employeeProjectStoneJobBucketList: payload.updateJob?.employeeProjectStoneTaskBucketList ?? [],
    employeeProjectStoneJobWorkID: payload.updateRecordWork?.employeeProjectStoneJobWorkID ?? 0,
    employeeProjectStoneJobWorkStartTime:
      payload.updateRecordWork?.employeeProjectStoneJobWorkStartTime || null,
    employeeProjectStoneJobWorkEndTime:
      payload.updateRecordWork?.employeeProjectStoneJobWorkEndTime || null,
    employeeProjectStoneJobWorkRemark:
      payload.updateRecordWork?.employeeProjectStoneJobContent ?? null,
    employeeProjectStoneJobWorkFileList: (
      payload.updateRecordWork?.employeeProjectStoneJobWorkFileList ?? []
    ).map((f) => ({
      employeeProjectStoneJobWorkFileID: f.employeeProjectStoneJobWorkFileID ?? 0,
      employeeProjectStoneJobWorkFileUrl: f.employeeProjectStoneJobWorkFileUrl,
    })),
  };
  const responseData = await updateProjectStoneJobWork(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉彈窗
  showUpdateWorkJobRecordModal.value = false;

  // 顯示成功訊息
  displaySuccess("更新工作記錄成功!");

  // 成功後刷新列表
  await getWorkRecordList();
};

/** 送出快速新增工作記錄 */
const submitAddWorkJobRecordOnly = async (payload: WorkJobRecordOnlyAddPayloadMdl) => {
  if (!requireToken()) return;

  const requestData: MbsWrkJobHttpAddProjectStoneJobWorkReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobID: null,
    employeeProjectStoneJobRemark: null,
    employeeProjectStoneJobBucketList: null,
    employeeProjectStoneJobWorkStartTime: payload.employeeProjectStoneJobWorkStartTime,
    employeeProjectStoneJobWorkEndTime: payload.employeeProjectStoneJobWorkEndTime,
    employeeProjectStoneJobWorkRemark: payload.employeeProjectStoneJobWorkRemark,
    employeeProjectStoneJobWorkFileList: payload.employeeProjectStoneJobWorkFileList,
  };

  const responseData = await addProjectStoneJobWork(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "新增工作記錄失敗，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉彈窗
  showOnlyAddWorkJobRecordModal.value = false;

  // 成功後刷新列表
  await getWorkJobList();
  await getWorkRecordList();

  // 顯示成功訊息
  displaySuccess("新增工作記錄成功!");
};
//#endregion

//#region 按鈕事件
/** 點擊【查詢工項】按鈕 */
const clickSearchWorkJobBtn = () => {
  workJobListViewObj.query.pageIndex = 1;
  getWorkJobList();
};

/** 點擊【清除工項】按鈕 */
const clickClearSearchWorkJobBtn = () => {
  workJobListViewObj.query.employeeProjectID = null;
  workJobListViewObj.query.employeeProjectName = null;
  workJobListViewObj.query.employeeProjectStoneID = null;
  workJobListViewObj.query.employeeProjectStoneName = null;
  workJobListViewObj.query.employeeProjectStoneJobID = null;
  workJobListViewObj.query.employeeProjectStoneJobName = null;
  workJobListViewObj.query.employeeProjectStoneJobStatus = null;
  activityTimeRange.value = null;
};

/** 點擊【查詢工作紀錄】按鈕 */
const clickSearchWorkReocrdBtn = () => {
  workRecordListViewObj.query.pageIndex = 1;
  getWorkRecordList();
};

/** 點擊【清除工作紀錄】按鈕 */
const clickClearSearchWorkRecordBtn = () => {
  workRecordListViewObj.query.employeeProjectID = null;
  workRecordListViewObj.query.employeeProjectName = null;
  workRecordListViewObj.query.employeeProjectStoneID = null;
  workRecordListViewObj.query.employeeProjectStoneName = null;
  workRecordListViewObj.query.employeeProjectStoneJobID = null;
  workRecordListViewObj.query.employeeProjectStoneJobName = null;
  workRecordListViewObj.query.employeeID = null;
  activityTimeRange.value = null;
};

/** 點擊刪除工作記錄 */
const clickRemoveWorkRecordBtn = (workRecordID: number) => {
  showActionMenu.value = null;
  confirmWorkRecordID.value = workRecordID;
  showConfirmWorkRecordDialog.value = true;
};

/** 取消刪除工作記錄 */
const handleCancelRemoveWorkRecord = () => {
  showConfirmWorkRecordDialog.value = false;
  confirmWorkRecordID.value = null;
};

/** 點擊【新增工作記錄】按鈕 */
const clickAddBtn = () => {
  showOnlyAddWorkJobRecordModal.value = true;
};

/** 點擊【操作-編輯工作記錄】按鈕 */
const clickUpdateWorkRecordBtn = (item: WorkJobRecordListItemMdl) => {
  showActionMenu.value = null;
  currentWorkRecord.value = item;

  const hasJobContext =
    !!item.employeeProjectName &&
    !!item.employeeProjectStoneName &&
    !!item.employeeProjectStoneJobName;
  workJobRecordUpdateMode.value = hasJobContext ? "full" : "record-only";
  showUpdateWorkJobRecordModal.value = true;
};

/** 點擊【操作-檢視工作記錄】按鈕 */
const clickDetailWorkRecordBtn = (item: WorkJobRecordListItemMdl) => {
  showActionMenu.value = null;
  currentWorkRecord.value = item;

  const hasJobContext =
    !!item.employeeProjectName &&
    !!item.employeeProjectStoneName &&
    !!item.employeeProjectStoneJobName;
  workJobRecordDetailMode.value = hasJobContext ? "full" : "record-only";
  showWorkJobRecordDetailModal.value = true;
};

/** 點擊操作按鈕 */
const clickActionBtn = (id: number, event: MouseEvent) => {
  if (showActionMenu.value === id) {
    showActionMenu.value = null;
    return;
  }

  showActionMenu.value = id;

  // 計算按鈕位置
  const rect = (event.target as HTMLElement).getBoundingClientRect();
  actionMenuPosition.value = {
    top: `${rect.top + window.scrollY}px`,
    left: `${rect.right - 180 + window.scrollX}px`,
  };
};

/** 關閉操作選單 */
const closeActionMenu = () => {
  showActionMenu.value = null;
};

/** 點擊檢視工項 */
const clickWorkJobDetailBtn = (stoneJobID: number) => {
  showActionMenu.value = null;
  router.push(`/work/job/detail/${stoneJobID}`);
};

//#endregion

//#region 生命週期
onMounted(() => {
  getWorkJobList();
});
//#endregion
</script>
<template>
  <div class="flex flex-col h-full overflow-hidden p-2">
    <!-- 標題列 -->
    <div class="flex items-center flex-row gap-2 justify-between mb-3"></div>

    <!-- Tabs選單 -->
    <div class="flex mb-0 gap-y-4">
      <button
        :class="['tab-btn', { active: activeTab === TabEnum.WorkJob }]"
        @click="changeTab(TabEnum.WorkJob)"
      >
        工項
      </button>
      <button
        :class="['tab-btn', { active: activeTab === TabEnum.WorkRecord }]"
        @click="changeTab(TabEnum.WorkRecord)"
      >
        工作記錄
      </button>
    </div>

    <!-- Tabs內容 -->
    <div class="tab-content flex-1 overflow-hidden">
      <!--工項-->
      <div v-if="activeTab === TabEnum.WorkJob" class="tab h-full">
        <div
          class="flex flex-col bg-white rounded-lg p-6 gap-4 rounded-tl-none h-full overflow-y-auto"
        >
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <!-- 專案名稱 -->
              <div>
<GetManyEmployeeProjectComboBox
                  v-model:employeeProjectID="workJobListViewObj.query.employeeProjectID"
                  v-model:employeeProjectName="workJobListViewObj.query.employeeProjectName"
                  :disabled="false"
                />
              </div>

              <!-- 里程碑名稱 -->
              <div>
<GetManyEmployeeProjectStoneComboBox
                  v-model:employeeProjectStoneID="workJobListViewObj.query.employeeProjectStoneID"
                  v-model:employeeProjectStoneName="
                    workJobListViewObj.query.employeeProjectStoneName
                  "
                  :employeeProjectID="workJobListViewObj.query.employeeProjectID"
                  :disabled="!workJobListViewObj.query.employeeProjectID"
                />
              </div>

              <!-- 工項名稱 -->
              <div>
<GetManyEmployeeProjectStoneJobComboBox
                  v-model:employeeProjectStoneJobID="
                    workJobListViewObj.query.employeeProjectStoneJobID
                  "
                  v-model:employeeProjectStoneJobName="
                    workJobListViewObj.query.employeeProjectStoneJobName
                  "
                  :employeeProjectID="workJobListViewObj.query.employeeProjectID"
                  :employeeProjectStoneID="workJobListViewObj.query.employeeProjectStoneID"
                  :disabled="!workJobListViewObj.query.employeeProjectStoneID"
                />
              </div>

              <!-- 工項狀態 -->
              <div>
<select
                  v-model="workJobListViewObj.query.employeeProjectStoneJobStatus"
                  class="select-box"
                >
                  <option :value="null">全部</option>
                  <option :value="DbAtomEmployeeProjectStatusEnum.NotStarted">未開始</option>
                  <option :value="DbAtomEmployeeProjectStatusEnum.OnSchedule">如期</option>
                  <option :value="DbAtomEmployeeProjectStatusEnum.AtRisk">注意</option>
                  <option :value="DbAtomEmployeeProjectStatusEnum.Delayed">延遲</option>
                  <option :value="DbAtomEmployeeProjectStatusEnum.Completed">已完成</option>
                </select>
              </div>

              <!-- 結束日期 -->
              <div>
<VueDatePicker
                  v-model="activityTimeRange"
                  range
                  format="yyyy-MM-dd"
                  :enable-time-picker="false"
                  placeholder="選擇時間範圍"
                  class="date-picker-custom"
                />
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchWorkJobBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchWorkJobBtn">清除</button>
            </div>
          </div>

          <hr />

          <!-- 列表 -->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start w-36">專案名稱</th>
                  <th class="text-start w-36">里程碑名稱</th>
                  <th class="text-start w-44">工項名稱</th>
                  <th class="text-start w-24">預估時間</th>
                  <th class="text-start w-28">當月總時數</th>
                  <th class="text-start w-24">OnSite 時數</th>
                  <th class="text-start w-24">OffSite 時數</th>
                  <th class="text-center w-28">工項狀態</th>
                  <th class="text-start w-28">工項開始日期</th>
                  <th class="text-start w-28">工項結束日期</th>
                  <th class="text-start w-24">執行者名稱</th>
                </tr>
              </thead>

              <tbody>
                <!-- 無資料 -->
                <template v-if="workJobListViewObj.employeeProjectJobList.length === 0">
                  <tr class="text-center">
                    <td colspan="11">無資料</td>
                  </tr>
                </template>

                <!-- 有資料 -->
                <template v-else>
                  <tr
                    v-for="item in workJobListViewObj.employeeProjectJobList"
                    :key="item.employeeProjectStoneJobID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="clickWorkJobDetailBtn(item.employeeProjectStoneJobID)"
                  >
                    <!-- 專案名稱 -->
                    <td class="text-start">
                      {{ item.employeeProjectName }}
                    </td>

                    <!-- 里程碑名稱 -->
                    <td class="text-start">
                      {{ item.employeeProjectStoneName }}
                    </td>

                    <!-- 工項名稱 -->
                    <td class="text-start">
                      {{ item.employeeProjectStoneJobName }}
                    </td>

                    <!-- 預估時間 -->
                    <td class="text-start">
                      {{ formatHours(item.estimatedHours) }}
                    </td>

                    <!-- 當月總時數 -->
                    <td class="text-start">
                      {{ formatHours(item.monthTotalHours) }}
                    </td>

                    <!-- OnSite 時數 -->
                    <td class="text-start">
                      {{ formatHours(item.onsiteHours) }}
                    </td>

                    <!-- OffSite 時數 -->
                    <td class="text-start">
                      {{ formatHours(item.offsiteHours) }}
                    </td>

                    <!-- 工項狀態 -->
                    <td class="text-center">
                      <span
                        class="p-1.5 px-3 rounded-full text-sm text-nowrap"
                        :class="getStatusClass(item.employeeProjectStoneJobStatus)"
                      >
                        {{ getEmployeeProjectStatusLabel(item.employeeProjectStoneJobStatus) }}
                      </span>
                    </td>

                    <!-- 工項開始日期 -->
                    <td class="text-start">
                      {{ formatDate(item.employeeProjectStoneJobStartTime) || "-" }}
                    </td>

                    <!-- 工項結束日期 -->
                    <td class="text-start">
                      {{ formatDate(item.employeeProjectStoneJobEndTime) || "-" }}
                    </td>

                    <!-- 執行者名稱 -->
                    <td class="text-start">
                      {{
                        item.employeeProjectStoneJobExecutorList.length > 0
                          ? item.employeeProjectStoneJobExecutorList
                              .map((e) => e.employeeProjectStoneJobExecutorName)
                              .join("、")
                          : "-"
                      }}
                    </td>

                  </tr>
                </template>
              </tbody>
            </table>
          </div>

          <button
            v-if="employeeInfoStore.hasPermission(menu, 'create')"
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
            style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickAddBtn"
          >
            +新增工作記錄
          </button>

          <!-- 分頁 -->
          <div class="flex justify-center pt-3">
            <Pagination
              :pageIndex="workJobListViewObj.query.pageIndex"
              :pageSize="workJobListViewObj.query.pageSize"
              :totalCount="workJobListViewObj.totalCount"
              @update:current-page="
                (newPage) => {
                  workJobListViewObj.query.pageIndex = newPage;
                  getWorkJobList();
                }
              "
              @update:page-size="
                (newSize) => {
                  workJobListViewObj.query.pageSize = newSize;
                  workJobListViewObj.query.pageIndex = 1;
                  getWorkJobList();
                }
              "
            />
          </div>
        </div>
      </div>

      <!--工作紀錄-->
      <div v-if="activeTab === TabEnum.WorkRecord" class="tab h-full">
        <div
          class="flex flex-col bg-white rounded-lg p-6 gap-4 rounded-tl-none h-full overflow-y-auto"
        >
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <!-- 日期 -->
              <div>
<VueDatePicker
                  v-model="activityTimeRange"
                  range
                  format="yyyy-MM-dd"
                  :enable-time-picker="false"
                  placeholder="選擇時間範圍"
                  class="date-picker-custom"
                />
              </div>

              <!-- 專案名稱 -->
              <div>
<GetManyEmployeeProjectComboBox
                  v-model:employeeProjectID="workRecordListViewObj.query.employeeProjectID"
                  v-model:employeeProjectName="workRecordListViewObj.query.employeeProjectName"
                  :disabled="false"
                />
              </div>

              <!-- 里程碑名稱 -->
              <div>
<GetManyEmployeeProjectStoneComboBox
                  v-model:employeeProjectStoneID="
                    workRecordListViewObj.query.employeeProjectStoneID
                  "
                  v-model:employeeProjectStoneName="
                    workRecordListViewObj.query.employeeProjectStoneName
                  "
                  :employeeProjectID="workRecordListViewObj.query.employeeProjectID"
                  :disabled="!workRecordListViewObj.query.employeeProjectID"
                />
              </div>

              <!-- 工項名稱 -->
              <div>
<GetManyEmployeeProjectStoneJobComboBox
                  v-model:employeeProjectStoneJobID="
                    workRecordListViewObj.query.employeeProjectStoneJobID
                  "
                  v-model:employeeProjectStoneJobName="
                    workRecordListViewObj.query.employeeProjectStoneJobName
                  "
                  :employeeProjectID="workRecordListViewObj.query.employeeProjectID"
                  :employeeProjectStoneID="workRecordListViewObj.query.employeeProjectStoneID"
                  :disabled="!workRecordListViewObj.query.employeeProjectStoneID"
                />
              </div>

              <div v-if="false">
<GetManyEmployeeProjectComboBox
                  v-model:employeeProjectID="workRecordListViewObj.query.employeeProjectID"
                  v-model:employeeProjectName="workRecordListViewObj.query.employeeProjectName"
                  :disabled="false"
                  label="員工"
                  :is-employee="true"
                />
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchWorkReocrdBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchWorkRecordBtn">清除</button>
            </div>
          </div>

          <hr />

          <!-- 列表 -->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start w-32">日期</th>
                  <th class="text-start w-32">時間</th>
                  <th class="text-start w-32">專案名稱</th>
                  <th class="text-start w-32">里程碑名稱</th>
                  <th class="text-start w-32">工項名稱</th>
                  <th class="text-start w-32">員工</th>
                  <th class="text-center w-48">工作內容</th>
                  <th class="text-center w-24">操作</th>
                </tr>
              </thead>

              <tbody>
                <!-- 無資料 -->
                <template v-if="workRecordListViewObj.employeeProjectJobRecordList.length === 0">
                  <tr class="text-center">
                    <td colspan="8">無資料</td>
                  </tr>
                </template>

                <!-- 有資料 -->
                <template v-else>
                  <tr
                    v-for="item in workRecordListViewObj.employeeProjectJobRecordList"
                    :key="item.employeeProjectStoneJobWorkID"
                  >
                    <!-- 日期 -->
                    <td class="text-start">
                      {{ formatDate(item.employeeProjectStoneJobWorkStartTime) || "-" }}
                    </td>

                    <!-- 時間 -->
                    <td class="text-start">
                      {{ formatTime(item.employeeProjectStoneJobWorkStartTime) }}
                      ~
                      {{ formatTime(item.employeeProjectStoneJobWorkEndTime) }}
                    </td>

                    <!-- 專案名稱 -->
                    <td class="text-start">
                      {{ item.employeeProjectName || "-" }}
                    </td>

                    <!-- 里程碑名稱 -->
                    <td class="text-start">
                      {{ item.employeeProjectStoneName || "-" }}
                    </td>

                    <!-- 工項名稱 -->
                    <td class="text-start">
                      {{ item.employeeProjectStoneJobName || "-" }}
                    </td>

                    <!-- 員工 -->
                    <td class="text-start">
                      {{ item.employeeName || "-" }}
                    </td>

                    <!-- 工作內容 -->
                    <td class="text-start">
                      <div class="truncate max-w-full">
                        {{ item.employeeProjectStoneJobWorkRemark || "-" }}
                      </div>
                    </td>

                    <!-- 操作 -->
                    <td class="text-center relative">
                      <div class="inline-block relative">
                        <button
                          v-if="employeeInfoStore.hasPermission(menu, 'view')"
                          class="btn-detail"
                          @click="clickActionBtn(item.employeeProjectStoneJobWorkID, $event)"
                        >
                          操作
                        </button>

                        <!-- 操作選單 -->
                        <div
                          v-if="showActionMenu === item.employeeProjectStoneJobWorkID"
                          class="action-menu"
                          :style="actionMenuPosition"
                        >
                          <button class="action-menu-item" @click="clickDetailWorkRecordBtn(item)">
                            檢視工作記錄
                          </button>

                          <button class="action-menu-item" @click="clickUpdateWorkRecordBtn(item)">
                            編輯工作記錄
                          </button>
                          <button
                            class="action-menu-item"
                            @click="clickRemoveWorkRecordBtn(item.employeeProjectStoneJobWorkID)"
                          >
                            <span class="text-red-600"> 刪除工作記錄 </span>
                          </button>
                        </div>
                      </div>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>

          <!-- 分頁 -->
          <div class="flex justify-center pt-3">
            <Pagination
              :pageIndex="workRecordListViewObj.query.pageIndex"
              :pageSize="workRecordListViewObj.query.pageSize"
              :totalCount="workRecordListViewObj.totalCount"
              @update:current-page="
                (newPage) => {
                  workRecordListViewObj.query.pageIndex = newPage;
                  getWorkRecordList();
                }
              "
              @update:page-size="
                (newSize) => {
                  workRecordListViewObj.query.pageSize = newSize;
                  workRecordListViewObj.query.pageIndex = 1;
                  getWorkRecordList();
                }
              "
            />
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- 背景遮罩 - 點擊關閉選單 -->
  <div v-if="showActionMenu !== null" class="fixed inset-0 z-10" @click="closeActionMenu"></div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

  <!-- 新增工作記錄彈跳視窗（簡化版：從頂部按鈕進入） -->
  <WorkJobRecordOnlyAdd
    v-if="showOnlyAddWorkJobRecordModal"
    :employeeProjectJobID="workJobListViewObj.query.employeeProjectStoneJobID ?? 0"
    @submit="submitAddWorkJobRecordOnly"
    @close="showOnlyAddWorkJobRecordModal = false"
  />

  <!-- 更新工作記錄彈跳視窗 -->
  <WorkJobRecordUpdate
    v-if="showUpdateWorkJobRecordModal && currentWorkRecord"
    :employeeProjectStoneJobWorkID="currentWorkRecord.employeeProjectStoneJobWorkID"
    :employeeProjectName="currentWorkRecord.employeeProjectName"
    :employeeProjectStoneName="currentWorkRecord.employeeProjectStoneName"
    :employeeProjectStoneTaskName="currentWorkRecord.employeeProjectStoneJobName"
    :mode="workJobRecordUpdateMode"
    @submit="clickUpdateSubmitJobWorkBtn"
    @close="showUpdateWorkJobRecordModal = false"
  />

  <!-- 詳細工作記錄彈跳視窗 -->
  <WorkJobRecordDetail
    v-if="showWorkJobRecordDetailModal && currentWorkRecord"
    :employeeProjectStoneJobWorkID="currentWorkRecord.employeeProjectStoneJobWorkID"
    :employeeProjectName="currentWorkRecord.employeeProjectName"
    :employeeProjectStoneName="currentWorkRecord.employeeProjectStoneName"
    :employeeProjectStoneTaskName="currentWorkRecord.employeeProjectStoneJobName"
    :mode="workJobRecordDetailMode"
    @close="showWorkJobRecordDetailModal = false"
  />

  <!-- 確認刪除工作記錄彈跳視窗  -->
  <ConfirmDialog
    :show="showConfirmWorkRecordDialog"
    title="確認刪除"
    message="確定要刪除工作記錄嗎？"
    confirm-text="確定"
    cancel-text="取消"
    @confirm="handleRemoveWorkRecord()"
    @cancel="handleCancelRemoveWorkRecord()"
  />
</template>
<style scoped>
.action-menu {
  position: fixed;
  top: 0;
  left: calc(100% + 8px);
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  box-shadow:
    0 4px 6px -1px rgba(0, 0, 0, 0.1),
    0 2px 4px -1px rgba(0, 0, 0, 0.06);
  z-index: 20;
  min-width: 120px;
  overflow: hidden;
}

.action-menu-item {
  display: block;
  width: 100%;
  padding: 12px 16px;
  background: white;
  border: none;
  text-align: left;
  font-size: 14px;
  color: #374151;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.action-menu-item:hover {
  background-color: #f3f4f6;
}

.action-menu-item:not(:last-child) {
  border-bottom: 1px solid #f3f4f6;
}

/* DatePicker */
.date-picker-custom :deep(.dp__input_wrap) {
  height: 32px !important;
}

.date-picker-custom :deep(.dp__input) {
  height: 32px !important;
  padding: 0 12px !important;
  border: 1px solid #d1d5db !important;
  border-radius: 4px !important;
  font-size: 14px !important;
  line-height: 1.5 !important;
  box-sizing: border-box !important;
}

.date-picker-custom :deep(.dp__input:focus) {
  border-color: #3b82f6 !important;
  outline: none !important;
}

.date-picker-custom :deep(.dp__input:hover:not(.dp__input_focus)) {
  border-color: #9ca3af !important;
}

/* 調整圖示位置 */
.date-picker-custom :deep(.dp__input_icon),
.date-picker-custom :deep(.dp--clear-btn) {
  height: 16px !important;
  width: 16px !important;
  display: none !important;
}
</style>
