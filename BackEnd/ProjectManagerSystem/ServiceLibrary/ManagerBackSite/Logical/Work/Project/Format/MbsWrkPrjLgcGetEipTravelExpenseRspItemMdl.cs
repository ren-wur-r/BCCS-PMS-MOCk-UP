using System;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-取得Eip專案旅費-回應項目模型</summary>
public class MbsWrkPrjLgcGetEipTravelExpenseRspItemMdl
{
    /// <summary>簽核狀態</summary>
    public string ProjectTravelExpenseApprovalStatus { get; set; }

    /// <summary>申請人員</summary>
    public string ProjectTravelExpenseApplicant { get; set; }

    /// <summary>日期</summary>
    public DateTimeOffset ProjectTravelExpenseTravelDate { get; set; }

    /// <summary>起訖地點</summary>
    public string ProjectTravelExpenseTravelRoute { get; set; }

    /// <summary>工作記要</summary>
    public string ProjectTravelExpenseWorkDescription { get; set; }

    /// <summary>交通工具</summary>
    public string ProjectTravelExpenseTransportationTool { get; set; }

    /// <summary>交通費金額</summary>
    public decimal? ProjectTravelExpenseTransportationAmount { get; set; }

    /// <summary>里程</summary>
    public decimal? ProjectTravelExpenseMileage { get; set; }

    /// <summary>膳宿費</summary>
    public decimal? ProjectTravelExpenseAccommodationAmount { get; set; }

    /// <summary>特別費事由</summary>
    public string ProjectTravelExpenseSpecialExpenseReason { get; set; }

    /// <summary>特別費金額</summary>
    public decimal? ProjectTravelExpenseSpecialExpenseAmount { get; set; }

    /// <summary>合計</summary>
    public decimal? ProjectTravelExpenseTotalAmount { get; set; }
}