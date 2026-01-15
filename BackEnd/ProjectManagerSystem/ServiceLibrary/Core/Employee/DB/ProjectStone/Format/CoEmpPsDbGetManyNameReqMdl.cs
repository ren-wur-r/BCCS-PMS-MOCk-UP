using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-取得多筆[名稱]-請求模型</summary>
public class CoEmpPsDbGetManyNameReqMdl
{
    /// <summary>員工-專案里程碑-ID-列表</summary>
    public List<int> EmployeeProjectStoneIdList { get; set; }
}