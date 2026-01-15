<script setup lang="ts">
import { reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useProjectTimeInfoStore } from "@/stores/projectTimeInfo";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { addProjectStone } from "@/services";
import GetManyEmployeeExecutorComboBox, {
  type GetManyEmployeeExecutorComboBoxItemMdl,
} from "./GetManyEmployeeExecutorComboBox.vue";

import type {
  MbsWrkPrjHttpAddProjectStoneReqMdl,
  MbsWrkPrjHttpAddProjectStoneRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import {
  formatToServerDateEndISO8,
  formatToServerDateStartISO8,
  formatDate,
} from "@/utils/timeFormatter";

//-------------------------------------------------------------
const tokenStore = useTokenStore();
const projectTimeStore = useProjectTimeInfoStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();
//-------------------------------------------------------------
const props = defineProps<{
  employeeProjectID: number;
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//-------------------------------------------------------------
/** 新增-工作-專案管理-里程碑-模型 */
interface AddWorkProjectStoneMdl {
  employeeProjectStoneName: string;
  employeeProjectStonePreNotifyDay: number | null;
  employeeProjectStoneStartTime: string;
  employeeProjectStoneEndTime: string;
  employeeProjectStoneTaskList: AddWorkProjectStoneTaskItemMdl[];
}

/** 新增-工作-專案管理-里程碑-驗證模型 */
interface AddWorkProjectStoneValidateMdl {
  employeeProjectStoneName: boolean;
  employeeProjectStoneStartTime: boolean;
  employeeProjectStoneEndTime: boolean;
  // 開始日期必須小於結束日期
  employeeProjectStoneDateRange: boolean;
  // 里程碑時間不可超過專案時間
  employeeProjectStoneWithinProjectRange: boolean;
  employeeProjectStonePreNotifyDay: boolean;
  // 結束日期減N天要大於開始日期
  employeeProjectStoneEndMinusNDaysValid: boolean;
  employeeProjectStoneTaskList: {
    employeeProjectStoneTaskName: boolean;
    employeeProjectStoneTaskStartTime: boolean;
    employeeProjectStoneTaskEndTime: boolean;
    employeeProjectStoneTaskStartRange: boolean; // 開始是否合法
    employeeProjectStoneTaskEndRange: boolean; // 結束是否合法
    employeeProjectStoneTaskDateOrder: boolean; // start < end
    employeeProjectStoneTaskWorkHour: boolean;
    employeeProjectStoneTaskExecutorList: boolean;
  }[];
}

/** 新增-工作-專案管理-工項-項目模型 */
interface AddWorkProjectStoneTaskItemMdl {
  employeeProjectStoneTaskName: string;
  employeeProjectStoneTaskStartTime: string;
  employeeProjectStoneTaskEndTime: string;
  employeeProjectStoneTaskWorkHour: number | null;
  employeeProjectStoneTaskRemark: string;
  employeeProjectStoneTaskExecutorList: GetManyEmployeeExecutorComboBoxItemMdl[];
}
//-------------------------------------------------------------
/** 新增-工作-專案管理-里程碑-物件 */
const addWorkProjectStoneObj = reactive<AddWorkProjectStoneMdl>({
  employeeProjectStoneName: "",
  employeeProjectStonePreNotifyDay: null,
  employeeProjectStoneStartTime: "",
  employeeProjectStoneEndTime: "",
  employeeProjectStoneTaskList: [
    {
      employeeProjectStoneTaskName: "",
      employeeProjectStoneTaskStartTime: "",
      employeeProjectStoneTaskEndTime: "",
      employeeProjectStoneTaskWorkHour: null,
      employeeProjectStoneTaskRemark: "",
      employeeProjectStoneTaskExecutorList: [],
    },
  ],
});

/** 新增-工作-專案管理-里程碑-驗證物件 */
const addWorkProjectStoneValidateObj = reactive<AddWorkProjectStoneValidateMdl>({
  employeeProjectStoneName: true,
  employeeProjectStoneStartTime: true,
  employeeProjectStoneEndTime: true,
  employeeProjectStoneDateRange: true,
  employeeProjectStoneWithinProjectRange: true,
  employeeProjectStonePreNotifyDay: true,
  employeeProjectStoneEndMinusNDaysValid: true,
  employeeProjectStoneTaskList: [
    {
      employeeProjectStoneTaskName: true,
      employeeProjectStoneTaskStartTime: true,
      employeeProjectStoneTaskStartRange: true,
      employeeProjectStoneTaskEndRange: true,
      employeeProjectStoneTaskDateOrder: true,

      employeeProjectStoneTaskEndTime: true,
      employeeProjectStoneTaskWorkHour: true,
      employeeProjectStoneTaskExecutorList: true,
    },
  ],
});
//-------------------------------------------------------------
/** 新增工項 */
const addTask = () => {
  addWorkProjectStoneObj.employeeProjectStoneTaskList.push({
    employeeProjectStoneTaskName: "",
    employeeProjectStoneTaskStartTime: "",
    employeeProjectStoneTaskEndTime: "",
    employeeProjectStoneTaskWorkHour: null,
    employeeProjectStoneTaskRemark: "",
    employeeProjectStoneTaskExecutorList: [],
  });

  // 同步新增一筆驗證物件
  addWorkProjectStoneValidateObj.employeeProjectStoneTaskList.push({
    employeeProjectStoneTaskName: true,
    employeeProjectStoneTaskStartTime: true,
    employeeProjectStoneTaskEndTime: true,
    employeeProjectStoneTaskStartRange: true,
    employeeProjectStoneTaskEndRange: true,
    employeeProjectStoneTaskDateOrder: true,

    employeeProjectStoneTaskWorkHour: true,
    employeeProjectStoneTaskExecutorList: true,
  });
};

/** 刪除任務 */
const removeTask = (index: number) => {
  addWorkProjectStoneObj.employeeProjectStoneTaskList.splice(index, 1);
};
//-------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 重新建立工項驗證陣列
  addWorkProjectStoneValidateObj.employeeProjectStoneTaskList =
    addWorkProjectStoneObj.employeeProjectStoneTaskList.map(() => ({
      employeeProjectStoneTaskName: true,
      employeeProjectStoneTaskStartTime: true,
      employeeProjectStoneTaskEndTime: true,
      employeeProjectStoneTaskStartRange: true,
      employeeProjectStoneTaskEndRange: true,
      employeeProjectStoneTaskDateOrder: true,

      employeeProjectStoneTaskWorkHour: true,
      employeeProjectStoneTaskExecutorList: true,
    }));

  // ========= 里程碑欄位驗證 =========

  // 名稱（必填 + 50字）
  const nameValid =
    !!addWorkProjectStoneObj.employeeProjectStoneName &&
    addWorkProjectStoneObj.employeeProjectStoneName.length <= 50;
  addWorkProjectStoneValidateObj.employeeProjectStoneName = nameValid;
  if (!nameValid) isValid = false;

  // 開始日期
  const startValid = !!addWorkProjectStoneObj.employeeProjectStoneStartTime;
  addWorkProjectStoneValidateObj.employeeProjectStoneStartTime = startValid;
  if (!startValid) isValid = false;

  // 開始日期必須小於結束日期
  let dateRangeValid = true;
  if (
    addWorkProjectStoneObj.employeeProjectStoneStartTime &&
    addWorkProjectStoneObj.employeeProjectStoneEndTime
  ) {
    dateRangeValid =
      new Date(addWorkProjectStoneObj.employeeProjectStoneStartTime) <
      new Date(addWorkProjectStoneObj.employeeProjectStoneEndTime);
  } else {
    dateRangeValid = false;
  }
  addWorkProjectStoneValidateObj.employeeProjectStoneDateRange = dateRangeValid;
  if (!dateRangeValid) isValid = false;

  // 結束日期
  const endValid = !!addWorkProjectStoneObj.employeeProjectStoneEndTime;
  addWorkProjectStoneValidateObj.employeeProjectStoneEndTime = endValid;
  if (!endValid) isValid = false;

  // 里程碑時間不可超過專案時間
  let withinProjectRangeValid = true;

  const projectStart = new Date(projectTimeStore.employeeProjectStartTime);
  const projectEnd = new Date(projectTimeStore.employeeProjectEndTime);

  if (
    addWorkProjectStoneObj.employeeProjectStoneStartTime &&
    addWorkProjectStoneObj.employeeProjectStoneEndTime
  ) {
    const stoneStart = new Date(addWorkProjectStoneObj.employeeProjectStoneStartTime);
    const stoneEnd = new Date(addWorkProjectStoneObj.employeeProjectStoneEndTime);

    withinProjectRangeValid = stoneStart >= projectStart && stoneEnd <= projectEnd;
  } else {
    withinProjectRangeValid = false;
  }

  addWorkProjectStoneValidateObj.employeeProjectStoneWithinProjectRange = withinProjectRangeValid;

  if (!withinProjectRangeValid) isValid = false;

  // 前N日通知（不可為 null）
  const preNotifyDay = addWorkProjectStoneObj.employeeProjectStonePreNotifyDay;

  // 前 N 日通知（必填，且需 >= 0）
  const nDayValid = preNotifyDay !== null && typeof preNotifyDay === "number" && preNotifyDay >= 0;

  addWorkProjectStoneValidateObj.employeeProjectStonePreNotifyDay = nDayValid;

  if (!nDayValid) isValid = false;

  addWorkProjectStoneValidateObj.employeeProjectStonePreNotifyDay = nDayValid;

  if (!nDayValid) isValid = false;

  // 結束日期減N天要大於開始日期
  let endMinusNDaysValid = true;
  if (
    addWorkProjectStoneObj.employeeProjectStoneStartTime &&
    addWorkProjectStoneObj.employeeProjectStoneEndTime &&
    preNotifyDay !== null
  ) {
    const stoneStart = new Date(addWorkProjectStoneObj.employeeProjectStoneStartTime);
    const stoneEnd = new Date(addWorkProjectStoneObj.employeeProjectStoneEndTime);
    const endMinusNDays = new Date(stoneEnd);
    endMinusNDays.setDate(endMinusNDays.getDate() - preNotifyDay);
    
    endMinusNDaysValid = endMinusNDays >= stoneStart;
  } else {
    endMinusNDaysValid = false;
  }

  addWorkProjectStoneValidateObj.employeeProjectStoneEndMinusNDaysValid = endMinusNDaysValid;

  if (!endMinusNDaysValid) isValid = false;

  // ========= 工項驗證 =========

  addWorkProjectStoneObj.employeeProjectStoneTaskList.forEach((task, index) => {
    // 工項名稱
    const taskNameValid =
      !!task.employeeProjectStoneTaskName && task.employeeProjectStoneTaskName.length <= 50;

    addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskName = taskNameValid;
    if (!taskNameValid) isValid = false;

    // 工項開始日期
    const taskStartValid = !!task.employeeProjectStoneTaskStartTime;
    addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskStartTime = taskStartValid;
    if (!taskStartValid) isValid = false;

    // 工項結束日期
    const taskEndValid = !!task.employeeProjectStoneTaskEndTime;
    addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskEndTime = taskEndValid;
    // ========= 工項時間不可超過里程碑時間 =========
    const stoneStart = new Date(addWorkProjectStoneObj.employeeProjectStoneStartTime);
    const stoneEnd = new Date(addWorkProjectStoneObj.employeeProjectStoneEndTime);
    const preNotifyDay = addWorkProjectStoneObj.employeeProjectStonePreNotifyDay ?? 0;

    const latestAllowedTaskEnd = new Date(stoneEnd);
    latestAllowedTaskEnd.setDate(latestAllowedTaskEnd.getDate() - preNotifyDay);

    const taskStart = new Date(task.employeeProjectStoneTaskStartTime);
    const taskEnd = new Date(task.employeeProjectStoneTaskEndTime);

    const startRangeValid = taskStart >= stoneStart;
    const endRangeValid = taskEnd <= latestAllowedTaskEnd;
    const dateOrderValid = taskStart <= taskEnd;

    const validateItem = addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[index];

    validateItem.employeeProjectStoneTaskStartRange = startRangeValid;
    validateItem.employeeProjectStoneTaskEndRange = endRangeValid;
    validateItem.employeeProjectStoneTaskDateOrder = dateOrderValid;

    if (!startRangeValid || !endRangeValid || !dateOrderValid) {
      isValid = false;
    }

    // // ========= 時數驗證 =========
    const taskHourValid = task.employeeProjectStoneTaskWorkHour !== null;
    addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[
      index
    ].employeeProjectStoneTaskWorkHour = taskHourValid;
    if (!taskHourValid) isValid = false;

    // // 執行者
    // const executorValid = task.employeeProjectStoneTaskExecutorList.length > 0;
    // addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[
    //   index
    // ].employeeProjectStoneTaskExecutorList = executorValid;
    // if (!executorValid) isValid = false;
  });
  return isValid;
};

//-------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 驗證 Token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateField()) {
    return;
  }

  // 呼叫 api
  const requestData: MbsWrkPrjHttpAddProjectStoneReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: props.employeeProjectID!,
    employeeProjectStoneName: addWorkProjectStoneObj.employeeProjectStoneName,
    employeeProjectStonePreNotifyDay: addWorkProjectStoneObj.employeeProjectStonePreNotifyDay ?? 0,
    employeeProjectStoneStartTime: formatToServerDateStartISO8(
      addWorkProjectStoneObj.employeeProjectStoneStartTime
    ),
    employeeProjectStoneEndTime: formatToServerDateEndISO8(
      addWorkProjectStoneObj.employeeProjectStoneEndTime
    ),
    employeeProjectStoneTaskList:
      addWorkProjectStoneObj.employeeProjectStoneTaskList.map((item) => ({
        employeeProjectStoneTaskName: item.employeeProjectStoneTaskName,
        employeeProjectStoneTaskStartTime: formatToServerDateStartISO8(
          item.employeeProjectStoneTaskStartTime
        ),
        employeeProjectStoneTaskEndTime: formatToServerDateEndISO8(
          item.employeeProjectStoneTaskEndTime
        ),
        employeeProjectStoneTaskWorkHour: item.employeeProjectStoneTaskWorkHour ?? 0,
        employeeProjectStoneTaskRemark: item.employeeProjectStoneTaskRemark,
        employeeProjectStoneTaskExecutorList: item.employeeProjectStoneTaskExecutorList.map(
          (exe) => ({
            employeeID: exe.employeeProjectStoneTaskExecutorEmployeeID,
          })
        ),
      })) ?? [],
  };

  const responseData: MbsWrkPrjHttpAddProjectStoneRspMdl | null =
    await addProjectStone(requestData);

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
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[920px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">新增里程碑</h2>
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
            v-model="addWorkProjectStoneObj.employeeProjectStoneName"
            class="input-box"
            placeholder="請輸入名稱"
            @input="resetError"
          />
          <div>
            <span v-if="!addWorkProjectStoneValidateObj.employeeProjectStoneName" class="error-tip">
              不可為空，長度不可超過50個字
            </span>
          </div>
        </div>

        <div class="flex gap-4 w-full">
          <!-- 開始日期 -->
          <div class="flex-1">
            <label class="form-label">開始日期 <span class="required-label">*</span></label>
            <input
              v-model="addWorkProjectStoneObj.employeeProjectStoneStartTime"
              type="date"
              class="input-box"
            />
            <div>
              <span
                v-if="!addWorkProjectStoneValidateObj.employeeProjectStoneStartTime"
                class="error-tip"
              >
                不可為空
              </span>
              <span
                v-if="!addWorkProjectStoneValidateObj.employeeProjectStoneDateRange"
                class="error-tip"
              >
                開始日期必須早於結束日期
              </span>
            </div>
          </div>

          <!-- 結束日期 -->
          <div class="flex-1">
            <label class="form-label">結束日期 <span class="required-label">*</span></label>
            <input
              v-model="addWorkProjectStoneObj.employeeProjectStoneEndTime"
              type="date"
              class="input-box"
            />
            <div class="error-wrapper">
              <span
                v-if="!addWorkProjectStoneValidateObj.employeeProjectStoneEndTime"
                class="error-tip"
              >
                不可為空
              </span>

              <span
                v-if="!addWorkProjectStoneValidateObj.employeeProjectStoneWithinProjectRange"
                class="error-tip"
              >
                里程碑時間需要在專案時間內
              </span>
            </div>
          </div>

          <!--前 N 日通知-->
          <div class="">
            <label class="form-label">前 N 日通知 <span class="required-label">*</span></label>
            <input
              v-model="addWorkProjectStoneObj.employeeProjectStonePreNotifyDay"
              type="number"
              class="input-box"
              placeholder="請輸入天數"
            />
            <div class="error-wrapper">
              <span
                v-if="!addWorkProjectStoneValidateObj.employeeProjectStonePreNotifyDay"
                class="error-tip"
              >
                不可為空，且需要為大於等於 0 的數字
              </span>
              <span
                v-if="!addWorkProjectStoneValidateObj.employeeProjectStoneEndMinusNDaysValid"
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
            v-for="(task, tIndex) in addWorkProjectStoneObj.employeeProjectStoneTaskList"
            :key="tIndex"
            class="space-y-3 rounded-md bg-gray-100 border"
          >
            <!-- 任務標題 -->
            <div class="flex justify-between items-center border-b bg-gray-100 px-4 py-2">
              <h4 class="font-semibold text-gray-700">工項 {{ tIndex + 1 }}</h4>
              <button class="btn-delete" @click="removeTask(tIndex)">刪除</button>
            </div>

            <div class="p-4 space-y-4">
              <!-- 工項名稱 -->
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
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
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
                  <div class="error-wrapper">
                    <p
                      v-if="
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskExecutorList
                      "
                      class="error-tip"
                    >
                      請至少選擇一位執行者
                    </p>
                  </div>
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
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskStartTime
                      "
                      class="error-tip"
                    >
                      不可為空
                    </span>
                    <span
                      v-if="
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskStartRange
                      "
                      class="error-tip"
                    >
                      工項開始不可早於里程碑開始
                    </span>

                    <span
                      v-if="
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
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
                  <!-- <span class="text-[10px]">(不可超過里程碑結束日期減前N日通知)</span> -->
                  <input
                    v-model="task.employeeProjectStoneTaskEndTime"
                    type="date"
                    class="input-box"
                  />
                  <div class="error-wrapper">
                    <span
                      v-if="
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskEndTime
                      "
                      class="error-tip"
                    >
                      不可為空
                    </span>

                    <span
                      v-if="
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
                          .employeeProjectStoneTaskEndRange
                      "
                      class="error-tip"
                    >
                      工項結束不可晚於里程碑結束前 N 日
                    </span>

                    <span
                      v-if="
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
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
                        !addWorkProjectStoneValidateObj.employeeProjectStoneTaskList[tIndex]
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
