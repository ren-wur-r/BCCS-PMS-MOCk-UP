using System;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-新增-回應模型</summary>
public class CoMgrCmpDbAddRspMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司建立時間</summary>
    public DateTimeOffset ManagerCompanyCreateTime { get; set; }

    /// <summary>管理者公司更新時間</summary>
    public DateTimeOffset ManagerCompanyUpdateTime { get; set; }
}