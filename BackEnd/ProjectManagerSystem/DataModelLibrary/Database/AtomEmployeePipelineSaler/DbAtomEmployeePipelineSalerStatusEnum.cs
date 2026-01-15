namespace DataModelLibrary.Database.AtomEmployeePipelineSaler;

/// <summary>資料庫-原子-員工商機業務-狀態</summary>
public enum DbAtomEmployeePipelineSalerStatusEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>尚未回覆</summary>
    Pending = 1,

    /// <summary>接受</summary>
    Accepted = 2,

    /// <summary>拒絕</summary>
    Rejected = 3,

    /// <summary>轉指派業務</summary>
    Reassigned = 4,
}