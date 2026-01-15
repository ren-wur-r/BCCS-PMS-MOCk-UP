using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆管理者公司-回應項目模型</summary>
public class MbsBscCtlGetManyCompanyRspItemMdl
{
    /// <summary>管理者公司-編號</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyName { get; set; }
}
