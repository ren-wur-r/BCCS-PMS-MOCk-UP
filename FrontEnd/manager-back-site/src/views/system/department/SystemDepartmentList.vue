<script setup lang="ts">
//#region 引入
import { ref, reactive, onMounted } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysDptHttpGetManyReqMdl,
  MbsSysDptHttpGetManyRspMdl,
  MbsSysDptHttpGetManyRspItemMdl,
  MbsSysDptHttpGetReqMdl,
  MbsSysDptHttpAddReqMdl,
  MbsSysDptHttpUpdateReqMdl,
} from "@/services/pms-http/system/department/systemDepartmentHttpFormat";
import {
  getManySystemDepartment,
  getSystemDepartment,
  addSystemDepartment,
  updateSystemDepartment,
} from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import Pagination from "@/components/global/pagination/Pagination.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import AddSystemDepartmentModal from "./components/AddSystemDepartmentModal.vue";
import UpdateSystemDepartmentModal from "./components/UpdateSystemDepartmentModal.vue";
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
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
//#endregion

//#region 型別定義
/** 系統設定-部門-列表-查詢模型 */
interface SystemDepartmentListQueryMdl {
  managerDepartmentName: string | null;
  managerDepartmentIsEnable: boolean | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-部門-列表-項目模型 */
interface SystemDepartmentListItemMdl {
  managerDepartmentID: number;
  managerDepartmentName: string;
  managerDepartmentIsEnable: boolean;
}

/** 系統設定-部門-列表-頁面模型 */
interface SystemDepartmentListViewMdl {
  query: SystemDepartmentListQueryMdl;
  managerDepartmentList: SystemDepartmentListItemMdl[];
  totalCount: number;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemDepartment;
/** 是否顯示新增部門彈跳視窗 */
const isAddModalVisible = ref(false);
/** 是否顯示編輯部門彈跳視窗 */
const isUpdateModalVisible = ref(false);
/** 正在編輯的部門資料 */
const editingDepartmentData = ref<{
  managerDepartmentID: number;
  managerDepartmentName: string;
  managerDepartmentIsEnable: boolean;
} | null>(null);

/** 系統設定-部門-列表-頁面物件 */
const systemDepartmentListViewObj = reactive<SystemDepartmentListViewMdl>({
  query: {
    managerDepartmentName: null,
    managerDepartmentIsEnable: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerDepartmentList: [],
  totalCount: 0,
});
//#endregion

//#region API / 資料流程
/** 取得列表 */
const getList = async () => {
  if (!requireToken()) return;

  const requestData: MbsSysDptHttpGetManyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerDepartmentName: systemDepartmentListViewObj.query.managerDepartmentName,
    managerDepartmentIsEnable: systemDepartmentListViewObj.query.managerDepartmentIsEnable,
    pageIndex: systemDepartmentListViewObj.query.pageIndex,
    pageSize: systemDepartmentListViewObj.query.pageSize,
  };

  const responseData: MbsSysDptHttpGetManyRspMdl | null =
    await getManySystemDepartment(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 包裝資料
  systemDepartmentListViewObj.managerDepartmentList =
    responseData.managerDepartmentList?.map(
      (item: MbsSysDptHttpGetManyRspItemMdl) =>
        ({
          managerDepartmentID: item.managerDepartmentID,
          managerDepartmentName: item.managerDepartmentName,
          managerDepartmentIsEnable: item.managerDepartmentIsEnable,
        }) satisfies SystemDepartmentListItemMdl
    ) ?? [];

  systemDepartmentListViewObj.totalCount = responseData.totalCount;
};

/** 取得單筆部門資料 */
const getDepartmentData = async (id: number) => {
  if (!requireToken()) return null;

  const requestData: MbsSysDptHttpGetReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerDepartmentID: id,
  };

  const responseData = await getSystemDepartment(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return null;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  return {
    managerDepartmentID: id,
    managerDepartmentName: responseData.managerDepartmentName,
    managerDepartmentIsEnable: responseData.managerDepartmentIsEnable,
  };
};
//#endregion

//#region 按鈕事件
/** 點擊【查詢】按鈕 */
const clickSearchBtn = () => {
  systemDepartmentListViewObj.query.pageIndex = 1;
  getList();
};

/** 點擊【清除】按鈕 */
const clickClearSearchBtn = () => {
  systemDepartmentListViewObj.query.managerDepartmentName = null;
  systemDepartmentListViewObj.query.managerDepartmentIsEnable = null;
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  isAddModalVisible.value = true;
};

/** 點擊【編輯】按鈕 */
const clickUpdateBtn = async (id: number) => {
  const data = await getDepartmentData(id);
  if (!data) return;

  editingDepartmentData.value = data;
  isUpdateModalVisible.value = true;
};

//#endregion

//#region 彈跳視窗處理
/** 新增部門彈跳視窗:確認 */
const handleAddModalConfirm = async (data: {
  managerDepartmentName: string;
  managerDepartmentIsEnable: boolean;
}) => {
  // 驗證權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "create")) {
    displayError("您沒有權限執行此操作 !");
    return;
  }
  // 驗證 Token
  if (!requireToken()) return;
  // 送api
  const requestData: MbsSysDptHttpAddReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerDepartmentName: data.managerDepartmentName,
    managerDepartmentIsEnable: data.managerDepartmentIsEnable,
  };
  const responseData = await addSystemDepartment(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 新增成功，關閉 Modal
  isAddModalVisible.value = false;
  // 顯示成功訊息
  displaySuccess("新增部門成功！");
  // 重新載入列表
  await getList();
};

/** 新增部門彈跳視窗:取消 */
const handleAddModalCancel = () => {
  isAddModalVisible.value = false;
};

/** 編輯部門彈跳視窗:確認 */
const handleUpdateModalConfirm = async (data: {
  managerDepartmentName: string;
  managerDepartmentIsEnable: boolean;
}) => {
  if (!editingDepartmentData.value) return;

  // 驗證權限
  if (!employeeInfoStore.hasEveryonePermission(menu, "edit")) {
    displayError("您沒有權限執行此操作 !");
    return;
  }
  // 驗證 Token
  if (!requireToken()) return;

  // 取得變更的欄位
  const changes: Partial<MbsSysDptHttpUpdateReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerDepartmentID: editingDepartmentData.value.managerDepartmentID,
  };
  // 名稱
  if (data.managerDepartmentName !== editingDepartmentData.value.managerDepartmentName) {
    changes.managerDepartmentName = data.managerDepartmentName;
  }
  // 狀態
  if (data.managerDepartmentIsEnable !== editingDepartmentData.value.managerDepartmentIsEnable) {
    changes.managerDepartmentIsEnable = data.managerDepartmentIsEnable;
  }

  // 檢查是否有變更（排除必要的 token 和 ID）
  const hasChanges = Object.keys(changes).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  const responseData = await updateSystemDepartment(changes as MbsSysDptHttpUpdateReqMdl);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 編輯成功，關閉 Modal 並清除編輯資料
  isUpdateModalVisible.value = false;
  editingDepartmentData.value = null;
  // 顯示成功訊息
  displaySuccess("更新部門成功！");
  // 重新載入列表
  await getList();
};

/** 編輯部門彈跳視窗:取消 */
const handleUpdateModalCancel = () => {
  isUpdateModalVisible.value = false;
  editingDepartmentData.value = null;
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
              v-model="systemDepartmentListViewObj.query.managerDepartmentName"
              type="text"
              class="input-box"
              placeholder="部門名稱"
            />
          </div>

          <div>
<select
              v-model="systemDepartmentListViewObj.query.managerDepartmentIsEnable"
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
              <th class="text-start">部門名稱</th>
              <th class="text-start">狀態</th>
              <th class="text-start w-10"></th>
            </tr>
          </thead>
          <tbody>
            <template v-if="systemDepartmentListViewObj.managerDepartmentList.length === 0">
              <tr class="text-center">
                <td colspan="3">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="item in systemDepartmentListViewObj.managerDepartmentList"
                :key="item.managerDepartmentID"
                class="cursor-pointer hover:bg-gray-50 transition-colors"
                @click="
                  employeeInfoStore.hasEveryonePermission(menu, 'edit') &&
                    clickUpdateBtn(item.managerDepartmentID)
                "
              >
                <td>
                  {{ item.managerDepartmentName }}
                </td>
                <td
                  class="text-start"
                  :class="{
                    'enabled-text': item.managerDepartmentIsEnable,
                    'disabled-text': !item.managerDepartmentIsEnable,
                  }"
                >
                  {{ item.managerDepartmentIsEnable ? "啟用" : "停用" }}
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
                        @click.stop="clickUpdateBtn(item.managerDepartmentID)"
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
        +新增部門
      </button>

      <!-- 分頁 -->
      <div class="flex justify-center pt-3">
        <Pagination
          :pageIndex="systemDepartmentListViewObj.query.pageIndex"
          :pageSize="systemDepartmentListViewObj.query.pageSize"
          :totalCount="systemDepartmentListViewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              systemDepartmentListViewObj.query.pageIndex = newPage;
              getList();
            }
          "
          @update:page-size="
            (newSize: number) => {
              systemDepartmentListViewObj.query.pageSize = newSize;
              systemDepartmentListViewObj.query.pageIndex = 1;
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

  <!-- 新增部門彈跳視窗 -->
  <AddSystemDepartmentModal
    :show="isAddModalVisible"
    @cancel="handleAddModalCancel"
    @confirm="handleAddModalConfirm"
  />

  <!-- 編輯部門彈跳視窗 -->
  <UpdateSystemDepartmentModal
    :show="isUpdateModalVisible"
    :departmentData="editingDepartmentData"
    @cancel="handleUpdateModalCancel"
    @confirm="handleUpdateModalConfirm"
  />
</template>
