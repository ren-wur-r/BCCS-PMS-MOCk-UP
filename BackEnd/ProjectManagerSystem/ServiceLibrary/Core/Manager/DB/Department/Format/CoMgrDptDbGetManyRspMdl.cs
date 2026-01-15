using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得多筆-回應模型</summary>
public class CoMgrDptDbGetManyRspMdl
{
    /// <summary>管理者-部門-列表</summary>
    public List<CoMgrDptDbGetManyRspItemMdl> ManagerDepartmentList { get; set; }
}