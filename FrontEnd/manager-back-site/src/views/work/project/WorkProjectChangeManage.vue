<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { formatDate } from "@/utils/timeFormatter";

interface ProjectChangeRequestItem {
  requestId: number;
  projectId: number;
  projectName: string;
  companyName?: string;
  requesterName: string;
  requesterRole: string;
  requesterDepartment: string;
  requesterRegion: string;
  requestType: "member_adjust" | "task_adjust" | "cross_support";
  targetRegion?: string;
  targetDepartment?: string;
  reason: string;
  status: "pending" | "approved" | "rejected";
  createdAt: string;
  updatedAt?: string;
  reviewedAt?: string;
  reviewedBy?: string;
  reviewReason?: string;
}

const employeeInfoStore = useEmployeeInfoStore();
const changeRequestStorageKey = "cache.work.project.changeRequests";
const changeRequestEventName = "work-project-change-requests-updated";
const changeRequests = ref<ProjectChangeRequestItem[]>([]);
const showRejectModal = ref(false);
const rejectReason = ref("");
const rejectRequestId = ref<number | null>(null);
const rejectError = ref("");

const readChangeRequests = (): ProjectChangeRequestItem[] => {
  const raw = localStorage.getItem(changeRequestStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const writeChangeRequests = (nextList: ProjectChangeRequestItem[]) => {
  changeRequests.value = nextList;
  localStorage.setItem(changeRequestStorageKey, JSON.stringify(nextList));
  window.dispatchEvent(new CustomEvent(changeRequestEventName));
};

const pendingChangeRequests = computed(() =>
  changeRequests.value.filter((item) => item.status === "pending")
);
const changeRequestLastUpdated = computed(() => {
  if (changeRequests.value.length === 0) return "";
  const latest = changeRequests.value.reduce((acc, item) => {
    const current = new Date(item.updatedAt || item.createdAt).getTime();
    return current > acc ? current : acc;
  }, 0);
  return latest ? formatDate(new Date(latest).toISOString()) : "";
});
const canReviewChangeRequests = computed(() => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  return (
    roleName.includes("各部門經理") ||
    roleName.includes("各處處長") ||
    roleName.includes("Admin")
  );
});

const getChangeRequestTypeLabel = (type: ProjectChangeRequestItem["requestType"]) => {
  switch (type) {
    case "member_adjust":
      return "人員調整";
    case "task_adjust":
      return "工項異動";
    case "cross_support":
      return "跨部門/跨區";
    default:
      return "其他";
  }
};

const getChangeStatusClass = (status: ProjectChangeRequestItem["status"]) =>
  status === "approved"
    ? "bg-emerald-50 text-emerald-700"
    : status === "rejected"
      ? "bg-rose-50 text-rose-700"
      : "bg-amber-50 text-amber-700";

const updateChangeRequestStatus = (
  requestId: number,
  status: ProjectChangeRequestItem["status"],
  reviewReason?: string
) => {
  const reviewedAt = new Date().toISOString();
  const nextList = changeRequests.value.map((item) =>
    item.requestId === requestId
      ? {
          ...item,
          status,
          reviewedAt,
          updatedAt: reviewedAt,
          reviewedBy: employeeInfoStore.employeeName || "主管",
          reviewReason: status === "rejected" ? reviewReason : item.reviewReason,
        }
      : item
  );
  writeChangeRequests(nextList);
};


const openRejectModal = (requestId: number) => {
  rejectRequestId.value = requestId;
  rejectReason.value = "";
  rejectError.value = "";
  showRejectModal.value = true;
};

const submitReject = () => {
  if (!rejectRequestId.value) return;
  if (!rejectReason.value.trim()) {
    rejectError.value = "請輸入退回原因";
    return;
  }
  updateChangeRequestStatus(rejectRequestId.value, "rejected", rejectReason.value.trim());
  showRejectModal.value = false;
  rejectRequestId.value = null;
  rejectReason.value = "";
  rejectError.value = "";
};

const formatProjectNameDisplay = (name: string) =>
  name.replace(/\s*\+\s*/g, "\n");

const handleChangeRequestEvent = () => {
  changeRequests.value = readChangeRequests();
};
onMounted(() => {
  changeRequests.value = readChangeRequests();
  window.addEventListener(changeRequestEventName, handleChangeRequestEvent as EventListener);
});

onBeforeUnmount(() => {
  window.removeEventListener(changeRequestEventName, handleChangeRequestEvent as EventListener);
});
</script>

<template>
  <div class="flex flex-col h-full p-4 gap-4">
    <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div class="flex items-center justify-between">
        <div class="text-sm text-gray-500 flex items-center gap-3">
          <span>最後更新：{{ changeRequestLastUpdated || "無" }}</span>
          <span>待核可 {{ pendingChangeRequests.length }} 筆</span>
        </div>
      </div>

      <div v-if="changeRequests.length === 0" class="text-sm text-gray-500">
        尚未有異動申請。
      </div>
      <div v-else class="overflow-x-auto">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-32">專案</th>
              <th class="text-start w-24">類型</th>
              <th class="text-start w-24">申請人</th>
              <th class="text-start w-64">說明</th>
              <th class="text-start w-20">狀態</th>
              <th class="text-center w-32">處理</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in changeRequests" :key="item.requestId">
              <td class="text-start whitespace-pre-line">
                {{ formatProjectNameDisplay(item.projectName) }}
              </td>
              <td class="text-start">{{ getChangeRequestTypeLabel(item.requestType) }}</td>
              <td class="text-start">{{ item.requesterName }}</td>
              <td class="text-start whitespace-normal break-words">
                <p class="text-sm text-gray-700">{{ item.reason }}</p>
                <p
                  v-if="item.requestType === 'cross_support'"
                  class="text-xs text-gray-500 mt-1"
                >
                  目標：{{ item.targetDepartment || "-" }} / {{ item.targetRegion || "-" }}
                </p>
              </td>
              <td class="text-start">
                <span
                  class="inline-flex min-w-[72px] items-center justify-center rounded-md px-2 py-1 text-xs"
                  :class="getChangeStatusClass(item.status)"
                >
                  {{ item.status === "approved" ? "已核可" : item.status === "rejected" ? "已退回" : "待核可" }}
                </span>
              </td>
              <td class="text-center">
                <div class="flex items-center justify-center gap-2">
                  <button
                    class="btn-submit"
                    type="button"
                    :disabled="!canReviewChangeRequests || item.status !== 'pending'"
                    @click="updateChangeRequestStatus(item.requestId, 'approved')"
                  >
                    核可
                  </button>
                  <button
                    class="btn-cancel"
                    type="button"
                    :disabled="!canReviewChangeRequests || item.status !== 'pending'"
                    @click="openRejectModal(item.requestId)"
                  >
                    退回
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div
      v-if="showRejectModal"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4"
    >
      <div class="w-full max-w-lg rounded-lg bg-white p-6 shadow-lg">
        <div class="flex items-center justify-between">
          <h3 class="subtitle">退回原因</h3>
          <button
            type="button"
            class="text-gray-400 hover:text-gray-600"
            @click="showRejectModal = false"
          >
            ✕
          </button>
        </div>
        <div class="mt-4">
          <label class="form-label">必填原因</label>
          <textarea
            v-model="rejectReason"
            class="input-box min-h-[120px]"
            placeholder="請輸入退回原因"
          ></textarea>
          <p v-if="rejectError" class="text-xs text-rose-600 mt-1">
            {{ rejectError }}
          </p>
        </div>
        <div class="mt-4 flex justify-end gap-2">
          <button type="button" class="btn-cancel" @click="showRejectModal = false">
            取消
          </button>
          <button type="button" class="btn-submit" @click="submitReject">
            送出
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
