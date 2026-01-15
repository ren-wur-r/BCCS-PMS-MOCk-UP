using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.CmnSMTP.Config;
using CommonLibrary.CmnSMTP.Format;
using CommonLibrary.CmnSMTP.Service;
using DataModelLibrary.GlobalSystem.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Schedule.Logical.Timer.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>測試</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<TestController> _logger;

    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>全域-ApiAppSetting-設定</summary>
    private readonly GsApiAppSettingConfig _gsApiAppSettingConfig;

    /// <summary>通用-SMTP-服務</summary>
    private readonly ICmnSmtpService _cmnSmtpHttp;

    /// <summary>排程-計時器-邏輯服務</summary>
    private readonly ISchTimerLogicalService _schTimerLogical;

    /// <summary>建構式</summary>
    public TestController(
        ILogger<TestController> logger,
        ICoEmpInfoDbService coEmpInfoDb,
        GsApiAppSettingConfig gsApiAppSettingConfig,
        ICmnSmtpService cmnSmtpHttp,
        ISchTimerLogicalService schTimerLogical
        )
    {
        this._logger = logger;
        this._coEmpInfoDb = coEmpInfoDb;
        this._gsApiAppSettingConfig = gsApiAppSettingConfig;
        this._cmnSmtpHttp = cmnSmtpHttp;
        this._schTimerLogical = schTimerLogical;
    }

    /// <summary>測試-發送Email</summary>
    [HttpGet]
    public async Task<object> SendEmailAsync()
    {
        // 發送信件
        var smtpConfig = new CmnSmtpConfig
        {
            Host = this._gsApiAppSettingConfig.SmtpConfig.Host,
            Port = this._gsApiAppSettingConfig.SmtpConfig.Port,
            EnableSsl = this._gsApiAppSettingConfig.SmtpConfig.EnableSsl,
            SenderEmail = this._gsApiAppSettingConfig.SmtpConfig.FromEmail,
            SenderName = this._gsApiAppSettingConfig.SmtpConfig.FromName,
            Username = this._gsApiAppSettingConfig.SmtpConfig.Username,
            Password = this._gsApiAppSettingConfig.SmtpConfig.Password
        };
        var cmnSmtpSendReqObj = new CmnSmtpSendReqMdl()
        {
            Config = smtpConfig,
            ReceiverList = new List<string>() { "" },
            Subject = "test smtp config",
            Body = "test smtp config",
            IsHtml = true,
        };

        var cmnSmtpSendRspObj = await this._cmnSmtpHttp.SendAsync(cmnSmtpSendReqObj);
        return new { cmnSmtpSendRspObj };
    }

    /// <summary>測試-更新專案狀態</summary>
    [HttpGet]
    public async Task UpdateProjectStatusAsync()
    {
        await this._schTimerLogical.UpdateProjectStatusAsync();
    }

}