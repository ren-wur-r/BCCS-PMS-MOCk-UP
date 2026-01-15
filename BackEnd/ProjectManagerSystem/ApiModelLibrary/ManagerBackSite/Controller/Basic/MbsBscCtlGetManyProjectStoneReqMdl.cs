using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-專案里程碑-取得多筆-請求模型</summary>
public class MbsBscCtlGetManyProjectStoneReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    [JsonPropertyName("a")]
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工專案里程碑-名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }
}