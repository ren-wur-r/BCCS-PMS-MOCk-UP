using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Service;

/// <summary>核心-員工-商機業務開發紀錄-資料庫服務</summary>
public class CoEmpPipelineSalerTrackingDbService : ICoEmpPipelineSalerTrackingDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineSalerTrackingDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineSalerTrackingDbService(
        ILogger<CoEmpPipelineSalerTrackingDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得</summary>
    public async Task<CoEmpPplSlrTrkDbGetRspMdl> GetAsync(CoEmpPplSlrTrkDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineSalerTracking
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineSalerTrackingID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplSlrTrkDbGetRspMdl
            {
                EmployeePipelineSalerTrackingID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                EmployeePipelineSalerTrackingTime = item.TrackingTime,
                ManagerContacterID = item.ManagerContacterID,
                EmployeePipelineSalerTrackingRemark = item.Remark?.Trim(),
                EmployeePipelineSalerTrackingCreateEmployeeID = item.CreateEmployeeID
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆</summary>
    public async Task<CoEmpPplSlrTrkDbGetManyRspMdl> GetManyAsync(CoEmpPplSlrTrkDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineSalerTracking
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    x.ID,
                    x.EmployeePipelineID,
                    x.TrackingTime,
                    x.ManagerContacterID,
                    x.Remark,
                    x.CreateEmployeeID
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPplSlrTrkDbGetManyRspMdl
            {
                EmployeePipelineSalerTrackingList = itemList.Select(x => new CoEmpPplSlrTrkDbGetManyRspItemMdl
                {
                    EmployeePipelineSalerTrackingID = x.ID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    EmployeePipelineSalerTrackingTime = x.TrackingTime,
                    ManagerContacterID = x.ManagerContacterID,
                    EmployeePipelineSalerTrackingRemark = x.Remark?.Trim(),
                    EmployeePipelineSalerTrackingCreateEmployeeID = x.CreateEmployeeID
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-新增</summary>
    public async Task<CoEmpPplSlrTrkDbAddRspMdl> AddAsync(CoEmpPplSlrTrkDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineSalerTracking
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                TrackingTime = reqObj.EmployeePipelineSalerTrackingTime,
                ManagerContacterID = reqObj.ManagerContacterID,
                Remark = reqObj.EmployeePipelineSalerTrackingRemark?.Trim() ?? string.Empty,
                CreateEmployeeID = reqObj.EmployeePipelineSalerTrackingCreateEmployeeID,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeePipelineSalerTracking.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplSlrTrkDbAddRspMdl
            {
                EmployeePipelineSalerTrackingID = item.ID,
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-新增多筆</summary>
    public async Task<CoEmpPplSlrTrkDbAddManyRspMdl> AddManyAsync(CoEmpPplSlrTrkDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineSalerTrackingList
                .Select(x => new EmployeePipelineSalerTracking
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    TrackingTime = x.EmployeePipelineSalerTrackingTime,
                    ManagerContacterID = x.ManagerContacterID,
                    Remark = x.EmployeePipelineSalerTrackingRemark?.Trim() ?? string.Empty,
                    CreateEmployeeID = x.EmployeePipelineSalerTrackingCreateEmployeeID,
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeePipelineSalerTracking.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplSlrTrkDbAddManyRspMdl
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-更新</summary>
    public async Task<CoEmpPplSlrTrkDbUpdateRspMdl> UpdateAsync(CoEmpPplSlrTrkDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineSalerTracking
                .Where(x => x.ID == reqObj.EmployeePipelineSalerTrackingID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.TrackingTime, x => reqObj.EmployeePipelineSalerTrackingTime ?? x.TrackingTime)
                    .SetProperty(x => x.ManagerContacterID, x => reqObj.ManagerContacterID ?? x.ManagerContacterID)
                    .SetProperty(x => x.Remark, x => string.IsNullOrWhiteSpace(reqObj.EmployeePipelineSalerTrackingRemark) ? x.Remark : reqObj.EmployeePipelineSalerTrackingRemark.Trim())
                    .SetProperty(x => x.UpdateTime, currentTime)
                );

            var rspObj = new CoEmpPplSlrTrkDbUpdateRspMdl
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-移除</summary>
    public async Task<CoEmpPplSlrTrkDbRemoveRspMdl> RemoveAsync(CoEmpPplSlrTrkDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineSalerTracking
                .Where(x => x.ID == reqObj.EmployeePipelineSalerTrackingID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplSlrTrkDbRemoveRspMdl
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-移除多筆</summary>
    public async Task<CoEmpPplSlrTrkDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplSlrTrkDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineSalerTracking
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplSlrTrkDbRemoveManyRspMdl
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆開發時間從商機ID列表</summary>
    public async Task<CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspMdl> GetManyTrackingTimeFromPipelineIdListAsync(CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListReqMdl reqObj)
    {
        try
        {
            var query = from st in this._mainContext.EmployeePipelineSalerTracking
                            .AsNoTracking()
                            .Where(st => reqObj.EmployeePipelineIDList.Contains(st.EmployeePipelineID))
                        select new
                        {
                            st.EmployeePipelineID,
                            st.TrackingTime
                        };

            var itemList = await query.ToListAsync();

            var groupedList = itemList
                .GroupBy(x => x.EmployeePipelineID)
                .Select(g => new CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspItemMdl
                {
                    EmployeePipelineID = g.Key,
                    TrackingTimeList = g.Select(x => x.TrackingTime)
                        .OrderBy(x => x)
                        .ToList()
                })
                .ToList();

            var rspObj = new CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspMdl
            {
                EmployeePipelineSalerTrackingTimeList = groupedList
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

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆[管理者後台-CRM-商機管理]</summary>
    public async Task<CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspMdl> GetManyFromMbsCrmPplAsync(CoEmpPplSlrTrkDbGetManyFromMbsCrmPplReqMdl reqObj)
    {
        try
        {
            var itemList = await (
                from st in this._mainContext.EmployeePipelineSalerTracking
                                            .AsNoTracking()
                                            .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                from emp in this._mainContext.Employee
                                            .AsNoTracking()
                                            .Where(x => x.ID == st.CreateEmployeeID)
                                            .DefaultIfEmpty()
                from c in this._mainContext.ManagerContacter
                                            .AsNoTracking()
                                            .Where(x => x.ID == st.ManagerContacterID)
                                            .DefaultIfEmpty()
                select new
                {
                    st.ID,
                    st.TrackingTime,
                    st.ManagerContacterID,
                    ManagerContacterName = c.Name,
                    st.Remark,
                    CreateEmployeeName = emp.Name,
                }
            )
            .OrderByDescending(x => x.ID)
            .ToListAsync();

            var rspObj = new CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspMdl
            {
                EmployeePipelineSalerTrackingList = itemList.Select(x => new CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspItemMdl
                {
                    EmployeePipelineSalerTrackingID = x.ID,
                    EmployeePipelineSalerTrackingTime = x.TrackingTime,
                    ManagerContacterID = x.ManagerContacterID,
                    ManagerContacterName = x.ManagerContacterName?.Trim(),
                    EmployeePipelineSalerTrackingRemark = x.Remark?.Trim(),
                    EmployeePipelineSalerTrackingCreateEmployeeName = x.CreateEmployeeName?.Trim()
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
