namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得[筆數]過往活動從[窗口Email]-請求模型</summary>
public class CoMgrActivityDbGetCountPastActivityFromContacterEmailReqMdl
{
    /// <summary>商機原始資料-窗口Email</summary>
    public string EmployeePipelineOriginalManagerContacterEmail { get; set; }

    /// <summary>要過濾的活動ID</summary>
    public int? FilterManagerActivityID { get; set; }
}