<script setup lang="ts">
import { ref, watch } from "vue";
import GetManyEmployeeComboBox from "@/components/feature/search-bar/GetManyEmployeeComboBox.vue";

interface Props {
  show: boolean;
}

interface Emits {
  (e: "confirm", data: { employeeID: number; employeeName: string }): void;
  (e: "cancel"): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

/** 選擇的員工ID */
const selectedEmployeeID = ref<number | null>(null);
/** 選擇的員工名稱 */
const selectedEmployeeName = ref<string | null>(null);
/** 選擇的員工所屬區域ID */
const managerRegionID = ref<number | null>(null);
/** 選擇的員工所屬部門ID */
const managerDepartmentID = ref<number | null>(null);
/** 選擇的員工所屬區域名稱 */
const managerRegionName = ref<string | null>(null);
/** 選擇的員工所屬部門名稱 */
const managerDepartmentName = ref<string | null>(null);
/** 表單驗證錯誤訊息 */
const errorMessage = ref("");
/** 是否顯示錯誤 */
const showError = ref(false);

/** 重置表單 */
const resetForm = () => {
  selectedEmployeeID.value = null;
  selectedEmployeeName.value = null;
  managerRegionID.value = null;
  managerDepartmentID.value = null;
  managerRegionName.value = null;
  managerDepartmentName.value = null;
  errorMessage.value = "";
  showError.value = false;
};

/** 重置錯誤提示 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};

/** 確認送出 */
const handleConfirm = () => {
  // 驗證必填欄位
  if (!selectedEmployeeID.value || !selectedEmployeeName.value) {
    showError.value = true;
    errorMessage.value = "請選擇要轉指派的業務人員";
    return;
  }

  console.log("確認轉指派業務:", {
    employeeID: selectedEmployeeID.value,
    employeeName: selectedEmployeeName.value,
  });

  emit("confirm", {
    employeeID: selectedEmployeeID.value,
    employeeName: selectedEmployeeName.value,
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
    <div class="w-[520px] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-3 border-b">
        <h2 class="text-lg font-bold text-gray-800">轉指派業務</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="handleCancel"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-8">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded mb-3"
        >
          {{ errorMessage }}
        </p>

        <!-- 業務人員選擇 -->
        <div class="flex flex-col gap-1">
          <label class="form-label">選擇業務 <span class="required-label">*</span></label>
          <GetManyEmployeeComboBox
            v-model:manager-employee-i-d="selectedEmployeeID"
            v-model:manager-employee-name="selectedEmployeeName"
            v-model:manager-region-i-d="managerRegionID"
            v-model:manager-department-i-d="managerDepartmentID"
            v-model:manager-region-name="managerRegionName"
            v-model:manager-department-name="managerDepartmentName"
            :manager-role-i-d="null"
            :disabled="false"
            @change="resetError"
          />
          <div class="h-2"></div>
        </div>

        <!-- 員工資訊顯示區 -->
        <div class="w-full mt-3">
          <div class="bg-white rounded-lg border border-gray-300 flex flex-col h-[200px]">
           
            <!-- 內容 -->
            <div class="p-6 flex-1">
              <template v-if="!selectedEmployeeID">
                <div
                  class="flex flex-col items-center justify-center text-center text-gray-400"
                >
                  <div class="text-4xl mb-2">!</div>
                  <p class="text-sm">請先選擇業務人員</p>
                </div>
              </template>

              <template v-else>
                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">姓名</span>
                  <span class="display-value truncate">{{ selectedEmployeeName || "-" }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">地區</span>
                  <span class="display-value truncate">{{ managerRegionName || "-" }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">部門</span>
                  <span class="display-value truncate">{{
                    managerDepartmentName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />
              </template>
            </div>
          </div>
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
