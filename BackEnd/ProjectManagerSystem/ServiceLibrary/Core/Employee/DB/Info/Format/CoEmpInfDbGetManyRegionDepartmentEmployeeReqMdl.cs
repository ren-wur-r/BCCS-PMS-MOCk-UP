using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[地區][部門][員工]<-請求模型</summary>
public class CoEmpInfDbGetManyRegionDepartmentEmployeeReqMdl
{
    /// <summary>員工ID列表</summary>
    public List<int> EmployeeIdList { get; set; }
}
