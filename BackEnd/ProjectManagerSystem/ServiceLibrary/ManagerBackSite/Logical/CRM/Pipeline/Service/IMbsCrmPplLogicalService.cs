using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Service;

/// <summary>管理者後台-CRM-商機管理-邏輯服務</summary>
public interface IMbsCrmPplLogicalService
{
    #region PipelineProduct 商機產品

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機產品</summary>
    public Task<MbsCrmPplLgcGetEmployeePipelineProductRspMdl> GetEmployeePipelineProductAsync(MbsCrmPplLgcGetEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機產品</summary>
    public Task<MbsCrmPplLgcAddEmployeePipelineProductRspMdl> AddEmployeePipelineProductAsync(MbsCrmPplLgcAddEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機產品</summary>
    public Task<MbsCrmPplLgcUpdateEmployeePipelineProductRspMdl> UpdateEmployeePipelineProductAsync(MbsCrmPplLgcUpdateEmployeePipelineProductReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除商機產品</summary>
    public Task<MbsCrmPplLgcRemoveEmployeePipelineProductRspMdl> RemoveEmployeePipelineProductAsync(MbsCrmPplLgcRemoveEmployeePipelineProductReqMdl reqObj);

    #endregion

    #region 名單

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆名單</summary>
    public Task<MbsCrmPplLgcGetManyEmployeePipelineRspMdl> GetManyEmployeePipelineAsync(MbsCrmPplLgcGetManyEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得名單</summary>
    public Task<MbsCrmPplLgcGetEmployeePipelineRspMdl> GetEmployeePipelineAsync(MbsCrmPplLgcGetEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機狀態</summary>
    public Task<MbsCrmPplLgcUpdatePipelineStatusRspMdl> UpdatePipelineStatusAsync(MbsCrmPplLgcUpdatePipelineStatusReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-轉換商機至專案</summary>
    public Task<MbsCrmPplLgcTransferPipelineToProjectRspMdl> TransferPipelineToProjectAsync(MbsCrmPplLgcTransferPipelineToProjectReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增名單</summary>
    public Task<MbsCrmPplLgcAddEmployeePipelineRspMdl> AddEmployeePipelineAsync(MbsCrmPplLgcAddEmployeePipelineReqMdl reqObj);

    #endregion

    #region 客戶

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得客戶</summary>
    public Task<MbsCrmPplLgcGetEmployeePipelineCompanyRspMdl> GetEmployeePipelineCompanyAsync(MbsCrmPplLgcGetEmployeePipelineCompanyReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新名單客戶</summary>
    public Task<MbsCrmPplLgcUpdateEmployeePipelineCompanyRspMdl> UpdateEmployeePipelineCompanyAsync(MbsCrmPplLgcUpdateEmployeePipelineCompanyReqMdl reqObj);

    #endregion

    #region 窗口       

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增名單窗口</summary>
    public Task<MbsCrmPplLgcAddEmployeePipelineContacterRspMdl> AddEmployeePipelineContacterAsync(MbsCrmPplLgcAddEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新名單窗口</summary>
    public Task<MbsCrmPplLgcUpdateEmployeePipelineContacterRspMdl> UpdateEmployeePipelineContacterAsync(MbsCrmPplLgcUpdateEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除名單窗口</summary>
    public Task<MbsCrmPplLgcRemoveEmployeePipelineContacterRspMdl> RemoveEmployeePipelineContacterAsync(MbsCrmPplLgcRemoveEmployeePipelineContacterReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆名單窗口</summary>
    public Task<MbsCrmPplLgcGetManyEmployeePipelineContacterRspMdl> GetManyEmployeePipelineContacterAsync(MbsCrmPplLgcGetManyEmployeePipelineContacterReqMdl reqObj);

    #endregion

    #region 業務紀錄

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務</summary>
    public Task<MbsCrmPplLgcGetManyEmployeePipelineSalerRspMdl> GetManyEmployeePipelineSalerAsync(MbsCrmPplLgcGetManyEmployeePipelineSalerReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-處理商機業務紀錄</summary>
    public Task<MbsCrmPplLgcHandleEmployeePipelineSalerRspMdl> HandleEmployeePipelineSalerAsync(MbsCrmPplLgcHandleEmployeePipelineSalerReqMdl reqObj);

    #endregion

    #region 開發紀錄

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務開發紀錄</summary>
    public Task<MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingRspMdl> GetManyEmployeePipelineSalerTrackingAsync(MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機業務開發紀錄</summary>
    public Task<MbsCrmPplLgcAddEmployeePipelineSalerTrackingRspMdl> AddEmployeePipelineSalerTrackingAsync(MbsCrmPplLgcAddEmployeePipelineSalerTrackingReqMdl reqObj);

    #endregion

    #region 發票紀錄

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機發票紀錄</summary>
    public Task<MbsCrmPplLgcGetEmployeePipelineBillRspMdl> GetEmployeePipelineBillAsync(MbsCrmPplLgcGetEmployeePipelineBillReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機發票紀錄</summary>
    public Task<MbsCrmPplLgcGetManyEmployeePipelineBillRspMdl> GetManyEmployeePipelineBillAsync(MbsCrmPplLgcGetManyEmployeePipelineBillReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增多筆商機發票紀錄</summary>
    public Task<MbsCrmPplLgcAddManyEmployeePipelineBillRspMdl> AddManyEmployeePipelineBillAsync(MbsCrmPplLgcAddManyEmployeePipelineBillReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機發票紀錄</summary>
    public Task<MbsCrmPplLgcUpdateEmployeePipelineRspMdl> UpdateEmployeePipelineBillAsync(MbsCrmPplLgcUpdateEmployeePipelineReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新多筆商機發票紀錄</summary>
    public Task<MbsCrmPplLgcUpdateManyEmployeePipelineBillRspMdl> UpdateManyEmployeePipelineBillAsync(MbsCrmPplLgcUpdateManyEmployeePipelineBillReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-移除多筆商機發票紀錄</summary>
    public Task<MbsCrmPplLgcRemoveManyEmployeePipelineBillRspMdl> RemoveManyEmployeePipelineBillAsync(MbsCrmPplLgcRemoveManyEmployeePipelineBillReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-通知開立發票</summary>
    public Task<MbsCrmPplLgcNotifyBillIssueRspMdl> NotifyBillIssueAsync(MbsCrmPplLgcNotifyBillIssueReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-確認開立發票</summary>
    public Task<MbsCrmPplLgcConfirmBillIssueRspMdl> ConfirmBillIssueAsync(MbsCrmPplLgcConfirmBillIssueReqMdl reqObj);

    #endregion

    #region 履約期限通知

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機履約期限</summary>
    public Task<MbsCrmPplLgcGetEmployeePipelineDueRspMdl> GetEmployeePipelineDueAsync(MbsCrmPplLgcGetEmployeePipelineDueReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機履約期限</summary>
    public Task<MbsCrmPplLgcGetManyEmployeePipelineDueRspMdl> GetManyEmployeePipelineDueAsync(MbsCrmPplLgcGetManyEmployeePipelineDueReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機履約期限</summary>
    public Task<MbsCrmPplLgcAddEmployeePipelineDueRspMdl> AddEmployeePipelineDueAsync(MbsCrmPplLgcAddEmployeePipelineDueReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機履約期限</summary>
    public Task<MbsCrmPplLgcUpdateEmployeePipelineDueRspMdl> UpdateEmployeePipelineDueAsync(MbsCrmPplLgcUpdateEmployeePipelineDueReqMdl reqObj);

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除商機履約期限</summary>
    public Task<MbsCrmPplLgcRemoveEmployeePipelineDueRspMdl> RemoveEmployeePipelineDueAsync(MbsCrmPplLgcRemoveEmployeePipelineDueReqMdl reqObj);

    #endregion
}