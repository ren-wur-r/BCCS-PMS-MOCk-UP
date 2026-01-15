using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-系統-員工]-回應模型</summary>
public class CoEmpInfDbGetManyFromMbsSysEmpRspMdl
{
    /// <summary>員工-列表</summary>
    public List<CoEmpInfDbGetManyFromMbsSysEmpRspItemMdl> EmployeeList { get; set; }
}