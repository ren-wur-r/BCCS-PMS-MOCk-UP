namespace DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;

public enum DbAtomPhoneSalesCustomerStatusEnum : short
{
    /// <summary>待聯繫</summary>
    PendingContact = 1,

    /// <summary>已聯繫</summary>
    Contacted = 2,

    /// <summary>已約訪</summary>
    Appointed = 3,

    /// <summary>已派送</summary>
    Dispatched = 4,

    /// <summary>不再聯繫</summary>
    NoContact = 5
}
