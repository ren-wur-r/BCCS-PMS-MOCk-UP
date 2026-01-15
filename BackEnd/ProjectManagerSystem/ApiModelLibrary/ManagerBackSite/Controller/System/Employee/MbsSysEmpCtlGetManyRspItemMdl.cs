using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-取得多筆-回應項目模型</summary>
public class MbsSysEmpCtlGetManyRspItemMdl
{
    /// <summary>管理者-部門-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRoleName { get; set; }

    /// <summary>員工-ID</summary>
    [JsonPropertyName("c")]
    public int EmployeeID { get; set; }

    /// <summary>員工-姓名</summary>
    [JsonPropertyName("d")]
    public string EmployeeName { get; set; }

    /// <summary>員工-帳號</summary>
    [JsonPropertyName("e")]
    public string EmployeeAccount { get; set; }

    /// <summary>員工-是否啟用</summary>
    [JsonPropertyName("f")]
    public bool EmployeeIsEnable { get; set; }
}