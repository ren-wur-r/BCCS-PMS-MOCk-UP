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
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Service;

/// <summary>核心-員工-專案里程碑-資料庫服務</summary>
public class CoEmpProjectStoneDbService : ICoEmpProjectStoneDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectStoneDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectStoneDbService(
        ILogger<CoEmpProjectStoneDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案里程碑-資料庫-新增</summary>
    public async Task<CoEmpPsDbAddRspMdl> AddAsync(CoEmpPsDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProjectStone
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                Name = reqObj.EmployeeProjectStoneName.Trim(),
                PreNotifyDay = reqObj.EmployeeProjectStonePreNotifyDay,
                StartTime = reqObj.EmployeeProjectStoneStartTime,
                EndTime = reqObj.EmployeeProjectStoneEndTime,
                AtomEmployeeProjectStatusID = reqObj.AtomEmployeeProjectStatus.ToInt16(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProjectStone.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsDbAddRspMdl
            {
                EmployeeProjectStoneID = item.ID,
                EmployeeProjectStoneCreateTime = item.CreateTime,
                EmployeeProjectStoneUpdateTime = item.UpdateTime,
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

    /// <summary>核心-員工-專案里程碑-資料庫-更新</summary>
    public async Task<CoEmpPsDbUpdateRspMdl> UpdateAsync(CoEmpPsDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStone
                .Where(x => x.ID == reqObj.EmployeeProjectStoneID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectStoneName) ? x.Name : reqObj.EmployeeProjectStoneName.Trim())
                        .SetProperty(x => x.StartTime, x => reqObj.EmployeeProjectStoneStartTime.HasValue == false ? x.StartTime : reqObj.EmployeeProjectStoneStartTime.Value)
                        .SetProperty(x => x.EndTime, x => reqObj.EmployeeProjectStoneEndTime.HasValue == false ? x.EndTime : reqObj.EmployeeProjectStoneEndTime.Value)
                        .SetProperty(x => x.PreNotifyDay, x => reqObj.EmployeeProjectStonePreNotifyDay.HasValue == false ? x.PreNotifyDay : reqObj.EmployeeProjectStonePreNotifyDay.Value)
                        .SetProperty(x => x.AtomEmployeeProjectStatusID, x => reqObj.AtomEmployeeProjectStatus.HasValue == false ? x.AtomEmployeeProjectStatusID : reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPsDbUpdateRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeProjectStoneUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑-資料庫-更新多筆</summary>
    public async Task<CoEmpPsDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPsDbUpdateManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStone
                .Where(x => reqObj.EmployeeProjectStoneIdList.Contains(x.ID))
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.AtomEmployeeProjectStatusID, x => reqObj.AtomEmployeeProjectStatus.HasValue == false ? x.AtomEmployeeProjectStatusID : reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPsDbUpdateManyRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeProjectStoneUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑-資料庫-移除</summary>
    public async Task<CoEmpPsDbRemoveRspMdl> RemoveAsync(CoEmpPsDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStone
                .Where(x => x.ID == reqObj.EmployeeProjectStoneID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsDbRemoveRspMdl
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

    /// <summary>核心-員工-專案里程碑-取得</summary>
    public async Task<CoEmpPsDbGetRspMdl> GetAsync(CoEmpPsDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStone
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectStoneID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPsDbGetRspMdl()
            {
                EmployeeProjectID = item.EmployeeProjectID,
                EmployeeProjectStoneName = item.Name?.Trim(),
                EmployeeProjectStonePreNotifyDay = item.PreNotifyDay,
                EmployeeProjectStoneStartTime = item.StartTime.ToUniversalTime(),
                EmployeeProjectStoneEndTime = item.EndTime.ToUniversalTime(),
                AtomEmployeeProjectStatus = item.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
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

    /// <summary>核心-員工-專案里程碑-取得員工專案ID</summary>
    public async Task<CoEmpPsDbGetEmployeeProjectIdRspMdl> GetEmployeeProjectIdAsync(CoEmpPsDbGetEmployeeProjectIdReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStone
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectStoneID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPsDbGetEmployeeProjectIdRspMdl()
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

    /// <summary>核心-員工-專案里程碑-取得[名稱]</summary>
    public async Task<CoEmpPsDbGetNameRspMdl> GetNameAsync(CoEmpPsDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStone
                .AsNoTracking()
                .Where(x => x.ID == reqObj.EmployeeProjectStoneID)
                .Select(x => new
                {
                    x.Name
                })
                .SingleOrDefaultAsync();
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPsDbGetNameRspMdl
            {
                EmployeeProjectStoneName = item.Name?.Trim()
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

    /// <summary>核心-員工-專案里程碑-取得多筆</summary>
    public async Task<CoEmpPsDbGetManyRspMdl> GetManyAsync(CoEmpPsDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStone
                .AsNoTracking()
                .Where(x => reqObj.EmployeeProjectIdList.Contains(x.EmployeeProjectID))
                .ToListAsync();

            var rspObj = new CoEmpPsDbGetManyRspMdl()
            {
                EmployeeProjectStoneList = itemList
                    .Select(item => new CoEmpPsDbGetManyRspItemMdl
                    {
                        EmployeeProjectStoneID = item.ID,
                        EmployeeProjectID = item.EmployeeProjectID,
                        EmployeeProjectStoneName = item.Name?.Trim(),
                        EmployeeProjectStonePreNotifyDay = item.PreNotifyDay,
                        EmployeeProjectStoneStartTime = item.StartTime.ToUniversalTime(),
                        EmployeeProjectStoneEndTime = item.EndTime.ToUniversalTime(),
                        AtomEmployeeProjectStatus = item.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
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

    /// <summary>核心-員工-專案里程碑-取得多筆[名稱]</summary>
    public async Task<CoEmpPsDbGetManyNameRspMdl> GetManyNameAsync(CoEmpPsDbGetManyNameReqMdl reqObj)
    {
        try
        {

            var itemList = await this._mainContext.EmployeeProjectStone
                .AsNoTracking()
                .Where(x => reqObj.EmployeeProjectStoneIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoEmpPsDbGetManyNameRspMdl
            {
                EmployeeProjectStoneList = itemList
                    .Select(x => new CoEmpPsDbGetManyNameRspItemMdl
                    {
                        EmployeeProjectStoneID = x.ID,
                        EmployeeProjectStoneName = x.Name?.Trim()
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

    #region 管理者後台-基本

    /// <summary>核心-員工-專案里程碑-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoEmpPsDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpPsDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.EmployeeProjectStone
                .AsNoTracking()
                .Where(x =>
                    // 員工專案-ID
                    (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                    // 員工專案里程碑-名稱
                    && (string.IsNullOrEmpty(reqObj.EmployeeProjectStoneName) || x.Name.Contains(reqObj.EmployeeProjectStoneName))
                )
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoEmpPsDbGetManyFromMbsBscRspMdl
            {
                EmployeeProjectStoneList = itemList
                    .Select(x => new CoEmpPsDbGetManyFromMbsBscRspItemMdl
                    {
                        EmployeeProjectStoneID = x.ID,
                        EmployeeProjectStoneName = x.Name.Trim()
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
