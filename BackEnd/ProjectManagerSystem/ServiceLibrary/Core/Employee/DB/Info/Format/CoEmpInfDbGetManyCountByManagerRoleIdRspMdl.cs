using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[筆數]從[管理者角色ID]-回應模型</summary>
public class CoEmpInfDbGetManyCountByManagerRoleIdRspMdl
{
    /// <summary>管理者-角色-列表</summary>
    public List<CoEmpInfDbGetManyCountByManagerRoleIdRspItemMdl> ManagerRoleList { get; set; }
}
