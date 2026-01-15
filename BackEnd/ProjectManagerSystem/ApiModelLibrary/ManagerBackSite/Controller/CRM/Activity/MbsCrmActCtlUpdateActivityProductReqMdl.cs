using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動產品-控制器-更新-請求模型</summary>
public class MbsCrmActCtlUpdateActivityProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者活動產品ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerProductID { get; set; }
}
