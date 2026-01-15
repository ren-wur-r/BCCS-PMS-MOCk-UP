<script setup lang="ts">
import { reactive, watch, nextTick } from "vue";
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
// Props 與 v-model 綁定
//-------------------------------------------------------------
const props = defineProps<{ disabled?: boolean; managerContacterCompanyID: number }>();
const managerContacterID = defineModel<number | null>("managerContacterID");
const managerContacterName = defineModel<string | null>("managerContacterName");
//-------------------------------------------------------------
/** 取得多筆窗口-文字查詢框-頁面模型 */
interface GetManyManagerContacterComboBoxViewMdl {
  pageIndex: number;
  managerContacterName: string;
  managerContacterEmail: string;
  contacterList: GetManyManagerContacterComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}

/** 取得多筆窗口-文字查詢框-項目模型 */
interface GetManyManagerContacterComboBoxItemMdl {
  managerContacterID: number;
  managerContacterName: string;
  managerContacterEmail: string;
}
//-------------------------------------------------------------
/** 取得多筆窗口-文字查詢框-頁面物件 */
const getManyManagerContacterComboBoxViewObj = reactive<GetManyManagerContacterComboBoxViewMdl>({
  pageIndex: 1,
  managerContacterName: "",
  managerContacterEmail: "",
  contacterList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});
//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  if (!tokenStore.token) return [];

  const requestData: MbsBscHttpGetManyContacterReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerContacterName: getManyManagerContacterComboBoxViewObj.managerContacterName.trim(),
    managerContacterCompanyID: props.managerContacterCompanyID,
    managerContacterEmail: getManyManagerContacterComboBoxViewObj.managerContacterName.trim(),
    pageIndex,
  };

  const responseData = await GetManyManagerContacter(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  const fetchedList =
    responseData.managerContacterList?.map(
      (item: MbsBscHttpGetManyContacterRspItemMdl) =>
        ({
          managerContacterID: item.managerContacterID,
          managerContacterName: item.managerContacterName,
          managerContacterEmail: item.managerContacterEmail,
        }) satisfies GetManyManagerContacterComboBoxItemMdl
    ) ?? [];

  return fetchedList;
};

//-------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {
  const inputValue = getManyManagerContacterComboBoxViewObj.managerContacterName?.trim() || "";

  managerContacterID.value = null;
  managerContacterName.value = null;

  // 少於一個字時，也可以選擇是否查詢完整列表
  if (inputValue.length < 1) {
    getManyManagerContacterComboBoxViewObj.pageIndex = 1;
    getManyManagerContacterComboBoxViewObj.contacterList = [];
    getManyManagerContacterComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerContacterComboBoxItemMdl[] = await getData(
      getManyManagerContacterComboBoxViewObj.pageIndex
    );

    getManyManagerContacterComboBoxViewObj.contacterList.push(...fetchedList);

    if (fetchedList.length < PAGE_SIZE) {
      getManyManagerContacterComboBoxViewObj.hasMoreData = false;
    } else {
      getManyManagerContacterComboBoxViewObj.pageIndex++;
    }

    return;
  }
  // 有輸入文字時，正常查詢
  getManyManagerContacterComboBoxViewObj.pageIndex = 1;
  getManyManagerContacterComboBoxViewObj.contacterList = [];
  getManyManagerContacterComboBoxViewObj.hasMoreData = true;

  const fetchedList: GetManyManagerContacterComboBoxItemMdl[] = await getData(
    getManyManagerContacterComboBoxViewObj.pageIndex
  );

  getManyManagerContacterComboBoxViewObj.contacterList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) {
    getManyManagerContacterComboBoxViewObj.hasMoreData = false;
  } else {
    getManyManagerContacterComboBoxViewObj.pageIndex++;
  }
}, 300);

//-------------------------------------------------------------
/** focus輸入框時  */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerContacterComboBoxViewObj.managerContacterName = "";

  // 清空選擇的值
  managerContacterID.value = null;
  managerContacterName.value = null;

  getManyManagerContacterComboBoxViewObj.showDropdown = true;
  getManyManagerContacterComboBoxViewObj.pageIndex = 1;
  getManyManagerContacterComboBoxViewObj.contacterList = [];
  getManyManagerContacterComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerContacterComboBoxViewObj.pageIndex);
  getManyManagerContacterComboBoxViewObj.contacterList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) getManyManagerContacterComboBoxViewObj.hasMoreData = false;
  else getManyManagerContacterComboBoxViewObj.pageIndex++;
};

//-------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerContacterComboBoxViewObj.hasMoreData &&
    !getManyManagerContacterComboBoxViewObj.isLoading
  ) {
    getManyManagerContacterComboBoxViewObj.isLoading = true;

    const fetchedList = await getData(getManyManagerContacterComboBoxViewObj.pageIndex);
    const uniqueNewList = fetchedList.filter(
      (newItem) =>
        !getManyManagerContacterComboBoxViewObj.contacterList.some(
          (exist) => exist.managerContacterID === newItem.managerContacterID
        )
    );

    getManyManagerContacterComboBoxViewObj.contacterList.push(...uniqueNewList);

    if (fetchedList.length < PAGE_SIZE) getManyManagerContacterComboBoxViewObj.hasMoreData = false;
    else getManyManagerContacterComboBoxViewObj.pageIndex++;

    getManyManagerContacterComboBoxViewObj.isLoading = false;
  }
};

//-------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerContacterComboBoxItemMdl) => {
  getManyManagerContacterComboBoxViewObj.managerContacterName = item.managerContacterName;
  managerContacterID.value = item.managerContacterID;
  managerContacterName.value = item.managerContacterName;
  getManyManagerContacterComboBoxViewObj.showDropdown = false;
};

//-------------------------------------------------------------
/** 點擊下拉選單按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;
  getManyManagerContacterComboBoxViewObj.showDropdown =
    !getManyManagerContacterComboBoxViewObj.showDropdown;
  if (getManyManagerContacterComboBoxViewObj.showDropdown) await focusInput();
};

//-------------------------------------------------------------
/** 關閉下拉選單 */
const closeDropdown = () => {
  getManyManagerContacterComboBoxViewObj.showDropdown = false;
};
// -------------------------------------------------------------------
// 監聽當有名字的時候直接帶入
watch(
  () => managerContacterName.value,
  async (val) => {
    await nextTick();
    getManyManagerContacterComboBoxViewObj.managerContacterName = val ?? "";
  },
  { immediate: true }
);

//-------------------------------------------------------------
// 監聽ID變化
watch(
  () => managerContacterID.value,
  async (val) => {
    if (!val) {
      getManyManagerContacterComboBoxViewObj.managerContacterName = "";
      return;
    }

    const matched = getManyManagerContacterComboBoxViewObj.contacterList.find(
      (x) => x.managerContacterID === val
    );
    if (matched) {
      getManyManagerContacterComboBoxViewObj.managerContacterName = matched.managerContacterName;
      return;
    }

    const list = await getData(1);
    const found = list.find((x) => x.managerContacterID === val);
    if (found) {
      getManyManagerContacterComboBoxViewObj.contacterList.push(found);
      getManyManagerContacterComboBoxViewObj.managerContacterName = found.managerContacterName;
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input
      v-model="getManyManagerContacterComboBoxViewObj.managerContacterName"
      type="text"
      placeholder="查詢窗口"
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
        v-if="getManyManagerContacterComboBoxViewObj.showDropdown"
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
        v-if="getManyManagerContacterComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown"
      >
        <ul v-if="getManyManagerContacterComboBoxViewObj.contacterList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li
            v-for="item in getManyManagerContacterComboBoxViewObj.contacterList"
            :key="item.managerContacterID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100"
            @click="selectItem(item)"
          >
            {{ item.managerContacterName }} ( {{ item.managerContacterEmail }} )
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
