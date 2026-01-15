using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.EmployeePipeline;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Pipeline.Format;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Service;

/// <summary>核心-員工-商機-資料庫服務</summary>
public class CoEmpPipelineDbService : ICoEmpPipelineDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineDbService(
        ILogger<CoEmpPipelineDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機-資料庫-是否存在</summary>
    public async Task<CoEmpPplDbExistRspMdl> ExistAsync(CoEmpPplDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .AnyAsync(x => x.ID == reqObj.EmployeePipelineID);

            var rspObj = new CoEmpPplDbExistRspMdl
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

    /// <summary>核心-員工-商機-資料庫-取得</summary>
    public async Task<CoEmpPplDbGetRspMdl> GetAsync(CoEmpPplDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplDbGetRspMdl()
            {
                EmployeePipelineID = item.ID,
                ManagerActivityID = item.ManagerActivityID,
                ManagerCompanyID = item.ManagerCompanyID,
                ManagerCompanyLocationID = item.ManagerCompanyLocationID,
                AtomPipelineStatus = item.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                EmployeePipelineSalerEmployeeID = item.SalerEmployeeID,
                BusinessNeedStatus = item.BusinessNeedStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessTimelineStatus = item.BusinessTimelineStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessBudgetStatus = item.BusinessBudgetStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessPresentationStatus = item.BusinessPresentationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessQuotationStatus = item.BusinessQuotationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessNegotiationStatus = item.BusinessNegotiationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessStageRemark = item.BusinessStageRemark?.Trim()
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

    /// <summary>核心-員工-商機-資料庫-取得多筆</summary>
    public async Task<CoEmpPplDbGetManyRspMdl> GetManyAsync(CoEmpPplDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .Where(x => reqObj.EmployeePipelineIDList.Contains(x.ID))
                .Select(x => new
                {
                    EmployeePipelineID = x.ID,
                    x.ManagerActivityID,
                    x.ManagerCompanyID,
                    x.ManagerCompanyLocationID,
                    x.AtomPipelineStatusID,
                    x.SalerEmployeeID,
                    x.BusinessNeedStatusID,
                    x.BusinessTimelineStatusID,
                    x.BusinessBudgetStatusID,
                    x.BusinessPresentationStatusID,
                    x.BusinessQuotationStatusID,
                    x.BusinessNegotiationStatusID,
                    x.BusinessStageRemark
                })
                .ToListAsync();

            var rspObj = new CoEmpPplDbGetManyRspMdl
            {
                EmployeePipelineList = itemList.Select(x => new CoEmpPplDbGetManyRspItemMdl
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    ManagerActivityID = x.ManagerActivityID,
                    ManagerCompanyID = x.ManagerCompanyID,
                    ManagerCompanyLocationID = x.ManagerCompanyLocationID,
                    AtomPipelineStatus = x.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                    EmployeePipelineSalerEmployeeID = x.SalerEmployeeID,
                    BusinessNeedStatus = x.BusinessNeedStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                    BusinessTimelineStatus = x.BusinessTimelineStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                    BusinessBudgetStatus = x.BusinessBudgetStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                    BusinessPresentationStatus = x.BusinessPresentationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                    BusinessQuotationStatus = x.BusinessQuotationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                    BusinessNegotiationStatus = x.BusinessNegotiationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                    BusinessStageRemark = x.BusinessStageRemark?.Trim()
                }).ToList()
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

    /// <summary>核心-員工-商機-資料庫-取得多筆員工商機ID</summary>
    public async Task<CoEmpPplDbGetManyEmployeePipelineIDRspMdl> GetManyEmployeePipelineIDAsync(CoEmpPplDbGetManyEmployeePipelineIDReqMdl reqObj)
    {
        try
        {
            var employeePipelineIDList = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .Where(x =>
                    x.ManagerActivityID.HasValue &&
                    x.ManagerActivityID.Value == reqObj.ManagerActivityID)
                .Select(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPplDbGetManyEmployeePipelineIDRspMdl
            {
                EmployeePipelineIDList = employeePipelineIDList
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

    /// <summary>核心-員工-商機-資料庫-新增</summary>
    public async Task<CoEmpPplDbAddRspMdl> AddAsync(CoEmpPplDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipeline()
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                ManagerCompanyID = reqObj.ManagerCompanyID,
                ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
                AtomPipelineStatusID = reqObj.AtomPipelineStatus.ToInt16(),
                SalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };
            this._mainContext.EmployeePipeline.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplDbAddRspMdl()
            {
                EmployeePipelineID = item.ID
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

    /// <summary>核心-員工-商機-資料庫-新增多筆</summary>
    public async Task<CoEmpPplDbAddManyRspMdl> AddManyAsync(CoEmpPplDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineList
                        .Select(x => new EmployeePipeline()
                        {
                            ManagerActivityID = x.ManagerActivityID,
                            ManagerCompanyID = x.ManagerCompanyID,
                            ManagerCompanyLocationID = x.ManagerCompanyLocationID,
                            AtomPipelineStatusID = x.AtomPipelineStatus.ToInt16(),
                            CreateTime = currentTime,
                            UpdateTime = currentTime,
                        })
                        .ToList();

            this._mainContext.EmployeePipeline.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplDbAddManyRspMdl()
            {
                IsSuccess = true,
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

    /// <summary>核心-員工-商機-資料庫-更新</summary>
    public async Task<CoEmpPplDbUpdateRspMdl> UpdateAsync(CoEmpPplDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipeline
                .Where(x => x.ID == reqObj.EmployeePipelineID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.ManagerActivityID, x => reqObj.ManagerActivityID.HasValue ? reqObj.ManagerActivityID : x.ManagerActivityID)
                        .SetProperty(x => x.ManagerCompanyID, x => reqObj.ManagerCompanyID.HasValue ? reqObj.ManagerCompanyID : x.ManagerCompanyID)
                        .SetProperty(x => x.ManagerCompanyLocationID, x => reqObj.ManagerCompanyLocationID.HasValue ? reqObj.ManagerCompanyLocationID : x.ManagerCompanyLocationID)
                        .SetProperty(x => x.AtomPipelineStatusID, x => reqObj.AtomPipelineStatus.HasValue ? reqObj.AtomPipelineStatus.Value.ToInt16() : x.AtomPipelineStatusID)
                        .SetProperty(x => x.SalerEmployeeID, x =>
                            reqObj.EmployeePipelineSalerEmployeeID.HasValue
                                ? (reqObj.EmployeePipelineSalerEmployeeID.Value == -1
                                    ? null
                                    : reqObj.EmployeePipelineSalerEmployeeID.Value)
                                : x.SalerEmployeeID)
                        .SetProperty(x => x.BusinessNeedStatusID, x => reqObj.BusinessNeedStatus.HasValue ? reqObj.BusinessNeedStatus.Value.ToInt16() : x.BusinessNeedStatusID)
                        .SetProperty(x => x.BusinessTimelineStatusID, x => reqObj.BusinessTimelineStatus.HasValue ? reqObj.BusinessTimelineStatus.Value.ToInt16() : x.BusinessTimelineStatusID)
                        .SetProperty(x => x.BusinessBudgetStatusID, x => reqObj.BusinessBudgetStatus.HasValue ? reqObj.BusinessBudgetStatus.Value.ToInt16() : x.BusinessBudgetStatusID)
                        .SetProperty(x => x.BusinessPresentationStatusID, x => reqObj.BusinessPresentationStatus.HasValue ? reqObj.BusinessPresentationStatus.Value.ToInt16() : x.BusinessPresentationStatusID)
                        .SetProperty(x => x.BusinessQuotationStatusID, x => reqObj.BusinessQuotationStatus.HasValue ? reqObj.BusinessQuotationStatus.Value.ToInt16() : x.BusinessQuotationStatusID)
                        .SetProperty(x => x.BusinessNegotiationStatusID, x => reqObj.BusinessNegotiationStatus.HasValue ? reqObj.BusinessNegotiationStatus.Value.ToInt16() : x.BusinessNegotiationStatusID)
                        .SetProperty(x => x.BusinessStageRemark, x => reqObj.BusinessStageRemark != null ? reqObj.BusinessStageRemark.Trim() : x.BusinessStageRemark)
                        .SetProperty(x => x.BusinessStageUpdateEmployeeID, x => reqObj.BusinessStageUpdateEmployeeID.HasValue ? reqObj.BusinessStageUpdateEmployeeID : x.BusinessStageUpdateEmployeeID)
                        .SetProperty(x => x.BusinessStageUpdateTime, x => reqObj.UpdateBusinessStageTimestamp ? currentTime : x.BusinessStageUpdateTime)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplDbUpdateRspMdl()
            {
                AffectedCount = affectedCount
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

    /// <summary>核心-員工-商機-資料庫-批次更新商機狀態</summary>
    public async Task<CoEmpPplDbUpdateManyAtomPipelineStatusRspMdl> UpdateManyAtomPipelineStatusAsync(CoEmpPplDbUpdateManyAtomPipelineStatusReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipeline
                .Where(x => reqObj.EmployeePipelineIDList.Contains(x.ID))
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.AtomPipelineStatusID, reqObj.AtomPipelineStatus.ToInt16())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplDbUpdateManyAtomPipelineStatusRspMdl()
            {
                AffectedCount = affectedCount
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

    /// <summary>核心-員工-商機-資料庫-移除</summary>
    public async Task<CoEmpPplDbRemoveRspMdl> RemoveAsync(CoEmpPplDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipeline
                .Where(x => x.ID == reqObj.EmployeePipelineID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplDbRemoveRspMdl
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
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-員工-商機-資料庫-移除多筆</summary>
    public async Task<CoEmpPplDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipeline
                .Where(x => reqObj.EmployeePipelineIDList.Contains(x.ID))
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplDbRemoveManyRspMdl
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
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-員工-商機-資料庫-取得多筆[筆數]</summary>
    public async Task<CoEmpPplDbGetManyCountRspMdl> GetManyCountAsync(CoEmpPplDbGetManyCountReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .Where(x =>
                    x.ManagerActivityID.HasValue &&
                    reqObj.ManagerActivityIDList.Contains(x.ManagerActivityID.Value) &&
                    (reqObj.AtomPipelineStatus.HasValue == false || x.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16()))
                .GroupBy(x => x.ManagerActivityID)
                .Select(global => new
                {
                    ManagerActivityID = global.Key.Value,
                    Count = global.Count()
                })
                .ToListAsync();

            var rspObj = new CoEmpPplDbGetManyCountRspMdl
            {
                EmployeePipelineList = itemList.Select(item => new CoEmpPplDbGetManyCountRspItemMdl
                {
                    ManagerActivityID = item.ManagerActivityID,
                    Count = item.Count
                }).ToList()
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

    /// <summary>核心-員工-商機-資料庫-取得[筆數]</summary>
    public async Task<CoEmpPplDbGetCountRspMdl> GetCountAsync(CoEmpPplDbGetCountReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .CountAsync(x =>
                    x.ManagerActivityID.HasValue &&
                    x.ManagerActivityID.Value == reqObj.ManagerActivityID &&
                    (reqObj.AtomPipelineStatus.HasValue == false || x.AtomPipelineStatusID >= reqObj.AtomPipelineStatus.Value.ToInt16()));

            var rspObj = new CoEmpPplDbGetCountRspMdl
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

    #region ManagerBackSite.Crm.Phone 管理者後台-CRM-電銷管理

    /// <summary>核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]</summary>
    public async Task<CoEmpPplDbGetCountFromMbsCrmPhnRspMdl> GetCountFromMbsCrmPhnAsync(CoEmpPplDbGetCountFromMbsCrmPhnReqMdl reqObj)
    {
        try
        {
            var count = await (
                from ep in this._mainContext.EmployeePipeline
                    .Where(ep =>
                        ep.ManagerActivityID.HasValue &&
                        ep.ManagerActivityID.Value == reqObj.ManagerActivityID &&
                        // 商機狀態
                        (reqObj.AtomPipelineStatus.HasValue == false || ep.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16())
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Opened.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Clicked.ToInt16()
                    )
                from epc in this._mainContext.EmployeePipelineContacter
                    .Where(epc => epc.EmployeePipelineID == ep.ID &&
                                    epc.IsPrimary)
                    .DefaultIfEmpty()
                from ct in this._mainContext.ManagerContacter
                    .Where(ct =>
                        epc != null &&
                        ct.ID == epc.ManagerContacterID
                    )
                    .DefaultIfEmpty()
                from cmp in this._mainContext.ManagerCompany
                    .Where(cmp =>
                        ep.ManagerCompanyID.HasValue &&
                        cmp.ID == ep.ManagerCompanyID.Value
                    )
                    .DefaultIfEmpty()
                from epo in this._mainContext.EmployeePipelineOriginal
                    .Where(epo =>
                        epo.EmployeePipelineID == ep.ID
                    )
                    .DefaultIfEmpty()
                where
                    // 管理者公司名稱 - 必須在 cmp 或 epo 其中之一符合
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyName) ||
                    (cmp != null && cmp.Name.Contains(reqObj.ManagerCompanyName.Trim())) ||
                    (cmp == null && epo != null && epo.ManagerCompanyName.Contains(reqObj.ManagerCompanyName.Trim())))
                    &&
                    // 窗口姓名
                    (string.IsNullOrEmpty(reqObj.ManagerContacterName) ||
                    (ct != null && ct.Name.Contains(reqObj.ManagerContacterName.Trim())) ||
                    (ct == null && epo != null && epo.ManagerContacterName.Contains(reqObj.ManagerContacterName.Trim())))
                    &&
                    // 窗口Email
                    (string.IsNullOrEmpty(reqObj.ManagerContacterEmail) ||
                    (ct != null && ct.Email.Contains(reqObj.ManagerContacterEmail.Trim())) ||
                    (ct == null && epo != null && epo.ManagerContacterEmail.Contains(reqObj.ManagerContacterEmail.Trim())))
                select ep
            )
            .AsNoTracking()
            .CountAsync();

            var rspObj = new CoEmpPplDbGetCountFromMbsCrmPhnRspMdl
            {
                Count = count,
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

    /// <summary>核心-員工-商機-資料庫-取得多筆[管理者後台-CRM-電銷管理]</summary>
    public async Task<CoEmpPplDbGetManyFromMbsCrmPhnRspMdl> GetManyFromMbsCrmPhnAsync(CoEmpPplDbGetManyFromMbsCrmPhnReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await (
                from ep in this._mainContext.EmployeePipeline
                    .Where(ep =>
                        ep.ManagerActivityID.HasValue &&
                        ep.ManagerActivityID.Value == reqObj.ManagerActivityID &&
                        // 商機狀態
                        (reqObj.AtomPipelineStatus.HasValue == false || ep.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16())
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Opened.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Clicked.ToInt16()
                    )
                from epc in this._mainContext.EmployeePipelineContacter
                    .Where(epc => epc.EmployeePipelineID == ep.ID &&
                                    epc.IsPrimary)
                    .DefaultIfEmpty()
                from ct in this._mainContext.ManagerContacter
                    .Where(ct =>
                        epc != null &&
                        ct.ID == epc.ManagerContacterID
                    )
                    .DefaultIfEmpty()
                from cmp in this._mainContext.ManagerCompany
                    .Where(cmp =>
                        ep.ManagerCompanyID.HasValue &&
                        cmp.ID == ep.ManagerCompanyID.Value
                    )
                    .DefaultIfEmpty()
                from epo in this._mainContext.EmployeePipelineOriginal
                    .Where(epo =>
                        epo.EmployeePipelineID == ep.ID
                    )
                    .DefaultIfEmpty()
                where
                    // 管理者公司名稱 - 必須在 cmp 或 epo 其中之一符合
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyName) ||
                    (cmp != null && cmp.Name.Contains(reqObj.ManagerCompanyName.Trim())) ||
                    (cmp == null && epo != null && epo.ManagerCompanyName.Contains(reqObj.ManagerCompanyName.Trim())))
                    &&
                    // 窗口姓名
                    (string.IsNullOrEmpty(reqObj.ManagerContacterName) ||
                    (ct != null && ct.Name.Contains(reqObj.ManagerContacterName.Trim())) ||
                    (ct == null && epo != null && epo.ManagerContacterName.Contains(reqObj.ManagerContacterName.Trim())))
                    &&
                    // 窗口Email
                    (string.IsNullOrEmpty(reqObj.ManagerContacterEmail) ||
                    (ct != null && ct.Email.Contains(reqObj.ManagerContacterEmail.Trim())) ||
                    (ct == null && epo != null && epo.ManagerContacterEmail.Contains(reqObj.ManagerContacterEmail.Trim())))
                orderby ep.ID descending
                select new
                {
                    EmployeePipelineID = ep.ID,
                    ep.AtomPipelineStatusID,
                    HasCompany = cmp != null,
                    ManagerCompanyID = ep.ManagerCompanyID,
                    ManagerCompanyName = cmp != null ? cmp.Name : epo.ManagerCompanyName,
                    HasContacter = ct != null,
                    EmployeePipelineContacterID = epc != null ? (int?)epc.ID : null,
                    ManagerContacterDepartment = ct != null ? ct.Department : epo.ManagerContacterDepartment,
                    ManagerContacterJobTitle = ct != null ? ct.JobTitle : epo.ManagerContacterJobTitle,
                    ManagerContacterName = ct != null ? ct.Name : epo.ManagerContacterName,
                    ManagerContacterEmail = ct != null ? ct.Email : epo.ManagerContacterEmail,
                    ManagerContacterPhone = ct != null ? ct.Phone : epo.ManagerContacterPhone,
                    ManagerContacterTelephone = ct != null ? ct.Telephone : epo.ManagerContacterTelephone,
                }
            )
            .AsNoTracking()
            .Skip(skipRows)
            .Take(takeRows)
            .ToListAsync();

            var rspObj = new CoEmpPplDbGetManyFromMbsCrmPhnRspMdl
            {
                EmployeePipelineList = itemList.Select(x => new CoEmpPplDbGetManyFromMbsCrmPhnRspItemMdl
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    AtomPipelineStatus = x.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                    HasCompany = x.HasCompany,
                    ManagerCompanyID = x.ManagerCompanyID,
                    ManagerCompanyName = x.ManagerCompanyName?.Trim(),
                    HasContacter = x.HasContacter,
                    EmployeePipelineContacterID = x.EmployeePipelineContacterID,
                    ManagerContacterDepartment = x.ManagerContacterDepartment?.Trim(),
                    ManagerContacterJobTitle = x.ManagerContacterJobTitle?.Trim(),
                    ManagerContacterName = x.ManagerContacterName?.Trim(),
                    ManagerContacterEmail = x.ManagerContacterEmail?.Trim(),
                    ManagerContacterPhone = x.ManagerContacterPhone?.Trim(),
                    ManagerContacterTelephone = x.ManagerContacterTelephone?.Trim(),
                }).ToList()
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

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-商機管理]</summary>
    public async Task<CoEmpPplDbGetCountFromMbsCrmPipelineRspMdl> GetCountFromMbsCrmPipelineAsync(CoEmpPplDbGetCountFromMbsCrmPipelineReqMdl reqObj)
    {
        try
        {
            var count = await (
                from ep in this._mainContext.EmployeePipeline
                    .Where(ep =>
                        ep.SalerEmployeeID.HasValue &&
                        (reqObj.EmployeePipelineSalerEmployeeID.HasValue == false || ep.SalerEmployeeID == reqObj.EmployeePipelineSalerEmployeeID.Value) &&
                        // 商機狀態
                        (reqObj.AtomPipelineStatus.HasValue == false || ep.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16())
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Opened.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Clicked.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.TransferredToSales.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.CompletedSales.ToInt16()
                    )
                from epc in this._mainContext.EmployeePipelineContacter
                    .Where(epc => epc.EmployeePipelineID == ep.ID &&
                                    epc.IsPrimary)
                from ct in this._mainContext.ManagerContacter
                    .Where(ct =>
                        epc != null &&
                        ct.ID == epc.ManagerContacterID &&
                        // 窗口姓名
                        (string.IsNullOrEmpty(reqObj.ManagerContacterName) || ct.Name.Contains(reqObj.ManagerContacterName.Trim())) &&
                        // 窗口Email
                        (string.IsNullOrEmpty(reqObj.ManagerContacterEmail) || ct.Email.Contains(reqObj.ManagerContacterEmail.Trim()))
                    )
                from cmp in this._mainContext.ManagerCompany
                    .Where(cmp =>
                        ep.ManagerCompanyID.HasValue &&
                        cmp.ID == ep.ManagerCompanyID.Value &&
                        // 管理者公司名稱
                        (string.IsNullOrEmpty(reqObj.ManagerCompanyName) || cmp.Name.Contains(reqObj.ManagerCompanyName.Trim()))
                    )
                select ep
            )
            .AsNoTracking()
            .CountAsync();

            var rspObj = new CoEmpPplDbGetCountFromMbsCrmPipelineRspMdl
            {
                Count = count,
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

    /// <summary>核心-員工-商機-資料庫-取得多筆[管理者後台-CRM-商機管理]</summary>
    public async Task<CoEmpPplDbGetManyFromMbsCrmPipelineRspMdl> GetManyFromMbsCrmPipelineAsync(CoEmpPplDbGetManyFromMbsCrmPipelineReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await (
                from ep in this._mainContext.EmployeePipeline
                    .Where(ep =>
                        ep.SalerEmployeeID.HasValue &&
                        (reqObj.EmployeePipelineSalerEmployeeID.HasValue == false || ep.SalerEmployeeID == reqObj.EmployeePipelineSalerEmployeeID.Value) &&
                        // 商機狀態
                        (reqObj.AtomPipelineStatus.HasValue == false || ep.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16())
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Opened.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.Clicked.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.TransferredToSales.ToInt16()
                        && ep.AtomPipelineStatusID != DbAtomPipelineStatusEnum.CompletedSales.ToInt16()
                    )
                from epc in this._mainContext.EmployeePipelineContacter
                    .Where(epc => epc.EmployeePipelineID == ep.ID &&
                                    epc.IsPrimary)
                from ct in this._mainContext.ManagerContacter
                    .Where(ct =>
                        epc != null &&
                        ct.ID == epc.ManagerContacterID &&
                        // 窗口姓名
                        (string.IsNullOrEmpty(reqObj.ManagerContacterName) || ct.Name.Contains(reqObj.ManagerContacterName.Trim())) &&
                        // 窗口Email
                        (string.IsNullOrEmpty(reqObj.ManagerContacterEmail) || ct.Email.Contains(reqObj.ManagerContacterEmail.Trim()))
                    )
                from cmp in this._mainContext.ManagerCompany
                    .Where(cmp =>
                        ep.ManagerCompanyID.HasValue &&
                        cmp.ID == ep.ManagerCompanyID.Value &&
                        // 管理者公司名稱
                        (string.IsNullOrEmpty(reqObj.ManagerCompanyName) || cmp.Name.Contains(reqObj.ManagerCompanyName.Trim()))
                    )
                from saler in this._mainContext.Employee
                    .Where(e => ep.SalerEmployeeID.HasValue && e.ID == ep.SalerEmployeeID.Value)
                    .DefaultIfEmpty()
                orderby ep.ID descending
                select new
                {
                    EmployeePipelineID = ep.ID,
                    ep.AtomPipelineStatusID,
                    ManagerCompanyName = cmp.Name,
                    ManagerContacterDepartment = ct.Department,
                    ManagerContacterJobTitle = ct.JobTitle,
                    ManagerContacterName = ct.Name,
                    ManagerContacterEmail = ct.Email,
                    ManagerContacterPhone = ct.Phone,
                    ManagerContacterTelephone = ct.Telephone,
                    EmployeePipelineSalerEmployeeName = saler != null ? saler.Name : null,
                }
            )
            .AsNoTracking()
            .Skip(skipRows)
            .Take(takeRows)
            .ToListAsync();

            var rspObj = new CoEmpPplDbGetManyFromMbsCrmPipelineRspMdl
            {
                EmployeePipelineList = itemList.Select(x => new CoEmpPplDbGetManyFromMbsCrmPipelineRspItemMdl
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    AtomPipelineStatus = x.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                    ManagerCompanyName = x.ManagerCompanyName?.Trim(),
                    ManagerContacterDepartment = x.ManagerContacterDepartment?.Trim(),
                    ManagerContacterJobTitle = x.ManagerContacterJobTitle?.Trim(),
                    ManagerContacterName = x.ManagerContacterName?.Trim(),
                    ManagerContacterEmail = x.ManagerContacterEmail?.Trim(),
                    ManagerContacterPhone = x.ManagerContacterPhone?.Trim(),
                    ManagerContacterTelephone = x.ManagerContacterTelephone?.Trim(),
                    EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName?.Trim(),
                }).ToList()
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

    /// <summary>核心-員工-商機-資料庫-取得[管理者後台-CRM-商機管理]</summary>
    public async Task<CoEmpPplDbGetFromMbsCrmPipelineRspMdl> GetFromMbsCrmPipelineAsync(CoEmpPplDbGetFromMbsCrmPipelineReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipeline
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineID &&
                                x.SalerEmployeeID.HasValue &&
                                (reqObj.EmployeePipelineSalerEmployeeID.HasValue == false || x.SalerEmployeeID.Value == reqObj.EmployeePipelineSalerEmployeeID.Value));
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplDbGetFromMbsCrmPipelineRspMdl()
            {
                EmployeePipelineID = item.ID,
                ManagerActivityID = item.ManagerActivityID,
                ManagerCompanyID = item.ManagerCompanyID,
                ManagerCompanyLocationID = item.ManagerCompanyLocationID,
                AtomPipelineStatus = item.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                EmployeePipelineSalerEmployeeID = item.SalerEmployeeID,
                BusinessNeedStatus = item.BusinessNeedStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessTimelineStatus = item.BusinessTimelineStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessBudgetStatus = item.BusinessBudgetStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessPresentationStatus = item.BusinessPresentationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessQuotationStatus = item.BusinessQuotationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessNegotiationStatus = item.BusinessNegotiationStatusID?.ToEnum<DbEmployeePipelineStageCheckEnum>(),
                BusinessStageRemark = item.BusinessStageRemark?.Trim()
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
