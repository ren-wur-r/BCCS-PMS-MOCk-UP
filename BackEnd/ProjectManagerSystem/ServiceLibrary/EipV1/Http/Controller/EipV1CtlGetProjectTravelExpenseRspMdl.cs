using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Controller;

/// <summary>EipV1-控制器-查詢專案旅費-回應模型</summary>
public class EipV1CtlGetProjectTravelExpenseRspMdl
{
    /// <summary>是否成功</summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>訊息</summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>總筆數</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>旅費記錄列表</summary>
    [JsonPropertyName("data")]
    public List<EipV1CtlGetProjectTravelExpenseRspItemMdl> Data { get; set; }
}