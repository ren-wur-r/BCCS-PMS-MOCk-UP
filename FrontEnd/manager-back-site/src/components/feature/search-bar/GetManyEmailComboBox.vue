<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyContacterReqMdl,
  MbsBscHttpGetManyContacterRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { GetManyManagerContacter } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------
const props = defineProps<{ disabled?: boolean; managerContacterCompanyID: number | null }>();
const managerContacterID = defineModel<number | null>("managerContacterID");
const managerContacterName = defineModel<string | null>("managerContacterName");
const managerContacterEmail = defineModel<string | null>("managerContacterEmail");
//-------------------------------------------------------------
/** 取得多筆信箱-文字查詢框-頁面模型 */
interface GetManyEmailComboBoxViewMdl {
  pageIndex: number;
  managerContacterEmailQuery: string;
  emailList: GetManyManagerContacterComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆信箱-文字查詢框-頁面模型 */
interface GetManyManagerContacterComboBoxItemMdl {
  managerContacterID: number;
  managerContacterName: string;
  managerContacterEmail: string;
}
//-------------------------------------------------------------
/** 取得多筆信箱-文字查詢框-頁面物件 */
const getManyManagerEmailComboBoxViewObj = reactive<GetManyEmailComboBoxViewMdl>({
  pageIndex: 1,
  managerContacterEmailQuery: "",
  emailList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});

//-------------------------------------------------------------
// 取得資料
//-------------------------------------------------------------
const getData = async (pageIndex: number) => {
  if (!tokenStore.token) return [];

  if (props.managerContacterCompanyID === null) return [];

  const requestData: MbsBscHttpGetManyContacterReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerContacterCompanyID: props.managerContacterCompanyID,
    managerContacterName: null,
    managerContacterEmail: getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery.trim(),
    pageIndex,
  };

  const responseData = await GetManyManagerContacter(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  return (
    responseData.managerContacterList?.map(
      (item: MbsBscHttpGetManyContacterRspItemMdl) =>
        ({
          managerContacterID: item.managerContacterID,
          managerContacterName: item.managerContacterName,
          managerContacterEmail: item.managerContacterEmail,
        }) satisfies GetManyManagerContacterComboBoxItemMdl
    ) ?? []
  );
};

//-------------------------------------------------------------
// 使用者輸入時
//-------------------------------------------------------------
const onInput = debounce(async () => {
  const inputValue = getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery?.trim() || "";

  managerContacterID.value = null;
  managerContacterName.value = null;

  if (inputValue.length < 1) {
    getManyManagerEmailComboBoxViewObj.pageIndex = 1;
    getManyManagerEmailComboBoxViewObj.emailList = [];
    getManyManagerEmailComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerContacterComboBoxItemMdl[] = await getData(
      getManyManagerEmailComboBoxViewObj.pageIndex
    );

    getManyManagerEmailComboBoxViewObj.emailList.push(...fetchedList);

    if (getManyManagerEmailComboBoxViewObj.emailList.length < PAGE_SIZE)
      getManyManagerEmailComboBoxViewObj.hasMoreData = false;
    else {
      getManyManagerEmailComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerEmailComboBoxViewObj.pageIndex = 1;
  getManyManagerEmailComboBoxViewObj.emailList = [];
  getManyManagerEmailComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerEmailComboBoxViewObj.pageIndex);
  getManyManagerEmailComboBoxViewObj.emailList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerEmailComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerEmailComboBoxViewObj.pageIndex++;
  }
}, 300);
//--------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  // 取得滾動元素
  const dropdown = event.target as HTMLElement;

  // 判斷是否接近底部
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerEmailComboBoxViewObj.hasMoreData &&
    !getManyManagerEmailComboBoxViewObj.isLoading
  ) {
    getManyManagerEmailComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerContacterComboBoxItemMdl[] = await getData(
      getManyManagerEmailComboBoxViewObj.pageIndex
    );

    // 過濾出尚未加入的資料
    const uniqueNewList: GetManyManagerContacterComboBoxItemMdl[] = fetchedList.filter(
      (newItem) =>
        !getManyManagerEmailComboBoxViewObj.emailList.some(
          (exist) => exist.managerContacterID === newItem.managerContacterID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerEmailComboBoxViewObj.emailList.push(...uniqueNewList);

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE) getManyManagerEmailComboBoxViewObj.hasMoreData = false;
    else getManyManagerEmailComboBoxViewObj.pageIndex++;

    getManyManagerEmailComboBoxViewObj.isLoading = false;
  }
};

//-------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerContacterComboBoxItemMdl) => {
  // 雖然用信箱查，但最後顯示姓名
  getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery = item.managerContacterEmail;
  managerContacterID.value = item.managerContacterID;
  managerContacterName.value = item.managerContacterName;
  managerContacterEmail.value = item.managerContacterEmail;

  getManyManagerEmailComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------
/** focus輸入框時  */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery = "";

  // 清空選擇的值
  managerContacterID.value = null;
  managerContacterName.value = null;
  managerContacterEmail.value = null;

  getManyManagerEmailComboBoxViewObj.showDropdown = true;
  getManyManagerEmailComboBoxViewObj.pageIndex = 1;
  getManyManagerEmailComboBoxViewObj.emailList = [];
  getManyManagerEmailComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerEmailComboBoxViewObj.pageIndex);
  getManyManagerEmailComboBoxViewObj.emailList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) getManyManagerEmailComboBoxViewObj.hasMoreData = false;
  else getManyManagerEmailComboBoxViewObj.pageIndex++;
};
//-------------------------------------------------------------
/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  // 切換狀態
  getManyManagerEmailComboBoxViewObj.showDropdown =
    !getManyManagerEmailComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerEmailComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery = "";

    // 清空選擇的值
    managerContacterID.value = null;
    managerContacterName.value = null;
    managerContacterEmail.value = null;

    getManyManagerEmailComboBoxViewObj.pageIndex = 1;
    getManyManagerEmailComboBoxViewObj.emailList = [];
    getManyManagerEmailComboBoxViewObj.hasMoreData = true;

    const fetchedList: GetManyManagerContacterComboBoxItemMdl[] = await getData(
      getManyManagerEmailComboBoxViewObj.pageIndex
    );

    // 加入現有清單
    getManyManagerEmailComboBoxViewObj.emailList.push(...fetchedList);
    getManyManagerEmailComboBoxViewObj.pageIndex++;
  }
};
//-------------------------------------------------------------
/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerEmailComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------

// 監聽當有名字的時候直接帶入
watch(
  () => managerContacterEmail.value,
  (newValue) => {
    // 若是第一次進入編輯模式，有 email → 帶入 input
    if (newValue && getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery === "") {
      getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery = newValue;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <!-- 查詢電子信箱 -->
    <input
      v-model="getManyManagerEmailComboBoxViewObj.managerContacterEmailQuery"
      type="text"
      placeholder="查詢電子信箱"
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
        v-if="getManyManagerEmailComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerEmailComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerEmailComboBoxViewObj.emailList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <!-- 顯示窗口名稱 -->
          <li
            v-for="item in getManyManagerEmailComboBoxViewObj.emailList"
            :key="item.managerContacterID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerContacterName }}（{{ item.managerContacterEmail }}）
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
