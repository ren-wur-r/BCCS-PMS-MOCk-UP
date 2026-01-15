using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;

/// <summary>核心-管理者-產品主分類-資料庫服務</summary>
public class CoMgrProductMainKindDbService : ICoMgrProductMainKindDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrProductMainKindDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrProductMainKindDbService(
        ILogger<CoMgrProductMainKindDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-產品主分類-資料庫-是否存在</summary>
    public async Task<CoMgrPmkDbExistRspMdl> ExistAsync(CoMgrPmkDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerProductMainKindID.HasValue == false || x.ID != reqObj.ManagerProductMainKindID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerProductMainKindName) || x.Name == reqObj.ManagerProductMainKindName));

            var rspObj = new CoMgrPmkDbExistRspMdl
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

    /// <summary>核心-管理者-產品主分類-資料庫-新增</summary>
    public async Task<CoMgrPmkDbAddRspMdl> AddAsync(CoMgrPmkDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.ManagerProductMainKind
            {
                Name = reqObj.ManagerProductMainKindName.Trim(),
                CommissionRate = reqObj.ManagerProductMainKindCommissionRate,
                IsEnable = reqObj.ManagerProductMainKindIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerProductMainKind.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrPmkDbAddRspMdl
            {
                ManagerProductMainKindID = item.ID,
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

    /// <summary>核心-管理者-產品主分類-資料庫-更新</summary>
    public async Task<CoMgrPmkDbUpdateRspMdl> UpdateAsync(CoMgrPmkDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerProductMainKind
                .Where(x => x.ID == reqObj.ManagerProductMainKindID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerProductMainKindName) ? x.Name : reqObj.ManagerProductMainKindName.Trim())
                    .SetProperty(x => x.CommissionRate, x => reqObj.ManagerProductMainKindCommissionRate ?? x.CommissionRate)
                    .SetProperty(x => x.IsEnable, x => reqObj.ManagerProductMainKindIsEnable ?? x.IsEnable)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrPmkDbUpdateRspMdl
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

    /// <summary>核心-管理者-產品主分類-資料庫-取得</summary>
    public async Task<CoMgrPmkDbGetRspMdl> GetAsync(CoMgrPmkDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerProductMainKindID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrPmkDbGetRspMdl
            {
                ManagerProductMainKindID = item.ID,
                ManagerProductMainKindName = item.Name.Trim(),
                ManagerProductMainKindCommissionRate = item.CommissionRate,
                ManagerProductMainKindIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆</summary>
    public async Task<CoMgrPmkDbGetManyRspMdl> GetManyAsync(CoMgrPmkDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .Where(x =>
                    (reqObj.ManagerProductMainKindIdList == null || reqObj.ManagerProductMainKindIdList.Contains(x.ID))
                    // 名稱查詢
                    && (string.IsNullOrEmpty(reqObj.ManagerProductMainKindName) || x.Name.Contains(reqObj.ManagerProductMainKindName)
                    // 是否啟用
                    && (reqObj.ManagerProductMainKindIsEnable.HasValue == false || reqObj.ManagerProductMainKindIsEnable.Value == x.IsEnable))
                )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.CommissionRate,
                    x.IsEnable
                })
                .ToListAsync();

            var rspObj = new CoMgrPmkDbGetManyRspMdl
            {
                ManagerProductMainKindList = itemList
                    .Select(x => new CoMgrPmkDbGetManyRspItemMdl
                    {
                        ManagerProductMainKindID = x.ID,
                        ManagerProductMainKindName = x.Name.Trim(),
                        ManagerProductMainKindCommissionRate = x.CommissionRate,
                        ManagerProductMainKindIsEnable = x.IsEnable
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

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrPmkDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPmkDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .Where(x => reqObj.ManagerProductMainKindIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrPmkDbGetManyNameRspMdl
            {
                ManagerProductMainKindList = itemList
                    .Select(x => new CoMgrPmkDbGetManyNameRspItemMdl
                    {
                        ManagerProductMainKindID = x.ID,
                        ManagerProductMainKindName = x.Name.Trim(),
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

    #region ManagerBackSite.Basic.ProductMainKind 管理者後台-基本-產品

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrPmkDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPmkDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .Where(x =>
                    // 產品主分類-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerProductMainKindName) || x.Name.Contains(reqObj.ManagerProductMainKindName))
                    // 產品主分類-是否啟用
                    && (reqObj.ManagerProductMainKindIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductMainKindIsEnable.Value))
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable,
                })
                .ToListAsync();

            var rspObj = new CoMgrPmkDbGetManyFromMbsBscRspMdl
            {
                ManagerProductMainKindList = itemList
                    .Select(x => new CoMgrPmkDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerProductMainKindID = x.ID,
                        ManagerProductMainKindName = x.Name.Trim(),
                        ManagerProductMainKindIsEnable = x.IsEnable,
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

    #region ManagerBackSite.System.ProductMainKind 管理者後台-系統設定-產品[主分類]

    /// <summary>核心-管理者-產品主分類-資料庫-取得[筆數]從[管理者後台-系統設定-產品]</summary>
    public async Task<CoMgrPmkDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPmkDbGetCountFromMbsSysPrdReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .Where(x =>
                    // 名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerProductMainKindName) || x.Name.Contains(reqObj.ManagerProductMainKindName))
                    // 是否啟用
                    && (reqObj.ManagerProductMainKindIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductMainKindIsEnable.Value))
                .CountAsync();

            var rspObj = new CoMgrPmkDbGetCountFromMbsSysPrdRspMdl
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

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-系統設定-產品]</summary>
    public async Task<CoMgrPmkDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPmkDbGetManyFromMbsSysPrdReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerProductMainKind
                .AsNoTracking()
                .Where(x =>
                    // 產品主分類-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerProductMainKindName) || x.Name.Contains(reqObj.ManagerProductMainKindName))
                    // 產品主分類-是否啟用
                    && (reqObj.ManagerProductMainKindIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerProductMainKindIsEnable.Value)
                    )
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.CommissionRate,
                    x.IsEnable
                })
                .ToListAsync();

            var rspObj = new CoMgrPmkDbGetManyFromMbsSysPrdRspMdl
            {
                ManagerProductMainKindList = itemList
                    .Select(x => new CoMgrPmkDbGetManyFromMbsSysPrdRspItemMdl
                    {
                        ManagerProductMainKindID = x.ID,
                        ManagerProductMainKindName = x.Name.Trim(),
                        ManagerProductMainKindCommissionRate = x.CommissionRate,
                        ManagerProductMainKindIsEnable = x.IsEnable
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