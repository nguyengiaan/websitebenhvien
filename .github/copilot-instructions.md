# Copilot Instructions for Hospital Website (ASP.NET Core)

## Architecture Overview
- **ASP.NET Core 9.0** hospital management system with Vietnamese interface
- **Entity Framework Core** with `MyDbcontext` - all entities in `Models/Enitity/`
- **Service Layer Pattern** - `Service/Interface/` and `Service/Reponser/` (note: typo in folder name)
- **Area-based Admin** - `/Areas/Admin/Controllers/` with `[Authorize("admin")]`
- **Vietnamese URLs** - Custom routes like `/trang-quan-tri/`, `/bai-viet/`, `/chi-tiet-tin/`

## CRUD Patterns

### Controller Structure
```csharp
[Area("Admin")]
[Authorize("admin")]
public class XController : Controller
{
    private readonly IXService _xService;
    
    [Route("/trang-quan-tri/quan-li-x")]
    public async Task<IActionResult> Index(XSearchVM searchModel) { }
    
    [HttpGet]
    public async Task<IActionResult> GetX(int id) { } // JSON API
    
    [HttpPost]
    public async Task<IActionResult> CreateX(XVM model) { } // JSON API
    
    [HttpPost]  
    public async Task<IActionResult> UpdateX(XVM model) { } // JSON API
    
    [HttpPost]
    public async Task<IActionResult> DeleteX(int id) { } // JSON API
}
```

### Service Pattern
- Interface: `IXService` in `Service/Interface/`
- Implementation: `XReponser` in `Service/Reponser/` (note spelling)
- Always return `Task<bool>` for CUD operations, `Task<PagedResult>` for lists
- Use AutoMapper for Entity ↔ ViewModel mapping

### Search & Pagination Models
```csharp
public class XSearchVM
{
    public string? SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "Name";
    public string SortDirection { get; set; } = "asc";
}

public class PagedXResult
{
    public List<XVM> Items { get; set; }
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    // HasPreviousPage, HasNextPage properties
}
```

## Key Conventions
- **Entity naming**: All entities have `Id_entityname` primary key
- **Vietnamese properties**: `Name`, `Title`, `Description` often in Vietnamese
- **URL aliases**: Most entities have `alias_url` or `link_alias` for SEO URLs  
- **File uploads**: Use `IFormFile formFile` in ViewModels with `Uploadfile` helper
- **JSON responses**: `Json(new { status = true/false, message = "", data = "" })`
- **Authorization**: Admin area uses `[Authorize("admin")]` policy

## Database Context
- `MyDbcontext` in `Data/` folder  
- Entity relationships via navigation properties
- Migration pattern: `dotnet ef migrations add migration_name`

## File Upload Pattern
- `Helper/Uploadfile.cs` handles file operations
- Upload to `/Uploads/` directory
- Store filename/path in entity properties like `Url_icon`

This codebase follows Vietnamese naming conventions and has established patterns for admin CRUD operations with pagination and search.