using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-活動產品-控制器-取得-回應模型</summary>
public class MbsCrmActCtlGetActivityProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者活動產品ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    [JsonPropertyName("b")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-主分類-ID</summary>
    [JsonPropertyName("c")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類-ID</summary>
    [JsonPropertyName("d")]
    public int ManagerProductSubKindID { get; set; }
}
