namespace DataModelLibrary.Database.AtomPermissionKind;

/// <summary>資料庫-原子-權限-類型-列舉</summary>
public enum DbAtomPermissionKindEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>無權限</summary>
    Denied = 1,

    /// <summary>自身</summary>
    Self = 2,

    /// <summary>所有人</summary>
    Everyone = 3,

}