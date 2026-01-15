using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Service;

/// <summary>核心-管理者-關係公司-資料庫服務</summary>
public class CoMgrCompanyAffiliateDbService : ICoMgrCompanyAffiliateDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrCompanyAffiliateDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrCompanyAffiliateDbService(
        ILogger<CoMgrCompanyAffiliateDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-關係公司-資料庫-取得多筆</summary>
    public async Task<CoMgrCmpAffDbGetManyRspMdl> GetManyAsync(CoMgrCmpAffDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompanyAffiliate
                .AsNoTracking()
                .Where(x => x.ManagerCompanyID == reqObj.ManagerCompanyID &&
                            (reqObj.ManagerCompanyAffiliateIsDeleted.HasValue == false || x.IsDeleted == reqObj.ManagerCompanyAffiliateIsDeleted.Value))
                .Select(x => new
                {
                    x.ID,
                    x.UnifiedNumber,
                    x.Name,
                    x.IsDeleted,
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoMgrCmpAffDbGetManyRspMdl
            {
                ManagerCompanyAffiliateList = itemList?
                    .Select(x => new CoMgrCmpAffDbGetManyRspItemMdl
                    {
                        ManagerCompanyAffiliateID = x.ID,
                        ManagerCompanyAffiliateUnifiedNumber = x.UnifiedNumber?.Trim(),
                        ManagerCompanyAffiliateName = x.Name?.Trim(),
                        ManagerCompanyAffiliateIsDeleted = x.IsDeleted,
                    }).ToList()
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-管理者-關係公司-資料庫-取得</summary>
    public async Task<CoMgrCmpAffDbGetRspMdl> GetAsync(CoMgrCmpAffDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompanyAffiliate
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerCompanyAffiliateID &&
                                        (reqObj.ManagerCompanyAffiliateIsDeleted.HasValue == false || x.IsDeleted == reqObj.ManagerCompanyAffiliateIsDeleted.Value));
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpAffDbGetRspMdl
            {
                ManagerCompanyAffiliateID = item.ID,
                ManagerCompanyID = item.ManagerCompanyID,
                ManagerCompanyAffiliateUnifiedNumber = item.UnifiedNumber?.Trim(),
                ManagerCompanyAffiliateName = item.Name?.Trim(),
                ManagerCompanyAffiliateIsDeleted = item.IsDeleted,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
    }

    /// <summary>核心-管理者-關係公司-資料庫-新增</summary>
    public async Task<CoMgrCmpAffDbAddRspMdl> AddAsync(CoMgrCmpAffDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerCompanyAffiliate
            {
                ManagerCompanyID = reqObj.ManagerCompanyID,
                UnifiedNumber = reqObj.ManagerCompanyAffiliateUnifiedNumber?.Trim(),
                Name = reqObj.ManagerCompanyAffiliateName?.Trim(),
                IsDeleted = false,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerCompanyAffiliate.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpAffDbAddRspMdl
            {
                ManagerCompanyAffiliateID = item.ID
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-管理者-關係公司-資料庫-新增多筆</summary>
    public async Task<CoMgrCmpAffDbAddManyRspMdl> AddManyAsync(CoMgrCmpAffDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.ManagerCompanyAffiliateList
                .Select(x => new ManagerCompanyAffiliate
                {
                    ManagerCompanyID = x.ManagerCompanyID,
                    UnifiedNumber = x.ManagerCompanyAffiliateUnifiedNumber?.Trim(),
                    Name = x.ManagerCompanyAffiliateName?.Trim(),
                    IsDeleted = false,
                    CreateTime = currentTime,
                    UpdateTime = currentTime
                }).ToList();

            this._mainContext.ManagerCompanyAffiliate.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpAffDbAddManyRspMdl
            {
                AddedCount = itemList.Count
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-管理者-關係公司-資料庫-更新</summary>
    public async Task<CoMgrCmpAffDbUpdateRspMdl> UpdateAsync(CoMgrCmpAffDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerCompanyAffiliate
                .Where(x => x.ID == reqObj.ManagerCompanyAffiliateID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.UnifiedNumber, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyAffiliateUnifiedNumber) ? reqObj.ManagerCompanyAffiliateUnifiedNumber.Trim() : x.UnifiedNumber)
                    .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyAffiliateName) ? reqObj.ManagerCompanyAffiliateName.Trim() : x.Name)
                    .SetProperty(x => x.IsDeleted, x => reqObj.ManagerCompanyAffiliateIsDeleted.HasValue ? reqObj.ManagerCompanyAffiliateIsDeleted.Value : x.IsDeleted)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrCmpAffDbUpdateRspMdl
            {
                AffectedCount = affectedCount,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-管理者-關係公司-資料庫-依管理者公司ID移除</summary>
    public async Task<CoMgrCmpAffDbRemoveByManagerCompanyIDRspMdl> RemoveByManagerCompanyIDAsync(CoMgrCmpAffDbRemoveByManagerCompanyIDReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerCompanyAffiliate
                .Where(x => x.ManagerCompanyID == reqObj.ManagerCompanyID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrCmpAffDbRemoveByManagerCompanyIDRspMdl
            {
                AffectedCount = affectedCount
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }
}