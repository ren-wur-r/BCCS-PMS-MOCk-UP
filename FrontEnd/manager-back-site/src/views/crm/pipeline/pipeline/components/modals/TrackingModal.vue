<script setup lang="ts">
import { ref, watch } from "vue";
import { formatToDatetimeLocal } from "@/utils/timeFormatter";

interface TrackingData {
  employeePipelineSalerTrackingTime: string;
  managerContacterID: number;
  managerContacterName: string;
  employeePipelineSalerTrackingRemark: string;
}

interface Props {
  show: boolean;
  /** 可選擇的窗口列表 */
  contacterList?: Array<{
    managerContacterID: number;
    managerContacterName: string;
  }>;
  /** 編輯時傳入的開發記錄資料 */
  tracking?: TrackingData | null;
}

interface Emits {
  (
    e: "confirm",
    data: {
      employeePipelineSalerTrackingTime: string;
      managerContacterID: number;
      employeePipelineSalerTrackingRemark: string;
    }
  ): void;
  (e: "cancel"): void;
}

const props = withDefaults(defineProps<Props>(), {
  contacterList: () => [],
  tracking: null,
});

const emit = defineEmits<Emits>();

/** 開發時間 */
const trackingTime = ref("");
/** 選擇的窗口ID */
const selectedContacterID = ref<number | null>(null);
/** 開發記錄備註 */
const trackingRemark = ref("");
/** 表單驗證錯誤訊息 */
const errorMessages = ref({
  trackingTime: "",
  contacterID: "",
  trackingRemark: "",
});

/** 是否為編輯模式 */
const isEditMode = ref(false);

/** 重置表單 */
const resetForm = () => {
  if (props.tracking) {
    // 編輯模式：載入現有資料
    isEditMode.value = true;
    trackingTime.value = formatToDatetimeLocal(props.tracking.employeePipelineSalerTrackingTime);
    selectedContacterID.value = props.tracking.managerContacterID;
    trackingRemark.value = props.tracking.employeePipelineSalerTrackingRemark;
  } else {
    // 新增模式：預設為當前時間
    isEditMode.value = false;
    const now = new Date();
    trackingTime.value = formatToDatetimeLocal(now.toISOString());
    selectedContacterID.value = null;
    trackingRemark.value = "";
  }
  clearErrors();
};

/** 清除錯誤訊息 */
const clearErrors = () => {
  errorMessages.value = {
    trackingTime: "",
    contacterID: "",
    trackingRemark: "",
  };
};

/** 驗證表單 */
const validateForm = (): boolean => {
  clearErrors();
  let isValid = true;

  // 驗證開發時間
  if (!trackingTime.value) {
    errorMessages.value.trackingTime = "請選擇開發時間";
    isValid = false;
  }

  // 驗證窗口
  if (!selectedContacterID.value) {
    errorMessages.value.contacterID = "請選擇窗口";
    isValid = false;
  }

  // 驗證備註
  if (!trackingRemark.value.trim()) {
    errorMessages.value.trackingRemark = "請輸入備註";
    isValid = false;
  }

  return isValid;
};

/** 確認送出 */
const handleConfirm = () => {
  if (!validateForm()) {
    return;
  }

  emit("confirm", {
    employeePipelineSalerTrackingTime: trackingTime.value,
    managerContacterID: selectedContacterID.value!,
    employeePipelineSalerTrackingRemark: trackingRemark.value.trim(),
  });
};

/** 取消 */
const handleCancel = () => {
  emit("cancel");
};

/** 監聽 show 變化，重置表單 */
watch(
  () => props.show,
  (newShow) => {
    if (newShow) {
      resetForm();
    }
  }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div class="w-[500px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">
          {{ isEditMode ? "編輯商機開發記錄" : "附加商機開發記錄" }}
        </h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-6">
        <!-- 開發時間 -->
        <div class="mb-4">
          <label class="form-label">
            開發時間
            <span class="required-label">*</span>
          </label>
          <input
            v-model="trackingTime"
            type="datetime-local"
            class="input-box w-full"
            :class="{ 'border-red-500': errorMessages.trackingTime }"
          />
          <p v-if="errorMessages.trackingTime" class="error-tip">
            {{ errorMessages.trackingTime }}
          </p>
        </div>

        <!-- 窗口 -->
        <div class="flex flex-col mb-4 gap-2">
          <label class="form-label">
            窗口
            <span class="required-label">*</span>
          </label>
          <select
            v-model.number="selectedContacterID"
            class="select-box w-full"
            :class="{ 'border-red-500': errorMessages.contacterID }"
          >
            <option :value="null" disabled>請選擇窗口</option>
            <option
              v-for="contacter in contacterList"
              :key="contacter.managerContacterID"
              :value="contacter.managerContacterID"
            >
              {{ contacter.managerContacterName }}
            </option>
          </select>
          <p v-if="errorMessages.contacterID" class="error-tip">
            {{ errorMessages.contacterID }}
          </p>
        </div>

        <!-- 備註 -->
        <div class="mb-4">
          <label class="form-label">
            備註
            <span class="required-label">*</span>
          </label>
          <textarea
            v-model="trackingRemark"
            rows="4"
            class="textarea-box resize-none"
            :class="{ 'border-red-500': errorMessages.trackingRemark }"
            placeholder="請輸入開發記錄備註"
          ></textarea>
          <p v-if="errorMessages.trackingRemark" class="error-tip">
            {{ errorMessages.trackingRemark }}
          </p>
        </div>
      </div>

      <!-- 底部按鈕 -->
      <div class="flex justify-end gap-2 p-4 border-t">
        <button class="btn-cancel" @click="handleCancel">取消</button>
        <button class="btn-submit" @click="handleConfirm">送出</button>
      </div>
    </div>
  </div>
</template>
