using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆[名稱]-請求模型</summary>
public class CoMgrCmpMainKdDbGetManyNameReqMdl
{
    /// <summary>公司主分類-ID列表</summary>
    public List<int> ManagerCompanyMainKindIdList { get; set; }
}