using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Service;

/// <summary>核心-員工-專案里程碑工項工作檔案-資料庫服務</summary>
public class CoEmpProjectStoneJobWorkFileDbService : ICoEmpProjectStoneJobWorkFileDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectStoneJobWorkFileDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectStoneJobWorkFileDbService(
        ILogger<CoEmpProjectStoneJobWorkFileDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案里程碑工項工作檔案-新增</summary>
    public async Task<CoEmpPsjwfDbAddRspMdl> AddAsync(CoEmpPsjwfDbAddReqMdl reqObj)
    {
        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.EmployeeProjectStoneJobWorkFile()
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
                EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
                Url = reqObj.EmployeeProjectStoneJobWorkFileUrl?.Trim(),
                CreateTime = DateTimeOffset.UtcNow,
                UpdateTime = DateTimeOffset.UtcNow,
            };

            this._mainContext.EmployeeProjectStoneJobWorkFile.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjwfDbAddRspMdl()
            {
                EmployeeProjectStoneJobWorkFileID = item.ID,
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

    /// <summary>核心-員工-專案里程碑工項工作檔案-新增多筆</summary>
    public async Task<CoEmpPsjwfDbAddManyRspMdl> AddManyAsync(CoEmpPsjwfDbAddManyReqMdl reqObj)
    {
        try
        {
            var currentTime = DateTimeOffset.UtcNow;
            var items = reqObj.EmployeeProjectStoneJobWorkFileList
                .Select(x => new DataModelLibrary.EfCore.ProjectManagerMain.EmployeeProjectStoneJobWorkFile()
                {
                    EmployeeProjectID = x.EmployeeProjectID,
                    EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                    EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobWorkID = x.EmployeeProjectStoneJobWorkID,
                    Url = x.EmployeeProjectStoneJobWorkFileUrl?.Trim(),
                    CreateTime = currentTime,
                    UpdateTime = currentTime,
                })
                .ToList();

            this._mainContext.EmployeeProjectStoneJobWorkFile.AddRange(items);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPsjwfDbAddManyRspMdl()
            {
                EmployeeProjectStoneJobWorkFileList = items
                    .Select(i => new CoEmpPsjwfDbAddManyRspItemMdl { EmployeeProjectStoneJobWorkFileID = i.ID })
                    .ToList()
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

    /// <summary>核心-員工-專案里程碑工項工作檔案-移除多筆</summary>
    public async Task<CoEmpPsjwfDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjwfDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobWorkFile
                .Where(x => reqObj.EmployeeProjectStoneJobWorkFileIdList.Contains(x.ID))
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPsjwfDbRemoveManyRspMdl()
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

    /// <summary>核心-員工-專案里程碑工項工作檔案-更新</summary>
    public async Task<CoEmpPsjwfDbUpdateRspMdl> UpdateAsync(CoEmpPsjwfDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectStoneJobWorkFile
                .Where(x => x.ID == reqObj.EmployeeProjectStoneJobWorkFileID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Url, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectStoneJobWorkFileUrl) ? x.Url : reqObj.EmployeeProjectStoneJobWorkFileUrl.Trim())
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPsjwfDbUpdateRspMdl()
            {
                AffectedCount = affectedCount,
                EmployeeProjectStoneJobWorkFileUpdateTime = currentTime,
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

    /// <summary>核心-員工-專案里程碑工項工作檔案-取得多筆</summary>
    public async Task<CoEmpPsjwfDbGetManyRspMdl> GetManyAsync(CoEmpPsjwfDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectStoneJobWorkFile
                .AsNoTracking()
                .Where(x => reqObj.EmployeeProjectStoneJobWorkIDList.Contains(x.EmployeeProjectStoneJobWorkID))
                .Select(x => new
                {
                    EmployeeProjectStoneJobWorkFileID = x.ID,
                    EmployeeProjectStoneJobWorkFileUrl = x.Url,
                })
                .ToListAsync();

            var rspObj = new CoEmpPsjwfDbGetManyRspMdl()
            {
                EmployeeProjectStoneJobWorkFileList = itemList
                    .Select(x => new CoEmpPsjwfDbGetManyRspItemMdl
                    {
                        EmployeeProjectStoneJobWorkFileID = x.EmployeeProjectStoneJobWorkFileID,
                        EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl.Trim(),
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
