namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-新增[指定ID]-請求模型</summary>
public class CoMgrDptDbAddWithIdReqMdl
{
    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentId { get; set; }

    /// <summary>管理者-部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }

    /// <summary>管理者-部門-是否啟用</summary>
    public bool ManagerDepartmentIsEnable { get; set; }
}