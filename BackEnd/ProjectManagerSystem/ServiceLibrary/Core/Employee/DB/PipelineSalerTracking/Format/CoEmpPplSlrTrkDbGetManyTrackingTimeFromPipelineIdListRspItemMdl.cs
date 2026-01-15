using System;
using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆開發時間從商機ID列表-項目-回應模型</summary>
public class CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspItemMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>開發時間列表</summary>
    public List<DateTimeOffset> TrackingTimeList { get; set; }
}
