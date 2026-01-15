using System.Threading.Tasks;

namespace ServiceLibrary.Schedule.Logical.Timer.Service;

/// <summary>排程-邏輯-計時器-服務介面</summary>
public interface ISchTimerLogicalService
{
    /// <summary>更新專案狀態</summary>
    public Task UpdateProjectStatusAsync();

    /// <summary>通知異常專案狀態</summary>
    public Task NotifyAbnormalProjectStatusAsync();
}