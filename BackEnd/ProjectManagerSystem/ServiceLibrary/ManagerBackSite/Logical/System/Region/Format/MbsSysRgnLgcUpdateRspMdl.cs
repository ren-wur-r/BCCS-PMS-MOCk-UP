using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

/// <summary>管理者後台-系統設定-地區-更新-回應模型</summary>
public class MbsSysRgnLgcUpdateRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }
}