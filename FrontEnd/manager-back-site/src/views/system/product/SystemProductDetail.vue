<script setup lang="ts">
//#region 引入
import { reactive, ref, onMounted, defineAsyncComponent } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useRouter, useRoute } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import { DbManagerProductKindEnum } from "@/constants/DbManagerProductKindEnum";
import {
  getProduct,
  updateProduct,
  addProductSpecification,
  updateProductSpecification,
} from "@/services";
import type {
  MbsSysPrdHttpAddSpecificationReqMdl,
  MbsSysPrdHttpAddSpecificationRspMdl,
  MbsSysPrdHttpGetReqMdl,
  MbsSysPrdHttpGetRspMdl,
  MbsSysPrdHttpUpdateReqMdl,
  MbsSysPrdHttpUpdateRspMdl,
  MbsSysPrdHttpUpdateSpecificationReqMdl,
  MbsSysPrdHttpUpdateSpecificationRspMdl,
} from "@/services/pms-http/system/product/systemProductHttpFormat";
import type {
  SysPrdSpecificationAddPayload,
  SysPrdSpecificationUpdatePayload,
} from "./components/specification/model/payload.ts";
import TextCounter from "@/components/global/counter/TextCounter.vue";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
import { formatCurrency } from "@/utils/currencyFormatter";
const AddProductSpecificationModal = defineAsyncComponent(
  () => import("./components/specification/AddProductSpecificationModal.vue")
);
const UpdateProductSpecificationModal = defineAsyncComponent(
  () => import("./components/specification/UpdateProductSpecificationModal.vue")
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
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
/** 當前路由資訊（path / params / query） */
const route = useRoute();

//#endregion

//#region 型別定義
/** 系統設定-產品-檢視模型 */
interface SysProductDetailMdl {
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
  managerProductKind: DbManagerProductKindEnum;
  managerProductIsKey: boolean;
  managerProductRemark: string | null;
  managerProductIsEnable: boolean;
  managerProductSpecificationList: SysPrdSpecificationListItem[];
}

/** 系統設定-產品-檢視-驗證模型 */
interface SysPrdDetailValidateMdl {
  managerProductName: boolean;
  managerProductMainKindID: boolean;
  managerProductSubKindID: boolean;
  managerProductSpecificationList: boolean;
}

/** 系統設定-產品-原始模型 */
interface SysProductOriginalMdl {
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductSubKindID: number;
  managerProductKind: DbManagerProductKindEnum;
  managerProductIsKey: boolean;
  managerProductRemark: string | null;
  managerProductIsEnable: boolean;
  managerProductSpecificationList: SysPrdSpecificationListItem[];
}

/** 系統設定-產品-規格-列表-項目 */
interface SysPrdSpecificationListItem {
  managerProductSpecificationID: number;
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}

/** 系統設定-產品-規格-列表-原始模型 */
interface SysPrdSpecificationListOriginalMdl {
  managerProductSpecificationID: number;
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemProduct;
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 產品ID */
const managerProductID = Number(route.params.id);
/** 顯示-產品-規格-新增-Modal */
const showProductSpecificationAddModal = ref(false);
/** 顯示-產品-規格-更新-Modal */
const showProductSpecificationUpdateModal = ref(false);
/** 正在更新的規格資料 */
const updatingSpecification = ref<SysPrdSpecificationListItem | null>(null);
/** 系統設定-產品-檢視物件 */
const sysPrdDetailObj = reactive<SysProductDetailMdl>({
  managerProductName: "",
  managerProductMainKindID: 0,
  managerProductMainKindName: "",
  managerProductSubKindID: 0,
  managerProductSubKindName: "",
  managerProductKind: DbManagerProductKindEnum.Equipment,
  managerProductIsKey: false,
  managerProductRemark: "",
  managerProductIsEnable: true,
  managerProductSpecificationList: [],
});
/** 系統設定-產品-檢視-驗證物件 */
const sysPrdDetailValidObj = reactive<SysPrdDetailValidateMdl>({
  managerProductName: true,
  managerProductMainKindID: true,
  managerProductSubKindID: true,
  managerProductSpecificationList: true,
});
/** 系統設定-產品-原始物件 */
const sysPrdOriginalObj = reactive<SysProductOriginalMdl>({
  managerProductName: "",
  managerProductMainKindID: 0,
  managerProductSubKindID: 0,
  managerProductKind: DbManagerProductKindEnum.Equipment,
  managerProductIsKey: false,
  managerProductRemark: "",
  managerProductIsEnable: true,
  managerProductSpecificationList: [],
});
/** 系統設定-產品-規格-原始物件 */
const sysPrdSpecificationOriginalObj = reactive<SysPrdSpecificationListOriginalMdl>({
  managerProductSpecificationID: 0,
  managerProductSpecificationName: "",
  managerProductSpecificationSellPrice: 0,
  managerProductSpecificationCostPrice: 0,
  managerProductSpecificationIsEnable: true,
});

//#endregion

//#region API / 資料流程
/** 取得【產品/規格】資料 */
const getProductData = async () => {
  //驗證token
  if (!requireToken()) return;

  const requestData: MbsSysPrdHttpGetReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductID,
  };

  const responseData: MbsSysPrdHttpGetRspMdl | null = await getProduct(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 設定資料
  sysPrdDetailObj.managerProductName = responseData.managerProductName;
  sysPrdDetailObj.managerProductMainKindID = responseData.managerProductMainKindID;
  sysPrdDetailObj.managerProductMainKindName = responseData.managerProductMainKindName;
  sysPrdDetailObj.managerProductSubKindID = responseData.managerProductSubKindID;
  sysPrdDetailObj.managerProductSubKindName = responseData.managerProductSubKindName;
  sysPrdDetailObj.managerProductKind = responseData.managerProductKind;
  sysPrdDetailObj.managerProductIsKey = responseData.managerProductIsKey;
  sysPrdDetailObj.managerProductRemark = responseData.managerProductRemark;
  sysPrdDetailObj.managerProductIsEnable = responseData.managerProductIsEnable;
  sysPrdDetailObj.managerProductSpecificationList =
    responseData.managerProductSpecificationList?.map((item) => {
      return {
        managerProductSpecificationID: item.managerProductSpecificationID,
        managerProductSpecificationName: item.managerProductSpecificationName,
        managerProductSpecificationSellPrice: item.managerProductSpecificationSellPrice,
        managerProductSpecificationCostPrice: item.managerProductSpecificationCostPrice,
        managerProductSpecificationIsEnable: item.managerProductSpecificationIsEnable,
      };
    });

  // 複製原始資料
  Object.assign(sysPrdOriginalObj, {
    managerProductName: responseData.managerProductName,
    managerProductMainKindID: responseData.managerProductMainKindID,
    managerProductSubKindID: responseData.managerProductSubKindID,
    managerProductKind: responseData.managerProductKind,
    managerProductIsKey: responseData.managerProductIsKey,
    managerProductRemark: responseData.managerProductRemark,
    managerProductIsEnable: responseData.managerProductIsEnable,
    managerProductSpecificationList:
      responseData.managerProductSpecificationList?.map((item) => {
        return {
          managerProductSpecificationName: item.managerProductSpecificationName,
          managerProductSpecificationSellPrice: item.managerProductSpecificationSellPrice,
          managerProductSpecificationCostPrice: item.managerProductSpecificationCostPrice,
          managerProductSpecificationIsEnable: item.managerProductSpecificationIsEnable,
        };
      }) || [],
  });
};

/** 取得【產品】變更欄位 */
const getProductChangedFields = (): Partial<MbsSysPrdHttpUpdateReqMdl> => {
  const changes: Partial<MbsSysPrdHttpUpdateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerProductID,
  };

  if (sysPrdDetailObj.managerProductName !== sysPrdOriginalObj.managerProductName) {
    changes.managerProductName = sysPrdDetailObj.managerProductName;
  }

  if (sysPrdDetailObj.managerProductMainKindID !== sysPrdOriginalObj.managerProductMainKindID) {
    changes.managerProductMainKindID = sysPrdDetailObj.managerProductMainKindID;
  }

  if (sysPrdDetailObj.managerProductSubKindID !== sysPrdOriginalObj.managerProductSubKindID) {
    changes.managerProductSubKindID = sysPrdDetailObj.managerProductSubKindID;
  }

  if (sysPrdDetailObj.managerProductKind !== sysPrdOriginalObj.managerProductKind) {
    changes.managerProductKind = sysPrdDetailObj.managerProductKind;
  }

  if (sysPrdDetailObj.managerProductIsKey !== sysPrdOriginalObj.managerProductIsKey) {
    changes.managerProductIsKey = sysPrdDetailObj.managerProductIsKey;
  }

  if (sysPrdDetailObj.managerProductRemark !== sysPrdOriginalObj.managerProductRemark) {
    changes.managerProductRemark = sysPrdDetailObj.managerProductRemark;
  }

  if (sysPrdDetailObj.managerProductIsEnable !== sysPrdOriginalObj.managerProductIsEnable) {
    changes.managerProductIsEnable = sysPrdDetailObj.managerProductIsEnable;
  }

  return changes;
};

/** 取得【規格】變更欄位 */
const getSpecificationChangedFields = (
  original: SysPrdSpecificationUpdatePayload,
  current: SysPrdSpecificationUpdatePayload
): Partial<Omit<SysPrdSpecificationUpdatePayload, "managerProductSpecificationID">> => {
  const changes: Partial<Omit<SysPrdSpecificationUpdatePayload, "managerProductSpecificationID">> =
    {};

  if (original.managerProductSpecificationName !== current.managerProductSpecificationName) {
    changes.managerProductSpecificationName = current.managerProductSpecificationName;
  }

  if (
    original.managerProductSpecificationSellPrice !== current.managerProductSpecificationSellPrice
  ) {
    changes.managerProductSpecificationSellPrice = current.managerProductSpecificationSellPrice;
  }

  if (
    original.managerProductSpecificationCostPrice !== current.managerProductSpecificationCostPrice
  ) {
    changes.managerProductSpecificationCostPrice = current.managerProductSpecificationCostPrice;
  }

  if (
    original.managerProductSpecificationIsEnable !== current.managerProductSpecificationIsEnable
  ) {
    changes.managerProductSpecificationIsEnable = current.managerProductSpecificationIsEnable;
  }

  return changes;
};

/** 處理【新增規格】*/
const handleAddSpecification = async (payload: SysPrdSpecificationAddPayload) => {
  // 驗證token
  if (!requireToken()) return;

  // 呼叫API
  const requestData: MbsSysPrdHttpAddSpecificationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductID: managerProductID,
    managerProductSpecificationName: payload.managerProductSpecificationName,
    managerProductSpecificationSellPrice: payload.managerProductSpecificationSellPrice,
    managerProductSpecificationCostPrice: payload.managerProductSpecificationCostPrice,
    managerProductSpecificationIsEnable: payload.managerProductSpecificationIsEnable,
  };
  const responseData: MbsSysPrdHttpAddSpecificationRspMdl | null =
    await addProductSpecification(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增規格成功！");

  // 重新取得產品資料
  await getProductData();

  // 關閉Modal
  showProductSpecificationAddModal.value = false;
};

/** 處理【更新產品規格】 */
const handleUpdateSpecification = async (payload: SysPrdSpecificationUpdatePayload) => {
  if (!requireToken()) return;

  // 取得變更欄位
  const changedFields = getSpecificationChangedFields(sysPrdSpecificationOriginalObj, payload);
  if (Object.keys(changedFields).length === 0) {
    displayError("沒有任何變更需要儲存!");
    return;
  }

  // 呼叫API
  const requestData: MbsSysPrdHttpUpdateSpecificationReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductID: managerProductID,
    managerProductSpecificationID: payload.managerProductSpecificationID,
    ...(changedFields as Omit<SysPrdSpecificationUpdatePayload, "managerProductSpecificationID">),
  };
  const responseData: MbsSysPrdHttpUpdateSpecificationRspMdl | null =
    await updateProductSpecification(requestData as MbsSysPrdHttpUpdateSpecificationReqMdl);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新規格成功！");
  // 重新取得產品資料
  await getProductData();
  // 關閉Modal
  showProductSpecificationUpdateModal.value = false;
};

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/product");
};

/** 點擊【編輯】按鈕 */
const clickUpdateBtn = () => {
  isEditMode.value = true;
};

/** 點擊取消編輯【產品】按鈕 */
const clickCancelProductBtn = () => {
  isEditMode.value = false;
  resetProductValidation();
  getProductData();
};

/** 點擊提交【產品】按鈕 */
const clickSubmitProductBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateProductField()) return;

  // 取得變更欄位
  const changedFields = getProductChangedFields();
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存!");
    return;
  }
  // 呼叫api
  const responseData: MbsSysPrdHttpUpdateRspMdl | null = await updateProduct(
    changedFields as MbsSysPrdHttpUpdateReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新產品資料成功！");

  // 編輯模式關閉
  isEditMode.value = false;

  // 重新取得產品資料
  await getProductData();
};

/** 點擊打開【新增規格】Modal按鈕 */
const clickOpenAddSpecificationModalBtn = () => {
  showProductSpecificationAddModal.value = true;
};

/** 點擊打開【編輯規格】Modal按鈕 */
const clickOpenUpdateSpecificationModalBtn = (managerProductSpecificationID: number) => {
  // 找出該筆資料
  const target = sysPrdDetailObj.managerProductSpecificationList.find(
    (item) => item.managerProductSpecificationID === managerProductSpecificationID
  );
  if (!target) return;

  // 設定編輯資料
  updatingSpecification.value = { ...target };

  // 同步保存一份原始資料以供後續比對
  Object.assign(sysPrdSpecificationOriginalObj, target);
  showProductSpecificationUpdateModal.value = true;
};

//#endregion

//#region 驗證相關
/**驗證【產品】欄位 */
const validateProductField = () => {
  let isValid = true;

  // 名稱
  if (
    typeof sysPrdDetailObj.managerProductName !== "string" ||
    !sysPrdDetailObj.managerProductName.trim() ||
    sysPrdDetailObj.managerProductName.trim().length > 20
  ) {
    sysPrdDetailValidObj.managerProductName = false;
    isValid = false;
  } else {
    sysPrdDetailValidObj.managerProductName = true;
  }

  // 主分類
  if (!sysPrdDetailObj.managerProductMainKindID) {
    sysPrdDetailValidObj.managerProductMainKindID = false;
    isValid = false;
  } else {
    sysPrdDetailValidObj.managerProductMainKindID = true;
  }

  // 子分類
  if (!sysPrdDetailObj.managerProductSubKindID) {
    sysPrdDetailValidObj.managerProductSubKindID = false;
    isValid = false;
  } else {
    sysPrdDetailValidObj.managerProductSubKindID = true;
  }

  // 至少一筆規格
  if (!sysPrdDetailObj.managerProductSpecificationList.length) {
    sysPrdDetailValidObj.managerProductSpecificationList = false;
    isValid = false;
  } else {
    sysPrdDetailValidObj.managerProductSpecificationList = true;
  }

  return isValid;
};

/** 重設【產品】驗證狀態 */
const resetProductValidation = () => {
  Object.assign(sysPrdDetailValidObj, {
    managerProductName: true,
    managerProductMainKindID: true,
    managerProductSubKindID: true,
    managerProductSpecificationList: true,
  });
};

//#endregion

//#region 生命週期
onMounted(() => {
  getProductData();
});

//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
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
    </div>

    <!-- 產品資訊區塊 -->
    <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div class="flex justify-between items-center">
        <div class="subtitle">產品資訊</div>
        <div class="flex gap-4 w-full justify-end">
          <!--編輯模式-->
          <template v-if="isEditMode">
            <div class="flex gap-2">
              <button class="btn-cancel" @click="clickCancelProductBtn">取消編輯</button>
              <button class="btn-submit" @click="clickSubmitProductBtn">儲存</button>
            </div>
          </template>
          <!--檢視模式-->
          <template v-else>
            <button
              v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
              class="btn-update"
              @click="clickUpdateBtn"
            >
              編輯
            </button>
          </template>
        </div>
      </div>

      <!-- 產品名稱 -->
      <div>
        <label class="form-label">產品名稱 <span class="required-label">*</span></label>
        <input
          v-model="sysPrdDetailObj.managerProductName"
          class="input-box"
          :disabled="!isEditMode"
          placeholder="請輸入產品名稱"
        />
        <p class="error-wrapper">
          <span v-if="!sysPrdDetailValidObj.managerProductName" class="error-tip">
            不可為空，長度不可超過 30 個字
          </span>
        </p>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 產品主分類 -->
        <div class="flex-1">
          <label class="form-label">產品主分類 <span class="required-label">*</span></label>
          <div v-if="isEditMode">
            <GetManyManagerProductMainKindComboBox
              v-model:managerProductMainKindID="sysPrdDetailObj.managerProductMainKindID"
              v-model:managerProductMainKindName="sysPrdDetailObj.managerProductMainKindName"
              :disabled="!isEditMode"
            />
          </div>
          <!--顯示用-->
          <div v-else>
            <input
              v-model="sysPrdDetailObj.managerProductMainKindName"
              class="input-box"
              :disabled="!isEditMode"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!sysPrdDetailValidObj.managerProductMainKindID" class="error-tip">
              不可為空
            </span>
          </p>
        </div>

        <!-- 產品子分類 -->
        <div class="flex-1">
          <label class="form-label">產品子分類 <span class="required-label">*</span></label>
          <div v-if="isEditMode">
            <GetManyManagerProductSubKindComboBox
              v-model:managerProductMainKindID="sysPrdDetailObj.managerProductMainKindID"
              v-model:managerProductSubKindID="sysPrdDetailObj.managerProductSubKindID"
              v-model:managerProductSubKindName="sysPrdDetailObj.managerProductSubKindName"
              :disabled="!isEditMode"
            />
          </div>
          <!--顯示用-->
          <div v-else>
            <input
              v-model="sysPrdDetailObj.managerProductSubKindName"
              class="input-box"
              :disabled="!isEditMode"
            />
          </div>
          <p class="error-wrapper">
            <span v-if="!sysPrdDetailValidObj.managerProductSubKindID" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 類型 -->
        <div class="flex-1">
          <label class="form-label">類型 <span class="required-label">*</span></label>
          <div class="flex gap-8">
            <label>
              <input
                v-model="sysPrdDetailObj.managerProductKind"
                type="radio"
                :value="DbManagerProductKindEnum.Equipment"
                :disabled="!isEditMode"
              />
              設備
            </label>

            <label>
              <input
                v-model="sysPrdDetailObj.managerProductKind"
                type="radio"
                :value="DbManagerProductKindEnum.Service"
                :disabled="!isEditMode"
              />
              服務
            </label>
          </div>
          <p class="error-wrapper"></p>
        </div>

        <!-- 是否為主力產品 -->
        <div class="flex-1">
          <label class="form-label">主力產品 <span class="required-label">*</span></label>
          <div class="flex gap-8">
            <label>
              <input
                v-model="sysPrdDetailObj.managerProductIsKey"
                type="radio"
                :value="true"
                :disabled="!isEditMode"
              />
              是
            </label>
            <label>
              <input
                v-model="sysPrdDetailObj.managerProductIsKey"
                type="radio"
                :value="false"
                :disabled="!isEditMode"
              />
              否
            </label>
          </div>
          <p class="error-wrapper"></p>
        </div>
      </div>

      <!-- 狀態 -->
      <div class="w-48">
        <label class="form-label">狀態 <span class="required-label">*</span></label>
        <select
          v-model="sysPrdDetailObj.managerProductIsEnable"
          class="select-box"
          :disabled="!isEditMode"
        >
          <option :value="true">啟用</option>
          <option :value="false">停用</option>
        </select>
        <p class="error-wrapper"></p>
      </div>

      <!-- 備註 -->
      <div>
        <div>
          <label class="form-label">備註</label>
          <textarea
            v-model="sysPrdDetailObj.managerProductRemark"
            :disabled="!isEditMode"
            maxlength="500"
            rows="5"
            placeholder="請輸入備註"
            class="textarea-box resize-none w-full"
          ></textarea>
          <div>
            <TextCounter
              :text="sysPrdDetailObj.managerProductRemark"
              :max-length="500"
              :required="false"
            />
          </div>
          <p class="error-wrapper"></p>
        </div>
      </div>
    </div>

    <!-- 產品規格區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex items-center justify-between">
        <div class="subtitle">產品規格</div>
        <button
          v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-append"
          @click="clickOpenAddSpecificationModalBtn"
        >
          新增規格
        </button>
      </div>

      <div class="flex-1 overflow-y-auto table-scrollable">
        <!-- 產品規格列表 -->
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start">規格名稱</th>
              <th class="text-end">售價</th>
              <th class="text-end">成本</th>
              <th class="text-center">狀態</th>
              <th
                v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                class="text-center"
              >
                操作
              </th>
            </tr>
          </thead>

          <tbody>
            <template v-if="!sysPrdDetailObj.managerProductSpecificationList.length">
              <tr class="text-center">
                <td colspan="5">無資料</td>
              </tr>
            </template>

            <template v-else>
              <tr
                v-for="item in sysPrdDetailObj.managerProductSpecificationList"
                :key="item.managerProductSpecificationID"
                class="border border-gray-300"
              >
                <td class="text-start">{{ item.managerProductSpecificationName }}</td>
                <td class="text-end">
                  {{ formatCurrency(item.managerProductSpecificationSellPrice) }}
                </td>
                <td class="text-end">
                  {{ formatCurrency(item.managerProductSpecificationCostPrice) }}
                </td>
                <td
                  class="text-center"
                  :class="{
                    'enabled-text': item.managerProductSpecificationIsEnable,
                    'disabled-text': !item.managerProductSpecificationIsEnable,
                  }"
                >
                  {{ item.managerProductSpecificationIsEnable ? "啟用" : "停用" }}
                </td>
                <td
                  v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                  class="text-center"
                >
                  <button
                    v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                    class="btn-update"
                    @click="
                      clickOpenUpdateSpecificationModalBtn(item.managerProductSpecificationID)
                    "
                  >
                    編輯
                  </button>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>
    </div>
  </div>

  <!-- 新增規格 Modal -->
  <AddProductSpecificationModal
    v-if="showProductSpecificationAddModal"
    @close="showProductSpecificationAddModal = false"
    @confirm="handleAddSpecification"
  />

  <!-- 更新規格 Modal -->
  <UpdateProductSpecificationModal
    v-if="showProductSpecificationUpdateModal"
    :data="updatingSpecification!"
    @confirm="handleUpdateSpecification"
    @close="showProductSpecificationUpdateModal = false"
  />

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
