<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { loadProjectTemplateSettings, getStageTemplatesByServiceItems } from "@/stores/projectTemplateSettings";

type WorkProjectItem = {
  id: number;
  name: string;
  companyName: string;
  departmentName: string;
  status: DbAtomEmployeeProjectStatusEnum;
};

const employeeInfoStore = useEmployeeInfoStore();
const showModal = ref(false);
const activeTab = ref<"project" | "daily">("project");
const templateSettings = loadProjectTemplateSettings();

const timeSlots = ["上午", "下午"];
const projectLog = ref("");
const dailyLog = ref("");
const selectedProjectId = ref<number | null>(null);
const selectedProjectTimeSlot = ref("");
const dailyTimeSlot = ref("");
const savedMessage = ref("");
const projectLogStorageKey = "cache.worklog.project";
const dailyLogStorageKey = "cache.worklog.daily";
const projectLogList = ref<
  {
    id: number;
    projectId: number;
    projectName: string;
    milestone: string;
    timeSlot: string;
    dateLabel: string;
    timeLabel: string;
    content: string;
  }[]
>([]);
const dailyLogList = ref<
  {
    id: number;
    timeSlot: string;
    dateLabel: string;
    timeLabel: string;
    content: string;
  }[]
>([]);

const departmentName = computed(() => employeeInfoStore.managerDepartmentName || "");
const projectOptions = computed<WorkProjectItem[]>(() => {
  const list = (mockDataSets.workProjects ?? []) as WorkProjectItem[];
  if (!departmentName.value) return list;
  const filtered = list.filter((item) => item.departmentName === departmentName.value);
  return filtered.length ? filtered : list;
});

const selectedProject = computed(() =>
  projectOptions.value.find((item) => item.id === selectedProjectId.value)
);

const statusLabelMap: Record<DbAtomEmployeeProjectStatusEnum, string> = {
  [DbAtomEmployeeProjectStatusEnum.Undefined]: "未定義",
  [DbAtomEmployeeProjectStatusEnum.NotAssigned]: "未指派",
  [DbAtomEmployeeProjectStatusEnum.NotStarted]: "未開始",
  [DbAtomEmployeeProjectStatusEnum.OnSchedule]: "如期",
  [DbAtomEmployeeProjectStatusEnum.AtRisk]: "注意",
  [DbAtomEmployeeProjectStatusEnum.Delayed]: "延遲",
  [DbAtomEmployeeProjectStatusEnum.Completed]: "已完成",
};

const getServiceItemIdsForProject = (projectId: number) => {
  const raw = localStorage.getItem(`workProjectServiceItems:${projectId}`);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw) as number[];
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const milestoneStatusLabel = computed(() => {
  if (!selectedProject.value) return "-";
  const serviceItemIds = getServiceItemIdsForProject(selectedProject.value.id);
  if (serviceItemIds.length === 0) return "-";
  const templates = getStageTemplatesByServiceItems(templateSettings, serviceItemIds);
  for (const template of templates) {
    const stage = template.stages[0];
    if (stage?.stageName) return stage.stageName;
  }
  return "-";
});

const padNumber = (value: number) => value.toString().padStart(2, "0");
const nowDateLabel = computed(() => {
  const now = new Date();
  const year = now.getFullYear();
  const month = padNumber(now.getMonth() + 1);
  const day = padNumber(now.getDate());
  return `${year}/${month}/${day}`;
});
const nowTimeLabel = computed(() => {
  const now = new Date();
  const hours = padNumber(now.getHours());
  const minutes = padNumber(now.getMinutes());
  return `${hours}:${minutes}`;
});

const resetForm = () => {
  projectLog.value = "";
  dailyLog.value = "";
  selectedProjectId.value = null;
  selectedProjectTimeSlot.value = "";
  dailyTimeSlot.value = "";
  savedMessage.value = "";
};

const loadLogs = () => {
  const projectRaw = localStorage.getItem(projectLogStorageKey);
  if (projectRaw) {
    try {
      const parsed = JSON.parse(projectRaw);
      projectLogList.value = Array.isArray(parsed) ? parsed : [];
    } catch {
      projectLogList.value = [];
    }
  }
  const dailyRaw = localStorage.getItem(dailyLogStorageKey);
  if (dailyRaw) {
    try {
      const parsed = JSON.parse(dailyRaw);
      dailyLogList.value = Array.isArray(parsed) ? parsed : [];
    } catch {
      dailyLogList.value = [];
    }
  }
};

const persistLogs = () => {
  localStorage.setItem(projectLogStorageKey, JSON.stringify(projectLogList.value));
  localStorage.setItem(dailyLogStorageKey, JSON.stringify(dailyLogList.value));
};

const openModal = () => {
  resetForm();
  showModal.value = true;
};

const closeModal = () => {
  showModal.value = false;
};

const saveProjectLog = () => {
  if (!selectedProjectId.value) return;
  if (!projectLog.value.trim()) return;
  const projectName = selectedProject.value?.name ?? "-";
  const milestone = milestoneStatusLabel.value || "-";
  projectLogList.value = [
    {
      id: Date.now(),
      projectId: selectedProjectId.value,
      projectName,
      milestone,
      timeSlot: selectedProjectTimeSlot.value || "-",
      dateLabel: nowDateLabel.value,
      timeLabel: nowTimeLabel.value,
      content: projectLog.value.trim(),
    },
    ...projectLogList.value,
  ];
  persistLogs();
  savedMessage.value = "已儲存專案日誌";
  projectLog.value = "";
};

const saveDailyLog = () => {
  if (!dailyLog.value.trim()) return;
  dailyLogList.value = [
    {
      id: Date.now(),
      timeSlot: dailyTimeSlot.value || "-",
      dateLabel: nowDateLabel.value,
      timeLabel: nowTimeLabel.value,
      content: dailyLog.value.trim(),
    },
    ...dailyLogList.value,
  ];
  persistLogs();
  savedMessage.value = "已儲存日常日誌";
  dailyLog.value = "";
};

onMounted(() => {
  loadLogs();
});
</script>

<template>
  <div class="grid grid-cols-1 gap-6 lg:grid-cols-2">
    <div class="bg-white rounded-lg border border-gray-200 p-4">
      <div class="flex items-center justify-between mb-3">
        <h3 class="text-sm font-semibold text-gray-700">專案日誌</h3>
        <span class="text-xs text-gray-500">{{ projectLogList.length }} 筆</span>
      </div>
      <div v-if="projectLogList.length === 0" class="text-sm text-gray-400 text-center py-4">
        尚無專案日誌
      </div>
      <div v-else class="space-y-3">
        <div
          v-for="item in projectLogList"
          :key="item.id"
          class="rounded-lg border border-gray-200 p-3"
        >
          <div class="flex items-center justify-between text-xs text-gray-500">
            <span>{{ item.projectName }}</span>
            <span>{{ item.dateLabel }} {{ item.timeLabel }}</span>
          </div>
          <div class="mt-2 text-sm font-semibold text-gray-700">{{ item.milestone }}</div>
          <div class="mt-1 text-xs text-gray-500">時段：{{ item.timeSlot }}</div>
          <p class="mt-2 text-sm text-gray-700 whitespace-pre-wrap">{{ item.content }}</p>
        </div>
      </div>
    </div>

    <div class="bg-white rounded-lg border border-gray-200 p-4">
      <div class="flex items-center justify-between mb-3">
        <h3 class="text-sm font-semibold text-gray-700">日常日誌</h3>
        <span class="text-xs text-gray-500">{{ dailyLogList.length }} 筆</span>
      </div>
      <div v-if="dailyLogList.length === 0" class="text-sm text-gray-400 text-center py-4">
        尚無日常日誌
      </div>
      <div v-else class="space-y-3">
        <div
          v-for="item in dailyLogList"
          :key="item.id"
          class="rounded-lg border border-gray-200 p-3"
        >
          <div class="flex items-center justify-between text-xs text-gray-500">
            <span>日常</span>
            <span>{{ item.dateLabel }} {{ item.timeLabel }}</span>
          </div>
          <div class="mt-1 text-xs text-gray-500">時段：{{ item.timeSlot }}</div>
          <p class="mt-2 text-sm text-gray-700 whitespace-pre-wrap">{{ item.content }}</p>
        </div>
      </div>
    </div>
  </div>
  <button
    class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
    type="button"
    style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
    @click="openModal"
  >
    新增工作日誌
  </button>

  <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
    <div class="max-w-3xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col">
      <div class="flex items-center justify-between p-4">
        <h2 class="modal-title">新增工作日誌</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          type="button"
          @click="closeModal"
        >
          x
        </button>
      </div>
      <hr />
      <div class="p-6 overflow-y-auto flex-1 flex flex-col gap-4">
        <div class="flex gap-3">
          <button
            class="tab-btn"
            :class="{ active: activeTab === 'project' }"
            @click="activeTab = 'project'"
          >
            專案日誌
          </button>
          <button
            class="tab-btn"
            :class="{ active: activeTab === 'daily' }"
            @click="activeTab = 'daily'"
          >
            日常日誌
          </button>
        </div>

        <div v-if="activeTab === 'project'" class="flex flex-col gap-4">
          <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
            <div class="flex flex-col gap-2">
              <label class="form-label">選擇專案</label>
              <select v-model="selectedProjectId" class="select-box">
                <option value="">請選擇</option>
                <option v-for="item in projectOptions" :key="item.id" :value="item.id">
                  {{ item.name }}
                </option>
              </select>
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">里程碑進度</label>
              <input type="text" class="input-box" :value="milestoneStatusLabel" disabled />
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">時段</label>
              <select v-model="selectedProjectTimeSlot" class="select-box">
                <option value="">請選擇</option>
                <option v-for="slot in timeSlots" :key="slot" :value="slot">{{ slot }}</option>
              </select>
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">最後更新日</label>
              <div class="grid grid-cols-1 gap-2 sm:grid-cols-2">
                <input type="text" class="input-box" :value="nowDateLabel" disabled />
                <input type="text" class="input-box" :value="nowTimeLabel" disabled />
              </div>
            </div>
          </div>
          <div class="flex flex-col gap-2">
            <label class="form-label">日誌內容</label>
            <textarea
              v-model="projectLog"
              class="input-box h-28"
              placeholder="輸入專案日誌內容"
            ></textarea>
          </div>
          <button
            class="btn-submit self-center"
            type="button"
            :disabled="!selectedProjectId || !projectLog.trim()"
            @click="saveProjectLog"
          >
            儲存
          </button>
        </div>

        <div v-if="activeTab === 'daily'" class="flex flex-col gap-4">
          <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
            <div class="flex flex-col gap-2">
              <label class="form-label">時段</label>
              <select v-model="dailyTimeSlot" class="select-box">
                <option value="">請選擇</option>
                <option v-for="slot in timeSlots" :key="slot" :value="slot">{{ slot }}</option>
              </select>
            </div>
            <div class="flex flex-col gap-2">
              <label class="form-label">最後更新日</label>
              <div class="grid grid-cols-1 gap-2 sm:grid-cols-2">
                <input type="text" class="input-box" :value="nowDateLabel" disabled />
                <input type="text" class="input-box" :value="nowTimeLabel" disabled />
              </div>
            </div>
          </div>
          <div class="flex flex-col gap-2">
            <label class="form-label">日誌內容</label>
            <textarea
              v-model="dailyLog"
              class="input-box h-28"
              placeholder="輸入日常日誌內容"
            ></textarea>
          </div>
          <button
            class="btn-submit self-center"
            type="button"
            :disabled="!dailyLog.trim()"
            @click="saveDailyLog"
          >
            儲存
          </button>
        </div>

        <p v-if="savedMessage" class="text-sm text-emerald-600 text-center">{{ savedMessage }}</p>
      </div>
    </div>
  </div>
</template>
