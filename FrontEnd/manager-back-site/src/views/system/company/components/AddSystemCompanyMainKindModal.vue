<script setup lang="ts">
import { reactive } from "vue";
//----------------------------------------------------------------------------
const emit = defineEmits<{
  (e: "close"): void;
  (e: "confirm", data: { managerCompanyMainKindName: string; managerCompanyMainKindIsEnable: boolean }): void;
}>();
//----------------------------------------------------------------------------
/** 系統-名單公司-主分類-新增模型 */
interface SysComMainKindAddMdl {
  managerCompanyMainKindName: string;
  managerCompanyMainKindIsEnable: boolean;
}

/** 系統-名單公司-主分類-驗證模型 */
interface SysComMainKindValidateMdl {
  managerCompanyMainKindName: boolean;
}
//----------------------------------------------------------------------------
/** 系統-名單公司-主分類-新增物件 */
const sysComMainKindAddObj = reactive<SysComMainKindAddMdl>({
  managerCompanyMainKindName: "",
  managerCompanyMainKindIsEnable: true,
});

/** 系統-名單公司-主分類-驗證物件 */
const sysComMainKindValidObj = reactive<SysComMainKindValidateMdl>({
  managerCompanyMainKindName: true,
});

//----------------------------------------------------------------------------
/** 驗證【主分類】欄位 */
const validateMainKindField = () => {
  let isValid = true;

  const name = sysComMainKindAddObj.managerCompanyMainKindName.trim();

  // 名稱驗證
  if (!name || name.length > 50) {
    sysComMainKindValidObj.managerCompanyMainKindName = false;
    isValid = false;
  } else {
    sysComMainKindValidObj.managerCompanyMainKindName = true;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/** 點擊提交【主分類】按鈕 */
const clickSubmitMainKindBtn = () => {
  // 欄位驗證
  if (!validateMainKindField()) {
    return;
  }
  
  // 發送資料給父元件
  emit("confirm", {
    managerCompanyMainKindName: sysComMainKindAddObj.managerCompanyMainKindName.trim(),
    managerCompanyMainKindIsEnable: sysComMainKindAddObj.managerCompanyMainKindIsEnable,
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
        <div class="space-y-6">
          <!-- 客戶主分類名稱 -->
          <div>
            <label class="form-label">客戶主分類名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComMainKindAddObj.managerCompanyMainKindName"
              class="input-box"
              type="text"
              placeholder="請輸入客戶主分類名稱"
            />
            <p  class="error-wrapper">
              <span v-if="!sysComMainKindValidObj.managerCompanyMainKindName" class="error-tip">
                不可為空，長度不可超過 50 個字
              </span>
            </p>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態 <span class="required-label">*</span></label>
            <select
              v-model="sysComMainKindAddObj.managerCompanyMainKindIsEnable"
              class="select-box"
            >
              <option :value="true">啟用</option>
              <option :value="false">停用</option>
            </select>
            <p  class="error-wrapper"></p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitMainKindBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
