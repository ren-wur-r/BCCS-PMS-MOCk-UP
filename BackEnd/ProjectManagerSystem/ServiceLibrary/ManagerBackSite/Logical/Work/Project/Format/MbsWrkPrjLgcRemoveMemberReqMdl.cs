using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-移除成員-請求模型</summary>
public class MbsWrkPrjLgcRemoveMemberReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案成員角色-ID</summary>
    public int EmployeeProjectMemberID { get; set; }

}