using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統設定-窗口</summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class MbsSystemContacterController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemContacterController> _logger;

    /// <summary>管理者後台-系統設定-窗口-邏輯服務</summary>
    private readonly IMbsSysCtrLogicalService _mbsSysCtrLogical;

    /// <summary>建構式</summary>
    public MbsSystemContacterController(
        ILogger<MbsSystemContacterController> logger,
        IMbsSysCtrLogicalService mbsSysCtrLogical)
    {
        this._logger = logger;
        this._mbsSysCtrLogical = mbsSysCtrLogical;
    }

    #region 窗口

    /// <summary>取得多筆窗口</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlGetManyContacterRspMdl> GetManyContacter(MbsSysCtrCtlGetManyContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlGetManyContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-取得多筆窗口
        var logicalReqObj = new MbsSysCtrLgcGetManyContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.GetManyContacterAsync(logicalReqObj);
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
        rspObj.ManagerContacterList = logicalRspObj.ManagerContacterList?
            .Select(x => new MbsSysCtrCtlGetManyContacterRspItemMdl()
            {
                ManagerContacterID = x.ManagerContacterID,
                ManagerCompanyName = x.ManagerCompanyName,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail,
                ManagerContacterPhone = x.ManagerContacterPhone,
                ManagerContacterDepartment = x.ManagerContacterDepartment,
                ManagerContacterJobTitle = x.ManagerContacterJobTitle,
                ManagerContacterRatingKind = x.ManagerContacterRatingKind,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得單筆窗口</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlGetContacterRspMdl> GetContacter(MbsSysCtrCtlGetContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlGetContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-取得單筆窗口
        var logicalReqObj = new MbsSysCtrLgcGetContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterID = reqObj.ManagerContacterID,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.GetContacterAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingKind = logicalRspObj.ManagerContacterRatingKind;
        rspObj.ManagerContacterCreateEmployeeID = logicalRspObj.ManagerContacterCreateEmployeeID;
        rspObj.ManagerContacterRemark = logicalRspObj.ManagerContacterRemark;
        rspObj.ManagerCompanyName = logicalRspObj.ManagerCompanyName;
        return rspObj;
    }

    /// <summary>新增窗口</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlAddContacterRspMdl> AddContacter(MbsSysCtrCtlAddContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlAddContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-新增窗口
        var logicalReqObj = new MbsSysCtrLgcAddContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            ManagerContacterPhone = reqObj.ManagerContacterPhone?.Trim(),
            ManagerContacterDepartment = reqObj.ManagerContacterDepartment?.Trim(),
            ManagerContacterJobTitle = reqObj.ManagerContacterJobTitle?.Trim(),
            ManagerContacterTelephone = reqObj.ManagerContacterTelephone?.Trim(),
            ManagerContacterStatus = reqObj.ManagerContacterStatus,
            ManagerContacterIsConsent = reqObj.ManagerContacterIsConsent,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
            ManagerContacterRemark = reqObj.ManagerContacterRemark?.Trim(),
            ManagerContacterRatingList = reqObj.ManagerContacterRatingList?.Select(x => new MbsSysCtrLgcAddContacterReqRatingItemMdl
            {
                ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                ManagerContacterRatingRemark = x.ManagerContacterRatingRemark?.Trim()
            }).ToList()
        };
        var logicalRspObj = await this._mbsSysCtrLogical.AddContacterAsync(logicalReqObj);
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
        return rspObj;
    }

    /// <summary>更新窗口</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlUpdateContacterRspMdl> UpdateContacter(MbsSysCtrCtlUpdateContacterReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlUpdateContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-更新窗口
        var logicalReqObj = new MbsSysCtrLgcUpdateContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterID = reqObj.ManagerContacterID,
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            ManagerContacterPhone = reqObj.ManagerContacterPhone?.Trim(),
            ManagerContacterDepartment = reqObj.ManagerContacterDepartment?.Trim(),
            ManagerContacterJobTitle = reqObj.ManagerContacterJobTitle?.Trim(),
            ManagerContacterTelephone = reqObj.ManagerContacterTelephone?.Trim(),
            ManagerContacterStatus = reqObj.ManagerContacterStatus,
            ManagerContacterIsConsent = reqObj.ManagerContacterIsConsent,
            ManagerContacterRatingKind = reqObj.ManagerContacterRatingKind,
            ManagerContacterRemark = reqObj.ManagerContacterRemark?.Trim(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.UpdateContacterAsync(logicalReqObj);
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
        return rspObj;
    }

    #endregion

    #region 窗口開發評等原因

    /// <summary>取得多筆窗口開發評等原因</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlGetManyContacterRatingReasonRspMdl> GetManyContacterRatingReason(MbsSysCtrCtlGetManyContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlGetManyContacterRatingReasonRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-取得多筆窗口開發評等原因
        var logicalReqObj = new MbsSysCtrLgcGetManyContacterRatingReasonReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName?.Trim(),
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.GetManyContacterRatingReasonAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingReasonList = logicalRspObj.ManagerContacterRatingReasonList?
            .Select(x => new MbsSysCtrCtlGetManyContacterRatingReasonRspItemMdl()
            {
                ManagerContacterRatingReasonID = x.ManagerContacterRatingReasonID,
                ManagerContacterRatingReasonName = x.ManagerContacterRatingReasonName,
                ManagerContacterRatingReasonStatus = x.ManagerContacterRatingReasonStatus,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得單筆窗口開發評等原因</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlGetContacterRatingReasonRspMdl> GetContacterRatingReason(MbsSysCtrCtlGetContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlGetContacterRatingReasonRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-取得單筆窗口開發評等原因
        var logicalReqObj = new MbsSysCtrLgcGetContacterRatingReasonReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.GetContacterRatingReasonAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingReasonName = logicalRspObj.ManagerContacterRatingReasonName;
        rspObj.ManagerContacterRatingReasonStatus = logicalRspObj.ManagerContacterRatingReasonStatus;
        return rspObj;
    }

    /// <summary>新增窗口開發評等原因</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlAddContacterRatingReasonRspMdl> AddContacterRatingReason(MbsSysCtrCtlAddContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlAddContacterRatingReasonRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-新增窗口開發評等原因
        var logicalReqObj = new MbsSysCtrLgcAddContacterRatingReasonReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName.Trim(),
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.AddContacterRatingReasonAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingReasonID = logicalRspObj.ManagerContacterRatingReasonID;
        return rspObj;
    }

    /// <summary>更新窗口開發評等原因</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlUpdateContacterRatingReasonRspMdl> UpdateContacterRatingReason(MbsSysCtrCtlUpdateContacterRatingReasonReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlUpdateContacterRatingReasonRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-更新窗口開發評等原因
        var logicalReqObj = new MbsSysCtrLgcUpdateContacterRatingReasonReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
            ManagerContacterRatingReasonName = reqObj.ManagerContacterRatingReasonName?.Trim(),
            ManagerContacterRatingReasonStatus = reqObj.ManagerContacterRatingReasonStatus,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.UpdateContacterRatingReasonAsync(logicalReqObj);
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

    #endregion

    #region 窗口開發評等

    /// <summary>取得多筆窗口開發評等</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlGetManyContacterRatingRspMdl> GetManyContacterRating(MbsSysCtrCtlGetManyContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlGetManyContacterRatingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-取得多筆窗口開發評等
        var logicalReqObj = new MbsSysCtrLgcGetManyContacterRatingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterID = reqObj.ManagerContacterID,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.GetManyContacterRatingAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingList = logicalRspObj.ManagerContacterRatingList?
            .Select(x => new MbsSysCtrCtlGetManyContacterRatingRspItemMdl()
            {
                ManagerContacterRatingID = x.ManagerContacterRatingID,
                ManagerContacterRatingReasonName = x.ManagerContacterRatingReasonName,
                ManagerContacterRatingRemark = x.ManagerContacterRatingRemark,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得單筆窗口開發評等</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlGetContacterRatingRspMdl> GetContacterRating(MbsSysCtrCtlGetContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlGetContacterRatingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-取得單筆窗口開發評等
        var logicalReqObj = new MbsSysCtrLgcGetContacterRatingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingID = reqObj.ManagerContacterRatingID,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.GetContacterRatingAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingReasonID = logicalRspObj.ManagerContacterRatingReasonID;
        rspObj.ManagerContacterRatingRemark = logicalRspObj.ManagerContacterRatingRemark;
        return rspObj;
    }

    /// <summary>新增窗口開發評等</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlAddContacterRatingRspMdl> AddContacterRating(MbsSysCtrCtlAddContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlAddContacterRatingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-新增窗口開發評等
        var logicalReqObj = new MbsSysCtrLgcAddContacterRatingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterID = reqObj.ManagerContacterID,
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
            ManagerContacterRatingRemark = reqObj.ManagerContacterRatingRemark?.Trim(),
        };
        var logicalRspObj = await this._mbsSysCtrLogical.AddContacterRatingAsync(logicalReqObj);
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
        rspObj.ManagerContacterRatingID = logicalRspObj.ManagerContacterRatingID;
        return rspObj;
    }

    /// <summary>更新窗口開發評等</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlUpdateContacterRatingRspMdl> UpdateContacterRating(MbsSysCtrCtlUpdateContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlUpdateContacterRatingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-更新窗口開發評等
        var logicalReqObj = new MbsSysCtrLgcUpdateContacterRatingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingID = reqObj.ManagerContacterRatingID,
            ManagerContacterRatingReasonID = reqObj.ManagerContacterRatingReasonID,
            ManagerContacterRatingRemark = reqObj.ManagerContacterRatingRemark?.Trim(),
        };
        var logicalRspObj = await this._mbsSysCtrLogical.UpdateContacterRatingAsync(logicalReqObj);
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

    /// <summary>移除窗口開發評等</summary>
    [HttpPost]
    public async Task<MbsSysCtrCtlRemoveContacterRatingRspMdl> RemoveContacterRating(MbsSysCtrCtlRemoveContacterRatingReqMdl reqObj)
    {
        var rspObj = new MbsSysCtrCtlRemoveContacterRatingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-窗口-邏輯-移除窗口開發評等
        var logicalReqObj = new MbsSysCtrLgcRemoveContacterRatingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerContacterRatingID = reqObj.ManagerContacterRatingID,
        };
        var logicalRspObj = await this._mbsSysCtrLogical.RemoveContacterRatingAsync(logicalReqObj);
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

    #endregion
}