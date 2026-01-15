<script setup lang="ts">
import { watch } from "vue";

const props = defineProps({
  /** 錯誤訊息 */
  message: {
    type: String,
    default: "",
  },
  /** 是否顯示彈跳視窗 */
  show: Boolean,
});

const emit = defineEmits(["close"]);
//-------------------------------------------------------------
// 監聽 show 屬性變化，當為 true 時，設定定時器自動關閉視窗
watch(
  () => props.show,
  (newVal) => {
    if (newVal) {
      setTimeout(() => {
        emit("close");
      }, 200000);
    }
  }
);
</script>

<template>
  <!-- 彈跳視窗 (當 show 為 true 時顯示) -->
  <transition name="fade">
    <div
      v-if="show"
      class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/50"
      @click.self="emit('close')"
    >
      <div class="bg-white rounded-xl shadow-lg min-w-48 px-8 py-6 text-center animate-fade-in">
        <p class="mb-6 text-red-600 font-bold text-base">{{ message }}</p>
        <button class="btn-close" @click="emit('close')">關閉</button>
      </div>
    </div>
  </transition>
</template>

<style scoped>
/* 過渡動畫 */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
