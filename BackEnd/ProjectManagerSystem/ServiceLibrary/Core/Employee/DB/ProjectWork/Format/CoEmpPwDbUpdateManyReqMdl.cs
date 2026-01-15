using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-更新多筆-請求模型</summary>
public class CoEmpPwDbUpdateManyReqMdl
{
    /// <summary>員工專案工作計劃書ID列表</summary>
    public List<int> EmployeeProjectWorkIdList { get; set; }

    /// <summary>員工專案工作計劃書-是否最新</summary>
    public bool? EmployeeProjectWorkIsNewest { get; set; }
}