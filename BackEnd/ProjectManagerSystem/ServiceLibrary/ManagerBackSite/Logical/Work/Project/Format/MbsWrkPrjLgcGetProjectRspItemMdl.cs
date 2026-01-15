using DataModelLibrary.Database.EmployeeProjectMember;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

public class MbsWrkPrjLgcGetProjectRspItemMdl
{
    /// <summary>員工專案成員角色-ID</summary>
    public int EmployeeProjectMemberID { get; set; }

    /// <summary>員工專案成員角色</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>員工姓名</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

}