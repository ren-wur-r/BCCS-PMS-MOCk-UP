<script setup lang="ts">
import { reactive, watch } from "vue";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";

const { errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
const props = defineProps<{
  managerContacterRatingReasonID: number;
  initialName: string;
  initialStatus: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit", data: { managerContacterRatingReasonID: number; managerContacterRatingReasonName?: string; managerContacterRatingReasonStatus?: boolean }): void;
}>();
//----------------------------------------------------------------------------
/** 系統設定-客戶窗口-評等原因-更新模型 */
interface SysCttRatingReasonUpdateMdl {
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName: string | null;
  managerContacterRatingReasonStatus: boolean;
}

/** 系統設定-客戶窗口-評等原因-原始模型 */
interface SysCttRatingReasonOriginalMdl {
  managerContacterRatingReasonName: string | null;
  managerContacterRatingReasonStatus: boolean;
}

/** 系統設定-客戶窗口-評等原因-驗證模型 */
interface SysCttRatingReasonValidateMdl {
  managerContacterRatingReasonName: boolean;
}

/** 系統設定-客戶窗口-評等原因-更新物件 */
const sysCttRatingReasonUpdateObj = reactive<SysCttRatingReasonUpdateMdl>({
  managerContacterRatingReasonID: 0,
  managerContacterRatingReasonName: "",
  managerContacterRatingReasonStatus: true,
});

/** 系統設定-客戶窗口-評等原因-原始物件 */
const sysCttRatingReasonOriginalObj = reactive<SysCttRatingReasonOriginalMdl>({
  managerContacterRatingReasonName: null,
  managerContacterRatingReasonStatus: true,
});

/** 系統設定-客戶窗口-評等原因-驗證物件 */
const SysCttRatingReasonValidateObj = reactive<SysCttRatingReasonValidateMdl>({
  managerContacterRatingReasonName: true,
});
//------------------------------------------------------------
/** 驗證方法 */
const validateField = () => {
  let isValid = true;

  // 名稱不可為空
  if (
    !sysCttRatingReasonUpdateObj.managerContacterRatingReasonName ||
    sysCttRatingReasonUpdateObj.managerContacterRatingReasonName.trim() === "" ||
    sysCttRatingReasonUpdateObj.managerContacterRatingReasonName.length > 50
  ) {
    SysCttRatingReasonValidateObj.managerContacterRatingReasonName = false;
    isValid = false;
  }

  return isValid;
};
// ------------------------------------------------------------
/** 初始化資料 */
watch(
  () => [props.initialName, props.initialStatus],
  () => {
    sysCttRatingReasonUpdateObj.managerContacterRatingReasonID = props.managerContacterRatingReasonID;
    sysCttRatingReasonUpdateObj.managerContacterRatingReasonName = props.initialName;
    sysCttRatingReasonUpdateObj.managerContacterRatingReasonStatus = props.initialStatus;
    
    sysCttRatingReasonOriginalObj.managerContacterRatingReasonName = props.initialName;
    sysCttRatingReasonOriginalObj.managerContacterRatingReasonStatus = props.initialStatus;
  },
  { immediate: true }
);

/** 取得變更的欄位 */
const getChangedFields = () => {
  const changes: {
    managerContacterRatingReasonID: number;
    managerContacterRatingReasonName?: string;
    managerContacterRatingReasonStatus?: boolean;
  } = {
    managerContacterRatingReasonID: props.managerContacterRatingReasonID,
  };

  // 比較【名稱】
  const trimmedName = sysCttRatingReasonUpdateObj.managerContacterRatingReasonName?.trim();
  if (trimmedName !== sysCttRatingReasonOriginalObj.managerContacterRatingReasonName) {
    changes.managerContacterRatingReasonName = trimmedName;
  }
  // 比較【狀態】
  if (
    sysCttRatingReasonUpdateObj.managerContacterRatingReasonStatus !==
    sysCttRatingReasonOriginalObj.managerContacterRatingReasonStatus
  ) {
    changes.managerContacterRatingReasonStatus =
      sysCttRatingReasonUpdateObj.managerContacterRatingReasonStatus;
  }

  return changes;
};

/** 點擊【送出】按鈕 */
const clickSubmitBtn = () => {
  // 欄位驗證
  if (!validateField()) {
    return;
  }

  // 取得變更的欄位
  const changedFields = getChangedFields();

  // 檢查是否有變更（排除必要的 ID）
  const hasChanges = Object.keys(changedFields).length > 1;
  if (!hasChanges) {
    showError.value = true;
    errorMessage.value = "沒有任何變更需要儲存 !";
    return;
  }

  // Emit 變更的資料到父元件
  emit("submit", changedFields);
  emit("close");
};
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯開發評等原因</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <!-- 後端錯誤訊息 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded mb-4"
        >
          {{ errorMessage }}
        </p>

        <div class="space-y-6">
          <!-- 開發評等原因名稱 -->
          <div>
            <label class="form-label">開發評等原因名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysCttRatingReasonUpdateObj.managerContacterRatingReasonName"
              class="input-box"
              type="text"
              placeholder="請輸入名稱"
            />
            <p class="error-wrapper">
              <span
                v-if="!SysCttRatingReasonValidateObj.managerContacterRatingReasonName"
                class="error-tip"
              >
                不可為空，長度不可超過 50 個字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select
              v-model="sysCttRatingReasonUpdateObj.managerContacterRatingReasonStatus"
              class="select-box"
            >
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper"></p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
