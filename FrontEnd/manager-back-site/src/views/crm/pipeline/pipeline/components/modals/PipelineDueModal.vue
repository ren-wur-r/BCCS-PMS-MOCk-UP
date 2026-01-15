<script setup lang="ts">
import { ref, watch, computed } from "vue";

interface DueItem {
  employeePipelineDueID: number | null;
  employeePipelineDueTime: string;
  employeePipelineDueRemark: string;
}

interface Props {
  show: boolean;
  /** 編輯模式時傳入的履約期限資料 */
  due?: DueItem | null;
}

interface Emits {
  (
    e: "confirm",
    data: { employeePipelineDueTime: string; employeePipelineDueRemark: string }
  ): void;
  (e: "cancel"): void;
}

const props = withDefaults(defineProps<Props>(), {
  due: null,
});

const emit = defineEmits<Emits>();

/** 是否為編輯模式 */
const isEditMode = computed(() => !!props.due);

/** 履約日期 */
const employeePipelineDueTime = ref("");

/** 備註 */
const employeePipelineDueRemark = ref("");

/** 錯誤訊息 */
const errorMessage = ref("");

/** 載入資料到表單（編輯模式） */
const loadDueData = () => {
  if (!props.due) return;

  // 確保時間格式符合 date 輸入格式 (YYYY-MM-DD)
  const date = new Date(props.due.employeePipelineDueTime);
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");

  employeePipelineDueTime.value = `${year}-${month}-${day}`;
  employeePipelineDueRemark.value = props.due.employeePipelineDueRemark || "";
};

/** 重置表單 */
const resetForm = () => {
  employeePipelineDueTime.value = "";
  employeePipelineDueRemark.value = "";
  errorMessage.value = "";
};

/** 監聽 show 變化 */
watch(
  () => props.show,
  async (newVal) => {
    if (newVal) {
      if (props.due) {
        loadDueData();
      } else {
        resetForm();
      }
    }
  }
);

/** 監聽履約日期變化，清除錯誤訊息 */
watch(
  () => employeePipelineDueTime.value,
  () => {
    if (errorMessage.value && employeePipelineDueTime.value.trim()) {
      errorMessage.value = "";
    }
  }
);

/** 處理確認 */
const handleConfirm = () => {
  if (!employeePipelineDueTime.value.trim()) {
    errorMessage.value = "履約日期不可為空";
    return;
  }

  emit("confirm", {
    employeePipelineDueTime: employeePipelineDueTime.value,
    employeePipelineDueRemark: employeePipelineDueRemark.value,
  });
};

/** 處理取消 */
const handleCancel = () => {
  emit("cancel");
};
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">
          {{ isEditMode ? "編輯履約期限通知" : "附加履約期限通知" }}
        </h2>
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
      <div class="p-6 overflow-y-auto flex-1">
        <div class="space-y-4">
          <!-- 履約日期 -->
          <div class="flex flex-col gap-2">
            <label class="form-label"> 履約日期 <span class="required-label">*</span> </label>
            <input
              v-model="employeePipelineDueTime"
              type="date"
              class="input-box"
              :class="{ 'border-red-500': errorMessage }"
            />
            <p v-if="errorMessage" class="text-sm text-red-600">
              {{ errorMessage }}
            </p>
          </div>

          <!-- 備註 -->
          <div class="flex flex-col gap-2">
            <label class="form-label">備註</label>
            <textarea
              v-model="employeePipelineDueRemark"
              rows="7"
              class="textarea-box resize-none"
              placeholder="請輸入備註..."
            ></textarea>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="handleCancel">取消</button>
          <button class="btn-submit" @click="handleConfirm">
            {{ isEditMode ? "確定" : "送出" }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
