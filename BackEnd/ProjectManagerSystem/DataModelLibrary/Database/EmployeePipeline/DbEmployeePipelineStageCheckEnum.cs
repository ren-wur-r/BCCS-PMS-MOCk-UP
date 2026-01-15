namespace DataModelLibrary.Database.EmployeePipeline;

/// <summary>資料庫-員工商機-階段檢核狀態列舉</summary>
public enum DbEmployeePipelineStageCheckEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>尚未確認</summary>
    Pending = 1,

    /// <summary>已確認</summary>
    Confirmed = 2,

    /// <summary>不符合 / 無此需求</summary>
    NotApplicable = 3
}
