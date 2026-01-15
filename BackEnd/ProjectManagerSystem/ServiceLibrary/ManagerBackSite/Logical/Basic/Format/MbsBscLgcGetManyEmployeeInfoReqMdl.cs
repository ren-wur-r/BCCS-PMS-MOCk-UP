using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工資訊-請求模型</summary>
public class MbsBscLgcGetManyEmployeeInfoReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-地區-ID</summary>
    public int? ManagerRegionID { get; set; }

    /// <summary>管理者-部門-ID</summary>
    public int? ManagerDepartmentID { get; set; }

    /// <summary>管理者-角色-ID</summary>
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-姓名(模糊查詢)</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool? EmployeeIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}
