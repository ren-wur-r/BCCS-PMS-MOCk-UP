using DataModelLibrary.Database.ManagerCompanyMainKind;

namespace DataModelLibrary.Database.ManagerCompanySubKind;

/// <summary>資料庫-管理者-公司-子類別-常數</summary>
public class DbManagerCompanySubKindConst
{
    /// <summary>漢昕</summary>
    public static class BCCS
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -1;

        /// <summary>管理者-公司-主類別-ID</summary>
        public static int ManagerCompanyMainKindID { get; } = DbManagerCompanyMainKindConst.BCCS.ID;

        /// <summary>名稱</summary>
        public static string Name { get; } = "漢昕";

        /// <summary>是否啟用</summary>
        public static bool IsEnabled { get; } = true;
    }
}
