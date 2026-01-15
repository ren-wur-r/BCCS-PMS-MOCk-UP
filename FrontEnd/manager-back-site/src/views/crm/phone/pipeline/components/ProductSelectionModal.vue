<script setup lang="ts">
import { ref, reactive, watch, computed } from "vue";
import GetManyManagerProductMainKindComboBox from "@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue";
import GetManyManagerProductSubKindComboBox from "@/components/feature/search-bar/GetManyManagerProductSubKindComboBox.vue";
import GetManyManagerProductComboBox from "@/components/feature/search-bar/GetManyManagerProductComboBox.vue";

/** 產品篩選條件模型 */
interface ProductFilterMdl {
  mainKindID: number | null;
  mainKindName: string | null;
  subKindID: number | null;
  subKindName: string | null;
  productID: number | null;
  productName: string | null;
}

/** 產品項目資料 */
interface ProductItem {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductSubKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerActivityProductID?: number;
}

/** 從產品下拉選單回傳的資料結構 */
interface ProductComboBoxResult {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
}
//--------------------------------------------------------
// 篩選條件
const productFilterObj = reactive<ProductFilterMdl>({
  mainKindID: null,
  mainKindName: null,
  subKindID: null,
  subKindName: null,
  productID: null,
  productName: null,
});

// 選中的產品
const selectedProduct = ref<ProductItem | null>(null);

// 標誌：是否為程式自動更新（避免清除已選擇的產品）
// const isAutoUpdating = ref(false);

// 計算屬性：模態框標題
const modalTitle = computed(() => {
  return props.mode === "edit" ? "編輯產品" : "選擇產品";
});
// --------------------------------------------------------
interface Props {
  show: boolean;
  mode: "add" | "edit";
  defaultProduct?: ProductItem | null;
}

interface Emits {
  (e: "close"): void;
  (e: "confirm", product: ProductItem): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();
// --------------------------------------------------------
/** 重置篩選條件 */
const resetFilterObj = () => {
  productFilterObj.mainKindID = null;
  productFilterObj.subKindID = null;
  productFilterObj.mainKindName = null;
  productFilterObj.subKindName = null;
  productFilterObj.productName = null;
  selectedProduct.value = null;
};

/** 設定編輯模式的預設值 */
const setEditDefaultValue = () => {
  if (props.mode === "edit" && props.defaultProduct) {
    const product = props.defaultProduct;

    // 設定篩選條件
    productFilterObj.mainKindName = product.managerProductMainKindName;
    productFilterObj.subKindName = product.managerProductSubKindName;
    productFilterObj.productName = product.managerProductName;
    productFilterObj.mainKindID = product.managerProductMainKindID;
    productFilterObj.subKindID = product.managerProductSubKindID;
    productFilterObj.productID = product.managerProductID;

    // 設定選中的產品
    selectedProduct.value = {
      managerProductID: product.managerProductID,
      managerProductName: product.managerProductName,
      managerProductMainKindID: product.managerProductMainKindID,
      managerProductSubKindID: product.managerProductSubKindID,
      managerProductMainKindName: product.managerProductMainKindName,
      managerProductSubKindName: product.managerProductSubKindName,
      managerActivityProductID: product.managerActivityProductID,
    };
  }
};

/** 處理主分類變更 */
const handleMainKindChange = () => {
  // 用戶手動更改主分類時，清除子分類和產品
  productFilterObj.subKindID = null;
  productFilterObj.subKindName = null;
  productFilterObj.productID = null;
  productFilterObj.productName = null;
  selectedProduct.value = null;
};

// 處理子分類變更
const handleSubKindChange = () => {
  // 用戶手動更改子分類時，清除產品
  productFilterObj.productID = null;
  productFilterObj.productName = null;
  selectedProduct.value = null;
};

/** 處理產品選擇 */
const handleProductSelected = (productData: ProductComboBoxResult) => {
  if (!productData) return;

  // 先設置產品資訊
  productFilterObj.productID = productData.managerProductID;
  productFilterObj.productName = productData.managerProductName;

  // 設置自動更新標誌，防止回填時觸發清除邏輯
  // isAutoUpdating.value = true;

  // 將產品的主分類和子分類資訊回填到篩選條件中
  productFilterObj.mainKindID = productData.managerProductMainKindID;
  productFilterObj.mainKindName = productData.managerProductMainKindName;

  // 設置自動更新標誌，防止子分類回填時觸發清除邏輯
  // isAutoUpdating.value = true;
  productFilterObj.subKindID = productData.managerProductSubKindID;
  productFilterObj.subKindName = productData.managerProductSubKindName;

  // 設定選中的產品
  selectedProduct.value = {
    managerProductID: productData.managerProductID,
    managerProductName: productData.managerProductName,
    managerProductMainKindID: productData.managerProductMainKindID,
    managerProductSubKindID: productData.managerProductSubKindID,
    managerProductMainKindName: productData.managerProductMainKindName,
    managerProductSubKindName: productData.managerProductSubKindName,
  };

  // 如果是編輯模式，保留原有的 managerActivityProductID
  if (props.mode === "edit" && props.defaultProduct?.managerActivityProductID) {
    selectedProduct.value.managerActivityProductID = props.defaultProduct.managerActivityProductID;
  }
};

/** 關閉彈跳視窗 */
const closeModal = () => {
  emit("close");
};

/**點擊【確認】按鈕 */
const clickConfirmBtn = () => {
  if (selectedProduct.value) {
    const productToReturn: ProductItem = {
      managerProductID: selectedProduct.value.managerProductID,
      managerProductName: selectedProduct.value.managerProductName,
      managerProductMainKindID: selectedProduct.value.managerProductMainKindID,
      managerProductSubKindID: selectedProduct.value.managerProductSubKindID,
      managerProductMainKindName: selectedProduct.value.managerProductMainKindName,
      managerProductSubKindName: selectedProduct.value.managerProductSubKindName,
    };

    // 如果是編輯模式，保留 managerActivityProductID
    if (props.mode === "edit" && props.defaultProduct?.managerActivityProductID) {
      productToReturn.managerActivityProductID = props.defaultProduct.managerActivityProductID;
    }

    emit("confirm", productToReturn);
  }
};

// 監聽 show 變化
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      resetFilterObj();
      setEditDefaultValue();
    }
  }
);
</script>

<template>
  <div
    v-if="show"
    class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
  >
    <div class="bg-white rounded-lg p-6 w-full max-w-2xl">
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-xl font-bold">{{ modalTitle }}</h2>
        <button class="text-gray-500 hover:text-gray-700" @click="closeModal">✕</button>
      </div>

      <!-- 編輯模式顯示當前選中的產品 -->
      <div class="mb-4 p-4 bg-gray-50 rounded">
        <p class="text-sm text-gray-600">
          {{ selectedProduct?.managerProductMainKindName || "未選擇主分類" }} >
          {{ selectedProduct?.managerProductSubKindName || "未選擇子分類" }} >
          {{ selectedProduct?.managerProductName || "未選擇產品" }}
        </p>
      </div>

      <!-- 查詢條件區域 -->
      <div class="flex flex-col gap-4 mb-6">
        <!-- 主分類 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">產品主分類 <span class="required-label">*</span></label>
          <GetManyManagerProductMainKindComboBox
            v-model:managerProductMainKindID="productFilterObj.mainKindID"
            v-model:managerProductMainKindName="productFilterObj.mainKindName"
            :disabled="false"
            @update:manager-product-main-kind-id="handleMainKindChange"
          />
        </div>

        <!-- 子分類 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">產品子分類 <span class="required-label">*</span></label>
          <GetManyManagerProductSubKindComboBox
            v-model:managerProductSubKindID="productFilterObj.subKindID"
            v-model:managerProductSubKindName="productFilterObj.subKindName"
            :managerProductMainKindID="productFilterObj.mainKindID"
            :disabled="false"
            :clear-search="true"
            @update:manager-product-sub-kind-id="handleSubKindChange"
          />
        </div>

        <!-- 產品名稱 -->
        <div class="flex flex-col gap-2">
          <label class="form-label">產品名稱 <span class="required-label">*</span></label>
          <GetManyManagerProductComboBox
            v-model:managerProductName="productFilterObj.productName"
            :managerProductMainKindID="productFilterObj.mainKindID"
            :managerProductSubKindID="productFilterObj.subKindID"
            :preventAutoReset="true"
            :disabled="false"
            @product-selected="handleProductSelected"
          />
        </div>
      </div>

      <!-- 按鈕區域 -->
      <div class="flex justify-end gap-2">
        <button class="btn-cancel" @click="closeModal">取消</button>
        <button class="btn-save" @click="clickConfirmBtn">確定</button>
      </div>
    </div>
  </div>
</template>
