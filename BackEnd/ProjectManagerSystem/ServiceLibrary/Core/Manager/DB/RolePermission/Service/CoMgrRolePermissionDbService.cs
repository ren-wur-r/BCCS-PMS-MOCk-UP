using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.ManagerRegion;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.RolePermission.Format;

namespace ServiceLibrary.Core.Manager.DB.RolePermission.Service;

/// <summary>核心-管理者-角色權限-資料庫服務</summary>
public class CoMgrRolePermissionDbService : ICoMgrRolePermissionDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrRolePermissionDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrRolePermissionDbService(
        ILogger<CoMgrRolePermissionDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-角色權限-資料庫-新增多筆</summary>
    public async Task<CoMgrRpDbAddManyRspMdl> AddManyAsync(CoMgrRpDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var itemList = reqObj.ManagerRolePermissionList
            .Select(x => new ManagerRolePermission()
            {
                ManagerRoleID = x.ManagerRoleID,
                AtomMenuID = x.AtomMenu.ToInt16(),
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView.ToInt16(),
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate.ToInt16(),
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit.ToInt16(),
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete.ToInt16(),
                CreateTime = currentTime,
            })
            .ToList();

        try
        {
            this._mainContext.ManagerRolePermission.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrRpDbAddManyRspMdl()
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

    /// <summary>核心-管理者-角色權限-資料庫-移除多筆</summary>
    public async Task<CoMgrRpDbRemoveManyRspMdl> RemoveManyAsync(CoMgrRpDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerRolePermission
                .Where(x => x.ManagerRoleID == reqObj.ManagerRoleID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrRpDbRemoveManyRspMdl
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

    /// <summary>核心-管理者-角色權限-資料庫-取得</summary>
    public async Task<CoMgrRpDbGetRspMdl> GetAsync(CoMgrRpDbGetReqMdl reqObj)
    {
        try
        {
            var item = await
                (
                    from ammr in
                        from am in this._mainContext.AtomMenu
                            .Where(amx => amx.ID == reqObj.AtomMenu.ToInt16())
                        from mr in this._mainContext.ManagerRole
                            .Where(mrx => mrx.ID == reqObj.ManagerRoleID)
                        select new
                        {
                            ManagerRoleID = mr.ID,
                            AtomMenuID = am.ID,
                            ManagerRegionID = mr.ManagerRegionID,
                        }
                    from mrp in this._mainContext.ManagerRolePermission
                        .Where(mrpx =>
                            mrpx.ManagerRoleID == ammr.ManagerRoleID
                            && mrpx.AtomMenuID == ammr.AtomMenuID)
                        .DefaultIfEmpty()
                    select new
                    {
                        ManagerRegionID = ammr.ManagerRegionID,
                        AtomPermissionKindIdView = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdView,
                        AtomPermissionKindIdCreate = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdCreate,
                        AtomPermissionKindIdEdit = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdEdit,
                        AtomPermissionKindIdDelete = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdDelete,
                    }
                ).AsNoTracking()
                .SingleOrDefaultAsync();

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrRpDbGetRspMdl()
            {
                ManagerRegionID = item.ManagerRegionID,
                AtomPermissionKindIdView = item.AtomPermissionKindIdView.ToEnum<DbAtomPermissionKindEnum>(),
                AtomPermissionKindIdCreate = item.AtomPermissionKindIdCreate.ToEnum<DbAtomPermissionKindEnum>(),
                AtomPermissionKindIdEdit = item.AtomPermissionKindIdEdit.ToEnum<DbAtomPermissionKindEnum>(),
                AtomPermissionKindIdDelete = item.AtomPermissionKindIdDelete.ToEnum<DbAtomPermissionKindEnum>(),
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

    /// <summary>核心-管理者-角色權限-資料庫-取得多筆</summary>
    public async Task<CoMgrRpDbGetManyRspMdl> GetManyAsync(CoMgrRpDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from am in this._mainContext.AtomMenu
                    from mrp in this._mainContext.ManagerRolePermission
                        .Where(mrpx =>
                            mrpx.ManagerRoleID == reqObj.ManagerRoleID
                            && mrpx.AtomMenuID == am.ID)
                        .DefaultIfEmpty()
                    select new
                    {
                        AtomMenuID = am.ID,
                        ManagerRegionID = mrp == default ? DbManagerRegionConst.Denied.ID : mrp.ManagerRegionID, // 使用權限的地區 ID
                        AtomPermissionKindIdView = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdView,
                        AtomPermissionKindIdCreate = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdCreate,
                        AtomPermissionKindIdEdit = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdEdit,
                        AtomPermissionKindIdDelete = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdDelete,
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoMgrRpDbGetManyRspMdl()
            {
                ManagerRolePermissionList = itemList
                    .Select(x => new CoMgrRpDbGetManyRspItemMdl()
                    {
                        AtomMenu = x.AtomMenuID.ToEnum<DbAtomMenuEnum>(),
                        ManagerRegionID = x.ManagerRegionID,
                        AtomPermissionKindIdView = x.AtomPermissionKindIdView.ToEnum<DbAtomPermissionKindEnum>(),
                        AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate.ToEnum<DbAtomPermissionKindEnum>(),
                        AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit.ToEnum<DbAtomPermissionKindEnum>(),
                        AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete.ToEnum<DbAtomPermissionKindEnum>(),
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

    #region ManagerBackSite.Basic.RolePermission 管理者後台-基本-角色權限

    /// <summary>核心-管理者-角色權限-資料庫-取得多筆從[管理者後台-基本-角色權限]</summary>
    public async Task<CoMgrRpDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscRpAsync(CoMgrRpDbGetManyFromMbsBscReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from am in this._mainContext.AtomMenu
                    from mrp in this._mainContext.ManagerRolePermission
                        .Where(mrpx =>
                            mrpx.ManagerRoleID == reqObj.ManagerRoleID
                            && mrpx.AtomMenuID == am.ID)
                        .DefaultIfEmpty()
                    select new
                    {
                        AtomMenuID = am.ID,
                        ManagerRegionID = mrp == default ? DbManagerRegionConst.Denied.ID : mrp.ManagerRegionID,
                        AtomPermissionKindIdView = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdView,
                        AtomPermissionKindIdCreate = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdCreate,
                        AtomPermissionKindIdEdit = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdEdit,
                        AtomPermissionKindIdDelete = mrp == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : mrp.AtomPermissionKindIdDelete,
                    }
                ).AsNoTracking()
                .OrderBy(x => x.AtomMenuID)
                .ToListAsync();

            var rspObj = new CoMgrRpDbGetManyFromMbsBscRspMdl
            {
                ManagerRolePermissionList = itemList
                    .Select(x => new CoMgrRpDbGetManyFromMbsBscRspItemMdl
                    {
                        AtomMenu = (DbAtomMenuEnum)x.AtomMenuID,
                        ManagerRegionID = x.ManagerRegionID,
                        AtomPermissionKindIdView = x.AtomPermissionKindIdView.ToEnum<DbAtomPermissionKindEnum>(),
                        AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate.ToEnum<DbAtomPermissionKindEnum>(),
                        AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit.ToEnum<DbAtomPermissionKindEnum>(),
                        AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete.ToEnum<DbAtomPermissionKindEnum>(),
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