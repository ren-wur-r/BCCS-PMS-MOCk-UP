using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆窗口-回應模型</summary>
public class MbsBscCtlGetManyManagerContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyContacterRspItemMdl> ManagerContacterList { get; set; }
}
