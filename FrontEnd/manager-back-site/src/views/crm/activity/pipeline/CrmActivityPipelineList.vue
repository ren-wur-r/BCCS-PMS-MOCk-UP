<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, defineAsyncComponent, computed } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import { useRoute } from "vue-router";
import type {
  MbsCrmActHttpGetManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpGetManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetManyActivityEmployeePipelineRspItemMdl,
  MbsCrmActHttpDownloadTeamsTemplateReqMdl,
  MbsCrmActHttpDownloadEdmTemplateReqMdl,
  MbsCrmActHttpImportTeamsRspMdl,
  MbsCrmActHttpImportTeamsReqMdl,
  MbsCrmActHttpImportEdmReqMdl,
  MbsCrmActHttpImportEdmRspMdl,
  MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetActivityReqMdl,
  MbsCrmActHttpGetActivityRspMdl,
} from "@/services/pms-http/crm/activity/crmActivityHttpFormat";
import {
  downloadEdmTemplate,
  downloadTeamsTemplate,
  getActivity,
  getManyActivityEmployeePipeline,
  importEdm,
  importTeams,
  removeManyActivityEmployeePipeline,
  updateManyActivityEmployeePipeline,
} from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ActivityDetailTabs from "@/components/feature/activity/ActivityDetailTabs.vue";
import router from "@/router";
import { saveAs } from "file-saver";
import {
  MbsCrmActCtlDownloadEdmTemplateRspMdl,
  MbsCrmActCtlDownloadTeamsTemplateRspMdl,
} from "@/services/pms-http/crm/activity/crmActivityControllerFormat";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
const GetManyManagerCompanyNameFromPipelineOriginalComboBox = defineAsyncComponent(
  () =>
    import(
      "@/components/feature/search-bar/GetManyManagerCompanyNameFromPipelineOriginalComboBox.vue"
    )
);
const ImportTeamsResultModal = defineAsyncComponent(
  () => import("./component/ImportTeamsResultModal.vue")
);
const ImportEdmResultModal = defineAsyncComponent(
  () => import("./component/ImportEdmResultModal.vue")
);
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();

/** 路由參數取得 */
const route = useRoute();
//#endregion

//#region 型別定義
/** CRM-活動管理-名單-列表-查詢模型 */
interface CrmActivityEmployeePipelineListQueryMdl {
  managerActivityID: number;
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  managerCompanyID: number;
  managerCompanyName: string | null;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  pageIndex: number;
  pageSize: number;
}

/** CRM-活動管理-名單-列表-項目模型 */
interface CrmActivityEmployeePipelineListItemMdl {
  employeePipelineID: number;
  atomPipelineStatus: DbAtomPipelineStatusEnum | null;
  managerCompanyName: string;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterTelephone: string | null;
}

/** CRM-活動管理-名單-列表-頁面模型 */
interface CrmActivityEmployeePipelineListViewMdl {
  query: CrmActivityEmployeePipelineListQueryMdl;
  employeePipelineList: CrmActivityEmployeePipelineListItemMdl[];
  totalCount: number;
  managerActivityKind: DbAtomManagerActivityKindEnum | null;
  managerActivityName: string | null;
}

/** 匯入Teams-項目模型 */
interface ImportTeamsItemMdl {
  teamsName: string | null;
  teamsFirstJoinTime: string | null;
  teamsLastLeaveTime: string | null;
  teamsMeetingDuration: string;
  teamsEmail: string;
  teamsParticipantId: string;
  teamsRole: string;
  teamsContacterRegisterLastName: string;
  teamsContacterRegisterFirstName: string;
  teamsContacterRegisterEmail: string;
  teamsRegistrationTime: string | null;
  teamsRegistrationStatus: string;
  teamsCompanyName: string;
  teamsContacterDepartment: string;
  teamsContacterJobTitle: string;
  teamsCompanyTelephone: string;
  teamsContacterPhone: string;
  teamsActivityInfoSource: string;
  teamsContacterIsConsent: boolean;
}

/** 匯入Edm-項目模型 */
interface ImportEdmItemMdl {
  edmContacterEmail: string;
  edmFirstName: string;
  edmLastName: string;
  edmContacterTelephone: string;
  eEdmContacterPhone: string;
  edmCompanyName: string;
  edmContacterJobTitle: string;
  edmCompanyPhone: string;
  edmCompanyFax: string;
  edmCompanyAddress: string;
  edmRemark: string;
  edmContacterDepartment: string;
  edmCompanyMainKind: string;
  edmCompanySubKind: string;
  edmAccountSales: string;
  edmRegion: string;
  edmCreatedDate: string;
  edmDevice: string;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmActivity;
/** 取得活動ID(路由參數) */
const managerActivityID = Number(route.params.activityId);
/** 已選取名單的 ID 清單 */
const selectedIds = ref<number[]>([]);
/** 匯入Teams結果彈跳視窗顯示 */
const isImportTeamsResultModalVisible = ref(false);
/** 匯入Teams失敗清單 */
const importTeamsResultList = ref<ImportTeamsItemMdl[]>([]);
/** 匯入Teams失敗總數 */
const importTeamsResultTotal = ref<number>(0);
/** Teams檔案輸入框 */
const fileTeamsInputRef = ref<HTMLInputElement | null>(null);
/** 是否顯示Teams選單 */
const showTeamsMenu = ref(false);
/** 匯入Edm結果彈跳視窗顯示 */
const isImportEdmResultModalVisible = ref(false);
/** 匯入Edm失敗清單 */
const importEdmResultList = ref<ImportEdmItemMdl[]>([]);
/** 匯入Edm失敗總數 */
const importEdmResultTotal = ref<number>(0);
/** Edm檔案輸入框 */
const fileEdmInputRef = ref<HTMLInputElement | null>(null);
/** 是否顯示Edm選單 */
const showEdmMenu = ref(false);

/** CRM-活動管理-名單-列表-頁面物件 */
const crmEmployeePipelineListViewObj = reactive<CrmActivityEmployeePipelineListViewMdl>({
  query: {
    managerActivityID: 0,
    managerCompanyID: 0,
    atomPipelineStatus: null,
    managerCompanyName: "",
    managerContacterName: "",
    managerContacterEmail: "",
    pageIndex: 1,
    pageSize: 10,
  },
  employeePipelineList: [],
  totalCount: 0,
  managerActivityKind: null,
  managerActivityName: "",
});
//#endregion

//#region UI行為
/** 判斷是否全部選取 */
const isAllSelected = computed(() => {
  // 只取出可勾選的項目（排除已轉電銷）
  const selectableItems = crmEmployeePipelineListViewObj.employeePipelineList.filter(
    (item) => item.atomPipelineStatus == DbAtomPipelineStatusEnum.Hyphen
      || item.atomPipelineStatus == DbAtomPipelineStatusEnum.Opened
      || item.atomPipelineStatus == DbAtomPipelineStatusEnum.Clicked
  );

  // 沒有任何可勾選資料
  if (selectableItems.length === 0) return false;

  return selectableItems.every((item) => selectedIds.value.includes(item.employeePipelineID));
});

/** 點擊【選擇全部】 */
const clickSelectAll = (event: Event) => {
  const checked = (event.target as HTMLInputElement).checked;

  if (checked) {
    // 只全選「尚未轉電銷」的項目
    selectedIds.value = crmEmployeePipelineListViewObj.employeePipelineList
      .filter((item) => item.atomPipelineStatus == DbAtomPipelineStatusEnum.Hyphen
        || item.atomPipelineStatus == DbAtomPipelineStatusEnum.Opened
        || item.atomPipelineStatus == DbAtomPipelineStatusEnum.Clicked
      )
      .map((item) => item.employeePipelineID);
  } else {
    selectedIds.value = [];
  }
};
//#endregion

//#region API / 資料流程
/**取得【名單】列表 */
const getEmployeePipelineList = async () => {
  //驗證 token
  if (!requireToken()) return;

  //呼叫 api
  const requestData: MbsCrmActHttpGetManyActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: managerActivityID,
    atomPipelineStatus: crmEmployeePipelineListViewObj.query.atomPipelineStatus,
    managerCompanyName: crmEmployeePipelineListViewObj.query.managerCompanyName,
    managerContacterName: crmEmployeePipelineListViewObj.query.managerContacterName,
    managerContacterEmail: crmEmployeePipelineListViewObj.query.managerContacterEmail,
    pageIndex: crmEmployeePipelineListViewObj.query.pageIndex,
    pageSize: crmEmployeePipelineListViewObj.query.pageSize,
  };
  const responseData: MbsCrmActHttpGetManyActivityEmployeePipelineRspMdl | null =
    await getManyActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 資料處理
  crmEmployeePipelineListViewObj.employeePipelineList =
    responseData.employeePipelineList?.map(
      (item: MbsCrmActHttpGetManyActivityEmployeePipelineRspItemMdl) => ({
        employeePipelineID: item.employeePipelineID,
        atomPipelineStatus: item.atomPipelineStatus ?? null,
        managerCompanyName: item.managerCompanyName,
        managerContacterDepartment: item.managerContacterDepartment ?? null,
        managerContacterJobTitle: item.managerContacterJobTitle ?? null,
        managerContacterName: item.managerContacterName ?? null,
        managerContacterEmail: item.managerContacterEmail ?? null,
        managerContacterPhone: item.managerContacterPhone ?? null,
        managerContacterTelephone: item.managerContacterTelephone ?? null,
      })
    ) ?? [];
  crmEmployeePipelineListViewObj.totalCount = responseData.totalCount ?? 0;
};

/** 取得【活動】資料(獲取此活動名稱和類型) */
const getActivityData = async () => {
  //驗證 token
  if (!requireToken()) return;

  //呼叫 api
  const requestData: MbsCrmActHttpGetActivityReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: managerActivityID,
  };
  const responseData: MbsCrmActHttpGetActivityRspMdl | null = await getActivity(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 資料處理
  crmEmployeePipelineListViewObj.managerActivityKind = responseData.managerActivityKind;
  crmEmployeePipelineListViewObj.managerActivityName = responseData.managerActivityName;
};

/** 點擊【轉給電銷】按鈕 */
const clickTransferredToSalesBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證是否有選取名單
  if (selectedIds.value.length === 0) {
    displayError("請先選取名單！");
    return;
  }

  // 呼叫 api
  const requestData: MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineIDList: selectedIds.value,
    atomPipelineStatus: DbAtomPipelineStatusEnum.TransferredToSales,
  };
  const responseData: MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl | null =
    await updateManyActivityEmployeePipeline(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("轉給電銷成功！");

  // 清空已選取 ID 清單
  selectedIds.value = [];

  // 重新取得列表
  getEmployeePipelineList();
};

/** 處理【Teasm】檔案變化 */
const handleTeamsFileChange = async (event: Event) => {
  // 取得檔案
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];

  // 驗證是否有檔案
  if (!file) {
    displayError("請選擇檔案！");
    return;
  }

  // 呼叫 API
  const requestData: MbsCrmActHttpImportTeamsReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: managerActivityID,
    teamsFile: file,
  };
  const responseData: MbsCrmActHttpImportTeamsRspMdl | null = await importTeams(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("匯入 Teams 名單成功！");

  // 處理錯誤資料
  if (responseData.errorDataList.length) {
    importTeamsResultTotal.value = responseData.errorDataList.length;
    isImportTeamsResultModalVisible.value = true;
    // 錯誤資料傳遞給子元件顯示
    importTeamsResultList.value = responseData.errorDataList.map(
      (item) =>
        ({
          teamsName: item.teamsName,
          teamsFirstJoinTime: item.teamsFirstJoinTime,
          teamsLastLeaveTime: item.teamsLastLeaveTime,
          teamsMeetingDuration: item.teamsMeetingDuration,
          teamsEmail: item.teamsEmail,
          teamsParticipantId: item.teamsParticipantId,
          teamsRole: item.teamsRole,
          teamsContacterRegisterLastName: item.teamsContacterRegisterLastName,
          teamsContacterRegisterFirstName: item.teamsContacterRegisterFirstName,
          teamsContacterRegisterEmail: item.teamsContacterRegisterEmail,
          teamsRegistrationTime: item.teamsRegistrationTime,
          teamsRegistrationStatus: item.teamsRegistrationStatus,
          teamsCompanyName: item.teamsCompanyName,
          teamsContacterDepartment: item.teamsContacterDepartment,
          teamsContacterJobTitle: item.teamsContacterJobTitle,
          teamsCompanyTelephone: item.teamsCompanyTelephone,
          teamsContacterPhone: item.teamsContacterPhone,
          teamsActivityInfoSource: item.teamsActivityInfoSource,
          teamsContacterIsConsent: item.teamsContacterIsConsent,
        }) satisfies ImportTeamsItemMdl
    );
  }

  // 清空 input 讓使用者可以重選
  target.value = "";

  // 重新取得列表
  getEmployeePipelineList();
};

/** 處理檔案選擇後上傳 */
const handleEdmFileChange = async (event: Event) => {
  // 取得檔案
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];

  // 驗證是否有檔案
  if (!file) {
    displayError("請選擇檔案！");
    return;
  }

  // 呼叫 API
  const requestData: MbsCrmActHttpImportEdmReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: managerActivityID,
    edmFile: file,
  };
  const responseData: MbsCrmActHttpImportEdmRspMdl | null = await importEdm(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("匯入 eDM 名單成功！");

  // 處理錯誤資料
  if (responseData.errorDataList.length) {
    importEdmResultTotal.value = responseData.errorDataList.length;
    isImportEdmResultModalVisible.value = true;
    // 錯誤資料傳遞給子元件顯示
    importEdmResultList.value = responseData.errorDataList.map(
      (item) =>
        ({
          edmContacterEmail: item.edmContacterEmail,
          edmFirstName: item.edmFirstName,
          edmLastName: item.edmLastName,
          edmContacterTelephone: item.edmContacterTelephone,
          eEdmContacterPhone: item.eEdmContacterPhone,
          edmCompanyName: item.edmCompanyName,
          edmContacterJobTitle: item.edmContacterJobTitle,
          edmCompanyPhone: item.edmCompanyPhone,
          edmCompanyFax: item.edmCompanyFax,
          edmCompanyAddress: item.edmCompanyAddress,
          edmRemark: item.edmRemark,
          edmContacterDepartment: item.edmContacterDepartment,
          edmCompanyMainKind: item.edmCompanyMainKind,
          edmCompanySubKind: item.edmCompanySubKind,
          edmAccountSales: item.edmAccountSales,
          edmRegion: item.edmRegion,
          edmCreatedDate: item.edmCreatedDate,
          edmDevice: item.edmDevice,
        }) satisfies ImportEdmItemMdl
    );
  }

  // 清空 input 讓使用者可以重選
  target.value = "";

  // 重新取得列表
  getEmployeePipelineList();
};

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push(`/crm/activity/activity/detail/${managerActivityID}/`);
};

/** 點擊【查詢】按鈕 */
const clickSearchBtn = () => {
  crmEmployeePipelineListViewObj.query.pageIndex = 1;
  getEmployeePipelineList();
};

/** 點擊【清除】按鈕 */
const clickClearSearchBtn = () => {
  crmEmployeePipelineListViewObj.query.atomPipelineStatus = null;
  crmEmployeePipelineListViewObj.query.managerCompanyName = null;
  crmEmployeePipelineListViewObj.query.managerContacterName = null;
  crmEmployeePipelineListViewObj.query.managerContacterEmail = null;
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  router.push(`/crm/activity/activity/detail/${managerActivityID}/pipeline/add`);
};

/** 點擊【檢視】按鈕 */
const clickDetailBtn = (id: number) => {
  router.push(`/crm/activity/activity/detail/${managerActivityID}/pipeline/detail/${id}`);
};

/** 點擊【刪除名單】按鈕 */
const clickDeletePipelineBtn = async () => {
  // 驗證
  if (!requireToken()) return;

  // 驗證是否有選取名單
  if (selectedIds.value.length === 0) {
    displayError("請先選取名單！");
    return;
  }

  // 呼叫 api
  const requestData: MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineIDList: selectedIds.value,
  };
  const responseData: MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl | null =
    await removeManyActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除名單成功！");

  // 清空已選取 ID 清單
  selectedIds.value = [];

  // 重新取得列表
  getEmployeePipelineList();
};

/** 點擊【Teasm】選單按鈕 */
const clickTeamsMenuBtn = () => {
  showEdmMenu.value = false;
  showTeamsMenu.value = !showTeamsMenu.value;
};

/** 點擊【Teasm】開啟檔案選擇 */
const clickTeamsFileSelect = () => {
  fileTeamsInputRef.value?.click();
};

/** 點擊【下載Teams】按鈕 */
const clickDownloadTeamsBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmActHttpDownloadTeamsTemplateReqMdl = {
    employeeLoginToken: tokenStore.token!,
  };
  const responseData: MbsCrmActCtlDownloadTeamsTemplateRspMdl | null =
    await downloadTeamsTemplate(requestData);
  if (!responseData) {
    displayError("下載失敗，請稍後再試！");
    return;
  }
  if (responseData.size === 0) {
    displayError("下載失敗，檔案內容為空！");
    return;
  }

  // 產生 Blob 物件
  const blob = new Blob([responseData], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  });

  // 下載檔案
  saveAs(blob, "Teams名單匯入範本.xlsx");

  // 顯示成功訊息
  displaySuccess("下載 Teams 範本成功！");
};

/** 點擊【Edm】選單按鈕 */
const clickEdmMenuBtn = () => {
  showTeamsMenu.value = false;
  showEdmMenu.value = !showEdmMenu.value;
};

/** 點擊按鈕開啟檔案選擇 */
const clickEdmFileSelect = () => {
  fileEdmInputRef.value?.click();
};

/** 點擊【下載Edm】按鈕 */
const clickDownloadEdmBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsCrmActHttpDownloadEdmTemplateReqMdl = {
    employeeLoginToken: tokenStore.token!,
  };
  const responseData: MbsCrmActCtlDownloadEdmTemplateRspMdl | null =
    await downloadEdmTemplate(requestData);
  if (!responseData) {
    displayError("下載失敗，請稍後再試！");
    return;
  }
  if (responseData.size === 0) {
    displayError("下載失敗：檔案內容為空！");
    return;
  }

  // 產生 Blob 物件
  const blob = new Blob([responseData], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  });

  // 下載檔案
  saveAs(blob, "eDM名單匯入範本.xlsx");

  // 顯示成功訊息
  displaySuccess("下載 eDM 範本成功！");
};
//#endregion

//#region 生命週期
onMounted(() => {
  // 取得名單列表資料
  getEmployeePipelineList();
  // 取得活動資料
  getActivityData();
});
//#endregion
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden gap-4 p-2">
    <div class="flex items-center justify-between">
      <button class="btn-back flex items-center" @click="clickBackBtn">
        <svg class="w-4 h-4 inline-block mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
        </svg>
        {{ crmEmployeePipelineListViewObj.managerActivityName ?? "-" }}
      </button>

      <div class="flex flex-row gap-1">
        <div v-click-outside="() => (showTeamsMenu = false)" class="relative inline-block">
          <!-- 主按鈕 -->
          <button class="btn-add" @click.stop="clickTeamsMenuBtn">Teams</button>

          <!-- 下拉選單（出現在按鈕下方） -->
          <div v-if="showTeamsMenu" class="action-menu below">
            <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')" class="action-menu-item"
              @click="clickTeamsFileSelect">
              匯入 Teams
            </button>
            <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'view')" class="action-menu-item"
              @click="clickDownloadTeamsBtn">
              下載 Teams 範本
            </button>

            <input ref="fileTeamsInputRef" type="file" accept=".xlsx,.xls,.csv" class="hidden"
              @change="handleTeamsFileChange" />
          </div>
        </div>
        <div v-click-outside="() => (showEdmMenu = false)" class="relative inline-block">
          <!-- 主按鈕 -->
          <button class="btn-add" @click.stop="clickEdmMenuBtn">eDM</button>

          <!-- 下拉選單（出現在按鈕下方） -->
          <div v-if="showEdmMenu" class="action-menu below">
            <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')" class="action-menu-item"
              @click="clickEdmFileSelect">
              匯入eDM
            </button>
            <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'view')" class="action-menu-item"
              @click="clickDownloadEdmBtn">
              下載 eDM 範本
            </button>

            <input ref="fileEdmInputRef" type="file" accept=".xlsx,.xls,.csv" class="hidden"
              @change="handleEdmFileChange" />
          </div>
        </div>

        <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')" class="btn-add" @click="clickAddBtn">
          新增名單
        </button>
      </div>
    </div>

    <ActivityDetailTabs :activity-id="managerActivityID" base-path="/crm/activity/activity" />

    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-6 gap-4">
      <!-- 查詢條件 -->
      <div class="flex items-end gap-4">
        <div class="flex gap-2">
          <!--實體-->
          <div v-if="
            crmEmployeePipelineListViewObj.managerActivityKind ===
            DbAtomManagerActivityKindEnum.PhysicalActivity
          ">
<select v-model="crmEmployeePipelineListViewObj.query.atomPipelineStatus" class="select-box">
              <option :value="null">全部</option>
              <option :value="DbAtomPipelineStatusEnum.Hyphen">-</option>
              <option :value="DbAtomPipelineStatusEnum.Opened">已開啟</option>
              <option :value="DbAtomPipelineStatusEnum.Clicked">已點擊</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToSales">已轉電銷</option>
              <option :value="DbAtomPipelineStatusEnum.CompletedSales">已完成電銷</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToBusiness">已轉業務</option>
              <option :value="DbAtomPipelineStatusEnum.Business10Percent">商機10%</option>
              <option :value="DbAtomPipelineStatusEnum.Business30Percent">商機30%</option>
              <option :value="DbAtomPipelineStatusEnum.Business70Percent">商機70%</option>
              <option :value="DbAtomPipelineStatusEnum.Business100Percent">商機100%</option>
              <option :value="DbAtomPipelineStatusEnum.BusinessFailed">商機失敗</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToProject">已轉專案</option>
            </select>
          </div>

          <!--線上-->
          <div v-if="
            crmEmployeePipelineListViewObj.managerActivityKind ===
            DbAtomManagerActivityKindEnum.OnlineActivity
          ">
<select v-model="crmEmployeePipelineListViewObj.query.atomPipelineStatus" class="select-box">
              <option :value="null">全部</option>
              <option :value="DbAtomPipelineStatusEnum.Hyphen">-</option>
              <option :value="DbAtomPipelineStatusEnum.Opened">已開啟</option>
              <option :value="DbAtomPipelineStatusEnum.Clicked">已點擊</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToSales">已轉電銷</option>
              <option :value="DbAtomPipelineStatusEnum.CompletedSales">已完成電銷</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToBusiness">已轉業務</option>
              <option :value="DbAtomPipelineStatusEnum.Business10Percent">商機10%</option>
              <option :value="DbAtomPipelineStatusEnum.Business30Percent">商機30%</option>
              <option :value="DbAtomPipelineStatusEnum.Business70Percent">商機70%</option>
              <option :value="DbAtomPipelineStatusEnum.Business100Percent">商機100%</option>
              <option :value="DbAtomPipelineStatusEnum.BusinessFailed">商機失敗</option>
              <option :value="DbAtomPipelineStatusEnum.TransferredToProject">已轉專案</option>
            </select>
          </div>

          <div>
<GetManyManagerCompanyNameFromPipelineOriginalComboBox
              v-model:managerCompanyName="crmEmployeePipelineListViewObj.query.managerCompanyName" :disabled="false" />
          </div>

          <div>
<input v-model="crmEmployeePipelineListViewObj.query.managerContacterName" type="text" class="input-box"
              placeholder="原始窗口姓名" />
          </div>

          <div>
<input v-model="crmEmployeePipelineListViewObj.query.managerContacterEmail" type="text" class="input-box"
              placeholder="原始窗口電子信箱" />
          </div>
        </div>

        <div>
          <button class="btn-search me-1" @click="clickSearchBtn">查詢</button>
          <button class="btn-cancel" @click="clickClearSearchBtn">清除</button>
        </div>
      </div>

      <hr />
      <div class="flex flex-row justify-between items-center">
        <div class="flex flex-row justify-start">
          <p class="text-gray-600 text-sm items-center flex ml-3">
            已選取 {{ selectedIds.length ?? 0 }} 筆
          </p>
        </div>
        <div>
          <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')" class="btn-search me-1"
            @click="clickTransferredToSalesBtn">
            轉給電銷
          </button>
          <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'delete')" class="btn-delete"
            @click="clickDeletePipelineBtn">
            刪除
          </button>
        </div>
      </div>

      <!-- 列表 -->
      <div class="flex-1 overflow-y-auto table-scrollable">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-center w-20">
                <input type="checkbox" :checked="isAllSelected" @change="clickSelectAll" />
              </th>
              <th class="text-center w-28">名單狀態</th>
              <th class="text-start">原始客戶</th>
              <th class="text-start">原始窗口姓名</th>
              <th class="text-start">原始窗口電子信箱</th>
              <th class="text-start">原始窗口手機/電話</th>
              <th class="text-start">原始窗口部門/職稱</th>
              <th class="text-center w-24">操作</th>
            </tr>
          </thead>
          <tbody>
            <template v-if="crmEmployeePipelineListViewObj.employeePipelineList.length === 0">
              <tr class="text-center">
                <td colspan="8">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr v-for="item in crmEmployeePipelineListViewObj.employeePipelineList" :key="item.employeePipelineID">
                <td class="text-center">
                  <input v-model="selectedIds" type="checkbox" :value="item.employeePipelineID" :disabled="item.atomPipelineStatus !== DbAtomPipelineStatusEnum.Hyphen
                    && item.atomPipelineStatus !== DbAtomPipelineStatusEnum.Opened
                    && item.atomPipelineStatus !== DbAtomPipelineStatusEnum.Clicked
                    " />
                </td>
                <td class="text-center">
                  {{ getPipelineStatusLabel(item.atomPipelineStatus) }}
                </td>
                <td class="text-start">{{ item.managerCompanyName }}</td>
                <td class="text-start">{{ item.managerContacterName }}</td>
                <td class="text-start">{{ item.managerContacterEmail ?? "-" }}</td>
                <td class="text-start">
                  <p>{{ item.managerContacterPhone ?? "-" }}</p>
                  <p>{{ item.managerContacterTelephone ?? "-" }}</p>
                </td>
                <td class="text-start">
                  {{ item.managerContacterDepartment || "-" }}
                  /{{ item.managerContacterJobTitle || "-" }}
                </td>
                <td class="text-center">
                  <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'view')" class="btn-detail"
                    @click="clickDetailBtn(item.employeePipelineID)">
                    檢視
                  </button>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>

      <div class="flex justify-center pt-3">
        <!-- 分頁 -->
        <Pagination :pageIndex="crmEmployeePipelineListViewObj.query.pageIndex"
          :pageSize="crmEmployeePipelineListViewObj.query.pageSize"
          :totalCount="crmEmployeePipelineListViewObj.totalCount" @update:current-page="
            (newPage: number) => {
              crmEmployeePipelineListViewObj.query.pageIndex = newPage;
              getEmployeePipelineList();
            }
          " @update:page-size="
            (newSize: number) => {
              crmEmployeePipelineListViewObj.query.pageSize = newSize;
              crmEmployeePipelineListViewObj.query.pageIndex = 1;
              getEmployeePipelineList();
            }
          " />
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

  <!-- 匯入Teams失敗彈窗 -->
  <ImportTeamsResultModal v-if="isImportTeamsResultModalVisible" :show="isImportTeamsResultModalVisible"
    :result-list="importTeamsResultList" :total-count="importTeamsResultTotal"
    @close="isImportTeamsResultModalVisible = false" />

  <!-- 匯入Teams失敗彈窗 -->
  <ImportEdmResultModal v-if="isImportEdmResultModalVisible" :show="isImportEdmResultModalVisible"
    :result-list="importEdmResultList" :total-count="importEdmResultTotal"
    @close="isImportEdmResultModalVisible = false" />
</template>
<style scoped>
.action-menu {
  position: absolute;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  box-shadow:
    0 4px 6px -1px rgba(0, 0, 0, 0.1),
    0 2px 4px -1px rgba(0, 0, 0, 0.06);
  z-index: 20;
  min-width: 140px;
  overflow: hidden;
}

.action-menu-item {
  display: block;
  width: 100%;
  padding: 12px 16px;
  background: white;
  border: none;
  text-align: left;
  font-size: 14px;
  color: #374151;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.action-menu-item:hover {
  background-color: #f3f4f6;
}

.action-menu-item:not(:last-child) {
  border-bottom: 1px solid #f3f4f6;
}
</style>
