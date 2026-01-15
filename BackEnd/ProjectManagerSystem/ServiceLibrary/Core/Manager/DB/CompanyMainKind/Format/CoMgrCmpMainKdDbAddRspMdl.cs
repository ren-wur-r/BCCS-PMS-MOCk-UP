using System;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-新增-回應模型</summary>
public class CoMgrCmpMainKdDbAddRspMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司主分類-建立時間</summary>
    public DateTimeOffset ManagerCompanyMainKindCreateTime { get; set; }

    /// <summary>管理者-公司主分類-更新時間</summary>
    public DateTimeOffset ManagerCompanyMainKindUpdateTime { get; set; }
}