namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工資訊-項目-回應模型</summary>
public class MbsBasicLgcGetManyEmployeeInfoRspItemMdl
{
    /// <summary>管理者-地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工-名稱</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool EmployeeIsEnable { get; set; }
}
