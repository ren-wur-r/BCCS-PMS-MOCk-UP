<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { formatDate } from "@/utils/timeFormatter";

type ReviewStatus = "not_sent" | "waiting" | "pending" | "approved" | "rejected" | "not_required";

interface ProjectFileReviewItem {
  reviewId: number;
  projectId: number;
  projectName: string;
  stageId: number;
  stageName: string;
  outputId: number;
  outputName: string;
  fileName: string;
  fileUrl: string;
  uploadedBy: string;
  uploadedAt: string;
  assistantStatus: ReviewStatus;
  managerStatus: ReviewStatus;
  overallStatus: ReviewStatus;
  assistantFeedback?: string;
  managerFeedback?: string;
  lastFeedback?: string;
  assistantReviewedAt?: string;
  assistantReviewedBy?: string;
  managerReviewedAt?: string;
  managerReviewedBy?: string;
  updatedAt: string;
  reviewTarget: "assistant" | "manager";
}

const employeeInfoStore = useEmployeeInfoStore();
const fileReviewStorageKey = "cache.work.project.fileReviews";
const fileReviewEventName = "work-project-file-reviews-updated";

const fileReviews = ref<ProjectFileReviewItem[]>([]);
const showFileRejectModal = ref(false);
const fileRejectReason = ref("");
const rejectFileId = ref<number | null>(null);
const activeReviewRole = ref<"assistant" | "manager" | null>(null);
const fileRejectError = ref("");
const showFilePreviewModal = ref(false);
const previewFile = ref<ProjectFileReviewItem | null>(null);

const normalizeReviewItem = (item: any): ProjectFileReviewItem => {
  const updatedAt = item.updatedAt || item.reviewedAt || item.uploadedAt || new Date().toISOString();
  if (item.assistantStatus && item.managerStatus) {
    return { ...item, updatedAt } as ProjectFileReviewItem;
  }
  return {
    reviewId: item.reviewId || Date.now(),
    projectId: item.projectId || 0,
    projectName: item.projectName || "未命名專案",
    stageId: item.stageId || 0,
    stageName: item.stageName || "",
    outputId: item.outputId || 0,
    outputName: item.outputName || item.fileName || "",
    fileName: item.fileName || "",
    fileUrl: item.fileUrl || "",
    uploadedBy: item.uploadedBy || "專案成員",
    uploadedAt: item.uploadedAt || updatedAt,
    assistantStatus: "not_required",
    managerStatus: item.status || "pending",
    overallStatus: item.status || "pending",
    updatedAt,
    reviewTarget: "manager",
  };
};

const readFileReviews = (): ProjectFileReviewItem[] => {
  const raw = localStorage.getItem(fileReviewStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed.map(normalizeReviewItem) : [];
  } catch {
    return [];
  }
};

const writeFileReviews = (nextList: ProjectFileReviewItem[]) => {
  fileReviews.value = nextList;
  localStorage.setItem(fileReviewStorageKey, JSON.stringify(nextList));
  window.dispatchEvent(new CustomEvent(fileReviewEventName));
};

const pendingFileReviews = computed(() =>
  fileReviews.value.filter((item) => item.overallStatus === "pending")
);
const fileReviewLastUpdated = computed(() => {
  if (fileReviews.value.length === 0) return "";
  const latest = fileReviews.value.reduce((acc, item) => {
    const current = new Date(item.updatedAt || item.uploadedAt).getTime();
    return current > acc ? current : acc;
  }, 0);
  return latest ? formatDate(new Date(latest).toISOString()) : "";
});
const canReviewAssistant = computed(() => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  return roleName.includes("助理") || roleName.includes("Admin");
});

const canReviewManager = computed(() => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  return (
    roleName.includes("各部門經理") ||
    roleName.includes("各處處長") ||
    roleName.includes("總經理") ||
    roleName.includes("Admin")
  );
});

const getStatusLabel = (status: ReviewStatus) => {
  switch (status) {
    case "pending":
      return "待審";
    case "waiting":
      return "待助理";
    case "approved":
      return "已核可";
    case "rejected":
      return "已退回";
    case "not_required":
      return "不需審";
    default:
      return "未送審";
  }
};

const getStatusClass = (status: ReviewStatus) =>
  status === "approved"
    ? "bg-emerald-50 text-emerald-700"
    : status === "rejected"
      ? "bg-rose-50 text-rose-700"
      : status === "waiting"
        ? "bg-sky-50 text-sky-700"
        : status === "pending"
          ? "bg-amber-50 text-amber-700"
          : "bg-gray-100 text-gray-600";

const computeOverallStatus = (assistantStatus: ReviewStatus, managerStatus: ReviewStatus) => {
  if (assistantStatus === "rejected" || managerStatus === "rejected") return "rejected";
  if (managerStatus === "approved" && ["approved", "not_required"].includes(assistantStatus)) {
    return "approved";
  }
  if ([assistantStatus, managerStatus].includes("pending") || managerStatus === "waiting") {
    return "pending";
  }
  if (assistantStatus === "not_required" && managerStatus === "not_required") {
    return "not_required";
  }
  if (assistantStatus === "not_sent" && managerStatus === "not_sent") {
    return "not_sent";
  }
  return "pending";
};

const updateFileReviewStatus = (
  reviewId: number,
  role: "assistant" | "manager",
  status: ReviewStatus,
  reviewReason?: string
) => {
  const reviewedAt = new Date().toISOString();
  const reviewerName = employeeInfoStore.employeeName || "主管";
  const nextList = fileReviews.value.map((item) => {
    if (item.reviewId !== reviewId) return item;
    let assistantStatus = item.assistantStatus;
    let managerStatus = item.managerStatus;
    let assistantFeedback = item.assistantFeedback;
    let managerFeedback = item.managerFeedback;
    let assistantReviewedAt = item.assistantReviewedAt;
    let assistantReviewedBy = item.assistantReviewedBy;
    let managerReviewedAt = item.managerReviewedAt;
    let managerReviewedBy = item.managerReviewedBy;

    if (role === "assistant") {
      assistantStatus = status;
      assistantReviewedAt = reviewedAt;
      assistantReviewedBy = reviewerName;
      if (status === "rejected") assistantFeedback = reviewReason;
      if (status === "approved" && managerStatus === "waiting") {
        managerStatus = "pending";
      }
    } else {
      managerStatus = status;
      managerReviewedAt = reviewedAt;
      managerReviewedBy = reviewerName;
      if (status === "rejected") managerFeedback = reviewReason;
    }

    const overallStatus = computeOverallStatus(assistantStatus, managerStatus);
    const lastFeedback = reviewReason || item.lastFeedback || "";
    return {
      ...item,
      assistantStatus,
      managerStatus,
      overallStatus,
      assistantFeedback,
      managerFeedback,
      lastFeedback,
      assistantReviewedAt,
      assistantReviewedBy,
      managerReviewedAt,
      managerReviewedBy,
      updatedAt: reviewedAt,
    };
  });
  writeFileReviews(nextList);
};

const openFileRejectModal = (reviewId: number, role: "assistant" | "manager") => {
  rejectFileId.value = reviewId;
  activeReviewRole.value = role;
  fileRejectReason.value = "";
  fileRejectError.value = "";
  showFileRejectModal.value = true;
};

const submitFileReject = () => {
  if (!rejectFileId.value || !activeReviewRole.value) return;
  if (!fileRejectReason.value.trim()) {
    fileRejectError.value = "請輸入退回原因";
    return;
  }
  updateFileReviewStatus(
    rejectFileId.value,
    activeReviewRole.value,
    "rejected",
    fileRejectReason.value.trim()
  );
  showFileRejectModal.value = false;
  rejectFileId.value = null;
  activeReviewRole.value = null;
  fileRejectReason.value = "";
  fileRejectError.value = "";
};

const openFilePreview = (item: ProjectFileReviewItem) => {
  previewFile.value = item;
  showFilePreviewModal.value = true;
};

const getFileExtension = (url: string) => {
  const parts = url.split("?")[0].split(".");
  return parts.length > 1 ? parts[parts.length - 1].toLowerCase() : "";
};

const isPreviewableImage = (url: string) =>
  ["png", "jpg", "jpeg", "gif", "webp"].includes(getFileExtension(url));
const isPreviewablePdf = (url: string) => getFileExtension(url) === "pdf";

const seedFileReviews = () => {
  const existing = readFileReviews();
  if (existing.length > 0) return;
  const now = new Date().toISOString();
  const seed: ProjectFileReviewItem[] = [
    {
      reviewId: 9001,
      projectId: 1,
      projectName: "顧問諮詢-ISMS 導入",
      stageId: 101,
      stageName: "合約與需求審閱",
      outputId: 1001,
      outputName: "合約重點摘要",
      fileName: "合約重點摘要.pdf",
      fileUrl: "https://files.example.com/review/contract-summary.pdf",
      uploadedBy: "王怡君",
      uploadedAt: now,
      assistantStatus: "pending",
      managerStatus: "waiting",
      overallStatus: "pending",
      updatedAt: now,
      reviewTarget: "assistant",
    },
    {
      reviewId: 9002,
      projectId: 2,
      projectName: "顧問諮詢-資安治理",
      stageId: 102,
      stageName: "計畫書與起始會議準備",
      outputId: 1002,
      outputName: "起始會議紀錄",
      fileName: "起始會議紀錄.pdf",
      fileUrl: "https://files.example.com/review/kickoff-notes.pdf",
      uploadedBy: "林沛鑑",
      uploadedAt: now,
      assistantStatus: "not_required",
      managerStatus: "pending",
      overallStatus: "pending",
      updatedAt: now,
      reviewTarget: "manager",
    },
  ];
  writeFileReviews(seed);
};

const formatProjectNameDisplay = (name: string) =>
  name.replace(/\s*\+\s*/g, "\n");

const handleFileReviewEvent = () => {
  fileReviews.value = readFileReviews();
};

onMounted(() => {
  seedFileReviews();
  fileReviews.value = readFileReviews();
  window.addEventListener(fileReviewEventName, handleFileReviewEvent as EventListener);
});

onBeforeUnmount(() => {
  window.removeEventListener(fileReviewEventName, handleFileReviewEvent as EventListener);
});
</script>

<template>
  <div class="flex flex-col h-full p-4 gap-4">
    <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div class="flex items-center justify-between">
        <div class="text-sm text-gray-500 flex items-center gap-3">
          <span>最後更新：{{ fileReviewLastUpdated || "無" }}</span>
          <span>待核可 {{ pendingFileReviews.length }} 筆</span>
        </div>
      </div>

      <div v-if="fileReviews.length === 0" class="text-sm text-gray-500">
        尚未有文件待審核。
      </div>
      <div v-else class="overflow-x-auto">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-36">專案</th>
              <th class="text-start w-32">階段</th>
              <th class="text-start w-40">文件</th>
              <th class="text-start w-24">上傳人</th>
              <th class="text-start w-28">上傳時間</th>
              <th class="text-start w-20">助理</th>
              <th class="text-start w-20">經理</th>
              <th class="text-start w-20">總體</th>
              <th class="text-center w-32">處理</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in fileReviews" :key="item.reviewId">
              <td class="text-start whitespace-pre-line">
                {{ formatProjectNameDisplay(item.projectName) }}
              </td>
              <td class="text-start">
                <div class="text-sm text-gray-700">{{ item.stageName || "-" }}</div>
              </td>
              <td class="text-start">
                <div class="text-sm text-gray-700">{{ item.outputName || item.fileName }}</div>
                <div class="text-xs text-gray-500 mt-1">{{ item.fileName }}</div>
                <button
                  v-if="item.fileUrl"
                  type="button"
                  class="text-xs text-cyan-700 hover:text-cyan-800 mt-1"
                  @click="openFilePreview(item)"
                >
                  預覽
                </button>
                <span v-else class="mt-1 text-xs text-gray-400 inline-block">無預覽</span>
              </td>
              <td class="text-start">{{ item.uploadedBy }}</td>
              <td class="text-start">{{ formatDate(item.uploadedAt) || "-" }}</td>
              <td class="text-start">
                <span
                  class="inline-flex min-w-[72px] items-center justify-center rounded-md px-2 py-1 text-xs"
                  :class="getStatusClass(item.assistantStatus)"
                >
                  {{ getStatusLabel(item.assistantStatus) }}
                </span>
              </td>
              <td class="text-start">
                <span
                  class="inline-flex min-w-[72px] items-center justify-center rounded-md px-2 py-1 text-xs"
                  :class="getStatusClass(item.managerStatus)"
                >
                  {{ getStatusLabel(item.managerStatus) }}
                </span>
              </td>
              <td class="text-start">
                <span
                  class="inline-flex min-w-[72px] items-center justify-center rounded-md px-2 py-1 text-xs"
                  :class="getStatusClass(item.overallStatus)"
                >
                  {{ getStatusLabel(item.overallStatus) }}
                </span>
              </td>
              <td class="text-center">
                <div
                  v-if="canReviewAssistant"
                  class="flex items-center justify-center gap-2"
                >
                  <button
                    class="btn-submit"
                    type="button"
                    :disabled="item.assistantStatus !== 'pending'"
                    @click="updateFileReviewStatus(item.reviewId, 'assistant', 'approved')"
                  >
                    核可
                  </button>
                  <button
                    class="btn-cancel"
                    type="button"
                    :disabled="item.assistantStatus !== 'pending'"
                    @click="openFileRejectModal(item.reviewId, 'assistant')"
                  >
                    退回
                  </button>
                </div>
                <div
                  v-else-if="canReviewManager"
                  class="flex items-center justify-center gap-2"
                >
                  <button
                    class="btn-submit"
                    type="button"
                    :disabled="item.managerStatus !== 'pending'"
                    @click="updateFileReviewStatus(item.reviewId, 'manager', 'approved')"
                  >
                    核可
                  </button>
                  <button
                    class="btn-cancel"
                    type="button"
                    :disabled="item.managerStatus !== 'pending'"
                    @click="openFileRejectModal(item.reviewId, 'manager')"
                  >
                    退回
                  </button>
                </div>
                <span v-else class="text-xs text-gray-400">無權限</span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="showFilePreviewModal && previewFile" class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4">
      <div class="w-full max-w-3xl rounded-lg bg-white p-4 shadow-lg">
        <div class="flex items-center justify-between border-b pb-2">
          <h3 class="text-sm font-semibold text-gray-700">{{ previewFile.fileName }}</h3>
          <button class="btn-cancel" type="button" @click="showFilePreviewModal = false">
            關閉
          </button>
        </div>
        <div class="mt-4 max-h-[60vh] overflow-auto">
          <div v-if="!previewFile.fileUrl" class="text-sm text-gray-600">
            未提供預覽連結。
          </div>
          <iframe
            v-else-if="isPreviewablePdf(previewFile.fileUrl)"
            :src="previewFile.fileUrl"
            class="w-full h-[60vh] rounded"
          ></iframe>
          <img
            v-else-if="isPreviewableImage(previewFile.fileUrl)"
            :src="previewFile.fileUrl"
            :alt="previewFile.fileName"
            class="max-h-[60vh] w-full rounded object-contain"
          />
          <div v-else class="text-sm text-gray-600">
            無法預覽此格式，請下載查看：
            <a :href="previewFile.fileUrl" class="text-cyan-700 underline" target="_blank">
              下載文件
            </a>
          </div>
        </div>
      </div>
    </div>

    <div v-if="showFileRejectModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4">
      <div class="w-full max-w-md rounded-lg bg-white p-4 shadow-lg">
        <h3 class="text-sm font-semibold text-gray-700">退回原因</h3>
        <textarea
          v-model="fileRejectReason"
          class="input-box mt-3 min-h-[120px]"
          placeholder="請輸入退回原因"
        ></textarea>
        <p v-if="fileRejectError" class="text-xs text-rose-600 mt-1">{{ fileRejectError }}</p>
        <div class="mt-4 flex justify-end gap-2">
          <button class="btn-cancel" type="button" @click="showFileRejectModal = false">
            取消
          </button>
          <button class="btn-submit" type="button" @click="submitFileReject">確定退回</button>
        </div>
      </div>
    </div>
  </div>
</template>
