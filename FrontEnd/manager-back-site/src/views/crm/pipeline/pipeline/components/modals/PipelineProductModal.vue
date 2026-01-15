<script setup lang="ts">
import { ref, watch, computed } from "vue";
import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";
import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
import GetManyManagerProductComboBox from "@/components/feature/search-bar/GetManyManagerProductComboBox.vue";
import GetManyManagerProductSpecificationComboBox from "@/components/feature/search-bar/GetManyManagerProductSpecificationComboBox.vue";

interface ProductData {
  employeePipelineProductID: number | null;
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerProductSpecificationID: number;
  managerProductSpecificationName: string;
  employeePipelineProductSellPrice: number;
  employeePipelineProductClosingPrice: number;
  employeePipelineProductCostPrice: number;
  employeePipelineProductGrossProfit: number;
  employeePipelineProductCount: number;
  employeePipelineProductPurchaseKind: DbAtomEmployeePipelineProductPurchaseKindEnum;
  employeePipelineProductContractKind: DbAtomEmployeePipelineProductContractKindEnum;
  employeePipelineProductContractText?: string;
  managerProductMainKindCommissionRate: number;
}

/** 產品選擇事件回傳的資料模型 */
interface ProductSelectedData {
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string;
  /** 管理者產品主分類ID */
  managerProductMainKindID: number;
  /** 管理者產品主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品子分類ID */
  managerProductSubKindID: number;
  /** 管理者產品子分類名稱 */
  managerProductSubKindName: string;
  /** 管理者產品主分類佣金率 */
  managerProductMainKindCommissionRate: number;
}

/** 產品規格選擇事件回傳的資料模型 */
interface SpecificationSelectedData {
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 管理者產品規格名稱 */
  managerProductSpecificationName: string;
  /** 管理者產品規格售價 */
  managerProductSpecificationSellPrice: number;
  /** 管理者產品規格成本價 */
  managerProductSpecificationCostPrice: number;
}

interface Props {
  show: boolean;
  /** 編輯模式時傳入的產品資料 */
  product?: ProductData | null;
}

interface Emits {
  (
    e: "confirm",
    data: {
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
      // 額外的顯示資訊（給新增頁面用）
      managerProductName?: string;
      managerProductMainKindName?: string;
      managerProductSubKindName?: string;
      managerProductSpecificationName?: string;
      managerProductMainKindCommissionRate?: number;
    }
  ): void;
  (e: "cancel"): void;
}

/** 產品資料驗證模型 */
interface ProductDataValidationMdl {
  managerProductID: boolean;
  managerProductSpecificationID: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  product: null,
});

const emit = defineEmits<Emits>();

/** 產品資料驗證物件 */
const productDataValidationObj = ref<ProductDataValidationMdl>({
  managerProductID: true,
  managerProductSpecificationID: true,
});

/** 是否為編輯模式 */
const isEditMode = computed(() => !!props.product);

/** 產品主分類ID */
const managerProductMainKindID = ref<number | null>(null);
const managerProductMainKindName = ref<string | null>(null);

/** 產品子分類ID */
const managerProductSubKindID = ref<number | null>(null);
const managerProductSubKindName = ref<string | null>(null);

/** 產品ID */
const managerProductID = ref<number | null>(null);
const managerProductName = ref<string | null>(null);

/** 產品規格ID */
const managerProductSpecificationID = ref<number | null>(null);
const managerProductSpecificationName = ref<string | null>(null);

/** 產品主分類佣金率 */
const managerProductMainKindCommissionRate = ref<number>(0);

/** 售價 */
const employeePipelineProductSellPrice = ref<number>(0);

/** 成交價 */
const employeePipelineProductClosingPrice = ref<number>(0);

/** 成本 */
const employeePipelineProductCostPrice = ref<number>(0);

/** 數量 */
const employeePipelineProductCount = ref<number>(1);

/** 新購/續約 */
const employeePipelineProductPurchaseKindID = ref<DbAtomEmployeePipelineProductPurchaseKindEnum>(
  DbAtomEmployeePipelineProductPurchaseKindEnum.NewlyPurchase
);

/** 採購方式 */
const employeePipelineProductContractKindID = ref<DbAtomEmployeePipelineProductContractKindEnum>(
  DbAtomEmployeePipelineProductContractKindEnum.JointTendering
);

/** 合約內容 */
const employeePipelineProductContractText = ref("");

/** 計算毛利 */
const grossProfit = computed(() => {
  return employeePipelineProductClosingPrice.value - employeePipelineProductCostPrice.value;
});

/** 計算業務毛利率 */
const salesGrossProfitRate = computed(() => {
  return managerProductMainKindCommissionRate.value.toFixed(2);
});

/** 計算業務毛利 */
const salesGrossProfit = computed(() => {
  const profit = grossProfit.value * managerProductMainKindCommissionRate.value;
  return profit.toFixed(2);
});
/** 處理產品選擇 */
const handleProductSelected = (product: ProductSelectedData) => {
  // 設定所有相關欄位
  managerProductMainKindID.value = product.managerProductMainKindID;
  managerProductMainKindName.value = product.managerProductMainKindName;
  managerProductSubKindID.value = product.managerProductSubKindID;
  managerProductSubKindName.value = product.managerProductSubKindName;
  managerProductID.value = product.managerProductID;
  managerProductName.value = product.managerProductName;
  managerProductMainKindCommissionRate.value = product.managerProductMainKindCommissionRate;

  // 清空規格選擇
  managerProductSpecificationID.value = null;
  managerProductSpecificationName.value = null;

  // 清空價格資訊
  employeePipelineProductSellPrice.value = 0;
  employeePipelineProductClosingPrice.value = 0;
  employeePipelineProductCostPrice.value = 0;
};

/** 處理規格選擇 */
const handleSpecificationSelected = (specification: SpecificationSelectedData) => {
  employeePipelineProductSellPrice.value = specification.managerProductSpecificationSellPrice;
  employeePipelineProductCostPrice.value = specification.managerProductSpecificationCostPrice;
  employeePipelineProductClosingPrice.value = specification.managerProductSpecificationSellPrice;
};

/** 是否顯示採購文字輸入框 */
const showContractTextInput = computed(() => {
  return (
    employeePipelineProductContractKindID.value ===
    DbAtomEmployeePipelineProductContractKindEnum.Other
  );
});

/** 監聽採購方式變化 */
watch(
  () => employeePipelineProductContractKindID.value,
  (newValue) => {
    if (newValue !== DbAtomEmployeePipelineProductContractKindEnum.Other) {
      employeePipelineProductContractText.value = "";
    }
  }
);

/** 監聽主分類變化 */
watch(
  () => managerProductMainKindID.value,
  (newValue, oldValue) => {
    if (oldValue !== null && newValue !== oldValue) {
      managerProductSubKindID.value = null;
      managerProductSubKindName.value = null;
      managerProductID.value = null;
      managerProductName.value = null;
      managerProductSpecificationID.value = null;
      managerProductSpecificationName.value = null;
      managerProductMainKindCommissionRate.value = 0;
      employeePipelineProductSellPrice.value = 0;
      employeePipelineProductClosingPrice.value = 0;
      employeePipelineProductCostPrice.value = 0;
    }
  }
);

/** 監聽子分類變化 */
watch(
  () => managerProductSubKindID.value,
  (newValue, oldValue) => {
    if (oldValue !== null && newValue !== oldValue) {
      managerProductID.value = null;
      managerProductName.value = null;
      managerProductSpecificationID.value = null;
      managerProductSpecificationName.value = null;
      managerProductMainKindCommissionRate.value = 0;
      employeePipelineProductSellPrice.value = 0;
      employeePipelineProductClosingPrice.value = 0;
      employeePipelineProductCostPrice.value = 0;
    }
  }
);

/** 監聽產品變化 */
watch(
  () => managerProductID.value,
  (newValue, oldValue) => {
    if (oldValue !== null && newValue !== oldValue) {
      managerProductSpecificationID.value = null;
      managerProductSpecificationName.value = null;
      employeePipelineProductSellPrice.value = 0;
      employeePipelineProductClosingPrice.value = 0;
      employeePipelineProductCostPrice.value = 0;
    }
  }
);

/** 載入產品資料（編輯模式） */
const loadProductData = async () => {
  if (!props.product) return;

  // 載入產品資訊（顯示用）
  managerProductMainKindName.value = props.product.managerProductMainKindName;
  managerProductSubKindName.value = props.product.managerProductSubKindName;
  managerProductName.value = props.product.managerProductName;
  managerProductSpecificationName.value = props.product.managerProductSpecificationName;

  // 載入產品 ID（用於選擇）
  managerProductID.value = props.product.managerProductID;
  managerProductSpecificationID.value = props.product.managerProductSpecificationID;

  // 載入價格資訊
  employeePipelineProductSellPrice.value = props.product.employeePipelineProductSellPrice;
  employeePipelineProductClosingPrice.value = props.product.employeePipelineProductClosingPrice;
  employeePipelineProductCostPrice.value = props.product.employeePipelineProductCostPrice;
  employeePipelineProductCount.value = props.product.employeePipelineProductCount;
  employeePipelineProductPurchaseKindID.value = props.product.employeePipelineProductPurchaseKind;
  employeePipelineProductContractKindID.value = props.product.employeePipelineProductContractKind;
  employeePipelineProductContractText.value =
    props.product.employeePipelineProductContractText || "";
  managerProductMainKindCommissionRate.value = props.product.managerProductMainKindCommissionRate;
};

/** 重置表單 */
const resetForm = () => {
  managerProductMainKindID.value = null;
  managerProductMainKindName.value = null;
  managerProductSubKindID.value = null;
  managerProductSubKindName.value = null;
  managerProductID.value = null;
  managerProductName.value = null;
  managerProductSpecificationID.value = null;
  managerProductSpecificationName.value = null;
  managerProductMainKindCommissionRate.value = 0;
  employeePipelineProductSellPrice.value = 0;
  employeePipelineProductClosingPrice.value = 0;
  employeePipelineProductCostPrice.value = 0;
  employeePipelineProductCount.value = 1;
  employeePipelineProductPurchaseKindID.value =
    DbAtomEmployeePipelineProductPurchaseKindEnum.NewlyPurchase;
  employeePipelineProductContractKindID.value =
    DbAtomEmployeePipelineProductContractKindEnum.JointTendering;
  employeePipelineProductContractText.value = "";
  productDataValidationObj.value = {
    managerProductID: true,
    managerProductSpecificationID: true,
  };
};

/** 監聽 show 變化 */
watch(
  () => props.show,
  async (newVal) => {
    if (newVal) {
      //  清驗證狀態
      productDataValidationObj.value = {
        managerProductID: true,
        managerProductSpecificationID: true,
      };

      if (props.product) {
        await loadProductData();
      } else {
        resetForm();
      }
    }
  }
);

/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 產品ID
  if (!managerProductID.value) {
    productDataValidationObj.value.managerProductID = false;
    isValid = false;
  } else {
    productDataValidationObj.value.managerProductID = true;
  }

  // 產品規格ID
  if (!managerProductSpecificationID.value) {
    productDataValidationObj.value.managerProductSpecificationID = false;
    isValid = false;
  } else {
    productDataValidationObj.value.managerProductSpecificationID = true;
  }

  return isValid;
};

/** 處理確認 */
const handleConfirm = () => {
  if (!validateField()) {
    return;
  }

  emit("confirm", {
    employeePipelineProductID: props.product?.employeePipelineProductID ?? null,
    managerProductID: managerProductID.value!,
    managerProductSpecificationID: managerProductSpecificationID.value!,
    employeePipelineProductSellPrice: employeePipelineProductSellPrice.value,
    employeePipelineProductClosingPrice: employeePipelineProductClosingPrice.value,
    employeePipelineProductCostPrice: employeePipelineProductCostPrice.value,
    employeePipelineProductCount: employeePipelineProductCount.value,
    employeePipelineProductPurchaseKindID: employeePipelineProductPurchaseKindID.value,
    employeePipelineProductContractKindID: employeePipelineProductContractKindID.value,
    employeePipelineProductContractText: employeePipelineProductContractText.value,
    // 額外的顯示資訊（給新增頁面用）
    managerProductName: managerProductName.value || "",
    managerProductMainKindName: managerProductMainKindName.value || "",
    managerProductSubKindName: managerProductSubKindName.value || "",
    managerProductSpecificationName: managerProductSpecificationName.value || "",
    managerProductMainKindCommissionRate: managerProductMainKindCommissionRate.value,
  });
};

/** 處理取消 */
const handleCancel = () => {
  emit("cancel");
};
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-2xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3">
        <h2 class="modal-title">{{ isEditMode ? "編輯產品" : "附加產品" }}</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <hr />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-6 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 產品分類選擇 -->
          <div class="flex gap-4">
            <div class="flex-1 flex flex-col gap-2">
              <label class="form-label">產品主分類</label>
              <GetManyManagerProductMainKindComboBox
                v-model:manager-product-main-kind-i-d="managerProductMainKindID"
                v-model:manager-product-main-kind-name="managerProductMainKindName"
                :disabled="false"
              />
            </div>

            <div class="flex-1 flex flex-col gap-2">
              <label class="form-label">產品子分類</label>
              <GetManyManagerProductSubKindComboBox
                v-model:manager-product-sub-kind-i-d="managerProductSubKindID"
                v-model:manager-product-sub-kind-name="managerProductSubKindName"
                :disabled="false"
                :manager-product-main-kind-i-d="managerProductMainKindID"
              />
            </div>
          </div>

          <hr />

          <!-- 產品選擇 -->
          <div class="flex gap-4">
            <div class="flex-1 flex flex-col gap-2">
              <label class="form-label">產品 <span class="required-label">*</span></label>
              <GetManyManagerProductComboBox
                v-model:manager-product-i-d="managerProductID"
                v-model:manager-product-name="managerProductName"
                :disabled="false"
                :manager-product-main-kind-i-d="managerProductMainKindID"
                :manager-product-sub-kind-i-d="managerProductSubKindID"
                @product-selected="handleProductSelected"
              />
              <div class="h-2">
                <p v-if="!productDataValidationObj.managerProductID" class="error-tip">
                  請選擇產品
                </p>
              </div>
            </div>

            <div class="flex-1 flex flex-col gap-2">
              <label class="form-label">產品規格 <span class="required-label">*</span></label>
              <GetManyManagerProductSpecificationComboBox
                v-model:manager-product-specification-i-d="managerProductSpecificationID"
                v-model:manager-product-specification-name="managerProductSpecificationName"
                :disabled="!managerProductID"
                :manager-product-i-d="managerProductID"
                @specification-selected="handleSpecificationSelected"
              />
              <div class="h-2">
                <p v-if="!productDataValidationObj.managerProductSpecificationID" class="error-tip">
                  請選擇產品規格
                </p>
              </div>
            </div>
          </div>

          <!-- 價格資訊 -->
          <div class="grid grid-cols-3 gap-4 border p-4 rounded-lg bg-gray-50">
            <div class="flex flex-col gap-2">
              <label class="form-label">售價</label>
              <input
                v-model.number="employeePipelineProductSellPrice"
                type="number"
                min="0"
                class="input-box"
              />
            </div>

            <div class="flex flex-col gap-2">
              <label class="form-label">成交價</label>
              <input
                v-model.number="employeePipelineProductClosingPrice"
                type="number"
                min="0"
                class="input-box"
              />
            </div>

            <div class="flex flex-col gap-2">
              <label class="form-label">成本</label>
              <input
                v-model.number="employeePipelineProductCostPrice"
                type="number"
                min="0"
                class="input-box"
              />
            </div>

            <div class="flex flex-col gap-2">
              <label class="form-label">毛利</label>
              <input :value="grossProfit" type="number" class="input-box" disabled />
            </div>

            <div class="flex flex-col gap-2">
              <label class="form-label">業務毛利率(%)</label>
              <input :value="salesGrossProfitRate" type="text" class="input-box" disabled />
            </div>

            <div class="flex flex-col gap-2">
              <label class="form-label">業務毛利</label>
              <input :value="salesGrossProfit" type="text" class="input-box" disabled />
            </div>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <!-- 數量 -->
            <div class="flex flex-col gap-2">
              <label class="form-label">數量</label>
              <input
                v-model.number="employeePipelineProductCount"
                type="number"
                min="1"
                class="input-box"
              />
            </div>

            <!-- 新購/續約 -->
            <div class="flex flex-col gap-2">
              <label class="form-label">新購/續約</label>
              <select v-model.number="employeePipelineProductPurchaseKindID" class="select-box">
                <option :value="DbAtomEmployeePipelineProductPurchaseKindEnum.NewlyPurchase">
                  新購
                </option>
                <option :value="DbAtomEmployeePipelineProductPurchaseKindEnum.Renewal">續約</option>
              </select>
            </div>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <!-- 採購方式 -->
            <div class="flex flex-col gap-2">
              <label class="form-label">採購方式</label>
              <select v-model.number="employeePipelineProductContractKindID" class="select-box">
                <option :value="DbAtomEmployeePipelineProductContractKindEnum.JointTendering">
                  共契
                </option>
                <option :value="DbAtomEmployeePipelineProductContractKindEnum.Tender">標案</option>
                <option :value="DbAtomEmployeePipelineProductContractKindEnum.Other">其他</option>
              </select>
            </div>

            <!-- 採購文字 - 只在選擇「其他」時顯示 -->
            <div v-if="showContractTextInput" class="flex flex-col gap-2">
              <label class="form-label">其他採購方式</label>
              <input
                v-model="employeePipelineProductContractText"
                class="input-box"
                placeholder="請輸入其他採購方式"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleConfirm">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
