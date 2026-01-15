<script setup lang="ts">
import { computed, defineAsyncComponent } from "vue";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";

const employeeInfoStore = useEmployeeInfoStore();

const GMDashboard = defineAsyncComponent(() => import("./GMDashboard.vue"));
const ManagerDashboard = defineAsyncComponent(() => import("./ManagerDashboard.vue"));
const StaffDashboard = defineAsyncComponent(() => import("./StaffDashboard.vue"));
const SalesDashboard = defineAsyncComponent(() => import("./SalesDashboard.vue"));

const currentDashboard = computed(() => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  const managerViewRoles = ["工程部中部", "顧問中部"];

  if (roleName.includes("總經理") || roleName.includes("Admin")) {
    return GMDashboard;
  }
  if (roleName.includes("經理") || roleName.includes("處長") || managerViewRoles.includes(roleName)) {
    return ManagerDashboard;
  }
  if (roleName.includes("業務")) {
    return SalesDashboard;
  }
  return StaffDashboard;
});
</script>

<template>
  <div class="dashboard-container w-full h-full p-2.5">
    <component :is="currentDashboard" />
  </div>
</template>
