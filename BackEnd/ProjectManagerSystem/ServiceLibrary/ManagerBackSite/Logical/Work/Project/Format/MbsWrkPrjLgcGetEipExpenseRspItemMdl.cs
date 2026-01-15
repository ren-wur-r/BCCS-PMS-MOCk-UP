using System;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-取得Eip專案支出-回應項目模型</summary>
public class MbsWrkPrjLgcGetEipExpenseRspItemMdl
{
    /// <summary>簽核狀態</summary>
    public string ProjectExpenseApprovalStatus { get; set; }

    /// <summary>申請人員</summary>
    public string ProjectExpenseApplicant { get; set; }

    /// <summary>日期</summary>
    public DateTimeOffset ProjectExpenseDate { get; set; }

    /// <summary>事由</summary>
    public string ProjectExpenseReason { get; set; }

    /// <summary>參與人員</summary>
    public string ProjectExpenseParticipants { get; set; }

    /// <summary>金額</summary>
    public decimal ProjectExpenseAmount { get; set; }
}