namespace DataModelLibrary.Database.AtomPipeline;

/// <summary>資料庫-原子-商機-狀態-列舉</summary>
public enum DbAtomPipelineStatusEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>連字號</summary>
    Hyphen = 1,

    /// <summary>eDM已開啟</summary>
    Opened = 2,

    /// <summary>eDM已點擊</summary>
    Clicked = 3,

    /// <summary>已轉電銷</summary>
    TransferredToSales = 4,

    /// <summary>已完成電銷</summary>
    CompletedSales = 5,

    /// <summary>已轉業務</summary>
    TransferredToBusiness = 6,

    /// <summary>商機10%：業務接受 或 booking 接受</summary>
    Business10Percent = 7,

    /// <summary>商機30%</summary>
    Business30Percent = 8,

    /// <summary>商機70%</summary>
    Business70Percent = 9,

    /// <summary>商機100%</summary>
    Business100Percent = 10,

    /// <summary>商機失敗</summary>
    BusinessFailed = 11,

    /// <summary>已轉專案</summary>
    TransferredToProject = 12
}