<script setup lang="ts">
import { reactive,watch } from "vue";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";

const { requireToken } = useAuth();
const { errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
interface Props {
  data:SystemProductSubKindUpdateMdl;
}

interface Emits {
  (e: "confirm",payload:SystemProductSubKindUpdateMdl): void;
  (e: "close"): void;
}

// Props
const props = defineProps<Props>();
// Emits
const emit = defineEmits<Emits>();
//----------------------------------------------------------------------------
/** 系統-產品-子分類-更新模型 */
interface SystemProductSubKindUpdateMdl {
  managerProductSubKindID: number;
  managerProductSubKindName: string;
  managerProductSubKindCommissionRate: number;
  managerProductSubKindIsEnable: boolean;
  managerProductMainKindName: string;
}

/** 系統-產品-子分類-編輯-驗證模型*/
interface SystemProductSubKindUpdateValidateMdl {
  managerProductMainKindName: boolean;
  managerProductSubKindName: boolean;
  managerProductSubKindCommissionRate: boolean;
  managerProductSubKindIsEnable: boolean;
}

//----------------------------------------------------------------------------
/** 系統-產品-子分類-更新物件 */
const systemProductSubKindUpdateObj = reactive<SystemProductSubKindUpdateMdl>({
  managerProductSubKindID: 0,
  managerProductSubKindName: "",
  managerProductSubKindCommissionRate: 0,
  managerProductSubKindIsEnable: true,
  managerProductMainKindName: "",
});

/** 系統-產品-子分類-編輯-驗證物件*/
const systemProductSubKindValidObj = reactive<SystemProductSubKindUpdateValidateMdl>({
  managerProductMainKindName: true,
  managerProductSubKindName: true,
  managerProductSubKindCommissionRate: true,
  managerProductSubKindIsEnable: true,
});


//----------------------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 主分類驗證
  const mainKind = systemProductSubKindUpdateObj.managerProductMainKindName.trim();
  if (!mainKind || mainKind.length === 0) {
    systemProductSubKindValidObj.managerProductMainKindName = false;
    isValid = false;
  } else {
    systemProductSubKindValidObj.managerProductMainKindName = true;
  }

  // 子分類名稱驗證
  const name = systemProductSubKindUpdateObj.managerProductSubKindName.trim();
  if (!name || name.length === 0 || name.length > 30) {
    systemProductSubKindValidObj.managerProductSubKindName = false;
    isValid = false;
  } else {
    systemProductSubKindValidObj.managerProductSubKindName = true;
  }

  // 業務毛利率驗證 (0 ~ 100 之間)
  const rate = systemProductSubKindUpdateObj.managerProductSubKindCommissionRate;
  if (rate === null || isNaN(rate) || rate < 0 || rate > 100) {
    systemProductSubKindValidObj.managerProductSubKindCommissionRate = false;
    isValid = false;
  } else {
    systemProductSubKindValidObj.managerProductSubKindCommissionRate = true;
  }

  // 狀態驗證
  if (typeof systemProductSubKindUpdateObj.managerProductSubKindIsEnable !== "boolean") {
    systemProductSubKindValidObj.managerProductSubKindIsEnable = false;
    isValid = false;
  } else {
    systemProductSubKindValidObj.managerProductSubKindIsEnable = true;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;
  // 欄位驗證
  if (!validateField()) return;

  emit("confirm", { ...systemProductSubKindUpdateObj });
  emit("close");
};
//----------------------------------------------------------------------------
watch (
  () => props.data,
  (newData) => {
    systemProductSubKindUpdateObj.managerProductSubKindID = newData.managerProductSubKindID;
    systemProductSubKindUpdateObj.managerProductSubKindName = newData.managerProductSubKindName;
    systemProductSubKindUpdateObj.managerProductSubKindCommissionRate =
      newData.managerProductSubKindCommissionRate;
    systemProductSubKindUpdateObj.managerProductSubKindIsEnable =
      newData.managerProductSubKindIsEnable;
    systemProductSubKindUpdateObj.managerProductMainKindName = newData.managerProductMainKindName;

  },
  { immediate: true }
);

</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯子分類</h2>
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

        <div class="space-y-6">
          <!-- 產品子分類名稱 -->
          <div>
            <label class="form-label">產品子分類名稱 <span class="required-label">*</span></label>
            <input
              v-model="systemProductSubKindUpdateObj.managerProductSubKindName"
              class="input-box"
              type="text"
              placeholder="請輸入產品子分類名稱"
            />
            <p class="h-3">
              <span
                v-if="!systemProductSubKindValidObj.managerProductSubKindName"
                class="error-tip"
              >
                不可為空，長度不可超過 30 個字
              </span>
            </p>
          </div>

          <!-- 產品主分類 -->
          <div>
            <label class="form-label">產品主分類 <span class="required-label">*</span></label>
            <input
              v-model="systemProductSubKindUpdateObj.managerProductMainKindName"
              class="input-box"
              type="text"
              disabled
            />
            <p class="h-3">
              <span
                v-if="!systemProductSubKindValidObj.managerProductMainKindName"
                class="error-tip"
              >
                不可為空
              </span>
            </p>
          </div>

          <!-- 業務毛利率 -->
          <div>
            <label class="form-label">業務毛利率 <span class="required-label">*</span></label>
            <div class="flex items-center gap-2">
              <input
                v-model.number="systemProductSubKindUpdateObj.managerProductSubKindCommissionRate"
                class="input-box"
                type="number"
                placeholder="0~100"
              />
              <span class="text-gray-700">%</span>
            </div>
            <p class="h-3">
              <span
                v-if="!systemProductSubKindValidObj.managerProductSubKindCommissionRate"
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
              v-model="systemProductSubKindUpdateObj.managerProductSubKindIsEnable"
              class="select-box"
            >
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="h-3">
              <span
                v-if="!systemProductSubKindValidObj.managerProductSubKindIsEnable"
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
