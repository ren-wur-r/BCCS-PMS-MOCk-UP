<script setup lang="ts">
import { computed, reactive, ref, onMounted, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import Pagination from "@/components/global/pagination/Pagination.vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { mockDataSets } from "@/services/mockApi/mockDataSets";

interface TeleSaleRecord {
  id: number;
  activityId: number;
  activityName: string;
  activityKind: string;
  activityDate: string;
  teleSalesRep: string;
  callTimeRange: string;
  lastEditedAt?: string;
  industry: string;
  evaluationRange: string;
  discussItem: string;
  interestLevel: string;
  note: string;
  listStatus: string;
  assignedSales: string;
  assignedRegion?: string;
  discussNote: string;
  companyName: string;
  contactName: string;
  contactDepartment: string;
  contactTitle: string;
  contactEmail: string;
  companyPhone: string;
  mobilePhone: string;
  contactAddress: string;
  customerData1: string;
  customerData2: string;
  customerData3: string;
  customerData4: string;
  groupName?: string;
  groupIndustry?: string;
}

interface LeadPoolItem {
  id: number;
  source: "活動轉電銷" | "電銷自建";
  activityId?: number;
  activityName?: string;
  companyName: string;
  contactName: string;
  contactDepartment: string;
  industry: string;
  evaluationRange: string;
  interestLevel: string;
  assignedSales: string;
  listStatus: string;
  customerValue?: string;
  callDate?: string;
  lastEditedAt?: string;
  groupName?: string;
  groupIndustry?: string;
}

interface CrmPhonePipelineListQueryMdl {
  pageIndex: number;
  pageSize: number;
}

interface CrmPhonePipelineListViewMdl {
  query: CrmPhonePipelineListQueryMdl;
  recordList: TeleSaleRecord[];
  totalCount: number;
}

const route = useRoute();
const router = useRouter();
const employeeInfoStore = useEmployeeInfoStore();
const menu = DbAtomMenuEnum.CrmPhone;
const managerActivityID = ref<number>(Number(route.params.activityId || 0));
const leadPoolStorageKey = "cache.crm.phone.leadPool";
const leadPoolAddedCountKey = "cache.crm.phone.leadPool.addedCount";

const canViewPipeline = computed(() =>
  employeeInfoStore.hasEveryonePermission(menu, "view")
);

const activityName = computed(() => {
  const activity = mockDataSets.crmActivities.find((item) => item.id === managerActivityID.value);
  return activity?.name ?? "電銷名單";
});

const crmPhonePipelineListViewObj = reactive<CrmPhonePipelineListViewMdl>({
  query: {
    pageIndex: 1,
    pageSize: 10,
  },
  recordList: [],
  totalCount: 0,
});

const allRecords = ref<TeleSaleRecord[]>([]);
const selectedId = ref<number | null>(null);
const originalRecordSnapshot = ref<TeleSaleRecord | null>(null);
const showDetailPanel = ref(true);
const showUnsavedConfirm = ref(false);
const selectedRecordIds = ref<number[]>([]);
const showGroupModal = ref(false);
const groupStep = ref<1 | 2 | 3>(1);
const groupName = ref("");
const groupIndustry = ref("");
const groupAddedCount = ref(0);

const regionOptions = ["北區", "中區", "南區"];
const salesByRegion: Record<string, string[]> = {
  北區: ["Lisa", "Mandy", "Ray"],
  中區: ["Ann", "Kevin", "Stella"],
  南區: ["Evan", "Cindy", "Leo"],
};
const industryOptions = [
  "上市上櫃",
  "一般企業",
  "醫院",
  "金融",
  "公家機關",
  "學校",
  "其他",
];

const selectedRecord = computed(() =>
  allRecords.value.find((record) => record.id === selectedId.value) ?? null
);
const isDirty = computed(() => {
  if (!selectedRecord.value || !originalRecordSnapshot.value) return false;
  return (
    JSON.stringify(selectedRecord.value) !==
    JSON.stringify(originalRecordSnapshot.value)
  );
});
const isSelected = (id: number) => selectedRecordIds.value.includes(id);
const isAllSelectedOnPage = computed(() => {
  const ids = crmPhonePipelineListViewObj.recordList.map((item) => item.id);
  return ids.length > 0 && ids.every((id) => selectedRecordIds.value.includes(id));
});
const splitCompanyName = (name: string) => {
  const trimmed = name?.trim() ?? "";
  if (!trimmed) return ["-"];
  const suffixes = ["股份有限公司", "有限公司", "公司"];
  const matchedSuffix = suffixes.find(
    (suffix) => trimmed.length > suffix.length && trimmed.endsWith(suffix)
  );
  if (!matchedSuffix) return [trimmed];
  return [trimmed.slice(0, -matchedSuffix.length), matchedSuffix];
};

const salesOptions = computed(() => {
  const region = selectedRecord.value?.assignedRegion;
  return region ? salesByRegion[region] ?? [] : [];
});
const industrySelectValue = computed({
  get: () => {
    const value = selectedRecord.value?.industry?.trim() ?? "";
    if (!value) return "";
    return industryOptions.includes(value) ? value : "其他";
  },
  set: (value: string) => {
    if (!selectedRecord.value) return;
    if (value === "其他") {
      selectedRecord.value.industry = "";
    } else {
      selectedRecord.value.industry = value;
    }
    touchRecord();
  },
});

const getRowNo = (index: number) =>
  (crmPhonePipelineListViewObj.query.pageIndex - 1) *
    crmPhonePipelineListViewObj.query.pageSize +
  index +
  1;

const splitIndustry = (value: string) => {
  if (!value) return { category: "", detail: "" };
  const parts = value
    .split(/[、,]/)
    .map((part) => part.trim())
    .filter(Boolean);
  if (!parts.length) return { category: "", detail: "" };

  let category = parts[0];
  if (category === "上市櫃") category = "上市上櫃";
  const detail = parts.slice(1).join("、");
  return { category, detail };
};

const getIndustryCategory = (value: string) => splitIndustry(value).category || "-";
const formatDateTimeDisplay = (value?: string) => {
  if (!value) return "-";
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return "-";
  const yyyy = date.getFullYear();
  const mm = String(date.getMonth() + 1).padStart(2, "0");
  const dd = String(date.getDate()).padStart(2, "0");
  const hh = String(date.getHours()).padStart(2, "0");
  const min = String(date.getMinutes()).padStart(2, "0");
  return `${yyyy}-${mm}-${dd} | ${hh}:${min}`;
};
const getCallDateDisplay = (record: TeleSaleRecord) => {
  const value = formatDateTimeDisplay(record.lastEditedAt);
  if (value === "-") return "-";
  return value.split(" | ")[0] ?? "-";
};
const getCallTimeDisplay = (record: TeleSaleRecord) => {
  const value = formatDateTimeDisplay(record.lastEditedAt);
  if (value === "-") return "-";
  return value.split(" | ")[1] ?? "-";
};

const ensureSelection = (list: TeleSaleRecord[]) => {
  if (!list.length) {
    selectedId.value = null;
    return;
  }
  if (!selectedId.value || !list.find((item) => item.id === selectedId.value)) {
    selectedId.value = list[0].id;
  }
  const current = list.find((item) => item.id === selectedId.value);
  originalRecordSnapshot.value = current
    ? JSON.parse(JSON.stringify(current))
    : null;
};

const getList = () => {
  if (!allRecords.value.length) {
    const all = (mockDataSets.teleSalesRecords ?? []) as TeleSaleRecord[];
    const matched = all.filter((item) => item.activityId === managerActivityID.value);
    allRecords.value = matched.length > 0 ? matched : all;
  }
  const filtered = allRecords.value;
  ensureSelection(filtered);

  const startIndex =
    (crmPhonePipelineListViewObj.query.pageIndex - 1) *
    crmPhonePipelineListViewObj.query.pageSize;
  const endIndex = startIndex + crmPhonePipelineListViewObj.query.pageSize;
  crmPhonePipelineListViewObj.recordList = filtered.slice(startIndex, endIndex);
  crmPhonePipelineListViewObj.totalCount = filtered.length;
};

const handleRowClick = (id: number) => {
  if (!canViewPipeline.value) return;
  selectedId.value = id;
  showDetailPanel.value = true;
  const current = allRecords.value.find((item) => item.id === id) ?? null;
  originalRecordSnapshot.value = current ? JSON.parse(JSON.stringify(current)) : null;
};

const handleRegionChange = () => {
  if (!selectedRecord.value) return;
  touchRecord();
  const options = salesByRegion[selectedRecord.value.assignedRegion ?? ""] ?? [];
  if (selectedRecord.value.assignedSales && !options.includes(selectedRecord.value.assignedSales)) {
    selectedRecord.value.assignedSales = "";
  }
};
const touchRecord = () => {
  if (!selectedRecord.value) return;
  selectedRecord.value.lastEditedAt = new Date().toISOString();
};

const handleCancel = () => {
  if (!selectedRecord.value || !originalRecordSnapshot.value) return;
  if (selectedRecord.value.id !== originalRecordSnapshot.value.id) return;
  Object.assign(selectedRecord.value, JSON.parse(JSON.stringify(originalRecordSnapshot.value)));
};

const handleSave = () => {
  if (!selectedRecord.value) return;
  selectedRecord.value.lastEditedAt = new Date().toISOString();
  selectedRecord.value.listStatus = "已撥打";
  const currentIndex = allRecords.value.findIndex(
    (item) => item.id === selectedRecord.value?.id
  );
  if (currentIndex !== -1) {
    const [current] = allRecords.value.splice(currentIndex, 1);
    allRecords.value.push(current);
    originalRecordSnapshot.value = JSON.parse(JSON.stringify(current));
  }
  const totalPages = Math.max(
    1,
    Math.ceil(allRecords.value.length / crmPhonePipelineListViewObj.query.pageSize)
  );
  crmPhonePipelineListViewObj.query.pageIndex = totalPages;
  getList();
};

const readLeadPool = (): LeadPoolItem[] => {
  const raw = localStorage.getItem(leadPoolStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const writeLeadPool = (items: LeadPoolItem[]) => {
  localStorage.setItem(leadPoolStorageKey, JSON.stringify(items));
};

const addSelectedToLeadPool = (extra?: { groupName?: string; groupIndustry?: string }) => {
  const selected = allRecords.value.filter((item) => selectedRecordIds.value.includes(item.id));
  if (selected.length === 0) return 0;
  const pool = readLeadPool();
  const existingIds = new Set(pool.map((item) => item.id));
  const nextItems = [...pool];
  let addedCount = 0;

  selected.forEach((item) => {
    if (existingIds.has(item.id)) return;
    const lastEditedAt = item.lastEditedAt ?? new Date().toISOString();
    const callDate = lastEditedAt.split("T")[0];
    nextItems.push({
      id: item.id,
      source: "活動轉電銷",
      activityId: item.activityId,
      activityName: item.activityName,
      companyName: item.companyName,
      contactName: item.contactName,
      contactDepartment: item.contactDepartment,
      industry: item.industry,
      evaluationRange: item.evaluationRange,
      interestLevel: item.interestLevel,
      assignedSales: item.assignedSales,
      listStatus: item.listStatus,
      lastEditedAt,
      callDate,
      groupName: extra?.groupName ?? item.groupName ?? "",
      groupIndustry: extra?.groupIndustry ?? item.groupIndustry ?? "",
    });
    addedCount += 1;
  });

  writeLeadPool(nextItems);
  return addedCount;
};

const handleAddToLeadPool = () => {
  const addedCount = addSelectedToLeadPool();
  selectedRecordIds.value = [];
  if (addedCount > 0) {
    sessionStorage.setItem(leadPoolAddedCountKey, String(addedCount));
  }
};

const toggleSelect = (id: number) => {
  if (isSelected(id)) {
    selectedRecordIds.value = selectedRecordIds.value.filter((value) => value !== id);
  } else {
    selectedRecordIds.value = [...selectedRecordIds.value, id];
  }
};

const toggleSelectAllOnPage = () => {
  const ids = crmPhonePipelineListViewObj.recordList.map((item) => item.id);
  if (isAllSelectedOnPage.value) {
    selectedRecordIds.value = selectedRecordIds.value.filter((id) => !ids.includes(id));
    return;
  }
  const merged = new Set([...selectedRecordIds.value, ...ids]);
  selectedRecordIds.value = Array.from(merged);
};

const handleClosePanel = () => {
  if (isDirty.value) {
    showUnsavedConfirm.value = true;
    return;
  }
  showDetailPanel.value = false;
};

const closeWithoutSave = () => {
  handleCancel();
  showUnsavedConfirm.value = false;
  showDetailPanel.value = false;
};

const confirmSaveAndClose = () => {
  handleSave();
  showUnsavedConfirm.value = false;
  showDetailPanel.value = false;
};

const openGroupModal = () => {
  groupName.value = "";
  groupIndustry.value = "";
  groupStep.value = 1;
  showGroupModal.value = true;
};

const closeGroupModal = () => {
  showGroupModal.value = false;
  groupStep.value = 1;
};

const confirmGroupInfo = () => {
  if (!groupName.value.trim() || !groupIndustry.value) return;
  groupStep.value = 2;
};

const addGroupToListOnly = () => {
  const selected = allRecords.value.filter((item) => selectedRecordIds.value.includes(item.id));
  selected.forEach((item) => {
    item.groupName = groupName.value.trim();
    item.groupIndustry = groupIndustry.value;
  });
  closeGroupModal();
};

const addGroupToLeadPool = () => {
  const addedCount = addSelectedToLeadPool({
    groupName: groupName.value.trim(),
    groupIndustry: groupIndustry.value,
  });
  groupAddedCount.value = addedCount;
  sessionStorage.setItem(leadPoolAddedCountKey, String(addedCount));
  groupStep.value = 3;
};

const goToLeadPool = () => {
  closeGroupModal();
  selectedRecordIds.value = [];
  router.push("/crm/phone/pool");
};

watch(
  () => route.params.activityId,
  (value) => {
    managerActivityID.value = Number(value || 0);
    allRecords.value = [];
    selectedId.value = null;
    crmPhonePipelineListViewObj.query.pageIndex = 1;
    getList();
  }
);

onMounted(() => {
  getList();
});
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden gap-4 p-2">
    <div class="flex items-center justify-between">
      <button class="btn-back flex items-center" @click="router.push('/crm/phone/activity')">
        <svg
          class="w-4 h-4 inline-block mr-1"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M15 19l-7-7 7-7"
          />
        </svg>
        {{ activityName }}
      </button>
    </div>

    <div class="flex flex-1 flex-col lg:flex-row gap-4 overflow-hidden">
      <div class="flex flex-1 flex-col overflow-hidden bg-white rounded-lg p-4 gap-4">
        <div class="flex items-center justify-between">
          <div class="text-xs text-gray-500">
            已選 {{ selectedRecordIds.length }} 筆
          </div>
          <div class="flex items-center gap-2">
            <button
              v-if="selectedRecordIds.length > 0"
              class="btn-submit"
              type="button"
              @click="handleAddToLeadPool"
            >
              加入名單池
            </button>
            <button
              v-if="selectedRecordIds.length > 0"
              class="btn-cancel"
              type="button"
              @click="openGroupModal"
            >
              群組
            </button>
          </div>
        </div>
        <div class="flex-1 overflow-y-auto table-scrollable">
          <table class="table-base table-fixed table-sticky w-full">
            <thead class="sticky top-0 bg-white z-10">
              <tr>
                <th class="text-center w-10">
                  <input
                    type="checkbox"
                    class="h-4 w-4"
                    :checked="isAllSelectedOnPage"
                    @change="toggleSelectAllOnPage"
                  />
                </th>
                <th class="text-start w-12">NO</th>
                <th class="text-start w-44">公司全名</th>
                <th class="text-start w-20">窗口</th>
                <th class="text-start w-24">產業類別</th>
                <th class="text-start w-24">名單狀態</th>
                <th class="text-start w-32">興趣程度</th>
                <th class="text-start w-24">分派業務</th>
                <th class="text-start w-28">群組</th>
              </tr>
            </thead>
            <tbody>
              <template v-if="crmPhonePipelineListViewObj.recordList.length === 0">
                <tr class="text-center">
                  <td colspan="8">無資料</td>
                </tr>
              </template>
              <template v-else>
                <tr
                  v-for="(item, index) in crmPhonePipelineListViewObj.recordList"
                  :key="item.id"
                  :class="[
                    selectedId === item.id ? 'bg-cyan-50' : '',
                    canViewPipeline ? 'cursor-pointer transition hover:bg-cyan-50' : 'opacity-60',
                  ]"
                  @click="handleRowClick(item.id)"
                >
                  <td class="text-center" @click.stop>
                    <input
                      type="checkbox"
                      class="h-4 w-4"
                      :checked="isSelected(item.id)"
                      @change="toggleSelect(item.id)"
                    />
                  </td>
                  <td class="text-start">{{ getRowNo(index) }}</td>
                  <td class="text-start">
                    <span
                      v-for="(part, partIndex) in splitCompanyName(item.companyName)"
                      :key="`${item.id}-company-${partIndex}`"
                    >
                      {{ part }}
                      <br v-if="partIndex === 0" />
                    </span>
                  </td>
                  <td class="text-start">{{ item.contactName }}</td>
                  <td class="text-start">{{ getIndustryCategory(item.industry) }}</td>
                  <td class="text-start">{{ item.listStatus }}</td>
                  <td class="text-start">{{ item.interestLevel }}</td>
                  <td class="text-start">{{ item.assignedSales || "-" }}</td>
                  <td class="text-start">{{ item.groupName || "-" }}</td>
                </tr>
              </template>
            </tbody>
          </table>
        </div>

        <div class="flex justify-center pt-2">
          <Pagination
            :pageIndex="crmPhonePipelineListViewObj.query.pageIndex"
            :pageSize="crmPhonePipelineListViewObj.query.pageSize"
            :totalCount="crmPhonePipelineListViewObj.totalCount"
            @update:current-page="
              (newPage: number) => {
                crmPhonePipelineListViewObj.query.pageIndex = newPage;
                getList();
              }
            "
            @update:page-size="
              (newSize: number) => {
                crmPhonePipelineListViewObj.query.pageSize = newSize;
                crmPhonePipelineListViewObj.query.pageIndex = 1;
                getList();
              }
            "
          />
        </div>
      </div>

      <div
        v-if="showDetailPanel"
        class="w-full lg:w-[360px] xl:w-[420px] bg-white rounded-lg p-4 overflow-y-auto"
      >
        <div v-if="!selectedRecord" class="text-sm text-gray-500">
          先從左側選擇名單
        </div>
        <div v-else class="flex flex-col gap-4">
          <div class="flex items-center justify-between">
            <div class="text-sm font-semibold text-gray-700">名單紀錄</div>
            <button
              type="button"
              class="text-gray-400 hover:text-gray-600 text-lg leading-none"
              @click="handleClosePanel"
            >
              ×
            </button>
          </div>
          <div>
            <div class="text-xs text-gray-500">公司電話 / 手機</div>
            <div class="text-sm font-semibold text-gray-800">
              {{ selectedRecord.companyPhone || "-" }}
              <span class="mx-2 text-gray-300">|</span>
              {{ selectedRecord.mobilePhone || "-" }}
            </div>
          </div>

          <div>
            <div class="text-xs text-gray-500">窗口姓名 / 部門</div>
            <div class="text-sm font-semibold text-gray-800">
              {{ selectedRecord.contactName || "-" }}
              <span class="mx-2 text-gray-300">|</span>
              {{ selectedRecord.contactDepartment || "-" }}
            </div>
          </div>

          <div class="grid grid-cols-1 gap-3 md:grid-cols-2">
            <div>
              <label class="form-label">分派區域</label>
              <select
                v-model="selectedRecord.assignedRegion"
                class="select-box w-full"
                @change="handleRegionChange"
              >
                <option value="">請選擇區域</option>
                <option v-for="region in regionOptions" :key="region" :value="region">
                  {{ region }}
                </option>
              </select>
            </div>
            <div>
              <label class="form-label">分派業務</label>
              <select
                v-model="selectedRecord.assignedSales"
                class="select-box w-full"
                @change="touchRecord"
              >
                <option value="">請選擇業務</option>
                <option v-for="sales in salesOptions" :key="sales" :value="sales">
                  {{ sales }}
                </option>
              </select>
            </div>
          </div>

          <div class="grid grid-cols-1 gap-3 md:grid-cols-2">
            <div>
              <label class="form-label">撥打日期</label>
              <input
                :value="getCallDateDisplay(selectedRecord)"
                type="text"
                class="input-box"
                placeholder="YYYY-MM-DD"
                disabled
              />
            </div>
            <div>
              <label class="form-label">撥打時間</label>
              <input
                :value="getCallTimeDisplay(selectedRecord)"
                type="text"
                class="input-box"
                placeholder="HH:mm"
                disabled
              />
            </div>
            <div>
              <label class="form-label">產業（非必填）</label>
              <select v-model="industrySelectValue" class="select-box w-full">
                <option value="">請選擇產業</option>
                <option v-for="option in industryOptions" :key="option" :value="option">
                  {{ option }}
                </option>
              </select>
              <input
                v-if="industrySelectValue === '其他'"
                v-model="selectedRecord.industry"
                type="text"
                class="input-box mt-2"
                placeholder="請輸入產業"
                @input="touchRecord"
              />
            </div>
            <div>
              <label class="form-label">評估區間</label>
              <input
                v-model="selectedRecord.evaluationRange"
                type="text"
                class="input-box"
                placeholder="輸入評估區間"
                @input="touchRecord"
              />
            </div>
            <div>
              <label class="form-label">洽談項目</label>
              <input
                v-model="selectedRecord.discussItem"
                type="text"
                class="input-box"
                placeholder="輸入洽談項目"
                @input="touchRecord"
              />
            </div>
            <div>
              <label class="form-label">潛在客戶的興趣程度</label>
              <input
                v-model="selectedRecord.interestLevel"
                type="text"
                class="input-box"
                placeholder="輸入興趣程度"
                @input="touchRecord"
              />
            </div>
            <div>
              <label class="form-label">電銷紀錄註記（短句）</label>
              <input
                v-model="selectedRecord.note"
                type="text"
                class="input-box"
                placeholder="被拒絕／已離職開發新窗口／資料錯誤"
                @input="touchRecord"
              />
            </div>
            <div>
              <label class="form-label">洽談備註</label>
              <textarea
                v-model="selectedRecord.discussNote"
                class="input-box"
                rows="3"
                placeholder="輸入洽談備註"
                @input="touchRecord"
              ></textarea>
            </div>
          </div>
          <div class="flex justify-center mt-3 gap-2">
            <button class="btn-cancel" type="button" @click="handleCancel">取消</button>
            <button class="btn-submit" type="button" @click="handleSave">儲存</button>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div
    v-if="showUnsavedConfirm"
    class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4"
  >
    <div class="w-full max-w-sm rounded-lg bg-white p-6 shadow-lg">
      <div class="text-sm font-semibold text-gray-800 mb-4">是否儲存此筆紀錄</div>
      <div class="flex justify-center gap-3">
        <button class="btn-cancel" type="button" @click="closeWithoutSave">不儲存</button>
        <button class="btn-submit" type="button" @click="confirmSaveAndClose">確認儲存</button>
      </div>
    </div>
  </div>

  <div
    v-if="showGroupModal"
    class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4"
  >
    <div class="w-full max-w-md rounded-lg bg-white p-6 shadow-lg">
      <div class="flex items-center justify-between mb-4">
        <h3 class="subtitle">群組名單</h3>
        <button class="text-gray-400 hover:text-gray-600 text-lg" @click="closeGroupModal">
          ×
        </button>
      </div>

      <div v-if="groupStep === 1" class="flex flex-col gap-4">
        <div>
          <label class="form-label">產業別</label>
          <select v-model="groupIndustry" class="select-box w-full">
            <option value="">請選擇</option>
            <option v-for="option in industryOptions" :key="option" :value="option">
              {{ option }}
            </option>
          </select>
        </div>
        <div>
          <label class="form-label">群組命名</label>
          <input v-model="groupName" type="text" class="input-box" placeholder="輸入群組名稱" />
        </div>
        <div class="flex justify-center mt-2 gap-2">
          <button class="btn-cancel" type="button" @click="closeGroupModal">取消建立</button>
          <button class="btn-submit" type="button" @click="confirmGroupInfo">儲存群組</button>
        </div>
      </div>

      <div v-else-if="groupStep === 2" class="flex flex-col gap-4">
        <div class="text-sm text-gray-700">是否加到名單池？</div>
        <div class="flex justify-center gap-2">
          <button class="btn-cancel" type="button" @click="addGroupToListOnly">否</button>
          <button class="btn-submit" type="button" @click="addGroupToLeadPool">是</button>
        </div>
      </div>

      <div v-else class="flex flex-col gap-4">
        <div class="text-sm text-gray-700">已加到名單池（{{ groupAddedCount }} 筆）</div>
        <div class="flex justify-center gap-2">
          <button class="btn-cancel" type="button" @click="closeGroupModal">取消</button>
          <button class="btn-submit" type="button" @click="goToLeadPool">前往查看</button>
        </div>
      </div>
    </div>
  </div>
</template>
