using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

/// <summary>管理者後台-系統設定-地區-取得多筆-項目-回應模型</summary>
public class MbsSysRgnLgcGetManyItemRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者－地區-ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>管理者－地區-名稱</summary>
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    public bool ManagerRegionIsEnable { get; set; }
}