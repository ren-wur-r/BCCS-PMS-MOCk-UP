using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Service;

/// <summary>核心-管理者-活動問卷問題項目-資料庫服務</summary>
public class CoMgrActivitySurveyQuestionItemDbService : ICoMgrActivitySurveyQuestionItemDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivitySurveyQuestionItemDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivitySurveyQuestionItemDbService(
        ILogger<CoMgrActivitySurveyQuestionItemDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-新增</summary>
    public async Task<CoMgrActSqiDbAddRspMdl> AddAsync(CoMgrActSqiDbAddReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        try
        {
            var item = new ManagerActivitySurveyQuestionItem()
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                ManagerActivitySurveyQuestionID = reqObj.ManagerActivitySurveyQuestionID,
                Name = reqObj.ManagerActivitySurveyQuestionItemName?.Trim(),
                Sort = reqObj.ManagerActivitySurveyQuestionItemSort,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerActivitySurveyQuestionItem.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActSqiDbAddRspMdl()
            {
                ManagerActivitySurveyQuestionItemID = item.ID,
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

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-新增多筆</summary>
    public async Task<CoMgrActSqiDbAddManyRspMdl> AddManyAsync(CoMgrActSqiDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        var itemList = reqObj.ManagerActivitySurveyQuestionItemList
            .Select(x => new ManagerActivitySurveyQuestionItem()
            {
                ManagerActivityID = x.ManagerActivityID,
                ManagerActivitySurveyQuestionID = x.ManagerActivitySurveyQuestionID,
                Name = x.ManagerActivitySurveyQuestionItemName?.Trim(),
                Sort = x.ManagerActivitySurveyQuestionItemSort,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            })
            .ToList();

        try
        {
            this._mainContext.ManagerActivitySurveyQuestionItem.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrActSqiDbAddManyRspMdl()
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

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-更新</summary>
    public async Task<CoMgrActSqiDbUpdateRspMdl> UpdateAsync(CoMgrActSqiDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestionItem
                .Where(x => x.ID == reqObj.ManagerActivitySurveyQuestionItemID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyQuestionItemName) ? x.Name : reqObj.ManagerActivitySurveyQuestionItemName.Trim())
                    .SetProperty(x => x.Sort, x => reqObj.ManagerActivitySurveyQuestionItemSort ?? x.Sort)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrActSqiDbUpdateRspMdl()
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

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-移除</summary>
    public async Task<CoMgrActSqiDbRemoveRspMdl> RemoveAsync(CoMgrActSqiDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestionItem
                .Where(x => x.ID == reqObj.ManagerActivitySurveyQuestionItemID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrActSqiDbRemoveRspMdl()
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

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-移除多筆</summary>
    public async Task<CoMgrActSqiDbRemoveManyRspMdl> RemoveManyAsync(CoMgrActSqiDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestionItem
                .Where(x => x.ManagerActivitySurveyQuestionID == reqObj.ManagerActivitySurveyQuestionID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrActSqiDbRemoveManyRspMdl
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

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-移除多筆從[管理者活動ID]</summary>
    public async Task<CoMgrActSqiDbRemoveManyByActIdRspMdl> RemoveManyByActIdAsync(CoMgrActSqiDbRemoveManyByActIdReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestionItem
                .Where(x => x.ManagerActivityID == reqObj.ManagerActivityID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrActSqiDbRemoveManyByActIdRspMdl()
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


    /// <summary>核心-管理者-活動問卷問題項目-資料庫-取得多筆從[管理者活動問卷問題ID]</summary>
    public async Task<CoMgrActSqiDbGetManyFromAsqRspMdl> GetManyFromAsqAsync(CoMgrActSqiDbGetManyFromAsqReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivitySurveyQuestionItem
                .AsNoTracking()
                .Where(x => x.ManagerActivitySurveyQuestionID == reqObj.ManagerActivitySurveyQuestionID)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Sort,
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();

            var rspObj = new CoMgrActSqiDbGetManyFromAsqRspMdl()
            {
                ManagerActivitySurveyQuestionItemList = itemList
                .Select(x => new CoMgrActSqiDbGetManyFromAsqRspItemMdl()
                {
                    ManagerActivitySurveyQuestionItemID = x.ID,
                    ManagerActivitySurveyQuestionItemName = x.Name?.Trim(),
                    ManagerActivitySurveyQuestionItemSort = x.Sort,
                })
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
}