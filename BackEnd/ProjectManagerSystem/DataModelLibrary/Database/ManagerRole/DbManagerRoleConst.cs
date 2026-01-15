using DataModelLibrary.Database.ManagerDepartment;
using DataModelLibrary.Database.ManagerRegion;

namespace DataModelLibrary.Database.ManagerRole;

/// <summary>資料庫-管理者-角色-常數</summary>
public static class DbManagerRoleConst
{
    /// <summary>Admin</summary>
    public static class Admin
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -99;

        /// <summary>管理者-地區ID</summary>
        public static int ManagerRegionID { get; } = DbManagerRegionConst.AllRegion.ID;

        /// <summary>管理者-部門ID</summary>
        public static int ManagerDepartmentID { get; } = DbManagerDepartmentConst.Admin.ID;

        /// <summary>名稱</summary>
        public static string Name { get; } = "Admin角色";

        /// <summary>備註</summary>
        public static string Remark { get; } = string.Empty;

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }

    /// <summary>總經理</summary>
    public static class GeneralManager
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -1;

        /// <summary>管理者-地區ID</summary>
        public static int ManagerRegionID { get; } = DbManagerRegionConst.AllRegion.ID;

        /// <summary>管理者-部門ID</summary>
        public static int ManagerDepartmentID { get; } = DbManagerDepartmentConst.GeneralManagerOffice.ID;

        /// <summary>名稱</summary>
        public static string Name { get; } = "總經理";

        /// <summary>備註</summary>
        public static string Remark { get; } = string.Empty;

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }
}