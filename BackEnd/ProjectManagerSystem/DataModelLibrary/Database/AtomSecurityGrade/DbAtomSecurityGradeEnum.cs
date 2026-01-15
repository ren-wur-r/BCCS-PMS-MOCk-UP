namespace DataModelLibrary.Database.AtomSecurityGrade;

/// <summary>資料庫-原子-資安責任等級-類型</summary>
public enum DbAtomSecurityGradeEnum : short
{
    /// <summary>未選擇</summary>
    NotSelected = -1,

    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>A</summary>
    A = 1,

    /// <summary>B</summary>
    B = 2,

    /// <summary>C</summary>
    C = 3,

    /// <summary>D</summary>
    D = 4,
}