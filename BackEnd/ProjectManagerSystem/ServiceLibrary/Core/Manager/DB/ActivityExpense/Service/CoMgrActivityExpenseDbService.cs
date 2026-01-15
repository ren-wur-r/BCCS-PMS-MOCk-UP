using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Service;

/// <summary>核心-管理者-活動支出-資料庫服務</summary>
public class CoMgrActivityExpenseDbService : ICoMgrActivityExpenseDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivityExpenseDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivityExpenseDbService(
        ILogger<CoMgrActivityExpenseDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動支出-資料庫-是否存在</summary>
    public async Task<CoMgrActExpDbExistRspMdl> ExistAsync(CoMgrActExpDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerActivityExpense
                .AsNoTracking()
                .AnyAsync(x =>
                    (x.ID == reqObj.ManagerActivityID)
                    && (x.ExpenseItem == reqObj.ManagerActivityExpenseItem.Trim()));

            var rspObj = new CoMgrActExpDbExistRspMdl
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

    /// <summary>核心-管理者-活動支出-資料庫-新增</summary>
    public async Task<CoMgrActExpDbAddRspMdl> AddAsync(CoMgrActExpDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerActivityExpense
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                ExpenseItem = reqObj.ManagerActivityExpenseItem?.Trim(),
                Quantity = reqObj.ManagerActivityExpenseQuantity,
                TotalAmount = reqObj.ManagerActivityExpenseTotalAmount,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerActivityExpense.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActExpDbAddRspMdl
            {
                ManagerActivityExpenseID = item.ID
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

    /// <summary>核心-管理者-活動支出-資料庫-新增多筆</summary>
    public async Task<CoMgrActExpDbAddManyRspMdl> AddManyAsync(CoMgrActExpDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.ManagerActivityExpenseList
                        .Select(x => new ManagerActivityExpense()
                        {
                            ManagerActivityID = x.ManagerActivityID,
                            ExpenseItem = x.ManagerActivityExpenseItem?.Trim(),
                            Quantity = x.ManagerActivityExpenseQuantity,
                            TotalAmount = x.ManagerActivityExpenseTotalAmount,
                            CreateTime = currentTime,
                            UpdateTime = currentTime,
                        })
                        .ToList();

            this._mainContext.ManagerActivityExpense.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActExpDbAddManyRspMdl()
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

    /// <summary>核心-管理者-活動支出-資料庫-更新</summary>
    public async Task<CoMgrActExpDbUpdateRspMdl> UpdateAsync(CoMgrActExpDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivityExpense
                .Where(x => x.ID == reqObj.ManagerActivityExpenseID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerActivityID, x => reqObj.ManagerActivityID.HasValue ? reqObj.ManagerActivityID.Value : x.ManagerActivityID)
                    .SetProperty(x => x.ExpenseItem, x => !string.IsNullOrWhiteSpace(reqObj.ManagerActivityExpenseItem) ? reqObj.ManagerActivityExpenseItem.Trim() : x.ExpenseItem)
                    .SetProperty(x => x.Quantity, x => reqObj.ManagerActivityExpenseQuantity.HasValue ? reqObj.ManagerActivityExpenseQuantity.Value : x.Quantity)
                    .SetProperty(x => x.TotalAmount, x => reqObj.ManagerActivityExpenseTotalAmount.HasValue ? reqObj.ManagerActivityExpenseTotalAmount.Value : x.TotalAmount)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrActExpDbUpdateRspMdl
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

    /// <summary>核心-管理者-活動支出-資料庫-移除</summary>
    public async Task<CoMgrActExpDbRemoveRspMdl> RemoveAsync(CoMgrActExpDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivityExpense
                .Where(x => x.ID == reqObj.ManagerActivityExpenseID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrActExpDbRemoveRspMdl
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

    /// <summary>核心-管理者-活動支出-資料庫-取得</summary>
    public async Task<CoMgrActExpDbGetRspMdl> GetAsync(CoMgrActExpDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivityExpense
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivityExpenseID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrActExpDbGetRspMdl
            {
                ManagerActivityExpenseID = item.ID,
                ManagerActivityID = item.ManagerActivityID,
                ManagerActivityExpenseItem = item.ExpenseItem?.Trim(),
                ManagerActivityExpenseQuantity = item.Quantity,
                ManagerActivityExpenseTotalAmount = item.TotalAmount
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

    /// <summary>核心-管理者-活動支出-資料庫-取得多筆</summary>
    public async Task<CoMgrActExpDbGetManyRspMdl> GetManyAsync(CoMgrActExpDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivityExpense
                .AsNoTracking()
                .Where(x =>
                    // 管理者活動ID
                    (x.ManagerActivityID == reqObj.ManagerActivityID) &&
                    // 管理者活動支出-項目(模糊查詢)
                    (string.IsNullOrEmpty(reqObj.ManagerActivityExpenseItem) || x.ExpenseItem.Contains(reqObj.ManagerActivityExpenseItem))
                )
                .Select(x => new
                {
                    x.ID,
                    x.ExpenseItem,
                    x.Quantity,
                    x.TotalAmount
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoMgrActExpDbGetManyRspMdl
            {
                ManagerActivityExpenseList = itemList?
                .Select(x => new CoMgrActExpDbGetManyRspItemMdl
                {
                    ManagerActivityExpenseID = x.ID,
                    ManagerActivityExpenseItem = x.ExpenseItem?.Trim(),
                    ManagerActivityExpenseQuantity = x.Quantity,
                    ManagerActivityExpenseTotalAmount = x.TotalAmount
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

    #region ManagerBackSite.Marketing.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-活動支出-資料庫-取得多筆總支出額從[管理者後台-CRM-活動管理]</summary>
    public async Task<CoMgrActExpDbGetManyTotalExpenseRspMdl> GetManyTotalExpenseAsync(CoMgrActExpDbGetManyTotalExpenseReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivityExpense
                .AsNoTracking()
                .Where(x => reqObj.ManagerActivityIDList.Contains(x.ManagerActivityID))
                .GroupBy(x => x.ManagerActivityID)
                .Select(g => new CoMgrActExpDbGetManyTotalExpenseRspItemMdl
                {
                    ManagerActivityID = g.Key,
                    ManagerActivityExpenseTotalExpenseAmount = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            var rspObj = new CoMgrActExpDbGetManyTotalExpenseRspMdl
            {
                ManagerActivityExpenseList = itemList
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
