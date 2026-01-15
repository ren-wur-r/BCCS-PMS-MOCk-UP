using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆-回應模型</summary>
public class CoEmpPplSlrTrkDbGetManyRspMdl
{
    /// <summary>商機業務開發紀錄列表</summary>
    public List<CoEmpPplSlrTrkDbGetManyRspItemMdl> EmployeePipelineSalerTrackingList { get; set; }
}
