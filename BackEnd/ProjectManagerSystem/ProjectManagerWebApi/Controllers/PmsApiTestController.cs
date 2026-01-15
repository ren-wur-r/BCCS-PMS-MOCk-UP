using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.EipV1.Http.Format;
using ServiceLibrary.EipV1.Http.Service;
using System.Text.Json;

namespace ProjectManagerWebApi.Controllers;

/// <summary>PMS API 測試控制器</summary>
[ApiController]
[Route("api/[controller]")]
public class PmsApiTestController : ControllerBase
{
    private readonly IEipV1HttpService _eipV1HttpService;
    private readonly ILogger<PmsApiTestController> _logger;

    public PmsApiTestController(
        IEipV1HttpService eipV1HttpService,
        ILogger<PmsApiTestController> logger)
    {
        _eipV1HttpService = eipV1HttpService;
        _logger = logger;

        // 診斷 Log
        _logger.LogInformation("PmsApiTestController 建構完成");
        _logger.LogInformation($"EipV1HttpService 是否為 null: {_eipV1HttpService == null}");
    }

    /// <summary>測試依賴注入狀態</summary>
    [HttpGet("debug")]
    public IActionResult DebugDependencies()
    {
        try
        {
            return Ok(new
            {
                EipV1HttpServiceIsNull = _eipV1HttpService == null,
                EipV1HttpServiceType = _eipV1HttpService?.GetType().Name,
                LoggerIsNull = _logger == null,
                Message = "依賴注入檢查完成",
                Timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                Error = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>測試查詢專案費用</summary>
    [HttpGet("expense")]
    public async Task<IActionResult> TestProjectExpense(
        [FromQuery] string projectCode = "DB24121101",
        [FromQuery] string projectName = "113-114年ISMS_苗縣教_大埔國小",
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            _logger.LogInformation("開始測試專案費用查詢");
            _logger.LogInformation($"EipV1HttpService 狀態: {(_eipV1HttpService == null ? "null" : "正常")}");

            if (_eipV1HttpService == null)
            {
                return BadRequest("EipV1HttpService 注入失敗");
            }

            var request = new EipV1HttpPmsGetProjectExpensesReqMdl
            {
                ProjectCode = projectCode,
                ProjectName = projectName,
                StartDate = startDate ?? new DateTime(2024, 1, 1),
                EndDate = endDate ?? new DateTime(2024, 12, 31)
            };

            _logger.LogInformation($"測試 PMS 費用查詢，參數：{JsonSerializer.Serialize(request)}");

            var result = await _eipV1HttpService.PmsGetProjectExpenseAsync(request);

            _logger.LogInformation($"PMS API 回應結果：{(result == null ? "null" : "有資料")}");

            if (result == null)
            {
                return BadRequest("PMS API 呼叫失敗，回傳 null");
            }

            return Ok(new
            {
                Success = true,
                Message = "PMS 費用查詢測試成功",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PMS 費用查詢測試發生錯誤");
            return StatusCode(500, new
            {
                Success = false,
                Message = $"測試失敗：{ex.Message}",
                Error = ex.ToString()
            });
        }
    }

    /// <summary>測試查詢專案旅費</summary>
    [HttpGet("travel")]
    public async Task<IActionResult> TestProjectTravel(
        [FromQuery] string projectCode = "DB24121101",
        [FromQuery] string projectName = "113-114年ISMS_苗縣教_大埔國小",
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            _logger.LogInformation("開始測試專案旅費查詢");
            _logger.LogInformation($"EipV1HttpService 狀態: {(_eipV1HttpService == null ? "null" : "正常")}");

            if (_eipV1HttpService == null)
            {
                return BadRequest("EipV1HttpService 注入失敗");
            }

            var request = new EipV1HttpPmsGetProjectTravelExpenseReqMdl
            {
                ProjectCode = projectCode,
                ProjectName = projectName,
                StartDate = startDate ?? new DateTime(2024, 1, 1),
                EndDate = endDate ?? new DateTime(2024, 12, 31)
            };

            _logger.LogInformation($"測試 PMS 旅費查詢，參數：{JsonSerializer.Serialize(request)}");

            var result = await _eipV1HttpService.PmsGetProjectTravelExpenseAsync(request);

            _logger.LogInformation($"PMS API 回應結果：{(result == null ? "null" : "有資料")}");

            if (result == null)
            {
                return BadRequest("PMS API 呼叫失敗，回傳 null");
            }

            return Ok(new
            {
                Success = true,
                Message = "PMS 旅費查詢測試成功",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PMS 旅費查詢測試發生錯誤");
            return StatusCode(500, new
            {
                Success = false,
                Message = $"測試失敗：{ex.Message}",
                Error = ex.ToString()
            });
        }
    }

    /// <summary>測試 API 連線狀態</summary>
    [HttpGet("health")]
    public IActionResult TestHealth()
    {
        return Ok(new
        {
            Success = true,
            Message = "PMS API 測試控制器運作正常",
            Timestamp = DateTime.Now,
            EipV1HttpServiceStatus = _eipV1HttpService == null ? "注入失敗" : "正常"
        });
    }
}
