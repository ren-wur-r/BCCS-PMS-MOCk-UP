using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-資料庫-取得多筆ID-回應模型</summary>
public class CoEmpPcDbGetManyIdRspMdl
{
    /// <summary>員工專案契約列表</summary>
    public List<CoEmpPcDbGetManyIdRspItemMdl> EmployeeProjectContractList { get; set; }
}