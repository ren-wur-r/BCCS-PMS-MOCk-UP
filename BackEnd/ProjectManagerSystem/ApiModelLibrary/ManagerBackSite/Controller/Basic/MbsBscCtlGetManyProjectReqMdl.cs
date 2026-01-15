using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆專案-請求模型</summary>
public class MbsBscCtlGetManyProjectReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>專案-名稱(模糊查詢)</summary>
    [JsonPropertyName("a")]
    public string EmployeeProjectName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("b")]
    public int PageIndex { get; set; }
}