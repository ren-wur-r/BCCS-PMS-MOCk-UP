using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-檢查多筆存在-請求模型</summary>
public class CoEmpInfDbCheckManyExistReqMdl
{
    /// <summary>員工ID列表</summary>
    public List<int> EmployeeIdList { get; set; }
}
