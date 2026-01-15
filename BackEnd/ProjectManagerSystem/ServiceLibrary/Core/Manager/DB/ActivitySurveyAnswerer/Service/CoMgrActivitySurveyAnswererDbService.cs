using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.ManagerActivitySurveyAnswerer;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Service;

/// <summary>核心-管理者-活動問卷-資料庫服務</summary>
public class CoMgrActivitySurveyAnswererDbService : ICoMgrActivitySurveyAnswererDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrActivitySurveyAnswererDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrActivitySurveyAnswererDbService(
        ILogger<CoMgrActivitySurveyAnswererDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-活動問卷-資料庫-新增</summary>
    public async Task<CoMgrAsaDbAddRspMdl> AddAsync(CoMgrAsaDbAddReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        try
        {
            var item = new ManagerActivitySurveyAnswerer
            {
                ManagerActivityID = reqObj.ManagerActivityID,
                IsConsent = reqObj.IsConsent,
                CompanyName = reqObj.ManagerActivitySurveyAnswererCompanyName.Trim(),
                CompanyPhone = reqObj.ManagerActivitySurveyAnswererCompanyPhone?.Trim(),
                CompanyAddress = reqObj.ManagerActivitySurveyAnswererCompanyAddress?.Trim(),
                ContacterName = reqObj.ManagerActivitySurveyAnswererContacterName.Trim(),
                ContacterEmail = reqObj.ManagerActivitySurveyAnswererContacterEmail.Trim(),
                ContacterPhone = reqObj.ManagerActivitySurveyAnswererContacterPhone?.Trim(),
                ContacterDepartment = reqObj.ManagerActivitySurveyAnswererContacterDepartment?.Trim(),
                ContacterJobTitle = reqObj.ManagerActivitySurveyAnswererContacterJobTitle?.Trim(),
                ContacterTelephone = reqObj.ManagerActivitySurveyAnswererContacterTelephone?.Trim(),
                CompanyScaleID = reqObj.ManagerActivitySurveyAnswererCompanyScaleID.ToInt16(),
                CompanyBudgetID = reqObj.ManagerActivitySurveyAnswererCompanyBudgetID.ToInt16(),
                CompanyPurchaseID = reqObj.ManagerActivitySurveyAnswererCompanyPurchaseID.ToInt16(),
                FeedbackTargetID = reqObj.ManagerActivitySurveyAnswererFeedbackTargetID.ToInt16(),
                FeedbackScheduleID = reqObj.ManagerActivitySurveyAnswererFeedbackScheduleID.ToInt16(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };

            this._mainContext.ManagerActivitySurveyAnswerer.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrAsaDbAddRspMdl
            {
                ManagerActivitySurveyAnswererID = item.ID,
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

    /// <summary>核心-管理者-活動問卷-資料庫-更新</summary>
    public async Task<CoMgrAsaDbUpdateRspMdl> UpdateAsync(CoMgrAsaDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTime.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyAnswerer
                .Where(x => x.ID == reqObj.ManagerActivitySurveyAnswererID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.IsConsent, x => reqObj.IsConsent ?? x.IsConsent)
                        .SetProperty(x => x.CompanyName, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererCompanyName) ? x.CompanyName : reqObj.ManagerActivitySurveyAnswererCompanyName.Trim())
                        .SetProperty(x => x.CompanyPhone, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererCompanyPhone) ? x.CompanyPhone : reqObj.ManagerActivitySurveyAnswererCompanyPhone.Trim())
                        .SetProperty(x => x.CompanyAddress, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererCompanyAddress) ? x.CompanyAddress : reqObj.ManagerActivitySurveyAnswererCompanyAddress.Trim())
                        .SetProperty(x => x.ContacterName, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterName) ? x.ContacterName : reqObj.ManagerActivitySurveyAnswererContacterName.Trim())
                        .SetProperty(x => x.ContacterEmail, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterEmail) ? x.ContacterEmail : reqObj.ManagerActivitySurveyAnswererContacterEmail.Trim())
                        .SetProperty(x => x.ContacterPhone, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterPhone) ? x.ContacterPhone : reqObj.ManagerActivitySurveyAnswererContacterPhone.Trim())
                        .SetProperty(x => x.ContacterDepartment, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterDepartment) ? x.ContacterDepartment : reqObj.ManagerActivitySurveyAnswererContacterDepartment.Trim())
                        .SetProperty(x => x.ContacterJobTitle, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterJobTitle) ? x.ContacterJobTitle : reqObj.ManagerActivitySurveyAnswererContacterJobTitle.Trim())
                        .SetProperty(x => x.ContacterTelephone, x => string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterTelephone) ? x.ContacterTelephone : reqObj.ManagerActivitySurveyAnswererContacterTelephone.Trim())
                        .SetProperty(x => x.CompanyScaleID, x => reqObj.ManagerActivitySurveyAnswererCompanyScaleID.HasValue ? reqObj.ManagerActivitySurveyAnswererCompanyScaleID.Value.ToInt16() : x.CompanyScaleID)
                        .SetProperty(x => x.CompanyBudgetID, x => reqObj.ManagerActivitySurveyAnswererCompanyBudgetID.HasValue ? reqObj.ManagerActivitySurveyAnswererCompanyBudgetID.Value.ToInt16() : x.CompanyBudgetID)
                        .SetProperty(x => x.CompanyPurchaseID, x => reqObj.ManagerActivitySurveyAnswererCompanyPurchaseID.HasValue ? reqObj.ManagerActivitySurveyAnswererCompanyPurchaseID.Value.ToInt16() : x.CompanyPurchaseID)
                        .SetProperty(x => x.FeedbackTargetID, x => reqObj.ManagerActivitySurveyAnswererFeedbackTargetID.HasValue ? reqObj.ManagerActivitySurveyAnswererFeedbackTargetID.Value.ToInt16() : x.FeedbackTargetID)
                        .SetProperty(x => x.FeedbackScheduleID, x => reqObj.ManagerActivitySurveyAnswererFeedbackScheduleID.HasValue ? reqObj.ManagerActivitySurveyAnswererFeedbackScheduleID.Value.ToInt16() : x.FeedbackScheduleID)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrAsaDbUpdateRspMdl
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

    /// <summary>核心-管理者-活動問卷-資料庫-移除</summary>
    public async Task<CoMgrAsaDbRemoveRspMdl> RemoveAsync(CoMgrAsaDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerActivitySurveyAnswerer
                .Where(x => x.ID == reqObj.ManagerActivitySurveyAnswererID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrAsaDbRemoveRspMdl
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

    /// <summary>核心-管理者-活動問卷-資料庫-取得</summary>
    public async Task<CoMgrAsaDbGetRspMdl> GetAsync(CoMgrAsaDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerActivitySurveyAnswerer
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerActivitySurveyAnswererID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrAsaDbGetRspMdl
            {
                ManagerActivityID = item.ID,
                ManagerActivitySurveyAnswererIsConsent = item.IsConsent,
                ManagerActivitySurveyAnswererCompanyName = item.CompanyName.Trim(),
                ManagerActivitySurveyAnswererCompanyPhone = item.CompanyPhone?.Trim(),
                ManagerActivitySurveyAnswererCompanyAddress = item.CompanyAddress?.Trim(),
                ManagerActivitySurveyAnswererContacterName = item.ContacterName.Trim(),
                ManagerActivitySurveyAnswererContacterEmail = item.ContacterEmail.Trim(),
                ManagerActivitySurveyAnswererContacterPhone = item.ContacterPhone?.Trim(),
                ManagerActivitySurveyAnswererContacterDepartment = item.ContacterDepartment?.Trim(),
                ManagerActivitySurveyAnswererContacterJobTitle = item.ContacterJobTitle?.Trim(),
                ManagerActivitySurveyAnswererContacterTelephone = item.ContacterTelephone?.Trim(),
                ManagerActivitySurveyAnswererCompanyScaleID = item.CompanyScaleID.ToEnum<DbManagerActivitySurveyAnswererCompanyScaleEnum>(),
                ManagerActivitySurveyAnswererCompanyBudgetID = item.CompanyBudgetID.ToEnum<DbManagerActivitySurveyAnswererCompanyBudgetEnum>(),
                ManagerActivitySurveyAnswererCompanyPurchaseID = item.CompanyPurchaseID.ToEnum<DbManagerActivitySurveyAnswererCompanyPurchaseEnum>(),
                ManagerActivitySurveyAnswererFeedbackTargetID = item.FeedbackTargetID.ToFlagEnum<DbManagerActivitySurveyAnswererFeedbackTargetEnum>(),
                ManagerActivitySurveyAnswererFeedbackScheduleID = item.FeedbackScheduleID.ToEnum<DbManagerActivitySurveyAnswererFeedbackScheduleEnum>(),
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

    /// <summary>核心-管理者-活動問卷-資料庫-取得筆數從[管理者活動ID]</summary>
    public async Task<CoMgrAsaDbGetCountFromActivityIdRspMdl> GetCountFromActivityIdAsync(CoMgrAsaDbGetCountFromActivityIdReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerActivitySurveyAnswerer
                .AsNoTracking()
                .Where(x =>
                    // 管理者活動ID
                    x.ManagerActivityID == reqObj.ManagerActivityID
                    // 公司名稱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererCompanyName) || x.CompanyName.Contains(reqObj.ManagerActivitySurveyAnswererCompanyName))
                    // 窗口姓名
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterName) || x.ContacterName.Contains(reqObj.ManagerActivitySurveyAnswererContacterName))
                    // 窗口信箱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterEmail) || x.ContacterEmail.Contains(reqObj.ManagerActivitySurveyAnswererContacterEmail)))
                .CountAsync();

            var rspObj = new CoMgrAsaDbGetCountFromActivityIdRspMdl
            {
                Count = count,
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

    /// <summary>核心-管理者-活動問卷-資料庫-取得多筆從[管理者活動ID]</summary>
    public async Task<CoMgrAsaDbGetManyFromActivityIdRspMdl> GetManyFromActivityIdAsync(CoMgrAsaDbGetManyFromActivityIdReqMdl reqObj)
    {

        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerActivitySurveyAnswerer
                .AsNoTracking()
                .Where(x =>
                    // 管理者活動ID
                    x.ManagerActivityID == reqObj.ManagerActivityID
                    // 公司名稱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererCompanyName) || x.CompanyName.Contains(reqObj.ManagerActivitySurveyAnswererCompanyName))
                    // 窗口姓名
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterName) || x.ContacterName.Contains(reqObj.ManagerActivitySurveyAnswererContacterName))
                    // 窗口信箱
                    && (string.IsNullOrWhiteSpace(reqObj.ManagerActivitySurveyAnswererContacterEmail) || x.ContacterEmail.Contains(reqObj.ManagerActivitySurveyAnswererContacterEmail)))
                .Select(x => new
                {
                    x.ID,
                    x.CompanyName,
                    x.ContacterName,
                    x.ContacterEmail,
                    x.CompanyScaleID,
                    x.CompanyBudgetID,
                    x.CompanyPurchaseID,
                })
                .OrderByDescending(x => x.ID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrAsaDbGetManyFromActivityIdRspMdl
            {
                ManagerActivitySurveyAnswererList = itemList
                .Select(x => new CoMgrAsaDbGetManyFromActivityIdRspItemMdl
                {
                    ManagerActivitySurveyAnswererID = x.ID,
                    ManagerActivitySurveyAnswererCompanyName = x.CompanyName.Trim(),
                    ManagerActivitySurveyAnswererContacterName = x.ContacterName.Trim(),
                    ManagerActivitySurveyAnswererContacterEmail = x.ContacterEmail.Trim(),
                    ManagerActivitySurveyAnswererCompanyScaleID = x.CompanyScaleID.ToEnum<DbManagerActivitySurveyAnswererCompanyScaleEnum>(),
                    ManagerActivitySurveyAnswererCompanyBudgetID = x.CompanyBudgetID.ToEnum<DbManagerActivitySurveyAnswererCompanyBudgetEnum>(),
                    ManagerActivitySurveyAnswererCompanyPurchaseID = x.CompanyPurchaseID.ToEnum<DbManagerActivitySurveyAnswererCompanyPurchaseEnum>(),
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