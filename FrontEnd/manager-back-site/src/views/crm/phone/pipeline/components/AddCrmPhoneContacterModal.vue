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
interface ContacterItem {
  employeePipelineContacterID: number;
  managerContacterID: number;
  managerContacterName: string | null;
}

const props = defineProps<{
  employeePipelineContacterID: number;
  managerContacterCompanyID: number;
  contacterList: ContacterItem[];
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
  isDuplicateContacter: boolean;
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
  isDuplicateContacter: true,
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

  // 檢查窗口是否已存在於名單中
  const isDuplicate = props.contacterList.some(
    (contacter) => contacter.managerContacterID === addCrmPhoneContacterObj.managerContacterID
  );
  if (isDuplicate) {
    addCrmPhoneContacterValidObj.isDuplicateContacter = false;
    isValid = false;
  } else {
    addCrmPhoneContacterValidObj.isDuplicateContacter = true;
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
    <div
      class="max-w-xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">附加窗口</h2>
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
      <div class="p-8 flex-1">
        <div class="space-y-6">
          <!-- 錯誤提示 -->
          <p
            v-if="showError"
            class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded"
          >
            {{ errorMessage }}
          </p>

          <div class="flex gap-4 w-full">
            <!-- 窗口 -->
            <div class="flex flex-col gap-2 flex-0">
              <label class="form-label">窗口 <span class="required-label">*</span></label>
              <select
                v-model="addCrmPhoneContacterObj.employeePipelineContacterIsPrimary"
                class="select-box"
              >
                <option :value="true">主要窗口</option>
                <option :value="false">次要窗口</option>
              </select>
              <p class="error-wrapper">
                <span
                  v-if="!addCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary"
                  class="error-tip"
                >
                  不可為空
                </span>
              </p>
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
              <p class="error-wrapper">
                <span v-if="!addCrmPhoneContacterValidObj.managerContacterID" class="error-tip">
                  不可為空
                </span>
                <span
                  v-else-if="!addCrmPhoneContacterValidObj.isDuplicateContacter"
                  class="error-tip"
                >
                  此窗口已存在於名單中，請選擇其他窗口
                </span>
              </p>
            </div>
          </div>

          <!-- 詳細資訊表格 -->
          <div class="bg-gray-50 border border-gray-200 rounded-md p-6 text-sm text-gray-700 overflow-y-auto max-h-80">
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">姓名</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterName || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電子信箱</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterEmail || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">手機</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterPhone || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">部門</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterDepartment || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">職稱</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterJobTitle || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電話分機</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterTelephone || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">狀態</span>
              <span class="display-value truncate">
                {{
                  getManagerContacterStatusLabel(addCrmPhoneContacterObj.managerContacterStatus) ||
                  "-"
                }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">個資同意</span>
              <span class="display-value truncate">
                {{
                  addCrmPhoneContacterObj.managerContacterIsAgreeSurvey === null
                    ? "-"
                    : addCrmPhoneContacterObj.managerContacterIsAgreeSurvey
                      ? "是"
                      : "否"
                }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">開發評等</span>
              <span class="display-value truncate">
                {{
                  getManagerContacterRatingLabel(
                    addCrmPhoneContacterObj.atomManagerContacterRatingKind
                  ) || "-"
                }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">備註</span>
              <span class="display-value truncate">
                {{ addCrmPhoneContacterObj.managerContacterRemark || "-" }}
              </span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="$emit('close')">取消</button>
          <button class="btn-submit" @click="clickSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>
</template>
