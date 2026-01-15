import { AxiosError } from "axios";
import { apiJsonClient } from "@/services/httpClient";
import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";

/** 儀表板 API 路徑前綴 */
const API_PREFIX = "/api/ManagerBackSite/MbsDashboard";

//#region 取得資訊

/** 取得資訊-請求模型 (由 HTTP Utility 自動注入 Token，此處定義為空或僅包含必要欄位) */
export interface MbsDashboardHttpGetInfoReqMdl {
  /** 區域 ID (過濾用) */
  RegionID?: number | null;
}

/** 專案風險統計項目 */
export interface MbsDashboardHttpGetInfoRspRiskItemMdl {
  /** 專案狀態 */
  Status: DbAtomEmployeeProjectStatusEnum;
  /** 數量 */
  Count: number;
}

/** 商機階段統計項目 */
export interface MbsDashboardHttpPipelineStageItemMdl {
  /** 商機階段 */
  Status: DbAtomPipelineStatusEnum;
  /** 數量 */
  Count: number;
}

/** 商機階段明細 */
export interface MbsDashboardHttpPipelineStageDetailMdl {
  /** 商機ID */
  EmployeePipelineID: number;
  /** 階段 */
  Status: DbAtomPipelineStatusEnum;
  /** 客戶名稱 */
  ManagerCompanyName: string | null;
  /** 業務姓名 */
  OwnerName: string | null;
  /** 預估金額 */
  EstimatedAmount: number;
}

/** 取得資訊-回應模型 */
export interface MbsDashboardHttpGetInfoRspMdl {
  /** 錯誤代碼 */
  ErrorCode: MbsErrorCodeEnum;
  /** 總毛利 (公司) */
  TotalGrossProfit: number;
  /** 個人毛利 */
  PersonalGrossProfit: number;
  /** 個人商機數 */
  PersonalPipelineCount: number;
  /** 個人專案數 */
  PersonalProjectCount: number;
  /** 部門專案數 */
  DepartmentProjectCount: number;
  /** 專案風險統計 (公司) */
  ProjectRiskStatistics: MbsDashboardHttpGetInfoRspRiskItemMdl[];
  /** 個人專案風險統計 */
  PersonalProjectRiskStatistics: MbsDashboardHttpGetInfoRspRiskItemMdl[];
  /** 部門專案風險統計 */
  DepartmentProjectRiskStatistics: MbsDashboardHttpGetInfoRspRiskItemMdl[];
  /** 商機階段統計 (公司) */
  PipelineStageStatistics: MbsDashboardHttpPipelineStageItemMdl[];
  /** 商機階段統計 (個人) */
  PersonalPipelineStageStatistics: MbsDashboardHttpPipelineStageItemMdl[];
  /** 商機階段明細 (公司) */
  PipelineStageDetails: MbsDashboardHttpPipelineStageDetailMdl[];
  /** 商機階段明細 (個人) */
  PersonalPipelineStageDetails: MbsDashboardHttpPipelineStageDetailMdl[];
}

/** 取得儀表板資訊 */
export const mbsDashboardHttpGetInfo = async (
  data: MbsDashboardHttpGetInfoReqMdl
): Promise<MbsDashboardHttpGetInfoRspMdl> => {
  const useMockData = import.meta.env.VITE_USE_MOCK_DATA === "true";
  const cacheKey = "cache.dashboard.getInfo";
  const readCache = () => {
    const raw = localStorage.getItem(cacheKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as MbsDashboardHttpGetInfoRspMdl;
    } catch {
      return null;
    }
  };
  const writeCache = (payload: MbsDashboardHttpGetInfoRspMdl) => {
    localStorage.setItem(cacheKey, JSON.stringify(payload));
  };
  const buildFallback = (): MbsDashboardHttpGetInfoRspMdl => {
    const pipelineCounts = mockDataSets.crmPipelines.reduce<Record<number, number>>((acc, item) => {
      acc[item.status] = (acc[item.status] ?? 0) + 1;
      return acc;
    }, {});
    const pipelineStageStatistics = Object.entries(pipelineCounts).map(([status, count]) => ({
      Status: Number(status) as DbAtomPipelineStatusEnum,
      Count: count,
    }));
    const pipelineStageDetails = mockDataSets.crmPipelines.map((item) => ({
      EmployeePipelineID: item.id,
      Status: item.status as DbAtomPipelineStatusEnum,
      ManagerCompanyName: item.companyName,
      OwnerName: item.salerEmployeeName,
      EstimatedAmount: 0,
    }));

    return {
      ErrorCode: MbsErrorCodeEnum.Success,
      TotalGrossProfit: 0,
      PersonalGrossProfit: 0,
      PersonalPipelineCount: pipelineStageDetails.length,
      PersonalProjectCount: 0,
      DepartmentProjectCount: 0,
      ProjectRiskStatistics: [],
      PersonalProjectRiskStatistics: [],
      DepartmentProjectRiskStatistics: [],
      PipelineStageStatistics: pipelineStageStatistics,
      PersonalPipelineStageStatistics: pipelineStageStatistics,
      PipelineStageDetails: pipelineStageDetails,
      PersonalPipelineStageDetails: pipelineStageDetails,
    };
  };

  try {
    const httpResponse = await apiJsonClient.post<MbsDashboardHttpGetInfoRspMdl>(
      `${API_PREFIX}/GetInfo`,
      data
    );
    const payload = httpResponse.data;
    if (payload) {
      writeCache(payload);
      const cached = readCache();
      if (useMockData && cached) {
        return cached;
      }
      return payload;
    }
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  } catch (error) {
    const axiosError = error as AxiosError;
    console.warn("Dashboard API fallback:", axiosError.response?.data ?? error);
    const cached = readCache();
    if (cached) return cached;
    return buildFallback();
  }
};

//#endregion
