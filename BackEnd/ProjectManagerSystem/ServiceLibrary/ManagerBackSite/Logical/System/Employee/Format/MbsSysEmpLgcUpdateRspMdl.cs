using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

/// <summary>管理者後台-系統設定-員工-更新-回應模型</summary>
public class MbsSysEmpLgcUpdateRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }
}