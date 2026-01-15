using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;
using DataModelLibrary.Database.ManagerCompanyMainKind;
using DataModelLibrary.Database.ManagerCompanySubKind;

namespace DataModelLibrary.Database.ManagerCompany;

/// <summary>資料庫-管理者-公司-常數</summary>
public class DbManagerCompanyConst
{
    /// <summary>漢昕</summary>
    public static class BCCS
    {
        /// <summary>ID</summary>
        public static int ID { get; } = -1;

        /// <summary>管理者-公司-主類別-ID</summary>
        public static int ManagerCompanyMainKindID { get; } = DbManagerCompanyMainKindConst.BCCS.ID;

        /// <summary>管理者-公司-子類別-ID</summary>
        public static int ManagerCompanySubKindID { get; } = DbManagerCompanySubKindConst.BCCS.ID;

        /// <summary>統一編號</summary>
        public static string UnifiedNumber { get; } = "27488980";

        /// <summary>名稱</summary>
        public static string Name { get; } = "漢昕科技股份有限公司";

        /// <summary>客戶分級</summary>
        public static DbAtomCustomerGradeEnum CustomerGradeEnum { get; } = DbAtomCustomerGradeEnum.NotSelected;

        /// <summary>原子-管理者公司-狀態</summary>
        public static DbAtomManagerCompanyStatusEnum StatusEnum { get; } = DbAtomManagerCompanyStatusEnum.Operating;

        /// <summary>原子-資安責任等級</summary>
        public static DbAtomSecurityGradeEnum SecurityGradeEnum { get; } = DbAtomSecurityGradeEnum.NotSelected;

        /// <summary>原子-人員規模</summary>
        public static DbAtomEmployeeRangeEnum EmployeeRangeEnum { get; } = DbAtomEmployeeRangeEnum.NotSelected;
    }
}
