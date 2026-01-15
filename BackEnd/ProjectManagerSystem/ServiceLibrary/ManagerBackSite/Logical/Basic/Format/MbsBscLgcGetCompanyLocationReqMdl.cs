using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得公司營業地點-請求模型</summary>
public class MbsBscLgcGetCompanyLocationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點-是否已刪除</summary>
    public bool? ManagerCompanyLocationIsDeleted { get; set; }
}
