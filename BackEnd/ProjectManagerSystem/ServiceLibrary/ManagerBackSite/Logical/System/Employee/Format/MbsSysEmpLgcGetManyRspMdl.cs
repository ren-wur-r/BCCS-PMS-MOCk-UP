using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

/// <summary>管理者後台-系統設定-員工-取得多筆-回應模型</summary>
public class MbsSysEmpLgcGetManyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-列表</summary>
    public List<MbsSysEmpLgcGetManyItemRspMdl> EmployeeList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}