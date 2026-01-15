using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-基本]</summary>
public class CoMgrCttDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者窗口-列表</summary>
    public List<CoMgrCttDbGetManyFromMbsBscRspItemMdl> ManagerContacterList { get; set; }
}