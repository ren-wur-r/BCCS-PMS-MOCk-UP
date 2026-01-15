using System.Text.Json.Serialization;
using DataModelLibrary.Database.EmployeeProjectMember;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案-回應項目模型</summary>
public class MbsWrkPrjCtlGetProjectRspItemMdl
{
    /// <summary>員工專案成員角色-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectMemberID { get; set; }

    /// <summary>員工專案成員角色</summary>
    [JsonPropertyName("b")]
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>員工姓名</summary>
    [JsonPropertyName("e")]
    public string EmployeeName { get; set; }

    /// <summary>員工-ID</summary>
    [JsonPropertyName("f")]
    public int EmployeeID { get; set; }

}