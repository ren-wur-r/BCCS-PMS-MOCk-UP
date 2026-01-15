using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-取得多筆ID從[排程-計時器]-回應模型</summary>
public class CoEmpPrjDbGetManyIdFromSchTmrRspMdl
{
    /// <summary>員工專案ID列表</summary>
    public List<CoEmpPrjDbGetManyIdFromSchTmrRspItemMdl> EmployeeProjectList { get; set; }
}
