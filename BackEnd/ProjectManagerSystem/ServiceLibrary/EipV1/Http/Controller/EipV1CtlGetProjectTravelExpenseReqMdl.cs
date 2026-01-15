using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Controller;

/// <summary>EipV1-控制器-查詢專案旅費-請求模型</summary>
public class EipV1CtlGetProjectTravelExpenseReqMdl
{
    /// <summary>專案代碼</summary>
    [Required]
    [JsonPropertyName("projectCode")]
    public string ProjectCode { get; set; }

    /// <summary>專案名稱</summary>
    [Required]
    [JsonPropertyName("projectName")]
    public string ProjectName { get; set; }

    /// <summary>開始日期</summary>
    [Required]
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    /// <summary>結束日期</summary>
    [Required]
    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }
}