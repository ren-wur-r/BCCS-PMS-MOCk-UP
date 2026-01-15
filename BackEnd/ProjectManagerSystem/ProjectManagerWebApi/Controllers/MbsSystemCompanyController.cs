using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Company;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Company.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統設定-客戶</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsSystemCompanyController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemCompanyController> _logger;

    /// <summary>管理者後台-系統設定-客戶-邏輯服務</summary>
    private readonly IMbsSysCmpLogicalService _mbsSysCmpLogical;

    /// <summary>建構式</summary>
    public MbsSystemCompanyController(
        ILogger<MbsSystemCompanyController> logger,
        IMbsSysCmpLogicalService mbsSysCmpLogical)
    {
        this._logger = logger;
        this._mbsSysCmpLogical = mbsSysCmpLogical;
    }

    #region 客戶主分類

    /// <summary>取得多筆客戶主分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetManyCompanyMainKindRspMdl> GetManyCompanyMainKind(MbsSysCmpCtlGetManyCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetManyCompanyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得多筆客戶主分類
        var logicalReqObj = new MbsSysCmpLgcGetManyCompanyMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName?.Trim(),
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetManyCompanyMainKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanyMainKindList = logicalRspObj.ManagerCompanyMainKindList?
            .Select(x => new MbsSysCmpCtlGetManyCompanyMainKindRspItemMdl()
            {
                ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                ManagerCompanyMainKindName = x.ManagerCompanyMainKindName,
                ManagerCompanyMainKindIsEnable = x.ManagerCompanyMainKindIsEnable,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得客戶主分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetCompanyMainKindRspMdl> GetCompanyMainKind(MbsSysCmpCtlGetCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetCompanyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得客戶主分類
        var logicalReqObj = new MbsSysCmpLgcGetCompanyMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetCompanyMainKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanyMainKindName = logicalRspObj.ManagerCompanyMainKindName;
        rspObj.ManagerCompanyMainKindIsEnable = logicalRspObj.ManagerCompanyMainKindIsEnable;
        return rspObj;
    }

    /// <summary>新增客戶主分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlAddCompanyMainKindRspMdl> AddCompanyMainKind(MbsSysCmpCtlAddCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlAddCompanyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-新增客戶主分類
        var logicalReqObj = new MbsSysCmpLgcAddCompanyMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName.Trim(),
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.AddCompanyMainKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanyMainKindID = logicalRspObj.ManagerCompanyMainKindID;
        return rspObj;
    }

    /// <summary>更新客戶主分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlUpdateCompanyMainKindRspMdl> UpdateCompanyMainKind(MbsSysCmpCtlUpdateCompanyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlUpdateCompanyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-更新客戶主分類
        var logicalReqObj = new MbsSysCmpLgcUpdateCompanyMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName?.Trim(),
            ManagerCompanyMainKindIsEnable = reqObj.ManagerCompanyMainKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.UpdateCompanyMainKindAsync(logicalReqObj);
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

    #region 客戶子分類

    /// <summary>取得多筆客戶子分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetManyCompanySubKindRspMdl> GetManyCompanySubKind(MbsSysCmpCtlGetManyCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetManyCompanySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得多筆客戶子分類
        var logicalReqObj = new MbsSysCmpLgcGetManyCompanySubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName?.Trim(),
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetManyCompanySubKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanySubKindList = logicalRspObj.ManagerCompanySubKindList?
            .Select(x => new MbsSysCmpCtlGetManyCompanySubKindRspItemMdl()
            {
                ManagerCompanySubKindID = x.ManagerCompanySubKindID,
                ManagerCompanySubKindName = x.ManagerCompanySubKindName,
                ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                ManagerCompanyMainKindName = x.ManagerCompanyMainKindName,
                ManagerCompanySubKindIsEnable = x.ManagerCompanySubKindIsEnable,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得客戶子分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetCompanySubKindRspMdl> GetCompanySubKind(MbsSysCmpCtlGetCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetCompanySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得客戶子分類
        var logicalReqObj = new MbsSysCmpLgcGetCompanySubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetCompanySubKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanySubKindName = logicalRspObj.ManagerCompanySubKindName;
        rspObj.ManagerCompanyMainKindID = logicalRspObj.ManagerCompanyMainKindID;
        rspObj.ManagerCompanySubKindIsEnable = logicalRspObj.ManagerCompanySubKindIsEnable;
        rspObj.ManagerCompanyMainKindName = logicalRspObj.ManagerCompanyMainKindName;
        return rspObj;
    }

    /// <summary>新增客戶子分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlAddCompanySubKindRspMdl> AddCompanySubKind(MbsSysCmpCtlAddCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlAddCompanySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-新增客戶子分類
        var logicalReqObj = new MbsSysCmpLgcAddCompanySubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName.Trim(),
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.AddCompanySubKindAsync(logicalReqObj);
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
        rspObj.ManagerCompanySubKindID = logicalRspObj.ManagerCompanySubKindID;
        return rspObj;
    }

    /// <summary>更新客戶子分類</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlUpdateCompanySubKindRspMdl> UpdateCompanySubKind(MbsSysCmpCtlUpdateCompanySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlUpdateCompanySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-更新客戶子分類
        var logicalReqObj = new MbsSysCmpLgcUpdateCompanySubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName?.Trim(),
            ManagerCompanySubKindIsEnable = reqObj.ManagerCompanySubKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.UpdateCompanySubKindAsync(logicalReqObj);
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

    #region 客戶

    /// <summary>取得多筆客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetManyCompanyRspMdl> GetManyCompany(MbsSysCmpCtlGetManyCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetManyCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得多筆客戶
        var logicalReqObj = new MbsSysCmpLgcGetManyCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetManyCompanyAsync(logicalReqObj);
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
        rspObj.ManagerCompanyList = logicalRspObj.ManagerCompanyList?
            .Select(x => new MbsSysCmpCtlGetManyCompanyRspItemMdl()
            {
                ManagerCompanyID = x.ManagerCompanyID,
                ManagerCompanyUnifiedNumber = x.ManagerCompanyUnifiedNumber,
                ManagerCompanyName = x.ManagerCompanyName,
                AtomCustomerGrade = x.AtomCustomerGrade,
                ManagerCompanyMainKindName = x.ManagerCompanyMainKindName,
                ManagerCompanySubKindName = x.ManagerCompanySubKindName,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetCompanyRspMdl> GetCompany(MbsSysCmpCtlGetCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得客戶
        var logicalReqObj = new MbsSysCmpLgcGetCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetCompanyAsync(logicalReqObj);
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

    /// <summary>新增客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlAddCompanyRspMdl> AddCompany(MbsSysCmpCtlAddCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlAddCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-新增客戶
        var logicalReqObj = new MbsSysCmpLgcAddCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyUnifiedNumber = reqObj.ManagerCompanyUnifiedNumber?.Trim(),
            ManagerCompanyName = reqObj.ManagerCompanyName.Trim(),
            AtomManagerCompanyStatus = reqObj.AtomManagerCompanyStatus,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            AtomSecurityGrade = reqObj.AtomSecurityGrade,
            AtomEmployeeRange = reqObj.AtomEmployeeRange,
            ManagerCompanyAffiliateList = reqObj.ManagerCompanyAffiliateList?.Select(x => new MbsSysCmpLgcAddCompanyReqAffiliateItemMdl
            {
                ManagerCompanyAffiliateUnifiedNumber = x.ManagerCompanyAffiliateUnifiedNumber,
                ManagerCompanyAffiliateName = x.ManagerCompanyAffiliateName
            }).ToList(),
            ManagerCompanyLocationList = reqObj.ManagerCompanyLocationList?.Select(x => new MbsSysCmpLgcAddCompanyReqLocationItemMdl
            {
                ManagerCompanyLocationName = x.ManagerCompanyLocationName,
                AtomCity = x.AtomCity,
                ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress,
                ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone,
                AtomManagerCompanyStatus = x.AtomManagerCompanyStatus
            }).ToList()
        };
        var logicalRspObj = await this._mbsSysCmpLogical.AddCompanyAsync(logicalReqObj);
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
        return rspObj;
    }

    /// <summary>更新客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlUpdateCompanyRspMdl> UpdateCompany(MbsSysCmpCtlUpdateCompanyReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlUpdateCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-更新客戶
        var logicalReqObj = new MbsSysCmpLgcUpdateCompanyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyUnifiedNumber = reqObj.ManagerCompanyUnifiedNumber?.Trim(),
            ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
            AtomManagerCompanyStatus = reqObj.AtomManagerCompanyStatus,
            ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
            ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
            AtomCustomerGrade = reqObj.AtomCustomerGrade,
            AtomSecurityGrade = reqObj.AtomSecurityGrade,
            AtomEmployeeRange = reqObj.AtomEmployeeRange
        };
        var logicalRspObj = await this._mbsSysCmpLogical.UpdateCompanyAsync(logicalReqObj);
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

    #region 客戶營業地點

    /// <summary>取得多筆客戶營業地點</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetManyCompanyLocationRspMdl> GetManyCompanyLocation(MbsSysCmpCtlGetManyCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetManyCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得多筆客戶營業地點
        var logicalReqObj = new MbsSysCmpLgcGetManyCompanyLocationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetManyCompanyLocationAsync(logicalReqObj);
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
        rspObj.ManagerCompanyLocationList = logicalRspObj.ManagerCompanyLocationList?
            .Select(x => new MbsSysCmpCtlGetManyCompanyLocationRspItemMdl()
            {
                ManagerCompanyLocationID = x.ManagerCompanyLocationID,
                ManagerCompanyLocationName = x.ManagerCompanyLocationName,
                AtomCity = x.AtomCity,
                ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress,
                ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone,
                AtomManagerCompanyStatus = x.AtomManagerCompanyStatus
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得客戶營業地點</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetCompanyLocationRspMdl> GetCompanyLocation(MbsSysCmpCtlGetCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得客戶營業地點
        var logicalReqObj = new MbsSysCmpLgcGetCompanyLocationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetCompanyLocationAsync(logicalReqObj);
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
        rspObj.ManagerCompanyLocationName = logicalRspObj.ManagerCompanyLocationName;
        rspObj.AtomCity = logicalRspObj.AtomCity;
        rspObj.ManagerCompanyLocationAddress = logicalRspObj.ManagerCompanyLocationAddress;
        rspObj.ManagerCompanyLocationTelephone = logicalRspObj.ManagerCompanyLocationTelephone;
        rspObj.AtomManagerCompanyStatus = logicalRspObj.AtomManagerCompanyStatus;
        return rspObj;
    }

    /// <summary>新增客戶營業地點</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlAddCompanyLocationRspMdl> AddCompanyLocation(MbsSysCmpCtlAddCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlAddCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-新增客戶營業地點
        var logicalReqObj = new MbsSysCmpLgcAddCompanyLocationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationName = reqObj.ManagerCompanyLocationName?.Trim(),
            AtomCity = reqObj.AtomCity,
            ManagerCompanyLocationAddress = reqObj.ManagerCompanyLocationAddress?.Trim(),
            ManagerCompanyLocationTelephone = reqObj.ManagerCompanyLocationTelephone?.Trim(),
            AtomManagerCompanyStatus = reqObj.AtomManagerCompanyStatus
        };
        var logicalRspObj = await this._mbsSysCmpLogical.AddCompanyLocationAsync(logicalReqObj);
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
        return rspObj;
    }

    /// <summary>更新客戶營業地點</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlUpdateCompanyLocationRspMdl> UpdateCompanyLocation(MbsSysCmpCtlUpdateCompanyLocationReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlUpdateCompanyLocationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-更新客戶營業地點
        var logicalReqObj = new MbsSysCmpLgcUpdateCompanyLocationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            ManagerCompanyLocationName = reqObj.ManagerCompanyLocationName?.Trim(),
            AtomCity = reqObj.AtomCity,
            ManagerCompanyLocationAddress = reqObj.ManagerCompanyLocationAddress?.Trim(),
            ManagerCompanyLocationTelephone = reqObj.ManagerCompanyLocationTelephone?.Trim(),
            AtomManagerCompanyStatus = reqObj.AtomManagerCompanyStatus,
            ManagerCompanyLocationIsDeleted = reqObj.ManagerCompanyLocationIsDeleted
        };
        var logicalRspObj = await this._mbsSysCmpLogical.UpdateCompanyLocationAsync(logicalReqObj);
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

    #region 關係客戶

    /// <summary>取得多筆關係客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetManyCompanyAffiliateRspMdl> GetManyCompanyAffiliate(MbsSysCmpCtlGetManyCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetManyCompanyAffiliateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得多筆關係客戶
        var logicalReqObj = new MbsSysCmpLgcGetManyCompanyAffiliateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetManyCompanyAffiliateAsync(logicalReqObj);
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
        rspObj.ManagerCompanyAffiliateList = logicalRspObj.ManagerCompanyAffiliateList?
            .Select(x => new MbsSysCmpCtlGetManyCompanyAffiliateRspItemMdl()
            {
                ManagerCompanyAffiliateID = x.ManagerCompanyAffiliateID,
                ManagerCompanyAffiliateUnifiedNumber = x.ManagerCompanyAffiliateUnifiedNumber,
                ManagerCompanyAffiliateName = x.ManagerCompanyAffiliateName
            })
            .ToList();
        return rspObj;
    }

    /// <summary>取得關係客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlGetCompanyAffiliateRspMdl> GetCompanyAffiliate(MbsSysCmpCtlGetCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlGetCompanyAffiliateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-取得關係客戶
        var logicalReqObj = new MbsSysCmpLgcGetCompanyAffiliateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyAffiliateID = reqObj.ManagerAffiliateCompanyID,
        };
        var logicalRspObj = await this._mbsSysCmpLogical.GetCompanyAffiliateAsync(logicalReqObj);
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
        rspObj.ManagerCompanyAffiliateID = logicalRspObj.ManagerCompanyAffiliateID;
        rspObj.ManagerCompanyAffiliateUnifiedNumber = logicalRspObj.ManagerCompanyAffiliateUnifiedNumber;
        rspObj.ManagerCompanyAffiliateName = logicalRspObj.ManagerCompanyAffiliateName;
        return rspObj;
    }

    /// <summary>新增關係客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlAddCompanyAffiliateRspMdl> AddCompanyAffiliate(MbsSysCmpCtlAddCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlAddCompanyAffiliateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-新增關係客戶
        var logicalReqObj = new MbsSysCmpLgcAddCompanyAffiliateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyAffiliateUnifiedNumber = reqObj.ManagerAffiliateCompanyUnifiedNumber?.Trim(),
            ManagerCompanyAffiliateName = reqObj.ManagerAffiliateCompanyName?.Trim()
        };
        var logicalRspObj = await this._mbsSysCmpLogical.AddCompanyAffiliateAsync(logicalReqObj);
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
        rspObj.ManagerCompanyAffiliateID = logicalRspObj.ManagerCompanyAffiliateID;
        return rspObj;
    }

    /// <summary>更新關係客戶</summary>
    [HttpPost]
    public async Task<MbsSysCmpCtlUpdateCompanyAffiliateRspMdl> UpdateCompanyAffiliate(MbsSysCmpCtlUpdateCompanyAffiliateReqMdl reqObj)
    {
        var rspObj = new MbsSysCmpCtlUpdateCompanyAffiliateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統設定-客戶-邏輯-更新關係客戶
        var logicalReqObj = new MbsSysCmpLgcUpdateCompanyAffiliateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyAffiliateID = reqObj.ManagerCompanyAffiliateID,
            ManagerCompanyAffiliateUnifiedNumber = reqObj.ManagerCompanyAffiliateUnifiedNumber?.Trim(),
            ManagerCompanyAffiliateName = reqObj.ManagerCompanyAffiliateName?.Trim(),
            ManagerCompanyAffiliateIsDeleted = reqObj.ManagerCompanyAffiliateIsDeleted
        };
        var logicalRspObj = await this._mbsSysCmpLogical.UpdateCompanyAffiliateAsync(logicalReqObj);
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