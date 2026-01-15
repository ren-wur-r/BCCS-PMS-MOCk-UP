<script setup lang="ts">
//#region 引入
import { reactive, onMounted } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useTokenStore } from "@/stores/token";
import type {
  MbsSysRolHttpGetManyReqMdl,
  MbsSysRolHttpGetManyRspMdl,
  MbsSysRolHttpGetManyRspItemMdl,
} from "@/services/pms-http/system/role/systemRoleHttpFormat";
import { getManySystemRole } from "@/services/index";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import router from "@/router";
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
/** 系統設定-角色-列表-查詢模型 */
interface SystemRoleListQueryMdl {
  managerRoleName: string | null;
  managerRoleIsEnable: boolean | null;
  pageIndex: number;
  pageSize: number;
}

/** 系統設定-角色-列表-項目模型 */
interface SystemRoleListItemMdl {
  managerRoleID: number;
  managerRoleName: string;
  employeeCount: number;
  managerRoleIsEnable: boolean;
}

/** 系統設定-角色-列表-頁面模型 */
interface SystemRoleListViewMdl {
  query: SystemRoleListQueryMdl;
  managerRoleList: SystemRoleListItemMdl[];
  totalCount: number;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemRole;
/** 系統設定-角色-列表-頁面物件 */
const systemRoleListViewObj = reactive<SystemRoleListViewMdl>({
  query: {
    managerRoleName: null,
    managerRoleIsEnable: null,
    pageIndex: 1,
    pageSize: 10,
  },
  managerRoleList: [],
  totalCount: 0,
});
//#endregion

//#region API / 資料流程
/** 取得列表 */
const getList = async () => {
  if (!requireToken()) return;

  const requestData: MbsSysRolHttpGetManyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerRoleName: systemRoleListViewObj.query.managerRoleName,
    managerRoleIsEnable: systemRoleListViewObj.query.managerRoleIsEnable,
    pageIndex: systemRoleListViewObj.query.pageIndex,
    pageSize: systemRoleListViewObj.query.pageSize,
  };

  const responseData: MbsSysRolHttpGetManyRspMdl | null = await getManySystemRole(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試 !");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 包裝資料
  systemRoleListViewObj.managerRoleList =
    responseData.managerRoleList?.map(
      (item: MbsSysRolHttpGetManyRspItemMdl) =>
        ({
          managerRoleID: item.managerRoleID,
          managerRoleName: item.managerRoleName,
          employeeCount: item.employeeCount,
          managerRoleIsEnable: item.managerRoleIsEnable,
        }) satisfies SystemRoleListItemMdl
    ) ?? [];

  systemRoleListViewObj.totalCount = responseData.totalCount;
};
//#endregion

//#region 按鈕事件
/** 點擊【查詢】按鈕 */
const clickSearchBtn = () => {
  systemRoleListViewObj.query.pageIndex = 1;
  getList();
};

/** 點擊【清除】按鈕 */
const clickClearSearchBtn = () => {
  systemRoleListViewObj.query.managerRoleName = null;
  systemRoleListViewObj.query.managerRoleIsEnable = null;
};

/** 點擊【新增】按鈕 */
const clickAddBtn = () => {
  router.push("/system/role/add");
};

/** 點擊【檢視】按鈕 */
const clickDetailBtn = (id: number) => {
  router.push(`/system/role/detail/${id}`);
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

    <!-- 查詢條件 -->
    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-6 gap-4">
      <div class="flex items-end gap-4">
        <div class="flex gap-2">
          <div>
<input
              v-model="systemRoleListViewObj.query.managerRoleName"
              type="text"
              class="input-box"
              placeholder="角色名稱"
            />
          </div>

          <div>
<select v-model="systemRoleListViewObj.query.managerRoleIsEnable" class="select-box">
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
              <th class="text-start">角色名稱</th>
              <th class="text-start">員工數量</th>
              <th class="text-start">狀態</th>
              <th class="text-start w-10"></th>
            </tr>
          </thead>
          <tbody>
            <template v-if="systemRoleListViewObj.managerRoleList.length === 0">
              <tr class="text-center">
                <td colspan="4">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="item in systemRoleListViewObj.managerRoleList"
                :key="item.managerRoleID"
                class="cursor-pointer hover:bg-gray-50 transition-colors"
                @click="
                  employeeInfoStore.hasEveryonePermission(menu, 'view') &&
                    clickDetailBtn(item.managerRoleID)
                "
              >
                <td>
                  {{ item.managerRoleName }}
                </td>
                <td class="text-start">
                  {{ item.employeeCount }}
                </td>
                <td
                  class="text-start"
                  :class="{
                    'enabled-text': item.managerRoleIsEnable,
                    'disabled-text': !item.managerRoleIsEnable,
                  }"
                >
                  {{ item.managerRoleIsEnable ? "啟用" : "停用" }}
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
                        @click.stop="clickDetailBtn(item.managerRoleID)"
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
        v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="w-full rounded-lg border border-dashed py-2 text-sm font-medium text-[#082F49] hover:text-[#061F30]"
        style="margin-top:1cm;background-color:#F2F6F9;border-color:#082F49;"
        @click="clickAddBtn"
      >
        +新增角色
      </button>

      <div class="flex justify-center pt-3">
        <!-- 分頁 -->
        <Pagination
          :pageIndex="systemRoleListViewObj.query.pageIndex"
          :pageSize="systemRoleListViewObj.query.pageSize"
          :totalCount="systemRoleListViewObj.totalCount"
          @update:current-page="
            (newPage: number) => {
              systemRoleListViewObj.query.pageIndex = newPage;
              getList();
            }
          "
          @update:page-size="
            (newSize: number) => {
              systemRoleListViewObj.query.pageSize = newSize;
              systemRoleListViewObj.query.pageIndex = 1;
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
