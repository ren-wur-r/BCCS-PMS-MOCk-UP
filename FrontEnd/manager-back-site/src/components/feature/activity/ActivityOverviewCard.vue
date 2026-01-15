<script setup lang="ts">
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { getManagerActivityKindLabel } from "@/utils/getManagerActivityKindLabel";
import { formatDateTime } from "@/utils/timeFormatter";
import ActivityStatusBadge from "./ActivityStatusBadge.vue";

interface Props {
  activityName: string;
  activityKind: DbAtomManagerActivityKindEnum;
  startTime: string;
  endTime: string;
  expectedCount: number;
  registeredCount: number;
  transferredCount: number;
  opportunityCount: number;
  phoneCount?: number;
}

const props = defineProps<Props>();
</script>

<template>
  <div class="activity-overview-card bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg p-4 mb-4 border border-blue-100">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
      <div class="flex items-center gap-3">
        <div>
          <div class="flex items-center gap-2 mb-1">
            <h3 class="text-lg font-semibold text-gray-800">{{ activityName }}</h3>
            <ActivityStatusBadge :start-time="startTime" :end-time="endTime" />
          </div>
          <div class="text-sm text-gray-600">
            <span class="font-medium">{{ getManagerActivityKindLabel(activityKind) }}</span>
            <span class="mx-2">|</span>
            <span>{{ formatDateTime(startTime) }} ~ {{ formatDateTime(endTime) }}</span>
          </div>
        </div>
      </div>

      <div class="flex gap-4 text-sm">
        <div class="flex flex-col items-center">
          <span class="text-gray-500 text-xs mb-1">期望</span>
          <span class="text-lg font-bold text-gray-600">{{ expectedCount }}</span>
        </div>
        <div class="flex flex-col items-center">
          <span class="text-gray-500 text-xs mb-1">實際</span>
          <span class="text-lg font-bold text-sky-500">{{ registeredCount }}</span>
        </div>
        <div class="flex flex-col items-center">
          <span class="text-gray-500 text-xs mb-1">轉電</span>
          <span class="text-lg font-bold text-indigo-500">{{ transferredCount }}</span>
        </div>
        <div v-if="phoneCount !== undefined" class="flex flex-col items-center">
          <span class="text-gray-500 text-xs mb-1">撥打</span>
          <span class="text-lg font-bold text-amber-500">{{ phoneCount }}</span>
        </div>
        <div class="flex flex-col items-center">
          <span class="text-gray-500 text-xs mb-1">商機</span>
          <span class="text-lg font-bold text-green-500">{{ opportunityCount }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.activity-overview-card {
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06);
}
</style>
