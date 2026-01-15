<script setup lang="ts">
import { reactive, watch } from "vue";
//----------------------------------------------------------------------------
const props = defineProps<{
  managerCompanySubKindID: number;
  initialData: {
    managerCompanySubKindName: string;
    managerCompanySubKindIsEnable: boolean;
    managerCompanyMainKindID: number;
    managerCompanyMainKindName: string | null;
  };
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "confirm", data: { managerCompanySubKindName: string; managerCompanySubKindIsEnable: boolean }): void;
}>();
//----------------------------------------------------------------------------
/** 系統設定-名單公司-子分類-編輯模型 */
interface SysComSubKindUpdateMdl {
  managerCompanySubKindID: number;
  managerCompanySubKindName: string;
  managerCompanySubKindIsEnable: boolean;
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
}

/** 系統設定-名單公司-子分類-原始模型 */
interface SysComSubKindOriginalMdl {
  managerCompanySubKindName: string;
  managerCompanySubKindIsEnable: boolean;
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string | null;
}

/** 系統-名單公司-子分類-驗證模型 */
interface SysComSubKindValidateMdl {
  managerCompanyMainKindID: boolean;
  managerCompanySubKindName: boolean;
}
// ------------------------------------------------------------
/** 系統設定-名單公司-子分類-更新物件 */
const sysComSubKindUpdateObj = reactive<SysComSubKindUpdateMdl>({
  managerCompanySubKindID: props.managerCompanySubKindID,
  managerCompanySubKindName: props.initialData.managerCompanySubKindName,
  managerCompanySubKindIsEnable: props.initialData.managerCompanySubKindIsEnable,
  managerCompanyMainKindID: props.initialData.managerCompanyMainKindID,
  managerCompanyMainKindName: props.initialData.managerCompanyMainKindName,
});

/** 系統設定-名單公司-子分類-原始物件 */
const sysComSubKindOriginalObj = reactive<SysComSubKindOriginalMdl>({
  managerCompanySubKindName: props.initialData.managerCompanySubKindName,
  managerCompanySubKindIsEnable: props.initialData.managerCompanySubKindIsEnable,
  managerCompanyMainKindID: props.initialData.managerCompanyMainKindID,
  managerCompanyMainKindName: props.initialData.managerCompanyMainKindName,
});

/** 系統-名單公司-子分類-驗證物件 */
const sysComSubKindValidObj = reactive<SysComSubKindValidateMdl>({
  managerCompanyMainKindID: true,
  managerCompanySubKindName: true,
});

// 監聽 props 變化
watch(() => props.initialData, (newData) => {
  sysComSubKindUpdateObj.managerCompanySubKindName = newData.managerCompanySubKindName;
  sysComSubKindUpdateObj.managerCompanySubKindIsEnable = newData.managerCompanySubKindIsEnable;
  sysComSubKindUpdateObj.managerCompanyMainKindID = newData.managerCompanyMainKindID;
  sysComSubKindUpdateObj.managerCompanyMainKindName = newData.managerCompanyMainKindName;
  sysComSubKindOriginalObj.managerCompanySubKindName = newData.managerCompanySubKindName;
  sysComSubKindOriginalObj.managerCompanySubKindIsEnable = newData.managerCompanySubKindIsEnable;
  sysComSubKindOriginalObj.managerCompanyMainKindID = newData.managerCompanyMainKindID;
  sysComSubKindOriginalObj.managerCompanyMainKindName = newData.managerCompanyMainKindName;
}, { deep: true });
//----------------------------------------------------------------------------
/** 驗證【子分類】欄位 */
const validateSubKindField = () => {
  let isValid = true;

  const name = sysComSubKindUpdateObj.managerCompanySubKindName.trim();

  // 名稱驗證
  if (!name || name.length > 50) {
    sysComSubKindValidObj.managerCompanySubKindName = false;
    isValid = false;
  } else {
    sysComSubKindValidObj.managerCompanySubKindName = true;
  }

  return isValid;
};

/** 檢查是否有變更 */
const hasChanges = () => {
  const trimmedName = sysComSubKindUpdateObj.managerCompanySubKindName?.trim();
  return (
    trimmedName !== sysComSubKindOriginalObj.managerCompanySubKindName ||
    sysComSubKindUpdateObj.managerCompanySubKindIsEnable !== sysComSubKindOriginalObj.managerCompanySubKindIsEnable
  );
};

/** 點擊【送出】按鈕 */
const clickSubmitBtn = () => {
  // 欄位驗證
  if (!validateSubKindField()) {
    return;
  }

  // 檢查是否有變更
  if (!hasChanges()) {
    alert("沒有任何變更需要儲存！");
    return;
  }

  // 發送資料給父元件
  emit("confirm", {
    managerCompanySubKindName: sysComSubKindUpdateObj.managerCompanySubKindName.trim(),
    managerCompanySubKindIsEnable: sysComSubKindUpdateObj.managerCompanySubKindIsEnable,
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
        <div class="space-y-6">
          <!-- 客戶子分類名稱 -->
          <div>
            <label class="form-label">客戶子分類名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComSubKindUpdateObj.managerCompanySubKindName"
              class="input-box"
              type="text"
              placeholder="請輸入客戶子分類名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComSubKindValidObj.managerCompanySubKindName" class="error-tip">
                不可為空，長度不可超過 50 個字
              </span>
            </p>
          </div>

          <!-- 客戶主分類 -->
          <div>
            <label class="form-label">客戶主分類 <span class="required-label">*</span></label>
            <input
              v-model="sysComSubKindUpdateObj.managerCompanyMainKindName"
              class="input-box"
              type="text"
              disabled
            />
            <p class="error-wrapper"></p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select
              v-model="sysComSubKindUpdateObj.managerCompanySubKindIsEnable"
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
