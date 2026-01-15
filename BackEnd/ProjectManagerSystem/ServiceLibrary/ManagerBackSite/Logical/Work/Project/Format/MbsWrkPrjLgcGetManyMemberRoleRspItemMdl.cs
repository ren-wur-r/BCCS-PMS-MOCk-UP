using DataModelLibrary.Database.EmployeeProjectMember;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆成員角色-回應項目模型</summary>
public class MbsWrkPrjLgcGetManyMemberRoleRspItemMdl
{
    /// <summary>員工專案成員角色-列舉</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }
}