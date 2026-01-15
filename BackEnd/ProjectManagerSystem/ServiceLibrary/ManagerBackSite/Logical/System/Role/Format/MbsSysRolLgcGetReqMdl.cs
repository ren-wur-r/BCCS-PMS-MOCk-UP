using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

/// <summary>管理者後台-系統設定-角色-取得-請求模型</summary>
public class MbsSysRolLgcGetReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }
}
