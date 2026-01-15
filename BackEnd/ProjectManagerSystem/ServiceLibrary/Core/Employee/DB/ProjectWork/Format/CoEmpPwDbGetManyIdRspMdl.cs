using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

public class CoEmpPwDbGetManyIdRspMdl
{
    /// <summary>員工專案工作計劃書列表</summary>
    public List<CoEmpPwDbGetManyIdRspItemMdl> EmployeeProjectWorkList { get; set; }
}