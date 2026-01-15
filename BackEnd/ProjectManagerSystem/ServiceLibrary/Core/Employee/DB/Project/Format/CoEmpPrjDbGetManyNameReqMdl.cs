using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-取得多筆[名稱]-請求模型</summary>
public class CoEmpPrjDbGetManyNameReqMdl
{
    /// <summary>員工-專案-ID列表</summary>
    public List<int> EmployeeProjectIdList { get; set; }
}