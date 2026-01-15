<script setup lang="ts">
import { ref, onMounted, watch, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import AnnouncementBoard from "./AnnouncementBoard.vue";
import { 
  mbsDashboardHttpGetInfo, 
  type MbsDashboardHttpGetInfoRspMdl, 
  type MbsDashboardHttpGetInfoRspRiskItemMdl 
} from "@/services/pms-http/dashboard/dashboardHttp";
import { formatCurrency } from "@/utils/currencyFormatter";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import { getEmployeeProjectStatusLabel } from "@/utils/getEmployeeProjectStatusLabel";
import { formatDate } from "@/utils/timeFormatter";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { normalizeDashboardData } from "@/utils/buildDashboardMockData";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { loadProjectTemplateSettings } from "@/stores/projectTemplateSettings";
import { useMockDataStore } from "@/stores/mockDataStore";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";

const dashboardData = ref<MbsDashboardHttpGetInfoRspMdl | null>(null);
const templateSettings = computed(() => loadProjectTemplateSettings());
const mockStore = useMockDataStore();
const memberRegionMap = new Map(
  orgMemberDirectory.map((member) => [member.name, member.regionName])
);
const regionList = ref([
  { managerRegionID: 1, managerRegionName: "北區" },
  { managerRegionID: 2, managerRegionName: "中區" },
  { managerRegionID: 3, managerRegionName: "南區" },
]);
const selectedRegion = ref<number | null>(null); // null means "All"
const activeTab = ref<"project" | "pipeline">("project");
const selectedStage = ref<DbAtomPipelineStatusEnum | null>(null);
const selectedRisk = ref<DbAtomEmployeeProjectStatusEnum | null>(null);
const route = useRoute();
const router = useRouter();
const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";

const pipelineStageColorMap: Record<DbAtomPipelineStatusEnum, string> = {
  [DbAtomPipelineStatusEnum.Business10Percent]: "bg-blue-50 border-blue-300 text-blue-700",
  [DbAtomPipelineStatusEnum.Business30Percent]: "bg-indigo-50 border-indigo-300 text-indigo-700",
  [DbAtomPipelineStatusEnum.Business70Percent]: "bg-amber-50 border-amber-300 text-amber-700",
};

const { handleErrorCode } = useErrorCodeHandler();

const normalizeTab = (value: unknown) => (value === "pipeline" ? "pipeline" : "project");

const syncTabFromRoute = () => {
  const nextTab = normalizeTab(route.query.tab);
  activeTab.value = nextTab;
  if (route.query.tab !== nextTab) {
    router.replace({ query: { ...route.query, tab: nextTab } });
  }
};

const setActiveTab = (tab: "project" | "pipeline") => {
  if (route.query.tab === tab) {
    activeTab.value = tab;
    return;
  }
  router.replace({ query: { ...route.query, tab } });
};

const regionCodeFromAddress = (address: string) => {
  if (!address) return "";
  if (/(台中|彰化|南投|苗栗|雲林)/.test(address)) return "B";
  if (/(台南|高雄|屏東|嘉義)/.test(address)) return "C";
  if (/(台北|新北|基隆|桃園|新竹|宜蘭)/.test(address)) return "A";
  return "";
};

const regionCodeFromRegionId = (regionId: number | null) => {
  if (regionId === 1) return "A";
  if (regionId === 2) return "B";
  if (regionId === 3) return "C";
  return "";
};
const regionNameFromRegionId = (regionId: number | null) => {
  if (regionId === 1) return "北區";
  if (regionId === 2) return "中區";
  if (regionId === 3) return "南區";
  return "";
};

// Fetch Dashboard Data
const fetchData = async () => {
  try {
    // Note: mbsDashboardHttpGetInfo signature currently doesn't support request params in TS definition if I didn't update it?
    // Wait, I updated MbsDashboardCtlGetInfoReqMdl in Backend, but NOT in Frontend TS.
    // I need to update Frontend TS first or pass it as 'any' for now if I can't.
    // But since I'm editing this file, I should rely on updated TS.
    // I will cast to any to pass RegionID for now, assuming I will update TS next.
    const payload: any = {};
    if (selectedRegion.value !== null) {
      payload.RegionID = selectedRegion.value;
    }
    
    const res = await mbsDashboardHttpGetInfo(payload);
    dashboardData.value = normalizeDashboardData(res);
    selectedStage.value = null;
  } catch (e) {
    console.error(e);
    dashboardData.value = normalizeDashboardData(null);
  }
};

onMounted(async () => {
  await fetchData();
  syncTabFromRoute();
});

// Refetch when region changes
watch(selectedRegion, () => {
  fetchData();
});

watch(
  () => route.query.tab,
  () => {
    syncTabFromRoute();
  }
);

const getRiskCount = (status: DbAtomEmployeeProjectStatusEnum) => {
  if (useMockData) {
    return mockProjectSource.value.filter((item) => {
      if (status === DbAtomEmployeeProjectStatusEnum.OnSchedule) {
        return [
          DbAtomEmployeeProjectStatusEnum.OnSchedule,
          DbAtomEmployeeProjectStatusEnum.NotStarted,
          DbAtomEmployeeProjectStatusEnum.NotAssigned,
          DbAtomEmployeeProjectStatusEnum.Completed,
        ].includes(item.status);
      }
      return item.status === status;
    }).length;
  }
  return dashboardData.value?.ProjectRiskStatistics?.find(x => x.Status === status)?.Count || 0;
};
const toggleRisk = (status: DbAtomEmployeeProjectStatusEnum) => {
  selectedRisk.value = selectedRisk.value === status ? null : status;
};
const riskCardClass = (status: DbAtomEmployeeProjectStatusEnum) => {
  const isActive = selectedRisk.value === status ? " ring-2 ring-offset-2 ring-blue-500" : "";
  return `cursor-pointer transition-colors${isActive}`;
};
const riskProjects = computed(() => {
  const list = mockDataSets.workProjects;
  if (!selectedRisk.value) return list;
  return list.filter((item) => item.status === selectedRisk.value);
});
const riskProjectsPreview = computed(() => riskProjects.value.slice(0, 5));
const goToProjectDetail = (projectId?: number) => {
  if (!projectId) return;
  router.push(`/work/project/detail/${projectId}`);
};

const grossProfitDisplay = computed(() => {
  const fallback = dashboardData.value?.TotalGrossProfit ?? 0;
  if (!useMockData) return fallback;
  if (selectedRegion.value === null) return fallback;
  const total = mockDataSets.workProjects
    .filter((item) => item.regionId === selectedRegion.value)
    .reduce((sum, item) => sum + (item.grossProfit ?? 0), 0);
  return total || fallback;
});
const totalProjectDisplay = computed(() => {
  const fallback = dashboardData.value?.ProjectRiskStatistics?.reduce((a, b) => a + b.Count, 0) || 0;
  if (!useMockData) return fallback;
  return mockProjectSource.value.length || fallback;
});

const allowedPipelineStages = new Set<DbAtomPipelineStatusEnum>([
  DbAtomPipelineStatusEnum.Business10Percent,
  DbAtomPipelineStatusEnum.Business30Percent,
  DbAtomPipelineStatusEnum.Business70Percent,
]);
const pipelineStageSummary = computed(() =>
  (dashboardData.value?.PipelineStageStatistics ?? []).filter((item) =>
    allowedPipelineStages.has(item.Status)
  )
);
const totalPipelineStageCount = computed(() =>
  pipelineStageSummary.value.reduce((sum, stage) => sum + stage.Count, 0)
);
const pipelineStageCounts = computed(() => {
  const getCount = (status: DbAtomPipelineStatusEnum) =>
    pipelineStageSummary.value.find((item) => item.Status === status)?.Count || 0;
  return {
    total: totalPipelineStageCount.value,
    business10: getCount(DbAtomPipelineStatusEnum.Business10Percent),
    business30: getCount(DbAtomPipelineStatusEnum.Business30Percent),
    business70: getCount(DbAtomPipelineStatusEnum.Business70Percent),
  };
});

const pipelinePieSegments = computed(() => {
  const total = pipelineStageCounts.value.total;
  const segments = [
    {
      status: DbAtomPipelineStatusEnum.Business10Percent,
      label: "10%",
      color: "#60a5fa",
      count: pipelineStageCounts.value.business10,
    },
    {
      status: DbAtomPipelineStatusEnum.Business30Percent,
      label: "30%",
      color: "#6366f1",
      count: pipelineStageCounts.value.business30,
    },
    {
      status: DbAtomPipelineStatusEnum.Business70Percent,
      label: "70%",
      color: "#f59e0b",
      count: pipelineStageCounts.value.business70,
    },
  ];
  let start = 0;
  return segments.map((segment) => {
    const share = total ? (segment.count / total) * 100 : 0;
    const end = start + share;
    const entry = { ...segment, start, end };
    start = end;
    return entry;
  });
});

const pipelinePieStyle = computed(() => {
  if (!pipelineStageCounts.value.total) {
    return {
      background: "conic-gradient(#e5e7eb 0% 100%)",
    };
  }
  const gradientStops = pipelinePieSegments.value
    .map((segment) => `${segment.color} ${segment.start}% ${segment.end}%`)
    .join(", ");
  return {
    background: `conic-gradient(${gradientStops})`,
  };
});

const filteredPipelineDetails = computed(() => {
  const list = (dashboardData.value?.PipelineStageDetails ?? []).filter((item) =>
    allowedPipelineStages.has(item.Status)
  );
  if (!selectedStage.value) {
    return list;
  }
  return list.filter((item) => item.Status === selectedStage.value);
});

const stageCardClass = (status: DbAtomPipelineStatusEnum) => {
  const base = pipelineStageColorMap[status] || "bg-gray-50 border-gray-200 text-gray-700";
  const isActive = selectedStage.value === status ? " ring-2 ring-offset-2 ring-blue-500" : "";
  return `${base} border-2 rounded-xl p-4 cursor-pointer transition ${isActive}`;
};

const toggleStage = (status: DbAtomPipelineStatusEnum) => {
  selectedStage.value = selectedStage.value === status ? null : status;
};

const getStageShare = (status: DbAtomPipelineStatusEnum) => {
  const total = totalPipelineStageCount.value;
  if (!total) return 0;
  const stage = pipelineStageSummary.value.find((item) => item.Status === status);
  return stage ? stage.Count / total : 0;
};

const formatPercentage = (value: number) => `${Math.round(value * 100)}%`;

const navigateToPipeline = (pipelineId: number) => {
  if (!pipelineId) return;
  router.push(`/crm/pipeline/pipeline/detail/${pipelineId}`);
};

const projectTypeLabelMap = computed(
  () =>
    new Map(
      templateSettings.value.projectTypes.map((item) => [item.projectTypeId, item.projectTypeName])
    )
);
const mockProjectSource = computed(() => {
  if (!useMockData) return [];
  if (selectedRegion.value === null) return mockDataSets.workProjects;
  return mockDataSets.workProjects.filter((item) => item.regionId === selectedRegion.value);
});
const projectTypeCounts = computed(() => {
  if (useMockData) {
    const counts: Record<string, number> = {};
    mockProjectSource.value.forEach((item) => {
      const label =
        item.serviceItemNames && item.serviceItemNames.length > 0
          ? item.serviceItemNames.join("、")
          : "未設定";
      counts[label] = (counts[label] ?? 0) + 1;
    });
    return counts;
  }
  const counts: Record<string, number> = {};
  mockDataSets.workProjects.forEach((item) => {
    const raw = localStorage.getItem(`workProjectProjectType:${item.id}`);
    const projectTypeId = raw ? Number(raw) : NaN;
    const label = projectTypeLabelMap.value.get(projectTypeId) ?? "未設定";
    counts[label] = (counts[label] ?? 0) + 1;
  });
  return counts;
});
const filteredCertificates = computed(() => {
  const regionName = regionNameFromRegionId(selectedRegion.value);
  if (!regionName) {
    return mockStore.certificates;
  }
  return mockStore.certificates.filter(
    (cert) => memberRegionMap.get(cert.employeeName) === regionName
  );
});
const certificateCount = computed(() => filteredCertificates.value.length);
const projectTypeTotal = computed(() =>
  useMockData ? mockProjectSource.value.length : Object.values(projectTypeCounts.value).reduce((sum, count) => sum + count, 0)
);
const projectTypeSegments = computed(() => {
  const colors = ["#0ea5e9", "#14b8a6", "#f97316", "#a855f7", "#94a3b8"];
  const entries = Object.entries(projectTypeCounts.value);
  let start = 0;
  return entries.map(([label, count], index) => {
    const share = projectTypeTotal.value ? (count / projectTypeTotal.value) * 100 : 0;
    const end = start + share;
    const segment = {
      label,
      count,
      color: colors[index % colors.length],
      start,
      end,
      share,
    };
    start = end;
    return segment;
  });
});
const projectTypePieStyle = computed(() => {
  if (!projectTypeTotal.value) {
    return { background: "conic-gradient(#e5e7eb 0% 100%)" };
  }
  const gradientStops = projectTypeSegments.value
    .map((segment) => `${segment.color} ${segment.start}% ${segment.end}%`)
    .join(", ");
  return { background: `conic-gradient(${gradientStops})` };
});

const filteredCustomers = computed(() => {
  const regionCode = regionCodeFromRegionId(selectedRegion.value);
  if (!regionCode) return mockDataSets.phoneSalesCustomers;
  return mockDataSets.phoneSalesCustomers.filter(
    (item) => regionCodeFromAddress(item.companyAddress) === regionCode
  );
});
const getIndustryLevel = (industry: string) => {
  const ALevel = new Set(["金融服務", "能源", "醫療", "電信", "政府", "交通運輸"]);
  const BLevel = new Set(["製造業", "雲端服務", "資訊服務", "電商", "媒體", "教育"]);
  if (ALevel.has(industry)) return "A級";
  if (BLevel.has(industry)) return "B級";
  return "C級";
};
const customerTypeCounts = computed(() => {
  const counts: Record<string, number> = {};
  filteredCustomers.value.forEach((item) => {
    const industry = item.industry || "未分類";
    const level = getIndustryLevel(industry);
    const key = `${level}｜${industry}`;
    counts[key] = (counts[key] ?? 0) + 1;
  });
  return counts;
});
const customerTypeTotal = computed(() =>
  Object.values(customerTypeCounts.value).reduce((sum, count) => sum + count, 0)
);
const customerTypeSegments = computed(() => {
  const colors = ["#38bdf8", "#22c55e", "#f59e0b", "#ef4444", "#a855f7"];
  const entries = Object.entries(customerTypeCounts.value);
  let start = 0;
  return entries.map(([label, count], index) => {
    const share = customerTypeTotal.value ? (count / customerTypeTotal.value) * 100 : 0;
    const end = start + share;
    const segment = {
      label,
      count,
      color: colors[index % colors.length],
      start,
      end,
      share,
    };
    start = end;
    return segment;
  });
});
const customerTypePieStyle = computed(() => {
  if (!customerTypeTotal.value) {
    return { background: "conic-gradient(#e5e7eb 0% 100%)" };
  }
  const gradientStops = customerTypeSegments.value
    .map((segment) => `${segment.color} ${segment.start}% ${segment.end}%`)
    .join(", ");
  return { background: `conic-gradient(${gradientStops})` };
});
</script>

<template>
  <div class="flex flex-col gap-6">
    <AnnouncementBoard />

    <!-- Region Filter for GM -->
    <div class="flex justify-end items-center gap-2">
      <span class="text-sm font-bold text-gray-600">地區篩選：</span>
      <select 
        v-model="selectedRegion" 
        class="border border-gray-300 rounded px-3 py-1 text-sm bg-white shadow-sm focus:outline-none focus:border-blue-500"
      >
        <option :value="null">全公司 (All)</option>
        <option v-for="region in regionList" :key="region.managerRegionID" :value="region.managerRegionID">
          {{ region.managerRegionName }}
        </option>
      </select>
    </div>

    <div class="flex gap-3 border-b border-gray-200">
      <button
        class="px-4 py-2 text-sm font-semibold rounded-t-md"
        :class="activeTab === 'project' ? 'bg-white text-blue-600 border border-b-white border-gray-200' : 'text-gray-500'"
        @click="setActiveTab('project')"
      >
        專案概況
      </button>
      <button
        class="px-4 py-2 text-sm font-semibold rounded-t-md"
        :class="activeTab === 'pipeline' ? 'bg-white text-blue-600 border border-b-white border-gray-200' : 'text-gray-500'"
        @click="setActiveTab('pipeline')"
      >
        商機概況
      </button>
    </div>

    <div v-if="activeTab === 'project'" class="flex flex-col gap-6">
      <div class="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">公司總毛利</h3>
          <div class="text-3xl font-bold text-green-600">
            {{ formatCurrency(grossProfitDisplay) }}
          </div>
          <div class="text-xs text-gray-400 mt-2">
            已過濾：{{ selectedRegion ? regionList.find(r => r.managerRegionID === selectedRegion)?.managerRegionName : "全區" }}
          </div>
        </div>

        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">公司專案總數</h3>
          <div class="text-3xl font-bold text-blue-600">
            {{ totalProjectDisplay }}
          </div>
        </div>

        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">公司證照總數</h3>
          <div class="text-3xl font-bold text-emerald-600">
            {{ certificateCount }}
          </div>
          <div class="text-xs text-gray-400 mt-2">
            已過濾：{{ selectedRegion ? regionList.find(r => r.managerRegionID === selectedRegion)?.managerRegionName : "全區" }}
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow p-6">
        <h3 class="text-lg font-bold text-gray-800 mb-4">專案風險概況</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div
            class="bg-red-50 border-l-4 border-red-500 p-4 hover:bg-red-100"
            :class="riskCardClass(DbAtomEmployeeProjectStatusEnum.Delayed)"
            @click="toggleRisk(DbAtomEmployeeProjectStatusEnum.Delayed)"
          >
            <div class="flex justify-between items-center">
              <span class="text-red-700 font-bold">延遲 (Delayed)</span>
              <span class="text-2xl font-bold text-red-700">{{ getRiskCount(DbAtomEmployeeProjectStatusEnum.Delayed) }}</span>
            </div>
          </div>
          <div
            class="bg-yellow-50 border-l-4 border-yellow-500 p-4 hover:bg-yellow-100"
            :class="riskCardClass(DbAtomEmployeeProjectStatusEnum.AtRisk)"
            @click="toggleRisk(DbAtomEmployeeProjectStatusEnum.AtRisk)"
          >
            <div class="flex justify-between items-center">
              <span class="text-yellow-700 font-bold">注意 (At Risk)</span>
              <span class="text-2xl font-bold text-yellow-700">{{ getRiskCount(DbAtomEmployeeProjectStatusEnum.AtRisk) }}</span>
            </div>
          </div>
          <div
            class="bg-green-50 border-l-4 border-green-500 p-4 hover:bg-green-100"
            :class="riskCardClass(DbAtomEmployeeProjectStatusEnum.OnSchedule)"
            @click="toggleRisk(DbAtomEmployeeProjectStatusEnum.OnSchedule)"
          >
            <div class="flex justify-between items-center">
              <span class="text-green-700 font-bold">如期 (On Schedule)</span>
              <span class="text-2xl font-bold text-green-700">{{ getRiskCount(DbAtomEmployeeProjectStatusEnum.OnSchedule) }}</span>
            </div>
          </div>
        </div>
        <div v-if="selectedRisk" class="mt-4 rounded-lg border border-gray-200 p-4">
          <div class="flex items-center justify-between mb-3">
            <h4 class="text-sm font-semibold text-gray-700">風險專案清單</h4>
            <span class="text-xs text-gray-500">
              顯示：{{ getEmployeeProjectStatusLabel(selectedRisk) }}
            </span>
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
                <tr v-if="riskProjects.length === 0">
                  <td colspan="5" class="text-center py-6 text-gray-400">目前無專案</td>
                </tr>
                <tr
                  v-for="project in riskProjectsPreview"
                  :key="project.id"
                  class="border border-gray-300 cursor-pointer hover:bg-cyan-50"
                  role="button"
                  tabindex="0"
                  @click="goToProjectDetail(project.id)"
                  @keydown.enter="goToProjectDetail(project.id)"
                >
                  <td class="text-start">{{ project.name }}</td>
                  <td class="text-start">{{ project.companyName }}</td>
                  <td class="text-start">{{ project.departmentName || "-" }}</td>
                  <td class="text-start">
                    {{ formatDate(project.startTime) }} ~ {{ formatDate(project.endTime) }}
                  </td>
                  <td class="text-start">{{ getEmployeeProjectStatusLabel(project.status) }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div class="bg-white rounded-lg shadow p-6">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-lg font-bold text-gray-800">專案類型分布</h3>
            <span class="text-xs text-gray-500">共 {{ projectTypeTotal }} 筆</span>
          </div>
          <div class="flex flex-col sm:flex-row items-center gap-4">
            <div class="flex items-center justify-center">
              <div
                class="h-36 w-36 sm:h-40 sm:w-40 rounded-full border border-gray-200"
                :style="projectTypePieStyle"
              ></div>
            </div>
            <div class="flex-1 min-w-0 space-y-2">
              <div
                v-for="segment in projectTypeSegments"
                :key="segment.label"
                class="flex items-center justify-between gap-3 rounded-lg border border-gray-200 px-3 py-2 text-sm"
              >
                <div class="flex items-center gap-2 min-w-0">
                  <span class="h-2.5 w-2.5 rounded-full" :style="{ backgroundColor: segment.color }"></span>
                  <span class="truncate text-gray-700">{{ segment.label }}</span>
                </div>
                <div class="flex items-center gap-2 text-gray-600">
                  <span class="tabular-nums">{{ segment.count }}</span>
                  <span class="text-xs text-gray-400">{{ Math.round(segment.share) }}%</span>
                </div>
              </div>
              <div v-if="projectTypeSegments.length === 0" class="text-sm text-gray-400">
                目前無資料
              </div>
            </div>
          </div>
        </div>
        <div class="bg-white rounded-lg shadow p-6">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-lg font-bold text-gray-800">客戶分布</h3>
            <span class="text-xs text-gray-500">共 {{ customerTypeTotal }} 筆</span>
          </div>
          <div class="flex flex-col sm:flex-row items-center gap-4">
            <div class="flex items-center justify-center">
              <div
                class="h-36 w-36 sm:h-40 sm:w-40 rounded-full border border-gray-200"
                :style="customerTypePieStyle"
              ></div>
            </div>
            <div class="flex-1 min-w-0 space-y-2">
              <div
                v-for="segment in customerTypeSegments"
                :key="segment.label"
                class="flex items-center justify-between gap-3 rounded-lg border border-gray-200 px-3 py-2 text-sm"
              >
                <div class="flex items-center gap-2 min-w-0">
                  <span class="h-2.5 w-2.5 rounded-full" :style="{ backgroundColor: segment.color }"></span>
                  <span class="truncate text-gray-700">{{ segment.label }}</span>
                </div>
                <div class="flex items-center gap-2 text-gray-600">
                  <span class="tabular-nums">{{ segment.count }}</span>
                  <span class="text-xs text-gray-400">{{ Math.round(segment.share) }}%</span>
                </div>
              </div>
              <div v-if="customerTypeSegments.length === 0" class="text-sm text-gray-400">
                目前無資料
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="flex flex-col gap-6">
      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-lg font-bold text-gray-800">商機階段分布</h3>
          <span class="text-sm text-gray-500">10% / 30% / 70%</span>
        </div>
        <div class="flex flex-col md:flex-row md:items-center gap-6">
          <div class="flex items-center justify-center">
            <div
              class="h-44 w-44 rounded-full border border-gray-200"
              :style="pipelinePieStyle"
            ></div>
          </div>
          <div class="grid grid-cols-1 md:grid-cols-4 gap-4 flex-1">
            <div
              v-for="stage in pipelineStageSummary"
              :key="stage.Status"
              :class="stageCardClass(stage.Status)"
              @click="toggleStage(stage.Status)"
            >
              <div class="flex items-center justify-between text-sm font-medium">
                <span>{{ getPipelineStatusLabel(stage.Status) }}</span>
                <span class="text-xs text-gray-500">
                  {{ formatPercentage(getStageShare(stage.Status)) }}
                </span>
              </div>
              <div class="text-3xl font-bold mt-2">{{ stage.Count }}</div>
            </div>
          </div>
        </div>
      </div>
      <p class="text-sm text-gray-500">
        統計所有未轉專案的商機，協助管理層掌握 10%~70% 各階段的滯留量，點擊卡片可即時篩選下方列表。
      </p>

      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-lg font-bold text-gray-800">商機明細</h3>
          <span class="text-sm text-gray-500">
            {{ selectedStage ? getPipelineStatusLabel(selectedStage) : "全部階段" }} ·
            {{ filteredPipelineDetails.length }} 筆
          </span>
        </div>
        <div class="overflow-x-auto">
          <table class="min-w-full table-auto">
            <thead>
              <tr class="text-left text-sm text-gray-500 border-b">
                <th class="py-2">客戶</th>
                <th class="py-2">負責業務</th>
                <th class="py-2">階段</th>
                <th class="py-2 text-right">預估金額</th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="filteredPipelineDetails.length === 0">
                <td colspan="4" class="text-center py-6 text-gray-400">目前無商機資料</td>
              </tr>
              <tr
                v-for="item in filteredPipelineDetails"
                :key="item.EmployeePipelineID"
                class="border-b last:border-b-0 text-sm cursor-pointer hover:bg-cyan-50 transition"
                @click="navigateToPipeline(item.EmployeePipelineID)"
              >
                <td class="py-2">{{ item.ManagerCompanyName || "-" }}</td>
                <td class="py-2">{{ item.OwnerName || "-" }}</td>
                <td class="py-2">{{ getPipelineStatusLabel(item.Status) }}</td>
                <td class="py-2 text-right font-semibold">
                  {{ formatCurrency(item.EstimatedAmount || 0) }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
