using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動產品-回應模型</summary>
public class MbsCrmActLgcGetManyActivityProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動產品列表</summary>
    public List<MbsCrmActLgcGetManyActivityProductRspItemMdl> ManagerActivityProductList { get; set; }
}
