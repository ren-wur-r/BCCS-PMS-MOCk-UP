namespace DataModelLibrary.Database.AtomManagerContacter;

/// <summary>資料庫-原子-開發評等類型</summary>
public enum DbAtomManagerContacterRatingKindEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>白名單</summary>
    Whitelist = 1,

    /// <summary>灰名單</summary>    
    Graylist = 2,

    /// <summary>黑名單</summary>
    Blacklist = 3
}