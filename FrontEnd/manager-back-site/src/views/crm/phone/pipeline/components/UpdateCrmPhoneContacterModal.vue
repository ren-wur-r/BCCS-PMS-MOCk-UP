<script setup lang="ts">
import { watch, reactive } from "vue";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useAuth } from "@/composables/useAuth";
import {
  MbsCrmPhnHttpUpdateEmployeePipelineContacterReqMdl,
  MbsCrmPhnHttpUpdateEmployeePipelineContacterRspMdl,
} from "@/services/pms-http/crm/phone/crmPhoneHttpFormat";
import { getBasicManagerContacter, updatePhoneEmployeePipelineContacter } from "@/services";
import GetManyEmailComboBox from "@/components/feature/search-bar/GetManyEmailComboBox.vue";
import { MbsSysCttHttpGetManagerContacterReqMdl } from "@/services/pms-http/basic/basicHttpFormat";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { getManagerContacterStatusLabel } from "@/utils/getManagerContacterStatusLabel";
import { getManagerContacterRatingLabel } from "@/utils/getManagerContacterRatingLabel";

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
  employeePipelineContacterID: number; //商機窗口ID
  managerContacterCompanyID: number;
  managerContacterID: number;
  employeePipelineContacterIsPrimary: boolean;
  contacterList: ContacterItem[];
}>();

console.log("props資料", props.managerContacterID);
const emit = defineEmits<{
  (e: "close"): void;
  (e: "submit"): void;
}>();
//-------------------------------------------------------------
/** 更新-電銷-窗口-模型 */
interface UpdateCrmPhoneContacterMdl {
  employeePipelineContacterID: number | null;
  employeePipelineContacterIsPrimary: boolean;
  managerContacterID: number;
  managerContacterName: string | null;
  managerContacterEmail: string | null;
  managerContacterPhone: string | null;
  managerContacterDepartment: string | null;
  managerContacterJobTitle: string | null;
  managerContacterTelephone: string | null;
  managerContacterIsAgreeSurvey: boolean;
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum | null;
  managerContacterRemark: string | null;
}

/** 更新-電銷-窗口-驗證模型 */
interface UpdateCrmPhoneContacterValidMdl {
  employeePipelineContacterIsPrimary: boolean;
  managerContacterID: boolean;
  isDuplicateContacter: boolean;
}

/** 更新-電銷-窗口-原始模型 */
const updateCrmPhoneContacterOriginalObj = reactive({
  employeePipelineContacterID: props.employeePipelineContacterID,
  managerContacterID: props.managerContacterID,
  employeePipelineContacterIsPrimary: props.employeePipelineContacterIsPrimary,
});

/** 更新-電銷-窗口-物件 */
const updateCrmPhoneContacterObj = reactive<UpdateCrmPhoneContacterMdl>({
  employeePipelineContacterID: props.employeePipelineContacterID,
  managerContacterID: props.managerContacterID,
  employeePipelineContacterIsPrimary: props.employeePipelineContacterIsPrimary,
  managerContacterName: null,
  managerContacterEmail: null,
  managerContacterPhone: null,
  managerContacterDepartment: null,
  managerContacterJobTitle: null,
  managerContacterTelephone: null,
  managerContacterIsAgreeSurvey: false,
  managerContacterStatus: DbAtomManagerContacterStatusEnum.Unknown,
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum.NotSelected,
  managerContacterRemark: null,
});

/**  更新-電銷-窗口-驗證物件 */
const updateCrmPhoneContacterValidObj = reactive<UpdateCrmPhoneContacterValidMdl>({
  employeePipelineContacterIsPrimary: true,
  managerContacterID: true,
  isDuplicateContacter: true,
});

//-------------------------------------------------------------
/** 取得【單筆窗口】資料 */
const getPhoneEmployeePipelineContacterData = async () => {
  if (!requireToken()) return;

  const requestData: MbsSysCttHttpGetManagerContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID: updateCrmPhoneContacterObj.managerContacterID,
  };

  if (!updateCrmPhoneContacterObj.managerContacterID) {
    return;
  }

  const responseData = await getBasicManagerContacter(requestData);
  if (!responseData) {
    showError.value = true;
    errorMessage.value = "系統錯誤，請稍後再試";
    return;
  }

  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  updateCrmPhoneContacterObj.managerContacterID = responseData.managerContacterID;
  updateCrmPhoneContacterObj.managerContacterName = responseData.managerContacterName;
  updateCrmPhoneContacterObj.managerContacterEmail = responseData.managerContacterEmail;
  updateCrmPhoneContacterObj.managerContacterPhone = responseData.managerContacterPhone;
  updateCrmPhoneContacterObj.managerContacterDepartment = responseData.managerContacterDepartment;
  updateCrmPhoneContacterObj.managerContacterJobTitle = responseData.managerContacterJobTitle;
  updateCrmPhoneContacterObj.managerContacterTelephone = responseData.managerContacterTelephone;
  updateCrmPhoneContacterObj.managerContacterIsAgreeSurvey =
    responseData.managerContacterIsAgreeSurvey;
  updateCrmPhoneContacterObj.managerContacterStatus = responseData.atomManagerContacterStatus;
  updateCrmPhoneContacterObj.atomManagerContacterRatingKind =
    responseData.atomManagerContacterRatingKind;
  updateCrmPhoneContacterObj.managerContacterRemark = responseData.managerContacterRemark;
};
//------------------------------------------------------------
/** 取得變更的欄位 */
const getChangedFields = () => {
  const changes: Partial<MbsCrmPhnHttpUpdateEmployeePipelineContacterReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    employeePipelineContacterID: props.employeePipelineContacterID!,
    managerContacterID:props.managerContacterID,
  };

  // 【窗口類型：主要/次要】
  if (
    updateCrmPhoneContacterObj.employeePipelineContacterIsPrimary !==
    updateCrmPhoneContacterOriginalObj.employeePipelineContacterIsPrimary
  ) {
    changes.employeePipelineContacterIsPrimary =
      updateCrmPhoneContacterObj.employeePipelineContacterIsPrimary;
  }

  // 【電子信箱(managerContacterID)】
  if (
    updateCrmPhoneContacterObj.managerContacterID !==
    updateCrmPhoneContacterOriginalObj.managerContacterID
  ) {
    changes.managerContacterID = updateCrmPhoneContacterObj.managerContacterID;
  }

  return changes;
};
//------------------------------------------------------------
/** 驗證欄位 */
const validateField = () => {
  let isValid = true;

  // 窗口（主要/次要）
  if (
    updateCrmPhoneContacterObj.employeePipelineContacterIsPrimary === null ||
    updateCrmPhoneContacterObj.employeePipelineContacterIsPrimary === undefined
  ) {
    updateCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary = false;
    isValid = false;
  } else {
    updateCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary = true;
  }

  // 電子信箱（managerContacterID）
  if (!updateCrmPhoneContacterObj.managerContacterID) {
    updateCrmPhoneContacterValidObj.managerContacterID = false;
    isValid = false;
  } else {
    updateCrmPhoneContacterValidObj.managerContacterID = true;
  }

  // 檢查窗口是否已存在於名單中（排除自己）
  const isDuplicate = props.contacterList.some(
    (contacter) =>
      contacter.managerContacterID === updateCrmPhoneContacterObj.managerContacterID &&
      contacter.employeePipelineContacterID !== props.employeePipelineContacterID
  );
  if (isDuplicate) {
    updateCrmPhoneContacterValidObj.isDuplicateContacter = false;
    isValid = false;
  } else {
    updateCrmPhoneContacterValidObj.isDuplicateContacter = true;
  }

  return isValid;
};
//-------------------------------------------------------------
/** 點擊【送出】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;
  // 欄位驗證
  if (!validateField()) return;

  // 取得變更的欄位
  const changedFileds = getChangedFields();

  const hasChanges = Object.keys(changedFileds).length > 2; //除了token和ID外有變更欄位
  if (!hasChanges) {
    showError.value = true;
    errorMessage.value = "沒有任何變更需要儲存 !";
    return;
  }

  const responseData: MbsCrmPhnHttpUpdateEmployeePipelineContacterRspMdl | null =
    await updatePhoneEmployeePipelineContacter(
      changedFileds as MbsCrmPhnHttpUpdateEmployeePipelineContacterReqMdl
    );

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
watch(
  () => props.managerContacterID,
  (newVal) => {
    updateCrmPhoneContacterObj.managerContacterID = newVal;

    if (newVal) {
      getPhoneEmployeePipelineContacterData();
    }
  },
  { immediate: true }
);
</script>

<template>
  <div class="modal-overlay">
    <div
      class="max-w-xl w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯窗口</h2>
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
                v-model="updateCrmPhoneContacterObj.employeePipelineContacterIsPrimary"
                class="select-box"
              >
                <option :value="true">主要窗口</option>
                <option :value="false">次要窗口</option>
              </select>
              <p class="error-wrapper">
                <span
                  v-if="!updateCrmPhoneContacterValidObj.employeePipelineContacterIsPrimary"
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
                v-model:managerContacterID="updateCrmPhoneContacterObj.managerContacterID"
                v-model:managerContacterName="updateCrmPhoneContacterObj.managerContacterName"
                v-model:managerContacterEmail="updateCrmPhoneContacterObj.managerContacterEmail"
                :managerContacterCompanyID="props.managerContacterCompanyID"
                @update:manager-contacter-id="getPhoneEmployeePipelineContacterData"
              />
              <p class="error-wrapper">
                <span v-if="!updateCrmPhoneContacterValidObj.managerContacterID" class="error-tip">
                  不可為空
                </span>
                <span
                  v-else-if="!updateCrmPhoneContacterValidObj.isDuplicateContacter"
                  class="error-tip"
                >
                  此窗口已存在，請選擇其他窗口
                </span>
              </p>
            </div>
          </div>

          <!--內容顯示-->
          <div class="bg-gray-50 border border-gray-200 rounded-md p-6 text-sm text-gray-700  overflow-y-auto max-h-80">
            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">姓名</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterName || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電子信箱</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterEmail || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">手機</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterPhone || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">部門</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterDepartment || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">職稱</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterJobTitle || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">電話#分機</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterTelephone || "-" }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">狀態</span>
              <span class="display-value truncate">
                {{
                  getManagerContacterStatusLabel(
                    updateCrmPhoneContacterObj.managerContacterStatus
                  ) || "-"
                }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">個資同意</span>
              <span class="display-value truncate">
                {{
                  updateCrmPhoneContacterObj.managerContacterIsAgreeSurvey === null
                    ? "-"
                    : updateCrmPhoneContacterObj.managerContacterIsAgreeSurvey
                      ? "同意"
                      : "不同意"
                }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">開發評等</span>
              <span class="display-value truncate">
                {{
                  getManagerContacterRatingLabel(
                    updateCrmPhoneContacterObj.atomManagerContacterRatingKind
                  ) || "-"
                }}
              </span>
            </p>
            <hr class="my-2" />

            <p class="flex flex-row gap-5">
              <span class="display-label w-20 shrink-0">備註</span>
              <span class="display-value truncate">
                {{ updateCrmPhoneContacterObj.managerContacterRemark || "-" }}
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
