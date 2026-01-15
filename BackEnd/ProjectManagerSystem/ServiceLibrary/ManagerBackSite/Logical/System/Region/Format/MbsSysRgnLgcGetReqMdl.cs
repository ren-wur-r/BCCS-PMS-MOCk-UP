using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

/// <summary>管理者後台-系統設定-地區-取得-請求模型</summary>
public class MbsSysRgnLgcGetReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-地區-ID</summary>
    public int ManagerRegionID { get; set; }
}