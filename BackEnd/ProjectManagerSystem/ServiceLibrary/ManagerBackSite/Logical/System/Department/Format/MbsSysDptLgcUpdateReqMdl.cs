using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;

/// <summary>管理者後台-系統設定-部門-更新-請求模型</summary>
public class MbsSysDptLgcUpdateReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-部門-是否啟用</summary>
    public bool? ManagerDepartmentIsEnable { get; set; }
}