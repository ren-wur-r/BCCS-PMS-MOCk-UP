namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-統一編號是否存在-請求模型</summary>
public class CoMgrComDbUniNumExistReqMdl
{
    /// <summary>客戶公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    public string UnifiedNumber { get; set; }
}

