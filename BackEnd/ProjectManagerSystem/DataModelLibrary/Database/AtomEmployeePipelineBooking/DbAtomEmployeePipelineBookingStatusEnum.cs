namespace DataModelLibrary.Database.AtomEmployeePipelineBooking;

/// <summary>資料庫-原子-員工-商機Booking-狀態</summary>
public enum DbAtomEmployeePipelineBookingStatusEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>處理中</summary>
    InProgress = 1,

    /// <summary>接受</summary>
    Accepted = 2,

    /// <summary>拒絕</summary>
    Rejected = 3
}