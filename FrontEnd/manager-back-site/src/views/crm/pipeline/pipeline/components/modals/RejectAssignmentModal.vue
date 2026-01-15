<script setup lang="ts">
import { ref, watch } from "vue";

interface Props {
  show: boolean;
}

interface Emits {
  (e: "confirm", rejectReason: string): void;
  (e: "cancel"): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

/** 拒絕原因 */
const rejectReason = ref("");

/** 錯誤訊息 */
const errorMessage = ref("");

/** 重置表單 */
const resetForm = () => {
  rejectReason.value = "";
  errorMessage.value = "";
};

/** 清除錯誤訊息 */
const clearError = () => {
  errorMessage.value = "";
};

/** 驗證表單 */
const validateForm = (): boolean => {
  clearError();

  if (!rejectReason.value.trim()) {
    errorMessage.value = "請填寫拒絕原因";
    return false;
  }

  return true;
};

/** 確認送出 */
const handleConfirm = () => {
  if (!validateForm()) {
    return;
  }
  emit("confirm", rejectReason.value.trim());
};

/** 取消 */
const handleCancel = () => {
  emit("cancel");
};

/** 監聽 show 變化，重置表單 */
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      resetForm();
    }
  }
);

/** 監聽拒絕原因變化，清除錯誤訊息 */
watch(
  () => rejectReason.value,
  () => {
    if (errorMessage.value && rejectReason.value.trim()) {
      clearError();
    }
  }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div class="w-[500px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">拒絕指派</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-6">
        <!-- 拒絕原因 -->
        <div class="mb-4 flex flex-col gap-2">
          <label class="form-label">
            拒絕原因
            <span class="required-label">*</span>
          </label>
          <textarea
            v-model="rejectReason"
            rows="8"
            class="textarea-box resize-none"
            :class="{ 'border-red-500': errorMessage }"
            placeholder="請說明拒絕此指派的原因..."
          ></textarea>
          <p v-if="errorMessage" class="error-tip">
            {{ errorMessage }}
          </p>
        </div>
      </div>

      <!-- 底部按鈕 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="handleCancel">取消</button>
        <button class="btn-submit" @click="handleConfirm">送出</button>
      </div>
    </div>
  </div>
</template>
