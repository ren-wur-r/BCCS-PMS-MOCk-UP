import { defineStore } from "pinia";
import { ref } from "vue";
import { DbAtomManagerActivityKindEnum } from "@/constants/DbAtomManagerActivityKindEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { orgCertificationDirectory } from "@/constants/orgCertificationDirectory";

export interface Certificate {
  employeeName: string;
  certificateName: string;
  issueDate: string;
}

export interface Announcement {
  id: number;
  title: string;
  content: string;
  date: string;
  author: string;
}

export interface MockActivity {
  managerActivityID: number;
  managerActivityName: string;
  managerActivityKind: DbAtomManagerActivityKindEnum;
  managerActivityStartTime: string;
  managerActivityEndTime: string;
  managerActivityLocation: string;
  managerActivityContent: string;
  managerActivityExpectedLeadCount: number;
  managerActivityRegisteredCount: number;
  managerActivityTransferredToSalesCount: number;
  managerActivityOpportunityCount: number;
  managerActivityPhoneCount: number;
  managerActivityEmployeePipelineCount: number;
  managerActivityProductList: MockActivityProduct[];
}

export interface MockActivityProduct {
  managerActivityProductID: number;
  managerProductID: number;
  managerProductName: string;
  managerProductMainKindID: number;
  managerProductMainKindName: string;
  managerProductSubKindID: number;
  managerProductSubKindName: string;
}

export interface MockPipeline {
  employeePipelineID: number;
  managerActivityID: number;
  atomPipelineStatus: DbAtomPipelineStatusEnum;
  managerCompanyID: number;
  managerCompanyName: string;
  managerContacterID: number;
  managerContacterName: string;
  managerContacterEmail: string;
  managerContacterPhone: string;
  managerContacterTelephone: string;
  managerContacterDepartment: string;
  managerContacterJobTitle: string;
}

export const useMockDataStore = defineStore("mockData", () => {
  // 證照資料
  const certificates = ref<Certificate[]>([]);

  // 公告資料
  const announcements = ref<Announcement[]>([
    {
      id: 1,
      title: "系統維護通知",
      content: "本週末將進行系統維護，請提前備份資料。",
      date: "2026-01-05",
      author: "Admin",
    },
  ]);

  const mockActivities = ref<MockActivity[]>([
    {
      managerActivityID: 1,
      managerActivityName: "測試活動002",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-20T18:00:00",
      managerActivityEndTime: "2025-12-22T18:00:00",
      managerActivityLocation: "線上會議室",
      managerActivityContent: "本活動將介紹最新的產品功能與技術趨勢，歡迎業界先進參與交流。",
      managerActivityExpectedLeadCount: 200,
      managerActivityRegisteredCount: 14,
      managerActivityTransferredToSalesCount: 7,
      managerActivityOpportunityCount: 0,
      managerActivityPhoneCount: 3,
      managerActivityEmployeePipelineCount: 0,
      managerActivityProductList: [
        {
          managerActivityProductID: 1,
          managerProductID: 101,
          managerProductName: "雲端解決方案 Enterprise",
          managerProductMainKindID: 1,
          managerProductMainKindName: "雲端服務",
          managerProductSubKindID: 11,
          managerProductSubKindName: "企業方案"
        }
      ]
    },
    {
      managerActivityID: 2,
      managerActivityName: "測試活動001",
      managerActivityKind: DbAtomManagerActivityKindEnum.PhysicalActivity,
      managerActivityStartTime: "2025-12-18T14:37:00",
      managerActivityEndTime: "2025-12-19T14:37:00",
      managerActivityLocation: "台北國際會議中心",
      managerActivityContent: "實體產品展示會，提供現場諮詢與體驗服務。",
      managerActivityExpectedLeadCount: 100,
      managerActivityRegisteredCount: 1,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 0,
      managerActivityPhoneCount: 0,
      managerActivityEmployeePipelineCount: 0,
      managerActivityProductList: []
    },
    {
      managerActivityID: 3,
      managerActivityName: "1215實體活動測試",
      managerActivityKind: DbAtomManagerActivityKindEnum.PhysicalActivity,
      managerActivityStartTime: "2025-12-16T13:57:00",
      managerActivityEndTime: "2025-12-19T13:57:00",
      managerActivityLocation: "高雄展覽館",
      managerActivityContent: "年度大型展覽活動，展示最新技術與應用案例。",
      managerActivityExpectedLeadCount: 400,
      managerActivityRegisteredCount: 16,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 1,
      managerActivityPhoneCount: 1,
      managerActivityEmployeePipelineCount: 1,
      managerActivityProductList: [
        {
          managerActivityProductID: 3,
          managerProductID: 102,
          managerProductName: "AI 智慧分析平台",
          managerProductMainKindID: 2,
          managerProductMainKindName: "軟體服務",
          managerProductSubKindID: 21,
          managerProductSubKindName: "數據分析"
        }
      ]
    },
    {
      managerActivityID: 4,
      managerActivityName: "1215線上活動測試",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-20T09:00:00",
      managerActivityEndTime: "2025-12-20T17:00:00",
      managerActivityLocation: "Zoom 線上研討會",
      managerActivityContent: "線上技術研討會，分享產業最佳實踐。",
      managerActivityExpectedLeadCount: 100,
      managerActivityRegisteredCount: 0,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 0,
      managerActivityPhoneCount: 0,
      managerActivityEmployeePipelineCount: 0,
      managerActivityProductList: []
    },
    {
      managerActivityID: 5,
      managerActivityName: "1215活動",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-15T13:30:00",
      managerActivityEndTime: "2025-12-15T17:30:00",
      managerActivityLocation: "Microsoft Teams",
      managerActivityContent: "產品發表會，介紹新一代解決方案。",
      managerActivityExpectedLeadCount: 10,
      managerActivityRegisteredCount: 2,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 2,
      managerActivityPhoneCount: 0,
      managerActivityEmployeePipelineCount: 2,
      managerActivityProductList: []
    },
    {
      managerActivityID: 6,
      managerActivityName: "1208活動測試",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-10T08:00:00",
      managerActivityEndTime: "2025-12-22T08:00:00",
      managerActivityLocation: "Google Meet",
      managerActivityContent: "長期線上系列講座，涵蓋多個主題。",
      managerActivityExpectedLeadCount: 0,
      managerActivityRegisteredCount: 11,
      managerActivityTransferredToSalesCount: 6,
      managerActivityOpportunityCount: 3,
      managerActivityPhoneCount: 5,
      managerActivityEmployeePipelineCount: 3,
      managerActivityProductList: [
        {
          managerActivityProductID: 6,
          managerProductID: 103,
          managerProductName: "資安防護套件",
          managerProductMainKindID: 3,
          managerProductMainKindName: "資訊安全",
          managerProductSubKindID: 31,
          managerProductSubKindName: "防護軟體"
        }
      ]
    },
    {
      managerActivityID: 7,
      managerActivityName: "21.5線上查閱展覽",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-09T11:18:00",
      managerActivityEndTime: "2026-02-19T11:18:00",
      managerActivityLocation: "虛擬展覽平台",
      managerActivityContent: "線上展覽，提供 24/7 查閱服務。",
      managerActivityExpectedLeadCount: 60,
      managerActivityRegisteredCount: 9,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 0,
      managerActivityPhoneCount: 1,
      managerActivityEmployeePipelineCount: 0,
      managerActivityProductList: []
    },
    {
      managerActivityID: 8,
      managerActivityName: "hongkong tech",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-11T17:11:00",
      managerActivityEndTime: "2026-03-24T17:06:00",
      managerActivityLocation: "International Webinar",
      managerActivityContent: "Hong Kong technology trends and market insights.",
      managerActivityExpectedLeadCount: 5,
      managerActivityRegisteredCount: 6,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 1,
      managerActivityPhoneCount: 0,
      managerActivityEmployeePipelineCount: 1,
      managerActivityProductList: []
    },
    {
      managerActivityID: 9,
      managerActivityName: "21.5線上手帳展示",
      managerActivityKind: DbAtomManagerActivityKindEnum.OnlineActivity,
      managerActivityStartTime: "2025-12-04T17:05:00",
      managerActivityEndTime: "2026-01-13T17:05:00",
      managerActivityLocation: "線上展示間",
      managerActivityContent: "手帳應用與數位整合示範。",
      managerActivityExpectedLeadCount: 67,
      managerActivityRegisteredCount: 0,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 0,
      managerActivityPhoneCount: 0,
      managerActivityEmployeePipelineCount: 0,
      managerActivityProductList: []
    },
    {
      managerActivityID: 10,
      managerActivityName: "線上實體見面會",
      managerActivityKind: DbAtomManagerActivityKindEnum.PhysicalActivity,
      managerActivityStartTime: "2025-12-04T16:38:00",
      managerActivityEndTime: "2025-12-23T16:38:00",
      managerActivityLocation: "台中會議中心",
      managerActivityContent: "混合式活動，結合線上與實體互動。",
      managerActivityExpectedLeadCount: 0,
      managerActivityRegisteredCount: 0,
      managerActivityTransferredToSalesCount: 0,
      managerActivityOpportunityCount: 0,
      managerActivityPhoneCount: 0,
      managerActivityEmployeePipelineCount: 0,
      managerActivityProductList: []
    }
  ]);

  const mockPipelines = ref<MockPipeline[]>(generateMockPipelines());

  function generateMockPipelines(): MockPipeline[] {
    const pipelines: MockPipeline[] = [];
    let pipelineId = 1;

    const companies = [
      "台積電", "聯發科", "鴻海科技", "廣達電腦", "和碩聯合",
      "華碩電腦", "緯創資通", "仁寶電腦", "英業達", "台達電子",
      "日月光", "矽品精密", "台灣大哥大", "中華電信", "遠傳電信",
      "國泰金控", "富邦金控", "中國信託", "玉山銀行", "台新銀行"
    ];

    const firstNames = ["王", "李", "張", "劉", "陳", "林", "黃", "吳", "周", "徐"];
    const lastNames = ["小明", "大華", "志強", "淑芬", "雅婷", "建國", "美玲", "俊傑", "婉如", "文彥"];
    const departments = ["資訊部", "採購部", "行銷部", "業務部", "研發部", "人資部", "財務部", "總經理室"];
    const jobTitles = ["經理", "副理", "專員", "主任", "協理", "總監", "工程師", "顧問"];

    const statuses = [
      DbAtomPipelineStatusEnum.Hyphen,
      DbAtomPipelineStatusEnum.Opened,
      DbAtomPipelineStatusEnum.Clicked,
      DbAtomPipelineStatusEnum.TransferredToSales,
      DbAtomPipelineStatusEnum.CompletedSales,
      DbAtomPipelineStatusEnum.Business10Percent,
      DbAtomPipelineStatusEnum.Business30Percent,
      DbAtomPipelineStatusEnum.Business70Percent,
      DbAtomPipelineStatusEnum.Business100Percent,
      DbAtomPipelineStatusEnum.BusinessFailed
    ];

    for (let activityId = 1; activityId <= 10; activityId++) {
      const pipelineCount = 20;

      for (let i = 0; i < pipelineCount; i++) {
        const company = companies[i % companies.length];
        const firstName = firstNames[i % firstNames.length];
        const lastName = lastNames[i % lastNames.length];
        const name = firstName + lastName;
        const email = `${lastName}@${company.toLowerCase().replace(/\s/g, '')}.com`;
        const status = statuses[i % statuses.length];

        pipelines.push({
          employeePipelineID: pipelineId++,
          managerActivityID: activityId,
          atomPipelineStatus: status,
          managerCompanyID: (i % companies.length) + 1,
          managerCompanyName: company,
          managerContacterID: pipelineId,
          managerContacterName: name,
          managerContacterEmail: email,
          managerContacterPhone: `09${Math.floor(10000000 + Math.random() * 90000000)}`,
          managerContacterTelephone: `02-${Math.floor(10000000 + Math.random() * 90000000)}`,
          managerContacterDepartment: departments[i % departments.length],
          managerContacterJobTitle: jobTitles[i % jobTitles.length]
        });
      }
    }

    return pipelines;
  }

  // 匯入證照 CSV
  const importCertificatesFromCSV = (csvContent: string) => {
    const lines = csvContent.split("\n");
    const result: Certificate[] = [];
    // 假設 CSV 格式: 員工姓名,證照名稱,取得日期
    // 跳過標題列
    for (let i = 1; i < lines.length; i++) {
      const line = lines[i].trim();
      if (!line) continue;
      const parts = line.split(",");
      if (parts.length >= 2) {
        result.push({
          employeeName: parts[0].trim(),
          certificateName: parts[1].trim(),
          issueDate: parts[2]?.trim() || "",
        });
      }
    }
    certificates.value = result;
    // Persist to local storage if needed, or just keep in memory for demo
    localStorage.setItem("mockCertificates", JSON.stringify(result));
  };

  // 新增公告
  const addAnnouncement = (announcement: Omit<Announcement, "id">) => {
    const newId = Math.max(0, ...announcements.value.map((a) => a.id)) + 1;
    announcements.value.unshift({ ...announcement, id: newId });
    localStorage.setItem("mockAnnouncements", JSON.stringify(announcements.value));
  };

  // 刪除公告
  const removeAnnouncement = (id: number) => {
    announcements.value = announcements.value.filter((a) => a.id !== id);
    localStorage.setItem("mockAnnouncements", JSON.stringify(announcements.value));
  };

  // 初始化讀取
  const init = () => {
    const storedCerts = localStorage.getItem("mockCertificates");
    const storedVersion = localStorage.getItem("mockCertificatesVersion");
    const seedVersion = "2026-01-02-v3";
    if (storedCerts && storedVersion === seedVersion) {
      certificates.value = JSON.parse(storedCerts);
    } else {
      certificates.value = [...orgCertificationDirectory];
      localStorage.setItem("mockCertificates", JSON.stringify(certificates.value));
      localStorage.setItem("mockCertificatesVersion", seedVersion);
    }

    const storedAnns = localStorage.getItem("mockAnnouncements");
    if (storedAnns) announcements.value = JSON.parse(storedAnns);
  };

  const getMockActivities = (query?: {
    managerActivityStartTime?: string | null;
    managerActivityEndTime?: string | null;
    managerActivityKind?: DbAtomManagerActivityKindEnum | null;
    managerActivityName?: string | null;
    pageIndex?: number;
    pageSize?: number;
  }) => {
    let filtered = [...mockActivities.value];

    if (query?.managerActivityName) {
      filtered = filtered.filter(a =>
        a.managerActivityName.toLowerCase().includes(query.managerActivityName!.toLowerCase())
      );
    }

    if (query?.managerActivityKind) {
      filtered = filtered.filter(a => a.managerActivityKind === query.managerActivityKind);
    }

    if (query?.managerActivityStartTime) {
      filtered = filtered.filter(a =>
        new Date(a.managerActivityStartTime) >= new Date(query.managerActivityStartTime!)
      );
    }

    if (query?.managerActivityEndTime) {
      filtered = filtered.filter(a =>
        new Date(a.managerActivityEndTime) <= new Date(query.managerActivityEndTime!)
      );
    }

    const totalCount = filtered.length;
    const pageIndex = query?.pageIndex || 1;
    const pageSize = query?.pageSize || 10;
    const startIndex = (pageIndex - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const paged = filtered.slice(startIndex, endIndex);

    return { list: paged, totalCount };
  };

  const getMockActivity = (activityId: number) => {
    return mockActivities.value.find(a => a.managerActivityID === activityId);
  };

  const getMockPipelines = (query?: {
    managerActivityID?: number;
    atomPipelineStatus?: DbAtomPipelineStatusEnum | null;
    managerCompanyName?: string | null;
    managerContacterName?: string | null;
    managerContacterEmail?: string | null;
    pageIndex?: number;
    pageSize?: number;
  }) => {
    let filtered = [...mockPipelines.value];

    if (query?.managerActivityID) {
      filtered = filtered.filter(p => p.managerActivityID === query.managerActivityID);
    }

    if (query?.atomPipelineStatus) {
      filtered = filtered.filter(p => p.atomPipelineStatus === query.atomPipelineStatus);
    }

    if (query?.managerCompanyName) {
      filtered = filtered.filter(p =>
        p.managerCompanyName.includes(query.managerCompanyName!)
      );
    }

    if (query?.managerContacterName) {
      filtered = filtered.filter(p =>
        p.managerContacterName.includes(query.managerContacterName!)
      );
    }

    if (query?.managerContacterEmail) {
      filtered = filtered.filter(p =>
        p.managerContacterEmail.includes(query.managerContacterEmail!)
      );
    }

    const totalCount = filtered.length;
    const pageIndex = query?.pageIndex || 1;
    const pageSize = query?.pageSize || 10;
    const startIndex = (pageIndex - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const paged = filtered.slice(startIndex, endIndex);

    return { list: paged, totalCount };
  };

  init();

  return {
    certificates,
    announcements,
    mockActivities,
    mockPipelines,
    importCertificatesFromCSV,
    addAnnouncement,
    removeAnnouncement,
    getMockActivities,
    getMockActivity,
    getMockPipelines,
  };
});
