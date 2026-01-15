using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.ManagerActivitySurveyQuestion;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Service;

/// <summary>核心-管理者-活動問卷問題-資料庫服務</summary>
public class CoMgrActivitySurveyQuestionDbService : ICoMgrActivitySurveyQuestionDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivitySurveyQuestionDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivitySurveyQuestionDbService(
        ILogger<CoMgrActivitySurveyQuestionDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動問卷問題-資料庫-新增</summary>
    public async Task<CoMgrAsqDbAddRspMdl> AddAsync(CoMgrAsqDbAddReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        try
        {
            var item = new ManagerActivitySurveyQuestion()
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                KindID = reqObj.ManagerActivitySurveyQuestionKind.ToInt16(),
                Title = reqObj.ManagerActivitySurveyQuestionTitle.Trim(),
                Content = reqObj.ManagerActivitySurveyQuestionContent.Trim(),
                IsOther = reqObj.IsOther,
                Sort = reqObj.ManagerActivitySurveyQuestionSort,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerActivitySurveyQuestion.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrAsqDbAddRspMdl()
            {
                ManagerActivitySurveyQuestionID = item.ID,
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

    /// <summary>核心-管理者-活動問卷問題-資料庫-新增多筆</summary>
    public async Task<CoMgrAsqDbAddManyRspMdl> AddManyAsync(CoMgrAsqDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;


        var itemList = reqObj.ManagerActivitySurveyQuestionList
            .Select(x => new ManagerActivitySurveyQuestion()
            {
                ManagerActivityID = x.ManagerActivityID,
                KindID = x.ManagerActivitySurveyQuestionKind.ToInt16(),
                Title = x.ManagerActivitySurveyQuestionTitle.Trim(),
                Content = x.ManagerActivitySurveyQuestionContent.Trim(),
                IsOther = x.IsOther,
                Sort = x.ManagerActivitySurveyQuestionSort,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            })
            .ToList();

        try
        {
            this._mainContext.ManagerActivitySurveyQuestion.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrAsqDbAddManyRspMdl()
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
    /// <summary>核心-管理者-活動問卷問題-資料庫-更新</summary>
    public async Task<CoMgrAsqDbUpdateRspMdl> UpdateAsync(CoMgrAsqDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestion
                .Where(x => x.ID == reqObj.ManagerActivitySurveyQuestionID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.KindID, x => reqObj.ManagerActivitySurveyQuestionKind.HasValue ? reqObj.ManagerActivitySurveyQuestionKind.Value.ToInt16() : x.KindID)
                    .SetProperty(x => x.Title, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyQuestionTitle) ? x.Title : reqObj.ManagerActivitySurveyQuestionTitle.Trim())
                    .SetProperty(x => x.Content, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyQuestionContent) ? x.Content : reqObj.ManagerActivitySurveyQuestionContent.Trim())
                    .SetProperty(x => x.IsOther, x => reqObj.IsOther ?? x.IsOther)
                    .SetProperty(x => x.Sort, x => reqObj.ManagerActivitySurveyQuestionSort ?? x.Sort)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrAsqDbUpdateRspMdl
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

    /// <summary>核心-管理者-活動問卷問題-資料庫-移除</summary>
    public async Task<CoMgrAsqDbRemoveRspMdl> RemoveAsync(CoMgrAsqDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestion
                .Where(x => x.ID == reqObj.ManagerActivitySurveyQuestionID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrAsqDbRemoveRspMdl
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

    /// <summary>核心-管理者-活動問卷問題-資料庫-移除多筆</summary>
    public async Task<CoMgrAsqDbRemoveManyRspMdl> RemoveManyAsync(CoMgrAsqDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyQuestion
                .Where(x => x.ManagerActivityID == reqObj.ManagerActivityID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrAsqDbRemoveManyRspMdl
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

    /// <summary>核心-管理者-活動問卷問題-資料庫-取得</summary>
    public async Task<CoMgrAsqDbGetRspMdl> GetAsync(CoMgrAsqDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivitySurveyQuestion
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivitySurveyQuestionID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrAsqDbGetRspMdl()
            {
                ManagerActivityID = item.ManagerActivityID,
                ManagerActivitySurveyQuestionKind = item.KindID.ToEnum<DbManagerActivitySurveyQuestionKindEnum>(),
                ManagerActivitySurveyQuestionTitle = item.Title.Trim(),
                ManagerActivitySurveyQuestionContent = item.Content.Trim(),
                IsOther = item.IsOther,
                ManagerActivitySurveyQuestionSort = item.Sort,
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

    /// <summary>核心-管理者-活動問卷問題-資料庫-取得多筆從[管理者活動ID]</summary>
    public async Task<CoMgrAsqDbGetManyFromActIdRspMdl> GetManyFromActIdAsync(CoMgrAsqDbGetManyFromActIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivitySurveyQuestion
                .AsNoTracking()
                .Where(x => x.ManagerActivityID == reqObj.ManagerActivityID)
                .Select(x => new
                {
                    x.ID,
                    x.KindID,
                    x.Title,
                    x.Content,
                    x.IsOther,
                    x.Sort,
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();

            var rspObj = new CoMgrAsqDbGetManyFromActIdRspMdl()
            {
                ManagerActivitySurveyQuestionList = itemList
                    .Select(x => new CoMgrAsqDbGetManyFromActIdRspItemMdl()
                    {
                        ManagerActivitySurveyQuestionID = x.ID,
                        ManagerActivitySurveyQuestionKind = x.KindID.ToEnum<DbManagerActivitySurveyQuestionKindEnum>(),
                        ManagerActivitySurveyQuestionTitle = x.Title.Trim(),
                        ManagerActivitySurveyQuestionContent = x.Content.Trim(),
                        IsOther = x.IsOther,
                        ManagerActivitySurveyQuestionSort = x.Sort,
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