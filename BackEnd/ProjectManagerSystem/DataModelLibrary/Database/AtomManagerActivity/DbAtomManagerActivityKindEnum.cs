namespace DataModelLibrary.Database.AtomManagerActivity;

/// <summary>資料庫-原子-活動-類型</summary>
public enum DbAtomManagerActivityKindEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>實體活動</summary>
    PhysicalActivity = 1,

    /// <summary>線上活動</summary>
    OnlineActivity = 2
}