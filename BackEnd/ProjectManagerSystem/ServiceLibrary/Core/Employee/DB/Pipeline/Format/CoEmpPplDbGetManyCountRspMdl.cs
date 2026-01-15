using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得[筆數]從[商機狀態]-回應模型</summary>
public class CoEmpPplDbGetManyCountRspMdl
{
    /// <summary>商機列表</summary>
    public List<CoEmpPplDbGetManyCountRspItemMdl> EmployeePipelineList { get; set; }
}