using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆窗口-請求模型</summary>
public class MbsBscCtlGetManyManagerContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口-名稱(模糊查詢)</summary>
    [JsonPropertyName("a")]
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-公司ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-電子郵件(模糊查詢)</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageIndex { get; set; }
}
