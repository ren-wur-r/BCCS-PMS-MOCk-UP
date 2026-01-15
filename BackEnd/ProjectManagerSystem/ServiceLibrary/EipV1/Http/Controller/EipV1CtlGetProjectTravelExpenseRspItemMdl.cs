using System;
using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Controller;

/// <summary>EipV1-控制器-查詢專案旅費-回應項目模型</summary>
public class EipV1CtlGetProjectTravelExpenseRspItemMdl
{
    /// <summary>簽核狀態</summary>
    [JsonPropertyName("簽核狀態")]
    public string ApprovalStatus { get; set; }

    /// <summary>申請人員</summary>
    [JsonPropertyName("申請人員")]
    public string Applicant { get; set; }

    /// <summary>日期</summary>
    [JsonPropertyName("日期")]
    public DateTime TravelDate { get; set; }

    /// <summary>起訖地點</summary>
    [JsonPropertyName("起訖地點")]
    public string TravelRoute { get; set; }

    /// <summary>工作記要</summary>
    [JsonPropertyName("工作記要")]
    public string WorkDescription { get; set; }

    /// <summary>交通工具</summary>
    [JsonPropertyName("交通工具")]
    public string TransportationTool { get; set; }

    /// <summary>交通費金額</summary>
    [JsonPropertyName("交通費金額")]
    public decimal? TransportationAmount { get; set; }

    /// <summary>里程</summary>
    [JsonPropertyName("里程")]
    public decimal? Mileage { get; set; }

    /// <summary>膳宿費</summary>
    [JsonPropertyName("膳宿費")]
    public decimal? AccommodationAmount { get; set; }

    /// <summary>特別費事由</summary>
    [JsonPropertyName("特別費事由")]
    public string SpecialExpenseReason { get; set; }

    /// <summary>特別費金額</summary>
    [JsonPropertyName("特別費金額")]
    public decimal? SpecialExpenseAmount { get; set; }

    /// <summary>合計</summary>
    [JsonPropertyName("合計")]
    public decimal TotalAmount { get; set; }
}