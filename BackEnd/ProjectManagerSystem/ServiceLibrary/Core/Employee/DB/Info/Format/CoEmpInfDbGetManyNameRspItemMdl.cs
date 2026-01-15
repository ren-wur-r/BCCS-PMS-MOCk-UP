namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[名稱]-回應項目模型</summary>
public class CoEmpInfDbGetManyNameRspItemMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工名稱</summary>
    public string EmployeeName { get; set; }
}
