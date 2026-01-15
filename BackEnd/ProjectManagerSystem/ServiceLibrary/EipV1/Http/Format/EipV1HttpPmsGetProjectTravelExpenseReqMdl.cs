using System;

namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-PMS-查詢專案旅費-請求模型</summary>
public class EipV1HttpPmsGetProjectTravelExpenseReqMdl
{
    /// <summary>專案代碼</summary>
    public string ProjectCode { get; set; }

    /// <summary>專案名稱</summary>
    public string ProjectName { get; set; }

    /// <summary>開始日期</summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>結束日期</summary>
    public DateTimeOffset EndDate { get; set; }
}