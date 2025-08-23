# Hướng dẫn phát triển Website Bệnh viện

## Tổng quan dự án

Đây là website bệnh viện sử dụng ASP.NET Core MVC với cấu trúc Areas cho phần quản trị.

## Cấu trúc dự án

### Core Components
- **Areas/Admin**: Giao diện quản trị với controllers, views riêng biệt
- **Models**: Chứa Entity models và ViewModels (trong thư mục EnitityVM)
- **Service**: Pattern Repository với Interface và Implementation (Reponser)
- **Helper**: Các utility classes (MappingProfile, Uploadfile, SeoHelper, ...)
- **Data**: DbContext và Entity Framework configuration

### Database & Entities
- Sử dụng Entity Framework Core với Code First approach
- Conventions: Foreign key format `Id_[EntityName]` (vd: Id_Categoryactivity)
- Navigation properties: Luôn sử dụng `.Include()` khi cần load related data
- Relationships: One-to-Many, Many-to-Many đều được cấu hình explicit

### AutoMapper Configuration
- File: `Helper/MappingProfile.cs`
- Pattern: Entity -> ViewModel mapping
- Navigation properties cần mapping explicit:
```csharp
CreateMap<Postactivity, PostactivityVM>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Activitycategory.Title))
```

## Frontend Patterns

### Bootstrap Usage
- Sử dụng Bootstrap 5 (hoặc mixed version)
- Modal attributes: Support cả `data-dismiss` và `data-bs-dismiss`
- Form validation: Bootstrap classes (.is-invalid, .form-control)
- Responsive: CSS Grid và Flexbox với clamp() cho typography

### JavaScript Patterns
- File upload preview: Sử dụng FileReader API
- Modal handling: Bootstrap modal API
- Form validation: Client-side validation trước khi submit
- AJAX: Cho các thao tác không cần reload page

### ViewComponents
- Location: `Views/Shared/Components/[ComponentName]/Default.cshtml`
- Pattern: Sử dụng cho các phần UI có logic phức tạp
- Data: Truyền qua ViewModel riêng

## Admin Panel Conventions

### Controllers
- Location: `Areas/Admin/Controllers`
- Attribute: `[Area("Admin")]`
- Routing: Custom routes như `[Route("/trang-quan-tri/quan-li-hoat-dong")]`

### Views
- Layout: Riêng biệt cho admin area
- Modal forms: Bootstrap modal với form validation
- Search/Filter: Sử dụng ViewBag để maintain state
- Pagination: Custom pagination helper

### File Upload
- Service layer: `Helper/Uploadfile.cs`
- Preview: JavaScript FileReader cho thumbnail
- Validation: File type và size check
- Strategy: Preserve existing file until new upload success

## Common Patterns

### Search & Filter
- ViewModel: Dedicated SearchVM classes
- ViewBag: Để maintain dropdown selections
- Pagination: Page size và current page tracking

### Error Handling
- TempData: Cho user messages (SuccessMessage, ErrorMessage)
- Try-catch: Trong service layer
- Logging: NLog hoặc built-in logging

### Status Management
- Boolean status fields với dropdown filters
- Soft delete pattern cho entities

## Development Workflow

### Build & Run
```bash
dotnet build
dotnet run
```

### Migration
```bash
dotnet ef migrations add [MigrationName]
dotnet ef database update
```

### Admin URLs
- Base admin: `/trang-quan-tri`
- Features: `/trang-quan-tri/[feature-name]`

## File Structure Key Points

### Critical Files
- `Data/MyDbcontext.cs`: Entity configurations
- `Helper/MappingProfile.cs`: AutoMapper setup
- `Views/Shared/_Layout.cshtml`: Bootstrap và jQuery references
- `Program.cs`: Dependency injection và middleware setup

### Upload Directories
- `Uploads/`: File storage
- `wwwroot/uploads/`: Web-accessible uploads

## Best Practices

### Code Style
- Service pattern: Interface -> Implementation
- ViewModel pattern: Separate từ Entity models
- Repository pattern: Trong service layer

### Security
- Admin area protection
- File upload validation
- XSS protection với Html.Raw chỉ khi cần

### Performance
- Include navigation properties chỉ khi cần
- Async patterns cho database operations
- Caching cho static data

## Common Issues & Solutions

### Modal Not Closing
- Bootstrap version compatibility
- Event handler conflicts
- JavaScript errors blocking modal

### File Upload Issues
- Preview not showing: FileReader API
- Files lost on edit: Service layer file handling
- File size limits: Server và client validation

### AutoMapper Issues
- Navigation properties not mapped
- Circular references
- Missing Include() trong query

### Bootstrap Compatibility
- Mixed version support: `data-dismiss` và `data-bs-dismiss`
- CSS class changes between versions
- JavaScript API differences

## Testing

### Manual Testing
- Admin CRUD operations
- File upload/preview/delete
- Modal interactions
- Responsive design

### Key Test Scenarios
- Edit form với file upload
- Search và filter functionality
- Modal open/close behavior
- Navigation property display
