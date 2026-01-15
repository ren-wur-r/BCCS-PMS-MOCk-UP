using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-匯入eDM-回應模型</summary>
public class MbsCrmActLgcImportEdmRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>錯誤資料列表</summary>
    public List<MbsCrmActLgcImportEdmRspItemMdl> ErrorDataList { get; set; }
}