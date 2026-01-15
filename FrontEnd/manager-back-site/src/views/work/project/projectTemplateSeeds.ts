export type OutputKind = "document" | "meeting" | "report" | "other";

export interface ProjectTemplateOutput {
  outputId: number;
  outputName: string;
  outputKind: OutputKind;
  required: boolean;
}

export interface ProjectTemplateStage {
  stageId: number;
  stageName: string;
  ownerRoleLabel: string;
  requiredOutputs: ProjectTemplateOutput[];
  requiresWorkLog: boolean;
  requiresOnsiteLog: boolean;
}

export interface ProjectTypeTemplate {
  projectTypeId: number;
  projectTypeName: string;
  description: string;
  contractTemplateId: number;
  stageTemplates: ProjectTemplateStage[];
}

export interface ContractTemplate {
  contractTemplateId: number;
  contractTemplateName: string;
  description: string;
}

export const reminderRuleSeed = {
  supervisorNotifyBeforeDays: 5,
  gmNotifyBeforeDays: 3,
};

export const contractTemplateSeeds: ContractTemplate[] = [
  {
    contractTemplateId: 1,
    contractTemplateName: "標準系統導入合約",
    description: "適用於軟體/系統導入型專案。",
  },
  {
    contractTemplateId: 2,
    contractTemplateName: "標準維運合約",
    description: "適用於駐點維運與長期維護。",
  },
  {
    contractTemplateId: 3,
    contractTemplateName: "標準顧問合約",
    description: "適用於顧問輔導與顧問型服務。",
  },
];

export const projectTypeTemplateSeeds: ProjectTypeTemplate[] = [
  {
    projectTypeId: 1,
    projectTypeName: "系統導入",
    description: "標準導入流程，含啟動會議與驗收。",
    contractTemplateId: 1,
    stageTemplates: [
      {
        stageId: 101,
        stageName: "啟動準備",
        ownerRoleLabel: "專案經理",
        requiredOutputs: [
          { outputId: 1001, outputName: "工作計劃書", outputKind: "document", required: true },
          { outputId: 1002, outputName: "啟動會議紀錄", outputKind: "meeting", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: false,
      },
      {
        stageId: 102,
        stageName: "系統建置",
        ownerRoleLabel: "執行者",
        requiredOutputs: [
          { outputId: 1003, outputName: "建置報告", outputKind: "report", required: true },
          { outputId: 1004, outputName: "測試結果", outputKind: "report", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: false,
      },
      {
        stageId: 103,
        stageName: "驗收與移交",
        ownerRoleLabel: "部門主管",
        requiredOutputs: [
          { outputId: 1005, outputName: "驗收文件", outputKind: "document", required: true },
          { outputId: 1006, outputName: "移交清單", outputKind: "document", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: false,
      },
    ],
  },
  {
    projectTypeId: 2,
    projectTypeName: "駐點維運",
    description: "以駐點人力維運為主，需每日工作日誌。",
    contractTemplateId: 2,
    stageTemplates: [
      {
        stageId: 201,
        stageName: "進場準備",
        ownerRoleLabel: "專案經理",
        requiredOutputs: [
          { outputId: 2001, outputName: "進場計畫", outputKind: "document", required: true },
          { outputId: 2002, outputName: "作業排程", outputKind: "document", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: true,
      },
      {
        stageId: 202,
        stageName: "駐點執行",
        ownerRoleLabel: "執行者",
        requiredOutputs: [
          { outputId: 2003, outputName: "每日工作日誌", outputKind: "report", required: true },
          { outputId: 2004, outputName: "駐點工作內容", outputKind: "report", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: true,
      },
      {
        stageId: 203,
        stageName: "結案回顧",
        ownerRoleLabel: "部門主管",
        requiredOutputs: [
          { outputId: 2005, outputName: "服務報告", outputKind: "report", required: true },
          { outputId: 2006, outputName: "改善建議", outputKind: "report", required: false },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: false,
      },
    ],
  },
  {
    projectTypeId: 3,
    projectTypeName: "顧問輔導",
    description: "以顧問交付為主，強調文件產出與里程碑。",
    contractTemplateId: 3,
    stageTemplates: [
      {
        stageId: 301,
        stageName: "需求訪談",
        ownerRoleLabel: "專案經理",
        requiredOutputs: [
          { outputId: 3001, outputName: "需求訪談紀錄", outputKind: "meeting", required: true },
          { outputId: 3002, outputName: "需求確認書", outputKind: "document", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: true,
      },
      {
        stageId: 302,
        stageName: "方案設計",
        ownerRoleLabel: "執行者",
        requiredOutputs: [
          { outputId: 3003, outputName: "方案設計書", outputKind: "document", required: true },
          { outputId: 3004, outputName: "風險清單", outputKind: "report", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: false,
      },
      {
        stageId: 303,
        stageName: "成果交付",
        ownerRoleLabel: "部門主管",
        requiredOutputs: [
          { outputId: 3005, outputName: "成果報告", outputKind: "report", required: true },
          { outputId: 3006, outputName: "交付清單", outputKind: "document", required: true },
        ],
        requiresWorkLog: true,
        requiresOnsiteLog: false,
      },
    ],
  },
];
