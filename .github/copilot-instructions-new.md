# Hospital Website Development Guide

## Project Architecture

ASP.NET Core MVC hospital website with Areas-based admin panel and custom static file optimization.

### Core Stack
- **Framework**: ASP.NET Core MVC with Identity
- **Database**: SQL Server with Entity Framework Core + retry policy
- **Frontend**: Bootstrap 5, jQuery, SignalR for real-time features
- **File Management**: Custom `/Resources` endpoint + elFinder integration
- **Performance**: Rate limiting, memory caching, image optimization

### Key Patterns
```csharp
// Service Registration Pattern
builder.Services.AddScoped<IActivity, ActivityResponsive>();
builder.Services.AddScoped<IActivitypost, ActivitypostReponsive>();

// File Serving with Caching
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Resources"
});
```

## File Upload & Optimization

### Upload Structure
- **Physical Path**: `{ContentRoot}/Uploads/`  
- **Web Path**: `/Resources/{filename}`
- **Service**: `Helper/Uploadfile.cs` handles file operations
- **Admin**: elFinder integration at `/Areas/Admin/Views/Filemanager/`

### Image Optimization Requirements
- **Formats**: Prefer WebP/AVIF over PNG/JPG
- **Responsive**: Generate multiple sizes for different viewports
- **Cache Headers**: Set long-term caching (1 year) for `/Resources/*`
- **Compression**: Enable gzip/brotli for static assets

## Performance Critical Files

### Static File Configuration (Program.cs)
```csharp
// Custom uploads directory with caching
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Resources",
    OnPrepareResponse = ctx => {
        // Add cache headers here
    }
});
```

### Database Resilience
```csharp
// SQL Server with retry policy for connection issues
options.UseSqlServer(connectionString, sqlOptions => {
    sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 100,
        maxRetryDelay: TimeSpan.FromSeconds(30)
    );
});
```

## Admin Panel Conventions

### Areas Structure
- **Path**: `Areas/Admin/Controllers/` with `[Area("Admin")]`
- **Routes**: Custom SEO-friendly URLs like `/trang-quan-tri/quan-li-hoat-dong`
- **Layout**: Separate admin layout with elFinder file browser

### Service Layer Pattern
- **Interface**: `Service/Interface/I{EntityName}.cs`
- **Implementation**: `Service/Reponser/{EntityName}Reponser.cs` (note: typo in "Reponser")
- **Registration**: All services registered as Scoped in Program.cs

## ViewModels & Mapping

### Convention
- **Location**: `Models/EnitityVM/` (note: typo in "Enitity")
- **AutoMapper**: `Helper/MappingProfile.cs` with explicit navigation property mapping
- **Pattern**: Always map navigation properties explicitly:

```csharp
CreateMap<Postactivity, PostactivityVM>()
    .ForMember(dest => dest.CategoryName, 
              opt => opt.MapFrom(src => src.Activitycategory.Title));
```

## Frontend Optimization

### Bootstrap Version Compatibility
- Support both Bootstrap 4 (`data-dismiss`) and 5 (`data-bs-dismiss`) attributes
- Use `data-bs-*` for new components

### Image Display Pattern
```html
<!-- Responsive images with lazy loading -->
<img src="/Resources/{filename}" 
     loading="lazy" 
     width="800" height="600"
     style="width: 100%; height: auto;">
```

## Critical Development Issues

### Common Performance Problems
1. **Large Images**: Files served at full resolution without optimization
2. **No Cache Headers**: Static files lack browser caching directives  
3. **Render Blocking**: CSS/JS loaded synchronously in `<head>`
4. **No Image Formats**: Missing WebP/AVIF support for modern browsers

### Quick Fixes Needed
- Add cache headers to `/Resources/*` endpoint
- Implement image resizing service
- Defer non-critical CSS/JS loading
- Add `preconnect` hints for external CDNs

## Testing & Debugging

### Key URLs
- Admin: `/trang-quan-tri/` 
- File Manager: `/trang-quan-tri/filemanager`
- Image Resources: `/Resources/{filename}`

### Performance Monitoring
- Check Lighthouse scores for image optimization opportunities
- Monitor SQL retry logs for connection issues
- Test elFinder file upload/preview functionality
