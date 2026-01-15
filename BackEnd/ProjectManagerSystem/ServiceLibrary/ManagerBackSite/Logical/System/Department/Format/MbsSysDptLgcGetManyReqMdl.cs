using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;

/// <summary>管理者後台-系統設定-部門-取得多筆-請求模型</summary>
public class MbsSysDptLgcGetManyReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-部門-是否啟用</summary>
    public bool? ManagerDepartmentIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; } = 10;
}