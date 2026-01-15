using DataModelLibrary.Database.EmployeeProjectMember;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯轉換-商機至專案-項目-請求模型</summary>
public class MbsCrmPplLgcTransferPipelineToProjectReqItemMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工專案成員角色-列舉</summary>
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }
}