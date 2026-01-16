<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useRoute } from "vue-router";
import Sidebar from "@/components/global/layout/Sidebar.vue";
import Navbar from "@/components/global/layout/Navbar.vue";
import { getModuleInfoByPath } from "@/constants/moduleInfo";
import AnnotationOverlay from "@/components/global/annotation/AnnotationOverlay.vue";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";

// 側邊欄狀態：預設展開
const isSidebarOpen = ref(true);
const showModuleInfo = ref(import.meta.env.VITE_SHOW_MODULE_INFO === "true");
const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
const route = useRoute();
const { moduleTitleOverride } = useModuleTitleStore();
const moduleInfo = computed(() => getModuleInfoByPath(route.path));
const moduleTitle = computed(() => {
  return (
    moduleTitleOverride.value ||
    String(route.meta?.title ?? moduleInfo.value?.title ?? "模組")
  );
});
const noteKey = computed(() => `module-note:${route.path}`);
const draftNote = ref("");

const loadNote = () => {
  const raw = localStorage.getItem(noteKey.value);
  draftNote.value = raw ? String(raw) : "";
};

const toggleModuleInfo = () => {
  showModuleInfo.value = !showModuleInfo.value;
  localStorage.setItem("module-info-visible", showModuleInfo.value ? "true" : "false");
};

watch(
  () => route.path,
  () => {
    loadNote();
  },
  { immediate: true }
);

watch(
  () => draftNote.value,
  (value) => {
    const trimmed = value.trim();
    if (trimmed) {
      localStorage.setItem(noteKey.value, trimmed);
    } else {
      localStorage.removeItem(noteKey.value);
    }
  }
);

const storedModuleInfoVisible = localStorage.getItem("module-info-visible");
if (storedModuleInfoVisible === "true" || storedModuleInfoVisible === "false") {
  showModuleInfo.value = storedModuleInfoVisible === "true";
}

/** 側邊欄切換展開/收合 */
const toggleSidebar = () => {
  isSidebarOpen.value = !isSidebarOpen.value;
};
</script>

<template>
  <div class="app-container">
    <Navbar @toggle-sidebar="toggleSidebar" />
    <div
      class="content-container"
      :class="{ 'sidebar-collapsed': !isSidebarOpen, 'sidebar-expanded': isSidebarOpen }"
    >
      <div class="sidebar">
        <Sidebar :is-collapsed="!isSidebarOpen" @toggle-sidebar="toggleSidebar" />
      </div>

      <main class="main-content">
        <div v-if="showModuleInfo" class="module-info">
          <div class="module-info__header">
            <div class="module-info__title">模組說明 - {{ moduleTitle }}</div>
            <button class="module-info__toggle" type="button" @click="toggleModuleInfo">
              隱藏
            </button>
          </div>
          <div class="module-info__body">
            <div class="module-info__row">
              <span class="module-info__label">說明</span>
              <div class="module-info__note">
                <textarea
                  v-model="draftNote"
                  rows="3"
                  class="module-info__textarea"
                  placeholder="輸入模組說明（開發者可直接查看）"
                ></textarea>
              </div>
            </div>
          </div>
        </div>
        <button v-else class="module-info__show" type="button" @click="toggleModuleInfo">
          顯示模組說明
        </button>
        <router-view :key="$route.fullPath" />
        <AnnotationOverlay v-if="useMockData" />
      </main>
    </div>
  </div>
</template>

<style scoped>
.app-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.content-container {
  display: flex;
  flex: 1;
  width: 100%;
  height: calc(100vh - 60px);
  margin-top: 60px;
  background-color: #f3f4f6;
  padding-left: 208px;
  box-sizing: border-box;
  transition:
    margin-left 0.3s ease-in-out,
    width 0.3s ease-in-out,
    padding-left 0.3s ease-in-out;
}

/* 預設 sidebar 展開時 */
.main-content {
  flex-grow: 1;
  padding: 10px;
  height: 100%;
  width: 100%;
  margin-left: 0;
  box-sizing: border-box;
  overflow-y: auto;
  transition:
    margin-left 0.3s ease-in-out,
    width 0.3s ease-in-out;
}

.module-info {
  border: 1px dashed #94a3b8;
  background: #f8fafc;
  border-radius: 10px;
  padding: 12px 14px;
  margin-bottom: 10px;
  color: #0f172a;
}

.module-info__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
}

.module-info__title {
  font-size: 14px;
  font-weight: 600;
}

.module-info__toggle {
  border: 1px solid #cbd5f5;
  background: #ffffff;
  color: #1e293b;
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 12px;
  cursor: pointer;
}

.module-info__show {
  border: 1px dashed #94a3b8;
  background: #f8fafc;
  color: #0f172a;
  border-radius: 999px;
  padding: 6px 12px;
  font-size: 12px;
  cursor: pointer;
  margin-bottom: 10px;
}

.module-info__body {
  margin-top: 8px;
  display: grid;
  gap: 6px;
}

.module-info__row {
  display: flex;
  gap: 8px;
  align-items: flex-start;
}

.module-info__label {
  min-width: 80px;
  font-size: 12px;
  color: #64748b;
}

.module-info__note {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.module-info__textarea {
  width: 100%;
  min-height: 64px;
  border: 1px solid #cbd5f5;
  border-radius: 8px;
  padding: 8px;
  font-size: 12px;
  resize: vertical;
}

/* 當 sidebar 收合時 */
.sidebar-collapsed.content-container {
  padding-left: 56px;
}

.sidebar-collapsed .main-content {
  padding: 10px;
}

.sidebar {
  width: 0;
  flex-shrink: 0;
}

.sidebar-collapsed .sidebar {
  width: 0;
}

/* 平板尺寸 */
@media (max-width: 768px) {
  .sidebar {
    width: 100%;
    z-index: 1000;
  }

  .main-content {
    margin-left: 0;
    width: 100%;
    padding: 10px;
  }

  .content-container {
    padding-left: 0;
  }

  .sidebar-collapsed .sidebar {
    width: 100%;
  }
}
</style>
