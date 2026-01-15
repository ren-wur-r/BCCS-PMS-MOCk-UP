using System.Threading.Tasks;
using ServiceLibrary.EipV1.Http.Format;

namespace ServiceLibrary.EipV1.Http.Service;

/// <summary>EipV1-Http-服務</summary>
public interface IEipV1HttpService
{
    /// <summary>EipV1-Http-PMS-查詢專案費用</summary>
    public Task<EipV1HttpPmsGetProjectExpenseRspMdl> PmsGetProjectExpenseAsync(EipV1HttpPmsGetProjectExpensesReqMdl reqObj);

    /// <summary>EipV1-Http-PMS-查詢專案旅費</summary>
    public Task<EipV1HttpPmsGetProjectTravelExpenseRspMdl> PmsGetProjectTravelExpenseAsync(EipV1HttpPmsGetProjectTravelExpenseReqMdl reqObj);
}