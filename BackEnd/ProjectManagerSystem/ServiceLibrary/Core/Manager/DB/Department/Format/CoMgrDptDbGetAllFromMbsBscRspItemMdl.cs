namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得全部從[管理者後台-基本-部門]-回應項目模型</summary>
public class CoMgrDptDbGetAllFromMbsBscRspItemMdl
{
    /// <summary>部門-ID</summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>部門-名稱</summary>
    public string ManagerDepartmentName { get; set; }
}