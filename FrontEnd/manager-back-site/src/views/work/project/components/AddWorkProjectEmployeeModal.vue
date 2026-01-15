<script setup lang="ts">
import { reactive, computed } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { addProjectMember } from "@/services";
import GetManyEmployeeComboBox from "@/components/feature/search-bar/GetManyEmployeeComboBox.vue";
import GetManyManagerDepartmentComboBox from "@/components/feature/search-bar/GetManyManagerDepartmentComboBox.vue";
import {
  MbsWrkPrjHttpAddMemberReqMdl,
  MbsWrkPrjHttpAddMemberRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
//----------------------------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();

//-------------------------------------------------------------
const props = defineProps<{
  employeeProjectID: number;
  existMemberList: {
    employeeID: number;
    employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  }[];
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();

//-------------------------------------------------------------
/** 新增-工作-專案管理-成員-模型 */
interface AddWorkProjectMemberMdl {
  employeePipelineID: number;
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  managerProductName: string;
  employeeID: number;
  managerEmployeeName: string;
  managerDepartmentID: number;
  managerDepartmentName: string;
  managerRoleID: number;
  managerRoleName: string;
  managerRegionID: number;
  managerRegionName: string;
}

/** 新增-工作-專案管理-成員-驗證模型 */
interface AddWorkProjectMemberValidateMdl {
  employeeProjectMemberRole: boolean;
  employeeID: boolean;
}

//-------------------------------------------------------------------
/** 新增-工作-專案管理-成員-物件 */
const addWorkProjectMemberObj = reactive<AddWorkProjectMemberMdl>({
  employeePipelineID: 0,
  employeeProjectMemberRole: null,
  managerProductName: "",
  employeeID: 0,
  managerEmployeeName: "",
  managerDepartmentID: 0,
  managerDepartmentName: "",
  managerRoleID: 0,
  managerRoleName: "",
  managerRegionID: 0,
  managerRegionName: "",
});

/** 新增-工作-專案管理-成員-驗證物件 */
const addWorkProjectMemberValidateObj = reactive<AddWorkProjectMemberValidateMdl>({
  employeeProjectMemberRole: true,
  employeeID: true,
});
//-------------------------------------------------------------
/** 驗證方法 */
const validateField = () => {
  let isValid = true;

  // 驗證：是否已存在相同人員 + 角色
  if (isDuplicateMemberRole.value) {
    showError.value = true;
    errorMessage.value = "此人員已存在相同層級角色，請勿重複新增";
    isValid = false;
  }

  // 驗證：層級角色（employeeProjectMemberRole）
  if (addWorkProjectMemberObj.employeeProjectMemberRole === null) {
    addWorkProjectMemberValidateObj.employeeProjectMemberRole = false;
    isValid = false;
  }
  // 驗證：人員（employeeID）
  if (!addWorkProjectMemberObj.employeeID) {
    addWorkProjectMemberValidateObj.employeeID = false;
    isValid = false;
  }

  return isValid;
};

/** 是否重複人員 + 角色 */
const isDuplicateMemberRole = computed(() => {
  if (
    !addWorkProjectMemberObj.employeeID ||
    addWorkProjectMemberObj.employeeProjectMemberRole === null
  ) {
    return false;
  }

  return props.existMemberList.some(
    (m) =>
      m.employeeID === addWorkProjectMemberObj.employeeID &&
      m.employeeProjectMemberRole === addWorkProjectMemberObj.employeeProjectMemberRole
  );
});
//--------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 驗證
  if (!validateField()) return;

  // 呼叫api
  const requestData: MbsWrkPrjHttpAddMemberReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeeProjectID: props.employeeProjectID!,
    employeeID: addWorkProjectMemberObj.employeeID!,
    employeeProjectMemberRole: addWorkProjectMemberObj.employeeProjectMemberRole,
  };

  const responseData: MbsWrkPrjHttpAddMemberRspMdl | null = await addProjectMember(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  emit("submit");
  emit("close");
};
//-------------------------------------------------------------
/** 重置錯誤 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};
</script>

<template>
  <div class="modal-overlay">
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">附加人員</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-6 space-y-4">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
        >
          {{ errorMessage }}
        </p>

        <div class="flex gap-4 w-full mt-3">
          <!-- 地區 -->
          <!-- <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">地區</label>
            <GetManyManagerRegionComboBox
              v-model:managerRegionID="addWorkProjectMemberObj.managerRegionID"
              v-model:managerRegionName="addWorkProjectMemberObj.managerRegionName"
              :disabled="false"
              placeholder="請選擇地區"
              @change="resetError"
            />
          </div> -->

          <!-- 部門 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">部門</label>
            <GetManyManagerDepartmentComboBox
              v-model:managerDepartmentID="addWorkProjectMemberObj.managerDepartmentID"
              v-model:managerDepartmentName="addWorkProjectMemberObj.managerDepartmentName"
              :disabled="false"
              placeholder="請選擇部門"
              @change="resetError"
            />
          </div>
        </div>

        <hr class="my-6" />
        <!-- 角色 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">層級角色 <span class="required-label">*</span></label>
          <select v-model="addWorkProjectMemberObj.employeeProjectMemberRole" class="select-box">
            <option :value="null">請選擇</option>
            <option :value="DbEmployeeProjectMemberRoleEnum.ProjectManager">
              專案經理（完整權限：檢視 / 新增 / 編輯 / 刪除）
            </option>
            <option :value="DbEmployeeProjectMemberRoleEnum.DepartmentLeader">
              部門主管（完整權限：檢視 / 新增 / 編輯 / 刪除）
            </option>
            <option :value="DbEmployeeProjectMemberRoleEnum.Executor">
              執行者（檢視 + 工項編輯）
            </option>
            <option :value="DbEmployeeProjectMemberRoleEnum.Assistant">
              助理（檢視 + 工項編輯）
            </option>
            <option :value="DbEmployeeProjectMemberRoleEnum.Saler">業務（僅檢視）</option>
          </select>
          <div class="error-wrapper">
            <p v-if="!addWorkProjectMemberValidateObj.employeeProjectMemberRole" class="error-tip">
              不可為空
            </p>
          </div>
        </div>
        <!-- 人員 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">人員 <span class="required-label">*</span></label>
          <GetManyEmployeeComboBox
            v-model:managerEmployeeID="addWorkProjectMemberObj.employeeID"
            v-model:managerEmployeeName="addWorkProjectMemberObj.managerEmployeeName"
            v-model:managerDepartmentID="addWorkProjectMemberObj.managerDepartmentID"
            v-model:managerRegionName="addWorkProjectMemberObj.managerRegionName"
            v-model:managerDepartmentName="addWorkProjectMemberObj.managerDepartmentName"
            :managerRoleID="null"
            :disabled="false"
            placeholder="請選擇人員"
            @change="resetError"
          />
          <div class="error-wrapper">
            <p v-if="!addWorkProjectMemberValidateObj.employeeID" class="error-tip">不可為空</p>
          </div>
        </div>
      </div>

      <!-- 底部按鈕 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="$emit('close')">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>
</template>
