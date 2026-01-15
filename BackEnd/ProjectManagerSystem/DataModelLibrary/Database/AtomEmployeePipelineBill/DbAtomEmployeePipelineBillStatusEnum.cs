namespace DataModelLibrary.Database.AtomEmployeePipelineBill;

/// <summary>資料庫-原子-員工-商機發票紀錄-狀態</summary>
public enum DbAtomEmployeePipelineBillStatusEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>未結案</summary>
    NotCompleted = 1,

    /// <summary>處理中</summary>
    InProgress = 2,

    /// <summary>已結案</summary>
    Completed = 3,
}