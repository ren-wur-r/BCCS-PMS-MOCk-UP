namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆專案-項目-回應模型</summary>
public class MbsBscLgcGetManyProjectRspItemMdl
{
    /// <summary>員工專案-ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案-名稱</summary>
    public string EmployeeProjectName { get; set; }
}