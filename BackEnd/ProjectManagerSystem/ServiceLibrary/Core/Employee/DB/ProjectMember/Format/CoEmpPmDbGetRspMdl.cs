using DataModelLibrary.Database.EmployeeProjectMember;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-資料庫-取得-回應模型</summary>
public class CoEmpPmDbGetRspMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工專案成員角色-列舉</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }
}