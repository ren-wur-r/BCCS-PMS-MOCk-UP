using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-下載eDM範本-回應模型</summary>
public class MbsCrmActLgcDownloadEdmTemplateRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>本地絕對檔名</summary>
    public string LocalAbsoluteFileName { get; set; }

    /// <summary>檔案類型</summary>
    public string ContentType { get; set; }

    /// <summary>檔案名稱</summary>
    public string FileName { get; set; }
}