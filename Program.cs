using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Service.Interface;
using websitebenhvien.Service.Reponser;
using websitebenhvien.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using websitebenhvien.Helper;
using AspNetCoreRateLimit;
using elFinder.NetCore;
using websitebenhvien.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbcontext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 100, // Số lần thử lại tối đa
                maxRetryDelay: TimeSpan.FromSeconds(30), // Thời gian chờ tối đa giữa các lần thử lại
                errorNumbersToAdd: null); // Có thể chỉ định mã lỗi cụ thể để thử lại
        }));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDbcontext>()
    .AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IPageMain,PageMainReponser>();
builder.Services.AddScoped<IAllinone, AllinoneReponser>();  
builder.Services.AddScoped<IUser, UserReponser>();
builder.Services.AddScoped<IPost, PostReponser>();
builder.Services.AddScoped<ISpecialty, SpecialtyReponser>();
builder.Services.AddScoped<IWorkSchedule, WorkScheduleReponser>();
builder.Services.AddScoped<ISamplemessager, SampleReponser>();
builder.Services.AddScoped<IRecruitment, RecruitmentReponser>();
builder.Services.AddScoped<IForbusiness, ForbusinessReponser>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<Hubnot>();
builder.Services.AddScoped<Uploadfile>();
builder.Services.AddScoped<IAdminmenu, AdminmenuReponsive>();
builder.Services.Configure<FileSystemConfig>(builder.Configuration.GetSection(FileSystemConfig.ConfigName));
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddMemoryCache();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("user", policy => policy.RequireRole("user"));
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("CreatePolicy", policy => policy.RequireClaim("Permission", "Create"));
    options.AddPolicy("EditPolicy", policy => policy.RequireClaim("Permission", "Edit"));
    options.AddPolicy("DeletePolicy", policy => policy.RequireClaim("Permission", "Delete"));
    options.AddPolicy("ReadPolicy", policy => policy.RequireClaim("Permission", "Read"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/dang-nhap";
    options.AccessDeniedPath = "/trang-loi";
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/dang-nhap";
            options.LogoutPath = "/dang-nhap";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(1);
            options.SlidingExpiration = true;
        });


var app = builder.Build();
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == 403)
    {
        context.HttpContext.Response.Redirect("/");
    }
    return Task.CompletedTask;
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}
app.UseIpRateLimiting();
app.UseCustomRateLimit();
app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadsPath),
        RequestPath = "/Resources"
    }
);

app.UseHttpsRedirection();


app.UseHsts(); 
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapHub<Hubnot>("/friendHub");
// Route cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Trangquantri}/{action=Dangnhap}/{id?}");

// Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "bai-viet",
    defaults: new { controller = "Home", action = "PostCategory" },
    pattern: "bai-viet/{catogery}");
app.MapControllerRoute(
    name: "bai-viet-detail",
    defaults: new { controller = "Home", action = "PostDetail" },
    pattern: "chi-tiet-tin/{alias_url}");





app.Run();