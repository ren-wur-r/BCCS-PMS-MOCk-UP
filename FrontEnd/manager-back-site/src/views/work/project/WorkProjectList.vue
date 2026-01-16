<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, defineAsyncComponent, computed, watch, onBeforeUnmount } from "vue";
// Enums / 常數
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
// Stores
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
// Services
import type {
  MbsWrkPrjHttpGetManyProjectReqMdl,
  MbsWrkPrjHttpGetManyProjectRspItemMdl,
  MbsWrkPrjHttpGetManyProjectRspMdl,
} from "@/services/pms-http/work/project/workProjectHttpFormat";
import { getManyProject } from "@/services";
// Utils
import { formatDate } from "@/utils/timeFormatter";
import { getEmployeeProjectStatusLabel } from "@/utils/getEmployeeProjectStatusLabel";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { loadProjectTemplateSettings, getStageTemplatesByServiceItems } from "@/stores/projectTemplateSettings";
// Components
const ErrorAlert = defineAsyncComponent(
  () => import("@/components/global/feedback/ErrorAlert.vue")
);
const Pagination = defineAsyncComponent(
  () => import("@/components/global/pagination/Pagination.vue")
);
const WorkProjectAdd = defineAsyncComponent(
  () => import("@/views/work/project/WorkProjectAdd.vue")
);
// Router
import router from "@/router";
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
//#endregion

//#region 型別定義
/** 工作-專案管理-列表-查詢模型 */
interface WorkPrjListQueryMdl {
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum | null;
  employeeProjectName: string;
  pageIndex: number;
  pageSize: number;
}

/** 工作-專案管理-列表-項目模型 */
interface WorkPrjListItemMdl {
  employeeProjectID: number;
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  employeeProjectName: string;
  managerCompanyName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
}

/** 工作-專案管理-列表-頁面模型 */
interface WorkPrjListViewMdl {
  query: WorkPrjListQueryMdl;
  employeeProjectList: WorkPrjListItemMdl[];
  totalCount: number;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.WorkProject;
const statusTabs = [
  { label: "未開始", status: DbAtomEmployeeProjectStatusEnum.NotStarted },
  { label: "進行中", status: DbAtomEmployeeProjectStatusEnum.OnSchedule },
  { label: "注意", status: DbAtomEmployeeProjectStatusEnum.AtRisk },
  { label: "延遲", status: DbAtomEmployeeProjectStatusEnum.Delayed },
] as const;
const activeStatusTab = ref(statusTabs[0]);
const listMode = ref<"project" | "poc">("project");
const statusCounts = ref<Record<number, number>>({});
const roleName = computed(() => employeeInfoStore.effectiveRoleName || "");
const isEngineerView = computed(() => roleName.value.includes("工程師"));
const templateSettings = loadProjectTemplateSettings();
const projectTypeLabelMap = new Map(
  templateSettings.projectTypes.map((item) => [item.projectTypeId, item.projectTypeName])
);
const serviceItemMap = new Map(
  templateSettings.serviceItems.map((item) => [item.serviceItemId, item])
);
const readProjectFlags = (projectId: number) => {
  const raw = localStorage.getItem(`workProjectFlags:${projectId}`);
  if (raw) {
    try {
      return JSON.parse(raw) as { isRenewal?: boolean; isOutsourced?: boolean };
    } catch {
      return null;
    }
  }
  const fallback = mockDataSets.workProjects.find((item) => item.id === projectId);
  if (!fallback) return null;
  return { isRenewal: Boolean(fallback.isRenewal), isOutsourced: Boolean(fallback.isOutsourced) };
};
const getRenewalLabel = (projectId: number) => {
  const flags = readProjectFlags(projectId);
  if (!flags || typeof flags.isRenewal !== "boolean") return "-";
  return flags.isRenewal ? "續約" : "新案";
};
const getOutsourcedLabel = (projectId: number) => {
  const flags = readProjectFlags(projectId);
  if (!flags || typeof flags.isOutsourced !== "boolean") return "-";
  return flags.isOutsourced ? "委外" : "非委外";
};
const readDashboardProjectLimit = () => {
  const raw = localStorage.getItem("cache.dashboard.getInfo");
  if (!raw) return null;
  try {
    const parsed = JSON.parse(raw) as { PersonalProjectCount?: number };
    if (typeof parsed.PersonalProjectCount === "number") return parsed.PersonalProjectCount;
  } catch {
    return null;
  }
  return null;
};

/** 工作-專案管理-列表-頁面物件 */
const workProjectListViewObj = reactive<WorkPrjListViewMdl>({
  query: {
    atomEmployeeProjectStatus: statusTabs[0].status,
    employeeProjectName: "",
    pageIndex: 1,
    pageSize: 10,
  },
  employeeProjectList: [],
  totalCount: 0,
});
const isShowAddProjectModal = ref(false);
const pocRefreshTick = ref(0);
//#endregion

//#region UI行為 / 畫面處理
const canViewProject = computed(() =>
  employeeInfoStore.hasEveryonePermission(menu, "view")
);
const readPipelineList = () => {
  const raw = localStorage.getItem("cache.crm.pipeline.list");
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const getPipelineDetail = (pipelineId: number) => {
  const raw = localStorage.getItem(`cache.crm.pipeline.detail.${pipelineId}`);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as {
      bookingCode?: string;
      companyName?: string;
      status?: DbAtomPipelineStatusEnum;
      serviceItemIds?: number[];
      serviceProductIds?: Record<number, number[]>;
    };
  } catch {
    return null;
  }
};

const getStageSummary = (serviceItemIds: number[]) => {
  const templates = getStageTemplatesByServiceItems(templateSettings, serviceItemIds);
  const stages = templates.flatMap((template) => template.stages);
  return {
    current: stages[0]?.stageName ?? "-",
    next: stages[1]?.stageName ?? "-",
  };
};

const shouldIncludePocForRole = (assignments: { employeeName?: string; departmentName?: string }[]) => {
  const currentName = employeeInfoStore.employeeName || "";
  if (currentName && assignments.some((item) => item.employeeName === currentName)) {
    return true;
  }
  const currentDept = employeeInfoStore.managerDepartmentName || "";
  if (
    currentDept &&
    assignments.some((item) =>
      item.departmentName ? item.departmentName.includes(currentDept) || currentDept.includes(item.departmentName) : false
    )
  ) {
    return true;
  }
  const role = employeeInfoStore.effectiveRoleName || "";
  if (role.includes("工程")) {
    return assignments.some((item) => item.departmentName?.includes("工程"));
  }
  if (role.includes("顧問")) {
    return assignments.some((item) => item.departmentName?.includes("顧問"));
  }
  return true;
};

const pocList = computed(() => {
  pocRefreshTick.value;
  const list = readPipelineList();
  const result: {
    pipelineId: number;
    name: string;
    status: DbAtomPipelineStatusEnum | null;
    serviceItems: string;
    products: string;
    startDate: string;
    endDate: string;
    currentStage: string;
    nextStage: string;
    assignments: { employeeName?: string; departmentName?: string }[];
  }[] = [];
  Object.keys(localStorage).forEach((key) => {
    if (!key.startsWith("cache.crm.pipeline.poc.")) return;
    const id = Number(key.split(".").pop());
    if (!Number.isFinite(id)) return;
    const raw = localStorage.getItem(key);
    if (!raw) return;
    try {
      const parsed = JSON.parse(raw) as {
        form?: { startDate?: string; endDate?: string };
        assignments?: { employeeName?: string; departmentName?: string }[];
      };
      const detail = getPipelineDetail(id);
      const listRecord = list.find((item: { id: number }) => item.id === id);
      const serviceItemIds = detail?.serviceItemIds ?? [];
      const serviceItemNames = serviceItemIds
        .map((itemId) => serviceItemMap.get(itemId)?.serviceItemName)
        .filter(Boolean);
      const productNames: string[] = [];
      if (detail?.serviceProductIds) {
        Object.entries(detail.serviceProductIds).forEach(([serviceId, productIds]) => {
          const service = serviceItemMap.get(Number(serviceId));
          if (!service) return;
          service.products
            .filter((product) => productIds.includes(product.productId))
            .forEach((product) => productNames.push(product.productName));
        });
      }
      const stageSummary = getStageSummary(serviceItemIds);
      const assignments = Array.isArray(parsed.assignments) ? parsed.assignments : [];
      if (!shouldIncludePocForRole(assignments)) return;
      result.push({
        pipelineId: id,
        name: detail?.bookingCode || listRecord?.companyName || detail?.companyName || "-",
        status: detail?.status ?? listRecord?.status ?? null,
        serviceItems: serviceItemNames.length > 0 ? serviceItemNames.join("、") : "-",
        products: productNames.length > 0 ? productNames.join("、") : "-",
        startDate: parsed.form?.startDate || "-",
        endDate: parsed.form?.endDate || "-",
        currentStage: stageSummary.current,
        nextStage: stageSummary.next,
        assignments,
      });
    } catch (error) {
      console.warn("[Work Project] Failed to parse POC list item", error);
    }
  });

  return result;
});
const getProjectTypeLabel = (projectId: number) => {
  const raw = localStorage.getItem(`workProjectProjectType:${projectId}`);
  if (!raw) return "-";
  const projectTypeId = Number(raw);
  return projectTypeLabelMap.get(projectTypeId) ?? "-";
};

/** 取得指派業務狀態樣式 */
const getStatusClass = (status: DbAtomEmployeeProjectStatusEnum | null) => {
  switch (status) {
    case DbAtomEmployeeProjectStatusEnum.NotAssigned:
      return "bg-slate-50 text-slate-700";
    case DbAtomEmployeeProjectStatusEnum.NotStarted:
      return "bg-stone-50 text-stone-700";
    case DbAtomEmployeeProjectStatusEnum.Completed:
      return "bg-gray-50 text-gray-700";
    case DbAtomEmployeeProjectStatusEnum.OnSchedule:
      return "bg-green-50 text-green-700";
    case DbAtomEmployeeProjectStatusEnum.AtRisk:
      return "bg-yellow-50 text-yellow-700";
    case DbAtomEmployeeProjectStatusEnum.Delayed:
      return "bg-red-50 text-red-700";
    default:
      return "bg-gray-50 text-gray-700";
  }
};
//#endregion

//#region API / 資料流程
/** 取得【專案管理】列表 */
const getWorkProjectList = async () => {
  if (!requireToken()) return;

  //呼叫api
  const requestData: MbsWrkPrjHttpGetManyProjectReqMdl = {
    employeeLoginToken: tokenStore.token!,
    atomEmployeeProjectStatus: workProjectListViewObj.query.atomEmployeeProjectStatus,
    employeeProjectName: workProjectListViewObj.query.employeeProjectName,
    pageIndex: workProjectListViewObj.query.pageIndex,
    pageSize: workProjectListViewObj.query.pageSize,
  };

  const responseData: MbsWrkPrjHttpGetManyProjectRspMdl | null = await getManyProject(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  //設定資料
  let mappedList =
    responseData.employeeProjectList?.map(
      (item: MbsWrkPrjHttpGetManyProjectRspItemMdl) =>
        ({
          employeeProjectID: item.employeeProjectID,
          atomEmployeeProjectStatus: item.atomEmployeeProjectStatus,
          employeeProjectName: item.employeeProjectName,
          managerCompanyName: item.managerCompanyName,
          employeeProjectStartTime: item.employeeProjectStartTime,
          employeeProjectEndTime: item.employeeProjectEndTime,
        }) satisfies WorkPrjListItemMdl
    ) ?? [];

  if (mappedList.length === 0) {
    mappedList = mockDataSets.workProjects.map((item) => ({
      employeeProjectID: item.id,
      atomEmployeeProjectStatus: item.status,
      employeeProjectName: item.name,
      managerCompanyName: item.companyName,
      employeeProjectStartTime: item.startTime,
      employeeProjectEndTime: item.endTime,
    }));
  }

  if (isEngineerView.value) {
    const limit = readDashboardProjectLimit();
    if (limit !== null) {
      mappedList = mappedList.slice(0, Math.max(0, limit));
    }
  }

  const baseList = [...mappedList];
  const counts: Record<number, number> = {};
  for (const item of baseList) {
    const key = item.atomEmployeeProjectStatus as number;
    counts[key] = (counts[key] ?? 0) + 1;
  }
  statusCounts.value = counts;

  const selectedStatus = workProjectListViewObj.query.atomEmployeeProjectStatus;
  if (selectedStatus !== null) {
    const filtered = baseList.filter(
      (item) => item.atomEmployeeProjectStatus === selectedStatus
    );
    if (filtered.length !== mappedList.length) {
      mappedList = filtered;
      workProjectListViewObj.totalCount = filtered.length;
    } else {
      workProjectListViewObj.totalCount = isEngineerView.value
        ? mappedList.length
        : responseData.totalCount;
    }
  } else {
    workProjectListViewObj.totalCount = isEngineerView.value
      ? mappedList.length
      : responseData.totalCount;
  }

  workProjectListViewObj.employeeProjectList = mappedList;
};
//#endregion

//#region 按鈕事件
/** 點擊【檢視】按鈕 */
const clickDetailBtn = (employeeProjectID: number) => {
  router.push(`/work/project/detail/${employeeProjectID}`);
};

const clickPocListRow = (pipelineId: number) => {
  router.push(`/work/project/poc/${pipelineId}`);
};

const handleRowClick = (employeeProjectID: number) => {
  if (!canViewProject.value) return;
  clickDetailBtn(employeeProjectID);
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  isShowAddProjectModal.value = true;
};

const setStatusTab = (tab: (typeof statusTabs)[number]) => {
  activeStatusTab.value = tab;
  workProjectListViewObj.query.atomEmployeeProjectStatus = tab.status;
  workProjectListViewObj.query.pageIndex = 1;
  getWorkProjectList();
};

const handleProjectCreated = (payload: {
  employeeProjectID: number | null;
  atomEmployeeProjectStatus: DbAtomEmployeeProjectStatusEnum;
  employeeProjectName: string;
  managerCompanyName: string;
  employeeProjectStartTime: string;
  employeeProjectEndTime: string;
}) => {
  if (!payload.employeeProjectName) return;
  workProjectListViewObj.query.pageIndex = 1;
  getWorkProjectList();
};
//#endregion

//#region 生命週期
onMounted(() => {
  workProjectListViewObj.query.atomEmployeeProjectStatus = activeStatusTab.value.status;
  getWorkProjectList();
  window.addEventListener("crm-poc-updated", () => {
    pocRefreshTick.value += 1;
  });
});

watch(
  () => listMode.value,
  (mode) => {
    setModuleTitle(mode === "poc" ? "POC" : "專案");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});
//#endregion
</script>

<template>
  <div class="flex flex-col h-full p-2">
    <div class="flex items-center justify-between mb-3">
      <div class="flex gap-2">
        <button
          class="tab-btn"
          :class="listMode === 'project' ? 'active' : ''"
          @click="listMode = 'project'"
        >
          專案
        </button>
        <button
          class="tab-btn"
          :class="listMode === 'poc' ? 'active' : ''"
          @click="listMode = 'poc'"
        >
          POC
        </button>
      </div>
      <div v-if="listMode === 'project'" class="flex gap-2">
        <button
          v-for="tab in statusTabs"
          :key="tab.label"
          class="tab-btn"
          :class="activeStatusTab.label === tab.label ? 'active' : ''"
          @click="setStatusTab(tab)"
        >
          {{ tab.label }} ({{ statusCounts[tab.status] ?? 0 }})
        </button>
      </div>
    </div>

    <div class="flex flex-col bg-white rounded-lg p-6 gap-3 flex-1">
      <div class="flex-1">
        <table v-if="listMode === 'project'" data-annotation-scope="project" class="table-base table-fixed table-sticky w-full min-w-[720px]">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-40">專案名稱</th>
              <th class="text-start w-36">客戶</th>
              <th class="text-start w-28">專案類型</th>
              <th class="text-start w-20">案別</th>
              <th class="text-start w-20">委外</th>
              <th class="text-start w-24">起始日期</th>
              <th class="text-start w-24">結束日期</th>
            </tr>
          </thead>
          <tbody>
            <template v-if="workProjectListViewObj.employeeProjectList.length === 0">
              <tr class="text-center">
                <td colspan="7">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="item in workProjectListViewObj.employeeProjectList"
                :key="item.employeeProjectID"
                :class="canViewProject ? 'cursor-pointer hover:bg-cyan-50' : ''"
                :role="canViewProject ? 'button' : undefined"
                :tabindex="canViewProject ? 0 : undefined"
                @click="handleRowClick(item.employeeProjectID)"
                @mousedown.left="handleRowClick(item.employeeProjectID)"
                @keydown.enter="handleRowClick(item.employeeProjectID)"
              >
                <td class="text-start">
                  {{ item.employeeProjectName || "-" }}
                </td>
                <td class="text-start">
                  {{ item.managerCompanyName || "-" }}
                </td>
                <td class="text-start">
                  {{ getProjectTypeLabel(item.employeeProjectID) }}
                </td>
                <td class="text-start">
                  {{ getRenewalLabel(item.employeeProjectID) }}
                </td>
                <td class="text-start">
                  {{ getOutsourcedLabel(item.employeeProjectID) }}
                </td>
                <td class="text-start">
                  {{ formatDate(item.employeeProjectStartTime) ?? "-" }}
                </td>
                <td class="text-start">
                  {{ formatDate(item.employeeProjectEndTime) || "-" }}
                </td>
              </tr>
            </template>
          </tbody>
        </table>

        <table v-else data-annotation-scope="poc" class="table-base table-fixed table-sticky w-full min-w-[960px]">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-40">商機名稱</th>
              <th class="text-start w-24">商機狀態</th>
              <th class="text-start w-32">服務項目</th>
              <th class="text-start w-40">產品</th>
              <th class="text-start w-32">POC 起迄</th>
              <th class="text-start w-24">目前階段</th>
              <th class="text-start w-24">下一階段</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="pocList.length === 0">
              <td colspan="7" class="text-center text-gray-400 py-6">尚未建立 POC</td>
            </tr>
            <tr
              v-for="item in pocList"
              :key="item.pipelineId"
              class="cursor-pointer hover:bg-cyan-50 transition-colors"
              @click="clickPocListRow(item.pipelineId)"
              @mousedown.left="clickPocListRow(item.pipelineId)"
            >
              <td class="text-start">{{ item.name }}</td>
              <td class="text-start">{{ item.status ? getPipelineStatusLabel(item.status) : "-" }}</td>
              <td class="text-start">{{ item.serviceItems }}</td>
              <td class="text-start">{{ item.products }}</td>
              <td class="text-start">{{ item.startDate }} ~ {{ item.endDate }}</td>
              <td class="text-start">{{ item.currentStage }}</td>
              <td class="text-start">{{ item.nextStage }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <button
        v-if="listMode === 'project' && employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="background-color:#F2F6F9;border-color:#082F49;"
        @click="clickAddBtn"
      >
        +新增專案
      </button>

      <div v-if="listMode === 'project'" class="flex justify-center pt-2">
        <Pagination
          :pageIndex="workProjectListViewObj.query.pageIndex"
          :pageSize="workProjectListViewObj.query.pageSize"
          :totalCount="workProjectListViewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              workProjectListViewObj.query.pageIndex = newPage;
              getWorkProjectList();
            }
          "
          @update:page-size="
            (newSize: number) => {
              workProjectListViewObj.query.pageSize = newSize;
              workProjectListViewObj.query.pageIndex = 1;
              getWorkProjectList();
            }
          "
        />
      </div>
    </div>
  </div>
  <div v-if="isShowAddProjectModal" class="modal-overlay" @click.self="isShowAddProjectModal = false">
    <div class="h-[92vh] w-[95vw] max-w-[1200px] overflow-y-auto rounded-lg bg-white shadow-lg">
      <WorkProjectAdd
        isModal
        @close="isShowAddProjectModal = false"
        @created="handleProjectCreated"
      />
    </div>
  </div>
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
</template>
