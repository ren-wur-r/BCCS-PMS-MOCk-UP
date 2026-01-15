using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Department.Format;

/// <summary>核心-管理者-部門-資料庫-取得多筆-請求模型</summary>
public class CoMgrDptDbGetManyReqMdl
{
    /// <summary>管理者部門ID列表</summary>
    public List<int> ManagerDepartmentIdList { get; set; }

}