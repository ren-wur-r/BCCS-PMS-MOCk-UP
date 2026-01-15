import { jointDefenseProducts } from "@/data/jointDefenseProducts";

export type OutputKind = "document" | "meeting" | "report" | "other";

export interface TemplateOutput {
  outputId: number;
  outputName: string;
  outputKind: OutputKind;
  outputKindLabel?: string;
  required: boolean;
}

export interface TemplateStage {
  stageId: number;
  stageName: string;
  ownerRoleLabel: string;
  requiredOutputs: TemplateOutput[];
  requiresWorkLog: boolean;
  requiresOnsiteLog: boolean;
  reminderDays: number[];
  reminderAnchor: "start" | "end";
  estimatedDays?: number;
  estimatedHours?: number;
  onsiteHours?: number;
  offsiteHours?: number;
  deadlineDays?: number;
  deadlineAnchor?: "start" | "end";
  requireEstimatedDays?: boolean;
  requireEstimatedHours?: boolean;
}

export interface StageTemplate {
  stageTemplateId: number;
  stageTemplateName: string;
  departmentLabel: string;
  description: string;
  stages: TemplateStage[];
  lastEditedBy?: string;
  lastEditedAt?: string;
}

export interface ServiceItemProductTemplate {
  productId: number;
  productName: string;
  description: string;
  stageTemplateIds: number[];
}

export interface ServiceItemTemplate {
  serviceItemId: number;
  serviceItemName: string;
  description: string;
  departmentLabel: string;
  products: ServiceItemProductTemplate[];
}

export type ProjectTypeCategory = "single" | "hybrid";

export interface ProjectTypeTemplate {
  projectTypeId: number;
  projectTypeName: string;
  description: string;
  category: ProjectTypeCategory;
  serviceItemIds: number[];
  requiredAttachments: {
    order: boolean;
    sla: boolean;
  };
}

export interface ProjectTemplateSettings {
  schemaVersion: number;
  projectTypes: ProjectTypeTemplate[];
  serviceItems: ServiceItemTemplate[];
  stageTemplates: StageTemplate[];
  reminderRule: {
    supervisorNotifyBeforeDays: number;
    gmNotifyBeforeDays: number;
  };
}

const STORAGE_KEY = "projectTemplateSettings";
const CURRENT_SCHEMA_VERSION = 5;

const defaultSettings: ProjectTemplateSettings = {
  schemaVersion: CURRENT_SCHEMA_VERSION,
  reminderRule: {
    supervisorNotifyBeforeDays: 5,
    gmNotifyBeforeDays: 3,
  },
  stageTemplates: [
    {
      stageTemplateId: 1,
      stageTemplateName: "MSSP By Rapixus VANS 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 2,
      stageTemplateName: "聯防設備 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [
        {
          stageId: 201,
          stageName: "專案啟始與定義階段",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 2001, outputName: "專案工作計畫書", outputKind: "document", required: true },
            { outputId: 2002, outputName: "啟始會議紀錄", outputKind: "meeting", required: true },
            { outputId: 2003, outputName: "專案基本資訊表", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: undefined,
          onsiteHours: undefined,
          offsiteHours: undefined,
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 202,
          stageName: "資料準備與檢測規劃階段",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 2101, outputName: "受測系統與環境清單", outputKind: "document", required: true },
            { outputId: 2102, outputName: "檢測項目與方法對照表", outputKind: "document", required: true },
            { outputId: 2103, outputName: "檢測計畫文件", outputKind: "document", required: true },
            { outputId: 2104, outputName: "資料蒐集需求清單", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: undefined,
          onsiteHours: undefined,
          offsiteHours: undefined,
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 203,
          stageName: "現場執行與資料蒐集階段",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 2201, outputName: "原始檢測資料", outputKind: "report", required: true },
            { outputId: 2202, outputName: "檢測行動紀錄", outputKind: "report", required: true },
            { outputId: 2203, outputName: "訪談或現場確認紀錄", outputKind: "meeting", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: true,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: undefined,
          onsiteHours: undefined,
          offsiteHours: undefined,
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 204,
          stageName: "初步分析與風險分級階段",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 2301, outputName: "弱點與問題清單", outputKind: "report", required: true },
            { outputId: 2302, outputName: "風險分級表", outputKind: "report", required: true },
            { outputId: 2303, outputName: "問題分類與統計資料", outputKind: "report", required: true },
            { outputId: 2304, outputName: "初步改善建議摘要", outputKind: "report", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: undefined,
          onsiteHours: undefined,
          offsiteHours: undefined,
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 205,
          stageName: "結果確認與收斂階段",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 2401, outputName: "結果確認會議紀錄", outputKind: "meeting", required: true },
            { outputId: 2402, outputName: "修正後風險與問題清單", outputKind: "report", required: true },
            { outputId: 2403, outputName: "改善優先順序列表", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: undefined,
          onsiteHours: undefined,
          offsiteHours: undefined,
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 206,
          stageName: "結案準備與交付階段",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 2501, outputName: "專案執行紀錄彙整包", outputKind: "report", required: true },
            {
              outputId: 2502,
              outputName: "完整原始資料與分析資料集",
              outputKind: "report",
              required: true,
            },
            { outputId: 2503, outputName: "會議與決策紀錄", outputKind: "document", required: true },
            { outputId: 2504, outputName: "結案確認文件", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: undefined,
          onsiteHours: undefined,
          offsiteHours: undefined,
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
      ],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 3,
      stageTemplateName: "顧問諮詢 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [
        {
          stageId: 301,
          stageName: "合約與需求審閱",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3001, outputName: "合約重點摘要", outputKind: "document", required: true },
            { outputId: 3002, outputName: "需求規格檢核清單", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 4,
          onsiteHours: 0,
          offsiteHours: 4,
          deadlineDays: 3,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 302,
          stageName: "行政與保密需求確認",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3011, outputName: "保密同意書", outputKind: "document", required: true },
            { outputId: 3012, outputName: "發文方式確認", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 2,
          onsiteHours: 0,
          offsiteHours: 2,
          deadlineDays: 3,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 303,
          stageName: "計畫書與起始會議準備",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3021, outputName: "專案工作計畫書", outputKind: "document", required: true },
            { outputId: 3022, outputName: "起始會議簡報", outputKind: "meeting", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 8,
          onsiteHours: 0,
          offsiteHours: 8,
          deadlineDays: undefined,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 304,
          stageName: "起始會議",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3031, outputName: "起始會議紀錄", outputKind: "meeting", required: true },
            { outputId: 3032, outputName: "待辦清單", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: true,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 8,
          onsiteHours: 4,
          offsiteHours: 4,
          deadlineDays: undefined,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 305,
          stageName: "形式曆與人天規劃",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3041, outputName: "形式曆", outputKind: "document", required: true },
            { outputId: 3042, outputName: "人天規劃表", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 8,
          onsiteHours: 0,
          offsiteHours: 8,
          deadlineDays: undefined,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 306,
          stageName: "文件建置與訪談盤點",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3051, outputName: "訪談紀錄", outputKind: "meeting", required: true },
            { outputId: 3052, outputName: "資產盤點清單", outputKind: "document", required: true },
            { outputId: 3053, outputName: "文件草案", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: true,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 12,
          onsiteHours: 8,
          offsiteHours: 4,
          deadlineDays: undefined,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 307,
          stageName: "風險評鑑與改善建議",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3061, outputName: "風險評鑑報告", outputKind: "report", required: true },
            { outputId: 3062, outputName: "改善建議摘要", outputKind: "report", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 8,
          onsiteHours: 4,
          offsiteHours: 4,
          deadlineDays: undefined,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 308,
          stageName: "內稽/管審會議支援",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3071, outputName: "內稽紀錄", outputKind: "document", required: true },
            { outputId: 3072, outputName: "管審會議紀錄", outputKind: "meeting", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: true,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 6,
          onsiteHours: 4,
          offsiteHours: 2,
          deadlineDays: undefined,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 309,
          stageName: "結案準備與交付",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3081, outputName: "交付清單", outputKind: "document", required: true },
            { outputId: 3082, outputName: "結案報告", outputKind: "report", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "start",
          estimatedDays: undefined,
          estimatedHours: 4,
          onsiteHours: 0,
          offsiteHours: 4,
          deadlineDays: 180,
          deadlineAnchor: "start",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
        {
          stageId: 310,
          stageName: "第三方驗證準備（如適用）",
          ownerRoleLabel: "未設定",
          requiredOutputs: [
            { outputId: 3091, outputName: "驗證申請", outputKind: "document", required: true },
            { outputId: 3092, outputName: "文件發行", outputKind: "document", required: true },
          ],
          requiresWorkLog: true,
          requiresOnsiteLog: false,
          reminderDays: [],
          reminderAnchor: "end",
          estimatedDays: undefined,
          estimatedHours: 6,
          onsiteHours: 0,
          offsiteHours: 6,
          deadlineDays: 90,
          deadlineAnchor: "end",
          requireEstimatedDays: false,
          requireEstimatedHours: false,
        },
      ],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 4,
      stageTemplateName: "風險檢測 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 5,
      stageTemplateName: "資安學苑 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 6,
      stageTemplateName: "內部軟體開發 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 7,
      stageTemplateName: "外部軟體開發 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 8,
      stageTemplateName: "advMDR主動入侵威脅鑑識管理服務(含T5 EDR) 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
    {
      stageTemplateId: 9,
      stageTemplateName: "SOC服務低流量監測 標準階段",
      departmentLabel: "未設定",
      description: "",
      stages: [],
      lastEditedBy: "",
      lastEditedAt: "",
    },
  ],
  serviceItems: [
    {
      serviceItemId: 1,
      serviceItemName: "雲安維運",
      description: "",
      departmentLabel: "未設定",
      products: [
        {
          productId: 1,
          productName: "MSSP By Rapixus VANS",
          description: "",
          stageTemplateIds: [1],
        },
        {
          productId: 2,
          productName: "advMDR主動入侵威脅鑑識管理服務(含T5 EDR)",
          description: "",
          stageTemplateIds: [8],
        },
        {
          productId: 3,
          productName: "SOC服務低流量監測",
          description: "",
          stageTemplateIds: [9],
        },
      ],
    },
    {
      serviceItemId: 2,
      serviceItemName: "聯防設備",
      description: "",
      departmentLabel: "未設定",
      products: jointDefenseProducts.map((product, index) => ({
        productId: index + 1,
        productName: product.productName,
        description: product.description,
        stageTemplateIds: [2],
      })),
    },
    {
      serviceItemId: 3,
      serviceItemName: "顧問諮詢",
      description: "",
      departmentLabel: "未設定",
      products: [
        {
          productId: 1,
          productName: "預設產品",
          description: "",
          stageTemplateIds: [3],
        },
      ],
    },
    {
      serviceItemId: 4,
      serviceItemName: "風險檢測",
      description: "",
      departmentLabel: "未設定",
      products: [
        {
          productId: 1,
          productName: "預設產品",
          description: "",
          stageTemplateIds: [4],
        },
      ],
    },
    {
      serviceItemId: 5,
      serviceItemName: "資安學苑",
      description: "",
      departmentLabel: "未設定",
      products: [
        {
          productId: 1,
          productName: "預設產品",
          description: "",
          stageTemplateIds: [5],
        },
      ],
    },
    {
      serviceItemId: 6,
      serviceItemName: "內部軟體開發",
      description: "",
      departmentLabel: "未設定",
      products: [
        {
          productId: 1,
          productName: "預設產品",
          description: "",
          stageTemplateIds: [6],
        },
      ],
    },
    {
      serviceItemId: 7,
      serviceItemName: "外部軟體開發",
      description: "",
      departmentLabel: "未設定",
      products: [
        {
          productId: 1,
          productName: "預設產品",
          description: "",
          stageTemplateIds: [7],
        },
      ],
    },
  ],
  projectTypes: [
    {
      projectTypeId: 1,
      projectTypeName: "單一服務案",
      description: "單一服務項目交付。",
      category: "single",
      serviceItemIds: [1],
      requiredAttachments: {
        order: true,
        sla: true,
      },
    },
    {
      projectTypeId: 2,
      projectTypeName: "混合案",
      description: "可複選多個服務項目。",
      category: "hybrid",
      serviceItemIds: [1, 2, 3, 4, 5, 6, 7],
      requiredAttachments: {
        order: true,
        sla: true,
      },
    },
  ],
};

const cloneSettings = (settings: ProjectTemplateSettings): ProjectTemplateSettings =>
  JSON.parse(JSON.stringify(settings)) as ProjectTemplateSettings;

const normalizeServiceItems = (
  serviceItems: ServiceItemTemplate[]
): ServiceItemTemplate[] =>
  serviceItems.map((item) => {
    if (Array.isArray(item.products) && item.products.length > 0) {
      return item;
    }
    const legacyStageTemplateIds = (item as any).stageTemplateIds;
    return {
      ...item,
      products: [
        {
          productId: 1,
          productName: "預設產品",
          description: "",
          stageTemplateIds: Array.isArray(legacyStageTemplateIds) ? legacyStageTemplateIds : [],
        },
      ],
    };
  });

const applyJointDefenseProducts = (
  settings: ProjectTemplateSettings
): ProjectTemplateSettings => {
  const nextItems = settings.serviceItems.map((item) => {
    if (item.serviceItemName !== "聯防設備") return item;
    const hasOnlyDefault =
      item.products.length === 0 ||
      item.products.every((product) => product.productName === "預設產品");
    if (!hasOnlyDefault) return item;
    return {
      ...item,
      products: jointDefenseProducts.map((product, index) => ({
        productId: index + 1,
        productName: product.productName,
        description: product.description,
        stageTemplateIds: [2],
      })),
    };
  });
  return { ...settings, serviceItems: nextItems };
};

const getConsultantDefaultStages = (): TemplateStage[] =>
  cloneSettings(defaultSettings).stageTemplates.find((t) => t.stageTemplateId === 3)?.stages ?? [];

const applyConsultantStageDefaults = (
  settings: ProjectTemplateSettings
): ProjectTemplateSettings => {
  const consultantStages = getConsultantDefaultStages();
  if (consultantStages.length === 0) return settings;
  const nextTemplates = settings.stageTemplates.map((template) => {
    if (template.stageTemplateId !== 3) return template;
    const shouldReplace =
      template.stages.length === 0 ||
      (template.stages.length <= 1 && template.stages[0]?.stageName === "ISMS");
    if (!shouldReplace) return template;
    return {
      ...template,
      stages: consultantStages.map((stage) => ({ ...stage })),
    };
  });
  return { ...settings, stageTemplates: nextTemplates };
};

const normalizeStageTemplates = (stageTemplates: StageTemplate[]): StageTemplate[] =>
  stageTemplates.map((template) => ({
    ...template,
    stages: template.stages.map((stage) => ({
      ...stage,
      onsiteHours: stage.onsiteHours,
      offsiteHours: stage.offsiteHours,
      deadlineDays: stage.deadlineDays,
      deadlineAnchor: stage.deadlineAnchor,
    })),
  }));

export const loadProjectTemplateSettings = (): ProjectTemplateSettings => {
  const raw = localStorage.getItem(STORAGE_KEY);
  if (!raw) return cloneSettings(defaultSettings);
  try {
    const parsed = JSON.parse(raw) as ProjectTemplateSettings;
    if ((parsed as any).contractTypes) {
      const migrated = cloneSettings(defaultSettings);
      if ((parsed as any)?.reminderRule) {
        migrated.reminderRule = (parsed as any).reminderRule;
      }
      saveProjectTemplateSettings(migrated);
      return cloneSettings(migrated);
    }
    if (!parsed?.projectTypes || !parsed?.serviceItems || !parsed?.stageTemplates) {
      const migrated = applyConsultantStageDefaults(
        applyJointDefenseProducts(cloneSettings(defaultSettings))
      );
      saveProjectTemplateSettings(migrated);
      return cloneSettings(migrated);
    }
    if (parsed?.schemaVersion !== CURRENT_SCHEMA_VERSION) {
      const migrated = applyConsultantStageDefaults(
        applyJointDefenseProducts({
          ...parsed,
          schemaVersion: CURRENT_SCHEMA_VERSION,
          serviceItems: normalizeServiceItems(parsed.serviceItems),
          stageTemplates: normalizeStageTemplates(parsed.stageTemplates),
        })
      );
      saveProjectTemplateSettings(migrated);
      return cloneSettings(migrated);
    }
    return cloneSettings(
      applyConsultantStageDefaults(
        applyJointDefenseProducts({
          ...parsed,
          schemaVersion: CURRENT_SCHEMA_VERSION,
          serviceItems: normalizeServiceItems(parsed.serviceItems),
          stageTemplates: normalizeStageTemplates(parsed.stageTemplates),
        })
      )
    );
  } catch {
    return cloneSettings(defaultSettings);
  }
};

export const saveProjectTemplateSettings = (settings: ProjectTemplateSettings) => {
  const nextSettings = { ...settings, schemaVersion: CURRENT_SCHEMA_VERSION };
  localStorage.setItem(STORAGE_KEY, JSON.stringify(nextSettings));
};

export const getStageTemplatesByServiceItems = (
  settings: ProjectTemplateSettings,
  serviceItemIds: number[]
): StageTemplate[] => {
  const templateIds = new Set<number>();
  settings.serviceItems
    .filter((item) => serviceItemIds.includes(item.serviceItemId))
    .forEach((item) => {
      item.products.forEach((product) => {
        product.stageTemplateIds.forEach((id) => templateIds.add(id));
      });
    });
  return settings.stageTemplates.filter((template) => templateIds.has(template.stageTemplateId));
};

export const useProjectTemplateSettingsStore = () => ({
  loadProjectTemplateSettings,
  saveProjectTemplateSettings,
});
