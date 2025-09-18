


```csharp
# Hospital Website AI Coding Instructions

## Architecture Overview
This is an ASP.NET Core 8.0 MVC web application for a hospital management system with Vietnamese localization. The application follows a clean architecture pattern with Areas, service layer abstraction, and role-based authorization.

## Core Technologies
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with cookie authentication
- **UI**: Razor Views with Bootstrap, jQuery, Summernote editor
- **Real-time**: SignalR for notifications
- **Mapping**: AutoMapper for entity-ViewModel mapping
- **Performance**: Compression (Brotli/Gzip), memory caching, static file optimization

## Project Structure

### Areas Architecture
- **Admin Area**: `/Areas/Admin/` - Administrative functionality with Vietnamese routing patterns
- **Controllers**: Role-based authorization with granular permissions
- **Route Pattern**: `/trang-quan-tri/{action}` (Vietnamese admin URLs)

### Service Layer Pattern
```csharp
// Interface-based dependency injection
Service/Interface/IPageMain.cs, ISpecialty.cs, IUser.cs, etc.
Service/Reponser/{ServiceName}Reponser.cs (implementations)

// Registration in Program.cs
builder.Services.AddScoped<IPageMain, PageMainReponser>();
```

### Data Layer
- **DbContext**: `Data/MyDbcontext.cs` - Entity Framework configuration
- **Entities**: `Models/Enitity/` - Domain models with relationships
- **ViewModels**: `Models/EnitityVM/` - UI-specific data transfer objects
- **Migrations**: `Migrations/` - Database schema versions

### Authorization System
```csharp
// Role-based with granular permissions
[Authorize(Roles = "admin,Quanlytaikhoan")] // Admin OR specific permission
[Authorize(Roles = "admin")] // Admin only
```

**Permission Roles**:
- `admin` - Full system access
- `Quanlytaikhoan` - User management
- `Danhmucbaiviet` - Article categories
- `Baiviet` - Article management
- `Quanlychuyenkhoa` - Specialty management
- `Quanlybacsi` - Doctor management
- `Quanlyvideo` - Video management
- `Quanlythietbi` - Equipment management
- And many more specific permissions

## Key Conventions

### Vietnamese Naming
- **Routes**: Use Vietnamese slugs (`/trang-quan-tri/quan-ly-tai-khoan`)
- **Methods**: English method names with Vietnamese parameters
- **UI**: Vietnamese labels and messages
- **Database**: English entity names, Vietnamese display properties

### API Endpoints
```csharp
[HttpPost("/api-{function-name}")]           // Create/Update operations
[HttpGet("/api-get-{resource}")]             // Read operations
[HttpPost("/api-delete-{resource}")]         // Delete operations
```

### File Organization
```
Controllers/          # Main site controllers
Areas/Admin/Controllers/  # Admin panel controllers
Service/Interface/    # Service contracts
Service/Reponser/     # Service implementations (note: "Reponser" not "Responder")
Models/Enitity/       # Domain entities (note: "Enitity" not "Entity")
Models/EnitityVM/     # ViewModels
ViewComponents/       # Reusable view components
Helper/              # Utility classes
Middleware/          # Custom middleware
```

## Development Guidelines

### Entity-ViewModel Mapping
- Use AutoMapper for entity-ViewModel transformations
- ViewModels end with `VM` suffix
- Configure mapping in `Helper/MappingProfile.cs`

### Performance Optimizations
- **Security Headers**: `SecurityHeadersMiddleware` for HSTS, CSP, etc.
- **Compression**: Brotli/Gzip via `CompressionExtensions`
- **Rate Limiting**: IP-based with `CustomRateLimitMiddleware`
- **Static Files**: Long-term caching for images/assets
- **Image Optimization**: `OptimizedImageHelper` and `ResponsiveImageHelper`

### Database Patterns
```csharp
// Entity relationships
public class Specialty
{
    public List<Doctor> Doctor { get; set; }
    public List<Postrelate> Postrelate { get; set; }
}

// Service pattern
public async Task<(List<EntityVM> ds, int TotalPages)> GetPaginatedData(int page, int pageSize)
```

### Authentication Flow
1. Login route: `/dang-nhap`
2. Access denied: `/trang-loi`
3. Cookie-based authentication with 1-day expiration
4. Role-based authorization with policy-based permissions

### File Upload System
- **Helper**: `Uploadfile.cs` for file operations
- **Storage**: `/Uploads/` directory with `/Resources/` virtual path
- **Optimization**: Automatic image resizing and optimization

## Common Patterns

### Controller Structure
```csharp
[Area("Admin")]
[Authorize(Roles = "admin,SpecificPermission")]
public class FeatureController : Controller
{
    private readonly IFeatureService _service;
    
    public FeatureController(IFeatureService service)
    {
        _service = service;
    }
    
    [HttpPost("/api-feature-action")]
    public async Task<IActionResult> Action(FeatureVM model)
    {
        try
        {
            var result = await _service.ProcessAsync(model);
            return Json(new { status = true, data = result });
        }
        catch (Exception ex)
        {
            return Json(new { status = false, message = ex.Message });
        }
    }
}
```

### Service Implementation
```csharp
public class FeatureReponser : IFeature
{
    private readonly MyDbcontext _context;
    private readonly IMapper _mapper;
    
    public async Task<bool> CreateAsync(FeatureVM model)
    {
        var entity = _mapper.Map<FeatureEntity>(model);
        _context.Add(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
```

### ViewComponent Pattern
```csharp
public class FeatureViewComponent : ViewComponent
{
    private readonly IFeatureService _service;
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var data = await _service.GetDataAsync();
        return View(data);
    }
}
```

## Dependencies & Libraries
- **AspNetCoreRateLimit** - Rate limiting
- **AutoMapper** - Object mapping  
- **Microsoft.AspNetCore.SignalR** - Real-time features
- **Microsoft.EntityFrameworkCore** - ORM
- **Microsoft.AspNetCore.Identity** - Authentication
- **Summernote** - WYSIWYG editor
- **Bootstrap** - UI framework
- **jQuery** - Client-side scripting

## Best Practices

### Security
- Always use `[Authorize]` attributes with specific roles
- Implement CSRF protection for forms
- Use parameterized queries (Entity Framework handles this)
- Security headers via middleware

### Performance
- Use async/await for all database operations
- Implement caching where appropriate
- Optimize images before serving
- Use compression middleware

### Code Quality
- Follow interface segregation - each service has specific responsibilities
- Use dependency injection for all services
- Implement proper error handling with try-catch
- Return consistent JSON response format: `{ status: bool, data/message: any }`

### Vietnamese Localization
- Use Vietnamese for all user-facing text
- Keep route patterns in Vietnamese
- English for code/technical elements
- Maintain Vietnamese SEO-friendly URLs

When working with this codebase:
1. Follow the established naming conventions (including "Reponser" and "Enitity" spellings)
2. Use the service layer pattern - never access DbContext directly from controllers
3. Apply appropriate authorization attributes based on feature requirements
4. Maintain the Vietnamese routing and UI language patterns
5. Use the established JSON response format for APIs
6. Follow the pagination pattern: `(List<VM> ds, int TotalPages)` for list endpoints
```







