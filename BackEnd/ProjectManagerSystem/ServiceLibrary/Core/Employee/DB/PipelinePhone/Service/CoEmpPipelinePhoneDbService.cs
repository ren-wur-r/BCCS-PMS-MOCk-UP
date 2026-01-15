using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelinePhone.Service;

/// <summary>核心-員工-商機電銷-資料庫服務</summary>
public class CoEmpPipelinePhoneDbService : ICoEmpPipelinePhoneDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelinePhoneDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelinePhoneDbService(
        ILogger<CoEmpPipelinePhoneDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機電銷-資料庫-取得</summary>
    public async Task<CoEmpPplPhnDbGetRspMdl> GetAsync(CoEmpPplPhnDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelinePhone
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelinePhoneID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplPhnDbGetRspMdl
            {
                EmployeePipelinePhoneID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                EmployeePipelinePhoneRecordTime = item.RecordTime,
                ManagerContacterID = item.ManagerContacterID,
                EmployeePipelinePhoneTitle = item.Title?.Trim(),
                EmployeePipelinePhoneRemark = item.Remark?.Trim(),
                EmployeePipelinePhoneCreateEmployeeID = item.CreateEmployeeID
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

    /// <summary>核心-員工-商機電銷-資料庫-取得多筆</summary>
    public async Task<CoEmpPplPhnDbGetManyRspMdl> GetManyAsync(CoEmpPplPhnDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelinePhone
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    EmployeePipelinePhoneID = x.ID,
                    x.EmployeePipelineID,
                    x.RecordTime,
                    x.ManagerContacterID,
                    x.Title,
                    x.Remark,
                    x.CreateEmployeeID
                })
                .OrderByDescending(x => x.EmployeePipelinePhoneID)
                .ToListAsync();

            var rspObj = new CoEmpPplPhnDbGetManyRspMdl
            {
                EmployeePipelinePhoneList = itemList.Select(x => new CoEmpPplPhnDbGetManyRspItemMdl
                {
                    EmployeePipelinePhoneID = x.EmployeePipelinePhoneID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    EmployeePipelinePhoneRecordTime = x.RecordTime,
                    ManagerContacterID = x.ManagerContacterID,
                    EmployeePipelinePhoneTitle = x.Title?.Trim(),
                    EmployeePipelinePhoneRemark = x.Remark?.Trim(),
                    EmployeePipelinePhoneCreateEmployeeID = x.CreateEmployeeID
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

    /// <summary>核心-員工-商機電銷-資料庫-新增</summary>
    public async Task<CoEmpPplPhnDbAddRspMdl> AddAsync(CoEmpPplPhnDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelinePhone
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                RecordTime = reqObj.EmployeePipelinePhoneRecordTime,
                ManagerContacterID = reqObj.ManagerContacterID,
                Title = reqObj.EmployeePipelinePhoneTitle?.Trim(),
                Remark = reqObj.EmployeePipelinePhoneRemark?.Trim(),
                CreateEmployeeID = reqObj.EmployeePipelinePhoneCreateEmployeeID,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.EmployeePipelinePhone.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplPhnDbAddRspMdl
            {
                EmployeePipelinePhoneID = item.ID
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

    /// <summary>核心-員工-商機電銷-資料庫-更新</summary>
    public async Task<CoEmpPplPhnDbUpdateRspMdl> UpdateAsync(CoEmpPplPhnDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelinePhone
                .Where(x => x.ID == reqObj.EmployeePipelinePhoneID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.RecordTime, x => reqObj.EmployeePipelinePhoneRecordTime.HasValue ? reqObj.EmployeePipelinePhoneRecordTime.Value : x.RecordTime)
                    .SetProperty(x => x.ManagerContacterID, x => reqObj.ManagerContacterID.HasValue ? reqObj.ManagerContacterID.Value : x.ManagerContacterID)
                    .SetProperty(x => x.Title, x => !string.IsNullOrEmpty(reqObj.EmployeePipelinePhoneTitle) ? reqObj.EmployeePipelinePhoneTitle.Trim() : x.Title)
                    .SetProperty(x => x.Remark, x => !string.IsNullOrEmpty(reqObj.EmployeePipelinePhoneRemark) ? reqObj.EmployeePipelinePhoneRemark.Trim() : x.Remark)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplPhnDbUpdateRspMdl
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

    /// <summary>核心-員工-商機電銷-資料庫-移除</summary>
    public async Task<CoEmpPplPhnDbRemoveRspMdl> RemoveAsync(CoEmpPplPhnDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelinePhone
                .Where(x => x.ID == reqObj.EmployeePipelinePhoneID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplPhnDbRemoveRspMdl
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

    /// <summary>核心-員工-商機電銷-資料庫-取得[筆數]從[活動ID]</summary>
    public async Task<CoEmpPplPhnDbGetCountFromManagerActivityIDRspMdl> GetCountFromManagerActivityIDAsync(CoEmpPplPhnDbGetCountFromManagerActivityIDReqMdl reqObj)
    {
        try
        {
            var count = await (
                from epp in this._mainContext.EmployeePipelinePhone
                                        .AsNoTracking()
                from ep in this._mainContext.EmployeePipeline
                                        .AsNoTracking()
                                        .Where(ep =>
                                                ep.ManagerActivityID.HasValue &&
                                                ep.ManagerActivityID == reqObj.ManagerActivityID &&
                                                ep.ID == epp.EmployeePipelineID)
                select new
                {
                    epp.ID,
                }
            )
            .Distinct()
            .CountAsync();

            var rspObj = new CoEmpPplPhnDbGetCountFromManagerActivityIDRspMdl
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

    /// <summary>核心-員工-商機電銷-資料庫-取得多筆[筆數]從[活動ID]</summary>
    public async Task<CoEmpPplPhnDbGetManyCountFromManagerActivityIDRspMdl> GetManyCountFromManagerActivityIDAsync(CoEmpPplPhnDbGetManyCountFromManagerActivityIDReqMdl reqObj)
    {
        try
        {
            var itemList = await (
                from epp in this._mainContext.EmployeePipelinePhone
                                        .AsNoTracking()
                from ep in this._mainContext.EmployeePipeline
                                        .AsNoTracking()
                                        .Where(ep =>
                                                ep.ManagerActivityID.HasValue &&
                                                reqObj.ManagerActivityIDList.Contains(ep.ManagerActivityID.Value) &&
                                                ep.ID == epp.EmployeePipelineID)
                group ep by ep.ManagerActivityID.Value into g
                select new
                {
                    ManagerActivityID = g.Key,
                    Count = g.Count()
                }
            )
            .ToListAsync();

            var rspObj = new CoEmpPplPhnDbGetManyCountFromManagerActivityIDRspMdl
            {
                EmployeePipelinePhoneList = itemList.Select(x => new CoEmpPplPhnDbGetManyCountRspItemMdl
                {
                    ManagerActivityID = x.ManagerActivityID,
                    Count = x.Count
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
}
