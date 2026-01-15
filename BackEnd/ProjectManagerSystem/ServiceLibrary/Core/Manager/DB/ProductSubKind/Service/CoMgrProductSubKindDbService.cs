using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;

/// <summary>核心-管理者-產品子分類-資料庫服務</summary>
public class CoMgrProductSubKindDbService : ICoMgrProductSubKindDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrProductSubKindDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrProductSubKindDbService(
        ILogger<CoMgrProductSubKindDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-產品子分類-資料庫-是否存在</summary>
    public async Task<CoMgrPskDbExistRspMdl> ExistAsync(CoMgrPskDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerProductSubKind
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerProductSubKindID.HasValue == false || x.ID == reqObj.ManagerProductSubKindID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductSubKindName) || x.Name == reqObj.ManagerProductSubKindName));

            var rspObj = new CoMgrPskDbExistRspMdl
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

    /// <summary>核心-管理者-產品子分類-資料庫-新增</summary>
    public async Task<CoMgrPskDbAddRspMdl> AddAsync(CoMgrPskDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.ManagerProductSubKind
            {
                ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
                Name = reqObj.ManagerProductSubKindName.Trim(),
                CommissionRate = reqObj.ManagerProductSubKindCommissionRate,
                IsEnable = reqObj.ManagerProductSubKindIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerProductSubKind.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrPskDbAddRspMdl
            {
                ManagerProductSubKindID = item.ID,
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

    /// <summary>核心-管理者-產品子分類-資料庫-更新</summary>
    public async Task<CoMgrPskDbUpdateRspMdl> UpdateAsync(CoMgrPskDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerProductSubKind
                .Where(x => x.ID == reqObj.ManagerProductSubKindID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerProductSubKindName) ? x.Name : reqObj.ManagerProductSubKindName.Trim())
                    .SetProperty(x => x.CommissionRate, x => reqObj.ManagerProductSubKindCommissionRate ?? x.CommissionRate)
                    .SetProperty(x => x.IsEnable, x => reqObj.ManagerProductSubKindIsEnable ?? x.IsEnable)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrPskDbUpdateRspMdl
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

    /// <summary>核心-管理者-產品子分類-資料庫-取得</summary>
    public async Task<CoMgrPskDbGetRspMdl> GetAsync(CoMgrPskDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerProductSubKind
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerProductSubKindID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrPskDbGetRspMdl
            {
                ManagerProductSubKindID = item.ID,
                ManagerProductSubKindName = item.Name.Trim(),
                ManagerProductSubKindCommissionRate = item.CommissionRate,
                ManagerProductMainKindID = item.ManagerProductMainKindID,
                ManagerProductSubKindIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆</summary>
    public async Task<CoMgrPskDbGetManyRspMdl> GetManyAsync(CoMgrPskDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductSubKind
                .AsNoTracking()
                .Where(x =>
                    // 是否啟用
                    (reqObj.ManagerProductSubKindIsEnable.HasValue == false || reqObj.ManagerProductSubKindIsEnable.Value == x.IsEnable)
                    // 名稱查詢
                    && (string.IsNullOrEmpty(reqObj.ManagerProductSubKindName) || x.Name.Contains(reqObj.ManagerProductSubKindName))
                    // 主分類ID查詢
                    && (reqObj.ManagerProductMainKindID.HasValue == false || reqObj.ManagerProductMainKindID.Value == x.ManagerProductMainKindID)
                   )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.CommissionRate,
                    x.ManagerProductMainKindID,
                    x.IsEnable
                })
                .ToListAsync();

            var rspObj = new CoMgrPskDbGetManyRspMdl
            {
                ManagerProductSubKindList = itemList
                    .Select(x => new CoMgrPskDbGetManyRspItemMdl
                    {
                        ManagerProductSubKindID = x.ID,
                        ManagerProductSubKindName = x.Name.Trim(),
                        ManagerProductSubKindCommissionRate = x.CommissionRate,
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductSubKindIsEnable = x.IsEnable
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

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrPskDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPskDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductSubKind
                .AsNoTracking()
                .Where(x => reqObj.ManagerProductSubKindIDList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrPskDbGetManyNameRspMdl
            {
                ManagerProductSubKindList = itemList?.Select(x => new CoMgrPskDbGetManyNameRspItemMdl
                {
                    ManagerProductSubKindID = x.ID,
                    ManagerProductSubKindName = x.Name.Trim(),
                }).ToList(),
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

    #region ManagerBackSite.Basic.ProductSubKind 管理者後台-基本-產品

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrPskDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPskDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerProductSubKind
                .AsNoTracking()
                .Where(x =>
                    // 主分類ID查詢
                    (reqObj.ManagerProductMainKindID.HasValue == false || reqObj.ManagerProductMainKindID.Value == x.ManagerProductMainKindID)
                    // 產品子分類-是否啟用
                    && (reqObj.ManagerProductSubKindIsEnable.HasValue == false || reqObj.ManagerProductSubKindIsEnable.Value == x.IsEnable)
                    // 產品子分類-名稱查詢
                    && (string.IsNullOrEmpty(reqObj.ManagerProductSubKindName) || x.Name.Contains(reqObj.ManagerProductSubKindName)))
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable
                })
                .ToListAsync();

            var rspObj = new CoMgrPskDbGetManyFromMbsBscRspMdl
            {
                ManagerProductSubKindList = itemList
                    .Select(x => new CoMgrPskDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerProductSubKindID = x.ID,
                        ManagerProductSubKindName = x.Name.Trim(),
                        ManagerProductSubKindIsEnable = x.IsEnable
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

    #region ManagerBackSite.System.ProductSubKind 管理者後台-系統設定-產品

    /// <summary>核心-管理者-產品子分類-資料庫-取得[筆數]從[管理者後台-系統設定-產品[子分類]]</summary>
    public async Task<CoMgrPskDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPskDbGetCountFromMbsSysPrdReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerProductSubKind
                .AsNoTracking()
                .Where(x =>
                    // 主分類ID查詢
                    (reqObj.ManagerProductMainKindID.HasValue == false || reqObj.ManagerProductMainKindID.Value == x.ManagerProductMainKindID)
                    // 名稱查詢
                    && (string.IsNullOrEmpty(reqObj.ManagerProductSubKindName) || x.Name.Contains(reqObj.ManagerProductSubKindName))
                    // 是否啟用
                    && (reqObj.ManagerProductSubKindIsEnable.HasValue == false || reqObj.ManagerProductSubKindIsEnable.Value == x.IsEnable)
                )
                .CountAsync();

            var rspObj = new CoMgrPskDbGetCountFromMbsSysPrdRspMdl
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

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-系統設定-產品[子分類]]</summary>
    public async Task<CoMgrPskDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPskDbGetManyFromMbsSysPrdReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from sk in this._mainContext.ManagerProductSubKind
                        .Where(skx =>
                            // 主分類ID查詢
                            (reqObj.ManagerProductMainKindID.HasValue == false || reqObj.ManagerProductMainKindID.Value == skx.ManagerProductMainKindID)
                            // 名稱查詢
                            && (string.IsNullOrWhiteSpace(reqObj.ManagerProductSubKindName) || skx.Name.Contains(reqObj.ManagerProductSubKindName))
                            // 是否啟用
                            && (reqObj.ManagerProductSubKindIsEnable.HasValue == false || reqObj.ManagerProductSubKindIsEnable.Value == skx.IsEnable)
                        )
                    from mk in this._mainContext.ManagerProductMainKind
                        .Where(mkx => mkx.ID == sk.ManagerProductMainKindID)
                    select new
                    {
                        // 產品子分類
                        ManagerProductMainKindID = sk.ManagerProductMainKindID,
                        ManagerProductSubKindID = sk.ID,
                        ManagerProductSubKindName = sk.Name,
                        ManagerProductSubKindCommissionRate = sk.CommissionRate,
                        ManagerProductSubKindIsEnable = sk.IsEnable,
                        ManagerProductSubKindCreateTime = sk.CreateTime,
                        // 產品主分類
                        ManagerProductMainKindName = mk.Name
                    }
                ).AsNoTracking()
                .OrderByDescending(skx => skx.ManagerProductSubKindID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrPskDbGetManyFromMbsSysPrdRspMdl
            {
                ManagerProductSubKindList = itemList
                    .Select(x => new CoMgrPskDbGetManyFromMbsSysPrdRspItemMdl
                    {
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductMainKindName = x.ManagerProductMainKindName?.Trim(),
                        ManagerProductSubKindID = x.ManagerProductSubKindID,
                        ManagerProductSubKindName = x.ManagerProductSubKindName?.Trim(),
                        ManagerProductSubKindCommissionRate = x.ManagerProductSubKindCommissionRate,
                        ManagerProductSubKindIsEnable = x.ManagerProductSubKindIsEnable
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