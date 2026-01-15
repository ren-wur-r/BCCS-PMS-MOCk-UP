using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

public class MbsWrkPrjLgcGetProjectRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案狀態</summary>
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案代碼</summary>
    public string EmployeeProjectCode { get; set; }

    /// <summary>起始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>迄止時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>管理者-公司-名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>員工專案合約建立時間</summary>
    public DateTimeOffset? EmployeeProjectContractCreateTime { get; set; }

    /// <summary>員工專案合約網址</summary>
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案工作計劃書建立時間</summary>
    public DateTimeOffset? EmployeeProjectWorkCreateTime { get; set; }

    /// <summary>員工專案工作計劃書網址</summary>
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案成員列表</summary>
    public List<MbsWrkPrjLgcGetProjectRspItemMdl> EmployeeProjectMemberList { get; set; }

}