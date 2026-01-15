using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswererItem.Service;

/// <summary>核心-管理者-活動問卷回答項目-資料庫服務</summary>
public class CoMgrActivitySurveyAnswererItemDbService : ICoMgrActivitySurveyAnswererItemDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivitySurveyAnswererItemDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivitySurveyAnswererItemDbService(
        ILogger<CoMgrActivitySurveyAnswererItemDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動問卷回答項目-資料庫-取得多筆</summary>
    public async Task<CoMgrActSaiDbGetManyRspMdl> GetManyAsync(CoMgrActSaiDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerActivitySurveyAnswererItem
                .AsNoTracking()
                .Where(x => reqObj.ManagerActivitySurveyAnswererID == x.ManagerActivitySurveyAnswererID)
                .Select(x => new
                {
                    x.ID,
                    x.ManagerActivitySurveyQuestionID,
                    x.ManagerActivitySurveyQuestionItemID,
                    x.IsChecked,
                    x.RatingValue,
                    x.Content,
                })
                .OrderBy(x => x.ID)
                .ToListAsync();

            var rspObj = new CoMgrActSaiDbGetManyRspMdl()
            {
                ManagerActivitySurveyAnswererItemList = itemList.Select(x => new CoMgrActSaiDbGetManyRspItemMdl()
                {
                    ManagerActivitySurveyAnswererItemID = x.ID,
                    ManagerActivitySurveyQuestionID = x.ManagerActivitySurveyQuestionID,
                    ManagerActivitySurveyQuestionItemID = x.ManagerActivitySurveyQuestionItemID,
                    IsChecked = x.IsChecked ?? false,
                    RatingValue = x.RatingValue ?? 0,
                    Content = x.Content?.Trim(),
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
    /// <summary>核心-管理者-活動問卷回答項目-資料庫-取得</summary>
    public async Task<CoMgrActSaiDbGetRspMdl> GetAsync(CoMgrActSaiDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivitySurveyAnswererItem
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivitySurveyAnswererItemID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrActSaiDbGetRspMdl()
            {
                ManagerActivitySurveyAnswererID = item.ManagerActivitySurveyAnswererID,
                ManagerActivitySurveyQuestionID = item.ManagerActivitySurveyQuestionID,
                ManagerActivitySurveyQuestionItemID = item.ManagerActivitySurveyQuestionItemID,
                IsChecked = item.IsChecked ?? false,
                RatingValue = item.RatingValue ?? 0,
                Content = item.Content,
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