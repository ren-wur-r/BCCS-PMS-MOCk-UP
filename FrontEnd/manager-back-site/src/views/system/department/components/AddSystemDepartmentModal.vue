<script setup lang="ts">
import { reactive, watch } from "vue";

//----------------------------------------------------------------------------
interface Props {
  /** 顯示 Modal */
  show: boolean;
}

const props = defineProps<Props>();

interface Emits {
  /** 取消 */
  (e: "cancel"): void;
  /** 確認送出 */
  (
    e: "confirm",
    data: {
      managerDepartmentName: string;
      managerDepartmentIsEnable: boolean;
    }
  ): void;
}

const emit = defineEmits<Emits>();
//----------------------------------------------------------------------------
/** 系統設定-部門彈跳視窗-頁面模型 */
interface AddSystemDepartmentModalMdl {
  managerDepartmentName: string | null;
  managerDepartmentIsEnable: boolean;
  managerDepartmentNameIsValid: boolean;
  managerDepartmentNameLengthIsValid: boolean;
  managerDepartmentIsEnableIsValid: boolean;
}

/** 系統設定-部門彈跳視窗-頁面物件 */
const addSystemDepartmentModalObj = reactive<AddSystemDepartmentModalMdl>({
  managerDepartmentName: null,
  managerDepartmentIsEnable: true,
  managerDepartmentNameIsValid: true,
  managerDepartmentNameLengthIsValid: true,
  managerDepartmentIsEnableIsValid: true,
});

//--------------------------------------------------------------------------
/** 重置表單 */
const resetForm = () => {
  addSystemDepartmentModalObj.managerDepartmentName = null;
  addSystemDepartmentModalObj.managerDepartmentIsEnable = true;
  addSystemDepartmentModalObj.managerDepartmentNameIsValid = true;
  addSystemDepartmentModalObj.managerDepartmentNameLengthIsValid = true;
  addSystemDepartmentModalObj.managerDepartmentIsEnableIsValid = true;
};

/** 欄位驗證 */
const validateField = () => {
  let isValid = true;

  // 名稱
  if (
    typeof addSystemDepartmentModalObj.managerDepartmentName !== "string" ||
    !addSystemDepartmentModalObj.managerDepartmentName.trim()
  ) {
    // 空值錯誤
    addSystemDepartmentModalObj.managerDepartmentNameIsValid = false;
    isValid = false;
  } else {
    addSystemDepartmentModalObj.managerDepartmentNameIsValid = true;
  }

  // 名稱長度
  if (
    addSystemDepartmentModalObj.managerDepartmentName &&
    addSystemDepartmentModalObj.managerDepartmentName.trim().length > 10
  ) {
    addSystemDepartmentModalObj.managerDepartmentNameLengthIsValid = false;
    isValid = false;
  } else {
    addSystemDepartmentModalObj.managerDepartmentNameLengthIsValid = true;
  }

  // 是否啟用
  if (typeof addSystemDepartmentModalObj.managerDepartmentIsEnable !== "boolean") {
    addSystemDepartmentModalObj.managerDepartmentIsEnableIsValid = false;
    isValid = false;
  } else {
    addSystemDepartmentModalObj.managerDepartmentIsEnableIsValid = true;
  }

  return isValid;
};

/** 點擊【送出】按鈕 */
const handleSubmit = () => {
  // 驗證欄位
  if (!validateField()) return;

  // 發送確認事件給父組件
  emit("confirm", {
    managerDepartmentName: addSystemDepartmentModalObj.managerDepartmentName!.trim(),
    managerDepartmentIsEnable: addSystemDepartmentModalObj.managerDepartmentIsEnable,
  });
};

/** 點擊【取消】按鈕 */
const handleCancel = () => {
  emit("cancel");
};

/** 監聽 show 變化 */
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      resetForm();
    }
  },
  { immediate: true }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">新增部門</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 名稱 -->
          <div>
            <label class="form-label">部門名稱 <span class="required-label">*</span></label>
            <input
              v-model="addSystemDepartmentModalObj.managerDepartmentName"
              class="input-box"
              type="text"
              placeholder="請輸入部門名稱"
            />
            <p class="error-wrapper">
              <span
                v-if="!addSystemDepartmentModalObj.managerDepartmentNameIsValid"
                class="error-tip"
              >
                不可為空
              </span>
              <span
                v-if="!addSystemDepartmentModalObj.managerDepartmentNameLengthIsValid"
                class="error-tip"
              >
                不可超過 10 個字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select
              v-model="addSystemDepartmentModalObj.managerDepartmentIsEnable"
              class="select-box"
            >
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper">
              <span
                v-if="!addSystemDepartmentModalObj.managerDepartmentIsEnableIsValid"
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
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleSubmit">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
