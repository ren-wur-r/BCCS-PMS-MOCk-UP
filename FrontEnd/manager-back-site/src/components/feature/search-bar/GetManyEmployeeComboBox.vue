<script setup lang="ts">
import { reactive, watch } from "vue";
import { debounce } from "lodash-es";
import { useTokenStore } from "@/stores/token";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsBscHttpGetManyEmployeeInfoReqMdl,
  MbsBscHttpGetEmployeeInfoRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { getManyBasicEmployee } from "@/services";

//-------------------------------------------------------------------
const tokenStore = useTokenStore();
const PAGE_SIZE = 5;
const useMockData = true;
const normalizeName = (value: string | null | undefined) => {
  if (!value) return "";
  return value.replace(/^#+/, "").trim();
};
const resolveDepartmentManager = (region: string, department: string) => {
  const normalizedRegion = normalizeName(region);
  const normalizedDepartment = normalizeName(department);
  const explicitManagers: Record<string, Record<string, string>> = {
    北區: {
      工程部: "孫振聲",
    },
  };
  const explicit = explicitManagers[normalizedRegion]?.[normalizedDepartment];
  if (explicit) return explicit;
  const candidates = orgMemberDirectory.filter(
    (item) =>
      normalizeName(item.regionName) === normalizedRegion &&
      normalizeName(item.departmentName) === normalizedDepartment
  );
  return candidates[0]?.name ?? "";
};
//-------------------------------------------------------------------
const props = defineProps<{
  disabled: boolean;
  managerRoleID?: number | null;
}>();

const managerEmployeeID = defineModel<number | null>("managerEmployeeID");
const managerEmployeeName = defineModel<string | null>("managerEmployeeName");
const managerRegionID = defineModel<number | null>("managerRegionID");
const managerDepartmentID = defineModel<number | null>("managerDepartmentID");
const managerRegionName = defineModel<string | null>("managerRegionName");
const managerDepartmentName = defineModel<string | null>("managerDepartmentName");
//-------------------------------------------------------------------
/** 取得多筆員工-頁面模型 */
interface GetManyManagerEmployeeComboBoxViewMdl {
  pageIndex: number;
  keywordQuery: string | null;
  employeeList: GetManyManagerEmployeeComboBoxItemMdl[];
  hasMoreData: boolean;
  showDropdown: boolean;
  isLoading: boolean;
}
/** 項目模型 */
interface GetManyManagerEmployeeComboBoxItemMdl {
  employeeID: number;
  employeeName: string;
  employeeIsEnable: boolean;
  managerRegionID: number;
  managerRegionName: string;
  managerDepartmentID: number;
  managerDepartmentName: string;
}
//-------------------------------------------------------------------
/** 頁面物件 */
const getManyManagerEmployeeComboBoxViewObj = reactive<GetManyManagerEmployeeComboBoxViewMdl>({
  pageIndex: 1,
  keywordQuery: null,
  employeeList: [],
  hasMoreData: true,
  showDropdown: false,
  isLoading: false,
});
//-------------------------------------------------------------------
/** 取得資料 */
const getData = async (pageIndex: number) => {
  if (useMockData) {
    const keyword = getManyManagerEmployeeComboBoxViewObj.keywordQuery?.trim().toLowerCase() ?? "";
    let list = orgMemberDirectory;
    const normalizedRegion = normalizeName(managerRegionName.value);
    const normalizedDepartment = normalizeName(managerDepartmentName.value);
    if (normalizedRegion && normalizedRegion !== "跨區") {
      list = list.filter(
        (item) => normalizeName(item.regionName) === normalizedRegion
      );
    }
    if (normalizedDepartment) {
      list = list.filter(
        (item) => normalizeName(item.departmentName) === normalizedDepartment
      );
    }
    if (keyword) {
      list = list.filter((item) => item.name.toLowerCase().includes(keyword));
    }
    const start = (pageIndex - 1) * PAGE_SIZE;
    const paged = list.slice(start, start + PAGE_SIZE);
    const mapped = paged.map((item, index) => ({
      employeeID: start + index + 1,
      employeeName: item.name,
      employeeIsEnable: true,
      managerRegionID: managerRegionID.value ?? 0,
      managerRegionName: item.regionName,
      managerDepartmentID: managerDepartmentID.value ?? 0,
      managerDepartmentName: item.departmentName,
    }));
    if (normalizedDepartment) {
      mapped.unshift({
        employeeID: -999,
        employeeName: "由部門經理指派",
        employeeIsEnable: true,
        managerRegionID: managerRegionID.value ?? 0,
        managerRegionName: normalizedRegion,
        managerDepartmentID: managerDepartmentID.value ?? 0,
        managerDepartmentName: normalizedDepartment,
      });
    }
    return mapped;
  }
  if (!tokenStore.token) return [];

  const normalizedRegionId =
    managerRegionID.value && Number(managerRegionID.value) > 0
      ? Number(managerRegionID.value)
      : null;
  const normalizedDepartmentId =
    managerDepartmentID.value && Number(managerDepartmentID.value) > 0
      ? Number(managerDepartmentID.value)
      : null;

  const requestData: MbsBscHttpGetManyEmployeeInfoReqMdl = {
    employeeLoginToken: tokenStore.token,
    managerRoleID: props.managerRoleID ?? null,
    employeeIsEnable: true,
    pageIndex,
    managerRegionID: normalizedRegionId,
    managerDepartmentID: normalizedDepartmentId,
    employeeName: getManyManagerEmployeeComboBoxViewObj.keywordQuery ?? null,
  };

  const responseData = await getManyBasicEmployee(requestData);
  if (!responseData || responseData.errorCode !== MbsErrorCodeEnum.Success) return [];

  let fetchedList =
    responseData.employeeList?.map(
      (item: MbsBscHttpGetEmployeeInfoRspItemMdl) =>
        ({
          employeeID: item.employeeID,
          employeeName: item.employeeName,
          employeeIsEnable: item.employeeIsEnable,
          managerRegionID: item.managerRegionID,
          managerRegionName: item.managerRegionName,
          managerDepartmentID: item.managerDepartmentID,
          managerDepartmentName: item.managerDepartmentName,
        }) satisfies GetManyManagerEmployeeComboBoxItemMdl
    ) ?? [];

  if (managerRegionName.value && managerRegionName.value !== "跨區") {
    fetchedList = fetchedList.filter(
      (item) => item.managerRegionName === managerRegionName.value
    );
  }

  if (managerDepartmentName.value) {
    fetchedList = fetchedList.filter(
      (item) => item.managerDepartmentName === managerDepartmentName.value
    );
  }

  return fetchedList;
};
//-------------------------------------------------------------------
/** 使用者輸入時 */
const onInput = debounce(async () => {

  const inputValue = getManyManagerEmployeeComboBoxViewObj.keywordQuery?.trim() || "";

  managerEmployeeID.value = null;
  managerEmployeeName.value = null;

  // 若輸入少於 1 字 → 查全部
  if (inputValue.length < 1) {
    getManyManagerEmployeeComboBoxViewObj.pageIndex = 1;
    getManyManagerEmployeeComboBoxViewObj.employeeList = [];
    getManyManagerEmployeeComboBoxViewObj.hasMoreData = true;

    // 可選：刪空時查全部
    const fetchedList: GetManyManagerEmployeeComboBoxItemMdl[] = await getData(
      getManyManagerEmployeeComboBoxViewObj.pageIndex
    );

    getManyManagerEmployeeComboBoxViewObj.employeeList.push(...fetchedList);

    if (getManyManagerEmployeeComboBoxViewObj.employeeList.length < PAGE_SIZE)
      getManyManagerEmployeeComboBoxViewObj.hasMoreData = false;
    else {
      getManyManagerEmployeeComboBoxViewObj.pageIndex++;
    }

    return;
  }

  getManyManagerEmployeeComboBoxViewObj.pageIndex = 1;
  getManyManagerEmployeeComboBoxViewObj.employeeList = [];
  getManyManagerEmployeeComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerEmployeeComboBoxViewObj.pageIndex);
  getManyManagerEmployeeComboBoxViewObj.employeeList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) getManyManagerEmployeeComboBoxViewObj.hasMoreData = false;
  else getManyManagerEmployeeComboBoxViewObj.pageIndex++;
}, 300);
//-------------------------------------------------------------------
/** focus輸入框時 */
const focusInput = async () => {
  if (props.disabled) return;

  // 清空關鍵字查詢，以顯示所有選項
  getManyManagerEmployeeComboBoxViewObj.keywordQuery = null;

  // 清空人員選擇，保留區域/部門條件
  managerEmployeeID.value = null;
  managerEmployeeName.value = null;

  getManyManagerEmployeeComboBoxViewObj.showDropdown = true;

  getManyManagerEmployeeComboBoxViewObj.pageIndex = 1;
  getManyManagerEmployeeComboBoxViewObj.employeeList = [];
  getManyManagerEmployeeComboBoxViewObj.hasMoreData = true;

  const fetchedList = await getData(getManyManagerEmployeeComboBoxViewObj.pageIndex);
  getManyManagerEmployeeComboBoxViewObj.employeeList.push(...fetchedList);

  if (fetchedList.length < PAGE_SIZE) getManyManagerEmployeeComboBoxViewObj.hasMoreData = false;
  else getManyManagerEmployeeComboBoxViewObj.pageIndex++;
};
//-------------------------------------------------------------------
/** 滾動下拉選單 */
const scrollDropdown = async (event: Event) => {
  const dropdown = event.target as HTMLElement;
  const nearBottom = dropdown.scrollTop + dropdown.clientHeight >= dropdown.scrollHeight * 0.8;

  if (
    nearBottom &&
    getManyManagerEmployeeComboBoxViewObj.hasMoreData &&
    !getManyManagerEmployeeComboBoxViewObj.isLoading
  ) {
    getManyManagerEmployeeComboBoxViewObj.isLoading = true;

    const fetchedList = await getData(getManyManagerEmployeeComboBoxViewObj.pageIndex);

    const uniqueNewList = fetchedList.filter(
      (newItem) =>
        !getManyManagerEmployeeComboBoxViewObj.employeeList.some(
          (existing) => existing.employeeID === newItem.employeeID
        )
    );

    getManyManagerEmployeeComboBoxViewObj.employeeList.push(...uniqueNewList);

    if (fetchedList.length < PAGE_SIZE) getManyManagerEmployeeComboBoxViewObj.hasMoreData = false;
    else getManyManagerEmployeeComboBoxViewObj.pageIndex++;

    getManyManagerEmployeeComboBoxViewObj.isLoading = false;
  }
};
//-------------------------------------------------------------------
/** 選取項目 */
const selectItem = (item: GetManyManagerEmployeeComboBoxItemMdl) => {
  if (item.employeeID === -999) {
    const managerName = resolveDepartmentManager(
      managerRegionName.value ?? "",
      managerDepartmentName.value ?? ""
    );
    const managerIndex = orgMemberDirectory.findIndex((member) => member.name === managerName);
    const resolvedId = managerIndex >= 0 ? managerIndex + 1 : Date.now();
    getManyManagerEmployeeComboBoxViewObj.keywordQuery = managerName || item.employeeName;
    managerEmployeeID.value = resolvedId;
    managerEmployeeName.value = managerName || item.employeeName;
  } else {
    getManyManagerEmployeeComboBoxViewObj.keywordQuery = item.employeeName;
    managerEmployeeID.value = item.employeeID;
    managerEmployeeName.value = item.employeeName;
  }

  managerRegionID.value = item.managerRegionID;
  managerRegionName.value = item.managerRegionName;
  managerDepartmentID.value = item.managerDepartmentID;
  managerDepartmentName.value = item.managerDepartmentName;

  getManyManagerEmployeeComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------------
/** 點擊下拉按鈕 */
const clickDropdownBtn = async () => {
  if (props.disabled) return;

  getManyManagerEmployeeComboBoxViewObj.showDropdown =
    !getManyManagerEmployeeComboBoxViewObj.showDropdown;

  if (getManyManagerEmployeeComboBoxViewObj.showDropdown) {
    // 清空關鍵字查詢，以顯示所有選項
    getManyManagerEmployeeComboBoxViewObj.keywordQuery = null;

    // 清空人員選擇，保留區域/部門條件
    managerEmployeeID.value = null;
    managerEmployeeName.value = null;

    getManyManagerEmployeeComboBoxViewObj.pageIndex = 1;
    getManyManagerEmployeeComboBoxViewObj.employeeList = [];
    getManyManagerEmployeeComboBoxViewObj.hasMoreData = true;

    const fetchedList = await getData(getManyManagerEmployeeComboBoxViewObj.pageIndex);
    getManyManagerEmployeeComboBoxViewObj.employeeList.push(...fetchedList);
    getManyManagerEmployeeComboBoxViewObj.pageIndex++;
  }
};
//-------------------------------------------------------------------
/** 關閉下拉 */
const closeDropdown = () => {
  getManyManagerEmployeeComboBoxViewObj.showDropdown = false;
};
//-------------------------------------------------------------------
watch(
  () => managerEmployeeName.value,
  (newValue) => {
    getManyManagerEmployeeComboBoxViewObj.keywordQuery = newValue || null;
    if (!newValue) managerEmployeeID.value = null;
  },
  { immediate: true }
);
//-------------------------------------------------------------------
/** 監聽 managerEmployeeID，當外部設為 null 時清除輸入框 */
watch(
  () => managerEmployeeID.value,
  (newValue) => {
    if (newValue === null) {
      getManyManagerEmployeeComboBoxViewObj.keywordQuery = null;
    }
  }
);
</script>

<template>
  <div v-click-outside="closeDropdown" class="relative flex w-full">
    <input v-model="getManyManagerEmployeeComboBoxViewObj.keywordQuery" type="text" placeholder="查詢員工"
      class="input-box rounded-r-none" :disabled="props.disabled" @focus="focusInput" @input="onInput" />
    <button type="button"
      class="flex items-center justify-center px-2 border border-gray-300 border-l-0 rounded-r-md bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:cursor-not-allowed"
      :disabled="props.disabled" @click="clickDropdownBtn">
      <svg v-if="getManyManagerEmployeeComboBoxViewObj.showDropdown" class="w-5 h-5 text-gray-500" viewBox="0 0 20 25">
        <path
          d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
          fill="#787676" />
      </svg>
      <svg v-else class="w-5 h-5 text-gray-500" viewBox="0 0 20 25">
        <path
          d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
          fill="#787676" />
      </svg>
    </button>

    <transition enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 transform -translate-y-2" enter-to-class="opacity-100 transform translate-y-0"
      leave-active-class="transition-all duration-150 ease-in" leave-from-class="opacity-100 transform translate-y-0"
      leave-to-class="opacity-0 transform -translate-y-2">
      <div v-if="getManyManagerEmployeeComboBoxViewObj.showDropdown && !props.disabled"
        class="absolute top-full left-0 w-full max-h-32 overflow-y-auto border border-gray-300 bg-white z-50 shadow-md rounded-md mt-1"
        @scroll="scrollDropdown">
        <ul v-if="getManyManagerEmployeeComboBoxViewObj.employeeList.length === 0">
          <li class="px-3 py-2 text-sm text-gray-500">查無資料</li>
        </ul>
        <ul v-else>
          <li v-for="item in getManyManagerEmployeeComboBoxViewObj.employeeList" :key="item.employeeID"
            class="px-3 py-2 text-sm cursor-pointer hover:bg-gray-100" @click="selectItem(item)">
            {{ item.employeeName }}
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>
