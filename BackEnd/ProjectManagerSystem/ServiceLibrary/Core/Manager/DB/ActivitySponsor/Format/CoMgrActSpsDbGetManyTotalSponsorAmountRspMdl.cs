using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-取得多筆總贊助金額-回應模型</summary>
public class CoMgrActSpsDbGetManyTotalSponsorAmountRspMdl
{
    /// <summary>管理者活動贊助商列表</summary>
    public List<CoMgrActSpsDbGetManyTotalSponsorAmountRspItemMdl> ManagerActivitySponsorList { get; set; }
}