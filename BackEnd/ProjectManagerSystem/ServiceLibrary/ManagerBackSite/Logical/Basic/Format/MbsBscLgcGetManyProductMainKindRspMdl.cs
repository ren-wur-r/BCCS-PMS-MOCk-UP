using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆管理者產品主分類-回應模型</summary>
public class MbsBscLgcGetManyProductMainKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-產品主分類-列表</summary>
    public List<MbsBscLgcGetManyProductMainKindRspItemMdl> ManagerProductMainKindList { get; set; }
}