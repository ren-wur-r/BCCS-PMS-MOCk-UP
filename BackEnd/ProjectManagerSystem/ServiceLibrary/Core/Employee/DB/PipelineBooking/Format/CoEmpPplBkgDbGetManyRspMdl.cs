using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineBooking.Format;

/// <summary>核心-員工-商機Booking-資料庫-取得多筆-回應</summary>
public class CoEmpPplBkgDbGetManyRspMdl
{
    /// <summary>商機Booking清單</summary>
    public List<CoEmpPplBkgDbGetManyRspItemMdl> EmployeePipelineBookingList { get; set; }
}
