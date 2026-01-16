<script setup lang="ts">
import { ref, computed } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomEmployeePipelineProductPurchaseKindEnum } from "@/constants/DbAtomEmployeePipelineProductPurchaseKindEnum";
import { DbAtomEmployeePipelineProductContractKindEnum } from "@/constants/DbAtomEmployeePipelineProductContractKindEnum";
import { getEmployeePipelineProductPurchaseKindLabel } from "@/utils/getEmployeePipelineProductPurchaseKindLabel";
import { getEmployeePipelineProductContractKindLabel } from "@/utils/getEmployeePipelineProductContractKindLabel";
import { formatCurrency } from "@/utils/currencyFormatter";

const VAT_RATE = 0.05;
const toTaxExcluded = (amount: number) => Math.round(amount / (1 + VAT_RATE));

const menu = DbAtomMenuEnum.CrmPipeline;
const employeeInfoStore = useEmployeeInfoStore();
const isExpanded = ref(true);
// -----------------------------------------------------------------
/** 商機產品項目 */
interface ProductItem {
  /** 商機產品ID（編輯時必填，新增時可選） */
  employeePipelineProductID: number | null;
  /** 管理者產品ID */
  managerProductID: number;
  /** 管理者產品名稱 */
  managerProductName: string;
  /** 管理者產品主分類名稱 */
  managerProductMainKindName: string;
  /** 管理者產品子分類名稱 */
  managerProductSubKindName: string;
  /** 管理者產品規格ID */
  managerProductSpecificationID: number;
  /** 管理者產品規格名稱 */
  managerProductSpecificationName: string;
  /** 商機產品售價 */
  employeePipelineProductSellPrice: number;
  /** 商機產品成交價 */
  employeePipelineProductClosingPrice: number;
  /** 商機產品成本 */
  employeePipelineProductCostPrice: number;
  /** 商機產品毛利 */
  employeePipelineProductGrossProfit: number;
  /** 商機產品數量 */
  employeePipelineProductCount: number;
  /** 商機產品新購/續約 */
  employeePipelineProductPurchaseKind: DbAtomEmployeePipelineProductPurchaseKindEnum;
  /** 商機產品採購方式 */
  employeePipelineProductContractKind: DbAtomEmployeePipelineProductContractKindEnum;
  /** 業務毛利率 */
  managerProductMainKindCommissionRate: number;
  /** 採購方式文字（當選擇「其他」時） */
  employeePipelineProductContractText?: string;
}

interface Props {
  /** 產品列表 */
  productList: ProductItem[];
  /** 是否為唯讀模式（不顯示任何操作按鈕） */
  readonly?: boolean;
}

interface Emits {
  /** 新增產品 */
  (e: "add-product"): void;
  /** 編輯產品 */
  (e: "edit-product", product: ProductItem, index: number): void;
  /** 刪除產品 */
  (e: "delete-product", productId: number | null, index: number): void;
}

const props = withDefaults(defineProps<Props>(), {
  readonly: true,
});

const emit = defineEmits<Emits>();

/** 計算總售價 */
const totalSellPrice = computed(() => {
  return props.productList.reduce(
    (sum, item) => sum + item.employeePipelineProductSellPrice * item.employeePipelineProductCount,
    0
  );
});

/** 計算總成交價 */
const totalClosingPrice = computed(() => {
  return props.productList.reduce(
    (sum, item) =>
      sum + item.employeePipelineProductClosingPrice * item.employeePipelineProductCount,
    0
  );
});

/** 計算總成本 */
const totalCostPrice = computed(() => {
  return props.productList.reduce(
    (sum, item) => sum + item.employeePipelineProductCostPrice * item.employeePipelineProductCount,
    0
  );
});

/** 計算總毛利 */
const totalGrossProfit = computed(() => {
  return totalClosingPrice.value - totalCostPrice.value;
});

/** 處理新增產品 */
const handleAddProduct = () => {
  emit("add-product");
};

/** 處理編輯產品 */
const handleEditProduct = (product: ProductItem, index: number) => {
  emit("edit-product", product, index);
};

/** 處理刪除產品 */
const handleDeleteProduct = (productId: number | null, index: number) => {
  emit("delete-product", productId, index);
};
</script>

<template>
  <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
    <div class="h-8 flex justify-between items-center">
      <div class="flex items-center gap-2 cursor-pointer" @click="isExpanded = !isExpanded">
        <h2 class="subtitle">產品</h2>
        <!-- 展開/收合圖示 -->
        <svg
          class="w-5 h-5 transition-transform"
          :class="{ 'rotate-180': !isExpanded }"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M19 9l-7 7-7-7"
          />
        </svg>
      </div>
      <span v-if="!readonly && employeeInfoStore.hasPermission(menu, 'create') && isExpanded"></span>
    </div>

    <!-- 使用 v-show 控制內容顯示 -->
    <div v-show="isExpanded" class="flex flex-col gap-4">
      <!-- 總計資訊 -->
      <div class="grid grid-cols-4 gap-4 p-4 bg-gray-50 rounded-lg">
        <div class="flex flex-col gap-1">
          <span class="text-sm text-gray-600">總售價</span>
          <span class="text-lg font-semibold text-gray-800">{{
            formatCurrency(toTaxExcluded(totalSellPrice))
          }}</span>
        </div>
        <div class="flex flex-col gap-1">
          <span class="text-sm text-gray-600">總成交價（未稅）</span>
          <span class="text-lg font-semibold text-gray-800">{{
            formatCurrency(toTaxExcluded(totalClosingPrice))
          }}</span>
        </div>
        <div class="flex flex-col gap-1">
          <span class="text-sm text-gray-600">總成本</span>
          <span class="text-lg font-semibold text-gray-800">{{
            formatCurrency(toTaxExcluded(totalCostPrice))
          }}</span>
        </div>
        <div class="flex flex-col gap-1">
          <span class="text-sm text-gray-600">總毛利</span>
          <span class="text-lg font-semibold text-green-600">{{
            formatCurrency(toTaxExcluded(totalGrossProfit))
          }}</span>
        </div>
      </div>

      <div v-if="productList.length === 0" class="py-4">
        <div v-if="readonly" class="text-gray-400 text-center">無產品資料</div>
        <div v-else></div>
      </div>

      <div v-else class="overflow-x-auto">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start w-32">產品名稱</th>
              <th class="text-start w-32">產品主分類</th>
              <th class="text-start w-32">產品子分類</th>
              <th class="text-start w-32">產品規格</th>
              <th class="text-end w-24">售價</th>
              <th class="text-end w-24">成交價（未稅）</th>
              <th class="text-end w-24">成本</th>
              <th class="text-end w-24">毛利</th>
              <th class="text-end w-16">數量</th>
              <th class="text-center w-24">新購/續約</th>
              <th class="text-center w-28">採購方式</th>
              <th v-if="!readonly" class="text-center w-32">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in productList"
              :key="item.employeePipelineProductID || `temp-${index}`"
              class="border border-gray-300"
            >
              <td class="text-start">{{ item.managerProductName || "-" }}</td>
              <td class="text-start">{{ item.managerProductMainKindName || "-" }}</td>
              <td class="text-start">{{ item.managerProductSubKindName || "-" }}</td>
              <td class="text-start">{{ item.managerProductSpecificationName || "-" }}</td>
              <td class="text-end">
                {{ formatCurrency(toTaxExcluded(item.employeePipelineProductSellPrice)) }}
              </td>
              <td class="text-end">
                {{ formatCurrency(toTaxExcluded(item.employeePipelineProductClosingPrice)) }}
              </td>
              <td class="text-end">
                {{ formatCurrency(toTaxExcluded(item.employeePipelineProductCostPrice)) }}
              </td>
              <td class="text-end">
                {{ formatCurrency(toTaxExcluded(item.employeePipelineProductGrossProfit)) }}
              </td>
              <td class="text-end">{{ item.employeePipelineProductCount || 0 }}</td>
              <td class="text-center">
                {{
                  getEmployeePipelineProductPurchaseKindLabel(
                    item.employeePipelineProductPurchaseKind
                  )
                }}
              </td>
              <td class="text-center">
                <div
                  v-if="
                    item.employeePipelineProductContractKind ===
                    DbAtomEmployeePipelineProductContractKindEnum.Other
                  "
                >
                  其他: {{ item.employeePipelineProductContractText || "-" }}
                </div>
                <div v-else>
                  {{
                    getEmployeePipelineProductContractKindLabel(
                      item.employeePipelineProductContractKind
                    )
                  }}
                </div>
              </td>
              <td v-if="!readonly" class="text-center">
                <div class="flex justify-center gap-1">
                  <button
                    v-if="employeeInfoStore.hasPermission(menu, 'edit')"
                    class="rounded-lg border border-dashed px-3 py-1 text-xs font-medium text-[#082F49] hover:text-[#061F30]"
                    style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                    @click="handleEditProduct(item, index)"
                  >
                    編輯
                  </button>
                  <button
                    v-if="employeeInfoStore.hasPermission(menu, 'delete')"
                    class="btn-delete"
                    @click="handleDeleteProduct(item.employeePipelineProductID, index)"
                  >
                    刪除
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <button
        v-if="!readonly && employeeInfoStore.hasPermission(menu, 'create') && isExpanded"
        class="mt-4 w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
        @click="handleAddProduct"
      >
        附加產品
      </button>
    </div>
  </div>
</template>
