<script setup lang="ts">
import { reactive, watch, computed } from "vue";
import { useTokenStore } from "@/stores/token";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyEmployeeProjectMemberReqMdl,
  MbsBscHttpGetManyEmployeeProjectMemberRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicEmployeeProjectMember } from "@/services";
import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";

//-------------------------------------------------------------------
const tokenStore = useTokenStore();
//-------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
  employeeProjectID: number;
}>();

/** 選中的執行者列表 */
const selectedExecutorList = defineModel<GetManyEmployeeExecutorComboBoxItemMdl[]>(
  "selectedExecutorList",
  { default: [] }
);

//-------------------------------------------------------------------
/** 項目模型 */
export interface GetManyEmployeeExecutorComboBoxItemMdl {
  employeeProjectStoneTaskExecutorEmployeeID: number;
  employeeProjectStoneTaskExecutorEmployeeName: string;
}

/** 頁面模型 */
interface GetManyEmployeeExecutorComboBoxViewMdl {
  keywordQuery: string;
  allEmployeeList: GetManyEmployeeExecutorComboBoxItemMdl[];
  showDropdown: boolean;
  isLoading: boolean;
}

/** 頁面物件 */
const getManyEmployeeExecutorComboBoxViewObj = reactive<GetManyEmployeeExecutorComboBoxViewMdl>({
  keywordQuery: "",
  allEmployeeList: [],
  showDropdown: false,
  isLoading: false,
});

//-------------------------------------------------------------------
/** 取得資料 (一次取得所有專案成員) */
const getData = async () => {
  if (!tokenStore.token) return;
  if (!props.employeeProjectID) return;

  getManyEmployeeExecutorComboBoxViewObj.isLoading = true;

  const requestData: MbsBscHttpGetManyEmployeeProjectMemberReqMdl = {
    employeeLoginToken: tokenStore.token,
    employeeProjectID: props.employeeProjectID,
  };

  const responseData = await getManyBasicEmployeeProjectMember(requestData);

  getManyEmployeeExecutorComboBoxViewObj.isLoading = false;

  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) {
    getManyEmployeeExecutorComboBoxViewObj.allEmployeeList = [];
    return;
  }

  getManyEmployeeExecutorComboBoxViewObj.allEmployeeList =
    responseData.employeeProjectMemberList
      ?.filter(
        (item) => item.employeeProjectMemberRole === DbEmployeeProjectMemberRoleEnum.Executor
      )
      .map(
        (item: MbsBscHttpGetManyEmployeeProjectMemberRspItemMdl) =>
          ({
            employeeProjectStoneTaskExecutorEmployeeID: item.employeeID,
            employeeProjectStoneTaskExecutorEmployeeName: item.employeeName,
          }) satisfies GetManyEmployeeExecutorComboBoxItemMdl
      ) ?? [];
};

//-------------------------------------------------------------------
/** 過濾後的候選名單 (建議) */
const filteredCandidates = computed(() => {
  const keyword = getManyEmployeeExecutorComboBoxViewObj.keywordQuery.trim().toLowerCase();
  return getManyEmployeeExecutorComboBoxViewObj.allEmployeeList.filter((item) => {
    // 排除已選擇的
    const isSelected = selectedExecutorList.value.some(
      (selected) =>
        selected.employeeProjectStoneTaskExecutorEmployeeID ===
        item.employeeProjectStoneTaskExecutorEmployeeID
    );
    if (isSelected) return false;

    // 關鍵字過濾
    if (!keyword) return true;
    return item.employeeProjectStoneTaskExecutorEmployeeName.toLowerCase().includes(keyword);
  });
});

/** 已指派名單 (從 v-model 讀取) */
const assignedList = computed(() => selectedExecutorList.value);

//-------------------------------------------------------------------
/** focus輸入框時 */
const focusInput = async () => {
  if (props.disabled) return;
  getManyEmployeeExecutorComboBoxViewObj.showDropdown = true;

  // 如果還沒載入過資料，載入一次
  if (getManyEmployeeExecutorComboBoxViewObj.allEmployeeList.length === 0) {
    await getData();
  }
};

/** 選取項目 */
const selectItem = (item: GetManyEmployeeExecutorComboBoxItemMdl) => {
  selectedExecutorList.value.push(item);
  // 不清空關鍵字，方便繼續搜尋? 或者清空? 這裡選擇不清空，但 Input 會過濾掉已選的
  // getManyEmployeeExecutorComboBoxViewObj.keywordQuery = "";
};

/** 移除已選項目 */
const removeSelectedItem = (item: GetManyEmployeeExecutorComboBoxItemMdl) => {
  const index = selectedExecutorList.value.findIndex(
    (i) =>
      i.employeeProjectStoneTaskExecutorEmployeeID ===
      item.employeeProjectStoneTaskExecutorEmployeeID
  );
  if (index > -1) {
    selectedExecutorList.value.splice(index, 1);
  }
};

/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;
  getManyEmployeeExecutorComboBoxViewObj.showDropdown =
    !getManyEmployeeExecutorComboBoxViewObj.showDropdown;
  if (
    getManyEmployeeExecutorComboBoxViewObj.showDropdown &&
    getManyEmployeeExecutorComboBoxViewObj.allEmployeeList.length === 0
  ) {
    await getData();
  }
};

/** 關閉下拉 */
const closeDropdown = () => {
  getManyEmployeeExecutorComboBoxViewObj.showDropdown = false;
};

// 監聽 projectID 變更，重新載入
watch(
  () => props.employeeProjectID,
  async (newVal) => {
    if (newVal) {
      await getData();
    }
  }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <!-- 輸入框與已選標籤 -->
    <div
      class="flex-1 flex flex-wrap items-center gap-1 input-box rounded-r-none min-h-8 !h-auto max-h-[100px] overflow-y-auto p-1"
      :class="{ 'cursor-not-allowed bg-gray-100': props.disabled }"
      @click="focusInput"
    >
      <!-- 已選標籤 -->
      <span
        v-for="item in assignedList"
        :key="item.employeeProjectStoneTaskExecutorEmployeeID"
        class="bg-gray-100 text-gray-700 text-xs px-2 py-1 rounded flex items-center gap-1"
      >
        {{ item.employeeProjectStoneTaskExecutorEmployeeName }}
        <button
          type="button"
          class="text-gray-400 hover:text-red-500 font-bold"
          @click.stop="removeSelectedItem(item)"
        >
          ×
        </button>
      </span>

      <!-- 輸入框 -->
      <input
        v-model="getManyEmployeeExecutorComboBoxViewObj.keywordQuery"
        type="text"
        :placeholder="assignedList.length === 0 ? '請輸入姓名或帳號' : ''"
        class="flex-1 min-w-[60px] outline-none bg-transparent border-none p-0 text-sm focus:ring-0"
        :disabled="props.disabled"
        @focus="focusInput"
      />
    </div>

    <button
      type="button"
      class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:cursor-not-allowed h-auto"
      :disabled="props.disabled"
      @click="clickDropdownBtn"
    >
      <svg
        v-if="getManyEmployeeExecutorComboBoxViewObj.showDropdown"
        class="w-5 h-5 text-gray-500"
        viewBox="0 0 20 25"
      >
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676"
        />
      </svg>
      <svg v-else class="w-5 h-5 text-gray-500" viewBox="0 0 20 25">
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
        v-if="getManyEmployeeExecutorComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-60 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1 p-3 flex flex-col gap-2"
      >
        <!-- 已指派 -->
        <div v-if="assignedList.length > 0">
          <div class="text-sm font-bold text-gray-700 mb-1">已指派</div>
          <ul class="flex flex-col gap-1">
            <li
              v-for="item in assignedList"
              :key="item.employeeProjectStoneTaskExecutorEmployeeID"
              class="flex items-center justify-between px-2 py-1 text-sm bg-gray-100 rounded hover:bg-gray-200"
            >
              <span>{{ item.employeeProjectStoneTaskExecutorEmployeeName }}</span>
              <button
                type="button"
                class="text-gray-500 hover:text-red-500"
                @click="removeSelectedItem(item)"
              >
                ×
              </button>
            </li>
          </ul>
        </div>

        <!-- 建議 -->
        <div>
          <div class="text-sm font-bold text-gray-700 mb-1">建議</div>
          <ul v-if="filteredCandidates.length === 0">
            <li class="px-2 py-1 text-sm text-gray-500">無符合資料</li>
          </ul>
          <ul v-else class="flex flex-col gap-1">
            <li
              v-for="item in filteredCandidates"
              :key="item.employeeProjectStoneTaskExecutorEmployeeID"
              class="px-2 py-1 text-sm cursor-pointer hover:bg-gray-100 rounded transition-colors"
              @click="selectItem(item)"
            >
              {{ item.employeeProjectStoneTaskExecutorEmployeeName }}
            </li>
          </ul>
        </div>
      </div>
    </transition>
  </div>
</template>
