namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

/// <summary>核心-員工-專案里程碑-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoEmpPsDbGetManyFromMbsBscReqMdl
{
    /// <summary>員工專案-ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}