<script setup lang="ts">
import { reactive, onMounted } from "vue";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import {
  MbsCrmPhnHttpAddEmployeePipelinePhoneReqMdl,
  MbsCrmPhnHttpAddEmployeePipelinePhoneRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import { addPhoneEmployeePipelinePhone } from "@/services";
import TextCounter from "@/components/global/counter/TextCounter.vue";

const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, errorMessage, showError } = useErrorCodeHandler();
//----------------------------------------------------------------------------
interface ContacterItem {
  employeePipelineContacterID: number;
  managerContacterID: number;
  managerContacterName: string | null;
}

const props = defineProps<{
  employeePipelineID: number;
  managerContacterCompanyID: number;
  contacterList: ContacterItem[];
}>();

const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//----------------------------------------------------------------------------
/** 取得當前時間並格式化為 datetime-local 格式 (YYYY-MM-DDTHH:mm) */
const getCurrentDateTime = (): string => {
  const now = new Date();
  const year = now.getFullYear();
  const month = String(now.getMonth() + 1).padStart(2, "0");
  const day = String(now.getDate()).padStart(2, "0");
  const hours = String(now.getHours()).padStart(2, "0");
  const minutes = String(now.getMinutes()).padStart(2, "0");
  return `${year}-${month}-${day}T${hours}:${minutes}`;
};
//----------------------------------------------------------------------------
/** 新增-電銷-電銷-模型 */
interface AddCrmPhonePhoneMdl {
  employeePipelinePhoneRecordTime: string;
  managerContacterID: number;
  employeePipelinePhoneTitle: string;
  employeePipelinePhoneRemark: string;
}

/** 新增-電銷-電銷-驗證模型 */
interface AddCrmPhonePhoneValidateMdl {
  employeePipelinePhoneRecordTime: boolean;
  managerContacterID: boolean;
  employeePipelinePhoneTitle: boolean;
}
//----------------------------------------------------------------------------
/** 新增-電銷-電銷-物件 */
const crmPhoneRecordAddObj = reactive<AddCrmPhonePhoneMdl>({
  employeePipelinePhoneRecordTime: "",
  managerContacterID: 0,
  employeePipelinePhoneTitle: "",
  employeePipelinePhoneRemark: "",
});

/** 新增-電銷-電銷-驗證物件 */
const crmPhoneRecordValidObj = reactive<AddCrmPhonePhoneValidateMdl>({
  employeePipelinePhoneRecordTime: true,
  managerContacterID: true,
  employeePipelinePhoneTitle: true,
});
//----------------------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 時間
  if (!crmPhoneRecordAddObj.employeePipelinePhoneRecordTime) {
    crmPhoneRecordValidObj.employeePipelinePhoneRecordTime = false;
    isValid = false;
  } else {
    crmPhoneRecordValidObj.employeePipelinePhoneRecordTime = true;
  }

  // 窗口
  if (!crmPhoneRecordAddObj.managerContacterID) {
    crmPhoneRecordValidObj.managerContacterID = false;
    isValid = false;
  } else {
    crmPhoneRecordValidObj.managerContacterID = true;
  }

  // 電銷主題
  const title = crmPhoneRecordAddObj.employeePipelinePhoneTitle.trim();
  if (!title || title.length === 0 || title.length > 100) {
    crmPhoneRecordValidObj.employeePipelinePhoneTitle = false;
    isValid = false;
  } else {
    crmPhoneRecordValidObj.employeePipelinePhoneTitle = true;
  }

  return isValid;
};
//----------------------------------------------------------------------------
/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  //檢查token
  if (!requireToken()) return;
  // 驗證欄位
  if (!validateField()) return;

  // 呼叫api
  const requestData: MbsCrmPhnHttpAddEmployeePipelinePhoneReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: props.employeePipelineID,
    employeePipelinePhoneRecordTime: crmPhoneRecordAddObj.employeePipelinePhoneRecordTime,
    managerContacterID: crmPhoneRecordAddObj.managerContacterID,
    employeePipelinePhoneTitle: crmPhoneRecordAddObj.employeePipelinePhoneTitle,
    employeePipelinePhoneRemark: crmPhoneRecordAddObj.employeePipelinePhoneRemark,
  };

  const responseData: MbsCrmPhnHttpAddEmployeePipelinePhoneRspMdl | null =
    await addPhoneEmployeePipelinePhone(requestData);

  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  emit("submit");
  emit("close");
};

/** 初始化當前時間 */
onMounted(() => {
  crmPhoneRecordAddObj.employeePipelinePhoneRecordTime = getCurrentDateTime();
});
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">附加電銷記錄</h2>
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
          <!-- 錯誤訊息 -->
          <p
            v-if="showError"
            class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
          >
            {{ errorMessage }}
          </p>

          <!-- 第一排：時間 / 窗口 -->
          <div class="grid grid-cols-2 gap-4 w-full">
            <!-- 時間 -->
            <div>
              <label class="form-label">時間 <span class="required-label">*</span></label>
              <input
                v-model="crmPhoneRecordAddObj.employeePipelinePhoneRecordTime"
                type="datetime-local"
                class="input-box w-full min-w-0"
                placeholder="請選擇時間"
              />
              <p class="error-wrapper">
                <span
                  v-if="!crmPhoneRecordValidObj.employeePipelinePhoneRecordTime"
                  class="error-tip"
                >
                  不可為空
                </span>
              </p>
            </div>

            <!-- 窗口 -->
            <div>
              <label class="form-label">窗口 <span class="required-label">*</span></label>
              <select v-model="crmPhoneRecordAddObj.managerContacterID" class="select-box">
                <option :value="0" disabled>請選擇窗口</option>
                <option
                  v-for="contacter in props.contacterList"
                  :key="contacter.managerContacterID"
                  :value="contacter.managerContacterID"
                >
                  {{ contacter.managerContacterName || "-" }}
                </option>
              </select>
              <p class="error-wrapper">
                <span v-if="!crmPhoneRecordValidObj.managerContacterID" class="error-tip">
                  不可為空
                </span>
              </p>
            </div>
          </div>

          <!-- 電銷主題 -->
          <div>
            <label class="form-label">電銷主題 <span class="required-label">*</span></label>
            <input
              v-model="crmPhoneRecordAddObj.employeePipelinePhoneTitle"
              type="text"
              class="input-box"
              placeholder="請輸入電銷主題"
            />
            <p class="error-wrapper">
              <span v-if="!crmPhoneRecordValidObj.employeePipelinePhoneTitle" class="error-tip">
                不可為空，長度不可超過 100 個字
              </span>
            </p>
          </div>

          <!-- 備註 -->
          <div>
            <label class="form-label">備註</label>

            <textarea
              v-model="crmPhoneRecordAddObj.employeePipelinePhoneRemark"
              class="textarea-box"
              rows="6"
              placeholder="請輸入備註"
            ></textarea>

            <!-- 字數統計 -->
            <TextCounter
              :text="crmPhoneRecordAddObj.employeePipelinePhoneRemark"
              :max-length="2000"
              :required="false"
            />
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-4 border-t border-gray-300">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
