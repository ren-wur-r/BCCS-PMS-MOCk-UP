using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Service;

/// <summary>核心-管理者-公司營業地點-資料庫服務</summary>
public class CoMgrCompanyLocationDbService : ICoMgrCompanyLocationDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrCompanyLocationDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrCompanyLocationDbService(
        ILogger<CoMgrCompanyLocationDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-公司營業地點-資料庫-是否存在</summary>
    public async Task<CoMgrCmpLocDbExistRspMdl> ExistAsync(CoMgrCmpLocDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerCompanyLocation
                .AsNoTracking()
                .AnyAsync(x => x.ID == reqObj.ManagerCompanyLocationID);

            var rspObj = new CoMgrCmpLocDbExistRspMdl
            {
                IsExist = isExist,
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

    /// <summary>核心-管理者-公司營業地點-資料庫-取得多筆</summary>
    public async Task<CoMgrCmpLocDbGetManyRspMdl> GetManyAsync(CoMgrCmpLocDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompanyLocation
                .AsNoTracking()
                .Where(x => x.ManagerCompanyID == reqObj.ManagerCompanyID &&
                            (reqObj.ManagerCompanyLocationIsDeleted.HasValue == false || x.IsDeleted == reqObj.ManagerCompanyLocationIsDeleted.Value))
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.AtomCityID,
                    x.Address,
                    x.Telephone,
                    x.Status,
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoMgrCmpLocDbGetManyRspMdl
            {
                ManagerCompanyLocationList = itemList?
                    .Select(x => new CoMgrCmpLocDbGetManyRspItemMdl
                    {
                        ManagerCompanyLocationID = x.ID,
                        ManagerCompanyLocationName = x.Name?.Trim(),
                        AtomCity = x.AtomCityID.ToEnum<DbAtomCityEnum>(),
                        ManagerCompanyLocationAddress = x.Address?.Trim(),
                        ManagerCompanyLocationTelephone = x.Telephone?.Trim(),
                        ManagerCompanyLocationStatus = x.Status.ToEnum<DbAtomManagerCompanyStatusEnum>()
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

    /// <summary>核心-管理者-公司營業地點-資料庫-取得</summary>
    public async Task<CoMgrCmpLocDbGetRspMdl> GetAsync(CoMgrCmpLocDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompanyLocation
                .AsNoTracking()
                .SingleOrDefaultAsync(x => 
                    x.ID == reqObj.ManagerCompanyLocationID 
                    && (reqObj.ManagerCompanyLocationIsDeleted.HasValue == false || x.IsDeleted == reqObj.ManagerCompanyLocationIsDeleted.Value));
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpLocDbGetRspMdl
            {
                ManagerCompanyLocationID = item.ID,
                ManagerCompanyID = item.ManagerCompanyID,
                ManagerCompanyLocationName = item.Name?.Trim(),
                AtomCity = item.AtomCityID.ToEnum<DbAtomCityEnum>(),
                ManagerCompanyLocationAddress = item.Address?.Trim(),
                ManagerCompanyLocationTelephone = item.Telephone?.Trim(),
                ManagerCompanyLocationStatus = item.Status.ToEnum<DbAtomManagerCompanyStatusEnum>(),
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

    /// <summary>核心-管理者-公司營業地點-資料庫-新增</summary>
    public async Task<CoMgrCmpLocDbAddRspMdl> AddAsync(CoMgrCmpLocDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerCompanyLocation
            {
                ManagerCompanyID = reqObj.ManagerCompanyID,
                Name = reqObj.ManagerCompanyLocationName?.Trim(),
                AtomCityID = reqObj.AtomCity.ToInt16(),
                Address = reqObj.ManagerCompanyLocationAddress?.Trim(),
                Telephone = reqObj.ManagerCompanyLocationTelephone?.Trim(),
                Status = reqObj.ManagerCompanyLocationStatus.HasValue ? reqObj.ManagerCompanyLocationStatus.Value.ToInt16() : DbAtomManagerCompanyStatusEnum.NotSelected.ToInt16(),
                IsDeleted = false,
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerCompanyLocation.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpLocDbAddRspMdl
            {
                ManagerCompanyLocationID = item.ID
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

    /// <summary>核心-管理者-公司營業地點-資料庫-新增多筆</summary>
    public async Task<CoMgrCmpLocDbAddManyRspMdl> AddManyAsync(CoMgrCmpLocDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.ManagerCompanyLocationList
                .Select(x => new ManagerCompanyLocation
                {
                    ManagerCompanyID = x.ManagerCompanyID,
                    Name = x.ManagerCompanyLocationName?.Trim(),
                    AtomCityID = x.AtomCity.ToInt16(),
                    Address = x.ManagerCompanyLocationAddress?.Trim(),
                    Telephone = x.ManagerCompanyLocationTelephone?.Trim(),
                    Status = x.ManagerCompanyLocationStatus.HasValue ? x.ManagerCompanyLocationStatus.Value.ToInt16() : DbAtomManagerCompanyStatusEnum.NotSelected.ToInt16(),
                    IsDeleted = false,
                    CreateTime = currentTime,
                    UpdateTime = currentTime
                }).ToList();

            this._mainContext.ManagerCompanyLocation.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpLocDbAddManyRspMdl
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

    /// <summary>核心-管理者-公司營業地點-資料庫-更新</summary>
    public async Task<CoMgrCmpLocDbUpdateRspMdl> UpdateAsync(CoMgrCmpLocDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerCompanyLocation
                .Where(x => x.ID == reqObj.ManagerCompanyLocationID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyLocationName) ? reqObj.ManagerCompanyLocationName.Trim() : x.Name)
                        .SetProperty(x => x.AtomCityID, x => reqObj.AtomCity.HasValue ? reqObj.AtomCity.Value.ToInt16() : x.AtomCityID)
                        .SetProperty(x => x.Address, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyLocationAddress) ? reqObj.ManagerCompanyLocationAddress.Trim() : x.Address)
                        .SetProperty(x => x.Telephone, x => reqObj.ManagerCompanyLocationTelephone != null ? reqObj.ManagerCompanyLocationTelephone.Trim() : x.Telephone)
                        .SetProperty(x => x.Status, x => reqObj.ManagerCompanyLocationStatus.HasValue ? reqObj.ManagerCompanyLocationStatus.Value.ToInt16() : x.Status)
                        .SetProperty(x => x.IsDeleted, x => reqObj.ManagerCompanyLocationIsDeleted.HasValue ? reqObj.ManagerCompanyLocationIsDeleted.Value : x.IsDeleted)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrCmpLocDbUpdateRspMdl
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

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司營業地點-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrCmpLocDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpLocDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompanyLocation
                            .AsNoTracking()
                            .Where(x =>
                                // 公司ID
                                x.ManagerCompanyID == reqObj.ManagerCompanyID &&
                                // 公司營業地點名稱
                                (string.IsNullOrEmpty(reqObj.ManagerCompanyLocationName) || x.Name.Contains(reqObj.ManagerCompanyLocationName.Trim()))
                            )
                            .Select(x => new
                            {
                                x.ID,
                                x.Name,
                                x.IsDeleted
                            })
                            .OrderBy(x => x.ID)
                            .Skip(skipRows)
                            .Take(takeRows)
                            .ToListAsync();

            var rspObj = new CoMgrCmpLocDbGetManyFromMbsBscRspMdl
            {
                ManagerCompanyLocationList = itemList.Select(x => new CoMgrCmpLocDbGetManyFromMbsBscRspItemMdl
                {
                    ManagerCompanyLocationID = x.ID,
                    ManagerCompanyLocationName = x.Name?.Trim(),
                    ManagerCompanyLocationIsDeleted = x.IsDeleted
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

    #endregion
}