import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";

const routes: Array<RouteRecordRaw> = [
  // #region 登入
  { path: "/", redirect: useMockData ? "/home" : "/login" },
  ...(useMockData
    ? [
        {
          path: "/login",
          redirect: "/home",
        },
      ]
    : [
        {
          path: "/login",
          component: () => import("@/views/basic/Login.vue"),
          meta: { title: "登入", layout: "LoginLayout" },
        },
      ]),
  //#region 儀表板
  {
    path: "/home",
    component: () => import("@/layouts/DefaultLayout.vue"),
    meta: { requiresAuth: true },
    children: [
      {
        path: "",
        component: () => import("@/views/home/Home.vue"),
        meta: { title: "首頁" },
      },
    ],
  },
  //#region 專案管理
  {
    path: "/work",
    component: () => import("@/layouts/DefaultLayout.vue"),
    meta: { requiresAuth: true },
    children: [
      {
        path: "capacity",
        meta: { title: "部門成員" },
        component: () => import("@/views/work/capacity/WorkCapacityManage.vue"),
      },
      {
        path: "project",
        meta: { title: "專案管理" },
        children: [
          {
            path: "",
            meta: { title: "所有專案" },
            component: () => import("@/views/work/project/WorkProjectList.vue"),
          },
          {
            path: "file",
            meta: { title: "文件管理" },
            component: () => import("@/views/work/project/WorkProjectFileManage.vue"),
          },
          {
            path: "assign",
            meta: { title: "指派管理" },
            component: () => import("@/views/work/project/WorkProjectAssign.vue"),
          },
          {
            path: "change",
            meta: { title: "異動管理" },
            component: () => import("@/views/work/project/WorkProjectChangeManage.vue"),
          },
          {
            path: "add",
            component: () => import("@/views/work/project/WorkProjectAdd.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/work/project/WorkProjectDetail.vue"),
          },
          {
            path: "poc/:pipelineId",
            component: () => import("@/views/work/project/WorkProjectPocDetail.vue"),
          },
          {
            path: "templates",
            redirect: "/system/project-template",
          },
        ],
      },
       {
        path: "job",
        meta: { title: "工項管理" },
        children: [
          {
            path: "",
            component: () => import("@/views/work/job/WorkJobList.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/work/job/WorkJobDetail.vue"),
          },
        ],
      },
    ],
  },
  // #region PWA
  {
    path: "/pwa",
    component: () => import("@/layouts/BlankLayout.vue"),
    meta: { requiresAuth: true },
    children: [
      {
        path: "checkin",
        component: () => import("@/views/pwa/CheckInPwa.vue"),
        meta: { title: "打卡" },
      },
    ],
  },
  // #region CRM
  {
    path: "/crm",
    component: () => import("@/layouts/DefaultLayout.vue"),
    meta: { requiresAuth: true },
    children: [
      {
        path: "activity",
        meta: { title: "活動管理" },
        children: [
          {
            path: "activity",
            meta: { title: "活動" },
            children: [
              {
                path: "",
                component: () =>
                  import("@/views/crm/activity/activity/CrmActivityActivityList.vue"),
              },
              {
                path: "add",
                component: () => import("@/views/crm/activity/activity/CrmActivityActivityAdd.vue"),
              },
              {
                path: "detail/:activityId",
                component: () =>
                  import("@/views/crm/activity/activity/CrmActivityActivityDetail.vue"),
              },
              {
                path: "detail/:activityId/pipeline",
                meta: { title: "名單" },
                children: [
                  {
                    path: "",
                    component: () =>
                      import("@/views/crm/activity/pipeline/CrmActivityPipelineList.vue"),
                  },
                  {
                    path: "add",
                    component: () =>
                      import("@/views/crm/activity/pipeline/CrmActivityPipelineAdd.vue"),
                  },
                  {
                    path: "detail/:pipelineId",
                    component: () =>
                      import("@/views/crm/activity/pipeline/CrmActivityPipelineDetail.vue"),
                  },
                ],
              },
            ],
          },
        ],
      },
      {
        path: "phone",
        meta: { title: "電銷管理" },
        children: [
          {
            path: "activity",
            meta: { title: "活動" },
            children: [
              {
                path: "",
                component: () => import("@/views/crm/phone/activity/CrmPhoneActivityList.vue"),
              },
              {
                path: "detail/:activityId",
                component: () => import("@/views/crm/phone/activity/CrmPhoneActivityDetail.vue"),
              },
              {
                path: "detail/:activityId/pipeline",
                meta: { title: "名單" },
                children: [
                  {
                    path: "",
                    component: () => import("@/views/crm/phone/pipeline/CrmPhonePipelineList.vue"),
                  },
                  {
                    path: "detail/:pipelineId",
                    component: () =>
                      import("@/views/crm/phone/pipeline/CrmPhonePipelineDetail.vue"),
                  },
                ],
              },
            ],
          },
          {
            path: "pool",
            meta: { title: "名單池" },
            component: () => import("@/views/crm/phone/pool/CrmPhoneLeadPoolList.vue"),
          },
        ],
      },
      {
        path: "phone-sales",
        meta: { title: "電銷客戶管理" },
        children: [
          {
            path: "customer",
            meta: { title: "客戶" },
            children: [
              {
                path: "",
                component: () => import("@/views/crm/phone-sales/customer/CrmPhoneSalesCustomerList.vue"),
              },
              {
                path: "add",
                component: () => import("@/views/crm/phone-sales/customer/CrmPhoneSalesCustomerAdd.vue"),
              },
              {
                path: "detail/:id",
                component: () => import("@/views/crm/phone-sales/customer/CrmPhoneSalesCustomerDetail.vue"),
              },
            ],
          },
        ],
      },
      {
        path: "pipeline",
        meta: { title: "商機管理" },
        children: [
          {
            path: "handoff",
            meta: { title: "電銷開發" },
            component: () =>
              import("@/views/crm/pipeline/handoff/CrmPipelineHandoffList.vue"),
          },
          {
            path: "pipeline",
            meta: { title: "名單" },
            children: [
              {
                path: "",
                component: () =>
                  import("@/views/crm/pipeline/pipeline/CrmPipelinePipelineList.vue"),
              },
              {
                path: "add",
                component: () => import("@/views/crm/pipeline/pipeline/CrmPipelinePipelineAdd.vue"),
              },
              {
                path: "detail/:pipelineId",
                component: () =>
                  import("@/views/crm/pipeline/pipeline/CrmPipelinePipelineDetail.vue"),
              },
            ],
          },
        ],
      },
    ],
  },
  // #region 系統管理
  {
    path: "/system",
    component: () => import("@/layouts/DefaultLayout.vue"),
    meta: { requiresAuth: true },
    children: [
      {
        path: "master",
        meta: { title: "基礎資料" },
        component: () => import("@/views/system/master/SystemMasterData.vue"),
      },
      {
        path: "project-template",
        meta: { title: "專案標準階段" },
        component: () =>
          import("@/views/system/project-template/SystemProjectTemplateSettings.vue"),
      },
      {
        path: "contacter",
        meta: { title: "窗口" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/contacter/SystemContacterList.vue"),
          },
          {
            path: "add",
            component: () => import("@/views/system/contacter/SystemContacterAdd.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/system/contacter/SystemContacterDetail.vue"),
          },
        ],
      },
      {
        path: "company",
        meta: { title: "客戶" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/company/SystemCompanyList.vue"),
          },
          {
            path: "add",
            component: () => import("@/views/system/company/SystemCompanyAdd.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/system/company/SystemCompanyDetail.vue"),
          },
        ],
      },
      {
        path: "product",
        meta: { title: "產品" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/product/SystemProductList.vue"),
          },
          {
            path: "add",
            component: () => import("@/views/system/product/SystemProductAdd.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/system/product/SystemProductDetail.vue"),
          },
        ],
      },
      {
        path: "employee",
        meta: { title: "員工" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/employee/SystemEmployeeList.vue"),
          },
          {
            path: "add",
            component: () => import("@/views/system/employee/SystemEmployeeAdd.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/system/employee/SystemEmployeeDetail.vue"),
          },
        ],
      },
      {
        path: "role",
        meta: { title: "角色" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/role/SystemRoleList.vue"),
          },
          {
            path: "add",
            component: () => import("@/views/system/role/SystemRoleAdd.vue"),
          },
          {
            path: "detail/:id",
            component: () => import("@/views/system/role/SystemRoleDetail.vue"),
          },
        ],
      },
      {
        path: "department",
        meta: { title: "部門" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/department/SystemDepartmentList.vue"),
          },
        ],
      },
      {
        path: "region",
        meta: { title: "地區" },
        children: [
          {
            path: "",
            component: () => import("@/views/system/region/SystemRegionList.vue"),
          },
        ],
      },
    ],
  },
];

const ROUTE_PREFIX = import.meta.env.VITE_BASE_PATH || "";

const router = createRouter({
  history: createWebHistory(ROUTE_PREFIX),
  routes,
});

export default router;
