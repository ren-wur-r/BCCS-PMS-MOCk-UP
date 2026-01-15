using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;


/// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員-回應模型</summary>
public class MbsBscLgcGetManyEmployeeProjectMemberRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-專案成員-列表</summary>
    public List<MbsBscLgcGetManyEmployeeProjectMemberRspItemMdl> EmployeeProjectMemberList { get; set; }
}