using System.Threading.Tasks;

namespace ServiceLibrary.ManagerBackSite.Logical.Schedule.Service;

/// <summary>管理者後台-排程-邏輯服務</summary>
public interface IMbsScheduleLogicalService
{
    /// <summary>管理者後台-排程-登出到期時間</summary>
    public Task LogoutExpiredTimeAsync();
}