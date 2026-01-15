using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-邏輯-新增工作計劃書-請求模型</summary>
public class MbsWrkPrjLgcAddWorkReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案工作計劃書-網址</summary>
    public string EmployeeProjectWorkUrl { get; set; }
}