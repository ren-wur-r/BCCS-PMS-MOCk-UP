<script setup lang="ts">
import { ref, reactive, watch } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
// import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { getCustomerGradeLabel } from "@/utils/getCustomerGradeLabel";
import { getCityLabel } from "@/utils/getCityLabel";

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
const isExpanded = ref(true);
const isEditing = ref(false);
// -----------------------------------------------------------------
/** 客戶資訊項目 */
interface CompanyInfo {
  /** 公司統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 客戶公司ID */
  managerCompanyID: number | null;
  /** 客戶公司名稱 */
  managerCompanyName: string;
  /** 原子-人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  /** 原子-客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string;
  /** 原子-縣市 */
  atomCity: DbAtomCityEnum | null;
  /** 公司營業地點ID */
  managerCompanyLocationID: number | null;
  /** 公司營業地點地址 */
  managerCompanyLocationAddress: string;
  /** 公司營業地點電話 */
  managerCompanyLocationTelephone: string;
  /** 公司營業地點狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

interface Props {
  /** 客戶資訊 */
  company: CompanyInfo | null;
  /** 是否為唯讀模式 */
  readonly?: boolean;
  /** 客戶所在地區 */
  companyRegionLabel?: string;
  /** Booking Code */
  bookingCode?: string | null;
  /** 客戶簡稱 */
  companyShortName?: string | null;
  /** 英文名 */
  companyEnglishName?: string | null;
  /** 母/子公司關係 */
  companyRelationLabel?: string | null;
  /** 更新日期 */
  updatedAtLabel?: string | null;
}

interface Emits {
  /** 附加客戶事件 */
  (e: "add-company"): void;
  /** 編輯客戶事件 */
  (e: "edit-company"): void;
  /** 儲存客戶事件 */
  (e: "save-company", payload: CompanySavePayload): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: true,
  companyRegionLabel: "未設定",
  bookingCode: null,
  companyShortName: null,
  companyEnglishName: null,
  companyRelationLabel: null,
  updatedAtLabel: null,
});

const emit = defineEmits<Emits>();

interface CompanySavePayload {
  managerCompanyID: number | null;
  managerCompanyLocationID: number | null;
  managerCompanyName: string;
  managerCompanyUnifiedNumber: string;
  managerCompanyLocationTelephone: string;
  managerCompanyLocationAddress: string;
  managerCompanyMainKindName: string;
  managerCompanySubKindName: string;
  customerGradeLabel: string;
  companyStatusLabel: string;
  companyShortName: string;
  companyEnglishName: string;
  companyRelationLabel: string;
}

const draftCompany = reactive<CompanySavePayload>({
  managerCompanyID: null,
  managerCompanyLocationID: null,
  managerCompanyName: "",
  managerCompanyUnifiedNumber: "",
  managerCompanyLocationTelephone: "",
  managerCompanyLocationAddress: "",
  managerCompanyMainKindName: "",
  managerCompanySubKindName: "",
  customerGradeLabel: "",
  companyStatusLabel: "",
  companyShortName: "",
  companyEnglishName: "",
  companyRelationLabel: "",
});

const resetDraft = () => {
  draftCompany.managerCompanyID = props.company?.managerCompanyID ?? null;
  draftCompany.managerCompanyLocationID = props.company?.managerCompanyLocationID ?? null;
  draftCompany.managerCompanyName = props.company?.managerCompanyName ?? "-";
  draftCompany.managerCompanyUnifiedNumber = props.company?.managerCompanyUnifiedNumber ?? "-";
  draftCompany.managerCompanyLocationTelephone = props.company?.managerCompanyLocationTelephone ?? "-";
  draftCompany.managerCompanyLocationAddress = props.company?.managerCompanyLocationAddress ?? "-";
  draftCompany.managerCompanyMainKindName = props.company?.managerCompanyMainKindName ?? "-";
  draftCompany.managerCompanySubKindName = props.company?.managerCompanySubKindName ?? "-";
  draftCompany.customerGradeLabel = getCustomerGradeLabel(props.company?.atomCustomerGrade) || "-";
  draftCompany.companyStatusLabel =
    props.company?.managerCompanyLocationStatus
      ? DbAtomManagerCompanyStatusEnum[props.company.managerCompanyLocationStatus]
      : "-";
  draftCompany.companyShortName = props.companyShortName || "-";
  draftCompany.companyEnglishName = props.companyEnglishName || "-";
  draftCompany.companyRelationLabel = props.companyRelationLabel || "-";
};

watch(
  () => [props.company, props.companyShortName, props.companyEnglishName, props.companyRelationLabel],
  resetDraft,
  { immediate: true }
);

/** 處理附加客戶 */
const handleAddCompany = () => {
  emit("add-company");
};

/** 處理編輯客戶 */
const handleEditCompany = () => {
  resetDraft();
  isEditing.value = true;
};

const handleSaveCompany = () => {
  const confirmSave = window.confirm("此動作會更新該公司全系統資料，是否確認？");
  if (!confirmSave) return;
  emit("save-company", { ...draftCompany });
  isEditing.value = false;
};

const handleCancelEdit = () => {
  resetDraft();
  isEditing.value = false;
};
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex justify-between items-center">
      <div class="h-8 flex items-center gap-2 cursor-pointer" @click="isExpanded = !isExpanded">
        <h2 class="subtitle">客戶</h2>
        <!-- 展開/收合圖示 -->
        <svg
          class="w-5 h-5 transition-transform"
          :class="{ 'rotate-180': !isExpanded }"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M19 9l-7 7-7-7"
          />
        </svg>
      </div>
      <div v-if="isExpanded" class="flex items-center gap-3">
        <span v-if="props.updatedAtLabel" class="text-xs text-gray-500">
          更新日期：{{ props.updatedAtLabel }}
        </span>
        <div v-if="!props.readonly" class="flex gap-2">
          <button
            v-if="!company && employeeInfoStore.hasPermission(menu, 'create')"
            class="btn-add"
            @click="handleAddCompany"
          >
            附加客戶
          </button>
          <span
            v-if="company && employeeInfoStore.hasPermission(menu, 'edit')"
            class="hidden"
          ></span>
        </div>
      </div>
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded">
      <!-- 當沒有客戶資料時顯示提示 -->
      <div v-if="!company" class="text-center text-gray-400 py-8">
        尚未選擇客戶,請點擊「附加客戶」按鈕
      </div>

      <!-- 當有客戶資料時顯示資訊 -->
      <div v-else class="flex flex-col bg-white rounded-lg p-8 gap-4">
        <div class="grid grid-cols-1 gap-4 md:grid-cols-3">
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">客戶全名</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.managerCompanyName"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">統編</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.managerCompanyUnifiedNumber"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">電話</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.managerCompanyLocationTelephone"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">客戶簡稱</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.companyShortName"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">英文名</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.companyEnglishName"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">地址</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.managerCompanyLocationAddress"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">分級</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.customerGradeLabel"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">產業</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.managerCompanyMainKindName"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">類別</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.managerCompanySubKindName"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">新增子公司</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.companyRelationLabel"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">新增母公司</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.companyRelationLabel"
              :disabled="!isEditing"
            />
          </div>
          <div class="mb-3 flex flex-col gap-2">
            <label class="form-label">狀態</label>
            <input
              type="text"
              class="input-box"
              v-model="draftCompany.companyStatusLabel"
              :disabled="!isEditing"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
