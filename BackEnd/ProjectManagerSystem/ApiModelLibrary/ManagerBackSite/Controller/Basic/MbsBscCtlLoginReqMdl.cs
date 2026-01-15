using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

public class MbsBscCtlLoginReqMdl
{
    /// <summary>員工-帳號</summary>
    [Required]
    [JsonPropertyName("a")]
    public string EmployeeAccount { get; set; }

    /// <summary>員工-密碼</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeePassword { get; set; }

}
