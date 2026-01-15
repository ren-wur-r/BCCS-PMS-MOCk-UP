using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Project.Format;

namespace ServiceLibrary.Core.Employee.DB.Project.Service;

/// <summary>核心-員工-專案-資料庫服務</summary>
public class CoEmpProjectDbService : ICoEmpProjectDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectDbService(
        ILogger<CoEmpProjectDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案-資料庫-是否存在</summary>
    public async Task<CoEmpPrjDbExistRspMdl> ExistAsync(CoEmpPrjDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.EmployeeProject
                .AsNoTracking()
                .AnyAsync(x => (reqObj.EmployeeProjectID.HasValue == false || x.ID == reqObj.EmployeeProjectID.Value)
                && (string.IsNullOrWhiteSpace(reqObj.EmployeeProjectCode) || x.Code == reqObj.EmployeeProjectCode));

            var rspObj = new CoEmpPrjDbExistRspMdl
            {
                IsExist = isExist,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-員工-專案-資料庫-新增</summary>
    public async Task<CoEmpPrjDbAddRspMdl> AddAsync(CoEmpPrjDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProject
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                ManagerCompanyID = reqObj.ManagerCompanyID,
                AtomEmployeeProjectStatusID = reqObj.AtomEmployeeProjectStatus.ToInt16(),
                Code = reqObj.EmployeeProjectCode?.Trim() ?? string.Empty,
                Name = reqObj.EmployeeProjectName?.Trim() ?? string.Empty,
                StartTime = reqObj.EmployeeProjectStartTime.ToUniversalTime(),
                EndTime = reqObj.EmployeeProjectEndTime.ToUniversalTime(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProject.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPrjDbAddRspMdl
            {
                EmployeeProjectID = item.ID,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-員工-專案-資料庫-更新</summary>
    public async Task<CoEmpPrjDbUpdateRspMdl> UpdateAsync(CoEmpPrjDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProject
                .Where(x => x.ID == reqObj.EmployeeProjectID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.ManagerCompanyID, x => reqObj.ManagerCompanyID ?? x.ManagerCompanyID)
                        .SetProperty(x => x.AtomEmployeeProjectStatusID, x => reqObj.AtomEmployeeProjectStatus.HasValue == false ? x.AtomEmployeeProjectStatusID : reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                        .SetProperty(x => x.Code, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectCode) ? x.Code : reqObj.EmployeeProjectCode.Trim())
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectName) ? x.Name : reqObj.EmployeeProjectName.Trim())
                        .SetProperty(x => x.StartTime, x => reqObj.EmployeeProjectStartTime.HasValue == false ? x.StartTime : reqObj.EmployeeProjectStartTime.Value.ToUniversalTime())
                        .SetProperty(x => x.EndTime, x => reqObj.EmployeeProjectEndTime.HasValue == false ? x.EndTime : reqObj.EmployeeProjectEndTime.Value.ToUniversalTime())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPrjDbUpdateRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeProjectUpdateTime = currentTime,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-員工-專案-資料庫-更新多筆</summary>
    public async Task<CoEmpPrjDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPrjDbUpdateManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProject
                .Where(x => reqObj.EmployeeProjectIdList.Contains(x.ID))
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.AtomEmployeeProjectStatusID, x => reqObj.AtomEmployeeProjectStatus.HasValue == false ? x.AtomEmployeeProjectStatusID : reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPrjDbUpdateManyRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeProjectUpdateTime = currentTime,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-員工-專案-資料庫-移除</summary>
    public async Task<CoEmpPrjDbRemoveRspMdl> RemoveAsync(CoEmpPrjDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProject
                .Where(x => x.ID == reqObj.EmployeeProjectID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPrjDbRemoveRspMdl
            {
                AffectedCount = affectedCount,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-員工-專案-取得</summary>
    public async Task<CoEmpPrjDbGetRspMdl> GetAsync(CoEmpPrjDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProject
                .AsNoTracking()
                .Where(x =>
                    (reqObj.EmployeeProjectID.HasValue == false || x.ID == reqObj.EmployeeProjectID.Value)
                    && (reqObj.EmployeePipelineID.HasValue == false || x.EmployeePipelineID == reqObj.EmployeePipelineID.Value)
                )
                .SingleOrDefaultAsync();
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPrjDbGetRspMdl()
            {
                EmployeeProjectID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                AtomEmployeeProjectStatus = item.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
                ManagerCompanyID = item.ManagerCompanyID,
                EmployeeProjectCode = item.Code.Trim(),
                EmployeeProjectName = item.Name.Trim(),
                EmployeeProjectStartTime = item.StartTime.ToUniversalTime(),
                EmployeeProjectEndTime = item.EndTime.ToUniversalTime(),
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-員工-專案-取得多筆ID</summary>
    public async Task<CoEmpPrjDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPrjDbGetManyIdReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from ep in this._mainContext.EmployeeProject
                    where
                        reqObj.AtomEmployeeProjectStatus.HasValue == false || ep.AtomEmployeeProjectStatusID == reqObj.AtomEmployeeProjectStatus.Value.ToInt16()
                    select new
                    {
                        ep.ID,
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoEmpPrjDbGetManyIdRspMdl()
            {
                EmployeeProjectList = itemList
                    .Select(x => new CoEmpPrjDbGetManyIdRspItemMdl()
                    {
                        EmployeeProjectID = x.ID,
                    })
                    .ToList(),
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-員工-專案-取得多筆[名稱]</summary>
    public async Task<CoEmpPrjDbGetManyNameRspMdl> GetManyNameAsync(CoEmpPrjDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from ep in this._mainContext.EmployeeProject
                    where
                        reqObj.EmployeeProjectIdList.Contains(ep.ID)
                    select new
                    {
                        ep.ID,
                        ep.Name,
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoEmpPrjDbGetManyNameRspMdl()
            {
                EmployeeProjectList = itemList
                    .Select(x => new CoEmpPrjDbGetManyNameRspItemMdl()
                    {
                        EmployeeProjectID = x.ID,
                        EmployeeProjectName = x.Name?.Trim(),
                    })
                    .ToList(),
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }


    #region 管理後台-工作-專案

    /// <summary>核心-員工-專案-取得筆數從[管理後台-工作-專案]</summary>
    public async Task<CoEmpPrjDbGetCountFromMbsWrkPrjRspMdl> GetCountFromMbsWrkPrjAsync(CoEmpPrjDbGetCountFromMbsWrkPrjReqMdl reqObj)
    {
        try
        {
            var count = await
                (
                    from epm in this._mainContext.EmployeeProjectMember
                        .Where(epmx => epmx.EmployeeID == reqObj.EmployeeID)
                        .Select(x => x.EmployeeProjectID)
                        .Distinct()
                        .Select(x => new
                        {
                            EmployeeProjectID = x
                        })
                    from ep in this._mainContext.EmployeeProject
                        .Where(epx =>
                            epx.ID == epm.EmployeeProjectID
                            && (reqObj.AtomEmployeeProjectStatus.HasValue == false || epx.AtomEmployeeProjectStatusID == reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                            && (string.IsNullOrWhiteSpace(reqObj.EmployeeProjectName) || epx.Name.Contains(reqObj.EmployeeProjectName))
                        )
                    select 1
                ).CountAsync();

            var rspObj = new CoEmpPrjDbGetCountFromMbsWrkPrjRspMdl()
            {
                Count = count
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-員工-專案-取得多筆從[管理後台-工作-專案]</summary>
    public async Task<CoEmpPrjDbGetManyFromMbsWrkPrjRspMdl> GetManyFromMbsWrkPrjAsync(CoEmpPrjDbGetManyFromMbsWrkPrjReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from epm in this._mainContext.EmployeeProjectMember
                        .Where(epmx => epmx.EmployeeID == reqObj.EmployeeID)
                        .Select(x => x.EmployeeProjectID)
                        .Distinct()
                        .Select(x => new
                        {
                            EmployeeProjectID = x
                        })
                    from ep in this._mainContext.EmployeeProject
                        .Where(epx =>
                            epx.ID == epm.EmployeeProjectID
                            && (reqObj.AtomEmployeeProjectStatus.HasValue == false || epx.AtomEmployeeProjectStatusID == reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                            && (string.IsNullOrWhiteSpace(reqObj.EmployeeProjectName) || epx.Name.Contains(reqObj.EmployeeProjectName))
                        )
                    from mc in this._mainContext.ManagerCompany
                        .Where(mcx => mcx.ID == ep.ManagerCompanyID)
                    orderby ep.EndTime
                    select new
                    {
                        ep.ID,
                        ep.AtomEmployeeProjectStatusID,
                        ep.Name,
                        ManagerCompanyName = mc.Name,
                        ep.StartTime,
                        ep.EndTime,
                    }
                ).AsNoTracking()
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpPrjDbGetManyFromMbsWrkPrjRspMdl()
            {
                EmployeeProjectList = itemList
                    .Select(x => new CoEmpPrjDbGetManyFromMbsWrkPrjRspItemMdl()
                    {
                        EmployeeProjectID = x.ID,
                        ManagerCompanyName = x.ManagerCompanyName?.Trim(),
                        AtomEmployeeProjectStatus = x.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
                        EmployeeProjectName = x.Name?.Trim(),
                        EmployeeProjectStartTime = x.StartTime.ToUniversalTime(),
                        EmployeeProjectEndTime = x.EndTime.ToUniversalTime(),
                    })
                    .ToList(),
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-員工-專案-取得儀錶板從[管理後台-工作-專案]</summary>
    public async Task<CoEmpPrjDbGetDashboardFromMbsWrkPrjRspMdl> GetDashboardFromMbsWrkPrjAsync(CoEmpPrjDbGetDashboardFromMbsWrkPrjReqMdl reqObj)
    {
        var atomEmployeeProjectStatusIdList = reqObj.AtomEmployeeProjectStatusList
            .Select(x => x.ToInt16())
            .ToList();

        try
        {
            var itemList = await
                (
                    from epm in this._mainContext.EmployeeProjectMember
                        .Where(epmx => epmx.EmployeeID == reqObj.EmployeeID)
                        .Select(x => x.EmployeeProjectID)
                        .Distinct()
                        .Select(x => new
                        {
                            EmployeeProjectID = x
                        })
                    from ep in this._mainContext.EmployeeProject
                        .Where(epx =>
                            epx.ID == epm.EmployeeProjectID
                            && (atomEmployeeProjectStatusIdList.Contains(epx.AtomEmployeeProjectStatusID))
                        )
                    orderby ep.EndTime
                    select new
                    {
                        ep.ID,
                        ep.ManagerCompanyID,
                        ep.AtomEmployeeProjectStatusID,
                        ep.Name,
                        ep.StartTime,
                        ep.EndTime,
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoEmpPrjDbGetDashboardFromMbsWrkPrjRspMdl()
            {
                EmployeeProjectList = itemList
                    .Select(x => new CoEmpPrjDbGetDashboardFromMbsWrkPrjRspItemMdl()
                    {
                        EmployeeProjectID = x.ID,
                        ManagerCompanyID = x.ManagerCompanyID,
                        AtomEmployeeProjectStatus = x.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
                        EmployeeProjectName = x.Name?.Trim(),
                        EmployeeProjectStartTime = x.StartTime.ToUniversalTime(),
                        EmployeeProjectEndTime = x.EndTime.ToUniversalTime(),
                    })
                    .ToList(),
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    #endregion

    #region 管理者後台-基本

    /// <summary>核心-員工-專案-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoEmpPrjDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpPrjDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.EmployeeProject
                .AsNoTracking()
                .Where(x =>
                    string.IsNullOrEmpty(reqObj.EmployeeProjectName) || x.Name.Contains(reqObj.EmployeeProjectName.Trim())
                )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpPrjDbGetManyFromMbsBscRspMdl()
            {
                EmployeeProjectList = itemList
                .Select(x => new CoEmpPrjDbGetManyFromMbsBscRspItemMdl()
                {
                    EmployeeProjectID = x.ID,
                    EmployeeProjectName = x.Name?.Trim(),
                })
                .ToList(),
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    #endregion

    #region 排程-計時器

    /// <summary>核心-員工-專案-資料庫-取得多筆ID從[排程-計時器]</summary>
    public async Task<CoEmpPrjDbGetManyIdFromSchTmrRspMdl> GetManyIdFromSchTmrAsync(CoEmpPrjDbGetManyIdFromSchTmrReqMdl reqObj)
    {
        var atomEmployeeProjectStatusList = reqObj.AtomEmployeeProjectStatusList
            .Select(x => x.ToInt16())
            .ToList();

        try
        {
            var itemList = await this._mainContext.EmployeeProject
                .AsNoTracking()
                .Where(x => atomEmployeeProjectStatusList.Contains(x.AtomEmployeeProjectStatusID))
                .Select(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPrjDbGetManyIdFromSchTmrRspMdl()
            {
                EmployeeProjectList = itemList
                    .Select(id => new CoEmpPrjDbGetManyIdFromSchTmrRspItemMdl
                    {
                        EmployeeProjectID = id
                    })
                    .ToList()
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    #endregion

}
