<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";

const props = defineProps<{ isCollapsed?: boolean }>();
defineEmits<{
  (event: "toggle-sidebar"): void;
}>();

const router = useRouter();
const route = useRoute();
const employeeInfoStore = useEmployeeInfoStore();
const pendingAssignCount = ref(0);
const assignmentStorageKey = "cache.work.project.assignments";
const assignmentEventName = "work-project-assignments-updated";
const changeRequestStorageKey = "cache.work.project.changeRequests";
const changeRequestEventName = "work-project-change-requests-updated";
const pendingChangeCount = ref(0);

const readAssignments = () => {
  const raw = localStorage.getItem(assignmentStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const readChangeRequests = () => {
  const raw = localStorage.getItem(changeRequestStorageKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

const isCollapsed = computed(() => props.isCollapsed ?? false);

const ensureAssignmentSeed = () => {
  const existing = readAssignments();
  if (existing.length > 0) return;
  const snapshotRaw = localStorage.getItem("cache.work.project.snapshot");
  const snapshot = snapshotRaw ? JSON.parse(snapshotRaw) : [];
  const firstProject = snapshot[0];
  const seedProject = firstProject ?? {
    id: 1,
    name: "雲安維運-EDR",
    companyName: "王品有限公司",
    startTime: "2026-01-01",
    endTime: "2026-01-31",
  };
  const seedAssignment = {
    assignmentId: Date.now(),
    projectId: seedProject.id,
    projectName: seedProject.name,
    companyName: seedProject.companyName,
    regionName: employeeInfoStore.managerRegionName || "中區",
    departmentName: employeeInfoStore.managerDepartmentName || "策略執行部",
    assigneeName: employeeInfoStore.employeeName || "目前使用者",
    status: "pending",
    createdAt: new Date().toISOString(),
  };
  localStorage.setItem(assignmentStorageKey, JSON.stringify([seedAssignment]));
  window.dispatchEvent(new CustomEvent(assignmentEventName));
};

const refreshPendingAssignCount = () => {
  const assignments = readAssignments();
  pendingAssignCount.value = assignments.filter((item) => item?.status === "pending").length;
};
const refreshPendingChangeCount = () => {
  const changeRequests = readChangeRequests();
  pendingChangeCount.value = changeRequests.filter((item) => item?.status === "pending").length;
};
//--------------------------------------------------------------
/** 元件-全域-側邊選單-頁面模型 */
interface SidebarViewMdl {
  groupTitle: string;
  groupKey: string;
  menuItemList: {
    menuLabel: string;
    routePath: string;
    requiredPermission?: DbAtomMenuEnum;
    requiredPermissions?: DbAtomMenuEnum[];
  }[];
}

/** 元件-全域-側邊選單-頁面物件 */
const sidebarObj: SidebarViewMdl[] = [
  {
    groupTitle: "業務",
    groupKey: "business",
    menuItemList: [
      {
        menuLabel: "商機管理",
        routePath: "/crm/pipeline/pipeline",
        requiredPermission: DbAtomMenuEnum.CrmPipeline,
      },
      {
        menuLabel: "電銷轉派",
        routePath: "/crm/pipeline/handoff",
        requiredPermission: DbAtomMenuEnum.CrmPipeline,
      },
    ],
  },
  {
    groupTitle: "專案管理",
    groupKey: "work",
    menuItemList: [
      {
        menuLabel: "所有專案",
        routePath: "/work/project",
        requiredPermission: DbAtomMenuEnum.WorkProject,
      },
      {
        menuLabel: "文件管理",
        routePath: "/work/project/file",
        requiredPermission: DbAtomMenuEnum.WorkProject,
      },
      {
        menuLabel: "指派管理",
        routePath: "/work/project/assign",
        requiredPermission: DbAtomMenuEnum.WorkProject,
      },
      {
        menuLabel: "異動管理",
        routePath: "/work/project/change",
        requiredPermission: DbAtomMenuEnum.WorkProject,
      },
      {
        menuLabel: "部門成員",
        routePath: "/work/capacity",
        requiredPermission: DbAtomMenuEnum.WorkProject,
      },
      {
        menuLabel: "工項管理",
        routePath: "/work/job",
        requiredPermission: DbAtomMenuEnum.WorkProject,
      },
    ],
  },
  {
    groupTitle: "行銷管理",
    groupKey: "crm",
    menuItemList: [
      {
        menuLabel: "電銷管理",
        routePath: "/crm/phone/activity",
        requiredPermission: DbAtomMenuEnum.CrmPhone,
      },
      {
        menuLabel: "活動管理",
        routePath: "/crm/activity/activity",
        requiredPermission: DbAtomMenuEnum.CrmActivity,
      },
    ],
  },
  {
    groupTitle: "系統設定",
    groupKey: "system",
    menuItemList: [
      {
        menuLabel: "基礎資料",
        routePath: "/system/master",
        requiredPermissions: [
          DbAtomMenuEnum.SystemContacter,
          DbAtomMenuEnum.SystemCompany,
          DbAtomMenuEnum.SystemProduct,
          DbAtomMenuEnum.SystemEmployee,
          DbAtomMenuEnum.SystemRole,
          DbAtomMenuEnum.SystemDepartment,
          DbAtomMenuEnum.SystemRegion,
        ],
      },
      {
        menuLabel: "專案標準階段",
        routePath: "/system/project-template",
        requiredPermissions: [
          DbAtomMenuEnum.SystemContacter,
          DbAtomMenuEnum.SystemCompany,
          DbAtomMenuEnum.SystemProduct,
          DbAtomMenuEnum.SystemEmployee,
          DbAtomMenuEnum.SystemRole,
          DbAtomMenuEnum.SystemDepartment,
          DbAtomMenuEnum.SystemRegion,
        ],
      },
    ],
  },
];

const roleRouteAllowList: Record<string, string[]> = {
  Admin: [],
  總經理: [],
  各處處長: [
    "/crm/pipeline/pipeline",
    "/crm/pipeline/handoff",
    "/work/project",
    "/work/project/assign",
    "/work/project/change",
    "/work/capacity",
    "/work/job",
    "/crm/phone/activity",
    "/crm/activity/activity",
  ],
  各部門經理: [
    "/work/project",
    "/work/project/assign",
    "/work/project/change",
    "/work/capacity",
    "/work/job",
    "/system/project-template",
  ],
  工程部中部: [
    "/work/project",
    "/work/project/assign",
    "/work/project/change",
    "/work/capacity",
    "/work/job",
    "/system/project-template",
  ],
  顧問中部: [
    "/work/project",
    "/work/project/assign",
    "/work/project/change",
    "/work/capacity",
    "/work/job",
    "/system/project-template",
  ],
  活動人員: ["/crm/activity/activity", "/work/capacity"],
  電銷人員: ["/crm/phone/activity", "/crm/phone/pool", "/work/capacity"],
  業務: [
    "/crm/pipeline/pipeline",
    "/crm/pipeline/handoff",
    "/work/project",
    "/system/master",
    "/crm/phone/pool",
  ],
  顧問: ["/work/project", "/work/job"],
  產品經理: ["/work/project", "/work/job", "/work/capacity"],
  工程師: ["/work/project", "/work/job"],
  資深顧問: ["/work/project", "/work/job", "/work/capacity"],
};

const isRouteAllowedByRole = (routePath: string) => {
  const roleName = employeeInfoStore.effectiveRoleName || "";
  if (!roleName || roleName === "Admin" || roleName === "總經理") return true;
  const allowList = roleRouteAllowList[roleName];
  if (!allowList) return true;
  return allowList.includes(routePath);
};

/** 顯示的 sidebar 項目 */
const visibleSidebarGroups = computed(() =>
  sidebarObj
    .map((group) => ({
      ...group,
      menuItemList: group.menuItemList.filter((menuItem) => {
        if (!isRouteAllowedByRole(menuItem.routePath)) return false;
        if (menuItem.requiredPermissions?.length) {
          return menuItem.requiredPermissions.some((permission) =>
            employeeInfoStore.hasPermission(permission, "view")
          );
        }
        if (menuItem.requiredPermission) {
          return employeeInfoStore.hasPermission(menuItem.requiredPermission, "view");
        }
        return false;
      }),
    }))
    .filter((group) => group.menuItemList.length > 0)
);

const isActiveRoute = (routePath: string) => {
  if (routePath === "/") return route.path === "/";
  if (routePath === "/work/project") {
    return (
      route.path === "/work/project" ||
      (route.path.startsWith("/work/project/") &&
        !route.path.startsWith("/work/project/file") &&
        !route.path.startsWith("/work/project/assign") &&
        !route.path.startsWith("/work/project/change"))
    );
  }
  if (routePath === "/work/project/file") {
    return route.path === "/work/project/file" || route.path.startsWith("/work/project/file/");
  }
  if (routePath === "/work/project/assign") {
    return (
      route.path === "/work/project/assign" || route.path.startsWith("/work/project/assign/")
    );
  }
  if (routePath === "/work/project/change") {
    return (
      route.path === "/work/project/change" || route.path.startsWith("/work/project/change/")
    );
  }
  if (routePath === "/work/capacity") {
    return route.path === "/work/capacity" || route.path.startsWith("/work/capacity/");
  }
  if (routePath === "/work/job") {
    return route.path === "/work/job" || route.path.startsWith("/work/job/");
  }
  return route.path.startsWith(routePath);
};

onMounted(() => {
  ensureAssignmentSeed();
  refreshPendingAssignCount();
  refreshPendingChangeCount();
  window.addEventListener(assignmentEventName, refreshPendingAssignCount as EventListener);
  window.addEventListener(changeRequestEventName, refreshPendingChangeCount as EventListener);
});

onBeforeUnmount(() => {
  window.removeEventListener(assignmentEventName, refreshPendingAssignCount as EventListener);
  window.removeEventListener(changeRequestEventName, refreshPendingChangeCount as EventListener);
});

watch(
  () => route.fullPath,
  () => {
    refreshPendingAssignCount();
    refreshPendingChangeCount();
  }
);
</script>

<template>
  <aside
    class="fixed top-[60px] left-0 h-[calc(100vh-60px)] bg-white text-gray-900 p-4 overflow-y-auto overflow-x-hidden transition-all duration-200"
    :class="isCollapsed ? 'w-14' : 'w-52'"
  >
    <div>
      <!-- 首頁連結 -->
      <div class="mb-2">
        <div class="flex items-center justify-start px-2 pb-1">
          <button
            class="flex items-center justify-center rounded text-gray-500 hover:text-cyan-600"
            type="button"
            @click="$emit('toggle-sidebar')"
          >
            <svg
              class="h-4 w-4 transition-transform duration-200"
              :class="isCollapsed ? 'rotate-180' : ''"
              viewBox="0 0 21 17"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M10.8516 13.107L5.59331 8.11284L10.8516 3.11864"
                stroke="#99A1AF"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              <path
                d="M9.21094 8.16484L9.25501 8.11286"
                stroke="#99A1AF"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              <path
                d="M14.4805 13.107L8.85112 8.11284L14.4805 3.11864"
                stroke="#99A1AF"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
          </button>
        </div>
        <div class="flex items-center p-2 rounded-md">
          <router-link
            to="/home"
            class="flex items-center gap-1 text-black no-underline text-sm font-bold transition-colors hover:text-cyan-600"
          >
            <span v-if="!isCollapsed">首頁</span>
          </router-link>
        </div>
      </div>

      <!-- 選單群組 -->
      <div
        v-for="group in visibleSidebarGroups"
        :key="group.groupKey"
        class="mb-4"
        v-show="!isCollapsed"
      >
        <!-- 群組標題 -->
        <div class="text-sm font-bold text-gray-700 mb-2 px-2">
          {{ group.groupTitle }}
        </div>

        <!-- 選單項目 -->
        <nav>
          <ul class="list-none p-0 m-0">
            <li
              v-for="menuItem in group.menuItemList"
              :key="menuItem.routePath"
              class="mb-1 cursor-pointer rounded-md transition-colors"
              :class="isActiveRoute(menuItem.routePath) ? 'bg-cyan-50' : 'hover:bg-gray-100'"
              @click="router.push(menuItem.routePath)"
            >
              <div class="flex items-center justify-between p-2 ml-6">
                <div class="flex items-center gap-1">
                  <span
                    class="text-sm"
                    :class="isActiveRoute(menuItem.routePath) ? 'text-cyan-700 font-semibold' : 'text-black'"
                  >
                    {{ menuItem.menuLabel }}
                  </span>
                  <span
                    v-if="
                      menuItem.routePath === '/work/project/assign' && pendingAssignCount > 0
                    "
                    class="ml-1 inline-flex h-2 w-2 rounded-full bg-red-500"
                  ></span>
                  <span
                    v-if="
                      menuItem.routePath === '/work/project/change' && pendingChangeCount > 0
                    "
                    class="ml-1 inline-flex h-2 w-2 rounded-full bg-red-500"
                  ></span>
                </div>
              </div>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  </aside>
</template>
