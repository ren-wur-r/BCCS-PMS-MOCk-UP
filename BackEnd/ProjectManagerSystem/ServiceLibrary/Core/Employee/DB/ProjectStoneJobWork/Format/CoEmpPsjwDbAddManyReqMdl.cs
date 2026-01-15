using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-新增多筆-請求模型</summary>
public class CoEmpPsjwDbAddManyReqMdl
{
    public List<CoEmpPsjwDbAddReqMdl> EmployeeProjectStoneJobWorkList { get; set; }
}
