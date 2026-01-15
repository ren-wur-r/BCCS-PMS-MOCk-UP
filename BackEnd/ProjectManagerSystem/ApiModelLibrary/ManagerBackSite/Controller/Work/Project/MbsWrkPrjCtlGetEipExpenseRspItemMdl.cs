using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得Eip專案支出-回應項目模型</summary>
public class MbsWrkPrjCtlGetEipExpenseRspItemMdl
{
    /// <summary>簽核狀態</summary>
    [JsonPropertyName("a")]
    public string ProjectExpenseApprovalStatus { get; set; }

    /// <summary>申請人員</summary>
    [JsonPropertyName("b")]
    public string ProjectExpenseApplicant { get; set; }

    /// <summary>日期</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset ProjectExpenseDate { get; set; }

    /// <summary>事由</summary>
    [JsonPropertyName("d")]
    public string ProjectExpenseReason { get; set; }

    /// <summary>參與人員</summary>
    [JsonPropertyName("e")]
    public string ProjectExpenseParticipants { get; set; }

    /// <summary>金額</summary>
    [JsonPropertyName("f")]
    public decimal ProjectExpenseAmount { get; set; }
}