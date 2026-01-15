using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrCmpDbGetManyNameRspMdl
{
    /// <summary>管理者公司列表</summary>
    public List<CoMgrCmpDbGetManyNameRspItemMdl> ManagerCompanyList { get; set; }
}