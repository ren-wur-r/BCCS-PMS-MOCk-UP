namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得[名稱]-回應模型</summary>
public class CoMgrRolDbGetNameRspMdl
{
    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>管理者-地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }
}
