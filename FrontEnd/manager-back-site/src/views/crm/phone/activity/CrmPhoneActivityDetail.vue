<script setup lang="ts">
//#region 引入
// Vue
import { ref, reactive, onMounted } from "vue";
// Enums / 常數
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
// Stores
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
// Composables
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { useRouteParamId } from "@/composables/useRouteParamId";
// Services
import type {
  MbsCrmPhnHttpGetActivityReqMdl,
  MbsCrmPhnHttpGetActivityRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import { getPhoneActivity } from "@/services/index";
// Utils
import { getFullManagerActivityKindLabel } from "@/utils/getManagerActivityKindLabel";
import { formatToDatetimeLocal } from "@/utils/timeFormatter";
// Components
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import TextCounter from "@/components/global/counter/TextCounter.vue";
import ProductListTable from "./components/ProductListTable.vue";
import ActivityDetailTabs from "@/components/feature/activity/ActivityDetailTabs.vue";
// Router
import router from "@/router";
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
/** CRM-電銷管理-活動-檢視-頁面模型 */
interface CrmPhoneActivityDetailViewMdl {
  managerActivityID: number | null;
  managerActivityName: string | null;
  managerActivityKind: DbAtomManagerActivityKindEnum;
  managerActivityStartTime: string | null;
  managerActivityEndTime: string | null;
  managerActivityLocation: string | null;
  managerActivityExpectedLeadCount: number | null;
  managerActivityContent: string | null;
  managerActivityRegisteredCount: number | null;
  managerActivityTransferredToSalesCount: number | null;
  managerActivityEmployeePipelineCount: number | null;
  managerActivityPhoneCount: number | null;
  managerActivityProductList: ManagerActivityProductItemMdl[];
}
/** CRM-電銷管理-活動-產品項目 */
interface ManagerActivityProductItemMdl {
  managerActivityProductID: number;
  managerProductName: string;
  managerProductMainKindName: string;
  managerProductSubKindName: string;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmPhone;
/** 活動ID */
const activityId = useRouteParamId("activityId");
/** 是否為編輯模式 */
const isEditMode = ref(false);

/** CRM-電銷管理-活動-檢視-頁面物件 */
const crmPhoneActivityDetailViewObj = reactive<CrmPhoneActivityDetailViewMdl>({
  managerActivityID: activityId,
  managerActivityName: null,
  managerActivityKind: DbAtomManagerActivityKindEnum.PhysicalActivity,
  managerActivityStartTime: null,
  managerActivityEndTime: null,
  managerActivityLocation: null,
  managerActivityExpectedLeadCount: null,
  managerActivityContent: null,
  managerActivityRegisteredCount: null,
  managerActivityTransferredToSalesCount: null,
  managerActivityEmployeePipelineCount: null,
  managerActivityPhoneCount: null,
  managerActivityProductList: [],
});
//#endregion

//#region API / 資料流程
/** 取得資料 */
const getData = async () => {
  // 驗證 Token
  if (!requireToken()) return;

  // 驗證活動ID
  if (
    !crmPhoneActivityDetailViewObj.managerActivityID ||
    isNaN(crmPhoneActivityDetailViewObj.managerActivityID) ||
    crmPhoneActivityDetailViewObj.managerActivityID <= 0
  ) {
    displayError("活動ID 不正確！");
    router.push("/crm/phone/activity");
    return;
  }

  // 呼叫 API
  const requestData: MbsCrmPhnHttpGetActivityReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: crmPhoneActivityDetailViewObj.managerActivityID!,
  };
  const responseData: MbsCrmPhnHttpGetActivityRspMdl | null = await getPhoneActivity(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 更新頁面物件
  crmPhoneActivityDetailViewObj.managerActivityName = responseData.managerActivityName;
  crmPhoneActivityDetailViewObj.managerActivityKind = responseData.managerActivityKind;
  crmPhoneActivityDetailViewObj.managerActivityStartTime = formatToDatetimeLocal(
    responseData.managerActivityStartTime
  );
  crmPhoneActivityDetailViewObj.managerActivityEndTime = formatToDatetimeLocal(
    responseData.managerActivityEndTime
  );
  crmPhoneActivityDetailViewObj.managerActivityLocation = responseData.managerActivityLocation;
  crmPhoneActivityDetailViewObj.managerActivityExpectedLeadCount =
    responseData.managerActivityExpectedLeadCount;
  crmPhoneActivityDetailViewObj.managerActivityContent = responseData.managerActivityContent;
  crmPhoneActivityDetailViewObj.managerActivityRegisteredCount =
    responseData.managerActivityRegisteredCount;
  crmPhoneActivityDetailViewObj.managerActivityTransferredToSalesCount =
    responseData.managerActivityTransferredToSalesCount;
  crmPhoneActivityDetailViewObj.managerActivityEmployeePipelineCount =
    responseData.managerActivityEmployeePipelineCount;
  crmPhoneActivityDetailViewObj.managerActivityPhoneCount = responseData.managerActivityPhoneCount;
  crmPhoneActivityDetailViewObj.managerActivityProductList =
    responseData.managerActivityProductList ?? [];
};
//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/crm/phone/activity");
};

//#endregion

//#region 生命週期
onMounted(() => {
  getData();
});
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <!-- 標題區 -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-4">
        <button class="btn-back flex items-center" @click="clickBackBtn">
          <svg
            class="w-4 h-4 inline-block mr-1"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M15 19l-7-7 7-7"
            />
          </svg>
          <span>返回</span>
        </button>
      </div>
    </div>

    <ActivityDetailTabs :activity-id="activityId" base-path="/crm/phone/activity" />

    <!-- 內容 -->
    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-8 gap-4">
      <div class="subtitle">活動資訊</div>

      <!-- 活動類型 -->
      <div class="flex flex-col gap-2 mb-2">
        <label class="form-label">活動類型 <span class="required-label">*</span></label>
        <button type="button" class="btn-activity-type-disabled" disabled>
          {{ getFullManagerActivityKindLabel(crmPhoneActivityDetailViewObj.managerActivityKind) }}
        </button>
      </div>

      <!-- 活動名稱 -->
      <div class="flex flex-col gap-2 flex-1">
        <label class="form-label">活動名稱 <span class="required-label">*</span></label>
        <input
          v-model="crmPhoneActivityDetailViewObj.managerActivityName"
          type="text"
          class="input-box"
          :disabled="!isEditMode"
          maxlength="100"
        />
      </div>

      <div class="flex gap-4">
        <!-- 開始時間 -->
        <div class="flex flex-col gap-2 flex-1">
          <label class="form-label">開始時間 <span class="required-label">*</span></label>
          <input
            v-model="crmPhoneActivityDetailViewObj.managerActivityStartTime"
            type="datetime-local"
            class="input-box"
            disabled
          />
        </div>

        <!-- 結束時間 -->
        <div class="flex flex-col gap-2 flex-1">
          <label class="form-label">結束時間 <span class="required-label">*</span></label>
          <input
            v-model="crmPhoneActivityDetailViewObj.managerActivityEndTime"
            type="datetime-local"
            class="input-box"
            :disabled="!isEditMode"
          />
        </div>
      </div>

      <!-- 地點 -->
      <div>
        <label class="form-label"> 地點 <span class="required-label">*</span> </label>
        <input
          v-model="crmPhoneActivityDetailViewObj.managerActivityLocation"
          type="text"
          class="input-box"
          :disabled="
            !isEditMode ||
            crmPhoneActivityDetailViewObj.managerActivityKind ===
              DbAtomManagerActivityKindEnum.OnlineActivity
          "
          maxlength="200"
        />
      </div>

      <!-- 期望名單數 -->
      <div>
        <label class="form-label">期望名單數 <span class="required-label">*</span></label>
        <input
          v-model="crmPhoneActivityDetailViewObj.managerActivityExpectedLeadCount"
          type="number"
          class="input-box"
          :disabled="!isEditMode"
          min="0"
        />
      </div>

      <!-- 活動內容與備註 -->
      <div>
        <label class="form-label">活動內容與備註</label>
        <div class="select-wrapper">
          <textarea
            v-model="crmPhoneActivityDetailViewObj.managerActivityContent"
            type="text"
            class="textarea-box"
            rows="15"
            :disabled="!isEditMode"
            maxlength="2000"
          ></textarea>
        </div>
        <div>
          <TextCounter
            :text="crmPhoneActivityDetailViewObj.managerActivityContent"
            :max-length="2000"
            :required="false"
          />
        </div>
      </div>
    </div>

    <!-- 活動產品 區塊 -->
    <div v-if="!isEditMode" class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between items-center">
        <h2 class="subtitle">活動產品</h2>
      </div>

      <ProductListTable
        :productList="crmPhoneActivityDetailViewObj.managerActivityProductList"
        :show-action-row="false"
        :showAppendBtn="false"
      />
    </div>

    <!-- 活動名單 區塊 -->
    <div v-if="!isEditMode" class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="subtitle">活動名單</div>

      <div class="flex gap-2">
        <div class="px-4 py-3 border border-gray-300 bg-gray-100 rounded-lg w-36 text-start">
          <p class="text-sm">實際名單數</p>
          {{ crmPhoneActivityDetailViewObj.managerActivityRegisteredCount }}
        </div>

        <div class="px-4 py-3 border border-gray-300 bg-gray-100 rounded-lg w-36 text-start">
          <p class="text-sm">已轉電銷數</p>
          {{ crmPhoneActivityDetailViewObj.managerActivityTransferredToSalesCount }}
        </div>

        <div class="px-4 py-3 border border-gray-300 bg-gray-100 rounded-lg w-36 text-start">
          <p class="text-sm">已撥打數</p>
          {{ crmPhoneActivityDetailViewObj.managerActivityPhoneCount }}
        </div>

        <div class="px-4 py-3 border border-gray-300 bg-gray-100 rounded-lg w-36 text-start">
          <p class="text-sm">商機數</p>
          {{ crmPhoneActivityDetailViewObj.managerActivityEmployeePipelineCount }}
        </div>
      </div>

    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />
  </div>
</template>
