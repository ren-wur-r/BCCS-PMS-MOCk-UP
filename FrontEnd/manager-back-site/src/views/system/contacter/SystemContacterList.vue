<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted, watch, onBeforeUnmount } from "vue";
import { useRouter } from "vue-router";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import { useTokenStore } from "@/stores/token";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useModuleTitleStore } from "@/stores/moduleTitleStore";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";
import {
  getManyContacter,
  getManyContacterRatingReason,
  getContacterRatingReason,
  addContacterRatingReason,
  updateContacterRatingReason,
} from "@/services";
import type {
  MbsSysCttHttpGetManyContacterRatingReasonReqMdl,
  MbsSysCttHttpGetManyContacterRatingReasonRspMdl,
  MbsSysCttHttpGetManyContacterRatingReasonRspItemMdl,
  MbsSysCttHttpGetManyContacterReqMdl,
  MbsSysCttHttpGetManyContacterRspMdl,
  MbsSysCttHttpGetManyContacterRspItemMdl,
  MbsSysCttHttpGetContacterRatingReasonReqMdl,
  MbsSysCttHttpAddContacterRatingReasonReqMdl,
  MbsSysCttHttpUpdateContacterRatingReasonReqMdl,
} from "@/services/pms-http/system/contacter/systemContacterHttpFormat";
import AddSystemContacterRatingReasonModal from "@/views/system/contacter/components/AddSystemContacterRatingReasonModal.vue";
import UpdateSystemContacterRatingReasonModal from "@/views/system/contacter/components/UpdateSystemContacterRatingReasonModal.vue";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import GetManyManagerCompanyComboBox from "@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue";

//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
const { setModuleTitle, clearModuleTitle } = useModuleTitleStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
//#endregion

//#region 型別定義
/** 窗口頁籤種類列舉 */
enum ContacterTabEnum {
  Contacter = "contacter",
  RatingReason = "ratingReason",
}
// -------------------------------------------------------------
/** 系統設定-窗口-列表-查詢模型 */
interface SysContacterQueryMdl {
  managerCompanyID: number | null;
  managerCompanyName: string | null;
  managerContacterRatingKind: DbAtomManagerContacterRatingKindEnum | null;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-窗口-列表-項目模型 */
interface SysContacterItemMdl {
  managerContacterID: number;
  managerCompanyName: string | null;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
}

/** 系統設定-窗口-列表-頁面模型 */
interface SysContacterListViewMdl {
  query: SysContacterQueryMdl;
  managerContacterList: SysContacterItemMdl[];
  totalCount: number;
}
//-------------------------------------------------------------
/** 系統設定-窗口-開發評等原因-列表-查詢模型 */
interface SysCttRatingReasonQueryMdl {
  managerContacterRatingReasonName: string | null;
  managerContacterRatingReasonStatus: boolean | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-窗口-開發評等原因-列表-項目模型 */
interface SysCttRatingReasonItemMdl {
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName: string | null;
  managerContacterRatingReasonStatus: boolean;
}

/** 系統設定-窗口-開發評等原因-列表-頁面模型 */
interface SysCttRatingReasonListViewMdl {
  query: SysCttRatingReasonQueryMdl;
  managerContacterRatingReasonList: SysCttRatingReasonItemMdl[];
  totalCount: number;
}
// -------------------------------------------------------------
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemContacter;
/** 目前作用中的tab */
const activeTab = ref<ContacterTabEnum>(ContacterTabEnum.Contacter);
/** 系統設定-窗口-開發評等原因-新增-Modal-顯示 */
const sysCstRatingReasonAddModalShow = ref(false);
/** 系統設定-窗口-開發評等原因-修改-Modal-顯示 */
const sysCstRatingReasonUpdateModalShow = ref(false);
/** 選取的開發評等原因ID */
const selectedRatingReasonID = ref<number | null>(null);
/** 選取的開發評等原因名稱 */
const selectedRatingReasonName = ref<string>("");
/** 選取的開發評等原因狀態 */
const selectedRatingReasonStatus = ref<boolean>(true);

/** 系統設定-窗口-開發評等原因-列表-頁面物件 */
const sysContacterListViewObj = reactive<SysContacterListViewMdl>({
  query: {
    managerCompanyID: null,
    managerCompanyName: null,
    managerContacterRatingKind: null,
    managerContacterName: null,
    managerContacterEmail: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerContacterList: [],
  totalCount: 0,
});

/** 系統設定-窗口-開發評等原因-列表-頁面物件 */
const sysCttRatingReasonListViewObj = reactive<SysCttRatingReasonListViewMdl>({
  query: {
    managerContacterRatingReasonName: null,
    managerContacterRatingReasonStatus: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerContacterRatingReasonList: [],
  totalCount: 0,
});

//#endregion

//#region UI行為
// 切換tab
const changeTab = (tab: ContacterTabEnum) => {
  activeTab.value = tab;
  if (tab === ContacterTabEnum.RatingReason) {
    getCustomerRatingReasonList();
  } else {
    getContacterList();
  }
};

//#endregion

//#region API / 資料流程
/** 取得【窗口】列表 */
const getContacterList = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 呼叫api
  let requestData: MbsSysCttHttpGetManyContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: sysContacterListViewObj.query.managerCompanyID!,
    atomManagerContacterRatingKind: sysContacterListViewObj.query.managerContacterRatingKind!,
    managerContacterName: sysContacterListViewObj.query.managerContacterName,
    managerContacterEmail: sysContacterListViewObj.query.managerContacterEmail,
    pageIndex: sysContacterListViewObj.query.pageIndex,
    pageSize: sysContacterListViewObj.query.pageSize,
  };
  let responseData: MbsSysCttHttpGetManyContacterRspMdl | null =
    await getManyContacter(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) {
    return;
  }

  sysContacterListViewObj.managerContacterList = responseData.managerContacterList?.map(
    (item: MbsSysCttHttpGetManyContacterRspItemMdl) =>
      ({
        managerContacterID: item.managerContacterID,
        managerCompanyName: item.managerCompanyName,
        managerContacterName: item.managerContacterName,
        managerContacterEmail: item.managerContacterEmail,
        managerContacterDepartment: item.managerContacterDepartment,
        managerContacterJobTitle: item.managerContacterJobTitle,
        managerContacterPhone: item.managerContacterPhone,
        atomManagerContacterRatingKind: item.atomManagerContacterRatingKind,
      }) satisfies SysContacterItemMdl
  );
  sysContacterListViewObj.totalCount = responseData.totalCount;
};

/** 取得【開發評等原因】列表 */
const getCustomerRatingReasonList = async () => {
  if (!requireToken()) return;
  // 呼叫api
  const requestData: MbsSysCttHttpGetManyContacterRatingReasonReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingReasonName:
      sysCttRatingReasonListViewObj.query.managerContacterRatingReasonName,
    managerContacterRatingReasonStatus:
      sysCttRatingReasonListViewObj.query.managerContacterRatingReasonStatus,
    pageIndex: sysCttRatingReasonListViewObj.query.pageIndex,
    pageSize: sysCttRatingReasonListViewObj.query.pageSize,
  };
  let responseData: MbsSysCttHttpGetManyContacterRatingReasonRspMdl | null =
    await getManyContacterRatingReason(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  sysCttRatingReasonListViewObj.managerContacterRatingReasonList =
    responseData.managerContacterRatingReasonList?.map(
      (item: MbsSysCttHttpGetManyContacterRatingReasonRspItemMdl) => ({
        managerContacterRatingReasonID: item.managerContacterRatingReasonID,
        managerContacterRatingReasonName: item.managerContacterRatingReasonName,
        managerContacterRatingReasonStatus: item.managerContacterRatingReasonStatus,
      })
    );
  sysCttRatingReasonListViewObj.totalCount = responseData.totalCount;
};
//#endregion

//#region 按鈕事件
/** 點擊【新增窗口】按鈕 */
const clickAddContacterBtn = () => {
  router.push("/system/contacter/add");
};

/** 點擊【檢視窗口】按鈕 */
const clickDetailContacterBtn = (id: number) => {
  router.push(`/system/contacter/detail/${id}`);
};

/** 點擊【查詢窗口】按鈕 */
const clickSearchContacterBtn = () => {
  sysContacterListViewObj.query.pageIndex = 1;
  getContacterList();
};

/** 點擊【清除查詢窗口】按鈕 */
const clickClearSearchContacterBtn = () => {
  sysContacterListViewObj.query.managerCompanyID = null;
  sysContacterListViewObj.query.managerCompanyName = null;
  sysContacterListViewObj.query.managerContacterRatingKind = null;
  sysContacterListViewObj.query.managerContacterName = null;
  sysContacterListViewObj.query.managerContacterEmail = null;
};

/** 點擊【查詢開發評等原因】按鈕 */
const clickSearchRatingReasonBtn = () => {
  sysCttRatingReasonListViewObj.query.pageIndex = 1;
  getCustomerRatingReasonList();
};

/** 點擊【清除查詢開發評等原因】按鈕 */
const clickClearSearchRatingReasonBtn = () => {
  sysCttRatingReasonListViewObj.query.managerContacterRatingReasonName = null;
  sysCttRatingReasonListViewObj.query.managerContacterRatingReasonStatus = null;
};

/** 點擊【開啟新增開發評等原因 Modal】按鈕 */
const clickOpenAddRatingReasonModalBtn = () => {
  sysCstRatingReasonAddModalShow.value = true;
};

/** 點擊【開啟編輯開發評等原因 Modal】按鈕 */
const clickOpenUpdateRatingReasonModalBtn = async (id: number) => {
  if (!requireToken()) return;

  // 呼叫 API 取得資料
  const requestData: MbsSysCttHttpGetContacterRatingReasonReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingReasonID: id,
  };

  const responseData = await getContacterRatingReason(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 儲存資料並開啟 modal
  selectedRatingReasonID.value = id;
  selectedRatingReasonName.value = responseData.managerContacterRatingReasonName;
  selectedRatingReasonStatus.value = responseData.managerContacterRatingReasonStatus;
  sysCstRatingReasonUpdateModalShow.value = true;
};

//#endregion

//#region 彈跳視窗事件
/** 處理新增開發評等原因 */
const handleAddRatingReasonSubmit = async (data: {
  managerContacterRatingReasonName: string;
  managerContacterRatingReasonStatus: boolean;
}) => {
  if (!requireToken()) return;

  const requestData: MbsSysCttHttpAddContacterRatingReasonReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingReasonName: data.managerContacterRatingReasonName,
    managerContacterRatingReasonStatus: data.managerContacterRatingReasonStatus,
  };

  const responseData = await addContacterRatingReason(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增開發評等原因成功！");

  // 重新整理列表並關閉 modal
  await getCustomerRatingReasonList();
  sysCstRatingReasonAddModalShow.value = false;
};

/** 處理更新開發評等原因 */
const handleUpdateRatingReasonSubmit = async (data: {
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName?: string;
  managerContacterRatingReasonStatus?: boolean;
}) => {
  if (!requireToken()) return;

  const requestData: MbsSysCttHttpUpdateContacterRatingReasonReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingReasonID: data.managerContacterRatingReasonID,
    managerContacterRatingReasonName: data.managerContacterRatingReasonName ?? null,
    managerContacterRatingReasonStatus: data.managerContacterRatingReasonStatus ?? null,
  };

  const responseData = await updateContacterRatingReason(requestData);

  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("更新開發評等原因成功！");

  // 重新整理列表並關閉 modal
  await getCustomerRatingReasonList();
  sysCstRatingReasonUpdateModalShow.value = false;
};
//#endregion

//#region 生命週期
onMounted(() => {
  getContacterList();
});

watch(
  () => activeTab.value,
  (tab) => {
    const titleMap: Record<ContacterTabEnum, string> = {
      [ContacterTabEnum.Contacter]: "窗口",
      [ContacterTabEnum.RatingReason]: "開發評等原因",
    };
    setModuleTitle(titleMap[tab] ?? "窗口");
  },
  { immediate: true }
);

onBeforeUnmount(() => {
  clearModuleTitle();
});
//#endregion

</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden p-2">
    <!-- 標題列 -->
    <div class="flex items-center justify-between mb-4"></div>

    <!-- Tabs選單 -->
    <div class="flex mb-0 gap-y-4">
      <button :class="['tab-btn', { active: activeTab === ContacterTabEnum.Contacter }]"
        @click="changeTab(ContacterTabEnum.Contacter)">
        窗口
      </button>
      <button :class="['tab-btn', { active: activeTab === ContacterTabEnum.RatingReason }]"
        @click="changeTab(ContacterTabEnum.RatingReason)">
        開發評等原因
      </button>
    </div>

    <!-- Tabs內容 -->
    <div class="tab-content flex-1 overflow-hidden">
      <!--窗口-->
      <div v-if="activeTab === ContacterTabEnum.Contacter" class="tab h-full">
        <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg rounded-tl-none p-6 gap-4">
          <!-- 窗口查詢條件 -->
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <!--客戶-->
              <div>
<GetManyManagerCompanyComboBox v-model:managerCompanyID="sysContacterListViewObj.query.managerCompanyID"
                  v-model:managerCompanyName="sysContacterListViewObj.query.managerCompanyName" :disabled="false" />
              </div>

              <!--姓名-->
              <div>
<input v-model="sysContacterListViewObj.query.managerContacterName" type="text" class="input-box"
                  placeholder="姓名" />
              </div>

              <!--電子信箱-->
              <div>
<input v-model="sysContacterListViewObj.query.managerContacterEmail" type="text" class="input-box"
                  placeholder="電子信箱" />
              </div>

              <!--開發評等-->
              <div>
<select v-model="sysContacterListViewObj.query.managerContacterRatingKind" class="select-box">
                  <option :value="null">全部</option>
                  <option :value="DbAtomManagerContacterRatingKindEnum.Whitelist">白名單</option>
                  <option :value="DbAtomManagerContacterRatingKindEnum.Graylist">灰名單</option>
                  <option :value="DbAtomManagerContacterRatingKindEnum.Blacklist">黑名單</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchContacterBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchContacterBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--窗口列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start w-36">客戶</th>
                  <th class="text-start w-28">姓名</th>
                  <th class="text-start w-40">電子信箱</th>
                  <th class="text-start w-32">手機</th>
                  <th class="text-start w-28">部門</th>
                  <th class="text-start w-28">職稱</th>
                  <th class="text-start w-24">開發評等</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysContacterListViewObj.managerContacterList.length === 0">
                  <tr class="text-center">
                    <td colspan="8">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysContacterListViewObj.managerContacterList"
                    :key="item.managerContacterID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'view') &&
                        clickDetailContacterBtn(item.managerContacterID)
                    "
                  >
                    <td class="text-start">
                      {{ item.managerCompanyName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerContacterName }}
                    </td>
                    <td class="text-start">
                      {{ item.managerContacterEmail }}
                    </td>
                    <td class="text-start">
                      {{ item.managerContacterPhone || "-" }}
                    </td>
                    <td class="text-start">
                      {{ item.managerContacterDepartment || "-" }}
                    </td>
                    <td class="text-start">
                      {{ item.managerContacterJobTitle || "-" }}
                    </td>
                    <td class="text-start">
                      {{ getManagerContacterRatingLabel(item.atomManagerContacterRatingKind) }}
                    </td>
                    <td class="text-start">
                      <details
                        v-if="employeeInfoStore.hasEveryonePermission(menu, 'view')"
                        class="relative"
                        @click.stop
                      >
                        <summary class="cursor-pointer list-none text-gray-500 hover:text-gray-700">
                          ...
                        </summary>
                        <div
                          class="absolute right-0 mt-2 w-24 rounded-md border border-gray-200 bg-white shadow z-10"
                        >
                          <button
                            class="w-full px-3 py-2 text-left text-sm hover:bg-gray-50"
                            @click.stop="clickDetailContacterBtn(item.managerContacterID)"
                          >
                            檢視
                          </button>
                        </div>
                      </details>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>

          <button
            v-if="
              activeTab === ContacterTabEnum.Contacter &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickAddContacterBtn"
          >
            +新增窗口
          </button>
          <div v-if="activeTab === ContacterTabEnum.Contacter" class="flex justify-center pt-3">
            <!-- 窗口分頁 -->
            <Pagination :pageIndex="sysContacterListViewObj.query.pageIndex"
              :pageSize="sysContacterListViewObj.query.pageSize" :totalCount="sysContacterListViewObj.totalCount"
              @update:current-page="
                (newPage: number) => {
                  sysContacterListViewObj.query.pageIndex = newPage;
                  getContacterList();
                }
              " @update:page-size="
                (newSize: number) => {
                  sysContacterListViewObj.query.pageSize = newSize;
                  sysContacterListViewObj.query.pageIndex = 1;
                  getContacterList();
                }
              " />
          </div>
        </div>
      </div>

      <!--開發評等原因-->
      <div v-if="activeTab === ContacterTabEnum.RatingReason" class="tab h-full">
        <!-- 開發評等原因查詢條件 -->
        <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg rounded-tl-none p-6 gap-4">
          <div class="flex items-end gap-4">
            <div class="flex gap-2">
              <!--開發評等原因-->
              <div>
<input v-model="sysCttRatingReasonListViewObj.query.managerContacterRatingReasonName" class="input-box"
                  placeholder="開發評等原因名稱" />
              </div>

              <!--狀態-->
              <div>
<select v-model="sysCttRatingReasonListViewObj.query.managerContacterRatingReasonStatus"
                  class="select-box">
                  <option :value="null">全部</option>
                  <option :value="true">啟用</option>
                  <option :value="false">停用</option>
                </select>
              </div>
            </div>

            <div>
              <button class="btn-search me-1" @click="clickSearchRatingReasonBtn">查詢</button>
              <button class="btn-cancel" @click="clickClearSearchRatingReasonBtn">清除</button>
            </div>
          </div>

          <hr />

          <!--開發評等原因列表-->
          <div class="flex-1 overflow-y-auto table-scrollable">
            <table class="table-base table-fixed table-sticky w-full">
              <thead class="sticky top-0 bg-white z-10">
                <tr>
                  <th class="text-start">開發評等原因名稱</th>
                  <th class="text-start">狀態</th>
                  <th class="text-start w-10"></th>
                </tr>
              </thead>
              <tbody>
                <template v-if="sysCttRatingReasonListViewObj.managerContacterRatingReasonList.length === 0">
                  <tr class="text-center">
                    <td colspan="3">無資料</td>
                  </tr>
                </template>
                <template v-else>
                  <tr
                    v-for="item in sysCttRatingReasonListViewObj.managerContacterRatingReasonList"
                    :key="item.managerContacterRatingReasonID"
                    class="cursor-pointer hover:bg-gray-50 transition-colors"
                    @click="
                      employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                        clickOpenUpdateRatingReasonModalBtn(item.managerContacterRatingReasonID)
                    "
                  >
                    <td class="text-start">{{ item.managerContacterRatingReasonName }}</td>
                    <td
                      class="text-start"
                      :class="{
                        'enabled-text': item.managerContacterRatingReasonStatus,
                        'disabled-text': !item.managerContacterRatingReasonStatus,
                      }"
                    >
                      {{ item.managerContacterRatingReasonStatus ? "啟用" : "停用" }}
                    </td>
                    <td class="text-start">
                      <details
                        v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                        class="relative"
                        @click.stop
                      >
                        <summary class="cursor-pointer list-none text-gray-500 hover:text-gray-700">
                          ...
                        </summary>
                        <div
                          class="absolute right-0 mt-2 w-24 rounded-md border border-gray-200 bg-white shadow z-10"
                        >
                          <button
                            class="w-full px-3 py-2 text-left text-sm hover:bg-gray-50"
                            @click.stop="
                              clickOpenUpdateRatingReasonModalBtn(item.managerContacterRatingReasonID)
                            "
                          >
                            編輯
                          </button>
                        </div>
                      </details>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>
          <div v-if="activeTab === ContacterTabEnum.RatingReason" class="flex justify-center pt-3">
            <!--開發評等原因分頁-->
            <Pagination :pageIndex="sysCttRatingReasonListViewObj.query.pageIndex"
              :pageSize="sysCttRatingReasonListViewObj.query.pageSize"
              :totalCount="sysCttRatingReasonListViewObj.totalCount" @update:current-page="
                (newPage: number) => {
                  sysCttRatingReasonListViewObj.query.pageIndex = newPage;
                  getCustomerRatingReasonList();
                }
              " @update:page-size="
                (newSize: number) => {
                  sysCttRatingReasonListViewObj.query.pageSize = newSize;
                  sysCttRatingReasonListViewObj.query.pageIndex = 1;
                  getCustomerRatingReasonList();
                }
              " />
          </div>
          <button
            v-if="
              activeTab === ContacterTabEnum.RatingReason &&
              employeeInfoStore.hasEveryonePermission(menu, 'create')
            "
            class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]" style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
            @click="clickOpenAddRatingReasonModalBtn"
          >
            +新增開發評等原因
          </button>
        </div>
      </div>

      <!-- 錯誤訊息彈跳視窗 -->
      <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

      <!-- 成功訊息提示 -->
      <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

      <!--新增評等原因Modal-->
      <AddSystemContacterRatingReasonModal v-if="sysCstRatingReasonAddModalShow"
        @close="sysCstRatingReasonAddModalShow = false" @submit="handleAddRatingReasonSubmit" />

      <!-- 編輯評等原因Modal-->
      <UpdateSystemContacterRatingReasonModal v-if="sysCstRatingReasonUpdateModalShow"
        :managerContacterRatingReasonID="selectedRatingReasonID!" :initialName="selectedRatingReasonName"
        :initialStatus="selectedRatingReasonStatus" @close="sysCstRatingReasonUpdateModalShow = false"
        @submit="handleUpdateRatingReasonSubmit" />
    </div>
  </div>
</template>
<style lang="css" scoped>
td {
  word-break: break-all;
}
</style>
