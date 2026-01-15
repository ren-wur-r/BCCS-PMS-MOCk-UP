<script setup lang="ts">
import { reactive, watch } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import GetManyEmailComboBox from "@/components/feature/search-bar/GetManyEmailComboBox.vue";
import { getBasicManagerContacter } from "@/services";
import {
  MbsSysCttHttpGetManagerContacterReqMdl,
  MbsSysCttHttpGetManagerContacterRspMdl,
} from "@/services/pms-http/basic/basicHttpFormat";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";
import { getManagerContacterStatusLabel } from "@/utils/getManagerContacterStatusLabel";

//----------------------------------------------------------
const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();

//----------------------------------------------------------
/** 窗口資訊完整項目 */
interface ContacterInfo {
  managerContacterID: number;
  employeePipelineContacterIsPrimary: boolean;
  managerContacterName: string;
  managerContacterEmail: string;
  managerContacterPhone: string;
  managerContacterDepartment: string;
  managerContacterJobTitle: string;
  managerContacterTelephone: string;
  managerContacterIsConsent: boolean;
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  atomRatingKind: DbAtomManagerContacterRatingKindEnum;
  managerContacterRemark: string;
}

const props = defineProps<{
  show: boolean;
  managerCompanyID: number | null;
}>();

const emit = defineEmits<{
  (e: "confirm", data: ContacterInfo): void;
  (e: "cancel"): void;
}>();

//----------------------------------------------------------
/** 附加窗口模型 */
interface AddContacterMdl {
  managerContacterID: number | null;
  employeePipelineContacterIsPrimary: boolean;
  managerContacterEmail: string | null;
  managerContacterName: string | null;
  managerContacterPhone: string | null;
  managerContacterTelephone: string | null;
  managerContacterIsConsent: boolean | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterStatus: DbAtomManagerContacterStatusEnum | null;
  managerContacterRemark: string | null;
  atomRatingKind: DbAtomManagerContacterRatingKindEnum | null;
}

/** 附加窗口驗證模型 */
interface AddContacterValidateMdl {
  employeePipelineContacterIsPrimary: boolean;
  managerContacterID: boolean;
}

//--------------------------------------------------------------------------
/** 附加窗口物件 */
const addContacterObj = reactive<AddContacterMdl>({
  managerContacterID: null,
  employeePipelineContacterIsPrimary: true,
  managerContacterEmail: null,
  managerContacterName: null,
  managerContacterPhone: null,
  managerContacterDepartment: null,
  managerContacterJobTitle: null,
  managerContacterStatus: null,
  managerContacterRemark: null,
  managerContacterTelephone: null,
  managerContacterIsConsent: null,
  atomRatingKind: null,
});

/** 附加窗口驗證物件 */
const addContacterValidateObj = reactive<AddContacterValidateMdl>({
  employeePipelineContacterIsPrimary: true,
  managerContacterID: true,
});

/** 是否正在初始化資料 */
let isInitializing = false;

//----------------------------------------------------------
/** 取得【窗口】資料 */
const getContacterData = async (managerContacterID: number) => {
  if (!requireToken()) return;

  const requestData: MbsSysCttHttpGetManagerContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID: managerContacterID,
  };

  const responseData: MbsSysCttHttpGetManagerContacterRspMdl | null =
    await getBasicManagerContacter(requestData);

  if (!responseData || !handleErrorCode(responseData.errorCode)) return;

  addContacterObj.managerContacterID = responseData.managerContacterID;
  addContacterObj.managerContacterName = responseData.managerContacterName;
  addContacterObj.managerContacterEmail = responseData.managerContacterEmail;
  addContacterObj.managerContacterPhone = responseData.managerContacterPhone;
  addContacterObj.managerContacterDepartment = responseData.managerContacterDepartment;
  addContacterObj.managerContacterJobTitle = responseData.managerContacterJobTitle;
  addContacterObj.managerContacterTelephone = responseData.managerContacterTelephone;
  addContacterObj.managerContacterIsConsent = responseData.managerContacterIsAgreeSurvey;
  addContacterObj.managerContacterStatus = responseData.atomManagerContacterStatus;
  addContacterObj.atomRatingKind = responseData.atomManagerContacterRatingKind;
  addContacterObj.managerContacterRemark = responseData.managerContacterRemark;
};

//-----------------------------------------------------------
/** 驗證方法 */
const validateField = (): boolean => {
  let isValid = true;

  // 主要/次要窗口
  if (
    addContacterObj.employeePipelineContacterIsPrimary === null ||
    addContacterObj.employeePipelineContacterIsPrimary === undefined
  ) {
    addContacterValidateObj.employeePipelineContacterIsPrimary = false;
    isValid = false;
  } else {
    addContacterValidateObj.employeePipelineContacterIsPrimary = true;
  }

  // 窗口ID
  if (!addContacterObj.managerContacterID) {
    addContacterValidateObj.managerContacterID = false;
    isValid = false;
  } else {
    addContacterValidateObj.managerContacterID = true;
  }

  return isValid;
};

//----------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = () => {
  if (!validateField()) {
    return;
  }

  if (!addContacterObj.managerContacterID) {
    showError.value = true;
    errorMessage.value = "請選擇窗口";
    return;
  }

  const contacterData: ContacterInfo = {
    managerContacterID: addContacterObj.managerContacterID,
    employeePipelineContacterIsPrimary: addContacterObj.employeePipelineContacterIsPrimary,
    managerContacterName: addContacterObj.managerContacterName || "",
    managerContacterEmail: addContacterObj.managerContacterEmail || "",
    managerContacterPhone: addContacterObj.managerContacterPhone || "",
    managerContacterDepartment: addContacterObj.managerContacterDepartment || "",
    managerContacterJobTitle: addContacterObj.managerContacterJobTitle || "",
    managerContacterTelephone: addContacterObj.managerContacterTelephone || "",
    managerContacterIsConsent: addContacterObj.managerContacterIsConsent || false,
    managerContacterStatus:
      addContacterObj.managerContacterStatus || DbAtomManagerContacterStatusEnum.Employed,
    atomRatingKind:
      addContacterObj.atomRatingKind || DbAtomManagerContacterRatingKindEnum.Whitelist,
    managerContacterRemark: addContacterObj.managerContacterRemark || "",
  };

  emit("confirm", contacterData);
};

/** 點擊【取消】按鈕 */
const clickCancelBtn = () => {
  emit("cancel");
};

/** 重置錯誤提示 */
const resetError = () => {
  showError.value = false;
  errorMessage.value = "";
};

/** 重置表單 */
const resetForm = () => {
  addContacterObj.managerContacterID = null;
  addContacterObj.employeePipelineContacterIsPrimary = true;
  addContacterObj.managerContacterEmail = null;
  addContacterObj.managerContacterName = null;
  addContacterObj.managerContacterPhone = null;
  addContacterObj.managerContacterDepartment = null;
  addContacterObj.managerContacterJobTitle = null;
  addContacterObj.managerContacterStatus = null;
  addContacterObj.managerContacterRemark = null;
  addContacterObj.managerContacterTelephone = null;
  addContacterObj.managerContacterIsConsent = null;
  addContacterObj.atomRatingKind = null;

  addContacterValidateObj.employeePipelineContacterIsPrimary = true;
  addContacterValidateObj.managerContacterID = true;

  resetError();
};

//----------------------------------------------------------
// 當選擇窗口變更時，取得窗口資料
watch(
  () => addContacterObj.managerContacterID,
  async (newVal) => {
    // 跳過初始化階段的變更
    if (isInitializing) return;

    if (newVal) {
      await getContacterData(newVal);
    }
  }
);

// 監聽彈窗顯示狀態
watch(
  () => props.show,
  (newVal) => {
    if (newVal) {
      // 彈窗開啟時，重置表單
      resetForm();
    } else {
      // 彈窗關閉時重置表單
      resetForm();
    }
  }
);
</script>

<template>
  <div v-if="show" class="modal-overlay">
    <div class="w-[630px] max-w-[95vw] max-h-[90vh] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">附加窗口</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="clickCancelBtn"
        >
          ×
        </button>
      </div>

      <!-- 內容區 -->
      <div class="p-6 space-y-4">
        <!-- 錯誤提示 -->
        <p
          v-if="showError"
          class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
        >
          {{ errorMessage }}
        </p>

        <!--查詢-->
        <div class="flex gap-4 w-full">
          <!-- 窗口類別 -->
          <div>
            <label class="form-label">窗口 <span class="required-label">*</span></label>
            <select
              v-model="addContacterObj.employeePipelineContacterIsPrimary"
              class="select-box"
              @change="resetError"
            >
              <option :value="true">主要窗口</option>
              <option :value="false">次要窗口</option>
            </select>
            <div class="h-2">
              <p
                v-if="!addContacterValidateObj.employeePipelineContacterIsPrimary"
                class="error-tip"
              >
                不可為空
              </p>
            </div>
          </div>

          <!-- 電子信箱 -->
          <div class="flex-1">
            <label class="form-label">電子信箱 <span class="required-label">*</span></label>
            <GetManyEmailComboBox
              v-model:managerContacterID="addContacterObj.managerContacterID"
              v-model:managerContacterName="addContacterObj.managerContacterName"
              :managerContacterCompanyID="props.managerCompanyID"
              :disabled="!props.managerCompanyID"
              @change="resetError"
            />
            <div class="h-2">
              <p v-if="!addContacterValidateObj.managerContacterID" class="error-tip">不可為空</p>
            </div>
          </div>
        </div>

        <!-- 窗口資訊顯示區 -->
        <div class="w-full">
          <div class="bg-white rounded-lg border border-gray-300 flex flex-col h-64">

            <!-- 內容 -->
            <div class="p-6 flex-1 overflow-y-auto">
              <template v-if="!addContacterObj.managerContacterID">
                <div
                  class="flex flex-col items-center justify-center text-center text-gray-400 h-full"
                >
                  <div class="text-4xl mb-2">!</div>
                  <p class="mb-4 text-sm">
                    {{ !props.managerCompanyID ? "請先選擇客戶" : "請先查詢窗口資料" }}
                  </p>
                </div>
              </template>

              <template v-else>
                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">姓名</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電子信箱</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterEmail || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">手機</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterPhone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">部門</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterDepartment || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">職稱</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterJobTitle || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電話分機</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterTelephone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">狀態</span>
                  <span class="display-value break-words">{{
                    getManagerContacterStatusLabel(addContacterObj.managerContacterStatus) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">個資同意</span>
                  <span class="display-value break-words">
                    {{
                      addContacterObj.managerContacterIsConsent === null
                        ? "-"
                        : addContacterObj.managerContacterIsConsent
                          ? "是"
                          : "否"
                    }}
                  </span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">開發評等</span>
                  <span class="display-value break-words">{{
                    getManagerContacterRatingLabel(addContacterObj.atomRatingKind) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">備註</span>
                  <span class="display-value break-words">{{
                    addContacterObj.managerContacterRemark || "-"
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
        <button class="btn-cancel" @click="clickCancelBtn">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>
</template>
