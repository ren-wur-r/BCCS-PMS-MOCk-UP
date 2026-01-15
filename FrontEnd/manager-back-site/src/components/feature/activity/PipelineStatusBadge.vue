<script setup lang="ts">
import { computed } from "vue";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";

interface Props {
  status: DbAtomPipelineStatusEnum | null;
}

const props = defineProps<Props>();

const statusClass = computed(() => {
  switch (props.status) {
    case DbAtomPipelineStatusEnum.Hyphen:
      return "bg-gray-100 text-gray-700";
    case DbAtomPipelineStatusEnum.Opened:
      return "bg-blue-100 text-blue-700";
    case DbAtomPipelineStatusEnum.Clicked:
      return "bg-indigo-100 text-indigo-700";
    case DbAtomPipelineStatusEnum.TransferredToSales:
      return "bg-amber-100 text-amber-700";
    case DbAtomPipelineStatusEnum.CompletedSales:
      return "bg-amber-200 text-amber-800";
    case DbAtomPipelineStatusEnum.TransferredToBusiness:
      return "bg-purple-100 text-purple-700";
    case DbAtomPipelineStatusEnum.Business10Percent:
      return "bg-blue-200 text-blue-800";
    case DbAtomPipelineStatusEnum.Business30Percent:
      return "bg-indigo-200 text-indigo-800";
    case DbAtomPipelineStatusEnum.Business70Percent:
      return "bg-amber-200 text-amber-800";
    case DbAtomPipelineStatusEnum.Business100Percent:
      return "bg-emerald-200 text-emerald-800";
    case DbAtomPipelineStatusEnum.BusinessFailed:
      return "bg-red-100 text-red-700";
    case DbAtomPipelineStatusEnum.TransferredToProject:
      return "bg-green-200 text-green-800";
    default:
      return "bg-gray-100 text-gray-600";
  }
});

const label = computed(() => getPipelineStatusLabel(props.status));
</script>

<template>
  <span
    class="status-badge inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium"
    :class="statusClass"
  >
    {{ label }}
  </span>
</template>

<style scoped>
.status-badge {
  white-space: nowrap;
}
</style>
