<script setup lang="ts">
import { ref } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { formatDateTime } from "@/utils/timeFormatter";

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
const isExpanded = ref(true);
// -----------------------------------------------------------------
/** 商機開發記錄項目 */
interface TrackingItem {
  /** 商機業務開發紀錄ID */
  employeePipelineSalerTrackingID: number | null;
  /** 開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 窗口ID */
  managerContacterID: number;
  /** 窗口名稱 */
  managerContacterName: string;
  /** 備註 */
  employeePipelineSalerTrackingRemark: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  employeePipelineSalerTrackingCreateEmployeeName?: string;
}

interface Props {
  /** 商機開發記錄列表 */
  trackingList: TrackingItem[];
  /** 是否可以新增記錄 */
  canAdd?: boolean;
  /** 是否可以編輯/刪除記錄 */
  canModify?: boolean;
  /** 是否顯示業務人員欄位 */
  showSalerColumn?: boolean;
}

interface Emits {
  /** 新增開發記錄 */
  (e: "add-tracking"): void;
  /** 編輯開發記錄 */
  (e: "edit-tracking", tracking: TrackingItem): void;
  /** 刪除開發記錄 */
  (e: "delete-tracking", index: number): void;
}

const props = withDefaults(defineProps<Props>(), {
  canAdd: false,
  canModify: false,
  showSalerColumn: false,
});

const emit = defineEmits<Emits>();

/** 處理新增開發記錄 */
const handleAddTracking = () => {
  emit("add-tracking");
};

/** 處理編輯開發記錄 */
const handleEditTracking = (tracking: TrackingItem) => {
  emit("edit-tracking", tracking);
};

/** 處理刪除開發記錄 */
const handleDeleteTracking = (index: number) => {
  emit("delete-tracking", index);
};
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex justify-between items-center">
      <div class="h-8 flex items-center gap-2 cursor-pointer" @click="isExpanded = !isExpanded">
        <h2 class="subtitle">商機開發記錄</h2>
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
      <!-- 只要有新增權限就顯示按鈕 -->
      <button
        v-if="props.canAdd && employeeInfoStore.hasPermission(menu, 'create') && isExpanded"
        class="btn-add"
        @click="handleAddTracking"
      >
        附加商機開發記錄
      </button>
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded">
      <div v-if="trackingList.length === 0" class="text-gray-400 text-center py-4">
        {{ canAdd ? "尚未新增開發記錄,請點擊「附加商機開發記錄」按鈕" : "無開發記錄" }}
      </div>

       <div v-else class="overflow-x-auto">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start w-48">開發時間</th>
              <th class="text-start w-48">窗口</th>
              <!-- 根據 prop 決定是否顯示業務人員欄位 -->
              <th v-if="showSalerColumn" class="text-start w-48">業務人員</th>
              <th class="text-start w-48">備註</th>
              <!-- 只有可以編輯/刪除時才顯示操作欄位 -->
              <th v-if="canModify" class="text-center w-32">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in trackingList"
              :key="item.employeePipelineSalerTrackingID || index"
              class="border border-gray-300"
            >
              <td class="text-start">
                {{ formatDateTime(item.employeePipelineSalerTrackingTime) }}
              </td>
              <td class="text-start">{{ item.managerContacterName || "-" }}</td>
              <!-- 根據 prop 決定是否顯示業務人員 -->
              <td v-if="props.showSalerColumn" class="text-start">
                {{ item.employeePipelineSalerTrackingCreateEmployeeName || "-" }}
              </td>
              <td class="text-start">{{ item.employeePipelineSalerTrackingRemark || "-" }}</td>
              <!-- 只有可以編輯/刪除時才顯示操作按鈕 -->
              <td v-if="props.canModify" class="text-center">
                <div class="flex justify-center gap-2">
                  <button
                    v-if="employeeInfoStore.hasPermission(menu, 'edit')"
                    class="btn-update text-sm px-3 py-1"
                    @click="handleEditTracking(item)"
                  >
                    編輯
                  </button>
                  <button
                    v-if="employeeInfoStore.hasPermission(menu, 'delete')"
                    class="btn-delete text-sm px-3 py-1"
                    @click="handleDeleteTracking(index)"
                  >
                    刪除
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
