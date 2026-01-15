using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommonLibrary.CmnSwagger.SchemaFilter;

public class CmnSwaggerEnumSchemaFilter : ISchemaFilter
{
    private readonly XDocument _xmlDoc;

    public CmnSwaggerEnumSchemaFilter(string xmlPath)
    {
        if (File.Exists(xmlPath))
        {
            this._xmlDoc = XDocument.Load(xmlPath);
        }
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (this._xmlDoc == null
            || schema.Enum == null
            || schema.Enum.Count == 0
            || context.Type == null
            || context.Type.IsEnum == false)
        {
            return;
        }

        var fullTypeName = context.Type.FullName;
        var isExist = this._xmlDoc.Descendants("member").Any(x => x.Attribute("name").Value.Contains(fullTypeName));
        if (isExist == false)
        {
            return;
        }

        var sb = new StringBuilder();
        foreach (var item in schema.Enum.Cast<OpenApiInteger>())
        {
            var enumMemberName = Enum.GetName(context.Type, item.Value);
            var xmlTagName = $"F:{fullTypeName}.{enumMemberName}";
            var xmlElementMember = this._xmlDoc
                .Descendants("member")
                .FirstOrDefault(m => m.Attribute("name").Value == xmlTagName);
            if (xmlElementMember == default)
            {
                continue;
            }

            if (sb.Length == 0)
            {
                sb.AppendLine("<p>Members:</p>");
                sb.AppendLine("<ul>");
            }

            var xmlElementSummary = xmlElementMember
                .Descendants("summary")
                .FirstOrDefault();

            sb.AppendLine($"<li><i>{item.Value}</i> - {enumMemberName} - {(xmlElementSummary == default ? string.Empty : xmlElementSummary.Value.Trim())}</li>");
        }
        sb.AppendLine("</ul>");

        schema.Description += sb.ToString();
    }
}
