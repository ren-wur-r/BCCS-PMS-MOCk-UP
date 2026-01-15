using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-新增多筆-請求模型</summary>
public class CoEmpPplSlrTrkDbAddManyReqMdl
{
    /// <summary>商機業務開發紀錄列表</summary>
    public List<CoEmpPplSlrTrkDbAddManyReqItemMdl> EmployeePipelineSalerTrackingList { get; set; }
}
