using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動產品-控制器-新增-回應模型</summary>
public class MbsCrmActCtlAddActivityProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動產品ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivityProductID { get; set; }
}
