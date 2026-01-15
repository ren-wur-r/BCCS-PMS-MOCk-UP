<script setup lang="ts">
import { reactive, ref, onMounted, computed, nextTick, watch } from "vue";
import router from "@/router";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import Sortable from "sortablejs";
import {
  getManyCustomer,
  updateCustomerOrder,
} from "@/services";
import type {
  MbsCrmPhsHttpGetManyCustomerReqMdl,
  MbsCrmPhsHttpGetManyCustomerRspItemMdl,
} from "@/services/pms-http/crm/phone-sales/phoneSalesHttpFormat";
import {
  DbAtomPhoneSalesCustomerStatusEnum,
  DbAtomPhoneSalesCustomerStatusLabels,
  DbAtomPhoneSalesCustomerStatusColors,
} from "@/constants/DbAtomPhoneSalesCustomerStatusEnum";
import {
  DbAtomPhoneSalesCustomerValueEnum,
  DbAtomPhoneSalesCustomerValueLabels,
  DbAtomPhoneSalesCustomerValueColors,
} from "@/constants/DbAtomPhoneSalesCustomerValueEnum";
import {
  DbAtomPhoneSalesDataSourceLabels,
  DbAtomPhoneSalesDataSourceEnum,
} from "@/constants/DbAtomPhoneSalesDataSourceEnum";

const tokenStore = useTokenStore();
const { errorMessage, showError, handleErrorCode, displayError, closeError } = useErrorCodeHandler();

const isLoading = ref(false);
const customers = ref<MbsCrmPhsHttpGetManyCustomerRspItemMdl[]>([]);
const viewMode = ref<"board" | "list">("board");
const selectionMode = ref<"single" | "multiple">("multiple");
const groupBy = ref<"status" | "value" | "source">("status");
const selectedCustomerIds = ref<number[]>([]);
let sortableInstances: Sortable[] = [];

const filterQuery = reactive({
  status: null as DbAtomPhoneSalesCustomerStatusEnum | null,
  value: null as DbAtomPhoneSalesCustomerValueEnum | null,
  companyName: "",
  industry: "",
  contacterName: "",
});

const statusColumns = [
  DbAtomPhoneSalesCustomerStatusEnum.PendingContact,
  DbAtomPhoneSalesCustomerStatusEnum.Contacted,
  DbAtomPhoneSalesCustomerStatusEnum.Appointed,
  DbAtomPhoneSalesCustomerStatusEnum.Dispatched,
  DbAtomPhoneSalesCustomerStatusEnum.NoContact,
];

const valueGroups = [
  DbAtomPhoneSalesCustomerValueEnum.High,
  DbAtomPhoneSalesCustomerValueEnum.Medium,
  DbAtomPhoneSalesCustomerValueEnum.Low,
  DbAtomPhoneSalesCustomerValueEnum.Pending,
];

const sourceGroups = [
  DbAtomPhoneSalesDataSourceEnum.Activity,
  DbAtomPhoneSalesDataSourceEnum.PhoneSales,
  DbAtomPhoneSalesDataSourceEnum.Import,
];

const customersByStatus = computed(() => {
  const grouped: Record<number, MbsCrmPhsHttpGetManyCustomerRspItemMdl[]> = {};

  statusColumns.forEach((status) => {
    grouped[status] = customers.value
      .filter((c) => c.atomPhoneSalesCustomerStatus === status)
      .sort((a, b) => a.customOrder - b.customOrder);
  });

  return grouped;
});

const listGroups = computed(() => {
  const groups: Array<{
    id: string;
    label: string;
    items: MbsCrmPhsHttpGetManyCustomerRspItemMdl[];
  }> = [];

  if (groupBy.value === "status") {
    statusColumns.forEach((status) => {
      groups.push({
        id: `status-${status}`,
        label: DbAtomPhoneSalesCustomerStatusLabels[status],
        items: customers.value
          .filter((c) => c.atomPhoneSalesCustomerStatus === status)
          .sort((a, b) => a.customOrder - b.customOrder),
      });
    });
  } else if (groupBy.value === "value") {
    valueGroups.forEach((value) => {
      groups.push({
        id: `value-${value}`,
        label: DbAtomPhoneSalesCustomerValueLabels[value],
        items: customers.value
          .filter((c) => c.atomPhoneSalesCustomerValue === value)
          .sort((a, b) => a.customOrder - b.customOrder),
      });
    });
  } else {
    sourceGroups.forEach((source) => {
      groups.push({
        id: `source-${source}`,
        label: DbAtomPhoneSalesDataSourceLabels[source],
        items: customers.value
          .filter((c) => c.atomPhoneSalesDataSource === source)
          .sort((a, b) => a.customOrder - b.customOrder),
      });
    });
  }

  return groups;
});

const loadCustomers = async () => {
  isLoading.value = true;
  try {
    const requestData: MbsCrmPhsHttpGetManyCustomerReqMdl = {
      employeeLoginToken: tokenStore.token,
      atomPhoneSalesCustomerStatus: filterQuery.status,
      atomPhoneSalesCustomerValue: filterQuery.value,
      companyName: filterQuery.companyName || null,
      industry: filterQuery.industry || null,
      contacterName: filterQuery.contacterName || null,
      contacterEmail: null,
      isExistingCustomer: null,
      pageIndex: 1,
      pageSize: 1000,
    };

    const response = await getManyCustomer(requestData);
    if (response) {
      const errorCode = handleErrorCode(response.errorCode);
      if (errorCode) {
        customers.value = response.customerList;
        await nextTick();
        initializeSortables();
      } else {
        displayError();
      }
    }
  } finally {
    isLoading.value = false;
  }
};

const clearSortables = () => {
  sortableInstances.forEach((instance) => instance.destroy());
  sortableInstances = [];
};

const initializeBoardSortable = () => {
  statusColumns.forEach((status) => {
    const el = document.getElementById(`column-${status}`);
    if (el) {
      const instance = new Sortable(el, {
        group: "customers",
        animation: 150,
        onEnd: async (evt) => {
          if (evt.from === evt.to && evt.oldIndex === evt.newIndex) {
            return;
          }

          const targetStatus = parseInt(evt.to.id.replace("column-", ""));
          const items = Array.from(evt.to.children).map((child) =>
            parseInt(child.getAttribute("data-id") || "0")
          );

          await updateOrder(items);
        },
      });
      sortableInstances.push(instance);
    }
  });
};

const initializeListSortable = () => {
  listGroups.value.forEach((group) => {
    const el = document.getElementById(`list-group-${group.id}`);
    if (el) {
      const instance = new Sortable(el, {
        group: {
          name: "customer-list",
          pull: false,
          put: false,
        },
        animation: 150,
        onEnd: async (evt) => {
          if (evt.from === evt.to && evt.oldIndex === evt.newIndex) {
            return;
          }

          const items = Array.from(evt.to.children).map((child) =>
            parseInt(child.getAttribute("data-id") || "0")
          );

          await updateOrder(items);
        },
      });
      sortableInstances.push(instance);
    }
  });
};

const initializeSortables = () => {
  clearSortables();
  if (viewMode.value === "board") {
    initializeBoardSortable();
  } else {
    initializeListSortable();
  }
};

const updateOrder = async (customerIds: number[]) => {
  try {
    await updateCustomerOrder({
      employeeLoginToken: tokenStore.token,
      customerIDList: customerIds,
    });
  } catch (error) {
    console.error("Failed to update order:", error);
  }
};

const goToDetail = (id: number) => {
  router.push(`/crm/phone-sales/customer/detail/${id}`);
};

const goToAdd = () => {
  router.push("/crm/phone-sales/customer/add");
};

const isSelected = (id: number) => selectedCustomerIds.value.includes(id);

const toggleSelection = (id: number) => {
  if (selectionMode.value === "single") {
    selectedCustomerIds.value = [id];
    return;
  }

  if (selectedCustomerIds.value.includes(id)) {
    selectedCustomerIds.value = selectedCustomerIds.value.filter((item) => item !== id);
  } else {
    selectedCustomerIds.value = [...selectedCustomerIds.value, id];
  }
};

onMounted(() => {
  loadCustomers();
});

watch([viewMode, groupBy], async () => {
  await nextTick();
  initializeSortables();
});
</script>

<template>
  <div class="p-2 space-y-6">
    <div class="bg-white p-4 rounded-lg shadow">
      <div class="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-5 gap-4">
        <div>
          <label class="block text-sm font-medium mb-1">客戶狀態</label>
          <select
            v-model="filterQuery.status"
            class="w-full border rounded px-3 py-2"
            @change="loadCustomers"
          >
            <option :value="null">全部</option>
            <option
              v-for="status in statusColumns"
              :key="status"
              :value="status"
            >
              {{ DbAtomPhoneSalesCustomerStatusLabels[status] }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-medium mb-1">客戶價值</label>
          <select
            v-model="filterQuery.value"
            class="w-full border rounded px-3 py-2"
            @change="loadCustomers"
          >
            <option :value="null">全部</option>
            <option :value="DbAtomPhoneSalesCustomerValueEnum.High">
              {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.High] }}
            </option>
            <option :value="DbAtomPhoneSalesCustomerValueEnum.Medium">
              {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.Medium] }}
            </option>
            <option :value="DbAtomPhoneSalesCustomerValueEnum.Low">
              {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.Low] }}
            </option>
            <option :value="DbAtomPhoneSalesCustomerValueEnum.Pending">
              {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.Pending] }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-medium mb-1">公司名稱</label>
          <input
            v-model="filterQuery.companyName"
            type="text"
            class="w-full border rounded px-3 py-2"
            placeholder="搜尋公司..."
            @keyup.enter="loadCustomers"
          />
        </div>

        <div>
          <label class="block text-sm font-medium mb-1">產業</label>
          <input
            v-model="filterQuery.industry"
            type="text"
            class="w-full border rounded px-3 py-2"
            placeholder="搜尋產業..."
            @keyup.enter="loadCustomers"
          />
        </div>

        <div>
          <label class="block text-sm font-medium mb-1">窗口姓名</label>
          <input
            v-model="filterQuery.contacterName"
            type="text"
            class="w-full border rounded px-3 py-2"
            placeholder="搜尋窗口..."
            @keyup.enter="loadCustomers"
          />
        </div>
      </div>

      <div class="mt-4 flex justify-end">
        <button
          @click="loadCustomers"
          class="px-4 py-2 bg-gray-600 text-white rounded hover:bg-gray-700"
        >
          搜尋
        </button>
      </div>
    </div>

    <div class="flex flex-wrap items-center justify-between gap-3">
      <div class="flex items-center gap-2">
        <button
          class="px-3 py-1.5 rounded border text-sm"
          :class="viewMode === 'board' ? 'bg-cyan-600 text-white border-cyan-600' : 'bg-white text-gray-700'"
          @click="viewMode = 'board'"
        >
          看板
        </button>
        <button
          class="px-3 py-1.5 rounded border text-sm"
          :class="viewMode === 'list' ? 'bg-cyan-600 text-white border-cyan-600' : 'bg-white text-gray-700'"
          @click="viewMode = 'list'"
        >
          列表
        </button>
      </div>

      <div class="flex items-center gap-4">
        <div class="flex items-center gap-2">
          <span class="text-xs text-gray-500">群組</span>
          <select
            v-model="groupBy"
            class="border rounded px-2 py-1 text-sm"
            :disabled="viewMode === 'board'"
          >
            <option value="status">依狀態</option>
            <option value="value">依價值</option>
            <option value="source">依來源</option>
          </select>
        </div>
        <div class="flex items-center gap-2">
          <span class="text-xs text-gray-500">選取</span>
          <select v-model="selectionMode" class="border rounded px-2 py-1 text-sm">
            <option value="single">單選</option>
            <option value="multiple">多選</option>
          </select>
        </div>
      </div>
    </div>

    <div v-if="isLoading" class="text-center py-8">
      <div class="text-gray-500">載入中...</div>
    </div>

    <div v-else-if="viewMode === 'board'" class="flex gap-4 overflow-x-auto pb-4">
      <div
        v-for="status in statusColumns"
        :key="status"
        class="flex-shrink-0 w-80"
      >
        <div
          :class="[
            'rounded-lg p-4 mb-4',
            DbAtomPhoneSalesCustomerStatusColors[status],
          ]"
        >
          <div class="flex justify-between items-center">
            <h3 class="font-semibold">
              {{ DbAtomPhoneSalesCustomerStatusLabels[status] }}
            </h3>
            <span class="text-sm">
              {{ customersByStatus[status]?.length || 0 }}
            </span>
          </div>
        </div>

        <div
          :id="`column-${status}`"
          class="space-y-3 min-h-[200px] p-2 bg-gray-50 rounded"
        >
          <div
            v-for="customer in customersByStatus[status]"
            :key="customer.id"
            :data-id="customer.id"
            @click="goToDetail(customer.id)"
            class="bg-white p-4 rounded-lg shadow hover:shadow-md cursor-pointer transition-shadow"
          >
            <div class="flex justify-between items-start mb-2">
              <h4 class="font-semibold text-gray-900 line-clamp-1">
                {{ customer.companyName }}
              </h4>
              <span
                :class="[
                  'text-xs px-2 py-1 rounded border',
                  DbAtomPhoneSalesCustomerValueColors[customer.atomPhoneSalesCustomerValue],
                ]"
              >
                {{ DbAtomPhoneSalesCustomerValueLabels[customer.atomPhoneSalesCustomerValue] }}
              </span>
            </div>

            <div class="text-sm text-gray-600 space-y-1">
              <div class="flex items-center gap-2">
                <span class="text-gray-500">產業:</span>
                <span>{{ customer.industry || "-" }}</span>
              </div>

              <div class="flex items-center gap-2">
                <span class="text-gray-500">來源:</span>
                <span>{{ DbAtomPhoneSalesDataSourceLabels[customer.atomPhoneSalesDataSource] }}</span>
              </div>

              <div
                v-if="customer.contacterList.length > 0"
                class="pt-2 border-t"
              >
                <div class="flex items-center gap-2">
                  <span class="text-gray-500">窗口:</span>
                  <span>{{ customer.contacterList[0].contacterName }}</span>
                </div>
                <div class="flex items-center gap-2 text-xs">
                  <span class="text-gray-500">電話:</span>
                  <span>{{ customer.contacterList[0].contacterPhone || "-" }}</span>
                </div>
              </div>

              <div
                v-if="customer.isExistingCustomer"
                class="pt-2"
              >
                <span class="text-xs bg-green-100 text-green-800 px-2 py-1 rounded">
                  既有客戶
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="space-y-4">
      <div
        v-for="group in listGroups"
        :key="group.id"
        class="bg-white rounded-lg shadow"
      >
        <div class="flex items-center justify-between px-4 py-2 border-b">
          <div class="flex items-center gap-2">
            <span class="text-sm font-semibold text-gray-700">{{ group.label }}</span>
            <span class="text-xs text-gray-500">{{ group.items.length }}</span>
          </div>
        </div>

        <div
          :id="`list-group-${group.id}`"
          class="divide-y"
        >
          <div
            v-for="customer in group.items"
            :key="customer.id"
            :data-id="customer.id"
            class="flex items-center gap-3 px-4 py-3 hover:bg-cyan-50 cursor-pointer"
            @click="goToDetail(customer.id)"
          >
            <input
              type="checkbox"
              class="h-4 w-4"
              :checked="isSelected(customer.id)"
              @click.stop="toggleSelection(customer.id)"
            />
            <div class="flex-1 grid grid-cols-1 md:grid-cols-5 gap-2 text-sm">
              <div class="font-semibold text-gray-900">
                {{ customer.companyName }}
              </div>
              <div class="text-gray-600">
                產業: {{ customer.industry || "-" }}
              </div>
              <div class="text-gray-600">
                來源: {{ DbAtomPhoneSalesDataSourceLabels[customer.atomPhoneSalesDataSource] }}
              </div>
              <div class="text-gray-600">
                窗口: {{ customer.contacterList[0]?.contacterName || "-" }}
              </div>
              <div class="flex items-center gap-2 text-gray-600">
                <span
                  :class="[
                    'text-xs px-2 py-1 rounded border',
                    DbAtomPhoneSalesCustomerValueColors[customer.atomPhoneSalesCustomerValue],
                  ]"
                >
                  {{ DbAtomPhoneSalesCustomerValueLabels[customer.atomPhoneSalesCustomerValue] }}
                </span>
                <span class="text-xs text-gray-500">
                  {{ DbAtomPhoneSalesCustomerStatusLabels[customer.atomPhoneSalesCustomerStatus] }}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="showError"
      class="fixed top-4 right-4 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded shadow-lg"
      @click="closeError"
    >
      <p>{{ errorMessage }}</p>
    </div>
  </div>
</template>
