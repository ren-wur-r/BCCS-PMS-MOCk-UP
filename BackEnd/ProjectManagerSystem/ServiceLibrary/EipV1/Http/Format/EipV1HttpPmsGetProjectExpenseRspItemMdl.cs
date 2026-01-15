using System;
using System.Text.Json.Serialization;

namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-PMS-查詢專案費用-回應項目模型</summary>
public class EipV1HttpPmsGetProjectExpenseRspItemMdl
{
    /// <summary>電子簽核狀況</summary>
    public string ApprovalStatus { get; set; }

    /// <summary>申請人員名稱</summary>
    public string Applicant { get; set; }

    /// <summary>費用發生日期</summary>
    public DateTimeOffset ExpenseDate { get; set; }

    /// <summary>費用事由</summary>
    public string ExpenseReason { get; set; }

    /// <summary>參與人員名單</summary>
    public string Participants { get; set; }

    /// <summary>費用金額</summary>
    public decimal ExpenseAmount { get; set; }
}