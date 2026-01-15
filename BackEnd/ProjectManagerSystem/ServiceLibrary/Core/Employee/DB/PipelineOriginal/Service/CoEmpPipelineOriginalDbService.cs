using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomManagerContacter;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Service;

/// <summary>核心-員工-商機原始資料-資料庫服務</summary>
public class CoEmpPipelineOriginalDbService : ICoEmpPipelineOriginalDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineOriginalDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineOriginalDbService(
        ILogger<CoEmpPipelineOriginalDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機原始資料-資料庫-取得</summary>
    public async Task<CoEmpPplOgnDbGetRspMdl> GetAsync(CoEmpPplOgnDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineOriginal
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.EmployeePipelineID == reqObj.EmployeePipelineID);
            if (item == default)
            {
                return default;
            }

            var rspObj = new CoEmpPplOgnDbGetRspMdl()
            {
                EmployeePipelineID = item.EmployeePipelineID,
                ManagerCompanyUnifiedNumber = item.ManagerCompanyUnifiedNumber?.Trim(),
                ManagerCompanyName = item.ManagerCompanyName?.Trim(),
                AtomEmployeeRange = item.AtomEmployeeRangeID.HasValue ? item.AtomEmployeeRangeID.Value.ToEnum<DbAtomEmployeeRangeEnum>() : null,
                AtomCustomerGrade = item.AtomCustomerGradeID.HasValue ? item.AtomCustomerGradeID.Value.ToEnum<DbAtomCustomerGradeEnum>() : null,
                ManagerCompanyMainKindName = item.ManagerCompanyMainKindName?.Trim(),
                ManagerCompanySubKindName = item.ManagerCompanySubKindName?.Trim(),
                AtomCity = item.AtomCityID.HasValue ? item.AtomCityID.Value.ToEnum<DbAtomCityEnum>() : null,
                ManagerCompanyLocationAddress = item.ManagerCompanyLocationAddress?.Trim(),
                ManagerCompanyLocationTelephone = item.ManagerCompanyLocationTelephone?.Trim(),
                ManagerCompanyLocationStatus = item.ManagerCompanyLocationStatus.HasValue ? item.ManagerCompanyLocationStatus.Value.ToEnum<DbAtomManagerCompanyStatusEnum>() : null,
                ManagerContacterName = item.ManagerContacterName?.Trim(),
                ManagerContacterEmail = item.ManagerContacterEmail?.Trim(),
                ManagerContacterPhone = item.ManagerContacterPhone?.Trim(),
                ManagerContacterDepartment = item.ManagerContacterDepartment?.Trim(),
                ManagerContacterJobTitle = item.ManagerContacterJobTitle?.Trim(),
                ManagerContacterTelephone = item.ManagerContacterTelephone?.Trim(),
                ManagerContacterIsConsent = item.ManagerContacterIsConsent,
                ManagerContacterStatus = item.ManagerContacterStatus.ToEnum<DbAtomManagerContacterStatusEnum>(),
                AtomRatingKind = item.AtomRatingKindID.ToEnum<DbAtomManagerContacterRatingKindEnum>(),
                TeamsRegistrationStatus = item.TeamsRegistrationStatus?.Trim(),
                TeamsRegistrationTime = item.TeamsRegistrationTime,
                TeamsLastLeaveTime = item.TeamsLastLeaveTime,
                TeamsFirstJoinTime = item.TeamsFirstJoinTime,
                TeamsMeetingDuration = item.TeamsMeetingDuration?.Trim(),
                TeamsRole = item.TeamsRole?.Trim(),
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

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆</summary>
    public async Task<CoEmpPplOgnDbGetManyRspMdl> GetManyAsync(CoEmpPplOgnDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineOriginal
                .AsNoTracking()
                .Where(x => reqObj.EmployeePipelineIDList.Contains(x.EmployeePipelineID))
                .Select(x => new
                {
                    x.EmployeePipelineID,
                    x.ManagerCompanyUnifiedNumber,
                    x.ManagerCompanyName,
                    x.AtomEmployeeRangeID,
                    x.AtomCustomerGradeID,
                    x.ManagerCompanyMainKindName,
                    x.ManagerCompanySubKindName,
                    x.AtomCityID,
                    x.ManagerCompanyLocationAddress,
                    x.ManagerCompanyLocationTelephone,
                    x.ManagerCompanyLocationStatus,
                    x.ManagerContacterName,
                    x.ManagerContacterEmail,
                    x.ManagerContacterPhone,
                    x.ManagerContacterDepartment,
                    x.ManagerContacterJobTitle,
                    x.ManagerContacterTelephone,
                    x.ManagerContacterIsConsent,
                    x.ManagerContacterStatus,
                    x.AtomRatingKindID,
                    x.TeamsRegistrationStatus,
                    x.TeamsRegistrationTime,
                    x.TeamsLastLeaveTime,
                    x.TeamsFirstJoinTime,
                    x.TeamsMeetingDuration,
                    x.TeamsRole
                })
                .ToListAsync();

            var rspObj = new CoEmpPplOgnDbGetManyRspMdl
            {
                EmployeePipelineOriginalList = itemList.Select(x => new CoEmpPplOgnDbGetManyRspItemMdl
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    ManagerCompanyUnifiedNumber = x.ManagerCompanyUnifiedNumber?.Trim(),
                    ManagerCompanyName = x.ManagerCompanyName?.Trim(),
                    AtomEmployeeRange = x.AtomEmployeeRangeID.HasValue ? x.AtomEmployeeRangeID.Value.ToEnum<DbAtomEmployeeRangeEnum>() : null,
                    AtomCustomerGrade = x.AtomCustomerGradeID.HasValue ? x.AtomCustomerGradeID.Value.ToEnum<DbAtomCustomerGradeEnum>() : null,
                    ManagerCompanyMainKindName = x.ManagerCompanyMainKindName?.Trim(),
                    ManagerCompanySubKindName = x.ManagerCompanySubKindName?.Trim(),
                    AtomCity = x.AtomCityID.HasValue ? x.AtomCityID.Value.ToEnum<DbAtomCityEnum>() : null,
                    ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress?.Trim(),
                    ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone?.Trim(),
                    ManagerCompanyLocationStatus = x.ManagerCompanyLocationStatus.HasValue ? x.ManagerCompanyLocationStatus.Value.ToEnum<DbAtomManagerCompanyStatusEnum>() : null,
                    ManagerContacterName = x.ManagerContacterName?.Trim(),
                    ManagerContacterEmail = x.ManagerContacterEmail?.Trim(),
                    ManagerContacterPhone = x.ManagerContacterPhone?.Trim(),
                    ManagerContacterDepartment = x.ManagerContacterDepartment?.Trim(),
                    ManagerContacterJobTitle = x.ManagerContacterJobTitle?.Trim(),
                    ManagerContacterTelephone = x.ManagerContacterTelephone?.Trim(),
                    ManagerContacterIsConsent = x.ManagerContacterIsConsent,
                    ManagerContacterStatus = x.ManagerContacterStatus.ToEnum<DbAtomManagerContacterStatusEnum>(),
                    AtomRatingKind = x.AtomRatingKindID.ToEnum<DbAtomManagerContacterRatingKindEnum>(),
                    TeamsRegistrationStatus = x.TeamsRegistrationStatus?.Trim(),
                    TeamsRegistrationTime = x.TeamsRegistrationTime,
                    TeamsLastLeaveTime = x.TeamsLastLeaveTime,
                    TeamsFirstJoinTime = x.TeamsFirstJoinTime,
                    TeamsMeetingDuration = x.TeamsMeetingDuration?.Trim(),
                    TeamsRole = x.TeamsRole?.Trim(),
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

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆從[Email列表]</summary>
    public async Task<CoEmpPplOgnDbGetManyByEmailListRspMdl> GetManyByEmailListAsync(CoEmpPplOgnDbGetManyByEmailListReqMdl reqObj)
    {
        try
        {
            var itemList = await (
                    from ogn in this._mainContext.EmployeePipelineOriginal
                                        .AsNoTracking()
                                        .Where(x => reqObj.ManagerContacterEmailList.Contains(x.ManagerContacterEmail))
                    from pip in this._mainContext.EmployeePipeline
                                        .AsNoTracking()
                                        .Where(x => x.ManagerActivityID != null &&
                                                    x.ManagerActivityID == reqObj.ManagerActivityID &&
                                                    x.ID == ogn.EmployeePipelineID)
                    select new
                    {
                        ogn.EmployeePipelineID,
                        pip.AtomPipelineStatusID,
                        ogn.ManagerCompanyUnifiedNumber,
                        ogn.ManagerCompanyName,
                        ogn.AtomEmployeeRangeID,
                        ogn.AtomCustomerGradeID,
                        ogn.ManagerCompanyMainKindName,
                        ogn.ManagerCompanySubKindName,
                        ogn.AtomCityID,
                        ogn.ManagerCompanyLocationAddress,
                        ogn.ManagerCompanyLocationTelephone,
                        ogn.ManagerCompanyLocationStatus,
                        ogn.ManagerContacterName,
                        ogn.ManagerContacterEmail,
                        ogn.ManagerContacterPhone,
                        ogn.ManagerContacterDepartment,
                        ogn.ManagerContacterJobTitle,
                        ogn.ManagerContacterTelephone,
                        ogn.ManagerContacterIsConsent,
                        ogn.ManagerContacterStatus,
                        ogn.AtomRatingKindID,
                        ogn.TeamsRegistrationStatus,
                        ogn.TeamsRegistrationTime,
                        ogn.TeamsLastLeaveTime,
                        ogn.TeamsFirstJoinTime,
                        ogn.TeamsMeetingDuration,
                        ogn.TeamsRole
                    }
                ).ToListAsync();

            var rspObj = new CoEmpPplOgnDbGetManyByEmailListRspMdl
            {
                EmployeePipelineOriginalList = itemList.Select(x => new CoEmpPplOgnDbGetManyByEmailListRspItemMdl
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    AtomPipelineStatus = x.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                    ManagerCompanyUnifiedNumber = x.ManagerCompanyUnifiedNumber?.Trim(),
                    ManagerCompanyName = x.ManagerCompanyName?.Trim(),
                    AtomEmployeeRange = x.AtomEmployeeRangeID.HasValue ? x.AtomEmployeeRangeID.Value.ToEnum<DbAtomEmployeeRangeEnum>() : null,
                    AtomCustomerGrade = x.AtomCustomerGradeID.HasValue ? x.AtomCustomerGradeID.Value.ToEnum<DbAtomCustomerGradeEnum>() : null,
                    ManagerCompanyMainKindName = x.ManagerCompanyMainKindName?.Trim(),
                    ManagerCompanySubKindName = x.ManagerCompanySubKindName?.Trim(),
                    AtomCity = x.AtomCityID.HasValue ? x.AtomCityID.Value.ToEnum<DbAtomCityEnum>() : null,
                    ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress?.Trim(),
                    ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone?.Trim(),
                    ManagerCompanyLocationStatus = x.ManagerCompanyLocationStatus.HasValue ? x.ManagerCompanyLocationStatus.Value.ToEnum<DbAtomManagerCompanyStatusEnum>() : null,
                    ManagerContacterName = x.ManagerContacterName?.Trim(),
                    ManagerContacterEmail = x.ManagerContacterEmail?.Trim(),
                    ManagerContacterPhone = x.ManagerContacterPhone?.Trim(),
                    ManagerContacterDepartment = x.ManagerContacterDepartment?.Trim(),
                    ManagerContacterJobTitle = x.ManagerContacterJobTitle?.Trim(),
                    ManagerContacterTelephone = x.ManagerContacterTelephone?.Trim(),
                    ManagerContacterIsConsent = x.ManagerContacterIsConsent,
                    ManagerContacterStatus = x.ManagerContacterStatus.ToEnum<DbAtomManagerContacterStatusEnum>(),
                    AtomRatingKind = x.AtomRatingKindID.ToEnum<DbAtomManagerContacterRatingKindEnum>(),
                    TeamsRegistrationStatus = x.TeamsRegistrationStatus?.Trim(),
                    TeamsRegistrationTime = x.TeamsRegistrationTime,
                    TeamsLastLeaveTime = x.TeamsLastLeaveTime,
                    TeamsFirstJoinTime = x.TeamsFirstJoinTime,
                    TeamsMeetingDuration = x.TeamsMeetingDuration?.Trim(),
                    TeamsRole = x.TeamsRole?.Trim()
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

    /// <summary>核心-員工-商機原始資料-資料庫-新增</summary>
    public async Task<CoEmpPplOgnDbAddRspMdl> AddAsync(CoEmpPplOgnDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineOriginal()
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                ManagerCompanyUnifiedNumber = reqObj.ManagerCompanyUnifiedNumber?.Trim(),
                ManagerCompanyName = reqObj.ManagerCompanyName?.Trim(),
                AtomEmployeeRangeID = reqObj.AtomEmployeeRange.HasValue ? reqObj.AtomEmployeeRange.Value.ToInt16() : null,
                AtomCustomerGradeID = reqObj.AtomCustomerGrade.HasValue ? reqObj.AtomCustomerGrade.Value.ToInt16() : null,
                ManagerCompanyMainKindName = reqObj.ManagerCompanyMainKindName?.Trim(),
                ManagerCompanySubKindName = reqObj.ManagerCompanySubKindName?.Trim(),
                AtomCityID = reqObj.AtomCity.HasValue ? reqObj.AtomCity.Value.ToInt16() : null,
                ManagerCompanyLocationAddress = reqObj.ManagerCompanyLocationAddress?.Trim(),
                ManagerCompanyLocationTelephone = reqObj.ManagerCompanyLocationTelephone?.Trim(),
                ManagerCompanyLocationStatus = reqObj.ManagerCompanyLocationStatus.HasValue ? reqObj.ManagerCompanyLocationStatus.Value.ToInt16() : null,
                ManagerContacterName = reqObj.ManagerContacterName?.Trim(),
                ManagerContacterEmail = reqObj.ManagerContacterEmail?.Trim(),
                ManagerContacterPhone = reqObj.ManagerContacterPhone?.Trim(),
                ManagerContacterDepartment = reqObj.ManagerContacterDepartment?.Trim(),
                ManagerContacterJobTitle = reqObj.ManagerContacterJobTitle?.Trim(),
                ManagerContacterTelephone = reqObj.ManagerContacterTelephone?.Trim(),
                ManagerContacterIsConsent = reqObj.ManagerContacterIsConsent,
                ManagerContacterStatus = reqObj.ManagerContacterStatus.ToInt16(),
                AtomRatingKindID = reqObj.AtomRatingKind.ToInt16(),
                TeamsRegistrationStatus = reqObj.TeamsRegistrationStatus?.Trim(),
                TeamsRegistrationTime = reqObj.TeamsRegistrationTime,
                TeamsLastLeaveTime = reqObj.TeamsLastLeaveTime,
                TeamsFirstJoinTime = reqObj.TeamsFirstJoinTime,
                TeamsMeetingDuration = reqObj.TeamsMeetingDuration?.Trim(),
                TeamsRole = reqObj.TeamsRole?.Trim(),
                CreateTime = currentTime,
                UpdateTime = currentTime,
            };
            this._mainContext.EmployeePipelineOriginal.Add(item);

            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplOgnDbAddRspMdl()
            {
                EmployeePipelineID = item.EmployeePipelineID
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

    /// <summary>核心-員工-商機原始資料-資料庫-更新</summary>
    public async Task<CoEmpPplOgnDbUpdateRspMdl> UpdateAsync(CoEmpPplOgnDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineOriginal
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(x => x.ManagerCompanyUnifiedNumber, x => !string.IsNullOrEmpty(reqObj.ManagerCompanyUnifiedNumber) ? reqObj.ManagerCompanyUnifiedNumber.Trim() : x.ManagerCompanyUnifiedNumber)
                        .SetProperty(x => x.ManagerCompanyName, x => !string.IsNullOrEmpty(reqObj.ManagerCompanyName) ? reqObj.ManagerCompanyName.Trim() : x.ManagerCompanyName)
                        .SetProperty(x => x.AtomEmployeeRangeID, x => reqObj.AtomEmployeeRange.HasValue ? reqObj.AtomEmployeeRange.Value.ToInt16() : x.AtomEmployeeRangeID)
                        .SetProperty(x => x.AtomCustomerGradeID, x => reqObj.AtomCustomerGrade.HasValue ? reqObj.AtomCustomerGrade.Value.ToInt16() : x.AtomCustomerGradeID)
                        .SetProperty(x => x.ManagerCompanyMainKindName, x => !string.IsNullOrEmpty(reqObj.ManagerCompanyMainKindName) ? reqObj.ManagerCompanyMainKindName.Trim() : x.ManagerCompanyMainKindName)
                        .SetProperty(x => x.ManagerCompanySubKindName, x => !string.IsNullOrEmpty(reqObj.ManagerCompanySubKindName) ? reqObj.ManagerCompanySubKindName.Trim() : x.ManagerCompanySubKindName)
                        .SetProperty(x => x.AtomCityID, x => reqObj.AtomCity.HasValue ? reqObj.AtomCity.Value.ToInt16() : x.AtomCityID)
                        .SetProperty(x => x.ManagerCompanyLocationAddress, x => !string.IsNullOrEmpty(reqObj.ManagerCompanyLocationAddress) ? reqObj.ManagerCompanyLocationAddress.Trim() : x.ManagerCompanyLocationAddress)
                        .SetProperty(x => x.ManagerCompanyLocationTelephone, x => !string.IsNullOrEmpty(reqObj.ManagerCompanyLocationTelephone) ? reqObj.ManagerCompanyLocationTelephone.Trim() : x.ManagerCompanyLocationTelephone)
                        .SetProperty(x => x.ManagerCompanyLocationStatus, x => reqObj.ManagerCompanyLocationStatus.HasValue ? reqObj.ManagerCompanyLocationStatus.Value.ToInt16() : x.ManagerCompanyLocationStatus)
                        .SetProperty(x => x.ManagerContacterName, x => !string.IsNullOrEmpty(reqObj.ManagerContacterName) ? reqObj.ManagerContacterName.Trim() : x.ManagerContacterName)
                        .SetProperty(x => x.ManagerContacterEmail, x => !string.IsNullOrEmpty(reqObj.ManagerContacterEmail) ? reqObj.ManagerContacterEmail.Trim() : x.ManagerContacterEmail)
                        .SetProperty(x => x.ManagerContacterPhone, x => !string.IsNullOrEmpty(reqObj.ManagerContacterPhone) ? reqObj.ManagerContacterPhone.Trim() : x.ManagerContacterPhone)
                        .SetProperty(x => x.ManagerContacterDepartment, x => !string.IsNullOrEmpty(reqObj.ManagerContacterDepartment) ? reqObj.ManagerContacterDepartment.Trim() : x.ManagerContacterDepartment)
                        .SetProperty(x => x.ManagerContacterJobTitle, x => !string.IsNullOrEmpty(reqObj.ManagerContacterJobTitle) ? reqObj.ManagerContacterJobTitle.Trim() : x.ManagerContacterJobTitle)
                        .SetProperty(x => x.ManagerContacterTelephone, x => !string.IsNullOrEmpty(reqObj.ManagerContacterTelephone) ? reqObj.ManagerContacterTelephone.Trim() : x.ManagerContacterTelephone)
                        .SetProperty(x => x.ManagerContacterIsConsent, x => reqObj.ManagerContacterIsConsent.HasValue ? reqObj.ManagerContacterIsConsent.Value : x.ManagerContacterIsConsent)
                        .SetProperty(x => x.ManagerContacterStatus, x => reqObj.ManagerContacterStatus.HasValue ? reqObj.ManagerContacterStatus.Value.ToInt16() : x.ManagerContacterStatus)
                        .SetProperty(x => x.AtomRatingKindID, x => reqObj.AtomRatingKind.HasValue ? reqObj.AtomRatingKind.Value.ToInt16() : x.AtomRatingKindID)
                        .SetProperty(x => x.TeamsRegistrationStatus, x => !string.IsNullOrEmpty(reqObj.TeamsRegistrationStatus) ? reqObj.TeamsRegistrationStatus.Trim() : x.TeamsRegistrationStatus)
                        .SetProperty(x => x.TeamsRegistrationTime, x => reqObj.TeamsRegistrationTime.HasValue ? reqObj.TeamsRegistrationTime.Value : x.TeamsRegistrationTime)
                        .SetProperty(x => x.TeamsLastLeaveTime, x => reqObj.TeamsLastLeaveTime.HasValue ? reqObj.TeamsLastLeaveTime.Value : x.TeamsLastLeaveTime)
                        .SetProperty(x => x.TeamsFirstJoinTime, x => reqObj.TeamsFirstJoinTime.HasValue ? reqObj.TeamsFirstJoinTime.Value : x.TeamsFirstJoinTime)
                        .SetProperty(x => x.TeamsMeetingDuration, x => !string.IsNullOrEmpty(reqObj.TeamsMeetingDuration) ? reqObj.TeamsMeetingDuration.Trim() : x.TeamsMeetingDuration)
                        .SetProperty(x => x.TeamsRole, x => !string.IsNullOrEmpty(reqObj.TeamsRole) ? reqObj.TeamsRole.Trim() : x.TeamsRole)
                        .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplOgnDbUpdateRspMdl()
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

    /// <summary>核心-員工-商機原始資料-資料庫-移除</summary>
    public async Task<CoEmpPplOgnDbRemoveRspMdl> RemoveAsync(CoEmpPplOgnDbRemoveReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineOriginal
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplOgnDbRemoveRspMdl
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

    /// <summary>核心-員工-商機原始資料-資料庫-移除多筆</summary>
    public async Task<CoEmpPplOgnDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplOgnDbRemoveManyReqMdl reqObj)
    {
        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineOriginal
                .Where(x => reqObj.EmployeePipelineIDList.Contains(x.EmployeePipelineID))
                .ExecuteDeleteAsync();

            var rspObj = new CoEmpPplOgnDbRemoveManyRspMdl
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

    /// <summary>核心-員工-商機原始資料-資料庫-新增多筆</summary>
    public async Task<CoEmpPplOgnDbAddManyRspMdl> AddManyAsync(CoEmpPplOgnDbAddManyReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var itemList = reqObj.EmployeePipelineOriginalList
                        .Select(x => new EmployeePipelineOriginal()
                        {
                            EmployeePipelineID = x.EmployeePipelineID,
                            ManagerCompanyUnifiedNumber = x.ManagerCompanyUnifiedNumber?.Trim(),
                            ManagerCompanyName = x.ManagerCompanyName?.Trim(),
                            AtomEmployeeRangeID = x.AtomEmployeeRange.HasValue ? x.AtomEmployeeRange.Value.ToInt16() : null,
                            AtomCustomerGradeID = x.AtomCustomerGrade.HasValue ? x.AtomCustomerGrade.Value.ToInt16() : null,
                            ManagerCompanyMainKindName = x.ManagerCompanyMainKindName?.Trim(),
                            ManagerCompanySubKindName = x.ManagerCompanySubKindName?.Trim(),
                            AtomCityID = x.AtomCity.HasValue ? x.AtomCity.Value.ToInt16() : null,
                            ManagerCompanyLocationAddress = x.ManagerCompanyLocationAddress?.Trim(),
                            ManagerCompanyLocationTelephone = x.ManagerCompanyLocationTelephone?.Trim(),
                            ManagerCompanyLocationStatus = x.ManagerCompanyLocationStatus.HasValue ? x.ManagerCompanyLocationStatus.Value.ToInt16() : null,
                            ManagerContacterName = x.ManagerContacterName?.Trim(),
                            ManagerContacterEmail = x.ManagerContacterEmail?.Trim(),
                            ManagerContacterPhone = x.ManagerContacterPhone?.Trim(),
                            ManagerContacterDepartment = x.ManagerContacterDepartment?.Trim(),
                            ManagerContacterJobTitle = x.ManagerContacterJobTitle?.Trim(),
                            ManagerContacterTelephone = x.ManagerContacterTelephone?.Trim(),
                            ManagerContacterIsConsent = x.ManagerContacterIsConsent,
                            ManagerContacterStatus = x.ManagerContacterStatus.ToInt16(),
                            AtomRatingKindID = x.AtomRatingKind.ToInt16(),
                            TeamsRegistrationStatus = x.TeamsRegistrationStatus,
                            TeamsRegistrationTime = x.TeamsRegistrationTime,
                            TeamsLastLeaveTime = x.TeamsLastLeaveTime,
                            TeamsFirstJoinTime = x.TeamsFirstJoinTime,
                            TeamsMeetingDuration = x.TeamsMeetingDuration?.Trim(),
                            TeamsRole = x.TeamsRole?.Trim(),
                            CreateTime = currentTime,
                            UpdateTime = currentTime,
                        })
                        .ToList();

            this._mainContext.EmployeePipelineOriginal.AddRange(itemList);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplOgnDbAddManyRspMdl()
            {
                IsSuccess = true,
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

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆公司名稱從[管理者後台-基本]</summary>
    public async Task<CoEmpPplOgnDbGetManyMgrComNameFromMbsBscRspMdl> GetManyManagerCompanyNameFromMbsBscAsync(CoEmpPplOgnDbGetManyMgrComNameFromMbsBscReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var managerCompanyNameList = await this._mainContext.EmployeePipelineOriginal
                .AsNoTracking()
                .Where(x =>
                    !string.IsNullOrEmpty(x.ManagerCompanyName) &&
                    (string.IsNullOrEmpty(reqObj.ManagerCompanyName) || x.ManagerCompanyName.Contains(reqObj.ManagerCompanyName.Trim()))
                )
                .Select(x => x.ManagerCompanyName.Trim())
                .Distinct()
                .OrderBy(x => x)
                .Skip(skipRows)
                .Take(takeRows)
                .ToListAsync();

            var rspObj = new CoEmpPplOgnDbGetManyMgrComNameFromMbsBscRspMdl
            {
                ManagerCompanyNameList = managerCompanyNameList
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

    #region ManagerBackSite.Crm.Phone 管理者後台-CRM-活動管理

    /// <summary>核心-員工-商機原始資料-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]</summary>
    public async Task<CoEmpPplOgnDbGetCountFromMbsCrmActRspMdl> GetCountFromMbsCrmActAsync(CoEmpPplOgnDbGetCountFromMbsCrmActReqMdl reqObj)
    {
        try
        {
            var count = await (
                from ep in this._mainContext.EmployeePipeline
                    .Where(ep =>
                        ep.ManagerActivityID.HasValue &&
                        ep.ManagerActivityID.Value == reqObj.ManagerActivityID &&
                        // 商機狀態
                        (reqObj.AtomPipelineStatus.HasValue == false || ep.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16())
                    )
                from epo in this._mainContext.EmployeePipelineOriginal
                    .Where(epo =>
                        epo.EmployeePipelineID == ep.ID &&
                        // 管理者公司名稱
                        (string.IsNullOrEmpty(reqObj.EmployeePipelineOriginalManagerCompanyName) || epo.ManagerCompanyName.Contains(reqObj.EmployeePipelineOriginalManagerCompanyName.Trim())) &&
                        // 窗口姓名
                        (string.IsNullOrEmpty(reqObj.EmployeePipelineOriginalManagerContacterName) || epo.ManagerContacterName.Contains(reqObj.EmployeePipelineOriginalManagerContacterName.Trim())) &&
                        // 窗口Email
                        (string.IsNullOrEmpty(reqObj.EmployeePipelineOriginalManagerContacterEmail) || epo.ManagerContacterEmail.Contains(reqObj.EmployeePipelineOriginalManagerContacterEmail.Trim()))
                    )
                select ep
            )
            .AsNoTracking()
            .CountAsync();

            var rspObj = new CoEmpPplOgnDbGetCountFromMbsCrmActRspMdl
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

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆[管理者後台-CRM-活動管理]</summary>
    public async Task<CoEmpPplOgnDbGetManyFromMbsCrmActRspMdl> GetManyFromMbsCrmActAsync(CoEmpPplOgnDbGetManyFromMbsCrmActReqMdl reqObj)
    {
        var skipRows = (reqObj.PageIndex - 1) * reqObj.PageSize;
        var takeRows = reqObj.PageSize;

        try
        {
            var itemList = await (
                from ep in this._mainContext.EmployeePipeline
                    .Where(ep =>
                        ep.ManagerActivityID.HasValue &&
                        ep.ManagerActivityID.Value == reqObj.ManagerActivityID &&
                        // 商機狀態
                        (reqObj.AtomPipelineStatus.HasValue == false || ep.AtomPipelineStatusID == reqObj.AtomPipelineStatus.Value.ToInt16())
                    )
                from epo in this._mainContext.EmployeePipelineOriginal
                    .Where(epo =>
                        epo.EmployeePipelineID == ep.ID &&
                        // 管理者公司名稱
                        (string.IsNullOrEmpty(reqObj.EmployeePipelineOriginalManagerCompanyName) || epo.ManagerCompanyName.Contains(reqObj.EmployeePipelineOriginalManagerCompanyName.Trim())) &&
                        // 窗口姓名
                        (string.IsNullOrEmpty(reqObj.EmployeePipelineOriginalManagerContacterName) || epo.ManagerContacterName.Contains(reqObj.EmployeePipelineOriginalManagerContacterName.Trim())) &&
                        // 窗口Email
                        (string.IsNullOrEmpty(reqObj.EmployeePipelineOriginalManagerContacterEmail) || epo.ManagerContacterEmail.Contains(reqObj.EmployeePipelineOriginalManagerContacterEmail.Trim()))
                    )
                orderby ep.ID
                select new
                {
                    EmployeePipelineID = ep.ID,
                    ep.AtomPipelineStatusID,
                    epo.ManagerCompanyName,
                    epo.ManagerContacterDepartment,
                    epo.ManagerContacterJobTitle,
                    epo.ManagerContacterName,
                    epo.ManagerContacterEmail,
                    epo.ManagerContacterPhone,
                    epo.ManagerContacterTelephone,
                }
            )
            .AsNoTracking()
            .Skip(skipRows)
            .Take(takeRows)
            .ToListAsync();

            var rspObj = new CoEmpPplOgnDbGetManyFromMbsCrmActRspMdl
            {
                EmployeePipelineOriginalList = itemList.Select(x => new CoEmpPplOgnDbGetManyFromMbsCrmActRspItemMdl
                {
                    EmployeePipelineID = x.EmployeePipelineID,
                    AtomPipelineStatus = x.AtomPipelineStatusID.ToEnum<DbAtomPipelineStatusEnum>(),
                    EmployeePipelineOriginalManagerCompanyName = x.ManagerCompanyName?.Trim(),
                    EmployeePipelineOriginalManagerContacterDepartment = x.ManagerContacterDepartment?.Trim(),
                    EmployeePipelineOriginalManagerContacterJobTitle = x.ManagerContacterJobTitle?.Trim(),
                    EmployeePipelineOriginalManagerContacterName = x.ManagerContacterName?.Trim(),
                    EmployeePipelineOriginalManagerContacterEmail = x.ManagerContacterEmail?.Trim(),
                    EmployeePipelineOriginalManagerContacterPhone = x.ManagerContacterPhone?.Trim(),
                    EmployeePipelineOriginalManagerContacterTelephone = x.ManagerContacterTelephone?.Trim(),
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
