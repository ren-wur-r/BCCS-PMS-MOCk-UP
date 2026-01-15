<script setup lang="ts">
import { ref, computed, onMounted, watch } from "vue";
import draggable from "vuedraggable";
import { useRouter } from "vue-router";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import {
  loadProjectTemplateSettings,
  saveProjectTemplateSettings,
  type ProjectTypeTemplate,
  type ServiceItemProductTemplate,
  type ServiceItemTemplate,
  type StageTemplate,
  type TemplateStage,
  type TemplateOutput,
} from "@/stores/projectTemplateSettings";

const router = useRouter();
const employeeInfoStore = useEmployeeInfoStore();
const canEditStageHours = computed(() => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  return roleName.includes("各部門經理") || roleName.includes("Admin");
});

const templateSettings = ref(loadProjectTemplateSettings());
const projectTypes = computed(() => templateSettings.value.projectTypes);
const serviceItems = computed(() => templateSettings.value.serviceItems);
const stageTemplates = computed(() => templateSettings.value.stageTemplates);

const activeTab = ref<"projectType" | "serviceItem">("projectType");
const canViewProjectType = computed(() => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  return !roleName.includes("各部門經理");
});

const selectedProjectTypeId = ref<number | null>(null);
const selectedServiceItemId = ref<number | null>(null);
const selectedServiceProductId = ref<number | null>(null);
const selectedStageTemplateId = ref<number | null>(null);
const deliveryMode = ref<"product" | "poc">("product");

const selectedProjectType = computed(
  () => projectTypes.value.find((item) => item.projectTypeId === selectedProjectTypeId.value) ?? null
);
const selectedServiceItem = computed(
  () => serviceItems.value.find((item) => item.serviceItemId === selectedServiceItemId.value) ?? null
);
const selectedServiceProduct = computed(
  () =>
    selectedServiceItem.value?.products.find(
      (product) => product.productId === selectedServiceProductId.value
    ) ?? null
);
const filteredServiceProducts = computed(() => {
  if (!selectedServiceItem.value) return [];
  const keyword = productSearch.value.trim();
  if (!keyword) return selectedServiceItem.value.products;
  return selectedServiceItem.value.products.filter((product) => {
    const nameMatch = product.productName.includes(keyword);
    const descMatch = (product.description ?? "").includes(keyword);
    return nameMatch || descMatch;
  });
});
const visibleServiceProducts = computed(() => {
  if (!selectedServiceItem.value) return [];
  if (productSearch.value.trim()) return filteredServiceProducts.value;
  if (showAllProducts.value) return filteredServiceProducts.value;
  return filteredServiceProducts.value.slice(0, 5);
});
const hasMoreProducts = computed(
  () =>
    !productSearch.value.trim() &&
    filteredServiceProducts.value.length > 5 &&
    !showAllProducts.value
);
const selectedStageTemplate = computed(
  () =>
    stageTemplates.value.find((item) => item.stageTemplateId === selectedStageTemplateId.value) ??
    null
);
const defaultStageTemplateId = computed(() => {
  const firstProductTemplateId =
    selectedServiceItem.value?.products[0]?.stageTemplateIds[0] ?? null;
  return firstProductTemplateId ?? stageTemplates.value[0]?.stageTemplateId ?? null;
});
const lastEditedLabel = computed(() => {
  const editedBy = selectedStageTemplate.value?.lastEditedBy?.trim() || "無";
  const editedAt = selectedStageTemplate.value?.lastEditedAt?.trim() || "";
  if (!editedAt) return `最後編輯者：${editedBy}`;
  const date = new Date(editedAt);
  const formatted = Number.isNaN(date.getTime()) ? editedAt : date.toLocaleString();
  return `最後編輯者：${editedBy}｜最後編輯時間：${formatted}`;
});
const touchStageTemplate = (template: StageTemplate | null) => {
  if (!template) return;
  template.lastEditedBy = "本機使用者";
  template.lastEditedAt = new Date().toISOString();
};
const checkboxClass = "h-5 w-5 rounded border-gray-300 text-cyan-600 focus:ring-cyan-500 focus:ring-2";

const isEditingProjectType = ref(false);
const isProjectTypeReadonly = computed(() => !isEditingProjectType.value);
const projectTypeForm = ref({
  projectTypeName: "",
  description: "",
  category: "single" as ProjectTypeTemplate["category"],
  serviceItemIds: [] as number[],
  requireOrder: true,
  requireSla: true,
});

const isEditingServiceItem = ref(false);
const serviceItemForm = ref({
  serviceItemName: "",
  description: "",
  departmentLabel: "",
});

const isEditingServiceProduct = ref(false);
const serviceProductForm = ref({
  productName: "",
  description: "",
  copyStageTemplateId: null as number | null,
});
const productSearch = ref("");
const showAllProducts = ref(false);
const pendingCopyTemplateId = ref<number | null>(null);
const showCopyConfirm = ref(false);

const stripRenewalLabel = (value: string) =>
  value
    .replace(/\bRenewal\b/gi, "")
    .replace(/[（(]續約[)）]/g, "")
    .replace(/續約/g, "")
    .replace(/\s{2,}/g, " ")
    .trim();

const getProductTitle = (product: ServiceItemProductTemplate) => {
  const rawTitle = product.description || product.productName || "";
  const normalizedTitle = stripRenewalLabel(rawTitle);
  const brand = stripRenewalLabel(product.productName || "");
  if (brand && normalizedTitle.startsWith(brand)) {
    return normalizedTitle.slice(brand.length).trim() || brand;
  }
  return normalizedTitle || "無描述";
};

const getProductSubTitle = (product: ServiceItemProductTemplate) => {
  if (!product.description) return "";
  const brand = stripRenewalLabel(product.productName || "");
  return brand || "";
};

const isEditingStageTemplate = ref(false);
const stageTemplateForm = ref({
  stageTemplateName: "",
  description: "",
  departmentLabel: "",
});

const isAddingStage = ref(false);
const editingStageId = ref<number | null>(null);
const stageForm = ref({
  stageName: "",
  ownerRoleLabel: "",
  requiresWorkLog: true,
  requiresOnsiteLog: false,
  reminderDays: [] as number[],
  reminderAnchor: "start" as TemplateStage["reminderAnchor"],
  estimatedDays: "",
  estimatedHours: "",
  onsiteHours: "",
  offsiteHours: "",
  deadlineDays: "",
  deadlineAnchor: "start" as TemplateStage["reminderAnchor"],
  requireEstimatedDays: false,
  requireEstimatedHours: false,
});
const shouldShowStageSection = computed(() => {
  if (!selectedStageTemplate.value) return false;
  return (
    selectedStageTemplate.value.stages.length > 0 ||
    isAddingStage.value ||
    editingStageId.value !== null
  );
});

const isAddingOutput = ref(false);
const editingOutputId = ref<number | null>(null);
const outputForm = ref({
  stageId: 0,
  outputName: "",
  outputKind: "document" as TemplateOutput["outputKind"],
  outputKindLabel: "",
  required: true,
});

const persistSettings = () => {
  saveProjectTemplateSettings(templateSettings.value);
};

const parseOptionalNumber = (value: string) => {
  if (value === "") return undefined;
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : undefined;
};

const startAddProjectType = () => {
  isEditingProjectType.value = true;
  selectedProjectTypeId.value = null;
  projectTypeForm.value = {
    projectTypeName: "",
    description: "",
    category: "single",
    serviceItemIds: [],
    requireOrder: true,
    requireSla: true,
  };
};

const startEditProjectType = (projectType: ProjectTypeTemplate) => {
  isEditingProjectType.value = true;
  selectedProjectTypeId.value = projectType.projectTypeId;
  projectTypeForm.value = {
    projectTypeName: projectType.projectTypeName,
    description: projectType.description,
    category: projectType.category,
    serviceItemIds: [...projectType.serviceItemIds],
    requireOrder: projectType.requiredAttachments.order,
    requireSla: projectType.requiredAttachments.sla,
  };
};

const startViewProjectType = (projectType: ProjectTypeTemplate) => {
  isEditingProjectType.value = false;
  selectedProjectTypeId.value = projectType.projectTypeId;
  projectTypeForm.value = {
    projectTypeName: projectType.projectTypeName,
    description: projectType.description,
    category: projectType.category,
    serviceItemIds: [...projectType.serviceItemIds],
    requireOrder: projectType.requiredAttachments.order,
    requireSla: projectType.requiredAttachments.sla,
  };
};

const toggleProjectTypeServiceItem = (serviceItemId: number) => {
  const ids = new Set(projectTypeForm.value.serviceItemIds);
  if (ids.has(serviceItemId)) {
    ids.delete(serviceItemId);
  } else {
    if (projectTypeForm.value.category === "single") {
      ids.clear();
    }
    ids.add(serviceItemId);
  }
  projectTypeForm.value.serviceItemIds = Array.from(ids);
};

const saveProjectType = () => {
  const name = projectTypeForm.value.projectTypeName.trim();
  if (!name) return;
  const normalizedServiceItemIds =
    projectTypeForm.value.category === "single"
      ? projectTypeForm.value.serviceItemIds.slice(0, 1)
      : [...projectTypeForm.value.serviceItemIds];

  if (!selectedProjectType.value || selectedProjectTypeId.value === null) {
    const newId = Math.max(0, ...projectTypes.value.map((item) => item.projectTypeId)) + 1;
    templateSettings.value.projectTypes.push({
      projectTypeId: newId,
      projectTypeName: name,
      description: projectTypeForm.value.description.trim(),
      category: projectTypeForm.value.category,
      serviceItemIds: normalizedServiceItemIds,
      requiredAttachments: {
        order: projectTypeForm.value.requireOrder,
        sla: projectTypeForm.value.requireSla,
      },
    });
    selectedProjectTypeId.value = newId;
  } else {
    selectedProjectType.value.projectTypeName = name;
    selectedProjectType.value.description = projectTypeForm.value.description.trim();
    selectedProjectType.value.category = projectTypeForm.value.category;
    selectedProjectType.value.serviceItemIds = normalizedServiceItemIds;
    selectedProjectType.value.requiredAttachments.order = projectTypeForm.value.requireOrder;
    selectedProjectType.value.requiredAttachments.sla = projectTypeForm.value.requireSla;
  }

  isEditingProjectType.value = false;
  persistSettings();
};

const cancelProjectType = () => {
  isEditingProjectType.value = false;
};

const startAddServiceItem = () => {
  isEditingServiceItem.value = true;
  selectedServiceItemId.value = null;
  serviceItemForm.value = {
    serviceItemName: "",
    description: "",
    departmentLabel: "",
  };
};

const startViewServiceItem = (serviceItem: ServiceItemTemplate) => {
  isEditingServiceItem.value = false;
  selectedServiceItemId.value = serviceItem.serviceItemId;
  serviceItemForm.value = {
    serviceItemName: serviceItem.serviceItemName,
    description: serviceItem.description,
    departmentLabel: serviceItem.departmentLabel,
  };
};

const startEditServiceItem = (serviceItem: ServiceItemTemplate) => {
  isEditingServiceItem.value = true;
  selectedServiceItemId.value = serviceItem.serviceItemId;
  serviceItemForm.value = {
    serviceItemName: serviceItem.serviceItemName,
    description: serviceItem.description,
    departmentLabel: serviceItem.departmentLabel,
  };
};

const saveServiceItem = () => {
  const name = serviceItemForm.value.serviceItemName.trim();
  if (!name) return;

  if (!selectedServiceItem.value || selectedServiceItemId.value === null) {
    const newId = Math.max(0, ...serviceItems.value.map((item) => item.serviceItemId)) + 1;
    templateSettings.value.serviceItems.push({
      serviceItemId: newId,
      serviceItemName: name,
      description: serviceItemForm.value.description.trim(),
      departmentLabel: serviceItemForm.value.departmentLabel.trim() || "未設定",
      products: [],
    });
    selectedServiceItemId.value = newId;
  } else {
    selectedServiceItem.value.serviceItemName = name;
    selectedServiceItem.value.description = serviceItemForm.value.description.trim();
    selectedServiceItem.value.departmentLabel =
      serviceItemForm.value.departmentLabel.trim() || "未設定";
  }

  isEditingServiceItem.value = false;
  persistSettings();
};

const cancelServiceItem = () => {
  isEditingServiceItem.value = false;
};

const startAddStageTemplate = () => {
  if (!selectedServiceProduct.value) return;
  isEditingStageTemplate.value = true;
  selectedStageTemplateId.value = null;
  stageTemplateForm.value = {
    stageTemplateName: "",
    description: "",
    departmentLabel: "",
  };
};

const handleAddStandardStage = () => {
  if (selectedStageTemplate.value) {
    startAddStage();
  } else {
    startAddStageTemplate();
  }
};

const startEditStageTemplate = (stageTemplate: StageTemplate) => {
  isEditingStageTemplate.value = true;
  selectedStageTemplateId.value = stageTemplate.stageTemplateId;
  stageTemplateForm.value = {
    stageTemplateName: stageTemplate.stageTemplateName,
    description: stageTemplate.description,
    departmentLabel: stageTemplate.departmentLabel,
  };
};

const saveStageTemplate = () => {
  const name = stageTemplateForm.value.stageTemplateName.trim();
  if (!name) return;

  if (!selectedStageTemplate.value || selectedStageTemplateId.value === null) {
    const newId = Math.max(0, ...stageTemplates.value.map((item) => item.stageTemplateId)) + 1;
    templateSettings.value.stageTemplates.push({
      stageTemplateId: newId,
      stageTemplateName: name,
      description: stageTemplateForm.value.description.trim(),
      departmentLabel: stageTemplateForm.value.departmentLabel.trim() || "未設定",
      stages: [],
      lastEditedBy: "本機使用者",
      lastEditedAt: new Date().toISOString(),
    });
    selectedStageTemplateId.value = newId;
    if (selectedServiceProduct.value) {
      selectedServiceProduct.value.stageTemplateIds = [
        ...new Set([...selectedServiceProduct.value.stageTemplateIds, newId]),
      ];
    }
  } else {
    selectedStageTemplate.value.stageTemplateName = name;
    selectedStageTemplate.value.description = stageTemplateForm.value.description.trim();
    selectedStageTemplate.value.departmentLabel =
      stageTemplateForm.value.departmentLabel.trim() || "未設定";
    touchStageTemplate(selectedStageTemplate.value);
  }

  isEditingStageTemplate.value = false;
  persistSettings();
};

const cancelStageTemplate = () => {
  isEditingStageTemplate.value = false;
};

const startAddServiceProduct = () => {
  if (!selectedServiceItem.value) return;
  isEditingServiceProduct.value = true;
  selectedServiceProductId.value = null;
  serviceProductForm.value = {
    productName: "",
    description: "",
    copyStageTemplateId: null,
  };
};

const startViewServiceProduct = (product: ServiceItemProductTemplate) => {
  isEditingServiceProduct.value = false;
  selectedServiceProductId.value = product.productId;
  serviceProductForm.value = {
    productName: product.productName,
    description: product.description,
    copyStageTemplateId: null,
  };
};

const startEditServiceProduct = (product: ServiceItemProductTemplate) => {
  isEditingServiceProduct.value = true;
  selectedServiceProductId.value = product.productId;
  serviceProductForm.value = {
    productName: product.productName,
    description: product.description,
    copyStageTemplateId: null,
  };
};

const createCopiedStageTemplate = (sourceTemplateId: number, productName: string) => {
  const sourceTemplate = stageTemplates.value.find(
    (template) => template.stageTemplateId === sourceTemplateId
  );
  if (!sourceTemplate) return null;
  const nextTemplateId =
    Math.max(0, ...stageTemplates.value.map((item) => item.stageTemplateId)) + 1;
  let nextStageId =
    Math.max(0, ...stageTemplates.value.flatMap((item) => item.stages.map((stage) => stage.stageId))) +
    1;
  let nextOutputId =
    Math.max(
      0,
      ...stageTemplates.value.flatMap((item) =>
        item.stages.flatMap((stage) => stage.requiredOutputs.map((output) => output.outputId))
      )
    ) + 1;

  const clonedStages: TemplateStage[] = sourceTemplate.stages.map((stage) => {
    const clonedOutputs: TemplateOutput[] = stage.requiredOutputs.map((output) => ({
      ...output,
      outputId: nextOutputId++,
    }));
    const clonedStage: TemplateStage = {
      ...stage,
      stageId: nextStageId++,
      requiredOutputs: clonedOutputs,
    };
    return clonedStage;
  });

  const clonedTemplate: StageTemplate = {
    stageTemplateId: nextTemplateId,
    stageTemplateName: `${sourceTemplate.stageTemplateName}（${productName}）`,
    description: sourceTemplate.description,
    departmentLabel: sourceTemplate.departmentLabel || "未設定",
    stages: clonedStages,
    lastEditedBy: "本機使用者",
    lastEditedAt: new Date().toISOString(),
  };

  templateSettings.value.stageTemplates.push(clonedTemplate);
  return clonedTemplate;
};

const finalizeSaveServiceProduct = (copyTemplateId: number | null) => {
  if (!selectedServiceItem.value) return;
  const name = serviceProductForm.value.productName.trim();
  if (!name) return;

  if (!selectedServiceProduct.value || selectedServiceProductId.value === null) {
    const newId =
      Math.max(0, ...selectedServiceItem.value.products.map((item) => item.productId)) + 1;
    selectedServiceItem.value.products.push({
      productId: newId,
      productName: name,
      description: serviceProductForm.value.description.trim(),
      stageTemplateIds: [],
    });
    selectedServiceProductId.value = newId;
  } else {
    selectedServiceProduct.value.productName = name;
    selectedServiceProduct.value.description = serviceProductForm.value.description.trim();
  }

  if (copyTemplateId !== null) {
    const copiedTemplate = createCopiedStageTemplate(copyTemplateId, name);
    if (copiedTemplate) {
      const targetProduct = selectedServiceProduct.value;
      if (targetProduct) {
        targetProduct.stageTemplateIds = [copiedTemplate.stageTemplateId];
      }
      selectedStageTemplateId.value = copiedTemplate.stageTemplateId;
    }
  }

  isEditingServiceProduct.value = false;
  serviceProductForm.value.copyStageTemplateId = null;
  persistSettings();
};

const saveServiceProduct = () => {
  const copyTemplateId = serviceProductForm.value.copyStageTemplateId;
  if (copyTemplateId) {
    pendingCopyTemplateId.value = copyTemplateId;
    showCopyConfirm.value = true;
    return;
  }
  finalizeSaveServiceProduct(null);
};

const confirmCopyTemplate = () => {
  const copyTemplateId = pendingCopyTemplateId.value;
  showCopyConfirm.value = false;
  pendingCopyTemplateId.value = null;
  finalizeSaveServiceProduct(copyTemplateId ?? null);
};

const cancelCopyTemplate = () => {
  showCopyConfirm.value = false;
  pendingCopyTemplateId.value = null;
};

const cancelServiceProduct = () => {
  isEditingServiceProduct.value = false;
  serviceProductForm.value.copyStageTemplateId = null;
};

const removeServiceProduct = (product: ServiceItemProductTemplate) => {
  if (!selectedServiceItem.value) return;
  const confirmed = window.confirm(`確定要刪除產品「${product.productName}」嗎？`);
  if (!confirmed) return;
  selectedServiceItem.value.products = selectedServiceItem.value.products.filter(
    (item) => item.productId !== product.productId
  );
  if (selectedServiceProductId.value === product.productId) {
    selectedServiceProductId.value = selectedServiceItem.value.products[0]?.productId ?? null;
    selectedStageTemplateId.value = selectedServiceProduct.value?.stageTemplateIds[0] ?? null;
  }
  persistSettings();
};

const startAddStage = () => {
  if (!selectedStageTemplate.value) return;
  isAddingStage.value = true;
  editingStageId.value = null;
  stageForm.value = {
    stageName: "",
    ownerRoleLabel: "",
    requiresWorkLog: true,
    requiresOnsiteLog: false,
    reminderDays: [],
    reminderAnchor: "start",
    estimatedDays: "",
    estimatedHours: "",
    onsiteHours: "",
    offsiteHours: "",
    deadlineDays: "",
    deadlineAnchor: "start",
    requireEstimatedDays: false,
    requireEstimatedHours: false,
  };
};

const startEditStage = (stage: TemplateStage) => {
  editingStageId.value = stage.stageId;
  isAddingStage.value = false;
  stageForm.value = {
    stageName: stage.stageName,
    ownerRoleLabel: stage.ownerRoleLabel,
    requiresWorkLog: stage.requiresWorkLog,
    requiresOnsiteLog: stage.requiresOnsiteLog,
    reminderDays: [...stage.reminderDays],
    reminderAnchor: stage.reminderAnchor ?? "start",
    estimatedDays: stage.estimatedDays?.toString() ?? "",
    estimatedHours: stage.estimatedHours?.toString() ?? "",
    onsiteHours: stage.onsiteHours?.toString() ?? "",
    offsiteHours: stage.offsiteHours?.toString() ?? "",
    deadlineDays: stage.deadlineDays?.toString() ?? "",
    deadlineAnchor: stage.deadlineAnchor ?? "start",
    requireEstimatedDays: stage.requireEstimatedDays ?? false,
    requireEstimatedHours: stage.requireEstimatedHours ?? false,
  };
};

const saveStage = () => {
  if (!selectedStageTemplate.value) return;
  if (isAddingStage.value) {
    const fallbackEstimatedDays = canEditStageHours.value
      ? parseOptionalNumber(stageForm.value.estimatedDays)
      : null;
    const fallbackEstimatedHours = canEditStageHours.value
      ? parseOptionalNumber(stageForm.value.estimatedHours)
      : null;
    const fallbackOnsiteHours = canEditStageHours.value
      ? parseOptionalNumber(stageForm.value.onsiteHours)
      : null;
    const fallbackOffsiteHours = canEditStageHours.value
      ? parseOptionalNumber(stageForm.value.offsiteHours)
      : null;
    const fallbackDeadlineDays = parseOptionalNumber(stageForm.value.deadlineDays);
    const newId =
      Math.max(0, ...selectedStageTemplate.value.stages.map((item) => item.stageId)) + 1;
    selectedStageTemplate.value.stages.push({
      stageId: newId,
      stageName: stageForm.value.stageName.trim() || "未命名階段",
      ownerRoleLabel: stageForm.value.ownerRoleLabel.trim() || "未設定",
      requiredOutputs: [],
      requiresWorkLog: stageForm.value.requiresWorkLog,
      requiresOnsiteLog: stageForm.value.requiresOnsiteLog,
      reminderDays: [...stageForm.value.reminderDays],
      reminderAnchor: stageForm.value.reminderAnchor,
      estimatedDays: fallbackEstimatedDays,
      estimatedHours: fallbackEstimatedHours,
      onsiteHours: fallbackOnsiteHours,
      offsiteHours: fallbackOffsiteHours,
      deadlineDays: fallbackDeadlineDays,
      deadlineAnchor: stageForm.value.deadlineAnchor,
      requireEstimatedDays: canEditStageHours.value
        ? stageForm.value.requireEstimatedDays
        : false,
      requireEstimatedHours: canEditStageHours.value
        ? stageForm.value.requireEstimatedHours
        : false,
    });
  } else if (editingStageId.value !== null) {
    const target = selectedStageTemplate.value.stages.find(
      (item) => item.stageId === editingStageId.value
    );
    if (target) {
      target.stageName = stageForm.value.stageName.trim() || target.stageName;
      target.ownerRoleLabel = stageForm.value.ownerRoleLabel.trim() || target.ownerRoleLabel;
      target.requiresWorkLog = stageForm.value.requiresWorkLog;
      target.requiresOnsiteLog = stageForm.value.requiresOnsiteLog;
      target.reminderDays = [...stageForm.value.reminderDays];
      target.reminderAnchor = stageForm.value.reminderAnchor;
      if (canEditStageHours.value) {
        target.estimatedDays = parseOptionalNumber(stageForm.value.estimatedDays);
        target.estimatedHours = parseOptionalNumber(stageForm.value.estimatedHours);
        target.onsiteHours = parseOptionalNumber(stageForm.value.onsiteHours);
        target.offsiteHours = parseOptionalNumber(stageForm.value.offsiteHours);
        target.requireEstimatedDays = stageForm.value.requireEstimatedDays;
        target.requireEstimatedHours = stageForm.value.requireEstimatedHours;
      }
      target.deadlineDays = parseOptionalNumber(stageForm.value.deadlineDays);
      target.deadlineAnchor = stageForm.value.deadlineAnchor;
    }
  }
  touchStageTemplate(selectedStageTemplate.value);
  isAddingStage.value = false;
  editingStageId.value = null;
  persistSettings();
};

const cancelStageForm = () => {
  isAddingStage.value = false;
  editingStageId.value = null;
};


const startAddOutput = (stageId: number) => {
  isAddingOutput.value = true;
  editingOutputId.value = null;
  outputForm.value = {
    stageId,
    outputName: "",
    outputKind: "document",
    outputKindLabel: "",
    required: true,
  };
};

const startEditOutput = (stageId: number, output: TemplateOutput) => {
  isAddingOutput.value = false;
  editingOutputId.value = output.outputId;
  outputForm.value = {
    stageId,
    outputName: output.outputName,
    outputKind: output.outputKind,
    outputKindLabel: output.outputKindLabel ?? "",
    required: output.required,
  };
};

const saveOutput = () => {
  if (!selectedStageTemplate.value) return;
  const stage = selectedStageTemplate.value.stages.find(
    (item) => item.stageId === outputForm.value.stageId
  );
  if (!stage) return;

  if (isAddingOutput.value) {
    const newId = Math.max(0, ...stage.requiredOutputs.map((item) => item.outputId)) + 1;
    stage.requiredOutputs.push({
      outputId: newId,
      outputName: outputForm.value.outputName.trim() || "未命名產出",
      outputKind: outputForm.value.outputKind,
      outputKindLabel:
        outputForm.value.outputKind === "other"
          ? outputForm.value.outputKindLabel.trim()
          : undefined,
      required: outputForm.value.required,
    });
  } else if (editingOutputId.value !== null) {
    const output = stage.requiredOutputs.find((item) => item.outputId === editingOutputId.value);
    if (output) {
      output.outputName = outputForm.value.outputName.trim() || output.outputName;
      output.outputKind = outputForm.value.outputKind;
      output.outputKindLabel =
        outputForm.value.outputKind === "other"
          ? outputForm.value.outputKindLabel.trim()
          : undefined;
      output.required = outputForm.value.required;
    }
  }
  touchStageTemplate(selectedStageTemplate.value);
  isAddingOutput.value = false;
  editingOutputId.value = null;
  persistSettings();
};

const handleOutputDragEnd = () => {
  touchStageTemplate(selectedStageTemplate.value);
  persistSettings();
};

const cancelOutputForm = () => {
  isAddingOutput.value = false;
  editingOutputId.value = null;
};

onMounted(() => {
  templateSettings.value = loadProjectTemplateSettings();
  selectedProjectTypeId.value = projectTypes.value[0]?.projectTypeId ?? null;
  selectedServiceItemId.value = serviceItems.value[0]?.serviceItemId ?? null;
  selectedServiceProductId.value = selectedServiceItem.value?.products[0]?.productId ?? null;
  selectedStageTemplateId.value = defaultStageTemplateId.value;
  if (selectedProjectType.value) {
    startViewProjectType(selectedProjectType.value);
  }
});

watch(selectedServiceItemId, () => {
  if (deliveryMode.value === "product") {
    selectedServiceProductId.value = selectedServiceItem.value?.products[0]?.productId ?? null;
    selectedStageTemplateId.value = selectedServiceProduct.value?.stageTemplateIds[0] ?? null;
  } else {
    selectedServiceProductId.value = null;
    selectedStageTemplateId.value = defaultStageTemplateId.value;
  }
  productSearch.value = "";
  showAllProducts.value = false;
  isEditingServiceProduct.value = false;
  isEditingStageTemplate.value = false;
  isAddingStage.value = false;
  editingStageId.value = null;
  isAddingOutput.value = false;
  editingOutputId.value = null;
});

watch(selectedServiceProductId, () => {
  if (deliveryMode.value === "product") {
    selectedStageTemplateId.value = selectedServiceProduct.value?.stageTemplateIds[0] ?? null;
  }
  isEditingStageTemplate.value = false;
  isAddingStage.value = false;
  editingStageId.value = null;
  isAddingOutput.value = false;
  editingOutputId.value = null;
});

watch(deliveryMode, () => {
  if (deliveryMode.value === "product") {
    selectedServiceProductId.value = selectedServiceItem.value?.products[0]?.productId ?? null;
    selectedStageTemplateId.value = selectedServiceProduct.value?.stageTemplateIds[0] ?? null;
  } else {
    selectedServiceProductId.value = null;
    selectedStageTemplateId.value = defaultStageTemplateId.value;
  }
  isEditingServiceProduct.value = false;
  isEditingStageTemplate.value = false;
  isAddingStage.value = false;
  editingStageId.value = null;
  isAddingOutput.value = false;
  editingOutputId.value = null;
});

watch(selectedProjectTypeId, () => {
  if (selectedProjectType.value) {
    startViewProjectType(selectedProjectType.value);
  }
});

watch(
  () => canViewProjectType.value,
  (canView) => {
    if (!canView && activeTab.value === "projectType") {
      activeTab.value = "serviceItem";
    }
  },
  { immediate: true }
);
</script>

<template>
  <div class="flex flex-col h-full p-4 gap-4">
    <div class="flex gap-4">
      <button
        v-if="canViewProjectType"
        class="tab-btn"
        :class="{ active: activeTab === 'projectType' }"
        @click="activeTab = 'projectType'"
      >
        專案類型
      </button>
      <button
        class="tab-btn"
        :class="{ active: activeTab === 'serviceItem' }"
        @click="activeTab = 'serviceItem'"
      >
        服務項目
      </button>
    </div>

    <div v-if="canViewProjectType && activeTab === 'projectType'" class="flex flex-col gap-4">
      <div class="bg-white rounded-lg p-6 shadow-sm">
        <div class="flex items-center justify-between">
          <h2 class="subtitle">專案類型</h2>
        </div>
        <p class="text-xs text-gray-500 mt-1">
          專案類型決定可選服務項目、標準階段與合規附件需求。
        </p>

        <div class="mt-4 grid grid-cols-1 gap-3 md:grid-cols-2">
          <button
            v-for="projectType in projectTypes"
            :key="projectType.projectTypeId"
            class="rounded-lg border p-3 text-left transition"
            :class="
              projectType.projectTypeId === selectedProjectTypeId
                ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                : 'border-gray-200 hover:border-cyan-400'
            "
            @click="startViewProjectType(projectType)"
          >
            <p class="font-semibold">{{ projectType.projectTypeName }}</p>
            <p class="text-xs text-gray-500 line-clamp-2">
              {{ projectType.description || '無描述' }}
            </p>
            <p class="text-xs text-gray-500 mt-1">
              類型：{{ projectType.category === 'single' ? '單一' : '混合' }}
            </p>
          </button>
        </div>
        <div class="mt-3 flex">
          <button
            class="w-full rounded-lg border border-dashed border-gray-300 p-3 text-left text-sm font-medium text-gray-500 transition hover:border-cyan-400 hover:text-cyan-600 md:w-[calc(50%-0.375rem)]"
            @click="startAddProjectType"
          >
            +新增專案類型
          </button>
        </div>

        <div v-if="selectedProjectType || isEditingProjectType" class="mt-6 border-t pt-4 space-y-3">
          <div class="flex items-start justify-between gap-4">
            <div>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="form-label">專案類型名稱</label>
                  <input
                    v-model="projectTypeForm.projectTypeName"
                    class="input-box"
                    :disabled="isProjectTypeReadonly"
                  />
                </div>
                <div>
                  <label class="form-label">描述</label>
                  <input
                    v-model="projectTypeForm.description"
                    class="input-box"
                    :disabled="isProjectTypeReadonly"
                  />
                </div>
              </div>
            </div>
            <div class="pt-6">
              <button
                v-if="!isEditingProjectType && selectedProjectType"
                class="btn-update"
                @click="startEditProjectType(selectedProjectType)"
              >
                編輯
              </button>
            </div>
          </div>
          <div>
            <label class="form-label">專案類型分類</label>
            <div class="flex gap-4">
              <label class="flex items-center gap-2 text-sm text-gray-600">
                <input
                  type="radio"
                  value="single"
                  v-model="projectTypeForm.category"
                  class="w-4 h-4"
                  :disabled="isProjectTypeReadonly"
                />
                單一專案
              </label>
              <label class="flex items-center gap-2 text-sm text-gray-600">
                <input
                  type="radio"
                  value="hybrid"
                  v-model="projectTypeForm.category"
                  class="w-4 h-4"
                  :disabled="isProjectTypeReadonly"
                />
                混合專案
              </label>
            </div>
          </div>
          <div v-if="isEditingProjectType" class="flex justify-end gap-2">
            <button class="btn-cancel" @click="cancelProjectType">取消</button>
            <button class="btn-submit" @click="saveProjectType">儲存</button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="activeTab === 'serviceItem'" class="flex flex-col gap-4">
      <div class="bg-white rounded-lg p-6 shadow-sm">
        <div class="flex items-center justify-between">
          <h2 class="subtitle">服務項目</h2>
        </div>
        <p class="text-xs text-gray-500 mt-1">
          服務項目對應部門與標準階段模板，可供混合專案複選。
        </p>

        <div class="mt-4 grid grid-cols-3 gap-2">
          <button
            v-for="serviceItem in serviceItems"
            :key="serviceItem.serviceItemId"
            class="rounded-lg border px-3 py-2 text-left transition"
            :class="
              serviceItem.serviceItemId === selectedServiceItemId
                ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                : 'border-gray-200 hover:border-cyan-400'
            "
            @click="startViewServiceItem(serviceItem)"
          >
            <p class="font-semibold">{{ serviceItem.serviceItemName }}</p>
          </button>
          <button
            class="rounded-lg border border-dashed border-gray-300 px-3 py-2 text-left text-sm font-medium text-gray-500 transition hover:border-cyan-400 hover:text-cyan-600"
            @click="startAddServiceItem"
          >
            +新增服務項目
          </button>
        </div>
        <div v-if="isEditingServiceItem" class="mt-4 rounded-lg border border-gray-200 bg-gray-50 p-4">
          <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
            <div>
              <label class="form-label">服務項目名稱</label>
              <input v-model="serviceItemForm.serviceItemName" class="input-box" />
            </div>
            <div class="md:col-span-2">
              <label class="form-label">描述</label>
              <input v-model="serviceItemForm.description" class="input-box" />
            </div>
          </div>
          <div class="flex justify-end gap-2 mt-4">
            <button class="btn-cancel" @click="cancelServiceItem">取消</button>
            <button class="btn-submit" @click="saveServiceItem">儲存</button>
          </div>
        </div>

        <div v-if="selectedServiceItem" class="mt-6 border-t pt-4">
          <div class="flex items-center justify-between">
            <h3 class="subtitle text-base">交付方式</h3>
          </div>
          <p class="text-xs text-gray-500 mt-1">請先選擇產品或 POC。</p>
          <div class="mt-3 flex gap-2">
            <button
              class="rounded-lg border px-3 py-2 text-left transition"
              :class="
                deliveryMode === 'product'
                  ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                  : 'border-gray-200 hover:border-cyan-400'
              "
              @click="deliveryMode = 'product'"
            >
              產品
            </button>
            <button
              class="rounded-lg border px-3 py-2 text-left transition"
              :class="
                deliveryMode === 'poc'
                  ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                  : 'border-gray-200 hover:border-cyan-400'
              "
              @click="deliveryMode = 'poc'"
            >
              POC
            </button>
          </div>
        </div>

        <div v-if="selectedServiceItem && deliveryMode === 'product'" class="mt-6 border-t pt-4">
          <div class="flex items-center justify-between">
            <h3 class="subtitle text-base">產品</h3>
            <div class="flex items-center gap-2">
              <button
                v-if="hasMoreProducts"
                type="button"
                class="text-xs text-cyan-700 hover:text-cyan-800"
                @click="showAllProducts = true"
              >
                顯示更多
              </button>
              <button
                v-else-if="showAllProducts && !productSearch"
                type="button"
                class="text-xs text-gray-500 hover:text-gray-700"
                @click="showAllProducts = false"
              >
                收合
              </button>
            </div>
          </div>
          <p class="text-xs text-gray-500 mt-1">
            選擇服務項目後可維護對應產品，產品會套用自己的標準階段與產出。
          </p>
          <div class="mt-3">
            <input
              v-model="productSearch"
              type="text"
              class="input-box"
              placeholder="搜尋產品（可輸入中文或英文）"
            />
          </div>
          <div class="mt-3 grid grid-cols-3 gap-2">
            <div
              v-for="product in visibleServiceProducts"
              :key="product.productId"
              class="relative group"
            >
              <button
                class="w-full rounded-lg border px-3 py-2 text-left transition"
                :class="
                  product.productId === selectedServiceProductId
                    ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                    : 'border-gray-200 hover:border-cyan-400'
                "
                @click="startViewServiceProduct(product)"
              >
                <p class="font-semibold">{{ getProductTitle(product) }}</p>
                <p class="text-xs text-gray-500 line-clamp-1">
                  {{ getProductSubTitle(product) }}
                </p>
              </button>
              <div
                class="absolute right-2 top-2 hidden items-center gap-1 rounded-full border border-gray-200 bg-white px-2 py-1 text-xs text-gray-600 shadow-sm group-hover:flex"
              >
                <button
                  type="button"
                  class="flex items-center gap-1 text-cyan-700 hover:text-cyan-800"
                  @click.stop="startEditServiceProduct(product)"
                  title="編輯"
                >
                  <svg viewBox="0 0 20 20" class="h-4 w-4 fill-current">
                    <path
                      d="M4 13.5V16h2.5l7.37-7.37-2.5-2.5L4 13.5zm11.85-7.35a.5.5 0 000-.71l-1.29-1.29a.5.5 0 00-.71 0l-1.04 1.04 2.5 2.5 1.04-1.04z"
                    />
                  </svg>
                  編輯
                </button>
                <span class="text-gray-300">|</span>
                <button
                  type="button"
                  class="flex items-center gap-1 text-rose-600 hover:text-rose-700"
                  @click.stop="removeServiceProduct(product)"
                  title="刪除"
                >
                  <svg viewBox="0 0 20 20" class="h-4 w-4 fill-current">
                    <path
                      d="M6 7h1v8H6V7zm4 0h1v8h-1V7zm-5-3h10l1 2H4l1-2zm1 2h8l-.7 11.2a1 1 0 01-1 .8H6.7a1 1 0 01-1-.8L5 6z"
                    />
                  </svg>
                  刪除
                </button>
              </div>
            </div>
            <button
              class="rounded-lg border border-dashed border-gray-300 px-3 py-2 text-left text-sm font-medium text-gray-500 transition hover:border-cyan-400 hover:text-cyan-600"
              @click="startAddServiceProduct"
            >
              +新增產品
            </button>
          </div>

          <div v-if="isEditingServiceProduct" class="mt-4 grid grid-cols-2 gap-4">
            <div>
              <label class="form-label">產品名稱</label>
              <input v-model="serviceProductForm.productName" class="input-box" />
            </div>
            <div>
              <label class="form-label">描述</label>
              <input v-model="serviceProductForm.description" class="input-box" />
            </div>
            <div class="col-span-2">
              <label class="form-label">套用標準階段模板</label>
              <select v-model="serviceProductForm.copyStageTemplateId" class="select-box">
                <option :value="null">不套用</option>
                <option
                  v-for="template in stageTemplates"
                  :key="template.stageTemplateId"
                  :value="template.stageTemplateId"
                >
                  {{ template.stageTemplateName }}
                </option>
              </select>
              <p class="text-xs text-gray-500 mt-1">
                可從既有模板快速複製標準階段與產出。
              </p>
            </div>
            <div class="col-span-2 flex justify-center gap-2">
              <button class="btn-cancel" @click="cancelServiceProduct">取消</button>
              <button class="btn-submit" @click="saveServiceProduct">儲存</button>
            </div>
          </div>
        </div>

        <div v-if="selectedServiceItem && deliveryMode === 'poc'" class="mt-6 border-t pt-4">
          <div class="flex items-center justify-between">
            <h3 class="subtitle text-base">POC</h3>
          </div>
          <p class="text-xs text-gray-500 mt-1">
            POC 使用相同標準階段與產出模板。
          </p>
          <div class="mt-3 rounded-lg border border-cyan-500 bg-cyan-50 p-3 text-cyan-700">
            <p class="font-semibold">POC</p>
            <p class="text-xs text-gray-500 mt-1">專案評估與驗證</p>
          </div>
        </div>

      </div>
        <div v-if="selectedServiceItem || isEditingServiceItem" class="mt-6 bg-white rounded-lg p-6 shadow-sm">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="subtitle">
                {{ selectedServiceItem?.serviceItemName || serviceItemForm.serviceItemName || "新增服務項目" }}
                <span v-if="deliveryMode === 'product' && selectedServiceProduct">
                  / {{ selectedServiceProduct.productName }}
                </span>
                <span v-else-if="deliveryMode === 'poc'"> / POC</span>
                標準階段設置
              </h2>
              <p v-if="selectedStageTemplate" class="text-xs text-gray-500 mt-1">{{ lastEditedLabel }}</p>
              <p v-else class="text-xs text-gray-500 mt-1">請先建立產品或標準階段模板。</p>
            </div>
          </div>
          <div v-if="isAddingStage" class="mt-4 rounded-lg border border-dashed border-gray-200 bg-[#F9FAFB] p-4 space-y-3">
            <div class="grid grid-cols-1 gap-4 md:grid-cols-[minmax(0,2fr)_auto_auto] md:items-end">
              <div>
                <label class="form-label">階段名稱</label>
                <input v-model="stageForm.stageName" class="input-box" />
              </div>
              <div class="flex items-center gap-2 text-sm text-gray-600">
                <input v-model="stageForm.requiresWorkLog" type="checkbox" :class="checkboxClass" />
                需工作日誌
              </div>
              <div class="flex items-center gap-2 text-sm text-gray-600">
                <input v-model="stageForm.requiresOnsiteLog" type="checkbox" :class="checkboxClass" />
                需駐點打卡
              </div>
            </div>
            <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
              <div>
                <label class="form-label">提醒時間</label>
                <div class="flex flex-wrap gap-3">
                  <label class="flex items-center gap-2 text-sm text-gray-600">
                    <input v-model="stageForm.reminderDays" type="checkbox" :class="checkboxClass" :value="3" />
                    3 天
                  </label>
                  <label class="flex items-center gap-2 text-sm text-gray-600">
                    <input v-model="stageForm.reminderDays" type="checkbox" :class="checkboxClass" :value="5" />
                    5 天
                  </label>
                  <label class="flex items-center gap-2 text-sm text-gray-600">
                    <input v-model="stageForm.reminderDays" type="checkbox" :class="checkboxClass" :value="7" />
                    7 天
                  </label>
                </div>
              </div>
              <div>
                <label class="form-label">提醒基準</label>
                <div class="flex gap-4 text-sm text-gray-600">
                  <label class="flex items-center gap-2">
                    <input v-model="stageForm.reminderAnchor" type="radio" value="start" class="h-5 w-5" />
                    專案起日
                  </label>
                  <label class="flex items-center gap-2">
                    <input v-model="stageForm.reminderAnchor" type="radio" value="end" class="h-5 w-5" />
                    專案迄日
                  </label>
                </div>
              </div>
              <div>
                <label class="form-label">預計人天</label>
                <input
                  v-model="stageForm.estimatedDays"
                  type="number"
                  min="0"
                  class="input-box"
                  :disabled="!canEditStageHours"
                />
                <label class="mt-2 flex items-center gap-2 text-sm text-gray-600">
                  <input
                    v-model="stageForm.requireEstimatedDays"
                    type="checkbox"
                    :class="checkboxClass"
                    :disabled="!canEditStageHours"
                  />
                  必填
                </label>
                <p v-if="!canEditStageHours" class="text-xs text-gray-500 mt-1">
                  時數設定僅限部門經理
                </p>
              </div>
              <div>
                <label class="form-label">預估時間</label>
                <input
                  v-model="stageForm.estimatedHours"
                  type="number"
                  min="0"
                  class="input-box"
                  :disabled="!canEditStageHours"
                />
                <label class="mt-2 flex items-center gap-2 text-sm text-gray-600">
                  <input
                    v-model="stageForm.requireEstimatedHours"
                    type="checkbox"
                    :class="checkboxClass"
                    :disabled="!canEditStageHours"
                  />
                  必填
                </label>
              </div>
              <div>
                <label class="form-label">OnSite 時數</label>
                <input
                  v-model="stageForm.onsiteHours"
                  type="number"
                  min="0"
                  class="input-box"
                  :disabled="!canEditStageHours"
                />
              </div>
              <div>
                <label class="form-label">OffSite 時數</label>
                <input
                  v-model="stageForm.offsiteHours"
                  type="number"
                  min="0"
                  class="input-box"
                  :disabled="!canEditStageHours"
                />
              </div>
              <div>
                <label class="form-label">期限設定（天）</label>
                <div class="flex items-center gap-3">
                  <input
                    v-model="stageForm.deadlineDays"
                    type="number"
                    min="0"
                    class="input-box w-24"
                    placeholder="天數"
                  />
                  <select v-model="stageForm.deadlineAnchor" class="select-box">
                    <option value="start">專案起日</option>
                    <option value="end">專案迄日</option>
                  </select>
                </div>
              </div>
            </div>
            <div class="flex justify-end gap-2">
              <button class="btn-cancel" @click="cancelStageForm">取消</button>
              <button class="btn-submit" @click="saveStage">儲存</button>
            </div>
          </div>
          <button
            v-if="selectedServiceProduct"
            class="mt-4 w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
            style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
            @click="handleAddStandardStage"
          >
            +新增標準階段
          </button>

          <div v-if="isEditingStageTemplate" class="mt-4 grid grid-cols-2 gap-4">
            <div>
              <label class="form-label">模板名稱</label>
              <input v-model="stageTemplateForm.stageTemplateName" class="input-box" />
            </div>
            <div>
              <label class="form-label">描述</label>
              <input v-model="stageTemplateForm.description" class="input-box" />
            </div>
            <div class="col-span-2 flex justify-end gap-2">
              <button class="btn-cancel" @click="cancelStageTemplate">取消</button>
              <button class="btn-submit" @click="saveStageTemplate">儲存</button>
            </div>
          </div>

          <div v-if="shouldShowStageSection" class="mt-6 space-y-4">
            <div class="flex items-center justify-between">
              <h3 class="subtitle">階段與產出</h3>
            </div>

            <div
              v-for="(stage, stageIndex) in selectedStageTemplate.stages"
              :key="stage.stageId"
              class="rounded-lg border border-gray-200 bg-gray-50 p-4"
            >
              <div class="flex items-center justify-between gap-4">
                <div class="w-full">
                  <div v-if="editingStageId === stage.stageId">
                    <label class="form-label">階段名稱</label>
                    <input v-model="stageForm.stageName" class="input-box" />
                  </div>
                  <div v-else>
                    <p class="font-semibold text-gray-700">{{ stage.stageName }}</p>
                  </div>
                </div>
                <div class="flex items-center gap-2">
                  <button
                    v-if="editingStageId === stage.stageId"
                    class="btn-submit"
                    @click="saveStage"
                  >
                    儲存
                  </button>
                  <button
                    v-if="editingStageId === stage.stageId"
                    class="btn-cancel"
                    @click="cancelStageForm"
                  >
                    取消
                  </button>
                  <button
                    v-else
                    class="btn-update"
                    @click="startEditStage(stage)"
                  >
                    編輯
                  </button>
                </div>
              </div>
              <div v-if="editingStageId === stage.stageId" class="mt-3 grid grid-cols-1 gap-4 md:grid-cols-3">
                <div class="flex items-center gap-2 text-sm text-gray-600">
                  <input v-model="stageForm.requiresWorkLog" type="checkbox" :class="checkboxClass" />
                  需工作日誌
                </div>
                <div class="flex items-center gap-2 text-sm text-gray-600">
                  <input v-model="stageForm.requiresOnsiteLog" type="checkbox" :class="checkboxClass" />
                  需駐點打卡
                </div>
              </div>
              <div v-if="editingStageId === stage.stageId" class="mt-4 grid grid-cols-1 gap-4 md:grid-cols-2">
                <div>
                  <label class="form-label">提醒時間</label>
                  <div class="flex flex-wrap gap-3">
                    <label class="flex items-center gap-2 text-sm text-gray-600">
                      <input v-model="stageForm.reminderDays" type="checkbox" :class="checkboxClass" :value="3" />
                      3 天
                    </label>
                    <label class="flex items-center gap-2 text-sm text-gray-600">
                      <input v-model="stageForm.reminderDays" type="checkbox" :class="checkboxClass" :value="5" />
                      5 天
                    </label>
                    <label class="flex items-center gap-2 text-sm text-gray-600">
                      <input v-model="stageForm.reminderDays" type="checkbox" :class="checkboxClass" :value="7" />
                      7 天
                    </label>
                  </div>
                </div>
                <div>
                  <label class="form-label">提醒基準</label>
                  <div class="flex gap-4 text-sm text-gray-600">
                    <label class="flex items-center gap-2">
                      <input v-model="stageForm.reminderAnchor" type="radio" value="start" class="h-5 w-5" />
                      專案起日
                    </label>
                    <label class="flex items-center gap-2">
                      <input v-model="stageForm.reminderAnchor" type="radio" value="end" class="h-5 w-5" />
                      專案迄日
                    </label>
                  </div>
                </div>
                <div>
                  <label class="form-label">預計人天</label>
                  <div class="flex flex-wrap items-center gap-3">
                    <input
                      v-model="stageForm.estimatedDays"
                      type="number"
                      min="0"
                      max="999"
                      class="input-box w-24"
                      :disabled="!canEditStageHours"
                    />
                    <label class="flex items-center gap-2 text-sm text-gray-600">
                      <input
                        v-model="stageForm.requireEstimatedDays"
                        type="checkbox"
                        :class="checkboxClass"
                        :disabled="!canEditStageHours"
                      />
                      必填
                    </label>
                  </div>
                </div>
                <div>
                  <label class="form-label">預估時間</label>
                  <div class="flex flex-wrap items-center gap-3">
                    <input
                      v-model="stageForm.estimatedHours"
                      type="number"
                      min="0"
                      max="999"
                      class="input-box w-24"
                      :disabled="!canEditStageHours"
                    />
                    <label class="flex items-center gap-2 text-sm text-gray-600">
                      <input
                        v-model="stageForm.requireEstimatedHours"
                        type="checkbox"
                        :class="checkboxClass"
                        :disabled="!canEditStageHours"
                      />
                      必填
                    </label>
                  </div>
                </div>
                <div>
                  <label class="form-label">OnSite 時數</label>
                  <div class="flex flex-wrap items-center gap-3">
                    <input
                      v-model="stageForm.onsiteHours"
                      type="number"
                      min="0"
                      max="999"
                      class="input-box w-24"
                      :disabled="!canEditStageHours"
                    />
                  </div>
                </div>
                <div>
                  <label class="form-label">OffSite 時數</label>
                  <div class="flex flex-wrap items-center gap-3">
                    <input
                      v-model="stageForm.offsiteHours"
                      type="number"
                      min="0"
                      max="999"
                      class="input-box w-24"
                      :disabled="!canEditStageHours"
                    />
                  </div>
                </div>
                <div>
                  <label class="form-label">期限設定（天）</label>
                  <div class="flex flex-wrap items-center gap-3">
                    <input
                      v-model="stageForm.deadlineDays"
                      type="number"
                      min="0"
                      max="999"
                      class="input-box w-24"
                      placeholder="天數"
                    />
                    <select v-model="stageForm.deadlineAnchor" class="select-box">
                      <option value="start">專案起日</option>
                      <option value="end">專案迄日</option>
                    </select>
                  </div>
                </div>
              </div>
              <div class="flex flex-wrap gap-2 mt-2">
                <span
                  class="rounded-full bg-cyan-50 px-2 py-0.5 text-xs text-cyan-700"
                  v-if="stage.requiresWorkLog"
                >
                  需工作日誌
                </span>
                <span
                  class="rounded-full bg-amber-50 px-2 py-0.5 text-xs text-amber-700"
                  v-if="stage.requiresOnsiteLog"
                >
                  需駐點打卡
                </span>
                <span
                  v-if="stage.reminderDays?.length"
                  class="rounded-full bg-sky-50 px-2 py-0.5 text-xs text-sky-700"
                >
                  提醒：{{ stage.reminderDays.join(" / ") }} 天（{{ stage.reminderAnchor === "end" ? "迄日" : "起日" }}）
                </span>
                <span
                  v-if="Number.isFinite(stage.estimatedDays)"
                  class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                >
                  人天：{{ stage.estimatedDays }}{{ stage.requireEstimatedDays ? "（必填）" : "" }}
                </span>
                <span
                  v-if="Number.isFinite(stage.estimatedHours)"
                  class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                >
                  預估時間：{{ stage.estimatedHours }}{{ stage.requireEstimatedHours ? "（必填）" : "" }}
                </span>
                <span
                  v-if="Number.isFinite(stage.onsiteHours)"
                  class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                >
                  OnSite：{{ stage.onsiteHours }}
                </span>
                <span
                  v-if="Number.isFinite(stage.offsiteHours)"
                  class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                >
                  OffSite：{{ stage.offsiteHours }}
                </span>
                <span
                  v-if="Number.isFinite(stage.deadlineDays)"
                  class="rounded-full bg-slate-50 px-2 py-0.5 text-xs text-slate-700"
                >
                  期限：{{ stage.deadlineAnchor === "end" ? "迄日" : "起日" }} +{{ stage.deadlineDays }} 天
                </span>
              </div>

              <div class="mt-3">
                <div class="flex items-center justify-between">
                  <p class="text-sm font-semibold text-gray-600">產出清單</p>
                </div>
                <div v-if="isAddingOutput" class="mt-3 space-y-2">
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <label class="form-label">產出名稱</label>
                      <input v-model="outputForm.outputName" class="input-box" />
                    </div>
                    <div>
                      <label class="form-label">產出類型</label>
                      <select v-model="outputForm.outputKind" class="select-box">
                        <option value="document">文件</option>
                        <option value="meeting">會議</option>
                        <option value="report">報告</option>
                        <option value="other">其他</option>
                      </select>
                    </div>
                  </div>
                  <div class="flex items-center gap-2">
                    <input v-model="outputForm.required" type="checkbox" :class="checkboxClass" />
                    <span class="text-sm text-gray-600">必填</span>
                  </div>
                  <div class="flex justify-end gap-2">
                    <button class="btn-cancel" @click="cancelOutputForm">取消</button>
                    <button class="btn-submit" @click="saveOutput">儲存</button>
                  </div>
                </div>
                <button
                  v-if="!isAddingOutput"
                  class="mt-2 w-full rounded-lg border border-dashed border-gray-300 py-2 text-sm font-medium text-gray-600 hover:border-cyan-400 hover:text-cyan-600"
                  @click="startAddOutput(stage.stageId)"
                >
                  +新增產出
                </button>
                <draggable
                  class="mt-2 space-y-2"
                  tag="div"
                  :list="stage.requiredOutputs"
                  item-key="outputId"
                  handle=".drag-handle"
                  @end="handleOutputDragEnd"
                >
                  <template #item="{ element: output }">
                    <div class="flex flex-col gap-3 rounded-md border border-gray-200 bg-white px-4 py-3 md:flex-row md:items-center md:justify-between">
                      <div class="flex flex-1 items-start gap-3">
                        <span
                          class="drag-handle mt-1 inline-flex h-5 w-5 items-center justify-center text-gray-400 cursor-grab"
                          title="拖曳排序"
                        >
                          <svg viewBox="0 0 20 20" class="h-4 w-4 fill-current">
                            <path
                              d="M6 4a1 1 0 110 2 1 1 0 010-2zm0 5a1 1 0 110 2 1 1 0 010-2zm0 5a1 1 0 110 2 1 1 0 010-2zm8-10a1 1 0 110 2 1 1 0 010-2zm0 5a1 1 0 110 2 1 1 0 010-2zm0 5a1 1 0 110 2 1 1 0 010-2z"
                            />
                          </svg>
                        </span>
                        <div class="w-full">
                          <div
                            v-if="editingOutputId === output.outputId"
                            class="grid grid-cols-1 gap-4 md:grid-cols-3 md:items-end"
                          >
                            <div>
                              <label class="form-label">產出名稱</label>
                              <input v-model="outputForm.outputName" class="input-box" />
                            </div>
                            <div>
                              <label class="form-label">產出類型</label>
                              <select v-model="outputForm.outputKind" class="select-box">
                                <option value="document">文件</option>
                                <option value="meeting">會議</option>
                                <option value="report">報告</option>
                                <option value="other">其他</option>
                              </select>
                            </div>
                            <div v-if="outputForm.outputKind === 'other'">
                              <label class="form-label">其他類型</label>
                              <input v-model="outputForm.outputKindLabel" class="input-box" />
                            </div>
                            <div class="flex items-center gap-3">
                              <input v-model="outputForm.required" type="checkbox" :class="checkboxClass" />
                              <span class="text-sm font-medium text-gray-600">必填</span>
                            </div>
                          </div>
                          <div v-else>
                            <p class="text-sm text-gray-700">{{ output.outputName }}</p>
                            <p class="text-xs text-gray-500">
                              類型：{{
                                output.outputKind === "other" && output.outputKindLabel
                                  ? output.outputKindLabel
                                  : output.outputKind
                              }}｜{{ output.required ? "必填" : "選填" }}
                            </p>
                          </div>
                        </div>
                      </div>
                      <div class="flex items-center gap-3 md:shrink-0">
                        <button
                          v-if="editingOutputId === output.outputId"
                          class="btn-submit"
                          @click="saveOutput"
                        >
                          儲存
                        </button>
                        <button
                          v-if="editingOutputId === output.outputId"
                          class="btn-cancel"
                          @click="cancelOutputForm"
                        >
                          取消
                        </button>
                        <button
                          v-else
                          class="btn-update"
                          @click="startEditOutput(stage.stageId, output)"
                        >
                          編輯
                        </button>
                      </div>
                    </div>
                  </template>
                </draggable>

              </div>
            </div>

            <button
              v-if="selectedStageTemplate && selectedStageTemplate.stages.length > 0"
              class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="background-color:#F2F6F9;border-color:#082F49;"
              @click="startAddStage"
            >
              +新增階段與產出
            </button>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="showCopyConfirm"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4"
    >
      <div class="w-full max-w-md rounded-lg bg-white p-6 shadow-lg">
        <h3 class="subtitle">套用標準階段模板</h3>
        <p class="mt-2 text-sm text-gray-600">
          是否套用所選模板的標準階段與產出？
        </p>
        <div class="mt-5 flex justify-end gap-2">
          <button class="btn-cancel" @click="cancelCopyTemplate">取消</button>
          <button class="btn-submit" @click="confirmCopyTemplate">確定套用</button>
        </div>
      </div>
    </div>
</template>
