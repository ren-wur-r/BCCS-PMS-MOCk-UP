namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-系統-角色]-請求模型</summary>
public class CoMgrRolDbGetManyFromMbsSysRolReqMdl
{
    /// <summary>角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>角色-是否啟用</summary>
    public bool? ManagerRoleIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}