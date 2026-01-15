using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

/// <summary>核心-管理者-關係公司-資料庫-新增多筆-請求模型</summary>
public class CoMgrCmpAffDbAddManyReqMdl
{
    /// <summary>關係公司列表</summary>
    public List<CoMgrCmpAffDbAddManyReqItemMdl> ManagerCompanyAffiliateList { get; set; }
}
