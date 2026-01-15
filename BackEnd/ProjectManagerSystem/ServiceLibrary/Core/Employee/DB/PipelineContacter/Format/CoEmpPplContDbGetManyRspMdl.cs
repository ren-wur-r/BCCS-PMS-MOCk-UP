using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

/// <summary>核心-員工-商機窗口-資料庫-取得多筆-回應</summary>
public class CoEmpPplContDbGetManyRspMdl
{
    /// <summary>商機窗口清單</summary>
    public List<CoEmpPplContDbGetManyRspItemMdl> EmployeePipelineContacterList { get; set; }
}
