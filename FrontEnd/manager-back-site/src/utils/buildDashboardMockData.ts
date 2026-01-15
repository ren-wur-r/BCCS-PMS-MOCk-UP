import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";
import { DbAtomPipelineStatusEnum } from "@/constants/DbAtomPipelineStatusEnum";
import { MbsErrorCodeEnum } from "@/constants/MbsErrorCodeEnum";
import type {
  MbsDashboardHttpGetInfoRspMdl,
  MbsDashboardHttpPipelineStageDetailMdl,
  MbsDashboardHttpPipelineStageItemMdl,
  MbsDashboardHttpGetInfoRspRiskItemMdl,
} from "@/services/pms-http/dashboard/dashboardHttp";

const buildPipelineDetails = (): MbsDashboardHttpPipelineStageDetailMdl[] => {
  const businessStages = new Set([
    DbAtomPipelineStatusEnum.Business10Percent,
    DbAtomPipelineStatusEnum.Business30Percent,
    DbAtomPipelineStatusEnum.Business70Percent,
    DbAtomPipelineStatusEnum.Business100Percent,
  ]);
  const details = mockDataSets.crmPipelines
    .filter((item) => businessStages.has(item.status as DbAtomPipelineStatusEnum))
    .slice(0, 6)
    .map((item, index) => ({
      EmployeePipelineID: item.id,
      Status: item.status as DbAtomPipelineStatusEnum,
      ManagerCompanyName: item.companyName,
      OwnerName: item.salerEmployeeName,
      EstimatedAmount: 500000 + index * 150000,
    }));

  if (details.length > 0) return details;

  return [
    {
      EmployeePipelineID: 1001,
      Status: DbAtomPipelineStatusEnum.Business10Percent,
      ManagerCompanyName: "台積電",
      OwnerName: "林家豪",
      EstimatedAmount: 650000,
    },
    {
      EmployeePipelineID: 1002,
      Status: DbAtomPipelineStatusEnum.Business30Percent,
      ManagerCompanyName: "聯發科",
      OwnerName: "陳志強",
      EstimatedAmount: 920000,
    },
    {
      EmployeePipelineID: 1003,
      Status: DbAtomPipelineStatusEnum.Business70Percent,
      ManagerCompanyName: "鴻海科技",
      OwnerName: "周雅婷",
      EstimatedAmount: 1450000,
    },
    {
      EmployeePipelineID: 1004,
      Status: DbAtomPipelineStatusEnum.Business100Percent,
      ManagerCompanyName: "廣達電腦",
      OwnerName: "黃俊傑",
      EstimatedAmount: 2100000,
    },
  ];
};

const buildPipelineStageStats = (
  details: MbsDashboardHttpPipelineStageDetailMdl[]
): MbsDashboardHttpPipelineStageItemMdl[] => {
  const counts = details.reduce<Record<number, number>>((acc, item) => {
    acc[item.Status] = (acc[item.Status] ?? 0) + 1;
    return acc;
  }, {});
  const orderedStatuses = [
    DbAtomPipelineStatusEnum.Business10Percent,
    DbAtomPipelineStatusEnum.Business30Percent,
    DbAtomPipelineStatusEnum.Business70Percent,
    DbAtomPipelineStatusEnum.Business100Percent,
  ];
  return orderedStatuses.map((status) => ({
    Status: status,
    Count: counts[status] ?? 0,
  }));
};

const buildProjectRiskStats = (): MbsDashboardHttpGetInfoRspRiskItemMdl[] => {
  const counts = mockDataSets.workProjects.reduce<Record<number, number>>((acc, item) => {
    acc[item.status] = (acc[item.status] ?? 0) + 1;
    return acc;
  }, {});
  return [
    {
      Status: DbAtomEmployeeProjectStatusEnum.Delayed,
      Count: counts[DbAtomEmployeeProjectStatusEnum.Delayed] ?? 1,
    },
    {
      Status: DbAtomEmployeeProjectStatusEnum.AtRisk,
      Count: counts[DbAtomEmployeeProjectStatusEnum.AtRisk] ?? 1,
    },
    {
      Status: DbAtomEmployeeProjectStatusEnum.OnSchedule,
      Count: counts[DbAtomEmployeeProjectStatusEnum.OnSchedule] ?? 2,
    },
  ];
};

export const buildDashboardMockData = (): MbsDashboardHttpGetInfoRspMdl => {
  const pipelineDetails = buildPipelineDetails();
  const pipelineStageStatistics = buildPipelineStageStats(pipelineDetails);
  const projectRiskStatistics = buildProjectRiskStats();
  const totalProjects = mockDataSets.workProjects.length;

  return {
    ErrorCode: MbsErrorCodeEnum.Success,
    TotalGrossProfit: 12600000,
    PersonalGrossProfit: 1850000,
    PersonalPipelineCount: pipelineDetails.length,
    PersonalProjectCount: Math.max(1, Math.min(3, totalProjects)),
    DepartmentProjectCount: Math.max(3, Math.min(6, totalProjects)),
    ProjectRiskStatistics: projectRiskStatistics,
    PersonalProjectRiskStatistics: projectRiskStatistics,
    DepartmentProjectRiskStatistics: projectRiskStatistics,
    PipelineStageStatistics: pipelineStageStatistics,
    PersonalPipelineStageStatistics: pipelineStageStatistics,
    PipelineStageDetails: pipelineDetails,
    PersonalPipelineStageDetails: pipelineDetails,
  };
};

export const normalizeDashboardData = (
  payload: MbsDashboardHttpGetInfoRspMdl | null | undefined
): MbsDashboardHttpGetInfoRspMdl => {
  const errorCode = (payload as { ErrorCode?: number; errorCode?: number } | null)?.ErrorCode ??
    (payload as { ErrorCode?: number; errorCode?: number } | null)?.errorCode;
  if (payload && errorCode === MbsErrorCodeEnum.Success) {
    return payload;
  }
  return buildDashboardMockData();
};
