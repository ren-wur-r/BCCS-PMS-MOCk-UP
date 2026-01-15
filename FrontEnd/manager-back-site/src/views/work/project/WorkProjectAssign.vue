<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { formatDate } from "@/utils/timeFormatter";
import { loadCapacityRecords } from "@/stores/capacityStore";
import { getMemberAssignedHoursMap } from "@/utils/capacityUtils";

interface ProjectAssignmentItem {
  assignmentId: number;
  projectId: number;
  projectName: string;
  companyName: string;
  regionName: string;
  departmentName: string;
  assigneeName: string;
  status: "pending" | "assigned";
  createdAt: string;
}

const router = useRouter();
const employeeInfoStore = useEmployeeInfoStore();
const assignments = ref<ProjectAssignmentItem[]>([]);
const assignmentStorageKey = "cache.work.project.assignments";
const assignmentEventName = "work-project-assignments-updated";
const readAssignments = (): ProjectAssignmentItem[] => {
  const raw = localStorage.getItem(assignmentStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const writeAssignments = (nextList: ProjectAssignmentItem[]) => {
  assignments.value = nextList;
  localStorage.setItem(assignmentStorageKey, JSON.stringify(nextList));
  window.dispatchEvent(new CustomEvent(assignmentEventName));
};

const ensureAssignmentSeed = () => {
  const existing = readAssignments();
  if (existing.length > 0) {
    assignments.value = existing;
    return;
  }
  const snapshotRaw = localStorage.getItem("cache.work.project.snapshot");
  const snapshot = snapshotRaw ? JSON.parse(snapshotRaw) : [];
  const firstProject = snapshot[0];
  const seedProject = firstProject ?? {
    id: 1,
    name: "雲安維運-EDR",
    companyName: "王品有限公司",
    startTime: "2026-01-01",
    endTime: "2026-01-31",
  };
  const seedAssignment: ProjectAssignmentItem = {
    assignmentId: Date.now(),
    projectId: seedProject.id,
    projectName: seedProject.name,
    companyName: seedProject.companyName,
    regionName: employeeInfoStore.managerRegionName || "中區",
    departmentName: employeeInfoStore.managerDepartmentName || "策略執行部",
    assigneeName: employeeInfoStore.employeeName || "目前使用者",
    status: "pending",
    createdAt: new Date().toISOString(),
  };
  writeAssignments([seedAssignment]);
};

const refreshAssignments = () => {
  const existing = readAssignments();
  if (existing.length > 0) {
    assignments.value = existing;
    return;
  }
  ensureAssignmentSeed();
};

const pendingCount = computed(
  () => assignments.value.filter((item) => item.status === "pending").length
);
const getStatusLabel = (status: ProjectAssignmentItem["status"]) =>
  status === "pending" ? "待指派" : "已指派";

const getStatusClass = (status: ProjectAssignmentItem["status"]) =>
  status === "pending" ? "bg-amber-50 text-amber-700" : "bg-emerald-50 text-emerald-700";

const capacityRecords = computed(() => loadCapacityRecords());
const capacityMap = computed(() => {
  const map = new Map<string, { weeklyHours: number }>();
  capacityRecords.value.forEach((record) => {
    map.set(record.employeeName, { weeklyHours: record.weeklyHours });
  });
  return map;
});
const assignedHoursMap = computed(() => getMemberAssignedHoursMap());
const getRemainingHours = (name: string) => {
  const capacity = capacityMap.value.get(name)?.weeklyHours ?? 0;
  if (!capacity) return "-";
  const assigned = assignedHoursMap.value.get(name) || 0;
  return Math.max(capacity - assigned, 0);
};

const clickView = (projectId: number) => {
  router.push(`/work/project/detail/${projectId}`);
};

onMounted(() => {
  refreshAssignments();
  window.addEventListener(assignmentEventName, refreshAssignments as EventListener);
});

onBeforeUnmount(() => {
  window.removeEventListener(assignmentEventName, refreshAssignments as EventListener);
});
</script>

<template>
  <div class="flex flex-col h-full p-4 gap-4">
    <div class="flex items-center justify-between mb-3"></div>
    <div class="flex flex-col bg-white rounded-lg p-6 gap-4 w-full">
      <div
        v-if="pendingCount > 0"
        class="rounded-lg border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700"
      >
        目前有 {{ pendingCount }} 筆待指派專案需要處理。
      </div>
      <div v-if="assignments.length === 0" class="text-sm text-gray-500">
        尚未有待指派的專案。
      </div>
      <div v-else class="overflow-hidden">
        <table class="table-base table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-40">專案名稱</th>
              <th class="text-start w-32">客戶</th>
              <th class="text-start w-24">所屬區域</th>
              <th class="text-start w-32">所屬部門</th>
              <th class="text-start w-24">指派對象</th>
              <th class="text-start w-28">指派時間</th>
              <th class="text-start w-20">剩餘量能</th>
              <th class="text-start w-20">狀態</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="item in assignments"
              :key="item.assignmentId"
              class="cursor-pointer hover:bg-gray-50 transition-colors"
              role="button"
              tabindex="0"
              @click="clickView(item.projectId)"
            >
              <td class="text-start">{{ item.projectName }}</td>
              <td class="text-start">{{ item.companyName }}</td>
              <td class="text-start">{{ item.regionName }}</td>
              <td class="text-start">{{ item.departmentName }}</td>
              <td class="text-start">{{ item.assigneeName }}</td>
              <td class="text-start">{{ formatDate(item.createdAt) || "-" }}</td>
              <td class="text-start">{{ getRemainingHours(item.assigneeName) }}</td>
              <td class="text-start">
                <span
                  class="inline-flex min-w-[72px] items-center justify-center rounded-md px-2 py-1 text-sm"
                  :class="getStatusClass(item.status)"
                >
                  {{ getStatusLabel(item.status) }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
