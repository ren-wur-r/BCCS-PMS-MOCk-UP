namespace ServiceLibrary.Core.Employee.Logical.Format;

/// <summary>核心-員工-邏輯-取得密文密碼-請求模型</summary>
public class CoEmpLgcGetCypherPasswordReqMdl
{
    /// <summary>員工-明文密碼</summary>
    public string EmployeePlainPassword { get; set; }
}
