namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得多筆-回應項目模型</summary>
public class CoMgrDptDbGetManyRspItemMdl
{
    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }
}