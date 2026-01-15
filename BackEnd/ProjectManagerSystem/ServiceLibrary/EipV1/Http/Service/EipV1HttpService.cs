using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnUri;
using DataModelLibrary.GlobalSystem.Config;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using ServiceLibrary.EipV1.Http.Controller;
using ServiceLibrary.EipV1.Http.Format;

namespace ServiceLibrary.EipV1.Http.Service;

/// <summary>EipV1-Http-服務</summary>
public class EipV1HttpService : IEipV1HttpService
{
    /// <summary>logger</summary>
    private readonly ILogger<EipV1HttpService> _logger;

    /// <summary>httpClient</summary>
    private readonly HttpClient _httpClient;

    /// <summary>eipV1Config</summary>
    private readonly GsEipV1Config _eipV1Config;

    /// <summary>建構式</summary>
    public EipV1HttpService(
        ILogger<EipV1HttpService> logger,
        IHttpClientFactory httpFactory,
        GsEipV1Config eipV1Config)
    {
        this._logger = logger;
        this._httpClient = httpFactory.CreateClient(EipV1HttpConst.HTTP_CLIENT_NAME);
        this._eipV1Config = eipV1Config;
    }

    /// <summary>EipV1-Http-PMS-查詢專案費用</summary>
    public async Task<EipV1HttpPmsGetProjectExpenseRspMdl> PmsGetProjectExpenseAsync(EipV1HttpPmsGetProjectExpensesReqMdl reqObj)
    {
        var httpReqObj = new EipV1HttpGetReqMdl()
        {
            RelativeUrl = EipV1HttpConst.PMS_GET_PROJECT_EXPENSES +
                $"?ProjectCode={Uri.EscapeDataString(reqObj.ProjectCode)}" +
                $"&ProjectName={Uri.EscapeDataString(reqObj.ProjectName)}" +
                $"&StartDate={reqObj.StartDate.ToOffset(TimeSpan.FromHours(8)):yyyy-MM-dd}" +
                $"&EndDate={reqObj.EndDate.ToOffset(TimeSpan.FromHours(8)):yyyy-MM-dd}",
            //Method = HttpMethod.Get,
            //ReqData = null,
        };
        var httpRspObj = await this.GetAsync<EipV1CtlGetProjectExpenseRspMdl>(httpReqObj);
        if (httpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }
        if (httpRspObj.RspData == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }
        if (httpRspObj.RspData.Success == false)
        {
            this._logger.LogError($"reqobj：{httpRspObj.RspData.Message}");
            return default;
        }

        var dataList = new List<EipV1HttpPmsGetProjectExpenseRspItemMdl>();
        foreach (var item in httpRspObj.RspData.Data)
        {
            if (DateTimeOffset.TryParse(item.ExpenseDate + " +08:00", out var expenseDate) == false)
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", item: {JsonSerializer.Serialize(item)}");
                return default;
            }

            dataList.Add(new EipV1HttpPmsGetProjectExpenseRspItemMdl()
            {
                ApprovalStatus = item.ApprovalStatus,
                Applicant = item.Applicant,
                ExpenseDate = expenseDate,
                ExpenseReason = item.ExpenseReason,
                Participants = item.Participants,
                ExpenseAmount = item.ExpenseAmount,
            });
        }

        var rspObj = new EipV1HttpPmsGetProjectExpenseRspMdl()
        {
            Success = httpRspObj.RspData.Success,
            Message = httpRspObj.RspData.Message,
            Total = httpRspObj.RspData.Total,
            DataList = dataList,
        };
        return rspObj;
    }

    /// <summary>EipV1-Http-PMS-查詢專案旅費</summary>
    public async Task<EipV1HttpPmsGetProjectTravelExpenseRspMdl> PmsGetProjectTravelExpenseAsync(EipV1HttpPmsGetProjectTravelExpenseReqMdl reqObj)
    {
        var httpReqObj = new EipV1HttpGetReqMdl()
        {
            RelativeUrl = EipV1HttpConst.PMS_GET_PROJECT_TRAVEL_EXPENSES +
                $"?ProjectCode={Uri.EscapeDataString(reqObj.ProjectCode)}" +
                $"&ProjectName={Uri.EscapeDataString(reqObj.ProjectName)}" +
                $"&StartDate={reqObj.StartDate.ToOffset(TimeSpan.FromHours(8)):yyyy-MM-dd}" +
                $"&EndDate={reqObj.EndDate.ToOffset(TimeSpan.FromHours(8)):yyyy-MM-dd}",
            //Method = HttpMethod.Get,
            //ReqData = null,
        };
        var httpRspObj = await this.GetAsync<EipV1CtlGetProjectTravelExpenseRspMdl>(httpReqObj);
        if (httpRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }
        if (httpRspObj.RspData == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }
        if (httpRspObj.RspData.Success == false)
        {
            this._logger.LogError($"reqobj：{httpRspObj.RspData.Message}");
            return default;
        }

        var dataList = new List<EipV1HttpPmsGetProjectTravelExpenseRspItemMdl>();
        foreach (var item in httpRspObj.RspData.Data)
        {
            if (DateTimeOffset.TryParse(item.TravelDate + " +08:00", out var travelDate) == false)
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", item: {JsonSerializer.Serialize(item)}");
                return default;
            }

            dataList.Add(new EipV1HttpPmsGetProjectTravelExpenseRspItemMdl()
            {
                ApprovalStatus = item.ApprovalStatus,
                Applicant = item.Applicant,
                TravelDate = travelDate,
                TravelRoute = item.TravelRoute,
                WorkDescription = item.WorkDescription,
                TransportationTool = item.TransportationTool,
                TransportationAmount = item.TransportationAmount,
                Mileage = item.Mileage,
                AccommodationAmount = item.AccommodationAmount,
                SpecialExpenseReason = item.SpecialExpenseReason,
                SpecialExpenseAmount = item.SpecialExpenseAmount,
                TotalAmount = item.TotalAmount,
            });
        }

        var rspObj = new EipV1HttpPmsGetProjectTravelExpenseRspMdl()
        {
            Success = httpRspObj.RspData.Success,
            Message = httpRspObj.RspData.Message,
            Total = httpRspObj.RspData.Total,
            DataList = dataList,
        };
        return rspObj;
    }

    /// <summary>EipV1-Http-取得</summary>
    private async Task<EipV1HttpGetRspMdl<T>> GetAsync<T>(EipV1HttpGetReqMdl reqObj)
    {
        if (CmnUriParser.TryCreate(this._httpClient.BaseAddress.AbsoluteUri.ToString(), reqObj.RelativeUrl, out var fullUri) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        //var reqMsg = new HttpRequestMessage(reqObj.Method, fullUri);

        //reqMsg.Headers.Add("Api-Key", this._eipV1Config.ApiKey);

        try
        {
            var sw = new Stopwatch();
            sw.Start();
            var rspMsg = await this._httpClient.GetAsync(fullUri);
            sw.Stop();
            if (rspMsg.IsSuccessStatusCode == false)
            {
                this._logger.LogError(
                    $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                    $", rspMsg.StatusCode: {rspMsg.StatusCode}" +
                    $", sw.ElapsedMilliseconds: {sw.ElapsedMilliseconds}");
                return default;
            }
#if RELEASE
            // 接收資料，效率寫法
            var rspData = await JsonSerializer.DeserializeAsync<T>(await rspMsg.Content.ReadAsStreamAsync());
#else
            // 接收資料，DEBUG寫法
            var rspText = await rspMsg.Content.ReadAsStringAsync();
            var rspData = JsonSerializer.Deserialize<T>(rspText);
#endif               

            var rspObj = new EipV1HttpGetRspMdl<T>()
            {
                RspData = rspData,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {(ex.InnerException == null ? string.Empty : ex.InnerException.Message)}");
            return default;
        }
    }

}