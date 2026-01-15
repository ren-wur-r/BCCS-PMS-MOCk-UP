using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得窗口-請求模型</summary>
public class MbsBscLgcGetManagerContacterReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }
}
