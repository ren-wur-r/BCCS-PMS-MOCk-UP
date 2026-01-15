using System;
using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-PMS-查詢專案旅費-回應項目模型</summary>
public class EipV1HttpPmsGetProjectTravelExpenseRspItemMdl
{
    /// <summary>簽核狀態</summary>
    public string ApprovalStatus { get; set; }

    /// <summary>申請人員</summary>
    public string Applicant { get; set; }

    /// <summary>日期</summary>
    public DateTimeOffset TravelDate { get; set; }

    /// <summary>起訖地點</summary>
    public string TravelRoute { get; set; }

    /// <summary>工作記要</summary>
    public string WorkDescription { get; set; }

    /// <summary>交通工具</summary
    public string TransportationTool { get; set; }

    /// <summary>交通費金額</summary>
    public decimal? TransportationAmount { get; set; }

    /// <summary>里程</summary>
    public decimal? Mileage { get; set; }

    /// <summary>膳宿費</summary>
    public decimal? AccommodationAmount { get; set; }

    /// <summary>特別費事由</summary>
    public string SpecialExpenseReason { get; set; }

    /// <summary>特別費金額</summary>
    public decimal? SpecialExpenseAmount { get; set; }

    /// <summary>合計</summary>
    public decimal TotalAmount { get; set; }
}