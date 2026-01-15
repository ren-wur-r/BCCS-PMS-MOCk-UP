<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyCompanyMainKindReqMdl,
  MbsBscHttpGetManyCompanyMainKindRspItemMdl,
  MbsBscHttpGetManyCompanyMainKindRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicManagerCompanyMainKind } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerCompanyMainKindID = defineModel<number | null>("managerCompanyMainKindID");
const managerCompanyMainKindName = defineModel<string | null>("managerCompanyMainKindName");
//-------------------------------------------------------------
/** 取得多筆公司主分類-文字查詢框-頁面模型 */
interface GetManyManagerCompanyMainKindComboBoxViewMdl {
  pageIndex: number;
  managerCompanyMainKindKeywordQuery: string | null;
  managerCompanyMainKindList: GetManyManagerCompanyMainKindComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆公司主分類-文字查詢框-項目模型 */
interface GetManyManagerCompanyMainKindComboBoxItemMdl {
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
  managerCompanyMainKindIsEnable: boolean;
}
//-------------------------------------------------------------
/** 取得多筆公司主分類-文字查詢框-頁面物件 */
const getManyManagerCompanyMainKindComboBoxViewObj =
  reactive<GetManyManagerCompanyMainKindComboBoxViewMdl>({
    pageIndex: 1,
    managerCompanyMainKindKeywordQuery: null,
    managerCompanyMainKindList: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });
//-------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  // 檢查資料
  if (!tokenStore.token) return [];

  // 呼叫api
  const requestData: MbsBscHttpGetManyCompanyMainKindReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerProductMainKindName:
      getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery,
    pageIndex,
  };
  const responseData: MbsBscHttpGetManyCompanyMainKindRspMdl | null =
    await getManyBasicManagerCompanyMainKind(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  // 處理資料
  const fetchedList: GetManyManagerCompanyMainKindComboBoxItemMdl[] =
    responseData.managerCompanyMainKindList?.map(
      (item: MbsBscHttpGetManyCompanyMainKindRspItemMdl) =>
        ({
          managerCompanyMainKindID: item.managerCompanyMainKindID,
          managerCompanyMainKindName: item.managerCompanyMainKindName,
          managerCompanyMainKindIsEnable: item.managerCompanyMainKindIsEnable,
        }) satisfies GetManyManagerCompanyMainKindComboBoxItemMdl
    ) ?? [];
  return fetchedList;
};
//-------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue =
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery?.trim() || "";

  // 清空選擇
  managerCompanyMainKindID.value = null;
  managerCompanyMainKindName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerCompanyMainKindComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList = [];
    getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerCompanyMainKindComboBoxItemMdl[] = await getData(
      getManyManagerCompanyMainKindComboBoxViewObj.pageIndex
    );

    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.push(...fetchedList);

    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerCompanyMainKindComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerCompanyMainKindComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList = [];
  getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerCompanyMainKindComboBoxItemMdl[] = await getData(
    getManyManagerCompanyMainKindComboBoxViewObj.pageIndex
  );

  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerCompanyMainKindComboBoxViewObj.pageIndex++;
  }
}, 300);
//-------------------------------------------------------------
/** focus輸入框時 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery = null;

  // 清空選擇的值
  managerCompanyMainKindID.value = null;
  managerCompanyMainKindName.value = null;

  // 重設資料
  getManyManagerCompanyMainKindComboBoxViewObj.showDropdown = true;
  getManyManagerCompanyMainKindComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList = [];
  getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = true;

  // 查詢資料
  const fetchedList: GetManyManagerCompanyMainKindComboBoxItemMdl[] = await getData(
    getManyManagerCompanyMainKindComboBoxViewObj.pageIndex
  );

  if (fetchedList.length === 0) {
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList = [];
    getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE)
    getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = false;
  else getManyManagerCompanyMainKindComboBoxViewObj.pageIndex++;
};
//-------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  // 取得滾動元素
  const dropdown = event.target as HTMLElement;

  // 判斷是否接近底部
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData &&
    !getManyManagerCompanyMainKindComboBoxViewObj.isLoading
  ) {
    getManyManagerCompanyMainKindComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerCompanyMainKindComboBoxItemMdl[] = await getData(
      getManyManagerCompanyMainKindComboBoxViewObj.pageIndex
    );

    // 過濾出尚未加入的資料
    const uniqueNewList: GetManyManagerCompanyMainKindComboBoxItemMdl[] = fetchedList.filter(
      (newItem) =>
        !getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.some(
          (exist) => exist.managerCompanyMainKindID === newItem.managerCompanyMainKindID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.push(...uniqueNewList);

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE)
      getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = false;
    else getManyManagerCompanyMainKindComboBoxViewObj.pageIndex++;

    getManyManagerCompanyMainKindComboBoxViewObj.isLoading = false;
  }
};
//-------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerCompanyMainKindComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery =
    item.managerCompanyMainKindName;

  // 帶出ID、名稱
  managerCompanyMainKindID.value = item.managerCompanyMainKindID;
  managerCompanyMainKindName.value = item.managerCompanyMainKindName;

  // 關閉下拉選單
  getManyManagerCompanyMainKindComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------
/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  // 切換狀態
  getManyManagerCompanyMainKindComboBoxViewObj.showDropdown =
    !getManyManagerCompanyMainKindComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerCompanyMainKindComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery = null;

    // 清空選擇的值
    managerCompanyMainKindID.value = null;
    managerCompanyMainKindName.value = null;

    getManyManagerCompanyMainKindComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList = [];
    getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = true;

    const fetchedList: GetManyManagerCompanyMainKindComboBoxItemMdl[] = await getData(
      getManyManagerCompanyMainKindComboBoxViewObj.pageIndex
    );

    // 加入現有清單
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.push(...fetchedList);
    getManyManagerCompanyMainKindComboBoxViewObj.pageIndex++;
  }
};
//-------------------------------------------------------------
/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerCompanyMainKindComboBoxViewObj.showDropdown = false;
};

/** 對外提供清除方法（父層用） */
const clear = () => {
  // 清空輸入框文字
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery = null;

  // 清空選擇
  managerCompanyMainKindID.value = null;
  managerCompanyMainKindName.value = null;

  // 重置清單與狀態
  getManyManagerCompanyMainKindComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList = [];
  getManyManagerCompanyMainKindComboBoxViewObj.hasMoreData = true;
  getManyManagerCompanyMainKindComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------
/** 監聽當有名稱時直接帶入 */
watch(
  () => managerCompanyMainKindName.value,
  (newValue) => {
    getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery =
      newValue || null;
    // 如果清空名稱，也清空 ID
    if (!newValue) managerCompanyMainKindID.value = null;
  },
  { immediate: true }
);

defineExpose({
  clear,
});
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindKeywordQuery"
      type="text"
      placeholder="查詢客戶主分類"
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
        v-if="getManyManagerCompanyMainKindComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerCompanyMainKindComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul
          v-if="
            getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList.length === 0
          "
        >
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerCompanyMainKindComboBoxViewObj.managerCompanyMainKindList"
            :key="item.managerCompanyMainKindID"   :disabled="props.disabled"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerCompanyMainKindName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
