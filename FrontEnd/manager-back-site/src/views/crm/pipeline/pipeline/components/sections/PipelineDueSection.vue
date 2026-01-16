<script setup lang="ts">
import { ref } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { formatDate } from "@/utils/timeFormatter";

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
const isExpanded = ref(true);
// -----------------------------------------------------------------
/** 履約期限項目 */
interface DueItem {
  /** 商機履約通知ID */
  employeePipelineDueID: number | null;
  /** 履約日期 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}

interface Props {
  /** 履約期限列表 */
  dueList: DueItem[];
  /** 是否為唯讀模式 */
  readonly?: boolean;
}

interface Emits {
  /** 新增履約期限通知 */
  (e: "addDue"): void;
  /** 編輯履約期限通知 */
  (e: "editDue", due: DueItem, index: number): void;
  /** 刪除履約期限通知 */
  (e: "deleteDue", index: number): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: true,
});

const emit = defineEmits<Emits>();
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="flex justify-between items-center">
      <div class="h-8 flex items-center gap-2 cursor-pointer" @click="isExpanded = !isExpanded">
        <h2 class="subtitle">履約期限通知</h2>
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
      <div v-if="dueList.length === 0"></div>

      <div v-else>
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start w-32">履約日期</th>
              <th class="text-start">備註</th>
              <th v-if="!readonly" class="text-center w-32">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in dueList"
              :key="item.employeePipelineDueID || index"
              class="border border-gray-300"
            >
              <td class="text-start">
                {{ formatDate(item.employeePipelineDueTime) || "-" }}
              </td>
              <td class="text-start">{{ item.employeePipelineDueRemark || "-" }}</td>
              <td v-if="!readonly" class="text-center">
                <div class="flex gap-2 justify-center">
                  <button
                    v-if="employeeInfoStore.hasPermission(menu, 'edit')"
                    class="btn-update text-sm px-2 py-1"
                    @click="emit('editDue', item, index)"
                  >
                    編輯
                  </button>
                  <button
                    v-if="employeeInfoStore.hasPermission(menu, 'delete')"
                    class="btn-delete text-sm px-2 py-1"
                    @click="emit('deleteDue', index)"
                  >
                    刪除
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <button
        v-if="employeeInfoStore.hasPermission(menu, 'create') && isExpanded"
        class="mt-[3px] w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30] disabled:opacity-50 disabled:cursor-not-allowed"
        type="button"
        style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
        :disabled="props.readonly"
        @click="emit('addDue')"
      >
        新增履約通知
      </button>
    </div>
  </div>
</template>
