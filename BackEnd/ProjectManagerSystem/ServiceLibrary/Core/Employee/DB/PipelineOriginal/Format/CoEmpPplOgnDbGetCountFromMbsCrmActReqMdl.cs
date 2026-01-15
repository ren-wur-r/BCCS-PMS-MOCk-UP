using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]-請求模型</summary>
public class CoEmpPplOgnDbGetCountFromMbsCrmActReqMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum? AtomPipelineStatus { get; set; }

    /// <summary>商機原始資料-管理者公司名稱</summary>
    public string EmployeePipelineOriginalManagerCompanyName { get; set; }

    /// <summary>商機原始資料-窗口姓名</summary>
    public string EmployeePipelineOriginalManagerContacterName { get; set; }

    /// <summary>商機原始資料-窗口Email</summary>
    public string EmployeePipelineOriginalManagerContacterEmail { get; set; }
}