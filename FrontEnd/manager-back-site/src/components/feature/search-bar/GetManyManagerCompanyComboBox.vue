<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyCompanyReqMdl,
  MbsBscHttpGetManyCompanyRspItemMdl,
  MbsBscHttpGetManyCompanyRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicManagerCompany } from "@/services";
import { mockDataSets } from "@/services/mockApi/mockDataSets";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
const useMockData = true;
//-------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerCompanyID = defineModel<number | null>("managerCompanyID");
const managerCompanyName = defineModel<string | null>("managerCompanyName");
//-------------------------------------------------------------
/** 取得多筆管理者公司-文字查詢框-頁面模型 */
interface GetManyManagerCompanyComboBoxViewMdl {
  pageIndex: number;
  managerCompanyKeywordQuery: string | null;
  searchingKeyword: string | null; // 用於記錄搜尋時的關鍵字 (與顯示的 input 分開)
  managerCompanyList: GetManyManagerCompanyComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆管理者公司-文字查詢框-頁面模型 */
interface GetManyManagerCompanyComboBoxItemMdl {
  managerCompanyID: number;
  managerCompanyName: string;
}
//-------------------------------------------------------------
/** 取得多筆管理者公司-文字查詢框-頁面物件 */
const getManyManagerCompanyComboBoxViewObj = reactive<GetManyManagerCompanyComboBoxViewMdl>({
  pageIndex: 1,
  managerCompanyKeywordQuery: null,
  searchingKeyword: null,
  managerCompanyList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});
//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number, keyword: string | null = null) => {
  if (!tokenStore.token && !useMockData) return [];

  // 呼叫api
  const requestData: MbsBscHttpGetManyCompanyReqMdl = {
    employeeLoginToken: tokenStore.token || "mock-token",
    companyName: keyword,
    pageIndex,
  };

  const responseData: MbsBscHttpGetManyCompanyRspMdl | null =
    await getManyBasicManagerCompany(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) {
    const fallback = mockDataSets.companies.map((item, index) => ({
      managerCompanyID: Number(String(item.CompanyID).replace(/\D/g, "")) || index + 1,
      managerCompanyName: item.CompanyName,
    }));
    return fallback;
  }

  const mapped =
    responseData.companyList?.map(
      (item: MbsBscHttpGetManyCompanyRspItemMdl) =>
        ({
          managerCompanyID: item.companyID,
          managerCompanyName: item.companyName,
        }) satisfies GetManyManagerCompanyComboBoxItemMdl
    ) ?? [];

  if (mapped.length > 0) return mapped;

  return mockDataSets.companies.map((item, index) => ({
    managerCompanyID: Number(String(item.CompanyID).replace(/\D/g, "")) || index + 1,
    managerCompanyName: item.CompanyName,
  }));
};
//------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerCompanyComboBoxViewObj.managerCompanyKeywordQuery?.trim() || "";
  getManyManagerCompanyComboBoxViewObj.searchingKeyword = inputValue;

  // 清空選擇
  managerCompanyID.value = null;
  managerCompanyName.value = null;

  // 若輸入少於 1 字 → 查全部
  if (inputValue.length < 1) {
    getManyManagerCompanyComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyComboBoxViewObj.managerCompanyList = [];
    getManyManagerCompanyComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerCompanyComboBoxItemMdl[] = await getData(
      getManyManagerCompanyComboBoxViewObj.pageIndex,
      getManyManagerCompanyComboBoxViewObj.searchingKeyword
    );

    getManyManagerCompanyComboBoxViewObj.managerCompanyList.push(...fetchedList);

    if (getManyManagerCompanyComboBoxViewObj.managerCompanyList.length < PAGE_SIZE)
      getManyManagerCompanyComboBoxViewObj.hasMoreData = false;
    else {
      getManyManagerCompanyComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerCompanyComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyComboBoxViewObj.managerCompanyList = [];
  getManyManagerCompanyComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerCompanyComboBoxItemMdl[] = await getData(
    getManyManagerCompanyComboBoxViewObj.pageIndex,
    getManyManagerCompanyComboBoxViewObj.searchingKeyword
  );
  getManyManagerCompanyComboBoxViewObj.managerCompanyList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) getManyManagerCompanyComboBoxViewObj.hasMoreData = false;
  else getManyManagerCompanyComboBoxViewObj.pageIndex++;
}, 300);

//-------------------------------------------------------------
/** focus輸入框時  */
const focusInput = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerCompanyComboBoxViewObj.managerCompanyKeywordQuery = null;

  // 清空選擇的值
  managerCompanyID.value = null;
  managerCompanyName.value = null;

  // 重設資料
  getManyManagerCompanyComboBoxViewObj.showDropdown = true;
  getManyManagerCompanyComboBoxViewObj.pageIndex = 1;
  getManyManagerCompanyComboBoxViewObj.managerCompanyList = [];
  getManyManagerCompanyComboBoxViewObj.hasMoreData = true;
  getManyManagerCompanyComboBoxViewObj.searchingKeyword = "";

  // 查詢資料
  const fetchedList: GetManyManagerCompanyComboBoxItemMdl[] = await getData(
    getManyManagerCompanyComboBoxViewObj.pageIndex,
    getManyManagerCompanyComboBoxViewObj.searchingKeyword
  );

  if (fetchedList.length === 0) {
    getManyManagerCompanyComboBoxViewObj.managerCompanyList = [];
    getManyManagerCompanyComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerCompanyComboBoxViewObj.managerCompanyList.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE) getManyManagerCompanyComboBoxViewObj.hasMoreData = false;
  else getManyManagerCompanyComboBoxViewObj.pageIndex++;
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
    getManyManagerCompanyComboBoxViewObj.hasMoreData &&
    !getManyManagerCompanyComboBoxViewObj.isLoading
  ) {
    getManyManagerCompanyComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerCompanyComboBoxItemMdl[] = await getData(
      getManyManagerCompanyComboBoxViewObj.pageIndex,
      getManyManagerCompanyComboBoxViewObj.searchingKeyword
    );

    // 過濾出尚未加入的資料
    const uniqueNewList: GetManyManagerCompanyComboBoxItemMdl[] = fetchedList.filter(
      (newItem) =>
        !getManyManagerCompanyComboBoxViewObj.managerCompanyList.some(
          (exist) => exist.managerCompanyID === newItem.managerCompanyID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerCompanyComboBoxViewObj.managerCompanyList.push(...uniqueNewList);

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE) getManyManagerCompanyComboBoxViewObj.hasMoreData = false;
    else getManyManagerCompanyComboBoxViewObj.pageIndex++;

    getManyManagerCompanyComboBoxViewObj.isLoading = false;
  }
};

//-------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerCompanyComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerCompanyComboBoxViewObj.managerCompanyKeywordQuery = item.managerCompanyName;

  // 帶出ID、名稱
  managerCompanyID.value = item.managerCompanyID;
  managerCompanyName.value = item.managerCompanyName;

  // 關閉下拉選單
  getManyManagerCompanyComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------
/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  // 切換狀態
  getManyManagerCompanyComboBoxViewObj.showDropdown =
    !getManyManagerCompanyComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerCompanyComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerCompanyComboBoxViewObj.managerCompanyKeywordQuery = null;

    // 清空選擇的值
    managerCompanyID.value = null;
    managerCompanyName.value = null;

    getManyManagerCompanyComboBoxViewObj.pageIndex = 1;
    getManyManagerCompanyComboBoxViewObj.managerCompanyList = [];
    getManyManagerCompanyComboBoxViewObj.hasMoreData = true;
    getManyManagerCompanyComboBoxViewObj.searchingKeyword = "";

    const fetchedList: GetManyManagerCompanyComboBoxItemMdl[] = await getData(
      getManyManagerCompanyComboBoxViewObj.pageIndex,
      getManyManagerCompanyComboBoxViewObj.searchingKeyword
    );

    // 加入現有清單
    getManyManagerCompanyComboBoxViewObj.managerCompanyList.push(...fetchedList);
    getManyManagerCompanyComboBoxViewObj.pageIndex++;
  }
};
//-------------------------------------------------------------
/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerCompanyComboBoxViewObj.showDropdown = false;
};
// -------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerCompanyName.value,
  (newValue) => {
    getManyManagerCompanyComboBoxViewObj.managerCompanyKeywordQuery = newValue || null;
    if (!newValue) managerCompanyID.value = null;
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerCompanyComboBoxViewObj.managerCompanyKeywordQuery"
      type="text"
      placeholder="查詢客戶"
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
        v-if="getManyManagerCompanyComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerCompanyComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerCompanyComboBoxViewObj.managerCompanyList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerCompanyComboBoxViewObj.managerCompanyList"
            :key="item.managerCompanyID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerCompanyName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
