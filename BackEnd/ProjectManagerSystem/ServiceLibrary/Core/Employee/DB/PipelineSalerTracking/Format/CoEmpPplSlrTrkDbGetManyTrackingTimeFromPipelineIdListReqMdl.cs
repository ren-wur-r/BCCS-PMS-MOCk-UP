using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆開發時間從商機ID列表-請求模型</summary>
public class CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListReqMdl
{
    /// <summary>商機ID列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }
}
