namespace DataModelLibrary.Database.AtomManagerCompany;

/// <summary> 資料庫-原子-管理者-公司狀態-列舉 </summary>
public enum DbAtomManagerCompanyStatusEnum : short
{
    /// <summary>未選擇</summary>
    NotSelected = -1,

    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>營運中</summary>
    Operating = 1,

    /// <summary>停業</summary>
    Closed = 2,

    /// <summary>不清楚</summary>
    Unclear = 3
}