using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆名稱-回應模型</summary>
public class CoMgrCttDbGetManyNameRspMdl
{
    /// <summary>管理者窗口列表</summary>
    public List<CoMgrCttDbGetManyNameRspItemMdl> ManagerContacterList { get; set; }
}
