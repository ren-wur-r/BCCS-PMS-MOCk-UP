<script setup lang="ts">
//#region 引入
import { reactive, watch } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysEmpHttpAddReqMdl,
  MbsSysEmpHttpAddReqItemMdl,
  MbsSysEmpHttpAddRspMdl,
} from "@/services/pms-http/system/employee/systemEmployeeHttpFormat";
import { addSystemEmployee } from "@/services/index";
import { getManyBasicRolePermissionFromRoleId } from "@/services/pms-http/basic/basicHttpService";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
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
/** 系統設定-員工-新增-頁面模型 */
interface SystemEmployeeAddViewMdl {
  employeeLoginToken: string | null;
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
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemEmployee;
/** 系統設定-員工-新增-頁面物件 */
const systemEmployeeAddViewObj = reactive<SystemEmployeeAddViewMdl>({
  employeeLoginToken: null,
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

//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;
  //  帳號
  if (
    typeof systemEmployeeAddViewObj.employeeAccount !== "string" ||
    !systemEmployeeAddViewObj.employeeAccount.trim() ||
    systemEmployeeAddViewObj.employeeAccount.trim().length > 20
  ) {
    systemEmployeeAddViewObj.employeeAccountIsValid = false;
    isValid = false;
  } else {
    systemEmployeeAddViewObj.employeeAccountIsValid = true;
  }

  // 密碼
  if (
    typeof systemEmployeeAddViewObj.employeePassword !== "string" ||
    !systemEmployeeAddViewObj.employeePassword.trim()
  ) {
    systemEmployeeAddViewObj.employeePasswordIsValid = false;
    isValid = false;
  } else {
    systemEmployeeAddViewObj.employeePasswordIsValid = true;
  }

  // 確認密碼
  if (
    systemEmployeeAddViewObj.employeePassword !== systemEmployeeAddViewObj.employeeConfirmPassword
  ) {
    systemEmployeeAddViewObj.employeeConfirmPasswordIsValid = false;
    isValid = false;
  } else {
    systemEmployeeAddViewObj.employeeConfirmPasswordIsValid = true;
  }

  // 名稱
  if (
    typeof systemEmployeeAddViewObj.employeeName !== "string" ||
    !systemEmployeeAddViewObj.employeeName.trim() ||
    systemEmployeeAddViewObj.employeeName.trim().length > 20
  ) {
    systemEmployeeAddViewObj.employeeNameIsValid = false;
    isValid = false;
  } else {
    systemEmployeeAddViewObj.employeeNameIsValid = true;
  }

  // 角色
  if (
    systemEmployeeAddViewObj.managerRoleID === null ||
    systemEmployeeAddViewObj.managerRoleID === undefined
  ) {
    systemEmployeeAddViewObj.managerRoleIdIsValid = false;
    isValid = false;
  } else {
    systemEmployeeAddViewObj.managerRoleIdIsValid = true;
  }

  // 是否啟用
  if (typeof systemEmployeeAddViewObj.employeeIsEnable !== "boolean") {
    systemEmployeeAddViewObj.employeeIsEnableIsValid = false;
    isValid = false;
  } else {
    systemEmployeeAddViewObj.employeeIsEnableIsValid = true;
  }

  return isValid;
};
//#endregion

//#region API / 資料流程
/** 載入角色權限資料 */
const getRolePermissionList = async (roleId: number) => {
  // token 驗證
  if (!requireToken()) return;

  // 取得角色權限資料
  const responseData = await getManyBasicRolePermissionFromRoleId({
    employeeLoginToken: tokenStore.token!,
    managerRoleID: roleId,
  });
  if (!responseData) {
    displayError("系統錯誤，無法取得角色權限資料 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 將 API 回傳的角色權限資料轉換為 PermissionItemMdl 格式
  if (responseData.rolePermissionList && responseData.rolePermissionList.length > 0) {
    const permissionList: PermissionItemMdl[] = responseData.rolePermissionList.map(
      (permission) => {
        return {
          atomMenu: permission.atomMenu,
          managerRegionID: permission.managerRegionID,
          atomPermissionKindIdView: permission.atomPermissionKindIdView,
          atomPermissionKindIdCreate: permission.atomPermissionKindIdCreate,
          atomPermissionKindIdEdit: permission.atomPermissionKindIdEdit,
          atomPermissionKindIdDelete: permission.atomPermissionKindIdDelete,
        };
      }
    );

    // 更新權限列表
    systemEmployeeAddViewObj.employeePermissionList = permissionList;
  } else {
    // 如果沒有權限資料，使用預設的空權限列表
    systemEmployeeAddViewObj.employeePermissionList = initializePermissionList();
  }
};

//#endregion

//#region 按鈕事件
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 欄位驗證
  if (!validateField()) return;

  // 送api
  const requestData: MbsSysEmpHttpAddReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeAccount: systemEmployeeAddViewObj.employeeAccount!,
    employeePassword: systemEmployeeAddViewObj.employeePassword!,
    employeeName: systemEmployeeAddViewObj.employeeName!,
    managerRoleID: systemEmployeeAddViewObj.managerRoleID!,
    employeeIsEnable: systemEmployeeAddViewObj.employeeIsEnable!,
    employeePermissionList: systemEmployeeAddViewObj.employeePermissionList.map(
      (item): MbsSysEmpHttpAddReqItemMdl => ({
        atomMenu: item.atomMenu,
        managerRegionID: item.managerRegionID!,
        atomPermissionKindIdView: item.atomPermissionKindIdView!,
        atomPermissionKindIdCreate: item.atomPermissionKindIdCreate!,
        atomPermissionKindIdEdit: item.atomPermissionKindIdEdit!,
        atomPermissionKindIdDelete: item.atomPermissionKindIdDelete!,
      })
    ),
  };
  const responseData: MbsSysEmpHttpAddRspMdl | null = await addSystemEmployee(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增員工成功！");
  // 導向員工詳細頁
  setTimeout(() => {
    router.push(`/system/employee/detail/${responseData.employeeID}`);
  }, 1500);
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/employee");
};

//#endregion

//#region 監聽器
/** 監聽角色ID變化，自動載入角色權限 */
watch(
  () => systemEmployeeAddViewObj.managerRoleID,
  async (newRoleId, oldRoleId) => {
    // 只有當新的角色ID存在且與舊的不同時才執行
    if (newRoleId && newRoleId !== oldRoleId) {
      await getRolePermissionList(newRoleId);
    } else if (!newRoleId) {
      // 如果角色ID被清空，重置權限列表
      systemEmployeeAddViewObj.employeePermissionList = initializePermissionList();
    }
  },
  { immediate: false } // 不要在初始化時立即執行
);
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
    </div>

    <!-- 內容 -->
    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-8 gap-4">
      <div class="flex gap-4 w-full">
        <!-- 帳號 -->
        <div class="flex-1">
          <div>
            <label class="form-label">帳號 <span class="required-label">*</span></label>
            <input
              v-model="systemEmployeeAddViewObj.employeeAccount"
              class="input-box"
              type="text"
              placeholder="請輸入帳號"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!systemEmployeeAddViewObj.employeeAccountIsValid" class="error-tip">
              不可為空且不可超過 20 個字
            </span>
          </p>
        </div>

        <!-- 電子信箱 -->
        <div class="flex-1">
          <div>
            <label class="form-label">電子信箱 <span class="required-label">*</span></label>
            <div class="flex items-center gap-2">
              <input
                v-model="systemEmployeeAddViewObj.employeeAccount"
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

      <div class="flex gap-4 w-full">
        <!-- 密碼 -->
        <div class="flex-1 relative">
          <label class="form-label">密碼 <span class="required-label">*</span></label>
          <input
            id="password"
            v-model="systemEmployeeAddViewObj.employeePassword"
            :type="systemEmployeeAddViewObj.showPassword ? 'text' : 'password'"
            placeholder="請輸入密碼"
            class="input-box"
          />
          <p class="error-wrapper">
            <span v-if="!systemEmployeeAddViewObj.employeePasswordIsValid" class="error-tip">
              不可為空，長度需為 6~20 個字，可使用數字、英文大小寫及特殊符號（!@#$%^&*）
            </span>
          </p>
          <!-- 眼睛圖示 -->
          <button
            type="button"
            class="absolute right-2 top-[30px] text-gray-500 hover:text-gray-700"
            @click="systemEmployeeAddViewObj.showPassword = !systemEmployeeAddViewObj.showPassword"
          >
            <svg
              v-if="!systemEmployeeAddViewObj.showPassword"
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
            id="confirm-password"
            v-model="systemEmployeeAddViewObj.employeeConfirmPassword"
            :type="systemEmployeeAddViewObj.showConfirmPassword ? 'text' : 'password'"
            placeholder="請再次輸入密碼"
            class="input-box"
          />
          <p class="error-wrapper">
            <span v-if="!systemEmployeeAddViewObj.employeeConfirmPasswordIsValid" class="error-tip">
              密碼與確認密碼不相符
            </span>
          </p>
          <!-- 眼睛圖示 -->
          <button
            type="button"
            class="absolute right-2 top-[30px] text-gray-500 hover:text-gray-700"
            @click="
              systemEmployeeAddViewObj.showConfirmPassword =
                !systemEmployeeAddViewObj.showConfirmPassword
            "
          >
            <svg
              v-if="!systemEmployeeAddViewObj.showConfirmPassword"
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

      <!-- 姓名 -->
      <div>
        <label class="form-label">姓名 <span class="required-label">*</span></label>
        <input
          v-model="systemEmployeeAddViewObj.employeeName"
          class="input-box"
          type="text"
          placeholder="請輸入姓名"
        />
        <p class="error-wrapper">
          <span v-if="!systemEmployeeAddViewObj.employeeNameIsValid" class="error-tip">
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
              v-model:managerRoleID="systemEmployeeAddViewObj.managerRoleID"
              v-model:managerRoleName="systemEmployeeAddViewObj.managerRoleName"
              v-model:managerDepartmentName="systemEmployeeAddViewObj.managerDepartmentName"
              v-model:managerRegionID="systemEmployeeAddViewObj.managerRegionID"
              v-model:managerRegionName="systemEmployeeAddViewObj.managerRegionName"
              :disabled="false"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!systemEmployeeAddViewObj.managerRoleIdIsValid" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>

        <!-- 部門 -->
        <div class="flex flex-col gap-2 flex-1">
          <label class="form-label">部門 <span class="required-label">*</span></label>
          <input
            v-model="systemEmployeeAddViewObj.managerDepartmentName"
            class="input-box"
            type="text"
            placeholder="純顯示文字"
            disabled
          />
        </div>

        <!-- 地區 -->
        <div class="flex flex-col gap-2 flex-1">
          <label class="form-label">地區 <span class="required-label">*</span></label>
          <input
            v-model="systemEmployeeAddViewObj.managerRegionName"
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
        <select v-model="systemEmployeeAddViewObj.employeeIsEnable" class="select-box">
          <option :value="true" selected>啟用</option>
          <option :value="false">停用</option>
        </select>
        <p class="error-wrapper">
          <span v-if="!systemEmployeeAddViewObj.employeeIsEnableIsValid" class="error-tip"
            >不可為空</span
          >
        </p>
      </div>

      <!-- 權限 -->
      <div class="mb-4">
        <label class="form-label">員工權限 <span class="required-label">*</span></label>
        <PermissionTable
          v-model:permissionList="systemEmployeeAddViewObj.employeePermissionList"
          v-model:managerRegionID="systemEmployeeAddViewObj.managerRegionID"
          :readonly="false"
        />
      </div>
    </div>

    <div class="flex justify-center">
      <button
        v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="btn-submit"
        @click="clickSubmitBtn"
      >
        送出
      </button>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
