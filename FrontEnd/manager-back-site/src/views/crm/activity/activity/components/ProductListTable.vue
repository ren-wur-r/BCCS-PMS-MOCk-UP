<script setup lang="ts">
interface ProductItem {
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductSubKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
  managerActivityProductID?: number; // 編輯模式才有，用於識別是否為編輯模式
}

interface Props {
  productList: ProductItem[];
  loading?: boolean;
  showActionRow?: boolean;
  showAppendBtn?: boolean;
}

withDefaults(defineProps<Props>(), {
  loading: false,
  showActionRow: true,
  showAddBtn: true,
});

interface Emits {
  (e: "edit", index: number, product: ProductItem): void;
  (e: "delete", index: number, product: ProductItem): void;
  (e: "append"): void;
}

const emit = defineEmits<Emits>();

//--------------------------------------------------------
const clickEditBtn = (index: number, product: ProductItem) => {
  emit("edit", index, product);
};

const clickDeleteBtn = (index: number, product: ProductItem) => {
  emit("delete", index, product);
};

const clickAppendBtn = () => {
  emit("append");
};
</script>

<template>
  <div>
    <!-- 產品列表表格 -->
    <div v-if="productList.length > 0">
      <table class="table-base table-fixed table-sticky w-full">
        <thead class="bg-gray-800 text-white">
          <tr>
            <th class="text-start">產品主分類</th>
            <th class="text-start">產品子分類</th>
            <th class="text-start">產品名稱</th>
            <th v-if="showActionRow" class="text-center w-40">操作</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(product, index) in productList"
            :key="product.managerProductID || product.managerActivityProductID || index"
            class="border border-gray-300"
          >
            <td class="text-start">{{ product.managerProductMainKindName }}</td>
            <td class="text-start">{{ product.managerProductSubKindName }}</td>
            <td class="text-start">{{ product.managerProductName }}</td>
            <td v-if="showActionRow" class="text-center">
              <button
                class="me-1 rounded-lg border border-dashed px-3 py-1 text-xs font-medium text-[#082F49] hover:text-[#061F30]"
                style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
                :disabled="loading"
                @click="clickEditBtn(index, product)"
              >
                編輯
              </button>
              <button class="btn-delete" :disabled="loading" @click="clickDeleteBtn(index, product)">
                刪除
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- 無資料顯示 -->
    <div v-else class="text-center py-8 text-gray-500">尚未選擇任何產品</div>
  </div>

  <!-- 附加產品按鈕 -->
  <button
    v-if="showAppendBtn"
    class="rounded-lg border border-dashed px-4 py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
    style="background-color: rgb(242, 246, 249); border-color: rgb(8, 47, 73);"
    @click="clickAppendBtn"
  >
    附加產品
  </button>
</template>
