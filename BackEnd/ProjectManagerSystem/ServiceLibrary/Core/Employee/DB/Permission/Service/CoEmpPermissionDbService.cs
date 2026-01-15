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
using ServiceLibrary.Core.Employee.DB.Permission.Format;

namespace ServiceLibrary.Core.Employee.DB.Permission.Service;

/// <summary>核心-員工-目錄權限-資料庫服務</summary>
public class CoEmpPermissionDbService : ICoEmpPermissionDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPermissionDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPermissionDbService(
        ILogger<CoEmpPermissionDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-目錄權限-資料庫-新增多筆</summary>
    public async Task<CoEmpPmnDbAddManyRspMdl> AddManyAsync(CoEmpPmnDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var itemList = reqObj.EmployeePermissionList
            .Select(x => new EmployeePermission()
            {
                EmployeeID = x.EmployeeID,
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
            this._mainContext.EmployeePermission.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPmnDbAddManyRspMdl()
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

    /// <summary>核心-員工-目錄權限-資料庫-移除多筆</summary>
    public async Task<CoEmpPmnDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPmnDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePermission
                .Where(x => x.EmployeeID == reqObj.EmployeeID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPmnDbRemoveManyRspMdl
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

    /// <summary>核心-員工-目錄權限-資料庫-取得</summary>
    public async Task<CoEmpPmnDbGetRspMdl> GetAsync(CoEmpPmnDbGetReqMdl reqObj)
    {
        try
        {
            var item = await
                (
                    from amem in
                        from am in this._mainContext.AtomMenu
                            .Where(amx => amx.ID == reqObj.AtomMenu.ToInt16())
                        from em in this._mainContext.Employee
                            .Where(emx => emx.ID == reqObj.EmployeeID)
                        select new
                        {
                            AtomMenuID = am.ID,
                            EmployeeID = em.ID,
                        }
                    from ep in this._mainContext.EmployeePermission
                        .Where(epx =>
                            epx.AtomMenuID == amem.AtomMenuID
                            && epx.EmployeeID == amem.EmployeeID)
                        .DefaultIfEmpty()
                    select new
                    {
                        ManagerRegionID = ep == default ? DbManagerRegionConst.Denied.ID : ep.ManagerRegionID,
                        AtomPermissionKindIdView = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdView,
                        AtomPermissionKindIdCreate = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdCreate,
                        AtomPermissionKindIdEdit = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdEdit,
                        AtomPermissionKindIdDelete = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdDelete,
                    }
                ).AsNoTracking()
                .SingleOrDefaultAsync();
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPmnDbGetRspMdl()
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

    /// <summary>核心-管理者-目錄權限-資料庫-取得多筆</summary>
    public async Task<CoEmpPmnDbGetManyRspMdl> GetManyAsync(CoEmpPmnDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from amem in
                        from am in this._mainContext.AtomMenu
                        from em in this._mainContext.Employee
                            .Where(emx => emx.ID == reqObj.EmployeeID)
                        select new
                        {
                            AtomMenuID = am.ID,
                            EmployeeID = em.ID,
                        }
                    from ep in this._mainContext.EmployeePermission
                        .Where(epx =>
                            epx.AtomMenuID == amem.AtomMenuID
                            && epx.EmployeeID == amem.EmployeeID)
                        .DefaultIfEmpty()
                    select new
                    {
                        amem.AtomMenuID,
                        ManagerRegionID = ep == default ? DbManagerRegionConst.Denied.ID : ep.ManagerRegionID,
                        AtomPermissionKindIdView = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdView,
                        AtomPermissionKindIdCreate = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdCreate,
                        AtomPermissionKindIdEdit = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdEdit,
                        AtomPermissionKindIdDelete = ep == default ? DbAtomPermissionKindEnum.Denied.ToInt16() : ep.AtomPermissionKindIdDelete,
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoEmpPmnDbGetManyRspMdl()
            {
                EmployeePermissionList = itemList
                    .Select(x => new CoEmpPmnDbGetManyRspItemMdl()
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
}