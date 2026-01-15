<script setup lang="ts">
import { reactive, watch } from "vue";
//----------------------------------------------------------------------------
const props = defineProps<{
  managerCompanyMainKindID: number;
  initialData: {
    managerCompanyMainKindName: string;
    managerCompanyMainKindIsEnable: boolean;
  };
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "confirm", data: { managerCompanyMainKindName: string; managerCompanyMainKindIsEnable: boolean }): void;
}>();
//----------------------------------------------------------------------------
/** 系統設定-名單公司-主分類-更新模型 */
interface SysComMainKindUpdateMdl {
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
  managerCompanyMainKindIsEnable: boolean;
}

/** 系統設定-名單公司-主分類-原始模型 */
interface SysComMainKindOriginalMdl {
  managerCompanyMainKindName: string | null;
  managerCompanyMainKindIsEnable: boolean;
}

/** 系統設定-名單公司-主分類-驗證模型 */
interface SysComMainKindUpdateValidateMdl {
  managerCompanyMainKindName: boolean;
}

/** 系統設定-名單公司-主分類-更新物件 */
const sysComMainKindUpdateObj = reactive<SysComMainKindUpdateMdl>({
  managerCompanyMainKindID: props.managerCompanyMainKindID,
  managerCompanyMainKindName: props.initialData.managerCompanyMainKindName,
  managerCompanyMainKindIsEnable: props.initialData.managerCompanyMainKindIsEnable,
});

/** 系統設定-名單公司-主分類-原始物件 */
const sysComMainKindOriginalMdl = reactive<SysComMainKindOriginalMdl>({
  managerCompanyMainKindName: props.initialData.managerCompanyMainKindName,
  managerCompanyMainKindIsEnable: props.initialData.managerCompanyMainKindIsEnable,
});

/** 系統設定-名單公司-主分類-驗證物件 */
const sysComMainKindUpdateValidObj = reactive<SysComMainKindUpdateValidateMdl>({
  managerCompanyMainKindName: true,
});

// 監聽 props 變化
watch(() => props.initialData, (newData) => {
  sysComMainKindUpdateObj.managerCompanyMainKindName = newData.managerCompanyMainKindName;
  sysComMainKindUpdateObj.managerCompanyMainKindIsEnable = newData.managerCompanyMainKindIsEnable;
  sysComMainKindOriginalMdl.managerCompanyMainKindName = newData.managerCompanyMainKindName;
  sysComMainKindOriginalMdl.managerCompanyMainKindIsEnable = newData.managerCompanyMainKindIsEnable;
}, { deep: true });
// ------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  const name = sysComMainKindUpdateObj.managerCompanyMainKindName?.trim() ?? "";

  // 名稱不可空、不超過50字
  if (!name || name.length > 50) {
    sysComMainKindUpdateValidObj.managerCompanyMainKindName = false;
    isValid = false;
  } else {
    sysComMainKindUpdateValidObj.managerCompanyMainKindName = true;
  }

  return isValid;
};

/** 檢查是否有變更 */
const hasChanges = () => {
  const trimmedName = sysComMainKindUpdateObj.managerCompanyMainKindName?.trim();
  return (
    trimmedName !== sysComMainKindOriginalMdl.managerCompanyMainKindName ||
    sysComMainKindUpdateObj.managerCompanyMainKindIsEnable !== sysComMainKindOriginalMdl.managerCompanyMainKindIsEnable
  );
};

/** 點擊【送出】按鈕 */
const clickSubmitBtn = () => {
  // 欄位驗證
  if (!validateField()) {
    return;
  }

  // 檢查是否有變更
  if (!hasChanges()) {
    alert("沒有任何變更需要儲存！");
    return;
  }

  // 發送資料給父元件
  emit("confirm", {
    managerCompanyMainKindName: sysComMainKindUpdateObj.managerCompanyMainKindName?.trim() ?? "",
    managerCompanyMainKindIsEnable: sysComMainKindUpdateObj.managerCompanyMainKindIsEnable,
  });
};
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯主分類</h2>
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
        <div class="space-y-6">
          <!-- 客戶主分類名稱 -->
          <div>
            <label class="form-label">客戶主分類名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComMainKindUpdateObj.managerCompanyMainKindName"
              class="input-box"
              type="text"
              placeholder="請輸入客戶主分類名稱"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComMainKindUpdateValidObj.managerCompanyMainKindName"
                class="error-tip"
              >
                不可為空，長度不可超過 50 個字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select
              v-model="sysComMainKindUpdateObj.managerCompanyMainKindIsEnable"
              class="select-box"
            >
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p class="error-wrapper"></p>
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
