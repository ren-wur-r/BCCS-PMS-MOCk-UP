using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆角色權限從[管理者後台-基本-角色ID]-請求模型</summary>
public class MbsBscLgcGetManyRolePermissionFromRoleIdReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }
}