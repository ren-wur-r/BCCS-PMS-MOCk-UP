using DataModelLibrary.Database.AtomMenu;

namespace ServiceLibrary.Core.Employee.DB.Permission.Format;

/// <summary>核心-員工-目錄權限-資料庫-取得-請求模型</summary>
public class CoEmpPmnDbGetReqMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>原子-目錄</summary>
    public DbAtomMenuEnum AtomMenu { get; set; }
}
