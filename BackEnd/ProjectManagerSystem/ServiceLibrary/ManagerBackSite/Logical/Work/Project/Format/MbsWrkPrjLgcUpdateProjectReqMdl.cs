using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-更新專案-請求模型</summary>
public class MbsWrkPrjLgcUpdateProjectReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案-ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案-代碼</summary>
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案-名稱</summary>
    public string EmployeeProjectName { get; set; }
}