<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import {
  type MbsBscHttpGetManyProjectStoneJobReqMdl,
  type MbsBscHttpGetManyProjectStoneJobRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicProjectStoneJob } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;

// -------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
  employeeProjectID: number | null;
  employeeProjectStoneID: number | null;
}>();

const employeeProjectStoneJobID = defineModel<number | null>("employeeProjectStoneJobID");
const employeeProjectStoneJobName = defineModel<string | null>("employeeProjectStoneJobName");

// -------------------------------------------------------------------
/** ComboBox 頁面模型 */
interface GetManyProjectStoneJobComboBoxViewMdl {
  pageIndex: number;
  keywordQuery: string;
  searchingKeyword: string;
  projectStoneJobList: GetManyProjectStoneJobComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

/** ComboBox 項目模型 */
interface GetManyProjectStoneJobComboBoxItemMdl {
  employeeProjectStoneJobID: number;
  employeeProjectStoneJobName: string | null;
}

/** 頁面物件 */
const viewObj = reactive<GetManyProjectStoneJobComboBoxViewMdl>({
  pageIndex: 1,
  keywordQuery: "",
  searchingKeyword: "",
  projectStoneJobList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});

// -------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number, keyword: string) => {
  if (!tokenStore.token) return { filteredList: [], hasMore: false };
  if (!props.employeeProjectID) return { filteredList: [], hasMore: false };
  if (!props.employeeProjectStoneID) return { filteredList: [], hasMore: false };

  const requestData: MbsBscHttpGetManyProjectStoneJobReqMdl = {
    employeeLoginToken: tokenStore.token,
    employeeProjectID: props.employeeProjectID,
    employeeProjectStoneID: props.employeeProjectStoneID,
    employeeProjectStoneJobName: keyword,
    pageIndex,
  };

  const responseData = await getManyBasicProjectStoneJob(requestData);

  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success)
    return { filteredList: [], hasMore: false };

  const originalCount = responseData.projectStoneJobList?.length || 0;

  const filteredList =
    responseData.projectStoneJobList?.map(
      (item: MbsBscHttpGetManyProjectStoneJobRspItemMdl) =>
        ({
          employeeProjectStoneJobID: item.employeeProjectStoneJobID,
          employeeProjectStoneJobName: item.employeeProjectStoneJobName,
        }) satisfies GetManyProjectStoneJobComboBoxItemMdl
    ) ?? [];

  const hasMore = originalCount >= PAGE_SIZE;

  return { filteredList, hasMore };
};

// -------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = viewObj.keywordQuery?.trim() || "";
  viewObj.searchingKeyword = inputValue;

  employeeProjectStoneJobID.value = null;
  employeeProjectStoneJobName.value = null;

  viewObj.pageIndex = 1;
  viewObj.projectStoneJobList = [];
  viewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(viewObj.pageIndex, inputValue);

  viewObj.projectStoneJobList.push(...filteredList);
  viewObj.hasMoreData = hasMore;

  if (hasMore) viewObj.pageIndex++;
}, 300);

// -------------------------------------------------------------------
/** focus 輸入框 */
const focusInput = async () => {
  if (props.disabled) return;
  if (!props.employeeProjectID || !props.employeeProjectStoneID) return;

  // 清空關鍵字查詢，以顯示所有選項
  viewObj.keywordQuery = "";

  // 清空選擇的值
  employeeProjectStoneJobID.value = null;
  employeeProjectStoneJobName.value = null;

  viewObj.showDropdown = true;
  viewObj.pageIndex = 1;
  viewObj.projectStoneJobList = [];
  viewObj.hasMoreData = true;
  viewObj.searchingKeyword = "";

  const { filteredList, hasMore } = await getData(viewObj.pageIndex, "");

  viewObj.projectStoneJobList.push(...filteredList);
  viewObj.hasMoreData = hasMore;

  if (hasMore) viewObj.pageIndex++;
};

// -------------------------------------------------------------------
/** 下拉滾動 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom =
    dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (nearBottom && viewObj.hasMoreData && !viewObj.isLoading) {
    viewObj.isLoading = true;

    const { filteredList, hasMore } = await getData(
      viewObj.pageIndex,
      viewObj.searchingKeyword
    );

    const uniqueNewList = filteredList.filter(
      (newItem) =>
        !viewObj.projectStoneJobList.some(
          (existing) =>
            existing.employeeProjectStoneJobID === newItem.employeeProjectStoneJobID
        )
    );

    viewObj.projectStoneJobList.push(...uniqueNewList);
    viewObj.hasMoreData = hasMore;

    if (hasMore) viewObj.pageIndex++;

    viewObj.isLoading = false;
  }
};

// -------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyProjectStoneJobComboBoxItemMdl) => {
  viewObj.keywordQuery = item.employeeProjectStoneJobName || "";

  employeeProjectStoneJobID.value = item.employeeProjectStoneJobID;
  employeeProjectStoneJobName.value = item.employeeProjectStoneJobName;

  viewObj.showDropdown = false;
};

// -------------------------------------------------------------------
const clickDropdownBtn = async () => {
  if (props.disabled) return;
  if (!props.employeeProjectID || !props.employeeProjectStoneID) return;

  viewObj.showDropdown = !viewObj.showDropdown;

  if (viewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    viewObj.keywordQuery = "";

    // 清空選擇的值
    employeeProjectStoneJobID.value = null;
    employeeProjectStoneJobName.value = null;

    viewObj.pageIndex = 1;
    viewObj.projectStoneJobList = [];
    viewObj.hasMoreData = true;
    viewObj.searchingKeyword = "";

    const { filteredList, hasMore } = await getData(viewObj.pageIndex, "");

    viewObj.projectStoneJobList.push(...filteredList);
    viewObj.hasMoreData = hasMore;

    if (hasMore) viewObj.pageIndex++;
  }
};

// -------------------------------------------------------------------
const closeDropdown = () => {
  viewObj.showDropdown = false;
};

// -------------------------------------------------------------------
/** 同步外部 v-model 名稱 */
watch(
  () => employeeProjectStoneJobName.value,
  (newValue) => {
    viewObj.keywordQuery = newValue || "";
    if (!newValue) employeeProjectStoneJobID.value = null;
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="viewObj.keywordQuery"
      type="text"
      placeholder="查詢工項"
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
        v-if="viewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25"
        fill="none"
      >
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676"
        />
      </svg>

      <svg
        v-else
        class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25"
        fill="none"
      >
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
        v-if="viewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="viewObj.projectStoneJobList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>

        <ul v-else>
          <li
            v-for="item in viewObj.projectStoneJobList"
            :key="item.employeeProjectStoneJobID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.employeeProjectStoneJobName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
