using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆窗口開發評等原因-回應模型</summary>
public class MbsBscCtlGetManyContacterRatingReasonRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>窗口開發評等原因列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyContacterRatingReasonRspItemMdl> ManagerContacterRatingReasonList { get; set; }
}
