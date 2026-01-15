<script setup lang="ts">
//#region 引入
import { reactive, ref, onMounted, defineAsyncComponent } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useRouter, useRoute } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import {
  getContacter,
  updateContacter,
  getManyContacterRating,
  addContacterRating,
  updateContacterRating,
  removeContacterRating,
} from "@/services";
import type {
  MbsSysCttHttpGetContacterReqMdl,
  MbsSysCttHttpGetContacterRspMdl,
  MbsSysCttHttpUpdateContacterReqMdl,
  MbsSysCttHttpUpdateContacterRspMdl,
  MbsSysCttHttpGetManyContacterRatingReqMdl,
  MbsSysCttHttpAddContacterRatingReqMdl,
  MbsSysCttHttpAddContacterRatingRspMdl,
  MbsSysCttHttpUpdateContacterRatingReqMdl,
  MbsSysCttHttpUpdateContacterRatingRspMdl,
  MbsSysCttHttpGetManyContacterRatingRspMdl,
  MbsSysCttHttpRemoveContacterRatingReqMdl,
  MbsSysCttHttpRemoveContacterRatingRspMdl,
} from "@/services/pms-http/system/contacter/systemContacterHttpFormat";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import TextCounter from "@/components/global/counter/TextCounter.vue";
const GetManyManagerCompanyComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue")
);
const GetManyManagerContacterRatingReasonComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerContacterRatingReasonComboBox.vue")
);
//#endregion

//#region 外部依賴
/** 員工資訊儲存 */
const employeeInfoStore = useEmployeeInfoStore();
/** 令牌儲存 */
const tokenStore = useTokenStore();
/** token驗證相關 */
const { requireToken } = useAuth();
/** 錯誤訊息相關 */
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
/** 成功訊息相關 */
const { successMessage, showSuccess, closeSuccess, displaySuccess } = useSuccessHandler();
/** 路由操作物件（用於頁面跳轉） */
const router = useRouter();
/** 路由參數物件（用於取得路由參數） */
const route = useRoute();
//#endregion

//#region 型別定義
/** 系統設定-窗口-檢視模型 */
interface SysContacterDetailViewMdl {
  managerContacterID: number;
  managerCompanyID: number;
  managerCompanyName: string;
  managerContacterName: string;
  managerContacterEmail: string;
  managerContacterPhone: string;
  managerContacterDepartment: string;
  managerContacterJobTitle: string;
  managerContacterTelephone: string;
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  managerContacterIsAgreeSurvey: boolean;
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
  managerContacterRemark: string | null;
}

/** 系統設定-窗口-檢視-驗證模型 */
interface SysContacterDetailValidateMdl {
  managerCompanyID: boolean;
  managerContacterName: boolean;
  managerContacterEmail: boolean;
  managerContacterPhone: boolean;
  managerContacterDepartment: boolean;
  managerContacterJobTitle: boolean;
  managerContacterTelephone: boolean;
}

/** 系統設定-窗口-開發評等-檢視模型 */
interface SysCttRatingDetailViewMdl {
  managerContacterRatingID: number;
  managerContacterRatingReasonName: string;
  managerContacterRatingRemark: string | null;
}

/** 系統設定-窗口-開發評等-列表-項目 */
interface SysCttRatingListItemMdl {
  managerContacterRatingID: number;
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName: string;
  managerContacterRatingRemark: string;
}

/** 系統設定-窗口-開發評等原因-驗證模型 */
interface SysCttRatingReasonValidateMdl {
  managerContacterRatingReasonName: boolean;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemContacter;
/** 窗口ID(從路由參數取得) */
const managerContacterID = Number(route.params.id);
/** 是否為編輯模式 */
const isEditMode = ref(false);
/** 是否為編輯開發評等模式 */
const isEditRatingMode = ref(false);
/** 開發評等清單 */
const contacterRatingReasonList = ref<SysCttRatingDetailViewMdl[]>([]);
/** 是否顯示【新增開發評等原因】Modal */
const sysCttRatingReasonModalAddShow = ref(false);
/** 是否顯示【編輯開發評等原因】Modal */
const sysCttRatingReasonModalUpdateShow = ref(false);
/** 是否顯示【客戶更新邏輯說明】Modal */
const contacterCompanyInfoModalShow = ref(false);

/** 系統設定-窗口-檢視物件 */
const sysContacterDetailViewObj = reactive<SysContacterDetailViewMdl>({
  managerContacterID: 0,
  managerCompanyID: 0,
  managerCompanyName: "",
  managerContacterName: "",
  managerContacterEmail: "",
  managerContacterPhone: "",
  managerContacterDepartment: "",
  managerContacterJobTitle: "",
  managerContacterTelephone: "",
  managerContacterStatus: DbAtomManagerContacterStatusEnum.Unknown,
  managerContacterIsAgreeSurvey: false,
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum.Whitelist,
  managerContacterRemark: "",
});

/** 系統設定-窗口-原始物件 */
const sysContacterOriginalObj = reactive<Partial<SysContacterDetailViewMdl>>({});

/** 系統設定-窗口-驗證物件 */
const sysContacterAddValidObj = reactive<SysContacterDetailValidateMdl>({
  managerCompanyID: true,
  managerContacterName: true,
  managerContacterEmail: true,
  managerContacterPhone: true,
  managerContacterDepartment: true,
  managerContacterJobTitle: true,
  managerContacterTelephone: true,
});

/** 系統設定-窗口-開發評等-列表-項目 */
const sysCttRatingListItemObj = reactive<SysCttRatingListItemMdl>({
  managerContacterRatingID: 0,
  managerContacterRatingReasonID: 0,
  managerContacterRatingReasonName: "",
  managerContacterRatingRemark: "",
});

/**  系統設定-窗口-開發評等原因-驗證物件 */
const sysCttRatingReasonValidObj = reactive<SysCttRatingReasonValidateMdl>({
  managerContacterRatingReasonName: true,
});
//#endregion

//#region 驗證相關
/** 驗證【窗口】欄位 */
const validateContacterField = () => {
  let isValid = true;

  // 公司
  if (!sysContacterDetailViewObj.managerCompanyID) {
    sysContacterAddValidObj.managerCompanyID = false;
    isValid = false;
  } else sysContacterAddValidObj.managerCompanyID = true;

  // 姓名
  const name = sysContacterDetailViewObj.managerContacterName.trim();
  sysContacterAddValidObj.managerContacterName = !!name && name.length <= 30;
  if (!sysContacterAddValidObj.managerContacterName) isValid = false;

  // Email
  const email = sysContacterDetailViewObj.managerContacterEmail.trim();
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  sysContacterAddValidObj.managerContacterEmail =
    !!email && email.length <= 50 && emailPattern.test(email);
  if (!sysContacterAddValidObj.managerContacterEmail) isValid = false;

  // 手機（可空）
  const phone = sysContacterDetailViewObj.managerContacterPhone.trim();
  const phonePattern = /^09\d{2}-\d{3}-\d{3}$/;
  sysContacterAddValidObj.managerContacterPhone = !phone || phonePattern.test(phone);
  if (!sysContacterAddValidObj.managerContacterPhone) isValid = false;

  // 部門
  const dept = sysContacterDetailViewObj.managerContacterDepartment.trim();
  sysContacterAddValidObj.managerContacterDepartment = dept.length <= 10;
  if (!sysContacterAddValidObj.managerContacterDepartment) isValid = false;

  // 職稱
  const job = sysContacterDetailViewObj.managerContacterJobTitle.trim();
  sysContacterAddValidObj.managerContacterJobTitle = job.length <= 10;
  if (!sysContacterAddValidObj.managerContacterJobTitle) isValid = false;

  // 電話分機
  const tel = sysContacterDetailViewObj.managerContacterTelephone.trim();
  const telPattern = /^0[2-9]-\d{3,4}-\d{3,4}(#.*)?$/;
  sysContacterAddValidObj.managerContacterTelephone = !tel || telPattern.test(tel);
  if (!sysContacterAddValidObj.managerContacterTelephone) isValid = false;

  return isValid;
};

/** 重設【窗口】驗證狀態 */
const resetContacterValidation = () => {
  Object.assign(sysContacterAddValidObj, {
    managerCompanyID: true,
    managerContacterName: true,
    managerContacterEmail: true,
    managerContacterPhone: true,
    managerContacterDepartment: true,
    managerContacterJobTitle: true,
    managerContacterTelephone: true,
  });
};

/** 驗證【開發評等】欄位 */
const validateContacterRatingField = () => {
  let isValid = true;

  // 開發評等原因
  if (
    !sysCttRatingListItemObj.managerContacterRatingReasonID ||
    sysCttRatingListItemObj.managerContacterRatingReasonID <= 0
  ) {
    sysCttRatingReasonValidObj.managerContacterRatingReasonName = false;
    isValid = false;
  } else {
    sysCttRatingReasonValidObj.managerContacterRatingReasonName = true;
  }
  return isValid;
};

/** 重設【開發評等原因】驗證狀態 */
const resetContacterRatingValidation = () => {
  Object.assign(sysCttRatingReasonValidObj, {
    managerContacterRatingReasonName: true,
  });
};
//#endregion

//#region API / 資料流程
/** 取得【窗口】資料 */
const getContacterData = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 呼叫api
  const requestData: MbsSysCttHttpGetContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID,
  };
  const responseData: MbsSysCttHttpGetContacterRspMdl | null = await getContacter(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 資料帶入頁面物件
  sysContacterDetailViewObj.managerContacterID = responseData.managerContacterID;
  sysContacterDetailViewObj.managerCompanyID = responseData.managerCompanyID;
  sysContacterDetailViewObj.managerContacterName = responseData.managerContacterName;
  sysContacterDetailViewObj.managerContacterEmail = responseData.managerContacterEmail;
  sysContacterDetailViewObj.managerContacterPhone = responseData.managerContacterPhone;
  sysContacterDetailViewObj.managerContacterDepartment = responseData.managerContacterDepartment;
  sysContacterDetailViewObj.managerContacterJobTitle = responseData.managerContacterTitle;
  sysContacterDetailViewObj.managerContacterTelephone = responseData.managerContacterTelephone;
  sysContacterDetailViewObj.managerContacterStatus = responseData.atomManagerContacterStatus;
  sysContacterDetailViewObj.managerContacterIsAgreeSurvey =
    responseData.managerContacterIsAgreeSurvey;
  sysContacterDetailViewObj.atomManagerContacterRatingKind =
    responseData.atomManagerContacterRatingKind;
  sysContacterDetailViewObj.managerContacterRemark = responseData.managerContacterRemark;
  sysContacterDetailViewObj.managerCompanyName = responseData.managerCompanyName;

  // 取得該窗口的開發評等清單
  getCtrRatingList();

  // 儲存原始資料供
  Object.assign(sysContacterOriginalObj, {
    managerContacterName: sysContacterDetailViewObj.managerContacterName,
    managerCompanyID: sysContacterDetailViewObj.managerCompanyID,
    managerContacterEmail: sysContacterDetailViewObj.managerContacterEmail,
    managerContacterPhone: sysContacterDetailViewObj.managerContacterPhone,
    managerContacterDepartment: sysContacterDetailViewObj.managerContacterDepartment,
    managerContacterJobTitle: sysContacterDetailViewObj.managerContacterJobTitle,
    managerContacterTelephone: sysContacterDetailViewObj.managerContacterTelephone,
    managerContacterStatus: sysContacterDetailViewObj.managerContacterStatus,
    managerContacterIsAgreeSurvey: sysContacterDetailViewObj.managerContacterIsAgreeSurvey,
    atomManagerContacterRatingKind: sysContacterDetailViewObj.atomManagerContacterRatingKind,
  });
};

/** 取得【開發評等】列表 */
const getCtrRatingList = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 呼叫 api
  const requestData: MbsSysCttHttpGetManyContacterRatingReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingReasonID: managerContacterID,
  };
  const responseData: MbsSysCttHttpGetManyContacterRatingRspMdl | null =
    await getManyContacterRating(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 資料帶入頁面物件
  contacterRatingReasonList.value = responseData.managerContacterRatingList.map((r) => ({
    managerContacterRatingID: r.managerContacterRatingID,
    managerContacterRatingReasonName: r.managerContacterRatingReasonName,
    managerContacterRatingRemark: r.managerContacterRatingRemark,
  }));
};

/** 取得【窗口】變更欄位 */
const getChangedFields = (): Partial<MbsSysCttHttpUpdateContacterReqMdl> => {
  const changes: Partial<MbsSysCttHttpUpdateContacterReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID: managerContacterID,
  };

  if (
    sysContacterDetailViewObj.managerContacterName !== sysContacterOriginalObj.managerContacterName
  ) {
    changes.managerContacterName = sysContacterDetailViewObj.managerContacterName;
  }
  if (
    sysContacterDetailViewObj.managerContacterEmail !==
    sysContacterOriginalObj.managerContacterEmail
  ) {
    changes.managerContacterEmail = sysContacterDetailViewObj.managerContacterEmail;
  }
  if (
    sysContacterDetailViewObj.managerContacterPhone !==
    sysContacterOriginalObj.managerContacterPhone
  ) {
    changes.managerContacterPhone = sysContacterDetailViewObj.managerContacterPhone;
  }
  if (
    sysContacterDetailViewObj.managerContacterDepartment !==
    sysContacterOriginalObj.managerContacterDepartment
  ) {
    changes.managerContacterDepartment = sysContacterDetailViewObj.managerContacterDepartment;
  }
  if (
    sysContacterDetailViewObj.managerContacterJobTitle !==
    sysContacterOriginalObj.managerContacterJobTitle
  ) {
    changes.managerContacterTitle = sysContacterDetailViewObj.managerContacterJobTitle;
  }
  if (
    sysContacterDetailViewObj.managerContacterTelephone !==
    sysContacterOriginalObj.managerContacterTelephone
  ) {
    changes.managerContacterTel = sysContacterDetailViewObj.managerContacterTelephone;
  }
  if (
    sysContacterDetailViewObj.managerContacterStatus !==
    sysContacterOriginalObj.managerContacterStatus
  ) {
    changes.atomManagerContacterStatus = sysContacterDetailViewObj.managerContacterStatus;
  }
  if (
    sysContacterDetailViewObj.managerContacterIsAgreeSurvey !==
    sysContacterOriginalObj.managerContacterIsAgreeSurvey
  ) {
    changes.managerContacterIsAgreeSurvey = sysContacterDetailViewObj.managerContacterIsAgreeSurvey;
  }
  if (
    sysContacterDetailViewObj.atomManagerContacterRatingKind !==
    sysContacterOriginalObj.atomManagerContacterRatingKind
  ) {
    changes.atomManagerContacterRatingKind =
      sysContacterDetailViewObj.atomManagerContacterRatingKind;
  }

  if (
    sysContacterDetailViewObj.managerContacterRemark !==
    sysContacterOriginalObj.managerContacterRemark
  ) {
    changes.managerContacterRemark = sysContacterDetailViewObj.managerContacterRemark;
  }

  if (sysContacterDetailViewObj.managerCompanyID !== sysContacterOriginalObj.managerCompanyID) {
    changes.managerCompanyID = sysContacterDetailViewObj.managerCompanyID;
  }

  return changes;
};
//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/contacter");
};

/** 點擊取消編輯【窗口】按鈕 */
const clickCancelContacterBtn = () => {
  isEditMode.value = false;
  resetContacterValidation();
  getContacterData();
};

/** 點擊提交【窗口】按鈕 */
const clickSubmitContacterBtn = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateContacterField()) {
    return;
  }

  // 取得變更欄位
  const changedFields = getChangedFields();
  const hasChanges = Object.keys(changedFields).length > 2;
  if (!hasChanges) {
    displayError("沒有任何變更需要儲存！");
    return;
  }

  // 呼叫 API
  const responseData: MbsSysCttHttpUpdateContacterRspMdl | null = await updateContacter(
    changedFields as MbsSysCttHttpUpdateContacterReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  isEditMode.value = false;

  // 顯示成功訊息
  displaySuccess("更新窗口資料成功！");

  // 重新取得資料
  await getContacterData();
};

/** 點擊提交【開發評等】按鈕 */
const clickSubmitRatingReasonBtn = async () => {
  if (!requireToken()) return;

  const requestData: Partial<MbsSysCttHttpUpdateContacterReqMdl> = {
    employeeLoginToken: tokenStore.token!,
    managerContacterID: managerContacterID,
    atomManagerContacterRatingKind: sysContacterDetailViewObj.atomManagerContacterRatingKind,
  };

  const responseData: MbsSysCttHttpUpdateContacterRspMdl | null = await updateContacter(
    requestData as MbsSysCttHttpUpdateContacterReqMdl
  );
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 編輯模式關閉
  isEditRatingMode.value = false;

  // 顯示成功訊息
  displaySuccess("更新開發評等成功！");

  // 重新取得資料
  await getContacterData();
};

/** 點擊打開【新增開發評等】Modal按鈕 */
const clickOpenAddCtrtacterRatingReasonBtn = () => {
  Object.assign(sysCttRatingListItemObj, {
    managerContacterRatingID: 0,
    managerContacterRatingReasonID: null,
    managerContacterRatingRemark: "",
  });
  resetContacterRatingValidation();
  sysCttRatingReasonModalAddShow.value = true;
};

/** 點擊提交【新增開發評等】按鈕 */
const clickSubmitAddContacterRatingReasonBtn = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateContacterRatingField()) {
    return;
  }

  // 呼叫 API
  const requestData: MbsSysCttHttpAddContacterRatingReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingID: managerContacterID,
    managerContacterRatingReasonID: sysCttRatingListItemObj.managerContacterRatingReasonID,
    managerContacterRatingRemark: sysCttRatingListItemObj.managerContacterRatingRemark,
  };
  const responseData: MbsSysCttHttpAddContacterRatingRspMdl | null =
    await addContacterRating(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉 Modal
  sysCttRatingReasonModalAddShow.value = false;

  // 顯示成功訊息
  displaySuccess("新增開發評等成功！");

  // 重新取得開發評等清單
  getCtrRatingList();
};

/** 點擊打開【編輯開發評等】Modal 按鈕 */
const clickUpdateContacterRatingReasonBtn = (managerContacterRatingID: number) => {
  // 找出該筆資料
  const target = contacterRatingReasonList.value.find(
    (item) => item.managerContacterRatingID === managerContacterRatingID
  );
  if (!target) return;

  // 把該筆資料複製進編輯用物件
  Object.assign(sysCttRatingListItemObj, target);
  sysCttRatingReasonModalUpdateShow.value = true;
};

/** 點擊提交【編輯開發評等】按鈕 */
const clickSubmitUpdateContacterRatingReasonBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateContacterRatingField()) return;

  // 呼叫 API
  const requestData: MbsSysCttHttpUpdateContacterRatingReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingID: sysCttRatingListItemObj.managerContacterRatingID,
    managerContacterRatingReasonID: sysCttRatingListItemObj.managerContacterRatingReasonID,
    managerContacterRatingRemark: sysCttRatingListItemObj.managerContacterRatingRemark,
  };

  const responseData: MbsSysCttHttpUpdateContacterRatingRspMdl | null =
    await updateContacterRating(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉 Modal
  sysCttRatingReasonModalUpdateShow.value = false;

  // 顯示成功訊息
  displaySuccess("更新開發評等原因成功！");

  // 重新取得開發評等清單
  await getCtrRatingList();
};

/** 點擊提交【移除開發評等】按鈕 */
const clickSubmitRemoveContacterRatingReasonBtn = async (managerContacterRatingID: number) => {
  if (!requireToken()) return;

  const requestData: MbsSysCttHttpRemoveContacterRatingReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerContacterRatingID: managerContacterRatingID,
  };

  const responseData: MbsSysCttHttpRemoveContacterRatingRspMdl | null =
    await removeContacterRating(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 關閉 Modal
  sysCttRatingReasonModalUpdateShow.value = false;

  // 顯示成功訊息
  displaySuccess("刪除開發評等成功！");

  // 重新取得開發評等清單
  await getCtrRatingList();
};

/** 點擊【編輯窗口】按鈕 */
const clickUpdateContacterBtn = () => {
  isEditMode.value = true;
};

/** 點擊【編輯開發評等】按鈕 */
const clickUpdateRatingReasonBtn = () => {
  isEditRatingMode.value = true;
};

/** 點擊取消編輯【開發評等】按鈕 */
const clickCancelRatingReasonBtn = () => {
  isEditRatingMode.value = false;
  resetContacterRatingValidation();
  getContacterData();
};

//#endregion

//#region 生命週期
onMounted(() => {
  getContacterData();
});
//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-4">
        <button class="btn-back flex items-center" @click="clickBackBtn">
          <svg
            class="w-4 h-4 inline-block mr-1"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M15 19l-7-7 7-7"
            />
          </svg>
          <span>返回</span>
        </button>
      </div>
    </div>

    <!-- 窗口資訊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between">
        <div class="subtitle">窗口資訊</div>
        <div class="flex gap-4 w-full justify-end">
          <!--編輯模式-->
          <template v-if="isEditMode">
            <div class="flex gap-2">
              <button class="btn-cancel" @click="clickCancelContacterBtn">取消編輯</button>
              <button class="btn-submit" @click="clickSubmitContacterBtn">儲存</button>
            </div>
          </template>
          <!--檢視模式-->
          <template v-else>
            <button
              v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
              class="btn-update"
              @click="clickUpdateContacterBtn"
            >
              編輯
            </button>
          </template>
        </div>
      </div>

      <!-- 客戶 -->
      <div class="w-1/3">
        <div class="flex items-center gap-2">
          <label class="form-label">客戶 <span class="required-label">*</span></label>
          <button
            class="text-gray-500 hover:text-brand-100 transition-colors"
            title="查看客戶更新邏輯說明"
            @click="contacterCompanyInfoModalShow = true"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-5 w-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
          </button>
        </div>
        <GetManyManagerCompanyComboBox
          v-model:managerCompanyID="sysContacterDetailViewObj.managerCompanyID"
          v-model:managerCompanyName="sysContacterDetailViewObj.managerCompanyName"
          :disabled="!isEditMode"
        />
        <div class="error-wrapper">
          <p v-if="!sysContacterAddValidObj.managerCompanyID" class="error-tip">不可為空</p>
        </div>
      </div>

      <hr />

      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 w-full">
        <!-- 姓名 -->
        <div>
          <label class="form-label">姓名 <span class="required-label">*</span></label>
          <input
            v-model="sysContacterDetailViewObj.managerContacterName"
            class="input-box"
            placeholder="請輸入姓名"
            :disabled="!isEditMode"
          />
          <div class="error-wrapper">
            <span v-if="!sysContacterAddValidObj.managerContacterName" class="error-tip">
              不可為空，長度不可超過 30 個字
            </span>
          </div>
        </div>

        <!-- 電子信箱 -->
        <div>
          <label class="form-label">電子信箱 <span class="required-label">*</span></label>
          <input
            v-model="sysContacterDetailViewObj.managerContacterEmail"
            class="input-box"
            placeholder="請輸入電子信箱"
            :disabled="!isEditMode"
          />
          <div class="error-wrapper">
            <span v-if="!sysContacterAddValidObj.managerContacterEmail" class="error-tip">
              不可為空，長度不可超過 50 個字，需符合格式
            </span>
          </div>
        </div>

        <!-- 手機 -->
        <div>
          <label class="form-label">手機</label>
          <input
            v-model="sysContacterDetailViewObj.managerContacterPhone"
            class="input-box"
            :disabled="!isEditMode"
            placeholder="例 : 0912-345-678"
          />
          <div class="error-wrapper">
            <span v-if="!sysContacterAddValidObj.managerContacterPhone" class="error-tip">
              格式不符，範例：0912-345-678
            </span>
          </div>
        </div>

        <!--部門-->
        <div>
          <label class="form-label">部門</label>
          <input
            v-model="sysContacterDetailViewObj.managerContacterDepartment"
            class="input-box"
            :disabled="!isEditMode"
            placeholder="請輸入部門"
          />
          <div class="error-wrapper">
            <span v-if="!sysContacterAddValidObj.managerContacterDepartment" class="error-tip">
              長度不可超過 10 個字
            </span>
          </div>
        </div>

        <!--職稱-->
        <div>
          <label class="form-label">職稱</label>
          <input
            v-model="sysContacterDetailViewObj.managerContacterJobTitle"
            class="input-box"
            :disabled="!isEditMode"
            placeholder="請輸入職稱"
          />
          <div class="error-wrapper">
            <span v-if="!sysContacterAddValidObj.managerContacterJobTitle" class="error-tip">
              長度不可超過 10 個字
            </span>
          </div>
        </div>

        <!--電話 #分機-->
        <div>
          <label class="form-label">電話 #分機</label>
          <input
            v-model="sysContacterDetailViewObj.managerContacterTelephone"
            class="input-box"
            :disabled="!isEditMode"
            placeholder="例 : 02-1234-5678#123"
          />
          <div class="error-wrapper">
            <span v-if="!sysContacterAddValidObj.managerContacterTelephone" class="error-tip">
              格式不符，範例：02-1234-5678 或 02-1234-5678#123
            </span>
          </div>
        </div>

        <!--狀態-->
        <div>
          <label class="form-label">狀態<span class="required-label">*</span></label>
          <select
            v-model="sysContacterDetailViewObj.managerContacterStatus"
            class="select-box"
            :disabled="!isEditMode"
          >
            <option :value="DbAtomManagerContacterStatusEnum.Unknown">不清楚</option>
            <option :value="DbAtomManagerContacterStatusEnum.Employed">在職</option>
            <option :value="DbAtomManagerContacterStatusEnum.Unemployed">離職</option>
          </select>
        </div>
        <!--問卷同意-->
        <div>
          <label class="form-label">問卷同意 <span class="required-label">*</span></label>
          <div class="flex gap-8">
            <label
              ><input
                v-model="sysContacterDetailViewObj.managerContacterIsAgreeSurvey"
                type="radio"
                :value="true"
                :disabled="!isEditMode"
              />
              同意</label
            >
            <label
              ><input
                v-model="sysContacterDetailViewObj.managerContacterIsAgreeSurvey"
                type="radio"
                :value="false"
                :disabled="!isEditMode"
              />
              不同意</label
            >
          </div>
        </div>
      </div>

      <!--備註-->
      <div class="flex flex-col gap-2">
        <label class="form-label">備註</label>
        <textarea
          v-model="sysContacterDetailViewObj.managerContacterRemark"
          rows="4"
          class="textarea-box resize-none"
          :disabled="!isEditMode"
          placeholder="請輸入備註"
        ></textarea>
      </div>
    </div>

    <!-- 開發評等區塊 -->
    <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div class="flex justify-between items-center">
        <div class="subtitle">開發評等</div>
        <div class="flex gap-4 w-full justify-end">
          <template v-if="isEditRatingMode">
            <div class="flex gap-2">
              <button class="btn-cancel" @click="clickCancelRatingReasonBtn">取消編輯</button>
              <button class="btn-submit" @click="clickSubmitRatingReasonBtn">儲存</button>
            </div>
          </template>
          <template v-else>
            <button
              v-if="employeeInfoStore.hasEveryonePermission(menu, 'edit')"
              class="btn-update"
              @click="clickUpdateRatingReasonBtn"
            >
              編輯
            </button>
          </template>
        </div>
      </div>

      <!--開發評等-->
      <div class="w-1/3">
        <label class="form-label">開發評等 <span class="required-label">*</span></label>
        <select
          v-model="sysContacterDetailViewObj.atomManagerContacterRatingKind"
          class="select-box"
          :disabled="!isEditRatingMode"
        >
          <option :value="DbAtomManagerContacterRatingKindEnum.Whitelist">白名單</option>
          <option :value="DbAtomManagerContacterRatingKindEnum.Graylist">灰名單</option>
          <option :value="DbAtomManagerContacterRatingKindEnum.Blacklist">黑名單</option>
        </select>
        <p
          v-if="
            (sysContacterDetailViewObj.atomManagerContacterRatingKind ===
              DbAtomManagerContacterRatingKindEnum.Blacklist ||
              sysContacterDetailViewObj.atomManagerContacterRatingKind ===
                DbAtomManagerContacterRatingKindEnum.Graylist) &&
            contacterRatingReasonList.length === 0
          "
          class="text-sm text-orange-600 mt-2"
        >
          ⚠️ 選擇黑名單或灰名單時，必須填寫至少一筆開發評等原因
        </p>
      </div>

      <hr />

      <!-- 開發評等列表 -->
      <div v-if="contacterRatingReasonList.length > 0">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start">開發評等原因</th>
              <th class="text-start">備註</th>
              <th
                v-if="!isEditRatingMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                class="text-center w-40"
              >
                操作
              </th>
            </tr>
          </thead>

          <tbody>
            <template v-if="contacterRatingReasonList.length === 0">
              <tr class="text-center">
                <td colspan="3">無資料</td>
              </tr>
            </template>

            <template v-else>
              <tr
                v-for="item in contacterRatingReasonList"
                :key="item.managerContacterRatingID"
                class="border-b hover:bg-gray-100"
              >
                <td class="text-start">{{ item.managerContacterRatingReasonName }}</td>
                <td class="text-start">{{ item.managerContacterRatingRemark || "-" }}</td>
                <td
                  v-if="!isEditRatingMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')"
                  class="text-center"
                >
                  <button
                    v-if="
                      !isEditRatingMode && employeeInfoStore.hasEveryonePermission(menu, 'edit')
                    "
                    class="btn-update"
                    @click="clickUpdateContacterRatingReasonBtn(item.managerContacterRatingID)"
                  >
                    編輯
                  </button>
                  <button
                    v-if="
                      !isEditRatingMode && employeeInfoStore.hasEveryonePermission(menu, 'delete')
                    "
                    class="btn-delete ml-2"
                    @click="
                      clickSubmitRemoveContacterRatingReasonBtn(item.managerContacterRatingID)
                    "
                  >
                    刪除
                  </button>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>

      <button
        v-if="!isEditRatingMode && employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="btn-add"
        @click="clickOpenAddCtrtacterRatingReasonBtn"
      >
        附加開發評等原因
      </button>
    </div>
  </div>

  <!--新增開發評等原因 Modal-->
  <div v-if="sysCttRatingReasonModalAddShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">附加開發評等原因</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysCttRatingReasonModalAddShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-6">
          <!-- 附加開發評等原因 -->
          <div>
            <label class="form-label">附加開發評等原因 <span class="required-label">*</span></label>
            <GetManyManagerContacterRatingReasonComboBox
              v-model:managerContacterRatingReasonID="
                sysCttRatingListItemObj.managerContacterRatingReasonID
              "
              :disabled="isEditMode"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysCttRatingReasonValidObj.managerContacterRatingReasonName"
                class="error-tip"
              >
                不可為空
              </span>
            </p>
          </div>

          <!-- 備註 -->
          <div>
            <label class="form-label">備註</label>
            <textarea
              v-model="sysCttRatingListItemObj.managerContacterRatingRemark"
              class="textarea-box h-40 resize-none"
              type="text"
              placeholder="請輸入備註"
            ></textarea>
            <div>
              <TextCounter
                :text="sysCttRatingListItemObj.managerContacterRatingRemark"
                :max-length="500"
                :required="true"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button
            class="btn-cancel"
            @click="
              sysCttRatingReasonModalAddShow = false;
              resetContacterRatingValidation();
            "
          >
            取消
          </button>
          <button
            v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'create')"
            class="btn-submit"
            @click="clickSubmitAddContacterRatingReasonBtn"
          >
            送出
          </button>
        </div>
      </div>
    </div>
  </div>

  <!--編輯開發評等原因 Modal-->
  <div v-if="sysCttRatingReasonModalUpdateShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">編輯開發評等原因</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysCttRatingReasonModalUpdateShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-6">
          <!-- 附加開發評等原因 -->
          <div>
            <label class="form-label">附加開發評等原因 <span class="required-label">*</span></label>
            <GetManyManagerContacterRatingReasonComboBox
              v-model:managerContacterRatingReasonID="
                sysCttRatingListItemObj.managerContacterRatingReasonID
              "
              v-model:managerContacterRatingReasonName="
                sysCttRatingListItemObj.managerContacterRatingReasonName
              "
              :disabled="isEditMode"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysCttRatingReasonValidObj.managerContacterRatingReasonName"
                class="error-tip"
              >
                不可為空
              </span>
            </p>
          </div>

          <!-- 備註 -->
          <div>
            <label class="form-label">備註</label>
            <textarea
              v-model="sysCttRatingListItemObj.managerContacterRatingRemark"
              class="textarea-box h-40 resize-none"
              placeholder="請輸入備註"
            ></textarea>
            <div>
              <TextCounter
                :text="sysCttRatingListItemObj.managerContacterRatingRemark"
                :max-length="500"
                :required="true"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button
            class="btn-cancel"
            @click="
              sysCttRatingReasonModalUpdateShow = false;
              resetContacterRatingValidation();
            "
          >
            取消
          </button>
          <button
            v-if="!isEditMode && employeeInfoStore.hasEveryonePermission(menu, 'create')"
            class="btn-submit"
            @click="clickSubmitUpdateContacterRatingReasonBtn"
          >
            送出
          </button>
        </div>
      </div>
    </div>
  </div>
  <!--客戶更新邏輯說明 Modal-->
  <div v-if="contacterCompanyInfoModalShow" class="modal-overlay">
    <div
      class="max-w-lg w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">客戶更新說明</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="contacterCompanyInfoModalShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 -->
      <div class="p-8">
        <p class="text-gray-700">
          更新窗口時若更改客戶，系統會產生新的窗口資料，並且將舊的窗口狀態變更為已離職。
        </p>
      </div>

      <!-- 按鈕區域 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end">
          <button class="btn-cancel" @click="contacterCompanyInfoModalShow = false">關閉</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
