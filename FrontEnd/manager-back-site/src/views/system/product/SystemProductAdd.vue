<script setup lang="ts">
//#region 引入
import { reactive, ref } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import router from "@/router";
import { DbManagerProductKindEnum } from "@/constants/DbManagerProductKindEnum";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import { addProduct } from "@/services";
import {
  MbsSysPrdHttpAddReqMdl,
  MbsSysPrdHttpAddRspMdl,
  MbsSysPrdHttpAddSpecificationReqItemMdl,
  MbsSysPrdHttpAddSpecificationReqMdl,
} from "@/services/pms-http/system/product/systemProductHttpFormat";
import TextCounter from "@/components/global/counter/TextCounter.vue";
import { formatCurrency } from "@/utils/currencyFormatter";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
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
/** 系統設定-產品-新增-頁面模型 */
interface SysPrdAddViewMdl {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
  managerProductKind: DbManagerProductKindEnum;
  managerProductIsKey: boolean;
  managerProductRemark: string | null;
  managerProductIsEnable: boolean;
  managerProductSpecificationList: sysPrdSpecificationAddViewMdl[];
}
/** 系統設定-產品-新增-驗證模型 */
interface SysPrdAddValidatieMdl {
  managerProductName: boolean;
  managerProductMainKindID: boolean;
  managerProductSubKindID: boolean;
  managerProductKind: boolean;
  managerProductSpecificationList: boolean;
}
/**系統設定-產品-規格-新增-頁面模型 */
interface sysPrdSpecificationAddViewMdl {
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}
/** 系統設定-產品-規格-新增-驗證模型 */
interface SysPrdSpecificationAddValidateMdl {
  managerProductSpecificationName: boolean;
  managerProductSpecificationSellPrice: boolean;
  managerProductSpecificationCostPrice: boolean;
  managerProductSpecificationIsEnable: boolean;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemProduct;
/** 系統-產品-規格-新增-Modal-顯示 */
const sysPrdSpecificationAddModalShow = ref(false);
/** 暫存產品規格清單 */
const productSpecificationList = ref<sysPrdSpecificationAddViewMdl[]>([]);
/** 產品規格索引值 */
const productSpecificationIndex = ref<number | null>(null);
/** 系統設定-產品-規格-新增-驗證物件 */
const sysPrdSpecificationAddValidObj = reactive<SysPrdSpecificationAddValidateMdl>({
  managerProductSpecificationName: true,
  managerProductSpecificationSellPrice: true,
  managerProductSpecificationCostPrice: true,
  managerProductSpecificationIsEnable: true,
});
/** 系統設定-產品-新增-頁面物件 */
const sysPrdAddViewObj = reactive<SysPrdAddViewMdl>({
  managerProductID: 0,
  managerProductName: "",
  managerProductMainKindID: 0,
  managerProductMainKindName: "",
  managerProductSubKindID: 0,
  managerProductSubKindName: "",
  managerProductKind: DbManagerProductKindEnum.Equipment,
  managerProductIsKey: false,
  managerProductRemark: null,
  managerProductIsEnable: true,
  managerProductSpecificationList: [],
});
/** 系統設定-產品-規格-新增-頁面物件 */
const sysPrdSpecificationAddViewObj = reactive<sysPrdSpecificationAddViewMdl>({
  managerProductSpecificationName: "",
  managerProductSpecificationSellPrice: 0,
  managerProductSpecificationCostPrice: 0,
  managerProductSpecificationIsEnable: true,
});
/** 系統設定-產品-新增-驗證物件 */
const sysPrdAddValidObj = reactive<SysPrdAddValidatieMdl>({
  managerProductName: true,
  managerProductMainKindID: true,
  managerProductSubKindID: true,
  managerProductKind: true,
  managerProductSpecificationList: true,
});
//#endregion

//#region 驗證相關
/** 驗證【產品】欄位 */
const validateProductField = () => {
  let isValid = true;

  // 產品名稱：不可空、長度不超過30
  if (
    typeof sysPrdAddViewObj.managerProductName !== "string" ||
    !sysPrdAddViewObj.managerProductName.trim() ||
    sysPrdAddViewObj.managerProductName.trim().length > 30
  ) {
    sysPrdAddValidObj.managerProductName = false;
    isValid = false;
  } else {
    sysPrdAddValidObj.managerProductName = true;
  }

  // 主分類：不可為 0 或 null
  if (!sysPrdAddViewObj.managerProductMainKindID) {
    sysPrdAddValidObj.managerProductMainKindID = false;
    isValid = false;
  } else {
    sysPrdAddValidObj.managerProductMainKindID = true;
  }

  // 子分類：不可為 0 或 null
  if (!sysPrdAddViewObj.managerProductSubKindID) {
    sysPrdAddValidObj.managerProductSubKindID = false;
    isValid = false;
  } else {
    sysPrdAddValidObj.managerProductSubKindID = true;
  }

  // 規格至少要有一筆
  if (!productSpecificationList.value.length) {
    sysPrdAddValidObj.managerProductSpecificationList = false;
    isValid = false;
  } else {
    sysPrdAddValidObj.managerProductSpecificationList = true;
  }

  return isValid;
};

/** 驗證【規格】欄位 */
const validateSpecificationField = () => {
  let isValid = true;

  // 規格名稱
  if (
    typeof sysPrdSpecificationAddViewObj.managerProductSpecificationName !== "string" ||
    !sysPrdSpecificationAddViewObj.managerProductSpecificationName.trim() ||
    sysPrdSpecificationAddViewObj.managerProductSpecificationName.trim().length > 100
  ) {
    sysPrdSpecificationAddValidObj.managerProductSpecificationName = false;
    isValid = false;
  } else {
    sysPrdSpecificationAddValidObj.managerProductSpecificationName = true;
  }

  // 售價
  if (
    sysPrdSpecificationAddViewObj.managerProductSpecificationSellPrice === null ||
    sysPrdSpecificationAddViewObj.managerProductSpecificationSellPrice === undefined ||
    isNaN(sysPrdSpecificationAddViewObj.managerProductSpecificationSellPrice)
  ) {
    sysPrdSpecificationAddValidObj.managerProductSpecificationSellPrice = false;
    isValid = false;
  } else {
    sysPrdSpecificationAddValidObj.managerProductSpecificationSellPrice = true;
  }

  // 成本
  if (
    sysPrdSpecificationAddViewObj.managerProductSpecificationCostPrice === null ||
    sysPrdSpecificationAddViewObj.managerProductSpecificationCostPrice === undefined ||
    isNaN(sysPrdSpecificationAddViewObj.managerProductSpecificationCostPrice)
  ) {
    sysPrdSpecificationAddValidObj.managerProductSpecificationCostPrice = false;
    isValid = false;
  } else {
    sysPrdSpecificationAddValidObj.managerProductSpecificationCostPrice = true;
  }

  // 狀態
  if (typeof sysPrdSpecificationAddViewObj.managerProductSpecificationIsEnable !== "boolean") {
    sysPrdSpecificationAddValidObj.managerProductSpecificationIsEnable = false;
    isValid = false;
  } else {
    sysPrdSpecificationAddValidObj.managerProductSpecificationIsEnable = true;
  }

  return isValid;
};

//#endregion

//#region API / 資料流程
/** 新增產品 */
const submitProduct = async () => {
  // 檢查token
  if (!requireToken()) return;
  // 驗證欄位
  if (!validateProductField()) return;
  // 設定產品規格
  sysPrdAddViewObj.managerProductSpecificationList = productSpecificationList.value;

  // 呼叫api
  const requestData: MbsSysPrdHttpAddReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerProductName: sysPrdAddViewObj.managerProductName,
    managerProductMainKindID: sysPrdAddViewObj.managerProductMainKindID,
    managerProductSubKindID: sysPrdAddViewObj.managerProductSubKindID,
    managerProductKind: sysPrdAddViewObj.managerProductKind,
    managerProductIsKey: sysPrdAddViewObj.managerProductIsKey,
    managerProductRemark: sysPrdAddViewObj.managerProductRemark,
    managerProductIsEnable: sysPrdAddViewObj.managerProductIsEnable,
    managerProductSpecificationList: sysPrdAddViewObj.managerProductSpecificationList.map(
      (item: MbsSysPrdHttpAddSpecificationReqItemMdl) =>
        ({
          managerProductSpecificationName: item.managerProductSpecificationName,
          managerProductSpecificationSellPrice: item.managerProductSpecificationSellPrice,
          managerProductSpecificationCostPrice: item.managerProductSpecificationCostPrice,
          managerProductSpecificationIsEnable: item.managerProductSpecificationIsEnable,
        }) satisfies MbsSysPrdHttpAddSpecificationReqItemMdl
    ) as MbsSysPrdHttpAddSpecificationReqMdl[],
  };
  const responseData: MbsSysPrdHttpAddRspMdl | null = await addProduct(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增產品成功！");
  // 導向產品詳細頁
  setTimeout(() => {
    router.push(`/system/product/detail/${responseData.managerProductID}`);
  }, 1500);
};
//#endregion

//#region 按鈕事件
/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  await submitProduct();
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  router.push("/system/product");
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/product");
};

// -----------------------------------------------------
/** 點擊新增【產品規格】按鈕 */
const clickAddProductSpecificationBtn = () => {
  productSpecificationIndex.value = null;
  Object.assign(sysPrdSpecificationAddViewObj, {
    managerProductSpecificationName: "",
    managerProductSpecificationSellPrice: 0,
    managerProductSpecificationCostPrice: 0,
    managerProductSpecificationIsEnable: true,
  });
  sysPrdSpecificationAddModalShow.value = true;
};

/** 點擊編輯【產品規格】按鈕 */
const clickUpdateProductSpecificationBtn = (index: number) => {
  productSpecificationIndex.value = index;
  Object.assign(sysPrdSpecificationAddViewObj, { ...productSpecificationList.value[index] });
  sysPrdSpecificationAddModalShow.value = true;
};

/** 點擊送出【產品規格】按鈕 */
const clickSubmitProductSpecificationBtn = () => {
  // 驗證欄位
  if (!validateSpecificationField()) return;

  const objCopy = { ...sysPrdSpecificationAddViewObj };

  if (productSpecificationIndex.value !== null) {
    productSpecificationList.value[productSpecificationIndex.value] = objCopy;
  } else {
    productSpecificationList.value.push(objCopy);
  }

  sysPrdSpecificationAddModalShow.value = false;
};

/** 點擊刪除【產品規格】按鈕 */
const clickDeleteProductSpecificationBtn = (index: number) => {
  productSpecificationList.value.splice(index, 1);
};

//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-4">
        <!-- 返回按鈕 -->
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
      <div class="text-xl font-bold text-brand-100">產品資訊</div>
      <!-- 產品名稱 -->
      <div>
        <label class="form-label">產品名稱 <span class="required-label">*</span></label>
        <input
          v-model="sysPrdAddViewObj.managerProductName"
          type="text"
          placeholder="請輸入產品名稱"
          class="input-box"
        />
        <p class="error-wrapper">
          <span v-if="!sysPrdAddValidObj.managerProductName" class="error-tip">
            不可為空，長度不可超過 30 個字
          </span>
        </p>
      </div>

      <div class="grid grid-cols-2 gap-4 w-full">
        <!-- 產品主分類 -->
        <div>
          <label class="form-label">產品主分類 <span class="required-label">*</span></label>
          <GetManyManagerProductMainKindComboBox
            v-model:managerProductMainKindID="sysPrdAddViewObj.managerProductMainKindID"
            v-model:managerProductMainKindName="sysPrdAddViewObj.managerProductMainKindName"
            :disabled="false"
          />
          <p class="error-wrapper">
            <span v-if="!sysPrdAddValidObj.managerProductMainKindID" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>

        <!-- 產品子分類 -->
        <div>
          <label class="form-label">產品子分類 <span class="required-label">*</span></label>
          <GetManyManagerProductSubKindComboBox
            v-model:managerProductMainKindID="sysPrdAddViewObj.managerProductMainKindID"
            v-model:managerProductSubKindID="sysPrdAddViewObj.managerProductSubKindID"
            v-model:managerProductSubKindName="sysPrdAddViewObj.managerProductSubKindName"
            :disabled="false"
          />
          <p class="error-wrapper">
            <span v-if="!sysPrdAddValidObj.managerProductSubKindID" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>
      </div>

      <div class="flex gap-4 w-full">
        <!-- 類型 -->
        <div class="flex-1">
          <label class="form-label"> 類型 <span class="required-label">*</span> </label>
          <div class="flex gap-8">
            <label>
              <input
                v-model="sysPrdAddViewObj.managerProductKind"
                type="radio"
                :value="DbManagerProductKindEnum.Equipment"
              />
              設備
            </label>

            <label>
              <input
                v-model="sysPrdAddViewObj.managerProductKind"
                type="radio"
                :value="DbManagerProductKindEnum.Service"
              />
              服務
            </label>
          </div>
          <p class="error-wrapper"></p>
        </div>

        <!-- 是否為主力產品 -->
        <div class="flex-1">
          <label class="form-label">是否為主力產品 <span class="required-label">*</span></label>
          <div class="flex gap-8">
            <label>
              <input v-model="sysPrdAddViewObj.managerProductIsKey" type="radio" :value="true" />
              是
            </label>
            <label>
              <input v-model="sysPrdAddViewObj.managerProductIsKey" type="radio" :value="false" />
              否
            </label>
          </div>
          <p class="error-wrapper"></p>
        </div>
      </div>

      <!-- 狀態 -->
      <div class="w-48">
        <label class="form-label">狀態 <span class="required-label">*</span></label>
        <select v-model="sysPrdAddViewObj.managerProductIsEnable" class="select-box">
          <option :value="true">啟用</option>
          <option :value="false">停用</option>
        </select>
        <p class="error-wrapper"></p>
      </div>

      <!-- 備註 -->
      <div class="flex flex-col gap-2">
        <label class="form-label">備註 </label>
        <textarea
          v-model="sysPrdAddViewObj.managerProductRemark"
          type="text"
          rows="5"
          maxlength="500"
          placeholder="請輸入備註"
          class="textarea-box resize-none"
        ></textarea>
        <div>
          <TextCounter
            :text="sysPrdAddViewObj.managerProductRemark"
            :max-length="500"
            :required="false"
          />
        </div>
      </div>
    </div>

    <!-- 產品規格區塊 -->
    <div class="flex flex-col gap-2 p-8 bg-white rounded-lg">
      <div class="flex items-center justify-between">
        <div class="subtitle">產品規格</div>
        <button
          v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-append"
          @click="clickAddProductSpecificationBtn"
        >
          新增規格
        </button>
      </div>

      <p class="error-wrapper">
        <span v-if="!sysPrdAddValidObj.managerProductSpecificationList" class="error-tip">
          最少新增一筆規格
        </span>
      </p>
      <!-- 產品規格列表 -->
      <div v-if="productSpecificationList.length > 0">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start">規格名稱</th>
              <th class="text-end">售價</th>
              <th class="text-end">成本</th>
              <th class="text-center">狀態</th>
              <th v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')" class="text-center">
                操作
              </th>
            </tr>
          </thead>

          <tbody>
            <template v-if="productSpecificationList.length === 0">
              <tr v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')" class="text-center">
                <td colspan="5">無資料</td>
              </tr>
              <tr v-else class="text-center">
                <td colspan="4">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="(item, index) in productSpecificationList"
                :key="index"
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
                <td class="text-center">
                  <button
                    class="btn-update me-1"
                    @click="clickUpdateProductSpecificationBtn(index)"
                  >
                    編輯
                  </button>
                  <button class="btn-delete" @click="clickDeleteProductSpecificationBtn(index)">
                    刪除
                  </button>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>
    </div>

    <div>
      <div class="flex justify-center mt-3 gap-2">
        <button class="btn-cancel" @click="clickCancelBtn">取消</button>
        <button
          v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-submit"
          @click="clickSubmitBtn"
        >
          送出
        </button>
      </div>
    </div>
  </div>

  <!-- 新增規格 Modal -->
  <div v-if="sysPrdSpecificationAddModalShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">
          {{ productSpecificationIndex !== null ? "編輯規格" : "新增規格" }}
        </h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysPrdSpecificationAddModalShow = false"
        >
          x
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 規格名稱 -->
          <div>
            <label class="form-label">規格名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysPrdSpecificationAddViewObj.managerProductSpecificationName"
              class="input-box"
              type="text"
              placeholder="請輸入規格名稱"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysPrdSpecificationAddValidObj.managerProductSpecificationName"
                class="error-tip"
              >
                不可為空，長度不可超過 100 個字
              </span>
            </p>
          </div>

          <!-- 售價和成本 -->
          <div class="flex gap-4 w-full">
            <!-- 售價 -->
            <div class="flex-1">
              <label class="form-label">售價 <span class="required-label">*</span></label>
              <input
                v-model="sysPrdSpecificationAddViewObj.managerProductSpecificationSellPrice"
                class="input-box"
                type="number"
                placeholder="請輸入數字"
              />

              <p class="error-wrapper">
                <span
                  v-if="!sysPrdSpecificationAddValidObj.managerProductSpecificationSellPrice"
                  class="error-tip"
                >
                  不可為空
                </span>
              </p>
            </div>

            <!-- 成本 -->
            <div class="flex-1">
              <label class="form-label">成本 <span class="required-label">*</span></label>
              <input
                v-model="sysPrdSpecificationAddViewObj.managerProductSpecificationCostPrice"
                class="input-box"
                type="number"
                placeholder="請輸入成本"
              />
              <p class="error-wrapper">
                <span
                  v-if="!sysPrdSpecificationAddValidObj.managerProductSpecificationCostPrice"
                  class="error-tip"
                >
                  不可為空
                </span>
              </p>
            </div>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <div class="select-wrapper">
              <select
                v-model="sysPrdSpecificationAddViewObj.managerProductSpecificationIsEnable"
                class="select-box"
              >
                <option :value="true">啟用</option>
                <option :value="false">停用</option>
              </select>
            </div>
          </div>
          <p class="error-wrapper"></p>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="sysPrdSpecificationAddModalShow = false">取消</button>
          <button class="btn-submit" @click="clickSubmitProductSpecificationBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
