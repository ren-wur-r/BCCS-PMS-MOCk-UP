namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-是否存在-請求模型</summary>
public class CoMgrRolDbExistReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int? ManagerRoleID { get; set; }

    /// <summary>管理者-角色-名稱</summary>
    public string ManagerRoleName { get; set; }
}