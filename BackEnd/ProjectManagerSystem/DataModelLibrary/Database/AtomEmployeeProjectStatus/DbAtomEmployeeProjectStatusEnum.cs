namespace DataModelLibrary.Database.AtomEmployeeProjectStatus;

/// <summary>資料庫-原子-員工專案狀態</summary>
public enum DbAtomEmployeeProjectStatusEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>未指派</summary>
    NotAssigned = 1,

    /// <summary>未開始</summary>
    NotStarted = 2,

    /// <summary>如期</summary>
    OnSchedule = 3,

    /// <summary>注意</summary>
    AtRisk = 4,

    /// <summary>延遲</summary>
    Delayed = 5,

    /// <summary>已完成</summary>
    Completed = 6
}
