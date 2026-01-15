namespace DataModelLibrary.Database.AtomEmployeePipelineProduct;

/// <summary>資料庫-原子-員工商機產品-採購類型</summary>
public enum DbAtomEmployeePipelineProductPurchaseKindEnum : short
{

    /// <summary>未選擇</summary>
    NotSelected = -1,

    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>新購</summary>
    NewlyPurchase = 1,

    /// <summary>續約</summary>
    Renewal = 2,
}