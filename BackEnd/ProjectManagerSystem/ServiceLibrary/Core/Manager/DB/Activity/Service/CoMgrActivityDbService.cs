using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomManagerActivity;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Activity.Format;

namespace ServiceLibrary.Core.Manager.DB.Activity.Service;

/// <summary>核心-管理者-活動-資料庫服務</summary>
public class CoMgrActivityDbService : ICoMgrActivityDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivityDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivityDbService(
        ILogger<CoMgrActivityDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動-資料庫-是否存在</summary>
    public async Task<CoMgrActivityDbExistRspMdl> ExistAsync(CoMgrActivityDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerActivity
                .AsNoTracking()
                .AnyAsync(x => x.ID == reqObj.ManagerActivityID);

            var rspObj = new CoMgrActivityDbExistRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-新增</summary>
    public async Task<CoMgrActivityDbAddRspMdl> AddAsync(CoMgrActivityDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerActivity
            {
                AtomManagerActivityKindID = reqObj.ManagerActivityKind.ToInt16(),
                Name = reqObj.ManagerActivityName?.Trim(),
                StartTime = reqObj.ManagerActivityStartTime,
                EndTime = reqObj.ManagerActivityEndTime,
                Location = reqObj.ManagerActivityLocation?.Trim(),
                ExpectedLeadCount = reqObj.ManagerActivityExpectedLeadCount,
                Content = reqObj.ManagerActivityContent?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerActivity.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActivityDbAddRspMdl
            {
                ManagerActivityID = item.ID
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

    /// <summary>核心-管理者-活動-資料庫-更新</summary>
    public async Task<CoMgrActivityDbUpdateRspMdl> UpdateAsync(CoMgrActivityDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivity
                .Where(x => x.ID == reqObj.ManagerActivityID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerActivityName) ? reqObj.ManagerActivityName.Trim() : x.Name)
                    .SetProperty(x => x.AtomManagerActivityKindID, x => reqObj.ManagerActivityKind.HasValue ? reqObj.ManagerActivityKind.Value.ToInt16() : x.AtomManagerActivityKindID)
                    .SetProperty(x => x.StartTime, x => reqObj.ManagerActivityStartTime.HasValue ? reqObj.ManagerActivityStartTime.Value : x.StartTime)
                    .SetProperty(x => x.EndTime, x => reqObj.ManagerActivityEndTime.HasValue ? reqObj.ManagerActivityEndTime.Value : x.EndTime)
                    .SetProperty(x => x.Location, x => !string.IsNullOrWhiteSpace(reqObj.ManagerActivityLocation) ? reqObj.ManagerActivityLocation.Trim() : x.Location)
                    .SetProperty(x => x.ExpectedLeadCount, x => reqObj.ManagerActivityExpectedLeadCount.HasValue ? reqObj.ManagerActivityExpectedLeadCount.Value : x.ExpectedLeadCount)
                    .SetProperty(x => x.EmployeePipelineCount, x => reqObj.ManagerActivityEmployeePipelineCount.HasValue ? reqObj.ManagerActivityEmployeePipelineCount.Value : x.EmployeePipelineCount)
                    .SetProperty(x => x.Content, x => !string.IsNullOrWhiteSpace(reqObj.ManagerActivityContent) ? reqObj.ManagerActivityContent.Trim() : x.Content)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrActivityDbUpdateRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-移除</summary>
    public async Task<CoMgrActivityDbRemoveRspMdl> RemoveAsync(CoMgrActivityDbRemoveReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivity
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivityID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            this._mainContext.ManagerActivity.Remove(item);
            var affectedCount = await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActivityDbRemoveRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-取得</summary>
    public async Task<CoMgrActivityDbGetRspMdl> GetAsync(CoMgrActivityDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivity
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivityID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrActivityDbGetRspMdl
            {
                ManagerActivityName = item.Name?.Trim(),
                ManagerActivityKind = item.AtomManagerActivityKindID.ToEnum<DbAtomManagerActivityKindEnum>(),
                ManagerActivityStartTime = item.StartTime,
                ManagerActivityEndTime = item.EndTime,
                ManagerActivityLocation = item.Location?.Trim(),
                ManagerActivityExpectedLeadCount = item.ExpectedLeadCount,
                ManagerActivityContent = item.Content?.Trim(),
                ManagerActivityEmployeePipelineCount = item.EmployeePipelineCount
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

    /// <summary>核心-管理者-活動-資料庫-增加商機數</summary>
    public async Task<CoMgrActivityDbIncrementEmployeePipelineCountRspMdl> IncrementEmployeePipelineCountAsync(CoMgrActivityDbIncrementEmployeePipelineCountReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivity
                .Where(x => x.ID == reqObj.ManagerActivityID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.EmployeePipelineCount, x => x.EmployeePipelineCount + 1)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrActivityDbIncrementEmployeePipelineCountRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-取得最新過往活動從[窗口Email]</summary>
    public async Task<CoMgrActivityDbGetLatestPastActivityFromContacterEmailRspMdl> GetLatestPastActivityFromContacterEmailAsync(CoMgrActivityDbGetLatestPastActivityFromContacterEmailReqMdl reqObj)
    {
        try
        {
            var item = await (
                from a in this._mainContext.ManagerActivity
                                            .AsNoTracking()
                from p in this._mainContext.EmployeePipeline
                                            .AsNoTracking()
                                            .Where(p =>
                                                p.ManagerActivityID.HasValue &&
                                                p.ManagerActivityID == a.ID &&
                                                (reqObj.FilterManagerActivityID.HasValue == false || p.ManagerActivityID != reqObj.FilterManagerActivityID.Value))
                from c in this._mainContext.EmployeePipelineOriginal
                                            .AsNoTracking()
                                            .Where(c =>
                                                c.EmployeePipelineID == p.ID &&
                                                c.ManagerContacterEmail == reqObj.EmployeePipelineOriginalManagerContacterEmail.Trim())
                orderby a.ID descending
                select new
                {
                    a.ID,
                    a.Name,
                    a.StartTime,
                    a.EndTime,
                }
            )
            .FirstOrDefaultAsync();

            if (item == default)
            {
                return default;
            }

            var rspObj = new CoMgrActivityDbGetLatestPastActivityFromContacterEmailRspMdl
            {
                ManagerActivityName = item.Name?.Trim(),
                ManagerActivityStartTime = item.StartTime,
                ManagerActivityEndTime = item.EndTime
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

    /// <summary>核心-管理者-活動-資料庫-取得[筆數]過往活動從[窗口Email]</summary>
    public async Task<CoMgrActivityDbGetCountPastActivityFromContacterEmailRspMdl> GetCountPastActivityFromContacterEmailAsync(CoMgrActivityDbGetCountPastActivityFromContacterEmailReqMdl reqObj)
    {
        try
        {
            var count = await (
                from a in this._mainContext.ManagerActivity
                                            .AsNoTracking()
                from p in this._mainContext.EmployeePipeline
                                            .AsNoTracking()
                                            .Where(p =>
                                                p.ManagerActivityID.HasValue &&
                                                p.ManagerActivityID == a.ID &&
                                                (reqObj.FilterManagerActivityID.HasValue == false || p.ManagerActivityID != reqObj.FilterManagerActivityID.Value))
                from c in this._mainContext.EmployeePipelineOriginal
                                            .AsNoTracking()
                                            .Where(c =>
                                                c.EmployeePipelineID == p.ID &&
                                                c.ManagerContacterEmail == reqObj.EmployeePipelineOriginalManagerContacterEmail.Trim())
                orderby a.ID descending
                select new
                {
                    a.ID
                }
            )
            .Distinct()
            .CountAsync();

            var rspObj = new CoMgrActivityDbGetCountPastActivityFromContacterEmailRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-取得多筆過往活動從[窗口Email]</summary>
    public async Task<CoMgrActivityDbGetManyPastActivityFromContacterEmailRspMdl> GetManyPastActivityFromContacterEmailAsync(CoMgrActivityDbGetManyPastActivityFromContacterEmailReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await (
                from a in this._mainContext.ManagerActivity
                                            .AsNoTracking()
                from p in this._mainContext.EmployeePipeline
                                            .AsNoTracking()
                                            .Where(p =>
                                                p.ManagerActivityID.HasValue &&
                                                p.ManagerActivityID == a.ID &&
                                                (reqObj.FilterManagerActivityID.HasValue == false || p.ManagerActivityID != reqObj.FilterManagerActivityID.Value))
                from c in this._mainContext.EmployeePipelineOriginal
                                            .AsNoTracking()
                                            .Where(c =>
                                                c.EmployeePipelineID == p.ID &&
                                                c.ManagerContacterEmail == reqObj.EmployeePipelineOriginalManagerContacterEmail.Trim())
                orderby a.ID descending
                select new
                {
                    a.ID,
                    a.Name,
                    a.StartTime,
                    a.EndTime,
                }
            )
            .Distinct()
            .Skip(skipRows)
            .Take(takeRows)
            .ToListAsync();

            var rspObj = new CoMgrActivityDbGetManyPastActivityFromContacterEmailRspMdl
            {
                ManagerActivityList = itemList.Select(x => new CoMgrActivityDbGetManyFromEmployeePipelineRspItemMdl
                {
                    ManagerActivityName = x.Name?.Trim(),
                    ManagerActivityStartTime = x.StartTime,
                    ManagerActivityEndTime = x.EndTime
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

    #region ManagerBackSite.Crm.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]</summary>
    public async Task<CoMgrActivityDbGetCountFromMbsCrmActRspMdl> GetCountFromMbsCrmActAsync(CoMgrActivityDbGetCountFromMbsCrmActReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerActivity
                .AsNoTracking()
                .Where(x =>
                    // 活動名稱
                    (string.IsNullOrEmpty(reqObj.ManagerActivityName) || x.Name.Contains(reqObj.ManagerActivityName.Trim())) &&
                    // 活動類型
                    (reqObj.ManagerActivityKind.HasValue == false || x.AtomManagerActivityKindID == reqObj.ManagerActivityKind.Value.ToInt16()) &&
                    // 開始時間
                    (reqObj.ManagerActivityStartTime.HasValue == false || x.StartTime >= reqObj.ManagerActivityStartTime.Value) &&
                    // 結束時間
                    (reqObj.ManagerActivityEndTime.HasValue == false || x.StartTime <= reqObj.ManagerActivityEndTime.Value)
                )
                .CountAsync();

            var rspObj = new CoMgrActivityDbGetCountFromMbsCrmActRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-活動管理]</summary>
    public async Task<CoMgrActivityDbGetManyFromMbsCrmActRspMdl> GetManyFromMbsCrmActAsync(CoMgrActivityDbGetManyFromMbsCrmActReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerActivity
                .AsNoTracking()
                .Where(x =>
                    // 活動名稱
                    (string.IsNullOrEmpty(reqObj.ManagerActivityName) || x.Name.Contains(reqObj.ManagerActivityName.Trim())) &&
                    // 活動類型
                    (reqObj.ManagerActivityKind.HasValue == false || x.AtomManagerActivityKindID == reqObj.ManagerActivityKind.Value.ToInt16()) &&
                    // 開始時間
                    (reqObj.ManagerActivityStartTime.HasValue == false || x.StartTime >= reqObj.ManagerActivityStartTime.Value) &&
                    // 結束時間
                    (reqObj.ManagerActivityEndTime.HasValue == false || x.StartTime <= reqObj.ManagerActivityEndTime.Value)
                )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.AtomManagerActivityKindID,
                    x.StartTime,
                    x.EndTime,
                    x.Location,
                    x.ExpectedLeadCount,
                    x.EmployeePipelineCount
                })
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrActivityDbGetManyFromMbsCrmActRspMdl
            {
                ManagerActivityList = itemList.Select(x => new CoMgrActivityDbGetManyFromMbsMktActRspItemMdl
                {
                    ManagerActivityID = x.ID,
                    ManagerActivityName = x.Name?.Trim(),
                    ManagerActivityKind = x.AtomManagerActivityKindID.ToEnum<DbAtomManagerActivityKindEnum>(),
                    ManagerActivityStartTime = x.StartTime,
                    ManagerActivityEndTime = x.EndTime,
                    ManagerActivityLocation = x.Location?.Trim(),
                    ManagerActivityExpectedLeadCount = x.ExpectedLeadCount,
                    ManagerActivityEmployeePipelineCount = x.EmployeePipelineCount
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

    #region ManagerBackSite.Crm.Phone 管理者後台-CRM-電銷管理

    /// <summary>核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]</summary>
    public async Task<CoMgrActivityDbGetCountFromMbsCrmPhnRspMdl> GetCountFromMbsCrmPhnAsync(CoMgrActivityDbGetCountFromMbsCrmPhnReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerActivity
                .AsNoTracking()
                .Where(x =>
                    // 活動名稱
                    (string.IsNullOrEmpty(reqObj.ManagerActivityName) || x.Name.Contains(reqObj.ManagerActivityName.Trim())) &&
                    // 活動類型
                    (reqObj.ManagerActivityKind.HasValue == false || x.AtomManagerActivityKindID == reqObj.ManagerActivityKind.Value.ToInt16()) &&
                    // 開始時間
                    (reqObj.ManagerActivityStartTime.HasValue == false || x.StartTime >= reqObj.ManagerActivityStartTime.Value) &&
                    // 結束時間
                    (reqObj.ManagerActivityEndTime.HasValue == false || x.StartTime <= reqObj.ManagerActivityEndTime.Value)
                )
                .CountAsync();

            var rspObj = new CoMgrActivityDbGetCountFromMbsCrmPhnRspMdl
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

    /// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-電銷管理]</summary>
    public async Task<CoMgrActivityDbGetManyFromMbsCrmPhnRspMdl> GetManyFromMbsCrmPhnAsync(CoMgrActivityDbGetManyFromMbsCrmPhnReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerActivity
                .AsNoTracking()
                .Where(x =>
                    // 活動名稱
                    (string.IsNullOrEmpty(reqObj.ManagerActivityName) || x.Name.Contains(reqObj.ManagerActivityName.Trim())) &&
                    // 活動類型
                    (reqObj.ManagerActivityKind.HasValue == false || x.AtomManagerActivityKindID == reqObj.ManagerActivityKind.Value.ToInt16()) &&
                    // 開始時間
                    (reqObj.ManagerActivityStartTime.HasValue == false || x.StartTime >= reqObj.ManagerActivityStartTime.Value) &&
                    // 結束時間
                    (reqObj.ManagerActivityEndTime.HasValue == false || x.StartTime <= reqObj.ManagerActivityEndTime.Value)
                )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.AtomManagerActivityKindID,
                    x.StartTime,
                    x.EndTime,
                    x.EmployeePipelineCount
                })
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrActivityDbGetManyFromMbsCrmPhnRspMdl
            {
                ManagerActivityList = itemList.Select(x => new CoMgrActivityDbGetManyFromMbsCrmPhnRspItemMdl
                {
                    ManagerActivityID = x.ID,
                    ManagerActivityName = x.Name?.Trim(),
                    ManagerActivityKind = x.AtomManagerActivityKindID.ToEnum<DbAtomManagerActivityKindEnum>(),
                    ManagerActivityStartTime = x.StartTime,
                    ManagerActivityEndTime = x.EndTime,
                    ManagerActivityEmployeePipelineCount = x.EmployeePipelineCount
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
