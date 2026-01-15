namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得[管理者後台-CRM-商機管理]-請求模型</summary>
public class CoEmpPplDbGetFromMbsCrmPipelineReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機-業務員工ID</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }
}