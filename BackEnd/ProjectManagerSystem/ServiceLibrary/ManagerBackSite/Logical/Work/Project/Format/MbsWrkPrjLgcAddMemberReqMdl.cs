using DataModelLibrary.Database.EmployeeProjectMember;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-新增人員-請求模型</summary>
public class MbsWrkPrjLgcAddMemberReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工專案成員角色-列舉</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

}