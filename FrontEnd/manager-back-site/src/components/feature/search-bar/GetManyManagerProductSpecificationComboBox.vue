<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyProductSpecificationReqMdl,
  MbsBscHttpGetManyProductSpecificationRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicProductSpecification } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;

//------------------------------------------------------------
// Props + defineModel
const props = defineProps<{ disabled: boolean; managerProductID: number | null }>();

const managerProductSpecificationID = defineModel<number | null>("managerProductSpecificationID");
const managerProductSpecificationName = defineModel<string | null>(
  "managerProductSpecificationName"
);

const emit = defineEmits<{
  "specification-selected": [specification: GetManyManagerProductSpecificationComboBoxItemMdl];
}>();

//------------------------------------------------------------
// 型別宣告
interface GetManyManagerProductSpecificationComboBoxViewMdl {
  pageIndex: number;
  keywordQuery: string | null;
  isEnableQuery: boolean;
  list: GetManyManagerProductSpecificationComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

interface GetManyManagerProductSpecificationComboBoxItemMdl {
  managerProductSpecificationID: number;
  managerProductSpecificationName: string;
  managerProductSpecificationSellPrice: number;
  managerProductSpecificationCostPrice: number;
  managerProductSpecificationIsEnable: boolean;
}

//------------------------------------------------------------
// reactive 狀態
const getManyManagerProductSpecificationComboBoxViewObj =
  reactive<GetManyManagerProductSpecificationComboBoxViewMdl>({
    pageIndex: 1,
    keywordQuery: null,
    isEnableQuery: true,
    list: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });

//------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  if (!tokenStore.token || !props.managerProductID) return { filteredList: [], hasMore: false };

  const requestData: MbsBscHttpGetManyProductSpecificationReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerProductID: props.managerProductID,
    productSpecificationName: getManyManagerProductSpecificationComboBoxViewObj.keywordQuery,
    productSpecificationIsEnable: getManyManagerProductSpecificationComboBoxViewObj.isEnableQuery,
    pageIndex,
  };

  const responseData = await getManyBasicProductSpecification(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success)
    return { filteredList: [], hasMore: false };

  const originalCount = responseData.productSpecificationList?.length || 0;

  const filteredList =
    responseData.productSpecificationList?.map(
      (item: MbsBscHttpGetManyProductSpecificationRspItemMdl) =>
        ({
          managerProductSpecificationID: item.ManagerProductSpecificationID,
          managerProductSpecificationName: item.ManagerProductSpecificationName,
          managerProductSpecificationSellPrice: item.ManagerProductSpecificationSellPrice,
          managerProductSpecificationCostPrice: item.ManagerProductSpecificationCostPrice,
          managerProductSpecificationIsEnable: item.ManagerProductSpecificationIsEnable,
        }) satisfies GetManyManagerProductSpecificationComboBoxItemMdl
    ) ?? [];

  const hasMore = originalCount >= PAGE_SIZE;
  return { filteredList, hasMore };
};

//------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerProductSpecificationComboBoxViewObj.keywordQuery?.trim() || "";

  // 清空選擇
  managerProductSpecificationID.value = null;
  managerProductSpecificationName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerProductSpecificationComboBoxViewObj.pageIndex = 1;
    getManyManagerProductSpecificationComboBoxViewObj.list = [];
    getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const { filteredList: fetchedList } = await getData(
      getManyManagerProductSpecificationComboBoxViewObj.pageIndex
    );

    getManyManagerProductSpecificationComboBoxViewObj.list.push(...fetchedList);

    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerProductSpecificationComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 少於1字時查全部
  getManyManagerProductSpecificationComboBoxViewObj.pageIndex = 1;
  getManyManagerProductSpecificationComboBoxViewObj.list = [];
  getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(
    getManyManagerProductSpecificationComboBoxViewObj.pageIndex
  );

  getManyManagerProductSpecificationComboBoxViewObj.list.push(...filteredList);
  getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) getManyManagerProductSpecificationComboBoxViewObj.pageIndex++;
}, 300);

//------------------------------------------------------------
/** focus 輸入框 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerProductSpecificationComboBoxViewObj.keywordQuery = null;

  // 清空選擇的值
  managerProductSpecificationID.value = null;
  managerProductSpecificationName.value = null;

  getManyManagerProductSpecificationComboBoxViewObj.showDropdown = true;
  getManyManagerProductSpecificationComboBoxViewObj.pageIndex = 1;
  getManyManagerProductSpecificationComboBoxViewObj.list = [];
  getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(
    getManyManagerProductSpecificationComboBoxViewObj.pageIndex
  );
  getManyManagerProductSpecificationComboBoxViewObj.list.push(...filteredList);
  getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) getManyManagerProductSpecificationComboBoxViewObj.pageIndex++;
};

//------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerProductSpecificationComboBoxViewObj.hasMoreData &&
    !getManyManagerProductSpecificationComboBoxViewObj.isLoading
  ) {
    getManyManagerProductSpecificationComboBoxViewObj.isLoading = true;
    const { filteredList, hasMore } = await getData(
      getManyManagerProductSpecificationComboBoxViewObj.pageIndex
    );

    const uniqueNewList = filteredList.filter(
      (newItem) =>
        !getManyManagerProductSpecificationComboBoxViewObj.list.some(
          (existing) =>
            existing.managerProductSpecificationID === newItem.managerProductSpecificationID
        )
    );

    getManyManagerProductSpecificationComboBoxViewObj.list.push(...uniqueNewList);
    getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = hasMore;
    if (hasMore) getManyManagerProductSpecificationComboBoxViewObj.pageIndex++;
    getManyManagerProductSpecificationComboBoxViewObj.isLoading = false;
  }
};

//------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerProductSpecificationComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerProductSpecificationComboBoxViewObj.keywordQuery =
    item.managerProductSpecificationName;

  // 帶出ID、名稱
  managerProductSpecificationID.value = item.managerProductSpecificationID;
  managerProductSpecificationName.value = item.managerProductSpecificationName;

  // 發送選中的規格資料到父組件
  emit("specification-selected", item);

  // 關閉下拉選單
  getManyManagerProductSpecificationComboBoxViewObj.showDropdown = false;
};

//------------------------------------------------------------
/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  getManyManagerProductSpecificationComboBoxViewObj.showDropdown =
    !getManyManagerProductSpecificationComboBoxViewObj.showDropdown;

  if (getManyManagerProductSpecificationComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerProductSpecificationComboBoxViewObj.keywordQuery = null;

    // 清空選擇的值
    managerProductSpecificationID.value = null;
    managerProductSpecificationName.value = null;

    getManyManagerProductSpecificationComboBoxViewObj.pageIndex = 1;
    getManyManagerProductSpecificationComboBoxViewObj.list = [];
    getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = true;

    const { filteredList, hasMore } = await getData(
      getManyManagerProductSpecificationComboBoxViewObj.pageIndex
    );
    getManyManagerProductSpecificationComboBoxViewObj.list.push(...filteredList);
    getManyManagerProductSpecificationComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) getManyManagerProductSpecificationComboBoxViewObj.pageIndex++;
  }
};

/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerProductSpecificationComboBoxViewObj.showDropdown = false;
};

//------------------------------------------------------------
// 當名稱改變時更新輸入框
watch(
  () => managerProductSpecificationName.value,
  (newValue) => {
    getManyManagerProductSpecificationComboBoxViewObj.keywordQuery = newValue || null;
    if (!newValue) managerProductSpecificationID.value = null;
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerProductSpecificationComboBoxViewObj.keywordQuery"
      type="text"
      placeholder="查詢規格"
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
        v-if="getManyManagerProductSpecificationComboBoxViewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
        width="20"
        height="25"
        viewBox="0 0 20 25"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676"
        />
      </svg>
      <svg
        v-else
        class="w-5 h-5 text-gray-500"
        width="20"
        height="25"
        viewBox="0 0 20 25"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
          fill="#787676"
        />
      </svg>
    </button>

    <!-- 下拉選單 -->
    <transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2"
      enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2"
    >
      <div
        v-if="getManyManagerProductSpecificationComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-40 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerProductSpecificationComboBoxViewObj.list.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerProductSpecificationComboBoxViewObj.list"
            :key="item.managerProductSpecificationID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerProductSpecificationName }}
            <span class="text-gray-400 text-xs ml-2"
              >售價：{{ item.managerProductSpecificationSellPrice?.toLocaleString() }} 元</span
            >
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
