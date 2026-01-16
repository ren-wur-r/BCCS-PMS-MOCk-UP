<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, watch, onBeforeUnmount } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
import {
  getActivity,
  getActivityEmployeePipeline,
  removeManyActivityEmployeePipeline,
  updateManyActivityEmployeePipeline,
} from "@/services";
import type {
  MbsCrmActHttpGetActivityEmployeePipelineReqMdl,
  MbsCrmActHttpGetActivityEmployeePipelineRspMdl,
  MbsCrmActHttpGetActivityReqMdl,
  MbsCrmActHttpGetActivityRspMdl,
  MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl,
  MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl,
  MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl,
} from "@/services/pms-http/crm/activity/crmActivityHttpFormat";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { getPipelineStatusLabel } from "@/utils/getPipelineStatusLabel";
// import { getEmployeeRangeLabel } from "@/utils/getEmployeeRangeLabel";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import ConfirmDialog from "@/components/global/feedback/ConfirmDialog.vue";
import ActivityDetailTabs from "@/components/feature/activity/ActivityDetailTabs.vue";

//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
/** 路由參數取得 */
const route = useRoute();
//#endregion

//#region 型別定義
/** 名單頁籤種類列舉 */
enum PipelineTabEnum {
  BasicData = "basicData",
  Survey = "servey",
}

/** Crm-活動名單-檢視-頁面模型 */
interface CrmActivityPipelineDetailViewMdl {
  managerCompanyName: string | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  managerCompanyMainKindName: string | null;
  managerCompanySubKindName: string | null;
  managerCompanyLocationAddress: string | null;
  managerCompanyLocationTelephone: string | null;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterTelephone: string | null;
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  managerContacterIsConsent: boolean;
  managerActivityName: string | null;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.CrmActivity;
/** 目前作用中的tab */
const activeTab = ref<PipelineTabEnum>(PipelineTabEnum.BasicData);
/** 名單ID(路由參數取得) */
const managerActivityEmployeePipelineID = Number(route.params.pipelineId);
/** 活動ID(路由參數取得) */
const managerActivityID = Number(route.params.activityId);
/** 是否顯示轉給電銷確認視窗 */
const showTransferConfirm = ref(false);
/** 待轉給電銷的ID */
const pendingTransferId = ref<number | null>(null);
/** 是否顯示刪除確認視窗 */
const showDeleteConfirm = ref(false);
/** 待刪除的ID */
const pendingDeleteId = ref<number | null>(null);
/** 客戶資訊區塊是否展開 */
const isCompanyOpen = ref(true);
/** 窗口資訊區塊是否展開 */
const isContacterOpen = ref(true);

/** Crm-活動名單-檢視-頁面物件 */
const crmActivityPipelineDetailViewObj = reactive<CrmActivityPipelineDetailViewMdl>({
  managerCompanyName: "",
  atomEmployeeRange: DbAtomEmployeeRangeEnum.NotSelected,
  managerCompanyMainKindName: "",
  managerCompanySubKindName: "",
  managerCompanyLocationAddress: "",
  managerCompanyLocationTelephone: "",
  managerContacterName: "",
  managerContacterEmail: "",
  managerContacterPhone: "",
  managerContacterDepartment: "",
  managerContacterJobTitle: "",
  managerContacterTelephone: "",
  atomPipelineStatus: DbAtomPipelineStatusEnum.Hyphen,
  managerContacterIsConsent: false,
  managerActivityName: "",
});
//#endregion

//#region UI行為
/** 切換tab */
const changeTab = (tab: PipelineTabEnum) => {
  activeTab.value = tab;
  if (tab === PipelineTabEnum.BasicData) {
    getActivityPipelineData();
  }
};
//#endregion

//#region API / 資料流程
/** 取得【名單】資料 */
const getActivityPipelineData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  //呼叫 api
  const requestData: MbsCrmActHttpGetActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: managerActivityEmployeePipelineID,
  };
  const responseData: MbsCrmActHttpGetActivityEmployeePipelineRspMdl | null =
    await getActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.managerCompanyName = responseData.managerCompanyName;
  crmActivityPipelineDetailViewObj.atomEmployeeRange = responseData.atomEmployeeRange;
  crmActivityPipelineDetailViewObj.managerCompanyMainKindName =
    responseData.managerCompanyMainKindName;
  crmActivityPipelineDetailViewObj.managerCompanySubKindName =
    responseData.managerCompanySubKindName;
  crmActivityPipelineDetailViewObj.managerCompanyLocationAddress =
    responseData.managerCompanyLocationAddress;
  crmActivityPipelineDetailViewObj.managerCompanyLocationTelephone =
    responseData.managerCompanyLocationTelephone;
  crmActivityPipelineDetailViewObj.managerContacterName = responseData.managerContacterName;
  crmActivityPipelineDetailViewObj.managerContacterEmail = responseData.managerContacterEmail;
  crmActivityPipelineDetailViewObj.managerContacterPhone = responseData.managerContacterPhone;
  crmActivityPipelineDetailViewObj.managerContacterDepartment =
    responseData.managerContacterDepartment;
  crmActivityPipelineDetailViewObj.managerContacterJobTitle = responseData.managerContacterJobTitle;
  crmActivityPipelineDetailViewObj.managerContacterTelephone =
    responseData.managerContacterTelephone;
  crmActivityPipelineDetailViewObj.atomPipelineStatus = responseData.atomPipelineStatus;
  crmActivityPipelineDetailViewObj.managerContacterIsConsent =
    responseData.managerContacterIsConsent;
};

/** 取得【活動】資料(獲取此活動的名稱) */
const getActivityData = async () => {
  // 驗證 token
  if (!requireToken()) return;

  //呼叫 api
  const requestData: MbsCrmActHttpGetActivityReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerActivityID: managerActivityID,
  };
  const responseData: MbsCrmActHttpGetActivityRspMdl | null = await getActivity(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 處理資料
  crmActivityPipelineDetailViewObj.managerActivityName = responseData.managerActivityName;
};

/** 確認轉給電銷 */
const confirmTransferToSales = async () => {
  // 關閉確認視窗
  showTransferConfirm.value = false;

  // 驗證 token
  if (!requireToken()) return;

  // 驗證待轉給電銷ID
  if (pendingTransferId.value === null) {
    displayError("待轉給電銷 ID 錯誤！");
    return;
  }

  // 呼叫 API
  const requestData: MbsCrmActHttpUpdateManyActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineIDList: [pendingTransferId.value],
    atomPipelineStatus: DbAtomPipelineStatusEnum.TransferredToSales,
  };
  const responseData: MbsCrmActHttpUpdateManyActivityEmployeePipelineRspMdl | null =
    await updateManyActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("轉給電銷成功！");

  // 重新取得
  getActivityPipelineData();

  // 清除待轉給電銷ID
  pendingTransferId.value = null;
};

/** 確認刪除名單 */
const confirmDeletePipeline = async () => {
  // 關閉確認視窗
  showDeleteConfirm.value = false;

  // 驗證 token
  if (!requireToken()) return;

  // 驗證待刪除ID
  if (pendingDeleteId.value === null) {
    displayError("待刪除 ID 錯誤！");
    return;
  }

  // 呼叫 API
  const requestData: MbsCrmActHttpRemoveManyActivityEmployeePipelineReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineIDList: [pendingDeleteId.value],
  };
  const responseData: MbsCrmActHttpRemoveManyActivityEmployeePipelineRspMdl | null =
    await removeManyActivityEmployeePipeline(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("刪除名單成功！");

  // 返回名單列表頁
  setTimeout(() => {
    router.push(`/crm/activity/activity/detail/${managerActivityID}/pipeline`);
  }, 1500);

  // 清除待刪除ID
  pendingDeleteId.value = null;
};
//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push(`/crm/activity/activity/detail/${managerActivityID}/pipeline`);
};

/** 點擊【轉給電銷】按鈕 */
const clickTransferredToSalesBtn = (id: number) => {
  pendingTransferId.value = id;
  showTransferConfirm.value = true;
};

/** 點擊【刪除名單】按鈕 */
const clickDeletePipelineBtn = (id: number) => {
  pendingDeleteId.value = id;
  showDeleteConfirm.value = true;
};

//#endregion

//#region 彈跳視窗事件
/** 取消轉給電銷 */
const cancelTransferToSales = () => {
  showTransferConfirm.value = false;
  pendingTransferId.value = null;
};

/** 取消刪除名單 */
const cancelDeletePipeline = () => {
  showDeleteConfirm.value = false;
  pendingDeleteId.value = null;
};

//#endregion

//#region 生命週期
onMounted(() => {
  // 取得名單資料
  getActivityPipelineData();
  // 取得活動資料
  getActivityData();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<PipelineTabEnum, string> = {
      [PipelineTabEnum.BasicData]: "基本資料",
      [PipelineTabEnum.Survey]: "問卷",
    };
    setModuleTitle(titleMap[tab] ?? "基本資料");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});
//#endregion

</script>

<template>
  <div class="flex flex-col p-4 gap-4">
    <div class="flex items-center flex-row gap-2 justify-between">
      <div>
        <!-- 返回按鈕 -->
        <button class="btn-back flex items-center" @click="clickBackBtn">
          <svg class="w-4 h-4 inline-block mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
          </svg>
          {{ crmActivityPipelineDetailViewObj.managerActivityName ?? "-" }}
        </button>
      </div>

      <div class="flex flex-row justify-end gap-1">
        <!-- 轉給電銷按鈕 -->
        <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')" class="btn-search" :disabled="crmActivityPipelineDetailViewObj.atomPipelineStatus !== DbAtomPipelineStatusEnum.Hyphen
          && crmActivityPipelineDetailViewObj.atomPipelineStatus !== DbAtomPipelineStatusEnum.Opened
          && crmActivityPipelineDetailViewObj.atomPipelineStatus !== DbAtomPipelineStatusEnum.Clicked
          " @click="clickTransferredToSalesBtn(managerActivityEmployeePipelineID)">
          轉給電銷
        </button>
        <!-- 刪除按鈕 -->
        <button v-if="employeeInfoStore.hasEveryonePermission(menu, 'delete')" :disabled="crmActivityPipelineDetailViewObj.atomPipelineStatus !== DbAtomPipelineStatusEnum.Hyphen
          && crmActivityPipelineDetailViewObj.atomPipelineStatus !== DbAtomPipelineStatusEnum.Opened
          && crmActivityPipelineDetailViewObj.atomPipelineStatus !== DbAtomPipelineStatusEnum.Clicked
          " class="btn-delete" @click="clickDeletePipelineBtn(managerActivityEmployeePipelineID)">
          刪除
        </button>
      </div>
    </div>

    <ActivityDetailTabs :activity-id="managerActivityID" base-path="/crm/activity/activity" />

    <div>
      <!-- Tabs選單 -->
      <div class="flex mb-0 gap-y-4">
        <button :class="['tab-btn', { active: activeTab === PipelineTabEnum.BasicData }]"
          @click="changeTab(PipelineTabEnum.BasicData)">
          基本資料
        </button>
        <!-- <button
        :class="['tab-btn', { active: activeTab === PipelineTabEnum.Survey }]"
        @click="changeTab(PipelineTabEnum.Survey)"
      >
        問卷填寫
      </button> -->
      </div>

      <!-- Tabs內容 -->
      <div>
        <!-- 基本資料內容 -->
        <div v-if="activeTab === PipelineTabEnum.BasicData" class="tab flex flex-col gap-4">
          <!-- 基本資訊區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4 rounded-tl-none">
            <div class="subtitle">基本資訊</div>
            <!-- 狀態 -->
            <div>
              <label class="form-label">狀態</label>
              <select v-model="crmActivityPipelineDetailViewObj.atomPipelineStatus" class="select-box" disabled>
                <option :value="DbAtomPipelineStatusEnum.Hyphen">-</option>
                <option :value="DbAtomPipelineStatusEnum.Opened">已開啟</option>
                <option :value="DbAtomPipelineStatusEnum.Clicked">已點擊</option>
                <option :value="DbAtomPipelineStatusEnum.TransferredToSales">已轉電銷</option>
                <option :value="DbAtomPipelineStatusEnum.CompletedSales">已完成電銷</option>
                <option :value="DbAtomPipelineStatusEnum.TransferredToBusiness">已轉業務</option>
                <option :value="DbAtomPipelineStatusEnum.Business10Percent">商機10%</option>
                <option :value="DbAtomPipelineStatusEnum.Business30Percent">商機30%</option>
                <option :value="DbAtomPipelineStatusEnum.Business70Percent">商機70%</option>
                <option :value="DbAtomPipelineStatusEnum.Business100Percent">商機100%</option>
                <option :value="DbAtomPipelineStatusEnum.BusinessFailed">商機失敗</option>
                <option :value="DbAtomPipelineStatusEnum.TransferredToProject">已轉專案</option>
              </select>
            </div>
          </div>

          <!-- 客戶資訊區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="subtitle">客戶</div>
            <div class="bg-white rounded-lg border border-gray-200">
              <!-- 標題列 -->
              <button
                class="w-full flex justify-between items-center px-4 py-2 bg-gray-50 border-b border-gray-200 rounded-t-lg"
                @click="isCompanyOpen = !isCompanyOpen">
                <span class="text-brand-201 font-semibold">原始客戶</span>
                <svg :class="[
                  'w-4 h-4 transform transition-transform duration-200',
                  isCompanyOpen ? 'rotate-180' : 'rotate-0',
                ]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </button>

              <!-- 客戶內容 -->
              <div v-show="isCompanyOpen" class="p-8 text-sm text-gray-700">
                <div class="grid grid-cols-2 gap-x-6 gap-y-2">
                  <!-- 客戶全名 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">客戶全名</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerCompanyName || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 人員規模 -->
                  <!-- <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">人員規模</span>
                      <span class="display-value truncate">
                        {{
                          getEmployeeRangeLabel(
                            crmActivityPipelineDetailViewObj.atomEmployeeRange
                          ) || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div> -->

                  <!-- 客戶主分類 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">客戶主分類</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerCompanyMainKindName || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 客戶子分類 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">客戶子分類</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerCompanySubKindName || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 地址 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">地址</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerCompanyLocationAddress || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 電話 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">電話</span>
                      <span class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.managerCompanyLocationTelephone || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- 窗口資訊區塊 -->
          <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
            <div class="subtitle">窗口</div>
            <div class="bg-white rounded-lg border border-gray-200">
              <!-- 標題列 -->
              <button
                class="w-full flex justify-between items-center px-4 py-2 bg-gray-50 border-b border-gray-200 rounded-t-lg"
                @click="isContacterOpen = !isContacterOpen">
                <span class="text-brand-201 font-semibold">原始窗口</span>
                <svg :class="[
                  'w-4 h-4 transform transition-transform duration-200',
                  isCompanyOpen ? 'rotate-180' : 'rotate-0',
                ]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </button>

              <!-- 窗口內容區塊 -->
              <div v-show="isContacterOpen" class="p-8 text-sm text-gray-700">
                <div class="grid grid-cols-2 gap-x-6 gap-y-2">
                  <!-- 姓名 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">姓名</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerContacterName || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 電子信箱 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">電子信箱</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerContacterEmail || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 手機 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">手機</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerContacterPhone || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 部門 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">部門</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerContacterDepartment || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 職稱 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">職稱</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerContacterJobTitle || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 電話#分機 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">電話#分機</span>
                      <span class="display-value truncate">
                        {{ crmActivityPipelineDetailViewObj.managerContacterTelephone || "-" }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 狀態 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">狀態</span>
                      <span class="display-value truncate">
                        {{
                          getPipelineStatusLabel(
                            crmActivityPipelineDetailViewObj.atomPipelineStatus
                          ) || "-"
                        }}
                      </span>
                    </p>
                    <hr class="my-2" />
                  </div>

                  <!-- 個資同意 -->
                  <div>
                    <p class="flex flex-row gap-5">
                      <span class="display-label w-20 shrink-0">個資同意</span>
                      <span v-if="crmActivityPipelineDetailViewObj.managerContacterIsConsent !== null"
                        class="display-value truncate">
                        {{
                          crmActivityPipelineDetailViewObj.managerContacterIsConsent
                            ? "同意"
                            : "不同意"
                        }}
                      </span>
                      <span v-else class="display-value truncate">-</span>
                    </p>
                    <hr class="my-2" />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 錯誤訊息彈跳視窗 -->
    <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

    <!-- 成功訊息提示 -->
    <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

    <!-- 轉給電銷確認彈跳視窗 -->
    <ConfirmDialog :show="showTransferConfirm" title="確認轉給電銷" message="確定要將此名單轉給電銷嗎？" @confirm="confirmTransferToSales"
      @cancel="cancelTransferToSales" />

    <!-- 刪除名單確認彈跳視窗 -->
    <ConfirmDialog :show="showDeleteConfirm" title="確認刪除" message="確定要刪除此名單嗎？刪除後將無法復原。" @confirm="confirmDeletePipeline"
      @cancel="cancelDeletePipeline" />
  </div>
</template>
