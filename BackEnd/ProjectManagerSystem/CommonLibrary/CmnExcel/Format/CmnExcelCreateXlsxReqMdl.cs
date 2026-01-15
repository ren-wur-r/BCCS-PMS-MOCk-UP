using System.Data;
using System.Text.Json.Serialization;
using CommonLibrary.CmnJson;

namespace CommonLibrary.CmnExcel.Format;

/// <summary>通用-Excel-建立Xlsx-請求模型</summary>
public class CmnExcelCreateXlsxReqMdl
{
    /// <summary>絕對檔名</summary>
    public string AbsoluteFileName { get; set; }

    /// <summary>多個資料表</summary>
    [JsonConverter(typeof(CmnDataTableJsonConverter))]
    public DataTable DataTable { get; set; }

}
