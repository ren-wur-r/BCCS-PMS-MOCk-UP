namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

/// <summary>核心-員工-商機窗口-資料庫-取得-回應</summary>
public class CoEmpPplContDbGetRspMdl
{
    /// <summary>商機窗口ID</summary>
    public int EmployeePipelineContacterID { get; set; }

    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>是否為主要商機窗口</summary>
    public bool EmployeePipelineContacterIsPrimary { get; set; }
}
