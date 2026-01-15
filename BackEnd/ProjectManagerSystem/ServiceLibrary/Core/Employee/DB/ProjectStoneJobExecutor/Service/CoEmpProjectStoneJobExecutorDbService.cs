using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Service;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫服務</summary>
public class CoEmpProjectStoneJobExecutorDbService : ICoEmpProjectStoneJobExecutorDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectStoneJobExecutorDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectStoneJobExecutorDbService(
        ILogger<CoEmpProjectStoneJobExecutorDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增</summary>
    public async Task<CoEmpPsjeDbAddRspMdl> AddAsync(CoEmpPsjeDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProjectStoneJobExecutor
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
                EmployeeID = reqObj.EmployeeID,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProjectStoneJobExecutor.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjeDbAddRspMdl
            {
                EmployeeProjectStoneJobExecutorCreateTime = item.CreateTime,
                EmployeeProjectStoneJobExecutorUpdateTime = item.UpdateTime,
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

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增多筆</summary>
    public async Task<CoEmpPsjeDbAddManyRspMdl> AddManyAsync(CoEmpPsjeDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var itemList = reqObj.EmployeeProjectStoneJobExecutoList
            .Select(x => new EmployeeProjectStoneJobExecutor
            {
                EmployeeProjectID = x.EmployeeProjectID,
                EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                EmployeeID = x.EmployeeID,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            })
            .ToList();

        try
        {
            this._mainContext.EmployeeProjectStoneJobExecutor.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjeDbAddManyRspMdl
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

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-移除</summary>
    public async Task<CoEmpPsjeDbRemoveRspMdl> RemoveAsync(CoEmpPsjeDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobExecutor
                .Where(x =>
                    x.EmployeeProjectID == reqObj.EmployeeProjectID
                    && x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID
                    && x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID
                    && x.EmployeeID == reqObj.EmployeeID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjeDbRemoveRspMdl
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

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-移除多筆</summary>
    public async Task<CoEmpPsjeDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjeDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobExecutor
                .Where(x =>
                    (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID)
                    && (reqObj.EmployeeProjectStoneJobID.HasValue == false || x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID)
                )
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjeDbRemoveManyRspMdl
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

    /// <summary>核心-員工-專案里程碑工項執行者-取得多筆</summary>
    public async Task<CoEmpPsjeDbGetManyRspMdl> GetManyAsync(CoEmpPsjeDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJobExecutor
                .AsNoTracking()
                .Where(x =>
                    (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                    && (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                    && (reqObj.EmployeeProjectStoneJobID.HasValue == false || x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID.Value)
                )
                .ToListAsync();

            var rspObj = new CoEmpPsjeDbGetManyRspMdl()
            {
                EmployeeProjectStoneJobExecutorList = itemList
                    .Select(x => new CoEmpPsjeDbGetManyRspItemMdl
                    {
                        EmployeeProjectID = x.EmployeeProjectID,
                        EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                        EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                        EmployeeID = x.EmployeeID,
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

    /// <summary>核心-員工-專案里程碑工項執行者-取得多筆數量從工項ID列表</summary>
    public async Task<CoEmpPsjeDbGetManyCountFromTaskIdListRspMdl> GetManyCountFromTaskIdListAsync(CoEmpPsjeDbGetManyCountFromTaskIdListReqMdl reqObj)
    {
        try
        {
            var query = from executor in this._mainContext.EmployeeProjectStoneJobExecutor
                            .AsNoTracking()
                            .Where(x => reqObj.EmployeeProjectStoneJobIDList.Contains(x.EmployeeProjectStoneJobID))
                        group executor by executor.EmployeeProjectStoneJobID into g
                        select new CoEmpPsjeDbGetManyCountFromTaskIdListRspItemMdl
                        {
                            EmployeeProjectStoneJobID = g.Key,
                            ExecutorCount = g.Count()
                        };

            var itemList = await query
                .OrderBy(x => x.EmployeeProjectStoneJobID)
                .ToListAsync();

            var rspObj = new CoEmpPsjeDbGetManyCountFromTaskIdListRspMdl()
            {
                EmployeeProjectStoneJobExecutorCountList = itemList
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

    /// <summary>核心-員工-專案里程碑工項執行者-取得數量</summary>
    public async Task<CoEmpPsjeDbGetCountRspMdl> GetCountAsync(CoEmpPsjeDbGetCountReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.EmployeeProjectStoneJobExecutor
                .AsNoTracking()
                .Where(x => x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID)
                .CountAsync();

            var rspObj = new CoEmpPsjeDbGetCountRspMdl
            {
                ExecutorCount = count
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
