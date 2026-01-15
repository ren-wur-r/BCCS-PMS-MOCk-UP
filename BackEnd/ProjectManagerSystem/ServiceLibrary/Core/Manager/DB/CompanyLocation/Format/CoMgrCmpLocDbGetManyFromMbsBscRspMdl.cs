using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrCmpLocDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者公司營業地點列表</summary>
    public List<CoMgrCmpLocDbGetManyFromMbsBscRspItemMdl> ManagerCompanyLocationList { get; set; }
}