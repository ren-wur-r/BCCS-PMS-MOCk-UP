using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆-回應模型</summary>
public class CoMgrCmpDbGetManyRspMdl
{
    /// <summary>管理者公司清單</summary>
    public List<CoMgrCmpDbGetManyRspItemMdl> ManagerCompanyList { get; set; }
}