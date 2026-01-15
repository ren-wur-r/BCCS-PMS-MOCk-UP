using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Region;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Region.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統-地區</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsSystemRegionController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemRegionController> _logger;

    /// <summary>管理者後台-系統-地區-邏輯服務</summary>
    private readonly IMbsSysRegionLogicalService _mbsSysRegionLogical;

    /// <summary>建構式</summary>
    public MbsSystemRegionController(
        ILogger<MbsSystemRegionController> logger,
        IMbsSysRegionLogicalService mbsSysRegionLogical)
    {
        this._logger = logger;
        this._mbsSysRegionLogical = mbsSysRegionLogical;
    }

    /// <summary>取得多筆</summary>
    [HttpPost]
    public async Task<MbsSysRgnCtlGetManyRspMdl> GetMany(MbsSysRgnCtlGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnCtlGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-地區-取得多筆
        var logicalReqObj = new MbsSysRgnLgcGetManyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRegionName = reqObj.ManagerRegionName?.Trim(),
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysRegionLogical.GetManyAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRegionList = logicalRspObj.ManagerRegionList
            ?.Select(x => new MbsSysRgnCtlGetManyRspItemMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerRegionIsEnable = x.ManagerRegionIsEnable
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>新增</summary>
    [HttpPost]
    public async Task<MbsSysRgnCtlAddRspMdl> Add(MbsSysRgnCtlAddReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnCtlAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-地區-新增
        var logicalReqObj = new MbsSysRgnLgcAddReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRegionName = reqObj.ManagerRegionName.Trim(),
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable
        };
        var logicalRspObj = await this._mbsSysRegionLogical.AddAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>更新</summary>
    [HttpPost]
    public async Task<MbsSysRgnCtlUpdateRspMdl> Update(MbsSysRgnCtlUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnCtlUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-地區-更新
        var logicalReqObj = new MbsSysRgnLgcUpdateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerRegionName = reqObj.ManagerRegionName?.Trim(),
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable
        };
        var logicalRspObj = await this._mbsSysRegionLogical.UpdateAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>取得單筆</summary>
    [HttpPost]
    public async Task<MbsSysRgnCtlGetRspMdl> Get(MbsSysRgnCtlGetReqMdl reqObj)
    {
        var rspObj = new MbsSysRgnCtlGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-地區-取得單筆
        var logicalReqObj = new MbsSysRgnLgcGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRegionID = reqObj.ManagerRegionID
        };
        var logicalRspObj = await this._mbsSysRegionLogical.GetAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerRegionName = logicalRspObj.ManagerRegionName;
        rspObj.ManagerRegionIsEnable = logicalRspObj.ManagerRegionIsEnable;
        return rspObj;
    }
}