using System.Data;
using System.Text.Json.Serialization;
using CommonLibrary.CmnJson;

namespace CommonLibrary.CmnExcel.Format;

/// <summary>通用-Excel-讀取Xls依檔案-回應模型</summary>
public class CmnExcelReadXlsByFileRspMdl
{
    /// <summary>資料表</summary>
    [JsonConverter(typeof(CmnDataTableJsonConverter))]
    public DataTable DataTable { get; set; }
}
