<script setup lang="ts">
import { ref } from "vue";

/** 是否顯示說明視窗 */
const isShowModal = ref(false);

/** 打開說明視窗 */
const openModal = () => {
  isShowModal.value = true;
};

/** 關閉說明視窗 */
const closeModal = () => {
  isShowModal.value = false;
};

// 暴露方法給父元件使用
defineExpose({
  openModal,
  closeModal,
});
</script>

<template>
  <!-- 說明視窗 Modal -->
  <div v-if="isShowModal" class="modal-overlay" @click="closeModal">
    <div
      class="max-w-3xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
      @click.stop
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-4 border-b border-gray-200 flex-shrink-0">
        <h2 class="text-lg font-bold text-gray-800">專案角色權限說明</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-2xl font-bold w-8 h-8 flex items-center justify-center"
          aria-label="關閉"
          @click="closeModal"
        >
          ×
        </button>
      </div>

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="overflow-x-auto">
          <table class="table-base w-full border-collapse">
            <thead>
              <tr>
                <th class="text-left">角色</th>
                <th class="text-center">專案功能</th>
                <th class="text-center">里程碑功能</th>
                <th class="text-center">工項功能</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td class="font-medium">總經理</td>
                <td class="text-center">檢視</td>
                <td class="text-center">檢視</td>
                <td class="text-center">檢視</td>
              </tr>
              <tr>
                <td class="font-medium">專案經理</td>
                <td class="text-center">檢視 / 新增 / 編輯 / 刪除</td>
                <td class="text-center">檢視 / 新增 / 編輯 / 刪除</td>
                <td class="text-center">檢視 / 新增 / 編輯 / 刪除</td>
              </tr>
              <tr>
                <td class="font-medium">部門主管</td>
                <td class="text-center">檢視 / 新增 / 編輯 / 刪除</td>
                <td class="text-center">檢視 / 新增 / 編輯 / 刪除</td>
                <td class="text-center">檢視 / 新增 / 編輯 / 刪除</td>
              </tr>
              <tr>
                <td class="font-medium">執行者</td>
                <td class="text-center">檢視</td>
                <td class="text-center">檢視</td>
                <td class="text-center">編輯</td>
              </tr>
              <tr>
                <td class="font-medium">助理</td>
                <td class="text-center">檢視</td>
                <td class="text-center">檢視</td>
                <td class="text-center">編輯</td>
              </tr>
              <tr>
                <td class="font-medium">業務</td>
                <td class="text-center">檢視</td>
                <td class="text-center">檢視</td>
                <td class="text-center">檢視</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-4 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end">
          <button class="btn-cancel" @click="closeModal">關閉</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}
</style>
