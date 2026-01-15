using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Service;

/// <summary>核心-管理者-活動贊助商-資料庫服務</summary>
public class CoMgrActivitySponsorDbService : ICoMgrActivitySponsorDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivitySponsorDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivitySponsorDbService(
        ILogger<CoMgrActivitySponsorDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動贊助商-資料庫-是否存在</summary>
    public async Task<CoMgrActSpsDbExistRspMdl> ExistAsync(CoMgrActSpsDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerActivitySponsor
                .AsNoTracking()
                .AnyAsync(x =>
                    (x.ID == reqObj.ManagerActivityID)
                    && (x.SponsorName == reqObj.ManagerActivitySponsorName.Trim()));

            var rspObj = new CoMgrActSpsDbExistRspMdl
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

    /// <summary>核心-管理者-活動贊助商-資料庫-新增</summary>
    public async Task<CoMgrActSpsDbAddRspMdl> AddAsync(CoMgrActSpsDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerActivitySponsor
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                SponsorName = reqObj.ManagerActivitySponsorName?.Trim(),
                SponsorAmount = reqObj.ManagerActivitySponsorAmount,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerActivitySponsor.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActSpsDbAddRspMdl
            {
                ManagerActivitySponsorID = item.ID
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

    /// <summary>核心-管理者-活動贊助商-資料庫-新增多筆</summary>
    public async Task<CoMgrActSpsDbAddManyRspMdl> AddManyAsync(CoMgrActSpsDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.ManagerActivitySponsorList
                        .Select(x => new ManagerActivitySponsor()
                        {
                            ManagerActivityID = x.ManagerActivityID,
                            SponsorName = x.ManagerActivitySponsorName?.Trim(),
                            SponsorAmount = x.ManagerActivitySponsorAmount,
                            CreateTime = currentTime,
                            UpdateTime = currentTime,
                        })
                        .ToList();

            this._mainContext.ManagerActivitySponsor.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActSpsDbAddManyRspMdl()
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

    /// <summary>核心-管理者-活動贊助商-資料庫-更新</summary>
    public async Task<CoMgrActSpsDbUpdateRspMdl> UpdateAsync(CoMgrActSpsDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySponsor
                .Where(x => x.ID == reqObj.ManagerActivitySponsorID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerActivityID, x => reqObj.ManagerActivityID.HasValue ? reqObj.ManagerActivityID.Value : x.ManagerActivityID)
                    .SetProperty(x => x.SponsorName, x => !string.IsNullOrWhiteSpace(reqObj.ManagerActivitySponsorName) ? reqObj.ManagerActivitySponsorName.Trim() : x.SponsorName)
                    .SetProperty(x => x.SponsorAmount, x => reqObj.ManagerActivitySponsorAmount.HasValue ? reqObj.ManagerActivitySponsorAmount.Value : x.SponsorAmount)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrActSpsDbUpdateRspMdl
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

    /// <summary>核心-管理者-活動贊助商-資料庫-移除</summary>
    public async Task<CoMgrActSpsDbRemoveRspMdl> RemoveAsync(CoMgrActSpsDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySponsor
                .Where(x => x.ID == reqObj.ManagerActivitySponsorID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrActSpsDbRemoveRspMdl
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

    /// <summary>核心-管理者-活動贊助商-資料庫-取得</summary>
    public async Task<CoMgrActSpsDbGetRspMdl> GetAsync(CoMgrActSpsDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivitySponsor
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivitySponsorID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrActSpsDbGetRspMdl
            {
                ManagerActivityID = item.ManagerActivityID,
                ManagerActivitySponsorName = item.SponsorName?.Trim(),
                ManagerActivitySponsorAmount = item.SponsorAmount
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

    /// <summary>核心-管理者-活動贊助商-資料庫-取得多筆</summary>
    public async Task<CoMgrActSpsDbGetManyRspMdl> GetManyAsync(CoMgrActSpsDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivitySponsor
                .AsNoTracking()
                .Where(x =>
                    // 管理者活動ID
                    (x.ManagerActivityID == reqObj.ManagerActivityID) &&
                    // 管理者活動贊助商-名稱(模糊查詢)
                    (string.IsNullOrEmpty(reqObj.ManagerActivitySponsorName) || x.SponsorName.Contains(reqObj.ManagerActivitySponsorName))
                )
                .Select(x => new
                {
                    x.ID,
                    x.SponsorName,
                    x.SponsorAmount
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoMgrActSpsDbGetManyRspMdl
            {
                ManagerActivitySponsorList = itemList?
                .Select(x => new CoMgrActSpsDbGetManyRspItemMdl
                {
                    ManagerActivitySponsorID = x.ID,
                    ManagerActivitySponsorName = x.SponsorName?.Trim(),
                    ManagerActivitySponsorAmount = x.SponsorAmount
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

    #region ManagerBackSite.Marketing.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-活動贊助商-資料庫-取得多筆總贊助金額從[管理者後台-CRM-活動管理]</summary>
    public async Task<CoMgrActSpsDbGetManyTotalSponsorAmountRspMdl> GetManyTotalSponsorAmountFromMbsMktActAsync(CoMgrActSpsDbGetManyTotalSponsorAmountReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivitySponsor
                                .AsNoTracking()
                                .Where(x =>
                                    // 管理者活動ID列表
                                    reqObj.ManagerActivityIDList.Contains(x.ManagerActivityID)
                                )
                                .GroupBy(x => x.ManagerActivityID)
                                .Select(g => new CoMgrActSpsDbGetManyTotalSponsorAmountRspItemMdl
                                {
                                    ManagerActivityID = g.Key,
                                    ManagerActivitySponsorTotalSponsorAmount = g.Sum(x => x.SponsorAmount)
                                })
                                .ToListAsync();

            var rspObj = new CoMgrActSpsDbGetManyTotalSponsorAmountRspMdl
            {
                ManagerActivitySponsorList = itemList
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
