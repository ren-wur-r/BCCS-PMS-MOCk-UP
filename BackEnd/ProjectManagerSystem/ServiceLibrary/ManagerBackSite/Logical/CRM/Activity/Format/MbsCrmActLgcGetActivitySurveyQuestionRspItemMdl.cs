using System.Collections.Generic;
namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得活動問卷問題-回應項目模型</summary>
public class MbsCrmActLgcGetActivitySurveyQuestionRspItemMdl
{
    /// <summary>管理者活動問卷問題-問題ID</summary>
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>管理者活動問卷問題-問題類型ID</summary>
    public DataModelLibrary.Database.ManagerActivitySurveyQuestion.DbManagerActivitySurveyQuestionKindEnum ManagerActivitySurveyQuestionKind { get; set; }

    /// <summary>管理者活動問卷問題-問題標題</summary>
    public string ManagerActivitySurveyQuestionTitle { get; set; }

    /// <summary>管理者活動問卷問題-問題內容</summary>
    public string ManagerActivitySurveyQuestionContent { get; set; }

    /// <summary>是否為其他</summary>
    public bool IsOther { get; set; }

    /// <summary>管理者活動問卷問題-問題排序</summary>
    public short ManagerActivitySurveyQuestionSort { get; set; }

    /// <summary>管理者活動問卷問題-問題項目列表</summary>
    public List<MbsCrmActLgcGetActivitySurveyQuestionRspItemQuestionItemMdl> ManagerActivitySurveyQuestionItemList { get; set; }
}