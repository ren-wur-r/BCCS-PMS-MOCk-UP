using System;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-邏輯-取得儀表板-回應項目模型</summary> 
public class MbsWrkPrjLgcGetDashboardRspItemMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>管理者公司名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案起始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }
}
