using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-新增多筆-請求模型</summary>
public class CoMgrCmpLocDbAddManyReqMdl
{
    /// <summary>公司營業地點列表</summary>
    public List<CoMgrCmpLocDbAddManyReqItemMdl> ManagerCompanyLocationList { get; set; }
}
