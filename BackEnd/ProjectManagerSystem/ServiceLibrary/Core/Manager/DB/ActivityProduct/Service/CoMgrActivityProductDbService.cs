using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Service;

/// <summary>核心-管理者-活動產品-資料庫服務</summary>
public class CoMgrActivityProductDbService : ICoMgrActivityProductDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivityProductDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivityProductDbService(
        ILogger<CoMgrActivityProductDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動產品-資料庫-是否存在</summary>
    public async Task<CoMgrActPrdDbExistRspMdl> ExistAsync(CoMgrActPrdDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerActivityProduct
                .AsNoTracking()
                .AnyAsync(x =>
                    (x.ID == reqObj.ManagerProductID)
                    && (x.ManagerActivityID == reqObj.ManagerActivityID));

            var rspObj = new CoMgrActPrdDbExistRspMdl
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

    /// <summary>核心-管理者-活動產品-資料庫-新增</summary>
    public async Task<CoMgrActPrdDbAddRspMdl> AddAsync(CoMgrActPrdDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerActivityProduct
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                ManagerProductID = reqObj.ManagerProductID,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerActivityProduct.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActPrdDbAddRspMdl
            {
                ManagerActivityProductID = item.ID
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

    /// <summary>核心-管理者-活動產品-資料庫-新增多筆</summary>
    public async Task<CoMgrActPrdDbAddManyRspMdl> AddManyAsync(CoMgrActPrdDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.ManagerActivityProductList
                        .Select(x => new ManagerActivityProduct()
                        {
                            ManagerActivityID = x.ManagerActivityID,
                            ManagerProductID = x.ManagerProductID,
                            CreateTime = currentTime,
                            UpdateTime = currentTime,
                        })
                        .ToList();

            this._mainContext.ManagerActivityProduct.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActPrdDbAddManyRspMdl()
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

    /// <summary>核心-管理者-活動產品-資料庫-更新</summary>
    public async Task<CoMgrActPrdDbUpdateRspMdl> UpdateAsync(CoMgrActPrdDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivityProduct
                .Where(x => x.ID == reqObj.ManagerActivityProductID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerProductID, x => reqObj.ManagerProductID.HasValue ? reqObj.ManagerProductID.Value : x.ManagerProductID)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrActPrdDbUpdateRspMdl
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

    /// <summary>核心-管理者-活動產品-資料庫-移除</summary>
    public async Task<CoMgrActPrdDbRemoveRspMdl> RemoveAsync(CoMgrActPrdDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivityProduct
                .Where(x => x.ID == reqObj.ManagerActivityProductID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrActPrdDbRemoveRspMdl
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

    /// <summary>核心-管理者-活動產品-資料庫-取得</summary>
    public async Task<CoMgrActPrdDbGetRspMdl> GetAsync(CoMgrActPrdDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivityProduct
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivityProductID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrActPrdDbGetRspMdl
            {
                ManagerActivityProductID = item.ID,
                ManagerActivityID = item.ManagerActivityID,
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

    /// <summary>核心-管理者-活動產品-資料庫-取得多筆</summary>
    public async Task<CoMgrActPrdDbGetManyRspMdl> GetManyAsync(CoMgrActPrdDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivityProduct
                .AsNoTracking()
                .Where(x =>
                    // 管理者活動ID
                    x.ManagerActivityID == reqObj.ManagerActivityID
                )
                .Select(x => new CoMgrActPrdDbGetManyRspItemMdl
                {
                    ManagerActivityProductID = x.ID,
                    ManagerActivityID = x.ManagerActivityID,
                    ManagerProductID = x.ManagerProductID
                })
                .OrderByDescending(x => x.ManagerActivityProductID)
                .ToListAsync();

            var rspObj = new CoMgrActPrdDbGetManyRspMdl
            {
                ManagerActivityProductList = itemList
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
}
