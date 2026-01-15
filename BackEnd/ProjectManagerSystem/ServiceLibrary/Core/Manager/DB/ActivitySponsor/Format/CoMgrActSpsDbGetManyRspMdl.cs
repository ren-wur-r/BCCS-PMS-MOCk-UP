using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-取得多筆-回應</summary>
public class CoMgrActSpsDbGetManyRspMdl
{
    /// <summary>管理者活動贊助商列表</summary>
    public List<CoMgrActSpsDbGetManyRspItemMdl> ManagerActivitySponsorList { get; set; }
}
