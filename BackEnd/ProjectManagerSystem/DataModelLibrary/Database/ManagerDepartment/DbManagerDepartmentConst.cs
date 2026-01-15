namespace DataModelLibrary.Database.ManagerDepartment;

/// <summary>資料庫-管理者-部門-常數</summary>
public static class DbManagerDepartmentConst
{
    /// <summary>Admin</summary>
    public static class Admin
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -99;

        /// <summary>名稱</summary>
        public static string Name { get; } = "Admin部";

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }

    /// <summary>總經理室</summary>
    public static class GeneralManagerOffice
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -1;

        /// <summary>名稱</summary>
        public static string Name { get; } = "總經理室";

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;

    }

}