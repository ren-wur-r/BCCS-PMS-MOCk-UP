using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司名稱從[商機原始]-回應模型</summary>
public class MbsBscCtlGetManyCompanyNameFromPipelineOriginalRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者公司名稱-列表</summary>
    [JsonPropertyName("a")]
    public List<string> ManagerCompanyNameList { get; set; }
}
