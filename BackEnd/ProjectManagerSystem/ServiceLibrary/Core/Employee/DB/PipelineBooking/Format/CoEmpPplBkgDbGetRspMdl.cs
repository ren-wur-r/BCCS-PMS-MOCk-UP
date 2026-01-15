using DataModelLibrary.Database.AtomEmployeePipelineBooking;

namespace ServiceLibrary.Core.Employee.DB.PipelineBooking.Format;

/// <summary>核心-員工-商機Booking-資料庫-取得-回應</summary>
public class CoEmpPplBkgDbGetRspMdl
{
    /// <summary>商機BookingID</summary>
    public int EmployeePipelineBookingID { get; set; }

    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>需求內容</summary>
    public string EmployeePipelineBookingContent { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineBookingRemark { get; set; }

    /// <summary>狀態</summary>
    public DbAtomEmployeePipelineBookingStatusEnum EmployeePipelineBookingStatus { get; set; }
}
