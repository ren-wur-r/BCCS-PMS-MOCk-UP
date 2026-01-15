using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-取得多筆員工ID-請求模型</summary>
public class CoEmpPmDbGetManyEmployeeIdReqMdl
{
    /// <summary>員工專案ID-列表</summary>
    public List<int> EmployeeProjectIdList { get; set; }
}