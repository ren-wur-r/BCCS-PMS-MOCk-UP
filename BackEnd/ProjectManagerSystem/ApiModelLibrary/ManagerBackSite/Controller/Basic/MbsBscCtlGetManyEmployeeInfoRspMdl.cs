using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆員工資訊-回應模型</summary>
public class MbsBscCtlGetManyEmployeeInfoRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工-列表</summary>
    [JsonPropertyName("a")]
    public List<MbsBscCtlGetManyEmployeeInfoRspItemMdl> EmployeeList { get; set; }
}
