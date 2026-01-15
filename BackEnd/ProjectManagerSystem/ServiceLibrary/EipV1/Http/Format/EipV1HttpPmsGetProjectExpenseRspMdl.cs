using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-PMS-查詢專案費用-回應模型</summary>
public class EipV1HttpPmsGetProjectExpenseRspMdl
{
    /// <summary>是否成功</summary>
    public bool Success { get; set; }

    /// <summary>訊息</summary>
    public string Message { get; set; }

    /// <summary>總筆數</summary>
    public int Total { get; set; }

    /// <summary>費用紀錄列表</summary>
    public List<EipV1HttpPmsGetProjectExpenseRspItemMdl> DataList { get; set; }
}