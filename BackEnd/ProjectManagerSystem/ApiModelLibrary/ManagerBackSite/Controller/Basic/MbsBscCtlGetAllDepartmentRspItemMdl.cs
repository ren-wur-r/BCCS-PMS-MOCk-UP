using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得全部管理者部門-回應項目模型</summary>
public class MbsBscCtlGetAllDepartmentRspItemMdl
{
    /// <summary>管理者部門-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者部門-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerDepartmentName { get; set; }
}