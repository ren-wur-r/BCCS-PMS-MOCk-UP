namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得多筆從[管理者後台-系統-部門]-請求模型</summary>     
public class CoMgrDptDbGetManyFromMbsSysDptReqMdl
{
    /// <summary>部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>部門-是否啟用</summary>
    public bool? ManagerDepartmentIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}