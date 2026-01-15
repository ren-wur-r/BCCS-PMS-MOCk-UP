using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-新增工作計劃書-請求模型</summary>
public class MbsWrkPrjCtlAddWorkReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案工作計劃書-網址</summary>
    [Url]
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectWorkUrl { get; set; }
}
