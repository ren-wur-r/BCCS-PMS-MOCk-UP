using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.ProjectExpense.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectExpense.Service;

/// <summary>核心-員工-專案支出-資料庫服務</summary>
public class CoEmpProjectExpenseDbService : ICoEmpProjectExpenseDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpProjectExpenseDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpProjectExpenseDbService(
        ILogger<CoEmpProjectExpenseDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-專案支出-資料庫-取得</summary>
    public async Task<CoEmpPrjExpDbGetRspMdl> GetAsync(CoEmpPrjExpDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectExpense
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectExpenseID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPrjExpDbGetRspMdl
            {
                EmployeeProjectID = item.EmployeeProjectID,
                EmployeeProjectExpenseName = item.Name?.Trim(),
                EmployeeProjectExpensePredictAmount = item.PredictAmount,
                EmployeeProjectExpenseActualAmount = item.ActualAmount,
                EmployeeProjectExpenseBillNumber = item.BillNumber?.Trim(),
                EmployeeProjectExpenseBillTime = item.BillTime,
                EmployeeProjectExpenseRemark = item.Remark?.Trim(),
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

    /// <summary>核心-員工-專案支出-資料庫-取得員工專案ID</summary>
    public async Task<CoEmpPrjExpDbGetEmployeeProjectIdRspMdl> GetEmployeeProjectIdAsync(CoEmpPrjExpDbGetEmployeeProjectIdReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeeProjectExpense
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeProjectExpenseID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPrjExpDbGetEmployeeProjectIdRspMdl
            {
                EmployeeProjectID = item.EmployeeProjectID,
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

    /// <summary>核心-員工-專案支出-資料庫-取得多筆</summary>
    public async Task<CoEmpPrjExpDbGetManyRspMdl> GetManyAsync(CoEmpPrjExpDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeeProjectExpense
                .AsNoTracking()
                .Where(x => x.EmployeeProjectID == reqObj.EmployeeProjectID)
                .Select(x => new
                {
                    EmployeeProjectExpenseID = x.ID,
                    EmployeeProjectExpenseName = x.Name,
                    EmployeeProjectExpensePredictAmount = x.PredictAmount,
                    EmployeeProjectExpenseActualAmount = x.ActualAmount,
                    EmployeeProjectExpenseBillNumber = x.BillNumber,
                    EmployeeProjectExpenseBillTime = x.BillTime,
                    EmployeeProjectExpenseRemark = x.Remark,
                })
                .ToListAsync();

            var rspObj = new CoEmpPrjExpDbGetManyRspMdl
            {
                EmployeeProjectExpenseList = itemList.Select(x => new CoEmpPrjExpDbGetManyRspItemMdl
                {
                    EmployeeProjectExpenseID = x.EmployeeProjectExpenseID,
                    EmployeeProjectExpenseName = x.EmployeeProjectExpenseName?.Trim(),
                    EmployeeProjectExpensePredictAmount = x.EmployeeProjectExpensePredictAmount,
                    EmployeeProjectExpenseActualAmount = x.EmployeeProjectExpenseActualAmount,
                    EmployeeProjectExpenseBillNumber = x.EmployeeProjectExpenseBillNumber?.Trim(),
                    EmployeeProjectExpenseBillTime = x.EmployeeProjectExpenseBillTime,
                    EmployeeProjectExpenseRemark = x.EmployeeProjectExpenseRemark?.Trim(),
                })
                .OrderBy(x => x.EmployeeProjectExpenseID)
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

    /// <summary>核心-員工-專案支出-資料庫-新增</summary>
    public async Task<CoEmpPrjExpDbAddRspMdl> AddAsync(CoEmpPrjExpDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeeProjectExpense
            {
                EmployeeProjectID = reqObj.EmployeeProjectID,
                Name = reqObj.EmployeeProjectExpenseName.Trim(),
                PredictAmount = reqObj.EmployeeProjectExpensePredictAmount,
                ActualAmount = reqObj.EmployeeProjectExpenseActualAmount,
                BillNumber = reqObj.EmployeeProjectExpenseBillNumber?.Trim(),
                BillTime = reqObj.EmployeeProjectExpenseBillTime,
                Remark = reqObj.EmployeeProjectExpenseRemark?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.EmployeeProjectExpense.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPrjExpDbAddRspMdl
            {
                EmployeeProjectExpenseID = item.ID
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

    /// <summary>核心-員工-專案支出-資料庫-更新</summary>
    public async Task<CoEmpPrjExpDbUpdateRspMdl> UpdateAsync(CoEmpPrjExpDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectExpense
                .Where(x => x.ID == reqObj.EmployeeProjectExpenseID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectExpenseName) ? x.Name : reqObj.EmployeeProjectExpenseName.Trim())
                    .SetProperty(x => x.PredictAmount, x => reqObj.EmployeeProjectExpensePredictAmount.HasValue ? reqObj.EmployeeProjectExpensePredictAmount.Value : x.PredictAmount)
                    .SetProperty(x => x.ActualAmount, x => reqObj.EmployeeProjectExpenseActualAmount.HasValue ? reqObj.EmployeeProjectExpenseActualAmount.Value : x.ActualAmount)
                    .SetProperty(x => x.BillNumber, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectExpenseBillNumber) ? x.BillNumber : reqObj.EmployeeProjectExpenseBillNumber.Trim())
                    .SetProperty(x => x.BillTime, x => reqObj.EmployeeProjectExpenseBillTime.HasValue ? reqObj.EmployeeProjectExpenseBillTime.Value : x.BillTime)
                    .SetProperty(x => x.Remark, x => string.IsNullOrWhiteSpace(reqObj.EmployeeProjectExpenseRemark) ? x.Remark : reqObj.EmployeeProjectExpenseRemark.Trim())
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPrjExpDbUpdateRspMdl
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

    /// <summary>核心-員工-專案支出-資料庫-移除</summary>
    public async Task<CoEmpPrjExpDbRemoveRspMdl> RemoveAsync(CoEmpPrjExpDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeeProjectExpense
                .Where(x => x.ID == reqObj.EmployeeProjectExpenseID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPrjExpDbRemoveRspMdl
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
}