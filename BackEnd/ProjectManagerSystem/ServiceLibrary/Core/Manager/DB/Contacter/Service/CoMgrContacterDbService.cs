using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomManagerContacter;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Contacter.Format;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Service;

/// <summary>核心-管理者-窗口-資料庫服務</summary>
public class CoMgrContacterDbService : ICoMgrContacterDbService
{
    /// <summary>Logger</summary>
    private readonly ILogger<CoMgrContacterDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrContacterDbService(
        ProjectManagerMainContext mainContext,
        ILogger<CoMgrContacterDbService> logger)
    {
        this._mainContext = mainContext;
        this._logger = logger;
    }

    /// <summary>核心-管理者-窗口-資料庫服務-是否存在</summary>
    public async Task<CoMgrCttDbExistRspMdl> ExistAsync(CoMgrCttDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .AnyAsync(x => x.ID != reqObj.ManagerContacterID);

            var rspObj = new CoMgrCttDbExistRspMdl
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

    /// <summary>核心-管理者-窗口-資料庫-同公司Email是否存在</summary>
    public async Task<CoMgrCttDbExistByCompanyEmailRspMdl> ExistByCompanyEmailAsync(CoMgrCttDbExistByCompanyEmailReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .Where(x => x.ManagerCompanyID == reqObj.ManagerCompanyID)
                .Where(x => x.Email == reqObj.ManagerContacterEmail)
                .AnyAsync();

            var rspObj = new CoMgrCttDbExistByCompanyEmailRspMdl
            {
                IsExist = isExist
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

    /// <summary>核心-管理者-窗口-資料庫服務-取得</summary>
    public async Task<CoMgrCttDbGetRspMdl> GetAsync(CoMgrCttDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerContacterID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCttDbGetRspMdl
            {
                ManagerContacterID = item.ID,
                ManagerCompanyID = item.ManagerCompanyID,
                ManagerContacterName = item.Name?.Trim(),
                ManagerContacterEmail = item.Email?.Trim(),
                ManagerContacterPhone = item.Phone?.Trim(),
                ManagerContacterDepartment = item.Department?.Trim(),
                ManagerContacterJobTitle = item.JobTitle?.Trim(),
                ManagerContacterTelephone = item.Telephone?.Trim(),
                ManagerContacterStatus = item.Status.ToEnum<DbAtomManagerContacterStatusEnum>(),
                ManagerContacterIsConsent = item.IsConsent,
                AtomRatingKind = item.AtomRatingKindID.ToEnum<DbAtomManagerContacterRatingKindEnum>(),
                ManagerContacterCreateEmployeeID = item.CreateEmployeeID,
                ManagerContacterRemark = item.Remark?.Trim()
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

    /// <summary>核心-管理者-窗口-資料庫服務-取得多筆</summary>
    public async Task<CoMgrCttDbGetManyRspMdl> GetManyAsync(CoMgrCttDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .Where(x => reqObj.ManagerContacterIDList.Contains(x.ID))
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.ManagerCompanyID,
                    x.Name,
                    x.Email,
                    x.Phone,
                    x.Department,
                    x.JobTitle,
                    x.Telephone,
                    x.Status,
                    x.IsConsent,
                    x.AtomRatingKindID,
                    x.CreateEmployeeID,
                    x.Remark
                })
                .ToListAsync();

            var rspObj = new CoMgrCttDbGetManyRspMdl
            {
                ManagerContacterList = itemList.Select(x => new CoMgrCttDbGetManyRspItemMdl
                {
                    ManagerContacterID = x.ID,
                    ManagerCompanyID = x.ManagerCompanyID,
                    ManagerContacterName = x.Name?.Trim(),
                    ManagerContacterEmail = x.Email?.Trim(),
                    ManagerContacterPhone = x.Phone?.Trim(),
                    ManagerContacterDepartment = x.Department?.Trim(),
                    ManagerContacterJobTitle = x.JobTitle?.Trim(),
                    ManagerContacterTelephone = x.Telephone?.Trim(),
                    ManagerContacterStatus = x.Status.ToEnum<DbAtomManagerContacterStatusEnum>(),
                    ManagerContacterIsConsent = x.IsConsent,
                    AtomRatingKind = x.AtomRatingKindID.ToEnum<DbAtomManagerContacterRatingKindEnum>(),
                    ManagerContacterCreateEmployeeID = x.CreateEmployeeID,
                    ManagerContacterRemark = x.Remark?.Trim()
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

    /// <summary>核心-管理者-窗口-資料庫服務-取得多筆[名稱]</summary>
    public async Task<CoMgrCttDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCttDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .Where(x => reqObj.ManagerContacterIDList.Contains(x.ID))
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoMgrCttDbGetManyNameRspMdl
            {
                ManagerContacterList = itemList.Select(x => new CoMgrCttDbGetManyNameRspItemMdl
                {
                    ManagerContacterID = x.ID,
                    ManagerContacterName = x.Name?.Trim()
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
        finally
        {
            this._mainContext.ChangeTracker.Clear();
        }
    }

    /// <summary>核心-管理者-窗口-資料庫服務-新增</summary>
    public async Task<CoMgrCttDbAddRspMdl> AddAsync(CoMgrCttDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerContacter
            {
                ManagerCompanyID = reqObj.ManagerCompanyID,
                Name = reqObj.ManagerContacterName,
                Email = reqObj.ManagerContacterEmail,
                Phone = reqObj.ManagerContacterPhone,
                Department = reqObj.ManagerContacterDepartment,
                JobTitle = reqObj.ManagerContacterJobTitle,
                Telephone = reqObj.ManagerContacterTelephone,
                Status = reqObj.ManagerContacterStatus.ToInt16(),
                IsConsent = reqObj.ManagerContacterIsConsent,
                AtomRatingKindID = reqObj.ManagerContacterRatingKind.ToInt16(),
                CreateEmployeeID = reqObj.ManagerContacterCreateEmployeeID,
                Remark = reqObj.ManagerContacterRemark?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            await this._mainContext.ManagerContacter.AddAsync(item);
            await this._mainContext.SaveChangesAsync();

            var result = new CoMgrCttDbAddRspMdl
            {
                ManagerContacterID = item.ID,
            };

            return result;
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

    /// <summary>核心-管理者-窗口-資料庫服務-更新</summary>
    public async Task<CoMgrCttDbUpdateRspMdl> UpdateAsync(CoMgrCttDbUpdateReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerContacter
                .Where(x => x.ID == reqObj.ManagerContacterID)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.ManagerCompanyID, x => reqObj.ManagerCompanyID.HasValue ? reqObj.ManagerCompanyID.Value : x.ManagerCompanyID)
                    .SetProperty(x => x.Name, x => !string.IsNullOrEmpty(reqObj.ManagerContacterName) ? reqObj.ManagerContacterName.Trim() : x.Name)
                    .SetProperty(x => x.Email, x => !string.IsNullOrEmpty(reqObj.ManagerContacterEmail) ? reqObj.ManagerContacterEmail.Trim() : x.Email)
                    .SetProperty(x => x.Phone, x => !string.IsNullOrEmpty(reqObj.ManagerContacterPhone) ? reqObj.ManagerContacterPhone.Trim() : x.Phone)
                    .SetProperty(x => x.Department, x => !string.IsNullOrEmpty(reqObj.ManagerContacterDepartment) ? reqObj.ManagerContacterDepartment.Trim() : x.Department)
                    .SetProperty(x => x.JobTitle, x => !string.IsNullOrEmpty(reqObj.ManagerContacterJobTitle) ? reqObj.ManagerContacterJobTitle.Trim() : x.JobTitle)
                    .SetProperty(x => x.Telephone, x => !string.IsNullOrEmpty(reqObj.ManagerContacterTelephone) ? reqObj.ManagerContacterTelephone.Trim() : x.Telephone)
                    .SetProperty(x => x.Status, x => reqObj.ManagerContacterStatus.HasValue ? reqObj.ManagerContacterStatus.Value.ToInt16() : x.Status)
                    .SetProperty(x => x.IsConsent, x => reqObj.ManagerContacterIsConsent.HasValue ? reqObj.ManagerContacterIsConsent.Value : x.IsConsent)
                    .SetProperty(x => x.AtomRatingKindID, x => reqObj.ManagerContacterRatingKind.HasValue ? reqObj.ManagerContacterRatingKind.Value.ToInt16() : x.AtomRatingKindID)
                    .SetProperty(x => x.Remark, x => reqObj.ManagerContacterRemark != null ? reqObj.ManagerContacterRemark.Trim() : x.Remark)
                    .SetProperty(x => x.UpdateTime, DateTimeOffset.UtcNow));

            var result = new CoMgrCttDbUpdateRspMdl
            {
                AffectedCount = affectedCount
            };

            return result;
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

    /// <summary>核心-管理者-窗口-資料庫服務-移除</summary>
    public async Task<CoMgrCttDbRemoveRspMdl> RemoveAsync(CoMgrCttDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerContacter
                .Where(x => x.ID == reqObj.ManagerContacterID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrCttDbRemoveRspMdl
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

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-窗口

    /// <summary>核心-管理者-窗口-資料庫服務-取得[筆數]從[管理者後台-系統設定-窗口]</summary>
    public async Task<CoMgrCttDbGetCountFromMbsSysCtrRspMdl> GetCountFromMbsSysCtrAsync(CoMgrCttDbGetCountFromMbsSysCtrReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .Where(x =>
                    // 窗口姓名
                    (string.IsNullOrEmpty(reqObj.ManagerContacterName) || x.Name.Contains(reqObj.ManagerContacterName.Trim())) &&
                    // 窗口Email
                    (string.IsNullOrEmpty(reqObj.ManagerContacterEmail) || x.Email.Contains(reqObj.ManagerContacterEmail.Trim())) &&
                    // 公司ID
                    (reqObj.ManagerCompanyID.HasValue == false || x.ManagerCompanyID == reqObj.ManagerCompanyID.Value) &&
                    // 開發評等ID
                    (reqObj.ManagerContacterRatingKind.HasValue == false || x.AtomRatingKindID == reqObj.ManagerContacterRatingKind.Value.ToInt16())
                )
                .CountAsync();

            var rspObj = new CoMgrCttDbGetCountFromMbsSysCtrRspMdl
            {
                Count = count,
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

    /// <summary>核心-管理者-窗口-資料庫服務-取得多筆[管理者後台-系統設定-窗口]</summary>
    public async Task<CoMgrCttDbGetManyFromMbsSysCtrRspMdl> GetManyFromMbsSysCtrAsync(CoMgrCttDbGetManyFromMbsSysCtrReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerContacter
                            .AsNoTracking()
                            .Where(x =>
                                // 窗口姓名
                                (string.IsNullOrEmpty(reqObj.ManagerContacterName) || x.Name.Contains(reqObj.ManagerContacterName.Trim())) &&
                                // 窗口Email
                                (string.IsNullOrEmpty(reqObj.ManagerContacterEmail) || x.Email.Contains(reqObj.ManagerContacterEmail.Trim())) &&
                                // 公司ID
                                (reqObj.ManagerCompanyID.HasValue == false || x.ManagerCompanyID == reqObj.ManagerCompanyID.Value) &&
                                // 開發評等ID
                                (reqObj.ManagerContacterRatingKind.HasValue == false || x.AtomRatingKindID == reqObj.ManagerContacterRatingKind.Value.ToInt16())
                            )
                            .OrderByDescending(x => x.ID)
                            .Skip(skipRows)
                            .Take(takeRows)
                            .Select(x => new
                            {
                                x.ID,
                                x.ManagerCompanyID,
                                x.Name,
                                x.Email,
                                x.Phone,
                                x.Department,
                                x.JobTitle,
                                x.AtomRatingKindID
                            })
                            .ToListAsync();

            var rspObj = new CoMgrCttDbGetManyFromMbsSysCtrRspMdl
            {
                ManagerContacterList = itemList.Select(x => new CoMgrCttDbGetManyFrommbsSysCtrRspItemMdl
                {
                    ManagerContacterID = x.ID,
                    ManagerCompanyID = x.ManagerCompanyID,
                    ManagerContacterName = x.Name?.Trim(),
                    ManagerContacterEmail = x.Email?.Trim(),
                    ManagerContacterPhone = x.Phone?.Trim(),
                    ManagerContacterDepartment = x.Department?.Trim(),
                    ManagerContacterJobTitle = x.JobTitle?.Trim(),
                    ManagerContacterRatingKind = x.AtomRatingKindID.ToEnum<DbAtomManagerContacterRatingKindEnum>()
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

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrCttDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCttDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerContacter
                .AsNoTracking()
                .Where(x =>
                    (reqObj.ManagerCompanyID.HasValue == false || x.ManagerCompanyID == reqObj.ManagerCompanyID.Value) &&
                    (
                        (string.IsNullOrEmpty(reqObj.ManagerContacterName) && string.IsNullOrEmpty(reqObj.ManagerContacterEmail)) ||
                        (!string.IsNullOrEmpty(reqObj.ManagerContacterName) && x.Name.Contains(reqObj.ManagerContacterName.Trim())) ||
                        (!string.IsNullOrEmpty(reqObj.ManagerContacterEmail) && x.Email.Contains(reqObj.ManagerContacterEmail.Trim()))
                    )
                )
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Email,
                })
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoMgrCttDbGetManyFromMbsBscRspMdl
            {
                ManagerContacterList = itemList
                    .Select(x => new CoMgrCttDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerContacterID = x.ID,
                        ManagerContacterName = x.Name?.Trim(),
                        ManagerContacterEmail = x.Email?.Trim()
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
