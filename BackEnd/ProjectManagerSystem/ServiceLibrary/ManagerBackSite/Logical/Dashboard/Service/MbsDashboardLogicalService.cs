using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.Dashboard;
using CommonLibrary.CmnEnum;
using CommonLibrary.CmnEnum.Extension;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.ManagerRole;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;

namespace ServiceLibrary.ManagerBackSite.Logical.Dashboard.Service;

/// <summary>管理者後台-儀表板-邏輯服務</summary>
public class MbsDashboardLogicalService : IMbsDashboardLogicalService
{
    private readonly ILogger<MbsDashboardLogicalService> _logger;
    private readonly ICoEmpLoginInfoMemoryService _coEmpLoginInfoMemory;
    private readonly ProjectManagerMainContext _mainContext;

    public MbsDashboardLogicalService(
        ILogger<MbsDashboardLogicalService> logger,
        ICoEmpLoginInfoMemoryService coEmpLoginInfoMemory,
        ProjectManagerMainContext mainContext)
    {
        _logger = logger;
        _coEmpLoginInfoMemory = coEmpLoginInfoMemory;
        _mainContext = mainContext;
    }

    /// <summary>取得資訊</summary>
    public async Task<MbsDashboardCtlGetInfoRspMdl> GetInfoAsync(MbsDashboardCtlGetInfoReqMdl reqObj)
    {
        var rspObj = new MbsDashboardCtlGetInfoRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // 1. 驗證登入
        var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl { EmployeeLoginToken = reqObj.EmployeeLoginToken };
        var loginInfo = _coEmpLoginInfoMemory.Get(coEmpLiMemGetReqObj);
        if (loginInfo == default)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
            return rspObj;
        }

        try
        {
            int employeeId = loginInfo.EmployeeID;
            // 取得員工資訊以判斷角色
            var employee = await _mainContext.Employee.FindAsync(employeeId);
            if (employee == null)
            {
                return rspObj;
            }

            int roleId = employee.ManagerRoleID;
            var userRole = await _mainContext.ManagerRole.FindAsync(roleId);
            
            // 判斷是否為 Admin 或 總經理 (-1 or -99 usually, or by name? Let's assume permission check allows them to see all)
            // 這裡簡單實作：如果有傳入 RegionID，就過濾。
            
            // 2. 統計數據

            // 2.1 總毛利 (公司)
            // 需 join 取得 Region
            var grossProfitQuery = from p in _mainContext.EmployeePipelineProduct
                                   join ppl in _mainContext.EmployeePipeline on p.EmployeePipelineID equals ppl.ID
                                   join saler in _mainContext.EmployeePipelineSaler on ppl.ID equals saler.EmployeePipelineID
                                   join emp in _mainContext.Employee on saler.SalerEmployeeID equals emp.ID
                                   join role in _mainContext.ManagerRole on emp.ManagerRoleID equals role.ID
                                   where ppl.AtomPipelineStatusID == DbAtomPipelineStatusEnum.Business100Percent.ToInt16() ||
                                         ppl.AtomPipelineStatusID == DbAtomPipelineStatusEnum.TransferredToProject.ToInt16()
                                   select new { Product = p, RegionID = role.ManagerRegionID, PipelineID = ppl.ID };

            // Apply Region Filter
            if (reqObj.RegionID.HasValue)
            {
                grossProfitQuery = grossProfitQuery.Where(x => x.RegionID == reqObj.RegionID.Value);
            }

            // Distinct by Product ID to avoid double counting if multiple salers? 
            // EmployeePipelineSaler might have multiple entries for same pipeline?
            // Usually one main saler. If multiple, we might double count product value if we don't group.
            // Let's GroupBy Product ID.
            var totalGrossProfit = await grossProfitQuery
                .GroupBy(x => x.Product.ID)
                .Select(g => g.FirstOrDefault().Product)
                .SumAsync(x => (x.ClosingPrice - x.CostPrice) * x.Count);

            rspObj.TotalGrossProfit = totalGrossProfit;

            // 2.2 個人毛利 (不受 RegionID 影響)
            var personalGrossProfit = await (from p in _mainContext.EmployeePipelineProduct
                                             join ppl in _mainContext.EmployeePipeline on p.EmployeePipelineID equals ppl.ID
                                             join saler in _mainContext.EmployeePipelineSaler on ppl.ID equals saler.EmployeePipelineID
                                             where saler.SalerEmployeeID == employeeId &&
                                                   (ppl.AtomPipelineStatusID == DbAtomPipelineStatusEnum.Business100Percent.ToInt16() ||
                                                    ppl.AtomPipelineStatusID == DbAtomPipelineStatusEnum.TransferredToProject.ToInt16())
                                             select (p.ClosingPrice - p.CostPrice) * p.Count)
                                             .SumAsync();

            rspObj.PersonalGrossProfit = personalGrossProfit;

            // 2.3 個人商機數
            var personalPipelineCount = await _mainContext.EmployeePipelineSaler
                .AsNoTracking()
                .Where(x => x.SalerEmployeeID == employeeId)
                .Select(x => x.EmployeePipelineID)
                .Distinct()
                .CountAsync();

            rspObj.PersonalPipelineCount = personalPipelineCount;

            // 2.4 個人專案數
            var personalProjectCount = await _mainContext.EmployeeProjectMember
                .AsNoTracking()
                .Where(x => x.EmployeeID == employeeId)
                .Select(x => x.EmployeeProjectID)
                .Distinct()
                .CountAsync();

            rspObj.PersonalProjectCount = personalProjectCount;
            
            // 2.5 專案風險統計 (公司)
            // Join to filter by Region
            var projectQuery = from prj in _mainContext.EmployeeProject
                               join pm in _mainContext.EmployeeProjectMember on prj.ID equals pm.EmployeeProjectID
                               join emp in _mainContext.Employee on pm.EmployeeID equals emp.ID
                               join role in _mainContext.ManagerRole on emp.ManagerRoleID equals role.ID
                               select new { Project = prj, RegionID = role.ManagerRegionID };

            if (reqObj.RegionID.HasValue)
            {
                projectQuery = projectQuery.Where(x => x.RegionID == reqObj.RegionID.Value);
            }

            var projectRiskStats = await projectQuery
                .GroupBy(x => x.Project.ID) // Distinct Projects
                .Select(g => g.FirstOrDefault().Project)
                .GroupBy(x => x.AtomEmployeeProjectStatusID)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            rspObj.ProjectRiskStatistics = projectRiskStats
                .Select(x => new MbsDashboardCtlGetInfoRspRiskItemMdl
                {
                    Status = (DbAtomEmployeeProjectStatusEnum)x.Status,
                    Count = x.Count
                })
                .ToList();

            // 2.6 個人專案風險統計
            var personalRiskStats = await (from pm in _mainContext.EmployeeProjectMember
                                           join prj in _mainContext.EmployeeProject on pm.EmployeeProjectID equals prj.ID
                                           where pm.EmployeeID == employeeId
                                           group prj by prj.AtomEmployeeProjectStatusID into g
                                           select new { Status = g.Key, Count = g.Count() })
                                           .ToListAsync();

            rspObj.PersonalProjectRiskStatistics = personalRiskStats
                .Select(x => new MbsDashboardCtlGetInfoRspRiskItemMdl
                {
                    Status = (DbAtomEmployeeProjectStatusEnum)x.Status,
                    Count = x.Count
                })
                .ToList();

            var pipelineStageStatuses = new[]
            {
                DbAtomPipelineStatusEnum.Business10Percent,
                DbAtomPipelineStatusEnum.Business30Percent,
                DbAtomPipelineStatusEnum.Business70Percent,
                DbAtomPipelineStatusEnum.Business100Percent
            };

            var pipelineDetailQuery =
                from ppl in _mainContext.EmployeePipeline.AsNoTracking()
                join company in _mainContext.ManagerCompany on ppl.ManagerCompanyID equals company.ID into companyJoin
                from company in companyJoin.DefaultIfEmpty()
                join saler in _mainContext.Employee on ppl.SalerEmployeeID equals saler.ID into salerJoin
                from saler in salerJoin.DefaultIfEmpty()
                join role in _mainContext.ManagerRole on saler.ManagerRoleID equals role.ID into roleJoin
                from role in roleJoin.DefaultIfEmpty()
                join product in _mainContext.EmployeePipelineProduct on ppl.ID equals product.EmployeePipelineID into productGroup
                where pipelineStageStatuses.Select(x => x.ToInt16()).Contains(ppl.AtomPipelineStatusID)
                select new
                {
                    ppl.ID,
                    ppl.AtomPipelineStatusID,
                    CompanyName = company != null ? company.Name : null,
                    OwnerName = saler != null ? saler.Name : null,
                    OwnerId = saler != null ? (int?)saler.ID : null,
                    RegionID = role != null ? (int?)role.ManagerRegionID : null,
                    EstimatedAmount = productGroup.Sum(p => (decimal?)(p.ClosingPrice * p.Count)) ?? 0m
                };

            var companyPipelineDetails = await pipelineDetailQuery
                .Where(x => reqObj.RegionID.HasValue == false || x.RegionID == reqObj.RegionID.Value)
                .ToListAsync();

            var personalPipelineDetails = await pipelineDetailQuery
                .Where(x => x.OwnerId.HasValue && x.OwnerId.Value == employeeId)
                .ToListAsync();

            rspObj.PipelineStageStatistics = pipelineStageStatuses
                .Select(status => new MbsDashboardCtlGetInfoRspPipelineStageItemMdl
                {
                    Status = status,
                    Count = companyPipelineDetails.Count(x => x.AtomPipelineStatusID == status.ToInt16())
                })
                .ToList();

            rspObj.PersonalPipelineStageStatistics = pipelineStageStatuses
                .Select(status => new MbsDashboardCtlGetInfoRspPipelineStageItemMdl
                {
                    Status = status,
                    Count = personalPipelineDetails.Count(x => x.AtomPipelineStatusID == status.ToInt16())
                })
                .ToList();

            rspObj.PipelineStageDetails = companyPipelineDetails
                .Select(x => new MbsDashboardCtlGetInfoRspPipelineStageDetailMdl
                {
                    EmployeePipelineID = x.ID,
                    Status = (DbAtomPipelineStatusEnum)x.AtomPipelineStatusID,
                    ManagerCompanyName = x.CompanyName?.Trim(),
                    OwnerName = x.OwnerName?.Trim(),
                    EstimatedAmount = x.EstimatedAmount
                })
                .ToList();

            rspObj.PersonalPipelineStageDetails = personalPipelineDetails
                .Select(x => new MbsDashboardCtlGetInfoRspPipelineStageDetailMdl
                {
                    EmployeePipelineID = x.ID,
                    Status = (DbAtomPipelineStatusEnum)x.AtomPipelineStatusID,
                    ManagerCompanyName = x.CompanyName?.Trim(),
                    OwnerName = x.OwnerName?.Trim(),
                    EstimatedAmount = x.EstimatedAmount
                })
                .ToList();

            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get Dashboard Info Error");
            return rspObj; // Fail
        }

        return rspObj;
    }
}
