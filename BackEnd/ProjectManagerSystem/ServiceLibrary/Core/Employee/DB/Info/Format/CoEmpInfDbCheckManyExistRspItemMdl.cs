namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-檢查多筆存在-回應項目模型</summary>
public class CoEmpInfDbCheckManyExistRspItemMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>是否存在</summary>
    public bool IsExist { get; set; }
}
