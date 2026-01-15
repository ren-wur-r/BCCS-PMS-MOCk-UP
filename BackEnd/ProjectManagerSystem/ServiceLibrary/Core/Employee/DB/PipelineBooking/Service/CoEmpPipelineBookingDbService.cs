using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnInt16.Extension;
using DataModelLibrary.Database.AtomEmployeePipelineBooking;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.PipelineBooking.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineBooking.Service;

/// <summary>核心-員工-商機Booking-資料庫服務</summary>
public class CoEmpPipelineBookingDbService : ICoEmpPipelineBookingDbService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmpPipelineBookingDbService> _logger;

    /// <summary>主DB</summary>
    private readonly ProjectManagerMainContext _mainContext;

    /// <summary>建構式</summary>
    public CoEmpPipelineBookingDbService(
        ILogger<CoEmpPipelineBookingDbService> logger,
        ProjectManagerMainContext mainContext)
    {
        this._logger = logger;
        this._mainContext = mainContext;
    }

    /// <summary>核心-員工-商機Booking-資料庫-取得</summary>
    public async Task<CoEmpPplBkgDbGetRspMdl> GetAsync(CoEmpPplBkgDbGetReqMdl reqObj)
    {
        try
        {
            var item = await this._mainContext.EmployeePipelineBooking
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == reqObj.EmployeePipelineBookingID);

            if (item == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return default;
            }

            var rspObj = new CoEmpPplBkgDbGetRspMdl
            {
                EmployeePipelineBookingID = item.ID,
                EmployeePipelineID = item.EmployeePipelineID,
                ManagerProductID = item.ManagerProductID,
                ManagerProductSpecificationID = item.ManagerProductSpecificationID,
                EmployeePipelineBookingContent = item.Content,
                EmployeePipelineBookingRemark = item.Remark,
                EmployeePipelineBookingStatus = item.Status.ToEnum<DbAtomEmployeePipelineBookingStatusEnum>(),
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

    /// <summary>核心-員工-商機Booking-資料庫-取得多筆</summary>
    public async Task<CoEmpPplBkgDbGetManyRspMdl> GetManyAsync(CoEmpPplBkgDbGetManyReqMdl reqObj)
    {
        try
        {
            var itemList = await this._mainContext.EmployeePipelineBooking
                .AsNoTracking()
                .Where(x => x.EmployeePipelineID == reqObj.EmployeePipelineID)
                .Select(x => new
                {
                    x.ID,
                    x.EmployeePipelineID,
                    x.ManagerProductID,
                    x.ManagerProductSpecificationID,
                    x.Content,
                    x.Remark,
                    x.Status
                })
                .OrderByDescending(x => x.ID)
                .ToListAsync();

            var rspObj = new CoEmpPplBkgDbGetManyRspMdl
            {
                EmployeePipelineBookingList = itemList.Select(x => new CoEmpPplBkgDbGetManyRspItemMdl
                {
                    EmployeePipelineBookingID = x.ID,
                    EmployeePipelineID = x.EmployeePipelineID,
                    ManagerProductID = x.ManagerProductID,
                    ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                    EmployeePipelineBookingContent = x.Content,
                    EmployeePipelineBookingRemark = x.Remark,
                    EmployeePipelineBookingStatus = x.Status.ToEnum<DbAtomEmployeePipelineBookingStatusEnum>()
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

    /// <summary>核心-員工-商機Booking-資料庫-新增</summary>
    public async Task<CoEmpPplBkgDbAddRspMdl> AddAsync(CoEmpPplBkgDbAddReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var item = new EmployeePipelineBooking
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                ManagerProductID = reqObj.ManagerProductID,
                ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
                Content = reqObj.EmployeePipelineBookingContent,
                Remark = reqObj.EmployeePipelineBookingRemark,
                Status = reqObj.EmployeePipelineBookingStatus.ToInt16(),
                CreateTime = currentTime,
                UpdateTime = currentTime
            };

            this._mainContext.EmployeePipelineBooking.Add(item);
            await this._mainContext.SaveChangesAsync();

            var rspObj = new CoEmpPplBkgDbAddRspMdl
            {
                EmployeePipelineBookingID = item.ID
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

    /// <summary>核心-員工-商機Booking-資料庫-更新</summary>
    public async Task<CoEmpPplBkgDbUpdateRspMdl> UpdateAsync(CoEmpPplBkgDbUpdateReqMdl reqObj)
    {
        var currentTime = DateTimeOffset.UtcNow;

        try
        {
            var affectedCount = await this._mainContext.EmployeePipelineBooking
                .Where(x => x.ID == reqObj.EmployeePipelineBookingID)
                .ExecuteUpdateAsync(setter =>
                    setter
                    .SetProperty(x => x.ManagerProductID, x => reqObj.ManagerProductID.HasValue ? reqObj.ManagerProductID.Value : x.ManagerProductID)
                    .SetProperty(x => x.ManagerProductSpecificationID, x => reqObj.ManagerProductSpecificationID.HasValue ? reqObj.ManagerProductSpecificationID.Value : x.ManagerProductSpecificationID)
                    .SetProperty(x => x.Content, x => !string.IsNullOrEmpty(reqObj.EmployeePipelineBookingContent) ? reqObj.EmployeePipelineBookingContent.Trim() : x.Content)
                    .SetProperty(x => x.Remark, x => !string.IsNullOrEmpty(reqObj.EmployeePipelineBookingRemark) ? reqObj.EmployeePipelineBookingRemark.Trim() : x.Remark)
                    .SetProperty(x => x.Status, x => reqObj.EmployeePipelineBookingStatus.HasValue ? reqObj.EmployeePipelineBookingStatus.Value.ToInt16() : x.Status)
                    .SetProperty(x => x.UpdateTime, currentTime));

            var rspObj = new CoEmpPplBkgDbUpdateRspMdl
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
