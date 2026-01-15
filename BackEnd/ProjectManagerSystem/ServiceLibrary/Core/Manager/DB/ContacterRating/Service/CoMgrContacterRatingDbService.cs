using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ContacterRating.Format;

namespace ServiceLibrary.Core.Manager.DB.ContacterRating.Service;

/// <summary>核心-管理者-窗口開發評等-資料庫服務</summary>
public class CoMgrContacterRatingDbService : ICoMgrContacterRatingDbService
{
    private readonly ProjectManagerMainContext _mainContext;
    private readonly ILogger<CoMgrContacterRatingDbService> _logger;

    public CoMgrContacterRatingDbService(
        ProjectManagerMainContext mainContext,
        ILogger<CoMgrContacterRatingDbService> logger)
    {
        this._mainContext = mainContext;
        this._logger = logger;
    }

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-取得多筆</summary>
    public async Task<CoMgrCttRtgDbGetManyRspMdl> GetManyAsync(CoMgrCttRtgDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerContacterRating
                .AsNoTracking()
                .Where(x => x.ManagerContacterID == reqObj.ManagerContacterID)
                .OrderByDescending(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.ManagerContacterRatingReasonID,
                    x.Remark
                })
                .ToListAsync();

            var rspObj = new CoMgrCttRtgDbGetManyRspMdl
            {
                ManagerContacterRatingList = itemList.Select(x => new CoMgrCttRtgDbGetManyRspItemMdl
                {
                    ManagerContacterRatingID = x.ID,
                    ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                    ManagerContacterRatingRemark = x.Remark?.Trim()
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

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-取得</summary>
    public async Task<CoMgrCttRtgDbGetRspMdl> GetAsync(CoMgrCttRtgDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerContacterRating
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerContacterRatingID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCttRtgDbGetRspMdl
            {
                ManagerContacterID = item.ManagerContacterID,
                ManagerContacterRatingReasonID = item.ManagerContacterRatingReasonID,
                ManagerContacterRatingRemark = item.Remark?.Trim()
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

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-新增</summary>
    public async Task<CoMgrCttRtgDbAddRspMdl> AddAsync(CoMgrCttRtgDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerContacterRating
            {
                ManagerContacterID = reqObj.ManagerContacterID,
                ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
                Remark = reqObj.ManagerContacterRatingRemark,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            await this._mainContext.ManagerContacterRating.AddAsync(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCttRtgDbAddRspMdl
            {
                ManagerContacterRatingID = item.ID,
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

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-新增多筆</summary>
    public async Task<CoMgrCttRtgDbAddManyRspMdl> AddManyAsync(CoMgrCttRtgDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.ManagerContacterRatingList
                .Select(x => new ManagerContacterRating
                {
                    ManagerContacterID = x.ManagerContacterID,
                    ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                    Remark = x.ManagerContacterRatingRemark,
                    CreateTime = currentTime,
                    UpdateTime = currentTime
                }).ToList();

            this._mainContext.ManagerContacterRating.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCttRtgDbAddManyRspMdl
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

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-更新</summary>
    public async Task<CoMgrCttRtgDbUpdateRspMdl> UpdateAsync(CoMgrCttRtgDbUpdateReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerContacterRating
                .Where(x => x.ID == reqObj.ManagerContacterRatingID)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.ManagerContacterRatingReasonID, x => reqObj.ManagerContacterRatingReasonID.HasValue ? reqObj.ManagerContacterRatingReasonID.Value : x.ManagerContacterRatingReasonID)
                    .SetProperty(x => x.Remark, x => reqObj.ManagerContacterRatingRemark != null ? reqObj.ManagerContacterRatingRemark : x.Remark)
                    .SetProperty(x => x.UpdateTime, DateTimeOffset.UtcNow));

            var rspObj = new CoMgrCttRtgDbUpdateRspMdl
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

    /// <summary>核心-管理者-窗口開發評等-資料庫服務-移除</summary>
    public async Task<CoMgrCttRtgDbRemoveRspMdl> RemoveAsync(CoMgrCttRtgDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerContacterRating
                .Where(x => x.ID == reqObj.ManagerContacterRatingID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrCttRtgDbRemoveRspMdl
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
