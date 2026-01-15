using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Department.Format;

namespace ServiceLibrary.Core.Manager.DB.Department.Service;

/// <summary>核心-管理者-部門-資料庫服務</summary>
public class CoMgrDepartmentDbService : ICoMgrDepartmentDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrDepartmentDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrDepartmentDbService(
        ILogger<CoMgrDepartmentDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-部門-資料庫-是否存在</summary>
    public async Task<CoMgrDptDbExistRspMdl> ExistAsync(CoMgrDptDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerDepartmentID.HasValue == false || x.ID == reqObj.ManagerDepartmentID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerDepartmentName) || x.Name == reqObj.ManagerDepartmentName));

            var rspObj = new CoMgrDptDbExistRspMdl
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

    /// <summary>核心-管理者-部門-資料庫-新增</summary>
    public async Task<CoMgrDptDbAddRspMdl> AddAsync(CoMgrDptDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerDepartment()
            {
                Name = reqObj.ManagerDepartmentName.Trim(),
                IsEnable = reqObj.ManagerDepartmentIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };
            this._mainContext.ManagerDepartment.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrDptDbAddRspMdl()
            {
                ManagerDepartmentID = item.ID
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

    /// <summary>核心-管理者-部門-資料庫-新增[ID]</summary>
    public async Task<CoMgrDptDbAddWithIdRspMdl> AddWithIdAsync(CoMgrDptDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            // SQL參數
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.ManagerDepartmentId),
                new SqlParameter("@Name", reqObj.ManagerDepartmentName.Trim()),
                new SqlParameter("@IsEnable", reqObj.ManagerDepartmentIsEnable),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.ManagerDepartment ON;
                INSERT INTO dbo.ManagerDepartment (ID, Name, IsEnable, CreateTime, UpdateTime)
                VALUES (@ID, @Name, @IsEnable, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.ManagerDepartment OFF;
            ";
            var affectedCount = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameters);

            var rspObj = new CoMgrDptDbAddWithIdRspMdl()
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

    /// <summary>核心-管理者-部門-資料庫-更新</summary>
    public async Task<CoMgrDptDbUpdateRspMdl> UpdateAsync(CoMgrDptDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerDepartment
                .Where(x => x.ID == reqObj.ManagerDepartmentID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerDepartmentName) ? x.Name : reqObj.ManagerDepartmentName.Trim())
                        .SetProperty(x => x.IsEnable, x => reqObj.ManagerDepartmentIsEnable ?? x.IsEnable)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrDptDbUpdateRspMdl()
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

    /// <summary>核心-管理者-部門-資料庫-移除</summary>
    public async Task<CoMgrDptDbRemoveRspMdl> RemoveAsync(CoMgrDptDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerDepartment
                .Where(x => x.ID == reqObj.ManagerDepartmentID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrDptDbRemoveRspMdl()
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
    }

    /// <summary>核心-管理者-部門-資料庫-取得</summary>
    public async Task<CoMgrDptDbGetRspMdl> GetAsync(CoMgrDptDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerDepartmentID);
            if (item == default)
            {
                return default;
            }

            var rspObj = new CoMgrDptDbGetRspMdl()
            {
                ManagerDepartmentName = item.Name.Trim(),
                ManagerDepartmentIsEnable = item.IsEnable
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

    /// <summary>核心-管理者-部門-資料庫-取得[名稱]</summary>
    public async Task<CoMgrDptDbGetNameRspMdl> GetNameAsync(CoMgrDptDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .Where(x => x.ID == reqObj.ManagerDepartmentID)
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

            var rspObj = new CoMgrDptDbGetNameRspMdl()
            {
                ManagerDepartmentName = item.Name.Trim(),
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

    /// <summary>核心-管理者-部門-資料庫-取得多筆</summary>
    public async Task<CoMgrDptDbGetManyRspMdl> GetManyAsync(CoMgrDptDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .Where(x => reqObj.ManagerDepartmentIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrDptDbGetManyRspMdl()
            {
                ManagerDepartmentList = itemList
                    .Select(x => new CoMgrDptDbGetManyRspItemMdl()
                    {
                        ManagerDepartmentID = x.ID,
                        ManagerDepartmentName = x.Name.Trim(),
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

    #region ManagerBackSite.Basic.Department 管理者後台-基本-部門

    /// <summary>核心-管理者-部門-資料庫-取得多筆從[管理者後台-基本-部門]</summary>
    public async Task<CoMgrDptDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscDptAsync(CoMgrDptDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .Where(x =>
                    // 部門-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerDepartmentName) || x.Name.Contains(reqObj.ManagerDepartmentName))
                    // 部門-是否啟用
                    && (reqObj.ManagerDepartmentIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerDepartmentIsEnable.Value))
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable
                })
                .ToListAsync();

            var rspObj = new CoMgrDptDbGetManyFromMbsBscRspMdl()
            {
                ManagerDepartmentList = itemList
                    .Select(x => new CoMgrDptDbGetManyFromMbsBscRspItemMdl()
                    {
                        ManagerDepartmentID = x.ID,
                        ManagerDepartmentName = x.Name.Trim(),
                        ManagerDepartmentIsEnable = x.IsEnable,
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

    /// <summary>核心-管理者-部門-資料庫-取得全部從[管理者後台-基本-部門]</summary>
    public async Task<CoMgrDptDbGetAllFromMbsBscRspMdl> GetAllFromMbsBscDptAsync(CoMgrDptDbGetAllFromMbsBscReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .OrderBy(x => x.ID)
                .Select(x => new CoMgrDptDbGetAllFromMbsBscRspItemMdl()
                {
                    ManagerDepartmentID = x.ID,
                    ManagerDepartmentName = x.Name.Trim(),
                })
                .ToListAsync();

            var rspObj = new CoMgrDptDbGetAllFromMbsBscRspMdl()
            {
                ManagerDepartmentList = itemList
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

    #region ManagerBackSite.System.Department 管理者後台-系統設定-部門

    /// <summary>核心-管理者-部門-資料庫-取得[筆數]從[管理者後台-系統設定-部門]</summary>
    public async Task<CoMgrDptDbGetCountFromMbsSysDptRspMdl> GetCountFromMbsSysDptAsync(CoMgrDptDbGetCountFromMbsSysDptReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .Where(x =>
                    // 部門-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerDepartmentName) || x.Name.Contains(reqObj.ManagerDepartmentName))
                    // 部門-是否啟用
                    && (reqObj.ManagerDepartmentIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerDepartmentIsEnable.Value))
                .CountAsync();

            var rspObj = new CoMgrDptDbGetCountFromMbsSysDptRspMdl
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

    /// <summary>核心-管理者-部門-資料庫-取得多筆從[管理者後台-系統設定-部門]</summary>
    public async Task<CoMgrDptDbGetManyFromMbsSysDptRspMdl> GetManyFromMbsSysDptAsync(CoMgrDptDbGetManyFromMbsSysDptReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerDepartment
                .AsNoTracking()
                .Where(x =>
                    // 部門-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerDepartmentName) || x.Name.Contains(reqObj.ManagerDepartmentName))
                    // 部門-是否啟用
                    && (reqObj.ManagerDepartmentIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerDepartmentIsEnable.Value))
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

            var rspObj = new CoMgrDptDbGetManyFromMbsSysDptRspMdl()
            {
                ManagerDepartmentList = itemList
                    .Select(x => new CoMgrDptDbGetManyFromMbsSysDptRspItemMdl()
                    {
                        ManagerDepartmentID = x.ID,
                        ManagerDepartmentName = x.Name.Trim(),
                        ManagerDepartmentIsEnable = x.IsEnable,
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