namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoEmpPsDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>員工專案里程碑-ID</summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }
}