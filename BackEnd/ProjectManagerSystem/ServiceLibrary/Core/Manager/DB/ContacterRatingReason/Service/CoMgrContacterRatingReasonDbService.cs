using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Service;

/// <summary>核心-管理者-窗口開發評等原因-資料庫服務</summary>
public class CoMgrContacterRatingReasonDbService : ICoMgrContacterRatingReasonDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrContacterRatingReasonDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrContacterRatingReasonDbService(
        ILogger<CoMgrContacterRatingReasonDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-是否存在</summary>
    public async Task<CoMgrCttRtgRsnDbExistRspMdl> ExistAsync(CoMgrCttRtgRsnDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .AnyAsync(x => x.ID == reqObj.ManagerContacterRatingReasonID);

            var rspObj = new CoMgrCttRtgRsnDbExistRspMdl
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆</summary>
    public async Task<CoMgrCttRtgRsnDbGetManyRspMdl> GetManyAsync(CoMgrCttRtgRsnDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .Where(x =>
                    // 狀態
                    (reqObj.ManagerContacterRatingReasonStatus.HasValue == false || reqObj.ManagerContacterRatingReasonStatus.Value == x.Status) &&
                    // 名稱
                    (string.IsNullOrEmpty(reqObj.ManagerContacterRatingReasonName) || x.Name.Contains(reqObj.ManagerContacterRatingReasonName))
                   )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Status
                })
                .ToListAsync();

            var rspObj = new CoMgrCttRtgRsnDbGetManyRspMdl
            {
                ManagerContacterRatingReasonList = itemList?
                    .Select(x => new CoMgrCttRtgRsnDbGetManyRspItemMdl
                    {
                        ManagerContacterRatingReasonID = x.ID,
                        ManagerContacterRatingReasonName = x.Name?.Trim(),
                        ManagerContacterRatingReasonStatus = x.Status
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得</summary>
    public async Task<CoMgrCttRtgRsnDbGetRspMdl> GetAsync(CoMgrCttRtgRsnDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerContacterRatingReasonID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCttRtgRsnDbGetRspMdl
            {
                ManagerContacterRatingReasonName = item.Name?.Trim(),
                ManagerContacterRatingReasonStatus = item.Status
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-新增</summary>
    public async Task<CoMgrCttRtgRsnDbAddRspMdl> AddAsync(CoMgrCttRtgRsnDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerContacterRatingReason
            {
                Name = reqObj.ManagerContacterRatingReasonName?.Trim(),
                Status = reqObj.ManagerContacterRatingReasonStatus,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerContacterRatingReason.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCttRtgRsnDbAddRspMdl
            {
                ManagerContacterRatingReasonID = item.ID
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-更新</summary>
    public async Task<CoMgrCttRtgRsnDbUpdateRspMdl> UpdateAsync(CoMgrCttRtgRsnDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerContacterRatingReason
                .Where(x => x.ID == reqObj.ManagerContacterRatingReasonID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerContacterRatingReasonName) ? reqObj.ManagerContacterRatingReasonName.Trim() : x.Name)
                    .SetProperty(x => x.Status, x => reqObj.ManagerContacterRatingReasonStatus.HasValue ? reqObj.ManagerContacterRatingReasonStatus.Value : x.Status)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrCttRtgRsnDbUpdateRspMdl
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrCttRtgRsnDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCttRtgRsnDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .Where(x => reqObj.ManagerContacterRatingReasonIDList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoMgrCttRtgRsnDbGetManyNameRspMdl
            {
                ManagerContacterRatingReasonList = itemList.Select(x => new CoMgrCttRtgRsnDbGetManyNameRspItemMdl
                {
                    ManagerContacterRatingReasonID = x.ID,
                    ManagerContacterRatingReasonName = x.Name?.Trim()
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-是否存在多筆</summary>
    public async Task<CoMgrCttRtgRsnDbExistManyRspMdl> ExistManyAsync(CoMgrCttRtgRsnDbExistManyReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .Where(x => reqObj.ManagerContacterRatingReasonIDList.Contains(x.ID))
                .CountAsync();

            var rspObj = new CoMgrCttRtgRsnDbExistManyRspMdl
            {
                IsExist = count == reqObj.ManagerContacterRatingReasonIDList.Count,
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

    #region ManagerBackSite.Customer.Contacter 管理者後台-系統設定-窗口

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得[筆數]從[管理者後台-系統設定-窗口]</summary>
    public async Task<CoMgrCttRtgRsnDbGetCountFromMbsSysCtrRspMdl> GetCountFromMbsSysCtrAsync(CoMgrCttRtgRsnDbGetCountFromMbsSysCtrReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .Where(x =>
                    // 狀態
                    (reqObj.ManagerContacterRatingReasonStatus.HasValue == false || x.Status == reqObj.ManagerContacterRatingReasonStatus.Value) &&
                    // 名稱
                    (string.IsNullOrEmpty(reqObj.ManagerContacterRatingReasonName) || x.Name.Contains(reqObj.ManagerContacterRatingReasonName.Trim()))
                )
                .CountAsync();

            var rspObj = new CoMgrCttRtgRsnDbGetCountFromMbsSysCtrRspMdl
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

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-客戶名單-客戶窗口]</summary>
    public async Task<CoMgrCttRtgRsnDbGetManyFromMbsSysCtrRspMdl> GetManyFromMbsSysCtrAsync(CoMgrCttRtgRsnDbGetManyFromMbsSysCtrReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .Where(x =>
                    // 狀態
                    (reqObj.ManagerContacterRatingReasonStatus.HasValue == false || x.Status == reqObj.ManagerContacterRatingReasonStatus.Value) &&
                    // 名稱
                    (string.IsNullOrEmpty(reqObj.ManagerContacterRatingReasonName) || x.Name.Contains(reqObj.ManagerContacterRatingReasonName.Trim()))
                )
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Status
                })
                .ToListAsync();

            var rspObj = new CoMgrCttRtgRsnDbGetManyFromMbsSysCtrRspMdl
            {
                ManagerContacterRatingReasonList = itemList
                    .Select(x => new CoMgrCttRtgRsnDbGetManyFrommbsSysCtrRspItemMdl
                    {
                        ManagerContacterRatingReasonID = x.ID,
                        ManagerContacterRatingReasonName = x.Name?.Trim(),
                        ManagerContacterRatingReasonStatus = x.Status
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

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrCttRtgRsnDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCttRtgRsnDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerContacterRatingReason
                .AsNoTracking()
                .Where(x =>
                    // 狀態
                    (reqObj.ManagerContacterRatingReasonStatus.HasValue == false || x.Status == reqObj.ManagerContacterRatingReasonStatus.Value) &&
                    // 名稱
                    (string.IsNullOrEmpty(reqObj.ManagerContacterRatingReasonName) || x.Name.Contains(reqObj.ManagerContacterRatingReasonName.Trim()))
                )
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Status
                })
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrCttRtgRsnDbGetManyFromMbsBscRspMdl
            {
                ManagerContacterRatingReasonList = itemList
                    .Select(x => new CoMgrCttRtgRsnDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerContacterRatingReasonID = x.ID,
                        ManagerContacterRatingReasonName = x.Name?.Trim(),
                        ManagerContacterRatingReasonStatus = x.Status
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
