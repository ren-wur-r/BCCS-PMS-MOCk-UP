<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from "vue";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbEmployeePipelineStageCheckEnum } from "@/constants/DbEmployeePipelineStageCheckEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import GetManyManagerCompanyComboBox from "@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue";
import { loadProjectTemplateSettings } from "@/stores/projectTemplateSettings";

const emit = defineEmits<{
  (e: "close"): void;
  (e: "created"): void;
}>();

const steps = ["基本資訊", "確認"];
const stepIndex = ref(0);
const employeeInfoStore = useEmployeeInfoStore();
const templateSettings = ref(loadProjectTemplateSettings());

const form = reactive({
  salerEmployeeName: "" as string | null,
  managerCompanyID: null as number | null,
  managerCompanyName: "" as string | null,
  managerCompanyUnifiedNumber: "",
  bookingCode: "",
  customerRegionCode: "" as string,
  isRenewal: false,
  isOutsourced: false,
});

const isStageFieldConfirmed = (value: DbEmployeePipelineStageCheckEnum | null): boolean =>
  value === DbEmployeePipelineStageCheckEnum.Confirmed;

const derivedPipelineStatus = computed(() => {
  const hasNeed = isStageFieldConfirmed(stageStatusForm.businessNeedStatus);
  const hasTimeline = isStageFieldConfirmed(stageStatusForm.businessTimelineStatus);
  const hasBudget = isStageFieldConfirmed(stageStatusForm.businessBudgetStatus);
  const hasPresentation = isStageFieldConfirmed(stageStatusForm.businessPresentationStatus);
  const hasQuotation = isStageFieldConfirmed(stageStatusForm.businessQuotationStatus);
  const hasNegotiation = isStageFieldConfirmed(stageStatusForm.businessNegotiationStatus);

  if (hasNeed && hasTimeline && hasBudget && hasPresentation && hasQuotation && hasNegotiation) {
    return DbAtomPipelineStatusEnum.Business70Percent;
  }
  if (hasNeed && hasTimeline && hasBudget && hasPresentation) {
    return DbAtomPipelineStatusEnum.Business30Percent;
  }
  if (hasNeed) {
    return DbAtomPipelineStatusEnum.Business10Percent;
  }
  return DbAtomPipelineStatusEnum.TransferredToBusiness;
});

const regionLabelMap: Record<string, string> = {
  A: "北區",
  B: "中區",
  C: "南區",
};

const stageStatusFields = [
  {
    key: "businessNeedStatus",
    label: "需求",
    description: "確認客戶有明確的合作需求與方向。",
  },
  {
    key: "businessTimelineStatus",
    label: "時程",
    description: "已了解客戶預計啟動與完成的時程安排。",
  },
  {
    key: "businessBudgetStatus",
    label: "預算",
    description: "已確認客戶有對應預算並做好初步核准。",
  },
  {
    key: "businessPresentationStatus",
    label: "簡報",
    description: "已安排簡報或說明會，並取得客戶初步回饋。",
  },
  {
    key: "businessQuotationStatus",
    label: "報價",
    description: "已提供正式報價單或提案文件予客戶。",
  },
  {
    key: "businessNegotiationStatus",
    label: "議價",
    description: "目前與客戶進入議價或合約協商階段。",
  },
];

const stageStatusOptions = [
  { value: DbEmployeePipelineStageCheckEnum.NotConfirmed, label: "未確認" },
  { value: DbEmployeePipelineStageCheckEnum.Confirmed, label: "已確認" },
  { value: DbEmployeePipelineStageCheckEnum.NotApplicable, label: "不適用" },
];
const confirmedLabelOverrides: Record<string, string> = {
  businessBudgetStatus: "有預算",
  businessPresentationStatus: "已簡報",
  businessQuotationStatus: "已報價",
  businessNegotiationStatus: "議價中",
};
const getStatusOptionLabel = (fieldKey: string, optionLabel: string) => {
  if (optionLabel !== "已確認") return optionLabel;
  return confirmedLabelOverrides[fieldKey] ?? optionLabel;
};

const stageStatusForm = reactive<Record<string, DbEmployeePipelineStageCheckEnum | null>>({
  businessNeedStatus: null,
  businessTimelineStatus: null,
  businessBudgetStatus: null,
  businessPresentationStatus: null,
  businessQuotationStatus: null,
  businessNegotiationStatus: null,
});

const stageStatusNotes = reactive<Record<string, string>>({
  businessNeedStatus: "",
  businessTimelineStatus: "",
  businessBudgetStatus: "",
  businessPresentationStatus: "",
  businessQuotationStatus: "",
  businessNegotiationStatus: "",
});

const canNext = computed(() => {
  if (stepIndex.value === 0) {
    return (
      Boolean(form.salerEmployeeName) &&
      Boolean(form.customerRegionCode) &&
      Boolean(form.managerCompanyName) &&
      Boolean(selectedOpportunityTypeId.value) &&
      selectedServiceItemIds.value.length > 0 &&
      hasSelectedProducts.value
    );
  }
  return true;
});

const goNext = () => {
  if (!canNext.value) return;
  stepIndex.value = Math.min(stepIndex.value + 1, steps.length - 1);
};

const goPrev = () => {
  stepIndex.value = Math.max(stepIndex.value - 1, 0);
};

const createPipeline = () => {
  const listKey = "cache.crm.pipeline.list";
  const detailKeyPrefix = "cache.crm.pipeline.detail.";
  const listRaw = localStorage.getItem(listKey);
  const list = listRaw ? JSON.parse(listRaw) : [];
  const nextId = list.reduce((max: number, item: { id: number }) => Math.max(max, item.id), 0) + 1;
  const record = {
    id: nextId,
    status: derivedPipelineStatus.value,
    companyName: form.managerCompanyName ?? "",
    contacterDepartment: "",
    contacterJobTitle: "",
    contacterName: "",
    contacterEmail: "",
    contacterPhone: "",
    contacterTelephone: "",
    salerEmployeeName: form.salerEmployeeName ?? "",
    managerCompanyUnifiedNumber: form.managerCompanyUnifiedNumber,
    bookingCode: form.bookingCode,
    customerRegionCode: form.customerRegionCode,
    isRenewal: form.isRenewal,
    isOutsourced: form.isOutsourced,
    stageStatus: { ...stageStatusForm, notes: { ...stageStatusNotes } },
    opportunityTypeId: selectedOpportunityTypeId.value,
    serviceItemIds: [...selectedServiceItemIds.value],
    serviceProductIds: { ...selectedServiceProductIds.value },
  };
  list.unshift(record);
  localStorage.setItem(listKey, JSON.stringify(list));
  localStorage.setItem(`${detailKeyPrefix}${nextId}`, JSON.stringify(record));
  emit("created");
  emit("close");
};

onMounted(() => {
  form.salerEmployeeName = employeeInfoStore.employeeName || "目前登入者";
});

const projectTypeTemplates = computed(() => templateSettings.value.projectTypes);
const serviceItemTemplates = computed(() => templateSettings.value.serviceItems);
const selectedOpportunityTypeId = ref<number | null>(
  projectTypeTemplates.value.length > 0 ? projectTypeTemplates.value[0].projectTypeId : null
);
const selectedServiceItemIds = ref<number[]>([]);
const selectedServiceProductIds = ref<Record<number, number[]>>({});
const selectedOpportunityType = computed(
  () =>
    projectTypeTemplates.value.find(
      (item) => item.projectTypeId === selectedOpportunityTypeId.value
    ) ?? null
);
const availableServiceItems = computed(() => {
  if (!selectedOpportunityType.value) return [];
  return serviceItemTemplates.value.filter((item) =>
    selectedOpportunityType.value?.serviceItemIds.includes(item.serviceItemId)
  );
});
const displayServiceItems = computed(() =>
  availableServiceItems.value.length > 0 ? availableServiceItems.value : serviceItemTemplates.value
);
const selectedServiceItems = computed(() =>
  displayServiceItems.value.filter((item) =>
    selectedServiceItemIds.value.includes(item.serviceItemId)
  )
);
const selectedServiceProducts = computed(() =>
  selectedServiceItems.value.map((service) => {
    const productIds = selectedServiceProductIds.value[service.serviceItemId] ?? [];
    return {
      service,
      products: service.products.filter((product) => productIds.includes(product.productId)),
    };
  })
);
const hasSelectedProducts = computed(
  () =>
    selectedServiceItems.value.length > 0 &&
    selectedServiceItems.value.every(
      (service) => (selectedServiceProductIds.value[service.serviceItemId] ?? []).length > 0
    )
);
const collapsedStageKeys = ref<Set<string>>(new Set());
const toggleStageCollapse = (key: string) => {
  const next = new Set(collapsedStageKeys.value);
  if (next.has(key)) {
    next.delete(key);
  } else {
    next.add(key);
  }
  collapsedStageKeys.value = next;
};
const isStageCollapsed = (key: string) => collapsedStageKeys.value.has(key);
const stageGroups = computed(() =>
  selectedServiceProducts.value.map((serviceEntry) => ({
    service: serviceEntry.service,
    products: serviceEntry.products.map((product) => ({
      product,
      templates: templateSettings.value.stageTemplates.filter((template) =>
        product.stageTemplateIds.includes(template.stageTemplateId)
      ),
    })),
  }))
);
const hasStageTemplates = computed(() =>
  stageGroups.value.some((group) =>
    group.products.some((productGroup) => productGroup.templates.length > 0)
  )
);

const toggleServiceItem = (serviceItemId: number) => {
  if (!selectedOpportunityType.value) return;
  if (selectedOpportunityType.value.category === "single") {
    selectedServiceItemIds.value = [serviceItemId];
    selectedServiceProductIds.value = { [serviceItemId]: [] };
    return;
  }
  const set = new Set(selectedServiceItemIds.value);
  if (set.has(serviceItemId)) {
    set.delete(serviceItemId);
    const { [serviceItemId]: _, ...rest } = selectedServiceProductIds.value;
    selectedServiceProductIds.value = rest;
  } else {
    set.add(serviceItemId);
    selectedServiceProductIds.value = {
      ...selectedServiceProductIds.value,
      [serviceItemId]: selectedServiceProductIds.value[serviceItemId] ?? [],
    };
  }
  selectedServiceItemIds.value = Array.from(set);
};

const toggleProduct = (serviceItemId: number, productId: number) => {
  const existing = new Set(selectedServiceProductIds.value[serviceItemId] ?? []);
  if (existing.has(productId)) {
    existing.delete(productId);
  } else {
    existing.add(productId);
  }
  selectedServiceProductIds.value = {
    ...selectedServiceProductIds.value,
    [serviceItemId]: Array.from(existing),
  };
};

watch(
  () => selectedOpportunityTypeId.value,
  () => {
    if (!selectedOpportunityType.value) {
      selectedServiceItemIds.value = [];
      selectedServiceProductIds.value = {};
      return;
    }
    selectedServiceItemIds.value = [];
    selectedServiceProductIds.value = {};
  },
  { immediate: true }
);
</script>

<template>
  <div class="flex flex-col gap-4 p-6 project-add-font">
    <div class="flex items-center justify-between">
      <h2 class="subtitle">新增商機</h2>
      <div></div>
    </div>

    <div class="flex flex-col bg-white rounded-lg p-6 gap-4">
      <template v-if="stepIndex === 0">
        <div class="section-header">
          <h2 class="subtitle">基本資訊</h2>
        </div>
        <div class="section-content">
          <div class="grid grid-cols-2 gap-4">
            <div class="flex flex-col gap-2">
              <label class="form-label">承辦業務 <span class="required-label">*</span></label>
              <input
                v-model="form.salerEmployeeName"
                type="text"
                class="input-box"
                disabled
              />
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-4 gap-4 mt-4">
            <div class="flex flex-col gap-2">
              <label class="form-label">客戶所在地 <span class="required-label">*</span></label>
              <div class="relative flex w-full">
                <select v-model="form.customerRegionCode" class="input-box rounded-r-none select-reset">
                  <option value="">請選擇</option>
                  <option value="A">北區</option>
                  <option value="B">中區</option>
                  <option value="C">南區</option>
                </select>
                <button
                  type="button"
                  class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white"
                  disabled
                >
                  <svg class="w-5 h-5 text-gray-500" viewBox="0 0 20 25" fill="none">
                    <path
                      d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                      fill="#787676"
                    />
                  </svg>
                </button>
              </div>
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">客戶 <span class="required-label">*</span></label>
              <GetManyManagerCompanyComboBox
                :disabled="false"
                v-model:managerCompanyID="form.managerCompanyID"
                v-model:managerCompanyName="form.managerCompanyName"
              />
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">統編</label>
              <input v-model="form.managerCompanyUnifiedNumber" type="text" class="input-box" />
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">Booking Code</label>
              <input v-model="form.bookingCode" type="text" class="input-box" />
            </div>
          </div>

          <div class="mt-6">
            <div class="flex items-center justify-between">
              <h2 class="subtitle mt-2">選擇商機類型與服務項目</h2>
            </div>
            <p class="text-xs text-gray-500 mt-1">
              選擇完後，系統將套用標準階段、產出與提醒規則。
            </p>

            <div class="flex gap-4 w-full mt-4">
              <div class="flex-1">
                <h2 class="subtitle mt-2 text-gray-700">商機類型</h2>
                <div class="mt-2 grid grid-cols-2 gap-2">
                  <button
                    v-for="type in projectTypeTemplates"
                    :key="type.projectTypeId"
                    type="button"
                    class="rounded-lg border px-3 py-2 text-left transition"
                    :class="
                      selectedOpportunityTypeId === type.projectTypeId
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 hover:border-cyan-400'
                    "
                    @click="selectedOpportunityTypeId = type.projectTypeId"
                  >
                    <p class="font-semibold">{{ type.projectTypeName }}</p>
                    <p class="text-xs text-gray-500">{{ type.projectTypeDescription }}</p>
                  </button>
                </div>
              </div>
            </div>

            <div class="mt-4">
              <h2 class="subtitle mt-2 text-gray-700">服務項目</h2>
              <div class="grid grid-cols-3 gap-2 mt-2">
                <button
                  v-for="item in displayServiceItems"
                  :key="item.serviceItemId"
                  type="button"
                  class="rounded-lg border px-3 py-2 text-left transition"
                  :class="
                    selectedServiceItemIds.includes(item.serviceItemId)
                      ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                      : 'border-gray-200 hover:border-cyan-400'
                  "
                  @click="toggleServiceItem(item.serviceItemId)"
                >
                  <p class="font-semibold">{{ item.serviceItemName }}</p>
                </button>
              </div>
            </div>

            <div class="mt-4">
              <h2 class="subtitle mt-2 text-gray-700">產品</h2>
              <p v-if="selectedServiceItems.length === 0" class="text-sm text-gray-500">
                請先選擇服務項目以設定產品。
              </p>
              <div v-else class="space-y-4 mt-2">
                <div
                  v-for="service in selectedServiceItems"
                  :key="service.serviceItemId"
                  class="space-y-2"
                >
                  <p class="text-xs text-gray-500">{{ service.serviceItemName }}</p>
                  <div class="grid grid-cols-3 gap-2">
                    <button
                      v-for="product in service.products"
                      :key="product.productId"
                      type="button"
                      class="rounded-lg border px-3 py-2 text-left transition"
                      :class="
                        (selectedServiceProductIds[service.serviceItemId] ?? []).includes(
                          product.productId
                        )
                          ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                          : 'border-gray-200 hover:border-cyan-400'
                      "
                      @click="toggleProduct(service.serviceItemId, product.productId)"
                    >
                      <p class="font-semibold">{{ product.productName }}</p>
                      <p class="text-xs text-gray-500 line-clamp-1">
                        {{ product.description || '無描述' }}
                      </p>
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <div class="mt-4">
              <h2 class="subtitle mt-2 text-gray-700">商機屬性</h2>
              <div class="grid grid-cols-3 gap-2 mt-2">
                <button
                  type="button"
                  class="rounded-lg border px-3 py-2 text-left transition"
                  :class="
                    form.isRenewal
                      ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                      : 'border-gray-200 hover:border-cyan-400'
                  "
                  @click="form.isRenewal = !form.isRenewal"
                >
                  <p class="font-semibold">續約</p>
                </button>
                <button
                  type="button"
                  class="rounded-lg border px-3 py-2 text-left transition"
                  :class="
                    form.isOutsourced
                      ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                      : 'border-gray-200 hover:border-cyan-400'
                  "
                  @click="form.isOutsourced = !form.isOutsourced"
                >
                  <p class="font-semibold">委外</p>
                </button>
              </div>
              <p class="text-xs text-gray-500 mt-1">未勾選續約即視為新案。</p>
            </div>

          </div>

          <div class="overflow-x-auto mt-4">
            <table class="table-base table-fixed table-sticky w-full min-w-[720px]">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start w-24">條件</th>
                  <th class="text-start w-44">狀態</th>
                  <th class="text-start w-56">備註</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="field in stageStatusFields" :key="field.key" class="border border-gray-300">
                  <td class="text-start font-semibold text-gray-700">{{ field.label }}</td>
                  <td class="text-start">
                    <div class="flex flex-wrap gap-2">
                      <button
                        v-for="option in stageStatusOptions"
                        :key="option.value"
                        type="button"
                        class="rounded-md border px-2 py-1 text-xs font-semibold transition"
                        :class="
                          stageStatusForm[field.key] === option.value
                            ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                            : 'border-gray-200 text-gray-600 hover:border-cyan-300'
                        "
                        @click="
                          stageStatusForm[field.key] =
                            stageStatusForm[field.key] === option.value ? null : option.value
                        "
                      >
                        {{ getStatusOptionLabel(field.key, option.label) }}
                      </button>
                    </div>
                  </td>
                  <td class="text-start">
                    <template v-if="field.key === 'businessPresentationStatus'">
                      <select
                        v-if="stageStatusForm[field.key] === DbEmployeePipelineStageCheckEnum.Confirmed"
                        v-model="stageStatusNotes[field.key]"
                        class="select-box"
                      >
                        <option value="">請選擇</option>
                        <option value="李子涵">李子涵</option>
                        <option value="盧彥伶">盧彥伶</option>
                        <option value="施祥倫">施祥倫</option>
                      </select>
                      <input
                        v-else
                        v-model="stageStatusNotes[field.key]"
                        type="text"
                        class="input-box"
                        placeholder="請輸入備註"
                      />
                    </template>
                    <input
                      v-else
                      v-model="stageStatusNotes[field.key]"
                      type="text"
                      class="input-box"
                      placeholder="請輸入備註"
                    />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </template>

      <template v-else>
        <div class="section-header">
          <h2 class="subtitle">確認</h2>
        </div>
        <div class="section-content">
          <div class="grid grid-cols-2 gap-4 text-sm text-gray-700">
            <div>
              <p class="text-xs text-gray-500">商機狀態</p>
              <p class="font-semibold">{{ getPipelineStatusLabel(derivedPipelineStatus) }}</p>
            </div>
            <div>
              <p class="text-xs text-gray-500">承辦業務</p>
              <p class="font-semibold">{{ form.salerEmployeeName || "-" }}</p>
            </div>
            <div>
              <p class="text-xs text-gray-500">客戶名稱</p>
              <p class="font-semibold">{{ form.managerCompanyName || "-" }}</p>
            </div>
            <div>
              <p class="text-xs text-gray-500">統編</p>
              <p class="font-semibold">{{ form.managerCompanyUnifiedNumber || "-" }}</p>
            </div>
            <div>
              <p class="text-xs text-gray-500">客戶所在地區</p>
              <p class="font-semibold">
                {{ regionLabelMap[form.customerRegionCode] || "-" }}
              </p>
            </div>
            <div>
              <p class="text-xs text-gray-500">Booking Code</p>
              <p class="font-semibold">{{ form.bookingCode || "-" }}</p>
            </div>
            <div>
              <p class="text-xs text-gray-500">案別</p>
              <p class="font-semibold">{{ form.isRenewal ? "續約" : "新案" }}</p>
            </div>
            <div>
              <p class="text-xs text-gray-500">委外</p>
              <p class="font-semibold">{{ form.isOutsourced ? "委外" : "非委外" }}</p>
            </div>
          </div>
        </div>
      </template>

      <div class="flex justify-center mt-3 gap-2">
        <button class="btn-cancel" type="button" @click="emit('close')">取消建立</button>
        <button v-if="stepIndex > 0" class="btn-cancel" type="button" @click="goPrev">
          上一步
        </button>
        <button v-if="stepIndex === 0" class="btn-submit" type="button" :disabled="!canNext" @click="goNext">
          下一步
        </button>
        <button v-else class="btn-submit" type="button" @click="createPipeline">
          建立商機
        </button>
      </div>
    </div>
  </div>
</template>
