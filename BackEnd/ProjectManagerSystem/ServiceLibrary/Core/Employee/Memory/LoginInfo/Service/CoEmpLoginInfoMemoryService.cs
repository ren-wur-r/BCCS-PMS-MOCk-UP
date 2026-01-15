using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;

/// <summary>核心-員工-登入資訊-記憶體服務</summary>
public class CoEmpLoginInfoMemoryService : ICoEmpLoginInfoMemoryService
{
    /// <summary>Logger</summary>
    private readonly ILogger<CoEmpLoginInfoMemoryService> _logger;

    /// <summary>權杖</summary>
    private readonly object _locker;

    /// <summary>物件列表</summary>
    private readonly List<CoEmpLiMemInnerMdl> _itemList;

    /// <summary>建構式</summary>
    public CoEmpLoginInfoMemoryService(
        ILogger<CoEmpLoginInfoMemoryService> logger)
    {
        this._logger = logger;
        this._locker = new object();
        this._itemList = new List<CoEmpLiMemInnerMdl>();
    }

    /// <summary>核心-員工-登入資訊-記憶體-是否存在</summary>
    public CoEmpLiMemExistRspMdl Exist(CoEmpLiMemExistReqMdl reqObj)
    {
        lock (this._locker)
        {
            var isExist = this._itemList
                .Any(x => x.EmployeeLoginToken == reqObj.EmployeeLoginToken);

            var rspObj = new CoEmpLiMemExistRspMdl()
            {
                IsExist = isExist,
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-新增</summary>
    public CoEmpLiMemAddRspMdl Add(CoEmpLiMemAddReqMdl reqObj)
    {
        var item = new CoEmpLiMemInnerMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            EmployeeID = reqObj.EmployeeID,
            ExpiredTime = DateTimeOffset.UtcNow.AddMinutes(CoEmpLiMemConst.LIVE_TIME_MINUTES),
        };

        // lock
        lock (this._locker)
        {
            // check
            var isExist = this._itemList
               .Any(x => x.EmployeeLoginToken == reqObj.EmployeeLoginToken);
            if (isExist)
            {
                var existRspObj = new CoEmpLiMemAddRspMdl()
                {
                    IsSuccess = true,
                };
                return existRspObj;
            }

            // add
            this._itemList.Add(item);

            var rspObj = new CoEmpLiMemAddRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-更新到期時間</summary>
    public CoEmpLiMemUpdateExpiredTimeRspMdl UpdateExpiredTime(CoEmpLiMemUpdateExpiredTimeReqMdl reqObj)
    {
        // lock
        lock (this._locker)
        {
            // check and get
            var item = this._itemList
               .SingleOrDefault(x => x.EmployeeLoginToken == reqObj.EmployeeLoginToken);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            // update
            item.ExpiredTime = DateTimeOffset.UtcNow.AddMinutes(CoEmpLiMemConst.LIVE_TIME_MINUTES);

            var rspObj = new CoEmpLiMemUpdateExpiredTimeRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-移除</summary>
    public CoEmpLiMemRemoveRspMdl Remove(CoEmpLiMemRemoveReqMdl reqObj)
    {
        // lock
        lock (this._locker)
        {
            // remove
            _ = this._itemList
                .RemoveAll(x => x.EmployeeLoginToken == reqObj.EmployeeLoginToken);

            var rspObj = new CoEmpLiMemRemoveRspMdl()
            {
                IsSuccess = true,
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-移除多筆</summary>
    public CoEmpLiMemRemoveManyRspMdl RemoveMany(CoEmpLiMemRemoveManyReqMdl reqObj)
    {
        // lock
        lock (this._locker)
        {
            // remove
            var affectedCount = this._itemList
                .RemoveAll(x =>
                    (reqObj.EmployeeLoginTokenList != null && reqObj.EmployeeLoginTokenList.Contains(x.EmployeeLoginToken))
                    || (reqObj.EmployeeIdList != null && reqObj.EmployeeIdList.Contains(x.EmployeeID)));

            var rspObj = new CoEmpLiMemRemoveManyRspMdl()
            {
                AffectedCount = affectedCount,
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-取得</summary>
    public CoEmpLiMemGetRspMdl Get(CoEmpLiMemGetReqMdl reqObj)
    {
        // lock
        lock (this._locker)
        {
            var item = this._itemList
                .SingleOrDefault(x => x.EmployeeLoginToken == reqObj.EmployeeLoginToken);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpLiMemGetRspMdl()
            {
                EmployeeID = item.EmployeeID,
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-取得多筆[登入令牌]</summary>
    public CoEmpLiMemGetManyLoginTokenRspMdl GetManyLoginToken(CoEmpLiMemGetManyLoginTokenReqMdl reqObj)
    {
        // lock
        lock (this._locker)
        {
            var rspObj = new CoEmpLiMemGetManyLoginTokenRspMdl()
            {
                EmployeeLoginInfoList = this._itemList
                    .Where(x => x.EmployeeID == reqObj.EmployeeID)
                    .Select(x => new CoEmpLiMemGetManyLoginTokenRspItemMdl()
                    {
                        EmployeeLoginToken = x.EmployeeLoginToken,
                    })
                    .ToList()
            };
            return rspObj;
        }
    }

    /// <summary>核心-員工-登入資訊-記憶體-取得全部</summary>
    public CoEmpLiMemGetAllRspMdl GetAll()
    {
        // lock
        lock (this._locker)
        {
            var rspObj = new CoEmpLiMemGetAllRspMdl()
            {
                EmployeeLoginInfoList = this._itemList
                    .Select(x => new CoEmpLiMemGetAllRspItemMdl()
                    {
                        EmployeeLoginToken = x.EmployeeLoginToken,
                        EmployeeID = x.EmployeeID,
                        ExpiredTime = x.ExpiredTime,
                    })
                    .ToList(),
            };
            return rspObj;
        }
    }

}
