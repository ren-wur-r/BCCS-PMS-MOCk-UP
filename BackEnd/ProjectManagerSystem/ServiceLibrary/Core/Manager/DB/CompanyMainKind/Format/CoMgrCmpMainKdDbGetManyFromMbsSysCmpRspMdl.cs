using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]-回應模型</summary>
public class CoMgrCmpMainKdDbGetManyFromMbsSysCmpRspMdl
{
    /// <summary>管理者-公司主分類-列表</summary>   
    public List<CoMgrCmpMainKdDbGetManyFrommbsSysCmpRspItemMdl> ManagerCompanyMainKindList { get; set; }
}