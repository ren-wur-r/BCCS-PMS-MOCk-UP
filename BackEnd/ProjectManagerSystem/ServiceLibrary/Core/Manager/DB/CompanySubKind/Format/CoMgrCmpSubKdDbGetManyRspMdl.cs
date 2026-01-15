using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆-回應模型</summary>
public class CoMgrCmpSubKdDbGetManyRspMdl
{
    /// <summary>管理者公司子分類-列表</summary>
    public List<CoMgrCmpSubKdDbGetManyRspItemMdl> ManagerCompanySubKindList { get; set; }
}