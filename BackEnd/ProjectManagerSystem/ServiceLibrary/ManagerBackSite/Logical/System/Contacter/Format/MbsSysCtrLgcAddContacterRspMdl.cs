using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-新增窗口-回應模型</summary>
public class MbsSysCtrLgcAddContacterRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }
}
