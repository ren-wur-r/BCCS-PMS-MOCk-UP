using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司子分類-回應模型</summary>
public class MbsSysCmpLgcGetManyCompanySubKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>公司子分類列表</summary>
    public List<MbsSysCmpLgcGetManyCompanySubKindRspItemMdl> ManagerCompanySubKindList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}