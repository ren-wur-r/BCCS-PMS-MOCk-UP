using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Service;

/// <summary>管理者後台-CRM-活動管理-邏輯服務</summary>
public interface IMbsCrmActLogicalService
{
    #region 活動

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動</summary>
    public Task<MbsCrmActLgcGetManyActivityRspMdl> GetManyActivityAsync(MbsCrmActLgcGetManyActivityReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得單筆活動</summary>
    public Task<MbsCrmActLgcGetActivityRspMdl> GetActivityAsync(MbsCrmActLgcGetActivityReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動</summary>
    public Task<MbsCrmActLgcAddActivityRspMdl> AddActivityAsync(MbsCrmActLgcAddActivityReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動</summary>
    public Task<MbsCrmActLgcUpdateActivityRspMdl> UpdateActivityAsync(MbsCrmActLgcUpdateActivityReqMdl reqObj);

    #endregion

    #region 活動產品

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動產品</summary>
    public Task<MbsCrmActLgcAddActivityProductRspMdl> AddActivityProductAsync(MbsCrmActLgcAddActivityProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動產品</summary>
    public Task<MbsCrmActLgcUpdateActivityProductRspMdl> UpdateActivityProductAsync(MbsCrmActLgcUpdateActivityProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動產品</summary>
    public Task<MbsCrmActLgcRemoveActivityProductRspMdl> RemoveActivityProductAsync(MbsCrmActLgcRemoveActivityProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動產品</summary>
    public Task<MbsCrmActLgcGetActivityProductRspMdl> GetActivityProductAsync(MbsCrmActLgcGetActivityProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動產品</summary>
    public Task<MbsCrmActLgcGetManyActivityProductRspMdl> GetManyActivityProductAsync(MbsCrmActLgcGetManyActivityProductReqMdl reqObj);

    #endregion

    #region 活動贊助

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動贊助</summary>
    public Task<MbsCrmActLgcAddActivitySponsorRspMdl> AddActivitySponsorAsync(MbsCrmActLgcAddActivitySponsorReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動贊助</summary>
    public Task<MbsCrmActLgcUpdateActivitySponsorRspMdl> UpdateActivitySponsorAsync(MbsCrmActLgcUpdateActivitySponsorReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動贊助</summary>
    public Task<MbsCrmActLgcRemoveActivitySponsorRspMdl> RemoveActivitySponsorAsync(MbsCrmActLgcRemoveActivitySponsorReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動贊助</summary>
    public Task<MbsCrmActLgcGetActivitySponsorRspMdl> GetActivitySponsorAsync(MbsCrmActLgcGetActivitySponsorReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動贊助</summary>
    public Task<MbsCrmActLgcGetManyActivitySponsorRspMdl> GetManyActivitySponsorAsync(MbsCrmActLgcGetManyActivitySponsorReqMdl reqObj);

    #endregion

    #region 活動支出

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動支出</summary>
    public Task<MbsCrmActLgcAddActivityExpenseRspMdl> AddActivityExpenseAsync(MbsCrmActLgcAddActivityExpenseReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動支出</summary>
    public Task<MbsCrmActLgcUpdateActivityExpenseRspMdl> UpdateActivityExpenseAsync(MbsCrmActLgcUpdateActivityExpenseReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動支出</summary>
    public Task<MbsCrmActLgcRemoveActivityExpenseRspMdl> RemoveActivityExpenseAsync(MbsCrmActLgcRemoveActivityExpenseReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動支出</summary>
    public Task<MbsCrmActLgcGetActivityExpenseRspMdl> GetActivityExpenseAsync(MbsCrmActLgcGetActivityExpenseReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動支出</summary>
    public Task<MbsCrmActLgcGetManyActivityExpenseRspMdl> GetManyActivityExpenseAsync(MbsCrmActLgcGetManyActivityExpenseReqMdl reqObj);

    #endregion

    #region 活動名單

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動名單</summary>
    public Task<MbsCrmActLgcGetManyActivityEmployeePipelineRspMdl> GetManyActivityEmployeePipelineAsync(MbsCrmActLgcGetManyActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動名單</summary>
    public Task<MbsCrmActLgcGetActivityEmployeePipelineRspMdl> GetActivityEmployeePipelineAsync(MbsCrmActLgcGetActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動名單</summary>
    public Task<MbsCrmActLgcAddActivityEmployeePipelineRspMdl> AddActivityEmployeePipelineAsync(MbsCrmActLgcAddActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新多筆活動名單</summary>
    public Task<MbsCrmActLgcUpdateManyActivityEmployeePipelineRspMdl> UpdateManyActivityEmployeePipelineAsync(MbsCrmActLgcUpdateManyActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除多筆活動名單</summary>
    public Task<MbsCrmActLgcRemoveManyActivityEmployeePipelineRspMdl> RemoveManyActivityEmployeePipelineAsync(MbsCrmActLgcRemoveManyActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆客戶過往活動</summary>
    public Task<MbsCrmActLgcGetManyPastActivityRspMdl> GetManyPastActivityAsync(MbsCrmActLgcGetManyPastActivityReqMdl reqObj);

    #endregion

    #region 活動問卷

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動問卷問題</summary>
    public Task<MbsCrmActLgcAddActivitySurveyQuestionRspMdl> AddActivitySurveyQuestionAsync(MbsCrmActLgcAddActivitySurveyQuestionReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動問卷問題</summary>
    public Task<MbsCrmActLgcGetActivitySurveyQuestionRspMdl> GetActivitySurveyQuestionAsync(MbsCrmActLgcGetActivitySurveyQuestionReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動問卷問題</summary>
    public Task<MbsCrmActLgcUpdateActivitySurveyQuestionRspMdl> UpdateActivitySurveyQuestionAsync(MbsCrmActLgcUpdateActivitySurveyQuestionReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動問卷問題</summary>
    public Task<MbsCrmActLgcRemoveActivitySurveyQuestionRspMdl> RemoveActivitySurveyQuestionAsync(MbsCrmActLgcRemoveActivitySurveyQuestionReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動問卷回答者</summary>
    public Task<MbsCrmActLgcGetManyActivitySurveyAnswererRspMdl> GetManyActivitySurveyAnswererAsync(MbsCrmActLgcGetManyActivitySurveyAnswererReqMdl reqObj);
    #endregion

    #region eDM

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-下載eDM範本</summary>
    public Task<MbsCrmActLgcDownloadEdmTemplateRspMdl> DownloadEdmTemplateAsync(MbsCrmActLgcDownloadEdmTemplateReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-匯入eDM</summary>
    public Task<MbsCrmActLgcImportEdmRspMdl> ImportEdmAsync(MbsCrmActLgcImportEdmReqMdl reqObj);

    #endregion

    #region Teams

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-下載Teams範本</summary>
    public Task<MbsCrmActLgcDownloadTeamsTemplateRspMdl> DownloadTeamsTemplateAsync(MbsCrmActLgcDownloadTeamsTemplateReqMdl reqObj);

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-匯入Teams</summary>
    public Task<MbsCrmActLgcImportTeamsRspMdl> ImportTeamsAsync(MbsCrmActLgcImportTeamsReqMdl reqObj);

    #endregion
}