<script setup lang="ts">
import { computed, watch } from "vue";

interface Props {
  text: string | null;
  maxLength?: number;
  required?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  maxLength: 500,
  required: false,
});

// 使用 defineModel 來直接綁定驗證狀態
const isValid = defineModel<boolean>("isValid", { default: true });

// 計算當前字數
const currentLength = computed(() => {
  return props.text?.length || 0;
});

// 判斷是否超過最大字數
const isOverLimit = computed(() => {
  return currentLength.value > props.maxLength;
});

// 計算驗證結果
const validationResult = computed(() => {
  if (props.required && (!props.text || props.text.trim() === "")) {
    return false;
  }
  return !isOverLimit.value;
});

// 計數器文字顏色樣式
const counterClass = computed(() => {
  return isOverLimit.value ? "text-red-500" : "text-gray-500";
});

// 監聽驗證狀態變化，自動更新 model
watch(
  validationResult,
  (newValue) => {
    isValid.value = newValue;
  },
  { immediate: true }
);
</script>

<template>
  <p class="text-xs" :class="counterClass"> {{ currentLength }} / {{ maxLength }} </p>
</template>
