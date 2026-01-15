using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-取得多筆從[管理者後台-工作-工作紀錄]-回應模型</summary>
public class CoEmpPsjwDbGetManyFromMbsWrkJobRecRspMdl
{
    /// <summary>員工-專案里程碑工項工作-列表</summary>
    public List<CoEmpPsjwDbGetManyFromMbsWrkJobRecRspItemMdl> EmployeeProjectStoneJobWorkList { get; set; }
}