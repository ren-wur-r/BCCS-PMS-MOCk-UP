using System.Data;
using System.Text.Json.Serialization;
using CommonLibrary.CmnJson;

namespace CommonLibrary.CmnExcel.Format;

/// <summary>通用-Excel-讀取Xls依IFormFile-回應模型</summary>
public class CmnExcelReadXlsByIFormFilRspMdl
{
    /// <summary>資料表</summary>
    [JsonConverter(typeof(CmnDataTableJsonConverter))]
    public DataTable DataTable { get; set; }
}
