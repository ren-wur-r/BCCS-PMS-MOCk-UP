using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Service;

/// <summary>核心-員工-專案契約-資料庫服務</summary>
public class CoEmpProjectContractDbService : ICoEmpProjectContractDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectContractDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectContractDbService(
        ILogger<CoEmpProjectContractDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案契約-新增</summary>
    public async Task<CoEmpPcDbAddRspMdl> AddAsync(CoEmpPcDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.EmployeeProjectContract()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                Url = reqObj.EmployeeProjectContractUrl?.Trim(),
                IsNewest = reqObj.EmployeeProjectContractIsNewest,
                CreateTime = currentTime,
            };
            this._mainContext.EmployeeProjectContract.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPcDbAddRspMdl()
            {
                EmployeeProjectContractID = item.ID,
                EmployeeProjectContractCreateTime = currentTime,
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

    /// <summary>核心-員工-專案契約-資料庫-移除多筆</summary>
    public async Task<CoEmpPcDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPcDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectContract
                .Where(x => x.EmployeeProjectID == reqObj.EmployeeProjectID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPcDbRemoveManyRspMdl
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

    /// <summary>核心-員工-專案契約-更新多筆</summary>
    public async Task<CoEmpPcDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPcDbUpdateManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectContract
                .Where(x => reqObj.EmployeeProjectContractIdList.Contains(x.ID))
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.IsNewest, x => reqObj.EmployeeProjectContractIsNewest.HasValue == false ? x.IsNewest : reqObj.EmployeeProjectContractIsNewest.Value)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPcDbUpdateManyRspMdl()
            {
                AffectedCount = affectedCount,
                EmployeeProjectContractUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案契約-取得多筆ID</summary>
    public async Task<CoEmpPcDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPcDbGetManyIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectContract
                .AsNoTracking()
                .Where(x => x.EmployeeProjectID == reqObj.EmployeeProjectID)
                .Select(x => new
                {
                    x.ID,
                })
                .ToListAsync();

            var rspObj = new CoEmpPcDbGetManyIdRspMdl()
            {
                EmployeeProjectContractList = itemList
                    .Select(x => new CoEmpPcDbGetManyIdRspItemMdl()
                    {
                        EmployeeProjectContractID = x.ID,
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

    /// <summary>核心-員工-專案契約-取得多筆</summary>
    public async Task<CoEmpPcDbGetManyRspMdl> GetManyAsync(CoEmpPcDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectContract
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeProjectID == reqObj.EmployeeProjectID
                    && (reqObj.EmployeeProjectContractIsNewest.HasValue == false || x.IsNewest == reqObj.EmployeeProjectContractIsNewest.Value)
                )
                .Select(x => new
                {
                    x.Url,
                    x.IsNewest,
                    x.CreateTime,
                })
                .ToListAsync();

            var rspObj = new CoEmpPcDbGetManyRspMdl()
            {
                EmployeeProjectContractList = itemList
                    .Select(x => new CoEmpPcDbGetManyRspItemMdl()
                    {
                        EmployeeProjectContractUrl = x.Url?.Trim(),
                        EmployeeProjectContractIsNewest = x.IsNewest,
                        EmployeeProjectContractCreateTime = x.CreateTime,
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
