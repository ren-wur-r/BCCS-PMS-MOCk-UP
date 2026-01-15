namespace ApiModelLibrary.ManagerBackSite.Controller.Dashboard;

/// <summary>管理者後台-儀表板-控制器-取得資訊-請求模型</summary>
public class MbsDashboardCtlGetInfoReqMdl
{
    /// <summary>登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

    /// <summary>區域 ID (過濾用)</summary>
    public int? RegionID { get; set; }
}
