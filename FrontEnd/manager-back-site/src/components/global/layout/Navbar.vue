<script setup lang="ts">
//#region 引入
import { computed, ref } from "vue";
import type {
  MbsBscHttpLogoutReqMdl,
  MbsBscHttpGetEmployeeReqMdl,
  MbsBscHttpGetEmployeeRspMdl,
  MbsBscHttpUpdatePasswordReqMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import {
  logout,
  getEmployee,
  updateEmployeePassword,
} from "@/services/pms-http/basic/basicHttpService";
import { useTokenStore } from "@/stores/token";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useRouter } from "vue-router";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import EmployeeInfoModal from "@/components/global/modal/EmployeeInfoModal.vue";
import ChangePasswordModal from "@/components/global/modal/ChangePasswordModal.vue";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";

//#endregion

//#region 外部依賴
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
/** token 驗證相關 */
const { requireToken } = useAuth();
/** 錯誤代碼處理 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息處理 */
const { successMessage, showSuccess, displaySuccess, closeSuccess } = useSuccessHandler();

//#endregion

//#region 頁面物件
/** 是否顯示個人資訊彈跳視窗 */
const isEmployeeInfoModalVisible = ref(false);
/** 是否顯示修改密碼彈跳視窗 */
const isChangePasswordModalVisible = ref(false);
/** 員工資訊 */
const employeeData = ref<MbsBscHttpGetEmployeeRspMdl | null>(null);
/** 使用者下拉選單是否展開 */
const isUserDropdownOpen = ref(false);
const roleOptions = [
  { label: "使用登入角色", value: "" },
  { label: "Admin", value: "Admin" },
  { label: "總經理", value: "總經理" },
  { label: "各處處長", value: "各處處長" },
  { label: "各部門經理", value: "各部門經理" },
  { label: "工程部中部", value: "工程部中部" },
  { label: "顧問中部", value: "顧問中部" },
  { label: "活動人員", value: "活動人員" },
  { label: "電銷人員", value: "電銷人員" },
  { label: "業務", value: "業務" },
  { label: "顧問", value: "顧問" },
  { label: "產品經理", value: "產品經理" },
  { label: "工程師", value: "工程師" },
  { label: "資深顧問", value: "資深顧問" },
];

const selectedRole = computed({
  get: () => employeeInfoStore.devRoleName || "",
  set: (value: string) => {
    if (!value) {
      employeeInfoStore.clearDevRoleName();
      return;
    }
    employeeInfoStore.setDevRoleName(value);
  },
});

//#endregion

//#region Props / Emits
defineEmits(["toggle-sidebar"]);

//#endregion

//#region UI行為 / 畫面處理
/** 切換使用者下拉選單的顯示狀態 */
const toggleUserDropdown = () => {
  isUserDropdownOpen.value = !isUserDropdownOpen.value;
};

/** 關閉使用者下拉選單 */
const closeUserDropdown = () => {
  isUserDropdownOpen.value = false;
};
//#endregion

//#region 按鈕事件
/** 點擊【個人資訊】按鈕 */
const clickProfileBtn = async () => {
  // 關閉下拉選單
  isUserDropdownOpen.value = false;

  // 檢查token
  if (!requireToken()) return;

  // 顯示彈跳視窗
  isEmployeeInfoModalVisible.value = true;

  // 呼叫API取得員工資訊
  const requestData: MbsBscHttpGetEmployeeReqMdl = {
    employeeLoginToken: tokenStore.token!,
  };
  const response = await getEmployee(requestData);
  if (!response) {
    displayError("取得員工資訊失敗，請稍後再試。");
    return;
  }

  // 檢查錯誤代碼
  if (!handleErrorCode(response.errorCode)) {
    isEmployeeInfoModalVisible.value = false;
    return;
  }

  // 設定員工資訊
  employeeData.value = response;
};

/** 點擊【修改密碼】按鈕 */
const clickChangePasswordBtn = () => {
  // 關閉下拉選單
  isUserDropdownOpen.value = false;
  // 顯示修改密碼彈跳視窗
  isChangePasswordModalVisible.value = true;
};

/** 點擊【登出】按鈕 */
const clickLogoutBtn = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }

  // 呼叫api
  let requestData: MbsBscHttpLogoutReqMdl = {
    employeeLoginToken: tokenStore.token!,
  };
  await logout(requestData);

  //清除資料
  tokenStore.removeToken();
  employeeInfoStore.removeEmployeeInfo();

  //導向登入頁
  router.push("/login");
};

//#endregion

//#region 彈跳視窗事件
/** 關閉個人資訊彈跳視窗 */
const closeEmployeeInfoModal = () => {
  isEmployeeInfoModalVisible.value = false;
  employeeData.value = null;
};

/** 關閉修改密碼彈跳視窗 */
const closeChangePasswordModal = () => {
  isChangePasswordModalVisible.value = false;
};

/** 確認修改密碼 */
const handleChangePasswordConfirm = async (data: { oldPassword: string; newPassword: string }) => {
  // 檢查token
  if (!requireToken()) return;

  // 呼叫API修改密碼
  const requestData: MbsBscHttpUpdatePasswordReqMdl = {
    employeeLoginToken: tokenStore.token!,
    oldPassword: data.oldPassword,
    newPassword: data.newPassword,
  };
  const responseData = await updateEmployeePassword(requestData);
  if (!responseData) {
    displayError("修改密碼失敗，請稍後再試。");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉彈跳視窗
  isChangePasswordModalVisible.value = false;

  // 顯示成功訊息
  displaySuccess("密碼修改成功！");
};
//#endregion
</script>

<template>
  <header class="navbar">
    <div class="navbar-left">
      <div>
        <svg
          width="99"
          height="24"
          viewBox="0 0 99 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M41.2295 13.5866C39.5666 9.99798 40.1238 5.3824 43.1257 3.97754C44.3889 3.35155 45.97 3.42944 47.7087 3.51021L45.9584 0.772597C45.9584 0.772597 38.2947 -0.398606 33.1545 2.65056C28.0201 5.77761 25.8058 15.6982 31.1795 20.3888C36.556 25.0044 44.8527 23.2014 44.8527 23.2014L49.2082 17.5012C49.2082 17.5012 42.8894 17.2646 41.2295 13.5866Z"
            fill="#D71718"
          />
          <path
            d="M97.4862 1.90054C96.8745 1.56209 96.2215 1.30229 95.5433 1.12743C94.1475 0.844564 92.7328 0.662346 91.3103 0.582211C89.2682 0.507208 87.3136 0.426436 84.5714 2.13131C81.9079 3.60253 80.8081 7.78828 80.9656 10.2663C81.1261 12.825 81.4382 17.6281 79.8658 19.4109C78.3839 21.2687 75.875 20.2763 75.875 20.2763L76.4234 22.5841C76.4234 22.5841 81.5929 24.5226 86.6923 22.8985C91.7916 21.2744 92.801 18.0926 92.7193 12.75C92.7193 7.39596 93.1132 5.69685 94.2101 4.61219C95.307 3.52753 98.0521 4.14486 98.0521 4.14486L97.4862 1.90054Z"
            fill="#D71718"
          />
          <path
            d="M65.8735 13.5866C64.2107 9.99799 64.765 5.38241 67.7668 3.97755C69.0329 3.34867 70.6841 3.42945 72.4345 3.51022L70.6841 0.772605C70.6841 0.772605 62.9504 -0.401482 57.8131 2.65057C52.6787 5.77474 50.4645 15.6982 55.9198 20.3888C61.2963 25.0044 69.593 23.2014 69.593 23.2014L73.8581 17.5012C73.8581 17.5012 67.6151 17.2646 65.8735 13.5866Z"
            fill="#D71718"
          />
          <path
            d="M20.1421 10.9355C20.1421 10.9355 24.3692 9.57104 24.4538 5.78916C24.4538 1.99284 19.331 0.550476 18.1875 0.550476H1.10975L0.0507812 1.44186V22.5783L1.02807 23.5505H16.2417C20.2267 23.5505 25.2852 21.5312 25.5974 16.9589C25.8395 12.4558 20.1421 10.9355 20.1421 10.9355ZM10.6259 2.88134C11.2382 2.89194 11.833 3.08498 12.3325 3.43521C12.8419 3.80501 13.2552 4.28923 13.5382 4.84796C13.8212 5.40669 13.9659 6.02393 13.9603 6.64881C13.9779 7.27807 13.8386 7.90194 13.5547 8.46539C13.2708 9.02883 12.8511 9.51455 12.3325 9.87971C11.8224 10.2232 11.2378 10.4436 10.6259 10.523V2.88134ZM12.3325 20.5763C11.824 20.8055 11.2691 20.916 10.7105 20.8994H10.6259V13.0183H10.7105C11.2607 13.0112 11.8084 13.0921 12.3325 13.2577C13.0744 13.5611 13.7095 14.074 14.1583 14.7322C14.6071 15.3905 14.8498 16.165 14.8559 16.9589C14.8462 17.7415 14.601 18.5035 14.1515 19.1477C13.7021 19.792 13.0689 20.2893 12.3325 20.5763Z"
            fill="#D71718"
          />
        </svg>
      </div>
    </div>

    <div class="navbar-right">
      <div class="role-switcher">
        <span class="role-label">視角切換</span>
        <select v-model="selectedRole" class="role-select">
          <option v-for="option in roleOptions" :key="option.value" :value="option.value">
            {{ option.label }}
          </option>
        </select>
      </div>

      <div v-click-outside="closeUserDropdown" class="dropdown" @click.stop="toggleUserDropdown">
        <button type="button" class="dropdown-toggle-btn">
          <span class="dropdown-label">{{ employeeInfoStore.employeeName }}</span>

          <!--向下箭頭 -->
          <svg
            v-if="!isUserDropdownOpen"
            class="dropdown-arrow"
            width="20"
            height="25"
            viewBox="0 0 20 25"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M9.69149 8.50456L4.33316 15.4108C3.99899 15.8431 4.20149 16.666 4.64149 16.666H15.3582C15.7982 16.666 16.0007 15.8431 15.6665 15.4108L10.3082 8.50456C10.1307 8.27539 9.86899 8.27539 9.69149 8.50456Z"
              fill="#787676"
            />
          </svg>
          <!-- 向上箭頭 -->
          <svg
            v-else
            class="dropdown-arrow"
            width="20"
            height="25"
            viewBox="0 0 20 25"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M10.3085 16.4954L15.6668 9.58917C16.001 9.15687 15.7985 8.33398 15.3585 8.33398H4.64182C4.20182 8.33398 3.99932 9.15687 4.33349 9.58917L9.69182 16.4954C9.86932 16.7246 10.131 16.7246 10.3085 16.4954Z"
              fill="#787676"
            />
          </svg>
        </button>

        <div v-if="isUserDropdownOpen" class="dropdown-menu">
          <button class="dropdown-item" @click="clickProfileBtn">個人資訊</button>
          <button class="dropdown-item" @click="clickChangePasswordBtn">修改密碼</button>
          <hr />
          <button class="dropdown-item text-red-600" @click="clickLogoutBtn">登出</button>
        </div>
      </div>
    </div>
  </header>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息彈跳視窗 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />

  <!-- 個人資訊彈跳視窗 -->
  <EmployeeInfoModal
    :is-visible="isEmployeeInfoModalVisible"
    :employee-data="employeeData"
    @close="closeEmployeeInfoModal"
  />

  <!-- 修改密碼彈跳視窗 -->
  <ChangePasswordModal
    :is-visible="isChangePasswordModalVisible"
    @close="closeChangePasswordModal"
    @confirm="handleChangePasswordConfirm"
  />
</template>

<style scoped>
/* 導覽列 */
.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 60px;
  background: white;
  color: #3c3c3c;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px;
  z-index: 50;
}

.navbar-left {
  display: flex;
  align-items: center;
  font-size: 18px;
  font-weight: bold;
}

.navbar-right {
  display: flex;
  align-items: center;
  gap: 15px;
}

.role-switcher {
  display: flex;
  align-items: center;
  gap: 6px;
}

.role-label {
  font-size: 12px;
  color: #6b7280;
  font-weight: 600;
}

.role-select {
  border: 1px solid #d1d5db;
  border-radius: 6px;
  padding: 4px 8px;
  font-size: 12px;
  background: #ffffff;
  cursor: pointer;
}

.role-select:focus {
  outline: none;
  border-color: #22d3ee;
  box-shadow: 0 0 0 1px rgba(34, 211, 238, 0.5);
}

.username {
  font-size: var(--font-size-md);
}

.menu-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  margin-right: 10px;
}

.logout-btn {
  background: none;
  border: none;
  font-weight: bold;
  color: #3c3c3c;
  cursor: pointer;
  transition: background 0.3s;
}

.logout-btn:hover {
  border-bottom: 2px solid #3c3c3c;
}

/* 右邊選單 */
.dropdown {
  position: relative;
  cursor: pointer;
}

.dropdown-toggle-btn {
  background: none;
  border: none;
  display: flex;
  align-items: center;
  gap: 3px;
  font-weight: bold;
  color: #3c3c3c;
}

.dropdown-label:hover {
  border-bottom: 2px solid #3c3c3c;
}

.dropdown-arrow {
  transform: rotate(180deg);
  transition: transform 0.2s ease-in-out;
}

.dropdown-arrow.rotate {
  transform: rotate(0deg);
}

.dropdown-menu {
  position: absolute;
  top: 100%;
  right: 0;
  background: white;
  border: 1px solid #ccc;
  min-width: 120px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  z-index: 60;
  border-radius: 10px;
  margin-top: 8px;
  padding: 8px;
}

.dropdown-item {
  padding: 10px 10px;
  width: 100%;
  text-align: left;
  border: none;
  background: none;
  cursor: pointer;
}

.dropdown-item:hover {
  background-color: #f5f5f5;
}
</style>
