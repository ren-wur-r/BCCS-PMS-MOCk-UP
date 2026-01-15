using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得多筆-請求模型</summary>
public class CoMgrRolDbGetManyReqMdl
{
    /// <summary>管理者-角色-ID列表</summary>
    public List<int> ManagerRoleIdList { get; set; }
}