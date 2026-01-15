using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;

namespace ServiceLibrary.ManagerBackSite.Logical.Schedule.Service;

/// <summary>管理者後台-排程-邏輯服務</summary>
public class MbsScheduleLogicalService : IMbsScheduleLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsScheduleLogicalService> _logger;

    #region Core Manager        

    /// <summary>核心-管理者-登入資訊-記憶體服務</summary>
    private readonly ICoEmpLoginInfoMemoryService _coEmpLoginInfoMemory;

    #endregion

    #region ManagerBackSite

    /// <summary>管理者後台-通用-邏輯服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #endregion

    /// <summary>建構式</summary>
    public MbsScheduleLogicalService(
        ILogger<MbsScheduleLogicalService> logger,
        // Core Manager
        ICoEmpLoginInfoMemoryService coEmpLoginInfoMemory,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // Core Manager
        this._coEmpLoginInfoMemory = coEmpLoginInfoMemory;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;
    }

    /// <summary>管理者後台-排程-登出到期時間</summary>
    public async Task LogoutExpiredTimeAsync()
    {
        // memory, 核心-員工-登入資訊-記憶體-取得全部
        var coEmpLiMemGetAllRspObj = this._coEmpLoginInfoMemory.GetAll();
        if (coEmpLiMemGetAllRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 取得過期員工ID列表
        var expiredEmployeeIdList = coEmpLiMemGetAllRspObj.EmployeeLoginInfoList
            .Where(x => x.ExpiredTime < DateTime.UtcNow)
            .Select(x => x.EmployeeID)
            .Distinct()
            .ToList();

        // foreach, mbs, 管理者後台-通用-邏輯-登出
        foreach (var expiredEmployeeIdItem in expiredEmployeeIdList)
        {
            // mbs, 管理者後台-通用-邏輯-登出
            var mbsCmnLgcLogoutReqObj = new MbsCmnLgcLogoutReqMdl()
            {
                EmployeeID = expiredEmployeeIdItem,
            };
            var mbsCmnLgcLogoutRspObj = await this._mbsCommonLogical.LogoutAsync(mbsCmnLgcLogoutReqObj);
            if (mbsCmnLgcLogoutRspObj == default)
            {
                this._logger.LogError($"item: {JsonSerializer.Serialize(expiredEmployeeIdItem)}");
                continue;
            }
            if (mbsCmnLgcLogoutRspObj.ErrorCode != MbsErrorCodeEnum.Success)
            {
                this._logger.LogError($"item: {JsonSerializer.Serialize(expiredEmployeeIdItem)}");
                continue;
            }
        }

        await Task.CompletedTask;
    }
}
