<script setup lang="ts">
import { reactive, onMounted, ref, defineAsyncComponent, computed } from "vue";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import TextCounter from "@/components/global/counter/TextCounter.vue";
import {
  MbsWrkJobHttpGetProjectStoneJobWorkReqMdl,
  MbsWrkJobHttpGetProjectStoneJobWorkRspMdl,
} from "@/services/pms-http/work/job/workJobHttpFormat";
import { getProjectStoneJobWork } from "@/services";
import { formatDate, formatToDatetimeLocal } from "@/utils/timeFormatter";
// Components
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);

const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage, closeError } = useErrorCodeHandler();
// --------------------------------------------------------------
// Props + Emits
//--------------------------------------------------------------
const props = defineProps<DetailWorkJobRecordPropsMdl>();
const emit = defineEmits<{
  close: [];
}>();
// --------------------------------------------------------------
/** 工作紀錄模式 (用來判斷顯示) */
type WorkJobRecordMode = "full" | "record-only";
const isFullMode = computed(() => props.mode === "full");

/** 檢視工作紀錄-Props-模型 (父元件傳資料顯示)*/
interface DetailWorkJobRecordPropsMdl {
  employeeProjectStoneJobWorkID: number;
  employeeProjectName: string;
  employeeProjectStoneName: string;
  employeeProjectStoneTaskName: string;
  mode: WorkJobRecordMode | null;
}

/** 檢視工項內容-Payload-模型(傳遞給父元件) */
export interface DetailWorkJobPayloadMdl {
  employeeProjectStoneJobID: number | null;
  employeeProjectStoneTaskRemark: string | null;
  employeeProjectStoneTaskBucketList: UpdateWorkJobBucketItem[];
}

/** 更新工項內容-原始模型 */
export interface UpdateWorkJobOriginalMdl {
  employeeProjectStoneJobID: number;
  employeeProjectStoneTaskRemark: string | null;
  employeeProjectStoneTaskBucketList: UpdateWorkJobBucketItem[];
}

/** 更新工作紀錄-工作清單-模型 */
interface UpdateWorkJobBucketItem {
  employeeProjectStoneJobBucketID: number;
  employeeProjectStoneJobBucketName: string;
  employeeProjectStoneJobBucketIsFinished: boolean;
}

/** 更新工作紀錄-Payload-模型(傳遞給父元件) */
export interface UpdateWorkRecordPayloadMdl {
  employeeProjectStoneJobID: number | null; //工項ID
  employeeProjectStoneJobWorkID: number; //工作記錄ID
  employeeProjectStoneJobWorkDate: string;
  employeeProjectStoneJobWorkStartTime: string | null;
  employeeProjectStoneJobWorkEndTime: string | null;
  employeeProjectStoneJobContent: string;
  employeeProjectStoneJobWorkFileList: UpdateWorkRecordFileMdl[];
}

/** 更新工作紀錄-工作檔案模型 */
interface UpdateWorkRecordFileMdl {
  employeeProjectStoneJobWorkFileID: number;
  employeeProjectStoneJobWorkFileUrl: string;
}

// --------------------------------------------------------------
/** 拆解 yyyy-MM-ddTHH:mm:ss+08:00 為 date + time */
function splitISO(datetime: string) {
  if (!datetime) return { date: null, time: null };
  const [d, t] = datetime.split("T");
  const time = t?.substring(0, 5) ?? "";
  return { date: d, time };
}
// --------------------------------------------------------------
// 左側 - 工項內容
// --------------------------------------------------------------
/** 更新工作紀錄-工項內容-物件 */
const updateWorkJobWorkFormObj = reactive<DetailWorkJobPayloadMdl>({
  employeeProjectStoneJobID: 0,
  employeeProjectStoneTaskRemark: "",
  employeeProjectStoneTaskBucketList: [] as UpdateWorkJobBucketItem[],
});
// --------------------------------------------------------------
// 右側 - 工作紀錄
// --------------------------------------------------------------
/** 更新工作紀錄-Payload-物件 */
const updateWorkRecordPayloadObj = reactive<UpdateWorkRecordPayloadMdl>({
  employeeProjectStoneJobID: 0,
  employeeProjectStoneJobWorkID: 0,
  employeeProjectStoneJobWorkDate: "",
  employeeProjectStoneJobWorkStartTime: null,
  employeeProjectStoneJobWorkEndTime: null,
  employeeProjectStoneJobContent: "",
  employeeProjectStoneJobWorkFileList: [],
});

// --------------------------------------------------------------
const resolvedJobID = ref<number | null>(null);

/** 取得資料 */
async function getData() {
  if (!requireToken()) return;

  const requestData: MbsWrkJobHttpGetProjectStoneJobWorkReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectStoneJobWorkID: props.employeeProjectStoneJobWorkID,
  };

  const responseData: MbsWrkJobHttpGetProjectStoneJobWorkRspMdl | null =
    await getProjectStoneJobWork(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤";
    return;
  }

  if (!handleErrorCode(responseData.errorCode)) return;
  // 獲取工項ID
  resolvedJobID.value = responseData.employeeProjectStoneJobID;
  // 設定工項內容
  updateWorkJobWorkFormObj.employeeProjectStoneJobID = responseData.employeeProjectStoneJobID; //工項ID
  updateWorkJobWorkFormObj.employeeProjectStoneTaskRemark =
    responseData.employeeProjectStoneJobRemark;

  updateWorkJobWorkFormObj.employeeProjectStoneTaskBucketList =
    responseData.employeeProjectStoneJobBucketList.map((b) => ({
      employeeProjectStoneJobBucketID: b.employeeProjectStoneJobBucketID,
      employeeProjectStoneJobBucketName: b.employeeProjectStoneJobBucketName,
      employeeProjectStoneJobBucketIsFinished: b.employeeProjectStoneJobBucketIsFinished,
    }));
  // 設定工作紀錄
  const employeeProjectStoneJobWorkStartTime = splitISO(
    formatToDatetimeLocal(responseData.employeeProjectStoneJobWorkStartTime)
  );
  const employeeProjectStoneJobWorkEndTime = splitISO(
    formatToDatetimeLocal(responseData.employeeProjectStoneJobWorkEndTime)
  );

  updateWorkRecordPayloadObj.employeeProjectStoneJobWorkDate = formatDate(
    employeeProjectStoneJobWorkStartTime.date
  );
  updateWorkRecordPayloadObj.employeeProjectStoneJobWorkStartTime =
    employeeProjectStoneJobWorkStartTime.time;
  updateWorkRecordPayloadObj.employeeProjectStoneJobWorkEndTime =
    employeeProjectStoneJobWorkEndTime.time;
  updateWorkRecordPayloadObj.employeeProjectStoneJobContent =
    responseData.employeeProjectStoneJobWorkRemark;
  updateWorkRecordPayloadObj.employeeProjectStoneJobWorkFileList =
    responseData.employeeProjectStoneJobWorkFileList.map((f) => ({
      employeeProjectStoneJobWorkFileID: f.employeeProjectStoneJobWorkFileID,
      employeeProjectStoneJobWorkFileUrl: f.employeeProjectStoneJobWorkFileUrl,
    }));

}

/** 對話框寬度樣式 */
const dialogWidthClass = computed(() => {
  return props.mode === "full" ? "w-[1050px] max-w-5xl" : "w-[620px] max-w-xl";
});
//--------------------------------------------------------------
// 生命週期
onMounted(() => getData());
</script>

<template>
  <div class="modal-overlay">
    <div
      class="bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
      :class="dialogWidthClass"
    >
      <!-- Header -->
      <div class="flex items-center justify-between p-4 flex-shrink-0">
        <h2 class="modal-title">檢視工作記錄</h2>

        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="emit('close')"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- Body -->
      <div
        class="p-8 overflow-y-auto flex-1 grid gap-10"
        :class="isFullMode ? 'grid-cols-[1fr_1fr]' : 'grid-cols-1'"
      >
        <!-- 左側 -->
        <div v-if="isFullMode" class="space-y-4">
          <div class="subtitle">工項內容</div>

          <!-- 專案名 -->
          <div>
            <label class="form-label">專案名稱 <span class="required-label">*</span></label>
            <input
              type="text"
              class="input-box bg-gray-100"
              :value="props.employeeProjectName"
              disabled
            />
          </div>

          <!-- 里程碑 -->
          <div>
            <label class="form-label">里程碑名稱 <span class="required-label">*</span></label>
            <input
              type="text"
              class="input-box bg-gray-100"
              :value="props.employeeProjectStoneName"
              disabled
            />
          </div>

          <!-- 工項名稱 -->
          <div>
            <label class="form-label">工項名稱 <span class="required-label">*</span></label>
            <input
              type="text"
              class="input-box bg-gray-100"
              :value="props.employeeProjectStoneTaskName"
              disabled
            />
          </div>

          <!-- 備註 -->
          <div>
            <label class="form-label">備註</label>
            <textarea
              v-model="updateWorkJobWorkFormObj.employeeProjectStoneTaskRemark"
              class="textarea-box"
              rows="6"
              placeholder="請輸入備註"
              disabled
            ></textarea>
            <TextCounter
              :text="updateWorkJobWorkFormObj.employeeProjectStoneTaskRemark"
              :max-length="1000"
              :required="true"
            />
          </div>

          <!-- 子項目 Bucket -->
          <div class="border p-3 rounded-lg">
            <div class="flex justify-between mb-2">
              <h3 class="font-bold text-sm text-gray-700">子項目</h3>
            </div>

            <!-- 沒有子項目時 -->
            <p
              v-if="updateWorkJobWorkFormObj.employeeProjectStoneTaskBucketList.length === 0"
              class="text-gray-400 text-sm text-center py-2"
            >
              無子項目
            </p>

            <div
              v-for="(bucket, idx) in updateWorkJobWorkFormObj.employeeProjectStoneTaskBucketList"
              :key="idx"
              class="flex flex-col mt-2 py-1 px-1 rounded"
            >
              <div class="flex items-center gap-3">
                <!--勾選框-->
                <input
                  :id="'bucket_' + idx"
                  v-model="bucket.employeeProjectStoneJobBucketIsFinished"
                  type="checkbox"
                  disabled
                />
                <!--文字-->
                <label :for="'bucket_' + idx" class="flex-1 text-sm rounded cursor-pointer">
                  {{ bucket.employeeProjectStoneJobBucketName }}
                </label>
              </div>
            </div>
          </div>
        </div>

        <!-- 右側：工作記錄 -->
        <div class="space-y-4" :class="isFullMode ? 'pl-10 border-l border-gray-300' : ''">
          <div class="subtitle">工作記錄</div>

          <!-- 日期 -->
          <div class="">
            <label class="form-label">日期 <span class="required-label">*</span></label>
            <input
              v-model="updateWorkRecordPayloadObj.employeeProjectStoneJobWorkDate"
              type="date"
              class="input-box"
              disabled
            />

          </div>

          <!-- 工作時間 -->
          <div class="flex gap-4">
            <div class="flex-1">
              <label class="form-label">工作時間 <span class="required-label">*</span></label>
              <input
                v-model="updateWorkRecordPayloadObj.employeeProjectStoneJobWorkStartTime"
                type="time"
                class="input-box"
                disabled
              />
            </div>

            <div class="flex-1">
              <label class="form-label"></label>
              <input
                v-model="updateWorkRecordPayloadObj.employeeProjectStoneJobWorkEndTime"
                type="time"
                class="input-box mt-6"
                disabled
              />
            
            </div>
          </div>

          <!-- 工作內容 -->
          <div>
            <label class="form-label">工作內容 <span class="required-label">*</span></label>
            <textarea
              v-model="updateWorkRecordPayloadObj.employeeProjectStoneJobContent"
              class="textarea-box"
              rows="11"
              placeholder="請輸入工作內容"
              disabled
            ></textarea>
          </div>

          <!-- 附加檔案 -->
          <div class="border p-3 rounded-lg">
            <div class="flex justify-between">
              <label class="font-bold text-sm text-gray-700">附加檔案</label>
            </div>

            <div
              v-if="updateWorkRecordPayloadObj.employeeProjectStoneJobWorkFileList.length === 0"
              class="text-gray-400 text-sm"
            >
              尚無附件
            </div>

            <!-- 文件列表 -->
            <ul class="space-y-2">
              <li
                v-for="(
                  file, index
                ) in updateWorkRecordPayloadObj.employeeProjectStoneJobWorkFileList"
                :key="index"
                class="flex items-center p-2 rounded hover:bg-gray-100 transition-colors"
              >
                <!-- 左側：網址 -->
                <div class="flex-1 pr-2">
                  <a
                    :href="file.employeeProjectStoneJobWorkFileUrl"
                    target="_blank"
                    disabled
                    class="text-gray-600 text-sm underline break-all block"
                  >
                    {{ file.employeeProjectStoneJobWorkFileUrl }}
                  </a>
                </div>

              </li>
            </ul>
          </div>
        </div>
      </div>


    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
</template>
