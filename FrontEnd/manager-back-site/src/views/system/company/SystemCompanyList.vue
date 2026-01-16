<script setup lang="ts">
//#region 引入
import { reactive, ref, onMounted, watch, onBeforeUnmount } from "vue";
import { useRouter } from "vue-router";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useAuth } from "@/composables/useAuth";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
import { getManyCompany } from "@/services";
import { 
  getManyCompanyMainKind, 
  getManyCompanySubKind, 
  addCompanyMainKind,
  getCompanyMainKind,
  updateCompanyMainKind,
  addCompanySubKind,
  getCompanySubKind,
  updateCompanySubKind
} from "@/services";
import { getCustomerGradeLabel } from "@/utils/getCustomerGradeLabel";
import type {
  MbsSysComHttpGetManyCompanyMainKindReqMdl,
  MbsSysComHttpGetManyCompanySubKindReqMdl,
  MbsSysComHttpGetManyCompanyReqMdl,
  MbsSysComHttpGetManyCompanyRspMdl,
  MbsSysComHttpGetManyCompanySubKindRspMdl,
  MbsSysComHttpGetManyCompanyMainKindRspMdl,
  MbsSysComHttpGetManyCompanyMainKindRspItemMdl,
  MbsSysComHttpGetManyCompanySubKindRspItemMdl,
  MbsSysComHttpGetManyCompanyRspItemMdl,
  MbsSysComHttpAddCompanyMainKindReqMdl,
  MbsSysComHttpAddCompanyMainKindRspMdl,
  MbsSysComHttpGetCompanyMainKindReqMdl,
  MbsSysComHttpGetCompanyMainKindRspMdl,
  MbsSysComHttpUpdateCompanyMainKindReqMdl,
  MbsSysComHttpUpdateCompanyMainKindRspMdl,
  MbsSysComHttpAddCompanySubKindReqMdl,
  MbsSysComHttpAddCompanySubKindRspMdl,
  MbsSysComHttpGetCompanySubKindReqMdl,
  MbsSysComHttpGetCompanySubKindRspMdl,
  MbsSysComHttpUpdateCompanySubKindReqMdl,
  MbsSysComHttpUpdateCompanySubKindRspMdl,
} from "@/services/pms-http/system/company/systemCompanyHttpFormat";
import AddSystemCompanyMainKindModal from "./components/AddSystemCompanyMainKindModal.vue";
import UpdateSystemCompanyMainKindModal from "./components/UpdateSystemCompanyMainKindModal.vue";
import AddSystemCompanySubKindModal from "./components/AddSystemCompanySubKindModal.vue";
import UpdateSystemCompanySubKindModal from "./components/UpdateSystemCompanySubKindModal.vue";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyManagerCompanyMainKindComboBox from "@/components/feature/search-bar/GetManyManagerCompanyMainKindComboBox.vue";
import GetManyManagerCompanySubKindComboBox from "@/components/feature/search-bar/GetManyManagerCompanySubKindComboBox.vue";
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
//#endregion

//#region 型別定義
//------------------------------------------------------------
/** 系統設定-客戶頁籤種類列舉 */
enum CompanyTabEnum {
  Customer = "customer",
  MainKind = "mainKind",
  SubKind = "subKind",
}

//------------------------------------------------------------
/** 系統設定-客戶-列表-查詢模型 */
interface SysCompanyListQueryMdl {
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  managerCompanyMainKindID: number | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindID: number | null;
  managerCompanySubKindName: string | null;
  managerCompanyName: string | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-客戶-列表-項目模型 */
interface SysCompanyListItemMdl {
  managerCompanyID: number;
  managerCompanyUnifiedNumber: string | null;
  managerCompanyName: string | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
}

/** 系統設定-客戶-列表-頁面模型 */
interface SysCompanyListViewMdl {
  query: SysCompanyListQueryMdl;
  customercompanyList: SysCompanyListItemMdl[];
  totalCount: number;
}
/** 系統設定-客戶-主分類-列表-查詢模型 */
interface SysComMainKindListQueryMdl {
  managerCompanyMainKindName: string | null;
  managerCompanyMainKindIsEnable: boolean | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-客戶-主分類-列表-項目模型 */
interface SysComMainKindListItemMdl {
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
  managerCompanyMainKindIsEnable: boolean;
}

/** 系統設定-客戶-主分類-列表-頁面模型 */
interface SysComMainKindListViewMdl {
  query: SysComMainKindListQueryMdl;
  customercompanyMainKindList: SysComMainKindListItemMdl[];
  totalCount: number;
}
//------------------------------------------------------------
/** 系統設定-客戶-子分類-列表-查詢模型 */
interface SysComSubKindListQueryMdl {
  managerCompanySubKindName: string | null;
  managerCompanySubKindIsEnable: boolean | null;
  managerCompanyMainKindID: number | null;
  managerCompanyMainKindName: string | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-客戶-子分類-列表-項目模型 */
interface SysComSubKindListItemMdl {
  managerCompanySubKindID: number;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  managerCompanySubKindIsEnable: boolean;
}

/** 系統設定-客戶-子分類-列表-頁面模型 */
interface MbsCstComSubKindListViewMdl {
  query: SysComSubKindListQueryMdl;
  customercompanySubKindList: SysComSubKindListItemMdl[];
  totalCount: number;
}
//------------------------------------------------------------

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemCompany;
/** 目前作用中的tab */
const activeTab = ref<CompanyTabEnum>(CompanyTabEnum.Customer);
/** 系統設定-客戶-主分類-新增-Modal-顯示 */
const sysComMainKindAddModalShow = ref(false);
/** 系統設定-客戶-主分類-編輯-Modal-顯示 */
const sysComMainKindUpdateModalShow = ref(false);
/** 選取的主分類ID */
const selectedMainKindID = ref<number | null>(null);
/** 編輯主分類的資料 */
const editingMainKindData = ref<{
  managerCompanyMainKindName: string;
  managerCompanyMainKindIsEnable: boolean;
} | null>(null);
/** 系統設定-客戶-子分類-新增-Modal-顯示 */
const cstComSubKindAddModalShow = ref(false);
/** 系統設定-客戶-子分類-編輯-Modal-顯示 */
const cstComSubKindUpdateModalShow = ref(false);
/** 選取的子分類ID */
const selectedSubKindID = ref<number | null>(null);
/** 編輯子分類的資料 */
const editingSubKindData = ref<{
  managerCompanySubKindName: string;
  managerCompanySubKindIsEnable: boolean;
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
} | null>(null);
/** 客戶主分類ComboBox狀態 */
const getManyManagerCompanyMainKindComboBoxRef =
  ref<InstanceType<typeof GetManyManagerCompanyMainKindComboBox>>();
/** 客戶子分類ComboBox狀態 */
const getManyManagerCompanySubKindComboBoxRef =
  ref<InstanceType<typeof GetManyManagerCompanySubKindComboBox>>();
/** 系統設定-客戶-列表-頁面物件 */
const sysCompanyListViewObj = reactive<SysCompanyListViewMdl>({
  query: {
    atomCustomerGrade: null,
    managerCompanyMainKindID: null,
    managerCompanyMainKindName: null,
    managerCompanySubKindID: null,
    managerCompanySubKindName: null,
    managerCompanyName: null,
    pageIndex: 1,
    pageSize: 10,
  },
  customercompanyList: [],
  totalCount: 0,
});
/** 系統設定-客戶-主分類-列表-頁面物件 */
const sysComMainKindListViewObj = reactive<SysComMainKindListViewMdl>({
  query: {
    managerCompanyMainKindName: null,
    managerCompanyMainKindIsEnable: null,
    pageIndex: 1,
    pageSize: 10,
  },
  customercompanyMainKindList: [],
  totalCount: 0,
});
/** 系統設定-客戶-子分類-列表-頁面物件 */
const sysComSubKindListViewObj = reactive<MbsCstComSubKindListViewMdl>({
  query: {
    managerCompanySubKindName: null,
    managerCompanySubKindIsEnable: null,
    managerCompanyMainKindID: null,
    managerCompanyMainKindName: null,
    pageIndex: 1,
    pageSize: 10,
  },
  customercompanySubKindList: [],
  totalCount: 0,
});

//#endregion

//#region UI行為
/** 切換tab */
const changeTab = (tab: CompanyTabEnum) => {
  activeTab.value = tab;
  if (tab === CompanyTabEnum.MainKind) {
    getCompanyMainKindList();
  } else if (tab === CompanyTabEnum.SubKind) {
    getCompanySubKindList();
  } else {
    getCompanyList();
  }
};

//#endregion

//#region API / 資料流程
/** 取得【客戶】列表 */
const getCompanyList = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  let requestData: MbsSysComHttpGetManyCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    atomCustomerGrade: sysCompanyListViewObj.query.atomCustomerGrade!,
    managerCompanyMainKindID: sysCompanyListViewObj.query.managerCompanyMainKindID,
    managerCompanySubKindID: sysCompanyListViewObj.query.managerCompanySubKindID,
    managerCompanyName: sysCompanyListViewObj.query.managerCompanyName,
    pageIndex: sysCompanyListViewObj.query.pageIndex,
    pageSize: sysCompanyListViewObj.query.pageSize,
  };
  let responseData: MbsSysComHttpGetManyCompanyRspMdl | null = await getManyCompany(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 包裝資料
  sysCompanyListViewObj.customercompanyList = responseData.managerCompanyList?.map(
    (item: MbsSysComHttpGetManyCompanyRspItemMdl) => ({
      managerCompanyID: item.managerCompanyID,
      managerCompanyUnifiedNumber: item.managerCompanyUnifiedNumber,
      managerCompanyName: item.managerCompanyName,
      atomCustomerGrade: item.atomCustomerGrade,
      managerCompanyMainKindName: item.managerCompanyMainKindName,
      managerCompanySubKindName: item.managerCompanySubKindName,
    })
  );
  sysCompanyListViewObj.totalCount = responseData.totalCount;
};

/** 取得【客戶主分類】列表 */
const getCompanyMainKindList = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 呼叫api
  let requestData: MbsSysComHttpGetManyCompanyMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyMainKindName: sysComMainKindListViewObj.query.managerCompanyMainKindName,
    managerCompanyMainKindIsEnable: sysComMainKindListViewObj.query.managerCompanyMainKindIsEnable,
    pageIndex: sysComMainKindListViewObj.query.pageIndex,
    pageSize: sysComMainKindListViewObj.query.pageSize,
  };
  let responseData: MbsSysComHttpGetManyCompanyMainKindRspMdl | null =
    await getManyCompanyMainKind(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  sysComMainKindListViewObj.customercompanyMainKindList =
    responseData.managerCompanyMainKindList?.map(
      (item: MbsSysComHttpGetManyCompanyMainKindRspItemMdl) =>
        ({
          managerCompanyMainKindID: item.managerCompanyMainKindID,
          managerCompanyMainKindName: item.managerCompanyMainKindName,
          managerCompanyMainKindIsEnable: item.managerCompanyMainKindIsEnable,
        }) satisfies SysComMainKindListItemMdl
    );
  sysComMainKindListViewObj.totalCount = responseData.totalCount;
};

/** 取得【客戶子分類】列表 */
const getCompanySubKindList = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  let requestData: MbsSysComHttpGetManyCompanySubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanySubKindName: sysComSubKindListViewObj.query.managerCompanySubKindName,
    managerCompanySubKindIsEnable: sysComSubKindListViewObj.query.managerCompanySubKindIsEnable,
    managerCompanyMainKindID: sysComSubKindListViewObj.query.managerCompanyMainKindID,
    pageIndex: sysComSubKindListViewObj.query.pageIndex,
    pageSize: sysComSubKindListViewObj.query.pageSize,
  };
  let responseData: MbsSysComHttpGetManyCompanySubKindRspMdl | null =
    await getManyCompanySubKind(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  sysComSubKindListViewObj.customercompanySubKindList = responseData.managerCompanySubKindList?.map(
    (item: MbsSysComHttpGetManyCompanySubKindRspItemMdl) => ({
      managerCompanySubKindID: item.managerCompanySubKindID,
      managerCompanySubKindName: item.managerCompanySubKindName,
      managerCompanyMainKindName: item.managerCompanyMainKindName,
      managerCompanySubKindIsEnable: item.managerCompanySubKindIsEnable,
    })
  );
  sysComSubKindListViewObj.totalCount = responseData.totalCount;
};

//#endregion

//#region 按鈕事件
/** 點擊【新增公司】按鈕 */
const clickAddCompanyBtn = () => {
  router.push("/system/company/add");
};

/** 點擊【檢視公司】按鈕 */
const clickDetailCompanyBtn = (id: number) => {
  router.push(`/system/company/detail/${id}`);
};

/** 點擊【查詢公司】按鈕 */
const clickSearchCompanyBtn = () => {
  sysCompanyListViewObj.query.pageIndex = 1;
  getCompanyList();
};

/** 點擊【查詢主分類】按鈕 */
const clickSearchCompanyMainKindBtn = () => {
  sysComMainKindListViewObj.query.pageIndex = 1;
  getCompanyMainKindList();
};

/** 點擊【開啟新增主分類Modal】按鈕 */
const clickOpenAddMainKindModalBtn = () => {
  sysComMainKindAddModalShow.value = true;
};

/** 點擊【開啟編輯主分類Modal】按鈕 */
const clickOpenUpdateMainKindModalBtn = async (id: number) => {
  selectedMainKindID.value = id;
  
  // 先取得資料
  if (!requireToken()) return;
  
  const requestData: MbsSysComHttpGetCompanyMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyMainKindID: id,
  };
  
  const responseData: MbsSysComHttpGetCompanyMainKindRspMdl | null = await getCompanyMainKind(requestData);
  
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;
  
  // 儲存資料
  editingMainKindData.value = {
    managerCompanyMainKindName: responseData.managerCompanyMainKindName ?? '',
    managerCompanyMainKindIsEnable: responseData.managerCompanyMainKindIsEnable,
  };
  
  // 開啟 modal
  sysComMainKindUpdateModalShow.value = true;
};

/** 點擊【查詢子分類】按鈕 */
const clickSearchCompanySubKindBtn = () => {
  sysComSubKindListViewObj.query.pageIndex = 1;
  getCompanySubKindList();
};

/** 點擊【開啟新增子分類Modal】按鈕 */
const clickOpenAddSubKindModalBtn = () => {
  cstComSubKindAddModalShow.value = true;
};

/** 點擊【開啟編輯子分類Modal】按鈕 */
const clickOpenUpdateSubKindModalBtn = async (id: number) => {
  selectedSubKindID.value = id;
  
  // 先取得資料
  if (!requireToken()) return;
  
  const requestData: MbsSysComHttpGetCompanySubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanySubKindID: id,
  };
  
  const responseData: MbsSysComHttpGetCompanySubKindRspMdl | null = await getCompanySubKind(requestData);
  
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;
  
  // 儲存資料
  editingSubKindData.value = {
    managerCompanySubKindName: responseData.managerCompanySubKindName,
    managerCompanySubKindIsEnable: responseData.managerCompanySubKindIsEnable,
    managerCompanyMainKindID: responseData.managerCompanyMainKindID,
    managerCompanyMainKindName: responseData.managerCompanyMainKindName,
  };
  
  // 開啟 modal
  cstComSubKindUpdateModalShow.value = true;
};

/** 點擊【清除公司】按鈕 */
const clickClearSearchCompanyBtn = () => {
  sysCompanyListViewObj.query.atomCustomerGrade = null;
  sysCompanyListViewObj.query.managerCompanyMainKindID = null;
  sysCompanyListViewObj.query.managerCompanyMainKindName = null;
  sysCompanyListViewObj.query.managerCompanySubKindID = null;
  sysCompanyListViewObj.query.managerCompanySubKindName = null;
  sysCompanyListViewObj.query.managerCompanyName = null;

  getManyManagerCompanyMainKindComboBoxRef.value?.clear();
  getManyManagerCompanySubKindComboBoxRef.value?.clear();
};

/** 點擊【清除主分類】按鈕 */
const clickClearSearchCompanyMainKindBtn = () => {
  sysComMainKindListViewObj.query.managerCompanyMainKindName = null;
  sysComMainKindListViewObj.query.managerCompanyMainKindIsEnable = null;
};

/** 點擊【清除子分類】按鈕 */
const clickClearSearchCompanySubKindBtn = () => {
  sysComSubKindListViewObj.query.managerCompanySubKindName = null;
  sysComSubKindListViewObj.query.managerCompanyMainKindID = null;
  sysComSubKindListViewObj.query.managerCompanyMainKindName = null;
  sysComSubKindListViewObj.query.managerCompanySubKindIsEnable = null;

  getManyManagerCompanyMainKindComboBoxRef.value?.clear();
};

//#endregion

//#region 彈跳視窗事件
/** 新增主分類彈跳視窗:確認 */
const handleAddMainKindConfirm = async (data: {
  managerCompanyMainKindName: string;
  managerCompanyMainKindIsEnable: boolean;
}) => {
  // 檢查權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "create")) {
    displayError("您沒有權限執行此操作！");
    return;
  }

  // 檢查token
  if (!requireToken()) {
    return;
  }

  // 呼叫api
  const requestData: MbsSysComHttpAddCompanyMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyMainKindName: data.managerCompanyMainKindName,
    managerCompanyMainKindIsEnable: data.managerCompanyMainKindIsEnable,
  };

  const responseData: MbsSysComHttpAddCompanyMainKindRspMdl | null =
    await addCompanyMainKind(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增客戶主分類成功！");

  // 成功後關閉modal並重新整理列表
  sysComMainKindAddModalShow.value = false;
  await getCompanyMainKindList();
};

/** 編輯主分類彈跳視窗:確認 */
const handleUpdateMainKindConfirm = async (data: {
  managerCompanyMainKindName: string;
  managerCompanyMainKindIsEnable: boolean;
}) => {
  // 檢查權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "edit")) {
    displayError("您沒有權限執行此操作！");
    return;
  }

  // 檢查token
  if (!requireToken()) {
    return;
  }

  // 呼叫api
  const requestData: MbsSysComHttpUpdateCompanyMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyMainKindID: selectedMainKindID.value!,
    managerCompanyMainKindName: data.managerCompanyMainKindName,
    managerCompanyMainKindIsEnable: data.managerCompanyMainKindIsEnable,
  };

  const responseData: MbsSysComHttpUpdateCompanyMainKindRspMdl | null =
    await updateCompanyMainKind(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  // 成功後關閉modal並重新整理列表
  displaySuccess("更新客戶主分類成功！");
  sysComMainKindUpdateModalShow.value = false;
  await getCompanyMainKindList();
};

/** 新增子分類彈跳視窗:確認 */
const handleAddSubKindConfirm = async (data: {
  managerCompanySubKindName: string;
  managerCompanyMainKindID: number;
  managerCompanySubKindIsEnable: boolean;
}) => {
  // 檢查權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "create")) {
    displayError("您沒有權限執行此操作！");
    return;
  }

  // 檢查token
  if (!requireToken()) {
    return;
  }

  // 呼叫api
  const requestData: MbsSysComHttpAddCompanySubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanySubKindName: data.managerCompanySubKindName,
    managerCompanyMainKindID: data.managerCompanyMainKindID,
    managerCompanySubKindIsEnable: data.managerCompanySubKindIsEnable,
  };

  const responseData: MbsSysComHttpAddCompanySubKindRspMdl | null =
    await addCompanySubKind(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  // 成功後關閉modal並重新整理列表
  displaySuccess("新增客戶子分類成功！");
  cstComSubKindAddModalShow.value = false;
  await getCompanySubKindList();
};

/** 編輯子分類彈跳視窗:確認 */
const handleUpdateSubKindConfirm = async (data: {
  managerCompanySubKindName: string;
  managerCompanySubKindIsEnable: boolean;
}) => {
  // 檢查權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "edit")) {
    displayError("您沒有權限執行此操作！");
    return;
  }

  // 檢查token
  if (!requireToken()) {
    return;
  }

  // 呼叫api
  const requestData: MbsSysComHttpUpdateCompanySubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanySubKindID: selectedSubKindID.value!,
    managerCompanySubKindName: data.managerCompanySubKindName,
    managerCompanySubKindIsEnable: data.managerCompanySubKindIsEnable,
  };

  const responseData: MbsSysComHttpUpdateCompanySubKindRspMdl | null =
    await updateCompanySubKind(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  // 成功後關閉modal並重新整理列表
  displaySuccess("更新客戶子分類成功！");
  cstComSubKindUpdateModalShow.value = false;
  await getCompanySubKindList();
};

//#endregion

//#region 監聽
/** 監聽主分類ID變更，清除子分類 */
watch(
  () => sysCompanyListViewObj.query.managerCompanyMainKindID,
  (newValue, oldValue) => {
    // 只有當值真正改變時才清除（避免初始化時觸發）
    if (newValue !== oldValue && oldValue !== undefined) {
      sysCompanyListViewObj.query.managerCompanySubKindID = null;
      sysCompanyListViewObj.query.managerCompanySubKindName = null;
      getManyManagerCompanySubKindComboBoxRef.value?.clear();
    }
  }
);
//#endregion

//#region 生命週期
onMounted(() => {
  getCompanyList();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<CompanyTabEnum, string> = {
      [CompanyTabEnum.Customer]: "客戶",
      [CompanyTabEnum.SubKind]: "子分類",
      [CompanyTabEnum.MainKind]: "主分類",
    };
    setModuleTitle(titleMap[tab] ?? "客戶");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});

//#endregion
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden p-2">
    <!-- 標題列 -->
    <div class="flex items-center justify-between mb-4"></div>

    <!-- Tabs選單 -->
    <div class="flex mb-0 gap-y-4">
      <button :class="['tab-btn', { active: activeTab === CompanyTabEnum.Customer }]"
        @click="changeTab(CompanyTabEnum.Customer)">
        客戶
      </button>
      <button :class="['tab-btn', { active: activeTab === CompanyTabEnum.SubKind }]"
        @click="changeTab(CompanyTabEnum.SubKind)">
        子分類
      </button>
      <button :class="['tab-btn', { active: activeTab === CompanyTabEnum.MainKind }]"
        @click="changeTab(CompanyTabEnum.MainKind)">
        主分類
      </button>
    </div>

    <!-- Tabs內容 -->
    <div class="tab-content flex-1 overflow-hidden">
      <!--客戶-->
      <div v-if="activeTab === CompanyTabEnum.Customer" class="tab h-full">
        <div class="flex flex-col h-full overflow-hidden bg-white p-6 gap-4 rounded-tl-none rounded-lg">
          <!-- 客戶查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <div>
<input v-model="sysCompanyListViewObj.query.managerCompanyName" type="text" class="input-box"
                  placeholder="客戶名稱" />
              </div>

              <div>
<GetManyManagerCompanyMainKindComboBox ref="getManyManagerCompanyMainKindComboBoxRef"
                  v-model:managerCompanyMainKindID="sysCompanyListViewObj.query.managerCompanyMainKindID
                    " v-model:managerCompanyMainKindName="sysCompanyListViewObj.query.managerCompanyMainKindName
                      " :disabled="false" />
              </div>

              <div>
<GetManyManagerCompanySubKindComboBox ref="getManyManagerCompanySubKindComboBoxRef"
                  v-model:managerCompanyMainKindID="sysCompanyListViewObj.query.managerCompanyMainKindID
                    " v-model:managerCompanySubKindID="sysCompanyListViewObj.query.managerCompanySubKindID
                      " v-model:managerCompanySubKindName="sysCompanyListViewObj.query.managerCompanySubKindName
                        " :disabled="!sysCompanyListViewObj.query.managerCompanyMainKindID" />
              </div>

              <div>
<select v-model="sysCompanyListViewObj.query.atomCustomerGrade" class="select-box">
                  <option :value="null">請選擇客戶分級</option>
                  <option :value="DbAtomCustomerGradeEnum.A">A</option>
                  <option :value="DbAtomCustomerGradeEnum.B">B</option>
                  <option :value="DbAtomCustomerGradeEnum.C">C</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchCompanyBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchCompanyBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--客戶列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start">統編</th>
                  <th class="text-start">客戶名稱</th>
                  <th class="text-start">客戶主分類</th>
                  <th class="text-start">客戶子分類</th>
                  <th class="text-start">客戶分級</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysCompanyListViewObj.customercompanyList.length === 0">
                  <tr class="text-center">
                    <td colspan="6">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysCompanyListViewObj.customercompanyList"
                    :key="item.managerCompanyID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'view') &&
                        clickDetailCompanyBtn(item.managerCompanyID)
                    "
                  >
                    <td class="text-start">
                      {{ item.managerCompanyUnifiedNumber }}
                    </td>
                    <td class="text-start">
                      {{ item.managerCompanyName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerCompanyMainKindName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerCompanySubKindName }}
                    </td>
                    <td class="text-start">
                      {{ getCustomerGradeLabel(item.atomCustomerGrade) }}
                    </td>
                    <td class="text-start">
                      <details
                        v-if="employeeInfoStore.hasEveryonePermission(menu, 'view')"
                        class="relative"
                        @click.stop
                      >
                        <summary class="cursor-pointer list-none text-gray-500 hover:text-gray-700">
                          ...
                        </summary>
                        <div
                          class="absolute right-0 mt-2 w-24 rounded-md border border-gray-200 bg-white shadow z-10"
                        >
                          <button
                            class="w-full px-3 py-2 text-left text-sm hover:bg-gray-50"
                            @click.stop="clickDetailCompanyBtn(item.managerCompanyID)"
                          >
                            檢視
                          </button>
                        </div>
                      </details>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>
          <button
            v-if="
              activeTab === CompanyTabEnum.Customer &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
            style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickAddCompanyBtn"
          >
            +新增客戶
          </button>
          <div v-if="activeTab === CompanyTabEnum.Customer" class="flex justify-center pt-3">
            <!--客戶分頁 -->
            <Pagination :pageIndex="sysCompanyListViewObj.query.pageIndex"
              :pageSize="sysCompanyListViewObj.query.pageSize" :totalCount="sysCompanyListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysCompanyListViewObj.query.pageIndex = newPage;
                  getCompanyList();
                }
              " @update:page-size="
                (newSize: number) => {
                  sysCompanyListViewObj.query.pageSize = newSize;
                  sysCompanyListViewObj.query.pageIndex = 1;
                  getCompanyList();
                }
              " />
          </div>
        </div>
      </div>

      <!--客戶主分類-->
      <div v-if="activeTab === CompanyTabEnum.MainKind" class="tab h-full">
        <div class="flex flex-col h-full overflow-hidden bg-white p-6 gap-4">
          <!-- 客戶主分類查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <div>
<input v-model="sysComMainKindListViewObj.query.managerCompanyMainKindName" type="text"
                  class="input-box" placeholder="客戶主分類名稱" />
              </div>
              <div>
<select v-model="sysComMainKindListViewObj.query.managerCompanyMainKindIsEnable" class="select-box">
                  <option :value="null">全部</option>
                  <option :value="true">啟用</option>
                  <option :value="false">停用</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchCompanyMainKindBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchCompanyMainKindBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--客戶主分類列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start">客戶主分類名稱</th>
                  <th class="text-start">狀態</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysComMainKindListViewObj.customercompanyMainKindList.length === 0">
                  <tr class="text-center">
                    <td colspan="3">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysComMainKindListViewObj.customercompanyMainKindList"
                    :key="item.managerCompanyMainKindID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                        clickOpenUpdateMainKindModalBtn(item.managerCompanyMainKindID)
                    "
                  >
                    <td>
                      {{ item.managerCompanyMainKindName }}
                    </td>
                    <td class="text-start" :class="{
                      'enabled-text': item.managerCompanyMainKindIsEnable,
                      'disabled-text': !item.managerCompanyMainKindIsEnable,
                    }">
                      {{ item.managerCompanyMainKindIsEnable ? "啟用" : "停用" }}
                    </td>
                    <td class="text-start">
                      <details
                        v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                        class="relative"
                        @click.stop
                      >
                        <summary class="cursor-pointer list-none text-gray-500 hover:text-gray-700">
                          ...
                        </summary>
                        <div
                          class="absolute right-0 mt-2 w-24 rounded-md border border-gray-200 bg-white shadow z-10"
                        >
                          <button
                            class="w-full px-3 py-2 text-left text-sm hover:bg-gray-50"
                            @click.stop="clickOpenUpdateMainKindModalBtn(item.managerCompanyMainKindID)"
                          >
                            編輯
                          </button>
                        </div>
                      </details>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>

          <button
            v-if="
              activeTab === CompanyTabEnum.MainKind &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickOpenAddMainKindModalBtn"
          >
            +新增主分類
          </button>
          <div v-if="activeTab === CompanyTabEnum.MainKind" class="flex justify-center pt-3">
            <!-- 客戶主分類分頁 -->
            <Pagination :pageIndex="sysComMainKindListViewObj.query.pageIndex"
              :pageSize="sysComMainKindListViewObj.query.pageSize" :totalCount="sysComMainKindListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysComMainKindListViewObj.query.pageIndex = newPage;
                  getCompanyMainKindList();
                }
              " @update:page-size="
                (newSize: number) => {
                  sysComMainKindListViewObj.query.pageSize = newSize;
                  sysComMainKindListViewObj.query.pageIndex = 1;
                  getCompanyMainKindList();
                }
              " />
          </div>
        </div>
      </div>

      <!--客戶子分類-->
      <div v-if="activeTab === CompanyTabEnum.SubKind" class="tab h-full">
        <div class="flex flex-col h-full overflow-hidden bg-white p-6 gap-4">
          <!-- 客戶子分類查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <div>
<input v-model="sysComSubKindListViewObj.query.managerCompanySubKindName" type="text" class="input-box"
                  placeholder="客戶子分類名稱" />
              </div>

              <div>
<GetManyManagerCompanyMainKindComboBox ref="getManyManagerCompanyMainKindComboBoxRef"
                  v-model:managerCompanyMainKindID="sysComSubKindListViewObj.query.managerCompanyMainKindID
                    " v-model:managerCompanyMainKindName="sysComSubKindListViewObj.query.managerCompanyMainKindName
                      " :disabled="false" />
              </div>

              <div>
<select v-model="sysComSubKindListViewObj.query.managerCompanySubKindIsEnable" class="select-box">
                  <option :value="null">全部</option>
                  <option :value="true">啟用</option>
                  <option :value="false">停用</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchCompanySubKindBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchCompanySubKindBtn">清除</button>
            </div>
          </div>

          <hr />

          <!-- 客戶子分類列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start">客戶子分類名稱</th>
                  <th class="text-start">客戶主分類</th>
                  <th class="text-start">狀態</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysComSubKindListViewObj.customercompanySubKindList.length === 0">
                  <tr class="text-center">
                    <td colspan="4">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysComSubKindListViewObj.customercompanySubKindList"
                    :key="item.managerCompanySubKindID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                        clickOpenUpdateSubKindModalBtn(item.managerCompanySubKindID)
                    "
                  >
                    <td>
                      {{ item.managerCompanySubKindName }}
                    </td>
                    <td>
                      {{ item.managerCompanyMainKindName }}
                    </td>
                    <td class="text-start" :class="{
                      'enabled-text': item.managerCompanySubKindIsEnable,
                      'disabled-text': !item.managerCompanySubKindIsEnable,
                    }">
                      {{ item.managerCompanySubKindIsEnable ? "啟用" : "停用" }}
                    </td>
                    <td class="text-start">
                      <details
                        v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                        class="relative"
                        @click.stop
                      >
                        <summary class="cursor-pointer list-none text-gray-500 hover:text-gray-700">
                          ...
                        </summary>
                        <div
                          class="absolute right-0 mt-2 w-24 rounded-md border border-gray-200 bg-white shadow z-10"
                        >
                          <button
                            class="w-full px-3 py-2 text-left text-sm hover:bg-gray-50"
                            @click.stop="clickOpenUpdateSubKindModalBtn(item.managerCompanySubKindID)"
                          >
                            編輯
                          </button>
                        </div>
                      </details>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>

          <button
            v-if="
              activeTab === CompanyTabEnum.SubKind &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickOpenAddSubKindModalBtn"
          >
            +新增子分類
          </button>
          <div v-if="activeTab === CompanyTabEnum.SubKind" class="flex justify-center pt-3">
            <!-- 子分類分頁 -->
            <Pagination :pageIndex="sysComSubKindListViewObj.query.pageIndex"
              :pageSize="sysComSubKindListViewObj.query.pageSize" :totalCount="sysComSubKindListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysComSubKindListViewObj.query.pageIndex = newPage;
                  getCompanySubKindList();
                }
              " @update:page-size="
                (newSize: number) => {
                  sysComSubKindListViewObj.query.pageSize = newSize;
                  sysComSubKindListViewObj.query.pageIndex = 1;
                  getCompanySubKindList();
                }
              " />
          </div>
        </div>
      </div>
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />


    <!-- 新增主分類 Modal -->
    <AddSystemCompanyMainKindModal v-if="sysComMainKindAddModalShow" @close="sysComMainKindAddModalShow = false"
      @confirm="handleAddMainKindConfirm" />
      
    <!-- 編輯主分類 Modal -->
    <UpdateSystemCompanyMainKindModal v-if="sysComMainKindUpdateModalShow && editingMainKindData"
      :managerCompanyMainKindID="selectedMainKindID!" 
      :initialData="editingMainKindData"
      @close="sysComMainKindUpdateModalShow = false"
      @confirm="handleUpdateMainKindConfirm" />

    <!-- 新增子分類 Modal -->
    <AddSystemCompanySubKindModal v-if="cstComSubKindAddModalShow" @close="cstComSubKindAddModalShow = false"
      @confirm="handleAddSubKindConfirm" />
    <!-- 編輯子分類 Modal -->
    <UpdateSystemCompanySubKindModal v-if="cstComSubKindUpdateModalShow && editingSubKindData" 
      :managerCompanySubKindID="selectedSubKindID!"
      :initialData="editingSubKindData"
      @close="cstComSubKindUpdateModalShow = false" 
      @confirm="handleUpdateSubKindConfirm" />
  </div>
</template>
