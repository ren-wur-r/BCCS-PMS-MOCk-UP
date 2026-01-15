using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Service;

/// <summary>核心-員工-商機產品-資料庫服務</summary>
public class CoEmpPipelineProductDbService : ICoEmpPipelineProductDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineProductDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineProductDbService(
        ILogger<CoEmpPipelineProductDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機產品-資料庫-是否存在</summary>
    public async Task<CoEmpPplPrdDbExistRspMdl> ExistAsync(CoEmpPplPrdDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.EmployeePipelineProduct
            .AsNoTracking()
            .AnyAsync(x =>
                x.EmployeePipelineID == reqObj.EmployeePipelineID
                && x.ManagerProductSpecificationID == reqObj.ManagerProductSpecificationID);

            var rspObj = new CoEmpPplPrdDbExistRspMdl
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

    /// <summary>核心-員工-商機產品-資料庫-取得多筆</summary>
    public async Task<CoEmpPplPrdDbGetManyRspMdl> GetManyAsync(CoEmpPplPrdDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from epp in this._mainContext.EmployeePipelineProduct
                        .Where(eppx =>
                            eppx.EmployeePipelineID == reqObj.EmployeePipelineID
                  )
                    from prd in this._mainContext.ManagerProduct
                        .Where(prdx => prdx.ID == epp.ManagerProductID)
                    from mk in this._mainContext.ManagerProductMainKind
                        .Where(mkx => mkx.ID == prd.ManagerProductMainKindID)
                    from sk in this._mainContext.ManagerProductSubKind
                        .Where(skx => skx.ID == prd.ManagerProductSubKindID)
                    from ps in this._mainContext.ManagerProductSpecification
                        .Where(psx => psx.ID == epp.ManagerProductSpecificationID)
                    select new
                    {
                        // 商機產品
                        EmployeePipelineProductID = epp.ID,
                        EmployeePipelineProductSellPrice = epp.SellPrice,
                        EmployeePipelineProductClosingPrice = epp.ClosingPrice,
                        EmployeePipelineProductCostPrice = epp.CostPrice,
                        EmployeePipelineProductCount = epp.Count,
                        EmployeePipelineProductPurchaseKindID = epp.PurchaseKindID.ToEnum<DbAtomEmployeePipelineProductPurchaseKindEnum>(),
                        EmployeePipelineProductContractKindID = epp.ContractKindID.ToEnum<DbAtomEmployeePipelineProductContractKindEnum>(),
                        // 產品名稱
                        ManagerProductID = prd.ID,
                        ManagerProductName = prd.Name,
                        // 產品主分類
                        ManagerProductMainKindID = mk.ID,
                        ManagerProductMainKindName = mk.Name,
                        ManagerProductMainKindCommissionRate = mk.CommissionRate,
                        // 產品子分類
                        ManagerProductSubKindID = sk.ID,
                        ManagerProductSubKindName = sk.Name,
                        // 產品規格
                        ManagerProductSpecificationID = ps.ID,
                        ManagerProductSpecificationName = ps.Name,
                    }
                ).AsNoTracking()
                .OrderBy(eppx => eppx.EmployeePipelineProductID)
                .ToListAsync();

            var rspObj = new CoEmpPplPrdDbGetManyRspMdl
            {
                EmployeePipelineProductList = itemList
                    .Select(x => new CoEmpPplPrdDbGetManyRspItemMdl
                    {
                        EmployeePipelineProductID = x.EmployeePipelineProductID,
                        ManagerProductID = x.ManagerProductID,
                        ManagerProductName = x.ManagerProductName?.Trim(),
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductMainKindName = x.ManagerProductMainKindName?.Trim(),
                        ManagerProductMainKindCommissionRate = x.ManagerProductMainKindCommissionRate,
                        ManagerProductSubKindID = x.ManagerProductSubKindID,
                        ManagerProductSubKindName = x.ManagerProductSubKindName?.Trim(),
                        ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                        ManagerProductSpecificationName = x.ManagerProductSpecificationName?.Trim(),
                        EmployeePipelineProductSellPrice = x.EmployeePipelineProductSellPrice,
                        EmployeePipelineProductClosingPrice = x.EmployeePipelineProductClosingPrice,
                        EmployeePipelineProductCostPrice = x.EmployeePipelineProductCostPrice,
                        EmployeePipelineProductCount = x.EmployeePipelineProductCount,
                        EmployeePipelineProductPurchaseKindID = x.EmployeePipelineProductPurchaseKindID,
                        EmployeePipelineProductContractKindID = x.EmployeePipelineProductContractKindID,
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

    /// <summary>核心-員工-商機產品-資料庫-取得</summary>
    public async Task<CoEmpPplPrdDbGetRspMdl> GetAsync(CoEmpPplPrdDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineProduct
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineProductID);
            if (item == default)
            {
                return default;
            }

            var rspObj = new CoEmpPplPrdDbGetRspMdl()
            {
                EmployeePipelineID = item.EmployeePipelineID,
                ManagerCompanyID = item.ManagerCompanyID,
                ManagerProductID = item.ManagerProductID,
                ManagerProductSpecificationID = item.ManagerProductSpecificationID,
                EmployeePipelineProductSellPrice = item.SellPrice,
                EmployeePipelineProductClosingPrice = item.ClosingPrice,
                EmployeePipelineProductCostPrice = item.CostPrice,
                EmployeePipelineProductCount = item.Count,
                EmployeePipelineProductPurchaseKindID = item.PurchaseKindID.ToEnum<DbAtomEmployeePipelineProductPurchaseKindEnum>(),
                EmployeePipelineProductContractKindID = item.ContractKindID.ToEnum<DbAtomEmployeePipelineProductContractKindEnum>(),
                EmployeePipelineProductContractText = item.ContractText?.Trim() ?? string.Empty,
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

    /// <summary>核心-員工-商機產品-資料庫-新增</summary>
    public async Task<CoEmpPplPrdDbAddRspMdl> AddAsync(CoEmpPplPrdDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineProduct
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                ManagerCompanyID = reqObj.ManagerCompanyID,
                ManagerProductID = reqObj.ManagerProductID,
                ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
                SellPrice = reqObj.EmployeePipelineProductSellPrice,
                ClosingPrice = reqObj.EmployeePipelineProductClosingPrice,
                CostPrice = reqObj.EmployeePipelineProductCostPrice,
                Count = reqObj.EmployeePipelineProductCount,
                PurchaseKindID = reqObj.EmployeePipelineProductPurchaseKindID.ToInt16(),
                ContractKindID = reqObj.EmployeePipelineProductContractKindID.ToInt16(),
                ContractText = reqObj.EmployeePipelineProductContractText?.Trim() ?? string.Empty,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeePipelineProduct.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplPrdDbAddRspMdl
            {
                EmployeePipelineProductID = item.ID,
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

    /// <summary>核心-員工-商機產品-資料庫-新增多筆</summary>
    public async Task<CoEmpPplPrdDbAddManyRspMdl> AddManyAsync(CoEmpPplPrdDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineProductList
                .Select(x => new EmployeePipelineProduct
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    ManagerCompanyID = x.ManagerCompanyID,
                    ManagerProductID = x.ManagerProductID,
                    ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                    SellPrice = x.EmployeePipelineProductSellPrice,
                    ClosingPrice = x.EmployeePipelineProductClosingPrice,
                    CostPrice = x.EmployeePipelineProductCostPrice,
                    Count = x.EmployeePipelineProductCount,
                    PurchaseKindID = x.EmployeePipelineProductPurchaseKindID.ToInt16(),
                    ContractKindID = x.EmployeePipelineProductContractKindID.ToInt16(),
                    ContractText = x.EmployeePipelineProductContractText?.Trim() ?? string.Empty,
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeePipelineProduct.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplPrdDbAddManyRspMdl
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

    /// <summary>核心-員工-商機產品-資料庫-取得數量</summary>
    public async Task<CoEmpPplPrdDbGetCountRspMdl> GetCountAsync(CoEmpPplPrdDbGetCountReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.EmployeePipelineProduct
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .CountAsync();

            var rspObj = new CoEmpPplPrdDbGetCountRspMdl
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

    /// <summary>核心-員工-商機產品-資料庫-更新</summary>
    public async Task<CoEmpPplPrdDbUpdateRspMdl> UpdateAsync(CoEmpPplPrdDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineProduct
                .Where(x => x.ID == reqObj.EmployeePipelineProductID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerProductID, x => reqObj.ManagerProductID.HasValue ? reqObj.ManagerProductID.Value : x.ManagerProductID)
                    .SetProperty(x => x.ManagerProductSpecificationID, x => reqObj.ManagerProductSpecificationID.HasValue ? reqObj.ManagerProductSpecificationID.Value : x.ManagerProductSpecificationID)
                    .SetProperty(x => x.SellPrice, x => reqObj.EmployeePipelineProductSellPrice.HasValue ? reqObj.EmployeePipelineProductSellPrice.Value : x.SellPrice)
                    .SetProperty(x => x.ClosingPrice, x => reqObj.EmployeePipelineProductClosingPrice.HasValue ? reqObj.EmployeePipelineProductClosingPrice.Value : x.ClosingPrice)
                    .SetProperty(x => x.CostPrice, x => reqObj.EmployeePipelineProductCostPrice.HasValue ? reqObj.EmployeePipelineProductCostPrice.Value : x.CostPrice)
                    .SetProperty(x => x.Count, x => reqObj.EmployeePipelineProductCount.HasValue ? reqObj.EmployeePipelineProductCount.Value : x.Count)
                    .SetProperty(x => x.PurchaseKindID, x => reqObj.EmployeePipelineProductPurchaseKindID.HasValue ? reqObj.EmployeePipelineProductPurchaseKindID.Value.ToInt16() : x.PurchaseKindID)
                    .SetProperty(x => x.ContractKindID, x => reqObj.EmployeePipelineProductContractKindID.HasValue ? reqObj.EmployeePipelineProductContractKindID.Value.ToInt16() : x.ContractKindID)
                    .SetProperty(x => x.ContractText, x => (reqObj.EmployeePipelineProductContractText ?? string.Empty).Trim())
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplPrdDbUpdateRspMdl
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

    /// <summary>核心-員工-商機產品-資料庫-更新多筆公司ID</summary>
    public async Task<CoEmpPplPrdDbUpdateManyCompanyIdRspMdl> UpdateManyCompanyIdAsync(CoEmpPplPrdDbUpdateManyCompanyIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineProduct
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerCompanyID, reqObj.ManagerCompanyID)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplPrdDbUpdateManyCompanyIdRspMdl
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

    /// <summary>核心-員工-商機產品-資料庫-移除</summary>
    public async Task<CoEmpPplPrdDbRemoveRspMdl> RemoveAsync(CoEmpPplPrdDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineProduct
                .Where(x => x.ID == reqObj.EmployeePipelineProductID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplPrdDbRemoveRspMdl
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

    /// <summary>核心-員工-商機產品-資料庫-移除多筆</summary>
    public async Task<CoEmpPplPrdDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplPrdDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineProduct
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplPrdDbRemoveManyRspMdl
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

    #region 電銷管理

    /// <summary>核心-員工-商機產品-資料庫-取得多筆從[管理者後台-CRM-電銷管理]</summary>
    public async Task<CoEmpPplPrdDbGetManyFromPhoneRspMdl> GetManyFromPhoneAsync(CoEmpPplPrdDbGetManyFromPhoneReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from epp in this._mainContext.EmployeePipelineProduct
                        .Where(eppx =>
                            eppx.EmployeePipelineID == reqObj.EmployeePipelineID
                  )
                    from prd in this._mainContext.ManagerProduct
                        .Where(prdx => prdx.ID == epp.ManagerProductID)
                    from mk in this._mainContext.ManagerProductMainKind
                        .Where(mkx => mkx.ID == prd.ManagerProductMainKindID)
                    from sk in this._mainContext.ManagerProductSubKind
                        .Where(skx => skx.ID == prd.ManagerProductSubKindID)
                    from ps in this._mainContext.ManagerProductSpecification
                        .Where(psx => psx.ID == epp.ManagerProductSpecificationID)
                    select new
                    {
                        // 商機產品
                        EmployeePipelineProductID = epp.ID,
                        // 產品名稱
                        ManagerProductID = prd.ID,
                        ManagerProductName = prd.Name,
                        // 產品主分類
                        ManagerProductMainKindID = mk.ID,
                        ManagerProductMainKindName = mk.Name,
                        // 產品子分類
                        ManagerProductSubKindID = sk.ID,
                        ManagerProductSubKindName = sk.Name,
                        // 產品規格
                        ManagerProductSpecificationID = ps.ID,
                        ManagerProductSpecificationName = ps.Name,
                        ManagerProductSpecificationSellPrice = ps.SellPrice
                    }
                ).AsNoTracking()
                .OrderBy(eppx => eppx.EmployeePipelineProductID)
                .ToListAsync();

            var rspObj = new CoEmpPplPrdDbGetManyFromPhoneRspMdl
            {
                EmployeePipelineProductList = itemList
                    .Select(x => new CoEmpPplPrdDbGetManyFromPhoneRspItemMdl
                    {
                        EmployeePipelineProductID = x.EmployeePipelineProductID,
                        ManagerProductID = x.ManagerProductID,
                        ManagerProductName = x.ManagerProductName?.Trim(),
                        ManagerProductMainKindID = x.ManagerProductMainKindID,
                        ManagerProductMainKindName = x.ManagerProductMainKindName?.Trim(),
                        ManagerProductSubKindID = x.ManagerProductSubKindID,
                        ManagerProductSubKindName = x.ManagerProductSubKindName?.Trim(),
                        ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                        ManagerProductSpecificationName = x.ManagerProductSpecificationName?.Trim(),
                        ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
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

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-管理者-商機產品-資料庫-取得多筆從[管理者後台-CRM-商機管理]</summary>
    public async Task<CoEmpPplPrdDbGetManyFromCrmPplRspMdl> GetManyFromMbsCrmPplAsync(CoEmpPplPrdDbGetManyFromCrmPplReqMdl reqObj)
    {
        try
        {
            var itemList = await (
                from epp in this._mainContext.EmployeePipelineProduct
                                            .AsNoTracking()
                                            .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                from p in this._mainContext.ManagerProduct
                                            .AsNoTracking()
                                            .Where(x => x.ID == epp.ManagerProductID)
                                            .DefaultIfEmpty()
                from pm in this._mainContext.ManagerProductMainKind
                                            .AsNoTracking()
                                            .Where(x => x.ID == p.ManagerProductMainKindID)
                                            .DefaultIfEmpty()
                from sm in this._mainContext.ManagerProductSubKind
                                            .AsNoTracking()
                                            .Where(x => x.ID == p.ManagerProductSubKindID)
                                            .DefaultIfEmpty()
                from ps in this._mainContext.ManagerProductSpecification
                                            .AsNoTracking()
                                            .Where(x => x.ID == epp.ManagerProductSpecificationID)
                                            .DefaultIfEmpty()
                select new
                {
                    epp.ID,
                    epp.ManagerProductID,
                    epp.ManagerProductSpecificationID,
                    ManagerProductName = p.Name,
                    ManagerProductMainKindName = pm.Name,
                    ManagerProductSubKindName = sm.Name,
                    ManagerProductSpecificationName = ps.Name,
                    epp.SellPrice,
                    epp.ClosingPrice,
                    epp.CostPrice,
                    epp.Count,
                    epp.PurchaseKindID,
                    epp.ContractKindID,
                    epp.ContractText,
                    pm.CommissionRate,
                }
            )
            .OrderByDescending(x => x.ID)
            .ToListAsync();

            var rspObj = new CoEmpPplPrdDbGetManyFromCrmPplRspMdl
            {
                EmployeePipelineProductList = itemList.Select(x => new CoEmpPplPrdDbGetManyFromCrmPplRspItemMdl
                {
                    EmployeePipelineProductID = x.ID,
                    ManagerProductID = x.ManagerProductID,
                    ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                    ManagerProductName = x.ManagerProductName?.Trim(),
                    ManagerProductMainKindName = x.ManagerProductMainKindName?.Trim(),
                    ManagerProductSubKindName = x.ManagerProductSubKindName?.Trim(),
                    ManagerProductSpecificationName = x.ManagerProductSpecificationName?.Trim(),
                    EmployeePipelineProductSellPrice = x.SellPrice,
                    EmployeePipelineProductClosingPrice = x.ClosingPrice,
                    EmployeePipelineProductCostPrice = x.CostPrice,
                    EmployeePipelineProductGrossProfit = x.ClosingPrice - x.CostPrice, // 售價-成本
                    EmployeePipelineProductCount = x.Count,
                    EmployeePipelineProductPurchaseKind = x.PurchaseKindID.ToEnum<DbAtomEmployeePipelineProductPurchaseKindEnum>(),
                    EmployeePipelineProductContractKind = x.ContractKindID.ToEnum<DbAtomEmployeePipelineProductContractKindEnum>(),
                    EmployeePipelineProductContractText = x.ContractText?.Trim() ?? string.Empty,
                    ManagerProductMainKindCommissionRate = x.CommissionRate
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

    /// <summary>核心-員工-商機產品-資料庫-移除多筆根據公司ID不匹配</summary>
    public async Task<CoEmpPplPrdDbRemoveManyByCompanyIdMismatchRspMdl> RemoveManyByCompanyIdMismatchAsync(CoEmpPplPrdDbRemoveManyByCompanyIdMismatchReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineProduct
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID &&
                           x.ManagerCompanyID.HasValue &&
                           x.ManagerCompanyID.Value != reqObj.ManagerCompanyID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplPrdDbRemoveManyByCompanyIdMismatchRspMdl
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