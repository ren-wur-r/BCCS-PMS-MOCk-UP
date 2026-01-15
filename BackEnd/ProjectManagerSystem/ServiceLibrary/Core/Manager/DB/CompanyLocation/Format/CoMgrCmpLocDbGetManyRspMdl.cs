using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-取得多筆-回應模型</summary>
public class CoMgrCmpLocDbGetManyRspMdl
{
    /// <summary>管理者公司營業地點清單</summary>
    public List<CoMgrCmpLocDbGetManyRspItemMdl> ManagerCompanyLocationList { get; set; }
}