using System;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-新增-回應模型</summary>
public class CoMgrCmpLocDbAddRspMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點建立時間</summary>
    public DateTimeOffset ManagerCompanyLocationCreateTime { get; set; }

    /// <summary>管理者公司營業地點更新時間</summary>
    public DateTimeOffset ManagerCompanyLocationUpdateTime { get; set; }
}