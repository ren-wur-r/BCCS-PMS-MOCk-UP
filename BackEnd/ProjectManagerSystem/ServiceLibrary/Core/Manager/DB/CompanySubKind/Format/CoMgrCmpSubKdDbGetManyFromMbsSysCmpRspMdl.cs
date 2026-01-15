using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]-回應模型</summary>
public class CoMgrCmpSubKdDbGetManyFromMbsSysCmpRspMdl
{
    /// <summary>管理者公司子分類-列表</summary>
    public List<CoMgrCmpSubKdDbGetManyFrommbsSysCmpRspItemMdl> ManagerCompanySubKindList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}