<script setup lang="ts">
//#region 引入
import { reactive, watch } from "vue";

//#endregion

//#region Prop & Emit
interface Props {
  /** 顯示 Modal */
  show: boolean;
  /** 地區資料 */
  regionData: {
    managerRegionID: number;
    managerRegionName: string;
    managerRegionIsEnable: boolean;
  } | null;
}

interface Emits {
  /** 取消 */
  (e: "cancel"): void;
  /** 確認送出 */
  (
    e: "confirm",
    data: {
      managerRegionName: string;
      managerRegionIsEnable: boolean;
    }
  ): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();
//#endregion

//#region 型別定義
/** 系統設定-地區彈跳視窗-頁面模型 */
interface UpdateSystemRegionModalMdl {
  managerRegionName: string | null;
  managerRegionIsEnable: boolean;
  managerRegionNameIsValid: boolean;
  managerRegionNameLengthIsValid: boolean;
  managerRegionIsEnableIsValid: boolean;
}
//#endregion

//#region 頁面物件
/** 系統設定-地區彈跳視窗-頁面物件 */
const updateSystemRegionModalObj = reactive<UpdateSystemRegionModalMdl>({
  managerRegionName: null,
  managerRegionIsEnable: true,
  managerRegionNameIsValid: true,
  managerRegionNameLengthIsValid: true,
  managerRegionIsEnableIsValid: true,
});
//#endregion

//#region 驗證相關
/** 欄位驗證 */
const validateField = () => {
  let isValid = true;

  // 名稱
  if (
    typeof updateSystemRegionModalObj.managerRegionName !== "string" ||
    !updateSystemRegionModalObj.managerRegionName.trim()
  ) {
    // 空值錯誤
    updateSystemRegionModalObj.managerRegionNameIsValid = false;
    isValid = false;
  } else {
    updateSystemRegionModalObj.managerRegionNameIsValid = true;
  }

  // 名稱長度
  if (
    updateSystemRegionModalObj.managerRegionName &&
    updateSystemRegionModalObj.managerRegionName.trim().length > 10
  ) {
    updateSystemRegionModalObj.managerRegionNameLengthIsValid = false;
    isValid = false;
  } else {
    updateSystemRegionModalObj.managerRegionNameLengthIsValid = true;
  }

  // 是否啟用
  if (typeof updateSystemRegionModalObj.managerRegionIsEnable !== "boolean") {
    updateSystemRegionModalObj.managerRegionIsEnableIsValid = false;
    isValid = false;
  } else {
    updateSystemRegionModalObj.managerRegionIsEnableIsValid = true;
  }

  return isValid;
};
//#endregion

//#region UI行為 / 畫面處理
/** 載入資料到表單 */
const loadRegionData = () => {
  if (!props.regionData) return;

  updateSystemRegionModalObj.managerRegionName = props.regionData.managerRegionName;
  updateSystemRegionModalObj.managerRegionIsEnable = props.regionData.managerRegionIsEnable;
  updateSystemRegionModalObj.managerRegionNameIsValid = true;
  updateSystemRegionModalObj.managerRegionNameLengthIsValid = true;
  updateSystemRegionModalObj.managerRegionIsEnableIsValid = true;
};
//#endregion

//#region 按鈕事件
/** 點擊【送出】按鈕 */
const handleSubmit = () => {
  // 驗證欄位
  if (!validateField()) return;

  // 發送確認事件給父組件
  emit("confirm", {
    managerRegionName: updateSystemRegionModalObj.managerRegionName!.trim(),
    managerRegionIsEnable: updateSystemRegionModalObj.managerRegionIsEnable,
  });
};

/** 點擊【取消】按鈕 */
const handleCancel = () => {
  emit("cancel");
};
//#endregion

//#region 監聽器
/** 監聽 show 和 regionData 變化 */
watch(
  () => [props.show, props.regionData],
  () => {
    if (props.show && props.regionData) {
      loadRegionData();
    }
  },
  { immediate: true, deep: true }
);
//#endregion
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯地區</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 地區名稱 -->
          <div>
            <label class="form-label">地區名稱 <span class="required-label">*</span></label>
            <input
              v-model="updateSystemRegionModalObj.managerRegionName"
              class="input-box"
              type="text"
              placeholder="請輸入地區名稱"
            />
            <p class="error-wrapper">
              <span v-if="!updateSystemRegionModalObj.managerRegionNameIsValid" class="error-tip">
                不可為空
              </span>
              <span
                v-if="!updateSystemRegionModalObj.managerRegionNameLengthIsValid"
                class="error-tip"
              >
                不可超過 10 個字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select v-model="updateSystemRegionModalObj.managerRegionIsEnable" class="select-box">
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper">
              <span
                v-if="!updateSystemRegionModalObj.managerRegionIsEnableIsValid"
                class="error-tip"
              >
                不可為空</span
              >
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleSubmit">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
