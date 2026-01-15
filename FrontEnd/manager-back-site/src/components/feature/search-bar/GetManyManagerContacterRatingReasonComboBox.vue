<script setup lang="ts">
import { reactive, watch, nextTick } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyContacterRatingReasonReqMdl,
  MbsBscHttpGetManyContacterRatingReasonRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { GetManyBasicManagerContacterRatingReason } from "@/services";

const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
//-------------------------------------------------------------------
const props = defineProps<{ disabled: boolean }>();
const managerContacterRatingReasonID = defineModel<number | null>("managerContacterRatingReasonID");
const managerContacterRatingReasonName = defineModel<string | null>(
  "managerContacterRatingReasonName"
);
//-------------------------------------------------------------------
/** 取得多筆評等原因-文字查詢框-頁面模型 */
interface GetManyManagerContacterRatingReasonComboBoxViewMdl {
  pageIndex: number;
  keyword: string | null;
  statusQuery: boolean | null;
  list: GetManyManagerContacterRatingReasonComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 取得多筆評等原因-文字查詢框-項目模型 */
interface GetManyManagerContacterRatingReasonComboBoxItemMdl {
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName: string;
  managerContacterRatingReasonStatus: boolean;
}
/** 取得多筆評等原因-文字查詢框-頁面物件 */
const getManyManagerContacterRatingReasonComboBoxViewObj =
  reactive<GetManyManagerContacterRatingReasonComboBoxViewMdl>({
    pageIndex: 1,
    keyword: null,
    statusQuery: null,
    list: [],
    hasMoreData: true,
    showDropdown: false,
    isLoading: false,
  });

//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number, keyword: string | null) => {
  if (!tokenStore.token) return [];
  // 呼叫api
  const requestData: MbsBscHttpGetManyContacterRatingReasonReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerContacterRatingReasonName: keyword,
    managerContacterRatingReasonStatus:
      getManyManagerContacterRatingReasonComboBoxViewObj.statusQuery,
    pageIndex: pageIndex,
  };

  const responseData = await GetManyBasicManagerContacterRatingReason(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  const fetchedList =
    responseData.managerContacterRatingReasonList?.map(
      (item: MbsBscHttpGetManyContacterRatingReasonRspItemMdl) =>
        ({
          managerContacterRatingReasonID: item.managerContacterRatingReasonID,
          managerContacterRatingReasonName: item.managerContacterRatingReasonName,
          managerContacterRatingReasonStatus: item.managerContacterRatingReasonStatus,
        }) satisfies GetManyManagerContacterRatingReasonComboBoxItemMdl
    ) ?? [];
  return fetchedList;
};
//-------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerContacterRatingReasonComboBoxViewObj.keyword?.trim() || "";

  // 清空選擇
  managerContacterRatingReasonID.value = null;
  managerContacterRatingReasonName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex = 1;
    getManyManagerContacterRatingReasonComboBoxViewObj.list = [];
    getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerContacterRatingReasonComboBoxItemMdl[] = await getData(
      getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex,
      null
    );

    getManyManagerContacterRatingReasonComboBoxViewObj.list.push(...fetchedList);

    if (getManyManagerContacterRatingReasonComboBoxViewObj.list.length < PAGE_SIZE) {
      getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex++;
    }

    return;
  }

  // 有輸入文字時，正常查詢
  getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex = 1;
  getManyManagerContacterRatingReasonComboBoxViewObj.list = [];
  getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerContacterRatingReasonComboBoxItemMdl[] = await getData(
    getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex,
    null
  );

  getManyManagerContacterRatingReasonComboBoxViewObj.list.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex++;
  }

  return;
}, 300);

//-------------------------------------------------------------------
/** focus輸入框 */
const focusInput = async () => {
  // 如果已禁用則不動作
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerContacterRatingReasonComboBoxViewObj.keyword = null;

  // 清空選擇的值
  managerContacterRatingReasonID.value = null;
  managerContacterRatingReasonName.value = null;

  // 重設資料
  getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown = true;
  getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex = 1;
  getManyManagerContacterRatingReasonComboBoxViewObj.list = [];
  getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = true;

  // 查詢資料
  const fetchedList: GetManyManagerContacterRatingReasonComboBoxItemMdl[] = await getData(
    getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex,
    null
  );

  if (fetchedList.length === 0) {
    getManyManagerContacterRatingReasonComboBoxViewObj.list = [];
    getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = false;
    return;
  }

  // 將新取得的資料加入清單
  getManyManagerContacterRatingReasonComboBoxViewObj.list.push(...fetchedList);

  // 更新狀態
  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex++;
  }
};

/** 滾動載入更多 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData &&
    !getManyManagerContacterRatingReasonComboBoxViewObj.isLoading
  ) {
    getManyManagerContacterRatingReasonComboBoxViewObj.isLoading = true;
    const fetchedList = await getData(
      getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex,
      getManyManagerContacterRatingReasonComboBoxViewObj.keyword
    );

    const uniqueNewList = fetchedList.filter(
      (newItem) =>
        !getManyManagerContacterRatingReasonComboBoxViewObj.list.some(
          (existing) =>
            existing.managerContacterRatingReasonID === newItem.managerContacterRatingReasonID
        )
    );

    getManyManagerContacterRatingReasonComboBoxViewObj.list.push(...uniqueNewList);
    if (fetchedList.length < PAGE_SIZE)
      getManyManagerContacterRatingReasonComboBoxViewObj.hasMoreData = false;
    else getManyManagerContacterRatingReasonComboBoxViewObj.pageIndex++;

    getManyManagerContacterRatingReasonComboBoxViewObj.isLoading = false;
  }
};

/** 選取項目 */
const selectItem = (item: GetManyManagerContacterRatingReasonComboBoxItemMdl) => {
  getManyManagerContacterRatingReasonComboBoxViewObj.keyword =
    item.managerContacterRatingReasonName;
  managerContacterRatingReasonID.value = item.managerContacterRatingReasonID;
  managerContacterRatingReasonName.value = item.managerContacterRatingReasonName;
  getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown = false;
};

/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;
  getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown =
    !getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown;
  if (getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown) await focusInput();
};

/** 關閉下拉 */
const closeDropdown = () => {
  getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------------
/** 監聽名稱變化 */
watch(
  () => managerContacterRatingReasonName.value,
  async (val) => {
    if (val) {
      await nextTick();
      getManyManagerContacterRatingReasonComboBoxViewObj.keyword = val;
    } else {
      getManyManagerContacterRatingReasonComboBoxViewObj.keyword = "";
    }
  },
  { immediate: true }
);
//-------------------------------------------------------------------
/** 監聽ID變化 */
watch(
  () => managerContacterRatingReasonID.value,
  async (val) => {
    if (!val) {
      getManyManagerContacterRatingReasonComboBoxViewObj.keyword = "";
      return;
    }

    //  現有清單中有找到
    const matched = getManyManagerContacterRatingReasonComboBoxViewObj.list.find(
      (x) => x.managerContacterRatingReasonID === val
    );
    if (matched) {
      getManyManagerContacterRatingReasonComboBoxViewObj.keyword =
        matched.managerContacterRatingReasonName;
      return;
    }

    const list = await getData(1, null);
    const found = list.find((x) => x.managerContacterRatingReasonID === val);
    if (found) {
      getManyManagerContacterRatingReasonComboBoxViewObj.list.push(found);
      getManyManagerContacterRatingReasonComboBoxViewObj.keyword =
        found.managerContacterRatingReasonName;
    }
  },
  { immediate: true } // 初始化時也執行一次
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerContacterRatingReasonComboBoxViewObj.keyword"
      type="text"
      placeholder="查詢窗口評等原因"
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
        v-if="getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerContacterRatingReasonComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerContacterRatingReasonComboBoxViewObj.list.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerContacterRatingReasonComboBoxViewObj.list"
            :key="item.managerContacterRatingReasonID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerContacterRatingReasonName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
