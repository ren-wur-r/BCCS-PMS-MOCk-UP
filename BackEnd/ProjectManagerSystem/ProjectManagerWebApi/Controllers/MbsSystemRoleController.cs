using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Role;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Role.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統-角色</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsSystemRoleController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemRoleController> _logger;

    /// <summary>管理者後台-系統-角色-邏輯服務</summary>
    private readonly IMbsSysRoleLogicalService _mbsSysRoleLogical;

    /// <summary>建構式</summary>
    public MbsSystemRoleController(
        ILogger<MbsSystemRoleController> logger,
        IMbsSysRoleLogicalService mbsSysRoleLogical)
    {
        this._logger = logger;
        this._mbsSysRoleLogical = mbsSysRoleLogical;
    }

    /// <summary>取得多筆</summary>
    [HttpPost]
    public async Task<MbsSysRolCtlGetManyRspMdl> GetMany(MbsSysRolCtlGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysRolCtlGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-角色-取得多筆
        var logicalReqObj = new MbsSysRolLgcGetManyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRoleName = reqObj.ManagerRoleName,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysRoleLogical.GetManyAsync(logicalReqObj);
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
        rspObj.ManagerRoleList = logicalRspObj.ManagerRoleList
            ?.Select(x => new MbsSysRolCtlGetManyRspItemMdl()
            {
                ManagerRoleID = x.ManagerRoleID,
                ManagerRoleName = x.ManagerRoleName,
                EmployeeCount = x.EmployeeCount,
                ManagerRoleIsEnable = x.ManagerRoleIsEnable
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>新增</summary>
    [HttpPost]
    public async Task<MbsSysRolCtlAddRspMdl> Add(MbsSysRolCtlAddReqMdl reqObj)
    {
        var rspObj = new MbsSysRolCtlAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-角色-新增
        var logicalReqObj = new MbsSysRolLgcAddReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRoleName = reqObj.ManagerRoleName.Trim(),
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleRemark = reqObj.ManagerRoleRemark,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
            ManagerRolePermissionList = reqObj.ManagerRolePermissionList
                ?.Select(x => new MbsSysRolLgcAddItemReqMdl()
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
        var logicalRspObj = await this._mbsSysRoleLogical.AddAsync(logicalReqObj);
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
        rspObj.ManagerRoleID = logicalRspObj.ManagerRoleID;
        return rspObj;
    }

    /// <summary>更新</summary>
    [HttpPost]
    public async Task<MbsSysRolCtlUpdateRspMdl> Update(MbsSysRolCtlUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysRolCtlUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-角色-更新
        var logicalReqObj = new MbsSysRolLgcUpdateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRoleID = reqObj.ManagerRoleID,
            ManagerRoleName = reqObj.ManagerRoleName?.Trim(),
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleRemark = reqObj.ManagerRoleRemark,
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
            ManagerRolePermissionList = reqObj.ManagerRolePermissionList
                ?.Select(x => new MbsSysRolLgcUpdateItemReqMdl()
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
        var logicalRspObj = await this._mbsSysRoleLogical.UpdateAsync(logicalReqObj);
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
    public async Task<MbsSysRolCtlGetRspMdl> Get(MbsSysRolCtlGetReqMdl reqObj)
    {
        var rspObj = new MbsSysRolCtlGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-角色-取得單筆
        var logicalReqObj = new MbsSysRolLgcGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRoleID = reqObj.ManagerRoleID
        };
        var logicalRspObj = await this._mbsSysRoleLogical.GetAsync(logicalReqObj);
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
        rspObj.ManagerRoleName = logicalRspObj.ManagerRoleName;
        rspObj.ManagerRegionID = logicalRspObj.ManagerRegionID;
        rspObj.ManagerRegionName = logicalRspObj.ManagerRegionName;
        rspObj.ManagerDepartmentID = logicalRspObj.ManagerDepartmentID;
        rspObj.ManagerDepartmentName = logicalRspObj.ManagerDepartmentName;
        rspObj.ManagerRoleRemark = logicalRspObj.ManagerRoleRemark;
        rspObj.ManagerRoleIsEnable = logicalRspObj.ManagerRoleIsEnable;
        rspObj.EmployeeCount = logicalRspObj.EmployeeCount;
        rspObj.ManagerRolePermissionList = logicalRspObj.ManagerRolePermissionList
            ?.Select(x => new MbsSysRolCtlGetRspItemMdl()
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