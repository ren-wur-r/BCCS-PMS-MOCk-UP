using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-新增多筆-請求</summary>
public class CoMgrActSpsDbAddManyReqMdl
{
    /// <summary>管理者活動贊助商列表</summary>
    public List<CoMgrActSpsDbAddManyReqItemMdl> ManagerActivitySponsorList { get; set; }
}
