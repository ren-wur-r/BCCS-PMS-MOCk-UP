using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoEmpInfDbGetManyFromMbsBscRspMdl
{
    /// <summary>員工列表</summary>
    public List<CoEmpInfDbGetManyFromMbsBscRspItemMdl> EmployeeList { get; set; }
}