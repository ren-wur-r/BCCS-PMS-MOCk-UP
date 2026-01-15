using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Controller.Dashboard;

namespace ServiceLibrary.ManagerBackSite.Logical.Dashboard.Service;

/// <summary>管理者後台-儀表板-邏輯服務介面</summary>
public interface IMbsDashboardLogicalService
{
    /// <summary>取得資訊</summary>
    Task<MbsDashboardCtlGetInfoRspMdl> GetInfoAsync(MbsDashboardCtlGetInfoReqMdl reqObj);
}
