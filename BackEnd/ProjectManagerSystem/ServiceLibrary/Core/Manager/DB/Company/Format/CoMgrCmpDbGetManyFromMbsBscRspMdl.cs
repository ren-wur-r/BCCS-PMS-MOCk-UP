using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrCmpDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者公司-列表</summary>
    public List<CoMgrCmpDbGetManyFromMbsBscRspItemMdl> ManagerCompanyList { get; set; }
}