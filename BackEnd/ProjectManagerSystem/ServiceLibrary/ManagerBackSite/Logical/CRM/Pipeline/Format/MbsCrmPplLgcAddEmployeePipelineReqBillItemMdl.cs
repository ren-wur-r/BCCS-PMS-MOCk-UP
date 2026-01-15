using System;
using DataModelLibrary.Database.AtomEmployeePipelineBill;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機-發票項目-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineReqBillItemMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機發票期數</summary>
    public short EmployeePipelineBillPeriodNumber { get; set; }

    /// <summary>商機發票日期</summary>
    public DateTimeOffset EmployeePipelineBillBillTime { get; set; }

    /// <summary>商機發票未稅金額</summary>
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>商機發票備註</summary>
    public string EmployeePipelineBillRemark { get; set; }

    /// <summary>商機發票狀態</summary>
    public DbAtomEmployeePipelineBillStatusEnum EmployeePipelineBillStatus { get; set; }
}
