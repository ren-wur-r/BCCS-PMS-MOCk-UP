using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-資料庫-取得多筆-回應模型</summary>
public class CoEmpPmDbGetManyRspMdl
{
    /// <summary>員工專案成員列表</summary>
    public List<CoEmpPmDbGetManyRspItemMdl> EmployeeProjectMemberList { get; set; }
}