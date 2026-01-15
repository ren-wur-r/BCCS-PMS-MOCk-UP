<script setup lang="ts">
import { reactive, watch, computed } from "vue";

//----------------------------------------------------------------------------
interface Props {
  /** 顯示 Modal */
  show: boolean;
  /** 編輯模式時傳入的資料，null 表示新增模式 */
  departmentData?: {
    managerDepartmentID: number;
    managerDepartmentName: string;
    managerDepartmentIsEnable: boolean;
  } | null;
}

const props = withDefaults(defineProps<Props>(), {
  departmentData: null,
});

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
/** 是否為編輯模式 */
const isEditMode = computed(() => !!props.departmentData);

/** 系統設定-部門彈跳視窗-頁面模型 */
interface SystemDepartmentModalMdl {
  managerDepartmentName: string | null;
  managerDepartmentIsEnable: boolean;
  managerDepartmentNameIsValid: boolean;
  managerDepartmentNameLengthIsValid: boolean;
  managerDepartmentIsEnableIsValid: boolean;
}

/** 系統設定-部門彈跳視窗-頁面物件 */
const systemDepartmentModalObj = reactive<SystemDepartmentModalMdl>({
  managerDepartmentName: null,
  managerDepartmentIsEnable: true,
  managerDepartmentNameIsValid: true,
  managerDepartmentNameLengthIsValid: true,
  managerDepartmentIsEnableIsValid: true,
});

//--------------------------------------------------------------------------
/** 載入資料到表單（編輯模式） */
const loadDepartmentData = () => {
  if (!props.departmentData) {
    // 新增模式：重置表單
    systemDepartmentModalObj.managerDepartmentName = null;
    systemDepartmentModalObj.managerDepartmentIsEnable = true;
    systemDepartmentModalObj.managerDepartmentNameIsValid = true;
    systemDepartmentModalObj.managerDepartmentIsEnableIsValid = true;
    return;
  }

  // 編輯模式：載入資料
  systemDepartmentModalObj.managerDepartmentName = props.departmentData.managerDepartmentName;
  systemDepartmentModalObj.managerDepartmentIsEnable =
    props.departmentData.managerDepartmentIsEnable;
  systemDepartmentModalObj.managerDepartmentNameIsValid = true;
  systemDepartmentModalObj.managerDepartmentIsEnableIsValid = true;
};

/** 欄位驗證 */
const validateField = () => {
  let isValid = true;

  // 名稱
  if (
    typeof systemDepartmentModalObj.managerDepartmentName !== "string" ||
    !systemDepartmentModalObj.managerDepartmentName.trim()
  ) {
    // 空值錯誤
    systemDepartmentModalObj.managerDepartmentNameIsValid = false;
    isValid = false;
  } else {
    systemDepartmentModalObj.managerDepartmentNameIsValid = true;
  }

  // 名稱長度
  if (
    systemDepartmentModalObj.managerDepartmentName &&
    systemDepartmentModalObj.managerDepartmentName.trim().length > 10
  ) {
    systemDepartmentModalObj.managerDepartmentNameLengthIsValid = false;
    isValid = false;
  } else {
    systemDepartmentModalObj.managerDepartmentNameLengthIsValid = true;
  }

  // 是否啟用
  if (typeof systemDepartmentModalObj.managerDepartmentIsEnable !== "boolean") {
    systemDepartmentModalObj.managerDepartmentIsEnableIsValid = false;
    isValid = false;
  } else {
    systemDepartmentModalObj.managerDepartmentIsEnableIsValid = true;
  }

  return isValid;
};

/** 點擊【送出】按鈕 */
const handleSubmit = () => {
  // 驗證欄位
  if (!validateField()) return;

  // 發送確認事件給父組件
  emit("confirm", {
    managerDepartmentName: systemDepartmentModalObj.managerDepartmentName!.trim(),
    managerDepartmentIsEnable: systemDepartmentModalObj.managerDepartmentIsEnable,
  });
};

/** 點擊【取消】按鈕 */
const handleCancel = () => {
  emit("cancel");
};

/** 監聽 show 和 departmentData 變化 */
watch(
  () => [props.show, props.departmentData],
  () => {
    if (props.show) {
      loadDepartmentData();
    }
  },
  { immediate: true, deep: true }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">{{ isEditMode ? "編輯部門" : "新增部門" }}</h2>
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
              v-model="systemDepartmentModalObj.managerDepartmentName"
              class="input-box"
              type="text"
              placeholder="請輸入部門名稱"
            />
            <p class="error-wrapper">
              <span v-if="!systemDepartmentModalObj.managerDepartmentNameIsValid" class="error-tip">
                不可為空
              </span>
              <span
                v-if="!systemDepartmentModalObj.managerDepartmentNameLengthIsValid"
                class="error-tip"
              >
                不可超過 10 個字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select v-model="systemDepartmentModalObj.managerDepartmentIsEnable" class="select-box">
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper">
              <span
                v-if="!systemDepartmentModalObj.managerDepartmentIsEnableIsValid"
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
