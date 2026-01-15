using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Region.Format;

namespace ServiceLibrary.Core.Manager.DB.Region.Service;

/// <summary>核心-管理者-地區-資料庫服務</summary>
public class CoMgrRegionDbService : ICoMgrRegionDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrRegionDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrRegionDbService(
        ILogger<CoMgrRegionDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-地區-資料庫-是否存在</summary>
    public async Task<CoMgrRgnDbExistRspMdl> ExistAsync(CoMgrRgnDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerRegionID.HasValue == false || x.ID == reqObj.ManagerRegionID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerRegionName) || x.Name == reqObj.ManagerRegionName));

            var rspObj = new CoMgrRgnDbExistRspMdl
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

    /// <summary>核心-管理者-地區-資料庫-新增</summary>
    public async Task<CoMgrRgnDbAddRspMdl> AddAsync(CoMgrRgnDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerRegion()
            {
                Name = reqObj.ManagerRegionName.Trim(),
                IsEnable = reqObj.ManagerRegionIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };
            this._mainContext.ManagerRegion.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrRgnDbAddRspMdl()
            {
                ManagerRegionID = item.ID
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

    /// <summary>核心-管理者-地區-資料庫-新增[ID]</summary>
    public async Task<CoMgrRgnDbAddWithIdRspMdl> AddWithIdAsync(CoMgrRgnDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            // SQL參數
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.ManagerRegionId),
                new SqlParameter("@Name", reqObj.ManagerRegionName.Trim()),
                new SqlParameter("@IsEnable", reqObj.ManagerRegionIsEnable),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.ManagerRegion ON;
                INSERT INTO dbo.ManagerRegion (ID, Name, IsEnable, CreateTime, UpdateTime)
                VALUES (@ID, @Name, @IsEnable, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.ManagerRegion OFF;
            ";
            var affectedCount = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameters);

            var rspObj = new CoMgrRgnDbAddWithIdRspMdl()
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

    /// <summary>核心-管理者-地區-資料庫-更新</summary>
    public async Task<CoMgrRgnDbUpdateRspMdl> UpdateAsync(CoMgrRgnDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerRegion
                .Where(x => x.ID == reqObj.ManagerRegionID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerRegionName) ? x.Name : reqObj.ManagerRegionName.Trim())
                        .SetProperty(x => x.IsEnable, x => reqObj.ManagerRegionIsEnable ?? x.IsEnable)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrRgnDbUpdateRspMdl()
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

    /// <summary>核心-管理者-地區-資料庫-取得</summary>
    public async Task<CoMgrRgnDbGetRspMdl> GetAsync(CoMgrRgnDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerRegionID);
            if (item == default)
            {
                return default;
            }

            var rspObj = new CoMgrRgnDbGetRspMdl()
            {
                ManagerRegionName = item.Name.Trim(),
                ManagerRegionIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-地區-資料庫-取得[名稱]</summary>
    public async Task<CoMgrRgnDbGetNameRspMdl> GetNameAsync(CoMgrRgnDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .Where(x => x.ID == reqObj.ManagerRegionID)
                .Select(x => new
                {
                    x.Name,
                })
                .SingleOrDefaultAsync();

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrRgnDbGetNameRspMdl
            {
                ManagerRegionName = item.Name?.Trim(),
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

    /// <summary>核心-管理者-地區-資料庫-取得多筆</summary>
    public async Task<CoMgrRgnDbGetManyRspMdl> GetManyAsync(CoMgrRgnDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .Where(x => reqObj.ManagerRegionIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrRgnDbGetManyRspMdl()
            {
                ManagerRegionList = itemList
                    .Select(x => new CoMgrRgnDbGetManyRspItemMdl()
                    {
                        ManagerRegionID = x.ID,
                        ManagerRegionName = x.Name.Trim(),
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

    #region ManagerBackSite.Basic.Region 管理者後台-基本-地區

    /// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-基本-地區]</summary>
    public async Task<CoMgrRgnDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscRgnAsync(CoMgrRgnDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .Where(x =>
                    // 地區-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerRegionName) || x.Name.Contains(reqObj.ManagerRegionName))
                    // 地區-是否啟用
                    && (reqObj.ManagerRegionIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerRegionIsEnable.Value))
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable
                })
                .ToListAsync();

            var rspObj = new CoMgrRgnDbGetManyFromMbsBscRspMdl()
            {
                ManagerRegionList = itemList
                    .Select(x => new CoMgrRgnDbGetManyFromMbsBscRspItemMdl()
                    {
                        ManagerRegionID = x.ID,
                        ManagerRegionName = x.Name.Trim(),
                        ManagerRegionIsEnable = x.IsEnable,
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

    /// <summary>核心-管理者-地區-資料庫-取得全部從[管理者後台-基本-地區]</summary>
    public async Task<CoMgrRgnDbGetAllFromMbsBscRspMdl> GetAllFromMbsBscRgnAsync(CoMgrRgnDbGetAllFromMbsBscReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .OrderBy(x => x.ID)
                .Select(x => new CoMgrRgnDbGetAllFromMbsBscRspItemMdl
                {
                    ManagerRegionID = x.ID,
                    ManagerRegionName = x.Name.Trim(),
                })
                .ToListAsync();

            var rspObj = new CoMgrRgnDbGetAllFromMbsBscRspMdl
            {
                ManagerRegionList = itemList
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

    #region ManagerBackSite.System.Region 管理者後台-系統設定-地區

    /// <summary>核心-管理者-地區-資料庫-取得[筆數]從[管理者後台-系統設定-地區]</summary>
    public async Task<CoMgrRgnDbGetCountFromMbsSysRgnRspMdl> GetCountFromMbsSysRgnAsync(CoMgrRgnDbGetCountFromMbsSysRgnReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .Where(x =>
                    // 地區-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerRegionName) || x.Name.Contains(reqObj.ManagerRegionName))
                    // 地區-是否啟用
                    && (reqObj.ManagerRegionIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerRegionIsEnable.Value))
                .CountAsync();

            var rspObj = new CoMgrRgnDbGetCountFromMbsSysRgnRspMdl
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

    /// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-系統設定-地區]</summary>
    public async Task<CoMgrRgnDbGetManyFromMbsSysRgnRspMdl> GetManyFromMbsSysRgnAsync(CoMgrRgnDbGetManyFromMbsSysRgnReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerRegion
                .AsNoTracking()
                .Where(x =>
                    // 地區-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerRegionName) || x.Name.Contains(reqObj.ManagerRegionName))
                    // 地區-是否啟用
                    && (reqObj.ManagerRegionIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerRegionIsEnable.Value))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable
                })
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrRgnDbGetManyFromMbsSysRgnRspMdl()
            {
                ManagerRegionList = itemList
                    .Select(x => new CoMgrRgnDbGetManyFromMbsSysRgnRspItemMdl()
                    {
                        ManagerRegionID = x.ID,
                        ManagerRegionName = x.Name.Trim(),
                        ManagerRegionIsEnable = x.IsEnable,
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