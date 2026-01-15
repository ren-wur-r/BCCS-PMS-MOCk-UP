using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrPmkDbGetManyNameReqMdl
{
    /// <summary>產品主分類ID列表</summary>
    public List<int> ManagerProductMainKindIdList { get; set; }
}