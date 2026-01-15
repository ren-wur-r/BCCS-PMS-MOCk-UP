using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Controller;

/// <summary>EipV1-控制器-查詢專案費用-回應項目模型</summary>
public class EipV1CtlGetProjectExpenseRspItemMdl
{
    /// <summary>電子簽核狀況</summary>
    [JsonPropertyName("簽核狀態")]
    public string ApprovalStatus { get; set; }

    /// <summary>申請人員名稱</summary>
    [JsonPropertyName("申請人員")]
    public string Applicant { get; set; }

    /// <summary>費用發生日期</summary>
    [JsonPropertyName("日期")]
    public string ExpenseDate { get; set; }

    /// <summary>費用事由</summary>
    [JsonPropertyName("事由")]
    public string ExpenseReason { get; set; }

    /// <summary>參與人員名單</summary>
    [JsonPropertyName("參與人員")]
    public string Participants { get; set; }

    /// <summary>費用金額</summary>
    [JsonPropertyName("金額")]
    public decimal ExpenseAmount { get; set; }
}