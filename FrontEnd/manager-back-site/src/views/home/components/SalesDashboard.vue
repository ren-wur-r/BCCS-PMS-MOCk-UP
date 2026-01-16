<script setup lang="ts">
import { ref, onMounted, computed, watch, onBeforeUnmount } from "vue";
import { useRoute, useRouter } from "vue-router";
import AnnouncementBoard from "./AnnouncementBoard.vue";
import QuickWorkLog from "@/components/feature/work/QuickWorkLog.vue";
import { 
  mbsDashboardHttpGetInfo, 
  type MbsDashboardHttpGetInfoRspMdl 
} from "@/services/pms-http/dashboard/dashboardHttp";
import { formatCurrency } from "@/utils/currencyFormatter";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import { normalizeDashboardData } from "@/utils/buildDashboardMockData";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";

const dashboardData = ref<MbsDashboardHttpGetInfoRspMdl | null>(null);
const dashboardTab = ref<"overview" | "worklog">("overview");
const activeTab = ref<"project" | "pipeline">("project");
const selectedStage = ref<DbAtomPipelineStatusEnum | null>(null);
const route = useRoute();
const router = useRouter();
const employeeInfoStore = useEmployeeInfoStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
const handoffStorageKey = "cache.crm.pipeline.handoff.queue";

const pipelineStageColorMap: Record<DbAtomPipelineStatusEnum, string> = {
  [DbAtomPipelineStatusEnum.Business10Percent]: "bg-blue-50 border-blue-200 text-blue-700",
  [DbAtomPipelineStatusEnum.Business30Percent]: "bg-indigo-50 border-indigo-200 text-indigo-700",
  [DbAtomPipelineStatusEnum.Business70Percent]: "bg-amber-50 border-amber-200 text-amber-700",
};

onMounted(async () => {
  try {
    const res = await mbsDashboardHttpGetInfo({});
    dashboardData.value = normalizeDashboardData(res);
    selectedStage.value = null;
    syncTabFromRoute();
  } catch (e) {
    console.error(e);
    dashboardData.value = normalizeDashboardData(null);
    syncTabFromRoute();
  }
});

const getPersonalRiskCount = (status: DbAtomEmployeeProjectStatusEnum) =>
  dashboardData.value?.PersonalProjectRiskStatistics?.find(x => x.Status === status)?.Count || 0;

const pipelineStageSummary = computed(
  () => dashboardData.value?.PersonalPipelineStageStatistics ?? []
);
const mainStageSummary = computed(() =>
  pipelineStageSummary.value.filter((item) =>
    [
      DbAtomPipelineStatusEnum.Business10Percent,
      DbAtomPipelineStatusEnum.Business30Percent,
      DbAtomPipelineStatusEnum.Business70Percent,
    ].includes(item.Status)
  )
);
const totalPipelineStageCount = computed(() =>
  mainStageSummary.value.reduce((sum, stage) => sum + stage.Count, 0)
);
const readHandoffLeads = () => {
  const raw = localStorage.getItem(handoffStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};
const teleSalesTransferCount = computed(() => {
  const list = readHandoffLeads();
  const currentName = employeeInfoStore.employeeName?.trim();
  return list.filter((item) => {
    if (item?.status !== "pending") return false;
    if (!currentName) return true;
    return item.assignedSaler === currentName;
  }).length;
});

const allowedPipelineStages = new Set<DbAtomPipelineStatusEnum>([
  DbAtomPipelineStatusEnum.Business10Percent,
  DbAtomPipelineStatusEnum.Business30Percent,
  DbAtomPipelineStatusEnum.Business70Percent,
]);
const filteredPipelineDetails = computed(() => {
  const list = (dashboardData.value?.PersonalPipelineStageDetails ?? []).filter((item) =>
    allowedPipelineStages.has(item.Status)
  );
  if (!selectedStage.value) {
    return list;
  }
  return list.filter(item => item.Status === selectedStage.value);
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
  const stage = mainStageSummary.value.find((item) => item.Status === status);
  return stage ? stage.Count / total : 0;
};

const formatPercentage = (value: number) => `${Math.round(value * 100)}%`;

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

watch(
  () => route.query.tab,
  () => {
    syncTabFromRoute();
  }
);

watch(
  () => activeTab.value,
  (tab) => {
    setModuleTitle(tab === "pipeline" ? "商機概況" : "專案概況");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});

const navigateToPipeline = (pipelineId: number) => {
  if (!pipelineId) return;
  router.push(`/crm/pipeline/pipeline/detail/${pipelineId}`);
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
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">個人商機開發數</h3>
          <div class="text-3xl font-bold text-blue-600">
            {{ dashboardData?.PersonalPipelineCount || 0 }}
          </div>
        </div>

        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">個人商機毛利總額</h3>
          <div class="text-3xl font-bold text-green-600">
            {{ formatCurrency(dashboardData?.PersonalGrossProfit || 0) }}
          </div>
        </div>

        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">個人專案數量</h3>
          <div class="text-3xl font-bold text-purple-600">
            {{ dashboardData?.PersonalProjectCount || 0 }}
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow p-6">
        <h3 class="text-lg font-bold text-gray-800 mb-4">我的專案風險</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div class="bg-red-50 border-l-4 border-red-500 p-4">
            <div class="flex justify-between items-center">
              <span class="text-red-700 font-bold">延遲</span>
              <span class="text-2xl font-bold text-red-700">
                {{ getPersonalRiskCount(DbAtomEmployeeProjectStatusEnum.Delayed) }}
              </span>
            </div>
          </div>

          <div class="bg-yellow-50 border-l-4 border-yellow-500 p-4">
            <div class="flex justify-between items-center">
              <span class="text-yellow-700 font-bold">注意</span>
              <span class="text-2xl font-bold text-yellow-700">
                {{ getPersonalRiskCount(DbAtomEmployeeProjectStatusEnum.AtRisk) }}
              </span>
            </div>
          </div>

          <div class="bg-green-50 border-l-4 border-green-500 p-4">
            <div class="flex justify-between items-center">
              <span class="text-green-700 font-bold">如期</span>
              <span class="text-2xl font-bold text-green-700">
                {{ getPersonalRiskCount(DbAtomEmployeeProjectStatusEnum.OnSchedule) }}
              </span>
            </div>
          </div>
        </div>
      </div>
      </div>

      <div v-else class="flex flex-col gap-6">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">個人商機毛利總額</h3>
          <div class="text-3xl font-bold text-green-600">
            {{ formatCurrency(dashboardData?.PersonalGrossProfit || 0) }}
          </div>
        </div>
        <div class="bg-white rounded-lg shadow p-6">
          <h3 class="text-gray-500 text-sm font-medium mb-2">個人商機數</h3>
          <div class="text-3xl font-bold text-blue-600">
            {{ dashboardData?.PersonalPipelineCount || 0 }}
          </div>
        </div>
        <div
          class="bg-white rounded-lg shadow p-6 cursor-pointer transition hover:bg-cyan-50"
          @click="router.push('/crm/pipeline/handoff')"
        >
          <h3 class="text-gray-500 text-sm font-medium mb-2">電銷轉派</h3>
          <div class="text-3xl font-bold text-amber-600">
            {{ teleSalesTransferCount }}
          </div>
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div
          v-for="stage in mainStageSummary"
          :key="stage.Status"
          :class="stageCardClass(stage.Status)"
          @click="toggleStage(stage.Status)"
        >
          <div class="flex items-center justify-between text-sm font-medium">
            <span>{{ getPipelineStatusLabel(stage.Status) }}</span>
            <span class="text-xs text-gray-500">{{ formatPercentage(getStageShare(stage.Status)) }}</span>
          </div>
          <div class="text-3xl font-bold mt-2">{{ stage.Count }}</div>
        </div>
      </div>
      <p class="text-sm text-gray-500">
        包含所有尚未轉專案的商機，點擊卡片可篩選下方明細並追蹤開發狀態。
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
                <th class="py-2">階段</th>
                <th class="py-2 text-right">預估金額</th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="filteredPipelineDetails.length === 0">
                <td colspan="3" class="text-center py-6 text-gray-400">目前無商機資料</td>
              </tr>
              <tr
                v-for="item in filteredPipelineDetails"
                :key="item.EmployeePipelineID"
                class="border-b last:border-b-0 text-sm cursor-pointer hover:bg-cyan-50 transition"
                @click="navigateToPipeline(item.EmployeePipelineID)"
              >
                <td class="py-2">{{ item.ManagerCompanyName || "-" }}</td>
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

    <div v-else class="flex flex-col gap-6" data-annotation-scope="dashboard-worklog">
      <QuickWorkLog />
    </div>
  </div>
</template>
