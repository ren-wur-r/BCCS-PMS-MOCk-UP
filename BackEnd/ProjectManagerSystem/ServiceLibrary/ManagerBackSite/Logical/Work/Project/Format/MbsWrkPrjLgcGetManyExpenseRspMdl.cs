using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-取得多筆專案支出-回應模型</summary>
public class MbsWrkPrjLgcGetManyExpenseRspMdl : MbsLgcBaseRspMdl
{

    /// <summary>專案支出列表</summary>
    public List<MbsWrkPrjLgcGetManyExpenseRspItemMdl> EmployeeProjectExpenseList { get; set; }

    #region Eip專案支出列表

    /// <summary>Eip專案支出列表</summary>
    public List<MbsWrkPrjLgcGetEipExpenseRspItemMdl> EipProjectExpenseList { get; set; }

    /// <summary>Eip專案旅費列表</summary>
    public List<MbsWrkPrjLgcGetEipTravelExpenseRspItemMdl> EipProjectTravelExpenseList { get; set; }

    #endregion
}