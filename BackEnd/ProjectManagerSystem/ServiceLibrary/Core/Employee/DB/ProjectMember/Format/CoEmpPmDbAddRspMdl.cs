using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-資料庫-新增-回應模型</summary>
public class CoEmpPmDbAddRspMdl
{
    /// <summary>員工-專案成員-建立時間</summary>
    public DateTimeOffset EmployeeProjectMemberCreateTime { get; set; }

    /// <summary>員工-專案成員-更新時間</summary>
    public DateTimeOffset EmployeeProjectMemberUpdateTime { get; set; }
}