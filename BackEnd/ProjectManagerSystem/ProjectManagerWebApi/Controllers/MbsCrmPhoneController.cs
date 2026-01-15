using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-CRM-電銷管理</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsCrmPhoneController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsCrmPhoneController> _logger;

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務</summary>
    private readonly IMbsCrmPhnLogicalService _mbsCrmPhnLogical;

    /// <summary>建構式</summary>
    public MbsCrmPhoneController(
        ILogger<MbsCrmPhoneController> logger,
        IMbsCrmPhnLogicalService mbsCrmPhnLogical)
    {
        this._logger = logger;
        this._mbsCrmPhnLogical = mbsCrmPhnLogical;
    }

    #region 活動

    /// <summary>取得多筆活動</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetManyActivityRspMdl> GetManyActivity(MbsCrmPhnCtlGetManyActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動
        var logicalReqObj = new MbsCrmPhnLgcGetManyActivityReqMdl()
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
        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyActivityAsync(logicalReqObj);
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
            .Select(x => new MbsCrmPhnCtlGetManyActivityRspItemMdl()
            {
                ManagerActivityID = x.ManagerActivityID,
                ManagerActivityName = x.ManagerActivityName,
                ManagerActivityKind = x.ManagerActivityKind,
                ManagerActivityStartTime = x.ManagerActivityStartTime,
                ManagerActivityEndTime = x.ManagerActivityEndTime,
                ManagerActivityRegisteredCount = x.ManagerActivityRegisteredCount,
                ManagerActivityTransferredToSalesCount = x.ManagerActivityTransferredToSalesCount,
                ManagerActivityPhoneCount = x.ManagerActivityPhoneCount,
                ManagerActivityEmployeePipelineCount = x.ManagerActivityEmployeePipelineCount,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得單筆活動</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetActivityRspMdl> GetActivity(MbsCrmPhnCtlGetActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動
        var logicalReqObj = new MbsCrmPhnLgcGetActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetActivityAsync(logicalReqObj);
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
        rspObj.ManagerActivityPhoneCount = logicalRspObj.ManagerActivityPhoneCount;
        rspObj.ManagerActivityProductList = logicalRspObj.ManagerActivityProductList?
            .Select(x => new MbsCrmPhnCtlGetActivityProductRspItemMdl()
            {
                ManagerActivityProductID = x.ManagerActivityProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
            })
            .ToList();
        rspObj.ManagerActivitySponsorList = logicalRspObj.ManagerActivitySponsorList?
            .Select(x => new MbsCrmPhnCtlGetActivitySponsorRspItemMdl()
            {
                ManagerActivitySponsorID = x.ManagerActivitySponsorID,
                ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount,
            })
            .ToList();
        rspObj.ManagerActivityExpenseList = logicalRspObj.ManagerActivityExpenseList?
            .Select(x => new MbsCrmPhnCtlGetActivityExpenseRspItemMdl()
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
    public async Task<MbsCrmPhnCtlGetManyActivityEmployeePipelineRspMdl> GetManyActivityEmployeePipeline(MbsCrmPhnCtlGetManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單
        var logicalReqObj = new MbsCrmPhnLgcGetManyActivityEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerActivityID = reqObj.ManagerActivityID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyActivityEmployeePipelineAsync(logicalReqObj);
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
            .Select(x => new MbsCrmPhnCtlGetManyEmployeePipelineRspItemMdl()
            {
                EmployeePipelineID = x.EmployeePipelineID,
                AtomPipelineStatus = x.AtomPipelineStatus,
                HasCompany = x.HasCompany,
                ManagerCompanyID = x.ManagerCompanyID,
                ManagerCompanyName = x.ManagerCompanyName,
                HasContacter = x.HasContacter,
                EmployeePipelineContacterID = x.EmployeePipelineContacterID,
                ManagerContacterDepartment = x.ManagerContacterDepartment,
                ManagerContacterJobTitle = x.ManagerContacterJobTitle,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail,
                ManagerContacterPhone = x.ManagerContacterPhone,
                ManagerContacterTelephone = x.ManagerContacterTelephone,
                EmployeePipelineSalerTrackingTime = x.EmployeePipelineSalerTrackingTime
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得活動名單</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetActivityEmployeePipelineRspMdl> GetActivityEmployeePipeline(MbsCrmPhnCtlGetActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetActivityEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得活動名單
        var logicalReqObj = new MbsCrmPhnLgcGetActivityEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetActivityEmployeePipelineAsync(logicalReqObj);
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
        rspObj.OriginalCompany = logicalRspObj.OriginalCompany == null ? null : new MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalCompanyItemMdl
        {
            ManagerCompanyUnifiedNumber = logicalRspObj.OriginalCompany.ManagerCompanyUnifiedNumber,
            ManagerCompanyName = logicalRspObj.OriginalCompany.ManagerCompanyName,
            AtomEmployeeRange = logicalRspObj.OriginalCompany.AtomEmployeeRange,
            AtomCustomerGrade = logicalRspObj.OriginalCompany.AtomCustomerGrade,
            ManagerCompanyMainKindName = logicalRspObj.OriginalCompany.ManagerCompanyMainKindName,
            ManagerCompanySubKindName = logicalRspObj.OriginalCompany.ManagerCompanySubKindName,
            AtomCity = logicalRspObj.OriginalCompany.AtomCity,
            ManagerCompanyLocationAddress = logicalRspObj.OriginalCompany.ManagerCompanyLocationAddress,
            ManagerCompanyLocationTelephone = logicalRspObj.OriginalCompany.ManagerCompanyLocationTelephone,
            ManagerCompanyLocationStatus = logicalRspObj.OriginalCompany.ManagerCompanyLocationStatus
        };
        rspObj.HasCompany = logicalRspObj.HasCompany;
        rspObj.Company = logicalRspObj.Company == null ? null : new MbsCrmPhnCtlGetActivityEmployeePipelineRspCompanyItemMdl
        {
            ManagerCompanyUnifiedNumber = logicalRspObj.Company.ManagerCompanyUnifiedNumber,
            ManagerCompanyID = logicalRspObj.Company.ManagerCompanyID,
            ManagerCompanyName = logicalRspObj.Company.ManagerCompanyName,
            AtomEmployeeRange = logicalRspObj.Company.AtomEmployeeRange,
            AtomCustomerGrade = logicalRspObj.Company.AtomCustomerGrade,
            ManagerCompanyMainKindName = logicalRspObj.Company.ManagerCompanyMainKindName,
            ManagerCompanySubKindName = logicalRspObj.Company.ManagerCompanySubKindName,
            AtomCity = logicalRspObj.Company.AtomCity,
            ManagerCompanyLocationID = logicalRspObj.Company.ManagerCompanyLocationID,
            ManagerCompanyLocationAddress = logicalRspObj.Company.ManagerCompanyLocationAddress,
            ManagerCompanyLocationTelephone = logicalRspObj.Company.ManagerCompanyLocationTelephone,
            ManagerCompanyLocationStatus = logicalRspObj.Company.ManagerCompanyLocationStatus
        };
        rspObj.OriginalContacter = logicalRspObj.OriginalContacter == null ? null : new MbsCrmPhnCtlGetActivityEmployeePipelineRspOriginalContacterItemMdl
        {
            ManagerContacterName = logicalRspObj.OriginalContacter.ManagerContacterName,
            ManagerContacterEmail = logicalRspObj.OriginalContacter.ManagerContacterEmail,
            ManagerContacterPhone = logicalRspObj.OriginalContacter.ManagerContacterPhone,
            ManagerContacterDepartment = logicalRspObj.OriginalContacter.ManagerContacterDepartment,
            ManagerContacterJobTitle = logicalRspObj.OriginalContacter.ManagerContacterJobTitle,
            ManagerContacterTelephone = logicalRspObj.OriginalContacter.ManagerContacterTelephone,
            ManagerContacterIsConsent = logicalRspObj.OriginalContacter.ManagerContacterIsConsent,
            ManagerContacterStatus = logicalRspObj.OriginalContacter.ManagerContacterStatus,
            AtomRatingKind = logicalRspObj.OriginalContacter.AtomRatingKind
        };
        rspObj.ContacterList = logicalRspObj.ContacterList?.Select(x => new MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl
        {
            ManagerContacterID = x.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = x.EmployeePipelineContacterIsPrimary,
            ManagerContacterName = x.ManagerContacterName,
            ManagerContacterEmail = x.ManagerContacterEmail,
            ManagerContacterPhone = x.ManagerContacterPhone,
            ManagerContacterDepartment = x.ManagerContacterDepartment,
            ManagerContacterJobTitle = x.ManagerContacterJobTitle,
            ManagerContacterTelephone = x.ManagerContacterTelephone,
            ManagerContacterIsConsent = x.ManagerContacterIsConsent,
            ManagerContacterStatus = x.ManagerContacterStatus,
            AtomRatingKind = x.AtomRatingKind,
            ManagerContacterRemark = x.ManagerContacterRemark
        }).ToList();
        rspObj.TeamsMeetingDuration = logicalRspObj.TeamsMeetingDuration;
        rspObj.TeamsRole = logicalRspObj.TeamsRole;
        rspObj.ProductList = logicalRspObj.ProductList?.Select(x => new MbsCrmPhnCtlGetActivityEmployeePipelineRspProductItemMdl
        {
            EmployeePipelineProductID = x.EmployeePipelineProductID,
            ManagerProductID = x.ManagerProductID,
            ManagerProductName = x.ManagerProductName,
            ManagerProductMainKindID = x.ManagerProductMainKindID,
            ManagerProductMainKindName = x.ManagerProductMainKindName,
            ManagerProductSubKindID = x.ManagerProductSubKindID,
            ManagerProductSubKindName = x.ManagerProductSubKindName,
            ManagerProductSpecificationID = x.ManagerProductSpecificationID,
            ManagerProductSpecificationName = x.ManagerProductSpecificationName,
            ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice
        }).ToList();
        rspObj.PhoneList = logicalRspObj.PhoneList?.Select(x => new MbsCrmPhnCtlGetActivityEmployeePipelineRspPhoneItemMdl
        {
            EmployeePipelinePhoneID = x.EmployeePipelinePhoneID,
            EmployeePipelinePhoneRecordTime = x.EmployeePipelinePhoneRecordTime,
            ManagerContacterName = x.ManagerContacterName,
            EmployeePipelinePhoneTitle = x.EmployeePipelinePhoneTitle,
            EmployeePipelinePhoneRemark = x.EmployeePipelinePhoneRemark,
            EmployeePipelinePhoneCreateEmployeeName = x.EmployeePipelinePhoneCreateEmployeeName
        }).ToList();
        rspObj.SalerList = logicalRspObj.SalerList?.Select(x => new MbsCrmPhnCtlGetActivityEmployeePipelineRspSalerItemMdl
        {
            EmployeePipelineSalerID = x.EmployeePipelineSalerID,
            EmployeePipelineSalerStatus = x.EmployeePipelineSalerStatus,
            EmployeePipelineSalerCreateTime = x.EmployeePipelineSalerCreateTime,
            EmployeePipelineSalerCreateEmployeeName = x.EmployeePipelineSalerCreateEmployeeName,
            EmployeePipelineSalerReplyTime = x.EmployeePipelineSalerReplyTime,
            EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
            EmployeePipelineSalerRemark = x.EmployeePipelineSalerRemark
        }).ToList();
        return rspObj;
    }

    /// <summary>更新活動名單狀態</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusRspMdl> UpdateActivityEmployeePipelineStatus(MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlUpdateActivityEmployeePipelineStatusRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-更新活動名單狀態
        var logicalReqObj = new MbsCrmPhnLgcUpdateActivityEmployeePipelineStatusReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.UpdateActivityEmployeePipelineStatusAsync(logicalReqObj);
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
    public async Task<MbsCrmPhnCtlGetManyPastActivityRspMdl> GetManyPastActivity(MbsCrmPhnCtlGetManyPastActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyPastActivityRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得多筆客戶過往活動
        var logicalReqObj = new MbsCrmPhnLgcGetManyPastActivityReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyPastActivityAsync(logicalReqObj);
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
            ? new MbsCrmPhnCtlGetManyPastActivityRspItemMdl()
            {
                ManagerActivityName = logicalRspObj.LatestPastActivity.ManagerActivityName,
                ManagerActivityStartTime = logicalRspObj.LatestPastActivity.ManagerActivityStartTime,
                ManagerActivityEndTime = logicalRspObj.LatestPastActivity.ManagerActivityEndTime,
            }
            : null;
        rspObj.PastActivityList = logicalRspObj.PastActivityList?
            .Select(x => new MbsCrmPhnCtlGetManyPastActivityRspItemMdl()
            {
                ManagerActivityName = x.ManagerActivityName,
                ManagerActivityStartTime = x.ManagerActivityStartTime,
                ManagerActivityEndTime = x.ManagerActivityEndTime,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    #endregion

    #region 客戶

    /// <summary>取得客戶</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetEmployeePipelineCompanyRspMdl> GetEmployeePipelineCompany(MbsCrmPhnCtlGetEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetEmployeePipelineCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得原始客戶
        var logicalReqObj = new MbsCrmPhnLgcGetEmployeePipelineCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetEmployeePipelineCompanyAsync(logicalReqObj);
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
        rspObj.OriginalCompany = new MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl()
        {
            ManagerCompanyUnifiedNumber = logicalRspObj.OriginalCompany.ManagerCompanyUnifiedNumber,
            ManagerCompanyName = logicalRspObj.OriginalCompany.ManagerCompanyName,
            AtomEmployeeRange = logicalRspObj.OriginalCompany.AtomEmployeeRange,
            AtomCustomerGrade = logicalRspObj.OriginalCompany.AtomCustomerGrade,
            ManagerCompanyMainKindName = logicalRspObj.OriginalCompany.ManagerCompanyMainKindName,
            ManagerCompanySubKindName = logicalRspObj.OriginalCompany.ManagerCompanySubKindName,
            AtomCity = logicalRspObj.OriginalCompany.AtomCity,
            ManagerCompanyLocationAddress = logicalRspObj.OriginalCompany.ManagerCompanyLocationAddress,
            ManagerCompanyLocationTelephone = logicalRspObj.OriginalCompany.ManagerCompanyLocationTelephone,
            ManagerCompanyLocationStatus = logicalRspObj.OriginalCompany.ManagerCompanyLocationStatus
        };
        if (logicalRspObj.Company != default)
        {
            rspObj.Company = new MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl()
            {
                ManagerCompanyUnifiedNumber = logicalRspObj.Company.ManagerCompanyUnifiedNumber,
                ManagerCompanyName = logicalRspObj.Company.ManagerCompanyName,
                AtomEmployeeRange = logicalRspObj.Company.AtomEmployeeRange,
                AtomCustomerGrade = logicalRspObj.Company.AtomCustomerGrade,
                ManagerCompanyMainKindName = logicalRspObj.Company.ManagerCompanyMainKindName,
                ManagerCompanySubKindName = logicalRspObj.Company.ManagerCompanySubKindName,
                AtomCity = logicalRspObj.Company.AtomCity,
                ManagerCompanyLocationName = logicalRspObj.Company.ManagerCompanyLocationName,
                ManagerCompanyLocationAddress = logicalRspObj.Company.ManagerCompanyLocationAddress,
                ManagerCompanyLocationTelephone = logicalRspObj.Company.ManagerCompanyLocationTelephone,
                ManagerCompanyLocationStatus = logicalRspObj.Company.ManagerCompanyLocationStatus
            };
        }
        return rspObj;
    }

    /// <summary>更新名單客戶</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlUpdateEmployeePipelineCompanyRspMdl> UpdateEmployeePipelineCompany(MbsCrmPhnCtlUpdateEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlUpdateEmployeePipelineCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-更新名單客戶
        var logicalReqObj = new MbsCrmPhnLgcUpdateEmployeePipelineCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.UpdateEmployeePipelineCompanyAsync(logicalReqObj);
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

    #region 窗口

    /// <summary>取得原始窗口</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetOriginalEmployeePipelineContacterRspMdl> GetOriginalEmployeePipelineContacter(MbsCrmPhnCtlGetOriginalEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetOriginalEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得原始窗口
        var logicalReqObj = new MbsCrmPhnLgcGetOriginalEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetOriginalEmployeePipelineContacterAsync(logicalReqObj);
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
        rspObj.ManagerContacterName = logicalRspObj.ManagerContacterName;
        rspObj.ManagerContacterEmail = logicalRspObj.ManagerContacterEmail;
        rspObj.ManagerContacterPhone = logicalRspObj.ManagerContacterPhone;
        rspObj.ManagerContacterDepartment = logicalRspObj.ManagerContacterDepartment;
        rspObj.ManagerContacterJobTitle = logicalRspObj.ManagerContacterJobTitle;
        rspObj.ManagerContacterTelephone = logicalRspObj.ManagerContacterTelephone;
        rspObj.ManagerContacterIsConsent = logicalRspObj.ManagerContacterIsConsent;
        rspObj.ManagerContacterStatus = logicalRspObj.ManagerContacterStatus;
        rspObj.AtomRatingKind = logicalRspObj.AtomRatingKind;
        return rspObj;
    }

    /// <summary>新增名單窗口</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlAddEmployeePipelineContacterRspMdl> AddEmployeePipelineContacter(MbsCrmPhnCtlAddEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlAddEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-新增名單窗口
        var logicalReqObj = new MbsCrmPhnLgcAddEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = reqObj.EmployeePipelineContacterIsPrimary,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.AddEmployeePipelineContacterAsync(logicalReqObj);
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

    /// <summary>更新名單窗口</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlUpdateEmployeePipelineContacterRspMdl> UpdateEmployeePipelineContacter(MbsCrmPhnCtlUpdateEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlUpdateEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-更新名單窗口
        var logicalReqObj = new MbsCrmPhnLgcUpdateEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = reqObj.EmployeePipelineContacterIsPrimary,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.UpdateEmployeePipelineContacterAsync(logicalReqObj);
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

    /// <summary>刪除名單窗口</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlRemoveEmployeePipelineContacterRspMdl> RemoveEmployeePipelineContacter(MbsCrmPhnCtlRemoveEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlRemoveEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-刪除名單窗口
        var logicalReqObj = new MbsCrmPhnLgcRemoveEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.RemoveEmployeePipelineContacterAsync(logicalReqObj);
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

    /// <summary>取得多筆名單窗口</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetManyEmployeePipelineContacterRspMdl> GetManyEmployeePipelineContacter(MbsCrmPhnCtlGetManyEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsCrmPhnLgcGetManyEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };

        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyEmployeePipelineContacterAsync(logicalReqObj);
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
        rspObj.EmployeePipelineContacterList = logicalRspObj.EmployeePipelineContacterList?
            .Select(x => new MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl
            {
                EmployeePipelineContacterID = x.EmployeePipelineContacterID,
                ManagerContacterID = x.ManagerContacterID,
                EmployeePipelineContacterIsPrimary = x.EmployeePipelineContacterIsPrimary,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail,
                ManagerContacterPhone = x.ManagerContacterPhone,
                ManagerContacterDepartment = x.ManagerContacterDepartment,
                ManagerContacterJobTitle = x.ManagerContacterJobTitle,
                ManagerContacterTelephone = x.ManagerContacterTelephone,
                ManagerContacterIsConsent = x.ManagerContacterIsConsent,
                ManagerContacterStatus = x.ManagerContacterStatus,
                AtomRatingKind = x.AtomRatingKind,
                ManagerContacterRemark = x.ManagerContacterRemark,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 指派業務紀錄

    /// <summary>指派業務</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlAddEmployeePipelineSalerRspMdl> AddEmployeePipelineSaler(MbsCrmPhnCtlAddEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlAddEmployeePipelineSalerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-指派業務
        var logicalReqObj = new MbsCrmPhnLgcAddEmployeePipelineSalerReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.AddEmployeePipelineSalerAsync(logicalReqObj);
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

    /// <summary>取得多筆指派業務</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetManyEmployeePipelineSalerRspMdl> GetManyEmployeePipelineSaler(MbsCrmPhnCtlGetManyEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyEmployeePipelineSalerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsCrmPhnLgcGetManyEmployeePipelineSalerReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };

        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyEmployeePipelineSalerAsync(logicalReqObj);
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
        rspObj.EmployeePipelineSalerList = logicalRspObj.EmployeePipelineSalerList?
            .Select(x => new MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl
            {
                EmployeePipelineSalerID = x.EmployeePipelineSalerID,
                EmployeePipelineSalerStatus = x.EmployeePipelineSalerStatus,
                EmployeePipelineSalerCreateTime = x.EmployeePipelineSalerCreateTime,
                EmployeePipelineSalerCreateEmployeeName = x.EmployeePipelineSalerCreateEmployeeName,
                EmployeePipelineSalerReplyTime = x.EmployeePipelineSalerReplyTime,
                EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
                EmployeePipelineSalerRemark = x.EmployeePipelineSalerRemark
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 電銷紀錄

    /// <summary>新增電銷紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlAddEmployeePipelinePhoneRspMdl> AddEmployeePipelinePhone(MbsCrmPhnCtlAddEmployeePipelinePhoneReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlAddEmployeePipelinePhoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsCrmPhnLgcAddEmployeePipelinePhoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelinePhoneRecordTime = reqObj.EmployeePipelinePhoneRecordTime,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelinePhoneTitle = reqObj.EmployeePipelinePhoneTitle?.Trim(),
            EmployeePipelinePhoneRemark = reqObj.EmployeePipelinePhoneRemark?.Trim(),
        };

        var logicalRspObj = await this._mbsCrmPhnLogical.AddEmployeePipelinePhoneAsync(logicalReqObj);
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

    /// <summary>取得多筆電銷紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspMdl> GetManyEmployeePipelinePhone(MbsCrmPhnCtlGetManyEmployeePipelinePhoneReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsCrmPhnLgcGetManyEmployeePipelinePhoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };

        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyEmployeePipelinePhoneAsync(logicalReqObj);
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
        rspObj.EmployeePipelinePhoneList = logicalRspObj.EmployeePipelinePhoneList?
            .Select(x => new MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl
            {
                EmployeePipelinePhoneID = x.EmployeePipelinePhoneID,
                EmployeePipelinePhoneRecordTime = x.EmployeePipelinePhoneRecordTime,
                ManagerContacterName = x.ManagerContacterName,
                EmployeePipelinePhoneTitle = x.EmployeePipelinePhoneTitle,
                EmployeePipelinePhoneCreateEmployeeName = x.EmployeePipelinePhoneCreateEmployeeName,
                EmployeePipelinePhoneRemark = x.EmployeePipelinePhoneRemark,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 電銷產品

    /// <summary>取得電銷產品</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetEmployeePipelineProductRspMdl> GetEmployeePipelineProduct(MbsCrmPhnCtlGetEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得電銷產品
        var logicalReqObj = new MbsCrmPhnLgcGetEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetEmployeePipelineProductAsync(logicalReqObj);
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
        rspObj.ManagerProductMainKindID = logicalRspObj.ManagerProductMainKindID;
        rspObj.ManagerProductMainKindName = logicalRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductSubKindID = logicalRspObj.ManagerProductSubKindID;
        rspObj.ManagerProductSubKindName = logicalRspObj.ManagerProductSubKindName;
        rspObj.ManagerProductID = logicalRspObj.ManagerProductID;
        rspObj.ManagerProductName = logicalRspObj.ManagerProductName;
        rspObj.ManagerProductSpecificationID = logicalRspObj.ManagerProductSpecificationID;
        rspObj.ManagerProductSpecificationName = logicalRspObj.ManagerProductSpecificationName;
        return rspObj;
    }

    /// <summary>取得多筆電銷產品</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlGetManyEmployeePipelineProductRspMdl> GetManyEmployeePipelineProduct(MbsCrmPhnCtlGetManyEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlGetManyEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷產品
        var logicalReqObj = new MbsCrmPhnLgcGetManyEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.GetManyEmployeePipelineProductAsync(logicalReqObj);
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
        rspObj.EmployeePipelineProductList = logicalRspObj.EmployeePipelineProductList?
            .Select(x => new MbsCrmPhnCtlGetManyEmployeePipelineProductRspItemMdl()
            {
                EmployeePipelineProductID = x.EmployeePipelineProductID,
                ManagerProductID = x.ManagerProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>新增電銷產品</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlAddEmployeePipelineProductRspMdl> AddEmployeePipelineProduct(MbsCrmPhnCtlAddEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlAddEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-電銷管理-邏輯服務-新增電銷產品
        var logicalReqObj = new MbsCrmPhnLgcAddEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
        };

        var logicalRspObj = await this._mbsCrmPhnLogical.AddEmployeePipelineProductAsync(logicalReqObj);
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
        rspObj.EmployeePipelineProductID = logicalRspObj.EmployeePipelineProductID;
        return rspObj;
    }

    /// <summary>更新電銷產品</summary> 
    [HttpPost]
    public async Task<MbsCrmPhnCtlUpdateEmployeePipelineProductRspMdl> UpdateEmployeePipelineProduct(MbsCrmPhnCtlUpdateEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlUpdateEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };
        var logicalReqObj = new MbsCrmPhnLgcUpdateEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
        };
        var logicalRspObj = await this._mbsCrmPhnLogical.UpdateEmployeePipelineProductAsync(logicalReqObj);
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

    /// <summary>刪除電銷產品</summary>
    [HttpPost]
    public async Task<MbsCrmPhnCtlRemoveEmployeePipelineProductRspMdl> RemoveEmployeePipelineProduct(MbsCrmPhnCtlRemoveEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnCtlRemoveEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        var logicalReqObj = new MbsCrmPhnLgcRemoveEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
        };

        var logicalRspObj = await this._mbsCrmPhnLogical.RemoveEmployeePipelineProductAsync(logicalReqObj);
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