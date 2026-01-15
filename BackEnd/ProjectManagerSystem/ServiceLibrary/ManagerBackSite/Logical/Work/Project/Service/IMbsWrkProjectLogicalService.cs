using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Service;

/// <summary>管理者後台-工作-專案-邏輯服務介面</summary>
public interface IMbsWrkProjectLogicalService
{
    #region 儀錶板

    /// <summary>管理者後台-工作-專案-邏輯服務-取得儀錶板</summary>
    public Task<MbsWrkPrjLgcGetDashboardRspMdl> GetDashboardAsync(MbsWrkPrjLgcGetDashboardReqMdl reqObj);

    #endregion

    #region 專案

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案</summary>
    public Task<MbsWrkPrjLgcGetManyProjectRspMdl> GetManyProjectAsync(MbsWrkPrjLgcGetManyProjectReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案</summary>
    public Task<MbsWrkPrjLgcGetProjectRspMdl> GetProjectAsync(MbsWrkPrjLgcGetProjectReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-新增專案</summary>
    public Task<MbsWrkPrjLgcAddProjectRspMdl> AddProjectAsync(MbsWrkPrjLgcAddProjectReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案</summary>
    public Task<MbsWrkPrjLgcUpdateProjectRspMdl> UpdateProjectAsync(MbsWrkPrjLgcUpdateProjectReqMdl reqObj);

    #endregion

    #region 合約

    /// <summary>管理者後台-工作-專案-邏輯服務-新增合約</summary>
    public Task<MbsWrkPrjLgcAddContractRspMdl> AddContractAsync(MbsWrkPrjLgcAddContractReqMdl reqObj);

    #endregion

    #region 工作計劃書

    /// <summary>管理者後台-工作-專案-邏輯服務-新增工作計劃書</summary>
    public Task<MbsWrkPrjLgcAddWorkRspMdl> AddWorkAsync(MbsWrkPrjLgcAddWorkReqMdl reqObj);

    #endregion

    #region 成員

    /// <summary>管理者後台-工作-專案-邏輯服務-新增成員</summary>
    public Task<MbsWrkPrjLgcAddMemberRspMdl> AddMemberAsync(MbsWrkPrjLgcAddMemberReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-移除成員</summary>
    public Task<MbsWrkPrjLgcRemoveMemberRspMdl> RemoveMemberAsync(MbsWrkPrjLgcRemoveMemberReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆成員角色</summary>
    public Task<MbsWrkPrjLgcGetManyMemberRoleRspMdl> GetManyMemberRoleAsync(MbsWrkPrjLgcGetManyMemberRoleReqMdl reqObj);

    #endregion

    #region 里程碑

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑</summary>
    public Task<MbsWrkPrjLgcGetManyProjectStoneRspMdl> GetManyProjectStoneAsync(MbsWrkPrjLgcGetManyProjectStoneReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑</summary>
    public Task<MbsWrkPrjLgcGetProjectStoneRspMdl> GetProjectStoneAsync(MbsWrkPrjLgcGetProjectStoneReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-新增專案里程碑</summary>
    public Task<MbsWrkPrjLgcAddProjectStoneRspMdl> AddProjectStoneAsync(MbsWrkPrjLgcAddProjectStoneReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑</summary>
    public Task<MbsWrkPrjLgcUpdateProjectStoneRspMdl> UpdateProjectStoneAsync(MbsWrkPrjLgcUpdateProjectStoneReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-移除專案里程碑</summary>
    public Task<MbsWrkPrjLgcRemoveProjectStoneRspMdl> RemoveProjectStoneAsync(MbsWrkPrjLgcRemoveProjectStoneReqMdl reqObj);

    #endregion

    #region 工項

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項</summary>
    public Task<MbsWrkPrjLgcGetManyProjectStoneJobRspMdl> GetManyProjectStoneJobAsync(MbsWrkPrjLgcGetManyProjectStoneJobReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑工項</summary>
    public Task<MbsWrkPrjLgcGetProjectStoneJobRspMdl> GetProjectStoneJobAsync(MbsWrkPrjLgcGetProjectStoneJobReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑工項</summary>
    public Task<MbsWrkPrjLgcUpdateProjectStoneJobRspMdl> UpdateProjectStoneJobAsync(MbsWrkPrjLgcUpdateProjectStoneJobReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑工項清單</summary>
    public Task<MbsWrkPrjLgcUpdateProjectStoneJobBucketRspMdl> UpdateProjectStoneJobBucketAsync(MbsWrkPrjLgcUpdateProjectStoneJobBucketReqMdl reqObj);

    #endregion

    #region 支出

    /// <summary>管理者後台-工作-專案-邏輯服務-取得專案支出</summary>
    public Task<MbsWrkPrjLgcGetExpenseRspMdl> GetExpenseAsync(MbsWrkPrjLgcGetExpenseReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案支出</summary>
    public Task<MbsWrkPrjLgcGetManyExpenseRspMdl> GetManyExpenseAsync(MbsWrkPrjLgcGetManyExpenseReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-新增專案支出</summary>
    public Task<MbsWrkPrjLgcAddExpenseRspMdl> AddExpenseAsync(MbsWrkPrjLgcAddExpenseReqMdl reqObj);

    /// <summary>管理者後台-工作-專案-邏輯服務-更新專案支出</summary>
    public Task<MbsWrkPrjLgcUpdateExpenseRspMdl> UpdateExpenseAsync(MbsWrkPrjLgcUpdateExpenseReqMdl reqObj);
    #endregion

}