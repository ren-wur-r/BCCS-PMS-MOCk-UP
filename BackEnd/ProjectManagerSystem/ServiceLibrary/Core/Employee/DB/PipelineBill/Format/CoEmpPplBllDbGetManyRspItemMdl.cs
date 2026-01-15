using System;
using DataModelLibrary.Database.AtomEmployeePipelineBill;

namespace ServiceLibrary.Core.Employee.DB.PipelineBill.Format;

/// <summary>核心-員工-商機發票紀錄-資料庫-取得多筆-回應項目</summary>
public class CoEmpPplBllDbGetManyRspItemMdl
{
    /// <summary>商機發票紀錄ID</summary>
    public int EmployeePipelineBillID { get; set; }

    /// <summary>發票期數</summary>
    public short EmployeePipelineBillPeriodNumber { get; set; }

    /// <summary>發票日期</summary>
    public DateTimeOffset EmployeePipelineBillBillTime { get; set; }

    /// <summary>發票號碼</summary>
    public string EmployeePipelineBillBillNumber { get; set; }

    /// <summary>未稅發票金額</summary>
    public decimal EmployeePipelineBillNoTaxAmount { get; set; }

    /// <summary>備註</summary>
    public string EmployeePipelineBillRemark { get; set; }

    /// <summary>發票狀態</summary>
    public DbAtomEmployeePipelineBillStatusEnum EmployeePipelineBillStatus { get; set; }

}