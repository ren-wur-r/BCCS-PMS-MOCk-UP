using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得單筆活動-產品-回應項目模型</summary>
public class MbsCrmPhnCtlGetActivityProductRspItemMdl
{
    /// <summary>管理者活動產品ID</summary>
    [JsonPropertyName("a")]
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品-主分類名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品-子分類名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerProductSubKindName { get; set; }
}
