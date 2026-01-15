using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;

/// <summary>核心-管理者-產品規格-資料庫服務</summary>
public class CoMgrProductSpecificationDbService : ICoMgrProductSpecificationDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrProductSpecificationDbService> _logger;

    /// <summary>DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>核心-管理者-產品規格-資料庫服務</summary>
    public CoMgrProductSpecificationDbService(
        ILogger<CoMgrProductSpecificationDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-產品規格-資料庫-是否存在</summary>
    public async Task<CoMgrPsDbExistRspMdl> ExistAsync(CoMgrPsDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerProductSpecification
                .AsNoTracking()
                .AnyAsync(x =>
                    x.ManagerProductID == reqObj.ManagerProductID
                    && (reqObj.ManagerProductSpecificationID.HasValue == false || x.ID != reqObj.ManagerProductSpecificationID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductSpecificationName) || x.Name == reqObj.ManagerProductSpecificationName));

            var rspObj = new CoMgrPsDbExistRspMdl
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

    /// <summary>核心-管理者-產品規格-資料庫-新增</summary>
    public async Task<CoMgrPsDbAddRspMdl> AddAsync(CoMgrPsDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerProductSpecification
            {
                Name = reqObj.ManagerProductSpecificationName.Trim(),
                SellPrice = reqObj.ManagerProductSpecificationSellPrice,
                CostPrice = reqObj.ManagerProductSpecificationCostPrice,
                IsEnable = reqObj.ManagerProductSpecificationIsEnable,
                ManagerProductID = reqObj.ManagerProductID,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerProductSpecification.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrPsDbAddRspMdl
            {
                ManagerProductSpecificationID = item.ID,
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

    /// <summary>核心-管理者-產品規格-資料庫-新增多筆</summary>
    public async Task<CoMgrPsDbAddManyRspMdl> AddManyAsync(CoMgrPsDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var itemList = reqObj.ManagerProductSpecificationList
            .Select(x => new ManagerProductSpecification()
            {
                Name = x.ManagerProductSpecificationName.Trim(),
                SellPrice = x.ManagerProductSpecificationSellPrice,
                CostPrice = x.ManagerProductSpecificationCostPrice,
                IsEnable = x.ManagerProductSpecificationIsEnable,
                ManagerProductID = x.ManagerProductID,
                CreateTime = currentTime,
            })
            .ToList();

        try
        {
            this._mainContext.ManagerProductSpecification.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrPsDbAddManyRspMdl
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

    /// <summary>核心-管理者-產品規格-資料庫-更新</summary>
    public async Task<CoMgrPsDbUpdateRspMdl> UpdateAsync(CoMgrPsDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerProductSpecification
                .Where(x => x.ID == reqObj.ManagerProductSpecificationID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerProductSpecificationName) ? x.Name : reqObj.ManagerProductSpecificationName.Trim())
                    .SetProperty(x => x.SellPrice, x => reqObj.ManagerProductSpecificationSellPrice ?? x.SellPrice)
                    .SetProperty(x => x.CostPrice, x => reqObj.ManagerProductSpecificationCostPrice ?? x.CostPrice)
                    .SetProperty(x => x.IsEnable, x => reqObj.ManagerProductSpecificationIsEnable ?? x.IsEnable)
                    .SetProperty(x => x.UpdateTime, x => currentTime)
                );

            var rspObj = new CoMgrPsDbUpdateRspMdl
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

    /// <summary>核心-管理者-產品規格-資料庫-取得</summary>
    public async Task<CoMgrPsDbGetRspMdl> GetAsync(CoMgrPsDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerProductSpecification
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerProductSpecificationID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrPsDbGetRspMdl
            {
                ManagerProductSpecificationID = item.ID,
                ManagerProductSpecificationName = item.Name.Trim(),
                ManagerProductSpecificationSellPrice = item.SellPrice,
                ManagerProductSpecificationCostPrice = item.CostPrice,
                ManagerProductSpecificationIsEnable = item.IsEnable,
                ManagerProductID = item.ManagerProductID,
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

    /// <summary>核心-管理者-產品規格-資料庫-移除</summary>
    public async Task<CoMgrPsDbRemoveRspMdl> RemoveAsync(CoMgrPsDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerProductSpecification
                .Where(x => x.ID == reqObj.ManagerProductSpecificationID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrPsDbRemoveRspMdl
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

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆</summary>
    public async Task<CoMgrPsDbGetManyRspMdl> GetManyAsync(CoMgrPsDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductSpecification
                .AsNoTracking()
                .Where(x => x.ManagerProductID == reqObj.ManagerProductID)
                .ToListAsync();

            var rspObj = new CoMgrPsDbGetManyRspMdl
            {
                ManagerProductSpecificationList = itemList
                    .Select(x => new CoMgrPsDbGetManyRspItemMdl
                    {
                        ManagerProductSpecificationID = x.ID,
                        ManagerProductSpecificationName = x.Name.Trim(),
                        ManagerProductSpecificationSellPrice = x.SellPrice,
                        ManagerProductSpecificationCostPrice = x.CostPrice,
                        ManagerProductSpecificationIsEnable = x.IsEnable,
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

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrPdtSpcDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPdtSpcDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductSpecification
                .AsNoTracking()
                .Where(x => reqObj.ManagerProductSpecificationIDList.Contains(x.ID))
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoMgrPdtSpcDbGetManyNameRspMdl
            {
                ManagerProductSpecificationList = itemList.Select(x => new CoMgrPdtSpcDbGetManyNameRspItemMdl
                {
                    ManagerProductSpecificationID = x.ID,
                    ManagerProductSpecificationName = x.Name?.Trim()
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
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆從[產品ID]</summary>
    public async Task<CoMgrPsDbGetManyFromManagerProductIDRspMdl> GetManyFromManagerProductIDAsync(CoMgrPsDbGetManyFromManagerProductIDReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductSpecification
                .AsNoTracking()
                .Where(x => reqObj.ManagerProductIDList.Contains(x.ManagerProductID) &&
                            (reqObj.ManagerProductSpecificationIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductSpecificationIsEnable.Value))
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.SellPrice,
                    x.CostPrice,
                    x.IsEnable,
                    x.ManagerProductID,
                })
                .ToListAsync();

            var rspObj = new CoMgrPsDbGetManyFromManagerProductIDRspMdl
            {
                ManagerProductSpecificationList = itemList
                    .Select(x => new CoMgrPsDbGetManyFromManagerProductIDRspItemMdl
                    {
                        ManagerProductSpecificationID = x.ID,
                        ManagerProductSpecificationName = x.Name?.Trim(),
                        ManagerProductSpecificationSellPrice = x.SellPrice,
                        ManagerProductSpecificationCostPrice = x.CostPrice,
                        ManagerProductSpecificationIsEnable = x.IsEnable,
                        ManagerProductID = x.ManagerProductID,
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

    #region ManagerBackSite.Basic.ProductSpecification 管理者後台-基本-產品規格

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-基本-產品規格]</summary>
    public async Task<CoMgrPsDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPsDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerProductSpecification
                .AsNoTracking()
                .Where(x =>
                    // 產品-ID
                    (reqObj.ManagerProductID == x.ManagerProductID)
                    // 產品規格-名稱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductSpecificationName) || x.Name.Contains(reqObj.ManagerProductSpecificationName))
                    // 產品規格-是否啟用
                    && (reqObj.ManagerProductSpecificationIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductSpecificationIsEnable.Value))
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.SellPrice,
                    x.CostPrice,
                    x.IsEnable,
                    x.ManagerProductID,
                })
                .ToListAsync();

            var rspObj = new CoMgrPsDbGetManyFromMbsBscRspMdl
            {
                ManagerProductSpecificationList = itemList
                    .Select(x => new CoMgrPsDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerProductSpecificationID = x.ID,
                        ManagerProductSpecificationName = x.Name.Trim(),
                        ManagerProductSpecificationSellPrice = x.SellPrice,
                        ManagerProductSpecificationCostPrice = x.CostPrice,
                        ManagerProductSpecificationIsEnable = x.IsEnable,
                        ManagerProductID = x.ManagerProductID,
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

    #region ManagerBackSite.System.Product 管理者後台-系統設定-產品

    /// <summary>核心-管理者-產品規格-資料庫-取得[筆數]從[管理者後台-系統設定-產品]</summary>
    public async Task<CoMgrPsDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPsDbGetCountFromMbsSysPrdReqMdl reqObj)
    {
        try
        {
            var count = await
                (
                    from mp in this._mainContext.ManagerProduct
                        .Where(mpx =>
                            // 產品-主分類ID
                            (reqObj.ManagerProductMainKindID.HasValue == false || mpx.ManagerProductMainKindID == reqObj.ManagerProductMainKindID.Value)
                            // 產品-子分類ID
                            && (reqObj.ManagerProductSubKindID.HasValue == false || mpx.ManagerProductSubKindID == reqObj.ManagerProductSubKindID.Value)
                            // 產品-是否為主力產品
                            && (reqObj.ManagerProductIsKey.HasValue == false || mpx.IsKey == reqObj.ManagerProductIsKey.Value)
                            // 產品-名稱
                            && (string.IsNullOrWhiteSpace(reqObj.ManagerProductName) || mpx.Name.Contains(reqObj.ManagerProductName))
                        )
                    from mps in this._mainContext.ManagerProductSpecification
                        .Where(mpsx =>
                            mpsx.ManagerProductID == mp.ID
                            // 產品規格-名稱
                            && (string.IsNullOrWhiteSpace(reqObj.ManagerProductSpecificationName) || mpsx.Name.Contains(reqObj.ManagerProductSpecificationName))
                            // 產品規格-是否啟用
                            && (reqObj.ManagerProductSpecificationIsEnable.HasValue == false || mpsx.IsEnable == reqObj.ManagerProductSpecificationIsEnable.Value)
                        )
                    from mk in this._mainContext.ManagerProductMainKind
                        .Where(mkx => mkx.ID == mp.ManagerProductMainKindID)
                    from sk in this._mainContext.ManagerProductSubKind
                        .Where(skx => skx.ID == mp.ManagerProductSubKindID)
                    select 1
                ).CountAsync();

            var rspObj = new CoMgrPsDbGetCountFromMbsSysPrdRspMdl
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

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-系統設定-產品]</summary>
    public async Task<CoMgrPsDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPsDbGetManyFromMbsSysPrdReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from mp in this._mainContext.ManagerProduct
                        .Where(mpx =>
                            // 產品-ID
                            (reqObj.ManagerProductID.HasValue == false || mpx.ID == reqObj.ManagerProductID.Value)
                            // 產品-主分類ID
                            && (reqObj.ManagerProductMainKindID.HasValue == false || mpx.ManagerProductMainKindID == reqObj.ManagerProductMainKindID.Value)
                            // 產品-子分類ID
                            && (reqObj.ManagerProductSubKindID.HasValue == false || mpx.ManagerProductSubKindID == reqObj.ManagerProductSubKindID.Value)
                            // 產品-是否為主力產品
                            && (reqObj.ManagerProductIsKey.HasValue == false || mpx.IsKey == reqObj.ManagerProductIsKey.Value)
                            // 產品-名稱
                            && (string.IsNullOrWhiteSpace(reqObj.ManagerProductName) || mpx.Name.Contains(reqObj.ManagerProductName))
                        )
                        .OrderByDescending(x => x.ID)
                    from mps in this._mainContext.ManagerProductSpecification
                        .Where(mpsx =>
                            mpsx.ManagerProductID == mp.ID
                            // 產品規格-名稱
                            && (string.IsNullOrWhiteSpace(reqObj.ManagerProductSpecificationName) || mpsx.Name.Contains(reqObj.ManagerProductSpecificationName))
                            // 產品規格-是否啟用
                            && (reqObj.ManagerProductSpecificationIsEnable.HasValue == false || mpsx.IsEnable == reqObj.ManagerProductSpecificationIsEnable.Value)
                        )
                    from mk in this._mainContext.ManagerProductMainKind
                        .Where(mkx => mkx.ID == mp.ManagerProductMainKindID)
                    from sk in this._mainContext.ManagerProductSubKind
                        .Where(skx => skx.ID == mp.ManagerProductSubKindID)
                    select new
                    {
                        // 產品
                        ManagerProductID = mp.ID,
                        ManagerProductName = mp.Name,
                        ManagerProductMainKindID = mp.ManagerProductMainKindID,
                        ManagerProductSubKindID = mp.ManagerProductSubKindID,
                        ManagerProductIsKey = mp.IsKey,
                        // 產品規格
                        SpecificationName = mps.Name,
                        SpecificationSellPrice = mps.SellPrice,
                        SpecificationCostPrice = mps.CostPrice,
                        SpecificationIsEnable = mps.IsEnable,
                        // 產品主分類
                        ManagerProductMainKindName = mk.Name,
                        // 產品子分類
                        ManagerProductSubKindName = sk.Name,
                    }
                ).AsNoTracking()
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrPsDbGetManyFromMbsSysPrdRspMdl
            {
                ManagerProductSpecificationList = itemList
                    .Select(x => new CoMgrPsDbGetManyFromMbsSysPrdRspItemMdl
                    {
                        // 產品
                        ManagerProductID = x.ManagerProductID,
                        ManagerProductName = x.ManagerProductName?.Trim(),
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductMainKindName = x.ManagerProductMainKindName?.Trim(),
                        ManagerProductSubKindID = x.ManagerProductSubKindID,
                        ManagerProductSubKindName = x.ManagerProductSubKindName?.Trim(),
                        ManagerProductIsKey = x.ManagerProductIsKey,
                        // 產品規格
                        ManagerProductSpecificationName = x.SpecificationName?.Trim(),
                        ManagerProductSpecificationSellPrice = x.SpecificationSellPrice,
                        ManagerProductSpecificationCostPrice = x.SpecificationCostPrice,
                        ManagerProductSpecificationIsEnable = x.SpecificationIsEnable,
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