using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-取得多筆-回應項目模型</summary>
public class CoEmpPwDbGetManyRspItemMdl
{
    /// <summary>員工專案工作計劃書-網址</summary>
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案工作計劃書-是否最新</summary>
    public bool EmployeeProjectWorkIsNewest { get; set; }

    /// <summary>員工專案工作計劃書-建立時間</summary>
    public DateTimeOffset EmployeeProjectWorkCreateTime { get; set; }
}