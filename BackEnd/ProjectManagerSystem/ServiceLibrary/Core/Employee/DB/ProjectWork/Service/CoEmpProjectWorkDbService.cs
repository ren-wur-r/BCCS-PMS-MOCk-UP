using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Service;

/// <summary>核心-員工-專案工作計劃書-資料庫服務</summary>
public class CoEmpProjectWorkDbService : ICoEmpProjectWorkDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectWorkDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectWorkDbService(
        ILogger<CoEmpProjectWorkDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案工作計劃書-新增</summary>
    public async Task<CoEmpPwDbAddRspMdl> AddAsync(CoEmpPwDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.EmployeeProjectWork()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                Url = reqObj.EmployeeProjectWorkUrl?.Trim(),
                IsNewest = reqObj.EmployeeProjectWorkIsNewest,
                CreateTime = currentTime,
            };
            this._mainContext.EmployeeProjectWork.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPwDbAddRspMdl()
            {
                EmployeeProjectWorkID = item.ID,
                EmployeeProjectWorkCreateTime = currentTime,
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

    /// <summary>核心-員工-專案工作計劃書-資料庫-移除多筆</summary>
    public async Task<CoEmpPwDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPwDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectWork
                .Where(x => x.EmployeeProjectID == reqObj.EmployeeProjectID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPwDbRemoveManyRspMdl
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

    /// <summary>核心-員工-專案工作計劃書-更新多筆</summary>
    public async Task<CoEmpPwDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPwDbUpdateManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectWork
                .Where(x => reqObj.EmployeeProjectWorkIdList.Contains(x.ID))
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.IsNewest, x => reqObj.EmployeeProjectWorkIsNewest.HasValue == false ? x.IsNewest : reqObj.EmployeeProjectWorkIsNewest.Value)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPwDbUpdateManyRspMdl()
            {
                AffectedCount = affectedCount,
                EmployeeProjectWorkUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案工作計劃書-取得多筆ID</summary>
    public async Task<CoEmpPwDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPwDbGetManyIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectWork
                .AsNoTracking()
                .Where(x => x.EmployeeProjectID == reqObj.EmployeeProjectID)
                .Select(x => new
                {
                    x.ID,
                })
                .ToListAsync();

            var rspObj = new CoEmpPwDbGetManyIdRspMdl()
            {
                EmployeeProjectWorkList = itemList
                    .Select(x => new CoEmpPwDbGetManyIdRspItemMdl()
                    {
                        EmployeeProjectWorkID = x.ID,
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

    /// <summary>核心-員工-專案工作計劃書-取得多筆</summary>
    public async Task<CoEmpPwDbGetManyRspMdl> GetManyAsync(CoEmpPwDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectWork
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

            var rspObj = new CoEmpPwDbGetManyRspMdl()
            {
                EmployeeProjectWorkList = itemList
                    .Select(x => new CoEmpPwDbGetManyRspItemMdl()
                    {
                        EmployeeProjectWorkUrl = x.Url?.Trim(),
                        EmployeeProjectWorkIsNewest = x.IsNewest,
                        EmployeeProjectWorkCreateTime = x.CreateTime,
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
