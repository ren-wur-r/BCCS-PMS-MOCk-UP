<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyCompanyNameFromPipelineOriginalReqMdl,
  MbsBscHttpGetManyCompanyNameFromPipelineOriginalRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { GetManyBasicManagerCompanyNameFromPipelineOriginal } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;

//-------------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerCompanyName = defineModel<string | null>("managerCompanyName");
//-------------------------------------------------------------------
/** 取得多筆公司名稱從【商機原始】-文字查詢框-頁面模型 */
interface GetManyManagerCompanyNameFromPipelineOriginalComboBoxViewMdl {
  pageIndex: number;
  keywordQuery: string;
  companyNameList: string[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

/** 取得多筆公司名稱從【商機原始】-文字查詢框-項目模型 */
const getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj =
  reactive<GetManyManagerCompanyNameFromPipelineOriginalComboBoxViewMdl>({
    pageIndex: 1,
    keywordQuery: "",
    companyNameList: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });

//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  if (!tokenStore.token) return { filteredList: [], hasMore: false };

  const requestData: MbsBscHttpGetManyCompanyNameFromPipelineOriginalReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerCompanyName: getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery,
    pageIndex,
  };

  const responseData: MbsBscHttpGetManyCompanyNameFromPipelineOriginalRspMdl | null =
    await GetManyBasicManagerCompanyNameFromPipelineOriginal(requestData);

  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success)
    return { filteredList: [], hasMore: false };

  const list = responseData.managerCompanyNameList ?? [];
  const hasMore = list.length >= PAGE_SIZE;
  return { filteredList: list, hasMore };
};

//-------------------------------------------------------------------
/** 使用者輸入時觸發查詢 */
const onInput = debounce(async () => {
  const inputValue =
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery.trim();

  managerCompanyName.value = null;

// 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList = [];
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const { filteredList, hasMore } = await getData(getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex);

    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.push(...filteredList);

    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex++;
    }

    return;
  }


  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList = [];
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex
  );

  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.push(
    ...filteredList
  );
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) {
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex++;
  }
}, 300);

//-------------------------------------------------------------------
/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown =
    !getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown;

  if (getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery = "";

    // 清空選擇的值
    managerCompanyName.value = null;

    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = true;
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList = [];

    const { filteredList, hasMore } = await getData(
      getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex
    );

    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.push(
      ...filteredList
    );
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex++;
    }
  }
};

//-------------------------------------------------------------------
/** Focus 時自動載入 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery = "";

  // 清空選擇的值
  managerCompanyName.value = null;

  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown = true;

  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList = [];
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex
  );

  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.push(
    ...filteredList
  );
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) {
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex++;
  }
};

//-------------------------------------------------------------------
/** 滾動下拉載入更多 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData &&
    !getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.isLoading
  ) {
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.isLoading = true;

    const { filteredList, hasMore } = await getData(
      getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex
    );

    const uniqueNewList = filteredList.filter(
      (newItem) =>
        !getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.includes(
          newItem
        )
    );

    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.push(
      ...uniqueNewList
    );
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.pageIndex++;
    }

    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.isLoading = false;
  }
};

//-------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: string) => {
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery = item;
  managerCompanyName.value = item;
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------------
/** 關閉下拉 */
const closeDropdown = () => {
  getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------------
/** 同步 v-model */
watch(
  () => managerCompanyName.value,
  (newValue) => {
    getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery = newValue || "";
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.keywordQuery"
      type="text"
      placeholder="查詢原始客戶"
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
        v-if="getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
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

    <transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2"
      enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2"
    >
      <div
        v-if="
          getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.showDropdown &&
          !props.disabled
        "
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul
          v-if="
            getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList.length === 0
          "
        >
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerCompanyNameFromPipelineOriginalComboBoxViewObj.companyNameList"
            :key="item"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
