<script setup lang="ts">
import { reactive, watch, ref } from "vue";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import GetManyManagerRegionComboBox from "@/components/feature/search-bar/GetManyManagerRegionComboBox.vue";
import GetManyManagerDepartmentComboBox from "@/components/feature/search-bar/GetManyManagerDepartmentComboBox.vue";
import GetManyEmployeeComboBox from "@/components/feature/search-bar/GetManyEmployeeComboBox.vue";
import { getEmployeeProjectMemberRoleLabel } from "@/utils/getEmployeeProjectMemberRoleLabel";

/** 專案成員項目模型 */
interface ProjectMemberItem {
  employeeID: number | null;
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  managerRegionID: number | null;
  managerRegionName: string | null;
  managerDepartmentID: number | null;
  managerDepartmentName: string | null;
  employeeName: string | null;
}

/** 轉專案表單資料 */
interface TransferProjectFormData {
  employeeProjectCode: string;
  employeeProjectName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
  employeeProjectContractUrl: string;
  employeeProjectWorkUrl: string;
  employeeProjectMemberEmployeeList: ProjectMemberItem[];
}

/** 新增成員表單 */
interface AddMemberForm {
  employeeID: number | null;
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  managerRegionID: number | null;
  managerRegionName: string | null;
  managerDepartmentID: number | null;
  managerDepartmentName: string | null;
  employeeName: string | null;
}

interface Props {
  show: boolean;
  /** 承辦業務員工 ID */
  salerEmployeeID: number | null;
  /** 承辦業務員工名稱 */
  salerEmployeeName: string | null;
  /** 承辦業務地區 ID */
  salerRegionID: number | null;
  /** 承辦業務地區名稱 */
  salerRegionName: string | null;
  /** 承辦業務部門 ID */
  salerDepartmentID: number | null;
  /** 承辦業務部門名稱 */
  salerDepartmentName: string | null;
}

interface Emits {
  (e: "confirm", data: TransferProjectFormData): void;
  (e: "cancel"): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

/** 驗證狀態模型 */
interface ValidationState {
  employeeProjectCode: boolean;
  employeeProjectName: boolean;
  employeeProjectStartTime: boolean;
  employeeProjectEndTime: boolean;
  employeeProjectMemberID: boolean;
  employeeProjectMemberRole: boolean;
  // 專案成員角色至少各一
  projectMemberRoleComplete: boolean;
}

/** 驗證狀態物件 */
const validationState = reactive<ValidationState>({
  employeeProjectCode: true,
  employeeProjectName: true,
  employeeProjectStartTime: true,
  employeeProjectEndTime: true,
  employeeProjectMemberID: true,
  employeeProjectMemberRole: true,
  projectMemberRoleComplete: true,
});

/** 表單資料 */
const formData = reactive<TransferProjectFormData>({
  employeeProjectCode: "",
  employeeProjectName: "",
  employeeProjectStartTime: "",
  employeeProjectEndTime: "",
  employeeProjectContractUrl: "",
  employeeProjectWorkUrl: "",
  employeeProjectMemberEmployeeList: [],
});

/** 是否顯示新增成員彈跳視窗 */
const isShowAddMemberModal = ref(false);

/** 新增成員表單資料 */
const addMemberForm = reactive<AddMemberForm>({
  employeeID: null,
  employeeProjectMemberRole: null,
  managerRegionID: null,
  managerRegionName: null,
  managerDepartmentID: null,
  managerDepartmentName: null,
  employeeName: null,
});

/** 重置表單 */
const resetForm = () => {
  formData.employeeProjectCode = "";
  formData.employeeProjectName = "";
  formData.employeeProjectStartTime = "";
  formData.employeeProjectEndTime = "";
  formData.employeeProjectContractUrl = "";
  formData.employeeProjectWorkUrl = "";
  formData.employeeProjectMemberEmployeeList = [];
};

/** 重置新增成員表單 */
const resetAddMemberForm = () => {
  addMemberForm.employeeID = null;
  addMemberForm.employeeProjectMemberRole = null;
  addMemberForm.managerRegionID = null;
  addMemberForm.managerRegionName = null;
  addMemberForm.managerDepartmentID = null;
  addMemberForm.managerDepartmentName = null;
  addMemberForm.employeeName = null;
};

/** 是否已提交新增成員表單 */
const isAddMemberSubmitted = ref(false);

/** 點擊附加人員按鈕 */
const clickAddMemberBtn = () => {
  resetAddMemberForm();
  // 清除驗證狀態
  isAddMemberSubmitted.value = false;
  isShowAddMemberModal.value = true;
};

/** 確認新增成員 */
const confirmAddMember = () => {
  // 標記為已提交
  isAddMemberSubmitted.value = true;

  // 驗證必填欄位
  if (!addMemberForm.employeeID || !addMemberForm.employeeProjectMemberRole) {
    return;
  }

  // 新增到成員列表
  formData.employeeProjectMemberEmployeeList.push({
    employeeID: addMemberForm.employeeID,
    employeeProjectMemberRole: addMemberForm.employeeProjectMemberRole,
    managerRegionID: addMemberForm.managerRegionID,
    managerRegionName: addMemberForm.managerRegionName,
    managerDepartmentID: addMemberForm.managerDepartmentID,
    managerDepartmentName: addMemberForm.managerDepartmentName,
    employeeName: addMemberForm.employeeName,
  });

  // 關閉彈跳視窗
  isShowAddMemberModal.value = false;
};

/** 取消新增成員 */
const cancelAddMember = () => {
  isShowAddMemberModal.value = false;
};

/** 移除專案成員 */
const removeProjectMember = (index: number) => {
  formData.employeeProjectMemberEmployeeList.splice(index, 1);
};

/** 必須包含的專案成員角色 */
const requiredRoles = [
  DbEmployeeProjectMemberRoleEnum.Saler,
  DbEmployeeProjectMemberRoleEnum.ProjectManager,
  DbEmployeeProjectMemberRoleEnum.DepartmentLeader,
  DbEmployeeProjectMemberRoleEnum.Executor,
  DbEmployeeProjectMemberRoleEnum.Assistant,
];

/** 檢查專案成員角色是否齊全 */
const isProjectMemberRoleComplete = (): boolean => {
  return requiredRoles.every((role) =>
    formData.employeeProjectMemberEmployeeList.some(
      (member) => member.employeeProjectMemberRole === role
    )
  );
};

/** 確認送出 */
const handleConfirm = () => {
  // 重置驗證狀態
  validationState.employeeProjectCode = true;
  validationState.employeeProjectName = true;
  validationState.employeeProjectStartTime = true;
  validationState.employeeProjectEndTime = true;
  validationState.employeeProjectMemberID = true;
  validationState.employeeProjectMemberRole = true;
  validationState.projectMemberRoleComplete = true;

  let isValid = true;

  // 驗證必填欄位
  if (!formData.employeeProjectCode.trim()) {
    validationState.employeeProjectCode = false;
    isValid = false;
  }

  if (!formData.employeeProjectName.trim()) {
    validationState.employeeProjectName = false;
    isValid = false;
  }

  if (!formData.employeeProjectStartTime) {
    validationState.employeeProjectStartTime = false;
    isValid = false;
  }

  if (!formData.employeeProjectEndTime) {
    validationState.employeeProjectEndTime = false;
    isValid = false;
  }

  // 驗證專案成員是否至少各一
  if (!isProjectMemberRoleComplete()) {
    validationState.projectMemberRoleComplete = false;
    isValid = false;
  } else {
    validationState.projectMemberRoleComplete = true;
  }

  // 如果驗證失敗，不執行送出
  if (!isValid) {
    return;
  }

  emit("confirm", { ...formData });
};

/** 取消 */
const handleCancel = () => {
  // 重置驗證狀態
  validationState.employeeProjectCode = true;
  validationState.employeeProjectName = true;
  validationState.employeeProjectStartTime = true;
  validationState.employeeProjectEndTime = true;
  validationState.employeeProjectMemberID = true;
  validationState.employeeProjectMemberRole = true;
  validationState.projectMemberRoleComplete = true;
  emit("cancel");
};

/** 監聽 show 變化，重置表單並預設添加承辦業務 */
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      resetForm();

      // 預設添加承辦業務為專案成員
      if (props.salerEmployeeID && props.salerEmployeeName) {
        formData.employeeProjectMemberEmployeeList.push({
          employeeID: props.salerEmployeeID,
          employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum.Saler,
          managerRegionID: props.salerRegionID,
          managerRegionName: props.salerRegionName,
          managerDepartmentID: props.salerDepartmentID,
          managerDepartmentName: props.salerDepartmentName,
          employeeName: props.salerEmployeeName,
        });
      }
    }
  }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-5xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3">
        <h2 class="modal-title">新增專案</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          x
        </button>
      </div>

      <hr />

      <!-- 內容區域 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 基本資訊 -->
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
            <!-- 專案狀態 -->
            <div>
              <label class="form-label">專案狀態 <span class="required-label">*</span></label>
              <input :value="'未開始'" type="text" class="input-box" disabled />
            </div>

            <!-- 專案代碼 -->
            <div>
              <label class="form-label">專案代碼 <span class="required-label">*</span></label>
              <input
                v-model="formData.employeeProjectCode"
                type="text"
                class="input-box"
                placeholder="請輸入專案代碼"
              />
              <div class="error-wrapper">
                <span v-if="!validationState.employeeProjectCode" class="error-tip">
                  專案代碼為必填
                </span>
              </div>
            </div>

            <!-- 專案名稱 -->
            <div>
              <label class="form-label">專案名稱 <span class="required-label">*</span></label>
              <input
                v-model="formData.employeeProjectName"
                type="text"
                class="input-box"
                placeholder="請輸入專案名稱"
              />
              <div class="error-wrapper">
                <span v-if="!validationState.employeeProjectName" class="error-tip">
                  專案名稱為必填
                </span>
              </div>
            </div>
          </div>

          <!-- 時間資訊 -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">開始日期 <span class="required-label">*</span></label>
              <input v-model="formData.employeeProjectStartTime" type="date" class="input-box" />
              <div class="error-wrapper">
                <span v-if="!validationState.employeeProjectStartTime" class="error-tip">
                  專案開始日期為必填
                </span>
              </div>
            </div>

            <div>
              <label class="form-label">結束日期 <span class="required-label">*</span></label>
              <input v-model="formData.employeeProjectEndTime" type="date" class="input-box" />
              <div class="error-wrapper">
                <span v-if="!validationState.employeeProjectEndTime" class="error-tip">
                  專案結束日期為必填
                </span>
              </div>
            </div>
          </div>

          <!-- 網址資訊 -->
          <div>
            <label class="form-label">契約網址</label>
            <input
              v-model="formData.employeeProjectContractUrl"
              type="url"
              class="input-box"
              placeholder="請輸入契約網址"
            />
            <div class="error-wrapper"></div>
          </div>

          <div>
            <label class="form-label">工作企劃書網址</label>
            <input
              v-model="formData.employeeProjectWorkUrl"
              type="url"
              class="input-box"
              placeholder="請輸入工作企劃書網址"
            />
            <div class="error-wrapper"></div>
          </div>

          <!-- 專案成員表格 -->
          <div class="space-y-2">
            <div class="flex justify-between items-center">
              <label class="form-label">專案成員</label>
              <button class="btn-add" @click="clickAddMemberBtn">附加人員</button>
            </div>

            <span v-if="!validationState.projectMemberRoleComplete" class="error-tip">
              請確保每一個角色至少有一位人員
            </span>

            <!-- 成員列表表格 -->
            <table class="table-base table-fixed w-full">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start">層級角色</th>
                  <th class="text-start">地區</th>
                  <th class="text-start">部門</th>
                  <th class="text-start">人員</th>
                  <th class="text-center w-24">操作</th>
                </tr>
              </thead>
              <tbody>
                <!-- 總經理固定列 -->
                <tr class="bg-gray-50">
                  <td class="text-start">總經理</td>
                  <td class="text-start">全區</td>
                  <td class="text-start">總經理室</td>
                  <td class="text-start">陳建良</td>
                  <td class="text-center">
                    <button class="btn-delete text-sm px-3 py-1" disabled>刪除</button>
                  </td>
                </tr>

                <tr
                  v-for="(item, index) in formData.employeeProjectMemberEmployeeList"
                  :key="index"
                >
                  <td class="text-start">
                    {{ getEmployeeProjectMemberRoleLabel(item.employeeProjectMemberRole) }}
                  </td>
                  <td class="text-start">{{ item.managerRegionName || "-" }}</td>
                  <td class="text-start">{{ item.managerDepartmentName || "-" }}</td>
                  <td class="text-start">{{ item.employeeName || "-" }}</td>
                  <td class="text-center">
                    <button
                      class="btn-delete text-sm px-3 py-1"
                      @click="removeProjectMember(index)"
                    >
                      刪除
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleConfirm">送出</button>
        </div>
      </div>
    </div>

    <!-- 附加人員彈跳視窗 -->
    <div v-if="isShowAddMemberModal" class="modal-overlay" style="z-index: 1001">
      <div class="w-[520px] bg-white rounded-lg shadow-lg">
        <!-- 標題列 -->
        <div class="flex items-center justify-between p-3">
          <h2 class="modal-title">附加人員</h2>
          <button
            class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
            aria-label="關閉"
            @click="cancelAddMember"
          >
            x
          </button>
        </div>

        <hr />

        <!-- 內容區 -->
        <div class="p-6 space-y-4">
          <div class="flex gap-4 w-full">
            <!-- 地區 -->
            <div class="flex flex-col gap-2 flex-1">
              <label class="form-label">地區</label>
              <GetManyManagerRegionComboBox
                v-model:manager-region-i-d="addMemberForm.managerRegionID"
                v-model:manager-region-name="addMemberForm.managerRegionName"
                :disabled="false"
                placeholder="請選擇地區"
              />
            </div>

            <!-- 部門 -->
            <div class="flex flex-col gap-2 flex-1">
              <label class="form-label">部門</label>
              <GetManyManagerDepartmentComboBox
                v-model:manager-department-i-d="addMemberForm.managerDepartmentID"
                v-model:manager-department-name="addMemberForm.managerDepartmentName"
                :disabled="false"
                placeholder="請選擇部門"
              />
            </div>
          </div>

          <!-- 角色 -->
          <div>
            <label class="form-label">層級角色 <span class="required-label">*</span></label>
            <select v-model="addMemberForm.employeeProjectMemberRole" class="select-box">
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
              <span
                v-if="isAddMemberSubmitted && !addMemberForm.employeeProjectMemberRole"
                class="error-tip"
              >
                不可為空
              </span>
            </div>
          </div>

          <!-- 人員 -->
          <div>
            <label class="form-label">人員 <span class="required-label">*</span></label>
            <GetManyEmployeeComboBox
              v-model:manager-employee-i-d="addMemberForm.employeeID"
              v-model:manager-employee-name="addMemberForm.employeeName"
              v-model:manager-region-i-d="addMemberForm.managerRegionID"
              v-model:manager-department-i-d="addMemberForm.managerDepartmentID"
              v-model:manager-region-name="addMemberForm.managerRegionName"
              v-model:manager-department-name="addMemberForm.managerDepartmentName"
              :disabled="false"
              placeholder="請選擇人員"
            />
            <div class="error-wrapper">
              <span v-if="isAddMemberSubmitted && !addMemberForm.employeeID" class="error-tip">
                不可為空
              </span>
            </div>
          </div>
        </div>

        <!-- 底部按鈕 -->
        <div class="p-6 pt-4 border-t border-gray-300">
          <div class="flex justify-end gap-2">
            <button class="btn-cancel" @click="cancelAddMember">取消</button>
            <button class="btn-submit" @click="confirmAddMember">送出</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
