using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrRgnDbGetManyFromMbsBscRspMdl
{
    /// <summary>地區-列表</summary>
    public List<CoMgrRgnDbGetManyFromMbsBscRspItemMdl> ManagerRegionList { get; set; }
}