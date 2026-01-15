namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得多筆過往活動從[窗口Email]-請求模型</summary>
public class CoMgrActivityDbGetManyPastActivityFromContacterEmailReqMdl
{
    /// <summary>商機原始資料-窗口Email</summary>
    public string EmployeePipelineOriginalManagerContacterEmail { get; set; }

    /// <summary>要過濾的活動ID</summary>
    public int? FilterManagerActivityID { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}