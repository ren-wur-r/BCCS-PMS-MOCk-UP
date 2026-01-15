using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;

namespace ServiceLibrary.Core.Employee.DB.Info.Service;

/// <summary>核心-員工-資訊-資料庫服務</summary>
public class CoEmpInfoDbService : ICoEmpInfoDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpInfoDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpInfoDbService(
        ILogger<CoEmpInfoDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-資訊-資料庫-是否存在</summary>
    public async Task<CoEmpInfDbExistRspMdl> ExistAsync(CoEmpInfDbExistReqMdl reqObj)
    {
        try
        {
            var isExist = await this._mainContext.Employee
                .AsNoTracking()
                .AnyAsync(x =>
                    (reqObj.EmployeeID.HasValue == false || x.ID == reqObj.EmployeeID.Value)
                    && (string.IsNullOrWhiteSpace(reqObj.EmployeeAccount) || x.Account == reqObj.EmployeeAccount));

            var rspObj = new CoEmpInfDbExistRspMdl
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

    /// <summary>核心-員工-資訊-資料庫-新增</summary>
    public async Task<CoEmpInfDbAddRspMdl> AddAsync(CoEmpInfDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new DataModelLibrary.EfCore.ProjectManagerMain.Employee()
            {
                Account = reqObj.EmployeeAccount.Trim(),
                Password = reqObj.EmployeePassword.Trim(),
                Email = reqObj.EmployeeEmail.Trim(),
                Name = reqObj.EmployeeName.Trim(),
                Remark = reqObj.EmployeeRemark?.Trim(),
                ManagerRoleID = reqObj.ManagerRoleID,
                IsEnable = reqObj.EmployeeIsEnable,
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };
            this._mainContext.Employee.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpInfDbAddRspMdl()
            {
                EmployeeID = item.ID,
                EmployeeCreateTime = currentTime,
                EmployeeUpdateTime = currentTime,
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

    /// <summary>核心-員工-資訊-資料庫-新增[指定ID]</summary>
    public async Task<CoEmpInfDbAddWithIdRspMdl> AddWithIdAsync(CoEmpInfDbAddWithIdReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            //SQL參數
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", reqObj.EmployeeID),
                new SqlParameter("@Account", reqObj.EmployeeAccount.Trim()),
                new SqlParameter("@Password", reqObj.EmployeePassword.Trim()),
                new SqlParameter("@Email", reqObj.EmployeeEmail.Trim()),
                new SqlParameter("@Name", reqObj.EmployeeName.Trim()),
                new SqlParameter("@Remark", reqObj.EmployeeRemark?.Trim()),
                new SqlParameter("@ManagerRoleID", reqObj.ManagerRoleID),
                new SqlParameter("@IsEnable", reqObj.EmployeeIsEnable),
                new SqlParameter("@CreateTime", currentTime),
                new SqlParameter("@UpdateTime", currentTime),
            };

            var sql = @"
                SET IDENTITY_INSERT dbo.Employee ON;
                INSERT INTO dbo.Employee (ID, Account, Password, Email, Name, Remark, ManagerRoleID, IsEnable, CreateTime, UpdateTime)
                VALUES (@ID, @Account, @Password, @Email, @Name, @Remark, @ManagerRoleID, @IsEnable, @CreateTime, @UpdateTime);
                SET IDENTITY_INSERT dbo.Employee OFF;
            ";
            var result = await this._mainContext.Database.ExecuteSqlRawAsync(sql, parameters);

            if (result == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpInfDbAddWithIdRspMdl()
            {
                EmployeeID = reqObj.EmployeeID,
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

    /// <summary>核心-員工-資訊-資料庫-更新</summary>
    public async Task<CoEmpInfDbUpdateRspMdl> UpdateAsync(CoEmpInfDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.Employee
                .Where(x => x.ID == reqObj.EmployeeID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.Password, x => string.IsNullOrWhiteSpace(reqObj.EmployeePassword) ? x.Password : reqObj.EmployeePassword.Trim())
                        .SetProperty(x => x.Name, x => string.IsNullOrWhiteSpace(reqObj.EmployeeName) ? x.Name : reqObj.EmployeeName.Trim())
                        .SetProperty(x => x.Remark, x => string.IsNullOrWhiteSpace(reqObj.EmployeeRemark) ? x.Remark : reqObj.EmployeeRemark.Trim())
                        .SetProperty(x => x.ManagerRoleID, x => reqObj.ManagerRoleID.HasValue == false ? x.ManagerRoleID : reqObj.ManagerRoleID.Value)
                        .SetProperty(x => x.IsEnable, x => reqObj.EmployeeIsEnable.HasValue == false ? x.IsEnable : reqObj.EmployeeIsEnable.Value)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpInfDbUpdateRspMdl
            {
                AffectedCount = affectedCount,
                EmployeeUpdateTime = currentTime,
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

    /// <summary>核心-員工-資訊-資料庫-移除</summary>
    public async Task<CoEmpInfDbRemoveRspMdl> RemoveAsync(CoEmpInfDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.Employee
                .Where(x => x.ID == reqObj.EmployeeID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpInfDbRemoveRspMdl
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
    }

    /// <summary>核心-員工-資訊-資料庫-取得[ID]</summary>
    public async Task<CoEmpInfDbGetIdRspMdl> GetIdAsync(CoEmpInfDbGetIdReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x =>
                    // 員工-帳號
                    (string.IsNullOrWhiteSpace(reqObj.EmployeeAccount) || x.Account == reqObj.EmployeeAccount)
                    // 員工-密碼
                    && (string.IsNullOrWhiteSpace(reqObj.EmployeePassword) || x.Password == reqObj.EmployeePassword))
                .Select(x => new
                {
                    x.ID
                })
                .SingleOrDefaultAsync();
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpInfDbGetIdRspMdl()
            {
                EmployeeID = item.ID,
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

    /// <summary>核心-員工-資訊-資料庫-取得</summary>
    public async Task<CoEmpInfDbGetRspMdl> GetAsync(CoEmpInfDbGetReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = await this._mainContext.Employee
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeeID);
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpInfDbGetRspMdl()
            {
                EmployeeAccount = item.Account.Trim(),
                EmployeePassword = item.Password.Trim(),
                EmployeeEmail = item.Email.Trim(),
                EmployeeName = item.Name.Trim(),
                EmployeeRemark = item.Remark?.Trim(),
                ManagerRoleID = item.ManagerRoleID,
                EmployeeIsEnable = item.IsEnable,
                EmployeeCreateTime = currentTime,
                EmployeeUpdateTime = currentTime,
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

    /// <summary>核心-員工-資訊-資料庫-取得[筆數]</summary>
    public async Task<CoEmpInfDbGetCountRspMdl> GetCountAsync(CoEmpInfDbGetCountReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x =>
                    // 管理者-部門ID
                    (reqObj.EmployeeID.HasValue == false || reqObj.EmployeeID.Value == x.ID))
                .CountAsync();

            var rspObj = new CoEmpInfDbGetCountRspMdl()
            {
                Count = count
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

    /// <summary>核心-員工-資訊-資料庫-取得密碼</summary>
    public async Task<CoEmpInfDbGetPasswordRspMdl> GetPasswordAsync(CoEmpInfDbGetPasswordReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => x.ID == reqObj.EmployeeID)
                .Select(x => new
                {
                    x.Password
                })
                .SingleOrDefaultAsync();
            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpInfDbGetPasswordRspMdl()
            {
                EmployeePassword = item.Password.Trim(),
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

    /// <summary>核心-員工-資訊-資料庫-取得多筆[帳號]</summary>
    public async Task<CoEmpInfDbGetManyAccountRspMdl> GetManyAccountAsync(CoEmpInfDbGetManyAccountReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => reqObj.EmployeeIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.ID,
                    x.Account,
                })
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyAccountRspMdl()
            {
                EmployeeList = itemList
                    .Select(x => new CoEmpInfDbGetManyAccountRspItemMdl()
                    {
                        EmployeeID = x.ID,
                        EmployeeAccount = x.Account.Trim(),
                    })
                    .ToList(),
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

    /// <summary>核心-員工-資訊-資料庫-取得[名稱]</summary>
    public async Task<CoEmpInfDbGetNameRspMdl> GetNameAsync(CoEmpInfDbGetNameReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => x.ID == reqObj.EmployeeID)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .SingleOrDefaultAsync();

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpInfDbGetNameRspMdl
            {
                EmployeeID = item.ID,
                EmployeeName = item.Name?.Trim()
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

    /// <summary>核心-員工-資訊-資料庫-取得多筆[名稱]</summary>
    public async Task<CoEmpInfDbGetManyNameRspMdl> GetManyNameAsync(CoEmpInfDbGetManyNameReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => reqObj.EmployeeIdList.Contains(x.ID))
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyNameRspMdl
            {
                EmployeeList = itemList
                    .Select(x => new CoEmpInfDbGetManyNameRspItemMdl
                    {
                        EmployeeID = x.ID,
                        EmployeeName = x.Name?.Trim()
                    })
                    .ToList()
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

    /// <summary>核心-員工-資訊-資料庫-取得多筆[信箱]</summary>
    public async Task<CoEmpInfDbGetManyEmailRspMdl> GetManyEmailAsync(CoEmpInfDbGetManyEmailReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => reqObj.EmployeeIdList.Contains(x.ID))
                .Select(x => new
                {
                    x.Email,
                })
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyEmailRspMdl
            {
                EmployeeList = itemList
                    .Select(x => new CoEmpInfDbGetManyEmailRspItemMdl
                    {
                        EmployeeEmail = x.Email.Trim(),
                    })
                    .ToList()
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

    /// <summary>核心-員工-資訊-資料庫-檢查多筆存在</summary>
    public async Task<CoEmpInfDbCheckManyExistRspMdl> CheckManyExistAsync(CoEmpInfDbCheckManyExistReqMdl reqObj)
    {
        try
        {
            var existingIds = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => reqObj.EmployeeIdList.Contains(x.ID))
                .Select(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpInfDbCheckManyExistRspMdl
            {
                EmployeeList = reqObj.EmployeeIdList
                    .Select(id => new CoEmpInfDbCheckManyExistRspItemMdl
                    {
                        EmployeeID = id,
                        IsExist = existingIds.Contains(id)
                    })
                    .ToList()
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

    #region ManagerBackSite.System.Employee 管理者後台-系統設定-員工

    /// <summary>核心-員工-資訊-資料庫-取得[筆數]從[管理者後台-系統設定-員工]</summary>

    public async Task<CoEmpInfDbGetCountFromMbsSysEmpRspMdl> GetCountFromMbsSysEmpAsync(CoEmpInfDbGetCountFromMbsSysEmpReqMdl reqObj)
    {
        try
        {
            var count = await
                (
                    from emp in this._mainContext.Employee
                        .Where(epx =>
                            // 管理者-角色ID
                            (reqObj.ManagerRoleID.HasValue == false || reqObj.ManagerRoleID == epx.ManagerRoleID)
                            // 員工-帳號
                            && (string.IsNullOrWhiteSpace(reqObj.EmployeeAccount) || epx.Account.Contains(reqObj.EmployeeAccount))
                            // 員工-是否啟用
                            && (reqObj.EmployeeIsEnable.HasValue == false || reqObj.EmployeeIsEnable == epx.IsEnable))
                    from rol in this._mainContext.ManagerRole
                        .Where(rox => rox.ID == emp.ManagerRoleID)
                    from dpt in this._mainContext.ManagerDepartment
                        .Where(dpx => dpx.ID == rol.ManagerDepartmentID
                            // 管理者-部門ID（在這裡篩選！）
                            && (reqObj.ManagerDepartmentID.HasValue == false || reqObj.ManagerDepartmentID == dpx.ID))
                    select 1
                ).CountAsync();

            var rspObj = new CoEmpInfDbGetCountFromMbsSysEmpRspMdl()
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

    /// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-系統設定-員工]</summary>
    public async Task<CoEmpInfDbGetManyFromMbsSysEmpRspMdl> GetManyFromMbsSysEmpAsync(CoEmpInfDbGetManyFromMbsSysEmpReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from emp in this._mainContext.Employee
                        .Where(epx =>
                            // 管理者-角色ID
                            (reqObj.ManagerRoleID.HasValue == false || reqObj.ManagerRoleID == epx.ManagerRoleID)
                            // 員工-帳號
                            && (string.IsNullOrWhiteSpace(reqObj.EmployeeAccount) || epx.Account.Contains(reqObj.EmployeeAccount))
                            // 員工-是否啟用
                            && (reqObj.EmployeeIsEnable.HasValue == false || reqObj.EmployeeIsEnable == epx.IsEnable))
                    from rol in this._mainContext.ManagerRole
                        .Where(rox => rox.ID == emp.ManagerRoleID)
                    from dpt in this._mainContext.ManagerDepartment
                        .Where(dpx => dpx.ID == rol.ManagerDepartmentID
                            // 管理者-部門ID（在這裡篩選！）
                            && (reqObj.ManagerDepartmentID.HasValue == false || reqObj.ManagerDepartmentID == dpx.ID))
                    select new
                    {
                        // 員工
                        EmployeeID = emp.ID,
                        EmployeeName = emp.Name,
                        EmployeeAccount = emp.Account,
                        EmployeeIsEnable = emp.IsEnable,
                        // 角色
                        ManagerRoleID = rol.ID,
                        ManagerRoleName = rol.Name,
                        // 部門
                        ManagerDepartmentID = dpt.ID,
                        ManagerDepartmentName = dpt.Name,
                    }
                ).AsNoTracking()
                .OrderByDescending(x => x.EmployeeID)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyFromMbsSysEmpRspMdl()
            {
                EmployeeList = itemList
                    .Select(x => new CoEmpInfDbGetManyFromMbsSysEmpRspItemMdl
                    {
                        EmployeeID = x.EmployeeID,
                        ManagerDepartmentID = x.ManagerDepartmentID,
                        ManagerDepartmentName = x.ManagerDepartmentName?.Trim(),
                        ManagerRoleID = x.ManagerRoleID,
                        ManagerRoleName = x.ManagerRoleName?.Trim(),
                        EmployeeName = x.EmployeeName?.Trim(),
                        EmployeeAccount = x.EmployeeAccount?.Trim(),
                        EmployeeIsEnable = x.EmployeeIsEnable,
                    })
                    .ToList(),
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

    #region ManagerBackSite.System.Role 管理者後台-系統設定-角色

    /// <summary>核心-員工-資訊-資料庫-取得多筆[筆數]依[管理者後台-系統設定-角色ID]</summary>
    public async Task<CoEmpInfDbGetManyCountByManagerRoleIdRspMdl> GetManyCountByManagerRoleIdAsync(CoEmpInfDbGetManyCountByManagerRoleIdReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from e in this._mainContext.Employee
                        .Where(ex => reqObj.ManagerRoleIdList.Contains(ex.ManagerRoleID))
                    group e by new { e.ManagerRoleID } into eg
                    select new
                    {
                        eg.Key.ManagerRoleID,
                        Count = eg.Count()
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyCountByManagerRoleIdRspMdl()
            {
                ManagerRoleList = itemList
                    .Select(x => new CoEmpInfDbGetManyCountByManagerRoleIdRspItemMdl()
                    {
                        ManagerRoleID = x.ManagerRoleID,
                        Count = x.Count,
                    })
                    .ToList(),
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

    /// <summary>核心-員工-資訊-資料庫-取得[筆數]依[管理者角色ID]</summary>
    public async Task<CoEmpInfDbGetCountByManagerRoleIdRspMdl> GetCountByManagerRoleIdAsync(CoEmpInfDbGetCountByManagerRoleIdReqMdl reqObj)
    {
        try
        {
            var count = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => x.ManagerRoleID == reqObj.ManagerRoleID)
                .CountAsync();

            var rspObj = new CoEmpInfDbGetCountByManagerRoleIdRspMdl()
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

    /// <summary>核心-員工-資訊-資料庫-取得多筆[員工ID]依[管理者角色ID]</summary>
    public async Task<CoEmpInfDbGetManyIdByManagerRoleIdRspMdl> GetManyIdByManagerRoleIdAsync(CoEmpInfDbGetManyIdByManagerRoleIdReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.Employee
                .AsNoTracking()
                .Where(x => x.ManagerRoleID == reqObj.ManagerRoleID)
                .Select(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyIdByManagerRoleIdRspMdl()
            {
                EmployeeIdList = itemList,
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

    #region 複雜查詢

    /// <summary>核心-員工-資訊-資料庫-取得多筆[地區][部門][員工]</summary>
    public async Task<CoEmpInfDbGetManyRegionDepartmentEmployeeRspMdl> GetManyRegionDepartmentEmployeeAsync(CoEmpInfDbGetManyRegionDepartmentEmployeeReqMdl reqObj)
    {
        try
        {
            var itemList = await
                (
                    from e in this._mainContext.Employee
                        .Where(ex => reqObj.EmployeeIdList.Contains(ex.ID))
                    from mr in this._mainContext.ManagerRole
                        .Where(mrx => mrx.ID == e.ManagerRoleID)
                    from mrg in this._mainContext.ManagerRegion
                        .Where(mrgx => mrgx.ID == mr.ManagerRegionID)
                    from md in this._mainContext.ManagerDepartment
                        .Where(mdx => mdx.ID == mr.ManagerDepartmentID)
                    select new
                    {
                        e.ID,
                        ManagerRegionName = mrg.Name,
                        ManagerDepartmentName = md.Name,
                        e.Name,
                    }
                ).AsNoTracking()
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyRegionDepartmentEmployeeRspMdl
            {
                EmployeeList = itemList
                    .Select(x => new CoEmpInfDbGetManyRegionDepartmentEmployeeRspItemMdl
                    {
                        EmployeeID = x.ID,
                        ManagerRegionName = x.ManagerRegionName?.Trim(),
                        ManagerDepartmentName = x.ManagerDepartmentName?.Trim(),
                        EmployeeName = x.Name?.Trim(),
                    })
                    .ToList()
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

    /// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-基本]</summary>
    public async Task<CoEmpInfDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpInfDbGetManyFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await
                (
                    from md in this._mainContext.ManagerDepartment
                        .Where(mdx =>
                            (reqObj.ManagerDepartmentID.HasValue == false || mdx.ID == reqObj.ManagerDepartmentID.Value)
                            && mdx.IsEnable == true
                        )
                    from mr in this._mainContext.ManagerRegion
                        .Where(mrx =>
                            (reqObj.ManagerRegionID.HasValue == false || mrx.ID == reqObj.ManagerRegionID.Value)
                            && mrx.IsEnable == true
                        )
                    from mro in this._mainContext.ManagerRole
                        .Where(mrox =>
                            (reqObj.ManagerRoleID.HasValue == false || mrox.ID == reqObj.ManagerRoleID)
                            && mrox.ManagerRegionID == mr.ID
                            && mrox.ManagerDepartmentID == md.ID
                            && mrox.IsEnable == true
                        )
                    from e in this._mainContext.Employee
                        .Where(ex =>
                            ex.ManagerRoleID == mro.ID
                            && (string.IsNullOrEmpty(reqObj.EmployeeName) || ex.Name.Contains(reqObj.EmployeeName))
                            && (reqObj.EmployeeIsEnable.HasValue == false || ex.IsEnable == reqObj.EmployeeIsEnable.Value)
                        )
                    select new
                    {
                        DID = md.ID,
                        DName = md.Name,
                        RID = mr.ID,
                        RName = mr.Name,
                        e.ID,
                        e.Name,
                        e.IsEnable
                    }
                )
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpInfDbGetManyFromMbsBscRspMdl
            {
                EmployeeList = itemList
                    .Select(x => new CoEmpInfDbGetManyFromMbsBscRspItemMdl
                    {
                        ManagerRegionID = x.RID,
                        ManagerRegionName = x.RName?.Trim(),
                        ManagerDepartmentID = x.DID,
                        ManagerDepartmentName = x.DName?.Trim(),
                        EmployeeID = x.ID,
                        EmployeeName = x.Name?.Trim(),
                        EmployeeIsEnable = x.IsEnable
                    })
                    .ToList()
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