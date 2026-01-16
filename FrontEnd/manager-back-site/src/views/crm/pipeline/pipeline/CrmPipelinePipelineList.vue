<script setup lang="ts">
//#region 引入
import { computed, reactive, onMounted, ref, watch } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsCrmPplHttpGetManyEmployeePipelineReqMdl,
  MbsCrmPplHttpGetManyEmployeePipelineRspMdl,
  MbsCrmPplHttpGetManyEmployeePipelineRspItemMdl,
} from "@/services/pms-http/crm/pipeline/crmPipelineHttpFormat";
import { getManyEmployeePipeline } from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { formatDate } from "@/utils/timeFormatter";
import router from "@/router";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import CrmPipelinePipelineAddModal from "./components/CrmPipelinePipelineAddModal.vue";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
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
const isShowAddPipelineModal = ref(false);
//#endregion

const allowedPipelineStatuses = new Set<DbAtomPipelineStatusEnum>([
  DbAtomPipelineStatusEnum.TransferredToBusiness,
  DbAtomPipelineStatusEnum.Business10Percent,
  DbAtomPipelineStatusEnum.Business30Percent,
  DbAtomPipelineStatusEnum.Business70Percent,
  DbAtomPipelineStatusEnum.Business100Percent,
  DbAtomPipelineStatusEnum.BusinessFailed,
  DbAtomPipelineStatusEnum.TransferredToProject,
]);

//#region 型別定義
/** CRM-商機管理-名單-列表-查詢模型 */
interface CrmPipelinePipelineListQueryMdl {
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  /** 管理者公司名稱 */
  managerCompanyName: string | null;
  /** 業務區域 */
  managerRegionName: string | null;
  /** 業務名稱 */
  employeePipelineSalerEmployeeName: string | null;
  /** 頁面索引 */
  pageIndex: number;
  /** 頁面筆數 */
  pageSize: number;
}

/** CRM-商機管理-名單-列表-項目模型 */
interface CrmPipelinePipelineListItemMdl {
  /** 商機ID */
  employeePipelineID: number;
  /** 商機狀態 */
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  /** 管理公司名稱 */
  managerCompanyName: string;
  /** 負責業務 */
  employeePipelineSalerEmployeeName: string;
  /** 負責業務區域 */
  employeePipelineSalerRegionName: string | null;
  /** 專案類型 */
  projectTypeName?: string | null;
  /** 案別 */
  isRenewal?: boolean | null;
  /** 是否委外 */
  isOutsourced?: boolean | null;
  /** 毛利 */
  grossMargin?: number | null;
  /** 初次商機日期 */
  initialPipelineDate?: string | null;
}

/** CRM-商機管理-名單-列表-頁面模型 */
interface CrmPipelinePipelineListViewMdl {
  /** 查詢條件 */
  query: CrmPipelinePipelineListQueryMdl;
  /** 商機列表 */
  employeePipelineList: CrmPipelinePipelineListItemMdl[];
  /** 總筆數 */
  totalCount: number;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmPipeline;

/** CRM-商機管理-名單-列表-頁面物件 */
const crmPipelinePipelineListViewObj = reactive<CrmPipelinePipelineListViewMdl>({
  query: {
    atomPipelineStatus: null,
    managerCompanyName: null,
    managerRegionName: null,
    employeePipelineSalerEmployeeName: null,
    pageIndex: 1,
    pageSize: 10,
  },
  employeePipelineList: [],
  totalCount: 0,
});

const isGeneralManager = computed(() => employeeInfoStore.effectiveRoleName === "總經理");
const isSalesRole = computed(() => (employeeInfoStore.effectiveRoleName || "").includes("業務"));
const currentRegionName = computed(() => employeeInfoStore.managerRegionName || "");

const salerNameOptions = computed(() => {
  const names = orgMemberDirectory.map((member) => member.name);
  return Array.from(new Set(names)).sort();
});

const getSalerRegionName = (name: string) => {
  const match = orgMemberDirectory.find((member) => member.name === name);
  return match?.regionName || null;
};

const selectedPipelineIds = ref<number[]>([]);
const batchAssigneeName = ref<string>("");
const batchRegionName = ref<string>("");
const batchRegionOptions = ["北區", "中區", "南區", "跨區"];

const selectedItems = computed(() =>
  crmPipelinePipelineListViewObj.employeePipelineList.filter((item) =>
    selectedPipelineIds.value.includes(item.employeePipelineID)
  )
);

const selectedAssigneeSummary = computed(() => {
  const names = Array.from(new Set(selectedItems.value.map((item) => item.employeePipelineSalerEmployeeName)));
  return names.length ? names.join("、") : "-";
});

const selectedRegionSummary = computed(() => {
  const regions = Array.from(
    new Set(
      selectedItems.value
        .map((item) => item.employeePipelineSalerRegionName)
        .filter((name): name is string => Boolean(name))
    )
  );
  return regions.length === 1 ? regions[0] : "";
});

const batchAssigneeOptions = computed(() => {
  const region = batchRegionName.value;
  const list =
    region && region !== "跨區"
      ? orgMemberDirectory.filter((member) => member.regionName === region)
      : orgMemberDirectory;
  const names = list.map((member) => member.name);
  return Array.from(new Set(names)).sort();
});

const toggleSelectAll = () => {
  if (selectedPipelineIds.value.length === crmPipelinePipelineListViewObj.employeePipelineList.length) {
    selectedPipelineIds.value = [];
    return;
  }
  selectedPipelineIds.value = crmPipelinePipelineListViewObj.employeePipelineList.map(
    (item) => item.employeePipelineID
  );
};

const toggleSelectItem = (id: number) => {
  if (selectedPipelineIds.value.includes(id)) {
    selectedPipelineIds.value = selectedPipelineIds.value.filter((itemId) => itemId !== id);
  } else {
    selectedPipelineIds.value = [...selectedPipelineIds.value, id];
  }
};

watch(
  () => selectedPipelineIds.value,
  () => {
    const autoRegion = selectedRegionSummary.value;
    batchRegionName.value = autoRegion || "";
    batchAssigneeName.value = "";
  },
  { deep: true }
);

watch(
  () => batchRegionName.value,
  () => {
    batchAssigneeName.value = "";
  }
);

const applyBatchAssignee = () => {
  if (!batchAssigneeName.value) return;
  const regionName =
    batchRegionName.value && batchRegionName.value !== "跨區"
      ? batchRegionName.value
      : getSalerRegionName(batchAssigneeName.value);
  crmPipelinePipelineListViewObj.employeePipelineList = crmPipelinePipelineListViewObj.employeePipelineList.map(
    (item) => {
      if (!selectedPipelineIds.value.includes(item.employeePipelineID)) return item;
      return {
        ...item,
        employeePipelineSalerEmployeeName: batchAssigneeName.value,
        employeePipelineSalerRegionName: regionName,
      };
    }
  );
  selectedPipelineIds.value = [];
  batchAssigneeName.value = "";
  batchRegionName.value = "";
};
//#endregion

//#region API / 資料流程
/** 取得列表 */
const getList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmPplHttpGetManyEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    atomPipelineStatus: crmPipelinePipelineListViewObj.query.atomPipelineStatus,
    managerCompanyName: crmPipelinePipelineListViewObj.query.managerCompanyName,
    pageIndex: crmPipelinePipelineListViewObj.query.pageIndex,
    pageSize: crmPipelinePipelineListViewObj.query.pageSize,
  };
  const responseData: MbsCrmPplHttpGetManyEmployeePipelineRspMdl | null =
    await getManyEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 包裝資料
  let mappedList =
    responseData.employeePipelineList?.map(
      (item: MbsCrmPplHttpGetManyEmployeePipelineRspItemMdl) =>
        ({
          employeePipelineID: item.employeePipelineID,
          atomPipelineStatus: item.atomPipelineStatus,
          managerCompanyName: item.managerCompanyName,
          employeePipelineSalerEmployeeName: item.employeePipelineSalerEmployeeName || "-",
          employeePipelineSalerRegionName: getSalerRegionName(item.employeePipelineSalerEmployeeName),
          projectTypeName: null,
          isRenewal: null,
          isOutsourced: null,
          grossMargin: null,
          initialPipelineDate: null,
        }) satisfies CrmPipelinePipelineListItemMdl
    ) ?? [];

  const localRaw = localStorage.getItem("cache.crm.pipeline.list");
  if (localRaw) {
    try {
      const localList = JSON.parse(localRaw) as Array<{
        id: number;
        status: DbAtomPipelineStatusEnum;
        companyName: string;
        isRenewal?: boolean;
        isOutsourced?: boolean;
      }>;
      const localMapped = localList.map((item) => ({
        employeePipelineID: item.id,
        atomPipelineStatus: item.status,
        managerCompanyName: item.companyName,
        employeePipelineSalerEmployeeName: "-",
        employeePipelineSalerRegionName: null,
        projectTypeName: null,
        isRenewal: item.isRenewal ?? null,
        isOutsourced: item.isOutsourced ?? null,
        grossMargin: null,
        initialPipelineDate: null,
      }));
      const existing = new Set(mappedList.map((item) => item.employeePipelineID));
      localMapped.forEach((item) => {
        if (!existing.has(item.employeePipelineID)) {
          mappedList.unshift(item);
        }
      });
    } catch (error) {
      console.warn("[CRM Pipeline] Failed to parse local cache list", error);
    }
  }

  const filteredList = mappedList.filter((item) =>
    allowedPipelineStatuses.has(item.atomPipelineStatus)
  );
  mappedList = filteredList;

  if (isSalesRole.value && !isGeneralManager.value && currentRegionName.value) {
    mappedList = mappedList.filter((item) => item.employeePipelineSalerRegionName === currentRegionName.value);
  }

  if (isGeneralManager.value) {
    if (crmPipelinePipelineListViewObj.query.managerRegionName) {
      mappedList = mappedList.filter(
        (item) =>
          item.employeePipelineSalerRegionName ===
          crmPipelinePipelineListViewObj.query.managerRegionName
      );
    }
    if (crmPipelinePipelineListViewObj.query.employeePipelineSalerEmployeeName) {
      mappedList = mappedList.filter(
        (item) =>
          item.employeePipelineSalerEmployeeName ===
          crmPipelinePipelineListViewObj.query.employeePipelineSalerEmployeeName
      );
    }
  }

  crmPipelinePipelineListViewObj.employeePipelineList = mappedList;
  crmPipelinePipelineListViewObj.totalCount = mappedList.length;
  selectedPipelineIds.value = [];
};
//#endregion

//#region 按鈕事件
/** 點擊【查詢】按鈕 */
const clickSearchBtn = () => {
  crmPipelinePipelineListViewObj.query.pageIndex = 1;
  getList();
};

/** 點擊【清除】按鈕 */
const clickClearSearchBtn = () => {
  crmPipelinePipelineListViewObj.query.atomPipelineStatus = null;
  crmPipelinePipelineListViewObj.query.managerCompanyName = null;
  crmPipelinePipelineListViewObj.query.managerRegionName = null;
  crmPipelinePipelineListViewObj.query.employeePipelineSalerEmployeeName = null;
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  isShowAddPipelineModal.value = true;
};

/** 點擊檢視活動 */
const clickDetailBtn = (id: number) => {
  router.push(`/crm/pipeline/pipeline/detail/${id}`);
};

/** 點擊修改 */
const clickEditBtn = (id: number) => {
  router.push(`/crm/pipeline/pipeline/detail/${id}`);
};

const getPipelineStatusBadgeClass = (status: DbAtomPipelineStatusEnum) => {
  switch (status) {
    case DbAtomPipelineStatusEnum.Business10Percent:
      return "bg-sky-50 text-sky-700";
    case DbAtomPipelineStatusEnum.Business30Percent:
      return "bg-amber-50 text-amber-700";
    case DbAtomPipelineStatusEnum.Business70Percent:
      return "bg-orange-50 text-orange-700";
    case DbAtomPipelineStatusEnum.Business100Percent:
      return "bg-emerald-50 text-emerald-700";
    case DbAtomPipelineStatusEnum.BusinessFailed:
      return "bg-red-50 text-red-700";
    case DbAtomPipelineStatusEnum.TransferredToProject:
      return "bg-indigo-50 text-indigo-700";
    case DbAtomPipelineStatusEnum.TransferredToBusiness:
      return "bg-gray-50 text-gray-700";
    default:
      return "bg-gray-50 text-gray-700";
  }
};

const getPipelineStatusDisplayLabel = (status: DbAtomPipelineStatusEnum) => {
  switch (status) {
    case DbAtomPipelineStatusEnum.Business10Percent:
      return "10%";
    case DbAtomPipelineStatusEnum.Business30Percent:
      return "30%";
    case DbAtomPipelineStatusEnum.Business70Percent:
      return "70%";
    case DbAtomPipelineStatusEnum.Business100Percent:
      return "100%";
    case DbAtomPipelineStatusEnum.BusinessFailed:
      return "Fail";
    case DbAtomPipelineStatusEnum.TransferredToProject:
      return "成案";
    default:
      return "-";
  }
};
//#endregion

//#region 生命週期
onMounted(() => {
  getList();
});
//#endregion
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden gap-4 p-2">
    <!-- 標題列 -->
    <div v-once class="flex items-center justify-end"></div>

    <div
      class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-6 gap-4"
      data-annotation-scope="crm-pipeline-list"
      :aria-hidden="isShowAddPipelineModal ? 'true' : undefined"
    >
      <!-- 查詢條件 -->
      <div class="flex items-end gap-4">
        <div class="flex gap-2">
          <div>
            <select
              v-model="crmPipelinePipelineListViewObj.query.atomPipelineStatus"
              class="select-box"
            >
              <option :value="null">全部</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToBusiness">已轉業務</option>
              <option :value="DbAtomPipelineStatusEnum.Business10Percent">商機10%</option>
              <option :value="DbAtomPipelineStatusEnum.Business30Percent">商機30%</option>
              <option :value="DbAtomPipelineStatusEnum.Business70Percent">商機70%</option>
              <option :value="DbAtomPipelineStatusEnum.Business100Percent">商機100%</option>
              <option :value="DbAtomPipelineStatusEnum.BusinessFailed">商機失敗</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToProject">已轉專案</option>
            </select>
          </div>

          <div>
            <input
              v-model="crmPipelinePipelineListViewObj.query.managerCompanyName"
              type="text"
              class="input-box"
              placeholder="客戶名稱"
            />
          </div>

          <template v-if="isGeneralManager">
            <div>
              <select
                v-model="crmPipelinePipelineListViewObj.query.managerRegionName"
                class="select-box"
              >
                <option :value="null">全部區域</option>
                <option value="北區">北區</option>
                <option value="中區">中區</option>
                <option value="南區">南區</option>
              </select>
            </div>
            <div>
              <select
                v-model="crmPipelinePipelineListViewObj.query.employeePipelineSalerEmployeeName"
                class="select-box"
              >
                <option :value="null">全部業務</option>
                <option v-for="name in salerNameOptions" :key="name" :value="name">
                  {{ name }}
                </option>
              </select>
            </div>
          </template>
        </div>

        <div class="flex gap-1">
          <button class="btn-search" @click="clickSearchBtn">查詢</button>
          <button class="btn-cancel" @click="clickClearSearchBtn">清除</button>
        </div>
      </div>

      <hr />

      <div
        v-if="isGeneralManager && selectedPipelineIds.length > 0"
        class="flex items-center justify-between rounded-lg border border-dashed border-gray-300 bg-gray-50 px-4 py-3"
      >
        <div class="text-sm text-gray-600">
          已選 {{ selectedPipelineIds.length }} 筆｜負責人：{{ selectedAssigneeSummary }}
        </div>
        <div class="flex items-center gap-2">
          <select v-model="batchRegionName" class="select-box">
            <option value="">選擇區域</option>
            <option v-for="region in batchRegionOptions" :key="region" :value="region">
              {{ region }}
            </option>
          </select>
          <select v-model="batchAssigneeName" class="select-box">
            <option value="">選擇新負責人</option>
            <option v-for="name in batchAssigneeOptions" :key="name" :value="name">
              {{ name }}
            </option>
          </select>
          <button class="btn-update" type="button" @click="applyBatchAssignee">
            更改業務
          </button>
        </div>
      </div>

      <!-- 列表 -->
      <div class="flex-1 overflow-y-auto table-scrollable">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th v-if="isGeneralManager" class="text-center w-12">
                <input
                  type="checkbox"
                  class="h-4 w-4"
                  :checked="
                    crmPipelinePipelineListViewObj.employeePipelineList.length > 0 &&
                    selectedPipelineIds.length ===
                      crmPipelinePipelineListViewObj.employeePipelineList.length
                  "
                  @click.stop="toggleSelectAll"
                />
              </th>
              <th class="text-start w-48">客戶名稱</th>
              <th class="text-start w-32">負責人</th>
              <th class="text-start w-24">商機狀態</th>
              <th class="text-start w-32">專案類型</th>
              <th class="text-start w-20">案別</th>
              <th class="text-start w-20">委外</th>
              <th class="text-start w-24">毛利</th>
              <th class="text-start w-32">初次商機</th>
              <!-- 操作欄位移除 -->
            </tr>
          </thead>
          <tbody>
            <template v-if="crmPipelinePipelineListViewObj.employeePipelineList.length === 0">
              <tr class="text-center">
                <td :colspan="isGeneralManager ? 9 : 8">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="item in crmPipelinePipelineListViewObj.employeePipelineList"
                :key="item.employeePipelineID"
                class="cursor-pointer hover:bg-gray-50 transition-colors"
                @click="clickDetailBtn(item.employeePipelineID)"
              >
                <td v-if="isGeneralManager" class="text-center">
                  <input
                    type="checkbox"
                    class="h-4 w-4"
                    :checked="selectedPipelineIds.includes(item.employeePipelineID)"
                    @click.stop="toggleSelectItem(item.employeePipelineID)"
                  />
                </td>
                <td class="text-start">
                  {{ item.managerCompanyName }}
                </td>
                <td class="text-start">
                  {{ item.employeePipelineSalerEmployeeName || "-" }}
                </td>
                <td class="text-start">
                  <span
                    class="inline-flex min-w-[88px] items-center justify-center rounded-md px-2 py-1 text-sm"
                    :class="getPipelineStatusBadgeClass(item.atomPipelineStatus)"
                  >
                    {{ getPipelineStatusDisplayLabel(item.atomPipelineStatus) }}
                  </span>
                </td>
                <td class="text-start">
                  {{ item.projectTypeName || "-" }}
                </td>
                <td class="text-start">
                  {{ item.isRenewal === null ? "-" : item.isRenewal ? "續約" : "新案" }}
                </td>
                <td class="text-start">
                  {{ item.isOutsourced === null ? "-" : item.isOutsourced ? "委外" : "非委外" }}
                </td>
                <td class="text-start">
                  {{ typeof item.grossMargin === "number" ? `${item.grossMargin}%` : "-" }}
                </td>
                <td class="text-start">
                  {{ item.initialPipelineDate ? formatDate(item.initialPipelineDate) : "-" }}
                </td>
                <!-- 操作欄位移除 -->
              </tr>
            </template>
          </tbody>
        </table>
      </div>
      <button
        v-if="!isGeneralManager"
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="margin-top:3px;background-color:#F2F6F9;border-color:#082F49;"
        type="button"
        @click.prevent="clickAddBtn"
      >
        +新增商機
      </button>

      <div class="flex justify-center pt-3">
        <!-- 分頁 -->
        <Pagination
          :pageIndex="crmPipelinePipelineListViewObj.query.pageIndex"
          :pageSize="crmPipelinePipelineListViewObj.query.pageSize"
          :totalCount="crmPipelinePipelineListViewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              crmPipelinePipelineListViewObj.query.pageIndex = newPage;
              getList();
            }
          "
          @update:page-size="
            (newSize: number) => {
              crmPipelinePipelineListViewObj.query.pageSize = newSize;
              crmPipelinePipelineListViewObj.query.pageIndex = 1;
              getList();
            }
          "
        />
      </div>
    </div>
  </div>

  <div
    v-if="isShowAddPipelineModal"
    class="modal-overlay"
    @click.self="isShowAddPipelineModal = false"
  >
    <div class="h-[90vh] w-[90vw] max-w-[1100px] overflow-y-auto rounded-lg bg-white shadow-lg">
      <CrmPipelinePipelineAddModal
        @close="isShowAddPipelineModal = false"
        @created="getList()"
      />
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
</template>
