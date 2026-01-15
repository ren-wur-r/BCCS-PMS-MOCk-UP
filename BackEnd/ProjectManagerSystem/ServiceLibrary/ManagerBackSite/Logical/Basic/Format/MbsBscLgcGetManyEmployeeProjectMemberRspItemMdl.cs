using DataModelLibrary.Database.EmployeeProjectMember;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員-回應項目模型</summary>
public class MbsBscLgcGetManyEmployeeProjectMemberRspItemMdl
{
    /// <summary>員工專案成員角色-列舉</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

    /// <summary>員工編號</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工名稱</summary>
    public string EmployeeName { get; set; }

}