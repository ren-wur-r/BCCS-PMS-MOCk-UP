<script setup lang="ts">
//#region 引入
import type { MbsBscHttpGetEmployeeRspMdl } from "@/services/pms-http/basic/basicHttpFormat";

//#endregion

interface Props {
  /** 是否顯示彈跳視窗 */
  isVisible: boolean;
  /** 員工資訊 */
  employeeData: MbsBscHttpGetEmployeeRspMdl | null;
}

defineProps<Props>();
const emit = defineEmits<{
  close: [];
}>();

//
/** 關閉彈跳視窗 */
const closeModal = () => {
  emit("close");
};

//-------------------------------------------------------------
</script>

<template>
  <div v-if="isVisible" class="modal-overlay">
    <div
      class="min-w-[600px] bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-4 flex-shrink-0">
        <h2 class="modal-title">個人資訊</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="closeModal"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div v-if="employeeData" class="p-8 overflow-y-auto flex-1">
        <div class="space-y-4">
          <div class="grid grid-cols-2 gap-4">
            <!-- 帳號 -->
            <div>
              <label class="form-label">帳號</label>
              <div class="text-sm text-gray-700 p-2 bg-gray-50 rounded-md border border-gray-200">
                {{ employeeData.employeeAccount }}
              </div>
            </div>
            <!-- 電子信箱 -->
            <div>
              <label class="form-label">電子信箱</label>
              <div class="text-sm text-gray-700 p-2 bg-gray-50 rounded-md border border-gray-200">
                {{ employeeData.employeeAccount }}@bccs.com.tw
              </div>
            </div>
          </div>

          <!-- 姓名 -->
          <div>
            <label class="form-label">姓名</label>
            <div class="text-sm text-gray-700 p-2 bg-gray-50 rounded-md border border-gray-200">
              {{ employeeData.employeeName }}
            </div>
          </div>

          <div class="grid grid-cols-3 gap-4">
            <!-- 角色 -->
            <div>
              <label class="form-label">角色</label>
              <div class="text-sm text-gray-700 p-2 bg-gray-50 rounded-md border border-gray-200">
                {{ employeeData.managerRoleName }}
              </div>
            </div>
            <!-- 地區 -->
            <div>
              <label class="form-label">地區</label>
              <div class="text-sm text-gray-700 p-2 bg-gray-50 rounded-md border border-gray-200">
                {{ employeeData.managerRegionName }}
              </div>
            </div>
            <!-- 部門 -->
            <div>
              <label class="form-label">部門</label>
              <div class="text-sm text-gray-700 p-2 bg-gray-50 rounded-md border border-gray-200">
                {{ employeeData.managerDepartmentName }}
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="p-8 overflow-y-auto flex-1">
        <p class="loading-text">載入中...</p>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-4 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="closeModal">關閉</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.info-value {
  font-size: 16px;
  color: #3c3c3c;
  padding: 12px;
  background-color: #f9fafb;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
}
</style>
