<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";

import type {
  MbsBscHttpGetManyProjectReqMdl,
  MbsBscHttpGetManyProjectRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";

import { getManyBasicProject } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;

// -------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
}>();

const employeeProjectID = defineModel<number | null>("employeeProjectID");
const employeeProjectName = defineModel<string | null>("employeeProjectName");

// -------------------------------------------------------------------
/** ComboBox 頁面模型 */
interface GetManyProjectComboBoxViewMdl {
  pageIndex: number;
  keywordQuery: string;
  searchingKeyword: string;
  projectList: GetManyProjectComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

/** ComboBox 項目模型 */
interface GetManyProjectComboBoxItemMdl {
  employeeProjectID: number;
  employeeProjectName: string | null;
}

/** 頁面物件 */
const viewObj = reactive<GetManyProjectComboBoxViewMdl>({
  pageIndex: 1,
  keywordQuery: "",
  searchingKeyword: "",
  projectList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});

// -------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number, keyword: string) => {
  if (!tokenStore.token) return { filteredList: [], hasMore: false };

  const requestData: MbsBscHttpGetManyProjectReqMdl = {
    employeeLoginToken: tokenStore.token,
    projectName: keyword,
    pageIndex,
  };

  const responseData = await getManyBasicProject(requestData);

  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success)
    return { filteredList: [], hasMore: false };

  const count = responseData.employeeProjectList?.length ?? 0;

  const filteredList =
    responseData.employeeProjectList?.map(
      (item: MbsBscHttpGetManyProjectRspItemMdl) =>
        ({
          employeeProjectID: item.employeeProjectID,
          employeeProjectName: item.employeeProjectName,
        }) satisfies GetManyProjectComboBoxItemMdl
    ) ?? [];
 
  return {
    filteredList,
    hasMore: count >= PAGE_SIZE,
  };
};

// -------------------------------------------------------------------
/** 使用者輸入 */
const onInput = debounce(async () => {
  const inputVal = viewObj.keywordQuery.trim();
  viewObj.searchingKeyword = inputVal;

  employeeProjectID.value = null;
  employeeProjectName.value = null;

  viewObj.pageIndex = 1;
  viewObj.projectList = [];
  viewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(
    viewObj.pageIndex,
    inputVal
  );

  viewObj.projectList.push(...filteredList);
  viewObj.hasMoreData = hasMore;

  if (hasMore) viewObj.pageIndex++;
}, 300);

// -------------------------------------------------------------------
/** Focus → 開啟下拉 + 查全部 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  viewObj.keywordQuery = "";

  // 清空選擇的值
  employeeProjectID.value = null;
  employeeProjectName.value = null;

  viewObj.showDropdown = true;
  viewObj.pageIndex = 1;
  viewObj.projectList = [];
  viewObj.hasMoreData = true;
  viewObj.searchingKeyword = "";

  const { filteredList, hasMore } = await getData(1, "");

  viewObj.projectList.push(...filteredList);
  viewObj.hasMoreData = hasMore;

  if (hasMore) viewObj.pageIndex++;
};

// -------------------------------------------------------------------
/** Lazy loading */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const reachBottom =
    dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    reachBottom &&
    viewObj.hasMoreData &&
    !viewObj.isLoading
  ) {
    viewObj.isLoading = true;

    const { filteredList, hasMore } = await getData(
      viewObj.pageIndex,
      viewObj.searchingKeyword
    );

    const uniqueList = filteredList.filter(
      (item) =>
        !viewObj.projectList.some(
          (existing) => existing.employeeProjectID === item.employeeProjectID
        )
    );

    viewObj.projectList.push(...uniqueList);
    viewObj.hasMoreData = hasMore;

    if (hasMore) viewObj.pageIndex++;

    viewObj.isLoading = false;
  }
};

// -------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyProjectComboBoxItemMdl) => {
  viewObj.keywordQuery = item.employeeProjectName ?? "";

  employeeProjectID.value = item.employeeProjectID;
  employeeProjectName.value = item.employeeProjectName;

  viewObj.showDropdown = false;
};

// -------------------------------------------------------------------
/** 按下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  viewObj.showDropdown = !viewObj.showDropdown;

  if (viewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    viewObj.keywordQuery = "";

    // 清空選擇的值
    employeeProjectID.value = null;
    employeeProjectName.value = null;

    viewObj.pageIndex = 1;
    viewObj.projectList = [];
    viewObj.hasMoreData = true;

    const { filteredList, hasMore } = await getData(1, "");

    viewObj.projectList.push(...filteredList);
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
  () => employeeProjectName.value,
  (newVal) => {
    viewObj.keywordQuery = newVal || "";
    if (!newVal) employeeProjectID.value = null;
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="viewObj.keywordQuery"
      type="text"
      placeholder="查詢專案"
      class="input-box rounded-r-none"
      :disabled="props.disabled"
      @focus="focusInput"
      @input="onInput"
    />

    <button
      type="button"
      class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white hover:bg-gray-50 disabled:bg-gray-200"
      :disabled="props.disabled"
      @click="clickDropdownBtn"
    >
      <svg
        v-if="viewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25"
      >
        <path
          d="M9.7 8.5 4.3 15.4c-.34.43-.14 1.26.3 1.26h10.7c.44 0 .64-.83.3-1.26L10.3 8.5z"
          fill="#787676"
        />
      </svg>

      <svg
        v-else
        class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25"
      >
        <path
          d="m10.3 16.5 5.4-6.9c.34-.43.14-1.26-.3-1.26H4.6c-.44 0-.64.83-.3 1.26l5.4 6.9z"
          fill="#787676"
        />
      </svg>
    </button>

    <!-- 下拉 -->
    <transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 -translate-y-2"
      enter-to-class="opacity-100 translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 translate-y-0"
      leave-to-class="opacity-0 -translate-y-2"
    >
      <div
        v-if="viewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white shadow-md z-50 rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="viewObj.projectList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>

        <ul v-else>
          <li
            v-for="item in viewObj.projectList"
            :key="item.employeeProjectID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.employeeProjectName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
