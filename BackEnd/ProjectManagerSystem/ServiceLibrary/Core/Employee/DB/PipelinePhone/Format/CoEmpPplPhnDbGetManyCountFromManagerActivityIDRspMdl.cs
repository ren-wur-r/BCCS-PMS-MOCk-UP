using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;

/// <summary>核心-員工-商機電銷-資料庫-取得多筆[筆數]-回應模型</summary>
public class CoEmpPplPhnDbGetManyCountFromManagerActivityIDRspMdl
{
    /// <summary>商機電銷列表</summary>
    public List<CoEmpPplPhnDbGetManyCountRspItemMdl> EmployeePipelinePhoneList { get; set; }
}