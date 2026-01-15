<script setup lang="ts">
import { reactive, watch } from "vue";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { useTokenStore } from "@/stores/token";
import type {
  MbsBscHttpGetManyRegionReqMdl,
  MbsBscHttpGetManyRegionRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicManagerRegion } from "@/services";
import { debounce } from "lodash-es";
import { DbManagerRegionConst } from "@/constants/DbManagerRegionConst";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerRegionID = defineModel<number | null>("managerRegionID");
const managerRegionName = defineModel<string | null>("managerRegionName");
//-------------------------------------------------------------------
/** 取得多筆管理者地區-文字查詢框-頁面模型 */
interface GetManyManagerRegionComboBoxViewMdl {
  pageIndex: number;
  managerRegionKeywordQuery: string | null;
  managerRegionIsEnableQuery: boolean | null;
  managerRegionList: GetManyManagerRegionComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆管理者地區-文字查詢框-項目模型 */
interface GetManyManagerRegionComboBoxItemMdl {
  managerRegionID: number;
  managerRegionName: string;
  managerRegionIsEnable: boolean;
}
/** 取得多筆管理者地區-文字查詢框-頁面物件 */
const getManyManagerRegionComboBoxViewObj = reactive<GetManyManagerRegionComboBoxViewMdl>({
  pageIndex: 1,
  managerRegionKeywordQuery: null,
  managerRegionIsEnableQuery: null,
  managerRegionList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});

//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  // 檢查資料
  if (!tokenStore.token) return { filteredList: [], hasMore: false };

  // 呼叫api
  const requestData: MbsBscHttpGetManyRegionReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerRegionName: getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery,
    managerRegionIsEnable: getManyManagerRegionComboBoxViewObj.managerRegionIsEnableQuery,
    pageIndex: pageIndex,
  };
  const responseData = await getManyBasicManagerRegion(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success)
    return { filteredList: [], hasMore: false };

  // 記錄原始資料數量
  const originalCount = responseData.managerRegionList.length || 0;

  // 處理資料
  const filteredList =
    responseData.managerRegionList
      .filter(
        (item: MbsBscHttpGetManyRegionRspItemMdl) =>
          item.managerRegionID !== DbManagerRegionConst.Denied.ID
      )
      .map(
        (item: MbsBscHttpGetManyRegionRspItemMdl) =>
          ({
            managerRegionID: item.managerRegionID,
            managerRegionName: item.managerRegionName,
            managerRegionIsEnable: item.managerRegionIsEnable,
          }) satisfies GetManyManagerRegionComboBoxItemMdl
      ) ?? [];

  // 基於原始資料數量判斷是否還有更多資料
  const hasMore = originalCount >= PAGE_SIZE;

  return { filteredList, hasMore };
};

/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery?.trim() || "";

  // 清空選擇
  managerRegionID.value = null;
  managerRegionName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerRegionComboBoxViewObj.pageIndex = 1;
    getManyManagerRegionComboBoxViewObj.managerRegionList = [];
    getManyManagerRegionComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const { filteredList, hasMore } = await getData(getManyManagerRegionComboBoxViewObj.pageIndex);

    getManyManagerRegionComboBoxViewObj.managerRegionList.push(...filteredList);

    getManyManagerRegionComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerRegionComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerRegionComboBoxViewObj.pageIndex = 1;
  getManyManagerRegionComboBoxViewObj.managerRegionList = [];
  getManyManagerRegionComboBoxViewObj.hasMoreData = true;

  const { filteredList, hasMore } = await getData(getManyManagerRegionComboBoxViewObj.pageIndex);

  getManyManagerRegionComboBoxViewObj.managerRegionList.push(...filteredList);
  getManyManagerRegionComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) {
    getManyManagerRegionComboBoxViewObj.pageIndex++;
  }
}, 300);

/** focus輸入框  */
const focusInput = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery = null;

  // 清空選擇的值
  managerRegionID.value = null;
  managerRegionName.value = null;

  // 重設資料
  getManyManagerRegionComboBoxViewObj.showDropdown = true;
  getManyManagerRegionComboBoxViewObj.pageIndex = 1;
  getManyManagerRegionComboBoxViewObj.managerRegionList = [];
  getManyManagerRegionComboBoxViewObj.hasMoreData = true;

  // 查詢資料
  const { filteredList, hasMore } = await getData(getManyManagerRegionComboBoxViewObj.pageIndex);

  if (filteredList.length === 0) {
    getManyManagerRegionComboBoxViewObj.managerRegionList = [];
    getManyManagerRegionComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerRegionComboBoxViewObj.managerRegionList.push(...filteredList);
  getManyManagerRegionComboBoxViewObj.hasMoreData = hasMore;

  if (hasMore) {
    getManyManagerRegionComboBoxViewObj.pageIndex++;
  }
};

/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  // 取得滾動元素
  const dropdown = event.target as HTMLElement;

  // 判斷是否接近底部
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerRegionComboBoxViewObj.hasMoreData &&
    !getManyManagerRegionComboBoxViewObj.isLoading
  ) {
    getManyManagerRegionComboBoxViewObj.isLoading = true;

    // 查詢資料
    const { filteredList, hasMore } = await getData(getManyManagerRegionComboBoxViewObj.pageIndex);

    // 過濾出尚未加入的資料
    const uniqueNewList: GetManyManagerRegionComboBoxItemMdl[] = filteredList.filter(
      (newItem) =>
        !getManyManagerRegionComboBoxViewObj.managerRegionList.some(
          (existing) => existing.managerRegionID === newItem.managerRegionID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerRegionComboBoxViewObj.managerRegionList.push(...uniqueNewList);
    getManyManagerRegionComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerRegionComboBoxViewObj.pageIndex++;
    }

    getManyManagerRegionComboBoxViewObj.isLoading = false;
  }
};

/** 選取項目 */
const selectItem = (item: GetManyManagerRegionComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery = item.managerRegionName;

  // 帶出ID、名稱
  managerRegionID.value = item.managerRegionID;
  managerRegionName.value = item.managerRegionName;

  // 關閉下拉選單
  getManyManagerRegionComboBoxViewObj.showDropdown = false;
};

/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 切換狀態
  getManyManagerRegionComboBoxViewObj.showDropdown =
    !getManyManagerRegionComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerRegionComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery = null;

    // 清空選擇的值
    managerRegionID.value = null;
    managerRegionName.value = null;

    getManyManagerRegionComboBoxViewObj.pageIndex = 1;
    getManyManagerRegionComboBoxViewObj.hasMoreData = true;
    getManyManagerRegionComboBoxViewObj.managerRegionList = [];

    const { filteredList, hasMore } = await getData(getManyManagerRegionComboBoxViewObj.pageIndex);

    // 加入現有清單
    getManyManagerRegionComboBoxViewObj.managerRegionList.push(...filteredList);
    getManyManagerRegionComboBoxViewObj.hasMoreData = hasMore;

    if (hasMore) {
      getManyManagerRegionComboBoxViewObj.pageIndex++;
    }
  }
};

/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerRegionComboBoxViewObj.showDropdown = false;
};
// -------------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerRegionName.value,
  (newValue) => {
    getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery = newValue || null;

    // 如果清空名稱，也清空 ID
    if (!newValue) {
      managerRegionID.value = null;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerRegionComboBoxViewObj.managerRegionKeywordQuery"
      type="text"
      placeholder="查詢地區"
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
        v-if="getManyManagerRegionComboBoxViewObj.showDropdown"
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

    <transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2"
      enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2"
    >
      <div
        v-if="getManyManagerRegionComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerRegionComboBoxViewObj.managerRegionList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerRegionComboBoxViewObj.managerRegionList"
            :key="item.managerRegionID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerRegionName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
