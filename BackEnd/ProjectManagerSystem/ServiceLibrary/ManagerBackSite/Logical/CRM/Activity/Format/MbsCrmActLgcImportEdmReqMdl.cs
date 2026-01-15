using Microsoft.AspNetCore.Http;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-匯入eDM-請求模型</summary>
public class MbsCrmActLgcImportEdmReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>匯入的eDM檔案</summary>
    public IFormFile EdmFile { get; set; }
}