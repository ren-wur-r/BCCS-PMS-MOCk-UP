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
});

const emit = defineEmits<Emits>();

/** 處理新增窗口 */
const handleAddContacter = () => {
  emit("add-contacter");
};

/** 處理刪除窗口 */
const handleDeleteContacter = (contacterId: number) => {
  emit("delete-contacter", contacterId);
};
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
      <button
        v-if="!props.readonly && employeeInfoStore.hasPermission(menu, 'edit') && isExpanded"
        class="btn-add"
        @click="handleAddContacter"
      >
        附加窗口
      </button>
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded">
      <div v-if="contacterList.length === 0" class="text-gray-400 text-center py-4">
        {{ readonly ? "無窗口資料" : "尚未新增窗口,請點擊「附加窗口」按鈕" }}
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <!-- 每一筆窗口 -->
        <div
          v-for="(item, index) in contacterList"
          :key="item.managerContacterID || index"
          class="border border-gray-300 rounded-lg flex flex-col"
        >
          <!-- 窗口標題列 -->
          <div class="bg-gray-50 p-3 border-b border-gray-300 rounded-t-lg">
            <div class="flex flex-col gap-2">
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2">
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
                  <span class="font-medium text-gray-900">{{
                    item.managerContacterName || "-"
                  }}</span>
                </div>

                <!-- 操作按鈕 -->
                <button
                  v-if="!readonly"
                  class="btn-delete text-sm px-3 py-1"
                  @click="handleDeleteContacter(item.managerContacterID)"
                >
                  刪除
                </button>
              </div>
            </div>
          </div>

          <!-- 窗口詳細資訊 -->
          <div class="px-6 py-4 text-sm text-gray-700 flex-1">
            <!-- 電子信箱 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">電子信箱</span>
              <span class="display-value">{{ item.managerContacterEmail || "-" }}</span>
            </p>
            <hr class="my-2" />

            <!-- 手機 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">手機</span>
              <span class="display-value">{{ item.managerContacterPhone || "-" }}</span>
            </p>
            <hr class="my-2" />

            <!-- 電話#分機 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">電話#分機</span>
              <span class="display-value">{{ item.managerContacterTelephone || "-" }}</span>
            </p>
            <hr class="my-2" />

            <!-- 部門 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">部門</span>
              <span class="display-value">{{ item.managerContacterDepartment || "-" }}</span>
            </p>
            <hr class="my-2" />

            <!-- 職稱 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">職稱</span>
              <span class="display-value">{{ item.managerContacterJobTitle || "-" }}</span>
            </p>
            <hr class="my-2" />

            <!-- 狀態 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">狀態</span>
              <span class="display-value">
                {{ getManagerContacterStatusLabel(item.managerContacterStatus) || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <!-- 個資同意 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">個資同意</span>
              <span class="display-value">
                {{ item.managerContacterIsConsent ? "同意" : "不同意" }}
              </span>
            </p>
            <hr class="my-2" />

            <!-- 開發評等 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">開發評等</span>
              <span class="display-value">
                {{ getManagerContacterRatingLabel(item.atomRatingKind) || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <!-- 備註 -->
            <p class="flex flex-row gap-3">
              <span class="display-label w-20 shrink-0">備註</span>
              <span class="display-value">{{ item.managerContacterRemark || "-" }}</span>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
