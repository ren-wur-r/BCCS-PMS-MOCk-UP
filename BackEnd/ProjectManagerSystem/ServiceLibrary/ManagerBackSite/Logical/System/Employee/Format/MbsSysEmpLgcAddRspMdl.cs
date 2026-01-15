using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

/// <summary>管理者後台-系統設定-員工-新增-回應模型</summary>
public class MbsSysEmpLgcAddRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }
}