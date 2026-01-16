<script setup lang="ts">
import { ref } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { getManagerContacterStatusLabel } from "@/utils/getManagerContacterStatusLabel";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
const isExpanded = ref(true);
const editingContacterId = ref<number | null>(null);
// -----------------------------------------------------------------
/** 窗口資訊項目 */
interface ContacterInfo {
  /** 窗口ID */
  managerContacterID: number;
  /** 商機窗口-是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 窗口姓名 */
  managerContacterName: string;
  /** 窗口Email */
  managerContacterEmail: string;
  /** 窗口手機 */
  managerContacterPhone: string;
  /** 窗口部門 */
  managerContacterDepartment: string;
  /** 窗口職稱 */
  managerContacterJobTitle: string;
  /** 窗口電話(市話) */
  managerContacterTelephone: string;
  /** 窗口是否個資同意 */
  managerContacterIsConsent: boolean;
  /** 窗口在職狀態 */
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  /** 窗口開發評等ID */
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  /** 窗口備註 */
  managerContacterRemark: string;
}

interface Props {
  /** 窗口資訊列表 */
  contacterList: ContacterInfo[];
  /** 是否為唯讀模式 */
  readonly?: boolean;
  /** 是否顯示編輯按鈕 */
  enableEdit?: boolean;
}

interface Emits {
  /** 新增窗口 */
  (e: "add-contacter"): void;
  /** 編輯窗口 */
  (e: "edit-contacter", contacterInfo: ContacterInfo): void;
  /** 刪除窗口 */
  (e: "delete-contacter", contacterId: number): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: true,
  enableEdit: false,
});

const emit = defineEmits<Emits>();

const statusOptions = (() => {
  const numericValues = Object.values(DbAtomManagerContacterStatusEnum).filter(
    (value) => typeof value === "number"
  ) as DbAtomManagerContacterStatusEnum[];
  if (numericValues.length > 0) {
    return numericValues.map((value) => ({
      value,
      label: getManagerContacterStatusLabel(value) || String(value),
    }));
  }
  const stringValues = Object.values(DbAtomManagerContacterStatusEnum).filter(
    (value) => typeof value === "string"
  ) as DbAtomManagerContacterStatusEnum[];
  return stringValues.map((value) => ({
    value,
    label: getManagerContacterStatusLabel(value) || String(value),
  }));
})();

const ratingOptions = (() => {
  const numericValues = Object.values(DbAtomManagerContacterRatingKindEnum).filter(
    (value) => typeof value === "number"
  ) as DbAtomManagerContacterRatingKindEnum[];
  if (numericValues.length > 0) {
    return numericValues.map((value) => ({
      value,
      label: getManagerContacterRatingLabel(value) || String(value),
    }));
  }
  const stringValues = Object.values(DbAtomManagerContacterRatingKindEnum).filter(
    (value) => typeof value === "string"
  ) as DbAtomManagerContacterRatingKindEnum[];
  return stringValues.map((value) => ({
    value,
    label: getManagerContacterRatingLabel(value) || String(value),
  }));
})();

/** 處理新增窗口 */
const handleAddContacter = () => {
  emit("add-contacter");
};

/** 處理編輯窗口 */
const handleEditContacter = (contacterInfo: ContacterInfo) => {
  editingContacterId.value =
    editingContacterId.value === contacterInfo.managerContacterID
      ? null
      : contacterInfo.managerContacterID;
};

/** 處理刪除窗口 */
const handleDeleteContacter = (contacterId: number) => {
  emit("delete-contacter", contacterId);
};

const isEditing = (contacterId: number) => editingContacterId.value === contacterId;
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex justify-between items-center">
      <div class="h-8 flex items-center gap-2 cursor-pointer" @click="isExpanded = !isExpanded">
        <h2 class="subtitle">窗口</h2>
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
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded">
      <div v-if="contacterList.length === 0" class="text-gray-400 text-center py-4">
        {{ readonly ? "無窗口資料" : "尚未新增窗口,請點擊「附加窗口」按鈕" }}
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-2 gap-4">
        <!-- 每一筆窗口 -->
        <div
          v-for="(item, index) in contacterList"
          :key="item.managerContacterID || index"
          class="border border-gray-300 rounded-lg flex flex-col"
        >
          <!-- 窗口標題列 -->
          <div class="bg-gray-50 p-3 border-b border-gray-300 rounded-t-lg">
            <div class="flex flex-col gap-2">
              <div class="flex items-center justify-between gap-3">
                <h2 class="text-xl font-semibold text-gray-900">
                  {{ item.managerContacterName || "-" }}
                </h2>
                <span
                  class="px-3 py-1 text-sm rounded-md inline-block"
                  :class="
                    item.employeePipelineContacterIsPrimary
                      ? 'badge-contacter badge-contacter-primary'
                      : 'badge-contacter badge-contacter-secondary'
                  "
                >
                  {{ item.employeePipelineContacterIsPrimary ? "主要窗口" : "次要窗口" }}
                </span>
              </div>

            </div>
          </div>

          <!-- 窗口詳細資訊 -->
          <div class="px-6 py-4 text-sm text-gray-700 flex-1">
            <!-- 電子信箱 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">電子信箱</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterEmail || "-" }}
              </span>
              <input
                v-else
                v-model="item.managerContacterEmail"
                type="text"
                class="input-box"
                placeholder="輸入電子信箱"
              />
            </p>
            <hr class="my-2" />

            <!-- 分機 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">分機</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterTelephone || "-" }}
              </span>
              <input
                v-else
                v-model="item.managerContacterTelephone"
                type="text"
                class="input-box"
                placeholder="輸入分機"
              />
            </p>
            <hr class="my-2" />

            <!-- 手機 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">手機</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterPhone || "-" }}
              </span>
              <input
                v-else
                v-model="item.managerContacterPhone"
                type="text"
                class="input-box"
                placeholder="輸入手機"
              />
            </p>
            <hr class="my-2" />

            <!-- 部門 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">部門</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterDepartment || "-" }}
              </span>
              <input
                v-else
                v-model="item.managerContacterDepartment"
                type="text"
                class="input-box"
                placeholder="輸入部門"
              />
            </p>
            <hr class="my-2" />

            <!-- 職稱 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">職稱</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterJobTitle || "-" }}
              </span>
              <input
                v-else
                v-model="item.managerContacterJobTitle"
                type="text"
                class="input-box"
                placeholder="輸入職稱"
              />
            </p>
            <hr class="my-2" />

            <!-- 狀態 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">狀態</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ getManagerContacterStatusLabel(item.managerContacterStatus) || "-" }}
              </span>
              <select v-else v-model="item.managerContacterStatus" class="select-box">
                <option v-for="option in statusOptions" :key="String(option.value)" :value="option.value">
                  {{ option.label }}
                </option>
              </select>
            </p>
            <hr class="my-2" />

            <!-- 個資同意 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">個資同意</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterIsConsent ? "同意" : "不同意" }}
              </span>
              <select v-else v-model="item.managerContacterIsConsent" class="select-box">
                <option :value="true">同意</option>
                <option :value="false">不同意</option>
              </select>
            </p>
            <hr class="my-2" />

            <!-- 開發評等 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">開發評等</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ getManagerContacterRatingLabel(item.atomRatingKind) || "-" }}
              </span>
              <select v-else v-model="item.atomRatingKind" class="select-box">
                <option v-for="option in ratingOptions" :key="String(option.value)" :value="option.value">
                  {{ option.label }}
                </option>
              </select>
            </p>
            <hr class="my-2" />

            <!-- 備註 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-24 shrink-0">備註</span>
              <span v-if="!isEditing(item.managerContacterID)" class="display-value">
                {{ item.managerContacterRemark || "-" }}
              </span>
              <input
                v-else
                v-model="item.managerContacterRemark"
                type="text"
                class="input-box"
                placeholder="輸入備註"
              />
            </p>
            <div v-if="!readonly" class="mt-3 border-t border-gray-200 pt-3">
              <div class="flex justify-center gap-2">
                <button
                  class="rounded-lg border border-dashed px-3 py-1 text-xs font-medium text-rose-700 hover:text-rose-800"
                  style="background-color: rgb(254, 242, 242); border-color: rgb(252, 165, 165);"
                  @click="handleDeleteContacter(item.managerContacterID)"
                >
                  刪除
                </button>
                <button
                  v-if="props.enableEdit"
                  class="rounded-lg border border-dashed px-3 py-1 text-xs font-medium text-[#082F49] hover:text-[#061F30]"
                  style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                  @click="handleEditContacter(item)"
                >
                  {{ isEditing(item.managerContacterID) ? "完成" : "編輯" }}
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <button
        v-if="!props.readonly && employeeInfoStore.hasPermission(menu, 'edit')"
        class="mt-4 w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
        @click="handleAddContacter"
      >
        附加窗口
      </button>
    </div>
  </div>
</template>
