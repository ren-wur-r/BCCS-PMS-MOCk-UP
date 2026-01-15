<script setup lang="ts">
import { reactive, ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useTokenStore } from "@/stores/token";
import { useErrorCodeHandler } from "@/composables/useErrorCodeHandler";
import { useSuccessHandler } from "@/composables/useSuccessHandler";
import {
  getCustomer,
  updateCustomer,
  deleteCustomer,
} from "@/services";
import type {
  MbsCrmPhsHttpGetCustomerReqMdl,
  MbsCrmPhsHttpUpdateCustomerReqMdl,
  MbsCrmPhsHttpUpdateCustomerReqContacterItemMdl,
  MbsCrmPhsHttpDeleteCustomerReqMdl,
} from "@/services/pms-http/crm/phone-sales/phoneSalesHttpFormat";
import {
  DbAtomPhoneSalesCustomerStatusEnum,
  DbAtomPhoneSalesCustomerStatusLabels,
} from "@/constants/DbAtomPhoneSalesCustomerStatusEnum";
import {
  DbAtomPhoneSalesCustomerValueEnum,
  DbAtomPhoneSalesCustomerValueLabels,
} from "@/constants/DbAtomPhoneSalesCustomerValueEnum";
import {
  DbAtomPhoneSalesDataSourceEnum,
  DbAtomPhoneSalesDataSourceLabels,
} from "@/constants/DbAtomPhoneSalesDataSourceEnum";

const route = useRoute();
const router = useRouter();
const tokenStore = useTokenStore();
const { errorMessage, showError, handleErrorCode, displayError, closeError } =
  useErrorCodeHandler();
const { successMessage, showSuccess, closeSuccess, displaySuccess } =
  useSuccessHandler();

interface ContacterFormMdl {
  id: number | null;
  contacterName: string;
  contacterJobTitle: string;
  contacterPhone: string;
  contacterEmail: string;
  contacterLineID: string;
  isPrimary: boolean;
}

const customerId = ref(parseInt(route.params.id as string));
const isLoading = ref(false);
const isSubmitting = ref(false);
const showContacterModal = ref(false);
const editingContacterIndex = ref<number | null>(null);

const formData = reactive({
  companyName: "",
  unifiedNumber: "",
  industry: "",
  companyAddress: "",
  companyPhone: "",
  companyWebsite: "",
  atomPhoneSalesCustomerStatus: DbAtomPhoneSalesCustomerStatusEnum.PendingContact,
  atomPhoneSalesCustomerValue: DbAtomPhoneSalesCustomerValueEnum.Pending,
  atomPhoneSalesDataSource: DbAtomPhoneSalesDataSourceEnum.PhoneSales,
  isExistingCustomer: false,
  remark: "",
  contacterList: [] as ContacterFormMdl[],
});

const validation = reactive({
  companyName: true,
  industry: true,
  contacterList: true,
});

const contacterForm = reactive<ContacterFormMdl>({
  id: null,
  contacterName: "",
  contacterJobTitle: "",
  contacterPhone: "",
  contacterEmail: "",
  contacterLineID: "",
  isPrimary: false,
});

const loadCustomer = async () => {
  isLoading.value = true;
  try {
    const requestData: MbsCrmPhsHttpGetCustomerReqMdl = {
      employeeLoginToken: tokenStore.token,
      id: customerId.value,
    };

    const response = await getCustomer(requestData);
    if (response && handleErrorCode(response.errorCode)) {
      formData.companyName = response.companyName;
      formData.unifiedNumber = response.unifiedNumber;
      formData.industry = response.industry;
      formData.companyAddress = response.companyAddress;
      formData.companyPhone = response.companyPhone;
      formData.companyWebsite = response.companyWebsite;
      formData.atomPhoneSalesCustomerStatus = response.atomPhoneSalesCustomerStatus;
      formData.atomPhoneSalesCustomerValue = response.atomPhoneSalesCustomerValue;
      formData.atomPhoneSalesDataSource = response.atomPhoneSalesDataSource;
      formData.isExistingCustomer = response.isExistingCustomer;
      formData.remark = response.remark;
      formData.contacterList = response.contacterList.map((c) => ({
        id: c.id,
        contacterName: c.contacterName,
        contacterJobTitle: c.contacterJobTitle,
        contacterPhone: c.contacterPhone,
        contacterEmail: c.contacterEmail,
        contacterLineID: c.contacterLineID,
        isPrimary: c.isPrimary,
      }));
    } else {
      displayError();
    }
  } finally {
    isLoading.value = false;
  }
};

const resetContacterForm = () => {
  contacterForm.id = null;
  contacterForm.contacterName = "";
  contacterForm.contacterJobTitle = "";
  contacterForm.contacterPhone = "";
  contacterForm.contacterEmail = "";
  contacterForm.contacterLineID = "";
  contacterForm.isPrimary = false;
};

const openAddContacterModal = () => {
  resetContacterForm();
  editingContacterIndex.value = null;
  showContacterModal.value = true;
};

const openEditContacterModal = (index: number) => {
  const contacter = formData.contacterList[index];
  contacterForm.id = contacter.id;
  contacterForm.contacterName = contacter.contacterName;
  contacterForm.contacterJobTitle = contacter.contacterJobTitle;
  contacterForm.contacterPhone = contacter.contacterPhone;
  contacterForm.contacterEmail = contacter.contacterEmail;
  contacterForm.contacterLineID = contacter.contacterLineID;
  contacterForm.isPrimary = contacter.isPrimary;
  editingContacterIndex.value = index;
  showContacterModal.value = true;
};

const saveContacter = () => {
  if (!contacterForm.contacterName.trim()) {
    alert("請輸入窗口姓名");
    return;
  }

  if (editingContacterIndex.value !== null) {
    formData.contacterList[editingContacterIndex.value] = { ...contacterForm };
  } else {
    if (contacterForm.isPrimary) {
      formData.contacterList.forEach((c) => (c.isPrimary = false));
    }
    formData.contacterList.push({ ...contacterForm });
  }

  showContacterModal.value = false;
  resetContacterForm();
};

const removeContacter = (index: number) => {
  if (confirm("確定要移除此窗口嗎？")) {
    formData.contacterList.splice(index, 1);
  }
};

const validateForm = (): boolean => {
  validation.companyName = !!formData.companyName.trim();
  validation.industry = !!formData.industry.trim();
  validation.contacterList = formData.contacterList.length > 0;

  return (
    validation.companyName && validation.industry && validation.contacterList
  );
};

const handleSubmit = async () => {
  if (!validateForm()) {
    displayError();
    errorMessage.value = "請填寫必填欄位";
    return;
  }

  isSubmitting.value = true;
  try {
    const requestData: MbsCrmPhsHttpUpdateCustomerReqMdl = {
      employeeLoginToken: tokenStore.token,
      id: customerId.value,
      companyName: formData.companyName.trim(),
      unifiedNumber: formData.unifiedNumber.trim(),
      industry: formData.industry.trim(),
      companyAddress: formData.companyAddress.trim(),
      companyPhone: formData.companyPhone.trim(),
      companyWebsite: formData.companyWebsite.trim(),
      atomPhoneSalesCustomerStatus: formData.atomPhoneSalesCustomerStatus,
      atomPhoneSalesCustomerValue: formData.atomPhoneSalesCustomerValue,
      isExistingCustomer: formData.isExistingCustomer,
      remark: formData.remark.trim(),
      contacterList: formData.contacterList.map((c) => ({
        id: c.id,
        contacterName: c.contacterName.trim(),
        contacterJobTitle: c.contacterJobTitle.trim(),
        contacterPhone: c.contacterPhone.trim(),
        contacterEmail: c.contacterEmail.trim(),
        contacterLineID: c.contacterLineID.trim(),
        isPrimary: c.isPrimary,
      })),
    };

    const response = await updateCustomer(requestData);
    if (response && handleErrorCode(response.errorCode)) {
      displaySuccess();
      successMessage.value = "客戶更新成功";
      setTimeout(() => {
        router.push("/crm/phone-sales/customer");
      }, 1500);
    } else {
      displayError();
    }
  } finally {
    isSubmitting.value = false;
  }
};

const handleDelete = async () => {
  if (!confirm("確定要刪除此客戶嗎？此操作無法復原。")) {
    return;
  }

  isSubmitting.value = true;
  try {
    const requestData: MbsCrmPhsHttpDeleteCustomerReqMdl = {
      employeeLoginToken: tokenStore.token,
      id: customerId.value,
    };

    const response = await deleteCustomer(requestData);
    if (response && handleErrorCode(response.errorCode)) {
      displaySuccess();
      successMessage.value = "客戶刪除成功";
      setTimeout(() => {
        router.push("/crm/phone-sales/customer");
      }, 1500);
    } else {
      displayError();
    }
  } finally {
    isSubmitting.value = false;
  }
};

const handleCancel = () => {
  if (confirm("確定要取消嗎？未儲存的資料將會遺失。")) {
    router.push("/crm/phone-sales/customer");
  }
};

onMounted(() => {
  loadCustomer();
});
</script>

<template>
  <div class="p-6 max-w-4xl mx-auto">
    <div class="mb-6">
      <div class="text-2xl font-bold">編輯電銷客戶</div>
      <p class="text-gray-600 mt-1">修改客戶基本資料與窗口資訊</p>
    </div>

    <div v-if="isLoading" class="text-center py-12">
      <div class="text-gray-500">載入中...</div>
    </div>

    <div v-else class="bg-white rounded-lg shadow p-6 space-y-6">
      <div>
        <h2 class="text-lg font-semibold mb-4 border-b pb-2">公司基本資料</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium mb-1">
              公司名稱 <span class="text-red-500">*</span>
            </label>
            <input
              v-model="formData.companyName"
              type="text"
              :class="[
                'w-full border rounded px-3 py-2',
                !validation.companyName ? 'border-red-500' : '',
              ]"
              placeholder="請輸入公司名稱"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">統一編號</label>
            <input
              v-model="formData.unifiedNumber"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入統一編號"
              maxlength="8"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">
              產業 <span class="text-red-500">*</span>
            </label>
            <input
              v-model="formData.industry"
              type="text"
              :class="[
                'w-full border rounded px-3 py-2',
                !validation.industry ? 'border-red-500' : '',
              ]"
              placeholder="請輸入產業分類"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">公司電話</label>
            <input
              v-model="formData.companyPhone"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入公司電話"
            />
          </div>

          <div class="md:col-span-2">
            <label class="block text-sm font-medium mb-1">公司地址</label>
            <input
              v-model="formData.companyAddress"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入公司地址"
            />
          </div>

          <div class="md:col-span-2">
            <label class="block text-sm font-medium mb-1">公司網站</label>
            <input
              v-model="formData.companyWebsite"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入公司網站"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">客戶狀態</label>
            <select
              v-model="formData.atomPhoneSalesCustomerStatus"
              class="w-full border rounded px-3 py-2"
            >
              <option :value="DbAtomPhoneSalesCustomerStatusEnum.PendingContact">
                {{ DbAtomPhoneSalesCustomerStatusLabels[DbAtomPhoneSalesCustomerStatusEnum.PendingContact] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerStatusEnum.Contacted">
                {{ DbAtomPhoneSalesCustomerStatusLabels[DbAtomPhoneSalesCustomerStatusEnum.Contacted] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerStatusEnum.Appointed">
                {{ DbAtomPhoneSalesCustomerStatusLabels[DbAtomPhoneSalesCustomerStatusEnum.Appointed] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerStatusEnum.Dispatched">
                {{ DbAtomPhoneSalesCustomerStatusLabels[DbAtomPhoneSalesCustomerStatusEnum.Dispatched] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerStatusEnum.NoContact">
                {{ DbAtomPhoneSalesCustomerStatusLabels[DbAtomPhoneSalesCustomerStatusEnum.NoContact] }}
              </option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">客戶價值</label>
            <select
              v-model="formData.atomPhoneSalesCustomerValue"
              class="w-full border rounded px-3 py-2"
            >
              <option :value="DbAtomPhoneSalesCustomerValueEnum.High">
                {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.High] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerValueEnum.Medium">
                {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.Medium] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerValueEnum.Low">
                {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.Low] }}
              </option>
              <option :value="DbAtomPhoneSalesCustomerValueEnum.Pending">
                {{ DbAtomPhoneSalesCustomerValueLabels[DbAtomPhoneSalesCustomerValueEnum.Pending] }}
              </option>
            </select>
          </div>

          <div class="md:col-span-2">
            <label class="block text-sm font-medium mb-1">資料來源</label>
            <div class="w-full border rounded px-3 py-2 bg-gray-50">
              {{ DbAtomPhoneSalesDataSourceLabels[formData.atomPhoneSalesDataSource] }}
            </div>
          </div>

          <div class="md:col-span-2">
            <label class="flex items-center gap-2">
              <input
                v-model="formData.isExistingCustomer"
                type="checkbox"
                class="rounded"
              />
              <span class="text-sm font-medium">既有客戶</span>
            </label>
          </div>

          <div class="md:col-span-2">
            <label class="block text-sm font-medium mb-1">備註</label>
            <textarea
              v-model="formData.remark"
              class="w-full border rounded px-3 py-2"
              rows="3"
              placeholder="請輸入備註資訊"
            ></textarea>
          </div>
        </div>
      </div>

      <div>
        <div class="flex justify-between items-center mb-4 border-b pb-2">
          <h2 class="text-lg font-semibold">
            窗口資訊 <span class="text-red-500">*</span>
          </h2>
          <button
            @click="openAddContacterModal"
            type="button"
            class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 text-sm"
          >
            新增窗口
          </button>
        </div>

        <div
          v-if="!validation.contacterList && formData.contacterList.length === 0"
          class="text-red-500 text-sm mb-2"
        >
          請至少新增一位窗口
        </div>

        <div v-if="formData.contacterList.length === 0" class="text-center py-8 text-gray-500">
          尚無窗口資料，請點擊上方按鈕新增
        </div>

        <div v-else class="space-y-3">
          <div
            v-for="(contacter, index) in formData.contacterList"
            :key="index"
            class="border rounded p-4 hover:bg-gray-50"
          >
            <div class="flex justify-between items-start">
              <div class="flex-1">
                <div class="flex items-center gap-2 mb-2">
                  <h4 class="font-semibold">{{ contacter.contacterName }}</h4>
                  <span v-if="contacter.isPrimary" class="text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded">
                    主要窗口
                  </span>
                </div>
                <div class="text-sm text-gray-600 space-y-1">
                  <div v-if="contacter.contacterJobTitle">
                    職稱: {{ contacter.contacterJobTitle }}
                  </div>
                  <div v-if="contacter.contacterPhone">
                    電話: {{ contacter.contacterPhone }}
                  </div>
                  <div v-if="contacter.contacterEmail">
                    Email: {{ contacter.contacterEmail }}
                  </div>
                  <div v-if="contacter.contacterLineID">
                    LINE ID: {{ contacter.contacterLineID }}
                  </div>
                </div>
              </div>
              <div class="flex gap-2">
                <button
                  @click="openEditContacterModal(index)"
                  type="button"
                  class="px-3 py-1 bg-gray-600 text-white rounded hover:bg-gray-700 text-sm"
                >
                  編輯
                </button>
                <button
                  @click="removeContacter(index)"
                  type="button"
                  class="px-3 py-1 bg-red-600 text-white rounded hover:bg-red-700 text-sm"
                >
                  移除
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="flex justify-between pt-6 border-t">
        <button
          @click="handleDelete"
          type="button"
          :disabled="isSubmitting"
          class="px-6 py-2 bg-red-600 text-white rounded hover:bg-red-700 disabled:opacity-50"
        >
          刪除客戶
        </button>
        <div class="flex gap-4">
          <button
            @click="handleCancel"
            type="button"
            :disabled="isSubmitting"
            class="px-6 py-2 border rounded hover:bg-gray-50"
          >
            取消
          </button>
          <button
            @click="handleSubmit"
            type="button"
            :disabled="isSubmitting"
            class="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:opacity-50"
          >
            {{ isSubmitting ? "處理中..." : "儲存變更" }}
          </button>
        </div>
      </div>
    </div>

    <div
      v-if="showContacterModal"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      @click.self="showContacterModal = false"
    >
      <div class="bg-white rounded-lg p-6 w-full max-w-md">
        <h3 class="text-lg font-semibold mb-4">
          {{ editingContacterIndex !== null ? "編輯窗口" : "新增窗口" }}
        </h3>

        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium mb-1">
              姓名 <span class="text-red-500">*</span>
            </label>
            <input
              v-model="contacterForm.contacterName"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入姓名"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">職稱</label>
            <input
              v-model="contacterForm.contacterJobTitle"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入職稱"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">電話</label>
            <input
              v-model="contacterForm.contacterPhone"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入電話"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">Email</label>
            <input
              v-model="contacterForm.contacterEmail"
              type="email"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入Email"
            />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">LINE ID</label>
            <input
              v-model="contacterForm.contacterLineID"
              type="text"
              class="w-full border rounded px-3 py-2"
              placeholder="請輸入LINE ID"
            />
          </div>

          <div>
            <label class="flex items-center gap-2">
              <input
                v-model="contacterForm.isPrimary"
                type="checkbox"
                class="rounded"
              />
              <span class="text-sm font-medium">設為主要窗口</span>
            </label>
          </div>
        </div>

        <div class="flex justify-end gap-3 mt-6">
          <button
            @click="showContacterModal = false"
            type="button"
            class="px-4 py-2 border rounded hover:bg-gray-50"
          >
            取消
          </button>
          <button
            @click="saveContacter"
            type="button"
            class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            確認
          </button>
        </div>
      </div>
    </div>

    <div
      v-if="showError"
      class="fixed top-4 right-4 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded shadow-lg z-50"
      @click="closeError"
    >
      <p>{{ errorMessage }}</p>
    </div>

    <div
      v-if="showSuccess"
      class="fixed top-4 right-4 bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded shadow-lg z-50"
      @click="closeSuccess"
    >
      <p>{{ successMessage }}</p>
    </div>
  </div>
</template>
