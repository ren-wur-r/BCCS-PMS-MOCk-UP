namespace DataModelLibrary.Database.AtomEmployeeRange;

/// <summary>資料庫-原子-人員規模-類型</summary>
public enum DbAtomEmployeeRangeEnum : short
{
    /// <summary>未選擇</summary>
    NotSelected = -1,

    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>不清楚</summary>
    Unclear = 1,

    /// <summary>少於100人</summary>
    LessThan100 = 2,

    /// <summary>100-500人</summary>
    Range100To500 = 3,

    /// <summary>500-1000人</summary>
    Range500To1000 = 4,

    /// <summary>1000人以上</summary>
    MoreThan1000 = 5
}