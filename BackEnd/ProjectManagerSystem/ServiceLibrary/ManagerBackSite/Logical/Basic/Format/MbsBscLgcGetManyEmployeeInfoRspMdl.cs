using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工資訊-回應模型</summary>
public class MbsBscLgcGetManyEmployeeInfoRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工列表</summary>
    public List<MbsBasicLgcGetManyEmployeeInfoRspItemMdl> EmployeeList { get; set; }
}
