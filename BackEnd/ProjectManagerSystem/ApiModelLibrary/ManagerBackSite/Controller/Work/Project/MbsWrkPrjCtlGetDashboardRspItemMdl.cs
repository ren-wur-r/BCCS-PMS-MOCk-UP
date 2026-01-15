using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得儀表板-回應項目模型</summary>
public class MbsWrkPrjCtlGetDashboardRspItemMdl
{
    /// <summary>員工專案ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>管理者公司名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyName { get; set; }

    /// <summary>員工專案名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案起始時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }
}
