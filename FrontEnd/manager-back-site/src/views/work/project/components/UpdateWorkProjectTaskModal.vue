<script setup lang="ts">
// ==================== Imports ====================
// Vue
import { reactive, onMounted, defineAsyncComponent } from "vue";
// Stores
import { useTokenStore } from "@/stores/token";
import { useProjectTimeInfoStore } from "@/stores/projectTimeInfo";
// Composables
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
// Services
import {
  getProjectStoneTask,
  updateProjectStoneTask,
  updateProjectStoneTaskBucket,
} from "@/services";
import type {
  MbsWrkPrjHttpGetProjectStoneTaskReqMdl,
  MbsWrkPrjHttpGetProjectStoneTaskRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskReqMdl,
  MbsWrkPrjHttpUpdateProjectStoneTaskRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
// Enums
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
// Utils
import { formatDate } from "@/utils/timeFormatter";
// Components
import GetManyEmployeeExecutorComboBox from "./GetManyEmployeeExecutorComboBox.vue";
import TextCounter from "@/components/global/counter/TextCounter.vue";
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);

// Props
const props = defineProps<{
  employeeProjectStoneTaskID: number;
  employeeProjectID: number;
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
  (e: "checkBucket"): void;
}>();
// ==================== 全域狀態 ====================
const tokenStore = useTokenStore();
const projectTimeStore = useProjectTimeInfoStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage, closeError } = useErrorCodeHandler();

// ==================== 型別定義 ====================
/** 更新-工作-專案-任務-模型 */
interface UpdateWorkProjectTaskMdl {
  employeeProjectStoneID: number;
  employeeProjectStoneName: string;
  employeeProjectStoneStartTime: string;
  employeeProjectStoneEndTime: string;
  employeeProjectStoneTaskID: number;
  employeeProjectStoneTaskName: string;
  employeeProjectStoneTaskStartTime: string;
  employeeProjectStoneTaskEndTime: string;
  employeeProjectStoneTaskWorkHour: number | null;
  employeeProjectStoneTaskRemark: string | null;
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum | null;
  // 執行者
  employeeProjectStoneTaskExecutorList: {
    employeeProjectStoneTaskExecutorEmployeeID: number;
    employeeProjectStoneTaskExecutorEmployeeName: string;
  }[];
  // 子項目清單模型
  employeeProjectStoneTaskBucketList: EmployeeProjectTaskBucketMdl[];
}

/** 執行者-專案-工項-清單-模型 */
interface EmployeeProjectTaskBucketMdl {
  employeeProjectStoneTaskBucketID: number;
  employeeProjectStoneTaskBucketName: string;
  employeeProjectStoneTaskBucketIsFinish: boolean;
}

/** 更新-工作-專案-工項-驗證模型 */
interface UpdateWorkProjectTaskValidateMdl {
  employeeProjectStoneTaskRemark: boolean;
  employeeProjectStoneTaskExecutorList: boolean;
  employeeProjectStoneTaskBucketList: {
    employeeProjectStoneTaskBucketName: boolean;
  }[];
}

/** 更新-工作-專案-工項-原始資料模型 */
interface UpdateWorkProjectTaskOriginalMdl {
  employeeProjectStoneTaskRemark: string | null;
  employeeProjectStoneTaskExecutorList: {
    employeeProjectStoneTaskExecutorEmployeeID: number;
    employeeProjectStoneTaskExecutorEmployeeName: string;
  }[];
  employeeProjectStoneTaskBucketList: EmployeeProjectTaskBucketMdl[];
}
// ==================== 頁面狀態 ====================
/** 更新-工作-專案-工項-模型 */
const updateWorkProjectTaskObj = reactive<UpdateWorkProjectTaskMdl>({
  employeeProjectStoneID: 0,
  employeeProjectStoneTaskID: props.employeeProjectStoneTaskID,
  employeeProjectStoneName: "",
  employeeProjectStoneStartTime: "",
  employeeProjectStoneEndTime: "",
  employeeProjectStoneTaskName: "",
  employeeProjectStoneTaskStartTime: "",
  employeeProjectStoneTaskEndTime: "",
  employeeProjectStoneTaskWorkHour: null,
  employeeProjectStoneTaskRemark: "",
  atomEmployeeProjectStatus: null,
  employeeProjectStoneTaskExecutorList: [],
  employeeProjectStoneTaskBucketList: [],
});

/** 更新-工作-專案-工項-驗證模型 */
const updateWorkProjectTaskValidateObj = reactive<UpdateWorkProjectTaskValidateMdl>({
  employeeProjectStoneTaskRemark: true,
  employeeProjectStoneTaskExecutorList: true,
  employeeProjectStoneTaskBucketList: [],
});

/** 更新-工作-專案-工項-原始資料模型 */
const updateWorkProjectTaskOriginalObj = reactive<UpdateWorkProjectTaskOriginalMdl>({
  employeeProjectStoneTaskRemark: "",
  employeeProjectStoneTaskExecutorList: [],
  employeeProjectStoneTaskBucketList: [],
});

// ==================== 資料操作 ====================
/** 取得工項資料 */
const getStoneTaskData = async () => {
  if (!requireToken()) return;

  const requestData: MbsWrkPrjHttpGetProjectStoneTaskReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneTaskID: props.employeeProjectStoneTaskID,
  };

  const responseData: MbsWrkPrjHttpGetProjectStoneTaskRspMdl | null =
    await getProjectStoneTask(requestData);
  if (!responseData) return;

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定
  updateWorkProjectTaskObj.employeeProjectStoneID = responseData.employeeProjectStoneID;
  updateWorkProjectTaskObj.employeeProjectStoneName = responseData.employeeProjectStoneName;
  updateWorkProjectTaskObj.employeeProjectStoneStartTime = formatDate(
    responseData.employeeProjectStoneStartTime
  );
  updateWorkProjectTaskObj.employeeProjectStoneEndTime = formatDate(
    responseData.employeeProjectStoneEndTime
  );
  updateWorkProjectTaskObj.employeeProjectStoneTaskName = responseData.employeeProjectStoneTaskName;
  updateWorkProjectTaskObj.employeeProjectStoneTaskStartTime = formatDate(
    responseData.employeeProjectStoneTaskStartTime
  );
  updateWorkProjectTaskObj.employeeProjectStoneTaskEndTime = formatDate(
    responseData.employeeProjectStoneTaskEndTime
  );
  updateWorkProjectTaskObj.employeeProjectStoneTaskWorkHour =
    responseData.employeeProjectStoneTaskWorkHour;
  updateWorkProjectTaskObj.employeeProjectStoneTaskRemark =
    responseData.employeeProjectStoneTaskRemark;
  updateWorkProjectTaskObj.atomEmployeeProjectStatus = responseData.atomEmployeeProjectStatus;
  // 執行者
  updateWorkProjectTaskObj.employeeProjectStoneTaskExecutorList =
    responseData.employeeProjectStoneTaskExecutorList.map((e) => ({
      employeeProjectStoneTaskExecutorEmployeeID: e.employeeProjectStoneTaskExecutorEmployeeID,
      employeeProjectStoneTaskExecutorEmployeeName: e.employeeProjectStoneTaskExecutorEmployeeName,
    }));

  //  子項目清單模型
  updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList =
    responseData.employeeProjectStoneTaskBucketList;

  // ===== 設定原始資料，用於後續比較 =====
  updateWorkProjectTaskOriginalObj.employeeProjectStoneTaskRemark =
    responseData.employeeProjectStoneTaskRemark ?? "";

  updateWorkProjectTaskOriginalObj.employeeProjectStoneTaskExecutorList =
    responseData.employeeProjectStoneTaskExecutorList.map((e) => ({
      employeeProjectStoneTaskExecutorEmployeeID: e.employeeProjectStoneTaskExecutorEmployeeID,
      employeeProjectStoneTaskExecutorEmployeeName: e.employeeProjectStoneTaskExecutorEmployeeName,
    }));

  updateWorkProjectTaskOriginalObj.employeeProjectStoneTaskBucketList =
    responseData.employeeProjectStoneTaskBucketList.map((b) => ({
      employeeProjectStoneTaskBucketID: b.employeeProjectStoneTaskBucketID,
      employeeProjectStoneTaskBucketName: b.employeeProjectStoneTaskBucketName,
      employeeProjectStoneTaskBucketIsFinish: b.employeeProjectStoneTaskBucketIsFinish,
    }));
};

// ==================== 表單驗證 ====================
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // =============== 備註（可輸入但不能超過 1000 字） ===============
  updateWorkProjectTaskValidateObj.employeeProjectStoneTaskRemark =
    (updateWorkProjectTaskObj.employeeProjectStoneTaskRemark ?? "").length <= 1000;

  if (!updateWorkProjectTaskValidateObj.employeeProjectStoneTaskRemark) {
    isValid = false;
  }

  // =============== 執行者（至少 1 人） ===============
  const executorValid = updateWorkProjectTaskObj.employeeProjectStoneTaskExecutorList.length > 0;

  updateWorkProjectTaskValidateObj.employeeProjectStoneTaskExecutorList = executorValid;
  if (!executorValid) isValid = false;

  // =============== 子項目清單驗證 ===============
  updateWorkProjectTaskValidateObj.employeeProjectStoneTaskBucketList =
    updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList.map((b) => ({
      employeeProjectStoneTaskBucketName: !!b.employeeProjectStoneTaskBucketName,
    }));

  // =============== 子項目至少一項 ===============
  if (updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList.length === 0) {
    showError.value = true;
    errorMessage.value = "子項目至少需保留一項";
    isValid = false;
  }

  updateWorkProjectTaskValidateObj.employeeProjectStoneTaskBucketList.forEach((v) => {
    if (!v.employeeProjectStoneTaskBucketName) isValid = false;
  });

  return isValid;
};

/** 判斷執行者是否變更 */
const isExecutorChanged = () => {
  return (
    JSON.stringify(updateWorkProjectTaskObj.employeeProjectStoneTaskExecutorList) !==
    JSON.stringify(updateWorkProjectTaskOriginalObj.employeeProjectStoneTaskExecutorList)
  );
};

/** 判斷 Bucket 是否變更 */
const isBucketChanged = () => {
  // 若新增 bucket（ID < 0） 一定有變更
  if (
    updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList.some(
      (b) => b.employeeProjectStoneTaskBucketID < 0
    )
  )
    return true;

  return (
    JSON.stringify(updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList) !==
    JSON.stringify(updateWorkProjectTaskOriginalObj.employeeProjectStoneTaskBucketList)
  );
};

/** 判斷備註是否變更 */
const isRemarkChanged = () => {
  const oldVal = (updateWorkProjectTaskOriginalObj.employeeProjectStoneTaskRemark ?? "").trim();
  const newVal = (updateWorkProjectTaskObj.employeeProjectStoneTaskRemark ?? "").trim();
  return oldVal !== newVal;
};

/** 取得變更欄位 */
const getChangedFields = () => {
  const changed: Partial<MbsWrkPrjHttpUpdateProjectStoneTaskReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneTaskID: updateWorkProjectTaskObj.employeeProjectStoneTaskID,
  };

  // 備註
  if (isRemarkChanged()) {
    changed.employeeProjectStoneTaskRemark =
      updateWorkProjectTaskObj.employeeProjectStoneTaskRemark ?? "";
  }

  // 執行者
  if (isExecutorChanged()) {
    changed.employeeProjectStoneTaskExecutorIdList =
      updateWorkProjectTaskObj.employeeProjectStoneTaskExecutorList.map(
        (e) => e.employeeProjectStoneTaskExecutorEmployeeID
      );
  }

  // Bucket
  if (isBucketChanged()) {
    changed.employeeProjectStoneTaskBucketList =
      updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList;
  }

  return changed;
};

// ==================== 子項目相關 ====================
// 清單id暫存用
let tempBucketId = -1;
/** 新增子項目清單 */
const addBucket = () => {
  updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList.push({
    employeeProjectStoneTaskBucketID: tempBucketId,
    employeeProjectStoneTaskBucketName: "",
    employeeProjectStoneTaskBucketIsFinish: false,
  });

  // 清單驗證物件同步新增
  updateWorkProjectTaskValidateObj.employeeProjectStoneTaskBucketList.push({
    employeeProjectStoneTaskBucketName: true,
  });

  tempBucketId--;
};

/** 刪除子項目清單 */
const removeBucket = (index: number) => {
  updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList.splice(index, 1);
  // 清單驗證物件同步刪除
  updateWorkProjectTaskValidateObj.employeeProjectStoneTaskBucketList.splice(index, 1);
};

/** 勾選子項目清單 */
const toggleBucket = async (bucket: EmployeeProjectTaskBucketMdl) => {
  if (bucket.employeeProjectStoneTaskBucketID < 0) return;

  const requestData = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneTaskBucketID: bucket.employeeProjectStoneTaskBucketID,
    employeeProjectStoneTaskBucketIsFinish: bucket.employeeProjectStoneTaskBucketIsFinish,
  };

  const responseData: MbsWrkPrjHttpUpdateProjectStoneTaskBucketRspMdl | null =
    await updateProjectStoneTaskBucket(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  emit("checkBucket");
};

// ==================== 活動操作 ====================
/** 點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateField()) return;

  // 取得變更欄位
  const changedFields = getChangedFields();
  if (Object.keys(changedFields).length <= 2) {
    emit("close");
    return;
  }

  const responseData: MbsWrkPrjHttpUpdateProjectStoneTaskRspMdl | null =
    await updateProjectStoneTask(changedFields as MbsWrkPrjHttpUpdateProjectStoneTaskReqMdl);
  if (!responseData) return;

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  emit("submit");
  emit("close");
};

// ==================== 生命週期 ====================
onMounted(() => getStoneTaskData());
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[820px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">編輯工項</h2>
        <button class="rounded hover:bg-gray-200 text-gray-500 px-2" @click="$emit('close')">
          ×
        </button>
      </div>

      <!-- 內容 -->
      <div class="p-6 space-y-4 max-h-[70vh] overflow-y-auto">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
        >
          {{ errorMessage }}
        </p>

        <!-- 專案名稱 / 專案期間 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">專案名稱</label>
            <input :value="projectTimeStore.employeeProjectName" class="input-box" disabled />
          </div>
          <div class="flex-1">
            <label class="form-label">專案起訖時間</label>
            <input
              :value="`${formatDate(projectTimeStore.employeeProjectStartTime)} ~ ${formatDate(projectTimeStore.employeeProjectEndTime)}`"
              class="input-box"
              disabled
            />
          </div>
        </div>

        <!-- 里程碑 / 里程碑期間 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">里程碑 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectTaskObj.employeeProjectStoneName"
              class="input-box"
              disabled
              placeholder="請輸入里程碑"
            />
          </div>
          <div class="flex-1">
            <label class="form-label">里程碑起訖時間</label>
            <input
              :value="`${formatDate(updateWorkProjectTaskObj.employeeProjectStoneStartTime)} ~ ${formatDate(updateWorkProjectTaskObj.employeeProjectStoneEndTime)}`"
              class="input-box"
              disabled
            />
          </div>
        </div>

        <!-- 工項名稱 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">工項名稱 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectTaskObj.employeeProjectStoneTaskName"
              class="input-box"
              disabled
              placeholder="請輸入名稱"
            />
          </div>
        </div>

        <!-- 起訖時間 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">開始日期 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectTaskObj.employeeProjectStoneTaskStartTime"
              type="date"
              disabled
              class="input-box"
            />
          </div>
          <div class="flex-1">
            <label class="form-label">結束日期 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectTaskObj.employeeProjectStoneTaskEndTime"
              type="date"
              disabled
              class="input-box"
            />
          </div>
        </div>

        <div class="flex gap-4 w-full">
          <!-- 工項狀態 -->
          <div class="flex-1">
            <label class="form-label">工項狀態</label>
            <select
              v-model="updateWorkProjectTaskObj.atomEmployeeProjectStatus"
              class="input-box"
              disabled
            >
              <option :value="null">請選擇</option>
              <option :value="DbAtomEmployeeProjectStatusEnum.NotAssigned">未指派</option>
              <option :value="DbAtomEmployeeProjectStatusEnum.NotStarted">未開始</option>
              <option :value="DbAtomEmployeeProjectStatusEnum.OnSchedule">如期</option>
              <option :value="DbAtomEmployeeProjectStatusEnum.Delayed">延遲</option>
              <option :value="DbAtomEmployeeProjectStatusEnum.Completed">已完成</option>
            </select>
          </div>

          <!-- 執行者 -->
          <div class="flex-1">
            <label class="form-label">執行者 </label>
            <GetManyEmployeeExecutorComboBox
              v-model:selectedExecutorList="
                updateWorkProjectTaskObj.employeeProjectStoneTaskExecutorList
              "
              :employeeProjectID="props.employeeProjectID"
              :disabled="false"
            />
          </div>
        </div>

        <!-- 備註 -->
        <div class="">
          <label class="form-label">備註 </label>
          <textarea
            v-model="updateWorkProjectTaskObj.employeeProjectStoneTaskRemark"
            class="textarea-box"
            rows="10"
            placeholder="請輸入備註"
          ></textarea>
          <div>
            <TextCounter
              :text="updateWorkProjectTaskObj.employeeProjectStoneTaskRemark"
              :max-length="1000"
              :required="false"
            />
          </div>
        </div>

        <!-- 子項目 Bucket -->
        <div class="border p-6 rounded-lg">
          <div class="flex justify-between items-center">
            <h3 class="text-md font-semibold">子項目</h3>
            <button class="btn-add" @click="addBucket">新增子項目</button>
          </div>

          <div
            v-for="(bucket, bIndex) in updateWorkProjectTaskObj.employeeProjectStoneTaskBucketList"
            :key="bIndex"
            class="flex flex-col mt-2 bg-gray-100 py-1 px-1 rounded"
          >
            <!-- 上層（checkbox + input + X） -->
            <div class="flex items-center gap-3">
              <input
                v-model="bucket.employeeProjectStoneTaskBucketIsFinish"
                class="ml-2"
                type="checkbox"
                @change="toggleBucket(bucket)"
              />

              <input
                v-model="bucket.employeeProjectStoneTaskBucketName"
                :class="[
                  'input-box',
                  !bucket.employeeProjectStoneTaskBucketName ? 'border-red-500' : '',
                ]"
                placeholder="子項目名稱"
              />

              <button class="mr-3" @click="removeBucket(bIndex)">
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
            </div>
          </div>
        </div>
      </div>

      <!-- 底部 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="$emit('close')">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
</template>
