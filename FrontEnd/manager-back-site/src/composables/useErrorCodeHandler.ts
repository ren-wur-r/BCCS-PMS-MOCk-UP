// src/composables/useErrorCodeHandler.ts
import { ref } from "vue";
import { useRouter } from "vue-router";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

/** useErrorCodeHandler - 處理錯誤代碼邏輯的 composable */
export function useErrorCodeHandler() {
  /** 錯誤訊息文字 */
  const errorMessage = ref("");
  /** 錯誤訊息顯示開關，控制彈跳視窗或提示框的顯示 */
  const showError = ref(false);
  /** Vue Router 實例，用於頁面跳轉 */
  const router = useRouter();

  /**
   * 根據錯誤代碼處理錯誤訊息及後續行為
   * @param errorCode - 後端回傳的錯誤代碼（MbsErrorCodeEnum）
   * @returns 是否成功（true 表示無錯誤，false 表示有錯誤）
   */
  function handleErrorCode(errorCode: MbsErrorCodeEnum): boolean {
    switch (errorCode) {
      case MbsErrorCodeEnum.Success:
        return true;

      case MbsErrorCodeEnum.Fail:
        showError.value = true;
        errorMessage.value = `失敗，請稍後再試！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.AccountOrPasswordError:
        showError.value = true;
        errorMessage.value = `帳號或密碼錯誤！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.LoginTokenError:
        router.push("/login");
        return false;

      case MbsErrorCodeEnum.PermissionDenied:
        showError.value = true;
        errorMessage.value = `您沒有執行此操作的權限！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.AccountDisabled:
        showError.value = true;
        errorMessage.value = `帳號已被停用！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.AccountDuplicate:
        showError.value = true;
        errorMessage.value = `帳號重複，請檢查！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.NameDuplicate:
        showError.value = true;
        errorMessage.value = `名稱重複，請檢查！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.DataAlreadyExists:
        showError.value = true;
        errorMessage.value = `資料已存在！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.FileFormatError:
        showError.value = true;
        errorMessage.value = `附件格式錯誤！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.FileContentError:
        showError.value = true;
        errorMessage.value = `附件內容錯誤！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.ProjectCodeDuplicate:
        showError.value = true;
        errorMessage.value = `專案代碼重複！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.UniformNumberDuplicate:
        showError.value = true;
        errorMessage.value = `統一編號重複！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.PipelineAlreadyTransferred:
        showError.value = true;
        errorMessage.value = `此商機已轉換為專案！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.ContacterEmailDuplicate:
        showError.value = true;
        errorMessage.value = `此公司已存在相同 Email 的窗口！(${errorCode})`;
        return false;

      case MbsErrorCodeEnum.PipelineStageConditionNotMet:
        showError.value = true;
        errorMessage.value = `請先完成此階段所需的條件後再更新！(${errorCode})`;
        return false;

      default:
        showError.value = true;
        errorMessage.value = `發生錯誤！(${errorCode})`;
        return false;
    }
  }

  /**
   * 顯示錯誤訊息
   * @param message - 要顯示的錯誤訊息文字
   */
  function displayError(message: string) {
    errorMessage.value = message;
    showError.value = true;
  }

  /**
   * 關閉錯誤訊息，重置錯誤狀態
   */
  function closeError() {
    showError.value = false;
    errorMessage.value = "";
  }

  return {
    errorMessage,
    showError,
    handleErrorCode,
    displayError,
    closeError,
  };
}
