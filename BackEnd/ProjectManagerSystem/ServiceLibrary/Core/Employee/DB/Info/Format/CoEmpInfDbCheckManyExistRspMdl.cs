using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-檢查多筆存在-回應模型</summary>
public class CoEmpInfDbCheckManyExistRspMdl
{
    /// <summary>員工列表</summary>
    public List<CoEmpInfDbCheckManyExistRspItemMdl> EmployeeList { get; set; }
}
