<script setup lang="ts">
import { onMounted, reactive, ref, defineAsyncComponent } from "vue";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";

// Components
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);

const { requireToken } = useAuth();
const { errorMessage, showError, closeError } = useErrorCodeHandler();
//--------------------------------------------------------------
const props = defineProps<WorkJobRecordOnlyAddPropsMdl>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit", payload: WorkJobRecordOnlyAddPayloadMdl): void;
}>();
//----------------------------------------------------------------------
/** 新增工作記錄-props-模型 */
interface WorkJobRecordOnlyAddPropsMdl {
  employeeProjectJobID: number;
}

/** 新增工作記錄-Payload模型(傳送給父元件) */
export interface WorkJobRecordOnlyAddPayloadMdl {
  employeeProjectStoneJobID: number;
  employeeProjectStoneJobWorkStartTime: string;
  employeeProjectStoneJobWorkEndTime: string;
  employeeProjectStoneJobWorkRemark: string;
  employeeProjectStoneJobWorkFileList: WorkJobRecordOnlyAddFileMdl[];
}

/** 新增工作記錄-檔案模型（內部使用） */
interface WorkJobRecordOnlyAddFileMdl {
  employeeProjectStoneJobWorkFileUrl: string;
}

/** 新增工作記錄-表單模型（內部使用） */
interface WorkJobRecordOnlyAddFormMdl {
  employeeProjectStoneJobWorkDate: string;
  employeeProjectStoneJobStartTime: string;
  employeeProjectStoneJobEndTime: string;
  employeeProjectStoneJobContent: string;
  employeeProjectStoneJobWorkFileList: WorkJobRecordOnlyAddFileMdl[];
}

/** 新增工作記錄-表單驗證模型（內部使用） */
interface WorkJobRecordOnlyAddFormValidateMdl {
  employeeProjectStoneJobWorkDate: boolean;
  employeeProjectStoneJobStartTime: boolean;
  employeeProjectStoneJobEndTime: boolean;
  employeeProjectStoneJobContent: boolean;
}
//----------------------------------------------
/** 新增工作記錄-表單物件 */
const addWorkJobRecordFormObj = reactive<WorkJobRecordOnlyAddFormMdl>({
  employeeProjectStoneJobWorkDate: "",
  employeeProjectStoneJobStartTime: "",
  employeeProjectStoneJobEndTime: "",
  employeeProjectStoneJobContent: "",
  employeeProjectStoneJobWorkFileList: [],
});

/** 新增工作記錄-表單驗證物件 */
const addWorkJobRecordFormValidateObj = reactive<Partial<WorkJobRecordOnlyAddFormValidateMdl>>({
  employeeProjectStoneJobWorkDate: true,
  employeeProjectStoneJobStartTime: true,
  employeeProjectStoneJobEndTime: true,
  employeeProjectStoneJobContent: true,
});

//----------------------------------------------
// 工具：組成 yyyy-MM-ddTHH:mm:ss+08:00
//----------------------------------------------
function buildISO(date: string, time: string) {
  if (!date || !time) return "";
  return `${date}T${time}:00+08:00`;
}
//----------------------------------------------
/** 點擊送出按鈕 */
function clickSubmitBtn() {
  let ok = true;

  addWorkJobRecordFormValidateObj.employeeProjectStoneJobWorkDate =
    !!addWorkJobRecordFormObj.employeeProjectStoneJobWorkDate;
  if (!addWorkJobRecordFormValidateObj.employeeProjectStoneJobWorkDate) ok = false;

  addWorkJobRecordFormValidateObj.employeeProjectStoneJobStartTime =
    !!addWorkJobRecordFormObj.employeeProjectStoneJobStartTime;
  addWorkJobRecordFormValidateObj.employeeProjectStoneJobEndTime =
    !!addWorkJobRecordFormObj.employeeProjectStoneJobEndTime;
  if (
    !addWorkJobRecordFormValidateObj.employeeProjectStoneJobStartTime ||
    !addWorkJobRecordFormValidateObj.employeeProjectStoneJobEndTime
  )
    ok = false;

  addWorkJobRecordFormValidateObj.employeeProjectStoneJobContent =
    !!addWorkJobRecordFormObj.employeeProjectStoneJobContent.trim();
  if (!addWorkJobRecordFormValidateObj.employeeProjectStoneJobContent) ok = false;

  if (!ok) return;

  const payload: WorkJobRecordOnlyAddPayloadMdl = {
    employeeProjectStoneJobID: props.employeeProjectJobID,
    employeeProjectStoneJobWorkStartTime: buildISO(
      addWorkJobRecordFormObj.employeeProjectStoneJobWorkDate,
      addWorkJobRecordFormObj.employeeProjectStoneJobStartTime
    ),
    employeeProjectStoneJobWorkEndTime: buildISO(
      addWorkJobRecordFormObj.employeeProjectStoneJobWorkDate,
      addWorkJobRecordFormObj.employeeProjectStoneJobEndTime
    ),
    employeeProjectStoneJobWorkRemark:
      addWorkJobRecordFormObj.employeeProjectStoneJobContent.trim(),
    employeeProjectStoneJobWorkFileList:
      addWorkJobRecordFormObj.employeeProjectStoneJobWorkFileList,
  };

  emit("submit", payload);
  emit("close");
}
// ==================== 附加檔案相關 ====================
/** 附加網址彈跳視窗 */
const showAddFileModal = ref(false);
const tempFileUrl = ref("");

/** 確認新增附加檔案 */
function clickConfirmAddFileBtn() {
  if (!tempFileUrl.value.trim()) return;

  addWorkJobRecordFormObj.employeeProjectStoneJobWorkFileList.push({
    employeeProjectStoneJobWorkFileUrl: tempFileUrl.value.trim(),
  });

  tempFileUrl.value = "";
  showAddFileModal.value = false;
}
/** 刪除附加檔案 */
function clickRemoveFileBtn(index: number) {
  addWorkJobRecordFormObj.employeeProjectStoneJobWorkFileList.splice(index, 1);
}

//----------------------------------------------
/** 取得今天 yyyy-MM-dd */
function getTodayYMD() {
  const d = new Date();
  return d.toISOString().slice(0, 10);
}

/** 建立固定時間格式 HH:mm */
function fixedTime(hh: number, mm: number = 0) {
  const h = hh.toString().padStart(2, "0");
  const m = mm.toString().padStart(2, "0");
  return `${h}:${m}`;
}

onMounted(async () => {
  if (!(await requireToken())) {
    return;
  }

  // 預設日期（使用者一打開彈窗就會看到今天）
  addWorkJobRecordFormObj.employeeProjectStoneJobWorkDate = getTodayYMD();
  addWorkJobRecordFormObj.employeeProjectStoneJobStartTime = fixedTime(9, 0); // 09:00
  addWorkJobRecordFormObj.employeeProjectStoneJobEndTime = fixedTime(18, 0); // 18:00
});
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-xl w-[720px] bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- Header -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">新增工作記錄</h2>

        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- Body -->
      <div class="p-8 overflow-y-auto flex-1">
        <!-- 工作記錄 -->
        <div class="space-y-4">
          <!-- 日期 -->
          <div class="">
            <label class="form-label">日期 <span class="required-label">*</span></label>
            <input
              v-model="addWorkJobRecordFormObj.employeeProjectStoneJobWorkDate"
              type="date"
              class="input-box"
            />
            <p class="error-wrapper">
              <span
                v-if="!addWorkJobRecordFormValidateObj.employeeProjectStoneJobWorkDate"
                class="error-tip"
                >不可為空</span
              >
            </p>
          </div>

          <!-- 工作時間 -->
          <div class="flex gap-4">
            <div class="flex-1">
              <label class="form-label">工作時間 <span class="required-label">*</span></label>
              <input
                v-model="addWorkJobRecordFormObj.employeeProjectStoneJobStartTime"
                type="time"
                class="input-box"
              />
              <p class="error-wrapper">
                <span
                  v-if="!addWorkJobRecordFormValidateObj.employeeProjectStoneJobStartTime"
                  class="error-tip"
                  >不可為空</span
                >
              </p>
            </div>

            <div class="flex-1">
              <label class="form-label"></label>
              <input
                v-model="addWorkJobRecordFormObj.employeeProjectStoneJobEndTime"
                type="time"
                class="input-box mt-6"
              />
              <p class="error-wrapper">
                <span
                  v-if="!addWorkJobRecordFormValidateObj.employeeProjectStoneJobEndTime"
                  class="error-tip"
                  >不可為空</span
                >
              </p>
            </div>
          </div>

          <!-- 工作內容 -->
          <div>
            <label class="form-label">工作內容 <span class="required-label">*</span></label>
            <textarea
              v-model="addWorkJobRecordFormObj.employeeProjectStoneJobContent"
              class="textarea-box"
              rows="11"
              placeholder="請輸入工作內容"
            ></textarea>
            <p class="error-wrapper">
              <span
                v-if="!addWorkJobRecordFormValidateObj.employeeProjectStoneJobContent"
                class="error-tip"
                >不可為空</span
              >
            </p>
          </div>

          <!-- 附加檔案 -->
          <div class="border p-3 rounded-lg">
            <div class="flex justify-between">
              <label class="font-bold text-sm text-gray-700">附加檔案</label>
              <div class="flex justify-between items-center mb-2">
                <button
                  class="text-brand-100 text-sm hover:underline"
                  @click="showAddFileModal = true"
                >
                  + 新增檔案
                </button>
              </div>
            </div>

            <div
              v-if="addWorkJobRecordFormObj.employeeProjectStoneJobWorkFileList.length === 0"
              class="text-gray-400 text-sm"
            >
              尚無附件
            </div>

            <!-- 文件列表 -->
            <ul class="space-y-2">
              <li
                v-for="(file, index) in addWorkJobRecordFormObj.employeeProjectStoneJobWorkFileList"
                :key="index"
                class="flex items-center p-2 rounded hover:bg-gray-100 transition-colors"
              >
                <!-- 左側：網址 -->
                <div class="flex-1 pr-2">
                  <a
                    :href="file.employeeProjectStoneJobWorkFileUrl"
                    target="_blank"
                    class="text-gray-600 text-sm underline break-all block"
                  >
                    {{ file.employeeProjectStoneJobWorkFileUrl }}
                  </a>
                </div>

                <!-- 右側：刪除按鈕-->
                <button
                  class="text-red-500 text-sm w-[40px] text-right hover:underline"
                  @click="clickRemoveFileBtn(index)"
                >
                  刪除
                </button>
              </li>
            </ul>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div class="p-4 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 新增附件 Modal -->
  <div v-if="showAddFileModal" class="modal-overlay">
    <div class="w-[400px] bg-white p-6 rounded-lg shadow-lg">
      <h2 class="modal-title mb-4">新增附件網址</h2>

      <input v-model="tempFileUrl" type="text" class="input-box w-full" placeholder="請輸入網址" />

      <div class="flex justify-end mt-4 gap-2">
        <button class="btn-cancel" @click="showAddFileModal = false">取消</button>
        <button class="btn-submit" @click="clickConfirmAddFileBtn">確認</button>
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
</template>
