using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-資料庫-更新多筆-請求模型</summary>
public class CoEmpPcDbUpdateManyReqMdl
{
    /// <summary>員工專案契約ID列表</summary>
    public List<int> EmployeeProjectContractIdList { get; set; }

    /// <summary>員工專案契約-是否最新</summary>
    public bool? EmployeeProjectContractIsNewest { get; set; }
}