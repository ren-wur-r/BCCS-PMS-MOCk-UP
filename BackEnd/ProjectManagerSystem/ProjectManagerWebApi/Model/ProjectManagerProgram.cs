using System;
using System.Linq;
using System.Reflection;

namespace ProjectManagerWebApi.Model;

/// <summary>專案管理-程式</summary>
public class ProjectManagerProgram
{
    /// <summary>取得專案名稱</summary>
    public string ProjectName { get; protected set; }

    /// <summary>版本</summary>
    public string Version { get; protected set; }

    /// <summary>程式ID(每次new都不一樣)</summary>
    public string ProgramID { get; protected set; }

    /// <summary>建構式</summary>
    public ProjectManagerProgram()
    {
        this.ProjectName = this.GetProjectName();
        this.Version = this.GetVersion();
        this.ProgramID = this.GetProgramID();
    }

    /// <summary>取得專案名稱</summary>
    protected virtual string GetProjectName()
    {
        var name = this.GetType().Assembly.GetName().Name;
        if (string.IsNullOrWhiteSpace(name) == true)
        {
            return "Project Name Error";
        }
        return name;
    }

    /// <summary>取得版本號</summary>
    protected virtual string GetVersion()
    {
        var attr = this.GetType().Assembly.CustomAttributes
            .FirstOrDefault(x => x.AttributeType == typeof(AssemblyInformationalVersionAttribute));
        if (attr == default
            || attr.ConstructorArguments.Count == 0)
        {
            return "Version Error";
        }

        return attr.ConstructorArguments[0].Value.ToString();
    }

    /// <summary>取得程式ID</summary>
    protected virtual string GetProgramID()
    {
        var id = $"{DateTimeOffset.UtcNow.ToString("MMddHHmmssfffff")}_{Guid.NewGuid().ToString("N").Substring(26, 6)}"; ;
        return id;
    }

}
