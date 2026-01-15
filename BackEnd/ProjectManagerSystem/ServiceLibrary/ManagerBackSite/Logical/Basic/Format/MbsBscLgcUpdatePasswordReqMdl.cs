using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-修改密碼-請求模型</summary>
public class MbsBscLgcUpdatePasswordReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-舊密碼</summary>
    public string OldPassword { get; set; }

    /// <summary>管理者-新密碼</summary>
    public string NewPassword { get; set; }
}