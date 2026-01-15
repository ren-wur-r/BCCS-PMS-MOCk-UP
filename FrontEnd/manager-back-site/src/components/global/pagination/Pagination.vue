<script setup lang="ts">
import { computed } from "vue";

const props = defineProps({
  pageIndex: { type: Number, required: true }, // 當前頁碼
  pageSize: { type: Number, required: true }, // 每頁筆數
  totalCount: { type: Number, required: true }, // 總筆數
});

const emit = defineEmits<{
  (event: "update:current-page", value: number): void;
  (event: "update:page-size", value: number): void;
}>();
//--------------------------------------------------------------
// 計算總頁數
const totalPages = computed(() => Math.ceil(props.totalCount / props.pageSize));

/** 上一頁 */
const prevPage = () => {
  if (props.pageIndex > 1) {
    emit("update:current-page", props.pageIndex - 1);
  }
};

/** 下一頁 */
const nextPage = () => {
  if (props.pageIndex < totalPages.value) {
    emit("update:current-page", props.pageIndex + 1);
  }
};

/** 更新頁數 */
const changePageSize = (event: Event) => {
  const target = event.target as HTMLSelectElement;
  emit("update:page-size", Number(target.value));
};
</script>

<template>
  <div
    class="flex items-center justify-center gap-4 p-2.5 rounded-lg"
  >
    <!-- 每頁筆數選擇 -->
    <div class="select-wrapper">
      <select :value="pageSize" class="select-box" @change="changePageSize">
        <option value="10">10筆</option>
        <option value="50">50筆</option>
        <option value="100">100筆</option>
      </select>
    </div>

    <div class="flex items-center gap-2">
      <!-- 上一頁 -->
      <button
        class="flex items-center justify-center w-8 h-8 rounded bg-gray-200 hover:bg-cyan-500 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed"
        :disabled="pageIndex === 1"
        @click="prevPage"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="w-4 h-4 text-gray-700 hover:text-white transition-colors"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          stroke-width="2"
        >
          <path stroke-linecap="round" stroke-linejoin="round" d="M15 19l-7-7 7-7" />
        </svg>
      </button>

      <!-- 當前頁碼 -->
      <span class="whitespace-nowrap text-sm text-gray-700"
        >{{ pageIndex }} / {{ totalPages }}</span
      >

      <!-- 下一頁 -->
      <button
        class="flex items-center justify-center w-8 h-8 rounded bg-gray-200 hover:bg-cyan-500 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed"
        :disabled="pageIndex >= totalPages"
        @click="nextPage"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="w-4 h-4 text-gray-700 hover:text-white transition-colors"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          stroke-width="2"
        >
          <path stroke-linecap="round" stroke-linejoin="round" d="M9 5l7 7-7 7" />
        </svg>
      </button>
    </div>

    <!-- 總筆數 -->
    <span class="whitespace-nowrap text-sm text-gray-700">共 {{ totalCount }} 筆</span>
  </div>
</template>
