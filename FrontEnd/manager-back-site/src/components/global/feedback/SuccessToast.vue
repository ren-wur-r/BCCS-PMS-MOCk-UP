<script setup lang="ts">
import { watch, onMounted } from "vue";

interface Props {
  /** 訊息文字 */
  message: string;
  /** 是否顯示 */
  show: boolean;
  /** 顯示時間，預設 3000ms */
  duration?: number;
}

interface Emits {
  (e: "close"): void;
}

const props = withDefaults(defineProps<Props>(), {
  duration: 3000,
});

const emit = defineEmits<Emits>();

let timer: number | null = null;

/** 開始倒數計時 */
const startTimer = () => {
  if (timer) {
    clearTimeout(timer);
  }
  timer = window.setTimeout(() => {
    emit("close");
  }, props.duration);
};

/** 清除計時器 */
const clearTimer = () => {
  if (timer) {
    clearTimeout(timer);
    timer = null;
  }
};

/** 手動關閉 */
const handleClose = () => {
  clearTimer();
  emit("close");
};

/** 監聽 show 變化 */
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      startTimer();
    } else {
      clearTimer();
    }
  }
);

/** 組件掛載時檢查是否需要開始計時 */
onMounted(() => {
  if (props.show) {
    startTimer();
  }
});
</script>

<template>
  <Transition name="toast">
    <div
      v-if="show"
      class="fixed top-10 left-1/2 -translate-x-1/2 bg-green-100 border border-green-300 text-green-700 px-4 py-3 rounded-lg shadow-lg z-50 flex items-center gap-3 min-w-[300px] max-w-[500px]"
      role="alert"
    >
      <!-- 成功圖示 -->
      <svg
        class="w-5 h-5 flex-shrink-0"
        fill="currentColor"
        viewBox="0 0 20 20"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          fill-rule="evenodd"
          d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
          clip-rule="evenodd"
        />
      </svg>

      <!-- 訊息文字 -->
      <span class="flex-1">{{ message }}</span>

      <!-- 關閉按鈕 -->
      <button
        class="text-green-700 hover:text-green-900 font-bold text-lg leading-none transition-colors"
        aria-label="關閉"
        @click="handleClose"
      >
        ×
      </button>
    </div>
  </Transition>
</template>

<style scoped>
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from {
  opacity: 0;
  transform: translateX(-50%) translateY(-100%);
}

.toast-leave-to {
  opacity: 0;
  transform: translateX(-50%) translateY(-100%);
}
</style>
