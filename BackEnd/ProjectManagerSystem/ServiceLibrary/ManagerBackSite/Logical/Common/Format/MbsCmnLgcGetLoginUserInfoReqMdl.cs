using DataModelLibrary.Database.AtomMenu;

namespace ServiceLibrary.ManagerBackSite.Logical.Common.Format;

/// <summary>管理者後台-通用-邏輯-取得登入使用者資訊-請求模型</summary>
public class MbsCmnLgcGetLoginUserInfoReqMdl
{
    /// <summary>員工登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

    /// <summary>原子-目錄</summary>
    public DbAtomMenuEnum AtomMenu { get; set; }

}
