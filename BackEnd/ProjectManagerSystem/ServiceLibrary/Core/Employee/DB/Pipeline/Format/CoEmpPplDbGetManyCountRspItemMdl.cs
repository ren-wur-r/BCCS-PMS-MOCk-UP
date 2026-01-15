namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得多筆[筆數]-項目-回應模型</summary>
public class CoEmpPplDbGetManyCountRspItemMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>筆數</summary>
    public int Count { get; set; }
}