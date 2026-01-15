using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-取得多筆員工ID-回應模型</summary>
public class CoEmpPmDbGetManyEmployeeIdRspMdl
{
    /// <summary>員工專案成員-列表</summary>
    public List<CoEmpPmDbGetManyEmployeeIdRspItemMdl> EmployeeProjectMemberList { get; set; }
}