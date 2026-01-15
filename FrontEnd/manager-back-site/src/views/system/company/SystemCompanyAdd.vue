<script setup lang="ts">
//#region 引入
import { reactive, defineAsyncComponent, ref } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useRouter } from "vue-router";
import { DbAtomManagerCompanyStatusEnum } from "@/constants/DbAtomManagerCompanyStatusEnum";
import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";
import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { DbAtomEmployeeRangeEnum } from "@/constants/DbAtomEmployeeRangeEnum";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { useAuth } from "@/composables/useAuth";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import type {
  MbsSysComHttpAddCompanyReqMdl,
  MbsSysComHttpAddCompanyRspMdl,
  MbsSysComHttpAddCompanyLocationReqMdl,
  MbsSysComHttpAddCompanyLocationReqItemMdl,
  MbsSysComHttpAddCompanyAffiliateReqMdl,
  MbsSysComHttpAddCompanyAffiliateReqItemMdl,
} from "@/services/pms-http/system/company/systemCompanyHttpFormat";
import { addCompany } from "@/services";
import { getManagerCompanyStatusLabel } from "@/utils/getManagerCompanyStatusLabel";
import { getCityLabel } from "@/utils/getCityLabel";
const GetManyManagerCompanyMainKindCombox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyMainKindComboBox.vue")
);
const GetManyManagerCompanySubKindCombox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanySubKindComboBox.vue")
);
const GetManyCityComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyCityComboBox.vue")
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
//#endregion

//#region 型別定義
/** 系統設定-客戶-新增-頁面模型 */
interface SysCompanyAddViewMdl {
  managerCompanyID: number;
  managerCompanyUnifiedNumber: string;
  managerCompanyName: string;
  managerCompanyMainKindID: number;
  managerCompanyMainKindName: string;
  managerCompanySubKindID: number;
  managerCompanySubKindName: string;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  atomCustomerGrade: DbAtomCustomerGradeEnum | null;
  atomSecurityGrade: DbAtomSecurityGradeEnum | null;
  managerCompanyAffiliateList: SysComAffiliateAddViewMdl[];
  managerCompanyLocationList: SysComLocationAddViewMdl[];
}

/** 系統設定-客戶-驗證模型 */
interface SysCompanyValidMdl {
  managerCompanyUnifiedNumber: boolean;
  managerCompanyName: boolean;
  managerCompanyMainKindID: boolean;
  managerCompanySubKindID: boolean;
  atomManagerCompanyStatus: boolean;
}
//--------------------------------------------
/** 系統設定-客戶-營業地點-新增-頁面模型 */
interface SysComLocationAddViewMdl {
  managerCompanyLocationName: string;
  atomCity: DbAtomCityEnum;
  managerCompanyLocationAddress: string;
  managerCompanyLocationTelephone: string | null;
  atomEmployeeRange: DbAtomEmployeeRangeEnum | null;
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum | null;
}

/** 系統設定-客戶-營業地點-驗證模型 */
interface SysComLocationValidMdl {
  managerCompanyLocationName: boolean;
  managerCompanyLocationTelephone: boolean;
  atomCity: boolean;
  managerCompanyLocationAddress: boolean;
  atomManagerCompanyStatus: boolean;
}
//--------------------------------------------------
/** 系統設定-客戶-公司關係-新增-頁面模型 */
interface SysComAffiliateAddViewMdl {
  managerCompanyAffiliateUnifiedNumber: string;
  managerCompanyAffiliateName: string;
}

/** 系統設定-客戶-公司關係-驗證模型 */
interface SysComAffiliateValidMdl {
  managerCompanyAffiliateUnifiedNumber: boolean;
  managerCompanyAffiliateName: boolean;
}

//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemCompany;
/** 系統設定-客戶-營業地點-新增-Modal-顯示 */
const sysComLocationAddModalShow = ref(false);
/** 暫存營業地點清單 */
const managerCompanyLocationList = ref<SysComLocationAddViewMdl[]>([]);
/** 暫存目前編輯的營業地點索引 */
const managerCompanyLocationIndex = ref<number | null>(null);
/** 系統設定-客戶-關係公司-新增-Modal-顯示 */
const sysComAffiliateAddModalShow = ref(false);
/** 暫存關係公司清單 */
const managerCompanyAffiliateList = ref<SysComAffiliateAddViewMdl[]>([]);
/** 暫存目前編輯的關係公司索引 */
const managerCompanyAffiliateIndex = ref<number | null>(null);

/** 系統設定-客戶-新增-頁面物件 */
const sysCompanyAddViewObj = reactive<SysCompanyAddViewMdl>({
  managerCompanyID: 0,
  managerCompanyUnifiedNumber: "",
  managerCompanyName: "",
  atomManagerCompanyStatus: DbAtomManagerCompanyStatusEnum.NotSelected,
  managerCompanyMainKindID: 0,
  managerCompanyMainKindName: "",
  managerCompanySubKindID: 0,
  managerCompanySubKindName: "",
  atomCustomerGrade: null,
  atomSecurityGrade: null,
  atomEmployeeRange: null,
  managerCompanyAffiliateList: [],
  managerCompanyLocationList: [],
});
/** 系統設定-客戶-驗證物件 */
const sysCompanyValidObj = reactive<SysCompanyValidMdl>({
  managerCompanyUnifiedNumber: true,
  managerCompanyName: true,
  managerCompanyMainKindID: true,
  managerCompanySubKindID: true,
  atomManagerCompanyStatus: true,
});
/** 系統設定-客戶-營業地點-新增-頁面物件 */
const sysComLocationAddViewObj = reactive<SysComLocationAddViewMdl>({
  managerCompanyLocationName: "",
  atomCity: 0,
  managerCompanyLocationAddress: "",
  managerCompanyLocationTelephone: "",
  atomEmployeeRange: null,
  atomManagerCompanyStatus: null,
});
/** 系統設定-客戶-營業地點-驗證物件 */
const sysComLocationValidObj = reactive<SysComLocationValidMdl>({
  managerCompanyLocationName: true,
  managerCompanyLocationTelephone: true,
  atomCity: true,
  managerCompanyLocationAddress: true,
  atomManagerCompanyStatus: true,
});
/** 系統設定-客戶-公司關係-新增-頁面物件 */
const sysComAffiliateAddViewObj = reactive<SysComAffiliateAddViewMdl>({
  managerCompanyAffiliateUnifiedNumber: "",
  managerCompanyAffiliateName: "",
});
/** 系統設定-客戶-公司關係-驗證物件 */
const sysComAffiliateValidObj = reactive<SysComAffiliateValidMdl>({
  managerCompanyAffiliateUnifiedNumber: true,
  managerCompanyAffiliateName: true,
});

//#endregion

//#region 驗證相關
/** 驗證台灣統一編號 */
const validateTaiwanUnifiedNumber = (unifiedNumber: string): boolean => {
  // 格式檢查：必須是8位數字
  if (!/^\d{8}$/.test(unifiedNumber)) {
    return false;
  }

  const multipliers = [1, 2, 1, 2, 1, 2, 4, 1];
  const digits: number[] = [];

  for (let i = 0; i < 8; i++) {
    digits[i] = parseInt(unifiedNumber[i]);
  }

  // 計算乘積並拆分為個位數相加
  let digitSum = 0;
  for (let i = 0; i < 8; i++) {
    let product = digits[i] * multipliers[i];
    // 將乘積的十位和個位分開相加
    while (product > 0) {
      digitSum += product % 10;
      product = Math.floor(product / 10);
    }
  }

  // 第7位（索引6，倒數第二位）不是7的情況
  if (digits[6] !== 7) {
    return digitSum % 5 === 0;
  }

  // 第7位是7的情況：需要計算Z1和Z2
  // 計算前7位的和
  let sumExceptLast = 0;
  for (let i = 0; i < 7; i++) {
    let product = digits[i] * multipliers[i];
    while (product > 0) {
      sumExceptLast += product % 10;
      product = Math.floor(product / 10);
    }
  }

  // 最後一位的乘積（digits[7] * 1 = digits[7]）
  const lastProduct = digits[7] * multipliers[7];
  const lastTens = Math.floor(lastProduct / 10);

  // Z1: 最後一位個位數取1
  const z1 = sumExceptLast + lastTens + 1;

  // Z2: 最後一位個位數取0
  const z2 = sumExceptLast + lastTens + 0;

  return z1 % 5 === 0 || z2 % 5 === 0;
};

/** 驗證【公司】欄位 */
const validateCompanyField = () => {
  let isValid = true;

  // 統一編號
  const unifiedNumber = sysCompanyAddViewObj.managerCompanyUnifiedNumber?.trim() || "";
  if (
    typeof sysCompanyAddViewObj.managerCompanyUnifiedNumber !== "string" ||
    !unifiedNumber ||
    !validateTaiwanUnifiedNumber(unifiedNumber)
  ) {
    sysCompanyValidObj.managerCompanyUnifiedNumber = false;
    isValid = false;
  } else {
    sysCompanyValidObj.managerCompanyUnifiedNumber = true;
  }

  // 公司名稱
  if (
    typeof sysCompanyAddViewObj.managerCompanyName !== "string" ||
    !sysCompanyAddViewObj.managerCompanyName.trim() ||
    sysCompanyAddViewObj.managerCompanyName.trim().length > 300
  ) {
    sysCompanyValidObj.managerCompanyName = false;
    isValid = false;
  } else {
    sysCompanyValidObj.managerCompanyName = true;
  }

  // 主分類
  if (
    typeof sysCompanyAddViewObj.managerCompanyMainKindID !== "number" ||
    sysCompanyAddViewObj.managerCompanyMainKindID <= 0
  ) {
    sysCompanyValidObj.managerCompanyMainKindID = false;
    isValid = false;
  } else {
    sysCompanyValidObj.managerCompanyMainKindID = true;
  }

  // 子分類
  if (
    typeof sysCompanyAddViewObj.managerCompanySubKindID !== "number" ||
    sysCompanyAddViewObj.managerCompanySubKindID <= 0
  ) {
    sysCompanyValidObj.managerCompanySubKindID = false;
    isValid = false;
  } else {
    sysCompanyValidObj.managerCompanySubKindID = true;
  }

  // 狀態
  if (
    sysCompanyAddViewObj.atomManagerCompanyStatus === null ||
    sysCompanyAddViewObj.atomManagerCompanyStatus === undefined ||
    sysCompanyAddViewObj.atomManagerCompanyStatus === DbAtomManagerCompanyStatusEnum.NotSelected
  ) {
    sysCompanyValidObj.atomManagerCompanyStatus = false;
    isValid = false;
  } else {
    sysCompanyValidObj.atomManagerCompanyStatus = true;
  }

  return isValid;
};

/** 驗證【營業地點】欄位 */
const validateLocationField = () => {
  let isValid = true;

  // 名稱：不可空、長度 ≤ 100
  if (
    typeof sysComLocationAddViewObj.managerCompanyLocationName !== "string" ||
    !sysComLocationAddViewObj.managerCompanyLocationName.trim() ||
    sysComLocationAddViewObj.managerCompanyLocationName.trim().length > 100
  ) {
    sysComLocationValidObj.managerCompanyLocationName = false;
    isValid = false;
  } else {
    sysComLocationValidObj.managerCompanyLocationName = true;
  }

  // 電話：可空，但若有輸入要符合格式 (02-123-4567 或 02-1234-5678)
  const tel = sysComLocationAddViewObj.managerCompanyLocationTelephone?.trim();
  const phonePattern = /^(0\d{1,2}-\d{3,4}-\d{3,4})$/;
  if (tel && !phonePattern.test(tel)) {
    sysComLocationValidObj.managerCompanyLocationTelephone = false;
    isValid = false;
  } else {
    sysComLocationValidObj.managerCompanyLocationTelephone = true;
  }

  // 縣市：不可空
  if (
    sysComLocationAddViewObj.atomCity === null ||
    sysComLocationAddViewObj.atomCity === undefined ||
    sysComLocationAddViewObj.atomCity === 0 ||
    sysComLocationAddViewObj.atomCity === DbAtomCityEnum.NotSelected
  ) {
    sysComLocationValidObj.atomCity = false;
    isValid = false;
  } else {
    sysComLocationValidObj.atomCity = true;
  }

  // 地址：不可空、長度 ≤ 100
  if (
    typeof sysComLocationAddViewObj.managerCompanyLocationAddress !== "string" ||
    !sysComLocationAddViewObj.managerCompanyLocationAddress.trim() ||
    sysComLocationAddViewObj.managerCompanyLocationAddress.trim().length > 100
  ) {
    sysComLocationValidObj.managerCompanyLocationAddress = false;
    isValid = false;
  } else {
    sysComLocationValidObj.managerCompanyLocationAddress = true;
  }

  // 狀態：可空（這裡預設通過）
  sysComLocationValidObj.atomManagerCompanyStatus = true;

  return isValid;
};

/** 驗證【關係客戶】欄位 */
const validateAffiliateField = () => {
  let isValid = true;

  // 統編：不可為空，長度 ≤ 8
  if (
    typeof sysComAffiliateAddViewObj.managerCompanyAffiliateUnifiedNumber !== "string" ||
    !sysComAffiliateAddViewObj.managerCompanyAffiliateUnifiedNumber.trim() ||
    sysComAffiliateAddViewObj.managerCompanyAffiliateUnifiedNumber.trim().length > 8
  ) {
    sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber = false;
    isValid = false;
  } else {
    sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber = true;
  }

  // 名稱：不可為空，長度 ≤ 300
  if (
    typeof sysComAffiliateAddViewObj.managerCompanyAffiliateName !== "string" ||
    !sysComAffiliateAddViewObj.managerCompanyAffiliateName.trim() ||
    sysComAffiliateAddViewObj.managerCompanyAffiliateName.trim().length > 300
  ) {
    sysComAffiliateValidObj.managerCompanyAffiliateName = false;
    isValid = false;
  } else {
    sysComAffiliateValidObj.managerCompanyAffiliateName = true;
  }

  return isValid;
};

//#endregion

//#region 按鈕事件
/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push(`/system/company`);
};

/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateCompanyField()) return;

  // 設定關係公司與營業地點
  sysCompanyAddViewObj.managerCompanyAffiliateList = managerCompanyAffiliateList.value;
  sysCompanyAddViewObj.managerCompanyLocationList = managerCompanyLocationList.value;

  // 呼叫api
  const requestData: MbsSysComHttpAddCompanyReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyUnifiedNumber: sysCompanyAddViewObj.managerCompanyUnifiedNumber,
    managerCompanyName: sysCompanyAddViewObj.managerCompanyName,
    atomManagerCompanyStatus: sysCompanyAddViewObj.atomManagerCompanyStatus!,
    managerCompanyMainKindID: sysCompanyAddViewObj.managerCompanyMainKindID,
    managerCompanySubKindID: sysCompanyAddViewObj.managerCompanySubKindID,
    atomCustomerGrade: sysCompanyAddViewObj.atomCustomerGrade!,
    atomSecurityGrade: sysCompanyAddViewObj.atomSecurityGrade!,
    atomEmployeeRange: sysCompanyAddViewObj.atomEmployeeRange!,
    managerCompanyAffiliateList: sysCompanyAddViewObj.managerCompanyAffiliateList.map(
      (item: MbsSysComHttpAddCompanyAffiliateReqItemMdl) =>
        ({
          managerCompanyAffiliateUnifiedNumber: item.managerCompanyAffiliateUnifiedNumber,
          managerCompanyAffiliateName: item.managerCompanyAffiliateName,
        }) satisfies MbsSysComHttpAddCompanyAffiliateReqItemMdl
    ) as MbsSysComHttpAddCompanyAffiliateReqMdl[],
    managerCompanyLocationList: sysCompanyAddViewObj.managerCompanyLocationList.map(
      (item: MbsSysComHttpAddCompanyLocationReqItemMdl) =>
        ({
          managerCompanyLocationName: item.managerCompanyLocationName,
          atomCity: item.atomCity,
          managerCompanyLocationAddress: item.managerCompanyLocationAddress,
          managerCompanyLocationTelephone: item.managerCompanyLocationTelephone,
          atomManagerCompanyStatus: item.atomManagerCompanyStatus,
        }) satisfies MbsSysComHttpAddCompanyLocationReqItemMdl
    ) as MbsSysComHttpAddCompanyLocationReqMdl[],
  };
  const responseData: MbsSysComHttpAddCompanyRspMdl | null = await addCompany(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增客戶成功！");

  // 導向詳細頁
  setTimeout(() => {
    router.push(`/system/company/detail/${responseData.managerCompanyID}`);
  }, 1500);
};

/** 點擊【取消客戶公司】按鈕 */
const clickCancelCustomerCompanyBtn = () => {
  router.push("/system/company");
};

/** 點擊【營業地點新增】按鈕 */
const clickAddLocationBtn = () => {
  managerCompanyLocationIndex.value = null;
  Object.assign(sysComLocationAddViewObj, {
    managerCompanyLocationName: "",
    atomCity: null,
    managerCompanyLocationAddress: "",
    managerCompanyLocationTelephone: "",
    atomEmployeeRange: null,
    atomManagerCompanyStatus: null,
  });
  sysComLocationAddModalShow.value = true;
};

/** 點擊【營業地點更新】按鈕 */
const clickLocationUpdateBtn = (index: number) => {
  managerCompanyLocationIndex.value = index;
  Object.assign(sysComLocationAddViewObj, { ...managerCompanyLocationList.value[index] });
  sysComLocationAddModalShow.value = true;
};

/** 點擊【營業地點送出】按鈕 */
const clickLocationSubmitBtn = async () => {
  const objCopy = { ...sysComLocationAddViewObj };

  // 驗證欄位
  const isValid = await validateLocationField();
  if (!isValid) {
    return;
  }

  if (managerCompanyLocationIndex.value !== null) {
    managerCompanyLocationList.value[managerCompanyLocationIndex.value] = objCopy;
  } else {
    managerCompanyLocationList.value.push(objCopy);
  }

  sysComLocationAddModalShow.value = false;
};

/** 點擊【營業地點刪除】按鈕 */
const clickLocationDeleteBtn = (index: number) => {
  if (confirm("確定要刪除此營業地點嗎？")) {
    managerCompanyLocationList.value.splice(index, 1);
  }
};

/** 點擊【新增關係公司】按鈕 */
const clickAddAffiliateBtn = () => {
  managerCompanyAffiliateIndex.value = null;

  Object.assign(sysComAffiliateAddViewObj, {
    managerCompanyAffiliateUnifiedNumber: "",
    managerCompanyAffiliateName: "",
  });
  sysComAffiliateAddModalShow.value = true;
};

/** 點擊【編輯關係公司】按鈕 */
const clickUpdateAffiliateBtn = (index: number) => {
  managerCompanyAffiliateIndex.value = index;
  Object.assign(sysComAffiliateAddViewObj, { ...managerCompanyAffiliateList.value[index] });
  sysComAffiliateAddModalShow.value = true;
};

/** 點擊【送出關係公司】按鈕 */
const clickSubmitAffiliateBtn = async () => {
  const objCopy = { ...sysComAffiliateAddViewObj };
  // 驗證欄位
  const valid = await validateAffiliateField();
  if (!valid) return;

  if (managerCompanyAffiliateIndex.value !== null) {
    managerCompanyAffiliateList.value[managerCompanyAffiliateIndex.value] = objCopy;
  } else {
    managerCompanyAffiliateList.value.push(objCopy);
  }

  sysComAffiliateAddModalShow.value = false;
};

/** 點擊【刪除關係公司】按鈕 */
const clickDeleteAffiliateBtn = (index: number) => {
  if (confirm("確定要刪除此關係公司嗎？")) {
    managerCompanyAffiliateList.value.splice(index, 1);
  }
};

//#endregion
</script>

<template>
  <div class="flex flex-col gap-4 p-4">
    <div class="flex items-center justify-between">
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

    <!-- 客戶資訊區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="text-xl font-bold text-brand-100">客戶資訊</div>
      <!-- 後端錯誤訊息 -->
      <p
        v-if="showError"
        class="bg-red-100 border border-red-300 text-red-700 text-sm px-4 py-2 rounded mb-4"
      >
        {{ errorMessage }}
      </p>

      <form class="grid grid-cols-2 gap-4">
        <!-- 統編 -->
        <div>
          <label class="form-label">統編 <span class="required-label">*</span></label>
          <input
            v-model="sysCompanyAddViewObj.managerCompanyUnifiedNumber"
            type="text"
            placeholder="請輸入統編"
            class="input-box"
          />
          <p class="error-wrapper">
            <span v-if="!sysCompanyValidObj.managerCompanyUnifiedNumber" class="error-tip">
              請輸入正確的統一編號，不可為空，長度不可超過 8 個字
            </span>
          </p>
        </div>

        <!-- 客戶全名 -->
        <div>
          <label class="form-label">客戶全名 <span class="required-label">*</span></label>
          <input
            v-model="sysCompanyAddViewObj.managerCompanyName"
            type="text"
            placeholder="請輸入客戶全名"
            class="input-box"
          />
          <p class="error-wrapper">
            <span v-if="!sysCompanyValidObj.managerCompanyName" class="error-tip">
              不可為空，長度不可超過 300 個字
            </span>
          </p>
        </div>

        <!-- 客戶主分類 -->
        <div>
          <label class="form-label">客戶主分類 <span class="required-label">*</span></label>
          <GetManyManagerCompanyMainKindCombox
            v-model:managerCompanyMainKindID="sysCompanyAddViewObj.managerCompanyMainKindID"
            v-model:managerCompanyMainKindName="sysCompanyAddViewObj.managerCompanyMainKindName"
            :disabled="false"
          />
          <p class="error-wrapper">
            <span v-if="!sysCompanyValidObj.managerCompanyMainKindID" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>

        <!-- 客戶子分類 -->
        <div>
          <label class="form-label">客戶子分類 <span class="required-label">*</span></label>
          <GetManyManagerCompanySubKindCombox
            v-model:managerCompanyMainKindID="sysCompanyAddViewObj.managerCompanyMainKindID"
            v-model:managerCompanySubKindID="sysCompanyAddViewObj.managerCompanySubKindID"
            :disabled="!sysCompanyAddViewObj.managerCompanyMainKindID"
          />
          <p class="error-wrapper">
            <span v-if="!sysCompanyValidObj.managerCompanySubKindID" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>

        <!-- 狀態 -->
        <div>
          <label class="form-label"> 狀態 <span class="required-label">*</span> </label>
          <div class="select-wrapper">
            <select v-model="sysCompanyAddViewObj.atomManagerCompanyStatus" class="select-box">
              <option :value="DbAtomManagerCompanyStatusEnum.NotSelected">請選擇狀態</option>
              <option :value="1">營運中</option>
              <option :value="2">停業中</option>
              <option :value="3">不清楚</option>
            </select>
          </div>
          <p class="error-wrapper">
            <span v-if="!sysCompanyValidObj.atomManagerCompanyStatus" class="error-tip"
              >不可為空</span
            >
          </p>
        </div>

        <!-- 客戶分級 -->
        <div>
          <label class="form-label">客戶分級</label>
          <div class="select-wrapper">
            <select v-model="sysCompanyAddViewObj.atomCustomerGrade" class="select-box">
              <option :value="null">請選擇客戶分級</option>
              <option :value="1">A</option>
              <option :value="2">B</option>
              <option :value="3">C</option>
            </select>
          </div>
        </div>

        <!-- 資安責任等級 -->
        <div>
          <label class="form-label">資安責任等級</label>
          <div class="select-wrapper">
            <select v-model="sysCompanyAddViewObj.atomSecurityGrade" class="select-box">
              <option :value="null">請選擇資安責任等級</option>
              <option :value="1">A</option>
              <option :value="2">B</option>
              <option :value="3">C</option>
              <option :value="4">D</option>
            </select>
          </div>
        </div>
      </form>
    </div>

    <!-- 營業地點區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div>
        <div class="flex justify-between items-center">
          <div class="text-xl font-bold text-brand-100">營業地點</div>
          <button class="btn-add" @click="clickAddLocationBtn">新增營業地點</button>
        </div>
      </div>

      <!-- 營業地點列表 -->
      <div v-if="managerCompanyLocationList.length > 0">
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start">營業地點名稱</th>
              <th class="text-start w-32">電話</th>
              <th class="text-start w-32">縣市</th>
              <th class="text-start">地址</th>
              <th class="text-center w-24">狀態</th>
              <th class="text-center w-40">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in managerCompanyLocationList"
              :key="index"
              class="border border-gray-300"
            >
              <td class="text-start">{{ item.managerCompanyLocationName }}</td>
              <td class="text-start">{{ item.managerCompanyLocationTelephone }}</td>
              <td class="text-start">{{ getCityLabel(item.atomCity) }}</td>
              <td class="text-start">{{ item.managerCompanyLocationAddress }}</td>
              <td class="text-center">
                {{ getManagerCompanyStatusLabel(item.atomManagerCompanyStatus) }}
              </td>
              <td class="text-center">
                <button class="btn-update me-1" @click="clickLocationUpdateBtn(index)">編輯</button>
                <button class="btn-delete" @click="clickLocationDeleteBtn(index)">刪除</button>
              </td>
            </tr>
            <template v-if="managerCompanyLocationList.length === 0">
              <tr class="text-center">
                <td colspan="6">無資料</td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>
    </div>

    <!-- 關係客戶區塊 -->
    <div class="flex flex-col gap-6 p-8 bg-white rounded-lg">
      <div class="flex justify-between items-center">
        <div class="text-xl font-bold text-brand-100">關係客戶</div>
        <button class="btn-add" @click="clickAddAffiliateBtn">新增關係客戶</button>
      </div>

      <div v-if="managerCompanyAffiliateList.length > 0">
        <!-- 關係客戶列表 -->
        <table class="table-base table-fixed table-sticky w-full">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="text-start w-40">統編</th>
              <th class="text-start">關係客戶名稱</th>
              <th class="text-center w-40">操作</th>
            </tr>
          </thead>

          <tbody>
            <template v-if="managerCompanyAffiliateList.length === 0">
              <tr>
                <td colspan="3">無資料</td>
              </tr>
            </template>
            <template v-else>
              <tr
                v-for="(item, index) in managerCompanyAffiliateList"
                :key="index"
                class="border border-gray-300"
              >
                <td class="text-start">{{ item.managerCompanyAffiliateUnifiedNumber }}</td>
                <td class="text-start">{{ item.managerCompanyAffiliateName }}</td>
                <td class="text-center">
                  <button class="btn-update me-1" @click="clickUpdateAffiliateBtn(index)">
                    編輯
                  </button>
                  <button class="btn-delete" @click="clickDeleteAffiliateBtn(index)">刪除</button>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>
    </div>

    <div>
      <div class="flex justify-center mt-3 gap-2">
        <button class="btn-cancel" @click="clickCancelCustomerCompanyBtn">取消</button>
        <button class="btn-submit" @click="clickSubmitBtn">送出</button>
      </div>
    </div>
  </div>

  <!-- 營業地點 Modal -->
  <div v-if="sysComLocationAddModalShow" class="modal-overlay">
    <div class="w-[480px] bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col">
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">
          {{ managerCompanyLocationIndex !== null ? "編輯營業地點" : "新增營業地點" }}
        </h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysComLocationAddModalShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-2">
          <!-- 營業地點名稱 -->
          <div>
            <label class="form-label">營業地點名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComLocationAddViewObj.managerCompanyLocationName"
              class="input-box"
              type="text"
              placeholder="請輸入營業地點名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComLocationValidObj.managerCompanyLocationName" class="error-tip">
                不可為空，長度不可超過 100 個字
              </span>
            </p>
          </div>

          <!-- 電話 -->
          <div>
            <label class="form-label">電話</label>
            <input
              v-model="sysComLocationAddViewObj.managerCompanyLocationTelephone"
              class="input-box"
              type="text"
              placeholder="請輸入電話"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComLocationValidObj.managerCompanyLocationTelephone"
                class="error-tip"
              >
                格式不符 02-123-4567 或 02-1234-5678
              </span>
            </p>
          </div>

          <!-- 縣市和地址 -->
          <div class="flex gap-4">
            <!-- 縣市 -->
            <div class="w-40">
              <label class="form-label">縣市 <span class="required-label">*</span></label>
              <GetManyCityComboBox v-model:atomCity="sysComLocationAddViewObj.atomCity" />
              <p class="error-wrapper">
                <span v-if="!sysComLocationValidObj.atomCity" class="error-tip">不可為空</span>
              </p>
            </div>

            <!-- 地址 -->
            <div class="flex-1">
              <label class="form-label">地址 <span class="required-label">*</span></label>
              <input
                v-model="sysComLocationAddViewObj.managerCompanyLocationAddress"
                class="input-box"
                type="text"
                placeholder="請輸入詳細地址"
              />
              <p class="error-wrapper">
                <span
                  v-if="!sysComLocationValidObj.managerCompanyLocationAddress"
                  class="error-tip"
                >
                  不可為空，長度不可超過 100 個字
                </span>
              </p>
            </div>
          </div>

          <!-- 狀態 -->
          <div>
            <label class="form-label">狀態</label>
            <select v-model="sysComLocationAddViewObj.atomManagerCompanyStatus" class="select-box">
              <option :value="null">請選擇</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Operating">營運中</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Closed">停業</option>
              <option :value="DbAtomManagerCompanyStatusEnum.Unclear">不清楚</option>
            </select>
            <p class="error-wrapper"></p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="sysComLocationAddModalShow = false">取消</button>
          <button class="btn-submit" @click="clickLocationSubmitBtn">送出</button>
        </div>
      </div>
    </div>
  </div>

  <!-- 關係客戶 Modal -->
  <div v-if="sysComAffiliateAddModalShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">
          {{ managerCompanyAffiliateIndex !== null ? "編輯關係客戶" : "新增關係客戶" }}
        </h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysComAffiliateAddModalShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-2">
          <!-- 編號 -->
          <div>
            <label class="form-label">編號 <span class="required-label">*</span></label>
            <input
              v-model="sysComAffiliateAddViewObj.managerCompanyAffiliateUnifiedNumber"
              class="input-box"
              type="text"
              placeholder="請輸入 8 位數統編"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysComAffiliateValidObj.managerCompanyAffiliateUnifiedNumber"
                class="error-tip"
              >
                不可為空，長度不可超過 8 個字
              </span>
            </p>
          </div>

          <!-- 關係客戶 -->
          <div>
            <label class="form-label">關係客戶名稱 <span class="required-label">*</span></label>
            <input
              v-model="sysComAffiliateAddViewObj.managerCompanyAffiliateName"
              class="input-box"
              type="text"
              placeholder="請輸入關係客戶名稱"
            />
            <p class="error-wrapper">
              <span v-if="!sysComAffiliateValidObj.managerCompanyAffiliateName" class="error-tip">
                不可為空，長度不可超過 300 個字
              </span>
            </p>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="sysComAffiliateAddModalShow = false">取消</button>
          <button
            v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
            class="btn-submit"
            @click="clickSubmitAffiliateBtn"
          >
            送出
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- 錯誤訊息彈跳視窗 -->
  <ErrorAlert :message="errorMessage" :show="showError" @close="closeError" />

  <!-- 成功訊息提示 -->
  <SuccessToast :message="successMessage" :show="showSuccess" @close="closeSuccess" />
</template>
