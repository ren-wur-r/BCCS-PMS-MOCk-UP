namespace DataModelLibrary.Database.AtomManagerContacter;

/// <summary> 資料庫-原子-管理者-聯絡人狀態-列舉 </summary>
public enum DbAtomManagerContacterStatusEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>不清楚(預設)</summary>
    Unknown = 1,

    /// <summary>在職</summary>
    Employed = 2,

    /// <summary>離職</summary>
    Unemployed = 3,
}