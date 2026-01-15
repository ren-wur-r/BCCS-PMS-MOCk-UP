using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司名稱從[商機原始]-請求模型</summary>
public class MbsBscCtlGetManyCompanyNameFromPipelineOriginalReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司-名稱(模糊查詢)</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanyName { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("b")]
    public int PageIndex { get; set; }
}
