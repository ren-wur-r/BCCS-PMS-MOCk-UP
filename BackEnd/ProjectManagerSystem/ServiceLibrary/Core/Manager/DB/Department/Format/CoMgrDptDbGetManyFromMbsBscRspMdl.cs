using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoMgrDptDbGetManyFromMbsBscRspMdl
{
    /// <summary>部門-列表</summary>
    public List<CoMgrDptDbGetManyFromMbsBscRspItemMdl> ManagerDepartmentList { get; set; }
}