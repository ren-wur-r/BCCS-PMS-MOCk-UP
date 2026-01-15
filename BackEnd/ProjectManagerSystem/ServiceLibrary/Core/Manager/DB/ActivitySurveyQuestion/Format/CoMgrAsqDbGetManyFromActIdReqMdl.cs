namespace ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;

/// <summary>核心-管理者-活動問卷問題-資料庫-取得多筆從[管理者活動ID]-請求模型</summary>
public class CoMgrAsqDbGetManyFromActIdReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }
}