<script setup lang="ts">
import { reactive, watch } from "vue";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { useTokenStore } from "@/stores/token";
import type {
  MbsBscHttpGetManyRoleReqMdl,
  MbsBscHttpGetManyRoleRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicManagerRole } from "@/services";
import { debounce } from "lodash-es";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerRoleID = defineModel<number | null>("managerRoleID");
const managerRoleName = defineModel<string | null>("managerRoleName");
const managerDepartmentName = defineModel<string | null>("managerDepartmentName");
const managerRegionName = defineModel<string | null>("managerRegionName");
const managerRegionID = defineModel<number | null>("managerRegionID");
//-------------------------------------------------------------------
/** 取得多筆管理者角色-文字查詢框-頁面模型 */
interface GetManyManagerRoleComboBoxViewMdl {
  pageIndex: number;
  managerRoleKeywordQuery: string | null;
  managerRoleIsEnableQuery: boolean | null;
  managerRoleList: GetManyManagerRoleComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆管理者角色-文字查詢框-項目模型 */
interface GetManyManagerRoleComboBoxItemMdl {
  managerRoleID: number;
  managerRoleName: string;
  managerDepartmentName: string;
  managerRegionName: string;
  managerRegionID: number;
  managerRoleIsEnable: boolean;
}
/** 取得多筆管理者角色-文字查詢框-頁面物件 */
const getManyManagerRoleComboBoxViewObj = reactive<GetManyManagerRoleComboBoxViewMdl>({
  pageIndex: 1,
  managerRoleKeywordQuery: null,
  managerRoleIsEnableQuery: null,
  managerRoleList: [],
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
  const requestData: MbsBscHttpGetManyRoleReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerRoleName: getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery,
    managerRoleIsEnable: getManyManagerRoleComboBoxViewObj.managerRoleIsEnableQuery,
    pageIndex: pageIndex,
  };
  const responseData = await getManyBasicManagerRole(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  // 處理資料
  const fetchedList =
    responseData.managerRoleList.map(
      (item: MbsBscHttpGetManyRoleRspItemMdl) =>
        ({
          managerRoleID: item.managerRoleID,
          managerRoleName: item.managerRoleName,
          managerDepartmentName: item.managerDepartmentName,
          managerRegionID: item.managerRegionID,
          managerRegionName: item.managerRegionName,
          managerRoleIsEnable: item.managerRoleIsEnable,
        }) satisfies GetManyManagerRoleComboBoxItemMdl
    ) ?? [];
  return fetchedList;
};

/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery?.trim() || "";

  // 清空選擇
  managerRoleID.value = null;
  managerRoleName.value = null;
  managerDepartmentName.value = null;
  managerRegionID.value = null;
  managerRegionName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerRoleComboBoxViewObj.pageIndex = 1;
    getManyManagerRoleComboBoxViewObj.managerRoleList = [];
    getManyManagerRoleComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerRoleComboBoxItemMdl[] = await getData(
      getManyManagerRoleComboBoxViewObj.pageIndex
    );

    getManyManagerRoleComboBoxViewObj.managerRoleList.push(...fetchedList);

    if (getManyManagerRoleComboBoxViewObj.managerRoleList.length < PAGE_SIZE) {
      getManyManagerRoleComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerRoleComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerRoleComboBoxViewObj.pageIndex = 1;
  getManyManagerRoleComboBoxViewObj.managerRoleList = [];
  getManyManagerRoleComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerRoleComboBoxItemMdl[] = await getData(
    getManyManagerRoleComboBoxViewObj.pageIndex
  );

  getManyManagerRoleComboBoxViewObj.managerRoleList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerRoleComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerRoleComboBoxViewObj.pageIndex++;
  }
}, 300);

/** focus輸入框時  */
const focusInput = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery = null;

  // 清空選擇的值
  managerRoleID.value = null;
  managerRoleName.value = null;
  managerDepartmentName.value = null;
  managerRegionID.value = null;
  managerRegionName.value = null;

  // 重設資料
  getManyManagerRoleComboBoxViewObj.showDropdown = true;
  getManyManagerRoleComboBoxViewObj.pageIndex = 1;
  getManyManagerRoleComboBoxViewObj.managerRoleList = [];
  getManyManagerRoleComboBoxViewObj.hasMoreData = true;

  // 查詢資料
  const fetchedList: GetManyManagerRoleComboBoxItemMdl[] = await getData(
    getManyManagerRoleComboBoxViewObj.pageIndex
  );

  if (fetchedList.length === 0) {
    getManyManagerRoleComboBoxViewObj.managerRoleList = [];
    getManyManagerRoleComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerRoleComboBoxViewObj.managerRoleList.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerRoleComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerRoleComboBoxViewObj.pageIndex++;
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
    getManyManagerRoleComboBoxViewObj.hasMoreData &&
    !getManyManagerRoleComboBoxViewObj.isLoading
  ) {
    getManyManagerRoleComboBoxViewObj.isLoading = true;

    // 查詢資料
    const fetchedList: GetManyManagerRoleComboBoxItemMdl[] = await getData(
      getManyManagerRoleComboBoxViewObj.pageIndex
    );

    // 過濾出尚未加入的資料
    const uniqueNewList: GetManyManagerRoleComboBoxItemMdl[] = fetchedList.filter(
      (newItem) =>
        !getManyManagerRoleComboBoxViewObj.managerRoleList.some(
          (existing) => existing.managerRoleID === newItem.managerRoleID
        )
    );

    // 將新取得且不重複的資料加入現有清單
    getManyManagerRoleComboBoxViewObj.managerRoleList.push(...uniqueNewList);

    // 更新狀態
    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerRoleComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerRoleComboBoxViewObj.pageIndex++;
    }

    getManyManagerRoleComboBoxViewObj.isLoading = false;
  }
};

/** 選取項目 */
const selectItem = (item: GetManyManagerRoleComboBoxItemMdl) => {
  // 顯示名字
  getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery = item.managerRoleName;

  // 帶出ID、名稱
  managerRoleID.value = item.managerRoleID;
  managerRoleName.value = item.managerRoleName;
  managerDepartmentName.value = item.managerDepartmentName;
  managerRegionID.value = item.managerRegionID;
  managerRegionName.value = item.managerRegionName;

  // 關閉下拉選單
  getManyManagerRoleComboBoxViewObj.showDropdown = false;
};

/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 切換狀態
  getManyManagerRoleComboBoxViewObj.showDropdown = !getManyManagerRoleComboBoxViewObj.showDropdown;

  // 如果是打開狀態，則初始化資料
  if (getManyManagerRoleComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery = null;

    // 清空選擇的值
    managerRoleID.value = null;
    managerRoleName.value = null;
    managerDepartmentName.value = null;
    managerRegionID.value = null;
    managerRegionName.value = null;

    getManyManagerRoleComboBoxViewObj.pageIndex = 1;
    getManyManagerRoleComboBoxViewObj.hasMoreData = true;
    getManyManagerRoleComboBoxViewObj.managerRoleList = [];

    const fetchedList: GetManyManagerRoleComboBoxItemMdl[] = await getData(
      getManyManagerRoleComboBoxViewObj.pageIndex
    );

    // 加入現有清單
    getManyManagerRoleComboBoxViewObj.managerRoleList.push(...fetchedList);
    getManyManagerRoleComboBoxViewObj.pageIndex++;
  }
};

/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerRoleComboBoxViewObj.showDropdown = false;
};
// -------------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerRoleName.value,
  (newValue) => {
    getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery = newValue || null;

    // 如果清空名稱，也清空 ID
    if (!newValue) {
      managerRoleID.value = null;
      managerRoleName.value = null;
      managerDepartmentName.value = null;
      managerRegionID.value = null;
      managerRegionName.value = null;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerRoleComboBoxViewObj.managerRoleKeywordQuery"
      type="text"
      placeholder="查詢角色"
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
        v-if="getManyManagerRoleComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerRoleComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerRoleComboBoxViewObj.managerRoleList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerRoleComboBoxViewObj.managerRoleList"
            :key="item.managerRoleID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerRoleName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
