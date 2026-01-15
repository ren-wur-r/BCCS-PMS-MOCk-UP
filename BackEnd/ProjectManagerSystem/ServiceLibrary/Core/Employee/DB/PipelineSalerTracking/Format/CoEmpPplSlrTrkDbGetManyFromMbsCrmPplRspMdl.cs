using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆[管理者後台-CRM-商機管理]-回應模型</summary>
public class CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspMdl
{
    /// <summary>商機業務開發紀錄列表 </summary>
    public List<CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspItemMdl> EmployeePipelineSalerTrackingList { get; set; }
}