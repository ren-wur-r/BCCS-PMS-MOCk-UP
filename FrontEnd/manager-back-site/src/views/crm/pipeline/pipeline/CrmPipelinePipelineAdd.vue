<script setup lang="ts">
//#region 引入
// Vue
import { ref, reactive, computed } from "vue";
// Enums / 常數
// import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";
import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";
// Stores
import { useTokenStore } from "@/stores/token";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
// Services
import {
  MbsCrmPplHttpAddEmployeePipelineReqMdl,
  MbsCrmPplHttpAddEmployeePipelineReqContacterItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqSalerTrackingItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqProductItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqBillItemMdl,
  MbsCrmPplHttpAddEmployeePipelineReqDueItemMdl,
  MbsCrmPplHttpAddEmployeePipelineRspMdl,
} from "@/services/pms-http/crm/pipeline/crmPipelineHttpFormat";
import { addEmployeePipeline } from "@/services/pms-http/crm/pipeline/crmPipelineHttpService";
// Utils
import { formatToServerDateEndISO8, formatToServerDatetime } from "@/utils/timeFormatter";
// Components
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ConfirmDialog from "@/components/global/feedback/ConfirmDialog.vue";
import PipelineCompanySection from "./components/sections/PipelineCompanySection.vue";
import PipelineContacterSection from "./components/sections/PipelineContacterSection.vue";
import PipelineTrackingSection from "./components/sections/PipelineTrackingSection.vue";
import PipelineProductSection from "./components/sections/PipelineProductSection.vue";
import PipelineBillSection from "./components/sections/PipelineBillSection.vue";
import PipelineDueSection from "./components/sections/PipelineDueSection.vue";
import PipelineCompanyModal from "./components/modals/PipelineCompanyModal.vue";
import PipelineContacterModal from "./components/modals/PipelineContacterModal.vue";
import TrackingModal from "./components/modals/TrackingModal.vue";
import PipelineProductModal from "./components/modals/PipelineProductModal.vue";
import PipelineDueModal from "./components/modals/PipelineDueModal.vue";
import EditMultipleBillsModal from "./components/modals/EditMultipleBillsModal.vue";
import GetManyEmployeeComboBox from "@/components/feature/search-bar/GetManyEmployeeComboBox.vue";
// Router
import router from "@/router";
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
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
//#endregion

//#region 型別定義
/** CRM-商機管理-新增商機-客戶資訊項目-模型 */
interface CrmPipelinePipelineAddCompanyItemMdl {
  /** 統一編號 */
  managerCompanyUnifiedNumber: string;
  /** 公司ID */
  managerCompanyID: number | null;
  /** 公司名稱 */
  managerCompanyName: string;
  /** 人員規模 */
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  /** 客戶分級 */
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  /** 公司主類別名稱 */
  managerCompanyMainKindName: string;
  /** 公司子類別名稱 */
  managerCompanySubKindName: string;
  /** 公司所在城市 */
  atomCity: DbAtomCityEnum | null;
  /** 公司地址ID */
  managerCompanyLocationID: number | null;
  /** 公司地址名稱 */
  managerCompanyLocationName: string;
  /** 公司縣市 */
  managerCompanyLocationAddress: string;
  /** 公司電話 */
  managerCompanyLocationTelephone: string;
  /** 公司狀態 */
  managerCompanyLocationStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** CRM-商機管理-新增商機-窗口資訊項目-模型 */
interface CrmPipelinePipelineAddContacterItemMdl {
  /** 窗口ID */
  managerContacterID: number;
  /** 是否為主要窗口 */
  employeePipelineContacterIsPrimary: boolean;
  /** 管理者窗口-姓名 */
  managerContacterName: string;
  /** 管理者窗口-Email */
  managerContacterEmail: string;
  /** 管理者窗口-電話 */
  managerContacterPhone: string;
  /** 管理者窗口-部門 */
  managerContacterDepartment: string;
  /** 管理者窗口-職稱 */
  managerContacterJobTitle: string;
  /** 管理者窗口-電話(市話) */
  managerContacterTelephone: string;
  /** 管理者窗口-是否個資同意 */
  managerContacterIsConsent: boolean;
  /** 管理者窗口-狀態 */
  managerContacterStatus: number;
  /** 管理者窗口-開發評等 */
  atomRatingKind: number;
  /** 管理者窗口-備註 */
  managerContacterRemark: string;
}

/** CRM-商機管理-新增商機-業務商機開發紀錄項目-模型 */
interface CrmPipelinePipelineAddSalerTrackingItemMdl {
  /** 商機業務開發紀錄ID */
  employeePipelineSalerTrackingID: number | null;
  /** 開發時間 */
  employeePipelineSalerTrackingTime: string;
  /** 窗口ID */
  managerContacterID: number;
  /** 窗口名稱 */
  managerContacterName: string;
  /** 備註 */
  employeePipelineSalerTrackingRemark: string;
  /** 商機業務開發紀錄-建立人員名稱(業務員工名稱) */
  employeePipelineSalerTrackingCreateEmployeeName?: string;
}

/** CRM-商機管理-新增商機-商機產品項目-模型 */
interface CrmPipelinePipelineAddProductItemMdl {
  /** 商機產品ID（新增時不需要） */
  employeePipelineProductID: number | null;
  /** 產品ID */
  managerProductID: number;
  /** 產品名稱 */
  managerProductName: string;
  /** 業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 產品主類別名稱 */
  managerProductMainKindName: string;
  /** 產品子類別名稱 */
  managerProductSubKindName: string;
  /** 產品規格ID */
  managerProductSpecificationID: number;
  /** 產品規格名稱 */
  managerProductSpecificationName: string;
  /** 商機產品建議售價 */
  employeePipelineProductSellPrice: number;
  /** 商機產品成交價 */
  employeePipelineProductClosingPrice: number;
  /** 商機產品成本價 */
  employeePipelineProductCostPrice: number;
  /** 商機產品毛利 */
  employeePipelineProductGrossProfit: number;
  /** 商機產品數量 */
  employeePipelineProductCount: number;
  /** 採購類型 */
  employeePipelineProductPurchaseKind: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 合約類型 */
  employeePipelineProductContractKind: DbAtomEmployeePipelineProductContractKindEnum;
  /** 合約說明 */
  employeePipelineProductContractText?: string;
}

/** CRM-商機管理-新增商機-發票記錄項目-模型 */
interface CrmPipelinePipelineAddBillItemMdl {
  /** 商機發票紀錄ID */
  employeePipelineBillID: number | null;
  /** 發票號碼 */
  employeePipelineBillBillNumber: string | null;
  /** 發票期數 */
  employeePipelineBillPeriodNumber: number;
  /** 發票日期 */
  employeePipelineBillBillTime: string;
  /** 未稅發票金額 */
  employeePipelineBillNoTaxAmount: number;
  /** 備註 */
  employeePipelineBillRemark: string | null;
  /** 發票狀態 */
  employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum;
}

/** CRM-商機管理-新增商機-履約期限項目-模型 */
interface CrmPipelinePipelineAddDueItemMdl {
  /** 商機履約期限ID */
  employeePipelineDueID: number | null;
  /** 履約期限時間 */
  employeePipelineDueTime: string;
  /** 備註 */
  employeePipelineDueRemark: string;
}

// ---------------------------------------------------------------------------
/** 確認彈跳視窗狀態模型 */
interface ConfirmDialogState {
  show: boolean;
  title: string;
  message: string;
  onConfirm: (() => void | Promise<void>) | null;
}

//#endregion

//#region 頁面物件
/** 商機狀態 */
const atomPipelineStatus = ref<DbAtomPipelineStatusEnum>(
  DbAtomPipelineStatusEnum.Business10Percent
);
/** 承辦業務員工ID */
const employeePipelineSalerEmployeeID = ref<number | null>(null);
/** CRM-商機管理-新增商機-客戶資訊-物件 */
const crmPipelinePipelineAddCompanyInfoObj = ref<CrmPipelinePipelineAddCompanyItemMdl | null>(null);
/** CRM-商機管理-新增商機-窗口列表-物件 */
const crmPipelinePipelineAddContacterListObj = ref<CrmPipelinePipelineAddContacterItemMdl[]>([]);
/** CRM-商機管理-新增商機-商機開發記錄列表-物件 */
const crmPipelinePipelineAddSalerTrackingListObj = ref<
  CrmPipelinePipelineAddSalerTrackingItemMdl[]
>([]);
/** CRM-商機管理-新增商機-產品列表-物件 */
const crmPipelinePipelineAddProductListObj = ref<CrmPipelinePipelineAddProductItemMdl[]>([]);
/** CRM-商機管理-新增商機-發票記錄列表-物件 */
const crmPipelinePipelineAddBillListObj = ref<CrmPipelinePipelineAddBillItemMdl[]>([]);
/** CRM-商機管理-新增商機-履約期限列表-物件 */
const crmPipelinePipelineAddDueListObj = ref<CrmPipelinePipelineAddDueItemMdl[]>([]);
/** 是否正在提交 */
const isSubmitting = ref(false);
/** 是否顯示【附加/編輯客戶】彈跳視窗 */
const showCompanyModal = ref(false);
/** 是否顯示【附加窗口】彈跳視窗 */
const showContacterModal = ref(false);
/** 是否顯示【編輯商機開發記錄】彈跳視窗 */
const showTrackingModal = ref(false);
/** 正在編輯的開發記錄索引 */
const editingTrackingIndex = ref<number | null>(null);
/** 正在編輯的開發記錄資料 */
const editingTracking = ref<CrmPipelinePipelineAddSalerTrackingItemMdl | null>(null);
/** 是否顯示【產品】彈跳視窗 */
const showProductModal = ref(false);
/** 正在編輯的產品索引 */
const editingProductIndex = ref<number | null>(null);
/** 正在編輯的產品資料 */
const editingProduct = ref<CrmPipelinePipelineAddProductItemMdl | null>(null);
/** 是否顯示【編輯多筆發票記錄】彈跳視窗 */
const showEditMultipleBillsModal = ref(false);
/** 是否顯示【履約期限通知】彈跳視窗 */
const showDueModal = ref(false);
/** 正在編輯的履約期限索引 */
const editingDueIndex = ref<number | null>(null);
/** 正在編輯的履約期限資料 */
const editingDue = ref<CrmPipelinePipelineAddDueItemMdl | null>(null);

/** 確認彈跳視窗狀態物件 */
const confirmDialog = reactive<ConfirmDialogState>({
  show: false,
  title: "",
  message: "",
  onConfirm: null,
});

//#endregion

//#region 確認彈跳視窗
/** 顯示確認彈跳視窗 */
const showConfirmDialog = (options: {
  title: string;
  message: string;
  onConfirm: () => void | Promise<void>;
}) => {
  confirmDialog.title = options.title;
  confirmDialog.message = options.message;
  confirmDialog.onConfirm = options.onConfirm;
  confirmDialog.show = true;
};

/** 處理確認彈跳視窗確認 */
const handleConfirmDialog = async () => {
  confirmDialog.show = false;
  if (confirmDialog.onConfirm) {
    await confirmDialog.onConfirm();
  }
  confirmDialog.onConfirm = null;
};

/** 處理確認彈跳視窗取消 */
const handleCancelDialog = () => {
  confirmDialog.show = false;
  confirmDialog.onConfirm = null;
};

//#endregion

//#region 計算屬性
/** 計算產品總成交價 */
const totalProductClosingPrice = computed(() => {
  return crmPipelinePipelineAddProductListObj.value.reduce(
    (sum, product) =>
      sum + product.employeePipelineProductClosingPrice * product.employeePipelineProductCount,
    0
  );
});

/** 計算發票分期期數 */
const billPeriodCount = computed(() => {
  if (crmPipelinePipelineAddBillListObj.value.length === 0) return 0;
  return Math.max(
    ...crmPipelinePipelineAddBillListObj.value.map((bill) => bill.employeePipelineBillPeriodNumber)
  );
});

/** 判斷是否顯示發票記錄區塊 */
const shouldShowBillSection = computed(() => {
  return (
    atomPipelineStatus.value === DbAtomPipelineStatusEnum.Business10Percent ||
    atomPipelineStatus.value === DbAtomPipelineStatusEnum.Business30Percent ||
    atomPipelineStatus.value === DbAtomPipelineStatusEnum.Business70Percent ||
    atomPipelineStatus.value === DbAtomPipelineStatusEnum.Business100Percent
  );
});

/** 判斷是否顯示履約期限通知區塊 */
const showDueSection = computed(() => {
  const status = atomPipelineStatus.value;
  return (
    status === DbAtomPipelineStatusEnum.Business10Percent ||
    status === DbAtomPipelineStatusEnum.Business30Percent ||
    status === DbAtomPipelineStatusEnum.Business70Percent ||
    status === DbAtomPipelineStatusEnum.Business100Percent ||
    status === DbAtomPipelineStatusEnum.TransferredToProject
  );
});

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/crm/pipeline/pipeline");
};

//#endregion

//#region 【客戶】區塊事件處理
/** 處理附加客戶 */
const handleAddCompany = () => {
  showCompanyModal.value = true;
};

/** 處理編輯客戶 */
const handleEditCompany = () => {
  showCompanyModal.value = true;
};

/** 處理附加客戶確認 */
const handleCompanyModalConfirm = (data: CrmPipelinePipelineAddCompanyItemMdl) => {
  crmPipelinePipelineAddCompanyInfoObj.value = data;
  showCompanyModal.value = false;
};

/** 處理附加客戶取消 */
const handleCompanyModalCancel = () => {
  showCompanyModal.value = false;
};
//#endregion

//#region 【窗口】區塊事件處理
/** 處理附加窗口 */
const handleAddContacter = () => {
  if (!crmPipelinePipelineAddCompanyInfoObj.value) {
    displayError("請先新增客戶才能附加窗口");
    return;
  }
  showContacterModal.value = true;
};

/** 處理刪除窗口 */
const handleDeleteContacter = (contacterId: number) => {
  showConfirmDialog({
    title: "確認刪除",
    message: "確定要刪除此窗口嗎？",
    onConfirm: () => {
      const index = crmPipelinePipelineAddContacterListObj.value.findIndex(
        (c) => c.managerContacterID === contacterId
      );
      if (index !== -1) {
        crmPipelinePipelineAddContacterListObj.value.splice(index, 1);
      }
    },
  });
};

/** 處理附加窗口確認 */
const handleContacterModalConfirm = (data: CrmPipelinePipelineAddContacterItemMdl) => {
  // 檢查是否已經存在相同的窗口
  const exists = crmPipelinePipelineAddContacterListObj.value.some(
    (c) => c.managerContacterID === data.managerContacterID
  );

  if (exists) {
    displayError("此窗口已存在，無法重複新增");
    return;
  }

  // 如果新增的是主要窗口，將其他窗口設為次要
  if (data.employeePipelineContacterIsPrimary) {
    crmPipelinePipelineAddContacterListObj.value.forEach((c) => {
      c.employeePipelineContacterIsPrimary = false;
    });
  }

  crmPipelinePipelineAddContacterListObj.value.push(data);
  showContacterModal.value = false;
};

/** 處理附加窗口取消 */
const handleContacterModalCancel = () => {
  showContacterModal.value = false;
};

//#endregion

//#region 【商機開發記錄】區塊事件處理
/** 處理新增開發記錄 */
const handleAddTracking = () => {
  // 檢查是否已選擇窗口
  if (crmPipelinePipelineAddContacterListObj.value.length === 0) {
    displayError("請先新增窗口才能新增開發記錄");
    return;
  }
  showTrackingModal.value = true;
};

/** 處理編輯開發記錄 */
const handleEditTracking = (tracking: CrmPipelinePipelineAddSalerTrackingItemMdl) => {
  // 找到開發記錄在列表中的索引
  editingTrackingIndex.value = crmPipelinePipelineAddSalerTrackingListObj.value.findIndex(
    (t) =>
      t.employeePipelineSalerTrackingTime === tracking.employeePipelineSalerTrackingTime &&
      t.managerContacterID === tracking.managerContacterID
  );

  editingTracking.value = { ...tracking }; // 複製一份避免直接修改
  showTrackingModal.value = true;
};

/** 處理刪除開發記錄 */
const handleDeleteTracking = (index: number) => {
  showConfirmDialog({
    title: "確認刪除",
    message: "確定要刪除此開發記錄嗎？",
    onConfirm: () => {
      crmPipelinePipelineAddSalerTrackingListObj.value.splice(index, 1);
    },
  });
};

/** 處理新增開發記錄確認 */
const handleTrackingModalConfirm = (data: {
  employeePipelineSalerTrackingTime: string;
  managerContacterID: number;
  employeePipelineSalerTrackingRemark: string;
}) => {
  // 找到對應的窗口名稱
  const contacter = crmPipelinePipelineAddContacterListObj.value.find(
    (c) => c.managerContacterID === data.managerContacterID
  );

  if (!contacter) {
    displayError("找不到對應的窗口資料");
    return;
  }

  const newTracking: CrmPipelinePipelineAddSalerTrackingItemMdl = {
    employeePipelineSalerTrackingID: null,
    employeePipelineSalerTrackingTime: data.employeePipelineSalerTrackingTime,
    managerContacterID: data.managerContacterID,
    managerContacterName: contacter.managerContacterName,
    employeePipelineSalerTrackingRemark: data.employeePipelineSalerTrackingRemark,
  };

  // 如果是編輯模式
  if (editingTrackingIndex.value !== null) {
    crmPipelinePipelineAddSalerTrackingListObj.value[editingTrackingIndex.value] = newTracking;
    editingTrackingIndex.value = null;
    editingTracking.value = null;
    showTrackingModal.value = false;
  } else {
    // 新增模式
    crmPipelinePipelineAddSalerTrackingListObj.value.push(newTracking);
    showTrackingModal.value = false;
  }
};

/** 處理新增開發記錄取消 */
const handleTrackingModalCancel = () => {
  showTrackingModal.value = false;
};

//#endregion

//#region 【產品】區塊事件處理
/** 處理新增產品 */
const handleAddProduct = () => {
  editingProductIndex.value = null;
  editingProduct.value = null;
  showProductModal.value = true;
};

/** 處理編輯產品 */
const handleEditProduct = (product: CrmPipelinePipelineAddProductItemMdl, index: number) => {
  editingProductIndex.value = index;
  editingProduct.value = { ...product };
  showProductModal.value = true;
};

/** 處理產品彈跳視窗確認 */
const handleProductModalConfirm = (data: {
  employeePipelineProductID: number | null;
  managerProductID: number;
  managerProductSpecificationID: number;
  employeePipelineProductSellPrice: number;
  employeePipelineProductClosingPrice: number;
  employeePipelineProductCostPrice: number;
  employeePipelineProductCount: number;
  employeePipelineProductPurchaseKindID: DbAtomEmployeePipelineProductPurchaseKindEnum;
  employeePipelineProductContractKindID: DbAtomEmployeePipelineProductContractKindEnum;
  employeePipelineProductContractText: string;
  managerProductName?: string;
  managerProductMainKindName?: string;
  managerProductSubKindName?: string;
  managerProductSpecificationName?: string;
  managerProductMainKindCommissionRate?: number;
}) => {
  // 計算毛利
  const grossProfit =
    data.employeePipelineProductClosingPrice - data.employeePipelineProductCostPrice;

  const productData: CrmPipelinePipelineAddProductItemMdl = {
    employeePipelineProductID: null,
    managerProductID: data.managerProductID,
    managerProductName: data.managerProductName || "",
    managerProductMainKindName: data.managerProductMainKindName || "",
    managerProductSubKindName: data.managerProductSubKindName || "",
    managerProductSpecificationID: data.managerProductSpecificationID,
    managerProductSpecificationName: data.managerProductSpecificationName || "",
    employeePipelineProductSellPrice: data.employeePipelineProductSellPrice,
    employeePipelineProductClosingPrice: data.employeePipelineProductClosingPrice,
    employeePipelineProductCostPrice: data.employeePipelineProductCostPrice,
    employeePipelineProductGrossProfit: grossProfit,
    employeePipelineProductCount: data.employeePipelineProductCount,
    employeePipelineProductPurchaseKind: data.employeePipelineProductPurchaseKindID,
    employeePipelineProductContractKind: data.employeePipelineProductContractKindID,
    employeePipelineProductContractText: data.employeePipelineProductContractText,
    managerProductMainKindCommissionRate: data.managerProductMainKindCommissionRate || 0,
  };

  if (editingProductIndex.value !== null) {
    // 編輯模式：更新現有產品
    crmPipelinePipelineAddProductListObj.value[editingProductIndex.value] = productData;
  } else {
    // 新增模式：加入新產品
    crmPipelinePipelineAddProductListObj.value.push(productData);
  }

  showProductModal.value = false;
  editingProductIndex.value = null;
  editingProduct.value = null;
};

/** 處理產品彈跳視窗取消 */
const handleProductModalCancel = () => {
  showProductModal.value = false;
  editingProductIndex.value = null;
  editingProduct.value = null;
};

/** 處理刪除產品 */
const handleDeleteProduct = (productId: number | null, index: number) => {
  showConfirmDialog({
    title: "確認刪除",
    message: "確定要刪除此產品嗎？",
    onConfirm: () => {
      crmPipelinePipelineAddProductListObj.value.splice(index, 1);
    },
  });
};
//#endregion

//#region 【發票記錄】區塊事件處理
/** 處理編輯多筆發票記錄 */
const handleEditMultipleBills = () => {
  showEditMultipleBillsModal.value = true;
};

/** 處理編輯多筆發票記錄確認 */
const handleEditMultipleBillsConfirm = (data: {
  periodCount: number;
  billList: {
    employeePipelineBillID: number | null;
    employeePipelineBillBillNumber: string | null;
    employeePipelineBillPeriodNumber: number;
    employeePipelineBillBillTime: string;
    employeePipelineBillNoTaxAmount: number;
    employeePipelineBillRemark: string | null;
  }[];
}) => {
  crmPipelinePipelineAddBillListObj.value = data.billList.map((bill) => ({
    ...bill,
    employeePipelineBillStatus: DbAtomEmployeePipelineBillStatusEnum.NotCompleted,
  }));
  showEditMultipleBillsModal.value = false;
};

/** 處理編輯多筆發票記錄取消 */
const handleEditMultipleBillsCancel = () => {
  showEditMultipleBillsModal.value = false;
};
//#endregion

//#region 【履約期限通知】區塊事件處理
/** 處理新增履約期限通知 */
const handleAddDue = () => {
  editingDueIndex.value = null;
  editingDue.value = null;
  showDueModal.value = true;
};

/** 處理編輯履約期限通知 */
const handleEditDue = (due: CrmPipelinePipelineAddDueItemMdl, index: number) => {
  editingDueIndex.value = index;
  editingDue.value = { ...due };
  showDueModal.value = true;
};

/** 處理刪除履約期限通知 */
const handleDeleteDue = (index: number) => {
  showConfirmDialog({
    title: "確認刪除",
    message: "確定要刪除此履約期限通知嗎？",
    onConfirm: () => {
      crmPipelinePipelineAddDueListObj.value.splice(index, 1);
    },
  });
};

/** 處理履約期限通知確認 */
const handleDueModalConfirm = (data: {
  employeePipelineDueTime: string;
  employeePipelineDueRemark: string;
}) => {
  const dueData: CrmPipelinePipelineAddDueItemMdl = {
    employeePipelineDueID: null,
    employeePipelineDueTime: data.employeePipelineDueTime,
    employeePipelineDueRemark: data.employeePipelineDueRemark,
  };

  if (editingDueIndex.value !== null) {
    // 編輯模式：更新現有履約期限
    crmPipelinePipelineAddDueListObj.value[editingDueIndex.value] = dueData;
  } else {
    // 新增模式：加入新履約期限
    crmPipelinePipelineAddDueListObj.value.push(dueData);
  }

  showDueModal.value = false;
  editingDueIndex.value = null;
  editingDue.value = null;
};

/** 處理履約期限通知取消 */
const handleDueModalCancel = () => {
  showDueModal.value = false;
  editingDueIndex.value = null;
  editingDue.value = null;
};
//#endregion

//#region 提交處理
/** 處理提交 */
const handleSubmit = () => {
  showConfirmDialog({
    title: "確認新增",
    message: "確定要新增此商機嗎？",
    onConfirm: submitPipeline,
  });
};

/** 提交商機 */
const submitPipeline = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 驗證客戶
  if (!crmPipelinePipelineAddCompanyInfoObj.value) {
    displayError("請選擇客戶");
    return;
  }
  // 驗證窗口
  if (crmPipelinePipelineAddContacterListObj.value.length === 0) {
    displayError("請新增至少一個窗口");
    return;
  }

  // 開始提交
  isSubmitting.value = true;

  // 呼叫 API
  const requestData: MbsCrmPplHttpAddEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: crmPipelinePipelineAddCompanyInfoObj.value.managerCompanyID!,
    managerCompanyLocationID: crmPipelinePipelineAddCompanyInfoObj.value.managerCompanyLocationID!,
    atomPipelineStatus: atomPipelineStatus.value,
    employeePipelineSalerEmployeeID: employeePipelineSalerEmployeeID.value!,
    contacterList: crmPipelinePipelineAddContacterListObj.value.map(
      (item) =>
        ({
          managerContacterID: item.managerContacterID,
          employeePipelineContacterIsPrimary: item.employeePipelineContacterIsPrimary,
        }) satisfies MbsCrmPplHttpAddEmployeePipelineReqContacterItemMdl
    ),
    salerTrackingList: crmPipelinePipelineAddSalerTrackingListObj.value.map(
      (item) =>
        ({
          employeePipelineSalerTrackingTime: formatToServerDatetime(
            item.employeePipelineSalerTrackingTime
          ),
          managerContacterID: item.managerContacterID,
          employeePipelineSalerTrackingRemark: item.employeePipelineSalerTrackingRemark,
        }) satisfies MbsCrmPplHttpAddEmployeePipelineReqSalerTrackingItemMdl
    ),
    productList: crmPipelinePipelineAddProductListObj.value.map(
      (item) =>
        ({
          managerProductID: item.managerProductID,
          managerProductSpecificationID: item.managerProductSpecificationID,
          employeePipelineProductSellPrice: item.employeePipelineProductSellPrice,
          employeePipelineProductClosingPrice: item.employeePipelineProductClosingPrice,
          employeePipelineProductCostPrice: item.employeePipelineProductCostPrice,
          employeePipelineProductCount: item.employeePipelineProductCount,
          employeePipelineProductPurchaseKindID: item.employeePipelineProductPurchaseKind,
          employeePipelineProductContractKindID: item.employeePipelineProductContractKind,
          employeePipelineProductContractText: item.employeePipelineProductContractText,
        }) satisfies MbsCrmPplHttpAddEmployeePipelineReqProductItemMdl
    ),
    billList: crmPipelinePipelineAddBillListObj.value.map(
      (item) =>
        ({
          employeePipelineBillPeriodNumber: item.employeePipelineBillPeriodNumber,
          employeePipelineBillBillTime: formatToServerDatetime(item.employeePipelineBillBillTime),
          employeePipelineBillNoTaxAmount: item.employeePipelineBillNoTaxAmount,
          employeePipelineBillRemark: item.employeePipelineBillRemark,
          employeePipelineBillStatus: item.employeePipelineBillStatus,
        }) satisfies MbsCrmPplHttpAddEmployeePipelineReqBillItemMdl
    ),
    dueList: crmPipelinePipelineAddDueListObj.value.map(
      (item) =>
        ({
          employeePipelineDueTime: formatToServerDateEndISO8(item.employeePipelineDueTime),
          employeePipelineDueRemark: item.employeePipelineDueRemark,
        }) satisfies MbsCrmPplHttpAddEmployeePipelineReqDueItemMdl
    ),
  };
  const responseData: MbsCrmPplHttpAddEmployeePipelineRspMdl | null =
    await addEmployeePipeline(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("商機新增成功！");

  // 導向詳細頁
  setTimeout(() => {
    router.push(`/crm/pipeline/pipeline/detail/${responseData.employeePipelineID}`);
  }, 1500);

  // 提交結束
  isSubmitting.value = false;
};
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <!-- 頁面標題與操作按鈕 -->
    <div class="flex items-center justify-between">
      <div class="flex items-center justify-between">
        <button class="btn-back flex items-center" @click="clickBackBtn">
          <svg
            class="w-4 h-4 inline-block mr-1"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M15 19l-7-7 7-7"
            />
          </svg>
          <span>返回</span>
        </button>
      </div>
    </div>

    <div class="flex flex-col gap-4">
      <!-- 【基本資訊】區塊 -->
      <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
        <div class="section-header">
          <h2 class="subtitle">基本資訊</h2>
        </div>
        <div class="section-content">
          <div class="grid grid-cols-2 gap-4">
            <!-- 商機狀態 -->
            <div class="flex flex-col gap-2">
              <label class="form-label"> 商機狀態 <span class="required-label">*</span> </label>
              <select v-model.number="atomPipelineStatus" class="select-box">
                <option :value="DbAtomPipelineStatusEnum.Business10Percent">商機10%</option>
                <option :value="DbAtomPipelineStatusEnum.Business30Percent">商機30%</option>
                <option :value="DbAtomPipelineStatusEnum.Business70Percent">商機70%</option>
                <option :value="DbAtomPipelineStatusEnum.Business100Percent">商機100%</option>
              </select>
            </div>

            <!-- 承辦業務員工 -->
            <div class="flex flex-col gap-2">
              <label class="form-label">承辦業務 <span class="required-label">*</span></label>
              <GetManyEmployeeComboBox
                v-model:manager-employee-i-d="employeePipelineSalerEmployeeID"
                :disabled="false"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- 【客戶】區塊 -->
      <PipelineCompanySection
        :company="crmPipelinePipelineAddCompanyInfoObj"
        :readonly="false"
        @add-company="handleAddCompany"
        @edit-company="handleEditCompany"
      />

      <!-- 【窗口】區塊 -->
      <PipelineContacterSection
        :contacter-list="crmPipelinePipelineAddContacterListObj"
        :readonly="false"
        @add-contacter="handleAddContacter"
        @delete-contacter="handleDeleteContacter"
      />

      <!-- 【產品】區塊 -->
      <PipelineProductSection
        :product-list="crmPipelinePipelineAddProductListObj"
        :readonly="false"
        @add-product="handleAddProduct"
        @edit-product="handleEditProduct"
        @delete-product="handleDeleteProduct"
      />

      <!-- 【商機開發記錄】區塊 -->
      <PipelineTrackingSection
        :tracking-list="crmPipelinePipelineAddSalerTrackingListObj"
        :can-add="true"
        :can-modify="true"
        :show-saler-column="false"
        @add-tracking="handleAddTracking"
        @edit-tracking="handleEditTracking"
        @delete-tracking="handleDeleteTracking"
      />

      <!-- 【發票記錄】區塊 -->
      <PipelineBillSection
        v-if="shouldShowBillSection"
        :bill-list="crmPipelinePipelineAddBillListObj"
        :total-amount="totalProductClosingPrice"
        :period-count="billPeriodCount"
        :can-edit-multiple-bills="true"
        :show-confirm-button="false"
        :show-notify-button="false"
        :readonly="false"
        @edit-multiple-bills="handleEditMultipleBills"
      />

      <!-- 【履約期限通知】區塊 -->
      <PipelineDueSection
        v-if="showDueSection"
        :due-list="crmPipelinePipelineAddDueListObj"
        :readonly="false"
        @add-due="handleAddDue"
        @edit-due="handleEditDue"
        @delete-due="handleDeleteDue"
      />
    </div>
    <div class="flex justify-center">
      <button class="btn-submit" :disabled="isSubmitting" @click="handleSubmit">
        {{ isSubmitting ? "提交中..." : "提交" }}
      </button>
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 確認彈跳視窗 -->
    <ConfirmDialog
      :show="confirmDialog.show"
      :title="confirmDialog.title"
      :message="confirmDialog.message"
      @confirm="handleConfirmDialog"
      @cancel="handleCancelDialog"
    />

    <!-- 商機開發記錄-【附加/編輯商機開發記錄】彈跳視窗 -->
    <TrackingModal
      :show="showTrackingModal"
      :contacter-list="
        crmPipelinePipelineAddContacterListObj.map((c) => ({
          managerContacterID: c.managerContacterID,
          managerContacterName: c.managerContacterName,
        }))
      "
      :tracking="editingTracking"
      @confirm="handleTrackingModalConfirm"
      @cancel="handleTrackingModalCancel"
    />

    <!-- 客戶-【附加/編輯客戶】彈跳視窗 -->
    <PipelineCompanyModal
      :show="showCompanyModal"
      :current-company="crmPipelinePipelineAddCompanyInfoObj"
      @confirm="handleCompanyModalConfirm"
      @cancel="handleCompanyModalCancel"
    />

    <!-- 窗口-【附加窗口】彈跳視窗 -->
    <PipelineContacterModal
      :show="showContacterModal"
      :manager-company-i-d="crmPipelinePipelineAddCompanyInfoObj?.managerCompanyID ?? null"
      @confirm="handleContacterModalConfirm"
      @cancel="handleContacterModalCancel"
    />

    <!-- 產品-【產品】彈跳視窗 -->
    <PipelineProductModal
      :show="showProductModal"
      :product="editingProduct"
      @confirm="handleProductModalConfirm"
      @cancel="handleProductModalCancel"
    />

    <!-- 發票記錄-【編輯多筆發票紀錄】彈跳視窗 -->
    <EditMultipleBillsModal
      :show="showEditMultipleBillsModal"
      :total-amount="totalProductClosingPrice"
      :current-period-count="billPeriodCount"
      :current-bills="crmPipelinePipelineAddBillListObj"
      @confirm="handleEditMultipleBillsConfirm"
      @cancel="handleEditMultipleBillsCancel"
    />

    <!-- 履約期限通知-【新增/編輯履約期限通知】彈跳視窗 -->
    <PipelineDueModal
      :show="showDueModal"
      :due="editingDue"
      @confirm="handleDueModalConfirm"
      @cancel="handleDueModalCancel"
    />
  </div>
</template>
