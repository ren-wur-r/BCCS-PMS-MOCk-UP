using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-取得多筆-回應模型</summary>
public class CoEmpPcDbGetManyRspMdl
{
    /// <summary>員工專案契約列表</summary>
    public List<CoEmpPcDbGetManyRspItemMdl> EmployeeProjectContractList { get; set; }
}