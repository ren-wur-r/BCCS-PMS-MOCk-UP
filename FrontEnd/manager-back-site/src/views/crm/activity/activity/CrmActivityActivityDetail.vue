<script setup lang="ts">
//#region 引入
// Vue
import { ref, reactive, onMounted } from "vue";
// Enums / 常數
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
// Stores
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import { useRouteParamId } from "@/composables/useRouteParamId";
// Services
import type {
  MbsCrmActHttpGetActivityReqMdl,
  MbsCrmActHttpGetActivityRspMdl,
  MbsCrmActHttpUpdateActivityReqMdl,
  MbsCrmActHttpUpdateActivityRspMdl,
  MbsCrmActHttpAddActivityProductReqMdl,
  MbsCrmActHttpAddActivityProductRspMdl,
  MbsCrmActHttpUpdateActivityProductReqMdl,
  MbsCrmActHttpUpdateActivityProductRspMdl,
  MbsCrmActHttpRemoveActivityProductReqMdl,
  MbsCrmActHttpRemoveActivityProductRspMdl,
} from "@/services/pms-http/crm/activity/crmActivityHttpFormat";
import {
  getActivity,
  updateActivity,
  addActivityProduct,
  updateActivityProduct,
  removeActivityProduct,
} from "@/services/index";
// Utils
import { getFullManagerActivityKindLabel } from "@/utils/getManagerActivityKindLabel";
import { formatToDatetimeLocal, formatToServerDatetime } from "@/utils/timeFormatter";
// Components
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ConfirmDialog from "@/components/global/feedback/ConfirmDialog.vue";
import TextCounter from "@/components/global/counter/TextCounter.vue";
import ProductListTable from "./components/ProductListTable.vue";
import ProductSelectionModal from "./components/ProductSelectionModal.vue";
import ActivityDetailTabs from "@/components/feature/activity/ActivityDetailTabs.vue";
// Router
import router from "@/router";

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
//#endregion

//#region 型別定義
/** CRM-活動管理-活動-檢視-頁面模型 */
interface CrmActivityActivityDetailViewMdl {
  managerActivityID: number | null;
  managerActivityName: string | null;
  managerActivityKind: DbAtomManagerActivityKindEnum;
  managerActivityStartTime: string | null;
  managerActivityEndTime: string | null;
  managerActivityLocation: string | null;
  managerActivityExpectedLeadCount: number | null;
  managerActivityContent: string | null;
  managerActivityRegisteredCount: number | null;
  managerActivityTransferredToSalesCount: number | null;
  managerActivityOpportunityCount: number | null;
  managerActivityProductList: ManagerActivityProductItemMdl[];
}

/** CRM-活動管理-活動-產品項目 */
interface ManagerActivityProductItemMdl {
  managerActivityProductID: number;
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
}

/** 原始資料模型 */
interface OriginalDataMdl {
  managerActivityID: number | null;
  managerActivityName: string | null;
  managerActivityKind: DbAtomManagerActivityKindEnum | null;
  managerActivityStartTime: string | null;
  managerActivityEndTime: string | null;
  managerActivityLocation: string | null;
  managerActivityExpectedLeadCount: number | null;
  managerActivityContent: string | null;
  managerActivityRegisteredCount: number | null;
  managerActivityTransferredToSalesCount: number | null;
  managerActivityOpportunityCount: number | null;
  managerActivityProductList: ManagerActivityProductItemMdl[];
}

/** 產品項目模型 */
interface ProductItem {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductSubKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerActivityProductID?: number;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmActivity;
/** 活動ID */
const activityId = useRouteParamId("activityId");
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 確認儲存彈跳視窗 */
const showConfirmDialog = ref(false);
/** 是否顯示產品刪除確認彈跳視窗 */
const showProductDeleteConfirmDialog = ref(false);
/** 刪除中的產品索引 */
const deletingProductIndex = ref<number | null>(null);
/** [產品彈跳視窗]狀態 */
const productModalState = ref({
  show: false,
  mode: "add" as "add" | "edit",
  editingIndex: null as number | null,
  editingProduct: null as ProductItem | null,
});

/** CRM-活動管理-活動-檢視-頁面物件 */
const crmActivityActivityDetailViewObj = reactive<CrmActivityActivityDetailViewMdl>({
  managerActivityID: activityId,
  managerActivityName: null,
  managerActivityKind: DbAtomManagerActivityKindEnum.PhysicalActivity,
  managerActivityStartTime: null,
  managerActivityEndTime: null,
  managerActivityLocation: null,
  managerActivityExpectedLeadCount: null,
  managerActivityContent: null,
  managerActivityRegisteredCount: null,
  managerActivityTransferredToSalesCount: null,
  managerActivityOpportunityCount: null,
  managerActivityProductList: [],
});
/** 原始資料物件 */
const originalData = reactive<OriginalDataMdl>({
  managerActivityID: activityId,
  managerActivityName: null,
  managerActivityKind: null,
  managerActivityStartTime: null,
  managerActivityEndTime: null,
  managerActivityLocation: null,
  managerActivityExpectedLeadCount: null,
  managerActivityContent: null,
  managerActivityRegisteredCount: null,
  managerActivityTransferredToSalesCount: null,
  managerActivityOpportunityCount: null,
  managerActivityProductList: [],
});
/** 驗證錯誤文字 */
const validationErrorText = reactive({
  managerActivityName: "",
  managerActivityEndTime: "",
  managerActivityLocation: "",
  managerActivityExpectedLeadCount: "",
  managerActivityContent: "",
  timeLogic: "",
});

//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;
  clearValidationErrors();

  // 驗證名稱
  if (!crmActivityActivityDetailViewObj.managerActivityName?.trim()) {
    validationErrorText.managerActivityName = "不可為空";
    isValid = false;
  } else if (crmActivityActivityDetailViewObj.managerActivityName.trim().length > 100) {
    validationErrorText.managerActivityName = "不可超過100個字元";
    isValid = false;
  }

  // 驗證結束時間
  if (!crmActivityActivityDetailViewObj.managerActivityEndTime) {
    validationErrorText.managerActivityEndTime = "不可為空";
    isValid = false;
  }

  // 驗證時間邏輯
  if (
    crmActivityActivityDetailViewObj.managerActivityStartTime &&
    crmActivityActivityDetailViewObj.managerActivityEndTime
  ) {
    const startTime = new Date(crmActivityActivityDetailViewObj.managerActivityStartTime);
    const endTime = new Date(crmActivityActivityDetailViewObj.managerActivityEndTime);
    if (startTime >= endTime) {
      validationErrorText.timeLogic = "結束時間必須晚於開始時間";
      isValid = false;
    }
  }

  // 驗證地點 (只有實體活動才需要驗證地點)
  if (
    crmActivityActivityDetailViewObj.managerActivityKind ===
    DbAtomManagerActivityKindEnum.PhysicalActivity
  ) {
    if (!crmActivityActivityDetailViewObj.managerActivityLocation?.trim()) {
      validationErrorText.managerActivityLocation = "不可為空";
      isValid = false;
    } else if (crmActivityActivityDetailViewObj.managerActivityLocation.trim().length > 200) {
      validationErrorText.managerActivityLocation = "不可超過200個字元";
      isValid = false;
    }
  }

  // 驗證期望名單數
  if (
    crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount === null ||
    crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount === undefined ||
    crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount < 0
  ) {
    validationErrorText.managerActivityExpectedLeadCount = "期望名單數必須為正數";
    isValid = false;
  }

  // 驗證活動內容
  if (
    crmActivityActivityDetailViewObj.managerActivityContent &&
    crmActivityActivityDetailViewObj.managerActivityContent.length > 2000
  ) {
    validationErrorText.managerActivityContent = "不可超過2000個字元";
    isValid = false;
  }

  return isValid;
};

/** 清除驗證錯誤 */
const clearValidationErrors = () => {
  Object.keys(validationErrorText).forEach((key) => {
    validationErrorText[key as keyof typeof validationErrorText] = "";
  });
};
//#endregion

//#region API / 資料流程
/** 取得資料 */
const getData = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 檢查活動ID
  if (
    !crmActivityActivityDetailViewObj.managerActivityID ||
    isNaN(crmActivityActivityDetailViewObj.managerActivityID) ||
    crmActivityActivityDetailViewObj.managerActivityID <= 0
  ) {
    displayError("活動ID 不正確！");
    router.push("/crm/activity/activity");
    return;
  }

  // 呼叫 API 取得資料
  const requestData: MbsCrmActHttpGetActivityReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: crmActivityActivityDetailViewObj.managerActivityID!,
  };
  const responseData: MbsCrmActHttpGetActivityRspMdl | null = await getActivity(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  crmActivityActivityDetailViewObj.managerActivityName = responseData.managerActivityName;
  crmActivityActivityDetailViewObj.managerActivityKind = responseData.managerActivityKind;
  crmActivityActivityDetailViewObj.managerActivityStartTime = formatToDatetimeLocal(
    responseData.managerActivityStartTime
  );
  crmActivityActivityDetailViewObj.managerActivityEndTime = formatToDatetimeLocal(
    responseData.managerActivityEndTime
  );
  crmActivityActivityDetailViewObj.managerActivityLocation = responseData.managerActivityLocation;
  crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount =
    responseData.managerActivityExpectedLeadCount;
  crmActivityActivityDetailViewObj.managerActivityContent = responseData.managerActivityContent;
  crmActivityActivityDetailViewObj.managerActivityRegisteredCount =
    responseData.managerActivityRegisteredCount;
  crmActivityActivityDetailViewObj.managerActivityTransferredToSalesCount =
    responseData.managerActivityTransferredToSalesCount;
  crmActivityActivityDetailViewObj.managerActivityOpportunityCount =
    responseData.managerActivityOpportunityCount;
  crmActivityActivityDetailViewObj.managerActivityProductList =
    responseData.managerActivityProductList;

  // 儲存原始資料
  originalData.managerActivityID = crmActivityActivityDetailViewObj.managerActivityID;
  originalData.managerActivityName = responseData.managerActivityName;
  originalData.managerActivityKind = responseData.managerActivityKind;
  originalData.managerActivityEndTime = formatToDatetimeLocal(responseData.managerActivityEndTime);
  originalData.managerActivityLocation = responseData.managerActivityLocation;
  originalData.managerActivityExpectedLeadCount = responseData.managerActivityExpectedLeadCount;
  originalData.managerActivityContent = responseData.managerActivityContent;
  originalData.managerActivityRegisteredCount = responseData.managerActivityRegisteredCount;
  originalData.managerActivityTransferredToSalesCount =
    responseData.managerActivityTransferredToSalesCount;
  originalData.managerActivityOpportunityCount = responseData.managerActivityOpportunityCount;
  originalData.managerActivityProductList = [...responseData.managerActivityProductList];
};

/** 取得變更的欄位 */
const getChangedFields = () => {
  const changes: Partial<MbsCrmActHttpUpdateActivityReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: crmActivityActivityDetailViewObj.managerActivityID!,
  };

  let hasChanges = false;

  // 名稱
  if (
    crmActivityActivityDetailViewObj.managerActivityName?.trim() !==
    originalData.managerActivityName
  ) {
    changes.managerActivityName =
      crmActivityActivityDetailViewObj.managerActivityName?.trim() || null;
    hasChanges = true;
  }

  // 結束時間
  if (
    crmActivityActivityDetailViewObj.managerActivityEndTime !== originalData.managerActivityEndTime
  ) {
    changes.managerActivityEndTime = formatToServerDatetime(
      crmActivityActivityDetailViewObj.managerActivityEndTime
    );
    hasChanges = true;
  }

  // 地點 (只有實體活動才檢查地點變更)
  if (
    crmActivityActivityDetailViewObj.managerActivityKind ===
    DbAtomManagerActivityKindEnum.PhysicalActivity
  ) {
    if (
      crmActivityActivityDetailViewObj.managerActivityLocation?.trim() !==
      originalData.managerActivityLocation
    ) {
      changes.managerActivityLocation =
        crmActivityActivityDetailViewObj.managerActivityLocation?.trim() || null;
      hasChanges = true;
    }
  }

  // 期望名單數
  if (
    crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount !==
    originalData.managerActivityExpectedLeadCount
  ) {
    changes.managerActivityExpectedLeadCount =
      crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount;
    hasChanges = true;
  }

  // 活動內容
  if (
    crmActivityActivityDetailViewObj.managerActivityContent?.trim() !==
    originalData.managerActivityContent
  ) {
    changes.managerActivityContent =
      crmActivityActivityDetailViewObj.managerActivityContent?.trim() || null;
    hasChanges = true;
  }

  return { changes, hasChanges };
};

/** 確認儲存 */
const confirmSave = async () => {
  // 關閉確認對話框
  showConfirmDialog.value = false;

  // 檢查 token
  if (!requireToken()) return;

  // 取得變更欄位
  const { changes } = getChangedFields();

  // 呼叫 API
  const responseData: MbsCrmActHttpUpdateActivityRspMdl | null = await updateActivity(
    changes as MbsCrmActHttpUpdateActivityReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 切換回檢視模式
  isEditMode.value = false;

  // 清除驗證錯誤
  clearValidationErrors();

  // 顯示成功訊息
  displaySuccess("更新活動資訊成功！");

  // 重新載入資料
  await getData();
};

/** 取消儲存 */
const cancelSave = () => {
  showConfirmDialog.value = false;
};

/** 確認刪除產品 */
const confirmProductDelete = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 檢查刪除中的產品索引
  if (deletingProductIndex.value === null) {
    displayError("產品索引無效！");
    return;
  }

  // 取得產品
  const product =
    crmActivityActivityDetailViewObj.managerActivityProductList[deletingProductIndex.value];
  if (!product) return;

  // 呼叫 API
  const requestData: MbsCrmActHttpRemoveActivityProductReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityProductID: product.managerActivityProductID,
  };
  const responseData: MbsCrmActHttpRemoveActivityProductRspMdl | null =
    await removeActivityProduct(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉刪除確認對話窗
  showProductDeleteConfirmDialog.value = false;

  // 顯示成功訊息
  displaySuccess("刪除產品成功！");

  // 重新載入資料
  await getData();
};
//#endregion

//#region 按鈕事件
/** 點擊【編輯】按鈕 */
const clickEditBtn = () => {
  // 檢查活動是否已開始
  if (crmActivityActivityDetailViewObj.managerActivityStartTime) {
    const startTime = new Date(crmActivityActivityDetailViewObj.managerActivityStartTime);
    const currentTime = new Date();

    if (startTime <= currentTime) {
      displayError("活動已開始，無法進入編輯模式！");
      return;
    }
  }

  isEditMode.value = true;
  clearValidationErrors();
};

/** 點擊【儲存】按鈕 */
const clickSaveBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 欄位驗證
  if (!validateField()) return;

  // 檢查是否有變更
  const { hasChanges } = getChangedFields();
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 顯示確認對話框
  showConfirmDialog.value = true;
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  isEditMode.value = false;
  clearValidationErrors();
  getData();
};

/** 點擊【產品新增】按鈕 */
const clickProductAddBtn = () => {
  window.open("/ManagerBackSite/system/product/add", "_blank");
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/crm/activity/activity");
};

//#endregion

//#region 彈跳視窗事件
/** [產品]附加 */
const handleProductAppend = () => {
  productModalState.value = {
    show: true,
    mode: "add",
    editingIndex: null,
    editingProduct: null,
  };
};

/** [產品]編輯 */
const handleProductEdit = (index: number, product: ProductItem) => {
  productModalState.value = {
    show: true,
    mode: "edit",
    editingIndex: index,
    editingProduct: { ...product },
  };
};

/** [產品]刪除 */
const handleProductDelete = (index: number) => {
  deletingProductIndex.value = index;
  showProductDeleteConfirmDialog.value = true;
};

/** 取消刪除產品 */
const cancelProductDelete = () => {
  showProductDeleteConfirmDialog.value = false;
  deletingProductIndex.value = null;
};

/** [產品彈跳視窗]確認 */
const handleProductModalConfirm = async (selectedProduct: ProductItem) => {
  if (!requireToken()) return;

  // 檢查產品是否已存在於列表中
  if (productModalState.value.mode === "add") {
    const isDuplicate = crmActivityActivityDetailViewObj.managerActivityProductList.some(
      (existingProduct) => {
        return existingProduct.managerProductID === selectedProduct.managerProductID;
      }
    );

    if (isDuplicate) {
      displayError(`「${selectedProduct.managerProductName}」已存在於列表中，無法重複新增！`);
      return;
    }
  }

  // 根據模式呼叫相應的 API
  if (productModalState.value.mode === "edit" && productModalState.value.editingIndex !== null) {
    // 編輯模式
    const currentProduct =
      crmActivityActivityDetailViewObj.managerActivityProductList[
        productModalState.value.editingIndex
      ];

    const requestData: MbsCrmActHttpUpdateActivityProductReqMdl = {
      employeeLoginToken: tokenStore.token!,
      managerActivityProductID: currentProduct.managerActivityProductID,
      managerProductID: selectedProduct.managerProductID,
    };

    const responseData: MbsCrmActHttpUpdateActivityProductRspMdl | null =
      await updateActivityProduct(requestData);

    if (!responseData) {
      displayError("系統錯誤，請稍後再試！");
      return;
    }
    const isSuccess = handleErrorCode(responseData.errorCode);
    if (!isSuccess) return;

    // 顯示成功訊息
    displaySuccess("更新產品成功！");

    // 關閉彈跳視窗
    handleProductModalClose();

    // 重新載入資料
    await getData();
  } else {
    // 新增模式
    const requestData: MbsCrmActHttpAddActivityProductReqMdl = {
      employeeLoginToken: tokenStore.token!,
      managerActivityID: crmActivityActivityDetailViewObj.managerActivityID!,
      managerProductID: selectedProduct.managerProductID,
    };

    const responseData: MbsCrmActHttpAddActivityProductRspMdl | null =
      await addActivityProduct(requestData);

    if (!responseData) {
      displayError("系統錯誤，請稍後再試！");
      return;
    }

    const isSuccess = handleErrorCode(responseData.errorCode);
    if (!isSuccess) return;

    // 顯示成功訊息
    displaySuccess("新增產品成功！");

    // 關閉彈跳視窗
    handleProductModalClose();

    // 重新載入資料
    await getData();
  }
};

/** [產品彈跳視窗]關閉 */
const handleProductModalClose = () => {
  productModalState.value = {
    show: false,
    mode: "add",
    editingIndex: null,
    editingProduct: null,
  };
};
//#endregion

//#region 生命週期
onMounted(() => {
  getData();
});
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <!-- 標題區 -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-4">
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
      <div class="flex items-center gap-2">
        <button
          v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
          class="btn-update"
          @click="clickEditBtn()"
        >
          編輯
        </button>
        <template v-else>
          <div class="flex gap-1">
            <button class="btn-cancel" @click="clickCancelBtn()">取消</button>
            <button class="btn-save" @click="clickSaveBtn()">儲存</button>
          </div>
        </template>
      </div>
    </div>

    <ActivityDetailTabs :activity-id="activityId" base-path="/crm/activity/activity" />

    <!-- 內容 -->
    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-8 gap-4">
      <div class="subtitle">活動資訊</div>

      <!-- 活動類型 -->
      <div class="flex flex-col gap-2">
        <label class="form-label">活動類型 <span class="required-label">*</span></label>
        <button type="button" class="btn-activity-type-disabled" disabled>
          {{
            getFullManagerActivityKindLabel(crmActivityActivityDetailViewObj.managerActivityKind)
          }}
        </button>
      </div>

      <!-- 活動名稱 -->
      <div>
        <label class="form-label">活動名稱 <span class="required-label">*</span></label>
        <input
          v-model="crmActivityActivityDetailViewObj.managerActivityName"
          type="text"
          class="input-box"
          :disabled="!isEditMode"
          placeholder="請輸入活動名稱"
          maxlength="100"
        />
        <div class="error-wrapper">
          <div v-if="isEditMode && validationErrorText.managerActivityName" class="error-tip">
            {{ validationErrorText.managerActivityName }}
          </div>
        </div>
      </div>

      <div class="grid grid-cols-2 gap-4">
        <!-- 開始時間 -->
        <div>
          <label class="form-label">開始時間 <span class="required-label">*</span></label>
          <input
            v-model="crmActivityActivityDetailViewObj.managerActivityStartTime"
            type="datetime-local"
            class="input-box"
            disabled
          />
        </div>

        <!-- 結束時間 -->
        <div>
          <label class="form-label">結束時間 <span class="required-label">*</span></label>
          <input
            v-model="crmActivityActivityDetailViewObj.managerActivityEndTime"
            type="datetime-local"
            class="input-box"
            :disabled="!isEditMode"
          />
          <div class="error-wrapper">
            <p v-if="isEditMode && validationErrorText.managerActivityEndTime" class="error-tip">
              {{ validationErrorText.managerActivityEndTime }}
            </p>
          </div>
        </div>
      </div>

      <!-- 時間邏輯驗證錯誤訊息 -->
      <div v-if="isEditMode && validationErrorText.timeLogic" class="error-tip">
        {{ validationErrorText.timeLogic }}
      </div>

      <!-- 地點 -->
      <div class="flex flex-col gap-2">
        <label class="form-label"> 地點 <span class="required-label">*</span> </label>
        <input
          v-model="crmActivityActivityDetailViewObj.managerActivityLocation"
          type="text"
          class="input-box"
          :disabled="
            !isEditMode ||
            crmActivityActivityDetailViewObj.managerActivityKind ===
              DbAtomManagerActivityKindEnum.OnlineActivity
          "
          placeholder="請輸入活動地點"
          maxlength="200"
        />
        <div class="error-wrapper">
          <p v-if="isEditMode && validationErrorText.managerActivityLocation" class="error-tip">
            {{ validationErrorText.managerActivityLocation }}
          </p>
        </div>
      </div>

      <!-- 期望名單數 -->
      <div class="w-1/4">
        <label class="form-label">期望名單數 <span class="required-label">*</span></label>
        <input
          v-model="crmActivityActivityDetailViewObj.managerActivityExpectedLeadCount"
          type="number"
          class="input-box"
          :disabled="!isEditMode"
          placeholder="請輸入期望名單數"
          min="0"
        />
        <div class="error-wrapper">
          <p
            v-if="isEditMode && validationErrorText.managerActivityExpectedLeadCount"
            class="error-tip"
          >
            {{ validationErrorText.managerActivityExpectedLeadCount }}
          </p>
        </div>
      </div>

      <!-- 活動內容與備註 -->
      <div class="flex flex-col gap-2">
        <label class="form-label">活動內容與備註</label>
        <div class="select-wrapper">
          <textarea
            v-model="crmActivityActivityDetailViewObj.managerActivityContent"
            type="text"
            class="textarea-box"
            placeholder="請輸入活動內容與備註"
            rows="15"
            :disabled="!isEditMode"
            maxlength="2000"
          ></textarea>
        </div>
        <div>
          <TextCounter
            :text="crmActivityActivityDetailViewObj.managerActivityContent"
            :max-length="2000"
            :required="false"
          />
        </div>
        <div class="error-wrapper">
          <p v-if="isEditMode && validationErrorText.managerActivityContent" class="error-tip">
            {{ validationErrorText.managerActivityContent }}
          </p>
        </div>
      </div>
    </div>

    <!-- 活動產品區塊 -->
    <div v-if="!isEditMode" class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between items-center">
        <h2 class="subtitle">活動產品</h2>
        <button class="btn-add" @click="clickProductAddBtn">新增產品</button>
      </div>

      <ProductListTable
        :productList="crmActivityActivityDetailViewObj.managerActivityProductList"
        :show-action-row="true"
        :showAppendBtn="true"
        @edit="handleProductEdit"
        @delete="handleProductDelete"
        @append="handleProductAppend"
      />
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 產品彈跳視窗 -->
    <ProductSelectionModal
      :show="productModalState.show"
      :mode="productModalState.mode"
      :default-product="productModalState.editingProduct"
      @close="handleProductModalClose"
      @confirm="handleProductModalConfirm"
    />

    <!-- 確認彈跳視窗:刪除產品 -->
    <ConfirmDialog
      :show="showProductDeleteConfirmDialog"
      title="確認刪除"
      message="確定要刪除此產品嗎？"
      confirm-text="確定"
      cancel-text="取消"
      @confirm="confirmProductDelete"
      @cancel="cancelProductDelete"
    />

    <!-- 彈跳視窗:確認儲存  -->
    <ConfirmDialog
      :show="showConfirmDialog"
      title="確認修改"
      message="確定要儲存活動資料的修改嗎？"
      confirm-text="確定"
      cancel-text="取消"
      @confirm="confirmSave"
      @cancel="cancelSave"
    />
  </div>
</template>
