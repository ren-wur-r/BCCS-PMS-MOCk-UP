<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import type {
  MbsBscHttpGetManyProductReqMdl,
  MbsBscHttpGetManyProductRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicProduct } from "@/services";

//-------------------------------------------------------------------
const tokenStore = useTokenStore();
const PAGE_SIZE = 5;

//-------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
  managerProductMainKindID?: number | null;
  managerProductSubKindID?: number | null;
}>();

/** 產品選擇事件回傳的資料模型 */
export interface GetManyManagerProductComboBoxSelectedData {
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

const emit = defineEmits<{
  "product-selected": [product: GetManyManagerProductComboBoxSelectedData];
}>();

const managerProductID = defineModel<number | null>("managerProductID");
const managerProductName = defineModel<string | null>("managerProductName");

//-------------------------------------------------------------------
/** 取得多筆產品-項目模型 */
interface GetManyManagerProductComboBoxItemMdl {
  managerProductID: number;
  managerProductName: string;
  managerProductIsEnable: boolean;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
  managerProductMainKindCommissionRate: number;
}

/** 取得多筆產品-頁面模型 */
interface GetManyManagerProductComboBoxViewMdl {
  pageIndex: number;
  keywordQuery: string | null;
  productList: GetManyManagerProductComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

//-------------------------------------------------------------------
/** 頁面物件 */
const getManyManagerProductComboBoxViewObj = reactive<GetManyManagerProductComboBoxViewMdl>({
  pageIndex: 1,
  keywordQuery: null,
  productList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});

//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  if (!tokenStore.token) return [];

  const requestData: MbsBscHttpGetManyProductReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerProductMainKindID: props.managerProductMainKindID ?? null,
    managerProductSubKindID: props.managerProductSubKindID ?? null,
    managerProductName: getManyManagerProductComboBoxViewObj.keywordQuery,
    pageIndex: pageIndex,
  };

  const responseData = await getManyBasicProduct(requestData);
  if (!responseData) return [];

  const fetchedList =
    responseData.productList?.map(
      (item: MbsBscHttpGetManyProductRspItemMdl) =>
        ({
          managerProductID: item.managerProductID,
          managerProductName: item.managerProductName,
          managerProductIsEnable: item.managerProductIsEnable,
          managerProductMainKindID: item.managerProductMainKindID,
          managerProductMainKindName: item.managerProductMainKindName,
          managerProductSubKindID: item.managerProductSubKindID,
          managerProductSubKindName: item.managerProductSubKindName,
          managerProductMainKindCommissionRate: item.managerProductMainKindCommissionRate,
        }) satisfies GetManyManagerProductComboBoxItemMdl
    ) ?? [];

  return fetchedList;
};

//-------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerProductComboBoxViewObj.keywordQuery?.trim() || "";

  // 清空選擇
  managerProductID.value = null;
  managerProductName.value = null;

  // 若輸入少於 1 字 → 查全部
  if (inputValue.length < 1) {
    getManyManagerProductComboBoxViewObj.pageIndex = 1;
    getManyManagerProductComboBoxViewObj.productList = [];
    getManyManagerProductComboBoxViewObj.hasMoreData = true;

    const fetchedList: GetManyManagerProductComboBoxItemMdl[] = await getData(
      getManyManagerProductComboBoxViewObj.pageIndex
    );

    getManyManagerProductComboBoxViewObj.productList.push(...fetchedList);

    if (getManyManagerProductComboBoxViewObj.productList.length < PAGE_SIZE) {
      getManyManagerProductComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerProductComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 重新搜尋
  getManyManagerProductComboBoxViewObj.pageIndex = 1;
  getManyManagerProductComboBoxViewObj.productList = [];
  getManyManagerProductComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerProductComboBoxViewObj.pageIndex);
  getManyManagerProductComboBoxViewObj.productList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerProductComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerProductComboBoxViewObj.pageIndex++;
  }
}, 300);

//-------------------------------------------------------------------
/** focus輸入框時 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerProductComboBoxViewObj.keywordQuery = null;

  // 清空選擇的值
  managerProductID.value = null;
  managerProductName.value = null;

  getManyManagerProductComboBoxViewObj.showDropdown = true;
  getManyManagerProductComboBoxViewObj.pageIndex = 1;
  getManyManagerProductComboBoxViewObj.productList = [];
  getManyManagerProductComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerProductComboBoxViewObj.pageIndex);
  getManyManagerProductComboBoxViewObj.productList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerProductComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerProductComboBoxViewObj.pageIndex++;
  }
};

//-------------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerProductComboBoxViewObj.hasMoreData &&
    !getManyManagerProductComboBoxViewObj.isLoading
  ) {
    getManyManagerProductComboBoxViewObj.isLoading = true;

    const fetchedList = await getData(getManyManagerProductComboBoxViewObj.pageIndex);

    const uniqueNewList = fetchedList.filter(
      (newItem) =>
        !getManyManagerProductComboBoxViewObj.productList.some(
          (existing) => existing.managerProductID === newItem.managerProductID
        )
    );

    getManyManagerProductComboBoxViewObj.productList.push(...uniqueNewList);

    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerProductComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerProductComboBoxViewObj.pageIndex++;
    }

    getManyManagerProductComboBoxViewObj.isLoading = false;
  }
};

//-------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerProductComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerProductComboBoxViewObj.keywordQuery = item.managerProductName;

  // 帶出ID、名稱
  managerProductID.value = item.managerProductID;
  managerProductName.value = item.managerProductName;

  // 發送產品選擇事件（帶完整資訊）
  emit("product-selected", {
    managerProductID: item.managerProductID,
    managerProductName: item.managerProductName,
    managerProductMainKindID: item.managerProductMainKindID,
    managerProductMainKindName: item.managerProductMainKindName,
    managerProductSubKindID: item.managerProductSubKindID,
    managerProductSubKindName: item.managerProductSubKindName,
    managerProductMainKindCommissionRate: item.managerProductMainKindCommissionRate,
  });

  // 關閉下拉選單
  getManyManagerProductComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------------
/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  getManyManagerProductComboBoxViewObj.showDropdown =
    !getManyManagerProductComboBoxViewObj.showDropdown;

  if (getManyManagerProductComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerProductComboBoxViewObj.keywordQuery = null;

    // 清空選擇的值
    managerProductID.value = null;
    managerProductName.value = null;

    getManyManagerProductComboBoxViewObj.pageIndex = 1;
    getManyManagerProductComboBoxViewObj.productList = [];
    getManyManagerProductComboBoxViewObj.hasMoreData = true;

    const fetchedList = await getData(getManyManagerProductComboBoxViewObj.pageIndex);
    getManyManagerProductComboBoxViewObj.productList.push(...fetchedList);
    getManyManagerProductComboBoxViewObj.pageIndex++;
  }
};

//-------------------------------------------------------------------
/** 關閉下拉 */
const closeDropdown = () => {
  getManyManagerProductComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------------
/** 監聽產品名稱變化 */
watch(
  () => managerProductName.value,
  (newValue) => {
    getManyManagerProductComboBoxViewObj.keywordQuery = newValue || null;
    if (!newValue) {
      managerProductID.value = null;
    }
  },
  { immediate: true }
);

/** 監聽主分類或子分類變化，重新載入列表 */
watch(
  () => [props.managerProductMainKindID, props.managerProductSubKindID],
  async () => {
    // 如果下拉選單是開啟狀態，重新載入資料
    if (getManyManagerProductComboBoxViewObj.showDropdown) {
      getManyManagerProductComboBoxViewObj.pageIndex = 1;
      getManyManagerProductComboBoxViewObj.productList = [];
      getManyManagerProductComboBoxViewObj.hasMoreData = true;

      const fetchedList = await getData(getManyManagerProductComboBoxViewObj.pageIndex);
      getManyManagerProductComboBoxViewObj.productList.push(...fetchedList);

      if (fetchedList.length < PAGE_SIZE) {
        getManyManagerProductComboBoxViewObj.hasMoreData = false;
      } else {
        getManyManagerProductComboBoxViewObj.pageIndex++;
      }
    }
  }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerProductComboBoxViewObj.keywordQuery"
      type="text"
      placeholder="查詢產品"
      class="input-box rounded-r-none"
      :disabled="props.disabled"
      @focus="focusInput"
      @input="onInput"
    />
    <button
      type="button"
      class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:cursor-not-allowed"
      :disabled="props.disabled"
      @click="clickDropdownBtn"
    >
      <svg
        v-if="getManyManagerProductComboBoxViewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25"
      >
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676"
        />
      </svg>
      <svg v-else class="w-5 h-5 text-gray-500" viewBox="0 0 20 25">
        <path
          d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
          fill="#787676"
        />
      </svg>
    </button>

    <transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2"
      enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2"
    >
      <div
        v-if="getManyManagerProductComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-9999 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerProductComboBoxViewObj.productList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerProductComboBoxViewObj.productList"
            :key="item.managerProductID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerProductName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
