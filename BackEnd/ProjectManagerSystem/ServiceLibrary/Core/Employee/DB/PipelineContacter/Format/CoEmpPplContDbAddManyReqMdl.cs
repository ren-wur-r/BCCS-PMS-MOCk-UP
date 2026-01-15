using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

/// <summary>核心-員工-商機窗口-資料庫-新增多筆-請求模型</summary>
public class CoEmpPplContDbAddManyReqMdl
{
    /// <summary>商機窗口列表</summary>
    public List<CoEmpPplContDbAddManyReqItemMdl> EmployeePipelineContacterList { get; set; }
}
