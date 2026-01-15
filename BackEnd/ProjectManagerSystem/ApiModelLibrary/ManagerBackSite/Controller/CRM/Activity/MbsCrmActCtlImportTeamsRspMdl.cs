using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-匯入Teams-回應模型</summary>
public class MbsCrmActCtlImportTeamsRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>錯誤資料列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmActCtlImportTeamsRspItemMdl> ErrorDataList { get; set; }
}
