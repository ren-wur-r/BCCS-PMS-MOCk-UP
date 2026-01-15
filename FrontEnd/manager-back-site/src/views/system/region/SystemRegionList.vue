<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysRgnHttpGetManyReqMdl,
  MbsSysRgnHttpGetManyRspMdl,
  MbsSysRgnHttpGetManyRspItemMdl,
  MbsSysRgnHttpGetReqMdl,
  MbsSysRgnHttpAddReqMdl,
  MbsSysRgnHttpUpdateReqMdl,
} from "@/services/pms-http/system/region/systemRegionHttpFormat";
import {
  getManySystemRegion,
  getSystemRegion,
  addSystemRegion,
  updateSystemRegion,
} from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import AddSystemRegionModal from "./components/AddSystemRegionModal.vue";
import UpdateSystemRegionModal from "./components/UpdateSystemRegionModal.vue";
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { handleErrorCode, errorMessage, showError, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
//#endregion

//#region 型別定義
/** 系統設定-地區-列表-查詢模型 */
interface SystemRegionListQueryMdl {
  /** 管理者-地區-名稱 */
  managerRegionName: string | null;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean | null;
  /** 頁數 */
  pageIndex: number;
  /** 每頁筆數 */
  pageSize: number;
}

/** 系統設定-地區-列表-項目模型 */
interface SystemRegionListItemMdl {
  /** 管理者-地區-ID */
  managerRegionID: number;
  /** 管理者-地區-名稱 */
  managerRegionName: string;
  /** 管理者-地區-是否啟用 */
  managerRegionIsEnable: boolean;
}

/** 系統設定-地區-列表-頁面模型 */
interface SystemRegionListViewMdl {
  /** 查詢條件 */
  query: SystemRegionListQueryMdl;
  /** 地區列表 */
  managerRegionList: SystemRegionListItemMdl[];
  /** 總筆數 */
  totalCount: number;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemRegion;
/** 是否顯示新增地區彈跳視窗 */
const isAddModalVisible = ref(false);
/** 是否顯示編輯地區彈跳視窗 */
const isUpdateModalVisible = ref(false);
/** 正在編輯的地區資料 */
const editingRegionData = ref<{
  managerRegionID: number;
  managerRegionName: string;
  managerRegionIsEnable: boolean;
} | null>(null);

/** 系統設定-地區-列表-頁面物件 */
const systemRegionListViewObj = reactive<SystemRegionListViewMdl>({
  query: {
    managerRegionName: null,
    managerRegionIsEnable: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerRegionList: [],
  totalCount: 0,
});

//#endregion

//#region API / 資料流程
/** 取得列表 */
const getList = async () => {
  // 驗證 Token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsSysRgnHttpGetManyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerRegionName: systemRegionListViewObj.query.managerRegionName,
    managerRegionIsEnable: systemRegionListViewObj.query.managerRegionIsEnable,
    pageIndex: systemRegionListViewObj.query.pageIndex,
    pageSize: systemRegionListViewObj.query.pageSize,
  };
  const responseData: MbsSysRgnHttpGetManyRspMdl | null = await getManySystemRegion(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 包裝資料
  systemRegionListViewObj.managerRegionList =
    responseData.managerRegionList?.map(
      (item: MbsSysRgnHttpGetManyRspItemMdl) =>
        ({
          managerRegionID: item.managerRegionID,
          managerRegionName: item.managerRegionName,
          managerRegionIsEnable: item.managerRegionIsEnable,
        }) satisfies SystemRegionListItemMdl
    ) ?? [];
  systemRegionListViewObj.totalCount = responseData.totalCount;
};

/** 取得單筆地區資料 */
const getRegionData = async (id: number) => {
  // 驗證 Token
  if (!requireToken()) return null;

  // 呼叫 API
  const requestData: MbsSysRgnHttpGetReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerRegionID: id,
  };
  const responseData = await getSystemRegion(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return null;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 返回資料
  return {
    managerRegionID: id,
    managerRegionName: responseData.managerRegionName,
    managerRegionIsEnable: responseData.managerRegionIsEnable,
  };
};

//#endregion

//#region 按鈕事件
/** 點擊【查詢】按鈕 */
const clickSearchBtn = () => {
  // 重置頁數
  systemRegionListViewObj.query.pageIndex = 1;

  // 取得列表
  getList();
};

/** 點擊【清除】按鈕 */
const clickClearSearchBtn = () => {
  systemRegionListViewObj.query.managerRegionName = null;
  systemRegionListViewObj.query.managerRegionIsEnable = null;
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  isAddModalVisible.value = true;
};

/** 點擊【編輯】按鈕 */
const clickUpdateBtn = async (id: number) => {
  // 取得地區資料
  const data = await getRegionData(id);

  // 若無資料則不開啟編輯視窗
  if (!data) {
    displayError("無法取得地區資料，請稍後再試 !");
    return;
  }

  // 設定編輯資料
  editingRegionData.value = data;

  // 開啟編輯視窗
  isUpdateModalVisible.value = true;
};
//#endregion

//#region 彈跳視窗處理
/** 新增地區彈跳視窗:確認 */
const handleAddModalConfirm = async (data: {
  managerRegionName: string;
  managerRegionIsEnable: boolean;
}) => {
  // 驗證權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "create")) {
    displayError("您沒有權限執行此操作 !");
    return;
  }
  // 驗證 Token
  if (!requireToken()) return;

  // 呼叫 API
  const requestData: MbsSysRgnHttpAddReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerRegionName: data.managerRegionName,
    managerRegionIsEnable: data.managerRegionIsEnable,
  };
  const responseData = await addSystemRegion(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 新增成功，關閉 Modal
  isAddModalVisible.value = false;

  // 顯示成功訊息
  displaySuccess("新增地區成功！");

  // 重新載入列表
  await getList();
};

/** 新增地區彈跳視窗:取消 */
const handleAddModalCancel = () => {
  isAddModalVisible.value = false;
};

/** 編輯地區彈跳視窗:確認 */
const handleUpdateModalConfirm = async (data: {
  managerRegionName: string;
  managerRegionIsEnable: boolean;
}) => {
  // 確認有編輯資料
  if (!editingRegionData.value) return;

  // 驗證權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "edit")) {
    displayError("您沒有權限執行此操作 !");
    return;
  }

  // 驗證 Token
  if (!requireToken()) return;

  // 取得變更的欄位
  const changes: Partial<MbsSysRgnHttpUpdateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerRegionID: editingRegionData.value.managerRegionID,
  };
  // 名稱
  if (data.managerRegionName !== editingRegionData.value.managerRegionName) {
    changes.managerRegionName = data.managerRegionName;
  }
  // 狀態
  if (data.managerRegionIsEnable !== editingRegionData.value.managerRegionIsEnable) {
    changes.managerRegionIsEnable = data.managerRegionIsEnable;
  }

  // 檢查是否有變更（排除必要的 token 和 ID）
  const hasChanges = Object.keys(changes).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 呼叫 API
  const responseData = await updateSystemRegion(changes as MbsSysRgnHttpUpdateReqMdl);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉 Modal
  isUpdateModalVisible.value = false;

  // 清除編輯資料
  editingRegionData.value = null;

  // 顯示成功訊息
  displaySuccess("更新地區成功！");
  
  // 重新載入列表
  await getList();
};

/** 編輯地區彈跳視窗:取消 */
const handleUpdateModalCancel = () => {
  isUpdateModalVisible.value = false;
  editingRegionData.value = null;
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
<input
              v-model="systemRegionListViewObj.query.managerRegionName"
              type="text"
              class="input-box"
              placeholder="地區名稱"
            />
          </div>

          <div>
<select
              v-model="systemRegionListViewObj.query.managerRegionIsEnable"
              class="select-box"
            >
              <option :value="null">全部</option>
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
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
              <th class="text-start">地區名稱</th>
              <th class="text-start">狀態</th>
              <th class="text-start w-10"></th>
            </tr>
          </thead>
          <tbody>
            <template v-if="systemRegionListViewObj.managerRegionList.length === 0">
              <tr class="text-center">
                <td colspan="3">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="item in systemRegionListViewObj.managerRegionList"
                :key="item.managerRegionID"
                class="cursor-pointer hover:bg-gray-50 transition-colors"
                @click="
                  employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                    clickUpdateBtn(item.managerRegionID)
                "
              >
                <td>
                  {{ item.managerRegionName }}
                </td>
                <td
                  class="text-start"
                  :class="{
                    'enabled-text': item.managerRegionIsEnable,
                    'disabled-text': !item.managerRegionIsEnable,
                  }"
                >
                  {{ item.managerRegionIsEnable ? "啟用" : "停用" }}
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
                        @click.stop="clickUpdateBtn(item.managerRegionID)"
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
      <button
        v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
        @click="clickAddBtn"
      >
        +新增地區
      </button>

      <!-- 分頁 -->
      <div class="flex justify-center pt-3">
        <Pagination
          :pageIndex="systemRegionListViewObj.query.pageIndex"
          :pageSize="systemRegionListViewObj.query.pageSize"
          :totalCount="systemRegionListViewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              systemRegionListViewObj.query.pageIndex = newPage;
              getList();
            }
          "
          @update:page-size="
            (newSize: number) => {
              systemRegionListViewObj.query.pageSize = newSize;
              systemRegionListViewObj.query.pageIndex = 1;
              getList();
            }
          "
        />
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

  <!-- 新增地區彈跳視窗 -->
  <AddSystemRegionModal
    :show="isAddModalVisible"
    @cancel="handleAddModalCancel"
    @confirm="handleAddModalConfirm"
  />

  <!-- 編輯地區彈跳視窗 -->
  <UpdateSystemRegionModal
    :show="isUpdateModalVisible"
    :regionData="editingRegionData"
    @cancel="handleUpdateModalCancel"
    @confirm="handleUpdateModalConfirm"
  />
</template>
