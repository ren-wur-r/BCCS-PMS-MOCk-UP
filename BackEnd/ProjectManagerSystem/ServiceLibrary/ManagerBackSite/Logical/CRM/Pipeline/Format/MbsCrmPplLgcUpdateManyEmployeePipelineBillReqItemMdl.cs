using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-更新多筆商機發票紀錄-請求項目模型</summary>
public class MbsCrmPplLgcUpdateManyEmployeePipelineBillReqItemMdl : MbsLgcBaseReqMdl
{
    /// <summary>發票期數</summary>
    public short EmployeePipelineBillPeriodNumber { get; set; }

    /// <summary>發票日期</summary>
    public DateTimeOffset EmployeePipelineBillBillTime { get; set; }

    /// <summary>未稅發票金額</summary>
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>備註</summary>
    public string EmployeePipelineBillRemark { get; set; }

}