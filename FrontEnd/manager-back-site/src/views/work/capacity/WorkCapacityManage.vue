<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from "vue";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { loadCapacityRecords, saveCapacityRecords, type CapacityRecord } from "@/stores/capacityStore";
import { getMemberProjectStats } from "@/utils/capacityUtils";
import { getEmployeeProjectStatusLabel } from "@/utils/getEmployeeProjectStatusLabel";
import { useMockDataStore } from "@/stores/mockDataStore";

const employeeInfoStore = useEmployeeInfoStore();
const records = ref<CapacityRecord[]>(loadCapacityRecords());
const mockStore = useMockDataStore();
const activeTab = ref<"capacity" | "cert">("capacity");
const selectedMemberName = ref<string>("");
const capacityMode = ref<"week" | "month">("week");
const selectedCertificateMember = ref<string>("");
const refreshTick = ref(0);

const roleName = computed(() => employeeInfoStore.effectiveRoleName || "");
const canEdit = computed(
  () =>
    roleName.value.includes("各部門經理") ||
    roleName.value.includes("各處處長") ||
    roleName.value.includes("工程部中部") ||
    roleName.value.includes("顧問中部") ||
    roleName.value.includes("Admin")
);

const departmentContext = computed(() => {
  const devRole = employeeInfoStore.devRoleName;
  if (devRole === "工程部中部") {
    return { departmentName: "工程部", regionName: "中區" };
  }
  if (devRole === "顧問中部") {
    return { departmentName: "中區顧問部", regionName: "中區" };
  }
  return {
    departmentName: employeeInfoStore.managerDepartmentName || "",
    regionName: employeeInfoStore.managerRegionName || "",
  };
});
const visibleRecords = computed(() => {
  const { departmentName, regionName } = departmentContext.value;
  if (!departmentName) return records.value;
  return records.value.filter((item) => {
    if (item.departmentName !== departmentName) return false;
    if (regionName && item.regionName !== regionName) return false;
    return true;
  });
});

const lastUpdatedLabel = computed(() => {
  const timestamps = records.value
    .map((item) => item.lastUpdatedAt)
    .filter((value): value is string => Boolean(value));
  if (!timestamps.length) return "尚未更新";
  const latest = timestamps.sort().slice(-1)[0];
  return latest ? new Date(latest).toLocaleString("zh-TW") : "/assert";
});

const updateRecord = (index: number, updater: (record: CapacityRecord) => CapacityRecord) => {
  const next = [...records.value];
  next[index] = updater(next[index]);
  next[index].lastUpdatedAt = new Date().toISOString();
  records.value = next;
  saveCapacityRecords(next);
};

const toggleSupportRegion = (index: number, region: string) => {
  updateRecord(index, (record) => {
    const set = new Set(record.supportRegions);
    if (set.has(region)) {
      set.delete(region);
    } else {
      set.add(region);
    }
    return { ...record, supportRegions: Array.from(set) };
  });
};

const selectedMemberStats = computed(() => {
  refreshTick.value;
  if (!selectedMemberName.value) return null;
  return getMemberProjectStats(selectedMemberName.value);
});
const selectedMemberRecord = computed(() => {
  if (!selectedMemberName.value) return null;
  return (
    visibleRecords.value.find((item) => item.employeeName === selectedMemberName.value) || null
  );
});
const selectedMemberRecordIndex = computed(() => {
  if (!selectedMemberName.value) return -1;
  return visibleRecords.value.findIndex(
    (item) => item.employeeName === selectedMemberName.value
  );
});
const selectedMemberCertificates = computed(() => {
  if (!selectedMemberName.value) return [];
  return mockStore.certificates.filter(
    (item) => item.employeeName === selectedMemberName.value
  );
});

const getAssignedHours = (name: string) => getMemberProjectStats(name)?.assignedHours ?? 0;
const getAssignedManDays = (name: string) => getMemberProjectStats(name)?.assignedManDays ?? 0;
const getCapacityRatio = (record: CapacityRecord, mode: "week" | "month") => {
  refreshTick.value;
  const capacity = mode === "month" ? record.monthlyHours : record.weeklyHours;
  if (!capacity) return null;
  const assigned = getAssignedHours(record.employeeName);
  const normalizedAssigned = mode === "month" ? assigned : assigned / 4;
  return normalizedAssigned / capacity;
};
const getCapacityStatus = (ratio: number | null) => {
  if (ratio === null) return "unset";
  if (ratio < 0.8) return "ok";
  if (ratio <= 0.95) return "warn";
  return "over";
};
const getCapacityStatusLabel = (status: string) => {
  if (status === "ok") return "綠燈";
  if (status === "warn") return "黃燈";
  if (status === "over") return "紅燈";
  return "未設定";
};
const getCapacityStatusClass = (status: string) => {
  if (status === "ok") return "bg-emerald-50 text-emerald-700";
  if (status === "warn") return "bg-amber-50 text-amber-700";
  if (status === "over") return "bg-rose-50 text-rose-700";
  return "bg-gray-100 text-gray-600";
};
const getCapacityProgressClass = (status: string) => {
  if (status === "ok") return "bg-emerald-400";
  if (status === "warn") return "bg-amber-400";
  if (status === "over") return "bg-rose-400";
  return "bg-gray-300";
};

const selectedMonthlyRatio = computed(() => {
  refreshTick.value;
  if (!selectedMemberRecord.value) return null;
  return getCapacityRatio(selectedMemberRecord.value, "month");
});
const selectedWeeklyRatio = computed(() => {
  refreshTick.value;
  if (!selectedMemberRecord.value) return null;
  return getCapacityRatio(selectedMemberRecord.value, "week");
});
const selectedEstimatedOnsite = computed(() => {
  refreshTick.value;
  if (!selectedMemberName.value) return 0;
  return Math.round(getAssignedManDays(selectedMemberName.value) * 0.5 * 10) / 10;
});
const selectedEstimatedOffsite = computed(() => {
  refreshTick.value;
  if (!selectedMemberName.value) return 0;
  return Math.round(getAssignedManDays(selectedMemberName.value) * 0.5 * 10) / 10;
});

const visibleMemberNames = computed(() =>
  visibleRecords.value.map((item) => item.employeeName)
);
const visibleCertificates = computed(() =>
  mockStore.certificates.filter((item) =>
    visibleMemberNames.value.includes(item.employeeName)
  )
);
const certificateCountByMember = computed(() => {
  const counts = new Map<string, number>();
  visibleCertificates.value.forEach((item) => {
    counts.set(item.employeeName, (counts.get(item.employeeName) ?? 0) + 1);
  });
  return counts;
});
const selectedCertificateList = computed(() => {
  if (!selectedCertificateMember.value) return [];
  return visibleCertificates.value.filter(
    (item) => item.employeeName === selectedCertificateMember.value
  );
});

watch(
  visibleMemberNames,
  (names) => {
    if (!names.length) {
      selectedCertificateMember.value = "";
      return;
    }
    if (!selectedCertificateMember.value || !names.includes(selectedCertificateMember.value)) {
      selectedCertificateMember.value = names[0];
    }
  },
  { immediate: true }
);

const refreshCapacity = () => {
  refreshTick.value += 1;
};

onMounted(() => {
  window.addEventListener("work-project-assignments-updated", refreshCapacity);
  window.addEventListener("work-project-members-updated", refreshCapacity);
});

onBeforeUnmount(() => {
  window.removeEventListener("work-project-assignments-updated", refreshCapacity);
  window.removeEventListener("work-project-members-updated", refreshCapacity);
});
</script>

<template>
  <div class="flex flex-col bg-white rounded-lg p-6 gap-4 h-full">
    <div class="flex gap-2">
      <button
        type="button"
        class="tab-btn"
        :class="activeTab === 'capacity' ? 'active' : ''"
        @click="activeTab = 'capacity'"
      >
        量能管理
      </button>
      <button
        type="button"
        class="tab-btn"
        :class="activeTab === 'cert' ? 'active' : ''"
        @click="activeTab = 'cert'"
      >
        證照管理
      </button>
    </div>

    <div v-if="activeTab === 'capacity'" class="grid grid-cols-1 lg:grid-cols-[1.1fr_1.4fr] gap-4">
      <div class="overflow-x-auto">
        <div class="mb-3 text-xs text-gray-500">
          月設定用於承接案量水位，週監控用於排程衝突預警。
        </div>
        <div class="flex justify-end mb-3">
          <div class="flex gap-2">
            <button
              type="button"
              class="rounded-md border px-3 py-1.5 text-sm font-semibold transition"
              :class="capacityMode === 'week' ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
              @click="capacityMode = 'week'"
            >
              週量能
            </button>
            <button
              type="button"
              class="rounded-md border px-3 py-1.5 text-sm font-semibold transition"
              :class="capacityMode === 'month' ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
              @click="capacityMode = 'month'"
            >
              月量能
            </button>
          </div>
        </div>

        <table class="table-base table-fixed table-sticky w-full min-w-[520px]">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-28">成員</th>
              <th v-if="capacityMode === 'week'" class="text-start w-28">本週工時</th>
              <th v-if="capacityMode === 'week'" class="text-start w-28">本週人天</th>
              <th v-if="capacityMode === 'month'" class="text-start w-28">本月工時</th>
              <th v-if="capacityMode === 'month'" class="text-start w-28">本月人天</th>
              <th class="text-start w-24">負載</th>
              <th class="text-start w-40">最後更新</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="visibleRecords.length === 0">
              <td colspan="6" class="text-center text-gray-400 py-6">
                目前沒有量能資料
              </td>
            </tr>
            <tr
              v-for="item in visibleRecords"
              :key="item.employeeName"
              class="border border-gray-300 cursor-pointer hover:bg-cyan-50 transition-colors"
              :class="selectedMemberName === item.employeeName ? 'bg-cyan-50' : ''"
              @click="selectedMemberName = item.employeeName"
            >
              <td class="text-start font-semibold text-gray-700">{{ item.employeeName }}</td>
              <td v-if="capacityMode === 'week'" class="text-start">
                {{ item.weeklyHours || 0 }}
              </td>
              <td v-if="capacityMode === 'week'" class="text-start">
                {{ item.weeklyManDays || 0 }}
              </td>
              <td v-if="capacityMode === 'month'" class="text-start">
                {{ item.monthlyHours || 0 }}
              </td>
              <td v-if="capacityMode === 'month'" class="text-start">
                {{ item.monthlyManDays || 0 }}
              </td>
              <td class="text-start">
                <span
                  class="inline-flex items-center rounded-full px-2 py-0.5 text-xs font-semibold"
                  :class="getCapacityStatusClass(getCapacityStatus(getCapacityRatio(item, capacityMode)))"
                >
                  {{ getCapacityStatusLabel(getCapacityStatus(getCapacityRatio(item, capacityMode))) }}
                </span>
              </td>
              <td class="text-start text-xs text-gray-500">
                {{ item.lastUpdatedAt ? new Date(item.lastUpdatedAt).toLocaleString("zh-TW") : "尚未更新" }}
              </td>
            </tr>
          </tbody>
        </table>
        <!-- TODO: 假期/不可用欄位待串接 ERP 人員休假 API -->
      </div>

      <div class="rounded-lg border border-gray-200 bg-white p-4">
        <div class="flex items-center justify-between">
          <div>
            <h3 class="subtitle">成員量能設定</h3>
            <p class="text-xs text-gray-500 mt-1">
              {{ selectedMemberName || "請選擇成員" }}
            </p>
          </div>
          <button
            v-if="selectedMemberName"
            type="button"
            class="btn-cancel"
            @click="selectedMemberName = ''"
          >
            清除
          </button>
        </div>

        <div v-if="!selectedMemberRecord" class="text-sm text-gray-400 mt-4">
          點擊左側成員以檢視與編輯量能設定。
        </div>

        <div v-else class="mt-4 space-y-4">
          <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-xs text-gray-500">超載燈號</p>
                <p class="text-sm font-semibold text-gray-700">
                  {{ getCapacityStatusLabel(getCapacityStatus(selectedMonthlyRatio)) }}
                </p>
              </div>
              <span
                class="inline-flex items-center rounded-full px-2 py-0.5 text-xs font-semibold"
                :class="getCapacityStatusClass(getCapacityStatus(selectedMonthlyRatio))"
              >
                月負載
              </span>
            </div>
            <div class="mt-2 h-2 w-full rounded-full bg-gray-200">
              <div
                class="h-2 rounded-full transition-all"
                :class="getCapacityProgressClass(getCapacityStatus(selectedMonthlyRatio))"
                :style="{
                  width: `${Math.min(Math.round((selectedMonthlyRatio ?? 0) * 100), 100)}%`,
                }"
              />
            </div>
            <p class="text-xs text-gray-500 mt-2">綠燈 &lt; 80%｜黃燈 80%–95%｜紅燈 &gt; 95%</p>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div class="flex flex-col gap-2">
              <label class="form-label">本月工時上限</label>
              <input
                type="number"
                class="input-box"
                :disabled="!canEdit"
                :value="selectedMemberRecord.monthlyHours"
                @input="updateRecord(selectedMemberRecordIndex, (record) => ({ ...record, monthlyHours: Number(($event.target as HTMLInputElement).value) }))"
              />
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">本月人天上限</label>
              <input
                type="number"
                class="input-box"
                :disabled="!canEdit"
                :value="selectedMemberRecord.monthlyManDays"
                @input="updateRecord(selectedMemberRecordIndex, (record) => ({ ...record, monthlyManDays: Number(($event.target as HTMLInputElement).value) }))"
              />
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div class="flex flex-col gap-2">
              <label class="form-label">本週監控工時</label>
              <input
                type="number"
                class="input-box"
                :disabled="!canEdit"
                :value="selectedMemberRecord.weeklyHours"
                @input="updateRecord(selectedMemberRecordIndex, (record) => ({ ...record, weeklyHours: Number(($event.target as HTMLInputElement).value) }))"
              />
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">本週監控人天</label>
              <input
                type="number"
                class="input-box"
                :disabled="!canEdit"
                :value="selectedMemberRecord.weeklyManDays"
                @input="updateRecord(selectedMemberRecordIndex, (record) => ({ ...record, weeklyManDays: Number(($event.target as HTMLInputElement).value) }))"
              />
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
            <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
              <p class="text-xs text-gray-500">本週負載</p>
              <p class="text-lg font-semibold text-gray-700">
                {{ selectedWeeklyRatio === null ? "未設定" : `${Math.round(selectedWeeklyRatio * 100)}%` }}
              </p>
            </div>
            <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
              <p class="text-xs text-gray-500">Onsite 估算</p>
              <p class="text-lg font-semibold text-gray-700">{{ selectedEstimatedOnsite }}</p>
            </div>
            <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
              <p class="text-xs text-gray-500">Offsite 估算</p>
              <p class="text-lg font-semibold text-gray-700">{{ selectedEstimatedOffsite }}</p>
            </div>
          </div>

          <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
            <div class="flex items-center justify-between">
              <p class="text-sm font-semibold text-gray-700">驗證紅區提醒</p>
              <span class="text-xs text-gray-500">月設定</span>
            </div>
            <p class="text-xs text-gray-500 mt-2">尚未建立驗證死線或文件發行預留區。</p>
          </div>

          <div>
            <label class="form-label">可協作區</label>
            <div class="flex flex-wrap gap-2 mt-2">
              <button
                type="button"
                class="rounded-md border px-2 py-1 text-xs font-semibold transition"
                :class="selectedMemberRecord.supportRegions.includes('北區') ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                :disabled="!canEdit"
                @click="toggleSupportRegion(selectedMemberRecordIndex, '北區')"
              >
                北區
              </button>
              <button
                type="button"
                class="rounded-md border px-2 py-1 text-xs font-semibold transition"
                :class="selectedMemberRecord.supportRegions.includes('中區') ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                :disabled="!canEdit"
                @click="toggleSupportRegion(selectedMemberRecordIndex, '中區')"
              >
                中區
              </button>
              <button
                type="button"
                class="rounded-md border px-2 py-1 text-xs font-semibold transition"
                :class="selectedMemberRecord.supportRegions.includes('南區') ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                :disabled="!canEdit"
                @click="toggleSupportRegion(selectedMemberRecordIndex, '南區')"
              >
                南區
              </button>
              <button
                type="button"
                class="rounded-md border px-2 py-1 text-xs font-semibold transition"
                :class="selectedMemberRecord.supportRegions.includes('跨區') ? 'border-cyan-500 bg-cyan-50 text-cyan-700' : 'border-gray-200 text-gray-600 hover:border-cyan-300'"
                :disabled="!canEdit"
                @click="toggleSupportRegion(selectedMemberRecordIndex, '跨區')"
              >
                跨區
              </button>
            </div>
          </div>

          <div class="text-xs text-gray-500">
            最後更新：
            {{
              selectedMemberRecord.lastUpdatedAt
                ? new Date(selectedMemberRecord.lastUpdatedAt).toLocaleString("zh-TW")
                : "尚未更新"
            }}
          </div>

          <div v-if="selectedMemberStats" class="space-y-4">
            <div class="grid grid-cols-1 md:grid-cols-4 gap-3">
              <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
                <p class="text-xs text-gray-500">專案數</p>
                <p class="text-lg font-semibold text-gray-700">{{ selectedMemberStats.total }}</p>
              </div>
              <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
                <p class="text-xs text-gray-500">進行中</p>
                <p class="text-lg font-semibold text-gray-700">{{ selectedMemberStats.inProgress }}</p>
              </div>
              <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
                <p class="text-xs text-gray-500">即將結案</p>
                <p class="text-lg font-semibold text-gray-700">{{ selectedMemberStats.nearClose }}</p>
              </div>
              <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
                <p class="text-xs text-gray-500">未開始</p>
                <p class="text-lg font-semibold text-gray-700">{{ selectedMemberStats.notStarted }}</p>
              </div>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
              <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
                <p class="text-xs text-gray-500">人天估算</p>
                <p class="text-lg font-semibold text-gray-700">{{ selectedMemberStats.assignedManDays }}</p>
              </div>
              <div class="rounded-lg border border-gray-200 bg-gray-50 p-3">
                <p class="text-xs text-gray-500">工時估算</p>
                <p class="text-lg font-semibold text-gray-700">{{ selectedMemberStats.assignedHours }}</p>
              </div>
            </div>

            <div class="overflow-x-auto">
              <table class="table-base table-fixed table-sticky w-full min-w-[520px]">
                <thead class="sticky top-0 bg-white z-10">
                  <tr>
                    <th class="text-start w-40">專案名稱</th>
                    <th class="text-start w-32">客戶</th>
                    <th class="text-start w-24">起始日期</th>
                    <th class="text-start w-24">結束日期</th>
                    <th class="text-start w-24">狀態</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="selectedMemberStats.projects.length === 0">
                    <td colspan="5" class="text-center text-gray-400 py-6">目前沒有專案</td>
                  </tr>
                  <tr
                    v-for="project in selectedMemberStats.projects"
                    :key="project.id"
                    class="border border-gray-300"
                  >
                    <td class="text-start">{{ project.name }}</td>
                    <td class="text-start">{{ project.companyName }}</td>
                    <td class="text-start">{{ project.startTime }}</td>
                    <td class="text-start">{{ project.endTime }}</td>
                    <td class="text-start">{{ getEmployeeProjectStatusLabel(project.status) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="activeTab === 'cert'" class="flex flex-col gap-4">
      <div class="overflow-x-auto">
        <table class="table-base table-fixed table-sticky w-full min-w-[520px]">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-32">成員</th>
              <th class="text-start w-24">證照數量</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="visibleMemberNames.length === 0">
              <td colspan="2" class="text-center text-gray-400 py-6">目前沒有成員資料</td>
            </tr>
            <tr
              v-for="member in visibleMemberNames"
              :key="member"
              class="border border-gray-300 cursor-pointer hover:bg-cyan-50 transition-colors"
              :class="selectedCertificateMember === member ? 'bg-cyan-50' : ''"
              @click="selectedCertificateMember = member"
            >
              <td class="text-start font-semibold text-gray-700">{{ member }}</td>
              <td class="text-start text-gray-600">
                {{ certificateCountByMember.get(member) ?? 0 }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="rounded-lg border border-gray-200 bg-white p-4">
        <div class="flex items-center justify-between">
          <div>
            <h3 class="subtitle">證照明細</h3>
            <p class="text-xs text-gray-500 mt-1">{{ selectedCertificateMember || "未選擇成員" }}</p>
          </div>
        </div>
        <div class="mt-3 space-y-2">
          <div
            v-for="(cert, index) in selectedCertificateList"
            :key="`${cert.employeeName}-${index}`"
            class="rounded-md border border-gray-200 bg-gray-50 px-3 py-2 text-sm text-gray-700"
          >
            <div class="font-semibold">{{ cert.certificateName }}</div>
            <div class="text-xs text-gray-500 mt-1">取得日期：{{ cert.issueDate || "未填" }}</div>
          </div>
          <div v-if="selectedCertificateList.length === 0" class="text-sm text-gray-400">
            目前沒有證照資料
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
