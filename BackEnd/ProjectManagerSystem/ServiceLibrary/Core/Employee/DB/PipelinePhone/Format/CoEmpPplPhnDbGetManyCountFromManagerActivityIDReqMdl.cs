using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;

/// <summary>核心-員工-商機電銷-資料庫-取得多筆[筆數]-請求模型</summary>
public class CoEmpPplPhnDbGetManyCountFromManagerActivityIDReqMdl
{
    /// <summary>商機電銷ID列表</summary>
    public List<int> ManagerActivityIDList { get; set; }
}