using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-新增-回應模型</summary>
public class CoEmpPcDbAddRspMdl
{
    /// <summary>員工專案契約ID</summary>
    public int EmployeeProjectContractID { get; set; }

    /// <summary>員工專案契約-建立時間</summary>
    public DateTimeOffset EmployeeProjectContractCreateTime { get; set; }
}