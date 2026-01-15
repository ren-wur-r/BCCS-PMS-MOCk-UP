<script setup lang="ts">
interface Props {
  show: boolean;
  title?: string;
  message: string;
}

const props = defineProps<Props>();

defineEmits<{
  confirm: [];
  cancel: [];
}>();

</script>

<template>
  <div
    v-if="props.show"
    class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
  >
    <div class="bg-white rounded-lg p-6 max-w-md w-full mx-4">
      <!-- 標題 -->
      <div class="mb-4">
        <h3 class="modal-title">{{ title }}</h3>
      </div>

      <!-- 內容 -->
      <div class="mb-6">
        <p class="text-gray-700">{{ message }}</p>
      </div>

      <!-- 按鈕 -->
      <div class="flex justify-end gap-2">
        <button class="btn-cancel" @click="$emit('cancel')">取消</button>
        <button class="btn-confirm" @click="$emit('confirm')">確定</button>
      </div>
    </div>
  </div>
</template>
