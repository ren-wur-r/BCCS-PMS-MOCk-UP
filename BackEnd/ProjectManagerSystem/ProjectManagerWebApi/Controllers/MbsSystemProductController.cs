using System.Linq;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.System.Product;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;
using ServiceLibrary.ManagerBackSite.Logical.System.Product.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理者後台-系統-產品</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsSystemProductController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsSystemProductController> _logger;

    /// <summary>管理者後台-系統-產品-邏輯服務</summary>
    private readonly IMbsSysProductLogicalService _mbsSysProductLogical;


    /// <summary>建構式</summary>
    public MbsSystemProductController(
        ILogger<MbsSystemProductController> logger,
        IMbsSysProductLogicalService mbsSysProductLogical)
    {
        this._logger = logger;
        this._mbsSysProductLogical = mbsSysProductLogical;
    }

    #region 產品

    /// <summary>取得多筆產品</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlGetManyRspMdl> GetMany(MbsSysPrdCtlGetManyReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlGetManyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-取得多筆
        var logicalReqObj = new MbsSysPrdLgcGetManyReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductName = reqObj.ManagerProductName,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsSysProductLogical.GetManyAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductList = logicalRspObj.ManagerProductSpecificationList
            .Select(x => new MbsSysPrdCtlGetManyRspItemMdl
            {
                ManagerProductID = x.ManagerProductID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductIsKey = x.ManagerProductIsKey,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得單筆產品</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlGetRspMdl> Get(MbsSysPrdCtlGetReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlGetRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-取得單筆
        var logicalReqObj = new MbsSysPrdLgcGetReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductID = reqObj.ManagerProductID,
        };
        var logicalRspObj = await this._mbsSysProductLogical.GetAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductName = logicalRspObj.ManagerProductName;
        rspObj.ManagerProductMainKindID = logicalRspObj.ManagerProductMainKindID;
        rspObj.ManagerProductMainKindName = logicalRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductSubKindID = logicalRspObj.ManagerProductSubKindID;
        rspObj.ManagerProductSubKindName = logicalRspObj.ManagerProductSubKindName;
        rspObj.ManagerProductKind = logicalRspObj.ManagerProductKind;
        rspObj.ManagerProductIsKey = logicalRspObj.ManagerProductIsKey;
        rspObj.ManagerProductRemark = logicalRspObj.ManagerProductRemark;
        rspObj.ManagerProductIsEnable = logicalRspObj.ManagerProductIsEnable;
        rspObj.ManagerProductSpecificationList = logicalRspObj.ManagerProductSpecificationList
            .Select(x => new MbsSysPrdCtlGetSpecificationRspItemMdl
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

    /// <summary>新增產品</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlAddRspMdl> Add(MbsSysPrdCtlAddReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlAddRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-新增
        var logicalReqObj = new MbsSysPrdLgcAddReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductName = reqObj.ManagerProductName.Trim(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductKind = reqObj.ManagerProductKind,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductRemark = reqObj.ManagerProductRemark,
            ManagerProductIsEnable = reqObj.ManagerProductIsEnable,
            ManagerProductSpecificationList = reqObj.ManagerProductSpecificationList
                ?.Select(x => new MbsSysPrdLgcAddSpecificationItemReqMdl
                {
                    ManagerProductSpecificationName = x.ManagerProductSpecificationName.Trim(),
                    ManagerProductSpecificationSellPrice = x.ManagerProductSpecificationSellPrice,
                    ManagerProductSpecificationCostPrice = x.ManagerProductSpecificationCostPrice,
                    ManagerProductSpecificationIsEnable = x.ManagerProductSpecificationIsEnable,
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsSysProductLogical.AddAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductID = logicalRspObj.ManagerProductID;
        return rspObj;
    }

    /// <summary>更新產品</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlUpdateRspMdl> Update(MbsSysPrdCtlUpdateReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlUpdateRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-更新
        var logicalReqObj = new MbsSysPrdLgcUpdateReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductName = reqObj.ManagerProductName?.Trim(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductKind = reqObj.ManagerProductKind,
            ManagerProductIsKey = reqObj.ManagerProductIsKey,
            ManagerProductRemark = reqObj.ManagerProductRemark,
            ManagerProductIsEnable = reqObj.ManagerProductIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.UpdateAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>新增產品規格</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlAddSpecificationRspMdl> AddSpecification(MbsSysPrdCtlAddSpecificationReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlAddSpecificationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-新增規格
        var logicalReqObj = new MbsSysPrdLgcAddSpecificationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName.Trim(),
            ManagerProductSpecificationSellPrice = reqObj.ManagerProductSpecificationSellPrice,
            ManagerProductSpecificationCostPrice = reqObj.ManagerProductSpecificationCostPrice,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.AddSpecificationAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>更新產品規格</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlUpdateSpecificationRspMdl> UpdateSpecification(MbsSysPrdCtlUpdateSpecificationReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlUpdateSpecificationRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-更新規格
        var logicalReqObj = new MbsSysPrdLgcUpdateSpecificationReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            ManagerProductSpecificationName = reqObj.ManagerProductSpecificationName?.Trim(),
            ManagerProductSpecificationSellPrice = reqObj.ManagerProductSpecificationSellPrice,
            ManagerProductSpecificationCostPrice = reqObj.ManagerProductSpecificationCostPrice,
            ManagerProductSpecificationIsEnable = reqObj.ManagerProductSpecificationIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.UpdateSpecificationAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 產品主分類管理

    /// <summary>取得多筆產品主分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlGetManyMainKindRspMdl> GetManyMainKind(MbsSysPrdCtlGetManyMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlGetManyMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-取得多筆主分類
        var logicalReqObj = new MbsSysPrdLgcGetManyMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsSysProductLogical.GetManyMainKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindList = logicalRspObj.ManagerProductMainKindList
            .Select(x => new MbsSysPrdCtlGetManyMainKindRspItemMdl
            {
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductMainKindCommissionRate = x.ManagerProductMainKindCommissionRate,
                ManagerProductMainKindIsEnable = x.ManagerProductMainKindIsEnable,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得單筆產品主分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlGetMainKindRspMdl> GetMainKind(MbsSysPrdCtlGetMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlGetMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-取得單筆主分類
        var logicalReqObj = new MbsSysPrdLgcGetMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
        };
        var logicalRspObj = await this._mbsSysProductLogical.GetMainKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindName = logicalRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductMainKindCommissionRate = logicalRspObj.ManagerProductMainKindCommissionRate;
        rspObj.ManagerProductMainKindIsEnable = logicalRspObj.ManagerProductMainKindIsEnable;
        return rspObj;
    }

    /// <summary>新增產品主分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlAddMainKindRspMdl> AddMainKind(MbsSysPrdCtlAddMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlAddMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-新增主分類
        var logicalReqObj = new MbsSysPrdLgcAddMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName.Trim(),
            ManagerProductMainKindCommissionRate = reqObj.ManagerProductMainKindCommissionRate,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.AddMainKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>更新產品主分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlUpdateMainKindRspMdl> UpdateMainKind(MbsSysPrdCtlUpdateMainKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlUpdateMainKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-更新主分類
        var logicalReqObj = new MbsSysPrdLgcUpdateMainKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductMainKindName = reqObj.ManagerProductMainKindName?.Trim(),
            ManagerProductMainKindCommissionRate = reqObj.ManagerProductMainKindCommissionRate,
            ManagerProductMainKindIsEnable = reqObj.ManagerProductMainKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.UpdateMainKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 產品子分類管理

    /// <summary>取得多筆產品子分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlGetManySubKindRspMdl> GetManySubKind(MbsSysPrdCtlGetManySubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlGetManySubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-取得多筆子分類
        var logicalReqObj = new MbsSysPrdLgcGetManySubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsSysProductLogical.GetManySubKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductSubKindList = logicalRspObj.ManagerProductSubKindList
            .Select(x => new MbsSysPrdCtlGetManySubKindRspItemMdl
            {
                ManagerProductMainKindID = x.ManagerProductMainKindID,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindID = x.ManagerProductSubKindID,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductSubKindCommissionRate = x.ManagerProductSubKindCommissionRate,
                ManagerProductSubKindIsEnable = x.ManagerProductSubKindIsEnable,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>取得單筆產品子分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlGetSubKindRspMdl> GetSubKind(MbsSysPrdCtlGetSubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlGetSubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-取得單筆子分類
        var logicalReqObj = new MbsSysPrdLgcGetSubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
        };
        var logicalRspObj = await this._mbsSysProductLogical.GetSubKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerProductMainKindName = logicalRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductSubKindName = logicalRspObj.ManagerProductSubKindName;
        rspObj.ManagerProductSubKindCommissionRate = logicalRspObj.ManagerProductSubKindCommissionRate;
        rspObj.ManagerProductSubKindIsEnable = logicalRspObj.ManagerProductSubKindIsEnable;
        return rspObj;
    }

    /// <summary>新增產品子分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlAddSubKindRspMdl> AddSubKind(MbsSysPrdCtlAddSubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlAddSubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-新增子分類
        var logicalReqObj = new MbsSysPrdLgcAddSubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductMainKindID = reqObj.ManagerProductMainKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName.Trim(),
            ManagerProductSubKindCommissionRate = reqObj.ManagerProductSubKindCommissionRate,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.AddSubKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>更新產品子分類</summary>
    [HttpPost]
    public async Task<MbsSysPrdCtlUpdateSubKindRspMdl> UpdateSubKind(MbsSysPrdCtlUpdateSubKindReqMdl reqObj)
    {
        var rspObj = new MbsSysPrdCtlUpdateSubKindRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-系統-產品-更新子分類
        var logicalReqObj = new MbsSysPrdLgcUpdateSubKindReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerProductSubKindID = reqObj.ManagerProductSubKindID,
            ManagerProductSubKindName = reqObj.ManagerProductSubKindName?.Trim(),
            ManagerProductSubKindCommissionRate = reqObj.ManagerProductSubKindCommissionRate,
            ManagerProductSubKindIsEnable = reqObj.ManagerProductSubKindIsEnable,
        };
        var logicalRspObj = await this._mbsSysProductLogical.UpdateSubKindAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {System.Text.Json.JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion
}