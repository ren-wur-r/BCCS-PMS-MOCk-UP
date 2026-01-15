using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆名稱-請求模型</summary>
public class CoMgrCttDbGetManyNameReqMdl
{
    /// <summary>管理者窗口ID列表</summary>
    public List<int> ManagerContacterIDList { get; set; }
}
