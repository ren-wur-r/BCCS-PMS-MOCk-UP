using System.Threading.Tasks;

namespace ServiceLibrary.Schedule.Logical.Initial.Service;

/// <summary>排程-初始化-邏輯服務</summary>
public interface ISchInitialLogicalService
{
    /// <summary>排程-初始化-邏輯服務-開始任務</summary>
    public Task StartJobAsync();

    /// <summary>排程-初始化-邏輯服務-初始化地區</summary>
    public Task InitializeRegionAsync();

    /// <summary>排程-初始化-邏輯服務-初始化部門</summary>
    public Task InitializeDepartmentAsync();

    /// <summary>排程-初始化-邏輯服務-初始化公司-漢昕</summary>
    public Task InitializeCompanyBccsAsync();

    /// <summary>排程-初始化-邏輯服務-初始化員工-Admin</summary>
    public Task InitializeEmployeeAdminAsync();

    /// <summary>排程-初始化-邏輯服務-初始化員工-總經理</summary>
    public Task InitialEmployeeGeneralManagerAsync();

    /// <summary>排程-初始化-邏輯服務-結束任務</summary>
    public Task EndJobAsync();
}
