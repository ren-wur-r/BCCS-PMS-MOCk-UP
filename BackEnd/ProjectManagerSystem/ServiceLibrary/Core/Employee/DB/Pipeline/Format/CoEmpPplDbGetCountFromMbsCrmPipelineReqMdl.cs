using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-商機管理]-請求模型</summary>
public class CoEmpPplDbGetCountFromMbsCrmPipelineReqMdl
{
    /// <summary>員工商機-業務員工ID(依權限判斷,有值則是self)</summary>
    public int? EmployeePipelineSalerEmployeeID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum? AtomPipelineStatus { get; set; }

    /// <summary>管理者公司名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>窗口姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    public string ManagerContacterEmail { get; set; }
}