<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { useEmployeeInfoStore } from "@/stores/employeeInfo";
import router from "@/router";

interface HandoffLead {
  id: number;
  activityName: string;
  activityType: string;
  activityDate: string;
  teleSalesName: string;
  callTimeRange: string;
  industry: string;
  evaluationRange: string;
  talkItems: string;
  interestLevel: string;
  noteTag: string;
  handoffRemark: string;
  companyName: string;
  contactName: string;
  contactDepartment: string;
  contactTitle: string;
  contactEmail: string;
  companyPhone: string;
  contactMobile: string;
  contactAddress: string;
  customerData1: string;
  customerData2: string;
  customerData3: string;
  customerData4: string;
  assignedSaler: string;
  status: "pending" | "accepted";
  createdAt: string;
  updatedAt: string;
}

const storageKey = "cache.crm.pipeline.handoff.queue";
const employeeInfoStore = useEmployeeInfoStore();
const handoffLeads = ref<HandoffLead[]>([]);

const regionCodeByAddress = (address: string) => {
  if (!address) return "";
  if (/(台中|彰化|南投|苗栗|雲林)/.test(address)) return "B";
  if (/(台南|高雄|屏東|嘉義)/.test(address)) return "C";
  if (/(台北|新北|基隆|桃園|新竹|宜蘭)/.test(address)) return "A";
  return "";
};

const resolveAtomCity = (address: string) => {
  if (/(台中|彰化|南投|苗栗|雲林)/.test(address)) return DbAtomCityEnum.Taichung;
  if (/(台南|高雄|屏東|嘉義)/.test(address)) return DbAtomCityEnum.Tainan;
  if (/(台北|新北|基隆|桃園|新竹|宜蘭)/.test(address)) return DbAtomCityEnum.Taipei;
  return DbAtomCityEnum.Undefined;
};

const seedHandoffLeads = (): HandoffLead[] => [
  {
    id: 1,
    activityName: "4 月 T5 展攤_iThome 資安展",
    activityType: "實體活動",
    activityDate: "4/17/25",
    teleSalesName: "Evan",
    callTimeRange: "12/25-12/31",
    industry: "一般企業、自動控制相關業",
    evaluationRange: "非編列預算制度，採需求評估",
    talkItems: "EDR MDR 產品+公司服務",
    interestLevel: "直接商機-邀約成功到場產品+公司服務介紹",
    noteTag: "主要窗口",
    handoffRemark:
      "12/30 寄公司服務&_BCCS MDR EDM。非編列預算制度，採需求評估，在明年計畫有評估 EDR 或 MDR，邀約產品+公司服務介紹成功。客戶可以的時間 1 月份都可以。",
    companyName: "中和碁電股份有限公司",
    contactName: "陳信溢",
    contactDepartment: "管理部",
    contactTitle: "資訊工程",
    contactEmail: "ponisoon@jidien.com",
    companyPhone: "06-266-8899#504",
    contactMobile: "",
    contactAddress: "台南",
    customerData1: "",
    customerData2: "",
    customerData3: "",
    customerData4: "",
    assignedSaler: "Lisa",
    status: "pending",
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  },
  {
    id: 2,
    activityName: "4 月 T5 展攤_iThome 資安展",
    activityType: "實體活動",
    activityDate: "4/17/25",
    teleSalesName: "Evan",
    callTimeRange: "12/25-12/31",
    industry: "一般企業、自行車及其零件製造業",
    evaluationRange: "無資料",
    talkItems: "公司服務",
    interestLevel: "潛在商機_邀約介紹未直接答應表示要內部考慮",
    noteTag: "新增主要窗口",
    handoffRemark:
      "新增窗口 12/30 寄公司服務&_BCCS MDR EDM。請先寄資料來參考，表示未來會擴充資安架構。邀約公司服務介紹表示先看資料。",
    companyName: "久鼎金屬實業股份有限公司",
    contactName: "黃先生",
    contactDepartment: "資訊部",
    contactTitle: "",
    contactEmail: "fish.huang@jdco.com.tw",
    companyPhone: "04-22519325#121",
    contactMobile: "",
    contactAddress: "彰化",
    customerData1: "",
    customerData2: "",
    customerData3: "",
    customerData4: "",
    assignedSaler: "Mandy",
    status: "pending",
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  },
  {
    id: 3,
    activityName: "4 月 T5 展攤_iThome 資安展",
    activityType: "實體活動",
    activityDate: "4/17/25",
    teleSalesName: "Evan",
    callTimeRange: "12/25-12/31",
    industry: "一般企業、工具製造業",
    evaluationRange: "無資料",
    talkItems: "EDR MDR+ISMS",
    interestLevel: "客戶深耕",
    noteTag: "主要窗口",
    handoffRemark:
      "1/2 客戶回信。由於本年度透過導入 ISO 27001 進行初期風險盤點，短期內暫無需求，後續若有風險專案再評估。",
    companyName: "台中精機廠股份有限公司",
    contactName: "蔣坤樟",
    contactDepartment: "資訊課",
    contactTitle: "資安負責系統工程師",
    contactEmail: "rock@mail.or.com.tw",
    companyPhone: "04-2359-2101#1720",
    contactMobile: "938562623",
    contactAddress: "408 台中市南屯區精科中二路 1 號",
    customerData1: "台中",
    customerData2: "",
    customerData3: "",
    customerData4: "",
    assignedSaler: "Ann",
    status: "pending",
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  },
  {
    id: 4,
    activityName: "4 月 T5 展攤_iThome 資安展",
    activityType: "實體活動",
    activityDate: "4/17/25",
    teleSalesName: "Evan",
    callTimeRange: "12/25-12/31",
    industry: "一般企業、工具製造業",
    evaluationRange: "無資料",
    talkItems: "EDR MDR+ISMS",
    interestLevel: "客戶深耕",
    noteTag: "新增次要窗口",
    handoffRemark:
      "有評估 EDR/MDR，ISO 27001 2022 已找廠商導入，弱掃委外，邀約後續到場簡報 ISO 顧問 + EDR/MDR。",
    companyName: "台中精機廠股份有限公司",
    contactName: "林毓澤",
    contactDepartment: "資訊課",
    contactTitle: "",
    contactEmail: "m4977@mail.or.com.tw",
    companyPhone: "04-2359-2101#1721",
    contactMobile: "",
    contactAddress: "台中",
    customerData1: "",
    customerData2: "",
    customerData3: "",
    customerData4: "",
    assignedSaler: "Ann",
    status: "pending",
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  },
];

const loadHandoffLeads = () => {
  const raw = localStorage.getItem(storageKey);
  if (raw) {
    try {
      const parsed = JSON.parse(raw);
      handoffLeads.value = Array.isArray(parsed) ? parsed : [];
      return;
    } catch {
      // ignore
    }
  }
  const seeded = seedHandoffLeads();
  handoffLeads.value = seeded;
  localStorage.setItem(storageKey, JSON.stringify(seeded));
};

const formatDateTime = (value: string) => {
  if (!value) return "-";
  try {
    return new Date(value).toLocaleString("zh-TW", { hour12: false });
  } catch {
    return value;
  }
};

const pendingLeads = computed(() => {
  const currentName = employeeInfoStore.employeeName?.trim();
  return handoffLeads.value.filter((item) => {
    if (item.status !== "pending") return false;
    if (!currentName) return true;
    return item.assignedSaler === currentName;
  });
});

const acceptHandoff = (lead: HandoffLead) => {
  const listKey = "cache.crm.pipeline.list";
  const detailPrefix = "cache.crm.pipeline.detail.";
  const listRaw = localStorage.getItem(listKey);
  const list = listRaw ? JSON.parse(listRaw) : [];
  const nextId = list.reduce((max: number, item: { id: number }) => Math.max(max, item.id), 0) + 1;
  const record = {
    id: nextId,
    status: DbAtomPipelineStatusEnum.Business10Percent,
    companyName: lead.companyName,
    contacterDepartment: lead.contactDepartment,
    contacterJobTitle: lead.contactTitle,
    contacterName: lead.contactName,
    contacterEmail: lead.contactEmail,
    contacterPhone: lead.contactMobile,
    contacterTelephone: lead.companyPhone,
    salerEmployeeName: lead.assignedSaler || employeeInfoStore.employeeName || "目前登入者",
    managerCompanyUnifiedNumber: "",
    customerRegionCode: regionCodeByAddress(lead.contactAddress),
    stageStatus: {
      businessNeedStatus: null,
      businessTimelineStatus: null,
      businessBudgetStatus: null,
      businessPresentationStatus: null,
      businessQuotationStatus: null,
      businessNegotiationStatus: null,
      businessStageRemark: "",
      notes: {
        businessNeedStatus: "",
        businessTimelineStatus: "",
        businessBudgetStatus: "",
        businessPresentationStatus: "",
        businessQuotationStatus: "",
        businessNegotiationStatus: "",
      },
    },
  };
  list.unshift(record);
  localStorage.setItem(listKey, JSON.stringify(list));

  const detail = {
    ...record,
    company: {
      managerCompanyUnifiedNumber: "",
      managerCompanyID: nextId,
      managerCompanyName: lead.companyName,
      atomEmployeeRange: null,
      atomCustomerGrade: null,
      managerCompanyMainKindName: lead.customerData2 || "",
      managerCompanySubKindName: lead.customerData3 || "",
      atomCity: resolveAtomCity(lead.contactAddress),
      managerCompanyLocationID: 1,
      managerCompanyLocationAddress: lead.contactAddress,
      managerCompanyLocationTelephone: lead.companyPhone,
      managerCompanyLocationStatus: null,
    },
    contacterName: lead.contactName,
    contacterDepartment: lead.contactDepartment,
    contacterJobTitle: lead.contactTitle,
    contacterEmail: lead.contactEmail,
    contacterPhone: lead.contactMobile,
    contacterTelephone: lead.companyPhone,
  };
  localStorage.setItem(`${detailPrefix}${nextId}`, JSON.stringify(detail));

  handoffLeads.value = handoffLeads.value.filter((item) => item.id !== lead.id);
  localStorage.setItem(storageKey, JSON.stringify(handoffLeads.value));

  router.push(`/crm/pipeline/pipeline/detail/${nextId}`);
};

onMounted(() => {
  loadHandoffLeads();
});
</script>

<template>
  <div class="flex flex-col h-[calc(100vh-100px)] overflow-hidden gap-4 p-2">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-2">
        <span class="text-sm text-gray-500">電銷模組設計待確認</span>
      </div>
      <span class="text-xs text-gray-500">共 {{ pendingLeads.length }} 筆</span>
    </div>

    <div class="flex flex-col h-full overflow-hidden bg-white rounded-lg p-6 gap-4">
      <div class="flex-1 overflow-y-auto table-scrollable">
        <table class="table-base table-fixed table-sticky w-full min-w-[900px]">
          <thead class="sticky top-0 bg-white z-10">
            <tr>
              <th class="text-start w-56">公司全名</th>
              <th class="text-start w-32">窗口姓名</th>
              <th class="text-start w-56">洽談項目</th>
              <th class="text-start w-40">興趣程度</th>
              <th class="text-start w-24">分派業務</th>
              <th class="text-start w-40">電銷備註</th>
              <th class="text-center w-24">接單</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="pendingLeads.length === 0" class="text-center">
              <td colspan="7">目前無電銷開發資料</td>
            </tr>
            <tr
              v-for="lead in pendingLeads"
              :key="lead.id"
              class="hover:bg-gray-50 transition-colors"
            >
              <td class="text-start">{{ lead.companyName }}</td>
              <td class="text-start">{{ lead.contactName }}</td>
              <td class="text-start">{{ lead.talkItems }}</td>
              <td class="text-start">{{ lead.interestLevel }}</td>
              <td class="text-start">{{ lead.assignedSaler }}</td>
              <td class="text-start text-xs text-gray-500 line-clamp-2">
                {{ lead.handoffRemark }}
              </td>
              <td class="text-center">
                <button class="btn-submit" type="button" @click="acceptHandoff(lead)">
                  接單
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="text-xs text-gray-500">
        更新時間：{{ pendingLeads[0]?.updatedAt ? formatDateTime(pendingLeads[0].updatedAt) : "-" }}
      </div>
    </div>
  </div>
</template>
