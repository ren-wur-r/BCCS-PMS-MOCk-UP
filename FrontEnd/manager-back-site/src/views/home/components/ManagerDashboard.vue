<script setup lang="ts">
import { computed, ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import AnnouncementBoard from "./AnnouncementBoard.vue";
import QuickWorkLog from "@/components/feature/work/QuickWorkLog.vue";
import {
  mbsDashboardHttpGetInfo,
  type MbsDashboardHttpGetInfoRspMdl,
} from "@/services/pms-http/dashboard/dashboardHttp";
import { useMockDataStore } from "@/stores/mockDataStore";
import { normalizeDashboardData } from "@/utils/buildDashboardMockData";
import {
  loadProjectTemplateSettings,
  getStageTemplatesByServiceItems,
} from "@/stores/projectTemplateSettings";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
import { loadCapacityRecords } from "@/stores/capacityStore";
import { getMemberAssignedHoursMap } from "@/utils/capacityUtils";

const router = useRouter();
const employeeInfoStore = useEmployeeInfoStore();
const dashboardData = ref<MbsDashboardHttpGetInfoRspMdl | null>(null);
const mockStore = useMockDataStore();
const selectedRiskKey = ref<string>("all");
const selectedMemberName = ref<string>("");
const selectedMemberTab = ref<"projects" | "certs">("projects");
const dashboardTab = ref<"overview" | "worklog">("overview");

onMounted(async () => {
  try {
    const res = await mbsDashboardHttpGetInfo({});
    dashboardData.value = normalizeDashboardData(res);
  } catch (e) {
    console.error(e);
    dashboardData.value = normalizeDashboardData(null);
  }
});

const getProjectSnapshot = () => {
  const snapshotRaw = localStorage.getItem("cache.work.project.snapshot");
  if (snapshotRaw) {
    try {
      const parsed = JSON.parse(snapshotRaw);
      if (Array.isArray(parsed)) {
        return parsed;
      }
    } catch {
      return mockDataSets.workProjects;
    }
  }
  return mockDataSets.workProjects;
};

const getProjectDetailMembers = (projectId: number) => {
  const detailKey = `cache.work.project.detail.${projectId}`;
  const raw = localStorage.getItem(detailKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed?.employeeProjectMemberList)
      ? parsed.employeeProjectMemberList
      : [];
  } catch {
    return [];
  }
};

const departmentContext = computed(() => {
  const devRole = employeeInfoStore.devRoleName;
  if (devRole === "工程部中部") {
    return { departmentName: "工程部", regionName: "中區" };
  }
  if (devRole === "顧問中部") {
    return { departmentName: "中區顧問部", regionName: "中區" };
  }
  return {
    departmentName: employeeInfoStore.managerDepartmentName || "",
    regionName: employeeInfoStore.managerRegionName || "",
  };
});
const departmentName = computed(() => departmentContext.value.departmentName);
const departmentRegionName = computed(() => departmentContext.value.regionName);
const departmentProjects = computed(() => {
  const base = getProjectSnapshot();
  if (!departmentName.value) return base;
  return base.filter((project) => {
    const members = getProjectDetailMembers(project.id);
    if (members.length === 0) return true;
    return members.some((member) => {
      const deptMatch = (member.managerDepartmentName || "").includes(departmentName.value);
      if (!deptMatch) return false;
      if (!departmentRegionName.value) return true;
      return (member.managerRegionName || "").includes(departmentRegionName.value);
    });
  });
});

const templateSettings = loadProjectTemplateSettings();
const getProjectServiceItemIds = (projectId: number): number[] => {
  const key = `workProjectServiceItems:${projectId}`;
  const raw = localStorage.getItem(key);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const getProjectStageNames = (projectId: number): string[] => {
  const serviceItemIds = getProjectServiceItemIds(projectId);
  if (serviceItemIds.length === 0) return [];
  const templates = getStageTemplatesByServiceItems(templateSettings, serviceItemIds);
  const names: string[] = [];
  templates.forEach((template) => {
    template.stages.forEach((stage) => {
      if (!names.includes(stage.stageName)) names.push(stage.stageName);
    });
  });
  return names;
};

const projectStageMap = computed(() => {
  const map = new Map<number, { current: string; next: string }>();
  departmentProjects.value.forEach((project) => {
    const names = getProjectStageNames(project.id);
    map.set(project.id, {
      current: names[0] || "-",
      next: names[1] || "-",
    });
  });
  return map;
});

const riskCards = computed(() => [
  {
    key: "not-started",
    label: "未開始",
    status: DbAtomEmployeeProjectStatusEnum.NotStarted,
    className: "bg-slate-50 border-slate-400 text-slate-700",
  },
  {
    key: "on-schedule",
    label: "如期",
    status: DbAtomEmployeeProjectStatusEnum.OnSchedule,
    className: "bg-emerald-50 border-emerald-500 text-emerald-700",
  },
  {
    key: "at-risk",
    label: "注意",
    status: DbAtomEmployeeProjectStatusEnum.AtRisk,
    className: "bg-yellow-50 border-yellow-500 text-yellow-700",
  },
  {
    key: "delayed",
    label: "延遲",
    status: DbAtomEmployeeProjectStatusEnum.Delayed,
    className: "bg-rose-50 border-rose-500 text-rose-700",
  },
]);

const riskCounts = computed(() => {
  const counts = new Map<number, number>();
  riskCards.value.forEach((card) => counts.set(card.status, 0));
  departmentProjects.value.forEach((project) => {
    const current = counts.get(project.status) ?? 0;
    counts.set(project.status, current + 1);
  });
  return counts;
});

const filteredProjects = computed(() => {
  if (selectedRiskKey.value === "all") return departmentProjects.value;
  const card = riskCards.value.find((item) => item.key === selectedRiskKey.value);
  if (!card) return departmentProjects.value;
  return departmentProjects.value.filter((project) => project.status === card.status);
});

const clickRiskCard = (key: string) => {
  selectedRiskKey.value = selectedRiskKey.value === key ? "all" : key;
};

const departmentMembers = computed(() => {
  if (!departmentName.value) return orgMemberDirectory;
  return orgMemberDirectory.filter((member) => {
    if (member.departmentName !== departmentName.value) return false;
    if (!departmentRegionName.value) return true;
    return member.regionName === departmentRegionName.value;
  });
});

const memberProjectCounts = computed(() => {
  const counts = new Map<string, number>();
  departmentMembers.value.forEach((member) => counts.set(member.name, 0));
  departmentProjects.value.forEach((project) => {
    const members = getProjectDetailMembers(project.id);
    members.forEach((member) => {
      if (!counts.has(member.employeeName)) return;
      counts.set(member.employeeName, (counts.get(member.employeeName) ?? 0) + 1);
    });
  });
  return counts;
});

const selectedMemberProjects = computed(() => {
  if (!selectedMemberName.value) return [];
  return departmentProjects.value.filter((project) => {
    const members = getProjectDetailMembers(project.id);
    return members.some((member) => member.employeeName === selectedMemberName.value);
  });
});

const selectedMemberCertificates = computed(() =>
  mockStore.certificates.filter((item) => item.employeeName === selectedMemberName.value)
);

const capacityRecords = computed(() => loadCapacityRecords());
const departmentCapacityRecords = computed(() => {
  if (!departmentName.value) return capacityRecords.value;
  return capacityRecords.value.filter((item) => {
    if (item.departmentName !== departmentName.value) return false;
    if (!departmentRegionName.value) return true;
    return item.regionName === departmentRegionName.value;
  });
});
const assignedHoursMap = computed(() => getMemberAssignedHoursMap());
const departmentWeeklyCapacity = computed(() =>
  departmentCapacityRecords.value.reduce((sum, item) => sum + (item.weeklyHours || 0), 0)
);
const departmentAssignedHours = computed(() => {
  return departmentCapacityRecords.value.reduce((sum, item) => {
    return sum + (assignedHoursMap.value.get(item.employeeName) || 0);
  }, 0);
});
const overloadedMembers = computed(() =>
  departmentCapacityRecords.value.filter((item) => {
    const assigned = assignedHoursMap.value.get(item.employeeName) || 0;
    return item.weeklyHours > 0 && assigned > item.weeklyHours;
  })
);
const idleMembers = computed(() =>
  departmentCapacityRecords.value.filter((item) => {
    const assigned = assignedHoursMap.value.get(item.employeeName) || 0;
    return item.weeklyHours > 0 && assigned === 0;
  })
);

const clickMember = (name: string) => {
  selectedMemberName.value = name;
  selectedMemberTab.value = "projects";
};

const clickProjectRow = (projectId: number) => {
  router.push(`/work/project/detail/${projectId}`);
};
</script>

<template>
  <div class="flex flex-col gap-6">
    <AnnouncementBoard />
    <div class="flex gap-3 border-b border-gray-200">
      <button
        class="px-4 py-2 text-sm font-semibold rounded-t-md"
        :class="dashboardTab === 'overview' ? 'bg-white text-blue-600 border border-b-white border-gray-200' : 'text-gray-500'"
        @click="dashboardTab = 'overview'"
      >
        專案概況
      </button>
      <button
        class="px-4 py-2 text-sm font-semibold rounded-t-md"
        :class="dashboardTab === 'worklog' ? 'bg-white text-blue-600 border border-b-white border-gray-200' : 'text-gray-500'"
        @click="dashboardTab = 'worklog'"
      >
        工作日誌
      </button>
    </div>

    <div
      v-if="dashboardTab === 'overview'"
      class="flex flex-col gap-6"
      data-annotation-scope="dashboard-overview"
    >
      <!-- Department Overview -->
      <div class="grid grid-cols-1 lg:grid-cols-4 gap-6">
        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">部門專案總數</h3>
          <div class="text-3xl font-bold text-indigo-600">
            {{ dashboardData?.DepartmentProjectCount || 0 }}
          </div>
        </div>
        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">部門量能概況</h3>
          <div class="flex items-end justify-between">
            <div>
              <div class="text-xs text-gray-400">本週總量能</div>
              <div class="text-2xl font-bold text-emerald-600">
                {{ departmentWeeklyCapacity }}
              </div>
            </div>
            <div class="text-right">
              <div class="text-xs text-gray-400">已分派工時</div>
              <div class="text-lg font-semibold text-gray-700">
                {{ departmentAssignedHours }}
              </div>
            </div>
          </div>
        </div>
        <div class="bg-white rounded-lg shadow p-6 lg:col-span-2">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-lg font-bold text-gray-800">部門專案風險等級</h3>
            <span class="text-xs text-gray-500">點擊卡片可篩選專案列表</span>
          </div>
          <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
            <button
              v-for="card in riskCards"
              :key="card.key"
              type="button"
              class="rounded-lg border-l-4 p-4 text-left transition"
              :class="card.className"
              @click="clickRiskCard(card.key)"
            >
              <div class="flex items-center justify-between">
                <span class="font-semibold">{{ card.label }}</span>
                <span class="text-2xl font-bold">
                  {{ riskCounts.get(card.status) || 0 }}
                </span>
              </div>
            </button>
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-lg font-bold text-gray-800">部門專案列表</h3>
          <span class="text-xs text-gray-500">
            {{ selectedRiskKey === "all" ? "全部專案" : "已篩選" }} ·
            {{ filteredProjects.length }} 筆
          </span>
        </div>
      <div class="overflow-hidden">
        <table class="table-base table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start">專案名稱</th>
              <th class="text-start">客戶</th>
              <th class="text-start">起始日期</th>
              <th class="text-start">結束日期</th>
              <th class="text-start">目前階段</th>
              <th class="text-start">下一階段</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="item in filteredProjects"
              :key="item.id"
              class="cursor-pointer hover:bg-gray-50 transition-colors"
              role="button"
              tabindex="0"
              @click="clickProjectRow(item.id)"
            >
              <td class="text-start break-words">{{ item.name }}</td>
              <td class="text-start break-words">{{ item.companyName }}</td>
              <td class="text-start">{{ item.startTime }}</td>
              <td class="text-start">{{ item.endTime }}</td>
              <td class="text-start">
                {{ projectStageMap.get(item.id)?.current || "-" }}
              </td>
              <td class="text-start">
                {{ projectStageMap.get(item.id)?.next || "-" }}
              </td>
            </tr>
            <tr v-if="filteredProjects.length === 0">
              <td colspan="6" class="text-center text-sm text-gray-400 py-6">
                目前沒有符合條件的專案
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="bg-white rounded-lg shadow p-6">
      <div class="flex items-center justify-between mb-4">
        <h3 class="text-lg font-bold text-gray-800">量能警示</h3>
        <span class="text-xs text-gray-500">
          超載 {{ overloadedMembers.length }} / 閒置 {{ idleMembers.length }}
        </span>
      </div>
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-4">
        <div class="rounded-lg border border-gray-200 p-4">
          <div class="text-sm font-semibold text-gray-700 mb-3">超載成員</div>
          <div v-if="overloadedMembers.length === 0" class="text-sm text-gray-400">
            目前沒有超載成員
          </div>
          <div v-else class="space-y-2">
            <div
              v-for="member in overloadedMembers"
              :key="member.employeeName"
              class="flex items-center justify-between text-sm text-gray-600"
            >
              <span>{{ member.employeeName }}</span>
              <span class="text-rose-600">
                {{
                  (assignedHoursMap.get(member.employeeName) || 0)
                }}/{{ member.weeklyHours }}
              </span>
            </div>
          </div>
        </div>
        <div class="rounded-lg border border-gray-200 p-4">
          <div class="text-sm font-semibold text-gray-700 mb-3">閒置成員</div>
          <div v-if="idleMembers.length === 0" class="text-sm text-gray-400">
            目前沒有閒置成員
          </div>
          <div v-else class="space-y-2">
            <div
              v-for="member in idleMembers"
              :key="member.employeeName"
              class="flex items-center justify-between text-sm text-gray-600"
            >
              <span>{{ member.employeeName }}</span>
              <span class="text-amber-600">
                {{
                  (assignedHoursMap.get(member.employeeName) || 0)
                }}/{{ member.weeklyHours }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="bg-white rounded-lg shadow p-6">
      <div class="flex items-center justify-between mb-4">
        <h3 class="text-lg font-bold text-gray-800">部門成員名單</h3>
        <span class="text-xs text-gray-500">點擊成員查看專案/證照</span>
      </div>
      <div class="grid grid-cols-1 lg:grid-cols-[1.1fr_1.4fr] gap-4">
        <div class="overflow-hidden">
          <table class="table-base table-sticky w-full">
            <thead class="sticky top-0 bg-white z-10">
              <tr>
                <th class="text-start">成員</th>
                <th class="text-start">專案數</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="member in departmentMembers"
                :key="member.email"
                class="cursor-pointer hover:bg-gray-50 transition-colors"
                role="button"
                tabindex="0"
                @click="clickMember(member.name)"
              >
                  <td class="text-start">{{ member.name }}</td>
                  <td class="text-start">{{ memberProjectCounts.get(member.name) || 0 }}</td>
              </tr>
              <tr v-if="departmentMembers.length === 0">
                <td colspan="3" class="text-center text-sm text-gray-400 py-6">
                  尚未有部門成員資料
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="rounded-lg border border-gray-200 p-4">
          <div class="flex items-center justify-between">
            <h4 class="text-sm font-semibold text-gray-700">
              {{ selectedMemberName || "請選擇成員" }}
            </h4>
            <div class="flex gap-2" v-if="selectedMemberName">
              <button
                type="button"
                class="rounded-md border px-3 py-1 text-xs font-semibold transition"
                :class="
                  selectedMemberTab === 'projects'
                    ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                    : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                "
                @click="selectedMemberTab = 'projects'"
              >
                專案
              </button>
              <button
                type="button"
                class="rounded-md border px-3 py-1 text-xs font-semibold transition"
                :class="
                  selectedMemberTab === 'certs'
                    ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                    : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                "
                @click="selectedMemberTab = 'certs'"
              >
                證照
              </button>
            </div>
          </div>
          <div v-if="!selectedMemberName" class="text-sm text-gray-400 mt-4">
            點擊左側成員以檢視專案與證照資訊。
          </div>
          <div v-else class="mt-4 space-y-3">
            <div v-if="selectedMemberTab === 'projects'">
              <div class="text-xs text-gray-500 mb-2">
                專案數量：{{ selectedMemberProjects.length }}
              </div>
              <ul class="space-y-2">
                <li
                  v-for="item in selectedMemberProjects"
                  :key="item.id"
                  class="rounded-lg border border-gray-200 p-3 text-sm text-gray-700 cursor-pointer hover:bg-gray-50"
                  @click="clickProjectRow(item.id)"
                >
                  {{ item.name }}
                </li>
                <li v-if="selectedMemberProjects.length === 0" class="text-sm text-gray-400">
                  目前沒有指派中的專案
                </li>
              </ul>
            </div>
            <div v-else>
              <div class="text-xs text-gray-500 mb-2">
                證照數量：{{ selectedMemberCertificates.length }}
              </div>
              <ul class="space-y-2">
                <li
                  v-for="(cert, index) in selectedMemberCertificates"
                  :key="`${cert.employeeName}-${index}`"
                  class="rounded-lg border border-gray-200 p-3 text-sm text-gray-700"
                >
                  <div class="font-semibold">{{ cert.certificateName }}</div>
                  <div class="text-xs text-gray-500 mt-1">取得日期：{{ cert.issueDate }}</div>
                </li>
                <li v-if="selectedMemberCertificates.length === 0" class="text-sm text-gray-400">
                  目前沒有證照資料
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      </div>
    </div>

    <div v-else class="flex flex-col gap-6" data-annotation-scope="dashboard-worklog">
      <QuickWorkLog />
    </div>
  </div>
</template>
