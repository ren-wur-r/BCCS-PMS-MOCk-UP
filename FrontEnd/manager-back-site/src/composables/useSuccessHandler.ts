// src/composables/useSuccessHandler.ts
import { ref } from "vue";

/** useSuccessHandler - 處理成功訊息邏輯的 composable */
export function useSuccessHandler() {
  /** 成功訊息文字 */
  const successMessage = ref("");
  /** 成功訊息顯示開關，控制提示框的顯示 */
  const showSuccess = ref(false);

  /**
   * 顯示成功訊息
   * @param message - 要顯示的成功訊息文字
   */
  function displaySuccess(message: string) {
    successMessage.value = message;
    showSuccess.value = true;
  }

  /**
   * 關閉成功訊息，重置成功狀態
   */
  function closeSuccess() {
    showSuccess.value = false;
    successMessage.value = "";
  }

  return {
    successMessage,
    showSuccess,
    displaySuccess,
    closeSuccess,
  };
}
