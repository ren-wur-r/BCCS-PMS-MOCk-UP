using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Region.Format;

/// <summary>核心-管理者-地區-資料庫-取得多筆-請求模型</summary>
public class CoMgrRgnDbGetManyReqMdl
{
    /// <summary>管理者地區ID列表</summary>
    public List<int> ManagerRegionIdList { get; set; }

}