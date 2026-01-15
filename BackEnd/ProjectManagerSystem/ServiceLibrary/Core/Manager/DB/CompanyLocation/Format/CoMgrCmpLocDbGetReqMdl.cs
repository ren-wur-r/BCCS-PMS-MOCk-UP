namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-取得-請求模型</summary>
public class CoMgrCmpLocDbGetReqMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點-是否已刪除</summary>
    public bool? ManagerCompanyLocationIsDeleted { get; set; }
}