using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrCmpMainKdDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者公司主分類-列表</summary>
    public List<CoMgrCmpMainKdDbGetManyFromMbsBscRspItemMdl> ManagerCompanyMainKindList { get; set; }
}