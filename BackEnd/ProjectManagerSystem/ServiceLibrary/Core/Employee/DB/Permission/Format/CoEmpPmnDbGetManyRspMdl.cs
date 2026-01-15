using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Permission.Format;

/// <summary>核心-員工-目錄權限-資料庫-取得多筆-回應模型</summary>
public class CoEmpPmnDbGetManyRspMdl
{
    /// <summary>員工目錄權限-列表</summary>
    public List<CoEmpPmnDbGetManyRspItemMdl> EmployeePermissionList { get; set; }
}
