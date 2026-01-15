using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;

/// <summary>管理者後台-系統設定-部門-取得-請求模型</summary>
public class MbsSysDptLgcGetReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-部門-ID</summary>
    public int ManagerDepartmentID { get; set; }
}