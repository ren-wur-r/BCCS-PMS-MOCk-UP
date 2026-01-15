using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得多筆過往活動從[窗口Email]-回應模型</summary>
public class CoMgrActivityDbGetManyPastActivityFromContacterEmailRspMdl
{
    /// <summary>活動列表</summary>
    public List<CoMgrActivityDbGetManyFromEmployeePipelineRspItemMdl> ManagerActivityList { get; set; }
}