using DataModelLibrary.Database.AtomEmployeePipelineBill;

namespace ServiceLibrary.Core.Employee.DB.PipelineBill.Format;

/// <summary>核心-員工-商機發票紀錄-資料庫-更新-請求模型</summary>
public class CoEmpPplBllDbUpdateReqMdl
{
    /// <summary>商機發票紀錄ID</summary>
    public int EmployeePipelineBillID { get; set; }

    /// <summary>發票號碼</summary>
    public string EmployeePipelineBillNumber { get; set; }

    /// <summary>發票狀態</summary>
    public DbAtomEmployeePipelineBillStatusEnum? EmployeePipelineBillStatus { get; set; }

    /// <summary>備註</summary>
    public string EmployeePipelineBillRemark { get; set; }
}