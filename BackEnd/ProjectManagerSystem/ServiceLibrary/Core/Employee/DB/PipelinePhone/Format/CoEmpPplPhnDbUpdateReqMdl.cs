using System;

namespace ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;

/// <summary>核心-員工-商機電銷-資料庫-更新-請求</summary>
public class CoEmpPplPhnDbUpdateReqMdl
{
    /// <summary>商機電銷紀錄ID</summary>
    public int EmployeePipelinePhoneID { get; set; }

    /// <summary>商機電銷紀錄-紀錄時間</summary>
    public DateTimeOffset? EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>窗口ID</summary>
    public int? ManagerContacterID { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    public string EmployeePipelinePhoneRemark { get; set; }
}
