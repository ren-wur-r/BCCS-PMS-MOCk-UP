namespace DataModelLibrary.Database.ManagerRegion;

/// <summary>資料庫-管理者-地區-常數</summary>
public static class DbManagerRegionConst
{
    /// <summary>無權限</summary>
    public static class Denied
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -2;

        /// <summary>名稱</summary>
        public static string Name { get; } = "無權限";

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }

    /// <summary>全區</summary>
    public static class AllRegion
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -1;

        /// <summary>名稱</summary>
        public static string Name { get; } = "全區";

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }

}
