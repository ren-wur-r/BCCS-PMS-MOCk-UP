<script setup lang="ts">
import { computed, ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import AnnouncementBoard from "./AnnouncementBoard.vue";
import QuickWorkLog from "@/components/feature/work/QuickWorkLog.vue";
import { 
  mbsDashboardHttpGetInfo, 
  type MbsDashboardHttpGetInfoRspMdl 
} from "@/services/pms-http/dashboard/dashboardHttp";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { normalizeDashboardData } from "@/utils/buildDashboardMockData";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { mockDataSets } from "@/services/mockApi/mockDataSets";

const dashboardData = ref<MbsDashboardHttpGetInfoRspMdl | null>(null);
const employeeInfoStore = useEmployeeInfoStore();
const router = useRouter();
const roleName = computed(() => employeeInfoStore.effectiveRoleName || "");
const dashboardTab = ref<"overview" | "worklog">("overview");
const isTeleSales = computed(() => roleName.value.includes("電銷人員"));
const isActivityStaff = computed(() => roleName.value.includes("活動人員"));
const isProductManager = computed(() => roleName.value.includes("產品經理"));
const isProjectStaff = computed(() => !isTeleSales.value && !isActivityStaff.value);

const teleSalesLeads = computed(() => mockDataSets.teleSalesLeads ?? []);
const teleSalesTotal = computed(() => teleSalesLeads.value.length);
const teleSalesStatusCounts = computed(() => {
  const counts: Record<string, number> = {};
  teleSalesLeads.value.forEach((lead) => {
    counts[lead.status] = (counts[lead.status] ?? 0) + 1;
  });
  return counts;
});
const teleSalesIndustryTop = computed(() => {
  const counts: Record<string, number> = {};
  teleSalesLeads.value.forEach((lead) => {
    counts[lead.industry] = (counts[lead.industry] ?? 0) + 1;
  });
  return Object.entries(counts)
    .map(([industry, count]) => ({ industry, count }))
    .sort((a, b) => b.count - a.count)
    .slice(0, 3);
});
const teleSalesBlacklistCount = computed(
  () => teleSalesStatusCounts.value["不再聯繫"] ?? 0
);

const activityTotal = computed(() => mockDataSets.crmActivities.length);
const activityKindCounts = computed(() => {
  const counts: Record<number, number> = {};
  mockDataSets.crmActivities.forEach((item) => {
    counts[item.kind] = (counts[item.kind] ?? 0) + 1;
  });
  return counts;
});
const activityTransferTotal = computed(() =>
  mockDataSets.crmActivities.reduce(
    (sum, item) => sum + (item.transferredToSalesCount ?? 0),
    0
  )
);
const activityUpcoming = computed(() => {
  const now = new Date();
  return mockDataSets.crmActivities
    .filter((item) => new Date(item.startTime) >= now)
    .sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime())
    .slice(0, 5);
});
const activityNotTransferred = computed(() =>
  mockDataSets.crmActivities.filter((item) => (item.transferredToSalesCount ?? 0) === 0)
);
const formatActivityDate = (value: string) => {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return date.toLocaleDateString();
};

const presentationCount = computed(() => {
  const presentationStages = new Set([8, 9, 10]);
  return mockDataSets.crmPipelines.filter((item) =>
    presentationStages.has(item.status)
  ).length;
});

const personalProjectLimit = computed(() => dashboardData.value?.PersonalProjectCount || 0);
const personalProjects = computed(() => {
  const list = mockDataSets.workProjects ?? [];
  if (!personalProjectLimit.value) return list;
  return list.slice(0, personalProjectLimit.value);
});
const riskStatusSet = new Set([
  DbAtomEmployeeProjectStatusEnum.Delayed,
  DbAtomEmployeeProjectStatusEnum.AtRisk,
  DbAtomEmployeeProjectStatusEnum.OnSchedule,
]);
const riskProjects = computed(() =>
  personalProjects.value.filter((project) => riskStatusSet.has(project.status))
);
const personalProjectCounts = computed(() => {
  const counts = {
    total: 0,
    delayed: 0,
    atRisk: 0,
    onSchedule: 0,
  };
  riskProjects.value.forEach((project) => {
    if (project.status === DbAtomEmployeeProjectStatusEnum.Delayed) counts.delayed += 1;
    if (project.status === DbAtomEmployeeProjectStatusEnum.AtRisk) counts.atRisk += 1;
    if (project.status === DbAtomEmployeeProjectStatusEnum.OnSchedule) counts.onSchedule += 1;
  });
  counts.total = counts.delayed + counts.atRisk + counts.onSchedule;
  return counts;
});
const selectedRiskStatus = ref<DbAtomEmployeeProjectStatusEnum | "all">("all");
const selectRiskStatus = (status: DbAtomEmployeeProjectStatusEnum | "all") => {
  selectedRiskStatus.value = status;
};
const riskStatusLabelMap: Record<DbAtomEmployeeProjectStatusEnum, string> = {
  [DbAtomEmployeeProjectStatusEnum.Undefined]: "未定義",
  [DbAtomEmployeeProjectStatusEnum.NotAssigned]: "未指派",
  [DbAtomEmployeeProjectStatusEnum.NotStarted]: "未開始",
  [DbAtomEmployeeProjectStatusEnum.OnSchedule]: "如期",
  [DbAtomEmployeeProjectStatusEnum.AtRisk]: "注意",
  [DbAtomEmployeeProjectStatusEnum.Delayed]: "延遲",
  [DbAtomEmployeeProjectStatusEnum.Completed]: "已完成",
};
const selectedRiskLabel = computed(() =>
  selectedRiskStatus.value === "all"
    ? "全部"
    : riskStatusLabelMap[selectedRiskStatus.value]
);
const filteredProjectsByRisk = computed(() => {
  if (selectedRiskStatus.value === "all") return riskProjects.value;
  return personalProjects.value.filter((project) => project.status === selectedRiskStatus.value);
});
const riskProjectsPreview = computed(() => filteredProjectsByRisk.value.slice(0, 5));
const handleProjectClick = (projectId?: number) => {
  if (!projectId) return;
  router.push(`/work/project/detail/${projectId}`);
};

onMounted(async () => {
  try {
    const res = await mbsDashboardHttpGetInfo({});
    dashboardData.value = normalizeDashboardData(res);
  } catch (e) {
    console.error(e);
    dashboardData.value = normalizeDashboardData(null);
  }
});

const getPersonalRiskCount = (status: DbAtomEmployeeProjectStatusEnum) => {
  return dashboardData.value?.PersonalProjectRiskStatistics?.find(x => x.Status === status)?.Count || 0;
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

    <div v-if="dashboardTab === 'overview'" class="flex flex-col gap-6">
      <div v-if="isTeleSales" class="bg-white rounded-lg shadow p-6">
      <h3 class="text-lg font-bold text-gray-800 mb-4">電銷名單概況</h3>
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div class="bg-blue-50 border-l-4 border-blue-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-blue-700 font-bold">名單總數</span>
            <span class="text-2xl font-bold text-blue-700">{{ teleSalesTotal }}</span>
          </div>
        </div>
        <div class="bg-gray-50 border-l-4 border-gray-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-gray-700 font-bold">待聯繫</span>
            <span class="text-2xl font-bold text-gray-700">{{ teleSalesStatusCounts["待聯繫"] || 0 }}</span>
          </div>
        </div>
        <div class="bg-amber-50 border-l-4 border-amber-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-amber-700 font-bold">已約訪</span>
            <span class="text-2xl font-bold text-amber-700">{{ teleSalesStatusCounts["已約訪"] || 0 }}</span>
          </div>
        </div>
        <div class="bg-rose-50 border-l-4 border-rose-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-rose-700 font-bold">黑名單</span>
            <span class="text-2xl font-bold text-rose-700">{{ teleSalesBlacklistCount }}</span>
          </div>
        </div>
      </div>

      <div class="mt-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="bg-gray-50 rounded-lg p-4">
          <h4 class="text-sm font-semibold text-gray-700 mb-3">名單狀態分布</h4>
          <div class="space-y-2 text-sm text-gray-600">
            <div class="flex justify-between">
              <span>已聯繫</span>
              <span>{{ teleSalesStatusCounts["已聯繫"] || 0 }}</span>
            </div>
            <div class="flex justify-between">
              <span>已派送</span>
              <span>{{ teleSalesStatusCounts["已派送"] || 0 }}</span>
            </div>
            <div class="flex justify-between">
              <span>不再聯繫</span>
              <span>{{ teleSalesStatusCounts["不再聯繫"] || 0 }}</span>
            </div>
          </div>
        </div>
        <div class="bg-gray-50 rounded-lg p-4">
          <h4 class="text-sm font-semibold text-gray-700 mb-3">產業 Top 3</h4>
          <div class="space-y-2 text-sm text-gray-600">
            <div v-for="item in teleSalesIndustryTop" :key="item.industry" class="flex justify-between">
              <span>{{ item.industry }}</span>
              <span>{{ item.count }}</span>
            </div>
            <div v-if="teleSalesIndustryTop.length === 0" class="text-gray-400">目前無資料</div>
          </div>
        </div>
      </div>
    </div>

      <div v-else-if="isActivityStaff" class="bg-white rounded-lg shadow p-6">
      <h3 class="text-lg font-bold text-gray-800 mb-4">活動管理概況</h3>
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div class="bg-indigo-50 border-l-4 border-indigo-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-indigo-700 font-bold">活動總數</span>
            <span class="text-2xl font-bold text-indigo-700">{{ activityTotal }}</span>
          </div>
        </div>
        <div class="bg-emerald-50 border-l-4 border-emerald-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-emerald-700 font-bold">實體活動</span>
            <span class="text-2xl font-bold text-emerald-700">{{ activityKindCounts[1] || 0 }}</span>
          </div>
        </div>
        <div class="bg-sky-50 border-l-4 border-sky-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-sky-700 font-bold">線上活動</span>
            <span class="text-2xl font-bold text-sky-700">{{ activityKindCounts[2] || 0 }}</span>
          </div>
        </div>
        <div class="bg-amber-50 border-l-4 border-amber-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-amber-700 font-bold">轉電銷名單</span>
            <span class="text-2xl font-bold text-amber-700">{{ activityTransferTotal }}</span>
          </div>
        </div>
      </div>

      <div class="mt-6 grid grid-cols-1 lg:grid-cols-2 gap-4">
        <div class="bg-white rounded-lg border border-gray-200 p-4">
          <div class="flex items-center justify-between mb-3">
            <h4 class="text-sm font-semibold text-gray-700">即將到來的活動</h4>
            <span class="text-xs text-gray-500">{{ activityUpcoming.length }} 筆</span>
          </div>
          <div class="overflow-x-auto">
            <table class="table-base table-fixed table-sticky w-full min-w-[520px]">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-40">活動名稱</th>
                  <th class="text-start w-24">日期</th>
                  <th class="text-start w-28">形式</th>
                  <th class="text-start">地點</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in activityUpcoming" :key="item.id" class="border border-gray-300">
                  <td class="text-start">{{ item.name }}</td>
                  <td class="text-start">{{ formatActivityDate(item.startTime) }}</td>
                  <td class="text-start">{{ item.kind === 1 ? "實體" : "線上" }}</td>
                  <td class="text-start">{{ item.location }}</td>
                </tr>
                <tr v-if="activityUpcoming.length === 0">
                  <td class="text-center text-sm text-gray-500 py-4" colspan="4">目前無即將到來的活動</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div class="bg-white rounded-lg border border-gray-200 p-4">
          <div class="flex items-center justify-between mb-3">
            <h4 class="text-sm font-semibold text-gray-700">尚未轉電銷之活動</h4>
            <span class="text-xs text-gray-500">{{ activityNotTransferred.length }} 筆</span>
          </div>
          <div class="overflow-x-auto">
            <table class="table-base table-fixed table-sticky w-full min-w-[520px]">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-40">活動名稱</th>
                  <th class="text-start w-24">日期</th>
                  <th class="text-start w-28">預期名單</th>
                  <th class="text-start">已轉名單</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in activityNotTransferred" :key="item.id" class="border border-gray-300">
                  <td class="text-start">{{ item.name }}</td>
                  <td class="text-start">{{ formatActivityDate(item.startTime) }}</td>
                  <td class="text-start">{{ item.expectedLeadCount }}</td>
                  <td class="text-start">{{ item.transferredToSalesCount ?? 0 }}</td>
                </tr>
                <tr v-if="activityNotTransferred.length === 0">
                  <td class="text-center text-sm text-gray-500 py-4" colspan="4">目前皆已轉電銷</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

      <div v-else-if="isProjectStaff" class="bg-white rounded-lg shadow p-6">
      <h3 class="text-lg font-bold text-gray-800 mb-4">我的專案概況</h3>
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div
          class="bg-gray-50 border-l-4 border-gray-500 p-4 cursor-pointer transition hover:bg-gray-100"
          :class="selectedRiskStatus === 'all' ? 'ring-2 ring-gray-300' : ''"
          @click="selectRiskStatus('all')"
        >
          <div class="flex justify-between items-center">
            <span class="text-gray-700 font-bold">總數</span>
            <span class="text-2xl font-bold text-gray-700">{{ personalProjectCounts.total }}</span>
          </div>
        </div>
        <div
          class="bg-red-50 border-l-4 border-red-500 p-4 cursor-pointer transition hover:bg-red-100"
          :class="selectedRiskStatus === DbAtomEmployeeProjectStatusEnum.Delayed ? 'ring-2 ring-red-200' : ''"
          @click="selectRiskStatus(DbAtomEmployeeProjectStatusEnum.Delayed)"
        >
          <div class="flex justify-between items-center">
            <span class="text-red-700 font-bold">延遲</span>
            <span class="text-2xl font-bold text-red-700">{{ personalProjectCounts.delayed }}</span>
          </div>
        </div>
        <div
          class="bg-yellow-50 border-l-4 border-yellow-500 p-4 cursor-pointer transition hover:bg-yellow-100"
          :class="selectedRiskStatus === DbAtomEmployeeProjectStatusEnum.AtRisk ? 'ring-2 ring-yellow-200' : ''"
          @click="selectRiskStatus(DbAtomEmployeeProjectStatusEnum.AtRisk)"
        >
          <div class="flex justify-between items-center">
            <span class="text-yellow-700 font-bold">注意</span>
            <span class="text-2xl font-bold text-yellow-700">{{ personalProjectCounts.atRisk }}</span>
          </div>
        </div>
        <div
          class="bg-green-50 border-l-4 border-green-500 p-4 cursor-pointer transition hover:bg-green-100"
          :class="selectedRiskStatus === DbAtomEmployeeProjectStatusEnum.OnSchedule ? 'ring-2 ring-green-200' : ''"
          @click="selectRiskStatus(DbAtomEmployeeProjectStatusEnum.OnSchedule)"
        >
          <div class="flex justify-between items-center">
            <span class="text-green-700 font-bold">如期</span>
            <span class="text-2xl font-bold text-green-700">{{ personalProjectCounts.onSchedule }}</span>
          </div>
        </div>
      </div>

      <div class="mt-6 rounded-lg border border-gray-200 p-4">
        <div class="flex items-center justify-between mb-3">
          <h4 class="text-sm font-semibold text-gray-700">風險清單</h4>
          <span class="text-xs text-gray-500">顯示：{{ selectedRiskLabel }}</span>
        </div>
        <div class="overflow-x-auto">
          <table class="table-base table-fixed table-sticky w-full min-w-[520px]">
            <thead class="bg-gray-800 text-white">
              <tr>
                <th class="text-start w-44">專案名稱</th>
                <th class="text-start w-36">客戶</th>
                <th class="text-start w-24">部門</th>
                <th class="text-start w-32">期間</th>
                <th class="text-start w-24">狀態</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="project in riskProjectsPreview"
                :key="project.id"
                class="border border-gray-300 cursor-pointer hover:bg-cyan-50"
                role="button"
                tabindex="0"
                @click="handleProjectClick(project.id)"
                @keydown.enter="handleProjectClick(project.id)"
              >
                <td class="text-start">{{ project.name }}</td>
                <td class="text-start">{{ project.companyName }}</td>
                <td class="text-start">{{ project.departmentName || "-" }}</td>
                <td class="text-start">{{ project.startTime }} ~ {{ project.endTime }}</td>
                <td class="text-start">
                  {{ riskStatusLabelMap[project.status as DbAtomEmployeeProjectStatusEnum] || "-" }}
                </td>
              </tr>
              <tr v-if="filteredProjectsByRisk.length === 0">
                <td class="text-center text-sm text-gray-500 py-4" colspan="5">目前無符合的專案</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div v-if="isProductManager" class="mt-4 grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="bg-cyan-50 border-l-4 border-cyan-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-cyan-700 font-bold">陪同簡報次數</span>
            <span class="text-2xl font-bold text-cyan-700">{{ presentationCount }}</span>
          </div>
          <div class="mt-2 text-xs text-cyan-600">本月</div>
        </div>
        <div class="bg-purple-50 border-l-4 border-purple-500 p-4">
          <div class="flex justify-between items-center">
            <span class="text-purple-700 font-bold">產品線專案數</span>
            <span class="text-2xl font-bold text-purple-700">{{ dashboardData?.DepartmentProjectCount || 0 }}</span>
          </div>
        </div>
      </div>
      </div>
    </div>

    <div v-else class="flex flex-col gap-6">
      <QuickWorkLog />
    </div>
  </div>
</template>
