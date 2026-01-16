<script setup lang="ts">
//#region 引入
import { reactive, ref, onMounted, watch, onBeforeUnmount } from "vue";
import router from "@/router";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useAuth } from "@/composables/useAuth";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
import {
  getManyProductSubKind,
  getManyProductMainKind,
  getManyProduct,
  addProductMainKind,
  updateProductMainKind,
  addProductSubKind,
  updateProductSubKind,
} from "@/services";
import { formatCurrency } from "@/utils/currencyFormatter";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import AddSystemProductMainKindModal from "./components/AddSystemProductMainKindModal.vue";
import UpdateSystemProductMainKindModal from "./components/UpdateSystemProductMainKindModal.vue";
import AddSystemProductSubKindModal from "./components/AddSystemProductSubKindModal.vue";
import UpdateSystemProductSubKindModal from "./components/UpdateSystemProductSubKindModal.vue";
import type {
  MbsSysPrdHttpAddMainKindReqMdl,
  MbsSysPrdHttpAddMainKindRspMdl,
  MbsSysPrdHttpAddSubKindReqMdl,
  MbsSysPrdHttpAddSubKindRspMdl,
  MbsSysPrdHttpGetManyMainKindReqMdl,
  MbsSysPrdHttpGetManyMainKindRspItemMdl,
  MbsSysPrdHttpGetManyReqMdl,
  MbsSysPrdHttpGetManyRspItemMdl,
  MbsSysPrdHttpGetManySubKindReqMdl,
  MbsSysPrdHttpGetManySubKindRspItemMdl,
  MbsSysPrdHttpUpdateMainKindReqMdl,
  MbsSysPrdHttpUpdateSubKindReqMdl,
  MbsSysPrdHttpUpdateSubKindRspMdl,
} from "@/services/pms-http/system/product/systemProductHttpFormat";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
import {
  SystemProductMainKindAddPayload,
  SystemProductMainKindUpdatePayload,
  SystemProductSubKindAddPayloadMdl,
  SystemProductSubKindUpdatePayload,
} from "./components/specification/model/payload";

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
const isExporting = ref(false);

//#endregion

//#region 型別定義
//-------------------------------------------------------------
/** 產品頁籤種類列舉 */
enum ProductTabEnum {
  Product = "product",
  MainKind = "mainKind",
  SubKind = "subKind",
}
//-------------------------------------------------------------
/** 系統設定-產品-查詢 */
interface SysPrdQuery {
  managerProductMainKindID: number | null;
  managerProductMainKindName: string | null;
  managerProductSubKindID: number | null;
  managerProductSubKindName: string | null;
  managerProductIsKey: boolean | null;
  managerProductName: string | null;
  managerProductSpecificationName: string | null;
  managerProductSpecificationIsEnable: boolean | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-產品-列表-項目 */
interface SysPrdListItem {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerProductIsKey: boolean;
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}

/** 系統設定-產品-列表-頁面模型 */
interface SysPrdListViewMdl {
  query: SysPrdQuery;
  systemProductList: SysPrdListItem[];
  totalCount: number;
}

//------------------------------------------------------------
/** 系統設定-產品-主分類-查詢 */
interface SysPrdMainKindQuery {
  managerProductMainKindName: string | null;
  managerProductMainKindIsEnable: boolean | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-產品-主分類-列表-項目 */
interface SysPrdMainKindListItem {
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductMainKindCommissionRate: number;
  managerProductMainKindIsEnable: boolean;
}

/** 系統設定-產品-主分類-列表-頁面模型 */
interface SysPrdMainKindListViewMdl {
  query: SysPrdMainKindQuery;
  managerProductMainKindList: SysPrdMainKindListItem[];
  totalCount: number;
}

//------------------------------------------------------------
/** 系統設定-產品-子分類-查詢模型 */
interface SysPrdSubKindQueryMdl {
  managerProductSubKindName: string | null;
  managerProductSubKindIsEnable: boolean | null;
  managerProductMainKindID: number | null;
  managerProductMainKindName: string | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-產品-子分類-列表-項目 */
interface SysPrdSubKindListItem {
  managerProductSubKindID: number;
  managerProductMainKindName: string | null;
  managerProductSubKindName: string | null;
  managerProductSubKindCommissionRate: number;
  managerProductSubKindIsEnable: boolean;
}

/** 系統設定-產品-子分類-列表-頁面模型 */
interface SysPrdSubKindListViewMdl {
  query: SysPrdSubKindQueryMdl;
  managerProductSubKindList: SysPrdSubKindListItem[];
  totalCount: number;
}

/** 系統-產品-主分類-原始模型 */
interface SystemProductMainKindOriginalMdl {
  managerProductMainKindID: number;
  managerProductMainKindName: string | null;
  managerProductMainKindCommissionRate: number;
  managerProductMainKindIsEnable: boolean;
}

/** 系統-產品-子分類-原始模型 */
interface SystemProductSubKindOriginalMdl {
  managerProductSubKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerProductSubKindCommissionRate: number;
  managerProductSubKindIsEnable: boolean;
}
//------------------------------------------------------------

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemProduct;
/** 目前作用中的tab */
const activeTab = ref<ProductTabEnum>(ProductTabEnum.Product);
/** 顯示-產品-主分類-新增-Modal */
const showProductMainAddModal = ref(false);
/** 顯示-產品-主分類-更新-Modal */
const showProductMainKindUpdateModal = ref(false);
/** 系統-產品-主分類-更新模型 */
const updatingProductMainKind = ref<SystemProductMainKindUpdatePayload | null>(null);
/** 顯示-產品-子分類-新增-Modal */
const showProductSubKindAddModal = ref(false);
/** 系統設定-產品-子分類-編輯-Modal-顯示 */
const showProductSubKindUpdateModal = ref(false);
/** 系統-產品-子分類-更新模型 */
const updatingProductSubKind = ref<SystemProductSubKindUpdatePayload | null>(null);

/** 系統設定-產品-列表-頁面物件 */
const sysPrdListViewObj = reactive<SysPrdListViewMdl>({
  query: {
    managerProductMainKindID: null,
    managerProductMainKindName: null,
    managerProductSubKindID: null,
    managerProductSubKindName: null,
    managerProductName: null,
    managerProductSpecificationName: null,
    managerProductIsKey: null,
    managerProductSpecificationIsEnable: null,
    pageIndex: 1,
    pageSize: 10,
  },
  systemProductList: [],
  totalCount: 0,
});
/** 系統設定-產品-主分類-列表-頁面物件 */
const sysPrdMainKindListViewObj = reactive<SysPrdMainKindListViewMdl>({
  query: {
    managerProductMainKindName: null,
    managerProductMainKindIsEnable: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerProductMainKindList: [],
  totalCount: 0,
});
/** 系統設定-產品-子分類-列表-頁面物件 */
const sysPrdSubKindListViewObj = reactive<SysPrdSubKindListViewMdl>({
  query: {
    managerProductSubKindName: null,
    managerProductSubKindIsEnable: null,
    managerProductMainKindID: null,
    managerProductMainKindName: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerProductSubKindList: [],
  totalCount: 0,
});
/** 系統-產品-主分類-原始物件 */
const systemProductMainKindOriginalObj = reactive<SystemProductMainKindOriginalMdl>({
  managerProductMainKindID: 0,
  managerProductMainKindName: null,
  managerProductMainKindCommissionRate: 0,
  managerProductMainKindIsEnable: true,
});
/** 系統-產品-子分類-原始物件 */
const systemProductSubKindOriginalObj = reactive<SystemProductSubKindOriginalMdl>({
  managerProductSubKindID: 0,
  managerProductMainKindName: "",
  managerProductSubKindName: "",
  managerProductSubKindCommissionRate: 0,
  managerProductSubKindIsEnable: true,
});

//#endregion

//#region UI行為
/** 切換tab */
const changeTab = (tab: ProductTabEnum) => {
  activeTab.value = tab;

  if (tab === ProductTabEnum.MainKind) {
    getProductMainKindList();
  } else if (tab === ProductTabEnum.SubKind) {
    getProductSubKindList();
  } else {
    getProductList();
  }
};
//#endregion

//#region API / 資料流程
/** 取得【產品】列表 */
const getProductList = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  let requestData: MbsSysPrdHttpGetManyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductMainKindID: sysPrdListViewObj.query.managerProductMainKindID,
    managerProductSubKindID: sysPrdListViewObj.query.managerProductSubKindID,
    managerProductIsKey: sysPrdListViewObj.query.managerProductIsKey,
    managerProductName: sysPrdListViewObj.query.managerProductName,
    managerProductSpecificationName: sysPrdListViewObj.query.managerProductSpecificationName,
    managerProductSpecificationIsEnable:
      sysPrdListViewObj.query.managerProductSpecificationIsEnable,
    pageIndex: sysPrdListViewObj.query.pageIndex,
    pageSize: sysPrdListViewObj.query.pageSize,
  };
  let responseData = await getManyProduct(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  sysPrdListViewObj.systemProductList = responseData.managerProductList?.map(
    (item: MbsSysPrdHttpGetManyRspItemMdl) => ({
      managerProductID: item.managerProductID,
      managerProductName: item.managerProductName,
      managerProductMainKindName: item.managerProductMainKindName,
      managerProductSubKindName: item.managerProductSubKindName,
      managerProductIsKey: item.managerProductIsKey,
      managerProductSpecificationName: item.managerProductSpecificationName,
      managerProductSpecificationSellPrice: item.managerProductSpecificationSellPrice,
      managerProductSpecificationCostPrice: item.managerProductSpecificationCostPrice,
      managerProductSpecificationIsEnable: item.managerProductSpecificationIsEnable,
    })
  );
  sysPrdListViewObj.totalCount = responseData.totalCount;
};

/** 取得【產品主分類】列表 */
const getProductMainKindList = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  let requestData: MbsSysPrdHttpGetManyMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductMainKindName: sysPrdMainKindListViewObj.query.managerProductMainKindName,
    managerProductMainKindIsEnable: sysPrdMainKindListViewObj.query.managerProductMainKindIsEnable,
    pageIndex: sysPrdMainKindListViewObj.query.pageIndex,
    pageSize: sysPrdMainKindListViewObj.query.pageSize,
  };
  let responseData = await getManyProductMainKind(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  sysPrdMainKindListViewObj.managerProductMainKindList =
    responseData.managerProductMainKindList?.map(
      (item: MbsSysPrdHttpGetManyMainKindRspItemMdl) =>
        ({
          managerProductMainKindID: item.managerProductMainKindID,
          managerProductMainKindName: item.managerProductMainKindName,
          managerProductMainKindCommissionRate: Number(
            (item.managerProductMainKindCommissionRate * 100).toFixed(2)
          ),
          managerProductMainKindIsEnable: item.managerProductMainKindIsEnable,
        }) satisfies MbsSysPrdHttpGetManyMainKindRspItemMdl
    );
  sysPrdMainKindListViewObj.totalCount = responseData.totalCount;
};

/** 取得【產品子分類】列表 */
const getProductSubKindList = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  let requestData: MbsSysPrdHttpGetManySubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductSubKindName: sysPrdSubKindListViewObj.query.managerProductSubKindName,
    managerProductSubKindIsEnable: sysPrdSubKindListViewObj.query.managerProductSubKindIsEnable,
    managerProductMainKindID: sysPrdSubKindListViewObj.query.managerProductMainKindID,
    pageIndex: sysPrdSubKindListViewObj.query.pageIndex,
    pageSize: sysPrdSubKindListViewObj.query.pageSize,
  };
  let responseData = await getManyProductSubKind(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  sysPrdSubKindListViewObj.managerProductSubKindList = responseData.managerProductSubKindList?.map(
    (item: MbsSysPrdHttpGetManySubKindRspItemMdl) => ({
      managerProductMainKindID: item.managerProductMainKindID,
      managerProductSubKindID: item.managerProductSubKindID,
      managerProductSubKindName: item.managerProductSubKindName,
      managerProductMainKindName: item.managerProductMainKindName,
      managerProductSubKindCommissionRate: Number(
        (item.managerProductSubKindCommissionRate * 100).toFixed(2)
      ),
      managerProductSubKindIsEnable: item.managerProductSubKindIsEnable,
    })
  );
  sysPrdSubKindListViewObj.totalCount = responseData.totalCount;
};

/** 處理新增產品主分類 */
const handleAddProductMainKind = async (payload: SystemProductMainKindAddPayload) => {
  // 檢查token
  if (!requireToken()) return;

  //轉換 業務毛利率百分比為小數
  const commissionRateDecimal = payload.managerProductMainKindCommissionRate / 100;
  // 呼叫api
  const requestData: MbsSysPrdHttpAddMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductMainKindName: payload.managerProductMainKindName,
    managerProductMainKindIsEnable: payload.managerProductMainKindIsEnable,
    managerProductMainKindCommissionRate: commissionRateDecimal,
  };
  const responseData: MbsSysPrdHttpAddMainKindRspMdl | null = await addProductMainKind(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增產品主分類成功！");

  // 重新整理主分類列表
  await getProductMainKindList();

  // 關閉Modal
  showProductMainAddModal.value = false;
};

/** 取得【產品主分類】變更欄位 */
const getProductManKindChangedFields = (
  original: SystemProductMainKindUpdatePayload,
  current: SystemProductMainKindUpdatePayload
): Partial<Omit<SystemProductMainKindUpdatePayload, "managerProductMainKindID">> => {
  const changes: Partial<Omit<SystemProductMainKindUpdatePayload, "managerProductMainKindID">> = {};

  // 比較【名稱】
  if (current.managerProductMainKindName !== original.managerProductMainKindName) {
    changes.managerProductMainKindName = current.managerProductMainKindName;
  }

  // 比較【狀態】
  if (current.managerProductMainKindIsEnable !== original.managerProductMainKindIsEnable) {
    changes.managerProductMainKindIsEnable = current.managerProductMainKindIsEnable;
  }
  // 比較【業務毛利率】
  if (
    current.managerProductMainKindCommissionRate !== original.managerProductMainKindCommissionRate
  ) {
    changes.managerProductMainKindCommissionRate = current.managerProductMainKindCommissionRate;
  }
  return changes;
};

/** 處理【更新產品主分類】 */
const handleUpdateProductMainKind = async (payload: SystemProductMainKindUpdatePayload) => {
  if (!requireToken()) return;

  // 取得變更的欄位
  const changedFields = getProductManKindChangedFields(systemProductMainKindOriginalObj, payload);
  // 轉換業務毛利率百分比為小數
  if (changedFields.managerProductMainKindCommissionRate) {
    changedFields.managerProductMainKindCommissionRate =
      changedFields.managerProductMainKindCommissionRate / 100;
  }
  // 呼叫api
  const requestData: MbsSysPrdHttpUpdateMainKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductMainKindID: payload.managerProductMainKindID,
    ...(changedFields as Omit<SystemProductMainKindUpdatePayload, "managerProductMainKindID">),
  };

  if (Object.keys(changedFields).length === 0) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  const responseData = await updateProductMainKind(
    requestData as MbsSysPrdHttpUpdateMainKindReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新產品主分類成功！");

  // 重新整理主分類列表
  await getProductMainKindList();
};

/** 處理【新增產品子分類】 */
const handleAddProductSubKind = async (payload: SystemProductSubKindAddPayloadMdl) => {
  if (!requireToken()) {
    return;
  }

  //轉換 業務毛利率百分比為小數
  const commissionRateDecimal = payload.managerProductSubKindCommissionRate / 100;

  // 呼叫api
  const requestData: MbsSysPrdHttpAddSubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductMainKindID: payload.managerProductMainKindID,
    managerProductSubKindName: payload.managerProductSubKindName,
    managerProductSubKindCommissionRate: commissionRateDecimal,
    managerProductSubKindIsEnable: payload.managerProductSubKindIsEnable,
  };

  const responseData: MbsSysPrdHttpAddSubKindRspMdl | null = await addProductSubKind(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增產品子分類成功！");

  // 重新整理主分類列表
  await getProductSubKindList();
};

/** 取得【產品子分類】變更欄位 */
const getProductSubKindChangedFields = (
  original: SystemProductSubKindUpdatePayload,
  current: SystemProductSubKindUpdatePayload
): Partial<Omit<SystemProductSubKindUpdatePayload, "managerProductSubKindID">> => {
  const changes: Partial<Omit<SystemProductSubKindUpdatePayload, "managerProductSubKindID">> = {};

  // 比較【名稱】
  if (current.managerProductSubKindName !== original.managerProductSubKindName) {
    changes.managerProductSubKindName = current.managerProductSubKindName;
  }

  // 比較【業務毛利率】
  if (
    current.managerProductSubKindCommissionRate !== original.managerProductSubKindCommissionRate
  ) {
    changes.managerProductSubKindCommissionRate = current.managerProductSubKindCommissionRate;
  }

  // 比較【狀態】
  if (current.managerProductSubKindIsEnable !== original.managerProductSubKindIsEnable) {
    changes.managerProductSubKindIsEnable = current.managerProductSubKindIsEnable;
  }

  return changes;
};

/** 處理【更新產品子分類】 */
const handleUpdateProductSubKind = async (payload: SystemProductSubKindUpdatePayload) => {
  // 檢查token
  if (!requireToken()) return;

  // 取得變更的欄位
  const changedFields = getProductSubKindChangedFields(systemProductSubKindOriginalObj, payload);

  // 轉換業務毛利率百分比為小數
  if (changedFields.managerProductSubKindCommissionRate !== undefined) {
    changedFields.managerProductSubKindCommissionRate =
      changedFields.managerProductSubKindCommissionRate / 100;
  }

  // 呼叫api
  const requestData: MbsSysPrdHttpUpdateSubKindReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductSubKindID: payload.managerProductSubKindID,
    ...(changedFields as Omit<SystemProductSubKindUpdatePayload, "managerProductSubKindID">),
  };
  if (Object.keys(changedFields).length === 0) {
    displayError("沒有任何變更需要儲存！");
    return;
  }
  const responseData: MbsSysPrdHttpUpdateSubKindRspMdl | null = await updateProductSubKind(
    requestData as MbsSysPrdHttpUpdateSubKindReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新產品子分類成功！");

  // 重新整理子分類列表
  await getProductSubKindList();
};
//#endregion

//#region 按鈕事件
/** 點擊【查詢主分類】按鈕 */
const clickSearchProductMainKindBtn = () => {
  sysPrdMainKindListViewObj.query.pageIndex = 1;
  getProductMainKindList();
};

/** 點擊【開啟新增主分類Modal】按鈕 */
const clickOpenAddMainKindModalBtn = () => {
  showProductMainAddModal.value = true;
};

/** 點擊【開啟更新主分類Modal】按鈕 */
const clickOpenUpdateMainKindModalBtn = (item: SysPrdMainKindListItem) => {
  updatingProductMainKind.value = {
    managerProductMainKindID: item.managerProductMainKindID,
    managerProductMainKindName: item.managerProductMainKindName,
    managerProductMainKindIsEnable: item.managerProductMainKindIsEnable,
    managerProductMainKindCommissionRate: item.managerProductMainKindCommissionRate,
  };

  // 保存原始值
  Object.assign(systemProductMainKindOriginalObj, {
    managerProductMainKindName: item.managerProductMainKindName,
    managerProductMainKindIsEnable: item.managerProductMainKindIsEnable,
    managerProductMainKindCommissionRate: item.managerProductMainKindCommissionRate,
  });

  showProductMainKindUpdateModal.value = true;
};

/** 點擊【查詢子分類】按鈕 */
const clickSearchProductSubKindBtn = () => {
  sysPrdSubKindListViewObj.query.pageIndex = 1;
  getProductSubKindList();
};

/** 點擊【開啟新增子分類Modal】按鈕 */
const clickOpenAddSubKindModalBtn = () => {
  showProductSubKindAddModal.value = true;
};

/** 點擊【開啟編輯子分類Modal】按鈕 */
const clickOpenUpdateSubKindModalBtn = (item: SysPrdSubKindListItem) => {
  updatingProductSubKind.value = {
    managerProductSubKindID: item.managerProductSubKindID,
    managerProductSubKindName: item.managerProductSubKindName || "",
    managerProductMainKindName: item.managerProductMainKindName || "",
    managerProductSubKindCommissionRate: item.managerProductSubKindCommissionRate,
    managerProductSubKindIsEnable: item.managerProductSubKindIsEnable,
  };
  // 保存原始值
  Object.assign(systemProductSubKindOriginalObj, {
    managerProductSubKindName: item.managerProductSubKindName || "",
    managerProductSubKindCommissionRate: item.managerProductSubKindCommissionRate,
    managerProductSubKindIsEnable: item.managerProductSubKindIsEnable,
  });

  showProductSubKindUpdateModal.value = true;
};

/** 點擊【查詢產品】按鈕 */
const clickSearchProductBtn = () => {
  sysPrdListViewObj.query.pageIndex = 1;
  getProductList();
};

/** 點擊【新增產品】按鈕 */
const clickAddProductBtn = () => {
  router.push("/system/product/add");
};

/** 點擊【檢視產品】按鈕 */
const clickDetailProductBtn = (id: number) => {
  router.push(`/system/product/detail/${id}`);
};

/** 點擊【清除主分類區塊】按鈕 */
const clickClearSearchProductMainKindBtn = () => {
  sysPrdMainKindListViewObj.query.managerProductMainKindName = null;
  sysPrdMainKindListViewObj.query.managerProductMainKindIsEnable = null;
};

/** 點擊【清除子分類區塊】按鈕 */
const clickClearSearchProductSubKindBtn = () => {
  sysPrdSubKindListViewObj.query.managerProductSubKindName = null;
  sysPrdSubKindListViewObj.query.managerProductMainKindID = null;
  sysPrdSubKindListViewObj.query.managerProductMainKindName = null;
  sysPrdSubKindListViewObj.query.managerProductSubKindIsEnable = null;
};

/** 點擊【清除產品區塊】按鈕 */
const clickClearSearchProductBtn = () => {
  sysPrdListViewObj.query.managerProductMainKindID = null;
  sysPrdListViewObj.query.managerProductMainKindName = null;
  sysPrdListViewObj.query.managerProductSubKindID = null;
  sysPrdListViewObj.query.managerProductSubKindName = null;
  sysPrdListViewObj.query.managerProductIsKey = null;
  sysPrdListViewObj.query.managerProductName = null;
  sysPrdListViewObj.query.managerProductSpecificationName = null;
  sysPrdListViewObj.query.managerProductSpecificationIsEnable = null;
  sysPrdListViewObj.query.managerProductSubKindName = null;
};

//#endregion

//#region 監聽
/** 監聽主分類ID變更，清除子分類 */
watch(
  () => sysPrdListViewObj.query.managerProductMainKindID,
  (newValue, oldValue) => {
    // 只有當值真正改變時才清除（避免初始化時觸發）
    if (newValue !== oldValue && oldValue !== undefined) {
      sysPrdListViewObj.query.managerProductSubKindID = null;
      sysPrdListViewObj.query.managerProductSubKindName = null;
    }
  }
);
//#endregion

//#region 生命週期
onMounted(() => {
  getProductList();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<ProductTabEnum, string> = {
      [ProductTabEnum.Product]: "產品",
      [ProductTabEnum.SubKind]: "子分類",
      [ProductTabEnum.MainKind]: "主分類",
    };
    setModuleTitle(titleMap[tab] ?? "產品");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});
//#endregion

//#region 匯出/備份
const downloadBlob = (content: string, filename: string, type: string) => {
  const blob = new Blob([content], { type });
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = url;
  link.download = filename;
  link.click();
  URL.revokeObjectURL(url);
};

const toCsv = (headers: string[], rows: (string | number | boolean | null | undefined)[][]) => {
  const escapeCell = (value: string | number | boolean | null | undefined) => {
    const text = value === null || value === undefined ? "" : String(value);
    if (/[",\n]/.test(text)) {
      return `"${text.replace(/"/g, '""')}"`;
    }
    return text;
  };
  const lines = [
    headers.join(","),
    ...rows.map((row) => row.map((cell) => escapeCell(cell)).join(",")),
  ];
  return `\uFEFF${lines.join("\n")}`;
};

const toXls = (headers: string[], rows: (string | number | boolean | null | undefined)[][]) => {
  const escapeHtml = (value: string | number | boolean | null | undefined) => {
    const text = value === null || value === undefined ? "" : String(value);
    return text
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;");
  };
  const headerRow = headers.map((header) => `<th>${escapeHtml(header)}</th>`).join("");
  const bodyRows = rows
    .map((row) => `<tr>${row.map((cell) => `<td>${escapeHtml(cell)}</td>`).join("")}</tr>`)
    .join("");
  return `<!DOCTYPE html><html><head><meta charset="utf-8" /></head><body><table>${headerRow}${bodyRows}</table></body></html>`;
};

const fetchAllProducts = async () => {
  const allItems: MbsSysPrdHttpGetManyRspItemMdl[] = [];
  const pageSize = 200;
  let pageIndex = 1;
  let totalCount = 0;
  while (true) {
    const responseData = await getManyProduct({
      employeeLoginToken: tokenStore.token!,
      managerProductMainKindID: null,
      managerProductSubKindID: null,
      managerProductIsKey: null,
      managerProductName: null,
      managerProductSpecificationName: null,
      managerProductSpecificationIsEnable: null,
      pageIndex,
      pageSize,
    });
    if (!responseData) return null;
    const isSuccess = handleErrorCode(responseData.errorCode);
    if (!isSuccess) return null;
    const items = responseData.managerProductList ?? [];
    allItems.push(...items);
    totalCount = responseData.totalCount ?? allItems.length;
    if (allItems.length >= totalCount || items.length === 0) break;
    pageIndex += 1;
  }
  return allItems;
};

const fetchAllMainKinds = async () => {
  const allItems: MbsSysPrdHttpGetManyMainKindRspItemMdl[] = [];
  const pageSize = 200;
  let pageIndex = 1;
  let totalCount = 0;
  while (true) {
    const responseData = await getManyProductMainKind({
      employeeLoginToken: tokenStore.token!,
      managerProductMainKindName: null,
      managerProductMainKindIsEnable: null,
      pageIndex,
      pageSize,
    });
    if (!responseData) return null;
    const isSuccess = handleErrorCode(responseData.errorCode);
    if (!isSuccess) return null;
    const items = responseData.managerProductMainKindList ?? [];
    allItems.push(...items);
    totalCount = responseData.totalCount ?? allItems.length;
    if (allItems.length >= totalCount || items.length === 0) break;
    pageIndex += 1;
  }
  return allItems;
};

const fetchAllSubKinds = async () => {
  const allItems: MbsSysPrdHttpGetManySubKindRspItemMdl[] = [];
  const pageSize = 200;
  let pageIndex = 1;
  let totalCount = 0;
  while (true) {
    const responseData = await getManyProductSubKind({
      employeeLoginToken: tokenStore.token!,
      managerProductSubKindName: null,
      managerProductSubKindIsEnable: null,
      managerProductMainKindID: null,
      pageIndex,
      pageSize,
    });
    if (!responseData) return null;
    const isSuccess = handleErrorCode(responseData.errorCode);
    if (!isSuccess) return null;
    const items = responseData.managerProductSubKindList ?? [];
    allItems.push(...items);
    totalCount = responseData.totalCount ?? allItems.length;
    if (allItems.length >= totalCount || items.length === 0) break;
    pageIndex += 1;
  }
  return allItems;
};

const exportProductCatalog = async () => {
  if (isExporting.value || !requireToken()) return;
  isExporting.value = true;
  try {
    const products = await fetchAllProducts();
    const mainKinds = await fetchAllMainKinds();
    const subKinds = await fetchAllSubKinds();
    if (!products || !mainKinds || !subKinds) {
      displayError("匯出失敗，請稍後再試！");
      return;
    }

    localStorage.setItem("cache.system.product.list", JSON.stringify(products));
    localStorage.setItem("cache.system.product.main-kind.list", JSON.stringify(mainKinds));
    localStorage.setItem("cache.system.product.sub-kind.list", JSON.stringify(subKinds));

    const timestamp = new Date().toISOString().replace(/[:.]/g, "-");
    const exportPayload = {
      exportedAt: new Date().toISOString(),
      products,
      mainKinds,
      subKinds,
    };

    downloadBlob(
      JSON.stringify(exportPayload, null, 2),
      `system-product-export-${timestamp}.json`,
      "application/json"
    );

    const productHeaders = [
      "managerProductID",
      "managerProductName",
      "managerProductMainKindName",
      "managerProductSubKindName",
      "managerProductIsKey",
      "managerProductSpecificationName",
      "managerProductSpecificationSellPrice",
      "managerProductSpecificationCostPrice",
      "managerProductSpecificationIsEnable",
    ];
    const productRows = products.map((item) => [
      item.managerProductID,
      item.managerProductName,
      item.managerProductMainKindName,
      item.managerProductSubKindName,
      item.managerProductIsKey,
      item.managerProductSpecificationName,
      item.managerProductSpecificationSellPrice,
      item.managerProductSpecificationCostPrice,
      item.managerProductSpecificationIsEnable,
    ]);
    downloadBlob(
      toCsv(productHeaders, productRows),
      `system-products-${timestamp}.csv`,
      "text/csv"
    );
    downloadBlob(
      toXls(productHeaders, productRows),
      `system-products-${timestamp}.xls`,
      "application/vnd.ms-excel"
    );

    const mainKindHeaders = [
      "managerProductMainKindID",
      "managerProductMainKindName",
      "managerProductMainKindCommissionRate",
      "managerProductMainKindIsEnable",
    ];
    const mainKindRows = mainKinds.map((item) => [
      item.managerProductMainKindID,
      item.managerProductMainKindName,
      item.managerProductMainKindCommissionRate,
      item.managerProductMainKindIsEnable,
    ]);
    downloadBlob(
      toCsv(mainKindHeaders, mainKindRows),
      `system-product-main-kinds-${timestamp}.csv`,
      "text/csv"
    );
    downloadBlob(
      toXls(mainKindHeaders, mainKindRows),
      `system-product-main-kinds-${timestamp}.xls`,
      "application/vnd.ms-excel"
    );

    const subKindHeaders = [
      "managerProductSubKindID",
      "managerProductSubKindName",
      "managerProductMainKindID",
      "managerProductMainKindName",
      "managerProductSubKindCommissionRate",
      "managerProductSubKindIsEnable",
    ];
    const subKindRows = subKinds.map((item) => [
      item.managerProductSubKindID,
      item.managerProductSubKindName,
      item.managerProductMainKindID,
      item.managerProductMainKindName,
      item.managerProductSubKindCommissionRate,
      item.managerProductSubKindIsEnable,
    ]);
    downloadBlob(
      toCsv(subKindHeaders, subKindRows),
      `system-product-sub-kinds-${timestamp}.csv`,
      "text/csv"
    );
    downloadBlob(
      toXls(subKindHeaders, subKindRows),
      `system-product-sub-kinds-${timestamp}.xls`,
      "application/vnd.ms-excel"
    );

    displaySuccess("已完成匯出並同步離線備份。");
  } finally {
    isExporting.value = false;
  }
};
//#endregion
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden p-2">
    <!-- 標題列 -->
    <div class="flex items-center justify-between mb-4">
      <div></div>
      <button class="btn-update" :disabled="isExporting" @click="exportProductCatalog">
        匯出/備份
      </button>
    </div>

    <!-- Tabs 選單 -->
    <div class="flex mb-0 gap-y-4">
      <button
        :class="['tab-btn', { active: activeTab === ProductTabEnum.Product }]"
        @click="changeTab(ProductTabEnum.Product)"
      >
        產品
      </button>
      <button
        :class="['tab-btn', { active: activeTab === ProductTabEnum.SubKind }]"
        @click="changeTab(ProductTabEnum.SubKind)"
      >
        子分類
      </button>
      <button
        :class="['tab-btn', { active: activeTab === ProductTabEnum.MainKind }]"
        @click="changeTab(ProductTabEnum.MainKind)"
      >
        主分類
      </button>
    </div>

    <!-- Tabs內容 -->
    <div class="tab-content flex-1 overflow-hidden">
      <!-- 產品 -->
      <div v-if="activeTab === ProductTabEnum.Product" class="tab h-full">
        <div
          class="flex flex-col h-full overflow-hidden bg-white rounded-lg rounded-tl-none p-6 gap-4"
        >
          <!-- 產品查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <!-- 產品名稱 -->
              <div>
<input
                  v-model="sysPrdListViewObj.query.managerProductName"
                  type="text"
                  class="input-box"
                  placeholder="產品名稱"
                />
              </div>
              <!-- 產品主分類 -->
              <div>
<GetManyManagerProductMainKindComboBox
                  v-model:managerProductMainKindID="
                    sysPrdListViewObj.query.managerProductMainKindID
                  "
                  v-model:managerProductMainKindName="
                    sysPrdListViewObj.query.managerProductMainKindName
                  "
                  :disabled="false"
                />
              </div>
              <!-- 產品子分類 -->
              <div>
<GetManyManagerProductSubKindComboBox
                  v-model:managerProductMainKindID="
                    sysPrdListViewObj.query.managerProductMainKindID
                  "
                  v-model:managerProductSubKindID="sysPrdListViewObj.query.managerProductSubKindID"
                  v-model:managerProductSubKindName="
                    sysPrdListViewObj.query.managerProductSubKindName
                  "
                  :disabled="!sysPrdListViewObj.query.managerProductMainKindID"
                />
              </div>
              <!-- 主力產品 -->
              <div>
<select v-model="sysPrdListViewObj.query.managerProductIsKey" class="select-box">
                  <option :value="null">全部</option>
                  <option :value="true">是</option>
                  <option :value="false">否</option>
                </select>
              </div>
              <!-- 規格名稱 -->
              <div>
<input
                  v-model="sysPrdListViewObj.query.managerProductSpecificationName"
                  type="text"
                  class="input-box"
                  placeholder="規格名稱"
                />
              </div>
              <!-- 規格狀態 -->
              <div>
<select
                  v-model="sysPrdListViewObj.query.managerProductSpecificationIsEnable"
                  class="select-box"
                >
                  <option :value="null">全部</option>
                  <option :value="true">啟用</option>
                  <option :value="false">停用</option>
                </select>
              </div>
            </div>

            <div class="flex">
              <button class="btn-search me-1" @click="clickSearchProductBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchProductBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--產品列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start w-24">產品名稱</th>
                  <th class="text-start w-24">產品主分類</th>
                  <th class="text-start w-24">產品子分類</th>
                  <th class="text-start w-24">主力產品</th>
                  <th class="text-start w-24">規格名稱</th>
                  <th class="text-start w-36">售價</th>
                  <th class="text-start w-36">成本</th>
                  <th class="text-start w-24">規格狀態</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysPrdListViewObj.systemProductList.length === 0">
                  <tr
                    v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                    class="text-center"
                  >
                    <td colspan="9">無資料</td>
                  </tr>
                  <tr v-else class="text-center">
                    <td colspan="9">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysPrdListViewObj.systemProductList"
                    :key="item.managerProductID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'view') &&
                        clickDetailProductBtn(item.managerProductID)
                    "
                  >
                    <td class="text-start">
                      {{ item.managerProductName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerProductMainKindName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerProductSubKindName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerProductIsKey ? "是" : "否" }}
                    </td>
                    <td class="text-start">
                      {{ item.managerProductSpecificationName }}
                    </td>
                    <td class="text-start">
                      {{ formatCurrency(item.managerProductSpecificationSellPrice) }}
                    </td>
                    <td class="text-start">
                      {{ formatCurrency(item.managerProductSpecificationCostPrice) }}
                    </td>

                    <td
                      class="text-start"
                      :class="{
                        'enabled-text': item.managerProductSpecificationIsEnable,
                        'disabled-text': !item.managerProductSpecificationIsEnable,
                      }"
                    >
                      {{ item.managerProductSpecificationIsEnable ? "啟用" : "停用" }}
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
                            @click.stop="clickDetailProductBtn(item.managerProductID)"
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
              activeTab === ProductTabEnum.Product &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickAddProductBtn"
          >
            +新增產品
          </button>
          <div v-if="activeTab === ProductTabEnum.Product" class="flex justify-center pt-3">
            <!--產品分頁 -->
            <Pagination
              :pageIndex="sysPrdListViewObj.query.pageIndex"
              :pageSize="sysPrdListViewObj.query.pageSize"
              :totalCount="sysPrdListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysPrdListViewObj.query.pageIndex = newPage;
                  getProductList();
                }
              "
              @update:page-size="
                (newSize: number) => {
                  sysPrdListViewObj.query.pageSize = newSize;
                  sysPrdListViewObj.query.pageIndex = 1;
                  getProductList();
                }
              "
            />
          </div>
        </div>
      </div>

      <!--產品主分類-->
      <div v-if="activeTab === ProductTabEnum.MainKind" class="tab h-full">
        <div
          class="flex flex-col h-full overflow-hidden bg-white rounded-lg rounded-tl-none p-6 gap-4"
        >
          <!-- 產品主分類查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <div>
<input
                  v-model="sysPrdMainKindListViewObj.query.managerProductMainKindName"
                  type="text"
                  class="input-box"
                  placeholder="產品主分類名稱"
                />
              </div>
              <div>
<select
                  v-model="sysPrdMainKindListViewObj.query.managerProductMainKindIsEnable"
                  class="select-box"
                >
                  <option :value="null">全部</option>
                  <option :value="true">啟用</option>
                  <option :value="false">停用</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchProductMainKindBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchProductMainKindBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--產品主分類列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start">產品主分類名稱</th>
                  <th class="text-start">預設業務毛利率</th>
                  <th class="text-start">狀態</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysPrdMainKindListViewObj.managerProductMainKindList.length === 0">
                  <tr class="text-center">
                    <td colspan="4">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysPrdMainKindListViewObj.managerProductMainKindList"
                    :key="item.managerProductMainKindID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                        clickOpenUpdateMainKindModalBtn(item)
                    "
                  >
                    <td class="text-start">
                      {{ item.managerProductMainKindName }}
                    </td>
                    <td class="text-start">{{ item.managerProductMainKindCommissionRate }}%</td>
                    <td
                      class="text-start"
                      :class="{
                        'enabled-text': item.managerProductMainKindIsEnable,
                        'disabled-text': !item.managerProductMainKindIsEnable,
                      }"
                    >
                      {{ item.managerProductMainKindIsEnable ? "啟用" : "停用" }}
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
                            @click.stop="clickOpenUpdateMainKindModalBtn(item)"
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
              activeTab === ProductTabEnum.MainKind &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickOpenAddMainKindModalBtn"
          >
            +新增主分類
          </button>
          <div v-if="activeTab === ProductTabEnum.MainKind" class="flex justify-center pt-3">
            <!-- 主分類分頁 -->
            <Pagination
              :pageIndex="sysPrdMainKindListViewObj.query.pageIndex"
              :pageSize="sysPrdMainKindListViewObj.query.pageSize"
              :totalCount="sysPrdMainKindListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysPrdMainKindListViewObj.query.pageIndex = newPage;
                  getProductMainKindList();
                }
              "
              @update:page-size="
                (newSize: number) => {
                  sysPrdMainKindListViewObj.query.pageSize = newSize;
                  sysPrdMainKindListViewObj.query.pageIndex = 1;
                  getProductMainKindList();
                }
              "
            />
          </div>
        </div>
      </div>

      <!--子分類-->
      <div v-if="activeTab === ProductTabEnum.SubKind" class="tab h-full">
        <div
          class="flex flex-col h-full overflow-hidden bg-white rounded-lg rounded-tl-none p-6 gap-4"
        >
          <!-- 產品子分類查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <div>
<input
                  v-model="sysPrdSubKindListViewObj.query.managerProductSubKindName"
                  type="text"
                  class="input-box"
                  placeholder="產品子分類名稱"
                />
              </div>

              <div>
<GetManyManagerProductMainKindComboBox
                  v-model:managerProductMainKindID="
                    sysPrdSubKindListViewObj.query.managerProductMainKindID
                  "
                  v-model:managerProductMainKindName="
                    sysPrdSubKindListViewObj.query.managerProductMainKindName
                  "
                  :disabled="false"
                />
              </div>

              <div>
<select
                  v-model="sysPrdSubKindListViewObj.query.managerProductSubKindIsEnable"
                  class="select-box"
                >
                  <option :value="null">全部</option>
                  <option :value="true">啟用</option>
                  <option :value="false">停用</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchProductSubKindBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchProductSubKindBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--子分類列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start">產品子分類名稱</th>
                  <th class="text-start">產品主分類</th>
                  <th class="text-start">業務毛利率</th>
                  <th class="text-start">狀態</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysPrdSubKindListViewObj.managerProductSubKindList.length === 0">
                  <tr class="text-center">
                    <td colspan="5">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysPrdSubKindListViewObj.managerProductSubKindList"
                    :key="item.managerProductSubKindID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                        clickOpenUpdateSubKindModalBtn(item)
                    "
                  >
                    <td class="text-start">
                      {{ item.managerProductSubKindName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerProductMainKindName }}
                    </td>
                    <td class="text-start">{{ item.managerProductSubKindCommissionRate }}%</td>
                    <td
                      class="text-start"
                      :class="{
                        'enabled-text': item.managerProductSubKindIsEnable,
                        'disabled-text': !item.managerProductSubKindIsEnable,
                      }"
                    >
                      {{ item.managerProductSubKindIsEnable ? "啟用" : "停用" }}
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
                            @click.stop="clickOpenUpdateSubKindModalBtn(item)"
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
              activeTab === ProductTabEnum.SubKind &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickOpenAddSubKindModalBtn"
          >
            +新增子分類
          </button>
          <div v-if="activeTab === ProductTabEnum.SubKind" class="flex justify-center pt-3">
            <!-- 子分類分頁 -->
            <Pagination
              :pageIndex="sysPrdSubKindListViewObj.query.pageIndex"
              :pageSize="sysPrdSubKindListViewObj.query.pageSize"
              :totalCount="sysPrdSubKindListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysPrdSubKindListViewObj.query.pageIndex = newPage;
                  getProductSubKindList();
                }
              "
              @update:page-size="
                (newSize: number) => {
                  sysPrdSubKindListViewObj.query.pageSize = newSize;
                  sysPrdSubKindListViewObj.query.pageIndex = 1;
                  getProductSubKindList();
                }
              "
            />
          </div>
        </div>
      </div>
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 系統-產品-主分類-新增-Modal -->
    <AddSystemProductMainKindModal
      v-if="showProductMainAddModal"
      @close="showProductMainAddModal = false"
      @confirm="handleAddProductMainKind"
    />

    <!-- 系統-產品-主分類-編輯-Modal -->
    <UpdateSystemProductMainKindModal
      v-if="showProductMainKindUpdateModal"
      :data="updatingProductMainKind!"
      @close="showProductMainKindUpdateModal = false"
      @confirm="handleUpdateProductMainKind"
    />

    <!-- 新增-產品-子分類 Modal -->
    <AddSystemProductSubKindModal
      v-if="showProductSubKindAddModal"
      @close="showProductSubKindAddModal = false"
      @confirm="handleAddProductSubKind"
    />

    <!-- 編輯-產品-子分類 Modal -->
    <UpdateSystemProductSubKindModal
      v-if="showProductSubKindUpdateModal"
      :data="updatingProductSubKind!"
      @close="showProductSubKindUpdateModal = false"
      @confirm="handleUpdateProductSubKind"
    />
  </div>
</template>
