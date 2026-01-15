<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysEmpHttpGetReqMdl,
  MbsSysEmpHttpGetRspMdl,
  MbsSysEmpHttpUpdateReqMdl,
  MbsSysEmpHttpUpdateReqItemMdl,
  MbsSysEmpHttpUpdateRspMdl,
} from "@/services/pms-http/system/employee/systemEmployeeHttpFormat";
import { getSystemEmployee, updateSystemEmployee } from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import { useRouteParamId } from "@/composables/useRouteParamId";
import type { PermissionItemMdl } from "@/components/feature/permission/permissionModel";
import { initializePermissionList } from "@/utils/initializePermissionList";
import router from "@/router";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyManagerRoleComboBox from "@/components/feature/search-bar/GetManyManagerRoleComboBox.vue";
import PermissionTable from "@/components/feature/permission/PermissionTable.vue";

//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();

//#endregion

//#region 型別定義
/** 系統設定-員工-檢視-頁面模型 */
interface SystemEmployeeDetailViewMdl {
  employeeLoginToken: string | null;
  employeeID: number | null;
  employeeAccount: string | null;
  employeePassword: string | null;
  employeeConfirmPassword: string | null;
  employeeName: string | null;
  managerRoleID: number | null;
  managerRoleName: string | null;
  managerRegionID: number | null;
  managerRegionName: string | null;
  managerDepartmentName: string | null;
  employeeIsEnable: boolean | null;
  employeePermissionList: PermissionItemMdl[];
  employeeAccountIsValid: boolean;
  employeePasswordIsValid: boolean;
  employeeConfirmPasswordIsValid: boolean;
  employeeNameIsValid: boolean;
  managerRoleIdIsValid: boolean;
  employeeIsEnableIsValid: boolean;
  showPassword: boolean;
  showConfirmPassword: boolean;
}
/** 原始資料模型 */
interface OriginalDataMdl {
  employeeAccount: string | null;
  employeePassword: string | null;
  employeeName: string | null;
  managerRoleID: number | null;
  employeeIsEnable: boolean | null;
  employeePermissionList: PermissionItemMdl[];
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemEmployee;
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 目前編輯的ID */
const isEditingId = useRouteParamId();
/** 系統設定-員工-檢視-頁面物件 */
const systemEmployeeDetailViewObj = reactive<SystemEmployeeDetailViewMdl>({
  employeeLoginToken: null,
  employeeID: isEditingId,
  employeeAccount: null,
  employeePassword: null,
  employeeConfirmPassword: null,
  employeeName: null,
  managerRoleID: null,
  managerRoleName: null,
  managerRegionID: null,
  managerRegionName: null,
  managerDepartmentName: null,
  employeeIsEnable: true,
  employeePermissionList: initializePermissionList(),
  employeeAccountIsValid: true,
  employeePasswordIsValid: true,
  employeeConfirmPasswordIsValid: true,
  employeeNameIsValid: true,
  managerRoleIdIsValid: true,
  employeeIsEnableIsValid: true,
  showPassword: false,
  showConfirmPassword: false,
});
/** 原始資料 */
const originalData = reactive<OriginalDataMdl>({
  employeeAccount: null,
  employeePassword: null,
  employeeName: null,
  managerRoleID: null,
  employeeIsEnable: null,
  employeePermissionList: [],
});

//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;

  // 帳號
  if (
    typeof systemEmployeeDetailViewObj.employeeAccount !== "string" ||
    !systemEmployeeDetailViewObj.employeeAccount.trim() ||
    systemEmployeeDetailViewObj.employeeAccount.trim().length > 20
  ) {
    systemEmployeeDetailViewObj.employeeAccountIsValid = false;
    isValid = false;
  } else {
    systemEmployeeDetailViewObj.employeeAccountIsValid = true;
  }

  // 密碼（如果有輸入才驗證）
  if (systemEmployeeDetailViewObj.employeePassword) {
    if (
      typeof systemEmployeeDetailViewObj.employeePassword !== "string" ||
      systemEmployeeDetailViewObj.employeePassword.length < 6 ||
      systemEmployeeDetailViewObj.employeePassword.length > 20
    ) {
      systemEmployeeDetailViewObj.employeePasswordIsValid = false;
      isValid = false;
    } else {
      systemEmployeeDetailViewObj.employeePasswordIsValid = true;
    }

    // 確認密碼
    if (
      systemEmployeeDetailViewObj.employeePassword !==
      systemEmployeeDetailViewObj.employeeConfirmPassword
    ) {
      systemEmployeeDetailViewObj.employeeConfirmPasswordIsValid = false;
      isValid = false;
    } else {
      systemEmployeeDetailViewObj.employeeConfirmPasswordIsValid = true;
    }
  } else {
    systemEmployeeDetailViewObj.employeePasswordIsValid = true;
    systemEmployeeDetailViewObj.employeeConfirmPasswordIsValid = true;
  }

  // 姓名
  if (
    typeof systemEmployeeDetailViewObj.employeeName !== "string" ||
    !systemEmployeeDetailViewObj.employeeName.trim() ||
    systemEmployeeDetailViewObj.employeeName.trim().length > 20
  ) {
    systemEmployeeDetailViewObj.employeeNameIsValid = false;
    isValid = false;
  } else {
    systemEmployeeDetailViewObj.employeeNameIsValid = true;
  }

  // 角色
  if (
    systemEmployeeDetailViewObj.managerRoleID === null ||
    systemEmployeeDetailViewObj.managerRoleID === undefined
  ) {
    systemEmployeeDetailViewObj.managerRoleIdIsValid = false;
    isValid = false;
  } else {
    systemEmployeeDetailViewObj.managerRoleIdIsValid = true;
  }

  // 是否啟用
  if (typeof systemEmployeeDetailViewObj.employeeIsEnable !== "boolean") {
    systemEmployeeDetailViewObj.employeeIsEnableIsValid = false;
    isValid = false;
  } else {
    systemEmployeeDetailViewObj.employeeIsEnableIsValid = true;
  }

  return isValid;
};

//#endregion

//#region API / 資料流程
/** 取得資料 */
const getData = async () => {
  // token 驗證
  if (!requireToken()) return;

  // 送api
  const requestData: MbsSysEmpHttpGetReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeID: systemEmployeeDetailViewObj.employeeID!,
  };
  const responseData: MbsSysEmpHttpGetRspMdl | null = await getSystemEmployee(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定當前值
  systemEmployeeDetailViewObj.employeeAccount = responseData.employeeAccount;
  systemEmployeeDetailViewObj.employeeName = responseData.employeeName;
  systemEmployeeDetailViewObj.managerRoleID = responseData.managerRoleID;
  systemEmployeeDetailViewObj.managerRoleName = responseData.managerRoleName;
  systemEmployeeDetailViewObj.managerRegionID = responseData.managerRegionID;
  systemEmployeeDetailViewObj.managerRegionName = responseData.managerRegionName;
  systemEmployeeDetailViewObj.managerDepartmentName = responseData.managerDepartmentName;
  systemEmployeeDetailViewObj.employeeIsEnable = responseData.employeeIsEnable;
  // 設定權限列表
  if (responseData.employeePermissionList) {
    systemEmployeeDetailViewObj.employeePermissionList = responseData.employeePermissionList;
  }

  // 保存原始值
  originalData.employeeAccount = responseData.employeeAccount;
  originalData.employeePassword = null;
  originalData.employeeName = responseData.employeeName;
  originalData.managerRoleID = responseData.managerRoleID;
  originalData.employeeIsEnable = responseData.employeeIsEnable;
  if (responseData.employeePermissionList) {
    originalData.employeePermissionList = JSON.parse(
      JSON.stringify(responseData.employeePermissionList)
    );
  }
};

/** 取得變更的欄位 */
const getChangedFields = () => {
  const changes: Partial<MbsSysEmpHttpUpdateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    employeeID: systemEmployeeDetailViewObj.employeeID!,
  };

  // 密碼
  if (
    systemEmployeeDetailViewObj.employeePassword &&
    systemEmployeeDetailViewObj.employeePassword !== originalData.employeePassword
  ) {
    changes.employeePassword = systemEmployeeDetailViewObj.employeePassword;
  }

  // 姓名
  const trimmedName = systemEmployeeDetailViewObj.employeeName?.trim();
  if (trimmedName !== originalData.employeeName) {
    changes.employeeName = trimmedName!;
  }

  // 角色
  if (systemEmployeeDetailViewObj.managerRoleID !== originalData.managerRoleID) {
    changes.managerRoleID = systemEmployeeDetailViewObj.managerRoleID!;
  }

  // 狀態
  if (systemEmployeeDetailViewObj.employeeIsEnable !== originalData.employeeIsEnable) {
    changes.employeeIsEnable = systemEmployeeDetailViewObj.employeeIsEnable!;
  }

  // 權限列表
  const currentPermissions = JSON.stringify(systemEmployeeDetailViewObj.employeePermissionList);
  const originalPermissions = JSON.stringify(originalData.employeePermissionList);
  if (currentPermissions !== originalPermissions) {
    changes.employeePermissionList = systemEmployeeDetailViewObj.employeePermissionList.map(
      (item): MbsSysEmpHttpUpdateReqItemMdl => ({
        atomMenu: item.atomMenu,
        managerRegionID: item.managerRegionID,
        atomPermissionKindIdView: item.atomPermissionKindIdView,
        atomPermissionKindIdCreate: item.atomPermissionKindIdCreate,
        atomPermissionKindIdEdit: item.atomPermissionKindIdEdit,
        atomPermissionKindIdDelete: item.atomPermissionKindIdDelete,
      })
    );
  }

  return changes;
};
//#endregion

//#region 按鈕事件
/** 點擊【儲存】按鈕 */
const clickSaveBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 欄位驗證
  if (!validateField()) return;

  // 取得變更的欄位
  const changedFields = getChangedFields();
  // 檢查是否有變更（排除必要的 token 和 ID）
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 呼叫 API 更新資料
  const responseData: MbsSysEmpHttpUpdateRspMdl | null = await updateSystemEmployee(
    changedFields as MbsSysEmpHttpUpdateReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 編輯成功，切換回檢視模式
  isEditMode.value = false;
  // 顯示成功訊息
  displaySuccess("更新員工成功！");
  // 重新載入資料
  getData();
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  isEditMode.value = false;
  // 重新載入資料，恢復到編輯前的狀態
  getData();
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/employee");
};

/** 點擊【編輯】按鈕 */
const clickEditBtn = () => {
  isEditMode.value = true;
};

//#endregion

//#region 生命週期
onMounted(() => {
  getData();
});

//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <!-- 標題區 -->
    <div class="flex items-center justify-between">
      <button class="btn-back flex items-center" @click="clickBackBtn">
        <svg
          class="w-4 h-4 inline-block mr-1"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M15 19l-7-7 7-7"
          />
        </svg>
        <span>返回</span>
      </button>
      <div class="flex items-center gap-2">
        <button
          v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
          class="btn-update"
          @click="clickEditBtn()"
        >
          編輯
        </button>
        <template v-else>
          <div class="flex gap-1">
            <button class="btn-cancel" @click="clickCancelBtn()">取消</button>
            <button class="btn-save" @click="clickSaveBtn()">儲存</button>
          </div>
        </template>
      </div>
    </div>

    <!-- 內容 -->
    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-8 gap-4">
      <div class="flex gap-4 w-full">
        <!-- 帳號 -->
        <div class="flex flex-col flex-1 gap-2">
          <div>
            <label class="form-label">帳號 <span class="required-label">*</span></label>
            <input
              v-model="systemEmployeeDetailViewObj.employeeAccount"
              type="text"
              class="input-box"
              disabled
            />
          </div>
        </div>

        <!-- 電子信箱 -->
        <div class="flex flex-col flex-1 gap-2">
          <div>
            <label class="form-label">電子信箱 <span class="required-label">*</span></label>
            <div class="flex items-center gap-2">
              <input
                v-model="systemEmployeeDetailViewObj.employeeAccount"
                class="input-box"
                type="text"
                placeholder="純顯示文字"
                disabled
              />
              <span>@bccs.com.tw</span>
            </div>
          </div>
        </div>
      </div>

      <div>
        <p class="remind-text">
          ⚠ 提醒 : 如不更改密碼，請留空。若需修改，請輸入新密碼與確認密碼。
        </p>
        <div class="flex gap-4 w-full">
          <!-- 密碼 -->
          <div class="flex-1 relative">
            <label class="form-label">密碼 <span class="required-label">*</span></label>
            <input
              v-if="isEditMode"
              id="password"
              v-model="systemEmployeeDetailViewObj.employeePassword"
              :type="systemEmployeeDetailViewObj.showPassword ? 'text' : 'password'"
              placeholder="請輸入密碼"
              class="input-box"
            />
            <input
              v-else
              id="password"
              v-model="systemEmployeeDetailViewObj.employeePassword"
              :type="systemEmployeeDetailViewObj.showPassword ? 'text' : 'password'"
              placeholder="請輸入密碼"
              class="input-box"
              disabled
            />
            <p class="error-wrapper">
              <span v-if="!systemEmployeeDetailViewObj.employeePasswordIsValid" class="error-tip">
                不可為空，長度需為 6~20 個字，可使用數字、英文大小寫及特殊符號（!@#$%^&*）
              </span>
            </p>
            <!-- 眼睛圖示 -->
            <button
              v-if="isEditMode"
              type="button"
              class="absolute right-2 top-[30px] text-gray-500 hover:text-gray-700"
              @click="
                systemEmployeeDetailViewObj.showPassword = !systemEmployeeDetailViewObj.showPassword
              "
            >
              <svg
                v-if="!systemEmployeeDetailViewObj.showPassword"
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

          <!-- 確認密碼 -->
          <div class="flex-1 relative">
            <label class="form-label">確認密碼 <span class="required-label">*</span></label>
            <input
              v-if="isEditMode"
              id="confirm-password"
              v-model="systemEmployeeDetailViewObj.employeeConfirmPassword"
              :type="systemEmployeeDetailViewObj.showConfirmPassword ? 'text' : 'password'"
              placeholder="請再次輸入密碼"
              class="input-box"
            />
            <input
              v-else
              id="confirm-password"
              v-model="systemEmployeeDetailViewObj.employeeConfirmPassword"
              :type="systemEmployeeDetailViewObj.showConfirmPassword ? 'text' : 'password'"
              placeholder="請再次輸入密碼"
              class="input-box"
              disabled
            />
            <p class="error-wrapper">
              <span
                v-if="!systemEmployeeDetailViewObj.employeeConfirmPasswordIsValid"
                class="error-tip"
              >
                密碼與確認密碼不相符
              </span>
            </p>
            <!-- 眼睛圖示 -->
            <button
              v-if="isEditMode"
              type="button"
              class="absolute right-2 top-[30px] text-gray-500 hover:text-gray-700"
              @click="
                systemEmployeeDetailViewObj.showConfirmPassword =
                  !systemEmployeeDetailViewObj.showConfirmPassword
              "
            >
              <svg
                v-if="!systemEmployeeDetailViewObj.showConfirmPassword"
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
        </div>
      </div>

      <!-- 姓名 -->
      <div>
        <label class="form-label">姓名 <span class="required-label">*</span></label>
        <input
          v-if="isEditMode"
          v-model="systemEmployeeDetailViewObj.employeeName"
          type="text"
          placeholder="請輸入姓名"
          class="input-box"
        />
        <input
          v-else
          v-model="systemEmployeeDetailViewObj.employeeName"
          type="text"
          placeholder="請輸入姓名"
          class="input-box"
          disabled
        />
        <p class="error-wrapper">
          <span v-if="!systemEmployeeDetailViewObj.employeeNameIsValid" class="error-tip">
            不可為空，長度不可超過 20 個字
          </span>
        </p>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 角色 -->
        <div class="flex-1">
          <label class="form-label">角色 <span class="required-label">*</span></label>
          <div class="flex gap-2">
            <GetManyManagerRoleComboBox
              v-model:managerRoleID="systemEmployeeDetailViewObj.managerRoleID"
              v-model:managerRoleName="systemEmployeeDetailViewObj.managerRoleName"
              v-model:managerDepartmentName="systemEmployeeDetailViewObj.managerDepartmentName"
              v-model:managerRegionName="systemEmployeeDetailViewObj.managerRegionName"
              v-model:managerRegionID="systemEmployeeDetailViewObj.managerRegionID"
              :disabled="!isEditMode"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!systemEmployeeDetailViewObj.managerRoleIdIsValid" class="error-tip">
              不可為空
            </span>
          </p>
        </div>

        <!-- 部門 -->
        <div class="flex-1">
          <label class="form-label">部門 <span class="required-label">*</span></label>
          <input
            v-model="systemEmployeeDetailViewObj.managerDepartmentName"
            class="input-box"
            type="text"
            placeholder="純顯示文字"
            disabled
          />
        </div>

        <!-- 地區 -->
        <div class="flex-1">
          <label class="form-label">地區 <span class="required-label">*</span></label>
          <input
            v-model="systemEmployeeDetailViewObj.managerRegionName"
            class="input-box"
            type="text"
            placeholder="純顯示文字"
            disabled
          />
        </div>
      </div>

      <!-- 狀態 -->
      <div class="w-48">
        <label class="form-label">狀態 <span class="required-label">*</span></label>
        <select
          v-if="isEditMode"
          v-model="systemEmployeeDetailViewObj.employeeIsEnable"
          class="select-box"
        >
          <option :value="true">啟用</option>
          <option :value="false">停用</option>
        </select>
        <select
          v-else
          v-model="systemEmployeeDetailViewObj.employeeIsEnable"
          class="select-box"
          disabled
        >
          <option :value="true">啟用</option>
          <option :value="false">停用</option>
        </select>
        <p class="error-wrapper">
          <span v-if="!systemEmployeeDetailViewObj.employeeIsEnableIsValid" class="error-tip">
            不可為空
          </span>
        </p>
      </div>

      <!-- 權限 -->
      <div>
        <label class="form-label">員工權限 <span class="required-label">*</span></label>
        <PermissionTable
          v-model:permissionList="systemEmployeeDetailViewObj.employeePermissionList"
          v-model:managerRegionID="systemEmployeeDetailViewObj.managerRegionID"
          :readonly="!isEditMode"
        />
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
