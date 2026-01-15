using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.ManagerBackSite.Logical.PhoneSales.Service;

namespace ProjectManagerWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MbsCrmPhoneSalesController : ControllerBase
{
    private readonly IMbsPhsLogicalService _mbsPhsLogicalService;

    public MbsCrmPhoneSalesController(IMbsPhsLogicalService mbsPhsLogicalService)
    {
        _mbsPhsLogicalService = mbsPhsLogicalService;
    }

    [HttpPost("GetManyCustomer")]
    public async Task<MbsCrmPhsCtlGetManyCustomerRspMdl> GetManyCustomer([FromBody] MbsCrmPhsCtlGetManyCustomerReqMdl reqObj)
    {
        return await _mbsPhsLogicalService.GetManyCustomerAsync(reqObj);
    }

    [HttpPost("GetCustomer")]
    public async Task<MbsCrmPhsCtlGetCustomerRspMdl> GetCustomer([FromBody] MbsCrmPhsCtlGetCustomerReqMdl reqObj)
    {
        return await _mbsPhsLogicalService.GetCustomerAsync(reqObj);
    }

    [HttpPost("AddCustomer")]
    public async Task<MbsCrmPhsCtlAddCustomerRspMdl> AddCustomer([FromBody] MbsCrmPhsCtlAddCustomerReqMdl reqObj)
    {
        return await _mbsPhsLogicalService.AddCustomerAsync(reqObj);
    }

    [HttpPost("UpdateCustomer")]
    public async Task<MbsCrmPhsCtlUpdateCustomerRspMdl> UpdateCustomer([FromBody] MbsCrmPhsCtlUpdateCustomerReqMdl reqObj)
    {
        return await _mbsPhsLogicalService.UpdateCustomerAsync(reqObj);
    }

    [HttpPost("DeleteCustomer")]
    public async Task<MbsCrmPhsCtlDeleteCustomerRspMdl> DeleteCustomer([FromBody] MbsCrmPhsCtlDeleteCustomerReqMdl reqObj)
    {
        return await _mbsPhsLogicalService.DeleteCustomerAsync(reqObj);
    }

    [HttpPost("UpdateCustomerOrder")]
    public async Task<MbsCrmPhsCtlUpdateCustomerOrderRspMdl> UpdateCustomerOrder([FromBody] MbsCrmPhsCtlUpdateCustomerOrderReqMdl reqObj)
    {
        return await _mbsPhsLogicalService.UpdateCustomerOrderAsync(reqObj);
    }
}
