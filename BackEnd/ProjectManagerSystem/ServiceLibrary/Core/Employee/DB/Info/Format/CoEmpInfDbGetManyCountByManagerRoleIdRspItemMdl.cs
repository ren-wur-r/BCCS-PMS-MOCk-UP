namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[筆數]從[管理者角色ID]-回應項目模型</summary>
public class CoEmpInfDbGetManyCountByManagerRoleIdRspItemMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>筆數</summary>
    public int Count { get; set; }
}
