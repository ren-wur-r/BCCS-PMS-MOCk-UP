using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommonLibrary.CmnJson;

public class CmnDataTableJsonConverter : JsonConverter<DataTable>
{
    public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 反序列化為 List<List<string>>
        var rowlist = JsonSerializer.Deserialize<List<List<string>>>(ref reader, options);
        var table = new DataTable();

        if (rowlist == null || rowlist.Count == 0)
            return table;

        // 以第一列的欄位數決定 DataTable 欄位數
        var columnCount = rowlist[0].Count;
        for (var i = 0; i < columnCount; i++)
        {
            table.Columns.Add($"Column{i}");
        }

        // 填入資料
        foreach (var rowItem in rowlist)
        {
            var row = table.NewRow();
            for (var colIndex = 0; colIndex < columnCount; colIndex++)
            {
                row[colIndex] = rowItem.Count > colIndex ? rowItem[colIndex] : null;
            }
            table.Rows.Add(row);
        }

        return table;
    }

    public override void Write(Utf8JsonWriter writer, DataTable value, JsonSerializerOptions options)
    {
        var list = value.AsEnumerable()
            .Select(row => value.Columns
                .Cast<DataColumn>()
                .Select(col => row[col]?.ToString() ?? string.Empty)
                .ToList())
            .ToList();

        JsonSerializer.Serialize(writer, list, options);
    }
}
