using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-活動管理]-回應模型</summary>
public class CoMgrActivityDbGetManyFromMbsCrmActRspMdl
{
    /// <summary>管理者活動列表</summary>
    public List<CoMgrActivityDbGetManyFromMbsMktActRspItemMdl> ManagerActivityList { get; set; }
}