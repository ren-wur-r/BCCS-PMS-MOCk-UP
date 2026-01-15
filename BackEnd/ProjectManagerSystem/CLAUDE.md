# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

### Building & Running

- **Clean**: `dotnet clean`
- **Restore**: `dotnet restore`
- **Build solution**: `dotnet build ProjectManagerSystem.sln`
- **Run Web API**: `dotnet run --project ProjectManagerWebApi`
- **Run specific test**: `dotnet test ServiceLibraryTest --filter FullyQualifiedName~TestClassName.TestMethodName`
- **Run all tests**: `dotnet test ServiceLibraryTest`

### Database Updates (EF Core Scaffolding)

1. Build any WebAPI project first
2. Switch Package Manager Console to `DataModelLibrary`
3. Run scaffold command:

   ```powershell
   # Local environment
   Scaffold-DbContext -Connection "Server=192.168.3.236,1433;User ID=sa;Password=P@ssw0rd;Database=ProjectManagerMain;Persist Security Info=False;TrustServerCertificate=True" -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir "EfCore/ProjectManagerMainTemp" -DataAnnotations -UseDatabaseNames -Force -NoOnConfiguring -NoPluralize

   # Development environment
   Scaffold-DbContext -Connection "Server=192.168.3.236,1433;User ID=sa;Password=P@ssw0rd;Database=ProjectManagerMain;Persist Security Info=False;TrustServerCertificate=True" -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir "EfCore/ProjectManagerMainTemp" -DataAnnotations -UseDatabaseNames -Force -NoOnConfiguring -NoPluralize
   ```

4. Rename temp folder (e.g., `ProjectManagerMainTemp` → `ProjectManagerMain`)
5. Fix namespace references in generated classes

### Database Configuration

- Uses SQL Server with Entity Framework Core
- Connection string configured via environment variable `DB_MAIN_CONNECTION` or appsettings
- Database context: `ProjectManagerMainContext` in DataModelLibrary
- Environment variables override appsettings (see `GsEnvironmentVariable.cs`)

## Architecture Overview

This is a .NET 9 Web API project with a layered architecture following specific naming conventions:

### Project Structure

- **ProjectManagerWebApi**: Main Web API project with controllers
- **ServiceLibrary**: Business logic layer with services organized by domain
- **DataModelLibrary**: Entity Framework models and database context
- **ApiModelLibrary**: API request/response models for controllers
- **CommonLibrary**: Shared utilities (cryptography, extensions, Swagger config)
- **ServiceLibraryTest**: Unit tests using xUnit v3

### Domain Organization

The system is organized around two main domains:

- **Core**: Contains Manager, Atom, and Company modules
- **ManagerBackSite (Mbs)**: Management backend functionality

### Service Architecture Pattern

Services follow a consistent layered pattern:

- **DB Services**: Database operations (`Co{Domain}{Feature}DbService`)
- **Memory Services**: In-memory caching (`Co{Domain}{Feature}MemoryService`)
- **Logical Services**: Business logic (`Co{Domain}LogicalService`, `Mbs{Feature}LogicalService`)

### Naming Conventions

The codebase follows strict naming conventions defined in `Specification/程式碼縮寫規範.txt`:

- **Core domains**: Manager (Mgr), Atom (Atm), Company (Com)
- **ManagerBackSite**: Abbreviated as Mbs with submodules like Basic (Bsc), System (Sys), etc.
- **Model naming**: `Co{Domain}{Feature}{Layer}{Operation}{Req/Rsp}Mdl`
- **Service naming**: `Co{Domain}{Feature}{Layer}Service`

### Authentication & Authorization

- Uses token-based authentication with login/logout functionality
- Login tokens managed in memory via `CoMgrLoginInfoMemoryService`
- Manager permissions handled through `ManagerMenuPermission` system

### Database Specifications

Database follows specific collation rules defined in `Specification/Db規範.txt`:

- General fields: `Latin1_General_100_CS` (case-sensitive)
- Text IDs: `Latin1_General_100_BIN2` (binary sorting)
- User input: `Latin1_General_100_CS_AS_KS_WS_SC_UTF8` (full Unicode support)

### Testing Standards

Unit tests follow guidelines in `Specification/UnitTest測驗邏輯規範.txt`:

1. Validate request/response parameters
2. Test database CRUD operations
3. Verify auxiliary method calls that don't affect return results

### Environment Configuration

- Supports Local, Development, Staging environments
- Swagger UI available in Local/Development environments
- Configuration through appsettings files and environment variables
- NLog for logging with configuration in `NLog.config`

### Service Implementation Patterns

Services follow different patterns based on their layer:

#### Database Service Pattern

Database services handle Entity Framework operations and follow this pattern:

```csharp
public async Task<ResponseModel> MethodAsync(RequestModel reqObj)
{
    try
    {
        // Database operations here
        var result = new ResponseModel { /* ... */ };
        return result;
    }
    catch (Exception ex)
    {
        this._logger.LogError(
            $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
            $", ex: {ex?.Message}" +
            $", ex.InnerException: {ex?.InnerException?.Message}");
        return default;
    }
    finally
    {
        this._mainContext.ChangeTracker.Clear(); // Only for DB services
    }
}
```

#### Logical Service Pattern

Logical services handle business logic and follow this pattern:

```csharp
public async Task<ResponseModel> MethodAsync(RequestModel reqObj)
{
    var rspObj = new ResponseModel
    {
        ErrorCode = MbsErrorCodeEnum.Fail
    };

    // Permission validation (if required)
    // Business logic implementation
    // Call database services

    rspObj.ErrorCode = MbsErrorCodeEnum.Success;
    return rspObj;
}
```

### Pagination Query Standards

All paginated queries must:

1. Support PageIndex and PageSize parameters
2. Return TotalCount in response (if the response model defines this property)
3. Use Skip/Take for pagination implementation
4. Order by ID to ensure consistency
5. Implement count query separately when TotalCount is required

### Permission Validation Guidelines

- Permission validation may be commented out during development but must preserve the complete permission check structure
- All permission validation code must be enabled before production deployment
- Use TODO comments to mark permission-related code sections
- Permission checks should validate both login token and menu permissions

### Error Code Usage Standards

- **Name Duplicate**: Use `MbsErrorCodeEnum.NameDuplicate`
- **Permission Denied**: Use `MbsErrorCodeEnum.PermissionDenied`
- **General Failure**: Use `MbsErrorCodeEnum.Fail`
- **Success**: Use `MbsErrorCodeEnum.Success`

### Key Dependencies & Version Requirements

- **SQL Server** with Microsoft EntityFramework provider
- **Entity Framework Core**: Version **9.0.1** (critical for compatibility)
- **Microsoft.EntityFrameworkCore.SqlServer**: Version **9.0.7** (resolves version conflicts)
- **Hangfire**: For scheduled tasks and background jobs
  - Uses InMemoryStorage for development
  - Recommend SQL Server/Redis storage for production
- **xUnit v3** with Dependency Injection for testing
- **NLog** for logging (JSON format configuration)
- **Swagger** for API documentation (dev environments only)

### Important Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Environment name (Local, Development, Staging)
- `DB_MAIN_CONNECTION`: Database connection string override
- `SWAGGER_IS_ENABLE`: Enable/disable Swagger UI

## Schedule System (Hangfire)

### Hangfire Configuration

The project uses Hangfire for scheduled tasks and system initialization:

```csharp
// Program.cs - Hangfire setup
builder.Services.AddHangfire(hangfireConfig =>
    hangfireConfig
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage()); // Use SQL Server/Redis in production
builder.Services.AddHangfireServer();
```

### Dashboard Access

- **URL**: `/hangfire-schedule`
- **Authorization**: Currently uses `CmnHangfireNoAuthFilter` (dev only)
- **Mode**: Read-only dashboard
- **Important**: Enable proper authorization in production

### Schedule Registration

Schedules are registered in `ScheduleApplicationBuilderExtension.cs`:

```csharp
RecurringJob.AddOrUpdate<ISchInitialLogicalService>(
    "InitialJob",
    service => service.StartJobAsync(),
    "0 0 * * *",  // Daily at 00:00
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time")
    });
```

### System Initialization Flow

The initialization schedule (`SchInitialLogicalService`) performs:

1. **Region Initialization**: Creates system regions (AllRegion: -1, Denied: -2)
2. **Department Initialization**: Creates President department (-1)
3. **Admin Management**:
   - Creates Admin when database is empty
   - Removes Admin when real employees exist
   - Keeps Admin if it's the only employee

### Admin Auto-Management Logic

- **Employee Count = 0**: Create Admin user with full permissions
- **Employee Count = 1 (Admin only)**: Keep Admin
- **Employee Count > 1**: Remove Admin and related data

### Database Constants for System Data

System uses negative IDs for reserved data:

- **ManagerRegion**: AllRegion(-1), Denied(-2)
- **ManagerDepartment**: President(-1), Admin(-2)
- **ManagerRole**: Admin(-2)
- **Employee**: admin(-2)

### Insert with Specific ID

For system initialization data with specific IDs (like -1, -2):

```csharp
await this._mainContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Table] ON");
// Insert operation
await this._mainContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Table] OFF");
```

## Troubleshooting

### Build Errors After Merge Conflicts

If you encounter build errors like "cannot find type" after resolving merge conflicts:

1. **Clean build cache**: `dotnet clean`
2. **Remove bin/obj folders**: `find . -name "bin" -type d -exec rm -rf {} +` and `find . -name "obj" -type d -exec rm -rf {} +`
3. **Restore packages**: `dotnet restore`
4. **Rebuild**: `dotnet build`

This is a common issue where the compiler cache retains old file states.
