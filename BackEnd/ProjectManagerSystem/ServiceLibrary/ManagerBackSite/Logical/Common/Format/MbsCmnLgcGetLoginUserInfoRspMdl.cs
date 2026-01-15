using DataModelLibrary.Database.AtomPermissionKind;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Common.Format;

/// <summary>管理者後台-通用-邏輯-取得登入使用者資訊-請求模型</summary>
public class MbsCmnLgcGetLoginUserInfoRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>原子-權限-類型-檢視</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdView { get; set; }

    /// <summary>原子-權限-類型-新增</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdCreate { get; set; }

    /// <summary>原子-權限-類型-編輯</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdEdit { get; set; }

    /// <summary>原子-權限-類型-刪除</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdDelete { get; set; }

}
