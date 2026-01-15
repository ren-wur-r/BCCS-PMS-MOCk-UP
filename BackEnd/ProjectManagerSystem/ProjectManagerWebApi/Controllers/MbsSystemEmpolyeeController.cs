using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Employee;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Employee.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統-員工</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsSystemEmployeeController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemEmployeeController> _logger;

    /// <summary>管理者後台-系統-員工-邏輯服務</summary>
    private readonly IMbsSysEmployeeLogicalService _mbsSysEmployeeLogical;

    /// <summary>建構式</summary>
    public MbsSystemEmployeeController(
        ILogger<MbsSystemEmployeeController> logger,
        IMbsSysEmployeeLogicalService mbsSysEmployeeLogical)
    {
        this._logger = logger;
        this._mbsSysEmployeeLogical = mbsSysEmployeeLogical;
    }

    /// <summary>取得多筆</summary>
    [HttpPost]
    public async Task<MbsSysEmpCtlGetManyRspMdl> GetMany(MbsSysEmpCtlGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpCtlGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-員工-取得多筆
        var logicalReqObj = new MbsSysEmpLgcGetManyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
            EmployeeAccount = reqObj.EmployeeAccount,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysEmployeeLogical.GetManyAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeList = logicalRspObj.EmployeeList
            ?.Select(x => new MbsSysEmpCtlGetManyRspItemMdl()
            {
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerRoleName = x.ManagerRoleName,
                EmployeeID = x.EmployeeID,
                EmployeeName = x.EmployeeName,
                EmployeeAccount = x.EmployeeAccount,
                EmployeeIsEnable = x.EmployeeIsEnable
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>新增</summary>
    [HttpPost]
    public async Task<MbsSysEmpCtlAddRspMdl> Add(MbsSysEmpCtlAddReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpCtlAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-員工-新增
        var logicalReqObj = new MbsSysEmpLgcAddReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeAccount = reqObj.EmployeeAccount.Trim(),
            EmployeePassword = reqObj.EmployeePassword.Trim(),
            EmployeeName = reqObj.EmployeeName.Trim(),
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
            EmployeePermissionList = reqObj.EmployeePermissionList
                ?.Select(x => new MbsSysEmpLgcAddItemReqMdl()
                {
                    AtomMenu = x.AtomMenu,
                    ManagerRegionID = x.ManagerRegionID,
                    AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                    AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                    AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                    AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete
                })
                .ToList()
        };
        var logicalRspObj = await this._mbsSysEmployeeLogical.AddAsync(logicalReqObj);
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
        rspObj.EmployeeID = logicalRspObj.EmployeeID;
        return rspObj;
    }

    /// <summary>更新</summary>
    [HttpPost]
    public async Task<MbsSysEmpCtlUpdateRspMdl> Update(MbsSysEmpCtlUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpCtlUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-員工-更新
        var logicalReqObj = new MbsSysEmpLgcUpdateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeID = reqObj.EmployeeID,
            EmployeeName = reqObj.EmployeeName?.Trim(),
            EmployeePassword = reqObj.EmployeePassword?.Trim(),
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
            EmployeePermissionList = reqObj.EmployeePermissionList
                ?.Select(x => new MbsSysEmpLgcUpdateItemReqMdl()
                {
                    AtomMenu = x.AtomMenu,
                    ManagerRegionID = x.ManagerRegionID,
                    AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                    AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                    AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                    AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete
                })
                .ToList()
        };
        var logicalRspObj = await this._mbsSysEmployeeLogical.UpdateAsync(logicalReqObj);
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
    public async Task<MbsSysEmpCtlGetRspMdl> Get(MbsSysEmpCtlGetReqMdl reqObj)
    {
        var rspObj = new MbsSysEmpCtlGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-員工-取得單筆
        var logicalReqObj = new MbsSysEmpLgcGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeID = reqObj.EmployeeID
        };
        var logicalRspObj = await this._mbsSysEmployeeLogical.GetAsync(logicalReqObj);
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
        rspObj.EmployeeAccount = logicalRspObj.EmployeeAccount;
        rspObj.EmployeeEmail = logicalRspObj.EmployeeEmail;
        rspObj.EmployeeName = logicalRspObj.EmployeeName;
        rspObj.ManagerRoleID = logicalRspObj.ManagerRoleID;
        rspObj.ManagerRoleName = logicalRspObj.ManagerRoleName;
        rspObj.ManagerRegionID = logicalRspObj.ManagerRegionID;
        rspObj.ManagerRegionName = logicalRspObj.ManagerRegionName;
        rspObj.ManagerDepartmentName = logicalRspObj.ManagerDepartmentName;
        rspObj.EmployeeIsEnable = logicalRspObj.EmployeeIsEnable;
        rspObj.EmployeePermissionList = logicalRspObj.EmployeePermissionList
            ?.Select(x => new MbsSysEmpCtlGetRspItemMdl()
            {
                AtomMenu = x.AtomMenu,
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete
            })
            .ToList();
        return rspObj;
    }
}