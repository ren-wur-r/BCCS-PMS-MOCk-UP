using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Service;

/// <summary>核心-員工-專案里程碑工項清單-資料庫服務</summary>
public class CoEmpProjectStoneJobBucketDbService : ICoEmpProjectStoneJobBucketDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectStoneJobBucketDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectStoneJobBucketDbService(
        ILogger<CoEmpProjectStoneJobBucketDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-新增</summary>
    public async Task<CoEmpPsjbDbAddRspMdl> AddAsync(CoEmpPsjbDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProjectStoneJobBucket
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
                Name = reqObj.EmployeeProjectStoneJobBucketName?.Trim() ?? string.Empty,
                IsFinish = reqObj.EmployeeProjectStoneJobBucketIsFinish,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProjectStoneJobBucket.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjbDbAddRspMdl
            {
                EmployeeProjectStoneJobBucketID = item.ID,
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-新增多筆</summary>
    public async Task<CoEmpPsjbDbAddManyRspMdl> AddManyAsync(CoEmpPsjbDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeeProjectStoneJobBucketList
                .Select(reqItem => new EmployeeProjectStoneJobBucket
                {
                    EmployeeProjectID = reqItem.EmployeeProjectID,
                    EmployeeProjectStoneID = reqItem.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobID = reqItem.EmployeeProjectStoneJobID,
                    Name = reqItem.EmployeeProjectStoneJobBucketName?.Trim() ?? string.Empty,
                    IsFinish = reqItem.EmployeeProjectStoneJobBucketIsFinish,
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeeProjectStoneJobBucket.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjbDbAddManyRspMdl
            {
                EmployeeProjectStoneJobBucketList = itemList
                    .Select(item => new CoEmpPsjbDbAddManyRspItemMdl
                    {
                        EmployeeProjectStoneJobBucketID = item.ID
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
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-更新</summary>
    public async Task<CoEmpPsjbDbUpdateRspMdl> UpdateAsync(CoEmpPsjbDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobBucket
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobBucketID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectStoneJobBucketName) ? x.Name : reqObj.EmployeeProjectStoneJobBucketName.Trim())
                    .SetProperty(x => x.IsFinish, x => reqObj.EmployeeProjectStoneJobBucketIsFinish ?? x.IsFinish)
                    .SetProperty(x => x.UpdateTime, currentTime)
                );

            var rspObj = new CoEmpPsjbDbUpdateRspMdl
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-移除</summary>
    public async Task<CoEmpPsjbDbRemoveRspMdl> RemoveAsync(CoEmpPsjbDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobBucket
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobBucketID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjbDbRemoveRspMdl
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-移除多筆</summary>
    public async Task<CoEmpPsjbDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjbDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobBucket
                .Where(x =>
                    (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                    && (reqObj.EmployeeProjectStoneJobBucketIdList == null || reqObj.EmployeeProjectStoneJobBucketIdList.Contains(x.ID))
                )
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjbDbRemoveManyRspMdl
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得</summary>
    public async Task<CoEmpPsjbDbGetRspMdl> GetAsync(CoEmpPsjbDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStoneJobBucket
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectStoneJobBucketID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPsjbDbGetRspMdl
            {
                EmployeeProjectStoneJobBucketID = item.ID,
                EmployeeProjectID = item.EmployeeProjectID,
                EmployeeProjectStoneID = item.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = item.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobBucketName = item.Name?.Trim(),
                EmployeeProjectStoneJobBucketIsFinish = item.IsFinish
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得[員工專案ID]</summary>
    public async Task<CoEmpPsjbDbGetEmployeeProjectIdRspMdl> GetEmployeeProjectIdAsync(CoEmpPsjbDbGetEmployeeProjectIdReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStoneJobBucket
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectStoneJobBucketID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPsjbDbGetEmployeeProjectIdRspMdl
            {
                EmployeeProjectID = item.EmployeeProjectID,
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆</summary>
    public async Task<CoEmpPsjbDbGetManyRspMdl> GetManyAsync(CoEmpPsjbDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJobBucket
                .AsNoTracking()
                .Where(x =>
                    (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID)
                    && (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID)
                    && (reqObj.EmployeeProjectStoneJobID.HasValue == false || x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID))
                .Select(x => new
                {
                    x.ID,
                    x.EmployeeProjectID,
                    x.EmployeeProjectStoneID,
                    x.EmployeeProjectStoneJobID,
                    x.Name,
                    x.IsFinish
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPsjbDbGetManyRspMdl
            {
                EmployeeProjectStoneJobBucketList = itemList
                    .Select(x => new CoEmpPsjbDbGetManyRspItemMdl
                    {
                        EmployeeProjectStoneJobBucketID = x.ID,
                        EmployeeProjectID = x.EmployeeProjectID,
                        EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                        EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobBucketName = x.Name?.Trim(),
                        EmployeeProjectStoneJobBucketIsFinish = x.IsFinish
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆從工項ID列表</summary>
    public async Task<CoEmpPsjbDbGetManyFromTaskIdListRspMdl> GetManyFromTaskIdListAsync(CoEmpPsjbDbGetManyFromTaskIdListReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJobBucket
                .AsNoTracking()
                .Where(x => reqObj.EmployeeProjectStoneJobIDList.Contains(x.EmployeeProjectStoneJobID))
                .Select(x => new
                {
                    x.ID,
                    x.EmployeeProjectID,
                    x.EmployeeProjectStoneID,
                    x.EmployeeProjectStoneJobID,
                    x.Name,
                    x.IsFinish
                })
                .OrderBy(x => x.EmployeeProjectStoneJobID)
                .ThenBy(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPsjbDbGetManyFromTaskIdListRspMdl
            {
                EmployeeProjectStoneJobBucketList = itemList.Select(x => new CoEmpPsjbDbGetManyFromTaskIdListRspItemMdl
                {
                    EmployeeProjectStoneJobBucketID = x.ID,
                    EmployeeProjectID = x.EmployeeProjectID,
                    EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobBucketName = x.Name?.Trim(),
                    EmployeeProjectStoneJobBucketIsFinish = x.IsFinish
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

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆完成狀態從工項ID</summary>
    public async Task<CoEmpPsjbDbGetManyIsFinishFromTaskIdRspMdl> GetManyIsFinishFromTaskIdAsync(CoEmpPsjbDbGetManyIsFinishFromTaskIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJobBucket
                .AsNoTracking()
                .Where(x => x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID)
                .Select(x => new
                {
                    x.ID,
                    x.IsFinish
                })
                .OrderBy(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPsjbDbGetManyIsFinishFromTaskIdRspMdl
            {
                EmployeeProjectStoneJobBucketIsFinishList = itemList.Select(x => new CoEmpPsjbDbGetManyIsFinishFromTaskIdRspItemMdl
                {
                    EmployeeProjectStoneJobBucketID = x.ID,
                    EmployeeProjectStoneJobBucketIsFinish = x.IsFinish
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
}
