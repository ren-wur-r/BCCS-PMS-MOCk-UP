using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-登入-回應模型</summary>
public class MbsBscLgcLoginRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

}
