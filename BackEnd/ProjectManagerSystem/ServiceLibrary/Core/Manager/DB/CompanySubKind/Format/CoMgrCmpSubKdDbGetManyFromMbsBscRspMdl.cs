using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrCmpSubKdDbGetManyFromMbsBscRspMdl
{
    /// <summary>公司子分類列表</summary>
    public List<CoMgrCmpSubKdDbGetManyFromMbsBscRspItemMdl> ManagerCompanySubKindList { get; set; }
}