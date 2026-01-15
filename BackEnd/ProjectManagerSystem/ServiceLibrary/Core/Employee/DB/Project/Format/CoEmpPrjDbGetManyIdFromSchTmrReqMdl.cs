using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.Project.Format;

public class CoEmpPrjDbGetManyIdFromSchTmrReqMdl
{
    /// <summary>員工專案狀態-列表</summary>
    public List<DbAtomEmployeeProjectStatusEnum> AtomEmployeeProjectStatusList { get; set; }
}