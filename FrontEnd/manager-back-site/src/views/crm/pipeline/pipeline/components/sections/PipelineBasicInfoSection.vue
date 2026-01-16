<script setup lang="ts">
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import { computed } from "vue";
import { getCityLabel } from "@/utils/getCityLabel";

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
// -----------------------------------------------------------------
interface Props {
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  /** 顯示用商機狀態 */
  displayPipelineStatus?: DbAtomPipelineStatusEnum | null;
  /** 客戶資訊 */
  company?: {
    managerCompanyName: string;
    managerCompanyUnifiedNumber: string;
    atomCity: DbAtomCityEnum | null;
  } | null;
  /** 承辦業務員工名稱 */
  employeePipelineSalerEmployeeName: string | null;
  /** 是否為唯讀模式 */
  readonly?: boolean;
  /** Booking code */
  bookingCode?: string | null;
  /** 是否顯示客戶資訊欄位 */
  showCompanyFields?: boolean;
}

interface Emits {
  /** 商機狀態更新 */
  (e: "updateStatus"): void;
  /** 商機失敗 */
  (e: "failPipeline"): void;
  /** 轉專案 */
  (e: "transferProject"): void;
  /** 轉指派業務 */
  (e: "reassignSaler"): void;
  /** Booking code 更新 */
  (e: "updateBookingCode", value: string): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: false,
  displayPipelineStatus: null,
  company: null,
  bookingCode: null,
  showCompanyFields: true,
});

const emit = defineEmits<Emits>();

const displayStatusLabel = computed(() => {
  const status = props.displayPipelineStatus ?? props.atomPipelineStatus;
  if (status === null || status === undefined) return "0";
  if (status === DbAtomPipelineStatusEnum.TransferredToProject) return "成立專案";
  if (status === DbAtomPipelineStatusEnum.Business10Percent) return "10%";
  if (status === DbAtomPipelineStatusEnum.Business30Percent) return "30%";
  if (status === DbAtomPipelineStatusEnum.Business70Percent) return "70%";
  if (status === DbAtomPipelineStatusEnum.Business100Percent) return "100%";
  return getPipelineStatusLabel(status);
});

const northCities = new Set<DbAtomCityEnum>([
  DbAtomCityEnum.Taipei,
  DbAtomCityEnum.NewTaipei,
  DbAtomCityEnum.Keelung,
  DbAtomCityEnum.Taoyuan,
  DbAtomCityEnum.HsinchuCity,
  DbAtomCityEnum.HsinchuCounty,
  DbAtomCityEnum.Yilan,
]);
const centralCities = new Set<DbAtomCityEnum>([
  DbAtomCityEnum.Taichung,
  DbAtomCityEnum.Miaoli,
  DbAtomCityEnum.Changhua,
  DbAtomCityEnum.Nantou,
  DbAtomCityEnum.Yunlin,
]);
const southCities = new Set<DbAtomCityEnum>([
  DbAtomCityEnum.Kaohsiung,
  DbAtomCityEnum.Tainan,
  DbAtomCityEnum.Pingtung,
  DbAtomCityEnum.Chiayi,
  DbAtomCityEnum.ChiayiCounty,
  DbAtomCityEnum.Penghu,
  DbAtomCityEnum.Kinmen,
  DbAtomCityEnum.Lienchiang,
]);
const getRegionLabelByCity = (city: DbAtomCityEnum | null) => {
  if (!city) return "未設定";
  if (northCities.has(city)) return "北區";
  if (centralCities.has(city)) return "中區";
  if (southCities.has(city)) return "南區";
  return getCityLabel(city);
};

const companyRegionLabel = computed(() => getRegionLabelByCity(props.company?.atomCity ?? null));

const companyNameLabel = computed(() => props.company?.managerCompanyName || "未設定");

const companyUnifiedNumberLabel = computed(
  () => props.company?.managerCompanyUnifiedNumber || "未設定"
);

const canEditBookingCode = computed(() => {
  const status = props.atomPipelineStatus;
  return (
    !props.readonly &&
    status !== DbAtomPipelineStatusEnum.Business100Percent &&
    status !== DbAtomPipelineStatusEnum.TransferredToProject
  );
});

const bookingCodeDraft = computed({
  get: () => props.bookingCode ?? "",
  set: (value: string) => emit("updateBookingCode", value),
});

/** 取得商機按鈕文字 */
const getPipelineButtonText = (currentStatus: DbAtomPipelineStatusEnum | null): string => {
  if (!currentStatus) return "";

  switch (currentStatus) {
    case DbAtomPipelineStatusEnum.TransferredToBusiness:
      return "商機10%";
    case DbAtomPipelineStatusEnum.Business10Percent:
      return "商機30%";
    case DbAtomPipelineStatusEnum.Business30Percent:
      return "商機70%";
    case DbAtomPipelineStatusEnum.Business70Percent:
      return "商機100%";
    case DbAtomPipelineStatusEnum.Business100Percent:
      return "轉成專案";
    default:
      return "";
  }
};

/** 是否顯示商機按鈕 */
const shouldShowPipelineButton = (currentStatus: DbAtomPipelineStatusEnum | null): boolean => {
  if (!currentStatus) return false;

  return [
    DbAtomPipelineStatusEnum.TransferredToBusiness,
    DbAtomPipelineStatusEnum.Business10Percent,
    DbAtomPipelineStatusEnum.Business30Percent,
    DbAtomPipelineStatusEnum.Business70Percent,
    DbAtomPipelineStatusEnum.Business100Percent,
  ].includes(currentStatus);
};

/** 是否顯示失敗按鈕 */
const shouldShowFailButton = (currentStatus: DbAtomPipelineStatusEnum | null): boolean => {
  return (
    currentStatus !== null &&
    currentStatus !== DbAtomPipelineStatusEnum.BusinessFailed &&
    currentStatus !== DbAtomPipelineStatusEnum.TransferredToProject
  );
};

/** 點擊商機按鈕 */
const handlePipelineButtonClick = () => {
  const currentStatus = props.atomPipelineStatus;

  // 如果是商機100%，觸發轉專案事件
  if (currentStatus === DbAtomPipelineStatusEnum.Business100Percent) {
    emit("transferProject");
    return;
  }

  // 其他狀態觸發更新狀態事件
  emit("updateStatus");
};
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex justify-between items-center">
      <h2 class="subtitle">基本資訊</h2>
    </div>

    <div class="flex gap-4">
      <div class="flex flex-col gap-2 flex-1">
        <label class="form-label">商機狀態</label>
        <input type="text" class="input-box" :value="displayStatusLabel" disabled />
      </div>
      <div v-if="props.showCompanyFields" class="flex flex-col gap-2 flex-1">
        <label class="form-label">客戶所在地區</label>
        <input type="text" class="input-box" :value="companyRegionLabel" disabled />
      </div>
      <div v-if="props.showCompanyFields" class="flex flex-col gap-2 flex-1">
        <label class="form-label">客戶名稱</label>
        <input type="text" class="input-box" :value="companyNameLabel" disabled />
      </div>
      <div v-if="props.showCompanyFields" class="flex flex-col gap-2 flex-1">
        <label class="form-label">統編</label>
        <input type="text" class="input-box" :value="companyUnifiedNumberLabel" disabled />
      </div>
      <div v-if="props.showCompanyFields" class="flex flex-col gap-2 flex-1">
        <label class="form-label">Booking Code</label>
        <input
          v-model="bookingCodeDraft"
          type="text"
          class="input-box"
          :disabled="!canEditBookingCode"
          :placeholder="canEditBookingCode ? '請輸入 Booking Code' : '未設定'"
        />
      </div>
    </div>
  </div>
</template>
