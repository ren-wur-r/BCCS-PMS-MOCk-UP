using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-匯入eDM-請求模型</summary>
public class MbsCrmActCtlImportEdmReqMdl
{
    /// <summary>管理者登入令牌</summary>
    [Required]
    [FromForm(Name = "aa")]
    public string EmployeeLoginToken { get; set; }

    /// <summary>管理者活動ID</summary>
    [Required]
    [FromForm(Name = "a")]
    public int ManagerActivityID { get; set; }

    /// <summary>匯入的eDM檔案</summary>
    [Required]
    [FromForm(Name = "b")]
    public IFormFile EdmFile { get; set; }
}
