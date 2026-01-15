using System.Collections.Generic;
namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得全部從[管理者後台-基本-地區]-回應模型</summary>
public class CoMgrRgnDbGetAllFromMbsBscRspMdl
{
    /// <summary>管理者地區-列表</summary>
    public List<CoMgrRgnDbGetAllFromMbsBscRspItemMdl> ManagerRegionList { get; set; }
}