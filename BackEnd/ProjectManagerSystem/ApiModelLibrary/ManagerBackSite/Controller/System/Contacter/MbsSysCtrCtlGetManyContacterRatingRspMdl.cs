using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得多筆窗口開發評等-回應模型</summary>
public class MbsSysCtrCtlGetManyContacterRatingRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口開發評等-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsSysCtrCtlGetManyContacterRatingRspItemMdl> ManagerContacterRatingList { get; set; }
}
