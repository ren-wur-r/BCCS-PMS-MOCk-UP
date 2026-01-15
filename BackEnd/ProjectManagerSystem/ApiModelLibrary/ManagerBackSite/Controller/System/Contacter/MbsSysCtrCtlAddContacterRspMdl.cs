using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-新增窗口-回應模型</summary>
public class MbsSysCtrCtlAddContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }
}
