using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-取得多筆-回應項目模型</summary>
public class CoEmpPcDbGetManyRspItemMdl
{
    /// <summary>員工專案契約-網址</summary>
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案契約-是否最新</summary>
    public bool EmployeeProjectContractIsNewest { get; set; }

    /// <summary>員工專案契約-建立時間</summary>
    public DateTimeOffset EmployeeProjectContractCreateTime { get; set; }
}
