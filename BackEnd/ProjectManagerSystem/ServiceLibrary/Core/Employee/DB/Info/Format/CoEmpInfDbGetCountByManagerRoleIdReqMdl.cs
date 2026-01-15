namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得[筆數]依[管理者角色ID]-請求模型</summary>
public class CoEmpInfDbGetCountByManagerRoleIdReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }
}