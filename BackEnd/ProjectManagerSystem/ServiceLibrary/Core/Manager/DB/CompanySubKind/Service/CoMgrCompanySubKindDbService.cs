using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;

/// <summary>核心-管理者-公司子分類-資料庫服務</summary>
public class CoMgrCompanySubKindDbService : ICoMgrCompanySubKindDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrCompanySubKindDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrCompanySubKindDbService(
        ILogger<CoMgrCompanySubKindDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆</summary>
    public async Task<CoMgrCmpSubKdDbGetManyRspMdl> GetManyAsync(CoMgrCmpSubKdDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .Where(x =>
                    // 是否啟用
                    (reqObj.ManagerCompanySubKindIsEnable.HasValue == false || reqObj.ManagerCompanySubKindIsEnable.Value == x.IsEnable) &&
                    // 名稱查詢
                    (string.IsNullOrEmpty(reqObj.ManagerCompanySubKindName) || x.Name.Contains(reqObj.ManagerCompanySubKindName.Trim())) &&
                    // 主分類ID查詢
                    (reqObj.ManagerCompanyMainKindID.HasValue == false || reqObj.ManagerCompanyMainKindID.Value == x.ManagerCompanyMainKindID)
                   )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.ManagerCompanyMainKindID,
                    x.IsEnable,
                })
                .ToListAsync();

            var rspObj = new CoMgrCmpSubKdDbGetManyRspMdl
            {
                ManagerCompanySubKindList = itemList?
                    .Select(x => new CoMgrCmpSubKdDbGetManyRspItemMdl
                    {
                        ManagerCompanySubKindID = x.ID,
                        ManagerCompanySubKindName = x.Name,
                        ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                        ManagerCompanySubKindIsEnable = x.IsEnable
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

    /// <summary>核心-管理者-公司子分類-資料庫-取得</summary>
    public async Task<CoMgrCmpSubKdDbGetRspMdl> GetAsync(CoMgrCmpSubKdDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerCompanySubKindID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpSubKdDbGetRspMdl
            {
                ManagerCompanySubKindID = item.ID,
                ManagerCompanySubKindName = item.Name?.Trim(),
                ManagerCompanyMainKindID = item.ManagerCompanyMainKindID,
                ManagerCompanySubKindIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-公司子分類-資料庫-新增</summary>
    public async Task<CoMgrCmpSubKdDbAddRspMdl> AddAsync(CoMgrCmpSubKdDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerCompanySubKind
            {
                Name = reqObj.ManagerCompanySubKindName?.Trim(),
                ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
                IsEnable = reqObj.ManagerCompanySubKindIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerCompanySubKind.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpSubKdDbAddRspMdl
            {
                ManagerCompanySubKindID = item.ID
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

    /// <summary>核心-管理者-公司子分類-資料庫-新增[指定ID]</summary>
    public async Task<CoMgrCmpSubKdDbAddWithIdRspMdl> AddWithIdAsync(CoMgrCmpSubKdDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            // SQL參數
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.ManagerCompanySubKindId),
                new SqlParameter("@Name", reqObj.ManagerCompanySubKindName?.Trim()),
                new SqlParameter("@ManagerCompanyMainKindID", reqObj.ManagerCompanyMainKindID),
                new SqlParameter("@IsEnable", reqObj.ManagerCompanySubKindIsEnable),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.ManagerCompanySubKind ON;
                INSERT INTO dbo.ManagerCompanySubKind (ID, Name, ManagerCompanyMainKindID, IsEnable, CreateTime, UpdateTime)
                VALUES (@ID, @Name, @ManagerCompanyMainKindID, @IsEnable, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.ManagerCompanySubKind OFF;
            ";
            var result = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameters);

            if (result == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpSubKdDbAddWithIdRspMdl
            {
                ManagerCompanySubKindID = reqObj.ManagerCompanySubKindId,
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

    /// <summary>核心-管理者-公司子分類-資料庫-更新</summary>
    public async Task<CoMgrCmpSubKdDbUpdateRspMdl> UpdateAsync(CoMgrCmpSubKdDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerCompanySubKind
                .Where(x => x.ID == reqObj.ManagerCompanySubKindID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanySubKindName) ? reqObj.ManagerCompanySubKindName.Trim() : x.Name)
                    .SetProperty(x => x.IsEnable, x => reqObj.ManagerCompanySubKindIsEnable.HasValue ? reqObj.ManagerCompanySubKindIsEnable.Value : x.IsEnable)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrCmpSubKdDbUpdateRspMdl
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

    /// <summary>核心-管理者-公司子分類-資料庫-取得[名稱]</summary>
    public async Task<CoMgrCmpSubKdDbGetNameRspMdl> GetNameAsync(CoMgrCmpSubKdDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .Where(x => x.ID == reqObj.ManagerCompanySubKindID)
                .Select(x => new
                {
                    x.Name
                })
                .SingleOrDefaultAsync();

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpSubKdDbGetNameRspMdl
            {
                ManagerCompanySubKindName = item.Name?.Trim()
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

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrCmpSubKdDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCmpSubKdDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .Where(x => reqObj.ManagerCompanySubKindIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoMgrCmpSubKdDbGetManyNameRspMdl
            {
                ManagerCompanySubKindList = itemList.Select(x => new CoMgrCmpSubKdDbGetManyByNameRspItemMdl
                {
                    ManagerCompanySubKindID = x.ID,
                    ManagerCompanySubKindName = x.Name?.Trim()
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

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-客戶

    /// <summary>核心-管理者-公司子分類-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]</summary>
    public async Task<CoMgrCmpSubKdDbGetCountFromMbsSysCmpRspMdl> GetCountFromMbsSysCmpAsync(CoMgrCmpSubKdDbGetCountFromMbsSysCmpReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .Where(x =>
                    // 是否啟用
                    (reqObj.ManagerCompanySubKindIsEnable.HasValue == false || reqObj.ManagerCompanySubKindIsEnable.Value == x.IsEnable) &&
                    // 名稱查詢
                    (string.IsNullOrEmpty(reqObj.ManagerCompanySubKindName) || x.Name.Contains(reqObj.ManagerCompanySubKindName.Trim())) &&
                    // 主分類ID查詢
                    (reqObj.ManagerCompanyMainKindID.HasValue == false || reqObj.ManagerCompanyMainKindID.Value == x.ManagerCompanyMainKindID)
                   )
                .CountAsync();

            var rspObj = new CoMgrCmpSubKdDbGetCountFromMbsSysCmpRspMdl
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
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]</summary>
    public async Task<CoMgrCmpSubKdDbGetManyFromMbsSysCmpRspMdl> GetManyFromMbsSysCmpAsync(CoMgrCmpSubKdDbGetManyFromMbsSysCmpReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .Where(x =>
                    // 是否啟用
                    (reqObj.ManagerCompanySubKindIsEnable.HasValue == false || reqObj.ManagerCompanySubKindIsEnable.Value == x.IsEnable) &&
                    // 名稱查詢
                    (string.IsNullOrEmpty(reqObj.ManagerCompanySubKindName) || x.Name.Contains(reqObj.ManagerCompanySubKindName.Trim())) &&
                    // 主分類ID查詢
                    (reqObj.ManagerCompanyMainKindID.HasValue == false || reqObj.ManagerCompanyMainKindID.Value == x.ManagerCompanyMainKindID)
                   )
                   .OrderByDescending(x => x.ID)
                   .Skip(skipRows)
                   .Take(takeRows)
                   .ToListAsync();

            var rspObj = new CoMgrCmpSubKdDbGetManyFromMbsSysCmpRspMdl
            {
                ManagerCompanySubKindList = itemList?
                    .Select(x => new CoMgrCmpSubKdDbGetManyFrommbsSysCmpRspItemMdl
                    {
                        ManagerCompanySubKindID = x.ID,
                        ManagerCompanySubKindName = x.Name?.Trim(),
                        ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                        ManagerCompanySubKindIsEnable = x.IsEnable
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

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrCmpSubKdDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpSubKdDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompanySubKind
                .AsNoTracking()
                .Where(x =>
                    // 管理者公司主分類-ID
                    (reqObj.ManagerCompanyMainKindID.HasValue == false || x.ManagerCompanyMainKindID == reqObj.ManagerCompanyMainKindID) &&
                    // 管理者公司子分類-名稱(模糊查詢)
                    (string.IsNullOrEmpty(reqObj.ManagerCompanySubKindName) || x.Name.Contains(reqObj.ManagerCompanySubKindName.Trim())) &&
                    // 管理者公司子分類-是否啟用
                    (reqObj.ManagerCompanySubKindIsEnable.HasValue == false || reqObj.ManagerCompanySubKindIsEnable.Value == x.IsEnable)
                )
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable
                })
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrCmpSubKdDbGetManyFromMbsBscRspMdl
            {
                ManagerCompanySubKindList = itemList
                    .Select(x => new CoMgrCmpSubKdDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerCompanySubKindID = x.ID,
                        ManagerCompanySubKindName = x.Name,
                        ManagerCompanySubKindIsEnable = x.IsEnable,
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