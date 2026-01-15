<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyCompanyLocationReqMdl,
  MbsBscHttpGetManyCompanyLocationRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicCompanyLocation } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
// -------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
  managerCompanyID: number | null;
}>();
const managerCompanyLocationID = defineModel<number | null>("managerCompanyLocationID");
const managerCompanyLocationName = defineModel<string | null>("managerCompanyLocationName");
// -------------------------------------------------------------------
/** 取得多筆管理者公司營業地點-文字查詢框-頁面模型 */
interface GetManyManagerCompanyLocationComboBoxViewMdl {
  pageIndex: number;
  managerCompanyLocationKeywordQuery: string;
  searchingKeyword: string;
  managerCompanyLocationList: GetManyManagerCompanyLocationComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

/** 取得多筆管理者公司營業地點-文字查詢框-項目模型 */
interface GetManyManagerCompanyLocationComboBoxItemMdl {
  managerCompanyLocationID: number;
  managerCompanyLocationName: string;
  managerCompanyLocationIsDeleted: boolean;
}

/** 取得多筆管理者公司營業地點-文字查詢框-頁面物件 */
const getManyManagerCompanyLocationComboBoxViewObj =
  reactive<GetManyManagerCompanyLocationComboBoxViewMdl>({
    pageIndex: 1,
    managerCompanyLocationKeywordQuery: "",
    searchingKeyword: "",
    managerCompanyLocationList: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });

// -------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number, keyword: string) => {
  if (!tokenStore.token || !props.managerCompanyID) return { filteredList: [], hasMore: false };

  const requestData: MbsBscHttpGetManyCompanyLocationReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerCompanyID: props.managerCompanyID,
    managerCompanyLocationName: keyword,
    pageIndex,
  };

  const responseData = await getManyBasicCompanyLocation(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success)
    return { filteredList: [], hasMore: false };

  const originalCount = responseData.managerCompanyLocationList?.length || 0;

  const filteredList =
    responseData.managerCompanyLocationList?.filter(
      (item: MbsBscHttpGetManyCompanyLocationRspItemMdl) =>
        !item.managerCompanyLocationIsDeleted
    )
    .map(
      (item: MbsBscHttpGetManyCompanyLocationRspItemMdl) =>
        ({
          managerCompanyLocationID: item.managerCompanyLocationID,
          managerCompanyLocationName: item.managerCompanyLocationName,
          managerCompanyLocationIsDeleted: item.managerCompanyLocationIsDeleted,
        }) satisfies GetManyManagerCompanyLocationComboBoxItemMdl
    ) ?? [];

  const hasMore = originalCount >= PAGE_SIZE;

  return { filteredList, hasMore };
};

// -------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue =
    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationKeywordQuery?.trim() || "";
  getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword = inputValue;

  managerCompanyLocationID.value = null;
  managerCompanyLocationName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerCompanyLocationComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList = [];
    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const { filteredList, hasMore } = await getData(
      getManyManagerCompanyLocationComboBoxViewObj.pageIndex,
      getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword
    );

    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.push(...filteredList);

    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerCompanyLocationComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerCompanyLocationComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList = [];
  getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(
    getManyManagerCompanyLocationComboBoxViewObj.pageIndex,
    getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword
  );

  getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.push(...filteredList);
  getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) {
    getManyManagerCompanyLocationComboBoxViewObj.pageIndex++;
  }
}, 300);

/** focus輸入框 */
const focusInput = async () => {
  if (props.disabled || !props.managerCompanyID) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationKeywordQuery = "";

  // 清空選擇的值
  managerCompanyLocationID.value = null;
  managerCompanyLocationName.value = null;

  getManyManagerCompanyLocationComboBoxViewObj.showDropdown = true;
  getManyManagerCompanyLocationComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList = [];
  getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = true;
  getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword = "";

  const { filteredList, hasMore } = await getData(
    getManyManagerCompanyLocationComboBoxViewObj.pageIndex,
    getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword
  );

  if (filteredList.length === 0) {
    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList = [];
    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = false;
    return;
  }

  getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.push(...filteredList);
  getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) {
    getManyManagerCompanyLocationComboBoxViewObj.pageIndex++;
  }
};

/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData &&
    !getManyManagerCompanyLocationComboBoxViewObj.isLoading
  ) {
    getManyManagerCompanyLocationComboBoxViewObj.isLoading = true;

    const { filteredList, hasMore } = await getData(
      getManyManagerCompanyLocationComboBoxViewObj.pageIndex,
      getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword
    );

    const uniqueNewList = filteredList.filter(
      (newItem) =>
        !getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.some(
          (existing) =>
            existing.managerCompanyLocationID === newItem.managerCompanyLocationID
        )
    );

    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.push(...uniqueNewList);
    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerCompanyLocationComboBoxViewObj.pageIndex++;
    }

    getManyManagerCompanyLocationComboBoxViewObj.isLoading = false;
  }
};

/** 選取項目 */
const selectItem = (item: GetManyManagerCompanyLocationComboBoxItemMdl) => {
  getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationKeywordQuery =
    item.managerCompanyLocationName;

  managerCompanyLocationID.value = item.managerCompanyLocationID;
  managerCompanyLocationName.value = item.managerCompanyLocationName;

  getManyManagerCompanyLocationComboBoxViewObj.showDropdown = false;
};

/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled || !props.managerCompanyID) return;

  getManyManagerCompanyLocationComboBoxViewObj.showDropdown =
    !getManyManagerCompanyLocationComboBoxViewObj.showDropdown;

  if (getManyManagerCompanyLocationComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationKeywordQuery = "";

    // 清空選擇的值
    managerCompanyLocationID.value = null;
    managerCompanyLocationName.value = null;

    getManyManagerCompanyLocationComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = true;
    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList = [];
    getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword = "";

    const { filteredList, hasMore } = await getData(
      getManyManagerCompanyLocationComboBoxViewObj.pageIndex,
      getManyManagerCompanyLocationComboBoxViewObj.searchingKeyword
    );

    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.push(...filteredList);
    getManyManagerCompanyLocationComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerCompanyLocationComboBoxViewObj.pageIndex++;
    }
  }
};

/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerCompanyLocationComboBoxViewObj.showDropdown = false;
};

// -------------------------------------------------------------------
/** 當名稱變動時同步輸入框 */
watch(
  () => managerCompanyLocationName.value,
  (newValue) => {
    getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationKeywordQuery =
      newValue || "";

    if (!newValue) {
      managerCompanyLocationID.value = null;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input v-model="getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationKeywordQuery" type="text"
      placeholder="查詢營業地點" class="input-box rounded-r-none" :disabled="props.disabled" @focus="focusInput"
      @input="onInput" />
    <button type="button"
      class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:cursor-not-allowed"
      :disabled="props.disabled" @click="clickDropdownBtn">
      <svg v-if="getManyManagerCompanyLocationComboBoxViewObj.showDropdown" class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676" />
      </svg>
      <svg v-else class="w-5 h-5 text-gray-500" viewBox="0 0 20 25" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path
          d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
          fill="#787676" />
      </svg>
    </button>

    <transition enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2" enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in" leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2">
      <div v-if="
        getManyManagerCompanyLocationComboBoxViewObj.showDropdown && !props.disabled
      " class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown">
        <ul v-if="
          getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList.length === 0
        ">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li v-for="item in getManyManagerCompanyLocationComboBoxViewObj.managerCompanyLocationList"
            :key="item.managerCompanyLocationID" class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)">
            {{ item.managerCompanyLocationName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
