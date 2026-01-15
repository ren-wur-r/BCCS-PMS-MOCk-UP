using ServiceLibrary.Core.Employee.Logical.Format;

namespace ServiceLibrary.Core.Employee.Logical.Service;

/// <summary>核心-員工-邏輯服務</summary>
public interface ICoEmployeeLogicalService
{
    /// <summary>核心-員工-邏輯-取得密文密碼</summary>
    public CoEmpLgcGetCypherPasswordRspMdl GetCypherPassword(CoEmpLgcGetCypherPasswordReqMdl reqObj);

    /// <summary>核心-員工-邏輯-取得登入令牌</summary>
    public CoEmpLgcGetLoginTokenRspMdl GetLoginToken(CoEmpLgcGetLoginTokenReqMdl reqObj);

    /// <summary>核心-員工-邏輯-拆解登入令牌</summary>
    public CoEmpLgcSplitLoginTokenRspMdl SplitLoginToken(CoEmpLgcSplitLoginTokenReqMdl reqObj);

}
