using System.Data;
using System.Text.Json.Serialization;
using CommonLibrary.CmnJson;

namespace CommonLibrary.CmnExcel.Format;
public class CmnExcelCreateReqItemMdl
{
    /// <summary>多個資料表</summary>
    [JsonConverter(typeof(CmnDataTableJsonConverter))]
    public DataTable DataTable { get; set; }
}
