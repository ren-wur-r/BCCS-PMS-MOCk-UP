<script setup lang="ts">
import { computed } from "vue";

interface Props {
  startTime: string;
  endTime: string;
  hasUnprocessed?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  hasUnprocessed: false
});

const status = computed(() => {
  const now = new Date();
  const start = new Date(props.startTime);
  const end = new Date(props.endTime);

  if (now < start) {
    return { label: "即將開始", class: "bg-blue-100 text-blue-700", icon: "🔵" };
  } else if (now >= start && now <= end) {
    return { label: "進行中", class: "bg-green-100 text-green-700", icon: "🟢" };
  } else if (props.hasUnprocessed) {
    return { label: "待處理", class: "bg-yellow-100 text-yellow-700", icon: "🟡" };
  } else {
    return { label: "已完成", class: "bg-gray-100 text-gray-600", icon: "⚪" };
  }
});
</script>

<template>
  <span
    class="activity-status-badge inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-xs font-medium"
    :class="status.class"
  >
    <span>{{ status.icon }}</span>
    <span>{{ status.label }}</span>
  </span>
</template>

<style scoped>
.activity-status-badge {
  white-space: nowrap;
}
</style>
