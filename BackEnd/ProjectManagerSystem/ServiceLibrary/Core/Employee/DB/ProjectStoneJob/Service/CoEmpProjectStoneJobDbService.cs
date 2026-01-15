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
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;

/// <summary>核心-員工-專案里程碑工項-資料庫服務</summary>
public class CoEmpProjectStoneJobDbService : ICoEmpProjectStoneJobDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectStoneJobDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectStoneJobDbService(
        ILogger<CoEmpProjectStoneJobDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案里程碑工項-資料庫-新增</summary>
    public async Task<CoEmpPsjDbAddRspMdl> AddAsync(CoEmpPsjDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProjectStoneJob
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                Name = reqObj.EmployeeProjectStoneJobName.Trim(),
                StartTime = reqObj.EmployeeProjectStoneJobStartTime,
                EndTime = reqObj.EmployeeProjectStoneJobEndTime,
                WorkHour = reqObj.EmployeeProjectStoneJobWorkHour,
                AtomEmployeeProjectStatusID = reqObj.AtomEmployeeProjectStatus.ToInt16(),
                Remark = reqObj.EmployeeProjectStoneJobRemark?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProjectStoneJob.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjDbAddRspMdl
            {
                EmployeeProjectStoneJobID = item.ID,
                EmployeeProjectStoneJobCreateTime = item.CreateTime,
                EmployeeProjectStoneJobUpdateTime = item.UpdateTime,
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

    /// <summary>核心-員工-專案里程碑工項-資料庫-新增多筆</summary>
    public async Task<CoEmpPsjDbAddManyRspMdl> AddManyAsync(CoEmpPsjDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var itemList = reqObj.EmployeeProjectStoneJobList
            .Select(x => new EmployeeProjectStoneJob
            {
                EmployeeProjectID = x.EmployeeProjectID,
                EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                Name = x.EmployeeProjectStoneJobName.Trim(),
                StartTime = x.EmployeeProjectStoneJobStartTime,
                EndTime = x.EmployeeProjectStoneJobEndTime,
                WorkHour = x.EmployeeProjectStoneJobWorkHour,
                AtomEmployeeProjectStatusID = x.AtomEmployeeProjectStatus.ToInt16(),
                Remark = x.EmployeeProjectStoneJobRemark?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            })
            .ToList();

        try
        {
            this._mainContext.EmployeeProjectStoneJob.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjDbAddManyRspMdl
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

    /// <summary>核心-員工-專案里程碑工項-資料庫-更新</summary>
    public async Task<CoEmpPsjDbUpdateRspMdl> UpdateAsync(CoEmpPsjDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJob
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectStoneJobName) ? x.Name : reqObj.EmployeeProjectStoneJobName.Trim())
                        .SetProperty(x => x.StartTime, x => reqObj.EmployeeProjectStoneJobStartTime.HasValue == false ? x.StartTime : reqObj.EmployeeProjectStoneJobStartTime.Value)
                        .SetProperty(x => x.EndTime, x => reqObj.EmployeeProjectStoneJobEndTime.HasValue == false ? x.EndTime : reqObj.EmployeeProjectStoneJobEndTime.Value)
                        .SetProperty(x => x.WorkHour, x => reqObj.EmployeeProjectStoneJobWorkHour.HasValue == false ? x.WorkHour : reqObj.EmployeeProjectStoneJobWorkHour.Value)
                        .SetProperty(x => x.AtomEmployeeProjectStatusID, x => reqObj.AtomEmployeeProjectStatus.HasValue == false ? x.AtomEmployeeProjectStatusID : reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                        .SetProperty(x => x.Remark, x => reqObj.EmployeeProjectStoneJobRemark == null ? x.Remark : reqObj.EmployeeProjectStoneJobRemark.Trim())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPsjDbUpdateRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeProjectStoneJobUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑工項-資料庫-更新多筆</summary>
    public async Task<CoEmpPsjDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPsjDbUpdateManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJob
                .Where(x => reqObj.EmployeeProjectStoneJobIdList.Contains(x.ID))
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.AtomEmployeeProjectStatusID, x => reqObj.AtomEmployeeProjectStatus.HasValue == false ? x.AtomEmployeeProjectStatusID : reqObj.AtomEmployeeProjectStatus.Value.ToInt16())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPsjDbUpdateManyRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeProjectStoneJobUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑工項-資料庫-移除</summary>
    public async Task<CoEmpPsjDbRemoveRspMdl> RemoveAsync(CoEmpPsjDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJob
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjDbRemoveRspMdl
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

    /// <summary>核心-員工-專案里程碑工項-資料庫-移除多筆</summary>
    public async Task<CoEmpPsjDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJob
                .Where(x =>
                    (reqObj.EmployeeProjectStoneJobIdList == null || reqObj.EmployeeProjectStoneJobIdList.Contains(x.ID))
                    && (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID))
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjDbRemoveManyRspMdl
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

    /// <summary>核心-員工-專案里程碑工項-取得</summary>
    public async Task<CoEmpPsjDbGetRspMdl> GetAsync(CoEmpPsjDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStoneJob
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectStoneJobID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPsjDbGetRspMdl()
            {
                EmployeeProjectID = item.EmployeeProjectID,
                EmployeeProjectStoneID = item.EmployeeProjectStoneID,
                EmployeeProjectStoneJobName = item.Name?.Trim(),
                EmployeeProjectStoneJobStartTime = item.StartTime.ToUniversalTime(),
                EmployeeProjectStoneJobEndTime = item.EndTime.ToUniversalTime(),
                EmployeeProjectStoneJobWorkHour = item.WorkHour,
                EmployeeProjectStoneJobRemark = item.Remark?.Trim(),
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

    /// <summary>核心-員工-專案里程碑工項-取得多筆</summary>
    public async Task<CoEmpPsjDbGetManyRspMdl> GetManyAsync(CoEmpPsjDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJob
                .AsNoTracking()
                .Where(x =>
                    (reqObj.EmployeeProjectIdList == null || reqObj.EmployeeProjectIdList.Contains(x.EmployeeProjectID))
                    && (reqObj.EmployeeProjectStoneIdList == null || reqObj.EmployeeProjectStoneIdList.Contains(x.EmployeeProjectStoneID))
                )
                .ToListAsync();

            var rspObj = new CoEmpPsjDbGetManyRspMdl()
            {
                EmployeeProjectStoneJobList = itemList
                    .Select(item => new CoEmpPsjDbGetManyRspItemJobMdl
                    {
                        EmployeeProjectStoneJobID = item.ID,
                        EmployeeProjectID = item.EmployeeProjectID,
                        EmployeeProjectStoneID = item.EmployeeProjectStoneID,
                        EmployeeProjectStoneJobName = item.Name?.Trim(),
                        EmployeeProjectStoneJobStartTime = item.StartTime.ToUniversalTime(),
                        EmployeeProjectStoneJobEndTime = item.EndTime.ToUniversalTime(),
                        EmployeeProjectStoneJobWorkHour = item.WorkHour,
                        AtomEmployeeProjectStatus = item.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
                        EmployeeProjectStoneJobRemark = item.Remark?.Trim(),
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

    /// <summary>核心-員工-專案里程碑工項-取得多筆ID</summary>
    public async Task<CoEmpPsjDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPsjDbGetManyIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJob
                .AsNoTracking()
                .Where(x => x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID)
                .Select(x => new
                {
                    x.ID,
                })
                .ToListAsync();

            var rspObj = new CoEmpPsjDbGetManyIdRspMdl()
            {
                EmployeeProjectStoneJobList = itemList
                    .Select(item => new CoEmpPsjDbGetManyIdRspItemMdl
                    {
                        EmployeeProjectStoneJobID = item.ID,
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

    /// <summary>核心-員工-專案里程碑工項-取得多筆狀態從里程碑ID</summary>
    public async Task<CoEmpPsjDbGetManyStatusFromStoneIdRspMdl> GetManyStatusFromStoneIdAsync(CoEmpPsjDbGetManyStatusFromStoneIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJob
                .AsNoTracking()
                .Where(x => x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID)
                .Select(x => new
                {
                    x.ID,
                    x.AtomEmployeeProjectStatusID
                })
                .OrderBy(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPsjDbGetManyStatusFromStoneIdRspMdl
            {
                EmployeeProjectStoneJobStatusList = itemList
                    .Select(x => new CoEmpPsjDbGetManyStatusFromStoneIdRspItemMdl
                    {
                        EmployeeProjectStoneJobID = x.ID,
                        AtomEmployeeProjectStatus = x.AtomEmployeeProjectStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>()
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

    /// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoEmpPsjDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpPsjDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJob
                .AsNoTracking()
                .Where(x =>
                    // 員工專案-ID
                    (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                    // 員工專案里程碑-ID
                    && (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                    // 員工專案里程碑工項-名稱
                    && (string.IsNullOrEmpty(reqObj.EmployeeProjectStoneJobName) || x.Name.Contains(reqObj.EmployeeProjectStoneJobName))
                )
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoEmpPsjDbGetManyFromMbsBscRspMdl
            {
                EmployeeProjectStoneJobList = itemList
                .Select(x => new CoEmpPsjDbGetManyFromMbsBscRspItemMdl
                {
                    EmployeeProjectStoneJobID = x.ID,
                    EmployeeProjectStoneJobName = x.Name.Trim(),
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
