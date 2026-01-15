using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Permission.Format;

/// <summary>核心-員工-目錄權限-資料庫-新增多筆-請求模型</summary>
public class CoEmpPmnDbAddManyReqMdl
{
    /// <summary>員工權限-列表</summary>
    public List<CoEmpPmnDbAddManyReqItemMdl> EmployeePermissionList { get; set; }
}
