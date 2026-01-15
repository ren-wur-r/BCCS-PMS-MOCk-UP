using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Department;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Department.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統-部門</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsSystemDepartmentController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemDepartmentController> _logger;

    /// <summary>管理者後台-系統-部門-邏輯服務</summary>
    private readonly IMbsSysDepartmentLogicalService _mbsSysDepartmentLogical;

    /// <summary>建構式</summary>
    public MbsSystemDepartmentController(
        ILogger<MbsSystemDepartmentController> logger,
        IMbsSysDepartmentLogicalService mbsSysDepartmentLogical)
    {
        this._logger = logger;
        this._mbsSysDepartmentLogical = mbsSysDepartmentLogical;
    }

    /// <summary>取得多筆</summary>
    [HttpPost]
    public async Task<MbsSysDptCtlGetManyRspMdl> GetMany(MbsSysDptCtlGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysDptCtlGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-部門-取得多筆
        var logicalReqObj = new MbsSysDptLgcGetManyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerDepartmentName = reqObj.ManagerDepartmentName?.Trim(),
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysDepartmentLogical.GetManyAsync(logicalReqObj);
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
        rspObj.ManagerDepartmentList = logicalRspObj.ManagerDepartmentList
            ?.Select(x => new MbsSysDptCtlGetManyRspItemMdl()
            {
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerDepartmentIsEnable = x.ManagerDepartmentIsEnable
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>新增</summary>
    [HttpPost]
    public async Task<MbsSysDptCtlAddRspMdl> Add(MbsSysDptCtlAddReqMdl reqObj)
    {
        var rspObj = new MbsSysDptCtlAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-部門-新增
        var logicalReqObj = new MbsSysDptLgcAddReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerDepartmentName = reqObj.ManagerDepartmentName.Trim(),
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable
        };
        var logicalRspObj = await this._mbsSysDepartmentLogical.AddAsync(logicalReqObj);
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
    public async Task<MbsSysDptCtlUpdateRspMdl> Update(MbsSysDptCtlUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysDptCtlUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-部門-更新
        var logicalReqObj = new MbsSysDptLgcUpdateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerDepartmentName = reqObj.ManagerDepartmentName?.Trim(),
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable
        };
        var logicalRspObj = await this._mbsSysDepartmentLogical.UpdateAsync(logicalReqObj);
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
    public async Task<MbsSysDptCtlGetRspMdl> Get(MbsSysDptCtlGetReqMdl reqObj)
    {
        var rspObj = new MbsSysDptCtlGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-部門-取得單筆
        var logicalReqObj = new MbsSysDptLgcGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerDepartmentID = reqObj.ManagerDepartmentID
        };
        var logicalRspObj = await this._mbsSysDepartmentLogical.GetAsync(logicalReqObj);
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
        rspObj.ManagerDepartmentName = logicalRspObj.ManagerDepartmentName;
        rspObj.ManagerDepartmentIsEnable = logicalRspObj.ManagerDepartmentIsEnable;
        return rspObj;
    }
}