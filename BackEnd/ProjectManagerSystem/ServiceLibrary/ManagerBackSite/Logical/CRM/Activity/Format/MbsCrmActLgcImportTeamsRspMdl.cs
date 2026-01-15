using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-匯入Teams-回應模型</summary>
public class MbsCrmActLgcImportTeamsRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>錯誤資料列表</summary>
    public List<MbsCrmActLgcImportTeamsRspItemMdl> ErrorDataList { get; set; }
}