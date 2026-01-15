<script setup lang="ts">
import { reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import {
  MbsCrmPhnHttpAddEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpAddEmployeePipelineContacterRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import { addPhoneEmployeePipelineContacter, getBasicManagerContacter } from "@/services";
import GetManyEmailComboBox from "@/components/feature/search-bar/GetManyEmailComboBox.vue";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { MbsSysCttHttpGetManagerContacterReqMdl } from "@/services/pms-http/basic/basicHttpFormat";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";
import { getManagerContacterStatusLabel } from "@/utils/getManagerContacterStatusLabel";

const tokenStore = useTokenStore();
const { requireToken } = useAuth();
const { handleErrorCode, showError, errorMessage } = useErrorCodeHandler();
//-------------------------------------------------------------
const props = defineProps<{
  employeePipelineContacterID: number;
  managerContacterCompanyID: number;

  originalContacter?: {
    managerContacterName: string | null;
    managerContacterEmail: string | null;
    managerContacterPhone: string | null;
    managerContacterTelephone: string | null;
    managerContacterDepartment: string | null;
    managerContacterJobTitle: string | null;
  } | null;
}>();
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();

//-------------------------------------------------------------
/** 新增-電銷-窗口-模型 */
interface AddCrmPhoneContacterMdl {
  employeePipelineContacterID: number;
  managerContacterID: number;
  employeePipelineContacterIsPrimary: boolean;
  managerContacterEmail: string | null;
  managerContacterName: string | null;
  managerContacterPhone: string | null;
  managerContacterTelephone: string | null;
  managerContacterIsAgreeSurvey: boolean | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterStatus: DbAtomManagerContacterStatusEnum | null;
  managerContacterRemark: string | null;
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum | null;
}

/** 新增-電銷-窗口-驗證模型 */
interface AddCrmPhoneContacterValidMdl {
  employeePipelineContacterIsPrimary: boolean;
  managerContacterID: boolean;
}

/** 新增-電銷-窗口-物件 */
const addCrmPhoneContacterObj = reactive<AddCrmPhoneContacterMdl>({
  employeePipelineContacterID: props.employeePipelineContacterID,
  managerContacterID: 0,
  employeePipelineContacterIsPrimary: true,
  managerContacterEmail: null,
  managerContacterName: null,
  managerContacterPhone: null,
  managerContacterDepartment: null,
  managerContacterJobTitle: null,
  managerContacterStatus: null,
  managerContacterRemark: null,
  managerContacterTelephone: null,
  managerContacterIsAgreeSurvey: null,
  atomManagerContacterRatingKind: null,
});

/** 新增-電銷-窗口-驗證物件 */
const addCrmPhoneContacterValidObj = reactive<AddCrmPhoneContacterValidMdl>({
  employeePipelineContacterIsPrimary: true,
  managerContacterID: true,
});
//-------------------------------------------------------------
/** 取得【窗口】資料 */
const getPhoneEmployeePipelineContacterData = async () => {
  if (!requireToken()) return;

  const requestData: MbsSysCttHttpGetManagerContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID: addCrmPhoneContacterObj.managerContacterID,
  };

  if (!addCrmPhoneContacterObj.managerContacterID) return;

  const responseData = await getBasicManagerContacter(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  addCrmPhoneContacterObj.managerContacterID = responseData.managerContacterID;
  addCrmPhoneContacterObj.managerContacterName = responseData.managerContacterName;
  addCrmPhoneContacterObj.managerContacterEmail = responseData.managerContacterEmail;
  addCrmPhoneContacterObj.managerContacterPhone = responseData.managerContacterPhone;
  addCrmPhoneContacterObj.managerContacterDepartment = responseData.managerContacterDepartment;
  addCrmPhoneContacterObj.managerContacterJobTitle = responseData.managerContacterJobTitle;
  addCrmPhoneContacterObj.managerContacterTelephone = responseData.managerContacterTelephone;
  addCrmPhoneContacterObj.managerContacterIsAgreeSurvey =
    responseData.managerContacterIsAgreeSurvey;
  addCrmPhoneContacterObj.managerContacterStatus = responseData.atomManagerContacterStatus;
  addCrmPhoneContacterObj.atomManagerContacterRatingKind =
    responseData.atomManagerContacterRatingKind;
  addCrmPhoneContacterObj.managerContacterRemark = responseData.managerContacterRemark;
};
//--------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 主要/次要窗口
  if (
    addCrmPhoneContacterObj.employeePipelineContacterIsPrimary === null ||
    addCrmPhoneContacterObj.employeePipelineContacterIsPrimary === undefined
  ) {
    addCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary = false;
    isValid = false;
  } else {
    addCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary = true;
  }

  // 電子信箱（managerContacterID 必選）
  if (!addCrmPhoneContacterObj.managerContacterID) {
    addCrmPhoneContacterValidObj.managerContacterID = false;
    isValid = false;
  } else {
    addCrmPhoneContacterValidObj.managerContacterID = true;
  }

  return isValid;
};
//--------------------------------------------------------------
/** 點擊送出按鈕 */
const clickSubmitBtn = async () => {
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateField()) return;

  // 呼叫api
  const requestData: MbsCrmPhnHttpAddEmployeePipelineContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineID: addCrmPhoneContacterObj.employeePipelineContacterID!,
    managerContacterID: addCrmPhoneContacterObj.managerContacterID!,
    employeePipelineContacterIsPrimary: addCrmPhoneContacterObj.employeePipelineContacterIsPrimary,
  };

  const responseData: MbsCrmPhnHttpAddEmployeePipelineContacterRspMdl | null =
    await addPhoneEmployeePipelineContacter(requestData);

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

//-------------------------------------------------------------
</script>

<template>
  <div class="modal-overlay">
    <div class="max-h-[90vh] w-[900px] max-w-[95vw] bg-white rounded-lg shadow-lg">
      <!-- 標題列 -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-lg font-bold text-gray-800">更正窗口</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          @click="$emit('close')"
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
          <!-- 窗口 -->
          <div>
            <label class="form-label">窗口 <span class="required-label">*</span></label>
            <select
              v-model="addCrmPhoneContacterObj.employeePipelineContacterIsPrimary"
              disabled
              class="select-box"
            >
              <option :value="true">主要窗口</option>
              <option :value="false">次要窗口</option>
            </select>
            <div class="error-wrapper">
              <span
                v-if="!addCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary"
                class="error-tip"
              >
                不可為空
              </span>
            </div>
          </div>

          <!--電子信箱-->
          <div class="flex-1">
            <label class="form-label">電子信箱 <span class="required-label">*</span></label>
            <GetManyEmailComboBox
              v-model:managerContacterID="addCrmPhoneContacterObj.managerContacterID"
              v-model:managerContacterName="addCrmPhoneContacterObj.managerContacterName"
              :managerContacterCompanyID="props.managerContacterCompanyID"
              @update:manager-contacter-i-d="getPhoneEmployeePipelineContacterData"
            />
            <div class="error-wrapper">
              <span v-if="!addCrmPhoneContacterValidObj.managerContacterID" class="error-tip">
                不可為空
              </span>
            </div>
          </div>
        </div>

        <!-- 內容區:左右兩區分 -->
        <div class="flex gap-4 h-[280px]">
          <!-- 左邊：原始窗口 -->
          <div class="flex-1 w-1/2 bg-white rounded-lg border border-gray-300">
            <!-- 標題 -->
            <div class="bg-gray-100 p-1 rounded-t-lg">
              <h3 class="text-brand-201 font-semibold ml-2 text-sm p-1">原始窗口</h3>
            </div>

            <!-- 內容 -->
            <div class="p-4">
              <template v-if="props.originalContacter">
                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">姓名</span>
                  <span class="display-value break-words">{{
                    props.originalContacter.managerContacterName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電子信箱</span>
                  <span class="display-value break-words">{{
                    props.originalContacter.managerContacterEmail || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">手機</span>
                  <span class="display-value break-words">{{
                    props.originalContacter.managerContacterPhone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電話分機</span>
                  <span class="display-value break-words">{{
                    props.originalContacter.managerContacterTelephone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">部門</span>
                  <span class="display-value break-words">{{
                    props.originalContacter.managerContacterDepartment || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">職稱</span>
                  <span class="display-value break-words">{{
                    props.originalContacter.managerContacterJobTitle || "-"
                  }}</span>
                </p>
              </template>

              <template v-else>
                <div class="text-center text-gray-400 py-6">無原始窗口資料</div>
              </template>
            </div>
          </div>

          <!-- 右邊：確認窗口 -->
          <div class="flex-1 w-1/2 bg-white rounded-lg border border-gray-300">
            <!-- 標題 -->
            <div class="bg-gray-100 p-1 rounded-t-lg">
              <h3 class="text-brand-201 font-semibold ml-2 text-sm p-1">確認窗口</h3>
            </div>

            <!-- 內容 -->
            <div class="p-3 flex-1 overflow-y-auto h-[240px]">
              <template v-if="!addCrmPhoneContacterObj.managerContacterID">
                <div
                  class="flex flex-col items-center justify-center text-center text-gray-400 h-full"
                >
                  <div class="text-4xl mb-2">!</div>
                  <p class="mb-4 text-sm">請先查詢窗口資料</p>
                </div>
              </template>

              <template v-else>
                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">姓名</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterName || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電子信箱</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterEmail || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">手機</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterPhone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">部門</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterDepartment || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">職稱</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterJobTitle || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">電話分機</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterTelephone || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">狀態</span>
                  <span class="display-value break-words">{{
                    getManagerContacterStatusLabel(
                      addCrmPhoneContacterObj.managerContacterStatus
                    ) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">個資同意</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterIsAgreeSurvey ? "同意" : "不同意"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">開發評等</span>
                  <span class="display-value break-words">{{
                    getManagerContacterRatingLabel(
                      addCrmPhoneContacterObj.atomManagerContacterRatingKind
                    ) || "-"
                  }}</span>
                </p>
                <hr class="my-2" />

                <p class="flex flex-row gap-5">
                  <span class="display-label w-20 shrink-0">備註</span>
                  <span class="display-value break-words">{{
                    addCrmPhoneContacterObj.managerContacterRemark || "-"
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
        <button class="btn-cancel" @click="$emit('close')">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>
</template>
