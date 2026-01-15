namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆專案里程碑-回應項目模型</summary>
public class MbsBscLgcGetManyProjectStoneRspItemMdl
{
    /// <summary>員工專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }
}