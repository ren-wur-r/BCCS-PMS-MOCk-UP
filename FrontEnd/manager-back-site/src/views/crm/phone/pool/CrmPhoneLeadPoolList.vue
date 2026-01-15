<script setup lang="ts">
import { computed, reactive, ref, onMounted, watch } from "vue";
import { useRouter } from "vue-router";
import Pagination from "@/components/global/pagination/Pagination.vue";

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
}

interface LeadPoolQueryMdl {
  industry: string;
  customerValue: string;
  callDate: string;
  evaluationRange: string;
  pageIndex: number;
  pageSize: number;
}

interface LeadPoolViewMdl {
  query: LeadPoolQueryMdl;
  items: LeadPoolItem[];
  totalCount: number;
}

const router = useRouter();
const leadPoolStorageKey = "cache.crm.phone.leadPool";
const industryOptions = [
  "上市上櫃",
  "一般企業",
  "醫院",
  "金融",
  "公家機關",
  "學校",
  "其他",
];
const customerValueOptions = ["高價值", "中價值", "低價值", "待評估"];
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

const viewObj = reactive<LeadPoolViewMdl>({
  query: {
    industry: "",
    customerValue: "",
    callDate: "",
    evaluationRange: "",
    pageIndex: 1,
    pageSize: 10,
  },
  items: [],
  totalCount: 0,
});

const showAddModal = ref(false);
const addedCountNotice = ref(0);
const leadPoolAddedCountKey = "cache.crm.phone.leadPool.addedCount";
const addForm = reactive<LeadPoolItem>({
  id: 0,
  source: "電銷自建",
  companyName: "",
  contactName: "",
  contactDepartment: "",
  industry: "",
  evaluationRange: "",
  interestLevel: "",
  assignedSales: "",
  listStatus: "未撥打",
  customerValue: "",
  callDate: "",
  lastEditedAt: "",
});

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

const applyFilters = () => {
  const all = readLeadPool();
  let filtered = all;

  if (viewObj.query.industry) {
    filtered = filtered.filter((item) => item.industry === viewObj.query.industry);
  }
  if (viewObj.query.customerValue) {
    filtered = filtered.filter((item) => item.customerValue === viewObj.query.customerValue);
  }
  if (viewObj.query.callDate) {
    filtered = filtered.filter((item) => (item.callDate || "") === viewObj.query.callDate);
  }
  if (viewObj.query.evaluationRange) {
    filtered = filtered.filter((item) =>
      (item.evaluationRange || "").includes(viewObj.query.evaluationRange)
    );
  }

  const startIndex = (viewObj.query.pageIndex - 1) * viewObj.query.pageSize;
  const endIndex = startIndex + viewObj.query.pageSize;
  viewObj.items = filtered.slice(startIndex, endIndex);
  viewObj.totalCount = filtered.length;
};

const resetFilters = () => {
  viewObj.query.industry = "";
  viewObj.query.customerValue = "";
  viewObj.query.callDate = "";
  viewObj.query.evaluationRange = "";
  viewObj.query.pageIndex = 1;
  applyFilters();
};

const openAddModal = () => {
  addForm.id = Date.now();
  addForm.companyName = "";
  addForm.contactName = "";
  addForm.contactDepartment = "";
  addForm.industry = "";
  addForm.evaluationRange = "";
  addForm.interestLevel = "";
  addForm.assignedSales = "";
  addForm.listStatus = "未撥打";
  addForm.customerValue = "";
  addForm.callDate = "";
  addForm.lastEditedAt = "";
  showAddModal.value = true;
};

const saveNewLead = () => {
  if (!addForm.companyName || !addForm.contactName) return;
  const now = new Date().toISOString();
  const payload: LeadPoolItem = {
    ...addForm,
    id: Date.now(),
    lastEditedAt: now,
    callDate: addForm.callDate || now.split("T")[0],
  };
  const pool = readLeadPool();
  pool.unshift(payload);
  writeLeadPool(pool);
  showAddModal.value = false;
  viewObj.query.pageIndex = 1;
  applyFilters();
};

const tabs = computed(() => [
  { key: "activity", label: "活動轉電銷", path: "/crm/phone/activity" },
  { key: "pool", label: "電銷名單", path: "/crm/phone/pool" },
]);

const isActiveTab = (key: string) => key === "pool";

onMounted(() => {
  const addedCountRaw = sessionStorage.getItem(leadPoolAddedCountKey);
  if (addedCountRaw) {
    const parsed = Number(addedCountRaw);
    if (!Number.isNaN(parsed)) {
      addedCountNotice.value = parsed;
    }
    sessionStorage.removeItem(leadPoolAddedCountKey);
  }
  applyFilters();
});

watch(
  () => [
    viewObj.query.industry,
    viewObj.query.customerValue,
    viewObj.query.callDate,
    viewObj.query.evaluationRange,
  ],
  () => {
    viewObj.query.pageIndex = 1;
    applyFilters();
  }
);
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden gap-4 p-2">
    <div class="flex mb-0 gap-y-4">
      <button
        v-for="tab in tabs"
        :key="tab.key"
        class="tab-btn"
        :class="isActiveTab(tab.key) ? 'active' : ''"
        @click="router.push(tab.path)"
      >
        {{ tab.label }}
      </button>
    </div>

    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-6 gap-4">
      <div v-if="addedCountNotice" class="text-sm text-emerald-700">
        已加入名單池 {{ addedCountNotice }} 筆
      </div>
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div>
          <label class="form-label">產業</label>
          <select v-model="viewObj.query.industry" class="select-box w-full">
            <option value="">全部</option>
            <option v-for="option in industryOptions" :key="option" :value="option">
              {{ option }}
            </option>
          </select>
        </div>
        <div>
          <label class="form-label">客戶價值</label>
          <select v-model="viewObj.query.customerValue" class="select-box w-full">
            <option value="">全部</option>
            <option v-for="option in customerValueOptions" :key="option" :value="option">
              {{ option }}
            </option>
          </select>
        </div>
        <div>
          <label class="form-label">撥打日期</label>
          <input v-model="viewObj.query.callDate" type="date" class="input-box" />
        </div>
        <div>
          <label class="form-label">評估區間</label>
          <input
            v-model="viewObj.query.evaluationRange"
            type="text"
            class="input-box"
            placeholder="輸入評估區間"
          />
        </div>
      </div>
      <div></div>

      <div class="flex-1 overflow-y-auto table-scrollable">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-28">來源</th>
              <th class="text-start w-44">公司全名</th>
              <th class="text-start w-20">窗口</th>
              <th class="text-start w-24">產業</th>
              <th class="text-start w-24">客戶價值</th>
              <th class="text-start w-24">撥打日期</th>
              <th class="text-start w-32">評估區間</th>
              <th class="text-start w-32">興趣程度</th>
              <th class="text-start w-24">名單狀態</th>
            </tr>
          </thead>
          <tbody>
            <template v-if="viewObj.items.length === 0">
              <tr class="text-center">
                <td colspan="9">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr v-for="item in viewObj.items" :key="item.id">
                <td class="text-start">{{ item.source }}</td>
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
                <td class="text-start">{{ item.industry || "-" }}</td>
                <td class="text-start">{{ item.customerValue || "-" }}</td>
                <td class="text-start">{{ item.callDate || "-" }}</td>
                <td class="text-start">{{ item.evaluationRange || "-" }}</td>
                <td class="text-start">{{ item.interestLevel || "-" }}</td>
                <td class="text-start">{{ item.listStatus || "-" }}</td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>
      <button
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        type="button"
        style="margin-top: 10px; background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
        @click="openAddModal"
      >
        +新增名單
      </button>

      <div class="flex justify-center pt-2">
        <Pagination
          :pageIndex="viewObj.query.pageIndex"
          :pageSize="viewObj.query.pageSize"
          :totalCount="viewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              viewObj.query.pageIndex = newPage;
              applyFilters();
            }
          "
          @update:page-size="
            (newSize: number) => {
              viewObj.query.pageSize = newSize;
              viewObj.query.pageIndex = 1;
              applyFilters();
            }
          "
        />
      </div>
    </div>
  </div>

  <div
    v-if="showAddModal"
    class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4"
  >
    <div class="w-full max-w-xl rounded-lg bg-white p-6 shadow-lg">
      <div class="flex items-center justify-between mb-4">
        <h3 class="subtitle">新增名單</h3>
        <button class="text-gray-400 hover:text-gray-600 text-lg" @click="showAddModal = false">
          ×
        </button>
      </div>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
        <div>
          <label class="form-label">公司全名</label>
          <input v-model="addForm.companyName" type="text" class="input-box" />
        </div>
        <div>
          <label class="form-label">窗口</label>
          <input v-model="addForm.contactName" type="text" class="input-box" />
        </div>
        <div>
          <label class="form-label">窗口部門</label>
          <input v-model="addForm.contactDepartment" type="text" class="input-box" />
        </div>
        <div>
          <label class="form-label">產業</label>
          <select v-model="addForm.industry" class="select-box w-full">
            <option value="">請選擇</option>
            <option v-for="option in industryOptions" :key="option" :value="option">
              {{ option }}
            </option>
          </select>
        </div>
        <div>
          <label class="form-label">客戶價值</label>
          <select v-model="addForm.customerValue" class="select-box w-full">
            <option value="">請選擇</option>
            <option v-for="option in customerValueOptions" :key="option" :value="option">
              {{ option }}
            </option>
          </select>
        </div>
        <div>
          <label class="form-label">撥打日期</label>
          <input v-model="addForm.callDate" type="date" class="input-box" />
        </div>
        <div>
          <label class="form-label">評估區間</label>
          <input v-model="addForm.evaluationRange" type="text" class="input-box" />
        </div>
        <div>
          <label class="form-label">興趣程度</label>
          <input v-model="addForm.interestLevel" type="text" class="input-box" />
        </div>
      </div>
      <div class="flex justify-center mt-4 gap-2">
        <button class="btn-cancel" type="button" @click="showAddModal = false">
          取消
        </button>
        <button class="btn-submit" type="button" @click="saveNewLead">
          儲存
        </button>
      </div>
    </div>
  </div>
</template>
