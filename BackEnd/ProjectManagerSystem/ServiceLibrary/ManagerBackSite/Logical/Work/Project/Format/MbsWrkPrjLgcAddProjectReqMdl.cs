using System;
using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-新增專案-請求模型</summary>
public class MbsWrkPrjLgcAddProjectReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>員工專案代碼</summary>
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案起始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>員工專案合約網址</summary>
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案工作計劃書網址</summary>
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案成員列表</summary>
    public List<MbsWrkPrjLgcAddProjectReqItemMdl> EmployeeProjectMemberList { get; set; }
}
