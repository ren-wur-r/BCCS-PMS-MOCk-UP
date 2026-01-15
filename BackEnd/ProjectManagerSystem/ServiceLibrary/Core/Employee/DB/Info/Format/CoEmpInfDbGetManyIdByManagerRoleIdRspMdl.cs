using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[員工ID]依[管理者角色ID]-回應模型</summary>
public class CoEmpInfDbGetManyIdByManagerRoleIdRspMdl
{
    /// <summary>員工ID列表</summary>
    public List<int> EmployeeIdList { get; set; }
}