using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

/// <summary>核心-員工-專案里程碑工項工作檔案-資料庫-新增多筆-請求模型</summary>
public class CoEmpPsjwfDbAddManyReqMdl
{
    public List<CoEmpPsjwfDbAddReqMdl> EmployeeProjectStoneJobWorkFileList { get; set; }
}
