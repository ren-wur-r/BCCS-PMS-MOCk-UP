<script setup lang="ts">
import { computed } from "vue";
import { useRoute, useRouter } from "vue-router";

interface ActivityDetailTabsProps {
  /** 活動 ID */
  activityId: number;
  /** 模組基底路徑，例如 /crm/activity/activity 或 /crm/phone/activity */
  basePath: string;
}

const props = defineProps<ActivityDetailTabsProps>();
const route = useRoute();
const router = useRouter();

const tabs = computed(() => [
  {
    key: "activity",
    label: "檢視活動",
    path: `${props.basePath}/detail/${props.activityId}`,
  },
  {
    key: "pipeline",
    label: "檢視名單",
    path: `${props.basePath}/detail/${props.activityId}/pipeline`,
  },
]);

const activeTab = computed(() => (route.path.includes("/pipeline") ? "pipeline" : "activity"));

const switchTab = (tabKey: string) => {
  const target = tabs.value.find((tab) => tab.key === tabKey);
  if (!target || route.path === target.path) return;
  router.push(target.path);
};
</script>

<template>
  <div class="flex gap-3 border-b border-gray-200">
    <button
      v-for="tab in tabs"
      :key="tab.key"
      class="px-4 py-2 text-sm font-semibold rounded-t-md"
      :class="
        activeTab === tab.key
          ? 'bg-white text-blue-600 border border-b-white border-gray-200'
          : 'text-gray-500 hover:text-gray-700'
      "
      @click="switchTab(tab.key)"
    >
      {{ tab.label }}
    </button>
  </div>
</template>
