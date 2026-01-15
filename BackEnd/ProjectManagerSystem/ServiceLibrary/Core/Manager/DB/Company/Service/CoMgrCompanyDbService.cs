using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Manager.DB.Company.Format;

namespace ServiceLibrary.Core.Manager.DB.Company.Service;

/// <summary>核心-管理者-公司-資料庫服務</summary>
public class CoMgrCompanyDbService : ICoMgrCompanyDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoMgrCompanyDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoMgrCompanyDbService(
        ILogger<CoMgrCompanyDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-管理者-公司-資料庫-是否存在</summary>
    public async Task<CoMgrComDbExistRspMdl> ExistAsync(CoMgrComDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .AnyAsync(x => x.ID == reqObj.ManagerCompanyID);

            var rspObj = new CoMgrComDbExistRspMdl
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

    /// <summary>核心-管理者-公司-資料庫-檢查統一編號重複</summary>
    public async Task<CoMgrComDbUniNumExistRspMdl> UniNumExistAsync(CoMgrComDbUniNumExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.ManagerCompanyID.HasValue == false || x.ID == reqObj.ManagerCompanyID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.UnifiedNumber) || x.UnifiedNumber == reqObj.UnifiedNumber));

            var rspObj = new CoMgrComDbUniNumExistRspMdl
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

    /// <summary>核心-管理者-公司-資料庫-新增</summary>
    public async Task<CoMgrCmpDbAddRspMdl> AddAsync(CoMgrCmpDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new ManagerCompany
            {
                UnifiedNumber = reqObj.ManagerCompanyUnifiedNumber?.Trim(),
                Name = reqObj.ManagerCompanyName?.Trim(),
                Status = reqObj.AtomManagerCompanyStatus.ToInt16(),
                ManagerCompanyMainKindID = reqObj.ManagerCompanyMainKindID,
                ManagerCompanySubKindID = reqObj.ManagerCompanySubKindID,
                AtomCustomerGradeID = reqObj.AtomCustomerGrade.HasValue ? reqObj.AtomCustomerGrade.Value.ToInt16() : DbAtomCustomerGradeEnum.NotSelected.ToInt16(),
                AtomSecurityGradeID = reqObj.AtomSecurityGrade.HasValue ? reqObj.AtomSecurityGrade.Value.ToInt16() : DbAtomSecurityGradeEnum.NotSelected.ToInt16(),
                AtomEmployeeRangeID = reqObj.AtomEmployeeRange.HasValue ? reqObj.AtomEmployeeRange.Value.ToInt16() : DbAtomEmployeeRangeEnum.NotSelected.ToInt16(),
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.ManagerCompany.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoMgrCmpDbAddRspMdl
            {
                ManagerCompanyID = item.ID,
                ManagerCompanyCreateTime = item.CreateTime,
                ManagerCompanyUpdateTime = item.UpdateTime
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

    /// <summary>核心-管理者-公司-資料庫-新增[指定ID]</summary>
    public async Task<CoMgrCmpDbAddWithIdRspMdl> AddWithIdAsync(CoMgrCmpDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            // SQL參數
            var parameterList = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.ManagerCompanyID),
                new SqlParameter("@UnifiedNumber", reqObj.ManagerCompanyUnifiedNumber?.Trim()),
                new SqlParameter("@Name", reqObj.ManagerCompanyName?.Trim()),
                new SqlParameter("@Status", reqObj.AtomManagerCompanyStatus.ToInt16()),
                new SqlParameter("@ManagerCompanyMainKindID", reqObj.ManagerCompanyMainKindID),
                new SqlParameter("@ManagerCompanySubKindID", reqObj.ManagerCompanySubKindID),
                new SqlParameter("@AtomCustomerGradeID", (reqObj.AtomCustomerGrade ?? DbAtomCustomerGradeEnum.NotSelected).ToInt16()),
                new SqlParameter("@AtomSecurityGradeID", (reqObj.AtomSecurityGrade ?? DbAtomSecurityGradeEnum.NotSelected).ToInt16()),
                new SqlParameter("@AtomEmployeeRangeID", (reqObj.AtomEmployeeRange ?? DbAtomEmployeeRangeEnum.NotSelected).ToInt16()),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.ManagerCompany ON;
                INSERT INTO dbo.ManagerCompany (ID, UnifiedNumber, Name, Status, ManagerCompanyMainKindID, ManagerCompanySubKindID, AtomCustomerGradeID, AtomSecurityGradeID, AtomEmployeeRangeID, CreateTime, UpdateTime)
                VALUES (@ID, @UnifiedNumber, @Name, @Status, @ManagerCompanyMainKindID, @ManagerCompanySubKindID, @AtomCustomerGradeID, @AtomSecurityGradeID, @AtomEmployeeRangeID, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.ManagerCompany OFF;
            ";
            var result = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameterList);
            if (result == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpDbAddWithIdRspMdl
            {
                ManagerCompanyID = reqObj.ManagerCompanyID,
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

    /// <summary>核心-管理者-公司-資料庫-更新</summary>
    public async Task<CoMgrCmpDbUpdateRspMdl> UpdateAsync(CoMgrCmpDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.ManagerCompany
                .Where(x => x.ID == reqObj.ManagerCompanyID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.UnifiedNumber, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyUnifiedNumber) ? reqObj.ManagerCompanyUnifiedNumber.Trim() : x.UnifiedNumber)
                    .SetProperty(x => x.Name, x => !string.IsNullOrWhiteSpace(reqObj.ManagerCompanyName) ? reqObj.ManagerCompanyName.Trim() : x.Name)
                    .SetProperty(x => x.Status, x => reqObj.AtomManagerCompanyStatus.HasValue ? reqObj.AtomManagerCompanyStatus.Value.ToInt16() : x.Status)
                    .SetProperty(x => x.ManagerCompanyMainKindID, x => reqObj.ManagerCompanyMainKindID.HasValue ? reqObj.ManagerCompanyMainKindID.Value : x.ManagerCompanyMainKindID)
                    .SetProperty(x => x.ManagerCompanySubKindID, x => reqObj.ManagerCompanySubKindID.HasValue ? reqObj.ManagerCompanySubKindID.Value : x.ManagerCompanySubKindID)
                    .SetProperty(x => x.AtomCustomerGradeID, x => reqObj.AtomCustomerGrade.HasValue ? reqObj.AtomCustomerGrade.Value.ToInt16() : x.AtomCustomerGradeID)
                    .SetProperty(x => x.AtomSecurityGradeID, x => reqObj.AtomSecurityGrade.HasValue ? reqObj.AtomSecurityGrade.Value.ToInt16() : x.AtomSecurityGradeID)
                    .SetProperty(x => x.AtomEmployeeRangeID, x => reqObj.AtomEmployeeRange.HasValue ? reqObj.AtomEmployeeRange.Value.ToInt16() : x.AtomEmployeeRangeID)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoMgrCmpDbUpdateRspMdl
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

    /// <summary>核心-管理者-公司-資料庫-移除</summary>
    public async Task<CoMgrCmpDbRemoveRspMdl> RemoveAsync(CoMgrCmpDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.ManagerCompany
                .Where(x => x.ID == reqObj.ManagerCompanyID)
                .ExecuteDeleteAsync();

            var rspObj = new CoMgrCmpDbRemoveRspMdl
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

    /// <summary>核心-管理者-公司-資料庫-取得</summary>
    public async Task<CoMgrCmpDbGetRspMdl> GetAsync(CoMgrCmpDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.ManagerCompanyID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpDbGetRspMdl
            {
                ManagerCompanyID = item.ID,
                ManagerCompanyUnifiedNumber = item.UnifiedNumber?.Trim(),
                ManagerCompanyName = item.Name?.Trim(),
                AtomManagerCompanyStatus = item.Status.ToEnum<DbAtomManagerCompanyStatusEnum>(),
                ManagerCompanyMainKindID = item.ManagerCompanyMainKindID,
                ManagerCompanySubKindID = item.ManagerCompanySubKindID,
                AtomCustomerGrade = item.AtomCustomerGradeID.ToEnum<DbAtomCustomerGradeEnum>(),
                AtomSecurityGrade = item.AtomSecurityGradeID.ToEnum<DbAtomSecurityGradeEnum>(),
                AtomEmployeeRange = item.AtomEmployeeRangeID.ToEnum<DbAtomEmployeeRangeEnum>()
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

    /// <summary>核心-管理者-公司-資料庫-取得[名稱]</summary>
    public async Task<CoMgrCmpDbGetNameRspMdl> GetNameAsync(CoMgrCmpDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .Where(x => x.ID == reqObj.ManagerCompanyID)
                .Select(x => new
                {
                    x.Name
                })
                .SingleOrDefaultAsync();

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoMgrCmpDbGetNameRspMdl
            {
                ManagerCompanyName = item.Name?.Trim()
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

    /// <summary>核心-管理者-公司-資料庫-取得多筆</summary>
    public async Task<CoMgrCmpDbGetManyRspMdl> GetManyAsync(CoMgrCmpDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .Where(x =>
                    // 名稱查詢
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyName) || x.Name.Contains(reqObj.ManagerCompanyName.Trim())) &&
                    // 狀態查詢
                    (reqObj.AtomManagerCompanyStatus.HasValue == false || reqObj.AtomManagerCompanyStatus.Value.ToInt16() == x.Status) &&
                    // 主分類ID查詢
                    (reqObj.ManagerCompanyMainKindID.HasValue == false || reqObj.ManagerCompanyMainKindID.Value == x.ManagerCompanyMainKindID) &&
                    // 子分類ID查詢
                    (reqObj.ManagerCompanySubKindID.HasValue == false || reqObj.ManagerCompanySubKindID.Value == x.ManagerCompanySubKindID) &&
                    // 客戶分級查詢
                    (reqObj.AtomCustomerGrade.HasValue == false || reqObj.AtomCustomerGrade.Value.ToInt16() == x.AtomCustomerGradeID)
                   )
                .Select(x => new
                {
                    x.ID,
                    x.UnifiedNumber,
                    x.Name,
                    x.AtomCustomerGradeID,
                    x.ManagerCompanyMainKindID,
                    x.ManagerCompanySubKindID,
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoMgrCmpDbGetManyRspMdl
            {
                ManagerCompanyList = itemList?
                    .Select(x => new CoMgrCmpDbGetManyRspItemMdl
                    {
                        ManagerCompanyID = x.ID,
                        ManagerCompanyUnifiedNumber = x.UnifiedNumber?.Trim(),
                        ManagerCompanyName = x.Name?.Trim(),
                        AtomCustomerGrade = x.AtomCustomerGradeID.ToEnum<DbAtomCustomerGradeEnum>(),
                        ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                        ManagerCompanySubKindID = x.ManagerCompanySubKindID
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

    /// <summary>核心-管理者-公司-資料庫-取得多筆[名稱]</summary>
    public async Task<CoMgrCmpDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCmpDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .Where(x => reqObj.ManagerCompanyIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoMgrCmpDbGetManyNameRspMdl
            {
                ManagerCompanyList = itemList.Select(x => new CoMgrCmpDbGetManyNameRspItemMdl
                {
                    ManagerCompanyID = x.ID,
                    ManagerCompanyName = x.Name?.Trim(),
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

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-客戶

    /// <summary>核心-管理者-公司-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]</summary>
    public async Task<CoMgrCmpDbGetCountFromMbsSysCmpRspMdl> GetCountFromMbsSysCmpAsync(CoMgrCmpDbGetCountFromMbsSysCmpReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.ManagerCompany
                .AsNoTracking()
                .Where(x =>
                    // 客戶分級
                    (reqObj.AtomCustomerGrade.HasValue == false || x.AtomCustomerGradeID == reqObj.AtomCustomerGrade.Value.ToInt16()) &&
                    // 公司主分類
                    (reqObj.ManagerCompanyMainKindID.HasValue == false || x.ManagerCompanyMainKindID == reqObj.ManagerCompanyMainKindID.Value) &&
                    // 公司子分類
                    (reqObj.ManagerCompanySubKindID.HasValue == false || x.ManagerCompanySubKindID == reqObj.ManagerCompanySubKindID.Value) &&
                    // 公司名稱
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyName) || x.Name.Contains(reqObj.ManagerCompanyName.Trim()))
                )
                .CountAsync();

            var rspObj = new CoMgrCmpDbGetCountFromMbsSysCmpRspMdl
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

    /// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-系統設定-客戶]</summary>
    public async Task<CoMgrCmpDbGetManyFromMbsSysCmpRspMdl> GetManyFromMbsSysCmpAsync(CoMgrCmpDbGetManyFromMbsSysCmpReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompany
                            .AsNoTracking()
                            .Where(x =>
                                // 客戶分級
                                (reqObj.AtomCustomerGrade.HasValue == false || x.AtomCustomerGradeID == reqObj.AtomCustomerGrade.Value.ToInt16()) &&
                                // 公司主分類
                                (reqObj.ManagerCompanyMainKindID.HasValue == false || x.ManagerCompanyMainKindID == reqObj.ManagerCompanyMainKindID.Value) &&
                                // 公司子分類
                                (reqObj.ManagerCompanySubKindID.HasValue == false || x.ManagerCompanySubKindID == reqObj.ManagerCompanySubKindID.Value) &&
                                // 公司名稱
                                (string.IsNullOrEmpty(reqObj.ManagerCompanyName) || x.Name.Contains(reqObj.ManagerCompanyName.Trim()))
                            )
                            .Select(x => new
                            {
                                x.ID,
                                x.UnifiedNumber,
                                x.Name,
                                x.AtomCustomerGradeID,
                                x.ManagerCompanyMainKindID,
                                x.ManagerCompanySubKindID,
                            })
                            .OrderByDescending(x => x.ID)
                            .Skip(skipRows)
                            .Take(takeRows)
                            .ToListAsync();

            var rspObj = new CoMgrCmpDbGetManyFromMbsSysCmpRspMdl
            {
                ManagerCompanyList = itemList.Select(x => new CoMgrCmpDbGetManyFrommbsSysCmpRspItemMdl
                {
                    ManagerCompanyID = x.ID,
                    ManagerCompanyUnifiedNumber = x.UnifiedNumber?.Trim(),
                    ManagerCompanyName = x.Name?.Trim(),
                    AtomCustomerGrade = x.AtomCustomerGradeID.ToEnum<DbAtomCustomerGradeEnum>(),
                    ManagerCompanyMainKindID = x.ManagerCompanyMainKindID,
                    ManagerCompanySubKindID = x.ManagerCompanySubKindID
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

    /// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoMgrCmpDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await this._mainContext.ManagerCompany
                            .AsNoTracking()
                            .Where(x =>
                                // 公司名稱
                                string.IsNullOrEmpty(reqObj.ManagerCompanyName) || x.Name.Contains(reqObj.ManagerCompanyName.Trim())
                            )
                            .Select(x => new
                            {
                                x.ID,
                                x.Name,
                            })
                            .OrderBy(x => x.ID)
                            .Skip(skipRows)
                            .Take(takeRows)
                            .ToListAsync();

            var rspObj = new CoMgrCmpDbGetManyFromMbsBscRspMdl
            {
                ManagerCompanyList = itemList.Select(x => new CoMgrCmpDbGetManyFromMbsBscRspItemMdl
                {
                    ManagerCompanyID = x.ID,
                    ManagerCompanyName = x.Name?.Trim(),
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