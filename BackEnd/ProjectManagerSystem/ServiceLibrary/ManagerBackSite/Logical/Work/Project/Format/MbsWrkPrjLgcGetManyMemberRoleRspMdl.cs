using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆成員角色-回應模型</summary>
public class MbsWrkPrjLgcGetManyMemberRoleRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案成員列表</summary>
    public List<MbsWrkPrjLgcGetManyMemberRoleRspItemMdl> EmployeeProjectMemberList { get; set; }
}
