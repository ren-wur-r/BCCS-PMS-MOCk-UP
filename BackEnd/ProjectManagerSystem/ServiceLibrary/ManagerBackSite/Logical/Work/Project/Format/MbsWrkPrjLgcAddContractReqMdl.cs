using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-邏輯-新增合約-請求模型</summary>
public class MbsWrkPrjLgcAddContractReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案契約-網址</summary>
    public string EmployeeProjectContractUrl { get; set; }
}