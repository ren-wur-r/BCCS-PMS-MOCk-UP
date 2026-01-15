namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoEmpPrjDbGetManyFromMbsBscReqMdl
{
    /// <summary>員工專案-名稱(模糊查詢)</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}