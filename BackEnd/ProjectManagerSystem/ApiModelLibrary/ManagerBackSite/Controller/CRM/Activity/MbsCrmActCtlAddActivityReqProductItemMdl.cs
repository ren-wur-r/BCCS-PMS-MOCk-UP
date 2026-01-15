using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-新增-產品項目-請求模型</summary>
public class MbsCrmActCtlAddActivityReqProductItemMdl
{
    /// <summary>管理者產品ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerProductID { get; set; }
}
