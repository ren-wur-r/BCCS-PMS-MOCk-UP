using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Controller.Dashboard;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.ManagerBackSite.Logical.Dashboard.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-儀表板-控制器</summary>
[ApiController]
[Route("api/ManagerBackSite/[controller]")]
public class MbsDashboardController : ControllerBase
{
    private readonly IMbsDashboardLogicalService _mbsDashboardLogicalService;

    public MbsDashboardController(IMbsDashboardLogicalService mbsDashboardLogicalService)
    {
        _mbsDashboardLogicalService = mbsDashboardLogicalService;
    }

    /// <summary>取得資訊</summary>
    [HttpPost("GetInfo")]
    public async Task<MbsDashboardCtlGetInfoRspMdl> GetInfo([FromBody] MbsDashboardCtlGetInfoReqMdl reqObj)
    {
        return await _mbsDashboardLogicalService.GetInfoAsync(reqObj);
    }
}
