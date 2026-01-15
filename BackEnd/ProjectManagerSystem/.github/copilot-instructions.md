# Copilot Instructions for ProjectManagerSystem

## Architecture Overview

This is a **layered ASP.NET Core Web API** project using **Entity Framework Core with PostgreSQL**. The solution follows a **strict naming convention pattern** based on abbreviated functional domains.

### Project Structure
- **CommonLibrary**: Shared utilities, Swagger configurations, common extensions
- **DataModelLibrary**: EF Core entities, database contexts, global configurations, environment variables
- **ApiModelLibrary**: API-specific models and DTOs
- **ServiceLibrary**: Business logic services organized by functional domains
- **ProjectManagerWebApi**: Web API controllers and startup configuration

## Critical Naming Conventions

**ALWAYS follow these abbreviation patterns** (from `Specification/程式碼縮寫規範.txt`):

### Core Domain Abbreviations
- `Co` = Core (核心)
- `Mgr` = Manager (管理者) 
- `Inf` = Info (資訊)
- `Db` = Database
- `Req` = Request
- `Rsp` = Response
- `Mdl` = Model

### Service Layer Patterns
- **Format**: `{Domain}{Function}Db{Operation}{Req/Rsp}Mdl`
  - Example: `CoMgrCmpMainKdDbAddReqMdl` = Core-Manager-CompanyMainKind-Database-Add-Request-Model
- **Service**: `{Domain}{Function}DbService`
  - Example: `CoMgrCompanyMainKindDbService` = Core-Manager-CompanyMainKind-Database-Service
- **Interface**: `I{ServiceName}`
  - Example: `ICoMgrCompanyMainKindDbService`

### Model Naming Pattern
- **Property Names**: Use full descriptive names with domain prefix
  - Example: `ManagerCompanyMainKindName`, `ManagerCompanyMainKindIsEnable`
- **Collection Properties**: Use `{Entity}List` suffix
  - Example: `ManagerCompanyMainKindList`

## Database Integration Workflow

### EF Core Scaffolding Process (from `DataModelLibrary/EfCore/Command.txt`):
1. Build any WebAPI project
2. Switch Package Manager Console to `DataModelLibrary`
3. Run scaffold command:
   ```powershell
   Scaffold-DbContext -Connection "Host=localhost;Port=5432;Database=ProjectManagerMain;Username=postgres;Password=postgres;" -Provider "Npgsql.EntityFrameworkCore.PostgreSQL" -OutputDir "EfCore/ProjectManagerMainTemp" -DataAnnotations -UseDatabaseNames -Force -NoOnConfiguring -NoPluralize
   ```
4. Rename temp folder (e.g., `ProjectManagerMainTemp` → `ProjectManagerMain`)
5. Fix namespace references in generated classes

### Database Configuration
- Uses **PostgreSQL** with connection string from environment variables or appsettings
- Environment variables override appsettings (see `GsEnvironmentVariable.cs`)
- Database context: `ProjectManagerMainContext`

### DateTime Handling with PostgreSQL
**Critical**: PostgreSQL requires UTC timestamps. Add this configuration to avoid "Cannot write DateTime with Kind=Unspecified" errors:

```csharp
// In Program.cs - Enable legacy timestamp behavior
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Or configure in DbContext registration:
options.UseNpgsql(connectionString, npgsqlOptions =>
{
    npgsqlOptions.EnableLegacyTimestampBehavior();
});
```

## Service Layer Architecture

### Service Organization Pattern
```
ServiceLibrary/
├── Core/                    # Core business domains
│   ├── Manager/            # Management-related services
│   │   ├── DB/            # Database operations
│   │   │   ├── CompanyMainKind/   # Company main classification operations
│   │   │   ├── CompanySubKind/    # Company sub classification operations
│   │   │   ├── Region/           # Regional operations
│   │   │   └── Role/             # Role management operations
│   │   │       ├── Format/       # Request/Response models
│   │   │       └── Service/      # Service implementations
│   │   └── Extension/     # DI registration extensions
├── ManagerBackSite/        # Manager backend site services
│   ├── Logical/            # Business logic layer
│   └── Extension/          # DI registration extensions
```

### Standard CRUD Operations
Each domain service implements these standard methods:
- `GetManyAsync(GetManyReqMdl)` - List with filtering (name search, status, parent IDs)
- `GetAsync(GetReqMdl)` - Single item by ID
- `AddAsync(AddReqMdl)` - Create new item
- `UpdateAsync(UpdateReqMdl)` - Update existing item

## Service Implementation Workflow

### Creating New Data Access Layer
When implementing CRUD operations for a new entity (e.g., `ManagerCompanyMainKind`):

1. **Create Folder Structure**: 
   ```
   ServiceLibrary/Core/Manager/DB/{EntityName}/
   ├── Format/    # All request/response models
   └── Service/   # Interface and implementation
   ```

2. **Model Creation Pattern**:
   - `{Prefix}DbGetManyReqMdl` - with search filters (Name, IsEnable, parent IDs)
   - `{Prefix}DbGetManyRspMdl` - with List property containing items
   - `{Prefix}DbGetManyRspItemMdl` - individual item structure
   - `{Prefix}DbGetReqMdl` - single ID lookup
   - `{Prefix}DbGetRspMdl` - full entity data
   - `{Prefix}DbAddReqMdl` - creation fields only
   - `{Prefix}DbAddRspMdl` - full entity after creation
   - `{Prefix}DbUpdateReqMdl` - ID + updatable fields
   - `{Prefix}DbUpdateRspMdl` - full entity after update

3. **Service Implementation**:
   - Interface with standard CRUD methods
   - Implementation with Logger + DbContext injection
   - All methods follow try-catch-finally pattern
   - Always call `ChangeTracker.Clear()` in finally

4. **DI Registration**:
   - Add to appropriate ServiceCollectionExtensions
   - Follow alphabetical order within category comments

## Service Implementation Pattern
- **Constructor**: Logger + DbContext injection
- **Methods**: Async with try-catch-finally
- **Error Handling**: Log full request object + exception details using `JsonSerializer.Serialize`
- **DbContext**: Always call `ChangeTracker.Clear()` in finally block
- **Return**: `default` on error, structured response models on success

### Example Service Method Pattern:
```csharp
public async Task<ResponseModel> MethodAsync(RequestModel reqObj)
{
    try
    {
        // Business logic here
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
        this._mainContext.ChangeTracker.Clear();
    }
}
```

## Dependency Injection Patterns

### Service Registration (see `CoManagerServiceCollectionExtensions.cs`):
```csharp
public static IServiceCollection AddCoreManager(this IServiceCollection services)
{
    // Services registered in DB folder alphabetical order
    services.AddScoped<ICoMgrCompanyMainKindDbService, CoMgrCompanyMainKindDbService>(); 
    services.AddScoped<ICoMgrDepartmentDbService, CoMgrDepartmentDbService>();
    services.AddScoped<ICoMgrRegionDbService, CoMgrRegionDbService>();
    return services;
}
```

### Controller Injection:
- Controllers inherit from `ControllerBase`
- Use `[Route("api/[controller]/[action]")]` pattern
- Inject services through constructor with proper naming

## Configuration Management

### Environment Variables (see `GsEnvironmentVariable.cs`):
- `ASPNETCORE_ENVIRONMENT`: Environment name
- `DB_MAIN_CONNECTION`: Database connection string override
- `SWAGGER_IS_ENABLE`: Swagger enable/disable

### AppSettings Structure:
```json
{
  "AppConfig": {
    "Database": {
      "MainConnection": "connection_string_here"
    }
  }
}
```

## Package Version Management

**Critical**: Maintain EF Core version compatibility:
- `Microsoft.EntityFrameworkCore.*`: Use version **9.0.1**
- `Npgsql.EntityFrameworkCore.PostgreSQL`: Use version **9.0.4**
- This specific combination resolves known version conflicts

## Testing Patterns

From `UnitTest測驗邏輯規範.txt`, unit tests must verify:
1. **Parameter validation**: Request and response parameter validation
2. **Storage operations**: Create, read, update, delete operations on storage media
3. **Side effects**: Verify other methods called during logic that don't affect return results

## Build Commands

- **Clean**: `dotnet clean`
- **Restore**: `dotnet restore` 
- **Build**: `dotnet build`
- **Run**: `dotnet run --project ProjectManagerWebApi`

When working with this codebase, always follow the established abbreviation patterns, use the service layer architecture, and maintain the strict naming conventions for consistency with the existing codebase.
