<script setup lang="ts">
import { reactive, watch, ref, computed } from "vue";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";

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

interface Props {
  show: boolean;
  companyRegionLabel: string | null;
  companyName: string | null;
  projectTypeLabel: string | null;
  serviceItemLabel: string | null;
  productLabel: string | null;
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
}

/** 驗證狀態物件 */
const validationState = reactive<ValidationState>({
  employeeProjectCode: true,
  employeeProjectName: true,
  employeeProjectStartTime: true,
  employeeProjectEndTime: true,
  employeeProjectMemberID: true,
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

const transferStep = ref<1 | 2>(1);
const isProjectCodeManual = ref(false);
const isProjectNameManual = ref(false);
const contractFiles = ref<File[]>([]);

const regionCodeMap: Record<string, string> = {
  北區: "A",
  中區: "B",
  南區: "C",
  跨區: "X",
};

const getProjectSequenceKey = (regionCode: string, date: Date) => {
  const year = date.getFullYear() - 1911;
  const month = String(date.getMonth() + 1).padStart(2, "0");
  return `cache.work.project.seq.${regionCode}.${year}${month}`;
};

const getNextProjectSequence = (regionCode: string, date: Date) => {
  const key = getProjectSequenceKey(regionCode, date);
  const raw = localStorage.getItem(key);
  const lastSeq = raw ? Number(raw) : 0;
  const nextSeq = Math.min(lastSeq + 1, 999);
  return String(nextSeq).padStart(3, "0");
};

const autoProjectCode = computed(() => {
  const regionCode = regionCodeMap[props.companyRegionLabel || ""] || "A";
  const startDate = formData.employeeProjectStartTime
    ? new Date(formData.employeeProjectStartTime)
    : new Date();
  const rocYear = String(startDate.getFullYear() - 1911).padStart(3, "0");
  const month = String(startDate.getMonth() + 1).padStart(2, "0");
  const day = String(startDate.getDate()).padStart(2, "0");
  const sequence = getNextProjectSequence(regionCode, startDate);
  return `P${regionCode}${rocYear}${month}${day}${sequence}`;
});

const autoProjectName = computed(() => {
  const typePrefix = props.projectTypeLabel === "混合案" ? "H" : "S";
  const sequence = autoProjectCode.value ? autoProjectCode.value.slice(-3) : "000";
  const serviceName = props.serviceItemLabel || "";
  const productName = props.productLabel || "";
  if (!serviceName || !productName) return "";
  return `${typePrefix}${sequence}-${serviceName}-${productName}`;
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
  transferStep.value = 1;
  isProjectCodeManual.value = false;
  isProjectNameManual.value = false;
  contractFiles.value = [];
};

/** 移除專案成員 */
const removeProjectMember = (index: number) => {
  formData.employeeProjectMemberEmployeeList.splice(index, 1);
};

const regionOptions = ["北區", "中區", "南區", "跨區"];

const getDepartmentOptions = (regionName: string | null) => {
  const regionFiltered = orgMemberDirectory.filter((member) =>
    regionName && regionName !== "跨區" ? member.regionName === regionName : true
  );
  const serviceKeyword = props.serviceItemLabel || "";
  let filtered = regionFiltered;
  if (serviceKeyword.includes("顧問")) {
    filtered = regionFiltered.filter((member) => member.departmentName.includes("顧問"));
  } else if (serviceKeyword.includes("工程")) {
    filtered = regionFiltered.filter((member) => member.departmentName.includes("工程"));
  }
  const departments = filtered.map((member) => member.departmentName);
  return Array.from(new Set(departments));
};

const getEmployeeOptions = (regionName: string | null, departmentName: string | null) => {
  const filtered = orgMemberDirectory.filter((member) => {
    const regionMatch = regionName && regionName !== "跨區" ? member.regionName === regionName : true;
    const departmentMatch = departmentName ? member.departmentName === departmentName : true;
    return regionMatch && departmentMatch;
  });
  return filtered;
};

const addProjectMemberRow = () => {
  const region = props.companyRegionLabel || "北區";
  const departments = getDepartmentOptions(region);
  formData.employeeProjectMemberEmployeeList.push({
    employeeID: null,
    employeeProjectMemberRole: null,
    managerRegionID: null,
    managerRegionName: region,
    managerDepartmentID: null,
    managerDepartmentName: departments[0] || null,
    employeeName: null,
  });
};

const handleRegionChange = (index: number) => {
  const item = formData.employeeProjectMemberEmployeeList[index];
  const departments = getDepartmentOptions(item.managerRegionName);
  item.managerDepartmentName = departments[0] || null;
  item.employeeName = null;
  item.employeeID = null;
};

const handleDepartmentChange = (index: number) => {
  const item = formData.employeeProjectMemberEmployeeList[index];
  item.employeeName = null;
  item.employeeID = null;
};

const handleEmployeeChange = (index: number) => {
  const item = formData.employeeProjectMemberEmployeeList[index];
  const match = getEmployeeOptions(item.managerRegionName, item.managerDepartmentName).find(
    (member) => member.name === item.employeeName
  );
  item.employeeID = match ? orgMemberDirectory.indexOf(match) + 1 : null;
};

/** 確認送出 */
const handleConfirm = () => {
  // 重置驗證狀態
  validationState.employeeProjectCode = true;
  validationState.employeeProjectName = true;
  validationState.employeeProjectStartTime = true;
  validationState.employeeProjectEndTime = true;
  validationState.employeeProjectMemberID = true;

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

  // 驗證專案成員至少一位且資料完整
  if (
    formData.employeeProjectMemberEmployeeList.length === 0 ||
    formData.employeeProjectMemberEmployeeList.some(
      (member) => !member.employeeName || !member.managerRegionName || !member.managerDepartmentName
    )
  ) {
    validationState.employeeProjectMemberID = false;
    isValid = false;
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
  emit("cancel");
};

const handleContractFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement | null;
  if (!target?.files) {
    contractFiles.value = [];
    return;
  }
  contractFiles.value = Array.from(target.files);
};

const goToMemberStep = () => {
  transferStep.value = 2;
  if (formData.employeeProjectMemberEmployeeList.length === 0) {
    addProjectMemberRow();
  }
};

const goToBasicStep = () => {
  transferStep.value = 1;
};

/** 監聽 show 變化，重置表單 */
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      resetForm();

      if (autoProjectCode.value) {
        formData.employeeProjectCode = autoProjectCode.value;
      }
      if (autoProjectName.value) {
        formData.employeeProjectName = autoProjectName.value;
      }
      addProjectMemberRow();
    }
  }
);

watch(
  () => autoProjectCode.value,
  (value) => {
    if (!isProjectCodeManual.value && value) {
      formData.employeeProjectCode = value;
    }
  }
);

watch(
  () => autoProjectName.value,
  (value) => {
    if (!isProjectNameManual.value && value) {
      formData.employeeProjectName = value;
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
        <div v-if="transferStep === 1" class="space-y-4">
          <div class="rounded-lg border border-gray-200 p-4">
            <h3 class="subtitle text-gray-700">基本資訊</h3>
            <div class="mt-3 grid grid-cols-1 md:grid-cols-2 gap-4 text-sm text-gray-700">
              <div>客戶所在地：{{ props.companyRegionLabel || "-" }}</div>
              <div>
                <label class="form-label">專案代碼 <span class="required-label">*</span></label>
                <input
                  v-model="formData.employeeProjectCode"
                  type="text"
                  class="input-box"
                  placeholder="請輸入專案代碼"
                  @input="isProjectCodeManual = true"
                />
                <div class="error-wrapper">
                  <span v-if="!validationState.employeeProjectCode" class="error-tip">
                    專案代碼為必填
                  </span>
                </div>
              </div>
              <div>
                <label class="form-label">專案名稱 <span class="required-label">*</span></label>
                <input
                  v-model="formData.employeeProjectName"
                  type="text"
                  class="input-box"
                  placeholder="請輸入專案名稱"
                  @input="isProjectNameManual = true"
                />
                <div class="error-wrapper">
                  <span v-if="!validationState.employeeProjectName" class="error-tip">
                    專案名稱為必填
                  </span>
                </div>
              </div>
              <div>客戶：{{ props.companyName || "-" }}</div>
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
            <div class="mt-3">
              <label class="form-label">上傳合約</label>
              <div class="mt-2 flex flex-col gap-2">
                <label class="btn-update cursor-pointer text-xs" title="支援多檔上傳">
                  上傳合約
                  <input type="file" class="hidden" multiple @change="handleContractFileChange" />
                </label>
                <div v-if="contractFiles.length > 0" class="flex flex-wrap gap-2">
                  <span
                    v-for="file in contractFiles"
                    :key="file.name"
                    class="rounded-full bg-gray-100 px-2 py-0.5 text-xs text-gray-600"
                  >
                    {{ file.name }}
                  </span>
                </div>
                <div v-else class="text-xs text-gray-500">尚未選擇檔案</div>
              </div>
            </div>
          </div>

          <div class="rounded-lg border border-gray-200 p-4">
            <h3 class="subtitle text-gray-700">類型與服務</h3>
            <div class="mt-3 text-sm text-gray-700">
              <div>專案類型：{{ props.projectTypeLabel || "-" }}</div>
              <div class="mt-2">服務項目：{{ props.serviceItemLabel || "-" }}</div>
              <div class="mt-2">產品：{{ props.productLabel || "-" }}</div>
            </div>
          </div>
        </div>

        <div v-else class="flex flex-col bg-white rounded-lg p-8 gap-4">
          <div class="flex items-center justify-between">
            <h2 class="subtitle mt-2">專案人員</h2>
          </div>
          <p class="text-xs text-gray-500 mt-1">
            系統會依區域與服務類型帶入預設人員，仍可自行調整。
          </p>
          <div class="space-y-6">
            <div class="rounded-lg border border-gray-200 bg-white p-4">
              <div class="flex items-center justify-between">
                <h3 class="subtitle text-gray-700">{{ props.serviceItemLabel || "服務項目" }}</h3>
              </div>
              <table class="table-base table-fixed table-sticky w-full mt-3">
                <thead class="bg-gray-800 text-white">
                  <tr>
                    <th class="text-start">所屬區域</th>
                    <th class="text-start">所屬部門</th>
                    <th class="text-start">專案人員</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="formData.employeeProjectMemberEmployeeList.length === 0">
                    <td colspan="3" class="text-center text-gray-400 py-4">尚未新增人員</td>
                  </tr>
                  <tr
                    v-for="(item, index) in formData.employeeProjectMemberEmployeeList"
                    :key="index"
                  >
                    <td class="text-start">
                      <div class="combo-custom">
                        <div class="relative flex w-full">
                          <select
                            v-model="item.managerRegionName"
                            class="input-box select-reset"
                            @change="handleRegionChange(index)"
                          >
                            <option v-for="region in regionOptions" :key="region" :value="region">
                              {{ region }}
                            </option>
                          </select>
                          <span class="select-icon">
                            <svg class="w-4 h-4" viewBox="0 0 20 25" fill="none">
                              <path
                                d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                                fill="#787676"
                              />
                            </svg>
                          </span>
                        </div>
                      </div>
                    </td>
                    <td class="text-start">
                      <div class="combo-custom">
                        <div class="relative flex w-full">
                          <select
                            v-model="item.managerDepartmentName"
                            class="input-box select-reset"
                            @change="handleDepartmentChange(index)"
                          >
                            <option
                              v-for="department in getDepartmentOptions(item.managerRegionName)"
                              :key="department"
                              :value="department"
                            >
                              {{ department }}
                            </option>
                          </select>
                          <span class="select-icon">
                            <svg class="w-4 h-4" viewBox="0 0 20 25" fill="none">
                              <path
                                d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                                fill="#787676"
                              />
                            </svg>
                          </span>
                        </div>
                      </div>
                    </td>
                    <td class="text-start">
                      <div class="combo-custom">
                        <div class="relative flex w-full">
                          <select
                            v-model="item.employeeName"
                            class="input-box select-reset"
                            @change="handleEmployeeChange(index)"
                          >
                            <option value="">請選擇人員</option>
                            <option
                              v-for="employee in getEmployeeOptions(
                                item.managerRegionName,
                                item.managerDepartmentName
                              )"
                              :key="employee.email"
                              :value="employee.name"
                            >
                              {{ employee.name }}
                            </option>
                          </select>
                          <span class="select-icon">
                            <svg class="w-4 h-4" viewBox="0 0 20 25" fill="none">
                              <path
                                d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                                fill="#787676"
                              />
                            </svg>
                          </span>
                        </div>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
              <button
                type="button"
                class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30] mt-3"
                style="background-color: #F2F6F9; border-color: #082F49;"
                @click="addProjectMemberRow"
              >
                新增專案人員
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300">
        <div v-if="transferStep === 1" class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="goToMemberStep">確認</button>
        </div>
        <div v-else class="flex justify-center gap-2">
          <button class="btn-cancel" @click="goToBasicStep">上一步</button>
          <button class="btn-submit" @click="handleConfirm">確認建立專案</button>
        </div>
      </div>
    </div>

  </div>
</template>
