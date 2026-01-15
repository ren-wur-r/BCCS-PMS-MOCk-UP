using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得全部管理者地區-回應模型</summary>
public class MbsBscLgcGetAllRegionRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者地區-列表</summary>
    public List<MbsBscLgcGetAllRegionRspItemMdl> ManagerRegionList { get; set; }
}