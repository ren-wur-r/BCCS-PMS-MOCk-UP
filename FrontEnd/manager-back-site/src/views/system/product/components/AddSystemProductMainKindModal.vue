<script setup lang="ts">
import { reactive } from "vue";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";

const { requireToken } = useAuth();
const { errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
interface Emits {
  (e: "close"): void;
  (e: "confirm", payload: SystemProductMainKindAddMdl): void;
}

// Emits
const emit = defineEmits<Emits>();
//----------------------------------------------------------------------------
/** 系統-產品-主分類-新增模型 */
interface SystemProductMainKindAddMdl {
  managerProductMainKindName: string;
  managerProductMainKindCommissionRate: number;
  managerProductMainKindIsEnable: boolean;
}

/** 系統-產品-主分類-新增驗證模型*/
interface SystemProductMainKindAddValidateMdl {
  managerProductMainKindName: boolean;
  managerProductMainKindCommissionRate: boolean;
  managerProductMainKindIsEnable: boolean;
}

/** 系統-產品-主分類-新增物件 */
const systemProductMainKindAddObj = reactive<SystemProductMainKindAddMdl>({
  managerProductMainKindName: "",
  managerProductMainKindCommissionRate: 0,
  managerProductMainKindIsEnable: true,
});

/** 系統-產品-主分類-新增-驗證物件 */
const systemProductMainKindValidObj = reactive<SystemProductMainKindAddValidateMdl>({
  managerProductMainKindName: true,
  managerProductMainKindCommissionRate: true,
  managerProductMainKindIsEnable: true,
});
//----------------------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 名稱：不可為空，長度不超過 50
  if (
    typeof systemProductMainKindAddObj.managerProductMainKindName !== "string" ||
    !systemProductMainKindAddObj.managerProductMainKindName.trim() ||
    systemProductMainKindAddObj.managerProductMainKindName.trim().length > 50
  ) {
    systemProductMainKindValidObj.managerProductMainKindName = false;
    isValid = false;
  } else {
    systemProductMainKindValidObj.managerProductMainKindName = true;
  }

  // 業務毛利率：必填且 0~100 之間
  const rate = systemProductMainKindAddObj.managerProductMainKindCommissionRate;
  if (rate === null || isNaN(rate) || rate < 0 || rate > 100) {
    systemProductMainKindValidObj.managerProductMainKindCommissionRate = false;
    isValid = false;
  } else {
    systemProductMainKindValidObj.managerProductMainKindCommissionRate = true;
  }

  // 狀態：必為 boolean
  if (typeof systemProductMainKindAddObj.managerProductMainKindIsEnable !== "boolean") {
    systemProductMainKindValidObj.managerProductMainKindIsEnable = false;
    isValid = false;
  } else {
    systemProductMainKindValidObj.managerProductMainKindIsEnable = true;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) {
    return;
  }
  // 欄位驗證
  if (!validateField()) {
    return;
  }

  emit("confirm", { ...systemProductMainKindAddObj });
  emit("close");
};
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">新增主分類</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="$emit('close')"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <!-- 後端錯誤訊息 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded mb-4"
        >
          {{ errorMessage }}
        </p>

        <div class="space-y-4">
          <!-- 名稱 -->
          <div>
            <label class="form-label">產品主分類名稱 <span class="required-label">*</span></label>
            <input
              v-model="systemProductMainKindAddObj.managerProductMainKindName"
              class="input-box"
              type="text"
              placeholder="請輸入產品主分類名稱"
            />
            <p class="error-wrapper">
              <span
                v-if="!systemProductMainKindValidObj.managerProductMainKindName"
                class="error-tip"
              >
                不可為空，長度不可超過 50 個字
              </span>
            </p>
          </div>

          <!-- 業務毛利率 -->
          <div>
            <label class="form-label">預設業務毛利率 <span class="required-label">*</span></label>
            <div class="flex items-center gap-2">
              <input
                v-model.number="systemProductMainKindAddObj.managerProductMainKindCommissionRate"
                class="input-box"
                type="number"
                placeholder="0~100"
              />
              <span class="text-gray-700">%</span>
            </div>
            <p class="error-wrapper">
              <span
                v-if="!systemProductMainKindValidObj.managerProductMainKindCommissionRate"
                class="error-tip"
              >
                請填寫 0 到 100 之間的數字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select
              v-model="systemProductMainKindAddObj.managerProductMainKindIsEnable"
              class="select-box"
            >
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper">
              <span
                v-if="!systemProductMainKindValidObj.managerProductMainKindIsEnable"
                class="error-tip"
              >
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
