using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動名單-回應項目模型</summary>
public class MbsCrmActLgcGetManyActivityEmployeePipelineRspItemMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>管理公司名稱</summary>
    public string EmployeePipelineOriginalManagerCompanyName { get; set; }

    /// <summary>窗口部門</summary>
    public string EmployeePipelineOriginalManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    public string EmployeePipelineOriginalManagerContacterJobTitle { get; set; }

    /// <summary>窗口姓名</summary>
    public string EmployeePipelineOriginalManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    public string EmployeePipelineOriginalManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    public string EmployeePipelineOriginalManagerContacterPhone { get; set; }

    /// <summary>窗口電話</summary>
    public string EmployeePipelineOriginalManagerContacterTelephone { get; set; }
}
