using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-電銷管理]-回應模型</summary>
public class CoMgrActivityDbGetManyFromMbsCrmPhnRspMdl
{
    /// <summary>活動列表</summary>
    public List<CoMgrActivityDbGetManyFromMbsCrmPhnRspItemMdl> ManagerActivityList { get; set; }
}