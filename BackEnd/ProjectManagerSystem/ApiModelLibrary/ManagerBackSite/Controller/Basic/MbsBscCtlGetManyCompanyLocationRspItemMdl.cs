using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-展示層-取得多筆公司營業地點-項目-回應模型</summary>
public class MbsBscCtlGetManyCompanyLocationRspItemMdl
{
    /// <summary>管理者公司營業地點ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>管理者公司營業地點是否已刪除</summary>
    [JsonPropertyName("c")]
    public bool ManagerCompanyLocationIsDeleted { get; set; }
}
