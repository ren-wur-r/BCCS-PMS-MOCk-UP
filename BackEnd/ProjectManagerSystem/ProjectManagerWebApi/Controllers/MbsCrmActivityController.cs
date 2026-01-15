
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-CRM-活動管理</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsCrmActivityController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsCrmActivityController> _logger;

    /// <summary>管理者後台-CRM-活動管理-邏輯服務</summary>
    private readonly IMbsCrmActLogicalService _mbsCrmActLogical;

    /// <summary>建構式</summary>
    public MbsCrmActivityController(
        ILogger<MbsCrmActivityController> logger,
        IMbsCrmActLogicalService mbsCrmActLogical)
    {
        this._logger = logger;
        this._mbsCrmActLogical = mbsCrmActLogical;
    }

    #region 活動

    /// <summary>新增活動</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlAddActivityRspMdl> AddActivity(MbsCrmActCtlAddActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlAddActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-新增
        var logicalReqObj = new MbsCrmActLgcAddActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityName = reqObj.ManagerActivityName?.Trim(),
            ManagerAcitivtyKind = reqObj.ManagerAcitivtyKind,
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityLocation = reqObj.ManagerActivityLocation?.Trim(),
            ManagerActivityExpectedLeadCount = reqObj.ManagerActivityExpectedLeadCount,
            ManagerActivityContent = reqObj.ManagerActivityContent?.Trim(),
            ManagerActivityProductList = reqObj.ManagerActivityProductList?.Select(x => new MbsCrmActLgcAddActivityReqProductItemMdl
            {
                ManagerProductID = x.ManagerProductID
            }).ToList(),
            ManagerActivitySponsorList = reqObj.ManagerActivitySponsorList?.Select(x => new MbsCrmActLgcAddActivityReqSponsorItemMdl
            {
                ManagerActivitySponsorName = x.ManagerActivitySponsorName?.Trim(),
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount
            }).ToList(),
            ManagerActivityExpenseList = reqObj.ManagerActivityExpenseList?.Select(x => new MbsCrmActLgcAddActivityReqExpenseItemMdl
            {
                ManagerActivityExpenseItem = x.ManagerActivityExpenseItem?.Trim(),
                ManagerActivityExpenseQuantity = x.ManagerActivityExpenseQuantity,
                ManagerActivityExpenseTotalAmount = x.ManagerActivityExpenseTotalAmount
            }).ToList(),
        };
        var logicalRspObj = await this._mbsCrmActLogical.AddActivityAsync(logicalReqObj);
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
        rspObj.ManagerActivityID = logicalRspObj.ManagerActivityID;
        return rspObj;
    }

    /// <summary>取得單筆活動</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetActivityRspMdl> GetActivity(MbsCrmActCtlGetActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-取得單筆
        var logicalReqObj = new MbsCrmActLgcGetActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetActivityAsync(logicalReqObj);
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
        rspObj.ManagerActivityName = logicalRspObj.ManagerActivityName;
        rspObj.ManagerActivityKind = logicalRspObj.ManagerActivityKind;
        rspObj.ManagerActivityStartTime = logicalRspObj.ManagerActivityStartTime;
        rspObj.ManagerActivityEndTime = logicalRspObj.ManagerActivityEndTime;
        rspObj.ManagerActivityLocation = logicalRspObj.ManagerActivityLocation;
        rspObj.ManagerActivityExpectedLeadCount = logicalRspObj.ManagerActivityExpectedLeadCount;
        rspObj.ManagerActivityContent = logicalRspObj.ManagerActivityContent;
        rspObj.ManagerActivityRegisteredCount = logicalRspObj.ManagerActivityRegisteredCount;
        rspObj.ManagerActivityTransferredToSalesCount = logicalRspObj.ManagerActivityTransferredToSalesCount;
        rspObj.ManagerActivityEmployeePipelineCount = logicalRspObj.ManagerActivityEmployeePipelineCount;
        rspObj.ManagerActivityProductList = logicalRspObj.ManagerActivityProductList?
            .Select(x => new MbsCrmActCtlGetManyActivityProductRspItemMdl()
            {
                ManagerActivityProductID = x.ManagerActivityProductID,
                ManagerProductID = x.ManagerProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
            })
            .ToList();
        rspObj.ManagerActivitySponsorList = logicalRspObj.ManagerActivitySponsorList?
            .Select(x => new MbsCrmActCtlGetManyActivitySponsorRspItemMdl()
            {
                ManagerActivitySponsorID = x.ManagerActivitySponsorID,
                ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount,
            })
            .ToList();
        rspObj.ManagerActivityExpenseList = logicalRspObj.ManagerActivityExpenseList?
            .Select(x => new MbsCrmActCtlGetManyActivityExpenseRspItemMdl()
            {
                ManagerActivityExpenseID = x.ManagerActivityExpenseID,
                ManagerActivityExpenseItem = x.ManagerActivityExpenseItem,
                ManagerActivityExpenseQuantity = x.ManagerActivityExpenseQuantity,
                ManagerActivityExpenseTotalAmount = x.ManagerActivityExpenseTotalAmount,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得多筆活動</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyActivityRspMdl> GetManyActivity(MbsCrmActCtlGetManyActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-取得多筆
        var logicalReqObj = new MbsCrmActLgcGetManyActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityKind = reqObj.ManagerActivityKind,
            ManagerActivityName = reqObj.ManagerActivityName?.Trim(),
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyActivityAsync(logicalReqObj);
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
        rspObj.ManagerActivityList = logicalRspObj.ManagerActivityList?
            .Select(x => new MbsCrmActCtlGetManyActivityRspItemMdl()
            {
                ManagerActivityID = x.ManagerActivityID,
                ManagerActivityName = x.ManagerActivityName,
                ManagerActivityKind = x.ManagerActivityKind,
                ManagerActivityStartTime = x.ManagerActivityStartTime,
                ManagerActivityEndTime = x.ManagerActivityEndTime,
                ManagerActivityLocation = x.ManagerActivityLocation,
                ManagerActivityExpectedLeadCount = x.ManagerActivityExpectedLeadCount,
                ManagerActivityRegisteredCount = x.ManagerActivityRegisteredCount,
                ManagerActivityTransferredToSalesCount = x.ManagerActivityTransferredToSalesCount,
                ManagerActivityEmployeePipelineCount = x.ManagerActivityEmployeePipelineCount,
                ManagerActivitySponsorTotalSponsorAmount = x.ManagerActivitySponsorTotalSponsorAmount,
                ManagerActivityExpenseTotalExpenseAmount = x.ManagerActivityExpenseTotalExpenseAmount,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>更新活動</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlUpdateActivityRspMdl> UpdateActivity(MbsCrmActCtlUpdateActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlUpdateActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-更新
        var logicalReqObj = new MbsCrmActLgcUpdateActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivityName = reqObj.ManagerActivityName?.Trim(),
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityLocation = reqObj.ManagerActivityLocation?.Trim(),
            ManagerActivityExpectedLeadCount = reqObj.ManagerActivityExpectedLeadCount,
            ManagerActivityContent = reqObj.ManagerActivityContent?.Trim(),
        };
        var logicalRspObj = await this._mbsCrmActLogical.UpdateActivityAsync(logicalReqObj);
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

    #region 活動產品

    /// <summary>新增活動產品</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlAddActivityProductRspMdl> AddActivityProduct(MbsCrmActCtlAddActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlAddActivityProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動產品-新增
        var logicalReqObj = new MbsCrmActLgcAddActivityProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerProductID = reqObj.ManagerProductID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.AddActivityProductAsync(logicalReqObj);
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
        rspObj.ManagerActivityProductID = logicalRspObj.ManagerActivityProductID;
        return rspObj;
    }

    /// <summary>更新活動產品</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlUpdateActivityProductRspMdl> UpdateActivityProduct(MbsCrmActCtlUpdateActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlUpdateActivityProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動產品-更新
        var logicalReqObj = new MbsCrmActLgcUpdateActivityProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityProductID = reqObj.ManagerActivityProductID,
            ManagerProductID = reqObj.ManagerProductID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.UpdateActivityProductAsync(logicalReqObj);
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

    /// <summary>刪除活動產品</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlRemoveActivityProductRspMdl> RemoveActivityProduct(MbsCrmActCtlRemoveActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlRemoveActivityProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動產品-刪除
        var logicalReqObj = new MbsCrmActLgcRemoveActivityProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityProductID = reqObj.ManagerActivityProductID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.RemoveActivityProductAsync(logicalReqObj);
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

    /// <summary>取得活動產品</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetActivityProductRspMdl> GetActivityProduct(MbsCrmActCtlGetActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetActivityProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動產品-取得
        var logicalReqObj = new MbsCrmActLgcGetActivityProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityProductID = reqObj.ManagerActivityProductID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetActivityProductAsync(logicalReqObj);
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
        rspObj.ManagerActivityProductID = logicalRspObj.ManagerActivityProductID;
        rspObj.ManagerProductID = logicalRspObj.ManagerProductID;
        rspObj.ManagerProductMainKindID = logicalRspObj.ManagerProductMainKindID;
        rspObj.ManagerProductSubKindID = logicalRspObj.ManagerProductSubKindID;
        return rspObj;
    }

    /// <summary>取得多筆活動產品</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyActivityProductRspMdl> GetManyActivityProduct(MbsCrmActCtlGetManyActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyActivityProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動產品-取得多筆
        var logicalReqObj = new MbsCrmActLgcGetManyActivityProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyActivityProductAsync(logicalReqObj);
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
        rspObj.ManagerActivityProductList = logicalRspObj.ManagerActivityProductList?
            .Select(x => new MbsCrmActCtlGetManyActivityProductRspItemMdl()
            {
                ManagerActivityProductID = x.ManagerActivityProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 活動贊助

    /// <summary>新增活動贊助</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlAddActivitySponsorRspMdl> AddActivitySponsor(MbsCrmActCtlAddActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlAddActivitySponsorRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動贊助-新增
        var logicalReqObj = new MbsCrmActLgcAddActivitySponsorReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySponsorName = reqObj.ManagerActivitySponsorName.Trim(),
            ManagerActivitySponsorAmount = reqObj.ManagerActivitySponsorAmount,
        };
        var logicalRspObj = await this._mbsCrmActLogical.AddActivitySponsorAsync(logicalReqObj);
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
        rspObj.ManagerActivitySponsorID = logicalRspObj.ManagerActivitySponsorID;
        return rspObj;
    }

    /// <summary>更新活動贊助</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlUpdateActivitySponsorRspMdl> UpdateActivitySponsor(MbsCrmActCtlUpdateActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlUpdateActivitySponsorRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動贊助-更新
        var logicalReqObj = new MbsCrmActLgcUpdateActivitySponsorReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID,
            ManagerActivitySponsorName = reqObj.ManagerActivitySponsorName?.Trim(),
            ManagerActivitySponsorAmount = reqObj.ManagerActivitySponsorAmount,
        };
        var logicalRspObj = await this._mbsCrmActLogical.UpdateActivitySponsorAsync(logicalReqObj);
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

    /// <summary>刪除活動贊助</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlRemoveActivitySponsorRspMdl> RemoveActivitySponsor(MbsCrmActCtlRemoveActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlRemoveActivitySponsorRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動贊助-刪除
        var logicalReqObj = new MbsCrmActLgcRemoveActivitySponsorReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.RemoveActivitySponsorAsync(logicalReqObj);
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

    /// <summary>取得活動贊助</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetActivitySponsorRspMdl> GetActivitySponsor(MbsCrmActCtlGetActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetActivitySponsorRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動贊助-取得
        var logicalReqObj = new MbsCrmActLgcGetActivitySponsorReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetActivitySponsorAsync(logicalReqObj);
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
        rspObj.ManagerActivitySponsorName = logicalRspObj.ManagerActivitySponsorName;
        rspObj.ManagerActivitySponsorAmount = logicalRspObj.ManagerActivitySponsorAmount;
        return rspObj;
    }

    /// <summary>取得多筆活動贊助</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyActivitySponsorRspMdl> GetManyActivitySponsor(MbsCrmActCtlGetManyActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyActivitySponsorRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動贊助-取得多筆
        var logicalReqObj = new MbsCrmActLgcGetManyActivitySponsorReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyActivitySponsorAsync(logicalReqObj);
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
        rspObj.ManagerActivitySponsorList = logicalRspObj.ManagerActivitySponsorList?
            .Select(x => new MbsCrmActCtlGetManyActivitySponsorRspItemMdl()
            {
                ManagerActivitySponsorID = x.ManagerActivitySponsorID,
                ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 活動支出

    /// <summary>新增活動支出</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlAddActivityExpenseRspMdl> AddActivityExpense(MbsCrmActCtlAddActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlAddActivityExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動支出-新增
        var logicalReqObj = new MbsCrmActLgcAddActivityExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivityExpenseItem = reqObj.ManagerActivityExpenseItem.Trim(),
            ManagerActivityExpenseQuantity = reqObj.ManagerActivityExpenseQuantity,
            ManagerActivityExpenseTotalAmount = reqObj.ManagerActivityExpenseTotalAmount,
        };
        var logicalRspObj = await this._mbsCrmActLogical.AddActivityExpenseAsync(logicalReqObj);
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
        rspObj.ManagerActivityExpenseID = logicalRspObj.ManagerActivityExpenseID;
        return rspObj;
    }

    /// <summary>更新活動支出</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlUpdateActivityExpenseRspMdl> UpdateActivityExpense(MbsCrmActCtlUpdateActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlUpdateActivityExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動支出-更新
        var logicalReqObj = new MbsCrmActLgcUpdateActivityExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID,
            ManagerActivityExpenseItem = reqObj.ManagerActivityExpenseItem?.Trim(),
            ManagerActivityExpenseQuantity = reqObj.ManagerActivityExpenseQuantity,
            ManagerActivityExpenseTotalAmount = reqObj.ManagerActivityExpenseTotalAmount,
        };
        var logicalRspObj = await this._mbsCrmActLogical.UpdateActivityExpenseAsync(logicalReqObj);
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

    /// <summary>刪除活動支出</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlRemoveActivityExpenseRspMdl> RemoveActivityExpense(MbsCrmActCtlRemoveActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlRemoveActivityExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動支出-刪除
        var logicalReqObj = new MbsCrmActLgcRemoveActivityExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.RemoveActivityExpenseAsync(logicalReqObj);
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

    /// <summary>取得活動支出</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetActivityExpenseRspMdl> GetActivityExpense(MbsCrmActCtlGetActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetActivityExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動支出-取得
        var logicalReqObj = new MbsCrmActLgcGetActivityExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetActivityExpenseAsync(logicalReqObj);
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
        rspObj.ManagerActivityExpenseID = logicalRspObj.ManagerActivityExpenseID;
        rspObj.ManagerActivityExpenseItem = logicalRspObj.ManagerActivityExpenseItem;
        rspObj.ManagerActivityExpenseQuantity = logicalRspObj.ManagerActivityExpenseQuantity;
        rspObj.ManagerActivityExpenseTotalAmount = logicalRspObj.ManagerActivityExpenseTotalAmount;
        return rspObj;
    }

    /// <summary>取得多筆活動支出</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyActivityExpenseRspMdl> GetManyActivityExpense(MbsCrmActCtlGetManyActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyActivityExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-活動支出-取得多筆
        var logicalReqObj = new MbsCrmActLgcGetManyActivityExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyActivityExpenseAsync(logicalReqObj);
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
        rspObj.ManagerActivityExpenseList = logicalRspObj.ManagerActivityExpenseList?
            .Select(x => new MbsCrmActCtlGetManyActivityExpenseRspItemMdl()
            {
                ManagerActivityExpenseID = x.ManagerActivityExpenseID,
                ManagerActivityExpenseItem = x.ManagerActivityExpenseItem,
                ManagerActivityExpenseQuantity = x.ManagerActivityExpenseQuantity,
                ManagerActivityExpenseTotalAmount = x.ManagerActivityExpenseTotalAmount,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 活動名單

    /// <summary>取得多筆活動名單</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyActivityEmployeePipelineRspMdl> GetManyActivityEmployeePipeline(MbsCrmActCtlGetManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-取得多筆活動名單
        var logicalReqObj = new MbsCrmActLgcGetManyActivityEmployeePipelineReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyActivityEmployeePipelineAsync(logicalReqObj);
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
        rspObj.EmployeePipelineList = logicalRspObj.EmployeePipelineList?
            .Select(x => new MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl()
            {
                EmployeePipelineID = x.EmployeePipelineID,
                AtomPipelineStatus = x.AtomPipelineStatus,
                EmployeePipelineOriginalManagerCompanyName = x.EmployeePipelineOriginalManagerCompanyName,
                EmployeePipelineOriginalManagerContacterDepartment = x.EmployeePipelineOriginalManagerContacterDepartment,
                EmployeePipelineOriginalManagerContacterJobTitle = x.EmployeePipelineOriginalManagerContacterJobTitle,
                EmployeePipelineOriginalManagerContacterName = x.EmployeePipelineOriginalManagerContacterName,
                EmployeePipelineOriginalManagerContacterEmail = x.EmployeePipelineOriginalManagerContacterEmail,
                EmployeePipelineOriginalManagerContacterPhone = x.EmployeePipelineOriginalManagerContacterPhone,
                EmployeePipelineOriginalManagerContacterTelephone = x.EmployeePipelineOriginalManagerContacterTelephone
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得活動名單</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetActivityEmployeePipelineRspMdl> GetActivityEmployeePipeline(MbsCrmActCtlGetActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-取得活動名單
        var logicalReqObj = new MbsCrmActLgcGetActivityEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetActivityEmployeePipelineAsync(logicalReqObj);
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
        rspObj.AtomPipelineStatus = logicalRspObj.AtomPipelineStatus;
        rspObj.ManagerCompanyUnifiedNumber = logicalRspObj.ManagerCompanyUnifiedNumber;
        rspObj.ManagerCompanyName = logicalRspObj.ManagerCompanyName;
        rspObj.AtomEmployeeRange = logicalRspObj.AtomEmployeeRange;
        rspObj.AtomCustomerGrade = logicalRspObj.AtomCustomerGrade;
        rspObj.ManagerCompanyMainKindName = logicalRspObj.ManagerCompanyMainKindName;
        rspObj.ManagerCompanySubKindName = logicalRspObj.ManagerCompanySubKindName;
        rspObj.AtomCity = logicalRspObj.AtomCity;
        rspObj.ManagerCompanyLocationAddress = logicalRspObj.ManagerCompanyLocationAddress;
        rspObj.ManagerCompanyLocationTelephone = logicalRspObj.ManagerCompanyLocationTelephone;
        rspObj.ManagerCompanyLocationStatus = logicalRspObj.ManagerCompanyLocationStatus;
        rspObj.ManagerContacterName = logicalRspObj.ManagerContacterName;
        rspObj.ManagerContacterEmail = logicalRspObj.ManagerContacterEmail;
        rspObj.ManagerContacterPhone = logicalRspObj.ManagerContacterPhone;
        rspObj.ManagerContacterDepartment = logicalRspObj.ManagerContacterDepartment;
        rspObj.ManagerContacterJobTitle = logicalRspObj.ManagerContacterJobTitle;
        rspObj.ManagerContacterTelephone = logicalRspObj.ManagerContacterTelephone;
        rspObj.ManagerContacterIsConsent = logicalRspObj.ManagerContacterIsConsent;
        rspObj.ManagerContacterStatus = logicalRspObj.ManagerContacterStatus;
        rspObj.AtomRatingKind = logicalRspObj.AtomRatingKind;
        rspObj.TeamsMeetingDuration = logicalRspObj.TeamsMeetingDuration;
        rspObj.TeamsRole = logicalRspObj.TeamsRole;
        return rspObj;
    }

    /// <summary>新增活動名單</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlAddActivityEmployeePipelineRspMdl> AddActivityEmployeePipeline(MbsCrmActCtlAddActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlAddActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-新增活動名單
        var logicalReqObj = new MbsCrmActLgcAddActivityEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            ManagerContacterID = reqObj.ManagerContacterID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus
        };
        var logicalRspObj = await this._mbsCrmActLogical.AddActivityEmployeePipelineAsync(logicalReqObj);
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
        rspObj.EmployeePipelineID = logicalRspObj.EmployeePipelineID;
        return rspObj;
    }

    /// <summary>更新多筆活動名單</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlUpdateManyActivityEmployeePipelineRspMdl> UpdateManyActivityEmployeePipeline(MbsCrmActCtlUpdateManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlUpdateManyActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-更新多筆活動名單
        var logicalReqObj = new MbsCrmActLgcUpdateManyActivityEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList,
            AtomPipelineStatus = reqObj.AtomPipelineStatus
        };
        var logicalRspObj = await this._mbsCrmActLogical.UpdateManyActivityEmployeePipelineAsync(logicalReqObj);
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

    /// <summary>刪除多筆活動名單</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlRemoveManyActivityEmployeePipelineRspMdl> RemoveManyActivityEmployeePipeline(MbsCrmActCtlRemoveManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlRemoveManyActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-刪除多筆活動名單
        var logicalReqObj = new MbsCrmActLgcRemoveManyActivityEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList
        };
        var logicalRspObj = await this._mbsCrmActLogical.RemoveManyActivityEmployeePipelineAsync(logicalReqObj);
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

    /// <summary>取得多筆客戶過往活動</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyPastActivityRspMdl> GetManyPastActivity(MbsCrmActCtlGetManyPastActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyPastActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-取得多筆客戶過往活動
        var logicalReqObj = new MbsCrmActLgcGetManyPastActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyPastActivityAsync(logicalReqObj);
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
        rspObj.LatestPastActivity = logicalRspObj.LatestPastActivity != null
            ? new MbsCrmActCtlGetManyPastActivityRspItemMdl()
            {
                ManagerActivityName = logicalRspObj.LatestPastActivity.ManagerActivityName,
                ManagerActivityStartTime = logicalRspObj.LatestPastActivity.ManagerActivityStartTime,
                ManagerActivityEndTime = logicalRspObj.LatestPastActivity.ManagerActivityEndTime
            }
            : null;
        rspObj.PastActivityList = logicalRspObj.PastActivityList?
            .Select(x => new MbsCrmActCtlGetManyPastActivityRspItemMdl()
            {
                ManagerActivityName = x.ManagerActivityName,
                ManagerActivityStartTime = x.ManagerActivityStartTime,
                ManagerActivityEndTime = x.ManagerActivityEndTime
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    #endregion

    #region 活動問卷

    /// <summary>新增活動問卷問題</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlAddActivitySurveyQuestionRspMdl> AddActivitySurveyQuestion(MbsCrmActCtlAddActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlAddActivitySurveyQuestionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-新增活動問卷問題
        var logicalReqObj = new MbsCrmActLgcAddActivitySurveyQuestionReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivitySurveyQuestionList = reqObj.ManagerActivitySurveyQuestionList
                .Select(x => new MbsCrmActLgcAddActivitySurveyQuestionReqItemQuestionMdl()
                {
                    ManagerActivityID = x.ManagerActivityID,
                    ManagerActivitySurveyQuestionKind = x.ManagerActivitySurveyQuestionKind,
                    ManagerActivitySurveyQuestionTitle = x.ManagerActivitySurveyQuestionTitle.Trim(),
                    ManagerActivitySurveyQuestionContent = x.ManagerActivitySurveyQuestionContent.Trim(),
                    IsOther = x.IsOther,
                    ManagerActivitySurveyQuestionSort = x.ManagerActivitySurveyQuestionSort,
                    ManagerActivitySurveyQuestionItemList = x.ManagerActivitySurveyQuestionItemList
                        .Select(x => new MbsCrmActLgcAddActivitySurveyQuestionReqItemQuestionItemMdl()
                        {
                            ManagerActivitySurveyQuestionItemName = x.ManagerActivitySurveyQuestionItemName?.Trim(),
                            ManagerActivitySurveyQuestionItemSort = x.ManagerActivitySurveyQuestionItemSort,
                        })
                        .ToList()
                })
                .ToList()
        };

        var logicalRspObj = await this._mbsCrmActLogical.AddActivitySurveyQuestionAsync(logicalReqObj);
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
        rspObj.ManagerActivitySurveyQuestionIDList = logicalRspObj.ManagerActivitySurveyQuestionIDList;
        return rspObj;
    }

    /// <summary>取得活動問卷問題</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetActivitySurveyQuestionRspMdl> GetActivitySurveyQuestion(MbsCrmActCtlGetActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetActivitySurveyQuestionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };
        // logical, 管理者後台-CRM-活動管理-取得活動問卷問題
        var logicalReqObj = new MbsCrmActLgcGetActivitySurveyQuestionReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetActivitySurveyQuestionAsync(logicalReqObj);
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
        rspObj.ManagerActivitySurveyQuestionList = logicalRspObj.ManagerActivitySurveyQuestionList
            ?.Select(x => new MbsCrmActCtlGetActivitySurveyQuestionRspItemMdl()
            {
                ManagerActivitySurveyQuestionID = x.ManagerActivitySurveyQuestionID,
                ManagerActivitySurveyQuestionKind = x.ManagerActivitySurveyQuestionKind,
                ManagerActivitySurveyQuestionTitle = x.ManagerActivitySurveyQuestionTitle,
                ManagerActivitySurveyQuestionContent = x.ManagerActivitySurveyQuestionContent,
                IsOther = x.IsOther,
                ManagerActivitySurveyQuestionSort = x.ManagerActivitySurveyQuestionSort,
                ManagerActivitySurveyQuestionItemList = x.ManagerActivitySurveyQuestionItemList
                    ?.Select(x => new MbsCrmActCtlGetActivitySurveyQuestionRspItemQuestionItemMdl()
                    {
                        ManagerActivitySurveyQuestionItemID = x.ManagerActivitySurveyQuestionItemID,
                        ManagerActivitySurveyQuestionItemName = x.ManagerActivitySurveyQuestionItemName,
                        ManagerActivitySurveyQuestionItemSort = x.ManagerActivitySurveyQuestionItemSort,
                    })
                    .ToList()
            })
            .ToList();
        return rspObj;
    }

    /// <summary>更新活動問卷問題</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlUpdateActivitySurveyQuestionRspMdl> UpdateActivitySurveyQuestion(MbsCrmActCtlUpdateActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlUpdateActivitySurveyQuestionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-更新活動問卷問題
        var logicalReqObj = new MbsCrmActLgcUpdateActivitySurveyQuestionReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySurveyQuestionList = reqObj.ManagerActivitySurveyQuestionList
                ?.Select(x => new MbsCrmActLgcUpdateActivitySurveyQuestionReqItemQuestionMdl()
                {
                    ManagerActivitySurveyQuestionKind = x.ManagerActivitySurveyQuestionKind,
                    ManagerActivitySurveyQuestionTitle = x.ManagerActivitySurveyQuestionTitle.Trim(),
                    ManagerActivitySurveyQuestionContent = x.ManagerActivitySurveyQuestionContent.Trim(),
                    IsOther = x.IsOther,
                    ManagerActivitySurveyQuestionSort = x.ManagerActivitySurveyQuestionSort,
                    ManagerActivitySurveyQuestionItemList = x.ManagerActivitySurveyQuestionItemList
                        ?.Select(x => new MbsCrmActLgcAddActivitySurveyQuestionReqItemQuestionItemMdl()
                        {
                            ManagerActivitySurveyQuestionItemName = x.ManagerActivitySurveyQuestionItemName?.Trim(),
                            ManagerActivitySurveyQuestionItemSort = x.ManagerActivitySurveyQuestionItemSort,
                        })
                        .ToList()
                })
                .ToList()
        };

        var logicalRspObj = await this._mbsCrmActLogical.UpdateActivitySurveyQuestionAsync(logicalReqObj);
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

    /// <summary>刪除活動問卷問題</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlRemoveActivitySurveyQuestionRspMdl> RemoveActivitySurveyQuestion(MbsCrmActCtlRemoveActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlRemoveActivitySurveyQuestionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-刪除活動問卷問題
        var logicalReqObj = new MbsCrmActLgcRemoveActivitySurveyQuestionReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivitySurveyQuestionID = reqObj.ManagerActivitySurveyQuestionID
        };
        var logicalRspObj = await this._mbsCrmActLogical.RemoveActivitySurveyQuestionAsync(logicalReqObj);
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

    /// <summary>取得多筆活動問卷回答者</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlGetManyActivitySurveyAnswererRspMdl> GetManyActivitySurveyAnswerer(MbsCrmActCtlGetManyActivitySurveyAnswererReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlGetManyActivitySurveyAnswererRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-取得多筆活動問卷回答者
        var logicalReqObj = new MbsCrmActLgcGetManyActivitySurveyAnswererReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySurveyAnswererCompanyName = reqObj.ManagerActivitySurveyAnswererCompanyName,
            ManagerActivitySurveyAnswererContacterName = reqObj.ManagerActivitySurveyAnswererContacterName,
            ManagerActivitySurveyAnswererContacterEmail = reqObj.ManagerActivitySurveyAnswererContacterEmail,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsCrmActLogical.GetManyActivitySurveyAnswererAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivitySurveyAnswererList = logicalRspObj.ManagerActivitySurveyAnswererList
            ?.Select(x => new MbsCrmActCtlGetManyActivitySurveyAnswererRspItemMdl()
            {
                ManagerActivitySurveyAnswererID = x.ManagerActivitySurveyAnswererID,
                ManagerActivitySurveyAnswererCompanyName = x.ManagerActivitySurveyAnswererCompanyName,
                ManagerActivitySurveyAnswererContacterName = x.ManagerActivitySurveyAnswererContacterName,
                ManagerActivitySurveyAnswererContacterEmail = x.ManagerActivitySurveyAnswererContacterEmail,
                ManagerActivitySurveyAnswererCompanyScaleID = x.ManagerActivitySurveyAnswererCompanyScaleID,
                ManagerActivitySurveyAnswererCompanyBudgetID = x.ManagerActivitySurveyAnswererCompanyBudgetID,
                ManagerActivitySurveyAnswererCompanyPurchaseID = x.ManagerActivitySurveyAnswererCompanyPurchaseID,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }
    #endregion

    #region eDM

    /// <summary>下載eDM範本</summary>
    [HttpPost]
    public async Task<IActionResult> DownloadEdmTemplate(MbsCrmActCtlDownloadEdmTemplateReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlDownloadEdmTemplateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-下載eDM範本
        var logicalReqObj = new MbsCrmActLgcDownloadEdmTemplateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP()
        };
        var logicalRspObj = await this._mbsCrmActLogical.DownloadEdmTemplateAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return this.Ok(reqObj);
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return this.Ok(reqObj);
        }

        return this.PhysicalFile(
            logicalRspObj.LocalAbsoluteFileName,
            logicalRspObj.ContentType,
            logicalRspObj.FileName);
    }

    /// <summary>匯入eDM</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlImportEdmRspMdl> ImportEdm([FromForm] MbsCrmActCtlImportEdmReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlImportEdmRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-匯入eDM
        var logicalReqObj = new MbsCrmActLgcImportEdmReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            EdmFile = reqObj.EdmFile
        };
        var logicalRspObj = await this._mbsCrmActLogical.ImportEdmAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            rspObj.ErrorDataList = logicalRspObj.ErrorDataList?
                .Select(x => new MbsCrmActCtlImportEdmRspItemMdl()
                {
                    EdmContacterEmail = x.EdmContacterEmail,
                    EdmFirstName = x.EdmFirstName,
                    EdmLastName = x.EdmLastName,
                    EdmContacterTelephone = x.EdmContacterTelephone,
                    EdmContacterPhone = x.EdmContacterPhone,
                    EdmCompanyName = x.EdmCompanyName,
                    EdmContacterJobTitle = x.EdmContacterJobTitle,
                    EdmCompanyPhone = x.EdmCompanyPhone,
                    EdmCompanyFax = x.EdmCompanyFax,
                    EdmCompanyAddress = x.EdmCompanyAddress,
                    EdmRemark = x.EdmRemark,
                    EdmContacterDepartment = x.EdmContacterDepartment,
                    EdmCompanyMainKind = x.EdmCompanyMainKind,
                    EdmCompanySubKind = x.EdmCompanySubKind,
                    EdmAccountSales = x.EdmAccountSales,
                    EdmRegion = x.EdmRegion,
                    EdmCreatedDate = x.EdmCreatedDate,
                    EdmDevice = x.EdmDevice
                })
                .ToList();
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ErrorDataList = logicalRspObj.ErrorDataList?
            .Select(x => new MbsCrmActCtlImportEdmRspItemMdl()
            {
                EdmContacterEmail = x.EdmContacterEmail,
                EdmFirstName = x.EdmFirstName,
                EdmLastName = x.EdmLastName,
                EdmContacterTelephone = x.EdmContacterTelephone,
                EdmContacterPhone = x.EdmContacterPhone,
                EdmCompanyName = x.EdmCompanyName,
                EdmContacterJobTitle = x.EdmContacterJobTitle,
                EdmCompanyPhone = x.EdmCompanyPhone,
                EdmCompanyFax = x.EdmCompanyFax,
                EdmCompanyAddress = x.EdmCompanyAddress,
                EdmRemark = x.EdmRemark,
                EdmContacterDepartment = x.EdmContacterDepartment,
                EdmCompanyMainKind = x.EdmCompanyMainKind,
                EdmCompanySubKind = x.EdmCompanySubKind,
                EdmAccountSales = x.EdmAccountSales,
                EdmRegion = x.EdmRegion,
                EdmCreatedDate = x.EdmCreatedDate,
                EdmDevice = x.EdmDevice
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region Teams

    /// <summary>下載Teams範本</summary>
    [HttpPost]
    public async Task<IActionResult> DownloadTeamsTemplate(MbsCrmActCtlDownloadTeamsTemplateReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlDownloadTeamsTemplateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-下載Teams範本
        var logicalReqObj = new MbsCrmActLgcDownloadTeamsTemplateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP()
        };
        var logicalRspObj = await this._mbsCrmActLogical.DownloadTeamsTemplateAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return this.Ok(reqObj);
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return this.Ok(reqObj);
        }

        return this.PhysicalFile(
            logicalRspObj.LocalAbsoluteFileName,
            logicalRspObj.ContentType,
            logicalRspObj.FileName);
    }

    /// <summary>匯入Teams</summary>
    [HttpPost]
    public async Task<MbsCrmActCtlImportTeamsRspMdl> ImportTeams([FromForm] MbsCrmActCtlImportTeamsReqMdl reqObj)
    {
        var rspObj = new MbsCrmActCtlImportTeamsRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-活動管理-匯入Teams
        var logicalReqObj = new MbsCrmActLgcImportTeamsReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            TeamsFile = reqObj.TeamsFile
        };
        var logicalRspObj = await this._mbsCrmActLogical.ImportTeamsAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            rspObj.ErrorDataList = logicalRspObj.ErrorDataList?
                .Select(x => new MbsCrmActCtlImportTeamsRspItemMdl()
                {
                    TeamsName = x.TeamsName,
                    TeamsFirstJoinTime = x.TeamsFirstJoinTime,
                    TeamsLastLeaveTime = x.TeamsLastLeaveTime,
                    TeamsMeetingDuration = x.TeamsMeetingDuration,
                    TeamsEmail = x.TeamsEmail,
                    TeamsParticipantId = x.TeamsParticipantId,
                    TeamsRole = x.TeamsRole,
                    TeamsContacterRegisterLastName = x.TeamsContacterRegisterLastName,
                    TeamsContacterRegisterFirstName = x.TeamsContacterRegisterFirstName,
                    TeamsContacterRegisterEmail = x.TeamsContacterRegisterEmail,
                    TeamsRegistrationTime = x.TeamsRegistrationTime,
                    TeamsRegistrationStatus = x.TeamsRegistrationStatus,
                    TeamsCompanyName = x.TeamsCompanyName,
                    TeamsContacterDepartment = x.TeamsContacterDepartment,
                    TeamsContacterJobTitle = x.TeamsContacterJobTitle,
                    TeamsCompanyTelephone = x.TeamsCompanyTelephone,
                    TeamsContacterPhone = x.TeamsContacterPhone,
                    TeamsActivityInfoSource = x.TeamsActivityInfoSource,
                    TeamsContacterIsConsent = x.TeamsContacterIsConsent
                })
                .ToList();
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ErrorDataList = logicalRspObj.ErrorDataList?
            .Select(x => new MbsCrmActCtlImportTeamsRspItemMdl()
            {
                TeamsName = x.TeamsName,
                TeamsFirstJoinTime = x.TeamsFirstJoinTime,
                TeamsLastLeaveTime = x.TeamsLastLeaveTime,
                TeamsMeetingDuration = x.TeamsMeetingDuration,
                TeamsEmail = x.TeamsEmail,
                TeamsParticipantId = x.TeamsParticipantId,
                TeamsRole = x.TeamsRole,
                TeamsContacterRegisterLastName = x.TeamsContacterRegisterLastName,
                TeamsContacterRegisterFirstName = x.TeamsContacterRegisterFirstName,
                TeamsContacterRegisterEmail = x.TeamsContacterRegisterEmail,
                TeamsRegistrationTime = x.TeamsRegistrationTime,
                TeamsRegistrationStatus = x.TeamsRegistrationStatus,
                TeamsCompanyName = x.TeamsCompanyName,
                TeamsContacterDepartment = x.TeamsContacterDepartment,
                TeamsContacterJobTitle = x.TeamsContacterJobTitle,
                TeamsCompanyTelephone = x.TeamsCompanyTelephone,
                TeamsContacterPhone = x.TeamsContacterPhone,
                TeamsActivityInfoSource = x.TeamsActivityInfoSource,
                TeamsContacterIsConsent = x.TeamsContacterIsConsent
            })
            .ToList();
        return rspObj;
    }

    #endregion
}
