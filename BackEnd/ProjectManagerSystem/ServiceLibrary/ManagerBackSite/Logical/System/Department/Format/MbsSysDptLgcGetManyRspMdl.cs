using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;

/// <summary>管理者後台-系統設定-部門-取得多筆-回應模型</summary>
public class MbsSysDptLgcGetManyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>部門列表</summary>
    public List<MbsSysDptLgcGetManyItemRspMdl> ManagerDepartmentList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}