using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Service;

/// <summary>核心-員工-商機窗口-資料庫服務</summary>
public class CoEmpPipelineContacterDbService : ICoEmpPipelineContacterDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineContacterDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineContacterDbService(
        ILogger<CoEmpPipelineContacterDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機窗口-資料庫-取得</summary>
    public async Task<CoEmpPplContDbGetRspMdl> GetAsync(CoEmpPplContDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineContacter
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineContacterID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplContDbGetRspMdl
            {
                EmployeePipelineContacterID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                ManagerContacterID = item.ManagerContacterID,
                EmployeePipelineContacterIsPrimary = item.IsPrimary,
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

    /// <summary>核心-員工-商機窗口-資料庫-取得多筆</summary>
    public async Task<CoEmpPplContDbGetManyRspMdl> GetManyAsync(CoEmpPplContDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineContacter
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    x.ID,
                    x.EmployeePipelineID,
                    x.ManagerContacterID,
                    x.IsPrimary
                })
                .OrderByDescending(x => x.ManagerContacterID)
                .ToListAsync();

            var rspObj = new CoEmpPplContDbGetManyRspMdl
            {
                EmployeePipelineContacterList = itemList.Select(x => new CoEmpPplContDbGetManyRspItemMdl
                {
                    EmployeePipelineContacterID = x.ID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    ManagerContacterID = x.ManagerContacterID,
                    EmployeePipelineContacterIsPrimary = x.IsPrimary
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

    /// <summary>核心-員工-商機窗口-資料庫-取得[筆數]</summary>
    public async Task<CoEmpPplContDbGetCountRspMdl> GetCountAsync(CoEmpPplContDbGetCountReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.EmployeePipelineContacter
                .AsNoTracking()
                .CountAsync(x => x.EmployeePipelineID == reqObj.EmployeePipelineID);

            var rspObj = new CoEmpPplContDbGetCountRspMdl
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

    /// <summary>核心-員工-商機窗口-資料庫-新增</summary>
    public async Task<CoEmpPplContDbAddRspMdl> AddAsync(CoEmpPplContDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineContacter
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                ManagerContacterID = reqObj.ManagerContacterID,
                IsPrimary = reqObj.EmployeePipelineContacterIsPrimary,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.EmployeePipelineContacter.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplContDbAddRspMdl
            {
                EmployeePipelineContacterID = item.ID
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

    /// <summary>核心-員工-商機窗口-資料庫-新增多筆</summary>
    public async Task<CoEmpPplContDbAddManyRspMdl> AddManyAsync(CoEmpPplContDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineContacterList
                .Select(x => new EmployeePipelineContacter
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    ManagerContacterID = x.ManagerContacterID,
                    IsPrimary = x.EmployeePipelineContacterIsPrimary,
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeePipelineContacter.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplContDbAddManyRspMdl
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

    /// <summary>核心-員工-商機窗口-資料庫-更新</summary>
    public async Task<CoEmpPplContDbUpdateRspMdl> UpdateAsync(CoEmpPplContDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineContacter
                .Where(x => x.ID == reqObj.EmployeePipelineContacterID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerContacterID, x => reqObj.ManagerContacterID.HasValue ? reqObj.ManagerContacterID.Value : x.ManagerContacterID)
                    .SetProperty(x => x.IsPrimary, x => reqObj.EmployeePipelineContacterIsPrimary.HasValue ? reqObj.EmployeePipelineContacterIsPrimary.Value : x.IsPrimary)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplContDbUpdateRspMdl
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

    /// <summary>核心-員工-商機窗口-資料庫-移除</summary>
    public async Task<CoEmpPplContDbRemoveRspMdl> RemoveAsync(CoEmpPplContDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineContacter
                .Where(x => x.ID == reqObj.EmployeePipelineContacterID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplContDbRemoveRspMdl
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

    /// <summary>核心-員工-商機窗口-資料庫-移除多筆</summary>
    public async Task<CoEmpPplContDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplContDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineContacter
                .Where(x => reqObj.EmployeePipelineIDList.Contains(x.EmployeePipelineID))
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplContDbRemoveManyRspMdl
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

    /// <summary>核心-員工-商機窗口-資料庫-更新多筆[是否為主要商機窗口]</summary>
    public async Task<CoEmpPplContDbUpdateManyIsPrimaryRspMdl> UpdateManyIsPrimaryAsync(CoEmpPplContDbUpdateManyIsPrimaryReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineContacter
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.IsPrimary, reqObj.EmployeePipelineContacterIsPrimary)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplContDbUpdateManyIsPrimaryRspMdl
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

    /// <summary>核心-員工-商機窗口-資料庫-移除多筆根據公司ID不匹配</summary>
    public async Task<CoEmpPplContDbRemoveManyByCompanyIdMismatchRspMdl> RemoveManyByCompanyIdMismatchAsync(CoEmpPplContDbRemoveManyByCompanyIdMismatchReqMdl reqObj)
    {
        try
        {
            var query = from epc in this._mainContext.EmployeePipelineContacter
                            .Where(epc => epc.EmployeePipelineID == reqObj.EmployeePipelineID)
                        from mc in this._mainContext.ManagerContacter
                            .Where(mc => mc.ID == epc.ManagerContacterID &&
                                        mc.ManagerCompanyID != reqObj.ManagerCompanyID)
                        select epc;

            var affectedCount = await query.ExecuteDeleteAsync();

            var rspObj = new CoEmpPplContDbRemoveManyByCompanyIdMismatchRspMdl
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
}
