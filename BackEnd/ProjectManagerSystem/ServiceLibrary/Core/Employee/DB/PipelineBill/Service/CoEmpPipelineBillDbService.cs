using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomEmployeePipelineBill;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineBill.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineBill.Service;


/// <summary>核心-員工-商機發票紀錄-資料庫服務</summary>
public class CoEmpPipelineBillDbService : ICoEmpPipelineBillDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineBillDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineBillDbService(
        ILogger<CoEmpPipelineBillDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機發票紀錄-資料庫-取得</summary>
    public async Task<CoEmpPplBllDbGetRspMdl> GetAsync(CoEmpPplBllDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineBill
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineBillID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplBllDbGetRspMdl
            {
                EmployeePipelineBillID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                EmployeePipelineBillPeriodNumber = item.PeriodNumber,
                EmployeePipelineBillBillTime = item.BillTime,
                EmployeePipelineBillBillNumber = item.BillNumber?.Trim(),
                EmployeePipelineBillNoTaxAmount = item.NoTaxAmount,
                EmployeePipelineBillRemark = item.Remark?.Trim(),
                EmployeePipelineBillStatus = item.Status.ToEnum<DbAtomEmployeePipelineBillStatusEnum>(),
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

    /// <summary>核心-員工-商機發票紀錄-資料庫-取得多筆</summary>
    public async Task<CoEmpPplBllDbGetManyRspMdl> GetManyAsync(CoEmpPplBllDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineBill
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    EmployeePipelineBillID = x.ID,
                    EmployeePipelineBillPeriodNumber = x.PeriodNumber,
                    EmployeePipelineBillBillTime = x.BillTime,
                    EmployeePipelineBillBillNumber = x.BillNumber,
                    EmployeePipelineBillNoTaxAmount = x.NoTaxAmount,
                    EmployeePipelineBillRemark = x.Remark,
                    EmployeePipelineBillStatus = x.Status.ToEnum<DbAtomEmployeePipelineBillStatusEnum>(),
                })
                .ToListAsync();

            var rspObj = new CoEmpPplBllDbGetManyRspMdl
            {
                EmployeePipelineBillList = itemList
                .Select(x => new CoEmpPplBllDbGetManyRspItemMdl
                {
                    EmployeePipelineBillID = x.EmployeePipelineBillID,
                    EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                    EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                    EmployeePipelineBillBillNumber = x.EmployeePipelineBillBillNumber,
                    EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                    EmployeePipelineBillRemark = x.EmployeePipelineBillRemark?.Trim(),
                    EmployeePipelineBillStatus = x.EmployeePipelineBillStatus,
                })
                .OrderBy(x => x.EmployeePipelineBillPeriodNumber)
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
    }

    /// <summary>核心-員工-商機發票紀錄-資料庫-新增多筆</summary>
    public async Task<CoEmpPplBllDbAddManyRspMdl> AddManyAsync(CoEmpPplBllDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineBillList
                .Select(x => new EmployeePipelineBill
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    PeriodNumber = x.EmployeePipelineBillPeriodNumber,
                    BillTime = x.EmployeePipelineBillBillTime,
                    NoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                    Remark = x.EmployeePipelineBillRemark,
                    Status = x.EmployeePipelineBillStatus.ToInt16(),
                })
                .ToList();

            this._mainContext.EmployeePipelineBill.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplBllDbAddManyRspMdl
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

    /// <summary>核心-員工-商機發票紀錄-資料庫-更新</summary>
    public async Task<CoEmpPplBllDbUpdateRspMdl> UpdateAsync(CoEmpPplBllDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineBill
                .Where(x => x.ID == reqObj.EmployeePipelineBillID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.BillNumber, x => string.IsNullOrWhiteSpace(reqObj.EmployeePipelineBillNumber) ? x.BillNumber : reqObj.EmployeePipelineBillNumber.Trim())
                        .SetProperty(x => x.Remark, x => string.IsNullOrWhiteSpace(reqObj.EmployeePipelineBillRemark) ? x.Remark : reqObj.EmployeePipelineBillRemark.Trim())
                        .SetProperty(x => x.Status, x => reqObj.EmployeePipelineBillStatus.HasValue ? reqObj.EmployeePipelineBillStatus.Value.ToInt16() : x.Status)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplBllDbUpdateRspMdl
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

    /// <summary>核心-員工-商機發票紀錄-資料庫-移除多筆</summary>
    public async Task<CoEmpPplBllDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplBllDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineBill
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplBllDbRemoveManyRspMdl
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
}