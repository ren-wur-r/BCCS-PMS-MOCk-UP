using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員-請求模型</summary>
public class MbsBscLgcGetManyEmployeeProjectMemberReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }
}