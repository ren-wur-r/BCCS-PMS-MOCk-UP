using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

/// <summary>管理者後台-系統設定-角色-新增-回應模型</summary>
public class MbsSysRolLgcAddRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-角色ID</summary>
    public int ManagerRoleID { get; set; }
}