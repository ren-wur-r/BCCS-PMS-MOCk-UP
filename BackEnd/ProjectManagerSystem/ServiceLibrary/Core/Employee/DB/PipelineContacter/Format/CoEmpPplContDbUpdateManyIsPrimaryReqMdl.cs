namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

/// <summary>核心-員工-商機窗口-資料庫-更新多筆[是否為主要商機窗口]-請求模型</summary>
public class CoEmpPplContDbUpdateManyIsPrimaryReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>是否為主要商機窗口</summary>
    public bool EmployeePipelineContacterIsPrimary { get; set; }
}