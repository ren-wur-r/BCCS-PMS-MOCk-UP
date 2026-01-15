<script setup lang="ts">
import { computed, onMounted, onUnmounted, reactive, ref, watch } from "vue";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";

const nowText = ref("");
let timer: ReturnType<typeof setInterval> | null = null;
const employeeInfoStore = useEmployeeInfoStore();

const updateClock = () => {
  const now = new Date();
  const yyyy = String(now.getFullYear());
  const mm = String(now.getMonth() + 1).padStart(2, "0");
  const dd = String(now.getDate()).padStart(2, "0");
  const hh = String(now.getHours()).padStart(2, "0");
  const min = String(now.getMinutes()).padStart(2, "0");
  const ss = String(now.getSeconds()).padStart(2, "0");
  nowText.value = `${yyyy}${mm}${dd}${hh}${min}${ss}`;
};

onMounted(() => {
  updateClock();
  timer = setInterval(updateClock, 1000);
});

onUnmounted(() => {
  if (timer) clearInterval(timer);
});

const showCheckInModal = ref(false);
const showRecordModal = ref(false);
const checkInForm = reactive({
  projectId: 0,
  stageName: "",
  transport: "",
  transportOther: "",
  customerName: "",
});

const stageOptions = [
  "啟動會議",
  "資料準備與檢測規劃",
  "現場執行與資料蒐集",
  "初步分析與風險分級",
  "結果確認與收斂",
  "結案準備與交付",
];

const projectOptions = computed(() => {
  const list = mockDataSets.workProjects ?? [];
  const roleName = employeeInfoStore.effectiveRoleName || "";
  const dept = employeeInfoStore.managerDepartmentName || "";
  if (roleName.includes("顧問")) {
    const matched = list.filter((item) => item.departmentName?.includes("顧問"));
    return matched.length > 0 ? matched : list;
  }
  if (roleName.includes("工程")) {
    const matched = list.filter((item) => item.departmentName?.includes("工程"));
    return matched.length > 0 ? matched : list;
  }
  if (!dept) return list;
  const matched = list.filter((item) => item.departmentName?.includes(dept));
  return matched.length > 0 ? matched : list;
});

const selectedProject = computed(() =>
  projectOptions.value.find((item) => item.id === checkInForm.projectId) ?? null
);

const getStorageKey = (projectId: number) => `cache.work.project.checkins.v2.${projectId}`;

const readLogs = (projectId: number) => {
  if (!projectId) return [];
  const raw = localStorage.getItem(getStorageKey(projectId));
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const recordList = computed(() => readLogs(checkInForm.projectId));

const resetCheckInForm = () => {
  checkInForm.projectId = 0;
  checkInForm.stageName = "";
  checkInForm.transport = "";
  checkInForm.transportOther = "";
  checkInForm.customerName = "";
};

const openCheckIn = () => {
  showCheckInModal.value = true;
};

const closeCheckIn = () => {
  showCheckInModal.value = false;
  resetCheckInForm();
};

const openRecord = () => {
  showRecordModal.value = true;
};

const closeRecord = () => {
  showRecordModal.value = false;
};

const saveCheckIn = () => {
  if (!checkInForm.projectId || !checkInForm.stageName) return;
  const now = new Date();
  const checkedAt = now.toISOString().slice(0, 16).replace("T", " ");
  const transportValue =
    checkInForm.transport === "其他"
      ? checkInForm.transportOther || "其他"
      : checkInForm.transport;
  const projectName = selectedProject.value?.name ?? "-";
  const companyName = selectedProject.value?.companyName ?? "-";
  const next = {
    id: Date.now(),
    checkedAt,
    stageName: checkInForm.stageName,
    taskName: "-",
    location: checkInForm.customerName || "-",
    transport: transportValue || "-",
    projectName,
    companyName,
  };
  const current = readLogs(checkInForm.projectId);
  const updated = [next, ...current];
  localStorage.setItem(getStorageKey(checkInForm.projectId), JSON.stringify(updated));
  window.dispatchEvent(
    new CustomEvent("pwa-checkin-updated", { detail: { projectId: checkInForm.projectId } })
  );
  closeCheckIn();
};

const transportOptions = ["公車", "火車", "捷運", "高鐵", "自強3000", "其他"];

watch(
  () => checkInForm.projectId,
  () => {
    if (!selectedProject.value) {
      checkInForm.stageName = "";
      checkInForm.customerName = "";
      return;
    }
    checkInForm.customerName = selectedProject.value.companyName || "";
    const mid = Math.floor(stageOptions.length / 2);
    checkInForm.stageName = stageOptions[mid];
  }
);

const dateDigits = computed(() => {
  if (!nowText.value) return [];
  return [
    nowText.value.slice(0, 4),
    nowText.value.slice(4, 6),
    nowText.value.slice(6, 8),
  ];
});

const timeDigits = computed(() => {
  if (!nowText.value) return [];
  return [
    nowText.value.slice(8, 10),
    nowText.value.slice(10, 12),
    nowText.value.slice(12, 14),
  ];
});
</script>

<template>
  <div class="pwa-wrapper">
    <header class="pwa-hero">
      <div class="clock-stack">
        <div class="clock-row">
          <span v-for="(group, index) in dateDigits" :key="`d-${index}`" class="clock-group">
            <span v-for="digit in group" :key="digit + index" class="clock-tile">{{ digit }}</span>
          </span>
        </div>
        <div class="clock-row">
          <span v-for="(group, index) in timeDigits" :key="`t-${index}`" class="clock-group">
            <span v-for="digit in group" :key="digit + index" class="clock-tile">{{ digit }}</span>
          </span>
        </div>
      </div>
    </header>

    <section class="pwa-grid">
      <button class="pwa-card is-disabled" type="button" @click="openCheckIn">
        <span class="pwa-icon" aria-hidden="true">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 7v5l3 3" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.5 3l-2 2M17.5 3l2 2" />
            <circle cx="12" cy="13" r="8" stroke-width="2" />
          </svg>
        </span>
        <span class="pwa-card-title">上班</span>
      </button>
      <button class="pwa-card is-primary" type="button">
        <span class="pwa-icon" aria-hidden="true">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 7v5l3 3" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4l16 16" />
            <circle cx="12" cy="13" r="8" stroke-width="2" />
          </svg>
        </span>
        <span class="pwa-card-title">下班</span>
      </button>
      <button class="pwa-card is-alert" type="button">
        <span class="pwa-icon" aria-hidden="true">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12v7a3 3 0 01-6 0v-6a2 2 0 012-2h4z" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 5v7" />
            <circle cx="12" cy="5" r="2" stroke-width="2" />
          </svg>
        </span>
        <span class="pwa-card-title">密碼</span>
      </button>
      <button class="pwa-card is-dark" type="button">
        <span class="pwa-icon" aria-hidden="true">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 17l5-5-5-5" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12H3" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 4v16" />
          </svg>
        </span>
        <span class="pwa-card-title">離開</span>
      </button>
    </section>

    <div v-if="showCheckInModal" class="pwa-modal">
      <div class="pwa-modal-content">
        <h3>打卡資料</h3>
        <div class="form-grid">
          <div>
            <label class="form-label">專案</label>
            <select v-model.number="checkInForm.projectId" class="select-box">
              <option :value="0">請選擇專案</option>
              <option v-for="project in projectOptions" :key="project.id" :value="project.id">
                {{ project.name }}（{{ project.companyName }}）
              </option>
            </select>
          </div>
          <div>
            <label class="form-label">階段</label>
            <input v-model="checkInForm.stageName" class="input-box" disabled />
          </div>
          <div>
            <label class="form-label">客戶</label>
            <input v-model="checkInForm.customerName" class="input-box" disabled />
          </div>
          <div>
            <label class="form-label">交通方式</label>
            <div class="transport-grid">
              <button
                v-for="option in transportOptions"
                :key="option"
                type="button"
                class="transport-btn"
                :class="checkInForm.transport === option ? 'is-active' : ''"
                @click="checkInForm.transport = option"
              >
                {{ option }}
              </button>
            </div>
            <input
              v-if="checkInForm.transport === '其他'"
              v-model="checkInForm.transportOther"
              class="input-box mt-2"
              placeholder="輸入其他交通工具"
            />
          </div>
        </div>
        <div class="modal-actions">
          <button class="btn-cancel" type="button" @click="closeCheckIn">放棄</button>
          <button class="btn-submit" type="button" @click="saveCheckIn">送出</button>
        </div>
      </div>
    </div>

    <div v-if="showRecordModal" class="pwa-modal">
      <div class="pwa-modal-content">
        <h3>紀錄明細</h3>
        <div class="record-list">
          <div v-if="recordList.length === 0" class="empty">尚無打卡紀錄</div>
          <div v-for="log in recordList" :key="log.id" class="record-item">
            <div class="record-time">{{ log.checkedAt }}</div>
            <div class="record-meta">
              <span>{{ log.stageName }}</span>
              <span class="divider">|</span>
              <span>{{ log.taskName }}</span>
            </div>
            <div class="record-sub">
              {{ log.location }} · {{ log.transport }}
            </div>
          </div>
        </div>
        <div class="modal-actions">
          <button class="btn-cancel" type="button" @click="closeRecord">離開</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.pwa-wrapper {
  min-height: 100vh;
  padding: 0;
  background: url("https://images.unsplash.com/photo-1441974231531-c6227db76b6e?auto=format&fit=crop&w=1200&q=60")
    center/cover no-repeat;
  color: #f8fafc;
  font-family: "Microsoft JhengHei", "PingFang TC", "Heiti TC", sans-serif;
}

.pwa-hero {
  text-align: center;
  padding: 18px 16px 14px;
  background: rgba(15, 23, 42, 0.5);
  backdrop-filter: blur(3px);
}

.clock-stack {
  display: inline-flex;
  flex-direction: column;
  gap: 6px;
}

.clock-row {
  display: flex;
  justify-content: center;
  gap: 10px;
}

.clock-group {
  display: flex;
  gap: 6px;
}

.clock-tile {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 34px;
  height: 42px;
  border-radius: 6px;
  background: rgba(40, 40, 40, 0.85);
  color: #ffffff;
  font-size: 22px;
  font-weight: 700;
  letter-spacing: 1px;
}

.pwa-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 16px;
  padding: 16px;
  background: rgba(15, 23, 42, 0.6);
}

.pwa-card {
  border-radius: 8px;
  padding: 24px 12px;
  border: none;
  color: #fff;
  text-align: center;
  background: #9e9e9e;
  box-shadow: 0 6px 14px rgba(15, 23, 42, 0.35);
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.pwa-card.is-primary {
  background: #00897b;
}

.pwa-card.is-alert {
  background: #ef6c00;
}

.pwa-card.is-dark {
  background: #424242;
}

.pwa-card.is-disabled {
  background: #9e9e9e;
}

.pwa-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.pwa-icon svg {
  width: 88px;
  height: 88px;
}

.pwa-card-title {
  font-size: 20px;
  font-weight: 700;
}


.pwa-modal {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 16px;
  z-index: 50;
}

.pwa-modal-content {
  width: 100%;
  max-width: 520px;
  background: #f8fafc;
  color: #0f172a;
  border-radius: 16px;
  padding: 20px;
}

.form-grid {
  display: grid;
  gap: 12px;
  margin-top: 12px;
}

.record-list {
  margin-top: 12px;
  max-height: 280px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.record-item {
  border: 1px solid #e2e8f0;
  padding: 10px;
  border-radius: 10px;
  background: #fff;
}

.record-time {
  font-weight: 700;
  font-size: 13px;
}

.record-meta {
  font-size: 13px;
  color: #334155;
  margin-top: 4px;
}

.record-sub {
  font-size: 12px;
  color: #64748b;
  margin-top: 4px;
}

.divider {
  margin: 0 6px;
  color: #94a3b8;
}

.empty {
  text-align: center;
  color: #94a3b8;
  padding: 20px 0;
}

.modal-actions {
  display: flex;
  justify-content: center;
  gap: 12px;
  margin-top: 16px;
}

.transport-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.transport-btn {
  border: 1px solid #cbd5f5;
  border-radius: 999px;
  padding: 6px 12px;
  font-size: 13px;
  color: #1f2937;
  background: #f8fafc;
}

.transport-btn.is-active {
  border-color: #0f766e;
  background: #ccfbf1;
  color: #115e59;
}
</style>
