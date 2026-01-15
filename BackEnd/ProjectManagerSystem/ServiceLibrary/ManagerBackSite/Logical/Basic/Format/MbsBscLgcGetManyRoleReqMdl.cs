using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者角色-取得多筆-請求模型</summary>
public class MbsBscLgcGetManyRoleReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>角色-名稱</summary>
    public string ManagerRoleName { get; set; }

    /// <summary>是否啟用</summary>
    public bool? ManagerRoleIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}