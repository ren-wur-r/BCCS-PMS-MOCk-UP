using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoMgrCmpMainKdDbGetManyNameRspMdl
{
    /// <summary>管理者公司主分類-列表</summary>
    public List<CoMgrCmpMainKdDbGetManyNameRspItemMdl> ManagerCompanyMainKindList { get; set; }
}