using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工項]-回應模型</summary>
public class CoEmpPsjwDbGetManyFromMbsWrkJobRspMdl
{
    /// <summary>專案里程碑工項工作列表</summary>
    public List<CoEmpPsjwDbGetManyFromMbsWrkJobRspItemJobMdl> EmployeeProjectStoneJobList { get; set; }
}