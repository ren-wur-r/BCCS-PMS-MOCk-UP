using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-CRM-商機管理</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsCrmPipelineController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsCrmPipelineController> _logger;

    /// <summary>管理者後台-CRM-商機管理-邏輯服務</summary>
    private readonly IMbsCrmPplLogicalService _mbsCrmPplLogical;

    /// <summary>建構式</summary>
    public MbsCrmPipelineController(
        ILogger<MbsCrmPipelineController> logger,
        IMbsCrmPplLogicalService mbsCrmPplLogical)
    {
        this._logger = logger;
        this._mbsCrmPplLogical = mbsCrmPplLogical;
    }

    #region 名單

    /// <summary>取得多筆名單</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetManyEmployeePipelineRspMdl> GetManyEmployeePipeline(MbsCrmPplCtlGetManyEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetManyEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得多筆名單
        var logicalReqObj = new MbsCrmPplLgcGetManyEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetManyEmployeePipelineAsync(logicalReqObj);
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
            .Select(x => new MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl()
            {
                EmployeePipelineID = x.EmployeePipelineID,
                AtomPipelineStatus = x.AtomPipelineStatus,
                ManagerCompanyName = x.ManagerCompanyName,
                ManagerContacterDepartment = x.ManagerContacterDepartment,
                ManagerContacterJobTitle = x.ManagerContacterJobTitle,
                ManagerContacterName = x.ManagerContacterName,
                ManagerContacterEmail = x.ManagerContacterEmail,
                ManagerContacterPhone = x.ManagerContacterPhone,
                ManagerContacterTelephone = x.ManagerContacterTelephone,
                EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得名單</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetEmployeePipelineRspMdl> GetEmployeePipeline(MbsCrmPplCtlGetEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得名單
        var logicalReqObj = new MbsCrmPplLgcGetEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetEmployeePipelineAsync(logicalReqObj);
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
        rspObj.EmployeePipelineSalerEmployeeID = logicalRspObj.EmployeePipelineSalerEmployeeID;
        rspObj.EmployeePipelineSalerEmployeeName = logicalRspObj.EmployeePipelineSalerEmployeeName;
        rspObj.EmployeePipelineSalerRegionID = logicalRspObj.EmployeePipelineSalerRegionID;
        rspObj.EmployeePipelineSalerRegionName = logicalRspObj.EmployeePipelineSalerRegionName;
        rspObj.EmployeePipelineSalerDepartmentID = logicalRspObj.EmployeePipelineSalerDepartmentID;
        rspObj.EmployeePipelineSalerDepartmentName = logicalRspObj.EmployeePipelineSalerDepartmentName;
        rspObj.Company = logicalRspObj.Company != null ? new MbsCrmPplCtlGetEmployeePipelineRspCompanyItemMdl
        {
            ManagerCompanyID = logicalRspObj.Company.ManagerCompanyID,
            ManagerCompanyUnifiedNumber = logicalRspObj.Company.ManagerCompanyUnifiedNumber,
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
        } : null;
        rspObj.ContacterList = logicalRspObj.ContacterList?.Select(x => new MbsCrmPplCtlGetEmployeePipelineRspContacterItemMdl
        {
            ManagerContacterID = x.ManagerContacterID,
            ManagerContacterName = x.ManagerContacterName,
            ManagerContacterEmail = x.ManagerContacterEmail,
            ManagerContacterPhone = x.ManagerContacterPhone,
            ManagerContacterDepartment = x.ManagerContacterDepartment,
            ManagerContacterJobTitle = x.ManagerContacterJobTitle,
            ManagerContacterTelephone = x.ManagerContacterTelephone,
            ManagerContacterIsConsent = x.ManagerContacterIsConsent,
            ManagerContacterStatus = x.ManagerContacterStatus,
            AtomRatingKind = x.AtomRatingKind,
            EmployeePipelineContacterIsPrimary = x.EmployeePipelineContacterIsPrimary,
            ManagerContacterRemark = x.ManagerContacterRemark
        }).ToList();
        rspObj.PendingEmployeePipelineSaler = logicalRspObj.PendingEmployeePipelineSaler != null ? new MbsCrmPplCtlGetEmployeePipelineRspPendingSalerItemMdl
        {
            EmployeePipelineSalerID = logicalRspObj.PendingEmployeePipelineSaler.EmployeePipelineSalerID,
            EmployeePipelineSalerEmployeeName = logicalRspObj.PendingEmployeePipelineSaler.EmployeePipelineSalerEmployeeName,
            EmployeePipelineSalerStatus = logicalRspObj.PendingEmployeePipelineSaler.EmployeePipelineSalerStatus,
            EmployeePipelineSalerCreateEmployeeName = logicalRspObj.PendingEmployeePipelineSaler.EmployeePipelineSalerCreateEmployeeName,
            EmployeePipelineSalerCreateTime = logicalRspObj.PendingEmployeePipelineSaler.EmployeePipelineSalerCreateTime,
            HasAcceptPermissions = logicalRspObj.PendingEmployeePipelineSaler.HasAcceptPermissions,
            HasRejectPermissions = logicalRspObj.PendingEmployeePipelineSaler.HasRejectPermissions,
            HasReassignPermissions = logicalRspObj.PendingEmployeePipelineSaler.HasReassignPermissions
        } : null;
        rspObj.EmployeePipelineSalerList = logicalRspObj.EmployeePipelineSalerList?.Select(x => new MbsCrmPplCtlGetEmployeePipelineRspSalerItemMdl
        {
            EmployeePipelineSalerID = x.EmployeePipelineSalerID,
            EmployeePipelineSalerStatus = x.EmployeePipelineSalerStatus,
            EmployeePipelineSalerCreateTime = x.EmployeePipelineSalerCreateTime,
            EmployeePipelineSalerCreateEmployeeName = x.EmployeePipelineSalerCreateEmployeeName,
            EmployeePipelineSalerReplyTime = x.EmployeePipelineSalerReplyTime,
            EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
            EmployeePipelineSalerRemark = x.EmployeePipelineSalerRemark
        }).ToList();
        rspObj.EmployeePipelineSalerTrackingList = logicalRspObj.EmployeePipelineSalerTrackingList?.Select(x => new MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl
        {
            EmployeePipelineSalerTrackingID = x.EmployeePipelineSalerTrackingID,
            EmployeePipelineSalerTrackingTime = x.EmployeePipelineSalerTrackingTime,
            ManagerContacterName = x.ManagerContacterName,
            EmployeePipelineSalerTrackingRemark = x.EmployeePipelineSalerTrackingRemark,
            EmployeePipelineSalerTrackingCreateEmployeeName = x.EmployeePipelineSalerTrackingCreateEmployeeName,
            ManagerContacterID = x.ManagerContacterID
        }).ToList();
        rspObj.EmployeePipelineProductList = logicalRspObj.EmployeePipelineProductList?.Select(x => new MbsCrmPplCtlGetEmployeePipelineRspProductItemMdl
        {
            EmployeePipelineProductID = x.EmployeePipelineProductID,
            ManagerProductID = x.ManagerProductID,
            ManagerProductSpecificationID = x.ManagerProductSpecificationID,
            ManagerProductName = x.ManagerProductName,
            ManagerProductMainKindName = x.ManagerProductMainKindName,
            ManagerProductSubKindName = x.ManagerProductSubKindName,
            ManagerProductSpecificationName = x.ManagerProductSpecificationName,
            EmployeePipelineProductSellPrice = x.EmployeePipelineProductSellPrice,
            EmployeePipelineProductClosingPrice = x.EmployeePipelineProductClosingPrice,
            EmployeePipelineProductCostPrice = x.EmployeePipelineProductCostPrice,
            EmployeePipelineProductGrossProfit = x.EmployeePipelineProductGrossProfit,
            EmployeePipelineProductCount = x.EmployeePipelineProductCount,
            EmployeePipelineProductPurchaseKind = x.EmployeePipelineProductPurchaseKind,
            EmployeePipelineProductContractKind = x.EmployeePipelineProductContractKind,
            EmployeePipelineProductContractText = x.EmployeePipelineProductContractText,
            ManagerProductMainKindCommissionRate = x.ManagerProductMainKindCommissionRate
        }).ToList();
        rspObj.EmployeePipelineDueList = logicalRspObj.EmployeePipelineDueList?.Select(x => new MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl
        {
            EmployeePipelineDueID = x.EmployeePipelineDueID,
            EmployeePipelineDueTime = x.EmployeePipelineDueTime,
            EmployeePipelineDueRemark = x.EmployeePipelineDueRemark
        }).ToList();
        rspObj.EmployeePipelineBillList = logicalRspObj.EmployeePipelineBillList?.Select(x => new MbsCrmPplCtlGetEmployeePipelineRspBillItemMdl
        {
            EmployeePipelineBillID = x.EmployeePipelineBillID,
            EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
            EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
            EmployeePipelineBillBillNumber = x.EmployeePipelineBillBillNumber,
            EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
            EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
            EmployeePipelineBillStatus = x.EmployeePipelineBillStatus
        }).ToList();
        return rspObj;
    }

    /// <summary>新增名單</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlAddEmployeePipelineRspMdl> AddEmployeePipeline(MbsCrmPplCtlAddEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlAddEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-新增名單
        var logicalReqObj = new MbsCrmPplLgcAddEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
            ContacterList = reqObj.ContacterList?.Select(x => new MbsCrmPplLgcAddEmployeePipelineReqContacterItemMdl
            {
                ManagerContacterID = x.ManagerContacterID,
                EmployeePipelineContacterIsPrimary = x.EmployeePipelineContacterIsPrimary
            }).ToList(),
            SalerTrackingList = reqObj.SalerTrackingList?.Select(x => new MbsCrmPplLgcAddEmployeePipelineReqSalerTrackingItemMdl
            {
                EmployeePipelineSalerTrackingTime = x.EmployeePipelineSalerTrackingTime,
                ManagerContacterID = x.ManagerContacterID,
                EmployeePipelineSalerTrackingRemark = x.EmployeePipelineSalerTrackingRemark
            }).ToList(),
            ProductList = reqObj.ProductList?.Select(x => new MbsCrmPplLgcAddEmployeePipelineReqProductItemMdl
            {
                ManagerProductID = x.ManagerProductID,
                ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                EmployeePipelineProductSellPrice = x.EmployeePipelineProductSellPrice,
                EmployeePipelineProductClosingPrice = x.EmployeePipelineProductClosingPrice,
                EmployeePipelineProductCostPrice = x.EmployeePipelineProductCostPrice,
                EmployeePipelineProductCount = x.EmployeePipelineProductCount,
                EmployeePipelineProductPurchaseKindID = x.EmployeePipelineProductPurchaseKindID,
                EmployeePipelineProductContractKindID = x.EmployeePipelineProductContractKindID,
                EmployeePipelineProductContractText = x.EmployeePipelineProductContractText
            }).ToList(),
            BillList = reqObj.BillList?.Select(x => new MbsCrmPplLgcAddEmployeePipelineReqBillItemMdl
            {
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
                EmployeePipelineBillStatus = x.EmployeePipelineBillStatus
            }).ToList(),
            DueList = reqObj.DueList?.Select(x => new MbsCrmPplLgcAddEmployeePipelineReqDueItemMdl
            {
                EmployeePipelineDueTime = x.EmployeePipelineDueTime,
                EmployeePipelineDueRemark = x.EmployeePipelineDueRemark
            }).ToList()
        };
        var logicalRspObj = await this._mbsCrmPplLogical.AddEmployeePipelineAsync(logicalReqObj);
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

    /// <summary>更新商機狀態</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlUpdatePipelineStatusRspMdl> UpdatePipelineStatus(MbsCrmPplCtlUpdatePipelineStatusReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdatePipelineStatusRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新商機狀態
        var logicalReqObj = new MbsCrmPplLgcUpdatePipelineStatusReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdatePipelineStatusAsync(logicalReqObj);
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

    /// <summary>轉換商機至專案</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlTransferPipelineToProjectRspMdl> TransferPipelineToProject(MbsCrmPplCtlTransferPipelineToProjectReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlTransferPipelineToProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-轉換商機至專案
        var logicalReqObj = new MbsCrmPplLgcTransferPipelineToProjectReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeeProjectCode = reqObj.EmployeeProjectCode?.Trim(),
            EmployeeProjectName = reqObj.EmployeeProjectName?.Trim(),
            EmployeeProjectStartTime = reqObj.EmployeeProjectStartTime,
            EmployeeProjectEndTime = reqObj.EmployeeProjectEndTime,
            EmployeeProjectContractUrl = reqObj.EmployeeProjectContractUrl?.Trim(),
            EmployeeProjectWorkUrl = reqObj.EmployeeProjectWorkUrl?.Trim(),
            EmployeeProjectMemberEmployeeList = reqObj.EmployeeProjectMemberEmployeeList?
                .Select(x => new MbsCrmPplLgcTransferPipelineToProjectReqItemMdl
                {
                    EmployeeID = x.EmployeeID,
                    EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsCrmPplLogical.TransferPipelineToProjectAsync(logicalReqObj);
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
        rspObj.EmployeeProjectID = logicalRspObj.EmployeeProjectID;
        return rspObj;
    }

    #endregion

    #region 客戶

    /// <summary>取得客戶</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetEmployeePipelineCompanyRspMdl> GetEmployeePipelineCompany(MbsCrmPplCtlGetEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetEmployeePipelineCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得客戶
        var logicalReqObj = new MbsCrmPplLgcGetEmployeePipelineCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetEmployeePipelineCompanyAsync(logicalReqObj);
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
        // 當前客戶
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
        return rspObj;
    }

    /// <summary>更新名單客戶</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlUpdateEmployeePipelineCompanyRspMdl> UpdateEmployeePipelineCompany(MbsCrmPplCtlUpdateEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdateEmployeePipelineCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新名單客戶
        var logicalReqObj = new MbsCrmPplLgcUpdateEmployeePipelineCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdateEmployeePipelineCompanyAsync(logicalReqObj);
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

    /// <summary>新增名單窗口</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlAddEmployeePipelineContacterRspMdl> AddEmployeePipelineContacter(MbsCrmPplCtlAddEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlAddEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-新增名單窗口
        var logicalReqObj = new MbsCrmPplLgcAddEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = reqObj.EmployeePipelineContacterIsPrimary,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.AddEmployeePipelineContacterAsync(logicalReqObj);
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
    public async Task<MbsCrmPplCtlUpdateEmployeePipelineContacterRspMdl> UpdateEmployeePipelineContacter(MbsCrmPplCtlUpdateEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdateEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新名單窗口
        var logicalReqObj = new MbsCrmPplLgcUpdateEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = reqObj.EmployeePipelineContacterIsPrimary,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdateEmployeePipelineContacterAsync(logicalReqObj);
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
    public async Task<MbsCrmPplCtlRemoveEmployeePipelineContacterRspMdl> RemoveEmployeePipelineContacter(MbsCrmPplCtlRemoveEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlRemoveEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-刪除名單窗口
        var logicalReqObj = new MbsCrmPplLgcRemoveEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.RemoveEmployeePipelineContacterAsync(logicalReqObj);
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
    public async Task<MbsCrmPplCtlGetManyEmployeePipelineContacterRspMdl> GetManyEmployeePipelineContacter(MbsCrmPplCtlGetManyEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetManyEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得多筆名單窗口
        var logicalReqObj = new MbsCrmPplLgcGetManyEmployeePipelineContacterReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetManyEmployeePipelineContacterAsync(logicalReqObj);
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
            .Select(x => new MbsCrmPplCtlGetManyEmployeePipelineContacterRspItemMdl()
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

    #region 業務紀錄

    /// <summary>取得多筆商機業務</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetManyEmployeePipelineSalerRspMdl> GetManyEmployeePipelineSaler(MbsCrmPplCtlGetManyEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetManyEmployeePipelineSalerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務
        var logicalReqObj = new MbsCrmPplLgcGetManyEmployeePipelineSalerReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerStatus = reqObj.EmployeePipelineSalerStatus,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetManyEmployeePipelineSalerAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerList = logicalRspObj.EmployeePipelineSalerList
            .Select(x => new MbsCrmPplCtlGetManyEmployeePipelineSalerRspItemMdl
            {
                EmployeePipelineSalerID = x.EmployeePipelineSalerID,
                EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
                EmployeePipelineSalerReplyTime = x.EmployeePipelineSalerReplyTime,
                EmployeePipelineSalerStatus = x.EmployeePipelineSalerStatus,
                EmployeePipelineSalerCreateEmployeeName = x.EmployeePipelineSalerCreateEmployeeName,
                EmployeePipelineSalerRemark = x.EmployeePipelineSalerRemark,
                EmployeePipelineSalerCreateTime = x.EmployeePipelineSalerCreateTime
            })
            .ToList();
        return rspObj;
    }

    /// <summary>處理商機業務</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlHandleEmployeePipelineSalerRspMdl> HandleEmployeePipelineSaler(MbsCrmPplCtlHandleEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlHandleEmployeePipelineSalerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-處理商機業務
        var logicalReqObj = new MbsCrmPplLgcHandleEmployeePipelineSalerReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerStatus = reqObj.EmployeePipelineSalerStatus,
            EmployeePipelineSalerRemark = reqObj.EmployeePipelineSalerRemark,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.HandleEmployeePipelineSalerAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 業務開發紀錄

    /// <summary>取得多筆商機業務開發紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspMdl> GetManyEmployeePipelineSalerTracking(MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務開發紀錄
        var logicalReqObj = new MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetManyEmployeePipelineSalerTrackingAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerTrackingList = logicalRspObj.EmployeePipelineSalerTrackingList
            .Select(x => new MbsCrmPplCtlGetManyEmployeePipelineSalerTrackingRspItemMdl
            {
                EmployeePipelineSalerTrackingID = x.EmployeePipelineSalerTrackingID,
                EmployeePipelineSalerTrackingTime = x.EmployeePipelineSalerTrackingTime,
                ManagerContacterName = x.ManagerContacterName,
                EmployeePipelineSalerTrackingRemark = x.EmployeePipelineSalerTrackingRemark,
                EmployeePipelineSalerTrackingCreateEmployeeName = x.EmployeePipelineSalerTrackingCreateEmployeeName,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>新增商機業務開發紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlAddEmployeePipelineSalerTrackingRspMdl> AddEmployeePipelineSalerTracking(MbsCrmPplCtlAddEmployeePipelineSalerTrackingReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlAddEmployeePipelineSalerTrackingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-新增商機業務開發紀錄
        var logicalReqObj = new MbsCrmPplLgcAddEmployeePipelineSalerTrackingReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerTrackingTime = reqObj.EmployeePipelineSalerTrackingTime,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineSalerTrackingRemark = reqObj.EmployeePipelineSalerTrackingRemark,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.AddEmployeePipelineSalerTrackingAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerTrackingID = logicalRspObj.EmployeePipelineSalerTrackingID;
        return rspObj;
    }

    #endregion

    #region 履約期限通知

    /// <summary>取得商機履約期限</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetEmployeePipelineDueRspMdl> GetEmployeePipelineDue(MbsCrmPplCtlGetEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得商機履約期限
        var logicalReqObj = new MbsCrmPplLgcGetEmployeePipelineDueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetEmployeePipelineDueAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineDueID = logicalRspObj.EmployeePipelineDueID;
        rspObj.EmployeePipelineID = logicalRspObj.EmployeePipelineID;
        rspObj.EmployeePipelineDueTime = logicalRspObj.EmployeePipelineDueTime;
        rspObj.EmployeePipelineDueRemark = logicalRspObj.EmployeePipelineDueRemark;
        return rspObj;
    }

    /// <summary>取得多筆商機履約期限</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetManyEmployeePipelineDueRspMdl> GetManyEmployeePipelineDue(MbsCrmPplCtlGetManyEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetManyEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得多筆商機履約期限
        var logicalReqObj = new MbsCrmPplLgcGetManyEmployeePipelineDueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetManyEmployeePipelineDueAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineDueList = logicalRspObj.EmployeePipelineDueList
            .Select(x => new MbsCrmPplCtlGetManyEmployeePipelineDueRspItemMdl
            {
                EmployeePipelineDueID = x.EmployeePipelineDueID,
                EmployeePipelineID = x.EmployeePipelineID,
                EmployeePipelineDueTime = x.EmployeePipelineDueTime,
                EmployeePipelineDueRemark = x.EmployeePipelineDueRemark,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>新增商機履約期限</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlAddEmployeePipelineDueRspMdl> AddEmployeePipelineDue(MbsCrmPplCtlAddEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlAddEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-新增商機履約期限
        var logicalReqObj = new MbsCrmPplLgcAddEmployeePipelineDueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineDueTime = reqObj.EmployeePipelineDueTime,
            EmployeePipelineDueRemark = reqObj.EmployeePipelineDueRemark,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.AddEmployeePipelineDueAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineDueID = logicalRspObj.EmployeePipelineDueID;
        return rspObj;
    }

    /// <summary>更新商機履約期限</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlUpdateEmployeePipelineDueRspMdl> UpdateEmployeePipelineDue(MbsCrmPplCtlUpdateEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdateEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新商機履約期限
        var logicalReqObj = new MbsCrmPplLgcUpdateEmployeePipelineDueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID,
            EmployeePipelineDueTime = reqObj.EmployeePipelineDueTime,
            EmployeePipelineDueRemark = reqObj.EmployeePipelineDueRemark,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdateEmployeePipelineDueAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>刪除商機履約期限</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlRemoveEmployeePipelineDueRspMdl> RemoveEmployeePipelineDue(MbsCrmPplCtlRemoveEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlRemoveEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-刪除商機履約期限
        var logicalReqObj = new MbsCrmPplLgcRemoveEmployeePipelineDueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.RemoveEmployeePipelineDueAsync(logicalReqObj);
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

        // rsp
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 商機產品

    /// <summary>取得商機產品</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetEmployeePipelineProductRspMdl> GetEmployeePipelineProduct(MbsCrmPplCtlGetEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得商機產品
        var logicalReqObj = new MbsCrmPplLgcGetEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
        };

        var logicalRspObj = await this._mbsCrmPplLogical.GetEmployeePipelineProductAsync(logicalReqObj);
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
        rspObj.EmployeePipelineProductSellPrice = logicalRspObj.EmployeePipelineProductSellPrice;
        rspObj.EmployeePipelineProductClosingPrice = logicalRspObj.EmployeePipelineProductClosingPrice;
        rspObj.EmployeePipelineProductCostPrice = logicalRspObj.EmployeePipelineProductCostPrice;
        rspObj.EmployeePipelineProductCount = logicalRspObj.EmployeePipelineProductCount;
        rspObj.EmployeePipelineProductPurchaseKindID = logicalRspObj.EmployeePipelineProductPurchaseKindID;
        rspObj.EmployeePipelineProductContractKindID = logicalRspObj.EmployeePipelineProductContractKindID;
        rspObj.EmployeePipelineProductContractText = logicalRspObj.EmployeePipelineProductContractText;
        return rspObj;
    }

    /// <summary>新增商機產品</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlAddEmployeePipelineProductRspMdl> AddEmployeePipelineProduct(MbsCrmPplCtlAddEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlAddEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-新增商機產品
        var logicalReqObj = new MbsCrmPplLgcAddEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            EmployeePipelineProductSellPrice = reqObj.EmployeePipelineProductSellPrice,
            EmployeePipelineProductClosingPrice = reqObj.EmployeePipelineProductClosingPrice,
            EmployeePipelineProductCostPrice = reqObj.EmployeePipelineProductCostPrice,
            EmployeePipelineProductCount = reqObj.EmployeePipelineProductCount,
            EmployeePipelineProductPurchaseKindID = reqObj.EmployeePipelineProductPurchaseKindID,
            EmployeePipelineProductContractKindID = reqObj.EmployeePipelineProductContractKindID,
            EmployeePipelineProductContractText = reqObj.EmployeePipelineProductContractText,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.AddEmployeePipelineProductAsync(logicalReqObj);
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

    /// <summary>更新商機產品</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlUpdateEmployeePipelineProductRspMdl> UpdateEmployeePipelineProduct(MbsCrmPplCtlUpdateEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdateEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新商機產品
        var logicalReqObj = new MbsCrmPplLgcUpdateEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            EmployeePipelineProductSellPrice = reqObj.EmployeePipelineProductSellPrice,
            EmployeePipelineProductClosingPrice = reqObj.EmployeePipelineProductClosingPrice,
            EmployeePipelineProductCostPrice = reqObj.EmployeePipelineProductCostPrice,
            EmployeePipelineProductCount = reqObj.EmployeePipelineProductCount,
            EmployeePipelineProductPurchaseKindID = reqObj.EmployeePipelineProductPurchaseKindID,
            EmployeePipelineProductContractKindID = reqObj.EmployeePipelineProductContractKindID,
            EmployeePipelineProductContractText = reqObj.EmployeePipelineProductContractText,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdateEmployeePipelineProductAsync(logicalReqObj);
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

    /// <summary>刪除商機產品</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlRemoveEmployeePipelineProductRspMdl> RemoveEmployeePipelineProduct(MbsCrmPplCtlRemoveEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlRemoveEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-刪除商機產品
        var logicalReqObj = new MbsCrmPplLgcRemoveEmployeePipelineProductReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.RemoveEmployeePipelineProductAsync(logicalReqObj);
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

    #region 發票紀錄

    /// <summary>取得商機發票紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetEmployeePipelineBillRspMdl> GetEmployeePipelineBill(MbsCrmPplCtlGetEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得商機發票紀錄
        var logicalReqObj = new MbsCrmPplLgcGetEmployeePipelineBillReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetEmployeePipelineBillAsync(logicalReqObj);
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
        rspObj.EmployeePipelineBillID = logicalRspObj.EmployeePipelineBillID;
        rspObj.EmployeePipelineBillPeriodNumber = logicalRspObj.EmployeePipelineBillPeriodNumber;
        rspObj.EmployeePipelineBillBillTime = logicalRspObj.EmployeePipelineBillBillTime;
        rspObj.EmployeePipelineBillBillNumber = logicalRspObj.EmployeePipelineBillBillNumber;
        rspObj.EmployeePipelineBillNoTaxAmount = logicalRspObj.EmployeePipelineBillNoTaxAmount;
        rspObj.EmployeePipelineBillRemark = logicalRspObj.EmployeePipelineBillRemark;
        rspObj.EmployeePipelineBillStatus = logicalRspObj.EmployeePipelineBillStatus;
        return rspObj;
    }

    /// <summary>取得多筆商機發票紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlGetManyEmployeePipelineBillRspMdl> GetManyEmployeePipelineBill(MbsCrmPplCtlGetManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlGetManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-取得多筆商機發票紀錄
        var logicalReqObj = new MbsCrmPplLgcGetManyEmployeePipelineBillReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.GetManyEmployeePipelineBillAsync(logicalReqObj);
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineBillList = logicalRspObj.EmployeePipelineBillList
            .Select(x => new MbsCrmPplCtlGetManyEmployeePipelineBillRspItemMdl
            {
                EmployeePipelineBillID = x.EmployeePipelineBillID,
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>新增多筆商機發票紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlAddManyEmployeePipelineBillRspMdl> AddManyEmployeePipelineBill(MbsCrmPplCtlAddManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlAddManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-新增多筆商機發票紀錄
        var logicalReqObj = new MbsCrmPplLgcAddManyEmployeePipelineBillReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineBillList = reqObj.EmployeePipelineBillList
            .Select(x => new MbsCrmPplLgcAddManyEmployeePipelineBillReqItemMdl()
            {
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
            })
            .ToList()
        };
        var logicalRspObj = await this._mbsCrmPplLogical.AddManyEmployeePipelineBillAsync(logicalReqObj);
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

    /// <summary>更新商機發票紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlUpdateEmployeePipelineBillRspMdl> UpdateEmployeePipelineBill(MbsCrmPplCtlUpdateEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdateEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新商機發票紀錄
        var logicalReqObj = new MbsCrmPplLgcUpdateEmployeePipelineReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
            EmployeePipelineBillNumber = reqObj.EmployeePipelineBillNumber?.Trim(),
            EmployeePipelineBillStatus = reqObj.EmployeePipelineBillStatus,
            EmployeePipelineBillRemark = reqObj.EmployeePipelineBillRemark,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdateEmployeePipelineBillAsync(logicalReqObj);
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

    /// <summary>更新多筆商機發票紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlUpdateManyEmployeePipelineBillRspMdl> UpdateManyEmployeePipelineBill(MbsCrmPplCtlUpdateManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlUpdateManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };
        // logical, 管理者後台-CRM-商機管理-邏輯服務-更新多筆商機發票紀錄
        var logicalReqObj = new MbsCrmPplLgcUpdateManyEmployeePipelineBillReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineBillList = reqObj.EmployeePipelineBillList
            .Select(x => new MbsCrmPplLgcUpdateManyEmployeePipelineBillReqItemMdl()
            {
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
            })
            .ToList()
        };
        var logicalRspObj = await this._mbsCrmPplLogical.UpdateManyEmployeePipelineBillAsync(logicalReqObj);
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

    /// <summary>刪除多筆商機發票紀錄</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlRemoveManyEmployeePipelineBillRspMdl> RemoveManyEmployeePipelineBill(MbsCrmPplCtlRemoveManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlRemoveManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-刪除多筆商機發票紀錄
        var logicalReqObj = new MbsCrmPplLgcRemoveManyEmployeePipelineBillReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.RemoveManyEmployeePipelineBillAsync(logicalReqObj);
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

    /// <summary>通知開立發票</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlNotifyBillIssueRspMdl> NotifyBillIssue(MbsCrmPplCtlNotifyBillIssueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlNotifyBillIssueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-通知開立發票
        var logicalReqObj = new MbsCrmPplLgcNotifyBillIssueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
        };
        var logicalRspObj = await this._mbsCrmPplLogical.NotifyBillIssueAsync(logicalReqObj);
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

    /// <summary>確認開立發票</summary>
    [HttpPost]
    public async Task<MbsCrmPplCtlConfirmBillIssueRspMdl> ConfirmBillIssue(MbsCrmPplCtlConfirmBillIssueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplCtlConfirmBillIssueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-CRM-商機管理-邏輯服務-確認開立發票
        var logicalReqObj = new MbsCrmPplLgcConfirmBillIssueReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
            EmployeePipelineBillNumber = reqObj.EmployeePipelineBillNumber?.Trim(),
            EmployeePipelineBillRemark = reqObj.EmployeePipelineBillRemark?.Trim(),
        };
        var logicalRspObj = await this._mbsCrmPplLogical.ConfirmBillIssueAsync(logicalReqObj);
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
