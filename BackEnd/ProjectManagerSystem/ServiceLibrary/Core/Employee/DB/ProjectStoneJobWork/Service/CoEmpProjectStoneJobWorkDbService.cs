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
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Service;

/// <summary>核心-員工-專案里程碑工項工作-資料庫服務</summary>
public class CoEmpProjectStoneJobWorkDbService : ICoEmpProjectStoneJobWorkDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectStoneJobWorkDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectStoneJobWorkDbService(
        ILogger<CoEmpProjectStoneJobWorkDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案里程碑工項工作-新增</summary>
    public async Task<CoEmpPsjwDbAddRspMdl> AddAsync(CoEmpPsjwDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.EmployeeProjectStoneJobWork()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
                EmployeeID = reqObj.EmployeeID,
                StartTime = reqObj.EmployeeProjectStoneJobWorkStartTime.UtcDateTime,
                EndTime = reqObj.EmployeeProjectStoneJobWorkEndTime.UtcDateTime,
                Remark = reqObj.EmployeeProjectStoneJobWorkRemark?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProjectStoneJobWork.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjwDbAddRspMdl()
            {
                EmployeeProjectStoneJobWorkID = item.ID,
                EmployeeProjectStoneJobWorkCreateTime = currentTime,
                EmployeeProjectStoneJobWorkUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑工項工作-更新</summary>
    public async Task<CoEmpPsjwDbUpdateRspMdl> UpdateAsync(CoEmpPsjwDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobWork
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobWorkID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.StartTime, x => reqObj.EmployeeProjectStoneJobWorkStartTime.HasValue ? reqObj.EmployeeProjectStoneJobWorkStartTime.Value.UtcDateTime : x.StartTime)
                        .SetProperty(x => x.EndTime, x => reqObj.EmployeeProjectStoneJobWorkEndTime.HasValue ? reqObj.EmployeeProjectStoneJobWorkEndTime.Value.UtcDateTime : x.EndTime)
                        .SetProperty(x => x.Remark, x => !string.IsNullOrEmpty(reqObj.EmployeeProjectStoneJobWorkRemark) ? reqObj.EmployeeProjectStoneJobWorkRemark.Trim() : x.Remark)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPsjwDbUpdateRspMdl()
            {
                AffectedCount = affectedCount,
                EmployeeProjectStoneJobWorkUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑工項工作-取得</summary>
    public async Task<CoEmpPsjwDbGetRspMdl> GetAsync(CoEmpPsjwDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectStoneJobWork
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectStoneJobWorkID);
            if (item == null)
            {
                this._logger.LogWarning($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return null;
            }

            var rspObj = new CoEmpPsjwDbGetRspMdl()
            {
                EmployeeProjectID = item.EmployeeProjectID,
                EmployeeProjectStoneID = item.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = item.EmployeeProjectStoneJobID,
                EmployeeID = item.EmployeeID,
                EmployeeProjectStoneJobWorkStartTime = item.StartTime.ToUniversalTime(),
                EmployeeProjectStoneJobWorkEndTime = item.EndTime.ToUniversalTime(),
                EmployeeProjectStoneJobWorkRemark = item.Remark?.Trim(),
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

    /// <summary>核心-員工-專案里程碑工項工作-取得多筆</summary>
    public async Task<CoEmpPsjwDbGetManyRspMdl> GetManyAsync(CoEmpPsjwDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJobWork
                .AsNoTracking()
                .Where(x => x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID)
                .Select(x => new CoEmpPsjwDbGetManyRspItemMdl()
                {
                    EmployeeProjectStoneJobWorkID = x.ID,
                    EmployeeID = x.EmployeeID,
                    EmployeeProjectStoneJobWorkStartTime = x.StartTime,
                    EmployeeProjectStoneJobWorkEndTime = x.EndTime,
                    EmployeeProjectStoneJobWorkRemark = x.Remark,
                })
                .ToListAsync();

            var rspObj = new CoEmpPsjwDbGetManyRspMdl()
            {
                EmployeeProjectStoneJobWorkList = itemList
                    .Select(x => new CoEmpPsjwDbGetManyRspItemMdl
                    {
                        EmployeeProjectStoneJobWorkID = x.EmployeeProjectStoneJobWorkID,
                        EmployeeID = x.EmployeeID,
                        EmployeeProjectStoneJobWorkStartTime = x.EmployeeProjectStoneJobWorkStartTime.ToUniversalTime(),
                        EmployeeProjectStoneJobWorkEndTime = x.EmployeeProjectStoneJobWorkEndTime.ToUniversalTime(),
                        EmployeeProjectStoneJobWorkRemark = x.EmployeeProjectStoneJobWorkRemark?.Trim(),
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

    /// <summary>核心-員工-專案里程碑工項工作-移除</summary>
    public async Task<CoEmpPsjwDbRemoveRspMdl> RemoveAsync(CoEmpPsjwDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobWork
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobWorkID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjwDbRemoveRspMdl()
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

    #region 管理者後台-工作-工項 MbsWrkJob

    /// <summary>核心-員工-專案里程碑工項工作-取得筆數從[管理者後台-工作-工項]</summary>
    public async Task<CoEmpPsjwDbGetCountFromMbsWrkJobRspMdl> GetCountFromMbsWrkJobAsync(CoEmpPsjwDbGetCountFromMbsWrkJobReqMdl reqObj)
    {
        try
        {
            var count = await
                (
                    from epsj in this._mainContext.EmployeeProjectStoneJob
                        .Where(epsjx =>
                            (reqObj.EmployeeProjectID.HasValue == false || epsjx.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                            && (reqObj.EmployeeProjectStoneID.HasValue == false || epsjx.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                            && (reqObj.EmployeeProjectStoneJobID.HasValue == false || epsjx.ID == reqObj.EmployeeProjectStoneJobID.Value)
                            && (reqObj.EmployeeProjectStoneJobStatus.HasValue == false || epsjx.AtomEmployeeProjectStatusID == reqObj.EmployeeProjectStoneJobStatus.Value.ToInt16())
                            && (reqObj.StartEmployeeProjectStoneJobEndTime.HasValue == false || epsjx.EndTime >= reqObj.StartEmployeeProjectStoneJobEndTime.Value.UtcDateTime)
                            && (reqObj.EndEmployeeProjectStoneJobEndTime.HasValue == false || epsjx.EndTime <= reqObj.EndEmployeeProjectStoneJobEndTime.Value.UtcDateTime)
                        )
                    from epsje in this._mainContext.EmployeeProjectStoneJobExecutor
                        .Where(epsjex =>
                            epsjex.EmployeeProjectStoneJobID == epsj.ID
                            && epsjex.EmployeeID == reqObj.EmployeeID
                        )
                    group epsj by new { epsj.ID } into g
                    select 1
                ).CountAsync();

            var rspObj = new CoEmpPsjwDbGetCountFromMbsWrkJobRspMdl()
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

    /// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工項]</summary>
    public async Task<CoEmpPsjwDbGetManyFromMbsWrkJobRspMdl> GetManyFromMbsWrkJobAsync(CoEmpPsjwDbGetManyFromMbsWrkJobReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from epsj in this._mainContext.EmployeeProjectStoneJob
                        .Where(epsjx =>
                            (reqObj.EmployeeProjectID.HasValue == false || epsjx.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                            && (reqObj.EmployeeProjectStoneID.HasValue == false || epsjx.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                            && (reqObj.EmployeeProjectStoneJobID.HasValue == false || epsjx.ID == reqObj.EmployeeProjectStoneJobID.Value)
                            && (reqObj.EmployeeProjectStoneJobStatus.HasValue == false || epsjx.AtomEmployeeProjectStatusID == reqObj.EmployeeProjectStoneJobStatus.Value.ToInt16())
                            && (reqObj.StartEmployeeProjectStoneJobEndTime.HasValue == false || epsjx.EndTime >= reqObj.StartEmployeeProjectStoneJobEndTime.Value.UtcDateTime)
                            && (reqObj.EndEmployeeProjectStoneJobEndTime.HasValue == false || epsjx.EndTime <= reqObj.EndEmployeeProjectStoneJobEndTime.Value.UtcDateTime)
                        )
                    from epsje in this._mainContext.EmployeeProjectStoneJobExecutor
                        .Where(epsjex =>
                            epsjex.EmployeeProjectStoneJobID == epsj.ID
                            && epsjex.EmployeeID == reqObj.EmployeeID
                        )
                    orderby epsj.EndTime descending
                    //from ep in this._mainContext.EmployeeProject
                    //    .Where(epx => epx.ID == epsj.EmployeeProjectID)
                    //from eps in this._mainContext.EmployeeProjectStone
                    //    .Where(epsx => epsx.ID == epsj.EmployeeProjectStoneID)
                    //from e in this._mainContext.Employee
                    //    .Where(ex => ex.ID == epsje.EmployeeID)
                    group new { epsj, epsje } by new { epsj.ID } into g
                    select new
                    {
                        EpID = g.First().epsj.EmployeeProjectID,
                        EpsID = g.First().epsj.EmployeeProjectStoneID,
                        EpsjID = g.First().epsj.ID,
                        EpsjName = g.First().epsj.Name,
                        EpsjStatusID = g.First().epsj.AtomEmployeeProjectStatusID,
                        EpsjStartTime = g.First().epsj.StartTime,
                        EpsjEndTime = g.First().epsj.EndTime,
                        ExecutorList = g.Select(x => x.epsje.EmployeeID).ToList(),
                    }
                ).AsNoTracking()
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpPsjwDbGetManyFromMbsWrkJobRspMdl()
            {
                EmployeeProjectStoneJobList = itemList
                    .Select(x => new CoEmpPsjwDbGetManyFromMbsWrkJobRspItemJobMdl
                    {
                        EmployeeProjectID = x.EpID,
                        EmployeeProjectStoneID = x.EpsID,
                        EmployeeProjectStoneJobID = x.EpsjID,
                        EmployeeProjectStoneJobName = x.EpsjName?.Trim(),
                        EmployeeProjectStoneJobStatus = x.EpsjStatusID.ToEnum<DbAtomEmployeeProjectStatusEnum>(),
                        EmployeeProjectStoneJobStartTime = x.EpsjStartTime.ToUniversalTime(),
                        EmployeeProjectStoneJobEndTime = x.EpsjEndTime.ToUniversalTime(),
                        EmployeeList = x.ExecutorList
                            .Select(y => new CoEmpPsjDbGetManyRspItemEmployeeMdl
                            {
                                EmployeeID = y,
                            })
                            .ToList(),
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

    #region 管理者後台-工作-工作紀錄 MbsWrkJobRec

    /// <summary>核心-員工-專案里程碑工項工作-取得筆數從[管理者後台-工作-工作紀錄]</summary>
    public async Task<CoEmpPsjwDbGetCountFromMbsWrkJobRecRspMdl> GetCountFromMbsWrkJobRecAsync(CoEmpPsjwDbGetCountFromMbsWrkJobRecReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.EmployeeProjectStoneJobWork
                .AsNoTracking()
                .Where(x =>
                    // 員工專案里程碑工項工作開始時間
                    (reqObj.StartEmployeeProjectStoneJobWorkStartTime.HasValue == false || x.StartTime >= reqObj.StartEmployeeProjectStoneJobWorkStartTime.Value.UtcDateTime)
                    // 員工專案里程碑工項工作結束時間
                    && (reqObj.EndEmployeeProjectStoneJobWorkEndTime.HasValue == false || x.EndTime <= reqObj.EndEmployeeProjectStoneJobWorkEndTime.Value.UtcDateTime)
                    // 員工專案ID
                    && (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                    // 員工專案里程碑ID
                    && (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                    // 員工專案里程碑工項ID
                    && (reqObj.EmployeeProjectStoneJobID.HasValue == false || x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID.Value)
                    // 員工ID
                    && (reqObj.EmployeeID.HasValue == false || x.EmployeeID == reqObj.EmployeeID.Value)
                )
                .CountAsync();

            var rspObj = new CoEmpPsjwDbGetCountFromMbsWrkJobRecRspMdl()
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

    /// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工作紀錄]</summary>
    public async Task<CoEmpPsjwDbGetManyFromMbsWrkJobRecRspMdl> GetManyFromMbsWrkJobRecAsync(CoEmpPsjwDbGetManyFromMbsWrkJobRecReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from work in this._mainContext.EmployeeProjectStoneJobWork
                        .Where(x =>
                            // 員工ID
                            (reqObj.EmployeeID.HasValue == false || x.EmployeeID == reqObj.EmployeeID.Value)
                            // 員工專案ID
                            && (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID.Value)
                            // 員工專案里程碑ID
                            && (reqObj.EmployeeProjectStoneID.HasValue == false || x.EmployeeProjectStoneID == reqObj.EmployeeProjectStoneID.Value)
                            // 員工專案里程碑工項ID
                            && (reqObj.EmployeeProjectStoneJobID.HasValue == false || x.EmployeeProjectStoneJobID == reqObj.EmployeeProjectStoneJobID.Value)
                            // 員工專案里程碑工項工作開始時間
                            && (reqObj.StartEmployeeProjectStoneJobWorkStartTime.HasValue == false || x.StartTime >= reqObj.StartEmployeeProjectStoneJobWorkStartTime.Value.UtcDateTime)
                            // 員工專案里程碑工項工作結束時間
                            && (reqObj.EndEmployeeProjectStoneJobWorkEndTime.HasValue == false || x.EndTime <= reqObj.EndEmployeeProjectStoneJobWorkEndTime.Value.UtcDateTime)
                        )
                    from emp in this._mainContext.Employee
                        .Where(empx => empx.ID == work.EmployeeID)
                    from prj in this._mainContext.EmployeeProject
                        .Where(prjx => work.EmployeeProjectID.HasValue && prjx.ID == work.EmployeeProjectID.Value)
                        .DefaultIfEmpty()
                    from stone in this._mainContext.EmployeeProjectStone
                        .Where(stonex => work.EmployeeProjectStoneID.HasValue && stonex.ID == work.EmployeeProjectStoneID.Value)
                        .DefaultIfEmpty()
                    from job in this._mainContext.EmployeeProjectStoneJob
                        .Where(jobx => work.EmployeeProjectStoneJobID.HasValue && jobx.ID == work.EmployeeProjectStoneJobID.Value)
                        .DefaultIfEmpty()
                    select new
                    {
                        EmployeeProjectStoneJobWorkID = work.ID,
                        EmployeeProjectStoneJobWorkStartTime = work.StartTime,
                        EmployeeProjectStoneJobWorkEndTime = work.EndTime,
                        EmployeeProjectStoneJobWorkRemark = work.Remark,
                        EmployeeProjectID = work.EmployeeProjectID,
                        EmployeeProjectName = prj.Name,
                        EmployeeProjectStoneID = work.EmployeeProjectStoneID,
                        EmployeeProjectStoneName = stone.Name,
                        EmployeeProjectStoneJobID = work.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobName = job.Name,
                        EmployeeID = work.EmployeeID,
                        EmployeeName = emp.Name,
                    }
                ).AsNoTracking()
                .OrderByDescending(x => x.EmployeeProjectStoneJobWorkID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpPsjwDbGetManyFromMbsWrkJobRecRspMdl()
            {
                EmployeeProjectStoneJobWorkList = itemList
                    .Select(x => new CoEmpPsjwDbGetManyFromMbsWrkJobRecRspItemMdl
                    {
                        EmployeeProjectStoneJobWorkID = x.EmployeeProjectStoneJobWorkID,
                        EmployeeProjectStoneJobWorkStartTime = x.EmployeeProjectStoneJobWorkStartTime,
                        EmployeeProjectStoneJobWorkEndTime = x.EmployeeProjectStoneJobWorkEndTime,
                        EmployeeProjectStoneJobWorkRemark = x.EmployeeProjectStoneJobWorkRemark?.Trim(),
                        EmployeeProjectID = x.EmployeeProjectID,
                        EmployeeProjectName = x.EmployeeProjectName?.Trim(),
                        EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                        EmployeeProjectStoneName = x.EmployeeProjectStoneName?.Trim(),
                        EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName?.Trim(),
                        EmployeeID = x.EmployeeID,
                        EmployeeName = x.EmployeeName?.Trim(),
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
}
