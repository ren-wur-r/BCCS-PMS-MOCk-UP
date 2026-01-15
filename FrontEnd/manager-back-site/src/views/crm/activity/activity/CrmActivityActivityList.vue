<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, computed } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsCrmActHttpGetManyActivityReqMdl,
  MbsCrmActHttpGetManyActivityRspMdl,
  MbsCrmActHttpGetManyActivityRspItemMdl,
} from "@/services/pms-http/crm/activity/crmActivityHttpFormat";
import { getManyActivity } from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { getManagerActivityKindLabel } from "@/utils/getManagerActivityKindLabel";
import {
  formatToServerDateEndISO8,
  formatDateTime,
  formatToServerDateStartISO8,
} from "@/utils/timeFormatter";
import router from "@/router";
import VueDatePicker from "@vuepic/vue-datepicker";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";

//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
//#endregion

//#region 型別定義
/** CRM-活動管理-活動-列表-查詢模型 */
interface CrmActivityActivityListQueryMdl {
  managerActivityStartTime: string | null;
  managerActivityEndTime: string | null;
  managerActivityKind: DbAtomManagerActivityKindEnum | null;
  managerActivityName: string | null;
  pageIndex: number;
  pageSize: number;
}

/** CRM-活動管理-活動-列表-項目模型 */
interface CrmActivityActivityListItemMdl {
  managerActivityID: number;
  managerActivityName: string;
  managerActivityKind: DbAtomManagerActivityKindEnum;
  managerActivityStartTime: string;
  managerActivityEndTime: string;
  managerActivityLocation: string;
  managerActivityExpectedLeadCount: number;
  managerActivityRegisteredCount: number;
  managerActivityTransferredToSalesCount: number;
  managerActivitySponsorTotalSponsorAmount: number;
  managerActivityExpenseTotalExpenseAmount: number;
  managerActivityOpportunityCount: number;
}

/** CRM-活動管理-活動-列表-頁面模型 */
interface CrmActivityActivityListViewMdl {
  query: CrmActivityActivityListQueryMdl;
  managerActivityList: CrmActivityActivityListItemMdl[];
  totalCount: number;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmActivity;
/** 活動時間範圍 */
const activityTimeRange = ref<[Date, Date] | null>(null);
/** CRM-活動管理-活動-列表-頁面物件 */
const crmActivityActivityListViewObj = reactive<CrmActivityActivityListViewMdl>({
  query: {
    managerActivityStartTime: null,
    managerActivityEndTime: null,
    managerActivityKind: null,
    managerActivityName: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerActivityList: [],
  totalCount: 0,
});

const canViewActivity = computed(() =>
  employeeInfoStore.hasEveryonePermission(menu, "view")
);
//#endregion

//#region API / 資料流程
/** 取得列表 */
const getList = async () => {
  // 驗證 token
  if (!requireToken()) return;

  // 處理日期範圍
  const startDate = activityTimeRange.value ? activityTimeRange.value[0] : null;
  const endDate = activityTimeRange.value ? activityTimeRange.value[1] : null;
  let formattedStartDate = null;
  let formattedEndDate = null;
  if (startDate) {
    formattedStartDate = formatToServerDateStartISO8(startDate);
  }
  if (endDate) {
    formattedEndDate = formatToServerDateEndISO8(endDate);
  }

  // 呼叫 API
  const requestData: MbsCrmActHttpGetManyActivityReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityStartTime: formattedStartDate,
    managerActivityEndTime: formattedEndDate,
    managerActivityKind: crmActivityActivityListViewObj.query.managerActivityKind,
    managerActivityName: crmActivityActivityListViewObj.query.managerActivityName,
    pageIndex: crmActivityActivityListViewObj.query.pageIndex,
    pageSize: crmActivityActivityListViewObj.query.pageSize,
  };
  const responseData: MbsCrmActHttpGetManyActivityRspMdl | null =
    await getManyActivity(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 包裝資料
  crmActivityActivityListViewObj.managerActivityList =
    responseData.managerActivityList?.map(
      (item: MbsCrmActHttpGetManyActivityRspItemMdl) =>
        ({
          managerActivityID: item.managerActivityID,
          managerActivityName: item.managerActivityName,
          managerActivityKind: item.managerActivityKind,
          managerActivityStartTime: item.managerActivityStartTime,
          managerActivityEndTime: item.managerActivityEndTime,
          managerActivityLocation: item.managerActivityLocation,
          managerActivityExpectedLeadCount: item.managerActivityExpectedLeadCount,
          managerActivityRegisteredCount: item.managerActivityRegisteredCount,
          managerActivityTransferredToSalesCount: item.managerActivityTransferredToSalesCount,
          managerActivitySponsorTotalSponsorAmount: item.managerActivitySponsorTotalSponsorAmount,
          managerActivityExpenseTotalExpenseAmount: item.managerActivityExpenseTotalExpenseAmount,
          managerActivityOpportunityCount: item.managerActivityOpportunityCount,
        }) satisfies CrmActivityActivityListItemMdl
    ) ?? [];
  crmActivityActivityListViewObj.totalCount = responseData.totalCount;
};
//#endregion

//#region 按鈕事件
/** 點擊【查詢】按鈕 */
const clickSearchBtn = () => {
  crmActivityActivityListViewObj.query.pageIndex = 1;
  getList();
};

/** 點擊【清除】按鈕 */
const clickClearSearchBtn = () => {
  activityTimeRange.value = null;
  crmActivityActivityListViewObj.query.managerActivityStartTime = null;
  crmActivityActivityListViewObj.query.managerActivityEndTime = null;
  crmActivityActivityListViewObj.query.managerActivityKind = null;
  crmActivityActivityListViewObj.query.managerActivityName = null;
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  router.push("/crm/activity/activity/add");
};

/** 點擊檢視活動 */
const handleRowClick = (id: number) => {
  if (!canViewActivity.value) return;
  router.push(`/crm/activity/activity/detail/${id}`);
};

//#endregion

//#region 生命週期
onMounted(() => {
  getList();
});
//#endregion
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden gap-4 p-2">
    <!-- 標題列 -->
    <div v-once class="flex items-center justify-between"></div>

    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-6 gap-4">
      <!-- 查詢條件 -->
      <div class="flex items-end gap-4">
        <div class="flex gap-2">
          <div>
<VueDatePicker
              v-model="activityTimeRange"
              range
              format="yyyy-MM-dd"
              :enable-time-picker="false"
              placeholder="選擇時間範圍"
              class="date-picker-custom"
            />
          </div>

          <div>
<input
              v-model="crmActivityActivityListViewObj.query.managerActivityName"
              type="text"
              class="input-box"
              placeholder="活動名稱"
            />
          </div>

          <div>
<select
              v-model="crmActivityActivityListViewObj.query.managerActivityKind"
              class="select-box"
            >
              <option :value="null">全部</option>
              <option :value="DbAtomManagerActivityKindEnum.PhysicalActivity">實體</option>
              <option :value="DbAtomManagerActivityKindEnum.OnlineActivity">線上</option>
            </select>
          </div>
        </div>

        <div>
          <button class="btn-search me-1" @click="clickSearchBtn">查詢</button>
          <button class="btn-cancel" @click="clickClearSearchBtn">清除</button>
        </div>
      </div>

      <hr />

      <!-- 列表 -->
      <div class="flex-1 overflow-y-auto table-scrollable">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-36">開始時間</th>
              <th class="text-start w-36">結束時間</th>
              <th class="text-start w-40">活動名稱</th>
              <th class="text-center w-24">活動類型</th>
              <th class="text-end w-24">期望名單數</th>
              <th class="text-end w-24">實際名單數</th>
              <th class="text-end w-24">已轉電銷數</th>
              <!-- <th class="text-end">總贊助</th> -->
              <!-- <th class="text-end">總支出</th> -->
              <th class="text-end w-24">商機數</th>
            </tr>
          </thead>
          <tbody>
            <template v-if="crmActivityActivityListViewObj.managerActivityList.length === 0">
              <tr class="text-center">
                <td colspan="8">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="item in crmActivityActivityListViewObj.managerActivityList"
                :key="item.managerActivityID"
                :class="[
                  canViewActivity
                    ? 'cursor-pointer transition hover:bg-cyan-50'
                    : 'opacity-60',
                ]"
                @click="handleRowClick(item.managerActivityID)"
              >
                <td class="text-start">
                  {{ formatDateTime(item.managerActivityStartTime) }}
                </td>
                <td class="text-start">
                  {{ formatDateTime(item.managerActivityEndTime) }}
                </td>
                <td class="text-start">
                  {{ item.managerActivityName }}
                </td>
                <td class="text-center">
                  {{ getManagerActivityKindLabel(item.managerActivityKind) }}
                </td>
                <td class="text-end">
                  {{ item.managerActivityExpectedLeadCount }}
                </td>
                <td class="text-end">
                  {{ item.managerActivityRegisteredCount }}
                </td>
                <td class="text-end">
                  {{ item.managerActivityTransferredToSalesCount }}
                </td>
                <!-- <td class="text-end">
                  {{ formatCurrency(item.managerActivitySponsorTotalSponsorAmount) }}
                </td> -->
                <!-- <td class="text-end">
                  {{ formatCurrency(item.managerActivityExpenseTotalExpenseAmount) }}
                </td> -->
                <td class="text-end">{{ item.managerActivityOpportunityCount }}</td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>
      <button
        v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
        @click="clickAddBtn"
      >
        +新增活動
      </button>

      <div class="flex justify-center pt-3">
        <!-- 分頁 -->
        <Pagination
          :pageIndex="crmActivityActivityListViewObj.query.pageIndex"
          :pageSize="crmActivityActivityListViewObj.query.pageSize"
          :totalCount="crmActivityActivityListViewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              crmActivityActivityListViewObj.query.pageIndex = newPage;
              getList();
            }
          "
          @update:page-size="
            (newSize: number) => {
              crmActivityActivityListViewObj.query.pageSize = newSize;
              crmActivityActivityListViewObj.query.pageIndex = 1;
              getList();
            }
          "
        />
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
</template>

<style scoped>
.date-picker-custom {
  width: 200px; /* 設定寬度 */
}

/* DatePicker */
.date-picker-custom :deep(.dp__input_wrap) {
  height: 32px !important;
}

.date-picker-custom :deep(.dp__input) {
  height: 32px !important;
  padding: 0 12px !important;
  border: 1px solid #d1d5db !important;
  border-radius: 4px !important;
  font-size: 14px !important;
  line-height: 1.5 !important;
  box-sizing: border-box !important;
}

.date-picker-custom :deep(.dp__input:focus) {
  border-color: #3b82f6 !important;
  outline: none !important;
}

.date-picker-custom :deep(.dp__input:hover:not(.dp__input_focus)) {
  border-color: #9ca3af !important;
}

/* 調整圖示位置 */
.date-picker-custom :deep(.dp__input_icon),
.date-picker-custom :deep(.dp--clear-btn) {
  height: 16px !important;
  width: 16px !important;
  display: none !important;
}
</style>
