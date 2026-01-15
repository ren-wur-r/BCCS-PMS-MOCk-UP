using CommonLibrary;
using DataModelLibrary;
using DataModelLibrary.GlobalSystem.Config;
using DataModelLibrary.GlobalSystem.Env;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagerWebApi.Model;
using ServiceLibrary;

namespace ProjectManagerWebApi.Controllers;

/// <summary>版本</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class VersionController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<VersionController> _logger;

    /// <summary>主機環境</summary>
    private readonly IWebHostEnvironment _env;

    ///// <summary>全系統(社交工程控制)-環境參數</summary>
    //private readonly GsEnvironmentVariable _envVar;

    /// <summary>全系統(社交工程控制)-ApiAppSetting-設定</summary>
    private readonly GsApiAppSettingConfig _gsApiConfig;

    /// <summary>通用專案</summary>
    private readonly CommonLibraryProject _commonProject;

    /// <summary>資料模型專案</summary>
    private readonly DataModelLibraryProject _dataModelProject;

    /// <summary>服務專案</summary>
    private readonly ServiceLibraryProject _serviceProject;

    #region ProjectManager

    /// <summary>專案管理-程式</summary>
    private readonly ProjectManagerProgram _pmProgram;

    #endregion

    /// <summary>建構式</summary>
    public VersionController(
        ILogger<VersionController> logger,
        IWebHostEnvironment env,
        //GsEnvironmentVariable envVar,
        GsApiAppSettingConfig gsApiConfig,
        CommonLibraryProject commonProject,
        DataModelLibraryProject dataModelproject,
        ServiceLibraryProject serviceProject,
        // ProjectManager
        ProjectManagerProgram pmProgram
        )
    {
        this._logger = logger;
        this._env = env;
        //this._envVar = envVar;
        this._gsApiConfig = gsApiConfig;
        this._commonProject = commonProject;
        this._dataModelProject = dataModelproject;
        this._serviceProject = serviceProject;
        // ProjectManager
        this._pmProgram = pmProgram;
    }

    /// <summary>首頁</summary>
    [HttpGet]
    public IActionResult Index()
    {
        var rspObj = new
        {
            Env = this._env.EnvironmentName,
            this._pmProgram.Version,
            Program = this._pmProgram,
            CommonLibrary = this._commonProject.Version,
            DataModelLibrary = this._dataModelProject.Version,
            ServiceLibrary = this._serviceProject.Version,
        };
        return this.Ok(rspObj);
    }

    /// <summary>設定檔</summary>
    [HttpGet]
    public object Config()
    {
        if (this._env.EnvironmentName != GsEnvironmentConst.LOCAL
            && this._env.EnvironmentName != GsEnvironmentConst.DEVELOPMENT)
        {
            return "Gandalf: You Shall Not Pass!";
        }

        var rspObj = new
        {
            //EnvVar = this._envVar,
            Program = this._pmProgram,
            Config = this._gsApiConfig,
        };
        return rspObj;
    }

}
