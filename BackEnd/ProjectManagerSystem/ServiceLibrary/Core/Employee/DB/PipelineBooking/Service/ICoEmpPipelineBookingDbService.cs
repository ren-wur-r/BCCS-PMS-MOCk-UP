using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineBooking.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineBooking.Service;

/// <summary>核心-員工-商機Booking-資料庫服務介面</summary>
public interface ICoEmpPipelineBookingDbService
{
    /// <summary>核心-員工-商機Booking-資料庫-取得</summary>
    public Task<CoEmpPplBkgDbGetRspMdl> GetAsync(CoEmpPplBkgDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機Booking-資料庫-取得多筆</summary>
    public Task<CoEmpPplBkgDbGetManyRspMdl> GetManyAsync(CoEmpPplBkgDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機Booking-資料庫-新增</summary>
    public Task<CoEmpPplBkgDbAddRspMdl> AddAsync(CoEmpPplBkgDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機Booking-資料庫-更新</summary>
    public Task<CoEmpPplBkgDbUpdateRspMdl> UpdateAsync(CoEmpPplBkgDbUpdateReqMdl reqObj);
}
