<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { useTokenStore } from "@/stores/token";
import type {
  MbsBscHttpGetManyProductMainKindReqMdl,
  MbsBscHttpGetManyProductMainKindRspItemMdl,
  MbsBscHttpGetManyProductMainKindRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicProductMainKind } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerProductMainKindID = defineModel<number | null>("managerProductMainKindID");
const managerProductMainKindName = defineModel<string | null>("managerProductMainKindName");
//-------------------------------------------------------------------
/** 取得多筆產品主分類-文字查詢框-頁面模型 */
interface GetManyManagerProductMainKindComboBoxViewMdl {
  pageIndex: number;
  managerProductMainKindKeywordQuery: string | null;
  managerProductMainKindIsEnableQuery: boolean | null;
  managerProductMainKindList: GetManyManagerProductMainKindComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆產品主分類-文字查詢框-項目模型 */
interface GetManyManagerProductMainKindComboBoxItemMdl {
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductMainKindIsEnable: boolean;
}
//-------------------------------------------------------------------
/** 取得多筆產品主分類-文字查詢框-頁面物件 */
const getManyManagerProductMainKindComboBoxViewObj =
  reactive<GetManyManagerProductMainKindComboBoxViewMdl>({
    pageIndex: 1,
    managerProductMainKindKeywordQuery: null,
    managerProductMainKindIsEnableQuery: true,
    managerProductMainKindList: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });
//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  // 檢查資料
  if (!tokenStore.token) return [];

  // 呼叫api
  const requestData: MbsBscHttpGetManyProductMainKindReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerProductMainKindName:
      getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery,
    managerProductMainKindIsEnable:
      getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindIsEnableQuery,
    pageIndex,
  };

  const responseData: MbsBscHttpGetManyProductMainKindRspMdl | null =
    await getManyBasicProductMainKind(requestData);

  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  // 處理資料
  const fetchedList: GetManyManagerProductMainKindComboBoxItemMdl[] =
    responseData.managerProductMainKindList.map(
      (item: MbsBscHttpGetManyProductMainKindRspItemMdl) =>
        ({
          managerProductMainKindID: item.managerProductMainKindID,
          managerProductMainKindName: item.managerProductMainKindName,
          managerProductMainKindIsEnable: item.managerProductMainKindIsEnable,
        }) satisfies GetManyManagerProductMainKindComboBoxItemMdl
    ) ?? [];

  return fetchedList;
};

//-------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue =
    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery?.trim() || "";

  // 清空選擇
  managerProductMainKindID.value = null;
  managerProductMainKindName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerProductMainKindComboBoxViewObj.pageIndex = 1;
    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList = [];
    getManyManagerProductMainKindComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerProductMainKindComboBoxItemMdl[] = await getData(
      getManyManagerProductMainKindComboBoxViewObj.pageIndex
    );

    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.push(...fetchedList);

    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerProductMainKindComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerProductMainKindComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerProductMainKindComboBoxViewObj.pageIndex = 1;
  getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList = [];
  getManyManagerProductMainKindComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerProductMainKindComboBoxItemMdl[] = await getData(
    getManyManagerProductMainKindComboBoxViewObj.pageIndex
  );

  getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerProductMainKindComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerProductMainKindComboBoxViewObj.pageIndex++;
  }
}, 300);
//-------------------------------------------------------------------
/** focus 輸入框時 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery = null;

  // 清空選擇的值
  managerProductMainKindID.value = null;
  managerProductMainKindName.value = null;

  // 重設資料
  getManyManagerProductMainKindComboBoxViewObj.showDropdown = true;
  getManyManagerProductMainKindComboBoxViewObj.pageIndex = 1;
  getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList = [];
  getManyManagerProductMainKindComboBoxViewObj.hasMoreData = true;

  // 查詢資料
  const fetchedList: GetManyManagerProductMainKindComboBoxItemMdl[] = await getData(
    getManyManagerProductMainKindComboBoxViewObj.pageIndex
  );

  if (fetchedList.length === 0) {
    getManyManagerProductMainKindComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerProductMainKindComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerProductMainKindComboBoxViewObj.pageIndex++;
  }
};
//-------------------------------------------------------------------
/** 滾動載入更多 */
const scrollDropdown = async (event: Event) => {
  // 取得滾動元素
  const dropdown = event.target as HTMLElement;

  // 判斷是否接近底部
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerProductMainKindComboBoxViewObj.hasMoreData &&
    !getManyManagerProductMainKindComboBoxViewObj.isLoading
  ) {
    getManyManagerProductMainKindComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerProductMainKindComboBoxItemMdl[] = await getData(
      getManyManagerProductMainKindComboBoxViewObj.pageIndex
    );

    // 過濾出尚未加入的資料
    const uniqueNewProductMainKindList: GetManyManagerProductMainKindComboBoxItemMdl[] =
      fetchedList.filter(
        (newItem) =>
          !getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.some(
            (existing) => existing.managerProductMainKindID === newItem.managerProductMainKindID
          )
      );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.push(
      ...uniqueNewProductMainKindList
    );

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerProductMainKindComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerProductMainKindComboBoxViewObj.pageIndex++;
    }

    getManyManagerProductMainKindComboBoxViewObj.isLoading = false;
  }
};

//-------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerProductMainKindComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery =
    item.managerProductMainKindName;

  // 帶出ID、名稱
  managerProductMainKindID.value = item.managerProductMainKindID;
  managerProductMainKindName.value = item.managerProductMainKindName;

  // 關閉下拉選單
  getManyManagerProductMainKindComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------------
/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  // 切換狀態
  getManyManagerProductMainKindComboBoxViewObj.showDropdown =
    !getManyManagerProductMainKindComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerProductMainKindComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery = null;

    // 清空選擇的值
    managerProductMainKindID.value = null;
    managerProductMainKindName.value = null;

    getManyManagerProductMainKindComboBoxViewObj.pageIndex = 1;
    getManyManagerProductMainKindComboBoxViewObj.hasMoreData = true;
    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList = [];

    const fetchedList: GetManyManagerProductMainKindComboBoxItemMdl[] = await getData(
      getManyManagerProductMainKindComboBoxViewObj.pageIndex
    );

    // 加入現有清單
    getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.push(...fetchedList);
    getManyManagerProductMainKindComboBoxViewObj.pageIndex++;
  }
};

/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerProductMainKindComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerProductMainKindName.value,
  (newValue) => {
    if (!newValue) {
      managerProductMainKindID.value = null;
      getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery = null; 
    } else {
      getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery = newValue;
    }
  },
  { immediate: true }
);

</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindKeywordQuery"
      type="text"
      placeholder="查詢產品主分類"
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
        v-if="getManyManagerProductMainKindComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerProductMainKindComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul
          v-if="
            getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList.length === 0
          "
        >
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerProductMainKindComboBoxViewObj.managerProductMainKindList"
            :key="item.managerProductMainKindID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerProductMainKindName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
