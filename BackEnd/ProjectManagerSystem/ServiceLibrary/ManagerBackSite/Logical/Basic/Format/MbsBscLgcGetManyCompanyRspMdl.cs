using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆管理者公司-回應模型</summary>
public class MbsBscLgcGetManyCompanyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者公司-列表</summary>
    public List<MbsBscLgcGetManyCompanyRspItemMdl> ManagerCompanyList { get; set; }
}
