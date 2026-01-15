using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆-要求模型</summary>
public class CoMgrPmkDbGetManyReqMdl
{
    /// <summary>產品主分類ID列表</summary>
    public List<int> ManagerProductMainKindIdList { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    public bool? ManagerProductMainKindIsEnable { get; set; }
}