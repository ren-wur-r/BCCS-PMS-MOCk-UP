using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

/// <summary>管理者後台-系統設定-地區-取得多筆-回應模型</summary>
public class MbsSysRgnLgcGetManyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>地區列表</summary>
    public List<MbsSysRgnLgcGetManyItemRspMdl> ManagerRegionList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}