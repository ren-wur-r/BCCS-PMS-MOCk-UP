using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Service;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務</summary>
public interface IMbsCrmPhnLogicalService
{
    #region 活動

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動</summary>
    public Task<MbsCrmPhnLgcGetManyActivityRspMdl> GetManyActivityAsync(MbsCrmPhnLgcGetManyActivityReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動</summary>
    public Task<MbsCrmPhnLgcGetActivityRspMdl> GetActivityAsync(MbsCrmPhnLgcGetActivityReqMdl reqObj);

    #endregion

    #region 活動名單

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動名單</summary>
    public Task<MbsCrmPhnLgcGetManyActivityEmployeePipelineRspMdl> GetManyActivityEmployeePipelineAsync(MbsCrmPhnLgcGetManyActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得活動名單</summary>
    public Task<MbsCrmPhnLgcGetActivityEmployeePipelineRspMdl> GetActivityEmployeePipelineAsync(MbsCrmPhnLgcGetActivityEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆客戶過往活動</summary>
    public Task<MbsCrmPhnLgcGetManyPastActivityRspMdl> GetManyPastActivityAsync(MbsCrmPhnLgcGetManyPastActivityReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新活動名單狀態</summary>
    public Task<MbsCrmPhnLgcUpdateActivityEmployeePipelineStatusRspMdl> UpdateActivityEmployeePipelineStatusAsync(MbsCrmPhnLgcUpdateActivityEmployeePipelineStatusReqMdl reqObj);

    #endregion

    #region 客戶

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得客戶</summary>
    public Task<MbsCrmPhnLgcGetEmployeePipelineCompanyRspMdl> GetEmployeePipelineCompanyAsync(MbsCrmPhnLgcGetEmployeePipelineCompanyReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新名單客戶</summary>
    public Task<MbsCrmPhnLgcUpdateEmployeePipelineCompanyRspMdl> UpdateEmployeePipelineCompanyAsync(MbsCrmPhnLgcUpdateEmployeePipelineCompanyReqMdl reqObj);

    #endregion

    #region 窗口

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得原始窗口</summary>
    public Task<MbsCrmPhnLgcGetOriginalEmployeePipelineContacterRspMdl> GetOriginalEmployeePipelineContacterAsync(MbsCrmPhnLgcGetOriginalEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增名單窗口</summary>
    public Task<MbsCrmPhnLgcAddEmployeePipelineContacterRspMdl> AddEmployeePipelineContacterAsync(MbsCrmPhnLgcAddEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新名單窗口</summary>
    public Task<MbsCrmPhnLgcUpdateEmployeePipelineContacterRspMdl> UpdateEmployeePipelineContacterAsync(MbsCrmPhnLgcUpdateEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-刪除名單窗口</summary>
    public Task<MbsCrmPhnLgcRemoveEmployeePipelineContacterRspMdl> RemoveEmployeePipelineContacterAsync(MbsCrmPhnLgcRemoveEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單窗口</summary>
    public Task<MbsCrmPhnLgcGetManyEmployeePipelineContacterRspMdl> GetManyEmployeePipelineContacterAsync(MbsCrmPhnLgcGetManyEmployeePipelineContacterReqMdl reqObj);

    #endregion

    #region 指派業務紀錄

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-指派業務</summary>
    public Task<MbsCrmPhnLgcAddEmployeePipelineSalerRspMdl> AddEmployeePipelineSalerAsync(MbsCrmPhnLgcAddEmployeePipelineSalerReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆指派業務</summary>
    public Task<MbsCrmPhnLgcGetManyEmployeePipelineSalerRspMdl> GetManyEmployeePipelineSalerAsync(MbsCrmPhnLgcGetManyEmployeePipelineSalerReqMdl reqObj);

    #endregion

    #region 電銷紀錄

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增電銷紀錄</summary>
    public Task<MbsCrmPhnLgcAddEmployeePipelinePhoneRspMdl> AddEmployeePipelinePhoneAsync(MbsCrmPhnLgcAddEmployeePipelinePhoneReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷紀錄</summary>
    public Task<MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspMdl> GetManyEmployeePipelinePhoneAsync(MbsCrmPhnLgcGetManyEmployeePipelinePhoneReqMdl reqObj);

    #endregion

    #region 產品

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷產品</summary>
    public Task<MbsCrmPhnLgcGetManyEmployeePipelineProductRspMdl> GetManyEmployeePipelineProductAsync(MbsCrmPhnLgcGetManyEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得電銷產品</summary>
    public Task<MbsCrmPhnLgcGetEmployeePipelineProductRspMdl> GetEmployeePipelineProductAsync(MbsCrmPhnLgcGetEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增商機產品</summary>
    public Task<MbsCrmPhnLgcAddEmployeePipelineProductRspMdl> AddEmployeePipelineProductAsync(MbsCrmPhnLgcAddEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新電銷產品</summary>
    public Task<MbsCrmPhnLgcUpdateEmployeePipelineProductRspMdl> UpdateEmployeePipelineProductAsync(MbsCrmPhnLgcUpdateEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-刪除電銷產品</summary>
    public Task<MbsCrmPhnLgcRemoveEmployeePipelineProductRspMdl> RemoveEmployeePipelineProductAsync(MbsCrmPhnLgcRemoveEmployeePipelineProductReqMdl reqObj);

    #endregion

}