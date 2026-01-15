using DataModelLibrary.Database.EmployeeProjectMember;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

public class MbsWrkPrjLgcAddProjectReqItemMdl
{
    /// <summary>員工專案成員角色</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

}