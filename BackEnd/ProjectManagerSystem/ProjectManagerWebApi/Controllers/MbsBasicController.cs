using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.Basic;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.Basic.Format;
using ServiceLibrary.ManagerBackSite.Logical.Basic.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-基本</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsBasicController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsBasicController> _logger;

    /// <summary>管理者後台-基本-邏輯服務</summary>
    private readonly IMbsBasicLogicalService _mbsBasicLogical;

    /// <summary>建構式</summary>
    public MbsBasicController(
        ILogger<MbsBasicController> logger,
        IMbsBasicLogicalService mbsBasicLogical)
    {
        this._logger = logger;
        this._mbsBasicLogical = mbsBasicLogical;
    }

    /// <summary>登入</summary>
    [HttpPost]
    public async Task<MbsBscCtlLoginRspMdl> Login(MbsBscCtlLoginReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlLoginRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-登入
        var logicalReqObj = new MbsBscLgcLoginReqMdl()
        {
            EmployeeAccount = reqObj.EmployeeAccount.Trim(),
            EmployeePassword = reqObj.EmployeePassword.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
        };
        var logicalRspObj = await this._mbsBasicLogical.LoginAsync(logicalReqObj);
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
        rspObj.LoginToken = logicalRspObj.EmployeeLoginToken;
        return rspObj;
    }

    /// <summary>登出</summary>
    [HttpPost]
    public async Task<MbsBscCtlLogoutRspMdl> Logout(MbsBscCtlLogoutReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlLogoutRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-登出
        var logicalReqObj = new MbsBscLgcLogoutReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
        };
        var logicalRspObj = await this._mbsBasicLogical.LogoutAsync(logicalReqObj);
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

    /// <summary>心跳</summary>
    [HttpPost]
    public async Task<MbsBscCtlHeartbeatRspMdl> Heartbeat(MbsBscCtlHeartbeatReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlHeartbeatRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-心跳
        var logicalReqObj = new MbsBscLgcHeartbeatReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
        };
        var logicalRspObj = await this._mbsBasicLogical.HeartbeatAsync(logicalReqObj);
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

    #region Employee

    /// <summary>取得員工資訊</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetEmployeeRspMdl> GetEmployee(MbsBscCtlGetEmployeeReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetEmployeeRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得員工資訊
        var logicalReqObj = new MbsBscLgcGetEmployeeReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetEmployeeAsync(logicalReqObj);
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
        rspObj.EmployeeName = logicalRspObj.EmployeeName;
        rspObj.ManagerBackSiteMenuPermissionList = logicalRspObj.ManagerBackSiteMenuPermissionList
            .Select(x => new MbsBscCtlGetEmployeeRspItemMdl()
            {
                AtomMenu = x.AtomMenu,
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
            })
            .ToList();
        rspObj.EmployeeAccount = logicalRspObj.EmployeeAccount;
        rspObj.ManagerRoleID = logicalRspObj.ManagerRoleID;
        rspObj.ManagerRoleName = logicalRspObj.ManagerRoleName;
        rspObj.ManagerRegionID = logicalRspObj.ManagerRegionID;
        rspObj.ManagerRegionName = logicalRspObj.ManagerRegionName;
        rspObj.ManagerDepartmentID = logicalRspObj.ManagerDepartmentID;
        rspObj.ManagerDepartmentName = logicalRspObj.ManagerDepartmentName;
        return rspObj;
    }

    /// <summary>修改員工密碼</summary>
    [HttpPost]
    public async Task<MbsBscCtlUpdatePasswordRspMdl> UpdateEmployeePassword(MbsBscCtlUpdatePasswordReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlUpdatePasswordRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-修改員工密碼
        var logicalReqObj = new MbsBscLgcUpdatePasswordReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            OldPassword = reqObj.OldPassword,
            NewPassword = reqObj.NewPassword,
        };
        var logicalRspObj = await this._mbsBasicLogical.UpdateEmployeePasswordAsync(logicalReqObj);
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

    /// <summary>取得多筆員工資訊</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyEmployeeInfoRspMdl> GetManyEmployeeInfo(MbsBscCtlGetManyEmployeeInfoReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyEmployeeInfoRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆員工資訊
        var logicalReqObj = new MbsBscLgcGetManyEmployeeInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRegionID = reqObj.ManagerRegionID,
            ManagerDepartmentID = reqObj.ManagerDepartmentID,
            ManagerRoleID = reqObj.ManagerRoleID,
            EmployeeName = reqObj.EmployeeName,
            EmployeeIsEnable = reqObj.EmployeeIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyEmployeeInfoAsync(logicalReqObj);
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
        rspObj.EmployeeList = logicalRspObj.EmployeeList
            .Select(x => new MbsBscCtlGetManyEmployeeInfoRspItemMdl()
            {
                EmployeeID = x.EmployeeID,
                EmployeeName = x.EmployeeName,
                EmployeeIsEnable = x.EmployeeIsEnable,
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProjectMember

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyEmployeeProjectMemberRspMdl> GetManyEmployeeProjectMember(MbsBscCtlGetManyEmployeeProjectMemberReqMdl reqObj)
    {
        // 預設回應
        var rspObj = new MbsBscCtlGetManyEmployeeProjectMemberRspMdl()
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
        };

        // logical, 管理者後台-基本-邏輯-取得多筆員工專案成員
        var logicalReqObj = new MbsBscLgcGetManyEmployeeProjectMemberReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyEmployeeProjectMemberAsync(logicalReqObj);
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
        rspObj.EmployeeProjectMemberList = logicalRspObj.EmployeeProjectMemberList
            .Select(x => new MbsBscCtlGetManyEmployeeProjectMemberRspItemMdl()
            {
                EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
                EmployeeID = x.EmployeeID,
                EmployeeName = x.EmployeeName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProject

    /// <summary>取得多筆員工專案</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProjectRspMdl> GetManyEmployeeProject(MbsBscCtlGetManyProjectReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆員工專案
        var logicalReqObj = new MbsBscLgcGetManyProjectReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectName = reqObj.EmployeeProjectName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyEmployeeProjectAsync(logicalReqObj);
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
        rspObj.EmployeeProjectList = logicalRspObj.EmployeeProjectList
            ?.Select(x => new MbsBscCtlGetManyProjectRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                EmployeeProjectName = x.EmployeeProjectName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region EmployeeProjectStone

    /// <summary>取得多筆員工專案里程碑</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProjectStoneRspMdl> GetManyEmployeeProjectStone(MbsBscCtlGetManyProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProjectStoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆員工專案里程碑
        var logicalReqObj = new MbsBscLgcGetManyProjectStoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneName = reqObj.EmployeeProjectStoneName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyEmployeeProjectStoneAsync(logicalReqObj);
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
        rspObj.EmployeeProjectStoneList = logicalRspObj.EmployeeProjectStoneList
            ?.Select(x => new MbsBscCtlGetManyProjectStoneRspItemMdl()
            {
                EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                EmployeeProjectStoneName = x.EmployeeProjectStoneName,
            })
            .ToList();
        return rspObj;
    }
    #endregion

    #region EmployeeProjectStoneJob

    /// <summary>取得多筆員工專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProjectStoneJobRspMdl> GetManyEmployeeProjectStoneJob(MbsBscCtlGetManyProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆員工專案里程碑工項
        var logicalReqObj = new MbsBscLgcGetManyProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobName = reqObj.EmployeeProjectStoneJobName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyEmployeeProjectStoneJobAsync(logicalReqObj);
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
        rspObj.EmployeeProjectStoneJobList = logicalRspObj.EmployeeProjectStoneJobList
            .Select(x => new MbsBscCtlGetManyProjectStoneJobRspItemMdl()
            {
                EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerDepartment

    /// <summary>取得多筆管理者部門</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyDepartmentRspMdl> GetManyManagerDepartment(MbsBscCtlGetManyDepartmentReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyDepartmentRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆管理者部門
        var logicalReqObj = new MbsBscLgcGetManyDepartmentReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerDepartmentName = reqObj.ManagerDepartmentName?.Trim(),
            ManagerDepartmentIsEnable = reqObj.ManagerDepartmentIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerDepartmentAsync(logicalReqObj);
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
            .Select(x => new MbsBscCtlGetManyDepartmentRspItemMdl()
            {
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerDepartmentIsEnable = x.ManagerDepartmentIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得全部管理者部門</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetAllDepartmentRspMdl> GetAllManagerDepartment(MbsBscCtlGetAllDepartmentReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetAllDepartmentRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得全部管理者部門
        var logicalReqObj = new MbsBscLgcGetAllDepartmentReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
        };
        var logicalRspObj = await this._mbsBasicLogical.GetAllManagerDepartmentAsync(logicalReqObj);
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
            .Select(x => new MbsBscCtlGetAllDepartmentRspItemMdl()
            {
                ManagerDepartmentID = x.ManagerDepartmentID,
                ManagerDepartmentName = x.ManagerDepartmentName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerRegion

    /// <summary>取得多筆管理者地區</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyRegionRspMdl> GetManyManagerRegion(MbsBscCtlGetManyRegionReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyRegionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆管理者地區
        var logicalReqObj = new MbsBscLgcGetManyRegionReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRegionName = reqObj.ManagerRegionName?.Trim(),
            ManagerRegionIsEnable = reqObj.ManagerRegionIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerRegionAsync(logicalReqObj);
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
            .Select(x => new MbsBscCtlGetManyRegionRspItemMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerRegionIsEnable = x.ManagerRegionIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得全部管理者地區</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetAllRegionRspMdl> GetAllManagerRegion(MbsBscCtlGetAllRegionReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetAllRegionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得全部管理者地區
        var logicalReqObj = new MbsBscLgcGetAllRegionReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
        };
        var logicalRspObj = await this._mbsBasicLogical.GetAllManagerRegionAsync(logicalReqObj);
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
            ?.Select(x => new MbsBscCtlGetAllRegionRspItemMdl()
            {
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
            })
            .ToList();
        return rspObj;
    }


    #endregion

    #region ManagerRole

    /// <summary>取得多筆管理者角色</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyRoleRspMdl> GetManyManagerRole(MbsBscCtlGetManyRoleReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyRoleRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆管理者角色
        var logicalReqObj = new MbsBscLgcGetManyRoleReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRoleName = reqObj.ManagerRoleName?.Trim(),
            ManagerRoleIsEnable = reqObj.ManagerRoleIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerRoleAsync(logicalReqObj);
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
            .Select(x => new MbsBscCtlGetManyRoleRspItemMdl()
            {
                ManagerRoleID = x.ManagerRoleID,
                ManagerRoleName = x.ManagerRoleName,
                ManagerRegionID = x.ManagerRegionID,
                ManagerRegionName = x.ManagerRegionName,
                ManagerDepartmentName = x.ManagerDepartmentName,
                ManagerRoleIsEnable = x.ManagerRoleIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerRolePermission

    /// <summary>取得多筆角色權限從[角色ID]</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyRolePermissionFromRoleIdRspMdl> GetManyRolePermissionFromRoleId(MbsBscCtlGetManyRolePermissionFromRoleIdReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyRolePermissionFromRoleIdRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆角色權限從[管理者後台-基本-角色ID]
        var logicalReqObj = new MbsBscLgcGetManyRolePermissionFromRoleIdReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerRoleID = reqObj.ManagerRoleID,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyRolePermissionFromRoleAsync(logicalReqObj);
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
        rspObj.RolePermissionList = logicalRspObj.ManagerRolePermissionList
            ?.Select(x => new MbsBscCtlGetManyRolePermissionFromRoleIdRspItemMdl()
            {
                AtomMenu = x.AtomMenu,
                ManagerRegionID = x.ManagerRegionID,
                AtomPermissionKindIdView = x.AtomPermissionKindIdView,
                AtomPermissionKindIdCreate = x.AtomPermissionKindIdCreate,
                AtomPermissionKindIdEdit = x.AtomPermissionKindIdEdit,
                AtomPermissionKindIdDelete = x.AtomPermissionKindIdDelete,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerProduct

    /// <summary>取得多筆產品主分類</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProductMainKindRspMdl> GetManyProductMainKind(MbsBscCtlGetManyProductMainKindReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProductMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆產品主分類
        var logicalReqObj = new MbsBscLgcGetManyProductMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindName = reqObj.ProductMainKindName?.Trim(),
            ManagerProductMainKindIsEnable = reqObj.ProductMainKindIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerProductMainKindAsync(logicalReqObj);
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
        rspObj.ProductMainKindList = logicalRspObj.ManagerProductMainKindList
            ?.Select(x => new MbsBscCtlGetManyProductMainKindRspItemMdl()
            {
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductMainKindIsEnable = x.ManagerProductMainKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得多筆產品子分類</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProductSubKindRspMdl> GetManyProductSubKind(MbsBscCtlGetManyProductSubKindReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProductSubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆產品子分類
        var logicalReqObj = new MbsBscLgcGetManyProductSubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ProductSubKindName?.Trim(),
            ManagerProductSubKindIsEnable = reqObj.ProductSubKindIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerProductSubKindAsync(logicalReqObj);
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
        rspObj.ProductSubKindList = logicalRspObj.ManagerProductSubKindList
            ?.Select(x => new MbsBscCtlGetManyProductSubKindRspItemMdl()
            {
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductSubKindIsEnable = x.ManagerProductSubKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得多筆產品</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProductRspMdl> GetManyProduct(MbsBscCtlGetManyProductReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆產品
        var logicalReqObj = new MbsBscLgcGetManyProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductName = reqObj.ManagerProductName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyProductAsync(logicalReqObj);
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
        rspObj.ProductList = logicalRspObj.ManagerProductList
            ?.Select(x => new MbsBscCtlGetManyProductRspItemMdl()
            {
                ManagerProductID = x.ManagerProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductIsEnable = x.ManagerProductIsEnable,
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductMainKindCommissionRate = x.ManagerProductMainKindCommissionRate,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得多筆產品規格</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyProductSpecificationRspMdl> GetManyProductSpecification(MbsBscCtlGetManyProductSpecificationReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyProductSpecificationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆產品規格
        var logicalReqObj = new MbsBscLgcGetManyProductSpecificationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationName = reqObj.ProductSpecificationName?.Trim(),
            ManagerProductSpecificationIsEnable = reqObj.ProductSpecificationIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyProductSpecificationAsync(logicalReqObj);
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
        rspObj.ProductSpecificationList = logicalRspObj.ManagerProductSpecificationList
            .Select(x => new MbsBscCtlGetManyProductSpecificationRspItemMdl()
            {
                ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerContacterRatingReason

    /// <summary>取得多筆窗口開發評等原因</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyContacterRatingReasonRspMdl> GetManyManagerContacterRatingReason(MbsBscCtlGetManyContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyContacterRatingReasonRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆窗口開發評等原因
        var logicalReqObj = new MbsBscLgcGetManyContacterRatingReasonReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName?.Trim(),
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerContacterRatingReasonAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingReasonList = logicalRspObj.ManagerContacterRatingReasonList
            .Select(x => new MbsBscCtlGetManyContacterRatingReasonRspItemMdl()
            {
                ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                ManagerContacterRatingReasonName = x.ManagerContacterRatingReasonName,
                ManagerContacterRatingReasonStatus = x.ManagerContacterRatingReasonStatus,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerCompanyMainKind

    /// <summary>取得多筆公司主分類</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyCompanyMainKindRspMdl> GetManyManagerCompanyMainKind(MbsBscCtlGetManyCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyCompanyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆公司主分類
        var logicalReqObj = new MbsBscLgcGetManyCompanyMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerCompanyMainKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanyMainKindList = logicalRspObj.ManagerCompanyMainKindList
            .Select(x => new MbsBscCtlGetManyCompanyMainKindRspItemMdl()
            {
                ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                ManagerCompanyMainKindName = x.ManagerCompanyMainKindName,
                ManagerCompanyMainKindIsEnable = x.ManagerCompanyMainKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerCompanySubKind

    /// <summary>取得多筆公司子分類</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyCompanySubKindRspMdl> GetManyManagerCompanySubKind(MbsBscCtlGetManyCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyCompanySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆公司子分類
        var logicalReqObj = new MbsBscLgcGetManyCompanySubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName?.Trim(),
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerCompanySubKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanySubKindList = logicalRspObj.ManagerCompanySubKindList
            .Select(x => new MbsBscCtlGetManyCompanySubKindRspItemMdl()
            {
                ManagerCompanySubKindID = x.ManagerCompanySubKindID,
                ManagerCompanySubKindName = x.ManagerCompanySubKindName,
                ManagerCompanySubKindIsEnable = x.ManagerCompanySubKindIsEnable,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region ManagerContacter

    /// <summary>取得多筆管理者窗口</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyManagerContacterRspMdl> GetManyManagerContacter(MbsBscCtlGetManyManagerContacterReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyManagerContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆窗口
        var logicalReqObj = new MbsBscLgcGetManyManagerContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerContacterAsync(logicalReqObj);
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
        rspObj.ManagerContacterList = logicalRspObj.ManagerContacterList
            .Select(x => new MbsBscCtlGetManyContacterRspItemMdl()
            {
                ManagerContacterID = x.ManagerContacterID,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得管理者窗口</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManagerContacterRspMdl> GetManagerContacter(MbsBscCtlGetManagerContacterReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManagerContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsBscLgcGetManagerContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterID = reqObj.ManagerContacterID,
        };

        var logicalRspObj = await this._mbsBasicLogical.GetManagerContacterAsync(logicalReqObj);
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
        rspObj.ManagerContacterID = logicalRspObj.ManagerContacterID;
        rspObj.ManagerCompanyID = logicalRspObj.ManagerCompanyID;
        rspObj.ManagerContacterName = logicalRspObj.ManagerContacterName;
        rspObj.ManagerContacterEmail = logicalRspObj.ManagerContacterEmail;
        rspObj.ManagerContacterPhone = logicalRspObj.ManagerContacterPhone;
        rspObj.ManagerContacterDepartment = logicalRspObj.ManagerContacterDepartment;
        rspObj.ManagerContacterJobTitle = logicalRspObj.ManagerContacterJobTitle;
        rspObj.ManagerContacterTelephone = logicalRspObj.ManagerContacterTelephone;
        rspObj.ManagerContacterStatus = logicalRspObj.ManagerContacterStatus;
        rspObj.ManagerContacterIsConsent = logicalRspObj.ManagerContacterIsConsent;
        rspObj.AtomRatingKind = logicalRspObj.AtomRatingKind;
        rspObj.ManagerContacterCreateEmployeeID = logicalRspObj.ManagerContacterCreateEmployeeID;
        rspObj.ManagerContacterRemark = logicalRspObj.ManagerContacterRemark;
        return rspObj;
    }

    #endregion

    #region ManagerCompany

    /// <summary>取得多筆管理者公司</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyCompanyRspMdl> GetManyManagerCompany(MbsBscCtlGetManyCompanyReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆管理者公司
        var logicalReqObj = new MbsBscLgcGetManyCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerCompanyAsync(logicalReqObj);
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
        rspObj.ManagerCompanyList = logicalRspObj.ManagerCompanyList
            .Select(x => new MbsBscCtlGetManyCompanyRspItemMdl()
            {
                ManagerCompanyID = x.ManagerCompanyID,
                ManagerCompanyName = x.ManagerCompanyName,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得多筆公司名稱從[商機原始]</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyCompanyNameFromPipelineOriginalRspMdl> GetManyManagerCompanyNameFromPipelineOriginal(MbsBscCtlGetManyCompanyNameFromPipelineOriginalReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyCompanyNameFromPipelineOriginalRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-基本-邏輯-取得多筆公司名稱從[商機原始]
        var logicalReqObj = new MbsBscLgcGetManyCompanyNameFromPipelineOriginalReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };
        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerCompanyNameFromPipelineOriginalAsync(logicalReqObj);
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
        rspObj.ManagerCompanyNameList = logicalRspObj.ManagerCompanyNameList;
        return rspObj;
    }

    /// <summary>取得管理者公司</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManagerCompanyRspMdl> GetManagerCompany(MbsBscCtlGetManagerCompanyReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManagerCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsBscLgcGetCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };

        var logicalRspObj = await this._mbsBasicLogical.GetManagerCompanyAsync(logicalReqObj);
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
        rspObj.ManagerCompanyID = logicalRspObj.ManagerCompanyID;
        rspObj.ManagerCompanyUnifiedNumber = logicalRspObj.ManagerCompanyUnifiedNumber;
        rspObj.ManagerCompanyName = logicalRspObj.ManagerCompanyName;
        rspObj.AtomManagerCompanyStatus = logicalRspObj.AtomManagerCompanyStatus;
        rspObj.ManagerCompanyMainKindID = logicalRspObj.ManagerCompanyMainKindID;
        rspObj.ManagerCompanyMainKindName = logicalRspObj.ManagerCompanyMainKindName;
        rspObj.ManagerCompanySubKindID = logicalRspObj.ManagerCompanySubKindID;
        rspObj.ManagerCompanySubKindName = logicalRspObj.ManagerCompanySubKindName;
        rspObj.AtomCustomerGrade = logicalRspObj.AtomCustomerGrade;
        rspObj.AtomSecurityGrade = logicalRspObj.AtomSecurityGrade;
        rspObj.AtomEmployeeRange = logicalRspObj.AtomEmployeeRange;
        return rspObj;
    }

    #endregion

    #region ManagerCompanyLocation

    /// <summary>取得多筆管理者公司營業地點</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManyCompanyLocationRspMdl> GetManyManagerCompanyLocation(MbsBscCtlGetManyCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManyCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsBscLgcGetManyCompanyLocationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationName = reqObj.ManagerCompanyLocationName?.Trim(),
            PageIndex = reqObj.PageIndex,
        };

        var logicalRspObj = await this._mbsBasicLogical.GetManyManagerCompanyLocationAsync(logicalReqObj);
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
        rspObj.CompanyLocationList = logicalRspObj.ManagerCompanyLocationList.Select(x => new MbsBscCtlGetManyCompanyLocationRspItemMdl
        {
            ManagerCompanyLocationID = x.ManagerCompanyLocationID,
            ManagerCompanyLocationName = x.ManagerCompanyLocationName,
            ManagerCompanyLocationIsDeleted = x.ManagerCompanyLocationIsDeleted,
        }).ToList();
        return rspObj;
    }

    /// <summary>取得管理者公司營業地點</summary>
    [HttpPost]
    public async Task<MbsBscCtlGetManagerCompanyLocationRspMdl> GetManagerCompanyLocation(MbsBscCtlGetManagerCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsBscCtlGetManagerCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsBscLgcGetCompanyLocationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            ManagerCompanyLocationIsDeleted = reqObj.ManagerCompanyLocationIsDeleted,
        };

        var logicalRspObj = await this._mbsBasicLogical.GetManagerCompanyLocationAsync(logicalReqObj);
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
        rspObj.ManagerCompanyLocationID = logicalRspObj.ManagerCompanyLocationID;
        rspObj.ManagerCompanyID = logicalRspObj.ManagerCompanyID;
        rspObj.ManagerCompanyLocationName = logicalRspObj.ManagerCompanyLocationName;
        rspObj.AtomCity = logicalRspObj.AtomCity;
        rspObj.ManagerCompanyLocationAddress = logicalRspObj.ManagerCompanyLocationAddress;
        rspObj.ManagerCompanyLocationTelephone = logicalRspObj.ManagerCompanyLocationTelephone;
        rspObj.ManagerCompanyLocationStatus = logicalRspObj.ManagerCompanyLocationStatus;
        return rspObj;
    }

    #endregion
}
