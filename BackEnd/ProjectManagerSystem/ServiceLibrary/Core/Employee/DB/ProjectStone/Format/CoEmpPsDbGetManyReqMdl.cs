using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-資料庫-取得多筆-請求模型</summary>
public class CoEmpPsDbGetManyReqMdl
{
    /// <summary>員工-專案-ID-列表</summary>
    public List<int> EmployeeProjectIdList { get; set; }
}