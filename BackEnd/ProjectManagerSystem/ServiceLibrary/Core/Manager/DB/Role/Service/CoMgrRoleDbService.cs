using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Role.Format;

namespace ServiceLibrary.Core.Manager.DB.Role.Service;

/// <summary>核心-管理者-角色-資料庫服務</summary>
public class CoMgrRoleDbService : ICoMgrRoleDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrRoleDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrRoleDbService(
        ILogger<CoMgrRoleDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-角色-資料庫-是否存在</summary>
    public async Task<CoMgrRolDbExistRspMdl> ExistAsync(CoMgrRolDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerRole
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerRoleID.HasValue == false || x.ID != reqObj.ManagerRoleID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerRoleName) || x.Name == reqObj.ManagerRoleName));

            var rspObj = new CoMgrRolDbExistRspMdl
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

    /// <summary>核心-管理者-角色-資料庫-新增</summary>
    public async Task<CoMgrRolDbAddRspMdl> AddAsync(CoMgrRolDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.ManagerRole()
            {
                Name = reqObj.ManagerRoleName.Trim(),
                ManagerRegionID = reqObj.ManagerRegionID,
                ManagerDepartmentID = reqObj.ManagerDepartmentID,
                Remark = reqObj.ManagerRoleRemark?.Trim(),
                IsEnable = reqObj.ManagerRoleIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerRole.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrRolDbAddRspMdl()
            {
                ManagerRoleID = item.ID,
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

    /// <summary>核心-管理者-角色-資料庫-新增[指定ID]</summary>
    public async Task<CoMgrRolDbAddWithIdRspMdl> AddWithIdAsync(CoMgrRolDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            //SQL參數
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.ManagerRoleId),
                new SqlParameter("@Name", reqObj.ManagerRoleName.Trim()),
                new SqlParameter("@ManagerRegionID", reqObj.ManagerRegionID),
                new SqlParameter("@ManagerDepartmentID", reqObj.ManagerDepartmentID),
                new SqlParameter("@Remark", reqObj.ManagerRoleRemark?.Trim()),
                new SqlParameter("@IsEnable", reqObj.ManagerRoleIsEnable),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.ManagerRole ON;
                INSERT INTO dbo.ManagerRole (ID, Name, ManagerRegionID, ManagerDepartmentID, Remark, IsEnable, CreateTime, UpdateTime)
                VALUES (@ID, @Name, @ManagerRegionID, @ManagerDepartmentID, @Remark, @IsEnable, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.ManagerRole OFF;
            ";
            var result = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameters);

            if (result == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrRolDbAddWithIdRspMdl()
            {
                ManagerRoleID = reqObj.ManagerRoleId,
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

    /// <summary>核心-管理者-角色-資料庫-更新</summary>
    public async Task<CoMgrRolDbUpdateRspMdl> UpdateAsync(CoMgrRolDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerRole
                .Where(x => x.ID == reqObj.ManagerRoleID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerRoleName) ? x.Name : reqObj.ManagerRoleName.Trim())
                        .SetProperty(x => x.ManagerRegionID, x => reqObj.ManagerRegionID ?? x.ManagerRegionID)
                        .SetProperty(x => x.ManagerDepartmentID, x => reqObj.ManagerDepartmentID ?? x.ManagerDepartmentID)
                        .SetProperty(x => x.Remark, x => reqObj.ManagerRoleRemark ?? x.Remark)
                        .SetProperty(x => x.IsEnable, x => reqObj.ManagerRoleIsEnable ?? x.IsEnable)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrRolDbUpdateRspMdl
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

    /// <summary>核心-管理者-角色-資料庫-移除</summary>
    public async Task<CoMgrRolDbRemoveRspMdl> RemoveAsync(CoMgrRolDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerRole
                .Where(x => x.ID == reqObj.ManagerRoleID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrRolDbRemoveRspMdl
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

    /// <summary>核心-管理者-角色-資料庫-取得</summary>
    public async Task<CoMgrRolDbGetRspMdl> GetAsync(CoMgrRolDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerRole
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerRoleID);
            if (item == default)
            {
                return default;
            }

            var rspObj = new CoMgrRolDbGetRspMdl
            {
                ManagerRegionID = item.ManagerRegionID,
                ManagerDepartmentID = item.ManagerDepartmentID,
                ManagerRoleName = item.Name.Trim(),
                ManagerRoleRemark = item.Remark?.Trim(),
                ManagerRoleIsEnable = item.IsEnable,
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

    /// <summary>核心-管理者-角色-資料庫-取得[名稱]</summary>
    public async Task<CoMgrRolDbGetNameRspMdl> GetNameAsync(CoMgrRolDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerRole
                .AsNoTracking()
                .Where(x => x.ID == reqObj.ManagerRoleID)
                .Select(x => new
                {
                    x.Name,
                    x.ManagerRegionID,
                    x.ManagerDepartmentID,
                })
                .SingleOrDefaultAsync();

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrRolDbGetNameRspMdl
            {
                ManagerRoleName = item.Name?.Trim(),
                ManagerRegionID = item.ManagerRegionID,
                ManagerDepartmentID = item.ManagerDepartmentID,
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

    /// <summary>核心-管理者-角色-資料庫-取得多筆</summary>
    public async Task<CoMgrRolDbGetManyRspMdl> GetManyAsync(CoMgrRolDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerRole
                .AsNoTracking()
                .Where(x => reqObj.ManagerRoleIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.ManagerRegionID,
                    x.ManagerDepartmentID,
                    x.Name,
                })
                .ToListAsync();

            var rspObj = new CoMgrRolDbGetManyRspMdl
            {
                ManagerRoleList = itemList
                    .Select(x => new CoMgrRolDbGetManyRspItemMdl
                    {
                        ManagerRoleID = x.ID,
                        ManagerRegionID = x.ManagerRegionID,
                        ManagerDepartmentID = x.ManagerDepartmentID,
                        ManagerRoleName = x.Name.Trim(),
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

    #region ManagerBackSite.Basic.Role 管理者後台-基本-角色
    public async Task<CoMgrRolDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscRolAsync(CoMgrRolDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from rol in this._mainContext.ManagerRole
                        .Where(rox =>
                            // 角色-名稱
                            (string.IsNullOrWhiteSpace(reqObj.ManagerRoleName) || rox.Name.Contains(reqObj.ManagerRoleName))
                            // 角色-是否啟用
                            && (reqObj.ManagerRoleIsEnable.HasValue == false || rox.IsEnable == reqObj.ManagerRoleIsEnable.Value))
                    from rgn in this._mainContext.ManagerRegion
                        .Where(rgx => rgx.ID == rol.ManagerRegionID)
                    from dpt in this._mainContext.ManagerDepartment
                        .Where(dpx => dpx.ID == rol.ManagerDepartmentID)
                    select new
                    {
                        // 角色
                        ManagerRoleID = rol.ID,
                        ManagerRoleName = rol.Name,
                        ManagerRoleIsEnable = rol.IsEnable,
                        // 地區
                        ManagerRegionID = rgn.ID,
                        ManagerRegionName = rgn.Name,
                        // 部門
                        ManagerDepartmentID = dpt.ID,
                        ManagerDepartmentName = dpt.Name,
                    }
                ).AsNoTracking()
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrRolDbGetManyFromMbsBscRspMdl()
            {
                ManagerRoleList = itemList
                    .Select(x => new CoMgrRolDbGetManyFromMbsBscRspItemMdl()
                    {
                        ManagerRoleID = x.ManagerRoleID,
                        ManagerRoleName = x.ManagerRoleName?.Trim(),
                        ManagerRegionID = x.ManagerRegionID,
                        ManagerRegionName = x.ManagerRegionName?.Trim(),
                        ManagerDepartmentID = x.ManagerDepartmentID,
                        ManagerDepartmentName = x.ManagerDepartmentName?.Trim(),
                        ManagerRoleIsEnable = x.ManagerRoleIsEnable,
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

    #region ManagerBackSite.System.Role 管理者後台-系統設定-角色

    /// <summary>核心-管理者-角色-資料庫-取得[筆數]從[管理者後台-系統設定-角色]</summary>
    public async Task<CoMgrRolDbGetCountFromMbsSysRolRspMdl> GetCountFromMbsSysRolAsync(CoMgrRolDbGetCountFromMbsSysRolReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerRole
                .AsNoTracking()
                .Where(x =>
                    // 角色-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerRoleName) || x.Name.Contains(reqObj.ManagerRoleName))
                    // 角色-是否啟用
                    && (reqObj.ManagerRoleIsEnable == null || x.IsEnable == reqObj.ManagerRoleIsEnable))
                .CountAsync();

            var rspObj = new CoMgrRolDbGetCountFromMbsSysRolRspMdl
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

    /// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-系統設定-角色]</summary>
    public async Task<CoMgrRolDbGetManyFromMbsSysRolRspMdl> GetManyFromMbsSysRolAsync(CoMgrRolDbGetManyFromMbsSysRolReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerRole
                .AsNoTracking()
                .Where(x =>
                    // 角色-名稱
                    (string.IsNullOrWhiteSpace(reqObj.ManagerRoleName) || x.Name.Contains(reqObj.ManagerRoleName))
                    // 角色-是否啟用
                    && (reqObj.ManagerRoleIsEnable.HasValue == false || x.IsEnable == reqObj.ManagerRoleIsEnable.Value))
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.IsEnable,
                })
                .ToListAsync();

            var rspObj = new CoMgrRolDbGetManyFromMbsSysRolRspMdl
            {
                ManagerRoleList = itemList
                    .Select(x => new CoMgrRolDbGetManyFromMbsSysRolRspItemMdl
                    {
                        ManagerRoleID = x.ID,
                        ManagerRoleName = x.Name.Trim(),
                        ManagerRoleIsEnable = x.IsEnable,
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