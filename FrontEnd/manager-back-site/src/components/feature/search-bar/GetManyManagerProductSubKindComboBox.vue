<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyProductSubKindReqMdl,
  MbsBscHttpGetManyProductSubKindRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicProductSubKind } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
  managerProductMainKindID: number | null;
}>();
const managerProductSubKindID = defineModel<number | null>("managerProductSubKindID");
const managerProductSubKindName = defineModel<string | null>("managerProductSubKindName");
//-------------------------------------------------------------------
/** 取得多筆產品子分類-文字查詢框-頁面模型 */
interface GetManyManagerProductSubKindComboBoxViewMdl {
  pageIndex: number;
  managerProductSubKindKeywordQuery: string;
  managerProductSubKindList: GetManyManagerProductSubKindComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆產品子分類-文字查詢框-項目模型 */
interface GetManyManagerProductSubKindComboBoxItemMdl {
  managerProductSubKindID: number;
  managerProductSubKindName: string;
  managerProductSubKindIsEnable: boolean;
}
//-------------------------------------------------------------------
/** 取得多筆產品子分類-文字查詢框-頁面物件 */
const getManyManagerProductSubKindComboBoxViewObj =
  reactive<GetManyManagerProductSubKindComboBoxViewMdl>({
    pageIndex: 1,
    managerProductSubKindKeywordQuery: "",
    managerProductSubKindList: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });
//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  if (!tokenStore.token) return [];

  const requestData: MbsBscHttpGetManyProductSubKindReqMdl = {
    employeeLoginToken: tokenStore.token,
    productMainKindID: props.managerProductMainKindID ?? null,
    productSubKindName:
      getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery,
    productSubKindIsEnable: true,
    pageIndex,
  };

  const responseData = await getManyBasicProductSubKind(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  // 處理資料
  const fetchedList =
    responseData.productSubKindList?.map(
      (item: MbsBscHttpGetManyProductSubKindRspItemMdl) =>
        ({
          managerProductSubKindID: item.productSubKindID,
          managerProductSubKindName: item.productSubKindName,
          managerProductSubKindIsEnable: item.productSubKindIsEnable,
        }) satisfies GetManyManagerProductSubKindComboBoxItemMdl
    ) ?? [];
  return fetchedList;
};
//-------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue =
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery?.trim() || "";

  managerProductSubKindID.value = null;
  managerProductSubKindName.value = null;

  // 少於一個字時查全部
  if (inputValue.length < 1) {
    getManyManagerProductSubKindComboBoxViewObj.pageIndex = 1;
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList = [];
    getManyManagerProductSubKindComboBoxViewObj.hasMoreData = true;

    const fetchedList = await getData(getManyManagerProductSubKindComboBoxViewObj.pageIndex);
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.push(...fetchedList);

    if (fetchedList.length < PAGE_SIZE)
      getManyManagerProductSubKindComboBoxViewObj.hasMoreData = false;
    else getManyManagerProductSubKindComboBoxViewObj.pageIndex++;

    return;
  }

  // 有輸入文字
  getManyManagerProductSubKindComboBoxViewObj.pageIndex = 1;
  getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList = [];
  getManyManagerProductSubKindComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerProductSubKindComboBoxViewObj.pageIndex);
  getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE)
    getManyManagerProductSubKindComboBoxViewObj.hasMoreData = false;
  else getManyManagerProductSubKindComboBoxViewObj.pageIndex++;
}, 300);

//-------------------------------------------------------------------
/** focus輸入框時  */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery = "";

  // 清空選擇的值
  managerProductSubKindID.value = null;
  managerProductSubKindName.value = null;

  getManyManagerProductSubKindComboBoxViewObj.showDropdown = true;
  getManyManagerProductSubKindComboBoxViewObj.pageIndex = 1;
  getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList = [];
  getManyManagerProductSubKindComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerProductSubKindComboBoxItemMdl[] = await getData(
    getManyManagerProductSubKindComboBoxViewObj.pageIndex
  );
  if (fetchedList.length === 0) {
    getManyManagerProductSubKindComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE)
    getManyManagerProductSubKindComboBoxViewObj.hasMoreData = false;
  else getManyManagerProductSubKindComboBoxViewObj.pageIndex++;
};

//-------------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  // 取得滾動元素
  const dropdown = event.target as HTMLElement;

  // 判斷是否接近底部
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerProductSubKindComboBoxViewObj.hasMoreData &&
    !getManyManagerProductSubKindComboBoxViewObj.isLoading
  ) {
    getManyManagerProductSubKindComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerProductSubKindComboBoxItemMdl[] = await getData(
      getManyManagerProductSubKindComboBoxViewObj.pageIndex
    );

    // 過濾出尚未加入的資料
    const uniqueNewList = fetchedList.filter(
      (newItem) =>
        !getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.some(
          (existing) => existing.managerProductSubKindID === newItem.managerProductSubKindID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.push(...uniqueNewList);

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE)
      getManyManagerProductSubKindComboBoxViewObj.hasMoreData = false;
    else getManyManagerProductSubKindComboBoxViewObj.pageIndex++;

    getManyManagerProductSubKindComboBoxViewObj.isLoading = false;
  }
};
//-------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerProductSubKindComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery =
    item.managerProductSubKindName;

  // 帶出ID、名稱
  managerProductSubKindID.value = item.managerProductSubKindID;
  managerProductSubKindName.value = item.managerProductSubKindName;

  // 關閉下拉選單
  getManyManagerProductSubKindComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------------
/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  // 切換狀態
  getManyManagerProductSubKindComboBoxViewObj.showDropdown =
    !getManyManagerProductSubKindComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerProductSubKindComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery = "";

    // 清空選擇的值
    managerProductSubKindID.value = null;
    managerProductSubKindName.value = null;

    getManyManagerProductSubKindComboBoxViewObj.pageIndex = 1;
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList = [];
    getManyManagerProductSubKindComboBoxViewObj.hasMoreData = true;

    const fetchedList: GetManyManagerProductSubKindComboBoxItemMdl[] = await getData(
      getManyManagerProductSubKindComboBoxViewObj.pageIndex
    );

    // 加入現有清單
    getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.push(...fetchedList);
    getManyManagerProductSubKindComboBoxViewObj.pageIndex++;
  }
};
//-------------------------------------------------------------------
/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerProductSubKindComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerProductSubKindName.value,
  (newValue) => {
    if (!newValue) {
      managerProductSubKindID.value = null;
      getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery = "";
    } else {
      getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery = newValue;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindKeywordQuery"
      type="text"
      placeholder="查詢產品子分類"
      :disabled="props.disabled"
      class="input-box rounded-r-none"
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
        v-if="getManyManagerProductSubKindComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerProductSubKindComboBoxViewObj.showDropdown"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul
          v-if="getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList.length === 0"
        >
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerProductSubKindComboBoxViewObj.managerProductSubKindList"
            :key="item.managerProductSubKindID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerProductSubKindName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
