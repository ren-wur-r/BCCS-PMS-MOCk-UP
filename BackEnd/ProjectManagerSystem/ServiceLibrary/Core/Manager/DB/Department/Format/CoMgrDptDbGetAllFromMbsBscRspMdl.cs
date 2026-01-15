using System.Collections.Generic;
namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得全部從[管理者後台-基本-部門]-回應模型</summary>
public class CoMgrDptDbGetAllFromMbsBscRspMdl
{
    /// <summary>管理者部門-列表</summary>
    public List<CoMgrDptDbGetAllFromMbsBscRspItemMdl> ManagerDepartmentList { get; set; }
}