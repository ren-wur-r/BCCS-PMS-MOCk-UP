using DataModelLibrary.Database.ManagerDepartment;
using DataModelLibrary.Database.ManagerRole;

namespace DataModelLibrary.Database.Employee;

/// <summary>資料庫-員工-常數</summary>
public static class DbEmployeeConst
{
    /// <summary>Admin</summary>
    public static class Admin
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -99;

        /// <summary>管理者-部門ID</summary>
        public static int ManagerDepartmentID { get; } = DbManagerDepartmentConst.Admin.ID;

        /// <summary>管理者-角色ID</summary>
        public static int ManagerRoleID { get; } = DbManagerRoleConst.Admin.ID;

        /// <summary>帳號</summary>
        public static string Account { get; } = "admin";

        /// <summary>明文密碼</summary>
        public static string Password { get; } = "admin";

        /// <summary>信箱網域</summary>
        public static string Email { get; } = "admin@bccs.com.tw";

        /// <summary>名稱</summary>
        public static string Name { get; } = "admin";

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

        /// <summary>管理者-部門ID</summary>
        public static int ManagerDepartmentID { get; } = DbManagerDepartmentConst.GeneralManagerOffice.ID;

        /// <summary>管理者-角色ID</summary>
        public static int ManagerRoleID { get; } = DbManagerRoleConst.GeneralManager.ID;

        /// <summary>帳號</summary>
        public static string Account { get; } = "alllenc";

        /// <summary>明文密碼</summary>
        public static string Password { get; } = "alllenc";

        /// <summary>信箱網域</summary>
        public static string Email { get; } = "alllenc@bccs.com.tw";

        /// <summary>名稱</summary>
        public static string Name { get; } = "陳建良";

        /// <summary>備註</summary>
        public static string Remark { get; } = string.Empty;

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }


}