<script setup lang="ts">
import { computed, ref, watch, onBeforeUnmount } from "vue";
import SystemContacterList from "@/views/system/contacter/SystemContacterList.vue";
import SystemCompanyList from "@/views/system/company/SystemCompanyList.vue";
import SystemProductList from "@/views/system/product/SystemProductList.vue";
import SystemEmployeeList from "@/views/system/employee/SystemEmployeeList.vue";
import SystemRoleList from "@/views/system/role/SystemRoleList.vue";
import SystemDepartmentList from "@/views/system/department/SystemDepartmentList.vue";
import SystemRegionList from "@/views/system/region/SystemRegionList.vue";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";

const tabs = [
  { key: "contacter", label: "窗口", component: SystemContacterList },
  { key: "company", label: "客戶", component: SystemCompanyList },
  { key: "product", label: "產品", component: SystemProductList },
  { key: "employee", label: "員工", component: SystemEmployeeList },
  { key: "role", label: "角色", component: SystemRoleList },
  { key: "department", label: "部門", component: SystemDepartmentList },
  { key: "region", label: "地區", component: SystemRegionList },
];

const activeTabKey = ref(tabs[0].key);
const activeTab = computed(() => tabs.find((tab) => tab.key === activeTabKey.value) ?? tabs[0]);
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();

watch(
  () => activeTab.value,
  (tab) => {
    setModuleTitle(tab.label);
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});
</script>

<template>
  <div class="flex flex-col h-full p-4 gap-4"><div class="flex gap-3 flex-wrap">
      <button
        v-for="tab in tabs"
        :key="tab.key"
        class="tab-btn"
        :class="{ active: activeTabKey === tab.key }"
        @click="activeTabKey = tab.key"
      >
        {{ tab.label }}
      </button>
    </div>

    <div class="tab-content flex-1">
      <keep-alive>
        <component :is="activeTab.component" :key="activeTab.key" />
      </keep-alive>
    </div>
  </div>
</template>
