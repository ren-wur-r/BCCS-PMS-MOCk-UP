<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysRolHttpGetReqMdl,
  MbsSysRolHttpGetRspMdl,
  MbsSysRolHttpUpdateReqMdl,
  MbsSysRolHttpAddReqItemMdl,
  MbsSysRolHttpUpdateRspMdl,
} from "@/services/pms-http/system/role/systemRoleHttpFormat";
import { getSystemRole, updateSystemRole } from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import { useRouteParamId } from "@/composables/useRouteParamId";
import type { PermissionItemMdl } from "@/components/feature/permission/permissionModel";
import { initializePermissionList } from "@/utils/initializePermissionList";
import router from "@/router";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ConfirmDialog from "@/components/global/feedback/ConfirmDialog.vue";
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
/** 系統設定-角色-檢視-頁面模型 */
interface SystemRoleDetailViewMdl {
  employeeLoginToken: string | null;
  managerRoleID: number | null;
  managerRoleName: string | null;
  employeeCount: number | null;
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
/** 原始資料模型 */
interface OriginalDataMdl {
  managerRoleName: string | null;
  managerRegionID: number | null;
  managerDepartmentID: number | null;
  managerRoleRemark: string | null;
  managerRoleIsEnable: boolean | null;
  managerRolePermissionList: PermissionItemMdl[];
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemRole;
/** 是否顯示確認對話框 */
const showConfirmDialog = ref(false);
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 目前編輯的ID */
const isEditingId = useRouteParamId();
/** 系統設定-角色-檢視-頁面物件 */
const systemRoleDetailViewObj = reactive<SystemRoleDetailViewMdl>({
  employeeLoginToken: null,
  managerRoleID: isEditingId,
  managerRoleName: null,
  employeeCount: 0,
  managerRegionID: null,
  managerRegionName: null,
  managerDepartmentID: null,
  managerDepartmentName: null,
  managerRoleRemark: null,
  managerRoleIsEnable: null,
  managerRolePermissionList: initializePermissionList(),
  managerRoleNameIsValid: true,
  managerRoleIsEnableIsValid: true,
  managerRegionIdIsValid: true,
  managerDepartmentIdIsValid: true,
  managerRoleRemarkIsValid: true,
});
/** 原始資料 */
const originalData = reactive<OriginalDataMdl>({
  managerRoleName: null,
  managerRegionID: null,
  managerDepartmentID: null,
  managerRoleRemark: null,
  managerRoleIsEnable: null,
  managerRolePermissionList: [],
});
//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;
  // 名稱
  if (
    typeof systemRoleDetailViewObj.managerRoleName !== "string" ||
    !systemRoleDetailViewObj.managerRoleName.trim() ||
    systemRoleDetailViewObj.managerRoleName.trim().length > 20
  ) {
    systemRoleDetailViewObj.managerRoleNameIsValid = false;
    isValid = false;
  } else {
    systemRoleDetailViewObj.managerRoleNameIsValid = true;
  }

  // 地區
  if (
    systemRoleDetailViewObj.managerRegionID === null ||
    systemRoleDetailViewObj.managerRegionID === undefined
  ) {
    systemRoleDetailViewObj.managerRegionIdIsValid = false;
    isValid = false;
  } else {
    systemRoleDetailViewObj.managerRegionIdIsValid = true;
  }

  // 部門
  if (
    systemRoleDetailViewObj.managerDepartmentID === null ||
    systemRoleDetailViewObj.managerDepartmentID === undefined
  ) {
    systemRoleDetailViewObj.managerDepartmentIdIsValid = false;
    isValid = false;
  } else {
    systemRoleDetailViewObj.managerDepartmentIdIsValid = true;
  }

  // 備註
  if (
    systemRoleDetailViewObj.managerRoleRemark &&
    systemRoleDetailViewObj.managerRoleRemark.trim().length > 20
  ) {
    systemRoleDetailViewObj.managerRoleRemarkIsValid = false;
    isValid = false;
  } else {
    systemRoleDetailViewObj.managerRoleRemarkIsValid = true;
  }

  // 是否啟用
  if (typeof systemRoleDetailViewObj.managerRoleIsEnable !== "boolean") {
    systemRoleDetailViewObj.managerRoleIsEnableIsValid = false;
    isValid = false;
  } else {
    systemRoleDetailViewObj.managerRoleIsEnableIsValid = true;
  }

  return isValid;
};
//#endregion

//#region API / 資料流程
/** 取得資料 */
const getData = async () => {
  // Token 驗證
  if (!requireToken()) return;

  // 檢查 ID 是否有效
  if (!systemRoleDetailViewObj.managerRoleID || isNaN(systemRoleDetailViewObj.managerRoleID)) {
    displayError("無效的角色ID !");
    return;
  }

  // 送api
  const requestData: MbsSysRolHttpGetReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerRoleID: systemRoleDetailViewObj.managerRoleID!,
  };
  const responseData: MbsSysRolHttpGetRspMdl | null = await getSystemRole(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定當前值
  systemRoleDetailViewObj.managerRoleName = responseData.managerRoleName;
  systemRoleDetailViewObj.managerRegionID = responseData.managerRegionID;
  systemRoleDetailViewObj.managerRegionName = responseData.managerRegionName;
  systemRoleDetailViewObj.managerDepartmentID = responseData.managerDepartmentID;
  systemRoleDetailViewObj.managerDepartmentName = responseData.managerDepartmentName;
  systemRoleDetailViewObj.managerRoleRemark = responseData.managerRoleRemark;
  systemRoleDetailViewObj.managerRoleIsEnable = responseData.managerRoleIsEnable;
  systemRoleDetailViewObj.employeeCount = responseData.employeeCount;
  if (responseData.managerRolePermissionList) {
    systemRoleDetailViewObj.managerRolePermissionList = responseData.managerRolePermissionList;
  }

  // 保存原始值
  originalData.managerRoleName = responseData.managerRoleName;
  originalData.managerRegionID = responseData.managerRegionID;
  originalData.managerDepartmentID = responseData.managerDepartmentID;
  originalData.managerRoleRemark = responseData.managerRoleRemark;
  originalData.managerRoleIsEnable = responseData.managerRoleIsEnable;
  if (responseData.managerRolePermissionList) {
    originalData.managerRolePermissionList = JSON.parse(
      JSON.stringify(responseData.managerRolePermissionList)
    );
  }
};

/** 取得變更的欄位 */
const getChangedFields = () => {
  const changes: Partial<MbsSysRolHttpUpdateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerRoleID: systemRoleDetailViewObj.managerRoleID!,
  };

  // 名稱
  if (systemRoleDetailViewObj.managerRoleName?.trim() !== originalData.managerRoleName) {
    changes.managerRoleName = systemRoleDetailViewObj.managerRoleName;
  }

  // 地區
  if (systemRoleDetailViewObj.managerRegionID !== originalData.managerRegionID) {
    changes.managerRoleRegionID = systemRoleDetailViewObj.managerRegionID;
  }

  // 部門
  if (systemRoleDetailViewObj.managerDepartmentID !== originalData.managerDepartmentID) {
    changes.managerDepartmentID = systemRoleDetailViewObj.managerDepartmentID;
  }

  // 備註
  if (systemRoleDetailViewObj.managerRoleRemark !== originalData.managerRoleRemark) {
    changes.managerRoleRemark = systemRoleDetailViewObj.managerRoleRemark;
  }

  // 狀態
  if (systemRoleDetailViewObj.managerRoleIsEnable !== originalData.managerRoleIsEnable) {
    changes.managerRoleIsEnable = systemRoleDetailViewObj.managerRoleIsEnable;
  }

  // 權限列表
  const currentPermissions = JSON.stringify(systemRoleDetailViewObj.managerRolePermissionList);
  const originalPermissions = JSON.stringify(originalData.managerRolePermissionList);
  if (currentPermissions !== originalPermissions) {
    changes.managerRolePermissionList = systemRoleDetailViewObj.managerRolePermissionList.map(
      (item): MbsSysRolHttpAddReqItemMdl => ({
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

/** 更新角色 */
const updateRole = async () => {
  // 隱藏確認對話框
  showConfirmDialog.value = false;

  // Token 驗證
  if (!requireToken()) return;

  // 取得變更的欄位
  const changedFields = getChangedFields();
  // 檢查是否有變更（排除必要的 token 和 ID）
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存 !");
    return;
  }
  // 送出更新請求
  const responseData: MbsSysRolHttpUpdateRspMdl | null = await updateSystemRole(
    changedFields as MbsSysRolHttpUpdateReqMdl
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
  displaySuccess("更新角色成功！");
  // 重新載入資料
  getData();
};
//#endregion

//#region 按鈕事件
/** 點擊【儲存】按鈕 */
const clickSaveBtn = async () => {
  // 欄位驗證
  if (!validateField()) {
    return;
  }
  // 顯示確認對話框
  showConfirmDialog.value = true;
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  isEditMode.value = false;
  // 重新載入資料，恢復到編輯前的狀態
  systemRoleDetailViewObj.managerRegionName = originalData.managerRoleName;
  systemRoleDetailViewObj.managerRegionID = originalData.managerRegionID;
  systemRoleDetailViewObj.managerDepartmentID = originalData.managerDepartmentID;
  systemRoleDetailViewObj.managerRoleRemark = originalData.managerRoleRemark;
  systemRoleDetailViewObj.managerRoleIsEnable = originalData.managerRoleIsEnable;
  systemRoleDetailViewObj.managerRolePermissionList = JSON.parse(
    JSON.stringify(originalData.managerRolePermissionList)
  );
  systemRoleDetailViewObj.managerRegionIdIsValid = true;
  systemRoleDetailViewObj.managerDepartmentIdIsValid = true;
  systemRoleDetailViewObj.managerRoleNameIsValid = true;
  systemRoleDetailViewObj.managerRoleIsEnableIsValid = true;
  systemRoleDetailViewObj.managerRoleRemarkIsValid = true;

  getData();
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/role");
};

/** 點擊【編輯】按鈕 */
const clickEditBtn = () => {
  isEditMode.value = true;
};
//#endregion

//#region 彈跳視窗處理
/** 確認彈跳視窗:確認 */
const confirmSave = async () => {
  await updateRole();
};

/** 確認彈跳視窗:取消 */
const cancelSave = () => {
  showConfirmDialog.value = false;
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
      <div class="flex items-center gap-4">
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
            <button class="btn-cancel" @click="clickCancelBtn()">取消編輯</button>
            <button class="btn-save" @click="clickSaveBtn()">儲存</button>
          </div>
        </template>
      </div>
    </div>

    <!-- 內容 -->
    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-8 gap-4">
      <div class="flex items-center gap-4 w-full">
        <!-- 角色名稱 -->
        <div class="flex-1">
          <label class="form-label">角色名稱 <span class="required-label">*</span></label>
          <input
            v-if="isEditMode"
            v-model="systemRoleDetailViewObj.managerRoleName"
            type="text"
            class="input-box"
            placeholder="請輸入角色名稱"
          />
          <input
            v-else
            v-model="systemRoleDetailViewObj.managerRoleName"
            type="text"
            class="input-box"
            disabled
          />
          <p class="error-wrapper">
            <span v-if="!systemRoleDetailViewObj.managerRoleNameIsValid" class="error-tip">
              不可為空且不可超過 20 個字
            </span>
          </p>
        </div>

        <div class="border border-brand-100/40 rounded-md p-1">
          <p class="text-brand-100 text-sm font-medium">
            當前使用人數: {{ systemRoleDetailViewObj.employeeCount }} 人
          </p>
        </div>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 地區 -->
        <div class="flex-1">
          <label class="form-label">地區 <span class="required-label">*</span></label>
          <GetManyManagerRegionComboBox
            v-model:managerRegionID="systemRoleDetailViewObj.managerRegionID"
            v-model:managerRegionName="systemRoleDetailViewObj.managerRegionName"
            :disabled="!isEditMode"
          />
          <p class="error-wrapper">
            <span v-if="!systemRoleDetailViewObj.managerRegionIdIsValid" class="error-tip">
              不可為空
            </span>
          </p>
        </div>

        <!-- 部門 -->
        <div class="flex-1">
          <label class="form-label">部門 <span class="required-label">*</span></label>
          <GetManyManagerDepartmentComboBox
            v-model:managerDepartmentID="systemRoleDetailViewObj.managerDepartmentID"
            v-model:managerDepartmentName="systemRoleDetailViewObj.managerDepartmentName"
            :disabled="!isEditMode"
          />
          <p class="error-wrapper">
            <span v-if="!systemRoleDetailViewObj.managerDepartmentIdIsValid" class="error-tip">
              不可為空
            </span>
          </p>
        </div>
      </div>

      <!-- 備註 -->
      <div>
        <label class="form-label">備註</label>
        <input
          v-if="isEditMode"
          v-model="systemRoleDetailViewObj.managerRoleRemark"
          type="text"
          class="input-box"
          placeholder="請輸入備註"
        />
        <input
          v-else
          v-model="systemRoleDetailViewObj.managerRoleRemark"
          type="text"
          class="input-box"
          disabled
          placeholder="未輸入備註"
        />
        <p class="error-wrapper">
          <span
            v-if="isEditMode && !systemRoleDetailViewObj.managerRoleRemarkIsValid"
            class="error-tip"
          >
            不可超過 20 個字
          </span>
        </p>
      </div>

      <!-- 狀態 -->
      <div class="w-48">
        <label class="form-label">狀態 <span class="required-label">*</span></label>
        <select
          v-if="isEditMode"
          v-model="systemRoleDetailViewObj.managerRoleIsEnable"
          class="select-box"
        >
          <option :value="true">啟用</option>
          <option :value="false">停用</option>
        </select>
        <select
          v-else
          v-model="systemRoleDetailViewObj.managerRoleIsEnable"
          class="select-box"
          disabled
        >
          <option :value="true">啟用</option>
          <option :value="false">停用</option>
        </select>
        <p class="error-wrapper">
          <span
            v-if="isEditMode && !systemRoleDetailViewObj.managerRoleIsEnableIsValid"
            class="error-tip"
          >
            不可為空
          </span>
        </p>
      </div>

      <!-- 權限 -->
      <div>
        <label class="form-label">角色權限 <span class="required-label">*</span></label>
        <PermissionTable
          v-model:permissionList="systemRoleDetailViewObj.managerRolePermissionList"
          v-model:managerRegionID="systemRoleDetailViewObj.managerRegionID"
          v-model:managerRegionName="systemRoleDetailViewObj.managerRegionName"
          :readonly="!isEditMode"
        />
      </div>
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 確認彈跳視窗 -->
    <ConfirmDialog
      :show="showConfirmDialog"
      title="確認修改"
      message="所有該角色的員工權限部分會重設，確定要修改嗎?"
      confirm-text="確定"
      cancel-text="取消"
      @confirm="confirmSave"
      @cancel="cancelSave"
    />
  </div>
</template>
