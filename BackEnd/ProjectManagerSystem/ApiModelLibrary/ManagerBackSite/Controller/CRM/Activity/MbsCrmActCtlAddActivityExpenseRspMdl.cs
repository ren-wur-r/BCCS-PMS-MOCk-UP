using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動支出-控制器-新增-回應模型</summary>
public class MbsCrmActCtlAddActivityExpenseRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動支出ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerActivityExpenseID { get; set; }
}
