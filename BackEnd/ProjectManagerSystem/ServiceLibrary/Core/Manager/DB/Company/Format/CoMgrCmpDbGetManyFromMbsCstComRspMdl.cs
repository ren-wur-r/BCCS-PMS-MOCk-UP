using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得[多筆]從[管理者後台-系統設定-客戶]-回應模型</summary>
public class CoMgrCmpDbGetManyFromMbsSysCmpRspMdl
{
    /// <summary>管理者公司清單</summary>
    public List<CoMgrCmpDbGetManyFrommbsSysCmpRspItemMdl> ManagerCompanyList { get; set; }
}