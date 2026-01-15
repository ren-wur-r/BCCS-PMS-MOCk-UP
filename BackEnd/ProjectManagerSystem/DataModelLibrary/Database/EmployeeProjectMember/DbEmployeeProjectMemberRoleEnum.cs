namespace DataModelLibrary.Database.EmployeeProjectMember;


/// <summary>資料庫-員工-專案成員-角色-列舉</summary>
public enum DbEmployeeProjectMemberRoleEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    /// <summary>總經理</summary>
    GeneralManager = 1,

    /// <summary>業務</summary>
    Saler = 2,

    /// <summary>專案經理</summary>
    ProjectManager = 3,

    /// <summary>部門主管</summary>
    DepartmentLeader = 4,

    /// <summary>執行者</summary>
    Executor = 5,

    /// <summary>助理</summary>
    Assistant = 6,
}
