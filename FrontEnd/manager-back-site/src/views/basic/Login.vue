<script setup lang="ts">
//#region 引入
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useManagerRegionListStore } from "@/stores/managerRegionList";
import { useManagerDepartmentListStore } from "@/stores/managerDepartmentList";
import {
  login,
  getEmployee,
  getAllBasicManagerRegion,
  getAllBasicManagerDepartment,
} from "@/services/pms-http/basic/basicHttpService";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import type { MbsBscHttpLoginReqMdl } from "@/services/pms-http/basic/basicHttpFormat";

//#endregion

//#region 外部依賴
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 管理者地區儲存 */
const managerRegionListStore = useManagerRegionListStore();
/** 管理者部門儲存 */
const managerDepartmentListStore = useManagerDepartmentListStore();
/** 錯誤訊息相關 */
const { handleErrorCode, errorMessage, showError } = useErrorCodeHandler();
/** 路由 */
const router = useRouter();

//#endregion

//#region 型別定義
/** 管理者後台-基本-登入-頁面模型 */
interface MbsBasicLoginViewMdl {
  account: string | null;
  password: string | null;
}
//#endregion

//#region 頁面物件
/** 是否顯示密碼 */
const showPassword = ref(false);

/** 管理者後台-基本-登入-頁面物件 */
const mbsBasicLoginViewObj = reactive<MbsBasicLoginViewMdl>({
  account: null,
  password: null,
});
//#endregion

//#region 資料流程
/** 取得員工資訊 */
const getEmployeeInfo = async () => {
  // 檢查 token 是否存在
  if (!tokenStore.token) return;

  // 呼叫 api
  const responseData = await getEmployee({
    employeeLoginToken: tokenStore.token,
  });
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "取得員工資訊失敗";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 儲存員工資訊至 store
  employeeInfoStore.setEmployeeInfo(responseData);
};

/** 取得全部管理者地區 */
const getAllManagerRegionList = async () => {
  // 檢查 token 是否存在
  if (!tokenStore.token) return;

  // 呼叫 api
  const responseData = await getAllBasicManagerRegion({
    employeeLoginToken: tokenStore.token,
  });
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "取得管理者地區失敗";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 儲存管理者地區資訊至 store
  managerRegionListStore.setManagerRegionList(responseData.managerRegionList);
};

/** 取得全部管理者部門 */
const getAllManagerDepartmentList = async () => {
  // 檢查 token 是否存在
  if (!tokenStore.token) return;

  // 呼叫 api
  const responseData = await getAllBasicManagerDepartment({
    employeeLoginToken: tokenStore.token,
  });
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "取得管理者部門失敗";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 儲存管理者部門資訊至 store
  managerDepartmentListStore.setManagerDepartmentList(responseData.managerDepartmentList);
};
//#endregion

//#region 按鈕事件
/** 點擊【登入】按鈕 */
const clickLoginBtn = async () => {
  // 檢查帳號密碼是否為空
  if (!mbsBasicLoginViewObj.account || !mbsBasicLoginViewObj.password) {
    showError.value = true;
    errorMessage.value = "帳號或密碼不能為空";
    return;
  }

  //呼叫 api
  let requestData: MbsBscHttpLoginReqMdl = {
    employeeAccount: mbsBasicLoginViewObj.account,
    employeePassword: mbsBasicLoginViewObj.password,
  };
  let responseData = await login(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "登入失敗，請稍後再試";
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 儲存 Token
  if (responseData.loginToken) {
    tokenStore.setToken(responseData.loginToken);
  } else {
    showError.value = true;
    errorMessage.value = "登入失敗，無法獲取 Token";
  }

  // 取得員工資訊
  await getEmployeeInfo();
  // 取得所有管理者地區
  await getAllManagerRegionList();
  // 取得所有管理者部門
  await getAllManagerDepartmentList();

  // 導向首頁
  router.push("/home");
};

//#endregion

//#region 版本資訊
const appVersion = ref<string>("");
const buildTime = ref<string>("");
const baseUrl = import.meta.env.BASE_URL;
const fetchVersionInfo = async () => {
  try {
    const res = await fetch(`${baseUrl}version.json`, {
      cache: "no-store",
    });
    const data = await res.json();

    appVersion.value =
      typeof data.version === "string" ? (data.version.split("/").pop() ?? "") : "";
    buildTime.value = new Date(data.buildTime).toLocaleString();
  } catch {
    // 登入頁不要因為版本失敗而報錯
    appVersion.value = "";
    buildTime.value = "";
  }
};

fetchVersionInfo();
//#endregion
</script>

<template>
  <!-- 外層 -->
  <div class="flex h-screen items-center justify-center bg-gray-50">
    <!-- 容器 -->
    <div class="flex w-[800px] bg-white shadow-lg rounded-lg overflow-hidden">
      <!-- 左側 Logo 區 -->
      <div class="login-side--logo flex flex-col justify-center items-center w-1/2 p-8">
        <img class="w-60 mb-4" src="/images/bccs_logo.svg" alt="bccs_logo" />
        <p class="text-gray-700 text-sm text-center leading-relaxed">
          Business Continuity Computing System Inc.
        </p>
      </div>

      <!-- 右側 表單區 -->
      <div class="flex flex-col justify-center w-1/2 p-8">
        <form class="flex flex-col gap-5" @submit.prevent="clickLoginBtn">
          <!-- 帳號 -->
          <div class="flex flex-col text-sm">
            <label for="account" class="mb-1 text-gray-600">帳號</label>
            <input
              id="account"
              v-model="mbsBasicLoginViewObj.account"
              placeholder="請輸入帳號"
              class="input-box"
            />
          </div>

          <!-- 密碼 -->
          <div class="flex flex-col text-sm relative">
            <label for="password" class="mb-1 text-gray-600">密碼</label>

            <input
              id="password"
              v-model="mbsBasicLoginViewObj.password"
              :type="showPassword ? 'text' : 'password'"
              placeholder="請輸入密碼"
              class="input-box pr-10"
            />

            <!-- 眼睛圖示 -->
            <button
              type="button"
              class="absolute right-2 top-[30px] text-gray-500 hover:text-gray-700"
              @click="showPassword = !showPassword"
            >
              <svg
                v-if="!showPassword"
                class="w-5 h-5"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
                stroke="currentColor"
              >
                <path
                  d="M3.275 15.296C2.425 14.192 2 13.639 2 12C2 10.36 2.425 9.809 3.275 8.704C4.972 6.5 7.818 4 12 4C16.182 4 19.028 6.5 20.725 8.704C21.575 9.81 22 10.361 22 12C22 13.64 21.575 14.191 20.725 15.296C19.028 17.5 16.182 20 12 20C7.818 20 4.972 17.5 3.275 15.296Z"
                  stroke-width="1.5"
                />
                <path
                  d="M15 12C15 12.7956 14.6839 13.5587 14.1213 14.1213C13.5587 14.6839 12.7956 15 12 15C11.2044 15 10.4413 14.6839 9.87868 14.1213C9.31607 13.5587 9 12.7956 9 12C9 11.2044 9.31607 10.4413 9.87868 9.87868C10.4413 9.31607 11.2044 9 12 9C12.7956 9 13.5587 9.31607 14.1213 9.87868C14.6839 10.4413 15 11.2044 15 12Z"
                  stroke-width="1.5"
                />
              </svg>

              <svg
                v-else
                class="w-5 h-5"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M9.75 12C9.75 11.4033 9.98705 10.831 10.409 10.409C10.831 9.98705 11.4033 9.75 12 9.75C12.5967 9.75 13.169 9.98705 13.591 10.409C14.0129 10.831 14.25 11.4033 14.25 12C14.25 12.5967 14.0129 13.169 13.591 13.591C13.169 14.0129 12.5967 14.25 12 14.25C11.4033 14.25 10.831 14.0129 10.409 13.591C9.98705 13.169 9.75 12.5967 9.75 12Z"
                  fill="#6A7282"
                />
                <path
                  fill-rule="evenodd"
                  clip-rule="evenodd"
                  d="M2 12C2 13.64 2.425 14.191 3.275 15.296C4.972 17.5 7.818 20 12 20C16.182 20 19.028 17.5 20.725 15.296C21.575 14.192 22 13.639 22 12C22 10.36 21.575 9.809 20.725 8.704C19.028 6.5 16.182 4 12 4C7.818 4 4.972 6.5 3.275 8.704C2.425 9.81 2 10.361 2 12ZM12 8.25C11.0054 8.25 10.0516 8.64509 9.34835 9.34835C8.64509 10.0516 8.25 11.0054 8.25 12C8.25 12.9946 8.64509 13.9484 9.34835 14.6517C10.0516 15.3549 11.0054 15.75 12 15.75C12.9946 15.75 13.9484 15.3549 14.6517 14.6517C15.3549 13.9484 15.75 12.9946 15.75 12C15.75 11.0054 15.3549 10.0516 14.6517 9.34835C13.9484 8.64509 12.9946 8.25 12 8.25Z"
                  fill="#6A7282"
                />
              </svg>
            </button>
          </div>

          <p class="h-5 text-sm text-center text-red-600">
            <span v-if="showError">{{ errorMessage }}</span>
          </p>

          <button type="submit" class="login-btn mt-2">登入</button>
        </form>
      </div>
    </div>

    <!-- 版本資訊 -->
    <div v-if="appVersion" class="absolute bottom-2 right-4 text-xs text-gray-400 text-right">
      <div>{{ appVersion }}</div>
    </div>
  </div>
</template>

<style scoped>
/* --- 左側 logo 邊界線 --- */
.login-side--logo {
  border-right: 1px solid #e5e7eb;
}

/* --- 登入按鈕 --- */
.login-btn {
  padding: 12px;
  background-color: #e5e7eb;
  border: none;
  border-radius: 4px;
  font-size: 15px;
  color: #111827;
  cursor: pointer;
  transition:
    transform 0.2s ease,
    background-color 0.2s ease;
}

.login-btn:hover {
  transform: translateY(-2px);
  background-color: #d1d5db;
}
</style>
