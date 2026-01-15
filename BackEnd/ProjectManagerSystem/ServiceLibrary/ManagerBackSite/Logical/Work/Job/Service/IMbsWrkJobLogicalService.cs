using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Service;

/// <summary>管理者後台-工作-工項-邏輯服務介面</summary>
public interface IMbsWrkJobLogicalService
{
    #region 專案里程碑工項

    /// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項</summary>
    public Task<MbsWrkJobLgcGetManyProjectStoneJobRspMdl> GetManyProjectStoneJobAsync(MbsWrkJobLgcGetManyProjectStoneJobReqMdl reqObj);

    /// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項</summary>
    public Task<MbsWrkJobLgcGetProjectStoneJobRspMdl> GetProjectStoneJobAsync(MbsWrkJobLgcGetProjectStoneJobReqMdl reqObj);

    /// <summary>管理者後台-工作-工項-邏輯服務-更新專案里程碑工項</summary>
    public Task<MbsWrkJobLgcUpdateProjectStoneJobRspMdl> UpdateProjectStoneJobAsync(MbsWrkJobLgcUpdateProjectStoneJobReqMdl reqObj);

    #endregion

    #region 專案里程碑工項工作

    /// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項工作</summary>
    public Task<MbsWrkJobLgcGetProjectStoneJobWorkRspMdl> GetProjectStoneJobWorkAsync(MbsWrkJobLgcGetProjectStoneJobWorkReqMdl reqObj);

    /// <summary>管理者後台-工作-工項-邏輯服務-新增專案里程碑工項工作</summary>
    public Task<MbsWrkJobLgcAddProjectStoneJobWorkRspMdl> AddProjectStoneJobWorkAsync(MbsWrkJobLgcAddProjectStoneJobWorkReqMdl reqObj);

    /// <summary>管理者後台-工作-工項-邏輯服務-更新專案里程碑工項工作</summary>
    public Task<MbsWrkJobLgcUpdateProjectStoneJobWorkRspMdl> UpdateProjectStoneJobWorkAsync(MbsWrkJobLgcUpdateProjectStoneJobWorkReqMdl reqObj);

    /// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項工作</summary>
    public Task<MbsWrkJobLgcGetManyProjectStoneJobWorkRspMdl> GetManyProjectStoneJobWorkAsync(MbsWrkJobLgcGetManyProjectStoneJobWorkReqMdl reqObj);

    /// <summary>管理者後台-工作-工項-邏輯服務-移除專案里程碑工項工作</summary>
    public Task<MbsWrkJobLgcRemoveProjectStoneJobWorkRspMdl> RemoveProjectStoneJobWorkAsync(MbsWrkJobLgcRemoveProjectStoneJobWorkReqMdl reqObj);

    #endregion

}