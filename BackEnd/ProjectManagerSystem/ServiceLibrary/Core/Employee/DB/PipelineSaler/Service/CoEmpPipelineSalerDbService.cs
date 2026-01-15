using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Service;

/// <summary>核心-員工-商機業務-資料庫服務</summary>
public class CoEmpPipelineSalerDbService : ICoEmpPipelineSalerDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineSalerDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineSalerDbService(
        ILogger<CoEmpPipelineSalerDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機業務-資料庫-取得</summary>
    public async Task<CoEmpPplSlrDbGetRspMdl> GetAsync(CoEmpPplSlrDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineSaler
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineSalerID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplSlrDbGetRspMdl
            {
                EmployeePipelineSalerID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                EmployeePipelineSalerEmployeeID = item.SalerEmployeeID,
                EmployeePipelineSalerSalerReplyTime = item.SalerReplyTime,
                EmployeePipelineSalerStatus = item.Status.ToEnum<DbAtomEmployeePipelineSalerStatusEnum>(),
                EmployeePipelineSalerCreateEmployeeID = item.CreateEmployeeID,
                EmployeePipelineSalerRemark = item.Remark?.Trim(),
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

    /// <summary>核心-員工-商機業務-資料庫-取得多筆</summary>
    public async Task<CoEmpPplSlrDbGetManyRspMdl> GetManyAsync(CoEmpPplSlrDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineSaler
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    x.ID,
                    x.EmployeePipelineID,
                    x.SalerEmployeeID,
                    x.SalerReplyTime,
                    x.Status,
                    x.CreateEmployeeID,
                    x.Remark,
                    x.CreateTime
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPplSlrDbGetManyRspMdl
            {
                EmployeePipelineSalerList = itemList.Select(x => new CoEmpPplSlrDbGetManyRspItemMdl
                {
                    EmployeePipelineSalerID = x.ID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    EmployeePipelineSalerEmployeeID = x.SalerEmployeeID,
                    EmployeePipelineSalerReplyTime = x.SalerReplyTime,
                    EmployeePipelineSalerStatus = x.Status.ToEnum<DbAtomEmployeePipelineSalerStatusEnum>(),
                    EmployeePipelineSalerCreateEmployeeID = x.CreateEmployeeID,
                    EmployeePipelineSalerRemark = x.Remark?.Trim(),
                    EmployeePipelineSalerCreateTime = x.CreateTime
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

    /// <summary>核心-員工-商機業務-資料庫-新增</summary>
    public async Task<CoEmpPplSlrDbAddRspMdl> AddAsync(CoEmpPplSlrDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineSaler
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                SalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
                SalerReplyTime = reqObj.EmployeePipelineSalerSalerReplyTime,
                Status = reqObj.EmployeePipelineSalerStatus.ToInt16(),
                CreateEmployeeID = reqObj.CreateEmployeeID,
                Remark = reqObj.EmployeePipelineSalerRemark?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.EmployeePipelineSaler.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplSlrDbAddRspMdl
            {
                EmployeePipelineSalerID = item.ID
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

    /// <summary>核心-員工-商機業務-資料庫-更新</summary>
    public async Task<CoEmpPplSlrDbUpdateRspMdl> UpdateAsync(CoEmpPplSlrDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineSaler
                .Where(x => x.ID == reqObj.EmployeePipelineSalerID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.SalerEmployeeID, x => reqObj.EmployeePipelineSalerEmployeeID.HasValue ? reqObj.EmployeePipelineSalerEmployeeID.Value : x.SalerEmployeeID)
                    .SetProperty(x => x.SalerReplyTime, x => reqObj.EmployeePipelineSalerReplyTime.HasValue ? reqObj.EmployeePipelineSalerReplyTime.Value : x.SalerReplyTime)
                    .SetProperty(x => x.Status, x => reqObj.EmployeePipelineSalerStatus.HasValue ? reqObj.EmployeePipelineSalerStatus.Value.ToInt16() : x.Status)
                    .SetProperty(x => x.Remark, x => !string.IsNullOrWhiteSpace(reqObj.EmployeePipelineSalerRemark) ? reqObj.EmployeePipelineSalerRemark.Trim() : x.Remark)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplSlrDbUpdateRspMdl
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

    /// <summary>核心-員工-商機業務-資料庫-是否存在</summary>
    public async Task<CoEmpPplSlrDbExistsRspMdl> ExistsAsync(CoEmpPplSlrDbExistsReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.EmployeePipelineSaler
                .AsNoTracking()
                .AnyAsync(x => x.ID == reqObj.EmployeePipelineID &&
                (reqObj.EmployeePipelineSalerStatus.HasValue == false || x.Status == reqObj.EmployeePipelineSalerStatus.Value.ToInt16()) &&
                (reqObj.EmployeePipelineSalerEmployeeID.HasValue == false || x.SalerEmployeeID == reqObj.EmployeePipelineSalerEmployeeID.Value));

            var rspObj = new CoEmpPplSlrDbExistsRspMdl
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

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]</summary>
    public async Task<CoEmpPplSlrDbGetManyFromMbsCrmPplRspMdl> GetManyFromMbsCrmPplAsync(CoEmpPplSlrDbGetManyFromMbsCrmPplReqMdl reqObj)
    {
        try
        {
            var itemList = await (
                from s in this._mainContext.EmployeePipelineSaler
                                            .AsNoTracking()
                                            .Where(x =>
                                            // 商機ID
                                            x.EmployeePipelineID == reqObj.EmployeePipelineID &&
                                            // 商機業務-狀態
                                            (reqObj.EmployeePipelineSalerStatus.HasValue == false || x.Status == reqObj.EmployeePipelineSalerStatus.Value.ToInt16()) &&
                                            // 商機業務-業務員工ID
                                            (reqObj.EmployeePipelineSalerEmployeeID.HasValue == false || x.SalerEmployeeID == reqObj.EmployeePipelineSalerEmployeeID))
                from se in this._mainContext.Employee
                                            .AsNoTracking()
                                            .Where(x => x.ID == s.SalerEmployeeID)
                                            .DefaultIfEmpty()
                from ce in this._mainContext.Employee
                                            .AsNoTracking()
                                            .Where(x => x.ID == s.CreateEmployeeID)
                                            .DefaultIfEmpty()
                select new
                {
                    s.ID,
                    s.EmployeePipelineID,
                    s.SalerEmployeeID,
                    SalerName = se.Name,
                    s.SalerReplyTime,
                    s.Status,
                    CreateEmployeeName = ce.Name,
                    s.Remark,
                    s.CreateTime
                }
            )
            .OrderByDescending(x => x.ID)
            .ToListAsync();

            var rspObj = new CoEmpPplSlrDbGetManyFromMbsCrmPplRspMdl
            {
                EmployeePipelineSalerList = itemList.Select(x => new CoEmpPplSlrDbGetManyFromMbsCrmPplRspItemMdl
                {
                    EmployeePipelineSalerID = x.ID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    EmployeePipelineSalerEmployeeID = x.SalerEmployeeID,
                    EmployeePipelineSalerEmployeeName = x.SalerName?.Trim(),
                    EmployeePipelineSalerReplyTime = x.SalerReplyTime,
                    EmployeePipelineSalerStatus = x.Status.ToEnum<DbAtomEmployeePipelineSalerStatusEnum>(),
                    EmployeePipelineSalerCreateEmployeeName = x.CreateEmployeeName?.Trim(),
                    EmployeePipelineSalerRemark = x.Remark?.Trim(),
                    EmployeePipelineSalerCreateTime = x.CreateTime
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
}
