namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Format;

/// <summary>核心-管理者-活動問卷回答者-資料庫-取得筆數從[管理者活動ID]-請求模型</summary>
public class CoMgrAsaDbGetCountFromActivityIdReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動問卷回答者-公司名稱</summary>
    public string ManagerActivitySurveyAnswererCompanyName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口姓名</summary>
    public string ManagerActivitySurveyAnswererContacterName { get; set; }

    /// <summary>管理者活動問卷回答者-窗口信箱</summary>
    public string ManagerActivitySurveyAnswererContacterEmail { get; set; }
}