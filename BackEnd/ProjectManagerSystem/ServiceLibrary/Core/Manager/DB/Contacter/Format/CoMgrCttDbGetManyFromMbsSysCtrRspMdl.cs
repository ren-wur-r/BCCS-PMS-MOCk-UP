using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-客戶名單-客戶窗口]-回應模型</summary>
public class CoMgrCttDbGetManyFromMbsSysCtrRspMdl
{
    /// <summary>管理者窗口-列表</summary>
    public List<CoMgrCttDbGetManyFrommbsSysCtrRspItemMdl> ManagerContacterList { get; set; }
}
