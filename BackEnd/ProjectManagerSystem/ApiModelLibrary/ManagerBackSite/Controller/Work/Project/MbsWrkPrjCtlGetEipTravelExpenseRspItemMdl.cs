using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得Eip專案旅費-回應項目模型</summary>
public class MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl
{
    /// <summary>簽核狀態</summary>
    [JsonPropertyName("a")]
    public string ProjectTravelExpenseApprovalStatus { get; set; }

    /// <summary>申請人員</summary>
    [JsonPropertyName("b")]
    public string ProjectTravelExpenseApplicant { get; set; }

    /// <summary>日期</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset ProjectTravelExpenseTravelDate { get; set; }

    /// <summary>起訖地點</summary>
    [JsonPropertyName("d")]
    public string ProjectTravelExpenseTravelRoute { get; set; }

    /// <summary>工作記要</summary>
    [JsonPropertyName("e")]
    public string ProjectTravelExpenseWorkDescription { get; set; }

    /// <summary>交通工具</summary>
    [JsonPropertyName("f")]
    public string ProjectTravelExpenseTransportationTool { get; set; }

    /// <summary>交通費金額</summary>
    [JsonPropertyName("g")]
    public decimal? ProjectTravelExpenseTransportationAmount { get; set; }

    /// <summary>里程</summary>
    [JsonPropertyName("h")]
    public decimal? ProjectTravelExpenseMileage { get; set; }

    /// <summary>膳宿費</summary>
    [JsonPropertyName("i")]
    public decimal? ProjectTravelExpenseAccommodationAmount { get; set; }

    /// <summary>特別費事由</summary>
    [JsonPropertyName("j")]
    public string ProjectTravelExpenseSpecialExpenseReason { get; set; }

    /// <summary>特別費金額</summary>
    [JsonPropertyName("k")]
    public decimal? ProjectTravelExpenseSpecialExpenseAmount { get; set; }

    /// <summary>合計</summary>
    [JsonPropertyName("l")]
    public decimal? ProjectTravelExpenseTotalAmount { get; set; }
}