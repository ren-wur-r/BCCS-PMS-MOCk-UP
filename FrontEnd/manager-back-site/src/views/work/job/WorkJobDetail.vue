<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, defineAsyncComponent, watch, onBeforeUnmount } from "vue";
import { useRoute, useRouter } from "vue-router";
// Enums / 常數
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
// Stores
import { useTokenStore } from "@/stores/token";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
// Composables
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
// Services
import type {
  MbsWrkJobHttpAddProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpAddProjectStoneJobWorkRspMdl,
  MbsWrkJobHttpGetProjectStoneJobReqMdl,
  MbsWrkJobHttpGetProjectStoneJobRspMdl,
  MbsWrkJobHttpUpdateProjectStoneJobReqMdl,
  MbsWrkJobHttpUpdateProjectStoneJobRspMdl,
} from "@/services/pms-http/work/job/workJobHttpFormat";
import {
  addProjectStoneJobWork,
  getProjectStoneJob,
  updateProjectStoneJob,
  updateProjectStoneJobWork,
} from "@/services";
// Components
import TextCounter from "@/components/global/counter/TextCounter.vue";
import { AddWorkJobRecordPayloadMdl } from "./components/WorkJobRecordAdd.vue";
import {
  UpdateWorkJobPayloadMdl,
  UpdateWorkRecordPayloadMdl,
} from "./components/WorkJobRecordUpdate.vue";
const WorkJobRecordUpdate = defineAsyncComponent(
  () => import("./components/WorkJobRecordUpdate.vue")
);
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);
const SuccessToast = defineAsyncComponent(
  () => import("@/components/global/feedback/SuccessToast.vue")
);
const WorkJobRecordAdd = defineAsyncComponent(() => import("./components/WorkJobRecordAdd.vue"));
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { handleErrorCode, errorMessage, showError, closeError } = useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
/** 路由物件（用於取得路由參數） */
const route = useRoute();
//#endregion

//#region 型別定義
/** 工作-工項管理-檢視模型 */
interface WrkJobDetailViewMdl {
  employeeProjectName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
  employeeProjectStoneName: string;
  employeeProjectStoneStartTime: string;
  employeeProjectStoneEndTime: string;
  employeeProjectStoneJobName: string;
  employeeProjectStoneJobStartTime: string;
  employeeProjectStoneJobEndTime: string;
  employeeProjectStoneJobStatus: number;
  employeeProjectStoneJobRemark: string;
  employeeProjectStoneJobExecutorList: WorkPrjJobExecutorMdl[];
  employeeProjectStoneJobBucketList: WorkPrjJobBucketMdl[];
  employeeProjectStoneJobWorkList: WorkPrjJobWorkMdl[];
  employeeProjectStoneJobWorkFileList: WorkPrjJobWorkFileMdl[];
  currentBucketID: number;
  currentWorkID: number;
}

/** 工作-工項管理-工作清單模型 */
interface WorkPrjJobBucketMdl {
  employeeProjectStoneJobBucketID: number;
  employeeProjectStoneJobBucketName: string;
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 工作-工項管理-工作記錄模型 */
interface WorkPrjJobWorkMdl {
  employeeProjectStoneJobWorkID: number;
  employeeProjectStoneJobWorkStartTime: string;
  employeeProjectStoneJobWorkEndTime: string;
  employeeProjectStoneJobWorkRemark: string;
  employeeName: string;
}

/** 工作-工項管理-工作檔案模型 */
interface WorkPrjJobWorkFileMdl {
  employeeProjectStoneJobWorkFileUrl: string;
}

/** 工作-工項管理-執行者模型 */
interface WorkPrjJobExecutorMdl {
  employeeProjectStoneJobExecutorName: string;
}
//#endregion

//#region 頁面物件
enum TabEnum {
  JobDetail = "JobDetail",
  WorkRecord = "WorkRecord",
}
/** 當前目錄 */
const menu = DbAtomMenuEnum.WorkJob;
/** 工項ID */
const employeeProjectStoneJobID = Number(route.params.id);
/** 詳情分頁 */
const activeTab = ref<TabEnum>(TabEnum.JobDetail);
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 是否顯示【新增工作紀錄】彈跳視窗 */
const showAddWorkJobRecordModal = ref(false);
/** 是否顯示【編輯工作紀錄】彈跳視窗 */
const showUpdateWorkJobRecordModal = ref(false);
/** 是否為權限錯誤 */
const isPermissionError = ref(false);
/** 暫存清單 ID */
let tempBucketId = -1;
/** 工作-工項管理-檢視物件 */
const workJobDetailViewObj = reactive<WrkJobDetailViewMdl>({
  employeeProjectName: "",
  employeeProjectStartTime: "",
  employeeProjectEndTime: "",
  employeeProjectStoneName: "",
  employeeProjectStoneStartTime: "",
  employeeProjectStoneEndTime: "",
  employeeProjectStoneJobName: "",
  employeeProjectStoneJobStartTime: "",
  employeeProjectStoneJobEndTime: "",
  employeeProjectStoneJobStatus: DbAtomEmployeeProjectStatusEnum.NotStarted,
  employeeProjectStoneJobRemark: "",
  employeeProjectStoneJobExecutorList: [],
  employeeProjectStoneJobBucketList: [],
  employeeProjectStoneJobWorkList: [],
  employeeProjectStoneJobWorkFileList: [],

  currentBucketID: 0,
  currentWorkID: 0,
});
//#endregion

//#region API / 資料流程
/** 取得資料 */
const getData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  const requestData: MbsWrkJobHttpGetProjectStoneJobReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobID: employeeProjectStoneJobID,
  };

  const responseData: MbsWrkJobHttpGetProjectStoneJobRspMdl | null =
    await getProjectStoneJob(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    if (responseData.errorCode === MbsErrorCodeEnum.PermissionDenied) {
      isPermissionError.value = true;
    }
    return;
  }

  workJobDetailViewObj.employeeProjectName = responseData.employeeProjectName;
  workJobDetailViewObj.employeeProjectStartTime = responseData.employeeProjectStartTime;
  workJobDetailViewObj.employeeProjectEndTime = responseData.employeeProjectEndTime;
  workJobDetailViewObj.employeeProjectStoneName = responseData.employeeProjectStoneName;
  workJobDetailViewObj.employeeProjectStoneStartTime = responseData.employeeProjectStoneStartTime;
  workJobDetailViewObj.employeeProjectStoneEndTime = responseData.employeeProjectStoneEndTime;
  workJobDetailViewObj.employeeProjectStoneJobName = responseData.employeeProjectStoneJobName;
  workJobDetailViewObj.employeeProjectStoneJobStartTime =
    responseData.employeeProjectStoneJobStartTime;
  workJobDetailViewObj.employeeProjectStoneJobEndTime = responseData.employeeProjectStoneJobEndTime;
  workJobDetailViewObj.employeeProjectStoneJobStatus = responseData.employeeProjectStoneJobStatus;
  workJobDetailViewObj.employeeProjectStoneJobRemark = responseData.employeeProjectStoneJobRemark;
  workJobDetailViewObj.employeeProjectStoneJobExecutorList =
    responseData.employeeProjectStoneJobExecutorList.map((item) => {
      return {
        employeeProjectStoneJobExecutorName: item.employeeProjectStoneJobExecutorName,
      };
    });
  workJobDetailViewObj.employeeProjectStoneJobBucketList =
    responseData.employeeProjectStoneJobBucketList.map((item) => {
      return {
        employeeProjectStoneJobBucketID: item.employeeProjectStoneJobBucketID,
        employeeProjectStoneJobBucketName: item.employeeProjectStoneJobBucketName,
        employeeProjectStoneJobBucketIsFinished: item.employeeProjectStoneJobBucketIsFinished,
      };
    });
  workJobDetailViewObj.employeeProjectStoneJobWorkList =
    responseData.employeeProjectStoneJobWorkList.map((item) => {
      return {
        employeeProjectStoneJobWorkID: item.employeeProjectStoneJobWorkID,
        employeeName: item.employeeName,
        employeeProjectStoneJobWorkStartTime: item.employeeProjectStoneJobWorkStartTime,
        employeeProjectStoneJobWorkEndTime: item.employeeProjectStoneJobWorkEndTime,
        employeeProjectStoneJobWorkRemark: item.employeeProjectStoneJobWorkRemark,
      };
    });
  workJobDetailViewObj.employeeProjectStoneJobWorkFileList =
    responseData.employeeProjectStoneJobWorkFileList.map((file) => ({
      employeeProjectStoneJobWorkFileUrl: file.employeeProjectStoneJobWorkFileUrl,
    }));
};

/** 點擊【新增送出工作記錄】按鈕  */
const clickAddSubmitWorkJobRecord = async (payload: AddWorkJobRecordPayloadMdl) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsWrkJobHttpAddProjectStoneJobWorkReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobID: employeeProjectStoneJobID,
    employeeProjectStoneJobRemark: payload.employeeProjectStoneJobRemark,
    employeeProjectStoneJobBucketList: payload.employeeProjectStoneJobBucketList,
    employeeProjectStoneJobWorkStartTime: payload.employeeProjectStoneJobWorkStartTime,
    employeeProjectStoneJobWorkEndTime: payload.employeeProjectStoneJobWorkEndTime,
    employeeProjectStoneJobWorkRemark: payload.employeeProjectStoneJobWorkRemark,
    employeeProjectStoneJobWorkFileList: payload.employeeProjectStoneJobWorkFileList,
  };

  // 呼叫 API
  const responseData: MbsWrkJobHttpAddProjectStoneJobWorkRspMdl | null =
    await addProjectStoneJobWork(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const success = handleErrorCode(responseData.errorCode);
  if (!success) return;

  // 關閉 Dialog
  showAddWorkJobRecordModal.value = false;

  // 顯示成功訊息
  displaySuccess("新增工作記錄成功!");

  // 重新載入資料
  await getData();
};

/** 點擊【編輯送出工項內容】按鈕 */
const clickUpdateSubmitBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 子項目至少一項
  if (workJobDetailViewObj.employeeProjectStoneJobBucketList.length === 0) {
    showError.value = true;
    errorMessage.value = "子項目至少需保留一項";
    return;
  }
  // 驗證：檢查子項目名稱是否都已填寫
  const hasEmptyBucket = workJobDetailViewObj.employeeProjectStoneJobBucketList.some(
    (bucket) => !bucket.employeeProjectStoneJobBucketName.trim()
  );
  if (hasEmptyBucket) {
    showError.value = true;
    errorMessage.value = "請填寫所有子項目名稱";
    return;
  }

  // 呼叫 api
  const requestData: MbsWrkJobHttpUpdateProjectStoneJobReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobID: employeeProjectStoneJobID,
    employeeProjectStoneJobRemark: workJobDetailViewObj.employeeProjectStoneJobRemark,
    employeeProjectStoneJobBucketList: workJobDetailViewObj.employeeProjectStoneJobBucketList.map(
      (item) => ({
        employeeProjectStoneJobBucketID: item.employeeProjectStoneJobBucketID,
        employeeProjectStoneJobBucketName: item.employeeProjectStoneJobBucketName,
        employeeProjectStoneJobBucketIsFinished: item.employeeProjectStoneJobBucketIsFinished,
      })
    ),
  };
  const responseData: MbsWrkJobHttpUpdateProjectStoneJobRspMdl | null =
    await updateProjectStoneJob(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  isEditMode.value = false;

  // 顯示成功訊息
  displaySuccess("更新工項內容成功！");

  // 重新載入資料
  await getData();
};

/** 點擊【編輯送出工作記錄】按鈕 */
const clickUpdateSubmitJobWorkBtn = async (payload: {
  updateJob: UpdateWorkJobPayloadMdl | null;
  updateRecordWork: UpdateWorkRecordPayloadMdl | null;
}) => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobID: employeeProjectStoneJobID,
    employeeProjectStoneJobRemark: payload.updateJob?.employeeProjectStoneTaskRemark ?? null,
    employeeProjectStoneJobBucketList: payload.updateJob?.employeeProjectStoneTaskBucketList ?? [],

    employeeProjectStoneJobWorkID: workJobDetailViewObj.currentWorkID,
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

  // 關閉 Dialog
  showUpdateWorkJobRecordModal.value = false;

  // 顯示成功訊息
  displaySuccess("更新工作記錄成功！");

  // 重新載入資料
  await getData();
};

//#endregion

//#region 工具
/** 日期格式化 */
const formatDate = (d: string) => {
  if (!d) return "-";
  return d.split("T")[0];
};

/** 計算時數 */
const calcHours = (start: string, end: string) => {
  const s = new Date(start);
  const e = new Date(end);
  return ((e.getTime() - s.getTime()) / (1000 * 60 * 60)).toFixed(1);
};
// #endregion

//#region 按鈕事件
/** 新增子項目 */
const addBucket = () => {
  // 驗證：檢查是否有空白的子項目名稱
  const hasEmptyBucket = workJobDetailViewObj.employeeProjectStoneJobBucketList.some(
    (bucket) => !bucket.employeeProjectStoneJobBucketName.trim()
  );

  if (hasEmptyBucket) {
    showError.value = true;
    errorMessage.value = "請先填寫所有子項目名稱後再新增";
    return;
  }

  workJobDetailViewObj.employeeProjectStoneJobBucketList.push({
    employeeProjectStoneJobBucketID: tempBucketId,
    employeeProjectStoneJobBucketName: "",
    employeeProjectStoneJobBucketIsFinished: false,
  });
  tempBucketId--;
};

/** 刪除子項目 */
const removeBucket = (index: number) => {
  workJobDetailViewObj.employeeProjectStoneJobBucketList.splice(index, 1);
};

/** 點擊【取消編輯工作】按鈕 */
const clickCancelJobBtn = () => {
  isEditMode.value = false;
  getData();
};

/** 點擊返回按鈕 */
const clickBackBtn = () => {
  router.push("/work/job");
};

/** 點擊【編輯工作】按鈕 */
const clickUpdateJobBtn = () => {
  isEditMode.value = true;
};

/** 點擊【操作-新增工作記錄】按鈕 */
const clickAddJobRecordBtn = () => {
  showAddWorkJobRecordModal.value = true;
};

/** 點擊【操作-編輯工作記錄】按鈕 */
const clickUpdateJobWorkBtn = (workID: number) => {
  showUpdateWorkJobRecordModal.value = true;
  workJobDetailViewObj.currentWorkID = workID;
};

/** 切換分頁 */
const changeTab = (tab: TabEnum) => {
  activeTab.value = tab;
};

/** 處理關閉錯誤訊息 */
const handleCloseError = () => {
  closeError();
  // 權限錯誤則返回列表頁
  if (isPermissionError.value) {
    isPermissionError.value = false;
    router.push("/work/job");
  }
};
//#endregion

//#region 生命週期
onMounted(() => {
  getData();
});

watch(
  () => activeTab.value,
  (tab) => {
    setModuleTitle(tab === TabEnum.WorkRecord ? "工作記錄" : "工項內容");
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
    <!-- 標題區 -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-4">
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
    </div>

    <!-- Tabs -->
    <div class="flex mb-0 gap-y-4">
      <button
        :class="['tab-btn', { active: activeTab === TabEnum.JobDetail }]"
        @click="changeTab(TabEnum.JobDetail)"
      >
        工項內容
      </button>
      <button
        :class="['tab-btn', { active: activeTab === TabEnum.WorkRecord }]"
        @click="changeTab(TabEnum.WorkRecord)"
      >
        工作記錄
      </button>
    </div>

    <!-- 工項內容 -->
    <div v-if="activeTab === TabEnum.JobDetail" class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between">
        <div class="subtitle">工項內容</div>
        <div class="flex gap-4 w-full justify-end">
          <!--編輯模式-->
          <template v-if="isEditMode">
            <div class="flex gap-2">
              <button class="btn-cancel" @click="clickCancelJobBtn()">取消編輯</button>
              <button class="btn-submit" @click="clickUpdateSubmitBtn()">儲存</button>
            </div>
          </template>
          <!--檢視模式-->
          <template v-else>
            <button
              v-if="employeeInfoStore.hasPermission(menu, 'edit')"
              class="btn-update"
              @click="clickUpdateJobBtn"
            >
              編輯
            </button>
          </template>
        </div>
      </div>

      <!-- 專案名稱 專案開始 專案結束 -->
      <div class="grid grid-cols-3 gap-4">
        <div>
          <label class="form-label">專案名稱</label>
          <input
            class="input-box"
            :value="workJobDetailViewObj.employeeProjectName"
            :disabled="true"
          />
        </div>

        <div>
          <label class="form-label">專案開始時間</label>
          <input
            class="input-box"
            :value="formatDate(workJobDetailViewObj.employeeProjectStartTime)"
            :disabled="true"
          />
        </div>

        <div>
          <label class="form-label">專案結束時間</label>
          <input
            class="input-box"
            :value="formatDate(workJobDetailViewObj.employeeProjectEndTime)"
            :disabled="true"
          />
        </div>
      </div>

      <hr />

      <!--里程碑名稱 開始時間 結束時間-->
      <div class="grid grid-cols-3 gap-4">
        <div>
          <label class="form-label">里程碑名稱</label>
          <input
            class="input-box"
            :value="workJobDetailViewObj.employeeProjectStoneName"
            :disabled="true"
          />
        </div>

        <div>
          <label class="form-label">里程碑開始時間</label>
          <input
            class="input-box"
            :value="formatDate(workJobDetailViewObj.employeeProjectStoneStartTime)"
            :disabled="true"
          />
        </div>

        <div>
          <label class="form-label">里程碑結束時間</label>
          <input
            class="input-box"
            :value="formatDate(workJobDetailViewObj.employeeProjectStoneEndTime)"
            :disabled="true"
          />
        </div>
      </div>

      <hr />

      <!--工項名稱 開始時間 結束時間- -->
      <div class="grid grid-cols-3 gap-4">
        <div>
          <label class="form-label">工項名稱</label>
          <input
            class="input-box"
            :value="workJobDetailViewObj.employeeProjectStoneJobName"
            :disabled="true"
          />
        </div>

        <div>
          <label class="form-label">工項開始時間</label>
          <input
            class="input-box"
            :value="formatDate(workJobDetailViewObj.employeeProjectStoneJobStartTime)"
            :disabled="true"
          />
        </div>

        <div>
          <label class="form-label">工項結束時間</label>
          <input
            class="input-box"
            :value="formatDate(workJobDetailViewObj.employeeProjectStoneJobEndTime)"
            :disabled="true"
          />
        </div>
      </div>

      <!-- 工項狀態 執行者 -->
      <div class="grid grid-cols-2 gap-4">
        <div>
          <label class="form-label">工項狀態</label>
          <select
            v-model="workJobDetailViewObj.employeeProjectStoneJobStatus"
            class="select-box"
            :disabled="true"
          >
            <option :value="DbAtomEmployeeProjectStatusEnum.NotAssigned">未指派</option>
            <option :value="DbAtomEmployeeProjectStatusEnum.NotStarted">未開始</option>
            <option :value="DbAtomEmployeeProjectStatusEnum.OnSchedule">如期</option>
            <option :value="DbAtomEmployeeProjectStatusEnum.AtRisk">注意</option>
            <option :value="DbAtomEmployeeProjectStatusEnum.Delayed">延遲</option>
            <option :value="DbAtomEmployeeProjectStatusEnum.Completed">已完成</option>
          </select>
        </div>

        <div>
          <label class="form-label">執行者</label>
          <input
            class="input-box"
            :value="
              workJobDetailViewObj.employeeProjectStoneJobExecutorList
                .map((x) => x.employeeProjectStoneJobExecutorName)
                .join('、')
            "
            :disabled="true"
          />
        </div>
      </div>

      <!-- 備註 -->
      <div class="flex flex-col gap-2">
        <label class="form-label">備註</label>
        <textarea
          v-model="workJobDetailViewObj.employeeProjectStoneJobRemark"
          class="textarea-box resize-none"
          rows="4"
          placeholder="請輸入備註"
          :disabled="!isEditMode"
        ></textarea>

        <TextCounter
          :text="workJobDetailViewObj.employeeProjectStoneJobRemark"
          :max-length="2000"
          :required="true"
        />
      </div>

      <!-- 子項目 Bucket -->
      <div class="flex flex-col gap-2 border p-5 rounded-md">
        <div class="flex justify-between items-center">
          <label class="font-bold text-sm text-gray-700">子項目</label>
          <button v-if="isEditMode" class="btn-add" @click="addBucket">新增子項目</button>
        </div>
        <div class="flex flex-col gap-2 mt-3">
          <!-- 沒有子項目時 -->
          <p
            v-if="workJobDetailViewObj.employeeProjectStoneJobBucketList.length === 0"
            class="text-gray-400 text-sm text-center py-2"
          >
            無子項目
          </p>
          <div
            v-for="(bucket, idx) in workJobDetailViewObj.employeeProjectStoneJobBucketList"
            :key="idx"
            class="flex items-center gap-3"
            :class="isEditMode ? 'bg-gray-100 py-1 px-1 rounded' : ''"
          >
            <!--勾選框-->
            <input
              :id="'bucket_' + idx"
              v-model="bucket.employeeProjectStoneJobBucketIsFinished"
              type="checkbox"
              :disabled="!isEditMode"
              :class="isEditMode ? 'ml-2' : ''"
            />
            <!--編輯模式：顯示輸入框-->
            <template v-if="isEditMode">
              <input
                v-model="bucket.employeeProjectStoneJobBucketName"
                :class="[
                  'input-box flex-1',
                  !bucket.employeeProjectStoneJobBucketName ? 'border-red-500' : '',
                ]"
                placeholder="子項目名稱"
              />
              <button class="mr-3" @click="removeBucket(idx)">
                <svg
                  width="11"
                  height="11"
                  viewBox="0 0 11 11"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M0.75 9.48833L5.11917 5.11917L9.48833 9.48833M9.48833 0.75L5.11833 5.11917L0.75 0.75"
                    stroke="#101828"
                    stroke-width="1.5"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  />
                </svg>
              </button>
            </template>
            <!--檢視模式：顯示文字-->
            <template v-else>
              <label :for="'bucket_' + idx" class="flex-1 text-sm rounded cursor-pointer">
                {{ bucket.employeeProjectStoneJobBucketName }}
              </label>
            </template>
          </div>
        </div>
      </div>
    </div>

    <!-- 工作記錄 -->
    <div
      v-if="activeTab === TabEnum.WorkRecord"
      class="flex flex-col gap-6 p-8 bg-white rounded-lg"
    >
      <div class="flex flex-row gap-2 justify-between w-full">
        <h2 class="subtitle">工作記錄</h2>
        <button
          v-if="employeeInfoStore.hasPermission(menu, 'create')"
          class="btn-add"
          @click="clickAddJobRecordBtn"
        >
          新增工作記錄
        </button>
      </div>

      <table class="table-base table-fixed w-full">
        <thead class="bg-gray-800 text-white">
          <tr>
            <th class="text-start">日期</th>
            <th class="text-start">執行者</th>
            <th class="text-end">時數</th>
            <th class="text-start">工作內容</th>
            <th class="text-center">操作</th>
          </tr>
        </thead>

        <tbody>
          <tr v-if="workJobDetailViewObj.employeeProjectStoneJobWorkList.length === 0">
            <td colspan="5" class="text-center">無資料</td>
          </tr>

          <tr
            v-for="work in workJobDetailViewObj.employeeProjectStoneJobWorkList"
            :key="work.employeeProjectStoneJobWorkID"
          >
            <td class="text-start">{{ formatDate(work.employeeProjectStoneJobWorkEndTime) }}</td>
            <td class="text-start">{{ work.employeeName }}</td>
            <td class="text-end">
              {{
                work.employeeProjectStoneJobWorkEndTime
                  ? calcHours(
                      work.employeeProjectStoneJobWorkStartTime,
                      work.employeeProjectStoneJobWorkEndTime
                    )
                  : "-"
              }}
            </td>
            <td class="text-start">{{ work.employeeProjectStoneJobWorkRemark }}</td>
            <td class="text-center relative">
              <div class="inline-block relative">
                <button
                  v-if="employeeInfoStore.hasPermission(menu, 'edit')"
                  class="btn-update"
                  @click="clickUpdateJobWorkBtn(work.employeeProjectStoneJobWorkID)"
                >
                  編輯
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- 附加檔案 -->
      <div class="flex flex-col gap-4 border-t pt-6">
        <h2 class="subtitle">附加檔案</h2>

        <div
          v-if="workJobDetailViewObj.employeeProjectStoneJobWorkFileList.length === 0"
          class="text-gray-500"
        >
          無附件
        </div>

        <div v-else class="flex flex-col gap-3">
          <a
            v-for="file in workJobDetailViewObj.employeeProjectStoneJobWorkFileList"
            :key="file.employeeProjectStoneJobWorkFileUrl"
            :href="file.employeeProjectStoneJobWorkFileUrl"
            target="_blank"
            class="text-gray-500 text-sm underline"
          >
            {{ file.employeeProjectStoneJobWorkFileUrl }}
          </a>
        </div>
      </div>
    </div>

    <!-- 錯誤訊息 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="handleCloseError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 新增工作記錄彈跳視窗 -->
    <WorkJobRecordAdd
      v-if="showAddWorkJobRecordModal"
      :employeeProjectJobID="employeeProjectStoneJobID"
      :employeeProjectStoneTaskRemark="workJobDetailViewObj.employeeProjectStoneJobRemark"
      :employeeProjectStoneTaskBucketList="workJobDetailViewObj.employeeProjectStoneJobBucketList"
      @submit="clickAddSubmitWorkJobRecord"
      @close="
        showAddWorkJobRecordModal = false;
        getData();
      "
    />

    <!-- 更新工作記錄彈跳視窗 -->
    <WorkJobRecordUpdate
      v-if="showUpdateWorkJobRecordModal"
      :employeeProjectStoneJobWorkID="workJobDetailViewObj.currentWorkID"
      :employeeProjectName="workJobDetailViewObj.employeeProjectName"
      :employeeProjectStoneName="workJobDetailViewObj.employeeProjectStoneName"
      :employeeProjectStoneTaskName="workJobDetailViewObj.employeeProjectStoneJobName"
      :mode="null"
      @submit="clickUpdateSubmitJobWorkBtn"
      @close="
        showUpdateWorkJobRecordModal = false;
        getData();
      "
    />
  </div>
</template>
