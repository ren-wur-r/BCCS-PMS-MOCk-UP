<script setup lang="ts">
//#region 引入
import { reactive, watch, ref } from "vue";
//#endregion

//#region Props / Emits
interface Props {
  /** 是否顯示彈跳視窗 */
  isVisible: boolean;
}

const props = defineProps<Props>();

interface Emits {
  /** 關閉 */
  (e: "close"): void;
  /** 確認送出 */
  (
    e: "confirm",
    data: {
      oldPassword: string;
      newPassword: string;
    }
  ): void;
}

const emit = defineEmits<Emits>();

//#endregion

//#region 頁面物件
/** 修改密碼彈跳視窗-頁面模型 */
interface ChangePasswordModalMdl {
  oldPassword: string | null;
  newPassword: string | null;
  confirmPassword: string | null;
  oldPasswordIsValid: boolean;
  newPasswordIsValid: boolean;
  newPasswordLengthIsValid: boolean;
  confirmPasswordIsValid: boolean;
  passwordMatchIsValid: boolean;
}

/** 修改密碼彈跳視窗-頁面物件 */
const changePasswordModalObj = reactive<ChangePasswordModalMdl>({
  oldPassword: null,
  newPassword: null,
  confirmPassword: null,
  oldPasswordIsValid: true,
  newPasswordIsValid: true,
  newPasswordLengthIsValid: true,
  confirmPasswordIsValid: true,
  passwordMatchIsValid: true,
});

// 密碼顯示切換
const showNewPassword = ref(false);
const showConfirmPassword = ref(false);

//#endregion

//#region UI行為 / 畫面處理
/** 重置表單 */
const resetForm = () => {
  changePasswordModalObj.oldPassword = null;
  changePasswordModalObj.newPassword = null;
  changePasswordModalObj.confirmPassword = null;
  changePasswordModalObj.oldPasswordIsValid = true;
  changePasswordModalObj.newPasswordIsValid = true;
  changePasswordModalObj.newPasswordLengthIsValid = true;
  changePasswordModalObj.confirmPasswordIsValid = true;
  changePasswordModalObj.passwordMatchIsValid = true;
  // 重置密碼顯示狀態
  showNewPassword.value = false;
  showConfirmPassword.value = false;
};

//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;

  // 舊密碼
  if (
    typeof changePasswordModalObj.oldPassword !== "string" ||
    !changePasswordModalObj.oldPassword.trim()
  ) {
    changePasswordModalObj.oldPasswordIsValid = false;
    isValid = false;
  } else {
    changePasswordModalObj.oldPasswordIsValid = true;
  }

  // 新密碼
  if (
    typeof changePasswordModalObj.newPassword !== "string" ||
    !changePasswordModalObj.newPassword.trim()
  ) {
    changePasswordModalObj.newPasswordIsValid = false;
    isValid = false;
  } else {
    changePasswordModalObj.newPasswordIsValid = true;
  }

  // 新密碼長度 (至少6個字元)
  if (changePasswordModalObj.newPassword && changePasswordModalObj.newPassword.trim().length < 6) {
    changePasswordModalObj.newPasswordLengthIsValid = false;
    isValid = false;
  } else {
    changePasswordModalObj.newPasswordLengthIsValid = true;
  }

  // 確認新密碼
  if (
    typeof changePasswordModalObj.confirmPassword !== "string" ||
    !changePasswordModalObj.confirmPassword.trim()
  ) {
    changePasswordModalObj.confirmPasswordIsValid = false;
    isValid = false;
  } else {
    changePasswordModalObj.confirmPasswordIsValid = true;
  }

  // 密碼一致性
  if (
    changePasswordModalObj.newPassword &&
    changePasswordModalObj.confirmPassword &&
    changePasswordModalObj.newPassword !== changePasswordModalObj.confirmPassword
  ) {
    changePasswordModalObj.passwordMatchIsValid = false;
    isValid = false;
  } else {
    changePasswordModalObj.passwordMatchIsValid = true;
  }

  return isValid;
};

//#endregion

//#region 按鈕事件
/** 點擊【送出】按鈕 */
const handleSubmit = () => {
  // 驗證欄位
  if (!validateField()) return;

  // 發送確認事件給父組件
  emit("confirm", {
    oldPassword: changePasswordModalObj.oldPassword!.trim(),
    newPassword: changePasswordModalObj.newPassword!.trim(),
  });
};

/** 點擊【取消】按鈕 */
const handleCancel = () => {
  emit("close");
};

//#endregion

//#region 監聽器
/** 監聽 isVisible 變化 */
watch(
  () => props.isVisible,
  (newShow) => {
    if (newShow) {
      resetForm();
    }
  },
  { immediate: true }
);

//#endregion

//-------------------------------------------------------------
</script>

<template>
  <div v-if="isVisible" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">修改密碼</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 舊密碼 -->
          <div>
            <label class="form-label">舊密碼 <span class="required-label">*</span></label>
            <input
              v-model="changePasswordModalObj.oldPassword"
              class="input-box"
              type="password"
              placeholder="請輸入舊密碼"
            />
            <p class="error-wrapper">
              <span v-if="!changePasswordModalObj.oldPasswordIsValid" class="error-tip">
                不可為空
              </span>
            </p>
          </div>

          <!-- 新密碼 -->
          <div>
            <label class="form-label">新密碼 <span class="required-label">*</span></label>
            <div class="relative">
              <input
                v-model="changePasswordModalObj.newPassword"
                :type="showNewPassword ? 'text' : 'password'"
                class="input-box pr-10"
                placeholder="請輸入新密碼"
              />

              <button
                type="button"
                class="absolute right-2 top-1.5 text-gray-500 hover:text-gray-700"
                aria-label="切換顯示新密碼"
                @click="showNewPassword = !showNewPassword"
              >
                <svg
                  v-if="!showNewPassword"
                  class="w-5 h-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                  stroke="currentColor"
                >
                  <path
                    d="M3.275 15.296C2.425 14.192 2 13.639 2 12C2 10.36 2.425 9.809 3.275 8.704C4.972 6.5 7.818 4 12 4C16.182 4 19.028 6.5 20.725 8.704C21.575 9.81 22 10.361 22 12C22 13.64 21.575 14.191 20.725 15.296C19.028 17.5 16.182 20 12 20C7.818 20 4.972 17.5 3.275 15.296Z"
                    stroke-width="1.5"
                  />
                  <path
                    d="M15 12C15 12.7956 14.6839 13.5587 14.1213 14.1213C13.5587 14.6839 12.7956 15 12 15C11.2044 15 10.4413 14.6839 9.87868 14.1213C9.31607 13.5587 9 12.7956 9 12C9 11.2044 9.31607 10.4413 9.87868 9.87868C10.4413 9.31607 11.2044 9 12 9C12.7956 9 13.5587 9.31607 14.1213 9.87868C14.6839 10.4413 15 11.2044 15 12Z"
                    stroke-width="1.5"
                  />
                </svg>

                <svg
                  v-else
                  class="w-5 h-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M9.75 12C9.75 11.4033 9.98705 10.831 10.409 10.409C10.831 9.98705 11.4033 9.75 12 9.75C12.5967 9.75 13.169 9.98705 13.591 10.409C14.0129 10.831 14.25 11.4033 14.25 12C14.25 12.5967 14.0129 13.169 13.591 13.591C13.169 14.0129 12.5967 14.25 12 14.25C11.4033 14.25 10.831 14.0129 10.409 13.591C9.98705 13.169 9.75 12.5967 9.75 12Z"
                    fill="#6A7282"
                  />
                  <path
                    fill-rule="evenodd"
                    clip-rule="evenodd"
                    d="M2 12C2 13.64 2.425 14.191 3.275 15.296C4.972 17.5 7.818 20 12 20C16.182 20 19.028 17.5 20.725 15.296C21.575 14.192 22 13.639 22 12C22 10.36 21.575 9.809 20.725 8.704C19.028 6.5 16.182 4 12 4C7.818 4 4.972 6.5 3.275 8.704C2.425 9.81 2 10.361 2 12ZM12 8.25C11.0054 8.25 10.0516 8.64509 9.34835 9.34835C8.64509 10.0516 8.25 11.0054 8.25 12C8.25 12.9946 8.64509 13.9484 9.34835 14.6517C10.0516 15.3549 11.0054 15.75 12 15.75C12.9946 15.75 13.9484 15.3549 14.6517 14.6517C15.3549 13.9484 15.75 12.9946 15.75 12C15.75 11.0054 15.3549 10.0516 14.6517 9.34835C13.9484 8.64509 12.9946 8.25 12 8.25Z"
                    fill="#6A7282"
                  />
                </svg>
              </button>
            </div>
            <p class="error-wrapper">
              <span v-if="!changePasswordModalObj.newPasswordIsValid" class="error-tip">
                不可為空
              </span>
              <span v-if="!changePasswordModalObj.newPasswordLengthIsValid" class="error-tip">
                密碼長度至少需要 6 個字元
              </span>
            </p>
          </div>

          <!-- 確認新密碼 -->
          <div>
            <label class="form-label">確認新密碼 <span class="required-label">*</span></label>
            <div class="relative">
              <input
                v-model="changePasswordModalObj.confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                class="input-box pr-10"
                placeholder="請再次輸入新密碼"
              />

              <button
                type="button"
                class="absolute right-2 top-1.5 text-gray-500 hover:text-gray-700"
                aria-label="切換顯示確認新密碼"
                @click="showConfirmPassword = !showConfirmPassword"
              >
                <svg
                  v-if="!showConfirmPassword"
                  class="w-5 h-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                  stroke="currentColor"
                >
                  <path
                    d="M3.275 15.296C2.425 14.192 2 13.639 2 12C2 10.36 2.425 9.809 3.275 8.704C4.972 6.5 7.818 4 12 4C16.182 4 19.028 6.5 20.725 8.704C21.575 9.81 22 10.361 22 12C22 13.64 21.575 14.191 20.725 15.296C19.028 17.5 16.182 20 12 20C7.818 20 4.972 17.5 3.275 15.296Z"
                    stroke-width="1.5"
                  />
                  <path
                    d="M15 12C15 12.7956 14.6839 13.5587 14.1213 14.1213C13.5587 14.6839 12.7956 15 12 15C11.2044 15 10.4413 14.6839 9.87868 14.1213C9.31607 13.5587 9 12.7956 9 12C9 11.2044 9.31607 10.4413 9.87868 9.87868C10.4413 9.31607 11.2044 9 12 9C12.7956 9 13.5587 9.31607 14.1213 9.87868C14.6839 10.4413 15 11.2044 15 12Z"
                    stroke-width="1.5"
                  />
                </svg>

                <svg
                  v-else
                  class="w-5 h-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M9.75 12C9.75 11.4033 9.98705 10.831 10.409 10.409C10.831 9.98705 11.4033 9.75 12 9.75C12.5967 9.75 13.169 9.98705 13.591 10.409C14.0129 10.831 14.25 11.4033 14.25 12C14.25 12.5967 14.0129 13.169 13.591 13.591C13.169 14.0129 12.5967 14.25 12 14.25C11.4033 14.25 10.831 14.0129 10.409 13.591C9.98705 13.169 9.75 12.5967 9.75 12Z"
                    fill="#6A7282"
                  />
                  <path
                    fill-rule="evenodd"
                    clip-rule="evenodd"
                    d="M2 12C2 13.64 2.425 14.191 3.275 15.296C4.972 17.5 7.818 20 12 20C16.182 20 19.028 17.5 20.725 15.296C21.575 14.192 22 13.639 22 12C22 10.36 21.575 9.809 20.725 8.704C19.028 6.5 16.182 4 12 4C7.818 4 4.972 6.5 3.275 8.704C2.425 9.81 2 10.361 2 12ZM12 8.25C11.0054 8.25 10.0516 8.64509 9.34835 9.34835C8.64509 10.0516 8.25 11.0054 8.25 12C8.25 12.9946 8.64509 13.9484 9.34835 14.6517C10.0516 15.3549 11.0054 15.75 12 15.75C12.9946 15.75 13.9484 15.3549 14.6517 14.6517C15.3549 13.9484 15.75 12.9946 15.75 12C15.75 11.0054 15.3549 10.0516 14.6517 9.34835C13.9484 8.64509 12.9946 8.25 12 8.25Z"
                    fill="#6A7282"
                  />
                </svg>
              </button>
            </div>
            <p class="error-wrapper">
              <span v-if="!changePasswordModalObj.confirmPasswordIsValid" class="error-tip">
                不可為空
              </span>
              <span v-if="!changePasswordModalObj.passwordMatchIsValid" class="error-tip">
                新密碼與確認密碼不一致
              </span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleSubmit">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}
</style>
