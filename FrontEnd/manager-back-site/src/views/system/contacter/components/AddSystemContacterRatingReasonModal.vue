<script setup lang="ts">
import { reactive } from "vue";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";

const { errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit", data: { managerContacterRatingReasonName: string; managerContacterRatingReasonStatus: boolean }): void;
}>();
//----------------------------------------------------------------------------
/** 系統設定-客戶窗口-評等原因-新增模型 */
interface SysCttRatingReasonAddMdl {
  managerContacterRatingReasonName: string;
  managerContacterRatingReasonStatus: boolean;
}

/** 系統設定-客戶窗口-評等原因-驗證模型 */
interface SysCttRatingReasonValidateMdl {
  managerContacterRatingReasonName: boolean;
}

/** 系統設定-客戶窗口-評等原因-新增物件 */
const SysCttRatingReasonAddObj = reactive<SysCttRatingReasonAddMdl>({
  managerContacterRatingReasonName: "",
  managerContacterRatingReasonStatus: true,
});

/** 系統設定-客戶窗口-評等原因-驗證物件 */
const SysCttRatingReasonValidateObj = reactive<SysCttRatingReasonValidateMdl>({
  managerContacterRatingReasonName: true,
});
//----------------------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 名稱不可為空
  if (
    !SysCttRatingReasonAddObj.managerContacterRatingReasonName ||
    SysCttRatingReasonAddObj.managerContacterRatingReasonName.trim() === "" ||
    SysCttRatingReasonAddObj.managerContacterRatingReasonName.length > 50
  ) {
    SysCttRatingReasonValidateObj.managerContacterRatingReasonName = false;
    isValid = false;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/** 點擊提交【開發評等原因】按鈕 */
const clickSubmitRatingReasonBtn = () => {
  // 欄位驗證
  if (!validateField()) {
    return;
  }
  
  // Emit 資料到父元件
  emit("submit", {
    managerContacterRatingReasonName: SysCttRatingReasonAddObj.managerContacterRatingReasonName,
    managerContacterRatingReasonStatus: SysCttRatingReasonAddObj.managerContacterRatingReasonStatus,
  });
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
        <h2 class="modal-title">新增開發評等原因</h2>
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
              v-model="SysCttRatingReasonAddObj.managerContacterRatingReasonName"
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
              v-model="SysCttRatingReasonAddObj.managerContacterRatingReasonStatus"
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
          <button class="btn-submit" @click="clickSubmitRatingReasonBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
