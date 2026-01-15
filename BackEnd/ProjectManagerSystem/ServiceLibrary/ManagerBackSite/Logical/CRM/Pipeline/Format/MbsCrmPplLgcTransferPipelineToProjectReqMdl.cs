using System;
using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-轉換商機至專案-請求模型</summary>
public class MbsCrmPplLgcTransferPipelineToProjectReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>員工專案代碼</summary>
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案起始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>員工專案契約網址</summary>
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案工作計劃書網址</summary>
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案成員列表</summary>
    public List<MbsCrmPplLgcTransferPipelineToProjectReqItemMdl> EmployeeProjectMemberEmployeeList { get; set; }
}
