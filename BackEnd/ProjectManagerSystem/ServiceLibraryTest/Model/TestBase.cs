using System;
using System.IO;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;

namespace ServiceLibraryTest.Model;

/// <summary>測試基底</summary>
public abstract class TestBase : IDisposable
{
    #region Static 靜態

    /// <summary>主要DB初始格式SQL</summary>
    private static readonly string _mainInitialSchemaSQL;

    /// <summary>主要DB初始資料SQL</summary>
    private static readonly string _mainInitialDataSQL;

    /// <summary>靜態建構式</summary>
    static TestBase()
    {
        TestBase._mainInitialSchemaSQL = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "DbSQL", "MainTest-Initial-Schema.sql"));
        TestBase._mainInitialDataSQL = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "DbSQL", "MainTest-Initial-Data.sql"));
    }

    #endregion

    /// <summary>主要資料庫</summary>
    protected readonly ProjectManagerMainContext _mainContext;

    /// <summary>主要測試資料庫名稱</summary>
    private readonly string _mainTestDatabaseName;

    /// <summary>建構式</summary>
    public TestBase(
        ProjectManagerMainContext mainContext)
    {
        var guidText = Guid.NewGuid().ToString("N");

        this._mainContext = mainContext;
        this._mainTestDatabaseName = $"ProjectManagerMainTest_{guidText.Substring(guidText.Length - 6)}";

        // 更換主要資料庫連線字串
        var newMainConn = this._mainContext.Database
            .GetConnectionString()
            .Replace("ProjectManagerMainTest", this._mainTestDatabaseName);
        this._mainContext.Database.SetConnectionString(newMainConn);
    }

    /// <summary>測試基底-重置全部資料庫</summary>
    protected void ResetAllDb()
    {
        if (this._mainContext.Database.CanConnect() == false)
        {
            _ = this._mainContext.Database.EnsureCreated();
            _ = this._mainContext.Database.ExecuteSqlRaw(_mainInitialSchemaSQL);
        }

        _ = this._mainContext.Database.ExecuteSqlRaw(_mainInitialDataSQL);
    }

    public void Dispose()
    {
        _ = this._mainContext.Database.EnsureDeleted();
        this._mainContext.Dispose();
    }

}
