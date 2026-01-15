<script setup lang="ts">
interface KpiItem {
  label: string;
  value: number;
  colorClass: string;
}

interface Props {
  kpis: KpiItem[];
}

defineProps<Props>();
</script>

<template>
  <div class="grid grid-cols-2 sm:grid-cols-4 gap-4">
    <div
      v-for="(kpi, index) in kpis"
      :key="index"
      class="kpi-card bg-white p-4 rounded-lg shadow hover:shadow-md transition-shadow"
    >
      <div class="kpi-label text-sm text-gray-500 mb-1">{{ kpi.label }}</div>
      <div class="kpi-value text-2xl font-bold" :class="kpi.colorClass">
        {{ kpi.value.toLocaleString() }}
      </div>
    </div>
  </div>
</template>

<style scoped>
.kpi-card {
  transition: all 0.2s ease;
}

.kpi-card:hover {
  transform: translateY(-2px);
}

.kpi-label {
  font-size: 0.875rem;
  line-height: 1.25rem;
}

.kpi-value {
  font-size: 1.5rem;
  line-height: 2rem;
}

@media (min-width: 640px) {
  .kpi-value {
    font-size: 1.875rem;
    line-height: 2.25rem;
  }
}
</style>
