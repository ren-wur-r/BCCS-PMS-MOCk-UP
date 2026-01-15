using ApiModelLibrary.ManagerBackSite.Controller.Base;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-更新窗口-回應模型</summary>
public class MbsSysCtrCtlUpdateContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口ID（如果修改了公司，回傳新的窗口 ID）</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ManagerContacterID { get; set; }
}