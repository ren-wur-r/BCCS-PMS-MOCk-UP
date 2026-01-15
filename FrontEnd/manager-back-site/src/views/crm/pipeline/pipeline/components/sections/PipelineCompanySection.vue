<script setup lang="ts">
import { ref } from "vue";
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
}

interface Emits {
  /** 附加客戶事件 */
  (e: "add-company"): void;
  /** 編輯客戶事件 */
  (e: "edit-company"): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: true,
});

const emit = defineEmits<Emits>();

/** 處理附加客戶 */
const handleAddCompany = () => {
  emit("add-company");
};

/** 處理編輯客戶 */
const handleEditCompany = () => {
  emit("edit-company");
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
      <div v-if="!props.readonly && isExpanded" class="flex gap-2">
        <button
          v-if="!company && employeeInfoStore.hasPermission(menu, 'create')"
          class="btn-add"
          @click="handleAddCompany"
        >
          附加客戶
        </button>
        <button
          v-if="company && employeeInfoStore.hasPermission(menu, 'edit')"
          class="btn-update"
          @click="handleEditCompany"
        >
          編輯客戶
        </button>
      </div>
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded">
      <!-- 當沒有客戶資料時顯示提示 -->
      <div v-if="!company" class="text-center text-gray-400 py-8">
        尚未選擇客戶,請點擊「附加客戶」按鈕
      </div>

      <!-- 當有客戶資料時顯示資訊 -->
      <div v-else class="grid grid-cols-2 gap-x-8 gap-y-2 p-2">
        <!-- 統編 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">統編</span>
            <span class="text-black text-sm break-words">
              {{ company.managerCompanyUnifiedNumber || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 客戶全名 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">客戶全名</span>
            <span class="text-black text-sm break-words">
              {{ company.managerCompanyName || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 人員規模 -->
        <!-- <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">人員規模</span>
            <span class="text-black text-sm break-words">
              {{ getEmployeeRangeLabel(company.atomEmployeeRange) || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div> -->

        <!-- 客戶分級 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">客戶分級</span>
            <span class="text-black text-sm break-words">
              {{ getCustomerGradeLabel(company.atomCustomerGrade) || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 客戶主分類 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">客戶主分類</span>
            <span class="text-black text-sm break-words">
              {{ company.managerCompanyMainKindName || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 客戶子分類 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">客戶子分類</span>
            <span class="text-black text-sm break-words">
              {{ company.managerCompanySubKindName || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 縣市 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">縣市</span>
            <span class="text-black text-sm break-words">
              {{ getCityLabel(company.atomCity) ?? "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 地址 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">地址</span>
            <span class="text-black text-sm break-words">
              {{ company.managerCompanyLocationAddress || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 電話 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">電話</span>
            <span class="text-black text-sm break-words">
              {{ company.managerCompanyLocationTelephone || "-" }}
            </span>
          </p>
          <hr class="my-2" />
        </div>

        <!-- 狀態 -->
        <div>
          <p class="flex flex-row gap-5">
            <span class="text-gray-400 text-sm w-24 shrink-0">狀態</span>
            <span class="text-black text-sm break-words">
              {{
                company.managerCompanyLocationStatus
                  ? DbAtomManagerCompanyStatusEnum[company.managerCompanyLocationStatus]
                  : "-"
              }}
            </span>
          </p>
          <hr class="my-2" />
        </div>
      </div>
    </div>
  </div>
</template>
