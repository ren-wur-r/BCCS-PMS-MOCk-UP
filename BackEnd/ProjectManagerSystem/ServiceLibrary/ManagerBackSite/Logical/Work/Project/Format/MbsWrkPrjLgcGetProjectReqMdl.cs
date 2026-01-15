using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

public class MbsWrkPrjLgcGetProjectReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }
}