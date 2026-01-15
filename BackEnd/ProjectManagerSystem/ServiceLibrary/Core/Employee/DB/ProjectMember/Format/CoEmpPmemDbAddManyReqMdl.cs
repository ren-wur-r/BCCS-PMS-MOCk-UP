using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-資料庫-新增多筆-請求模型</summary>
public class CoEmpPmemDbAddManyReqMdl
{
    /// <summary>員工專案成員列表</summary>
    public List<CoEmpPmemDbAddManyReqItemMdl> EmployeeProjectMemberList { get; set; }
}
