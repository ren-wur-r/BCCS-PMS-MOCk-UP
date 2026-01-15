<script setup lang="ts">
//#region 引入
import { reactive } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysRolHttpAddReqMdl,
  MbsSysRolHttpAddReqItemMdl,
  MbsSysRolHttpAddRspMdl,
} from "@/services/pms-http/system/role/systemRoleHttpFormat";
import { addSystemRole } from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import type { PermissionItemMdl } from "@/components/feature/permission/permissionModel";
import { initializePermissionList } from "@/utils/initializePermissionList";
import router from "@/router";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyManagerRegionComboBox from "@/components/feature/search-bar/GetManyManagerRegionComboBox.vue";
import GetManyManagerDepartmentComboBox from "@/components/feature/search-bar/GetManyManagerDepartmentComboBox.vue";
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
/** 系統設定-角色-新增-頁面模型 */
interface SystemRoleAddViewMdl {
  employeeLoginToken: string | null;
  managerRoleName: string | null;
  managerRegionID: number | null;
  managerRegionName: string | null;
  managerDepartmentID: number | null;
  managerDepartmentName: string | null;
  managerRoleRemark: string | null;
  managerRoleIsEnable: boolean | null;
  managerRolePermissionList: PermissionItemMdl[];
  managerRoleNameIsValid: boolean;
  managerRoleIsEnableIsValid: boolean;
  managerRegionIdIsValid: boolean;
  managerDepartmentIdIsValid: boolean;
  managerRoleRemarkIsValid: boolean;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemRole;
/** 系統設定-角色-新增-頁面物件 */
const mbsSystemRoleAddViewObj = reactive<SystemRoleAddViewMdl>({
  employeeLoginToken: null,
  managerRoleName: null,
  managerRegionID: null,
  managerRegionName: null,
  managerDepartmentID: null,
  managerDepartmentName: null,
  managerRoleRemark: null,
  managerRoleIsEnable: true,
  managerRolePermissionList: initializePermissionList(),
  managerRoleNameIsValid: true,
  managerRoleIsEnableIsValid: true,
  managerRegionIdIsValid: true,
  managerDepartmentIdIsValid: true,
  managerRoleRemarkIsValid: true,
});

//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;
  // 名稱
  if (
    typeof mbsSystemRoleAddViewObj.managerRoleName !== "string" ||
    !mbsSystemRoleAddViewObj.managerRoleName.trim() ||
    mbsSystemRoleAddViewObj.managerRoleName.trim().length > 20
  ) {
    mbsSystemRoleAddViewObj.managerRoleNameIsValid = false;
    isValid = false;
  } else {
    mbsSystemRoleAddViewObj.managerRoleNameIsValid = true;
  }

  // 地區
  if (
    mbsSystemRoleAddViewObj.managerRegionID === null ||
    mbsSystemRoleAddViewObj.managerRegionID === undefined
  ) {
    mbsSystemRoleAddViewObj.managerRegionIdIsValid = false;
    isValid = false;
  } else {
    mbsSystemRoleAddViewObj.managerRegionIdIsValid = true;
  }

  // 部門
  if (
    mbsSystemRoleAddViewObj.managerDepartmentID === null ||
    mbsSystemRoleAddViewObj.managerDepartmentID === undefined
  ) {
    mbsSystemRoleAddViewObj.managerDepartmentIdIsValid = false;
    isValid = false;
  } else {
    mbsSystemRoleAddViewObj.managerDepartmentIdIsValid = true;
  }

  // 備註
  if (
    mbsSystemRoleAddViewObj.managerRoleRemark &&
    mbsSystemRoleAddViewObj.managerRoleRemark.trim().length > 20
  ) {
    mbsSystemRoleAddViewObj.managerRoleRemarkIsValid = false;
    isValid = false;
  } else {
    mbsSystemRoleAddViewObj.managerRoleRemarkIsValid = true;
  }

  // 是否啟用
  if (typeof mbsSystemRoleAddViewObj.managerRoleIsEnable !== "boolean") {
    mbsSystemRoleAddViewObj.managerRoleIsEnableIsValid = false;
    isValid = false;
  } else {
    mbsSystemRoleAddViewObj.managerRoleIsEnableIsValid = true;
  }

  return isValid;
};

//#endregion

//#region API / 資料流程
/** 送出資料 */
const SubmitData = async () => {
  // Token驗證
  if (!requireToken()) return;
  // 欄位驗證
  if (!validateField()) return;
  // 送api
  const requestData: MbsSysRolHttpAddReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerRoleName: mbsSystemRoleAddViewObj.managerRoleName!,
    managerRoleIsEnable: mbsSystemRoleAddViewObj.managerRoleIsEnable!,
    managerRoleRemark: mbsSystemRoleAddViewObj.managerRoleRemark,
    managerRegionID: mbsSystemRoleAddViewObj.managerRegionID!,
    managerDepartmentID: mbsSystemRoleAddViewObj.managerDepartmentID!,
    managerRolePermissionList: mbsSystemRoleAddViewObj.managerRolePermissionList.map(
      (item): MbsSysRolHttpAddReqItemMdl => ({
        atomMenu: item.atomMenu,
        managerRegionID: item.managerRegionID!,
        atomPermissionKindIdView: item.atomPermissionKindIdView!,
        atomPermissionKindIdCreate: item.atomPermissionKindIdCreate!,
        atomPermissionKindIdEdit: item.atomPermissionKindIdEdit!,
        atomPermissionKindIdDelete: item.atomPermissionKindIdDelete!,
      })
    ),
  };
  const responseData: MbsSysRolHttpAddRspMdl | null = await addSystemRole(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);

  if (isSuccess && responseData.managerRoleID) {
    // 顯示成功訊息
    displaySuccess("新增角色成功！");
    // 導向角色詳細頁
    setTimeout(() => {
      router.push(`/system/role/detail/${responseData.managerRoleID}`);
    }, 1500);
  }
};

//#endregion

//#region 按鈕事件
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  await SubmitData();
};
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/role");
};
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <!-- 標題區 -->
    <div class="flex items-center justify-between font-bold">
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
      <!-- 角色名稱 -->
      <div>
        <label class="form-label">角色名稱 <span class="required-label">*</span></label>
        <input
          v-model="mbsSystemRoleAddViewObj.managerRoleName"
          class="input-box"
          type="text"
          placeholder="請輸入角色名稱"
        />
        <p class="error-wrapper">
          <span v-if="!mbsSystemRoleAddViewObj.managerRoleNameIsValid" class="error-tip">
            不可為空，長度不可超過 20 個字
          </span>
        </p>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 地區 -->
        <div class="flex-1">
          <label class="form-label">所屬地區 <span class="required-label">*</span></label>
          <div class="flex gap-2">
            <GetManyManagerRegionComboBox
              v-model:managerRegionID="mbsSystemRoleAddViewObj.managerRegionID"
              v-model:managerRegionName="mbsSystemRoleAddViewObj.managerRegionName"
              :disabled="false"
            />
          </div>
          <div class="error-wrapper">
            <p v-if="!mbsSystemRoleAddViewObj.managerRegionIdIsValid" class="error-tip">不可為空</p>
          </div>
        </div>

        <!-- 部門 -->
        <div class="flex-1">
          <label class="form-label">所屬部門 <span class="required-label">*</span></label>
          <div class="flex gap-2">
            <GetManyManagerDepartmentComboBox
              v-model:managerDepartmentID="mbsSystemRoleAddViewObj.managerDepartmentID"
              v-model:managerDepartmentName="mbsSystemRoleAddViewObj.managerDepartmentName"
              :disabled="false"
            />
          </div>
          <div class="error-wrapper">
            <p v-if="!mbsSystemRoleAddViewObj.managerDepartmentIdIsValid" class="error-tip">
              不可為空
            </p>
          </div>
        </div>
      </div>

      <!-- 備註 -->
      <div>
        <label class="form-label">備註</label>
        <input
          v-model="mbsSystemRoleAddViewObj.managerRoleRemark"
          class="input-box"
          type="text"
          placeholder="請輸入備註"
        />
        <div class="error-wrapper">
          <p v-if="!mbsSystemRoleAddViewObj.managerRoleRemarkIsValid" class="error-tip">
            長度不可超過 20 個字
          </p>
        </div>
      </div>

      <!-- 狀態 -->
      <div class="w-48">
        <label class="form-label">狀態 <span class="required-label">*</span></label>
        <select v-model="mbsSystemRoleAddViewObj.managerRoleIsEnable" class="select-box">
          <option :value="true" selected>啟用</option>
          <option :value="false">停用</option>
        </select>
        <div class="error-wrapper">
          <p v-if="!mbsSystemRoleAddViewObj.managerRoleIsEnableIsValid" class="error-tip">
            不可為空
          </p>
        </div>
      </div>

      <!-- 權限 -->
      <div>
        <label class="form-label">角色權限 <span class="required-label">*</span></label>
        <PermissionTable
          v-model:permissionList="mbsSystemRoleAddViewObj.managerRolePermissionList"
          v-model:managerRegionID="mbsSystemRoleAddViewObj.managerRegionID"
          v-model:managerRegionName="mbsSystemRoleAddViewObj.managerRegionName"
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
