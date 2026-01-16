<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import { loadProjectTemplateSettings, getStageTemplatesByServiceItems } from "@/stores/projectTemplateSettings";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";

const route = useRoute();
const router = useRouter();
const pipelineId = Number(route.params.pipelineId || 0);
const templateSettings = loadProjectTemplateSettings();
const activeTab = ref<"info" | "template" | "milestone">("info");
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();

const detailKey = `cache.crm.pipeline.detail.${pipelineId}`;
const pocKey = `cache.crm.pipeline.poc.${pipelineId}`;

const getPipelineDetail = () => {
  if (!pipelineId) return null;
  const raw = localStorage.getItem(detailKey);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as {
      bookingCode?: string;
      companyName?: string;
      status?: number;
      serviceItemIds?: number[];
      serviceProductIds?: Record<number, number[]>;
    };
  } catch {
    return null;
  }
};

const getPocData = () => {
  if (!pipelineId) return null;
  const raw = localStorage.getItem(pocKey);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as {
      form?: {
        startDate?: string;
        endDate?: string;
        durationWeeks?: string;
        result?: string;
        rejectReason?: string;
        rejectReasonNote?: string;
      };
    };
  } catch {
    return null;
  }
};

const buildFallbackDetail = () => {
  const fallback =
    mockDataSets.crmPipelines.find((item) => item.id === pipelineId) ??
    mockDataSets.crmPipelines[0];
  const defaultService = templateSettings.serviceItems[0];
  const defaultProduct = defaultService?.products?.[0];
  return {
    bookingCode: fallback?.companyName ?? "商機",
    companyName: fallback?.companyName ?? "未命名商機",
    status: fallback?.status ?? 0,
    serviceItemIds: defaultService ? [defaultService.serviceItemId] : [],
    serviceProductIds:
      defaultService && defaultProduct
        ? { [defaultService.serviceItemId]: [defaultProduct.productId] }
        : {},
  };
};

const pipelineDetail = ref<ReturnType<typeof getPipelineDetail>>(null);
const pocData = ref<ReturnType<typeof getPocData>>(null);

const loadData = () => {
  const detail = getPipelineDetail();
  if (detail) {
    pipelineDetail.value = detail;
  } else {
    const fallback = buildFallbackDetail();
    pipelineDetail.value = fallback;
    if (pipelineId) {
      localStorage.setItem(detailKey, JSON.stringify(fallback));
    }
  }

  const poc = getPocData();
  if (poc) {
    pocData.value = poc;
  } else {
    pocData.value = {
      form: {
        startDate: "",
        endDate: "",
        durationWeeks: "",
        result: "",
        rejectReason: "",
        rejectReasonNote: "",
      },
    };
  }
};
const serviceItemMap = new Map(
  templateSettings.serviceItems.map((item) => [item.serviceItemId, item])
);
const serviceItemIds = computed(() => pipelineDetail.value?.serviceItemIds ?? []);
const serviceItemNames = computed(() =>
  serviceItemIds.value
    .map((id) => serviceItemMap.get(id)?.serviceItemName)
    .filter(Boolean)
    .join("、") || "-"
);
const productNames = computed(() => {
  const detail = pipelineDetail.value;
  if (!detail?.serviceProductIds) return "-";
  const names: string[] = [];
  Object.entries(detail.serviceProductIds).forEach(([serviceId, productIds]) => {
    const service = serviceItemMap.get(Number(serviceId));
    if (!service) return;
    service.products
      .filter((product) => productIds.includes(product.productId))
      .forEach((product) => names.push(product.productName));
  });
  return names.length > 0 ? names.join("、") : "-";
});

const stageTemplates = computed(() =>
  getStageTemplatesByServiceItems(templateSettings, serviceItemIds.value)
);
const stageSummary = computed(() => {
  const stages = stageTemplates.value.flatMap((template) => template.stages);
  return {
    current: stages[0]?.stageName ?? "-",
    next: stages[1]?.stageName ?? "-",
  };
});

const pocStartDate = computed(() => pocData.value?.form?.startDate || "-");
const pocEndDate = computed(() => pocData.value?.form?.endDate || "-");

const goBack = () => router.push("/work/project");

const handlePocUpdated = () => {
  loadData();
};

const handleStorage = (event: StorageEvent) => {
  if (!event.key) return;
  if (event.key === detailKey || event.key === pocKey) {
    loadData();
  }
};

onMounted(() => {
  loadData();
  window.addEventListener("crm-poc-updated", handlePocUpdated as EventListener);
  window.addEventListener("storage", handleStorage);
});

onBeforeUnmount(() => {
  window.removeEventListener("crm-poc-updated", handlePocUpdated as EventListener);
  window.removeEventListener("storage", handleStorage);
  clearModuleTitle();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<string, string> = {
      info: "POC 資訊",
      template: "標準階段與產出",
      milestone: "里程碑",
    };
    setModuleTitle(titleMap[tab] ?? "POC 資訊");
  },
  { immediate: true }
);
</script>

<template>
  <div class="flex flex-col p-4">
    <div class="flex items-center flex-row gap-2 justify-between overflow-hidden mb-3">
      <button class="btn-back flex items-center" @click="goBack">
        <svg class="w-4 h-4 inline-block mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
        </svg>
        <span>返回</span>
      </button>
    </div>

    <div class="flex mb-0 gap-y-4">
      <button class="tab-btn" :class="{ active: activeTab === 'info' }" @click="activeTab = 'info'">
        POC 資訊
      </button>
      <button class="tab-btn" :class="{ active: activeTab === 'template' }" @click="activeTab = 'template'">
        標準階段與產出
      </button>
      <button class="tab-btn" :class="{ active: activeTab === 'milestone' }" @click="activeTab = 'milestone'">
        里程碑
      </button>
    </div>

    <div class="tab-content flex-1">
      <div v-if="activeTab === 'info'" class="tab h-full flex flex-col gap-4">
        <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <div class="text-xs text-gray-500">商機名稱</div>
              <div class="text-sm font-semibold text-gray-800">
                {{ pipelineDetail?.bookingCode || pipelineDetail?.companyName || "-" }}
              </div>
            </div>
            <div>
              <div class="text-xs text-gray-500">商機狀態</div>
              <div class="text-sm font-semibold text-gray-800">
                {{ pipelineDetail?.status ? getPipelineStatusLabel(pipelineDetail?.status) : "-" }}
              </div>
            </div>
            <div>
              <div class="text-xs text-gray-500">服務項目</div>
              <div class="text-sm font-semibold text-gray-800">{{ serviceItemNames }}</div>
            </div>
            <div>
              <div class="text-xs text-gray-500">產品</div>
              <div class="text-sm font-semibold text-gray-800">{{ productNames }}</div>
            </div>
            <div>
              <div class="text-xs text-gray-500">POC 起訖時間</div>
              <div class="text-sm font-semibold text-gray-800">{{ pocStartDate }} ~ {{ pocEndDate }}</div>
            </div>
            <div>
              <div class="text-xs text-gray-500">目前 / 下一階段</div>
              <div class="text-sm font-semibold text-gray-800">
                {{ stageSummary.current }} / {{ stageSummary.next }}
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-if="activeTab === 'template'" class="tab h-full flex flex-col gap-4">
        <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
          <div v-if="stageTemplates.length === 0" class="text-sm text-gray-500">
            尚未設定標準階段
          </div>
          <div v-else class="space-y-4">
            <div
              v-for="template in stageTemplates"
              :key="template.stageTemplateId"
              class="rounded-lg border border-gray-200 bg-gray-50 p-4"
            >
              <p class="font-semibold text-gray-700">{{ template.stageTemplateName }}</p>
              <div class="mt-3 space-y-3">
                <div
                  v-for="stage in template.stages"
                  :key="stage.stageId"
                  class="rounded-md border border-gray-200 bg-white px-4 py-3"
                >
                  <p class="text-sm font-semibold text-gray-700">{{ stage.stageName }}</p>
                  <div v-if="stage.requiredOutputs.length > 0" class="mt-2 text-xs text-gray-500">
                    產出：{{ stage.requiredOutputs.map((output) => output.outputName).join("、") }}
                  </div>
                  <div v-else class="mt-2 text-xs text-gray-400">尚未設定產出</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-if="activeTab === 'milestone'" class="tab h-full flex flex-col gap-4">
        <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
          <div class="flex items-center justify-between text-xs text-gray-500">
            <span>POC 起日：{{ pocStartDate }}</span>
            <span>POC 迄日：{{ pocEndDate }}</span>
          </div>
          <div v-if="stageTemplates.length === 0" class="text-sm text-gray-500">
            尚未設定里程碑
          </div>
          <div v-else class="space-y-3">
            <div
              v-for="template in stageTemplates"
              :key="`${template.stageTemplateId}-milestone`"
              class="rounded-lg border border-gray-200 bg-gray-50 p-4"
            >
              <p class="text-sm font-semibold text-gray-700">{{ template.stageTemplateName }}</p>
              <div class="mt-2 space-y-2">
                <div
                  v-for="stage in template.stages"
                  :key="stage.stageId"
                  class="flex items-center justify-between rounded-md border border-gray-200 bg-white px-4 py-2"
                >
                  <span class="text-sm text-gray-700">{{ stage.stageName }}</span>
                  <span class="text-xs text-gray-500">未開始</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
