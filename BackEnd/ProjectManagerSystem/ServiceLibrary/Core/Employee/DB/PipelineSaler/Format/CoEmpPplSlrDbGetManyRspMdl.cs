using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

/// <summary>核心-員工-商機業務-資料庫-取得多筆-回應</summary>
public class CoEmpPplSlrDbGetManyRspMdl
{
    /// <summary>商機業務清單</summary>
    public List<CoEmpPplSlrDbGetManyRspItemMdl> EmployeePipelineSalerList { get; set; }
}
