using System.Text.Json.Serialization;
using DataModelLibrary.Database.EmployeeProjectMember;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員-回應項目模型</summary>
public class MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl
{
    /// <summary>員工專案成員角色-列舉</summary>
    [JsonPropertyName("a")]
    public DbEmployeeProjectMemberRoleEnum EmployeeProjectMemberRole { get; set; }

    /// <summary>員工編號</summary>
    [JsonPropertyName("b")]
    public int EmployeeID { get; set; }

    /// <summary>員工名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeeName { get; set; }
}