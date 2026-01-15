using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineDue.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineDue.Service;

/// <summary>核心-員工-商機履約期限-資料庫服務</summary>
public class CoEmpPipelineDueDbService : ICoEmpPipelineDueDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineDueDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineDueDbService(
        ILogger<CoEmpPipelineDueDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機履約期限-資料庫-取得</summary>
    public async Task<CoEmpPplDueDbGetRspMdl> GetAsync(CoEmpPplDueDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineDue
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineDueID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplDueDbGetRspMdl
            {
                EmployeePipelineDueID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                EmployeePipelineDueTime = item.DueTime,
                EmployeePipelineDueRemark = item.Remark?.Trim(),
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

    /// <summary>核心-員工-商機履約期限-資料庫-取得多筆</summary>
    public async Task<CoEmpPplDueDbGetManyRspMdl> GetManyAsync(CoEmpPplDueDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineDue
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    x.ID,
                    x.EmployeePipelineID,
                    x.DueTime,
                    x.Remark,
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPplDueDbGetManyRspMdl
            {
                EmployeePipelineDueList = itemList.Select(x => new CoEmpPplDueDbGetManyRspItemMdl
                {
                    EmployeePipelineDueID = x.ID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    EmployeePipelineDueTime = x.DueTime,
                    EmployeePipelineDueRemark = x.Remark?.Trim(),
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

    /// <summary>核心-員工-商機履約期限-資料庫-新增</summary>
    public async Task<CoEmpPplDueDbAddRspMdl> AddAsync(CoEmpPplDueDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineDue
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                DueTime = reqObj.EmployeePipelineDueTime,
                Remark = reqObj.EmployeePipelineDueRemark?.Trim() ?? string.Empty,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeePipelineDue.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplDueDbAddRspMdl
            {
                EmployeePipelineDueID = item.ID,
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

    /// <summary>核心-員工-商機履約期限-資料庫-新增多筆</summary>
    public async Task<CoEmpPplDueDbAddManyRspMdl> AddManyAsync(CoEmpPplDueDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineDueList
                .Select(x => new EmployeePipelineDue
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    DueTime = x.EmployeePipelineDueTime,
                    Remark = x.EmployeePipelineDueRemark?.Trim() ?? string.Empty,
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeePipelineDue.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplDueDbAddManyRspMdl
            {
                AddedCount = itemList.Count
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

    /// <summary>核心-員工-商機履約期限-資料庫-更新</summary>
    public async Task<CoEmpPplDueDbUpdateRspMdl> UpdateAsync(CoEmpPplDueDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineDue
                .Where(x => x.ID == reqObj.EmployeePipelineDueID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.DueTime, x => reqObj.EmployeePipelineDueTime ?? x.DueTime)
                    .SetProperty(x => x.Remark, x => string.IsNullOrWhiteSpace(reqObj.EmployeePipelineDueRemark) ? x.Remark : reqObj.EmployeePipelineDueRemark.Trim())
                    .SetProperty(x => x.UpdateTime, currentTime)
                );

            var rspObj = new CoEmpPplDueDbUpdateRspMdl
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

    /// <summary>核心-員工-商機履約期限-資料庫-移除</summary>
    public async Task<CoEmpPplDueDbRemoveRspMdl> RemoveAsync(CoEmpPplDueDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineDue
                .Where(x => x.ID == reqObj.EmployeePipelineDueID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplDueDbRemoveRspMdl
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
}
