<script setup lang="ts">
//#region 引入
import { reactive, ref, defineAsyncComponent } from "vue";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import { useRouter } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useAuth } from "@/composables/useAuth";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import { addContacter } from "@/services";
import type {
  MbsSysCttHttpAddContacterReqMdl,
  MbsSysCttHttpAddContacterRspMdl,
  MbsSysCttHttpAddContacterReqRatingItemMdl,
} from "@/services/pms-http/system/contacter/systemContacterHttpFormat";
import { DbAtomManagerContacterStatusEnum } from "@/constants/DbAtomManagerContacterStatusEnum";
import { DbAtomManagerContacterRatingKindEnum } from "@/constants/DbAtomManagerContacterRatingKindEnum";
import ErrorAlert from "@/components/global/feedback/ErrorAlert.vue";
import SuccessToast from "@/components/global/feedback/SuccessToast.vue";
import TextCounter from "@/components/global/counter/TextCounter.vue";
const GetManyManagerContacterRatingReasonComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerContacterRatingReasonComboBox.vue")
);
const GetManyManagerCompanyComboBox = defineAsyncComponent(
  () => import("@/components/feature/search-bar/GetManyManagerCompanyComboBox.vue")
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
/** 系統設定-窗口-新增-頁面模型 */
interface SysContacterAddViewMdl {
  managerCompanyID: number;
  managerContacterName: string;
  managerContacterEmail: string;
  managerContacterPhone: string;
  managerContacterDepartment: string;
  managerContacterJobTitle: string;
  managerContacterTelephone: string;
  managerContacterStatus: DbAtomManagerContacterStatusEnum;
  managerContacterIsAgreeSurvey: boolean;
  atomManagerContacterRatingKind: DbAtomManagerContacterRatingKindEnum;
  managerContacterRemark: string;
  managerContacterRatingList: SysCttRatingReasonAddViewMdl[];
}

/** 系統設定-窗口-新增-驗證模型 */
interface SysContacterAddValidateMdl {
  managerCompanyID: boolean;
  managerContacterName: boolean;
  managerContacterEmail: boolean;
  managerContacterPhone: boolean;
  managerContacterDepartment: boolean;
  managerContacterJobTitle: boolean;
  managerContacterTelephone: boolean;
}

/** 系統設定-窗口-開發評等原因-新增-頁面模型 */
interface SysCttRatingReasonAddViewMdl {
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName: string;
  managerContacterRatingRemark: string;
}

/** 系統設定-窗口-開發評等原因-新增-驗證模型 */
interface SysCttRatingReasonAddValidateMdl {
  managerContacterRatingReasonName: boolean;
}
//#endregion

//#region 頁面物件
/** 當前目錄 */
const menu = DbAtomMenuEnum.SystemContacter;
/** 系統-窗口-開發評等-新增-Modal-顯示 */
const sysCttRatingReasonAddModalShow = ref(false);
/** 暫存開發評等清單 */
const contacterRatingReasonList = ref<SysCttRatingReasonAddViewMdl[]>([]);
/** 編輯中的開發評等索引 */
const contacterRatingReasonIndex = ref<number | null>(null);

/** 系統設定-窗口-新增-頁面物件 */
const sysContacterAddViewObj = reactive<SysContacterAddViewMdl>({
  managerCompanyID: 0,
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
  managerContacterRatingList: [],
});

/** 系統設定-窗口-新增-驗證物件 */
const sysContacterAddValidObj = reactive<SysContacterAddValidateMdl>({
  managerCompanyID: true,
  managerContacterName: true,
  managerContacterEmail: true,
  managerContacterPhone: true,
  managerContacterDepartment: true,
  managerContacterJobTitle: true,
  managerContacterTelephone: true,
});

/** 系統設定-窗口-開發評等原因-新增-頁面物件 */
const sysCttRatingReasonAddViewObj = reactive<SysCttRatingReasonAddViewMdl>({
  managerContacterRatingReasonID: 0,
  managerContacterRatingReasonName: "",
  managerContacterRatingRemark: "",
});

/**  系統設定-窗口-開發評等原因-新增-驗證物件 */
const sysCttRatingReasonAddValidObj = reactive<SysCttRatingReasonAddValidateMdl>({
  managerContacterRatingReasonName: true,
});
//#endregion

//#region 驗證相關
/** 驗證【窗口】欄位 */
const validateContacterField = () => {
  let isValid = true;

  // 公司
  if (!sysContacterAddViewObj.managerCompanyID) {
    sysContacterAddValidObj.managerCompanyID = false;
    isValid = false;
  } else sysContacterAddValidObj.managerCompanyID = true;

  // 姓名
  const name = sysContacterAddViewObj.managerContacterName.trim();
  sysContacterAddValidObj.managerContacterName = !!name && name.length <= 30;
  if (!sysContacterAddValidObj.managerContacterName) isValid = false;

  // Email
  const email = sysContacterAddViewObj.managerContacterEmail.trim();
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  sysContacterAddValidObj.managerContacterEmail =
    !!email && email.length <= 50 && emailPattern.test(email);
  if (!sysContacterAddValidObj.managerContacterEmail) isValid = false;

  // 手機（可空）
  const phone = sysContacterAddViewObj.managerContacterPhone.trim();
  const phonePattern = /^09\d{2}-\d{3}-\d{3}$/;
  sysContacterAddValidObj.managerContacterPhone = !phone || phonePattern.test(phone);
  if (!sysContacterAddValidObj.managerContacterPhone) isValid = false;

  // 部門
  const dept = sysContacterAddViewObj.managerContacterDepartment.trim();
  sysContacterAddValidObj.managerContacterDepartment = dept.length <= 10;
  if (!sysContacterAddValidObj.managerContacterDepartment) isValid = false;

  // 職稱
  const job = sysContacterAddViewObj.managerContacterJobTitle.trim();
  sysContacterAddValidObj.managerContacterJobTitle = job.length <= 10;
  if (!sysContacterAddValidObj.managerContacterJobTitle) isValid = false;

  // 電話分機
  const tel = sysContacterAddViewObj.managerContacterTelephone.trim();
  // 台灣電話格式：區號(02-09) + 電話號碼(7-8位，格式靈活)
  const telPattern = /^0[2-9]-\d{3,4}-\d{3,4}(#.*)?$/;
  sysContacterAddValidObj.managerContacterTelephone = !tel || telPattern.test(tel);
  if (!sysContacterAddValidObj.managerContacterTelephone) isValid = false;

  return isValid;
};

/** 驗證【開發評等】欄位 */
const validateContacterRatingField = () => {
  let isValid = true;

  // 開發評等原因
  if (
    !sysCttRatingReasonAddViewObj.managerContacterRatingReasonName ||
    !sysCttRatingReasonAddViewObj.managerContacterRatingReasonName.trim()
  ) {
    sysCttRatingReasonAddValidObj.managerContacterRatingReasonName = false;
    isValid = false;
  } else {
    sysCttRatingReasonAddValidObj.managerContacterRatingReasonName = true;
  }
  return isValid;
};
//#endregion

//#region API / 資料流程

//#endregion

//#region 按鈕事件
/**點擊【提交】按鈕 */
const clickSubmitBtn = async () => {
  // 檢查 token
  if (!requireToken()) return;

  // 驗證欄位
  if (!validateContacterField()) return;

  // 驗證黑/灰名單必須填寫開發評等原因
  if (
    (sysContacterAddViewObj.atomManagerContacterRatingKind ===
      DbAtomManagerContacterRatingKindEnum.Blacklist ||
      sysContacterAddViewObj.atomManagerContacterRatingKind ===
        DbAtomManagerContacterRatingKindEnum.Graylist) &&
    contacterRatingReasonList.value.length === 0
  ) {
    displayError("黑名單或灰名單必須填寫至少一筆開發評等原因！");
    return;
  }

  // 設定開發評等清單
  sysContacterAddViewObj.managerContacterRatingList = [...contacterRatingReasonList.value];

  // 呼叫 api
  const requestData: MbsSysCttHttpAddContacterReqMdl = {
    employeeLoginToken: tokenStore.token!,
    managerCompanyID: sysContacterAddViewObj.managerCompanyID!,
    managerContacterName: sysContacterAddViewObj.managerContacterName!,
    managerContacterEmail: sysContacterAddViewObj.managerContacterEmail!,
    managerContacterPhone: sysContacterAddViewObj.managerContacterPhone!,
    managerContacterDepartment: sysContacterAddViewObj.managerContacterDepartment!,
    managerContacterTitle: sysContacterAddViewObj.managerContacterJobTitle!,
    managerContacterTelephone: sysContacterAddViewObj.managerContacterTelephone!,
    atomManagerContacterStatus: sysContacterAddViewObj.managerContacterStatus,
    managerContacterIsAgreeSurvey: sysContacterAddViewObj.managerContacterIsAgreeSurvey,
    atomManagerContacterRatingKind: sysContacterAddViewObj.atomManagerContacterRatingKind!,
    managerContacterRatingList: sysContacterAddViewObj.managerContacterRatingList.map(
      (item: MbsSysCttHttpAddContacterReqRatingItemMdl) =>
        ({
          managerContacterRatingReasonID: item.managerContacterRatingReasonID,
          managerContacterRatingRemark: item.managerContacterRatingRemark,
        }) satisfies MbsSysCttHttpAddContacterReqRatingItemMdl
    ) as MbsSysCttHttpAddContacterReqRatingItemMdl[],
  };
  const responseData: MbsSysCttHttpAddContacterRspMdl | null = await addContacter(requestData);
  if (!responseData) {
    displayError("系統錯誤，請稍後再試！");
    return;
  }
  const isSuccess = handleErrorCode(responseData.errorCode);
  if (!isSuccess) return;

  // 顯示成功訊息
  displaySuccess("新增窗口成功！");

  // 導向詳細頁
  setTimeout(() => {
    router.push(`/system/contacter/detail/${responseData.managerContacterID}`);
  }, 1500);
};

/** 點擊【返回】按鈕 */
const clickBackBtn = () => {
  router.push("/system/contacter");
};

/** 點擊新增【開發評等原因】按鈕 */
const clickAddContacterRatingReasonBtn = () => {
  contacterRatingReasonIndex.value = null;
  Object.assign(sysCttRatingReasonAddViewObj, {
    managerContacterRatingReasonID: 0,
    managerContacterRatingReasonName: "",
    managerContacterRatingRemark: "",
  });
  sysCttRatingReasonAddModalShow.value = true;
};

/** 點擊編輯【開發評等原因】按鈕 */
const clickUpdateContacterRatingReasonBtn = (index: number) => {
  contacterRatingReasonIndex.value = index;
  Object.assign(sysCttRatingReasonAddViewObj, { ...contacterRatingReasonList.value[index] });
  sysCttRatingReasonAddModalShow.value = true;
};

/** 點擊刪除【開發評等原因】按鈕 */
const clickRemoveContacterRatingReasonBtn = (index: number) => {
  contacterRatingReasonList.value.splice(index, 1);
};

/** 點擊送出【開發評等原因】按鈕 */
const clickSubmitContacterRatingReasonBtn = () => {
  // 驗證欄位
  if (!validateContacterRatingField()) {
    return;
  }

  const objCopy = { ...sysCttRatingReasonAddViewObj };

  if (contacterRatingReasonIndex.value !== null) {
    contacterRatingReasonList.value[contacterRatingReasonIndex.value] = objCopy;
  } else {
    contacterRatingReasonList.value.push(objCopy);
  }

  sysCttRatingReasonAddModalShow.value = false;
};
//#endregion

//#region 彈跳視窗事件
/** 選擇開發評等原因 */
const onSelectRatingReason = (item: {
  managerContacterRatingReasonID: number;
  managerContacterRatingReasonName: string;
}) => {
  sysCttRatingReasonAddViewObj.managerContacterRatingReasonID = item.managerContacterRatingReasonID;
  sysCttRatingReasonAddViewObj.managerContacterRatingReasonName =
    item.managerContacterRatingReasonName;
};
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
    <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div class="text-xl font-bold text-brand-100">窗口資訊</div>

      <!-- 客戶 -->
      <div class="w-1/3">
        <label class="form-label">客戶 <span class="required-label">*</span></label>
        <GetManyManagerCompanyComboBox
          v-model:managerCompanyID="sysContacterAddViewObj.managerCompanyID"
          :disabled="false"
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
            v-model="sysContacterAddViewObj.managerContacterName"
            class="input-box"
            placeholder="請輸入姓名"
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
            v-model="sysContacterAddViewObj.managerContacterEmail"
            class="input-box"
            placeholder="請輸入電子信箱"
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
            v-model="sysContacterAddViewObj.managerContacterPhone"
            class="input-box"
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
            v-model="sysContacterAddViewObj.managerContacterDepartment"
            class="input-box"
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
            v-model="sysContacterAddViewObj.managerContacterJobTitle"
            class="input-box"
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
            v-model="sysContacterAddViewObj.managerContacterTelephone"
            class="input-box"
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
          <label class="form-label">狀態 <span class="required-label">*</span></label>
          <select v-model="sysContacterAddViewObj.managerContacterStatus" class="select-box">
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
                v-model="sysContacterAddViewObj.managerContacterIsAgreeSurvey"
                type="radio"
                :value="true"
              />
              同意</label
            >
            <label
              ><input
                v-model="sysContacterAddViewObj.managerContacterIsAgreeSurvey"
                type="radio"
                :value="false"
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
          v-model="sysContacterAddViewObj.managerContacterRemark"
          rows="4"
          class="textarea-box resize-none"
          placeholder="請輸入備註"
        ></textarea>
      </div>
    </div>

    <!-- 開發評等區塊 -->
    <div class="flex flex-col bg-white rounded-lg p-8 gap-4">
      <div class="flex items-center justify-between">
        <div class="text-xl font-bold text-brand-100">開發評等</div>
      </div>

      <!--開發評等-->
      <div class="w-1/3">
        <label class="form-label">開發評等 <span class="required-label">*</span></label>
        <select v-model="sysContacterAddViewObj.atomManagerContacterRatingKind" class="select-box">
          <option :value="DbAtomManagerContacterRatingKindEnum.Whitelist">白名單</option>
          <option :value="DbAtomManagerContacterRatingKindEnum.Graylist">灰名單</option>
          <option :value="DbAtomManagerContacterRatingKindEnum.Blacklist">黑名單</option>
        </select>
        <p
          v-if="
            sysContacterAddViewObj.atomManagerContacterRatingKind ===
              DbAtomManagerContacterRatingKindEnum.Blacklist ||
            sysContacterAddViewObj.atomManagerContacterRatingKind ===
              DbAtomManagerContacterRatingKindEnum.Graylist
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
              <th class="text-center w-40">操作</th>
            </tr>
          </thead>

          <tbody>
            <template v-if="contacterRatingReasonList.length === 0">
              <tr>
                <td colspan="3">無資料</td>
              </tr>
            </template>

            <template v-else>
              <tr
                v-for="(item, index) in contacterRatingReasonList"
                :key="index"
                class="border-b hover:bg-gray-100"
              >
                <td class="text-start">
                  {{ item.managerContacterRatingReasonName }}
                </td>
                <td class="text-start">{{ item.managerContacterRatingRemark || "-" }}</td>
                <td class="text-center">
                  <button
                    class="btn-update me-1"
                    @click="clickUpdateContacterRatingReasonBtn(index)"
                  >
                    編輯
                  </button>
                  <button class="btn-delete" @click="clickRemoveContacterRatingReasonBtn(index)">
                    刪除
                  </button>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>

      <button
        v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
        class="btn-append"
        @click="clickAddContacterRatingReasonBtn"
      >
        附加開發評等原因
      </button>
    </div>

    <!-- 按鈕列 -->
    <div>
      <div class="flex justify-center mt-3 gap-2">
        <button class="btn-cancel" @click="clickBackBtn">取消</button>
        <button
          v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
          class="btn-submit"
          @click="clickSubmitBtn"
        >
          送出
        </button>
      </div>
    </div>
  </div>

  <!-- 開發評等原因 Modal-->
  <div v-if="sysCttRatingReasonAddModalShow" class="modal-overlay">
    <div
      class="max-w-md w-full bg-white rounded-lg shadow-lg max-h-[90vh] overflow-hidden flex flex-col"
    >
      <!-- 標題區域 -->
      <div class="flex items-center justify-between p-3 flex-shrink-0">
        <h2 class="modal-title">附加開發評等原因</h2>
        <button
          class="rounded hover:bg-gray-200 text-gray-500 transition-colors text-lg font-bold px-2"
          aria-label="關閉"
          @click="sysCttRatingReasonAddModalShow = false"
        >
          ×
        </button>
      </div>

      <hr class="flex-shrink-0" />

      <!-- 內容區域 - 可滾動 -->
      <div class="p-8 overflow-y-auto flex-1">
        <div class="space-y-6">
          <!-- 原因 -->
          <div>
            <label class="form-label">開發評等原因 <span class="required-label">*</span></label>
            <GetManyManagerContacterRatingReasonComboBox
              v-model:managerContacterRatingReasonID="
                sysCttRatingReasonAddViewObj.managerContacterRatingReasonID
              "
              v-model:managerContacterRatingReasonName="
                sysCttRatingReasonAddViewObj.managerContacterRatingReasonName
              "
              :disabled="false"
              @selected="onSelectRatingReason"
            />
            <p class="error-wrapper">
              <span
                v-if="!sysCttRatingReasonAddValidObj.managerContacterRatingReasonName"
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
              v-model="sysCttRatingReasonAddViewObj.managerContacterRatingRemark"
              rows="3"
              class="textarea-box h-40 resize-none"
              placeholder="請輸入備註"
            ></textarea>
            <div>
              <TextCounter
                :text="sysCttRatingReasonAddViewObj.managerContacterRatingRemark"
                :max-length="500"
                :required="false"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- 按鈕區域 - 固定在底部 -->
      <div class="p-6 pt-4 border-t border-gray-300 flex-shrink-0">
        <div class="flex justify-end gap-2">
          <button class="btn-cancel" @click="sysCttRatingReasonAddModalShow = false">取消</button>
          <button
            v-if="employeeInfoStore.hasEveryonePermission(menu, 'create')"
            class="btn-submit"
            @click="clickSubmitContacterRatingReasonBtn"
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
