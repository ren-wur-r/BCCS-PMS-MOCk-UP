using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.ManagerProduct;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Product.Format;

namespace ServiceLibrary.Core.Manager.DB.Product.Service;

/// <summary>核心-管理者-產品-資料庫服務</summary>
public class CoMgrProductDbService : ICoMgrProductDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrProductDbService> _logger;

    /// <summary>DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>核心-管理者-產品-資料庫服務</summary>
    public CoMgrProductDbService(
        ILogger<CoMgrProductDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-產品-資料庫-是否存在</summary>
    public async Task<CoMgrPrdDbExistRspMdl> ExistAsync(CoMgrPrdDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerProductID.HasValue == false || x.ID != reqObj.ManagerProductID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductName) || x.Name == reqObj.ManagerProductName));

            var rspObj = new CoMgrPrdDbExistRspMdl
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

    /// <summary>核心-管理者-產品-資料庫-取得</summary>
    public async Task<CoMgrPrdDbGetRspMdl> GetAsync(CoMgrPrdDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerProductID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrPrdDbGetRspMdl
            {
                ManagerProductID = item.ID,
                ManagerProductName = item.Name.Trim(),
                ManagerProductMainKindID = item.ManagerProductMainKindID,
                ManagerProductSubKindID = item.ManagerProductSubKindID,
                ManagerProductKind = item.KindID.ToEnum<DbManagerProductKindEnum>(),
                ManagerProductIsKey = item.IsKey,
                ManagerProductRemark = item.Remark?.Trim(),
                ManagerProductIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-產品-資料庫-新增</summary>
    public async Task<CoMgrPrdDbAddRspMdl> AddAsync(CoMgrPrdDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerProduct
            {
                Name = reqObj.ManagerProductName.Trim(),
                ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
                ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
                KindID = reqObj.ManagerProductKind.ToInt16(),
                IsKey = reqObj.ManagerProductIsKey,
                Remark = reqObj.ManagerProductRemark?.Trim(),
                IsEnable = reqObj.ManagerProductIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerProduct.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrPrdDbAddRspMdl
            {
                ManagerProductID = item.ID,
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

    /// <summary>核心-管理者-產品-資料庫-更新</summary>
    public async Task<CoMgrPrdDbUpdateRspMdl> UpdateAsync(CoMgrPrdDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerProduct
                .Where(x => x.ID == reqObj.ManagerProductID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerProductName) ? x.Name : reqObj.ManagerProductName.Trim())
                    .SetProperty(x => x.ManagerProductMainKindID, x => reqObj.ManagerProductMainKindID ?? x.ManagerProductMainKindID)
                    .SetProperty(x => x.ManagerProductSubKindID, x => reqObj.ManagerProductSubKindID ?? x.ManagerProductSubKindID)
                    .SetProperty(x => x.KindID, x => reqObj.ManagerProductKind.HasValue ? reqObj.ManagerProductKind.Value.ToInt16() : x.KindID)
                    .SetProperty(x => x.IsKey, x => reqObj.ManagerProductIsKey ?? x.IsKey)
                    .SetProperty(x => x.Remark, x => string.IsNullOrWhiteSpace(reqObj.ManagerProductRemark) ? x.Remark : reqObj.ManagerProductRemark.Trim())
                    .SetProperty(x => x.IsEnable, x => reqObj.ManagerProductIsEnable ?? x.IsEnable)
                    .SetProperty(x => x.UpdateTime, currentTime)
                );

            var rspObj = new CoMgrPrdDbUpdateRspMdl
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

    /// <summary>核心-管理者-產品-資料庫-移除</summary>
    public async Task<CoMgrPrdDbRemoveRspMdl> RemoveAsync(CoMgrPrdDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerProduct
                .Where(x => x.ID == reqObj.ManagerProductID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrPrdDbRemoveRspMdl
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

    /// <summary>核心-管理者-產品-資料庫-取得多筆</summary>
    public async Task<CoMgrPrdDbGetManyRspMdl> GetManyAsync(CoMgrPrdDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .Where(x =>
                    reqObj.ManagerProductIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductIsEnable.Value)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrPrdDbGetManyRspMdl
            {
                ManagerProductList = itemList
                    .Select(x => new CoMgrPrdDbGetManyRspItemMdl
                    {
                        ManagerProductID = x.ID,
                        ManagerProductName = x.Name.Trim(),
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

    /// <summary>核心-管理者-產品-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrPrdDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPrdDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .Where(x => reqObj.ManagerProductIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrPrdDbGetManyNameRspMdl
            {
                ManagerProductList = itemList?.Select(x => new CoMgrPrdDbGetManyNameRspItemMdl
                {
                    ManagerProductID = x.ID,
                    ManagerProductName = x.Name.Trim(),
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

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[產品ID]</summary>
    public async Task<CoMgrPrdDbGetManyFromManagerProductIDRspMdl> GetManyFromManagerProductIDAsync(CoMgrPrdDbGetManyFromManagerProductIDReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .Where(x => reqObj.ManagerProductIDList.Contains(x.ID) &&
                           (reqObj.ManagerProductIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductIsEnable.Value))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.ManagerProductMainKindID,
                    x.ManagerProductSubKindID,
                    x.KindID,
                })
                .ToListAsync();

            var rspObj = new CoMgrPrdDbGetManyFromManagerProductIDRspMdl
            {
                ManagerProductList = itemList?.Select(x => new CoMgrPrdDbGetManyFromManagerProductIDRspItemMdl
                {
                    ManagerProductID = x.ID,
                    ManagerProductName = x.Name.Trim(),
                    ManagerProductMainKindID = x.ManagerProductMainKindID,
                    ManagerProductSubKindID = x.ManagerProductSubKindID,
                    ManagerProductKind = x.KindID.ToEnum<DbManagerProductKindEnum>(),
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

    #region ManagerBackSite.System.Product 管理者後台-系統設定-產品

    /// <summary>核心-管理者-產品-資料庫-取得[筆數]從[管理者後台-系統設定-產品]</summary>
    public async Task<CoMgrPrdDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPrdDbGetCountFromMbsSysPrdReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .Where(x =>
                    // 產品-主分類ID
                    (reqObj.ManagerProductMainKindID.HasValue == false || x.ManagerProductMainKindID == reqObj.ManagerProductMainKindID.Value)
                    // 產品-子分類ID
                    && (reqObj.ManagerProductSubKindID.HasValue == false || x.ManagerProductSubKindID == reqObj.ManagerProductSubKindID.Value)
                    // 產品-是否為主力產品
                    && (reqObj.ManagerProductIsKey.HasValue == false || x.IsKey == reqObj.ManagerProductIsKey.Value)
                    // 產品-名稱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductName) || x.Name.Contains(reqObj.ManagerProductName))
                )
                .CountAsync();

            var rspObj = new CoMgrPrdDbGetCountFromMbsSysPrdRspMdl
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

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-系統設定-產品]</summary>
    public async Task<CoMgrPrdDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPrdDbGetManyFromMbsSysPrdReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .Where(x =>
                    // 產品-主分類ID
                    (reqObj.ManagerProductMainKindID.HasValue == false || x.ManagerProductMainKindID == reqObj.ManagerProductMainKindID.Value)
                    // 產品-子分類ID
                    && (reqObj.ManagerProductSubKindID.HasValue == false || x.ManagerProductSubKindID == reqObj.ManagerProductSubKindID.Value)
                    // 產品-是否為主力產品
                    && (reqObj.ManagerProductIsKey.HasValue == false || x.IsKey == reqObj.ManagerProductIsKey.Value)
                    // 產品-名稱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductName) || x.Name.Contains(reqObj.ManagerProductName))
                )
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.ManagerProductMainKindID,
                    x.ManagerProductSubKindID,
                    x.IsKey,
                })
                .ToListAsync();

            var rspObj = new CoMgrPrdDbGetManyFromMbsSysPrdRspMdl
            {
                ManagerProductList = itemList
                    .Select(x => new CoMgrPrdDbGetManyFromMbsSysPrdRspItemMdl
                    {
                        ManagerProductID = x.ID,
                        ManagerProductName = x.Name.Trim(),
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductSubKindID = x.ManagerProductSubKindID,
                        ManagerProductIsKey = x.IsKey,
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

    #region ManagerBackSite.Marketing.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-CRM-活動管理]</summary>
    public async Task<CoMgrPrdDbGetManyFromMbsMktActRspMdl> GetManyFromMbsMktActAsync(CoMgrPrdDbGetManyFromMbsMktActReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProduct
                .AsNoTracking()
                .Where(x =>
                    // 管理者活動ID
                    reqObj.ManagerProductIDList.Contains(x.ID)
                )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.ManagerProductMainKindID,
                    x.ManagerProductSubKindID,
                })
                .ToListAsync();

            var rspObj = new CoMgrPrdDbGetManyFromMbsMktActRspMdl
            {
                ManagerProductList = itemList?
                .Select(x => new CoMgrPrdDbGetManyFromMbsMktActRspItemMdl
                {
                    ManagerProductID = x.ID,
                    ManagerProductName = x.Name.Trim(),
                    ManagerProductMainKindID = x.ManagerProductMainKindID,
                    ManagerProductSubKindID = x.ManagerProductSubKindID
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

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrPrdDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPrdDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
            (
                from p in this._mainContext.ManagerProduct
                        .Where(p =>
                            // 產品名稱
                            (string.IsNullOrEmpty(reqObj.ManagerProductName) || p.Name.Contains(reqObj.ManagerProductName.Trim())) &&
                            // 產品主類別ID
                            (reqObj.ManagerProductMainKindID.HasValue == false || p.ManagerProductMainKindID == reqObj.ManagerProductMainKindID.Value) &&
                            // 產品子類別ID
                            (reqObj.ManagerProductSubKindID.HasValue == false || p.ManagerProductSubKindID == reqObj.ManagerProductSubKindID.Value)
                        )
                select new
                {
                    p.ID,
                    p.Name,
                    p.IsEnable,
                    p.ManagerProductMainKindID,
                    p.ManagerProductSubKindID
                }
            )
            .Skip(skipRows)
            .Take(takeRows)
            .ToListAsync();

            var rspObj = new CoMgrPrdDbGetManyFromMbsBscRspMdl
            {
                ManagerProductList = itemList
                    .Select(x => new CoMgrPrdDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerProductID = x.ID,
                        ManagerProductName = x.Name?.Trim(),
                        ManagerProductIsEnable = x.IsEnable,
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductSubKindID = x.ManagerProductSubKindID
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
