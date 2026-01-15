using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆公司主分類-回應模型</summary>
public class MbsBscLgcGetManyCompanyMainKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>公司主分類列表</summary>
    public List<MbsBscLgcGetManyCompanyMainKindRspItemMdl> ManagerCompanyMainKindList { get; set; }
}