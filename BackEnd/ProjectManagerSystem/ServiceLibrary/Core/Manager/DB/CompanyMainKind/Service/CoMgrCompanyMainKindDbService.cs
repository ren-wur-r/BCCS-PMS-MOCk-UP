using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;

/// <summary>核心-管理者-公司主分類-資料庫服務</summary>
public class CoMgrCompanyMainKindDbService : ICoMgrCompanyMainKindDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrCompanyMainKindDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrCompanyMainKindDbService(
        ILogger<CoMgrCompanyMainKindDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆</summary>
    public async Task<CoMgrCmpMainKdDbGetManyRspMdl> GetManyAsync(CoMgrCmpMainKdDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x =>
                    // 是否啟用
                    (reqObj.ManagerCompanyMainKindIsEnable.HasValue == false || reqObj.ManagerCompanyMainKindIsEnable.Value == x.IsEnable) &&
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyMainKindName) || x.Name.Contains(reqObj.ManagerCompanyMainKindName))
                   )
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable,
                })
                .ToListAsync();

            var rspObj = new CoMgrCmpMainKdDbGetManyRspMdl
            {
                ManagerCompanyMainKindList = itemList?
                    .Select(x => new CoMgrCmpMainKdDbGetManyRspItemMdl
                    {
                        ManagerCompanyMainKindID = x.ID,
                        ManagerCompanyMainKindName = x.Name?.Trim(),
                        ManagerCompanyMainKindIsEnable = x.IsEnable
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

    /// <summary>核心-管理者-公司主分類-資料庫-取得</summary>
    public async Task<CoMgrCmpMainKdDbGetRspMdl> GetAsync(CoMgrCmpMainKdDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerCompanyMainKindID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpMainKdDbGetRspMdl
            {
                ManagerCompanyMainKindName = item.Name?.Trim(),
                ManagerCompanyMainKindIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-公司主分類-資料庫-新增</summary>
    public async Task<CoMgrCmpMainKdDbAddRspMdl> AddAsync(CoMgrCmpMainKdDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerCompanyMainKind
            {
                Name = reqObj.ManagerCompanyMainKindName?.Trim(),
                IsEnable = reqObj.ManagerCompanyMainKindIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerCompanyMainKind.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpMainKdDbAddRspMdl
            {
                ManagerCompanyMainKindID = item.ID,
                ManagerCompanyMainKindCreateTime = item.CreateTime,
                ManagerCompanyMainKindUpdateTime = item.UpdateTime
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

    /// <summary>核心-管理者-公司主分類-資料庫-新增[指定ID]</summary>
    public async Task<CoMgrCmpMainKdDbAddWithIdRspMdl> AddWithIdAsync(CoMgrCmpMainKdDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            // SQL參數
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.ManagerCompanyMainKindId),
                new SqlParameter("@Name", reqObj.ManagerCompanyMainKindName?.Trim()),
                new SqlParameter("@IsEnable", reqObj.ManagerCompanyMainKindIsEnable),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.ManagerCompanyMainKind ON;
                INSERT INTO dbo.ManagerCompanyMainKind (ID, Name, IsEnable, CreateTime, UpdateTime)
                VALUES (@ID, @Name, @IsEnable, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.ManagerCompanyMainKind OFF;
            ";
            var result = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameters);

            if (result == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpMainKdDbAddWithIdRspMdl
            {
                ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindId,
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

    /// <summary>核心-管理者-公司主分類-資料庫-更新</summary>
    public async Task<CoMgrCmpMainKdDbUpdateRspMdl> UpdateAsync(CoMgrCmpMainKdDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerCompanyMainKind
                .Where(x => x.ID == reqObj.ManagerCompanyMainKindID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyMainKindName) ? reqObj.ManagerCompanyMainKindName.Trim() : x.Name)
                    .SetProperty(x => x.IsEnable, x => reqObj.ManagerCompanyMainKindIsEnable.HasValue ? reqObj.ManagerCompanyMainKindIsEnable.Value : x.IsEnable)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrCmpMainKdDbUpdateRspMdl
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

    /// <summary>核心-管理者-公司主分類-資料庫-檢查名稱重複</summary>
    public async Task<CoMgrCmpMainKdDbCheckNameDuplicateRspMdl> CheckNameDuplicateAsync(CoMgrCmpMainKdDbCheckNameDuplicateReqMdl reqObj)
    {
        try
        {
            var duplicateItem = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x =>
                    x.Name == reqObj.ManagerCompanyMainKindName &&
                    (reqObj.ExcludeManagerCompanyMainKindID.HasValue == false || x.ID != reqObj.ExcludeManagerCompanyMainKindID.Value))
                .Select(x => new
                {
                    x.ID
                })
                .FirstOrDefaultAsync();

            var rspObj = new CoMgrCmpMainKdDbCheckNameDuplicateRspMdl
            {
                IsDuplicate = duplicateItem != null,
                DuplicateManagerCompanyMainKindID = duplicateItem?.ID
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

    /// <summary>核心-管理者-公司主分類-資料庫-取得[名稱]</summary>
    public async Task<CoMgrCmpMainKdDbGetNameRspMdl> GetNameAsync(CoMgrCmpMainKdDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x => x.ID == reqObj.ManagerCompanyMainKindID)
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

            var rspObj = new CoMgrCmpMainKdDbGetNameRspMdl
            {
                ManagerCompanyMainKindName = item.Name?.Trim()
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

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrCmpMainKdDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCmpMainKdDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x => reqObj.ManagerCompanyMainKindIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoMgrCmpMainKdDbGetManyNameRspMdl
            {
                ManagerCompanyMainKindList = itemList.Select(x => new CoMgrCmpMainKdDbGetManyNameRspItemMdl
                {
                    ManagerCompanyMainKindID = x.ID,
                    ManagerCompanyMainKindName = x.Name?.Trim()
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

    /// <summary>核心-管理者-公司主分類-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]</summary>
    public async Task<CoMgrCmpMainKdDbGetCountFromMbsSysCmpRspMdl> GetCountFromMbsSysCmpAsync(CoMgrCmpMainKdDbGetCountFromMbsSysCmpReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x =>
                    // 管理者-公司主分類-是否啟用
                    (reqObj.ManagerCompanyMainKindIsEnable.HasValue == false || reqObj.ManagerCompanyMainKindIsEnable.Value == x.IsEnable) &&
                    // 管理者-公司主分類-名稱(模糊查詢)
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyMainKindName) || x.Name.Contains(reqObj.ManagerCompanyMainKindName.Trim()))
                   )
                .CountAsync();

            var rspObj = new CoMgrCmpMainKdDbGetCountFromMbsSysCmpRspMdl
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

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆[管理者後台-系統設定-客戶]</summary>
    public async Task<CoMgrCmpMainKdDbGetManyFromMbsSysCmpRspMdl> GetManyFromMbsSysCmpAsync(CoMgrCmpMainKdDbGetManyFromMbsSysCmpReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x =>
                    // 管理者-公司主分類-是否啟用
                    (reqObj.ManagerCompanyMainKindIsEnable.HasValue == false || reqObj.ManagerCompanyMainKindIsEnable.Value == x.IsEnable) &&
                    // 管理者-公司主分類-名稱(模糊查詢)
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyMainKindName) || x.Name.Contains(reqObj.ManagerCompanyMainKindName.Trim()))
                )
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

            var rspObj = new CoMgrCmpMainKdDbGetManyFromMbsSysCmpRspMdl
            {
                ManagerCompanyMainKindList = itemList.Select(x => new CoMgrCmpMainKdDbGetManyFrommbsSysCmpRspItemMdl
                {
                    ManagerCompanyMainKindID = x.ID,
                    ManagerCompanyMainKindName = x.Name?.Trim(),
                    ManagerCompanyMainKindIsEnable = x.IsEnable
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

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrCmpMainKdDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpMainKdDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompanyMainKind
                .AsNoTracking()
                .Where(x =>
                    // 管理者公司主分類-名稱(模糊查詢)
                    string.IsNullOrEmpty(reqObj.ManagerCompanyMainKindName) || x.Name.Contains(reqObj.ManagerCompanyMainKindName.Trim())
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

            var rspObj = new CoMgrCmpMainKdDbGetManyFromMbsBscRspMdl
            {
                ManagerCompanyMainKindList = itemList
                    .Select(x => new CoMgrCmpMainKdDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerCompanyMainKindID = x.ID,
                        ManagerCompanyMainKindName = x.Name?.Trim(),
                        ManagerCompanyMainKindIsEnable = x.IsEnable
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