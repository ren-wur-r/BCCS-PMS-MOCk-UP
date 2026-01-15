using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;

/// <summary>核心-員工-商機電銷-資料庫-取得多筆-回應</summary>
public class CoEmpPplPhnDbGetManyRspMdl
{
    /// <summary>商機電銷紀錄清單</summary>
    public List<CoEmpPplPhnDbGetManyRspItemMdl> EmployeePipelinePhoneList { get; set; }
}
