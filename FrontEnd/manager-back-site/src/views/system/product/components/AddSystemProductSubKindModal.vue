<script setup lang="ts">
import { reactive, defineAsyncComponent, watch } from "vue";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import { useTokenStore } from "@/stores/token";
import { getProductMainKind } from "@/services";
const GetManyManagerProductMainKindComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerProductMainKindComboBox.vue")
);
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
const emit = defineEmits<{
  (e: "close"): void;
  (e: "confirm", payload: SystemProductSubKindAddMdl): void;
}>();
//----------------------------------------------------------------------------
/** 系統-產品-子分類-新增模型 */
interface SystemProductSubKindAddMdl {
  managerProductMainKindID: number;
  managerProductSubKindName: string;
  managerProductSubKindCommissionRate: number;
  managerProductSubKindIsEnable: boolean;
}

/** 系統-產品-子分類-新增-驗證模型*/
interface SystemProductSubKindAddValidateMdl {
  managerProductMainKindID: boolean;
  managerProductSubKindName: boolean;
  managerProductSubKindCommissionRate: boolean;
  managerProductSubKindIsEnable: boolean;
}

/** 系統-產品-子分類-新增物件 */
const systemProductSubKindAddObj = reactive<SystemProductSubKindAddMdl>({
  managerProductMainKindID: 0,
  managerProductSubKindName: "",
  managerProductSubKindCommissionRate: 0,
  managerProductSubKindIsEnable: true,
});
/** 系統-產品-子分類-新增-驗證物件*/
const systemProductSubKindAddValidObj = reactive<SystemProductSubKindAddValidateMdl>({
  managerProductMainKindID: true,
  managerProductSubKindName: true,
  managerProductSubKindCommissionRate: true,
  managerProductSubKindIsEnable: true,
});
//----------------------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 主分類驗證
  if (!systemProductSubKindAddObj.managerProductMainKindID) {
    systemProductSubKindAddValidObj.managerProductMainKindID = false;
    isValid = false;
  } else {
    systemProductSubKindAddValidObj.managerProductMainKindID = true;
  }

  // 子分類名稱驗證
  if (
    !systemProductSubKindAddObj.managerProductSubKindName ||
    systemProductSubKindAddObj.managerProductSubKindName.trim().length === 0 ||
    systemProductSubKindAddObj.managerProductSubKindName.trim().length > 30
  ) {
    systemProductSubKindAddValidObj.managerProductSubKindName = false;
    isValid = false;
  } else {
    systemProductSubKindAddValidObj.managerProductSubKindName = true;
  }

  // 業務毛利率驗證 (0 ~ 100 之間)
  const rate = systemProductSubKindAddObj.managerProductSubKindCommissionRate;
  if (rate === null || isNaN(rate) || rate < 0 || rate > 100) {
    systemProductSubKindAddValidObj.managerProductSubKindCommissionRate = false;
    isValid = false;
  } else {
    systemProductSubKindAddValidObj.managerProductSubKindCommissionRate = true;
  }

  // 狀態驗證
  if (typeof systemProductSubKindAddObj.managerProductSubKindIsEnable !== "boolean") {
    systemProductSubKindAddValidObj.managerProductSubKindIsEnable = false;
    isValid = false;
  } else {
    systemProductSubKindAddValidObj.managerProductSubKindIsEnable = true;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/** 點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 欄位驗證
  if (!validateField()) {
    return;
  }

  emit("confirm", { ...systemProductSubKindAddObj });
  emit("close");
};

/** 監聽主分類ID變化，自動載入該主分類的業務毛利率 */
watch(
  () => systemProductSubKindAddObj.managerProductMainKindID,
  async (newMainKindID, oldMainKindID) => {
    if (newMainKindID && newMainKindID !== oldMainKindID) {
      // 驗證Token
      if (!requireToken()) return;

      // 取得主分類資料
      const responseData = await getProductMainKind({
        employeeLoginToken: tokenStore.token!,
        managerProductMainKindID: newMainKindID,
      });
      if (!responseData) {
        errorMessage.value = "取得產品主分類資料失敗，無法帶入業務毛利率！";
        showError.value = true;
        return;
      }

      // 帶入主分類的業務毛利率
      systemProductSubKindAddObj.managerProductSubKindCommissionRate =
        Math.round(responseData.managerProductMainKindCommissionRate * 100 * 100) / 100;
    }
  },
  { immediate: false }
);
</script>

<template>
  <div class="modal-overlay">
    <div class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col">
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">新增子分類</h2>
        <button class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2" aria-label="關閉"
          @click="$emit('close')">
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <!-- 後端錯誤訊息 -->
        <p v-if="showError" class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded mb-4">
          {{ errorMessage }}
        </p>

        <div class="space-y-6">
          <!-- 產品子分類名稱 -->
          <div>
            <label class="form-label">產品子分類名稱 <span class="required-label">*</span></label>
            <input v-model="systemProductSubKindAddObj.managerProductSubKindName" class="input-box" type="text"
              placeholder="請輸入產品子分類名稱" />
            <p class="error-wrapper">
              <span v-if="!systemProductSubKindAddValidObj.managerProductSubKindName" class="error-tip">
                不可為空，長度不可超過 30 個字
              </span>
            </p>
          </div>

          <!-- 產品主分類 -->
          <div>
            <label class="form-label">產品主分類 <span class="required-label">*</span></label>
            <GetManyManagerProductMainKindComboBox
              v-model:managerProductMainKindID="systemProductSubKindAddObj.managerProductMainKindID"
              :disabled="false" />
            <p class="error-wrapper">
              <span v-if="!systemProductSubKindAddValidObj.managerProductMainKindID" class="error-tip">
                不可為空
              </span>
            </p>
          </div>

          <!-- 業務毛利率 -->
          <div>
            <label class="form-label">業務毛利率 <span class="required-label">*</span></label>
            <div class="flex items-center gap-2">
              <input v-model.number="systemProductSubKindAddObj.managerProductSubKindCommissionRate" class="input-box"
                type="number" placeholder="0~100" />
              <span class="text-gray-700">%</span>
            </div>
            <p class="error-wrapper">
              <span v-if="!systemProductSubKindAddValidObj.managerProductSubKindCommissionRate" class="error-tip">
                請填寫 0 到 100 之間的數字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select v-model="systemProductSubKindAddObj.managerProductSubKindIsEnable" class="select-box">
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper">
              <span v-if="!systemProductSubKindAddValidObj.managerProductSubKindIsEnable" class="error-tip">
                不可為空
              </span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
