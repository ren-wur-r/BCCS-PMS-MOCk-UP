using System.Linq;
using System.Reflection;

namespace DataModelLibrary;

/// <summary>資料模型-函式庫-專案</summary>
public class DataModelLibraryProject
{
    /// <summary>版本</summary>
    public string Version { get; private set; }

    /// <summary>取得專案名稱</summary>
    public string ProjectName { get; private set; }

    /// <summary>建構式</summary>
    public DataModelLibraryProject()
    {
        this.Version = this.GetVersion();
        this.ProjectName = this.GetProjectName();
    }

    /// <summary>取得版本號</summary>
    private string GetVersion()
    {
        var attr = this.GetType().Assembly.CustomAttributes
            .FirstOrDefault(x => x.AttributeType == typeof(AssemblyInformationalVersionAttribute));
        if (attr == default
            || attr.ConstructorArguments.Count == 0)
        {
            return "Version Error";
        }

        return attr.ConstructorArguments[0].Value?.ToString() ?? "Version Error";
    }

    /// <summary>取得專案名稱</summary>
    private string GetProjectName()
    {
        var name = this.GetType().Assembly.GetName().Name;
        if (string.IsNullOrWhiteSpace(name) == true)
        {
            return "Project Name Error";
        }
        return name;
    }
}