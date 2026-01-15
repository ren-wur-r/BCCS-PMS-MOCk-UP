using System;
using DataModelLibrary.Database.AtomEmployeePipelineBill;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機發票紀錄-回應模型</summary>
public class MbsCrmPplLgcGetEmployeePipelineBillRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機發票紀錄ID</summary>
    public int EmployeePipelineBillID { get; set; }

    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>發票期數</summary>
    public short EmployeePipelineBillPeriodNumber { get; set; }

    /// <summary>發票日期</summary>
    public DateTimeOffset EmployeePipelineBillBillTime { get; set; }

    /// <summary>未稅發票金額</summary>
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>發票號碼</summary>
    public string EmployeePipelineBillBillNumber { get; set; }

    /// <summary>發票狀態</summary>
    public DbAtomEmployeePipelineBillStatusEnum EmployeePipelineBillStatus { get; set; }

    /// <summary>備註</summary>
    public string EmployeePipelineBillRemark { get; set; }
}