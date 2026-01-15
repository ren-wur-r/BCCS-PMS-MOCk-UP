using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

/// <summary>管理者後台-系統設定-員工-取得-請求模型</summary>
public class MbsSysEmpLgcGetReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }
}