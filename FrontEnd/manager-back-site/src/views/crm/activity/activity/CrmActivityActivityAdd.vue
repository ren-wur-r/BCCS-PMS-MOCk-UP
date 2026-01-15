<script setup lang="ts">
//#region 引入
import { ref, reactive, computed } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useRouter } from "vue-router";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import type {
  MbsCrmActHttpAddActivityReqMdl,
  MbsCrmActHttpAddActivityRspMdl,
} from "@/services/pms-http/crm/activity/crmActivityHttpFormat";
import { addActivity } from "@/services";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ProductListTable from "./components/ProductListTable.vue";
import ProductSelectionModal from "./components/ProductSelectionModal.vue";
import TextCounter from "@/components/global/counter/TextCounter.vue";
import { formatToServerDatetime } from "@/utils/timeFormatter";
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
//#endregion

//#region 型別定義
/** CRM-活動管理-活動-新增-頁面模型 */
interface CrmActivityActivityAddViewMdl {
  managerActivityName: string | null;
  managerActivityKind: DbAtomManagerActivityKindEnum | null;
  managerActivityStartTime: string | null;
  managerActivityEndTime: string | null;
  managerActivityLocation: string | null;
  managerActivityExpectedLeadCount: number | null;
  managerActivityContent: string | null;
  managerActivityProductList: CrmActivityActivityProductListMdl[];
}
/** CRM-活動管理-活動-新增-產品項目模型 */
interface CrmActivityActivityProductListMdl {
  managerProductID: number;
}

/** CRM-活動管理-活動-新增-驗證模型 */
interface ValidateMdl {
  managerActivityName: boolean;
  managerActivityKind: boolean;
  managerActivityStartTime: boolean;
  managerActivityEndTime: boolean;
  managerActivityLocation: boolean;
  managerActivityExpectedLeadCount: boolean;
  managerActivityContent: boolean;
  managerActivityProductList: boolean;
  managerActivitySponsorList: boolean;
  managerActivityExpenseList: boolean;
}

/** 產品項目模型 */
interface ProductItem {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmActivity;
/** 產品列表(暫存) */
const productList = ref<ProductItem[]>([]);
/** [產品彈跳視窗]狀態 */
const productModalState = ref({
  show: false,
  mode: "add" as "add" | "edit",
  editingIndex: null as number | null,
  editingProduct: null as ProductItem | null,
});

/** CRM-活動管理-活動-新增-頁面物件 */
const crmActivityActivityAddViewObj = reactive<CrmActivityActivityAddViewMdl>({
  managerActivityName: null,
  managerActivityKind: DbAtomManagerActivityKindEnum.PhysicalActivity,
  managerActivityStartTime: null,
  managerActivityEndTime: null,
  managerActivityLocation: null,
  managerActivityExpectedLeadCount: 0,
  managerActivityContent: null,
  managerActivityProductList: [],
});

/** CRM-活動管理-活動-新增-驗證物件 */
const validateObj = reactive<ValidateMdl>({
  managerActivityName: true,
  managerActivityKind: true,
  managerActivityStartTime: true,
  managerActivityEndTime: true,
  managerActivityLocation: true,
  managerActivityExpectedLeadCount: true,
  managerActivityContent: true,
  managerActivityProductList: true,
  managerActivitySponsorList: true,
  managerActivityExpenseList: true,
});
//#endregion

//#region UI行為
/** 選擇活動類型 */
const selectActivityKind = (kind: DbAtomManagerActivityKindEnum) => {
  crmActivityActivityAddViewObj.managerActivityKind = kind;

  // 根據活動類型設定地點
  if (kind === DbAtomManagerActivityKindEnum.OnlineActivity) {
    crmActivityActivityAddViewObj.managerActivityLocation = "On Air";
  } else {
    crmActivityActivityAddViewObj.managerActivityLocation = null;
  }
};

/** 計算地點是否為只讀 */
const isLocationReadonly = computed(() => {
  return (
    crmActivityActivityAddViewObj.managerActivityKind ===
    DbAtomManagerActivityKindEnum.OnlineActivity
  );
});
//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;
  // 活動名稱
  if (
    !crmActivityActivityAddViewObj.managerActivityName ||
    crmActivityActivityAddViewObj.managerActivityName.trim() === ""
  ) {
    validateObj.managerActivityName = false;
    isValid = false;
  } else {
    validateObj.managerActivityName = true;
  }

  // 活動類型
  if (!crmActivityActivityAddViewObj.managerActivityKind) {
    validateObj.managerActivityKind = false;
    isValid = false;
  } else {
    validateObj.managerActivityKind = true;
  }

  // 開始時間
  if (!crmActivityActivityAddViewObj.managerActivityStartTime) {
    validateObj.managerActivityStartTime = false;
    isValid = false;
  } else {
    validateObj.managerActivityStartTime = true;
  }
  // 結束時間
  if (!crmActivityActivityAddViewObj.managerActivityEndTime) {
    validateObj.managerActivityEndTime = false;
    isValid = false;
  } else {
    validateObj.managerActivityEndTime = true;
  }
  // 地點
  if (
    !crmActivityActivityAddViewObj.managerActivityLocation ||
    crmActivityActivityAddViewObj.managerActivityLocation.trim() === ""
  ) {
    validateObj.managerActivityLocation = false;
    isValid = false;
  } else {
    validateObj.managerActivityLocation = true;
  }

  // 期望名單數
  if (
    crmActivityActivityAddViewObj.managerActivityExpectedLeadCount === null ||
    crmActivityActivityAddViewObj.managerActivityExpectedLeadCount < 0
  ) {
    validateObj.managerActivityExpectedLeadCount = false;
    isValid = false;
  } else {
    validateObj.managerActivityExpectedLeadCount = true;
  }
  return isValid;
};
//#endregion

//#region API / 資料流程

//#endregion

//#region 按鈕事件
/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 欄位驗證
  if (!validateField()) return;

  // 呼叫 API
  const requestData: MbsCrmActHttpAddActivityReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityName: crmActivityActivityAddViewObj.managerActivityName!,
    managerActivityKind: crmActivityActivityAddViewObj.managerActivityKind!,
    managerActivityStartTime: formatToServerDatetime(
      crmActivityActivityAddViewObj.managerActivityStartTime!
    ),
    managerActivityEndTime: formatToServerDatetime(
      crmActivityActivityAddViewObj.managerActivityEndTime!
    ),
    managerActivityLocation: crmActivityActivityAddViewObj.managerActivityLocation!,
    managerActivityExpectedLeadCount:
      crmActivityActivityAddViewObj.managerActivityExpectedLeadCount!,
    managerActivityContent: crmActivityActivityAddViewObj.managerActivityContent!,
    managerActivityProductList: productList.value.map((p) => ({
      managerProductID: p.managerProductID,
    })),
    managerActivitySponsorList: [],
    managerActivityExpenseList: [],
  };
  const responseData: MbsCrmActHttpAddActivityRspMdl | null = await addActivity(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增活動成功！");

  // 導向詳細頁
  setTimeout(() => {
    router.push(`/crm/activity/activity/detail/${responseData.managerActivityID}`);
  }, 1500);
};

/** 點擊【取消】按鈕 */
const clickCancelCustomerCompanyBtn = () => {
  router.push("/crm/activity/activity");
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push(`/crm/activity/activity`);
};

/** 點擊【新增產品】按鈕 */
const clickProductAddBtn = () => {
  window.open("/ManagerBackSite/system/product/add", "_blank");
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
    editingProduct: product as ProductItem,
  };
};

/** [產品]刪除 */
const handleProductDelete = (index: number) => {
  if (confirm("確定要刪除此產品嗎？")) {
    productList.value.splice(index, 1);
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

/** [產品彈跳視窗]確認 */
const handleProductModalConfirm = (selectedProduct: ProductItem) => {
  const productItem: ProductItem = {
    managerProductID: selectedProduct.managerProductID,
    managerProductName: selectedProduct.managerProductName,
    managerProductMainKindID: selectedProduct.managerProductMainKindID,
    managerProductSubKindID: selectedProduct.managerProductSubKindID,
    managerProductMainKindName: selectedProduct.managerProductMainKindName,
    managerProductSubKindName: selectedProduct.managerProductSubKindName,
  };

  if (productModalState.value.mode === "edit" && productModalState.value.editingIndex !== null) {
    productList.value[productModalState.value.editingIndex] = productItem;
  } else {
    const isDuplicate = productList.value.some(
      (p) => p.managerProductID === productItem.managerProductID
    );
    if (isDuplicate) {
      showError.value = true;
      errorMessage.value = `「${selectedProduct.managerProductName}」已存在於列表中，無法重複添加`;
      return;
    }
    productList.value.push(productItem);
  }

  handleProductModalClose();
};
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

    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="subtitle">活動資訊</div>

      <div class="flex flex-col gap-4">
        <div class="flex flex-col gap-4">
          <!-- 活動類型 -->
          <div>
            <label class="form-label">活動類型 <span class="required-label">*</span></label>
            <div class="flex gap-2">
              <button
                type="button"
                :class="[
                  crmActivityActivityAddViewObj.managerActivityKind ===
                  DbAtomManagerActivityKindEnum.PhysicalActivity
                    ? 'btn-activity-type-selected'
                    : 'btn-activity-type-unselected',
                ]"
                @click="selectActivityKind(DbAtomManagerActivityKindEnum.PhysicalActivity)"
              >
                實體活動
              </button>
              <button
                type="button"
                :class="[
                  crmActivityActivityAddViewObj.managerActivityKind ===
                  DbAtomManagerActivityKindEnum.OnlineActivity
                    ? 'btn-activity-type-selected'
                    : 'btn-activity-type-unselected',
                ]"
                @click="selectActivityKind(DbAtomManagerActivityKindEnum.OnlineActivity)"
              >
                線上活動
              </button>
            </div>
            <div class="error-wrapper"></div>
          </div>
        </div>

        <!-- 活動名稱 -->
        <div>
          <label class="form-label">活動名稱 <span class="required-label">*</span></label>
          <input
            v-model="crmActivityActivityAddViewObj.managerActivityName"
            type="text"
            placeholder="請輸入活動名稱"
            class="input-box"
          />
          <div class="error-wrapper">
            <p v-if="!validateObj.managerActivityName" class="error-tip">
              不可為空，長度不可超過 20 個字
            </p>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <!-- 開始時間 -->
          <div>
            <label class="form-label">開始時間 <span class="required-label">*</span></label>
            <input
              v-model="crmActivityActivityAddViewObj.managerActivityStartTime"
              type="datetime-local"
              class="input-box"
            />
            <div class="error-wrapper">
              <p v-if="!validateObj.managerActivityStartTime" class="error-tip">不可為空</p>
            </div>
          </div>

          <!-- 結束時間 -->
          <div>
            <label class="form-label">結束時間 <span class="required-label">*</span></label>
            <input
              v-model="crmActivityActivityAddViewObj.managerActivityEndTime"
              type="datetime-local"
              class="input-box"
            />
            <div class="error-wrapper">
              <p v-if="!validateObj.managerActivityEndTime" class="error-tip">不可為空</p>
            </div>
          </div>
        </div>

        <!-- 地點 -->
        <div>
          <label class="form-label"> 地點 <span class="required-label">*</span> </label>
          <input
            v-model="crmActivityActivityAddViewObj.managerActivityLocation"
            type="text"
            :placeholder="isLocationReadonly ? 'On Air' : '請輸入地點'"
            :readonly="isLocationReadonly"
            :class="['input-box', isLocationReadonly ? 'bg-gray-100 text-gray-500' : '']"
          />
          <div class="error-wrapper">
            <p v-if="!validateObj.managerActivityLocation" class="error-tip">
              不可為空，長度不可超過 50 個字
            </p>
          </div>
        </div>

        <!-- 期望名單數 -->
        <div class="w-1/4">
          <label class="form-label">期望名單數 <span class="required-label">*</span></label>
          <div class="select-wrapper">
            <input
              v-model="crmActivityActivityAddViewObj.managerActivityExpectedLeadCount"
              type="number"
              min="0"
              class="input-box"
            />
          </div>
          <div class="error-wrapper">
            <p v-if="!validateObj.managerActivityExpectedLeadCount" class="error-tip">不可為空</p>
          </div>
        </div>

        <!-- 活動內容與備註 -->
        <div>
          <label class="form-label">活動內容與備註</label>
          <div class="select-wrapper">
            <textarea
              v-model="crmActivityActivityAddViewObj.managerActivityContent"
              type="text"
              class="textarea-box"
              placeholder="請輸入活動內容與備註"
              rows="15"
            ></textarea>
          </div>
          <div>
            <TextCounter
              v-model:is-valid="validateObj.managerActivityContent"
              :text="crmActivityActivityAddViewObj.managerActivityContent"
              :max-length="2000"
              :required="true"
            />
          </div>
        </div>
      </div>
    </div>

    <!-- 活動產品區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between items-center">
        <h2 class="subtitle">活動產品</h2>
        <button class="btn-add" @click="clickProductAddBtn">新增產品</button>
      </div>

      <ProductListTable
        :productList="productList"
        :show-action-row="true"
        :showAppendBtn="true"
        @append="handleProductAppend"
        @edit="handleProductEdit"
        @delete="handleProductDelete"
      />
    </div>

    <div>
      <div class="flex justify-center mt-3 gap-2">
        <button class="btn-cancel" @click="clickCancelCustomerCompanyBtn">取消</button>
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
</template>
