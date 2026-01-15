using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.EmployeeProjectMember;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Service;

/// <summary>核心-員工-專案成員-資料庫服務</summary>
public class CoEmpProjectMemberDbService : ICoEmpProjectMemberDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectMemberDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectMemberDbService(
        ILogger<CoEmpProjectMemberDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案成員-是否存在</summary>
    public async Task<CoEmpPmDbExistRspMdl> ExistAsync(CoEmpPmDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.EmployeeProjectMember
                .AsNoTracking()
                .AnyAsync(x =>
                    x.EmployeeProjectID == reqObj.EmployeeProjectID
                    && x.EmployeeID == reqObj.EmployeeID
                    && x.RoleID == reqObj.EmployeeProjectMemberRole.ToInt16());

            var rspObj = new CoEmpPmDbExistRspMdl
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

    /// <summary>核心-員工-專案成員-新增</summary>
    public async Task<CoEmpPmDbAddRspMdl> AddAsync(CoEmpPmDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProjectMember()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeID = reqObj.EmployeeID,
                RoleID = reqObj.EmployeeProjectMemberRole.ToInt16(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };
            this._mainContext.EmployeeProjectMember.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPmDbAddRspMdl()
            {
                EmployeeProjectMemberCreateTime = currentTime,
                EmployeeProjectMemberUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案成員-新增多筆</summary>
    public async Task<CoEmpPmemDbAddManyRspMdl> AddManyAsync(CoEmpPmemDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeeProjectMemberList
                .Select(x => new EmployeeProjectMember
                {
                    EmployeeProjectID = x.EmployeeProjectID,
                    EmployeeID = x.EmployeeID,
                    RoleID = x.EmployeeProjectMemberRole.ToInt16(),
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeeProjectMember.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPmemDbAddManyRspMdl
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

    /// <summary>核心-員工-專案成員-移除</summary>
    public async Task<CoEmpPmDbRemoveRspMdl> RemoveAsync(CoEmpPmDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectMember
                .Where(x => x.ID == reqObj.EmployeeProjectMemberID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPmDbRemoveRspMdl
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

    /// <summary>核心-員工-專案成員-取得</summary>
    public async Task<CoEmpPmDbGetRspMdl> GetAsync(CoEmpPmDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectMember
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectMemberID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPmDbGetRspMdl()
            {
                EmployeeProjectID = item.EmployeeProjectID,
                EmployeeID = item.EmployeeID,
                EmployeeProjectMemberRole = item.RoleID.ToEnum<DbEmployeeProjectMemberRoleEnum>(),
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

    /// <summary>核心-員工-專案成員-取得多筆</summary>
    public async Task<CoEmpPmDbGetManyRspMdl> GetManyAsync(CoEmpPmDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectMember
                .AsNoTracking()
                .Where(x =>
                    (reqObj.EmployeeID.HasValue == false || x.EmployeeID == reqObj.EmployeeID.Value)
                    && (reqObj.EmployeeProjectID.HasValue == false || x.EmployeeProjectID == reqObj.EmployeeProjectID)
                )
                .Select(x => new
                {
                    x.ID,
                    x.EmployeeID,
                    x.EmployeeProjectID,
                    x.RoleID,
                })
                .ToListAsync();

            var rspObj = new CoEmpPmDbGetManyRspMdl()
            {
                EmployeeProjectMemberList = itemList
                    .Select(x => new CoEmpPmDbGetManyRspItemMdl()
                    {
                        EmployeeProjectMemberID = x.ID,
                        EmployeeID = x.EmployeeID,
                        EmployeeProjectID = x.EmployeeProjectID,
                        EmployeeProjectMemberRole = x.RoleID.ToEnum<DbEmployeeProjectMemberRoleEnum>()
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

    /// <summary>核心-員工-專案成員-取得多筆員工ID</summary>
    public async Task<CoEmpPmDbGetManyEmployeeIdRspMdl> GetManyEmployeeIdAsync(CoEmpPmDbGetManyEmployeeIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectMember
                .AsNoTracking()
                .Where(x =>
                    (reqObj.EmployeeProjectIdList == null || reqObj.EmployeeProjectIdList.Contains(x.EmployeeProjectID))
                )
                .Select(x => new
                {
                    x.EmployeeID,
                })
                .ToListAsync();

            var rspObj = new CoEmpPmDbGetManyEmployeeIdRspMdl()
            {
                EmployeeProjectMemberList = itemList
                    .Select(x => new CoEmpPmDbGetManyEmployeeIdRspItemMdl()
                    {
                        EmployeeID = x.EmployeeID,
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

