namespace DataModelLibrary.Database.AtomEmployeePipelineProduct;

/// <summary>資料庫-原子-員工商機產品-採購類型</summary>
public enum DbAtomEmployeePipelineProductContractKindEnum : short
{

    /// <summary>未選擇</summary>
    NotSelected = -1,

    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>共契</summary>
    JointTendering = 1,

    /// <summary>標案</summary>
    Tender = 2,

    /// <summary>其他</summary>
    Other = 3,
}