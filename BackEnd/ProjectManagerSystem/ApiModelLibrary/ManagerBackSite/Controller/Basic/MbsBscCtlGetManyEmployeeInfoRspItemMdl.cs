using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆員工資訊-回應項目模型</summary>
public class MbsBscCtlGetManyEmployeeInfoRspItemMdl
{
    /// <summary>員工-ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeID { get; set; }

    /// <summary>員工-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeName { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool EmployeeIsEnable { get; set; }

    /// <summary>管理者-地區-ID</summary>
    [JsonPropertyName("d")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-ID</summary>
    [JsonPropertyName("f")]
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("g")]
    public string ManagerDepartmentName { get; set; }
}
