using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;

namespace ServiceLibrary.ManagerBackSite.Logical.PhoneSales.Service;

public interface IMbsPhsLogicalService
{
    Task<MbsCrmPhsCtlGetManyCustomerRspMdl> GetManyCustomerAsync(MbsCrmPhsCtlGetManyCustomerReqMdl reqObj);
    Task<MbsCrmPhsCtlGetCustomerRspMdl> GetCustomerAsync(MbsCrmPhsCtlGetCustomerReqMdl reqObj);
    Task<MbsCrmPhsCtlAddCustomerRspMdl> AddCustomerAsync(MbsCrmPhsCtlAddCustomerReqMdl reqObj);
    Task<MbsCrmPhsCtlUpdateCustomerRspMdl> UpdateCustomerAsync(MbsCrmPhsCtlUpdateCustomerReqMdl reqObj);
    Task<MbsCrmPhsCtlDeleteCustomerRspMdl> DeleteCustomerAsync(MbsCrmPhsCtlDeleteCustomerReqMdl reqObj);
    Task<MbsCrmPhsCtlUpdateCustomerOrderRspMdl> UpdateCustomerOrderAsync(MbsCrmPhsCtlUpdateCustomerOrderReqMdl reqObj);
}
