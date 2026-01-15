<script setup lang="ts">
import { onMounted, reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useProjectTimeInfoStore } from "@/stores/projectTimeInfo";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { getProjectStone, updateProjectStone } from "@/services";
import GetManyEmployeeExecutorComboBox, {
  type GetManyEmployeeExecutorComboBoxItemMdl,
} from "./GetManyEmployeeExecutorComboBox.vue";
import type {
  MbsWrkPrjHttpGetProjectStoneReqMdl,
  MbsWrkPrjHttpGetProjectStoneRspMdl,
  MbsWrkPrjHttpUpdateProjectStoneReqMdl,
  MbsWrkPrjHttpUpdateProjectStoneRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import {
  formatDate,
  formatToServerDateEndISO8,
  formatToServerDateStartISO8,
} from "@/utils/timeFormatter";
//-------------------------------------------------------------
const tokenStore = useTokenStore();
const projectTimeStore = useProjectTimeInfoStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();

//-------------------------------------------------------------
const props = defineProps<{
  employeeProjectStoneID: number;
  employeeProjectID: number;
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();

//-------------------------------------------------------------
/** 更新-工作-專案管理-里程碑-模型 */
interface UpdateProjectStoneMdl {
  employeeProjectStoneName: string;
  employeeProjectStonePreNotifyDay: number;
  employeeProjectStoneStartTime: string;
  employeeProjectStoneEndTime: string;
  employeeProjectStoneTaskList: UpdateWorkProjectStoneTaskItemMdl[] | null;
}

/** 編輯里程碑 - 驗證模型 */
interface UpdateProjectStoneValidateMdl {
  employeeProjectStoneName: boolean;
  employeeProjectStoneStartTime: boolean;
  employeeProjectStoneEndTime: boolean;
  // 開始日期必須小於結束日期
  employeeProjectStoneDateRange: boolean;
  // 里程碑時間不可超過專案時間
  employeeProjectStoneWithinProjectRange: boolean;
  employeeProjectStonePreNotifyDay: boolean;
  // 結束日期減N天要大於等於開始日期
  employeeProjectStoneEndMinusNDaysValid: boolean;
  employeeProjectStoneTaskList: {
    employeeProjectStoneTaskName: boolean;
    employeeProjectStoneTaskStartTime: boolean;
    employeeProjectStoneTaskEndTime: boolean;
    employeeProjectStoneTaskStartRange: boolean; // 工項開始 >= 里程碑開始
    employeeProjectStoneTaskEndRange: boolean; // 工項結束 <= 里程碑結束 - N 日
    employeeProjectStoneTaskDateOrder: boolean; // start < end
    employeeProjectStoneTaskWorkHour: boolean;
    employeeProjectStoneTaskExecutorList: boolean;
  }[];
}

/** 更新-工作-專案管理-工項-項目模型 */
interface UpdateWorkProjectStoneTaskItemMdl {
  employeeProjectStoneTaskID: number;
  employeeProjectStoneTaskName: string;
  employeeProjectStoneTaskStartTime: string;
  employeeProjectStoneTaskEndTime: string;
  employeeProjectStoneTaskWorkHour: number | null;
  employeeProjectStoneTaskExecutorList: GetManyEmployeeExecutorComboBoxItemMdl[];
}
//-------------------------------------------------------------
/** 更新-工作-專案管理-里程碑-物件 */
const updateWorkProjectStoneObj = reactive<UpdateProjectStoneMdl>({
  employeeProjectStoneName: "",
  employeeProjectStonePreNotifyDay: 0,
  employeeProjectStoneStartTime: "",
  employeeProjectStoneEndTime: "",
  employeeProjectStoneTaskList: [],
});

/** 更新-工作-專案管理-里程碑-驗證物件 */
const updateProjectStoneValidateObj = reactive<UpdateProjectStoneValidateMdl>({
  employeeProjectStoneName: true,
  employeeProjectStoneStartTime: true,
  employeeProjectStoneEndTime: true,
  employeeProjectStoneDateRange: true,
  employeeProjectStoneWithinProjectRange: true,
  employeeProjectStonePreNotifyDay: true,
  employeeProjectStoneEndMinusNDaysValid: true,
  employeeProjectStoneTaskList: [],
});

/** 更新-工作-專案管理-里程碑-原始模型 */
const UpdateProjectStoneOriginalObj = reactive<UpdateProjectStoneMdl>({
  employeeProjectStoneName: "",
  employeeProjectStonePreNotifyDay: 0,
  employeeProjectStoneStartTime: "",
  employeeProjectStoneEndTime: "",
  employeeProjectStoneTaskList: null,
});
//-------------------------------------------------------------
/** 工項id(送給後端新增的工項 -1 -2....依此類推新增) */
let tempTaskId = -1;
/** 新增工項 */
const addTask = () => {
  if (!updateWorkProjectStoneObj.employeeProjectStoneTaskList) {
    updateWorkProjectStoneObj.employeeProjectStoneTaskList = [];
  }
  updateWorkProjectStoneObj.employeeProjectStoneTaskList.push({
    employeeProjectStoneTaskID: tempTaskId,
    employeeProjectStoneTaskName: "",
    employeeProjectStoneTaskStartTime: "",
    employeeProjectStoneTaskEndTime: "",
    employeeProjectStoneTaskWorkHour: null,
    employeeProjectStoneTaskExecutorList: [],
  });

  // 同步新增一筆驗證物件
  updateProjectStoneValidateObj.employeeProjectStoneTaskList.push({
    employeeProjectStoneTaskName: true,
    employeeProjectStoneTaskStartTime: true,
    employeeProjectStoneTaskEndTime: true,
    employeeProjectStoneTaskStartRange: true,
    employeeProjectStoneTaskEndRange: true,
    employeeProjectStoneTaskDateOrder: true,
    employeeProjectStoneTaskWorkHour: true,
    employeeProjectStoneTaskExecutorList: true,
  });

  tempTaskId--;
};

/** 刪除任務 */
const removeTask = (index: number) => {
  if (!updateWorkProjectStoneObj.employeeProjectStoneTaskList) {
    updateWorkProjectStoneObj.employeeProjectStoneTaskList = [];
  }
  updateWorkProjectStoneObj.employeeProjectStoneTaskList.splice(index, 1);
};
//-------------------------------------------------------------
/** 取得【里程碑】資料 */
const getProjectStoneData = async () => {
  if (!requireToken()) return;

  const requestData: MbsWrkPrjHttpGetProjectStoneReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneID: props.employeeProjectStoneID,
  };

  const responseData: MbsWrkPrjHttpGetProjectStoneRspMdl | null =
    await getProjectStone(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定里程碑基本資料
  updateWorkProjectStoneObj.employeeProjectStoneName = responseData.employeeProjectStoneName;
  updateWorkProjectStoneObj.employeeProjectStoneStartTime = formatDate(
    responseData.employeeProjectStoneStartTime
  );
  updateWorkProjectStoneObj.employeeProjectStoneEndTime = formatDate(
    responseData.employeeProjectStoneEndTime
  );
  updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay =
    responseData.employeeProjectStonePreNotifyDay;
  // 設定工項列表
  updateWorkProjectStoneObj.employeeProjectStoneTaskList =
    responseData.employeeProjectStoneTaskList.map((task) => ({
      employeeProjectStoneTaskID: task.employeeProjectStoneTaskID,
      employeeProjectStoneTaskName: task.employeeProjectStoneTaskName,
      employeeProjectStoneTaskStartTime: formatDate(task.employeeProjectStoneTaskStartTime),
      employeeProjectStoneTaskEndTime: formatDate(task.employeeProjectStoneTaskEndTime),
      employeeProjectStoneTaskWorkHour: task.employeeProjectStoneTaskWorkHour ?? null,
      // 設定工項的執行者列表
      employeeProjectStoneTaskExecutorList: task.employeeProjectStoneTaskExecutorList.map(
        (executor) => ({
          employeeProjectStoneTaskExecutorEmployeeID:
            executor.employeeProjectStoneTaskExecutorEmployeeID,
          employeeProjectStoneTaskExecutorEmployeeName:
            executor.employeeProjectStoneTaskExecutorEmployeeName,
        })
      ),
    }));

  //保存原始資料（深拷貝）
  UpdateProjectStoneOriginalObj.employeeProjectStoneName = responseData.employeeProjectStoneName;
  UpdateProjectStoneOriginalObj.employeeProjectStoneStartTime = formatDate(
    responseData.employeeProjectStoneStartTime
  );
  UpdateProjectStoneOriginalObj.employeeProjectStoneEndTime = formatDate(
    responseData.employeeProjectStoneEndTime
  );
  UpdateProjectStoneOriginalObj.employeeProjectStonePreNotifyDay =
    responseData.employeeProjectStonePreNotifyDay;

  UpdateProjectStoneOriginalObj.employeeProjectStoneTaskList =
    responseData.employeeProjectStoneTaskList.map((task) => ({
      employeeProjectStoneTaskID: task.employeeProjectStoneTaskID,
      employeeProjectStoneTaskName: task.employeeProjectStoneTaskName,
      employeeProjectStoneTaskStartTime: formatDate(task.employeeProjectStoneTaskStartTime),
      employeeProjectStoneTaskEndTime: formatDate(task.employeeProjectStoneTaskEndTime),
      employeeProjectStoneTaskWorkHour: task.employeeProjectStoneTaskWorkHour ?? null,
      employeeProjectStoneTaskExecutorList: task.employeeProjectStoneTaskExecutorList.map(
        (exe) => ({
          employeeProjectStoneTaskExecutorEmployeeID:
            exe.employeeProjectStoneTaskExecutorEmployeeID,
          employeeProjectStoneTaskExecutorEmployeeName:
            exe.employeeProjectStoneTaskExecutorEmployeeName,
        })
      ),
    }));
  // 初始化驗證物件
  updateProjectStoneValidateObj.employeeProjectStoneTaskList =
    updateWorkProjectStoneObj.employeeProjectStoneTaskList.map(() => ({
      employeeProjectStoneTaskName: true,
      employeeProjectStoneTaskStartTime: true,
      employeeProjectStoneTaskEndTime: true,
      employeeProjectStoneTaskStartRange: true,
      employeeProjectStoneTaskEndRange: true,
      employeeProjectStoneTaskDateOrder: true,
      employeeProjectStoneWithinProjectRange: true,
      employeeProjectStoneTaskWorkHour: true,
      employeeProjectStoneTaskExecutorList: true,
    }));
};
/** 取得變更的欄位 */
const getChangedFields = () => {
  const changes: Partial<MbsWrkPrjHttpUpdateProjectStoneReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneID: props.employeeProjectStoneID,
  };

  // ======================
  // 1. 比較基本欄位
  // ======================

  if (
    updateWorkProjectStoneObj.employeeProjectStoneName !==
    UpdateProjectStoneOriginalObj.employeeProjectStoneName
  )
    changes.employeeProjectStoneName = updateWorkProjectStoneObj.employeeProjectStoneName;

  if (
    updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay !==
    UpdateProjectStoneOriginalObj.employeeProjectStonePreNotifyDay
  )
    changes.employeeProjectStonePreNotifyDay =
      updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay;

  if (
    updateWorkProjectStoneObj.employeeProjectStoneStartTime !==
    UpdateProjectStoneOriginalObj.employeeProjectStoneStartTime
  )
    changes.employeeProjectStoneStartTime = formatToServerDateStartISO8(
      updateWorkProjectStoneObj.employeeProjectStoneStartTime
    );

  if (
    updateWorkProjectStoneObj.employeeProjectStoneEndTime !==
    UpdateProjectStoneOriginalObj.employeeProjectStoneEndTime
  )
    changes.employeeProjectStoneEndTime = formatToServerDateEndISO8(
      updateWorkProjectStoneObj.employeeProjectStoneEndTime
    );

  // ======================
  // 2. 比較工項 TaskList
  // ======================

  const taskChanged = isTaskListChanged();

  if (taskChanged) {
    // 只要有一筆變更 → 全部送出
    changes.employeeProjectStoneTaskList = buildFullTaskList();
  }

  return changes;
};

//-------------------------------------------------------------
/** 判斷單一工項是否有變更 */
const isTaskChanged = (
  task: UpdateWorkProjectStoneTaskItemMdl,
  originalTask: UpdateWorkProjectStoneTaskItemMdl
) => {
  return (
    task.employeeProjectStoneTaskName !== originalTask.employeeProjectStoneTaskName ||
    task.employeeProjectStoneTaskStartTime !== originalTask.employeeProjectStoneTaskStartTime ||
    task.employeeProjectStoneTaskEndTime !== originalTask.employeeProjectStoneTaskEndTime ||
    task.employeeProjectStoneTaskWorkHour !== originalTask.employeeProjectStoneTaskWorkHour ||
    JSON.stringify(task.employeeProjectStoneTaskExecutorList) !==
      JSON.stringify(originalTask.employeeProjectStoneTaskExecutorList)
  );
};

/** 判斷工項列表是否有變更 */
const isTaskListChanged = () => {
  const currentList = updateWorkProjectStoneObj.employeeProjectStoneTaskList ?? [];
  const originalList = UpdateProjectStoneOriginalObj.employeeProjectStoneTaskList ?? [];

  // 新增工項
  if (currentList.some((t) => t.employeeProjectStoneTaskID < 0)) {
    return true;
  }

  // 刪除工項（原本有，現在沒了）
  const hasDeletedTask = originalList.some(
    (o) => !currentList.some((c) => c.employeeProjectStoneTaskID === o.employeeProjectStoneTaskID)
  );
  if (hasDeletedTask) {
    return true;
  }

  // 修改工項
  return currentList.some((task) => {
    const originalTask = originalList.find(
      (o) => o.employeeProjectStoneTaskID === task.employeeProjectStoneTaskID
    );
    if (!originalTask) return true;
    return isTaskChanged(task, originalTask);
  });
};

/** 建立完整工項列表 */
const buildFullTaskList = () => {
  if (!updateWorkProjectStoneObj.employeeProjectStoneTaskList) {
    updateWorkProjectStoneObj.employeeProjectStoneTaskList = [];
  }
  return updateWorkProjectStoneObj.employeeProjectStoneTaskList.map((task) => ({
    employeeProjectStoneTaskID: task.employeeProjectStoneTaskID,
    employeeProjectStoneTaskName: task.employeeProjectStoneTaskName,
    employeeProjectStoneTaskStartTime: formatToServerDateStartISO8(
      task.employeeProjectStoneTaskStartTime
    ),
    employeeProjectStoneTaskEndTime: formatToServerDateEndISO8(
      task.employeeProjectStoneTaskEndTime
    ),
    employeeProjectStoneTaskWorkHour: task.employeeProjectStoneTaskWorkHour ?? 0,
    employeeProjectStoneTaskExecutorList: task.employeeProjectStoneTaskExecutorList.map((exe) => ({
      employeeID: exe.employeeProjectStoneTaskExecutorEmployeeID,
    })),
  }));
};
//-------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  if (!updateWorkProjectStoneObj.employeeProjectStoneTaskList) {
    updateWorkProjectStoneObj.employeeProjectStoneTaskList = [];
  }
  // 重新建立工項驗證陣列
  updateProjectStoneValidateObj.employeeProjectStoneTaskList =
    updateWorkProjectStoneObj.employeeProjectStoneTaskList.map(() => ({
      employeeProjectStoneTaskName: true,
      employeeProjectStoneTaskStartTime: true,
      employeeProjectStoneTaskEndTime: true,
      employeeProjectStoneTaskStartRange: true,
      employeeProjectStoneTaskEndRange: true,
      employeeProjectStoneTaskDateOrder: true,
      employeeProjectStoneWithinProjectRange: true,
      employeeProjectStoneTaskWorkHour: true,
      employeeProjectStoneTaskExecutorList: true,
    }));

  // ========= 里程碑欄位驗證 =========

  // 名稱（必填 + 50字）
  const nameValid =
    !!updateWorkProjectStoneObj.employeeProjectStoneName &&
    updateWorkProjectStoneObj.employeeProjectStoneName.length <= 50;
  updateProjectStoneValidateObj.employeeProjectStoneName = nameValid;
  if (!nameValid) isValid = false;

  // 開始日期
  const startValid = !!updateWorkProjectStoneObj.employeeProjectStoneStartTime;
  updateProjectStoneValidateObj.employeeProjectStoneStartTime = startValid;
  if (!startValid) isValid = false;

  // 結束日期
  const endValid = !!updateWorkProjectStoneObj.employeeProjectStoneEndTime;
  updateProjectStoneValidateObj.employeeProjectStoneEndTime = endValid;
  if (!endValid) isValid = false;

  // 前N日通知（必填，且需 >= 0）
  const nDayValid =
    updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay !== null &&
    typeof updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay === "number" &&
    updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay >= 0;
  updateProjectStoneValidateObj.employeeProjectStonePreNotifyDay = nDayValid;
  if (!nDayValid) isValid = false;

  // 結束日期減N天要大於等於開始日期
  let endMinusNDaysValid = true;
  if (
    updateWorkProjectStoneObj.employeeProjectStoneStartTime &&
    updateWorkProjectStoneObj.employeeProjectStoneEndTime &&
    updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay !== null
  ) {
    const stoneStart = new Date(updateWorkProjectStoneObj.employeeProjectStoneStartTime);
    const stoneEnd = new Date(updateWorkProjectStoneObj.employeeProjectStoneEndTime);
    const endMinusNDays = new Date(stoneEnd);
    endMinusNDays.setDate(endMinusNDays.getDate() - updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay);
    
    endMinusNDaysValid = endMinusNDays >= stoneStart;
  } else {
    endMinusNDaysValid = false;
  }

  updateProjectStoneValidateObj.employeeProjectStoneEndMinusNDaysValid = endMinusNDaysValid;

  if (!endMinusNDaysValid) isValid = false;

  // ========= 工項驗證 =========
  if (!updateWorkProjectStoneObj.employeeProjectStoneTaskList) {
    updateWorkProjectStoneObj.employeeProjectStoneTaskList = [];
  }

  updateWorkProjectStoneObj.employeeProjectStoneTaskList.forEach((task, index) => {
    // 工項名稱
    const taskNameValid =
      !!task.employeeProjectStoneTaskName && task.employeeProjectStoneTaskName.length <= 50;

    updateProjectStoneValidateObj.employeeProjectStoneTaskList[index].employeeProjectStoneTaskName =
      taskNameValid;
    if (!taskNameValid) isValid = false;

    // 工項開始日期
    const taskStartValid = !!task.employeeProjectStoneTaskStartTime;
    updateProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskStartTime = taskStartValid;
    if (!taskStartValid) isValid = false;

    // 工項結束日期
    const taskEndValid = !!task.employeeProjectStoneTaskEndTime;
    updateProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskEndTime = taskEndValid;
    if (!taskEndValid) isValid = false;

    // ========= 日期範圍驗證 =========
    const stoneStart = new Date(updateWorkProjectStoneObj.employeeProjectStoneStartTime);
    const stoneEnd = new Date(updateWorkProjectStoneObj.employeeProjectStoneEndTime);
    const preNotifyDay = updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay ?? 0;

    const latestAllowedTaskEnd = new Date(stoneEnd);
    latestAllowedTaskEnd.setDate(latestAllowedTaskEnd.getDate() - preNotifyDay);

    const taskStart = new Date(task.employeeProjectStoneTaskStartTime);
    const taskEnd = new Date(task.employeeProjectStoneTaskEndTime);

    const startRangeValid = taskStart >= stoneStart;
    const endRangeValid = taskEnd <= latestAllowedTaskEnd;
    const dateOrderValid = taskStart <= taskEnd;

    const validateItem = updateProjectStoneValidateObj.employeeProjectStoneTaskList[index];

    validateItem.employeeProjectStoneTaskStartRange = startRangeValid;
    validateItem.employeeProjectStoneTaskEndRange = endRangeValid;
    validateItem.employeeProjectStoneTaskDateOrder = dateOrderValid;

    if (!startRangeValid || !endRangeValid || !dateOrderValid) {
      isValid = false;
    }

    // ========= 時數驗證 =========
    const taskHourValid =
      task.employeeProjectStoneTaskWorkHour !== null && task.employeeProjectStoneTaskWorkHour > 0;

    updateProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskWorkHour = taskHourValid;

    if (!taskHourValid) isValid = false;
  });

  return isValid;
};

/** 點擊送出按鈕 */
const clickSubmitBtn = async () => {
  // 驗證token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateField()) {
    return;
  }

  const changedFields = getChangedFields();

  // 移除固定欄位
  const pureChangedFields = { ...changedFields };
  delete pureChangedFields.employeeLoginToken;
  delete pureChangedFields.employeeProjectStoneID;

  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    showError.value = true;
    errorMessage.value = "沒有任何變更需要儲存！";
    return;
  }

  const responseData: MbsWrkPrjHttpUpdateProjectStoneRspMdl | null = await updateProjectStone(
    changedFields as MbsWrkPrjHttpUpdateProjectStoneReqMdl
  );

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
//-------------------------------------------------------------
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};
//-------------------------------------------------------------
onMounted(() => {
  getProjectStoneData();
});
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[920px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">編輯里程碑</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <!-- 內容 -->
      <div class="p-8 space-y-4 max-h-[70vh] overflow-y-auto">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
        >
          {{ errorMessage }}
        </p>

        <!-- 專案名稱 / 專案起訖時間 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">專案名稱</label>
            <input
              :value="projectTimeStore.employeeProjectName"
              class="input-box"
              disabled
            />
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

        <!-- 里程碑名稱 -->
        <div class="flex-1">
          <label class="form-label">里程碑名稱 <span class="required-label">*</span></label>
          <input
            v-model="updateWorkProjectStoneObj.employeeProjectStoneName"
            class="input-box"
            placeholder="請輸入名稱"
            @input="resetError"
          />
          <div class="error-wrapper">
            <span v-if="!updateProjectStoneValidateObj.employeeProjectStoneName" class="error-tip">
              不可為空，長度不可超過50個字
            </span>
          </div>
        </div>

        <!-- 起訖時間 -->
        <div class="flex gap-4 w-full">
          <div class="flex-1">
            <label class="form-label">開始日期 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectStoneObj.employeeProjectStoneStartTime"
              type="date"
              class="input-box"
            />
            <div class="error-wrapper">
              <span
                v-if="!updateProjectStoneValidateObj.employeeProjectStoneStartTime"
                class="error-tip"
              >
                不可為空
              </span>
            </div>
          </div>
          <div class="flex-1">
            <label class="form-label">結束日期 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectStoneObj.employeeProjectStoneEndTime"
              type="date"
              class="input-box"
            />
            <div class="error-wrapper">
              <span
                v-if="!updateProjectStoneValidateObj.employeeProjectStoneEndTime"
                class="error-tip"
              >
                不可為空
              </span>
            </div>
          </div>

          <!--前 N 日通知-->
          <div>
            <label class="form-label">前 N 日通知 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkProjectStoneObj.employeeProjectStonePreNotifyDay"
              type="number"
              class="input-box"
              placeholder="請輸入天數"
            />
            <div class="error-wrapper">
              <span
                v-if="!updateProjectStoneValidateObj.employeeProjectStonePreNotifyDay"
                class="error-tip"
              >
                不可為空，且需要為大於等於 0 的數字
              </span>
              <span
                v-if="!updateProjectStoneValidateObj.employeeProjectStoneEndMinusNDaysValid"
                class="error-tip"
              >
                結束日期減去前 N 日的時間必須大於等於開始日期
              </span>
            </div>
          </div>
        </div>

        <!-- 任務列表 -->
        <div class="flex flex-col border p-6 rounded-lg space-y-4">
          <div class="flex flex-row justify-between items-center">
            <h3 class="text-md font-semibold">工項列表</h3>
            <button class="btn-add" @click="addTask">新增工項</button>
          </div>

          <div
            v-for="(task, tIndex) in updateWorkProjectStoneObj.employeeProjectStoneTaskList"
            :key="tIndex"
            class="space-y-3 rounded-md bg-gray-100 border"
          >
            <!-- 任務標題 -->
            <div class="flex justify-between items-center border-b bg-gray-100 px-4 py-2">
              <h4 class="font-semibold text-gray-700">工項 {{ tIndex + 1 }}</h4>
              <button class="btn-delete" @click="removeTask(tIndex)">刪除</button>
            </div>

            <div class="p-4 space-y-4">
              <!-- 工項名稱 + 執行者 -->
              <div class="grid grid-cols-2 gap-2 w-full">
                <div class="flex-1">
                  <label class="form-label">工項名稱 <span class="required-label">*</span></label>
                  <input
                    v-model="task.employeeProjectStoneTaskName"
                    class="input-box"
                    placeholder="請輸入工項名稱"
                  />
                  <div class="error-wrapper">
                    <p
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskName
                      "
                      class="error-tip"
                    >
                      不可為空
                    </p>
                  </div>
                </div>

                <!-- 執行者列表 -->
                <div class="flex-1">
                  <label class="form-label">執行者</label>
                  <div class="flex items-center gap-3">
                    <GetManyEmployeeExecutorComboBox
                      v-model:selectedExecutorList="task.employeeProjectStoneTaskExecutorList"
                      :employeeProjectID="props.employeeProjectID"
                      :disabled="false"
                    />
                  </div>
                  <!-- <div class="error-wrapper">
                    <p
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskExecutorList
                      "
                      class="error-tip"
                    >
                      請至少選擇一位執行者
                    </p>
                  </div> -->
                </div>
              </div>

              <div class="flex gap-2 w-full">
                <!-- 開始時間 -->
                <div class="flex-1">
                  <label class="form-label">開始日期 <span class="required-label">*</span></label>
                  <input
                    v-model="task.employeeProjectStoneTaskStartTime"
                    type="date"
                    class="input-box"
                  />
                  <div>
                    <span
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskStartTime
                      "
                      class="error-tip"
                    >
                      不可為空
                    </span>

                    <span
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskStartRange
                      "
                      class="error-tip"
                    >
                      工項開始不可早於里程碑開始
                    </span>

                    <span
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskDateOrder
                      "
                      class="error-tip"
                    >
                    工項開始日期不可晚於結束日期
                    </span>
                  </div>
                </div>

                <!-- 結束時間 -->
                <div class="flex-1">
                  <label class="form-label">結束日期 <span class="required-label">*</span></label>
                  <input
                    v-model="task.employeeProjectStoneTaskEndTime"
                    type="date"
                    class="input-box"
                  />
                  <div>
                    <span
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskEndTime
                      "
                      class="error-tip"
                    >
                      不可為空
                    </span>
                    <span
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskEndRange
                      "
                      class="error-tip"
                    >
                      工項結束不可晚於里程碑結束前 N 日
                    </span>

                    <span
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskDateOrder
                      "
                      class="error-tip"
                    >
                      工項結束需晚於開始日期
                    </span>
                  </div>
                </div>

                <!-- 時數/執行者 -->
                <div class="w-32">
                  <label class="form-label">時數 <span class="required-label">*</span></label>
                  <input
                    v-model="task.employeeProjectStoneTaskWorkHour"
                    type="number"
                    placeholder="請輸入時數"
                    class="input-box"
                  />
                  <div class="error-wrapper">
                    <p
                      v-if="
                        !updateProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskWorkHour
                      "
                      class="error-tip"
                    >
                      不可為空
                    </p>
                  </div>
                </div>
              </div>
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
</template>
