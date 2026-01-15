<script setup lang="ts">
//#region 引入
import { ref, reactive, defineAsyncComponent, computed, onMounted, watch } from "vue";
import VueDatePicker from "@vuepic/vue-datepicker";
import { useRouter } from "vue-router";
// Enums / 常數
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
import { orgDepartmentDirectory } from "@/constants/orgDepartmentDirectory";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
// Stores
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
// Components
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyEmployeeComboBox from "@/components/feature/search-bar/GetManyEmployeeComboBox.vue";
import GetManyManagerDepartmentComboBox from "@/components/feature/search-bar/GetManyManagerDepartmentComboBox.vue";
import GetManyManagerRegionComboBox from "@/components/feature/search-bar/GetManyManagerRegionComboBox.vue";
const GetManyManagerCompanyComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue")
);
const ConfirmDialog = defineAsyncComponent(
  () => import("@/components/global/feedback/ConfirmDialog.vue")
);
// Services
import { MbsWrkPrjHttpAddProjectReqMdl } from "@/services/pms-http/work/project/workProjectHttpFormat";
import {
  addProject,
  getManyCompanyLocation,
  getAllBasicManagerDepartment,
  getAllBasicManagerRegion,
  getManyProject,
} from "@/services";
import { getEmployeeProjectMemberRoleLabel } from "@/utils/getEmployeeProjectMemberRoleLabel";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";
import { formatToServerDateStartISO8, formatToServerDateEndISO8 } from "@/utils/timeFormatter";
import { loadProjectTemplateSettings } from "@/stores/projectTemplateSettings";

//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作 */
const router = useRouter();
//#endregion

const props = defineProps<{ isModal?: boolean }>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "created", payload: {
    employeeProjectID: number | null;
    atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
    employeeProjectName: string;
    managerCompanyName: string;
    employeeProjectStartTime: string;
    employeeProjectEndTime: string;
  }): void;
}>();

//#region 型別定義
/** 工作-專案管理-新增-頁面模型 */
interface WrkProjectDetailViewMdl {
  employeeProjectCode: string;
  employeeProjectName: string;
  managerCompanyID: number;
  primaryDepartmentID: number;
  primaryDepartmentName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum | null;
  employeeProjectContractUrl: string | null;
  employeeProjectWorkUrl: string | null;
  employeeProjectMemberList: WrkProjectEmployeeMemberAddItemMdl[];
}

/** 工作-專案管理-新增-驗證模型 */
interface WrkProjectDetailValidateMdl {
  employeeProjectCode: boolean;
  employeeProjectName: boolean;
  primaryDepartmentID: boolean;
  projectRegionCode: boolean;
  managerCompanyID: boolean;
  employeeProjectStartTime: boolean;
  employeeProjectEndTime: boolean;
  employeeProjectEndAfterStart: boolean;
  atomEmployeeProjectStatus: boolean;
  projectTypeId: boolean;
  serviceItems: boolean;
  serviceProducts: boolean;
  orderFiles: boolean;
  slaFiles: boolean;
  employeeProjectContractUrl: boolean;
  employeeProjectWorkUrl: boolean;
}

/** 工作-專案管理-人員-新增-項目模型 */
interface WrkProjectEmployeeMemberAddItemMdl {
  employeeProjectMemberID: number;
  employeeProjectMemberName: string;
  employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum | null;
  projectRoleName: string;
  serviceItemId?: number | null;
  serviceItemName?: string;
  managerDepartmentID: number;
  managerRoleID: number;
  managerRegionID: number;
  managerRegionName: string;
  managerDepartmentName: string;
  employeeName: string;
}

/** 工作-專案管理-人員-新增-項目驗證模型 */
interface WrkProjectEmployeeMemberAddItemValidateMdl {
  employeeProjectMemberID: boolean;
  employeeProjectMemberRole: boolean;
  managerDepartmentID: boolean;
  managerRoleID: boolean;
  managerRegionID: boolean;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.WorkProject;
/** 暫存欲編輯的人員索引 */
const employeeProjectMemberListIndex = ref<number | null>(null);
/** 確認儲存彈跳視窗 */
const showConfirmDialog = ref(false);
/** 暫存欲刪除的人員 ID */
const pendingRemoveEmployeeProjectMemberID = ref<number | null>(null);
/** 新增人員彈跳視窗 */
const isShowAddMemberModal = ref(false);
/** 暫存開發評等清單（預設加入總經理） */
const employeeProjectMemberList = ref<WrkProjectEmployeeMemberAddItemMdl[]>([]);

/** 工作-專案管理-新增-頁面物件 */
const wrkProjectAddViewObj = reactive<WrkProjectDetailViewMdl>({
  employeeProjectCode: "",
  employeeProjectName: "",
  managerCompanyID: 0,
  primaryDepartmentID: 0,
  primaryDepartmentName: "",
  employeeProjectStartTime: "",
  employeeProjectEndTime: "",
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum.NotAssigned,
  employeeProjectContractUrl: "",
  employeeProjectWorkUrl: "",
  employeeProjectMemberList: [],
});

/** 工作-專案管理-新增-驗證物件 */
const wrkProjectAddValidateObj = reactive<WrkProjectDetailValidateMdl>({
  employeeProjectCode: true,
  employeeProjectName: true,
  primaryDepartmentID: true,
  projectRegionCode: true,
  managerCompanyID: true,
  employeeProjectStartTime: true,
  employeeProjectEndTime: true,
  employeeProjectEndAfterStart: true,
  atomEmployeeProjectStatus: true,
  projectTypeId: true,
  serviceItems: true,
  serviceProducts: true,
  orderFiles: true,
  slaFiles: true,
  employeeProjectContractUrl: true,
  employeeProjectWorkUrl: true,
});

/** 工作-專案管理-人員-新增-項目模型 */
const wrkProjectEmployeeMemberAddItemObj = reactive<WrkProjectEmployeeMemberAddItemMdl>({
  employeeProjectMemberID: 0,
  employeeProjectMemberName: "",
  employeeProjectMemberRole: null,
  projectRoleName: "",
  serviceItemId: null,
  serviceItemName: "",
  managerDepartmentID: 0,
  managerRoleID: 0,
  managerRegionID: 0,
  managerRegionName: "",
  managerDepartmentName: "",
  employeeName: "",
});

/** 工作-專案管理-人員-新增-項目驗證模型 */
const wrkProjectEmployeeMemberAddItemValidateObj =
  reactive<WrkProjectEmployeeMemberAddItemValidateMdl>({
    employeeProjectMemberID: true,
    employeeProjectMemberRole: true,
    managerDepartmentID: true,
    managerRoleID: true,
    managerRegionID: true,
  });

const templateSettings = ref(loadProjectTemplateSettings());
const projectTypeTemplates = computed(() => templateSettings.value.projectTypes);
const serviceItemTemplates = computed(() => templateSettings.value.serviceItems);
const selectedProjectTypeId = ref<number | null>(null);
const selectedServiceItemIds = ref<number[]>([]);
const selectedServiceProductIds = ref<Record<number, number[]>>({});
const managerCompanyName = ref<string | null>(null);
const managerCompanyAtomCity = ref<DbAtomCityEnum | null>(null);
const currentStep = ref(1);
const selectedProjectRegionCode = ref<"A" | "B" | "C" | null>(null);
const managerDepartmentOptions = ref<{ id: number; name: string }[]>([]);
const managerRegionOptions = ref<{ id: number; name: string }[]>([]);
const primaryDepartmentTouched = ref(false);
const createdProjectId = ref<number | null>(null);
const createdProjectSummary = ref<{
  regionLabel: string;
  projectCode: string;
  projectName: string;
  companyName: string;
  startTime: string;
  endTime: string;
  projectTypeName: string;
  serviceItems: string[];
  products: string[];
  members: { roleLabel: string; regionName: string; departmentName: string; employeeName: string }[];
} | null>(null);
const selectedProjectType = computed(
  () =>
    projectTypeTemplates.value.find(
      (item) => item.projectTypeId === selectedProjectTypeId.value
    ) ?? null
);
const availableServiceItems = computed(() =>
  selectedProjectType.value
    ? selectedProjectType.value.category === "single"
      ? serviceItemTemplates.value
      : serviceItemTemplates.value.filter((item) =>
          selectedProjectType.value?.serviceItemIds.includes(item.serviceItemId)
        )
    : []
);
const selectedServiceItems = computed(() =>
  availableServiceItems.value.filter((item) =>
    selectedServiceItemIds.value.includes(item.serviceItemId)
  )
);
const productSearch = ref("");
const normalizedProductSearch = computed(() => productSearch.value.trim().toLowerCase());
const filteredServiceProducts = computed(() =>
  selectedServiceItems.value.map((service) => {
    const query = normalizedProductSearch.value;
    const products = query
      ? service.products.filter((product) => {
          const name = product.productName?.toLowerCase() ?? "";
          const desc = product.description?.toLowerCase() ?? "";
          return name.includes(query) || desc.includes(query);
        })
      : service.products;
    return { service, products };
  })
);
const primaryDepartmentOptions = computed(() => {
  const regionName =
    getRegionNameByCode(selectedProjectRegionCode.value) ||
    employeeInfoStore.managerRegionName ||
    "";
  const allowlist = new Set<string>();
  selectedServiceItems.value.forEach((service) => {
    const serviceAllowlist = resolveServiceDepartmentAllowlist(
      regionName,
      service.serviceItemName
    );
    serviceAllowlist?.forEach((name) => allowlist.add(name));
  });
  const baseList = managerDepartmentOptions.value.filter((dept) =>
    orgDepartmentAllowlist.has(dept.name)
  );
  if (allowlist.size === 0) return baseList;
  return baseList.filter((dept) => allowlist.has(dept.name));
});
const projectDateRange = ref<[Date, Date] | null>(null);
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
const selectedProductNames = computed(() =>
  selectedServiceProducts.value.flatMap((item) => item.products.map((product) => product.productName))
);
const isRenewal = ref(false);
const isOutsourced = ref(false);
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
const activeStageServiceId = ref<number | null>(null);
const activeStageGroup = computed(() => {
  if (stageGroups.value.length === 0) return null;
  const activeId = activeStageServiceId.value ?? stageGroups.value[0].service.serviceItemId;
  return (
    stageGroups.value.find((group) => group.service.serviceItemId === activeId) ??
    stageGroups.value[0]
  );
});
const hasStageTemplates = computed(() =>
  stageGroups.value.some((group) =>
    group.products.some((productGroup) => productGroup.templates.length > 0)
  )
);
const fixedMembers = computed(() =>
  employeeProjectMemberList.value.filter(
    (item) => item.employeeProjectMemberRole !== DbEmployeeProjectMemberRoleEnum.Executor
  )
);
const serviceMemberGroups = computed(() =>
  selectedServiceItems.value.map((service) => ({
    service,
    members: employeeProjectMemberList.value.filter(
      (item) =>
        item.employeeProjectMemberRole === DbEmployeeProjectMemberRoleEnum.Executor &&
        item.serviceItemId === service.serviceItemId
    ),
  }))
);

const projectRegionOptions = [
  { code: "A", label: "北區", regionName: "北區" },
  { code: "B", label: "中區", regionName: "中區" },
  { code: "C", label: "南區", regionName: "南區" },
] as const;

const memberRegionOptions = [
  ...projectRegionOptions,
  { code: "X", label: "跨區", regionName: "跨區" },
] as const;

const orgDepartmentAllowlist = new Set(orgDepartmentDirectory);

const departmentRegionMap = orgMemberDirectory.reduce((map, member) => {
  const regions = map.get(member.departmentName) ?? new Set<string>();
  regions.add(member.regionName);
  map.set(member.departmentName, regions);
  return map;
}, new Map<string, Set<string>>());

const resolveServiceDepartmentAllowlist = (regionName: string, serviceName?: string) => {
  if (!serviceName) return null;
  const allowlist = new Set<string>();
  const normalizedRegion = regionName || getRegionNameByCode(selectedProjectRegionCode.value) || "";
  const addAll = (names: string[]) => names.forEach((name) => allowlist.add(name));
  switch (serviceName) {
    case "雲安維運":
      addAll(["監控部", "鑑識部"]);
      break;
    case "顧問諮詢":
    case "顧問服務":
    case "資安學苑":
      if (!normalizedRegion) {
        addAll(["北區顧問部", "中區顧問部", "南區顧問部"]);
      } else if (normalizedRegion === "北區") {
        addAll(["北區顧問部"]);
      } else if (normalizedRegion === "中區") {
        addAll(["中區顧問部"]);
      } else if (normalizedRegion === "南區") {
        addAll(["南區顧問部"]);
      }
      break;
    case "風險檢測":
      addAll(["風險稽核處"]);
      if (!normalizedRegion) {
        addAll(["北區技服部", "中區技服部", "南區技服部"]);
      } else if (normalizedRegion === "北區") {
        addAll(["北區技服部"]);
      } else if (normalizedRegion === "中區") {
        addAll(["中區技服部"]);
      } else if (normalizedRegion === "南區") {
        addAll(["南區技服部"]);
      }
      break;
    case "聯防設備":
      addAll(["工程部"]);
      break;
    case "內部軟體開發":
    case "外部軟體開發":
      addAll(["研發部", "AI 智能部", "系統開發部"]);
      break;
    default:
      break;
  }
  return allowlist.size > 0 ? allowlist : null;
};

const filterDepartmentsByRegion = (regionName: string, serviceName?: string) => {
  const normalizedRegion = regionName || "";
  const serviceAllowlist = resolveServiceDepartmentAllowlist(normalizedRegion, serviceName);
  return managerDepartmentOptions.value.filter((dept) => {
    if (!orgDepartmentAllowlist.has(dept.name)) return false;
    if (serviceAllowlist && !serviceAllowlist.has(dept.name)) return false;
    const regions = departmentRegionMap.get(dept.name);
    if (!normalizedRegion || normalizedRegion === "跨區") return true;
    if (!regions || regions.size === 0) return true;
    return regions.has(normalizedRegion);
  });
};

const getRegionNameByCode = (code: "A" | "B" | "C" | null): string => {
  if (!code) return "";
  return projectRegionOptions.find((item) => item.code === code)?.regionName ?? "";
};

const normalizeDepartmentName = (name: string | null | undefined) => {
  if (!name) return "";
  return name.replace(/^#+/, "").trim();
};

const getMemberDirectoryMatch = (name: string) => {
  if (!name) return null;
  return orgMemberDirectory.find((member) => member.name === name) ?? null;
};

const ensurePrimaryMember = () => {
  const matchedMember = getMemberDirectoryMatch(employeeInfoStore.employeeName);
  const regionName =
    matchedMember?.regionName ||
    getRegionNameByCode(selectedProjectRegionCode.value) ||
    employeeInfoStore.managerRegionName ||
    "北區";
  const regionId =
    (resolveRegionIdByName(regionName) ?? employeeInfoStore.managerRegionID) || 0;
  const fallbackDepartmentName = normalizeDepartmentName(
    wrkProjectAddViewObj.primaryDepartmentName ||
      employeeInfoStore.managerDepartmentName ||
      "總經理室"
  );
  const primary = {
    employeeProjectMemberID: employeeInfoStore.managerRegionID || -1,
    employeeProjectMemberName: employeeInfoStore.employeeName || "操作者",
    employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum.Boss,
    projectRoleName: employeeInfoStore.managerRoleName || "處長",
    serviceItemId: null,
    serviceItemName: "",
    managerDepartmentID: employeeInfoStore.managerDepartmentID || 0,
    managerRoleID: employeeInfoStore.managerRoleID || 0,
    managerRegionID: regionId,
    managerRegionName: regionName,
    managerDepartmentName:
      normalizeDepartmentName(matchedMember?.departmentName) || fallbackDepartmentName,
    employeeName: employeeInfoStore.employeeName || "操作者",
  } satisfies WrkProjectEmployeeMemberAddItemMdl;

  const index = employeeProjectMemberList.value.findIndex(
    (item) => item.employeeProjectMemberRole === DbEmployeeProjectMemberRoleEnum.Boss
  );
  if (index === -1) {
    employeeProjectMemberList.value.unshift(primary);
  } else {
    employeeProjectMemberList.value[index] = {
      ...employeeProjectMemberList.value[index],
      ...primary,
    };
  }
};

const getServiceOwnerLabel = () => {
  const serviceName = selectedServiceItems.value[0]?.serviceItemName ?? "";
  switch (serviceName) {
    case "雲安維運":
      return "監控人員";
    case "顧問諮詢":
    case "顧問服務":
    case "資安學苑":
      return "顧問";
    case "風險檢測":
      return "工程師";
    case "聯防設備":
      return "工程師";
    case "內部軟體開發":
    case "外部軟體開發":
      return "開發人員";
    default:
      return "負責人";
  }
};

const ensureServiceOwnerMember = () => {
  const regionCode = selectedProjectRegionCode.value ?? "A";
  const regionName =
    getRegionNameByCode(regionCode) || employeeInfoStore.managerRegionName || "北區";
  const defaultRegionId =
    (resolveRegionIdByName(regionName) ?? employeeInfoStore.managerRegionID) || 0;
  const ownerRole = DbEmployeeProjectMemberRoleEnum.Executor;
  const selectedServiceIds = new Set(selectedServiceItems.value.map((item) => item.serviceItemId));
  employeeProjectMemberList.value = employeeProjectMemberList.value.filter((item) => {
    if (item.employeeProjectMemberRole !== ownerRole) return true;
    if (!item.serviceItemId) return true;
    return selectedServiceIds.has(item.serviceItemId);
  });

  selectedServiceItems.value.forEach((service) => {
    const defaultDepartmentName =
      normalizeDepartmentName(pickDefaultDepartmentName(regionCode, service.serviceItemName)) ||
      normalizeDepartmentName(employeeInfoStore.managerDepartmentName) ||
      "工程部";
    const defaultDepartmentId = resolveDepartmentIdByName(defaultDepartmentName) ?? 0;
    const defaultProjectRole = getDefaultProjectRoleName(service.serviceItemName);
    const existingIndex = employeeProjectMemberList.value.findIndex(
      (item) =>
        item.employeeProjectMemberRole === ownerRole &&
        item.serviceItemId === service.serviceItemId
    );
    if (existingIndex === -1) {
      employeeProjectMemberList.value.push({
        employeeProjectMemberID: -Date.now(),
        employeeProjectMemberName: "",
        employeeProjectMemberRole: ownerRole,
        projectRoleName: defaultProjectRole,
        serviceItemId: service.serviceItemId,
        serviceItemName: service.serviceItemName,
        managerDepartmentID: defaultDepartmentId,
        managerRoleID: 0,
        managerRegionID: defaultRegionId,
        managerRegionName: regionName,
        managerDepartmentName: defaultDepartmentName,
        employeeName: "",
      });
    } else {
      const existing = employeeProjectMemberList.value[existingIndex];
      employeeProjectMemberList.value[existingIndex] = {
        ...existing,
        serviceItemId: service.serviceItemId,
        serviceItemName: service.serviceItemName,
        managerRegionID: defaultRegionId,
        managerRegionName: regionName,
        managerDepartmentID: defaultDepartmentId,
        managerDepartmentName: defaultDepartmentName,
        projectRoleName: existing.projectRoleName || defaultProjectRole,
      };
    }
  });
};

const addProjectMemberRow = (service?: { serviceItemId: number; serviceItemName: string }) => {
  const regionCode = selectedProjectRegionCode.value ?? "A";
  const regionName =
    getRegionNameByCode(regionCode) || employeeInfoStore.managerRegionName || "北區";
  const defaultDepartmentName =
    normalizeDepartmentName(pickDefaultDepartmentName(regionCode, service?.serviceItemName)) ||
    normalizeDepartmentName(employeeInfoStore.managerDepartmentName) ||
    "工程部";
  const defaultDepartmentId = resolveDepartmentIdByName(defaultDepartmentName) ?? 0;
  const defaultRegionId = resolveRegionIdByName(regionName) ?? 0;

  employeeProjectMemberList.value.push({
    employeeProjectMemberID: -Date.now(),
    employeeProjectMemberName: "",
    employeeProjectMemberRole: DbEmployeeProjectMemberRoleEnum.Executor,
    projectRoleName: "",
    serviceItemId: service?.serviceItemId ?? null,
    serviceItemName: service?.serviceItemName ?? "",
    managerDepartmentID: defaultDepartmentId,
    managerRoleID: 0,
    managerRegionID: defaultRegionId,
    managerRegionName: regionName,
    managerDepartmentName: defaultDepartmentName,
    employeeName: "",
  });
};

const getDefaultDepartmentCandidates = (serviceName: string, regionCode: "A" | "B" | "C") => {
  switch (serviceName) {
    case "雲安維運":
      return ["監控部", "鑑識部"];
    case "顧問諮詢":
    case "顧問服務":
    case "資安學苑":
      return regionCode === "A"
        ? ["北區顧問部"]
        : regionCode === "B"
          ? ["中區顧問部"]
          : ["南區顧問部"];
    case "風險檢測":
      return regionCode === "A"
        ? ["風險稽核處", "北區技服部"]
        : regionCode === "B"
          ? ["風險稽核處", "中區技服部"]
          : ["風險稽核處", "南區技服部"];
    case "聯防設備":
      return ["工程部"];
    case "內部軟體開發":
    case "外部軟體開發":
      return ["研發部", "AI 智能部", "系統開發部"];
    default:
      return [];
  }
};

const getDefaultProjectRoleName = (serviceName: string) => {
  switch (serviceName) {
    case "顧問諮詢":
    case "顧問服務":
    case "資安學苑":
      return "顧問";
    case "雲安維運":
    case "風險檢測":
    case "聯防設備":
    case "內部軟體開發":
    case "外部軟體開發":
      return "工程師";
    default:
      return "工程師";
  }
};

const syncMemberRegion = (item: WrkProjectEmployeeMemberAddItemMdl) => {
  item.managerRegionID = resolveRegionIdByName(item.managerRegionName) ?? 0;
  if (!item.managerRegionName) {
    item.managerRegionName =
      getRegionNameByCode(selectedProjectRegionCode.value) ||
      employeeInfoStore.managerRegionName ||
      "北區";
  }
};

const syncMemberDepartment = (item: WrkProjectEmployeeMemberAddItemMdl) => {
  const matched = managerDepartmentOptions.value.find(
    (option) => option.id === item.managerDepartmentID
  );
  item.managerDepartmentName =
    normalizeDepartmentName(matched?.name) ||
    normalizeDepartmentName(item.managerDepartmentName);
  if (!item.managerDepartmentName) {
    item.managerDepartmentName =
      pickDefaultDepartmentName(selectedProjectRegionCode.value ?? "A", item.serviceItemName) ||
      normalizeDepartmentName(employeeInfoStore.managerDepartmentName) ||
      "工程部";
  }
  const allowed = resolveServiceDepartmentAllowlist(item.managerRegionName, item.serviceItemName);
  if (allowed && !allowed.has(item.managerDepartmentName)) {
    item.managerDepartmentName =
      pickDefaultDepartmentName(selectedProjectRegionCode.value ?? "A", item.serviceItemName) ||
      item.managerDepartmentName;
    item.managerDepartmentID = resolveDepartmentIdByName(item.managerDepartmentName) ?? 0;
  }
};

const resolveDepartmentIdByName = (name: string) => {
  return managerDepartmentOptions.value.find((item) => item.name === name)?.id ?? null;
};

const resolveRegionIdByName = (name: string) => {
  return managerRegionOptions.value.find((item) => item.name === name)?.id ?? null;
};

const syncPrimaryDepartmentName = () => {
  const matched = primaryDepartmentOptions.value.find(
    (item) => item.id === wrkProjectAddViewObj.primaryDepartmentID
  );
  wrkProjectAddViewObj.primaryDepartmentName = matched?.name ?? "";
};

const updatePrimaryDepartmentDefault = () => {
  if (selectedServiceItems.value.length === 0) {
    wrkProjectAddViewObj.primaryDepartmentID = 0;
    wrkProjectAddViewObj.primaryDepartmentName = "";
    primaryDepartmentTouched.value = false;
    return;
  }
  if (primaryDepartmentTouched.value) {
    const exists = primaryDepartmentOptions.value.some(
      (item) => item.id === wrkProjectAddViewObj.primaryDepartmentID
    );
    if (exists) return;
    primaryDepartmentTouched.value = false;
  }
  const regionCode = selectedProjectRegionCode.value ?? "A";
  const defaultName = pickDefaultDepartmentName(
    regionCode,
    selectedServiceItems.value[0]?.serviceItemName
  );
  const matched =
    primaryDepartmentOptions.value.find((item) => item.name === defaultName) ??
    primaryDepartmentOptions.value[0];
  if (matched) {
    wrkProjectAddViewObj.primaryDepartmentID = matched.id;
    wrkProjectAddViewObj.primaryDepartmentName = matched.name;
  }
};

const handlePrimaryDepartmentChange = () => {
  primaryDepartmentTouched.value = true;
  syncPrimaryDepartmentName();
};

const pickDefaultDepartmentName = (regionCode: "A" | "B" | "C", serviceName?: string) => {
  if (!serviceName && wrkProjectAddViewObj.primaryDepartmentName) {
    return wrkProjectAddViewObj.primaryDepartmentName;
  }
  const targetServiceName = serviceName || selectedServiceItems.value[0]?.serviceItemName || "";
  const candidates = getDefaultDepartmentCandidates(targetServiceName, regionCode);
  for (const name of candidates) {
    if (managerDepartmentOptions.value.some((item) => item.name === name)) {
      return name;
    }
  }
  return candidates[0] ?? "";
};

const northCities = new Set<DbAtomCityEnum>([
  DbAtomCityEnum.Keelung,
  DbAtomCityEnum.NewTaipei,
  DbAtomCityEnum.Taipei,
  DbAtomCityEnum.Taoyuan,
  DbAtomCityEnum.HsinchuCounty,
  DbAtomCityEnum.HsinchuCity,
  DbAtomCityEnum.Miaoli,
  DbAtomCityEnum.Yilan,
]);
const centralCities = new Set<DbAtomCityEnum>([
  DbAtomCityEnum.Taichung,
  DbAtomCityEnum.Changhua,
  DbAtomCityEnum.Nantou,
  DbAtomCityEnum.Yunlin,
]);
const islandCities = new Set<DbAtomCityEnum>([
  DbAtomCityEnum.Penghu,
  DbAtomCityEnum.Kinmen,
  DbAtomCityEnum.Lienchiang,
]);

const getRegionCodeByAtomCity = (atomCity: DbAtomCityEnum | null): string => {
  if (atomCity === null || atomCity === DbAtomCityEnum.NotSelected) return "A";
  if (atomCity === DbAtomCityEnum.Undefined) return "A";

  if (islandCities.has(atomCity)) return "D";
  if (northCities.has(atomCity)) return "A";
  if (centralCities.has(atomCity)) return "B";
  return "C";
};

const projectNameAuto = computed(() => {
  if (selectedServiceItems.value.length === 0 || selectedProductNames.value.length === 0)
    return "";
  const typePrefix = selectedProjectType.value?.category === "hybrid" ? "H" : "S";
  const sequence = projectCodeAuto.value ? projectCodeAuto.value.slice(-3) : "000";
  const primaryService = selectedServiceProducts.value[0];
  const serviceName = primaryService?.service.serviceItemName ?? "";
  const productName = primaryService?.products[0]?.productName ?? selectedProductNames.value[0] ?? "";
  if (!serviceName || !productName) return "";
  return `${typePrefix}${sequence}-${serviceName}-${productName}`;
});

const getProjectSequenceKey = (regionCode: string, date: Date) => {
  const year = date.getFullYear() - 1911;
  const month = String(date.getMonth() + 1).padStart(2, "0");
  return `cache.work.project.seq.${regionCode}.${year}${month}`;
};

const getNextProjectSequence = (regionCode: string, date: Date) => {
  const key = getProjectSequenceKey(regionCode, date);
  const raw = localStorage.getItem(key);
  const lastSeq = raw ? Number(raw) : 0;
  const nextSeq = Math.min(lastSeq + 1, 999);
  return String(nextSeq).padStart(3, "0");
};

const markProjectSequenceUsed = (regionCode: string, date: Date, sequence: string) => {
  const key = getProjectSequenceKey(regionCode, date);
  localStorage.setItem(key, String(Number(sequence)));
};

const projectCodeAuto = computed(() => {
  const regionCode =
    selectedProjectRegionCode.value ?? getRegionCodeByAtomCity(managerCompanyAtomCity.value);
  if (!regionCode) return "";
  const startDate = wrkProjectAddViewObj.employeeProjectStartTime
    ? new Date(wrkProjectAddViewObj.employeeProjectStartTime)
    : new Date();
  const rocYear = String(startDate.getFullYear() - 1911).padStart(3, "0");
  const month = String(startDate.getMonth() + 1).padStart(2, "0");
  const day = String(startDate.getDate()).padStart(2, "0");
  const sequence = getNextProjectSequence(regionCode, startDate);
  return `P${regionCode}${rocYear}${month}${day}${sequence}`;
});

onMounted(() => {
  templateSettings.value = loadProjectTemplateSettings();
  if (!selectedProjectTypeId.value) {
    selectedProjectTypeId.value = projectTypeTemplates.value[0]?.projectTypeId ?? null;
  }
  ensurePrimaryMember();
  ensureServiceOwnerMember();
});

const useMockData = true;

const loadDepartmentOptions = async () => {
  if (useMockData) {
    managerDepartmentOptions.value = orgDepartmentDirectory.map((name, index) => ({
      id: index + 1,
      name,
    }));
    return;
  }
  if (!tokenStore.token) return;
  const responseData = await getAllBasicManagerDepartment({
    employeeLoginToken: tokenStore.token,
  });
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return;
  const apiList =
    responseData.managerDepartmentList?.map((item) => ({
      id: item.managerDepartmentID,
      name: item.managerDepartmentName,
    })) ?? [];
  managerDepartmentOptions.value = apiList;
};

const loadRegionOptions = async () => {
  if (useMockData) {
    managerRegionOptions.value = projectRegionOptions.map((item, index) => ({
      id: index + 1,
      name: item.regionName,
    }));
    return;
  }
  if (!tokenStore.token) return;
  const responseData = await getAllBasicManagerRegion({
    employeeLoginToken: tokenStore.token,
  });
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return;
  managerRegionOptions.value =
    responseData.managerRegionList?.map((item) => ({
      id: item.managerRegionID,
      name: item.managerRegionName,
    })) ?? [];
};

watch(
  () => tokenStore.token,
  (token) => {
    if (!token && !useMockData) return;
    loadDepartmentOptions();
    loadRegionOptions();
  },
  { immediate: true }
);

watch(
  () => [employeeInfoStore.employeeName, employeeInfoStore.managerRegionName, selectedProjectRegionCode.value],
  () => {
    ensurePrimaryMember();
    ensureServiceOwnerMember();
  }
);

watch(
  () => selectedServiceItemIds.value,
  () => {
    ensureServiceOwnerMember();
  }
);

watch(
  () => [selectedServiceItemIds.value, selectedProjectRegionCode.value, managerDepartmentOptions.value],
  () => {
    updatePrimaryDepartmentDefault();
  },
  { immediate: true }
);

watch(
  () => wrkProjectAddViewObj.primaryDepartmentID,
  () => {
    syncPrimaryDepartmentName();
  }
);

watch(
  () => wrkProjectAddViewObj.primaryDepartmentName,
  () => {
    ensurePrimaryMember();
  }
);

watch(
  () => stageGroups.value,
  (groups) => {
    if (groups.length === 0) {
      activeStageServiceId.value = null;
      return;
    }
    const exists = groups.some(
      (group) => group.service.serviceItemId === activeStageServiceId.value
    );
    if (!exists) {
      activeStageServiceId.value = groups[0].service.serviceItemId;
    }
  },
  { immediate: true }
);

const resolveCreatedProjectId = async () => {
  if (!tokenStore.token) return null;
  const responseData = await getManyProject({
    employeeLoginToken: tokenStore.token,
    atomEmployeeProjectStatus: null,
    employeeProjectName: wrkProjectAddViewObj.employeeProjectName,
    pageIndex: 1,
    pageSize: 50,
  });
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return null;
  const matched = responseData.employeeProjectList?.find(
    (item) =>
      item.employeeProjectName === wrkProjectAddViewObj.employeeProjectName &&
      item.managerCompanyName === managerCompanyName.value &&
      item.employeeProjectStartTime === wrkProjectAddViewObj.employeeProjectStartTime &&
      item.employeeProjectEndTime === wrkProjectAddViewObj.employeeProjectEndTime
  );
  return matched?.employeeProjectID ?? null;
};

const buildCreatedSummary = () => {
  const regionLabel = getRegionNameByCode(selectedProjectRegionCode.value);
  const projectTypeName = selectedProjectType.value?.projectTypeName ?? "";
  const serviceItems = selectedServiceItems.value.map((item) => item.serviceItemName);
  const products = selectedServiceProducts.value.flatMap((service) =>
    service.products.map((product) => product.productName)
  );
  const members = employeeProjectMemberList.value.map((item) => ({
    roleLabel: item.projectRoleName || getEmployeeProjectMemberRoleLabel(item.employeeProjectMemberRole),
    regionName: item.managerRegionName,
    departmentName: item.managerDepartmentName,
    employeeName: item.employeeName,
  }));
  createdProjectSummary.value = {
    regionLabel,
    projectCode: wrkProjectAddViewObj.employeeProjectCode,
    projectName: wrkProjectAddViewObj.employeeProjectName,
    companyName: managerCompanyName.value ?? "",
    startTime: wrkProjectAddViewObj.employeeProjectStartTime,
    endTime: wrkProjectAddViewObj.employeeProjectEndTime,
    projectTypeName,
    serviceItems,
    products,
    members,
  };
};

watch(
  () => selectedProjectTypeId.value,
  () => {
    if (!selectedProjectType.value) {
      selectedServiceItemIds.value = [];
      selectedServiceProductIds.value = {};
      return;
    }
    selectedServiceItemIds.value = [];
    selectedServiceProductIds.value = {};
  }
);

watch(
  projectNameAuto,
  (value) => {
    wrkProjectAddViewObj.employeeProjectName = value;
  },
  { immediate: true }
);

watch(
  projectCodeAuto,
  (value) => {
    wrkProjectAddViewObj.employeeProjectCode = value;
  },
  { immediate: true }
);

watch(
  () => wrkProjectAddViewObj.managerCompanyID,
  async (companyId) => {
    managerCompanyAtomCity.value = null;
    if (!companyId || (!tokenStore.token && !useMockData)) return;
    const responseData = await getManyCompanyLocation({
      employeeLoginToken: tokenStore.token || "mock-token",
      managerCompanyID: companyId,
    });
    if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return;
    managerCompanyAtomCity.value = responseData.managerCompanyLocationList[0]?.atomCity ?? null;
    if (!selectedProjectRegionCode.value) {
      const autoCode = getRegionCodeByAtomCity(managerCompanyAtomCity.value);
      selectedProjectRegionCode.value = (autoCode === "D" ? "C" : autoCode) as
        | "A"
        | "B"
        | "C";
    }
  }
);

const formatDateInput = (value: Date) => {
  const year = value.getFullYear();
  const month = String(value.getMonth() + 1).padStart(2, "0");
  const day = String(value.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};

const formatDateRange = (range: [Date, Date] | null) => {
  if (!range) return "";
  const [start, end] = range;
  if (!start || !end) return "";
  return `${formatDateInput(start)} ~ ${formatDateInput(end)}`;
};

watch(
  projectDateRange,
  (range) => {
    if (!range) {
      wrkProjectAddViewObj.employeeProjectStartTime = "";
      wrkProjectAddViewObj.employeeProjectEndTime = "";
      return;
    }
    const [start, end] = range;
    wrkProjectAddViewObj.employeeProjectStartTime = start ? formatDateInput(start) : "";
    wrkProjectAddViewObj.employeeProjectEndTime = end ? formatDateInput(end) : "";
  },
  { deep: true }
);

watch(
  () => [
    wrkProjectAddViewObj.employeeProjectStartTime,
    wrkProjectAddViewObj.employeeProjectEndTime,
  ],
  ([start, end]) => {
    if (!start || !end) return;
    const startDate = new Date(start);
    const endDate = new Date(end);
    if (!projectDateRange.value) {
      projectDateRange.value = [startDate, endDate];
      return;
    }
    const [currentStart, currentEnd] = projectDateRange.value;
    if (currentStart?.getTime() !== startDate.getTime() || currentEnd?.getTime() !== endDate.getTime()) {
      projectDateRange.value = [startDate, endDate];
    }
  }
);

const toggleServiceItem = (serviceItemId: number) => {
  if (!selectedProjectType.value) return;
  if (selectedProjectType.value.category === "single") {
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

const toggleServiceProduct = (serviceItemId: number, productId: number) => {
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

//#endregion

//#region 驗證相關
const validateStepOne = () => {
  let isValid = true;

  wrkProjectAddValidateObj.projectRegionCode = !!selectedProjectRegionCode.value;
  if (!wrkProjectAddValidateObj.projectRegionCode) isValid = false;

  wrkProjectAddValidateObj.managerCompanyID = !!wrkProjectAddViewObj.managerCompanyID;
  if (!wrkProjectAddValidateObj.managerCompanyID) isValid = false;

  wrkProjectAddValidateObj.employeeProjectStartTime =
    !!wrkProjectAddViewObj.employeeProjectStartTime;
  if (!wrkProjectAddValidateObj.employeeProjectStartTime) isValid = false;

  wrkProjectAddValidateObj.employeeProjectEndTime = !!wrkProjectAddViewObj.employeeProjectEndTime;
  if (!wrkProjectAddValidateObj.employeeProjectEndTime) isValid = false;

  if (
    wrkProjectAddViewObj.employeeProjectStartTime &&
    wrkProjectAddViewObj.employeeProjectEndTime
  ) {
    const start = new Date(wrkProjectAddViewObj.employeeProjectStartTime);
    const end = new Date(wrkProjectAddViewObj.employeeProjectEndTime);
    wrkProjectAddValidateObj.employeeProjectEndAfterStart = end >= start;
    if (!wrkProjectAddValidateObj.employeeProjectEndAfterStart) isValid = false;
  } else {
    wrkProjectAddValidateObj.employeeProjectEndAfterStart = true;
  }

  return isValid;
};

const validateStepTwo = () => {
  let isValid = true;

  wrkProjectAddValidateObj.projectTypeId = !!selectedProjectTypeId.value;
  if (!wrkProjectAddValidateObj.projectTypeId) isValid = false;

  wrkProjectAddValidateObj.serviceItems = selectedServiceItemIds.value.length > 0;
  if (!wrkProjectAddValidateObj.serviceItems) isValid = false;

  wrkProjectAddValidateObj.serviceProducts =
    selectedServiceItemIds.value.length > 0 &&
    selectedServiceItems.value.every(
      (service) => (selectedServiceProductIds.value[service.serviceItemId] ?? []).length > 0
    );
  if (!wrkProjectAddValidateObj.serviceProducts) isValid = false;

  return isValid;
};

/** 驗證【專案】欄位 */
const validateProjectField = () => {
  let isValid = true;

  // 專案類型
  wrkProjectAddValidateObj.projectTypeId = !!selectedProjectTypeId.value;
  if (!wrkProjectAddValidateObj.projectTypeId) isValid = false;

  // 服務項目
  wrkProjectAddValidateObj.serviceItems = selectedServiceItemIds.value.length > 0;
  if (!wrkProjectAddValidateObj.serviceItems) isValid = false;

  // 產品
  wrkProjectAddValidateObj.serviceProducts =
    selectedServiceItemIds.value.length > 0 &&
    selectedServiceItems.value.every(
      (service) => (selectedServiceProductIds.value[service.serviceItemId] ?? []).length > 0
    );
  if (!wrkProjectAddValidateObj.serviceProducts) isValid = false;

  // 主部門
  wrkProjectAddValidateObj.primaryDepartmentID = !!wrkProjectAddViewObj.primaryDepartmentID;
  if (!wrkProjectAddValidateObj.primaryDepartmentID) isValid = false;

  wrkProjectAddValidateObj.orderFiles = true;
  wrkProjectAddValidateObj.slaFiles = true;

  // 專案名稱
  const name = wrkProjectAddViewObj.employeeProjectName.trim();
  wrkProjectAddValidateObj.employeeProjectName = !!name;
  if (!wrkProjectAddValidateObj.employeeProjectName) isValid = false;

  // 專案區域
  wrkProjectAddValidateObj.projectRegionCode = !!selectedProjectRegionCode.value;
  if (!wrkProjectAddValidateObj.projectRegionCode) isValid = false;

  // 專案代碼（不可為空，且長度 ≤ 50）
  const code = wrkProjectAddViewObj.employeeProjectCode.trim();
  wrkProjectAddValidateObj.employeeProjectCode = !!code;
  wrkProjectAddValidateObj.employeeProjectCode = code.length > 0 && code.length <= 50;
  if (!wrkProjectAddValidateObj.employeeProjectCode) isValid = false;

  // 公司
  wrkProjectAddValidateObj.managerCompanyID = !!wrkProjectAddViewObj.managerCompanyID;
  if (!wrkProjectAddValidateObj.managerCompanyID) isValid = false;

  // 開始時間
  wrkProjectAddValidateObj.employeeProjectStartTime =
    !!wrkProjectAddViewObj.employeeProjectStartTime;
  if (!wrkProjectAddValidateObj.employeeProjectStartTime) isValid = false;

  // 結束時間
  wrkProjectAddValidateObj.employeeProjectEndTime = !!wrkProjectAddViewObj.employeeProjectEndTime;
  if (!wrkProjectAddValidateObj.employeeProjectEndTime) isValid = false;

  // 結束時間需大於開始時間
  if (
    wrkProjectAddViewObj.employeeProjectStartTime &&
    wrkProjectAddViewObj.employeeProjectEndTime
  ) {
    const start = new Date(wrkProjectAddViewObj.employeeProjectStartTime);
    const end = new Date(wrkProjectAddViewObj.employeeProjectEndTime);
    wrkProjectAddValidateObj.employeeProjectEndAfterStart = end >= start;
    if (!wrkProjectAddValidateObj.employeeProjectEndAfterStart) isValid = false;
  } else {
    wrkProjectAddValidateObj.employeeProjectEndAfterStart = true;
  }

  // 契約網址
  const urlPattern = /^(https?:\/\/)([\w-]+\.)+[\w-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$/;

  const contractUrl = wrkProjectAddViewObj.employeeProjectContractUrl?.trim() ?? "";

  // 如果有輸入值，才驗證格式；沒輸入則視為有效
  if (contractUrl.length > 0) {
    wrkProjectAddValidateObj.employeeProjectContractUrl = urlPattern.test(contractUrl);
    if (!wrkProjectAddValidateObj.employeeProjectContractUrl) isValid = false;
  } else {
    wrkProjectAddValidateObj.employeeProjectContractUrl = true;
  }

  // 工作計劃書網址
  const workUrl = wrkProjectAddViewObj.employeeProjectWorkUrl?.trim() ?? "";

  // 如果有輸入值，才驗證格式；沒輸入則視為有效
  if (workUrl.length > 0) {
    wrkProjectAddValidateObj.employeeProjectWorkUrl = urlPattern.test(workUrl);
    if (!wrkProjectAddValidateObj.employeeProjectWorkUrl) isValid = false;
  } else {
    wrkProjectAddValidateObj.employeeProjectWorkUrl = true;
  }
  return isValid;
};

/** 驗證【專案人員】欄位 */
const validateProjectEmployeeField = () => {
  let isValid = true;

  // 人員 ID
  wrkProjectEmployeeMemberAddItemValidateObj.employeeProjectMemberID =
    !!wrkProjectEmployeeMemberAddItemObj.employeeProjectMemberID;
  if (!wrkProjectEmployeeMemberAddItemValidateObj.employeeProjectMemberID) isValid = false;

  return isValid;
};

/** 檢查是否已有相同「人員 + 角色」 */
const isDuplicateProjectMember = (): boolean => {
  return employeeProjectMemberList.value.some((item, idx) => {
    // 編輯模式時，跳過自己
    if (employeeProjectMemberListIndex.value === idx) return false;

    return (
      item.employeeProjectMemberID === wrkProjectEmployeeMemberAddItemObj.employeeProjectMemberID &&
      item.employeeProjectMemberRole ===
        wrkProjectEmployeeMemberAddItemObj.employeeProjectMemberRole
    );
  });
};


/** 重置錯誤 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};
//#endregion

//#region 按鈕事件
const goToNextStep = () => {
  if (currentStep.value === 1 && !validateStepOne()) return;
  if (currentStep.value === 2 && !validateStepTwo()) return;
  if (currentStep.value === 3) {
    buildCreatedSummary();
    currentStep.value = 4;
    return;
  }
  if (currentStep.value < 3) currentStep.value += 1;
};

const goToPrevStep = () => {
  if (currentStep.value > 1) currentStep.value -= 1;
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  if (props.isModal) {
    emit("close");
    return;
  }
  router.push("/work/project");
};

/** 點擊送出【專案】按鈕 */
const clickSubmitProjectBtn = async () => {
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateProjectField()) {
    return;
  }

  //設定新增人員列表
  wrkProjectAddViewObj.employeeProjectMemberList = [...employeeProjectMemberList.value];
  //呼叫api
  const requestData: MbsWrkPrjHttpAddProjectReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: wrkProjectAddViewObj.managerCompanyID,
    employeeProjectCode: wrkProjectAddViewObj.employeeProjectCode,
    employeeProjectName: wrkProjectAddViewObj.employeeProjectName,
    employeeProjectStartTime: formatToServerDateStartISO8(
      wrkProjectAddViewObj.employeeProjectStartTime
    ),
    employeeProjectEndTime: formatToServerDateEndISO8(wrkProjectAddViewObj.employeeProjectEndTime),
    employeeProjectContractUrl: wrkProjectAddViewObj.employeeProjectContractUrl?.trim() || null,
    employeeProjectWorkUrl: wrkProjectAddViewObj.employeeProjectWorkUrl?.trim() || null,
    employeeProjectMemberList:
      wrkProjectAddViewObj.employeeProjectMemberList
        .filter(
          (item) =>
            item.employeeProjectMemberRole !== DbEmployeeProjectMemberRoleEnum.Boss &&
            item.employeeProjectMemberID > 0
        )
        .map((item) => ({
          employeeProjectMemberRole: item.employeeProjectMemberRole,
          employeeID: item.employeeProjectMemberID,
        })) ?? [],
  } satisfies MbsWrkPrjHttpAddProjectReqMdl;

  const responseData = await addProject(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  if (wrkProjectAddViewObj.employeeProjectCode) {
    const regionCode =
      selectedProjectRegionCode.value ?? getRegionCodeByAtomCity(managerCompanyAtomCity.value);
    const startDate = wrkProjectAddViewObj.employeeProjectStartTime
      ? new Date(wrkProjectAddViewObj.employeeProjectStartTime)
      : new Date();
    if (regionCode) {
      const sequence = wrkProjectAddViewObj.employeeProjectCode.slice(-3);
      markProjectSequenceUsed(regionCode, startDate, sequence);
    }
  }

  // 顯示成功訊息
  displaySuccess("新增專案成功！");

  createdProjectId.value = await resolveCreatedProjectId();
  buildCreatedSummary();

  if (useMockData) {
    const snapshotKey = "cache.work.project.snapshot";
    const listCacheKey = "cache.work.project.list";
    const snapshotRaw = localStorage.getItem(snapshotKey);
    const snapshot = snapshotRaw ? JSON.parse(snapshotRaw) : [];
    const nextId =
      snapshot.reduce((max: number, item: { id: number }) => Math.max(max, item.id), 0) + 1;
    const projectId = createdProjectId.value ?? nextId;
    createdProjectId.value = projectId;
    const newSnapshotItem = {
      id: projectId,
      status: DbAtomEmployeeProjectStatusEnum.NotStarted,
      name: wrkProjectAddViewObj.employeeProjectName,
      companyName: managerCompanyName.value ?? "",
      startTime: wrkProjectAddViewObj.employeeProjectStartTime,
      endTime: wrkProjectAddViewObj.employeeProjectEndTime,
      isRenewal: isRenewal.value,
      isOutsourced: isOutsourced.value,
    };
    const snapshotExists = snapshot.some((item: { id: number }) => item.id === projectId);
    if (!snapshotExists) {
      snapshot.unshift(newSnapshotItem);
      localStorage.setItem(snapshotKey, JSON.stringify(snapshot));
    }

    const listCacheRaw = localStorage.getItem(listCacheKey);
    const listCache = listCacheRaw ? JSON.parse(listCacheRaw) : null;
    if (listCache?.employeeProjectList) {
      const listExists = listCache.employeeProjectList.some(
        (item: { employeeProjectID: number }) => item.employeeProjectID === projectId
      );
      if (!listExists) {
        listCache.employeeProjectList.unshift({
          employeeProjectID: projectId,
          atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum.NotStarted,
          employeeProjectName: newSnapshotItem.name,
          managerCompanyName: newSnapshotItem.companyName,
          employeeProjectStartTime: newSnapshotItem.startTime,
          employeeProjectEndTime: newSnapshotItem.endTime,
        });
        listCache.totalCount = (listCache.totalCount ?? 0) + 1;
        localStorage.setItem(listCacheKey, JSON.stringify(listCache));
      }
    }

    localStorage.setItem(
      `workProjectFlags:${projectId}`,
      JSON.stringify({ isRenewal: isRenewal.value, isOutsourced: isOutsourced.value })
    );

    const detailCacheKey = `cache.work.project.detail.${projectId}`;
    const persistedMembers = wrkProjectAddViewObj.employeeProjectMemberList
      .filter(
        (item) =>
          item.employeeProjectMemberRole === DbEmployeeProjectMemberRoleEnum.Boss
            ? Boolean(item.employeeName)
            : item.employeeProjectMemberID > 0
      )
      .map((item) => ({
        employeeProjectMemberID: item.employeeProjectMemberID,
        employeeProjectMemberRole: item.employeeProjectMemberRole,
        managerRegionName: item.managerRegionName,
        managerDepartmentName: item.managerDepartmentName,
        employeeName: item.employeeName,
        employeeID: item.employeeProjectMemberID,
      }));
    localStorage.setItem(
      detailCacheKey,
      JSON.stringify({
        errorCode: MbsErrorCodeEnum.Success,
        atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum.NotStarted,
        employeeProjectCode: wrkProjectAddViewObj.employeeProjectCode,
        employeeProjectContractCreateTime: "",
        employeeProjectContractUrl: null,
        employeeProjectWorkCreateTime: "",
        employeeProjectWorkUrl: null,
        employeeProjectMemberList: persistedMembers,
        employeeProjectName: wrkProjectAddViewObj.employeeProjectName,
        employeeProjectStartTime: wrkProjectAddViewObj.employeeProjectStartTime,
        employeeProjectEndTime: wrkProjectAddViewObj.employeeProjectEndTime,
        managerCompanyName: managerCompanyName.value ?? "",
      })
    );
    window.dispatchEvent(new CustomEvent("work-project-members-updated"));
  }

  if (createdProjectId.value) {
    const projectTypeKey = `workProjectProjectType:${createdProjectId.value}`;
    const serviceItemsKey = `workProjectServiceItems:${createdProjectId.value}`;
    const serviceProductsKey = `workProjectServiceProducts:${createdProjectId.value}`;
    const flagsKey = `workProjectFlags:${createdProjectId.value}`;
    if (selectedProjectTypeId.value) {
      localStorage.setItem(projectTypeKey, String(selectedProjectTypeId.value));
    }
    if (selectedServiceItemIds.value.length > 0) {
      localStorage.setItem(serviceItemsKey, JSON.stringify(selectedServiceItemIds.value));
    }
    if (Object.keys(selectedServiceProductIds.value).length > 0) {
      localStorage.setItem(serviceProductsKey, JSON.stringify(selectedServiceProductIds.value));
    }
    localStorage.setItem(
      flagsKey,
      JSON.stringify({ isRenewal: isRenewal.value, isOutsourced: isOutsourced.value })
    );
  }

  emit("created", {
    employeeProjectID: createdProjectId.value,
    atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum.NotStarted,
    employeeProjectName: wrkProjectAddViewObj.employeeProjectName,
    managerCompanyName: managerCompanyName.value ?? "",
    employeeProjectStartTime: wrkProjectAddViewObj.employeeProjectStartTime,
    employeeProjectEndTime: wrkProjectAddViewObj.employeeProjectEndTime,
  });

  if (createdProjectId.value) {
    router.push(`/work/project/detail/${createdProjectId.value}`);
  } else {
    router.push("/work/project");
  }
  if (props.isModal) emit("close");
};

/** 點擊新增【開發評等原因】按鈕 */
const clickAddWorkEmployeeBtn = () => {
  const regionCode = selectedProjectRegionCode.value ?? "A";
  const regionName = getRegionNameByCode(regionCode);
  const defaultDepartmentName = pickDefaultDepartmentName(
    regionCode,
    selectedServiceItems.value[0]?.serviceItemName
  );
  const defaultDepartmentId = resolveDepartmentIdByName(defaultDepartmentName) ?? 0;
  const defaultRegionId = resolveRegionIdByName(regionName) ?? 0;

  employeeProjectMemberListIndex.value = null;
  Object.assign(wrkProjectEmployeeMemberAddItemObj, {
    employeeProjectMemberID: -2,
    employeeProjectMemberName: "",
    employeeProjectMemberRole: null,
    projectRoleName: "",
    serviceItemId: selectedServiceItems.value[0]?.serviceItemId ?? null,
    serviceItemName: selectedServiceItems.value[0]?.serviceItemName ?? "",
    managerDepartmentID: defaultDepartmentId,
    managerRoleID: 0,
    managerRegionID: defaultRegionId,
    managerRegionName: regionName,
    managerDepartmentName: defaultDepartmentName,
    employeeName: "",
  });
  isShowAddMemberModal.value = true;
};

/** 點擊送出【人員】按鈕 */
const clickSubmitEmployeeMemberBtn = () => {
  // 驗證欄位
  if (!validateProjectEmployeeField()) {
    return;
  }

  // 角色、人員重複檢查
  if (isDuplicateProjectMember()) {
    displayError("相同角色與人員已存在，請重新選擇！");
    return;
  }

  const objCopy = { ...wrkProjectEmployeeMemberAddItemObj };

  if (employeeProjectMemberListIndex.value !== null) {
    employeeProjectMemberList.value[employeeProjectMemberListIndex.value] = objCopy;
  } else {
    employeeProjectMemberList.value.push(objCopy);
  }

  isShowAddMemberModal.value = false;
};


//#endregion

//#region 彈跳視窗事件
/** 開啟刪除確認視窗 */
const openRemoveConfirmDialog = (employeeProjectMemberID: number) => {
  pendingRemoveEmployeeProjectMemberID.value = employeeProjectMemberID;
  showConfirmDialog.value = true;
};

/** 確認刪除人員 */
const confirmSave = () => {
  if (pendingRemoveEmployeeProjectMemberID.value === null) return;

  const mIndex = employeeProjectMemberList.value.findIndex(
    (item) => item.employeeProjectMemberID === pendingRemoveEmployeeProjectMemberID.value
  );

  if (mIndex > -1) {
    employeeProjectMemberList.value.splice(mIndex, 1);
  }

  // 清空狀態
  pendingRemoveEmployeeProjectMemberID.value = null;
  showConfirmDialog.value = false;
};

/** 取消刪除 */
const cancelSave = () => {
  pendingRemoveEmployeeProjectMemberID.value = null;
  showConfirmDialog.value = false;
};
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4 project-add-font">
    <div v-if="currentStep === 1 || currentStep === 2" class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div v-if="currentStep === 1">
        <div class="flex items-center justify-between">
          <h2 class="subtitle mt-2">專案基本資訊</h2>
        </div>
        <p class="text-xs text-gray-500 mt-1">請先完成客戶所在地、客戶與專案起迄日。</p>

        <div class="flex gap-4 w-full mt-4">
          <!-- 客戶所在地 -->
          <div class="flex-1">
            <label class="form-label">客戶所在地 <span class="required-label">*</span></label>
            <div class="combo-custom">
              <div class="relative flex w-full">
                <select v-model="selectedProjectRegionCode" class="input-box select-reset">
                  <option :value="null">請選擇</option>
                  <option v-for="item in projectRegionOptions" :key="item.code" :value="item.code">
                    {{ item.label }}
                  </option>
                </select>
                <span class="select-icon">
                  <svg class="w-4 h-4" viewBox="0 0 20 25" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path
                      d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                      fill="#787676"
                    />
                  </svg>
                </span>
              </div>
            </div>
            <div class="error-wrapper">
              <p v-if="!wrkProjectAddValidateObj.projectRegionCode" class="error-tip">不可為空</p>
            </div>
          </div>

          <!-- 客戶 -->
          <div class="flex-1">
            <label class="form-label">客戶 <span class="required-label">*</span></label>
            <div class="combo-custom">
              <GetManyManagerCompanyComboBox
                v-model:managerCompanyID="wrkProjectAddViewObj.managerCompanyID"
                v-model:managerCompanyName="managerCompanyName"
                :disabled="false"
              />
              <span class="select-icon">
                <svg class="w-4 h-4" viewBox="0 0 20 25" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path
                    d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                    fill="#787676"
                  />
                </svg>
              </span>
            </div>
            <div class="error-wrapper">
              <p v-if="!wrkProjectAddValidateObj.managerCompanyID" class="error-tip">不可為空</p>
            </div>
          </div>
        </div>

        <div class="mt-4">
          <h2 class="subtitle mt-2 text-gray-700">專案期間</h2>
          <div class="flex gap-4 mt-2">
            <div class="flex-1">
              <label class="form-label">起迄日期 <span class="required-label">*</span></label>
              <VueDatePicker
                v-model="projectDateRange"
                range
                :auto-apply="true"
                :clearable="false"
                :enable-time-picker="false"
                :format="formatDateRange"
                class="date-picker-custom"
                placeholder="選擇專案起迄日"
              />
              <div class="error-wrapper">
                <p v-if="!wrkProjectAddValidateObj.employeeProjectStartTime" class="error-tip">
                  不可為空
                </p>
              </div>
              <div class="error-wrapper">
                <p v-if="!wrkProjectAddValidateObj.employeeProjectEndTime" class="error-tip">
                  不可為空
                </p>
              </div>
              <div class="error-wrapper">
                <p v-if="!wrkProjectAddValidateObj.employeeProjectEndAfterStart" class="error-tip">
                  結束日期需大於或等於開始日期
                </p>
              </div>
            </div>
          </div>
        </div>

        <div class="flex justify-center mt-3 gap-2">
          <button class="btn-cancel" @click="clickBackBtn">取消建立</button>
          <button class="btn-submit" @click="goToNextStep">下一步</button>
        </div>
      </div>

      <div v-if="currentStep === 2">
        <div>
          <div class="flex items-center justify-between">
            <h2 class="subtitle mt-2">選擇專案類型與服務項目</h2>
          </div>
          <p class="text-xs text-gray-500 mt-1">
            選擇完後，系統將套用標準階段、產出與提醒規則。
          </p>

          <div class="flex gap-4 w-full mt-4">
            <div class="flex-1">
              <h2 class="subtitle mt-2 text-gray-700">專案類型</h2>
              <div class="mt-2 grid grid-cols-2 gap-2">
                <button
                  v-for="item in projectTypeTemplates"
                  :key="item.projectTypeId"
                  class="rounded-lg border px-3 py-2 text-left transition"
                  :class="
                    item.projectTypeId === selectedProjectTypeId
                      ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                      : 'border-gray-200 hover:border-cyan-400'
                  "
                  @click="selectedProjectTypeId = item.projectTypeId"
                >
                  <p class="font-semibold">{{ item.projectTypeName }}</p>
                  <p class="text-xs text-gray-500">
                    {{ item.description || "無描述" }}
                  </p>
                </button>
              </div>
              <p v-if="!wrkProjectAddValidateObj.projectTypeId" class="error-tip">
                請選擇專案類型
              </p>
            </div>
          </div>

          <div class="mt-4">
            <h2 class="subtitle mt-2 text-gray-700">服務項目</h2>
            <p v-if="!selectedProjectType" class="text-sm text-gray-500">
              請先選擇專案類型以設定服務項目。
            </p>
            <div v-else class="grid grid-cols-3 gap-2 mt-2">
              <button
                v-for="item in availableServiceItems"
                :key="item.serviceItemId"
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
            <p v-if="!wrkProjectAddValidateObj.serviceItems" class="error-tip mt-1">
              請選擇至少一項服務
            </p>
          </div>

          <div class="mt-4">
            <h2 class="subtitle mt-2 text-gray-700">主部門</h2>
            <p v-if="selectedServiceItems.length === 0" class="text-sm text-gray-500">
              請先選擇服務項目以帶入主部門。
            </p>
            <div v-else class="combo-custom">
              <div class="relative flex w-full">
                <select
                  v-model="wrkProjectAddViewObj.primaryDepartmentID"
                  class="input-box select-reset"
                  :disabled="selectedServiceItems.length === 0"
                  @change="handlePrimaryDepartmentChange"
                >
                  <option :value="0">請選擇</option>
                  <option
                    v-for="dept in primaryDepartmentOptions"
                    :key="dept.id"
                    :value="dept.id"
                  >
                    {{ dept.name }}
                  </option>
                </select>
                <span class="select-icon">
                  <svg class="w-4 h-4" viewBox="0 0 20 25" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path
                      d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                      fill="#787676"
                    />
                  </svg>
                </span>
              </div>
            </div>
            <div class="error-wrapper">
              <p v-if="!wrkProjectAddValidateObj.primaryDepartmentID" class="error-tip">不可為空</p>
            </div>
          </div>

          <div class="mt-4">
            <h2 class="subtitle mt-2 text-gray-700">產品</h2>
            <p v-if="selectedServiceItems.length === 0" class="text-sm text-gray-500">
              請先選擇服務項目以設定產品。
            </p>
            <div v-else class="space-y-4 mt-2">
              <div class="mt-2">
                <input
                  v-model="productSearch"
                  type="text"
                  class="input-box"
                  placeholder="搜尋產品（可輸入中文或英文）"
                />
              </div>
              <div
                v-for="serviceEntry in filteredServiceProducts"
                :key="serviceEntry.service.serviceItemId"
                class="rounded-lg border border-gray-200 bg-gray-50 p-4"
              >
                <div class="flex items-center justify-between">
                  <h3 class="subtitle text-gray-700">
                    {{ serviceEntry.service.serviceItemName }}
                  </h3>
                  <span class="text-xs text-gray-500">
                    已選
                    {{ (selectedServiceProductIds[serviceEntry.service.serviceItemId] ?? []).length }}
                    項
                  </span>
                </div>
                <p v-if="serviceEntry.products.length === 0" class="text-xs text-gray-500 mt-3">
                  無符合產品
                </p>
                <div v-else class="grid grid-cols-3 gap-2 mt-3">
                  <button
                    v-for="product in serviceEntry.products"
                    :key="product.productId"
                    class="rounded-lg border px-3 py-2 text-left transition"
                    :class="
                      (selectedServiceProductIds[serviceEntry.service.serviceItemId] ?? []).includes(
                        product.productId
                      )
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 hover:border-cyan-400'
                    "
                    @click="toggleServiceProduct(serviceEntry.service.serviceItemId, product.productId)"
                  >
                    <p class="font-semibold">{{ product.productName }}</p>
                    <p class="text-xs text-gray-500 line-clamp-1">
                      {{ product.description || "無描述" }}
                    </p>
                  </button>
                </div>
              </div>
            </div>
            <p v-if="!wrkProjectAddValidateObj.serviceProducts" class="error-tip mt-1">
              請為每個服務項目選擇至少一項產品
            </p>
          </div>

          <div class="mt-4">
            <h2 class="subtitle mt-2 text-gray-700">專案屬性</h2>
            <div class="mt-2 flex flex-wrap gap-4 text-sm text-gray-700">
              <label class="flex items-center gap-2">
                <input v-model="isRenewal" type="checkbox" class="h-4 w-4 rounded border-gray-300" />
                續約
              </label>
              <label class="flex items-center gap-2">
                <input v-model="isOutsourced" type="checkbox" class="h-4 w-4 rounded border-gray-300" />
                委外
              </label>
            </div>
            <p class="text-xs text-gray-500 mt-1">未勾選續約即視為新案。</p>
          </div>

          <div class="mt-4">
            <h2 class="subtitle mt-2 text-gray-700">標準階段</h2>
            <div class="mt-4 rounded-lg border border-gray-200 bg-gray-50 p-4">
              <p
                v-if="
                  !selectedProjectType ||
                  selectedServiceItemIds.length === 0 ||
                  !hasSelectedProducts
                "
                class="text-sm text-gray-500 mt-2"
              >
                請先選擇專案類型、服務項目與產品以查看標準階段與產出。
              </p>
              <div v-else class="mt-3">
                <div v-if="stageGroups.length > 1" class="flex flex-wrap gap-2 mb-3">
                  <button
                    v-for="group in stageGroups"
                    :key="group.service.serviceItemId"
                    type="button"
                    class="rounded-lg border px-3 py-2 text-left transition"
                    :class="
                      group.service.serviceItemId === activeStageServiceId
                        ? 'border-cyan-500 bg-cyan-50 text-cyan-700'
                        : 'border-gray-200 hover:border-cyan-400'
                    "
                    @click="activeStageServiceId = group.service.serviceItemId"
                  >
                    <p class="font-semibold">{{ group.service.serviceItemName }}</p>
                  </button>
                </div>

                <p v-if="!hasStageTemplates" class="text-xs text-gray-500">
                  尚未設定標準階段模板
                </p>
                <div v-else class="space-y-4">
                  <div
                    v-for="productGroup in (activeStageGroup?.products ?? [])"
                    :key="productGroup.product.productId"
                    class="rounded-lg border border-gray-200 bg-white p-4"
                  >
                    <div class="flex items-center justify-between">
                      <div>
                        <p class="font-semibold text-gray-700">
                          {{ productGroup.product.productName }}
                        </p>
                        <p class="text-xs text-gray-500">
                          {{ productGroup.product.description || "無描述" }}
                        </p>
                      </div>
                    </div>
                    <div
                      v-if="productGroup.templates.length === 0"
                      class="text-xs text-gray-500 mt-2"
                    >
                      尚未設定標準階段模板
                    </div>
                    <div v-else class="space-y-3 mt-3">
                      <div
                        v-for="template in productGroup.templates"
                        :key="template.stageTemplateId"
                        class="space-y-3"
                      >
                        <div
                          v-for="stage in template.stages"
                          :key="`${productGroup.product.productId}-${stage.stageId}`"
                          class="rounded-lg border border-gray-200 bg-white p-4"
                        >
                          <div class="flex items-center justify-between gap-4">
                            <div class="w-full">
                              <p class="font-semibold text-gray-700">{{ stage.stageName }}</p>
                            </div>
                            <div class="flex items-center gap-2">
                              <button
                                class="btn-update"
                                @click="toggleStageCollapse(`${productGroup.product.productId}-${stage.stageId}`)"
                              >
                                {{
                                  isStageCollapsed(`${productGroup.product.productId}-${stage.stageId}`)
                                    ? "展開"
                                    : "收合"
                                }}
                              </button>
                            </div>
                          </div>
                          <div
                            v-if="!isStageCollapsed(`${productGroup.product.productId}-${stage.stageId}`)"
                          >
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
                                提醒：{{ stage.reminderDays.join(" / ") }} 天（{{
                                  stage.reminderAnchor === "end" ? "迄日" : "起日"
                                }}）
                              </span>
                              <span
                                v-if="Number.isFinite(stage.estimatedDays)"
                                class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                              >
                                人天：{{ stage.estimatedDays }}{{
                                  stage.requireEstimatedDays ? "（必填）" : ""
                                }}
                              </span>
                              <span
                                v-if="Number.isFinite(stage.estimatedHours)"
                                class="rounded-full bg-emerald-50 px-2 py-0.5 text-xs text-emerald-700"
                              >
                                時數：{{ stage.estimatedHours }}{{
                                  stage.requireEstimatedHours ? "（必填）" : ""
                                }}
                              </span>
                            </div>
                            <div class="mt-3">
                              <div class="flex items-center justify-between">
                                <p class="text-sm font-semibold text-gray-600">產出清單</p>
                              </div>
                              <div
                                v-if="stage.requiredOutputs.length === 0"
                                class="text-sm text-gray-500 mt-2"
                              >
                                尚未設定產出
                              </div>
                              <div v-else class="mt-2 space-y-2">
                                <div
                                  v-for="output in stage.requiredOutputs"
                                  :key="output.outputId"
                                  class="flex flex-col gap-2 rounded-md border border-gray-200 bg-white px-4 py-3 md:flex-row md:items-center md:justify-between"
                                >
                                  <div class="flex flex-1 items-start gap-3">
                                    <div class="w-full">
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
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="flex justify-center mt-3 gap-2">
          <button class="btn-cancel" @click="goToPrevStep">上一步</button>
          <button class="btn-submit" @click="goToNextStep">下一步</button>
        </div>
      </div>
    </div>
    <!--人員列表-->
    <div v-if="currentStep === 3">
      <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
        <div class="flex items-center justify-between">
          <h2 class="subtitle mt-2">專案人員</h2>
        </div>
        <p class="text-xs text-gray-500 mt-1">
          系統會依區域與服務類型帶入預設人員，仍可自行調整。
        </p>
        <div class="space-y-6">
          <div class="rounded-lg border border-gray-200 bg-white p-4">
            <div class="flex items-center justify-between">
              <h3 class="subtitle text-gray-700">固定成員</h3>
            </div>
            <table class="table-base table-fixed table-sticky w-full mt-3">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start">所屬區域</th>
                  <th class="text-start">所屬部門</th>
                  <th class="text-start">專案人員</th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="fixedMembers.length === 0" class="text-center">
                  <td colspan="3">無資料</td>
                </tr>
                <tr v-for="item in fixedMembers" :key="item.employeeProjectMemberID">
                  <td class="text-start">
                    {{
                      item.managerRegionName ||
                      getRegionNameByCode(selectedProjectRegionCode) ||
                      employeeInfoStore.managerRegionName ||
                      "北區"
                    }}
                  </td>
                  <td class="text-start">
                    {{
                      normalizeDepartmentName(item.managerDepartmentName) ||
                      normalizeDepartmentName(employeeInfoStore.managerDepartmentName) ||
                      "工程部"
                    }}
                  </td>
                  <td class="text-start">{{ item.employeeName }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div
            v-for="group in serviceMemberGroups"
            :key="group.service.serviceItemId"
            class="rounded-lg border border-gray-200 bg-white p-4"
          >
            <div class="flex items-center justify-between">
              <h3 class="subtitle text-gray-700">{{ group.service.serviceItemName }}</h3>
            </div>
            <table class="table-base table-fixed table-sticky w-full mt-3">
              <thead class="bg-gray-800 text-white">
                <tr>
                  <th class="text-start">所屬區域</th>
                  <th class="text-start">所屬部門</th>
                  <th class="text-start">專案人員</th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="group.members.length === 0" class="text-center">
                  <td colspan="3">無資料</td>
                </tr>
                <tr
                  v-for="item in group.members"
                  :key="`${item.employeeProjectMemberID}-${item.serviceItemId}`"
                >
                  <td class="text-start">
                    <div class="combo-custom">
                      <div class="relative flex w-full">
                        <select
                          v-model="item.managerRegionName"
                          class="input-box select-reset"
                          @change="syncMemberRegion(item)"
                        >
                          <option
                            v-for="region in memberRegionOptions"
                            :key="region.code"
                            :value="region.regionName"
                          >
                            {{ region.regionName }}
                          </option>
                        </select>
                        <span class="select-icon">
                          <svg
                            class="w-4 h-4"
                            viewBox="0 0 20 25"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                              fill="#787676"
                            />
                          </svg>
                        </span>
                      </div>
                    </div>
                  </td>
                  <td class="text-start">
                    <div class="combo-custom">
                      <div class="relative flex w-full">
                        <select
                          v-model.number="item.managerDepartmentID"
                          class="input-box select-reset"
                          @change="syncMemberDepartment(item)"
                        >
                          <option
                            v-for="dept in filterDepartmentsByRegion(item.managerRegionName, item.serviceItemName)"
                            :key="dept.id"
                            :value="dept.id"
                          >
                            {{ dept.name }}
                          </option>
                        </select>
                        <span class="select-icon">
                          <svg
                            class="w-4 h-4"
                            viewBox="0 0 20 25"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
                              fill="#787676"
                            />
                          </svg>
                        </span>
                      </div>
                    </div>
                  </td>
                  <td class="text-start">
                    <GetManyEmployeeComboBox
                      v-model:managerEmployeeID="item.employeeProjectMemberID"
                      v-model:managerEmployeeName="item.employeeName"
                      v-model:managerRegionID="item.managerRegionID"
                      v-model:managerDepartmentID="item.managerDepartmentID"
                      v-model:managerRegionName="item.managerRegionName"
                      v-model:managerDepartmentName="item.managerDepartmentName"
                      :managerRoleID="null"
                      :disabled="false"
                      placeholder="請選擇人員"
                    />
                  </td>
                </tr>
              </tbody>
            </table>
            <button
              type="button"
              class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30] mt-3"
              style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
              @click="addProjectMemberRow(group.service)"
            >
              新增專案人員
            </button>
          </div>
        </div>
      </div>

      <div class="flex justify-center mt-3 gap-2">
        <button class="btn-cancel" @click="goToPrevStep">上一步</button>
        <button
          v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-submit"
          @click="goToNextStep"
        >
          下一步
        </button>
      </div>
    </div>
  </div>

  <div v-if="currentStep === 4">
    <div class="flex flex-col bg-white rounded-lg p-8 gap-6">
      <div class="flex items-center justify-between">
        <h2 class="text-xl font-bold text-brand-100">專案預覽</h2>
      </div>
      <div v-if="createdProjectSummary" class="grid gap-4">
        <div class="rounded-lg border border-gray-200 p-4">
          <h3 class="subtitle text-gray-700">基本資訊</h3>
          <div class="mt-3 grid grid-cols-2 gap-4 text-sm text-gray-700">
            <div>客戶所在地：{{ createdProjectSummary.regionLabel }}</div>
            <div>專案代碼：{{ createdProjectSummary.projectCode }}</div>
            <div>專案名稱：{{ createdProjectSummary.projectName }}</div>
            <div>客戶：{{ createdProjectSummary.companyName }}</div>
            <div>開始日期：{{ createdProjectSummary.startTime }}</div>
            <div>結束日期：{{ createdProjectSummary.endTime }}</div>
          </div>
        </div>

        <div class="rounded-lg border border-gray-200 p-4">
          <h3 class="subtitle text-gray-700">類型與服務</h3>
          <div class="mt-3 text-sm text-gray-700">
            <div>專案類型：{{ createdProjectSummary.projectTypeName }}</div>
            <div class="mt-2">服務項目：{{ createdProjectSummary.serviceItems.join("、") }}</div>
            <div class="mt-2">產品：{{ createdProjectSummary.products.join("、") }}</div>
          </div>
        </div>

        <div class="rounded-lg border border-gray-200 p-4">
          <h3 class="subtitle text-gray-700">專案人員</h3>
          <div class="mt-3 space-y-2 text-sm text-gray-700">
            <div
              v-for="member in createdProjectSummary.members"
              :key="`${member.roleLabel}-${member.employeeName}`"
              class="flex flex-wrap gap-2"
            >
              <span class="font-semibold">{{ member.roleLabel }}</span>
              <span>{{ member.regionName }}</span>
              <span>{{ member.departmentName }}</span>
              <span>{{ member.employeeName }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="flex justify-center mt-3 gap-2">
      <button class="btn-cancel" @click="goToPrevStep">上一步</button>
      <button class="btn-submit" @click="clickSubmitProjectBtn">建立專案</button>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

  <!-- 彈跳視窗:確認儲存  -->
  <ConfirmDialog
    :show="showConfirmDialog"
    title="確認刪除"
    message="確定要刪除人員嗎？"
    confirm-text="確定"
    cancel-text="取消"
    @confirm="confirmSave"
    @cancel="cancelSave"
  />

  <!-- 人員新增 Modal-->
  <div v-if="isShowAddMemberModal" class="modal-overlay">
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">附加人員</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="isShowAddMemberModal = false"
        >
          x
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-6 space-y-4">
        <div class="flex gap-4 w-full mt-3">
          <!-- 地區 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">地區</label>
            <GetManyManagerRegionComboBox
              v-model:managerRegionID="wrkProjectEmployeeMemberAddItemObj.managerRegionID"
              v-model:managerRegionName="wrkProjectEmployeeMemberAddItemObj.managerRegionName"
              :disabled="false"
              placeholder="請選擇地區"
              @change="resetError"
            />
          </div>

          <!-- 部門 -->
          <div class="flex flex-col gap-2 flex-1">
            <label class="form-label">部門</label>
            <GetManyManagerDepartmentComboBox
              v-model:managerDepartmentID="wrkProjectEmployeeMemberAddItemObj.managerDepartmentID"
              v-model:managerDepartmentName="
                wrkProjectEmployeeMemberAddItemObj.managerDepartmentName
              "
              :disabled="false"
              placeholder="請選擇部門"
              @change="resetError"
            />
          </div>
        </div>

        <!-- 人員 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">人員 <span class="required-label">*</span></label>
          <GetManyEmployeeComboBox
            v-model:managerEmployeeID="wrkProjectEmployeeMemberAddItemObj.employeeProjectMemberID"
            v-model:managerEmployeeName="wrkProjectEmployeeMemberAddItemObj.employeeName"
            v-model:managerRegionID="wrkProjectEmployeeMemberAddItemObj.managerRegionID"
            v-model:managerDepartmentID="wrkProjectEmployeeMemberAddItemObj.managerDepartmentID"
            v-model:managerRegionName="wrkProjectEmployeeMemberAddItemObj.managerRegionName"
            v-model:managerDepartmentName="wrkProjectEmployeeMemberAddItemObj.managerDepartmentName"
            :managerRoleID="null"
            :disabled="false"
            placeholder="請選擇人員"
            @change="resetError"
          />
          <div class="error-wrapper">
            <p
              v-if="!wrkProjectEmployeeMemberAddItemValidateObj.employeeProjectMemberID"
              class="error-tip"
            >
              不可為空
            </p>
          </div>
        </div>
      </div>

      <!-- 底部按鈕 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="isShowAddMemberModal = false">取消</button>
        <button class="btn-submit" @click="clickSubmitEmployeeMemberBtn">送出</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.project-add-font {
  font-family: "Microsoft JhengHei", "PingFang TC", "Heiti TC", sans-serif;
}

.combo-custom {
  position: relative;
}

.select-reset {
  appearance: none;
  background-image: none;
  padding-right: 2.25rem;
}

.combo-custom :deep(.input-box) {
  padding-right: 2.25rem;
  border-top-right-radius: 0.5rem;
  border-bottom-right-radius: 0.5rem;
}

.combo-custom :deep(button) {
  display: none !important;
}

.combo-custom .select-icon {
  position: absolute;
  right: 0.75rem;
  top: 50%;
  transform: translateY(-50%);
  pointer-events: none;
  color: #787676;
  display: inline-flex;
  align-items: center;
}

.date-picker-custom :deep(.dp__input_wrap) {
  width: 100%;
}

.date-picker-custom :deep(.dp__input) {
  border-radius: 0.5rem;
  border: 1px solid #e5e7eb;
  min-height: 42px;
  padding-left: 0.75rem;
  font-size: 0.875rem;
  color: #111827;
}

.date-picker-custom :deep(.dp__input:focus) {
  border-color: #38bdf8;
  box-shadow: 0 0 0 2px rgba(56, 189, 248, 0.2);
}

.date-picker-custom :deep(.dp__input:hover:not(.dp__input_focus)) {
  border-color: #94a3b8;
}

.date-picker-custom :deep(.dp__input_icon),
.date-picker-custom :deep(.dp--clear-btn) {
  display: none;
}
</style>
