using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrCmpSubKdDbGetManyNameRspMdl
{
    /// <summary>公司子分類列表</summary>
    public List<CoMgrCmpSubKdDbGetManyByNameRspItemMdl> ManagerCompanySubKindList { get; set; }
}