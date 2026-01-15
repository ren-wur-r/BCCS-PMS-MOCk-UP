namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得[ID]-請求模型</summary>
public class CoEmpInfDbGetIdReqMdl
{
    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>員工-密碼</summary>
    public string EmployeePassword { get; set; }
}
