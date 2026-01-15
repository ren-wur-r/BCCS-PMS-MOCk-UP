<script setup lang="ts">
import { reactive, watch } from "vue";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { useTokenStore } from "@/stores/token";
import type {
  MbsBscHttpGetManyDepartmentReqMdl,
  MbsBscHttpGetManyDepartmentRspItemMdl,
  MbsBscHttpGetManyDepartmentRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicManagerDepartment } from "@/services";
import { debounce } from "lodash-es";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerDepartmentID = defineModel<number | null>("managerDepartmentID");
const managerDepartmentName = defineModel<string | null>("managerDepartmentName");
//-------------------------------------------------------------------
/** 取得多筆管理者部門-文字查詢框-頁面模型 */
interface GetManyManagerDepartmentComboBoxViewMdl {
  pageIndex: number;
  managerDepartmentKeywordQuery: string | null;
  managerDepartmentIsEnableQuery: boolean | null;
  managerDepartmentList: GetManyManagerDepartmentComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆管理者部門-文字查詢框-項目模型 */
interface GetManyManagerDepartmentComboBoxItemMdl {
  managerDepartmentID: number;
  managerDepartmentName: string;
  managerDepartmentIsEnable: boolean;
}
/** 取得多筆管理者部門-文字查詢框-頁面物件 */
const getManyManagerDepartmentComboBoxViewObj = reactive<GetManyManagerDepartmentComboBoxViewMdl>({
  pageIndex: 1,
  managerDepartmentKeywordQuery: null,
  managerDepartmentIsEnableQuery: null,
  managerDepartmentList: [],
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
  const requestData: MbsBscHttpGetManyDepartmentReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerDepartmentName: getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery,
    managerDepartmentIsEnable:
      getManyManagerDepartmentComboBoxViewObj.managerDepartmentIsEnableQuery,
    pageIndex: pageIndex,
  };
  const responseData: MbsBscHttpGetManyDepartmentRspMdl | null =
    await getManyBasicManagerDepartment(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  // 處理資料
  const fetchedList: GetManyManagerDepartmentComboBoxItemMdl[] =
    responseData.managerDepartmentList.map(
      (item: MbsBscHttpGetManyDepartmentRspItemMdl) =>
        ({
          managerDepartmentID: item.managerDepartmentID,
          managerDepartmentName: item.managerDepartmentName,
          managerDepartmentIsEnable: item.managerDepartmentIsEnable,
        }) satisfies GetManyManagerDepartmentComboBoxItemMdl
    ) ?? [];
  return fetchedList;
};

/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue =
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery?.trim() || "";

  // 清空選擇
  managerDepartmentID.value = null;
  managerDepartmentName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerDepartmentComboBoxViewObj.pageIndex = 1;
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentList = [];
    getManyManagerDepartmentComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerDepartmentComboBoxItemMdl[] = await getData(
      getManyManagerDepartmentComboBoxViewObj.pageIndex
    );

    getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.push(...fetchedList);

    if (getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.length < PAGE_SIZE) {
      getManyManagerDepartmentComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerDepartmentComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerDepartmentComboBoxViewObj.pageIndex = 1;
  getManyManagerDepartmentComboBoxViewObj.managerDepartmentList = [];
  getManyManagerDepartmentComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerDepartmentComboBoxItemMdl[] = await getData(
    getManyManagerDepartmentComboBoxViewObj.pageIndex
  );

  getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerDepartmentComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerDepartmentComboBoxViewObj.pageIndex++;
  }
}, 300);

/** focus輸入框時  */
const focusInput = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery = null;

  // 清空選擇的值
  managerDepartmentID.value = null;
  managerDepartmentName.value = null;

  // 重設資料
  getManyManagerDepartmentComboBoxViewObj.showDropdown = true;
  getManyManagerDepartmentComboBoxViewObj.pageIndex = 1;
  getManyManagerDepartmentComboBoxViewObj.managerDepartmentList = [];
  getManyManagerDepartmentComboBoxViewObj.hasMoreData = true;

  // 查詢資料
  const fetchedList: GetManyManagerDepartmentComboBoxItemMdl[] = await getData(
    getManyManagerDepartmentComboBoxViewObj.pageIndex
  );

  if (fetchedList.length === 0) {
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentList = [];
    getManyManagerDepartmentComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerDepartmentComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerDepartmentComboBoxViewObj.pageIndex++;
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
    getManyManagerDepartmentComboBoxViewObj.hasMoreData &&
    !getManyManagerDepartmentComboBoxViewObj.isLoading
  ) {
    getManyManagerDepartmentComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerDepartmentComboBoxItemMdl[] = await getData(
      getManyManagerDepartmentComboBoxViewObj.pageIndex
    );

    // 過濾出尚未加入的資料
    const uniqueNewDepartmentList: GetManyManagerDepartmentComboBoxItemMdl[] = fetchedList.filter(
      (newItem) =>
        !getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.some(
          (existing) => existing.managerDepartmentID === newItem.managerDepartmentID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.push(...uniqueNewDepartmentList);

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerDepartmentComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerDepartmentComboBoxViewObj.pageIndex++;
    }

    getManyManagerDepartmentComboBoxViewObj.isLoading = false;
  }
};

/** 選取項目 */
const selectItem = (item: GetManyManagerDepartmentComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery =
    item.managerDepartmentName;

  // 帶出ID、名稱
  managerDepartmentID.value = item.managerDepartmentID;
  managerDepartmentName.value = item.managerDepartmentName;

  // 關閉下拉選單
  getManyManagerDepartmentComboBoxViewObj.showDropdown = false;
};

/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 切換狀態
  getManyManagerDepartmentComboBoxViewObj.showDropdown =
    !getManyManagerDepartmentComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerDepartmentComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery = null;

    // 清空選擇的值
    managerDepartmentID.value = null;
    managerDepartmentName.value = null;

    getManyManagerDepartmentComboBoxViewObj.pageIndex = 1;
    getManyManagerDepartmentComboBoxViewObj.hasMoreData = true;
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentList = [];

    const fetchedList: GetManyManagerDepartmentComboBoxItemMdl[] = await getData(
      getManyManagerDepartmentComboBoxViewObj.pageIndex
    );

    // 加入現有清單
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.push(...fetchedList);
    getManyManagerDepartmentComboBoxViewObj.pageIndex++;
  }
};

/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerDepartmentComboBoxViewObj.showDropdown = false;
};

// -------------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerDepartmentName.value,
  (newValue) => {
    getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery = newValue || null;

    // 如果清空名稱，也清空 ID
    if (!newValue) {
      managerDepartmentID.value = null;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerDepartmentComboBoxViewObj.managerDepartmentKeywordQuery"
      type="text"
      placeholder="查詢部門"
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
        v-if="getManyManagerDepartmentComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerDepartmentComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerDepartmentComboBoxViewObj.managerDepartmentList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerDepartmentComboBoxViewObj.managerDepartmentList"
            :key="item.managerDepartmentID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerDepartmentName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
