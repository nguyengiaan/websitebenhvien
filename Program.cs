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
using websitebenhvien.Service;
using websitebenhvien.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using websitebenhvien.Helper;
using AspNetCoreRateLimit;
using elFinder.NetCore;
using websitebenhvien.Middleware;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;


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
builder.Services.AddScoped<IMenuadminuser, MenuadminuserReponser>();
builder.Services.AddScoped<IMenuUserService, MenuUserService>();
builder.Services.AddScoped<IActivity, ActivityResponsive>();
builder.Services.AddScoped<IActivitypost, ActivitypostReponsive>();
builder.Services.AddScoped<ITitlemenu, TitlemenuReponser>();
builder.Services.AddScoped<IPricelist, PricelistResponsive>();
builder.Services.AddScoped<ICatogeryguide, CatogeryguideResponsive>();
builder.Services.AddScoped<ICustomeguider, CustomerguiderResponsive>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<Hubnot>();
builder.Services.AddScoped<Uploadfile>();
builder.Services.AddScoped<ResponsiveImageHelper>();
builder.Services.AddScoped<IAdminmenu, AdminmenuReponsive>();
builder.Services.AddScoped<IFeedback, Feedbackresponsive>();
builder.Services.Configure<FileSystemConfig>(builder.Configuration.GetSection(FileSystemConfig.ConfigName));
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddMemoryCache();
builder.Services.AddCustomCompression();
builder.Services.AddServerSideBlazor();
// Configure multipart/form-data upload size limits to avoid buffering extremely large requests
builder.Services.Configure<FormOptions>(options =>
{
    // Allow up to 210 MB for multipart form uploads (adjust if needed).
    options.MultipartBodyLengthLimit = 210 * 1024 * 1024; // 210 MB
    // Keep default buffer limits for key/value form data
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = 16384;
});

// Response caching to reduce repeat work for static/short-lived responses
builder.Services.AddResponseCaching();


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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseHsts();
}
app.UseHttpsRedirection();



app.UseSecurityHeaders();
app.UseResponseCompression();
app.UseIpRateLimiting();
app.UseCustomRateLimit();


// ⚠️ Nếu hosting đã tự HTTPS redirect, comment dòng này lại
// app.UseCustomHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// ✅ Đúng thứ tự: Authentication trước, Authorization sau
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("Content-Security-Policy");
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        // --- SCRIPT ---
        "script-src 'self' 'unsafe-inline' 'unsafe-eval' " +
        "https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://www.gstatic.com " +
        "https://translate.google.com https://translate.googleapis.com https://translate-pa.googleapis.com " +
        "https://za.zdn.vn https://sp.zalo.me https://page.widget.zalo.me " +
        "https://www.youtube.com https://www.chatbase.co; " +

        "script-src-elem 'self' 'unsafe-inline' " +
        "https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://www.gstatic.com " +
        "https://translate.google.com https://translate.googleapis.com https://translate-pa.googleapis.com " +
        "https://za.zdn.vn https://sp.zalo.me https://page.widget.zalo.me " +
        "https://www.youtube.com https://www.chatbase.co; " +

        "script-src-attr 'self' 'unsafe-inline'; " +

        // --- STYLE & FONT ---
        "style-src 'self' 'unsafe-inline' " +
        "https://fonts.googleapis.com https://cdn.jsdelivr.net https://cdnjs.cloudflare.com " +
        "https://page.widget.zalo.me https://www.gstatic.com; " +

        "font-src 'self' data: https://fonts.gstatic.com https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://page.widget.zalo.me; " +

        // --- IMAGES ---
        "img-src 'self' data: blob: https: " +
        "https://i.ytimg.com https://yt3.ggpht.com https://cdn-icons-png.flaticon.com https://page.widget.zalo.me; " +

        // --- CONNECT ---
        "connect-src 'self' " +
        "https://cdn.jsdelivr.net https://cdnjs.cloudflare.com " + // ✅ Thêm dòng này để fix lỗi
        "https://api.widget.zalo.me https://widget.chat.zalo.me " +
        "https://translate.googleapis.com https://translate-pa.googleapis.com " +
        "https://www.youtube.com https://play.google.com https://googleads.g.doubleclick.net " +
        "https://px.dmp.zaloapp.com https://www.chatbase.co wss://myphuochospital.com.vn; " +

        // --- FRAME, MEDIA ---
        "frame-src 'self' https://www.youtube.com https://page.widget.zalo.me https://www.chatbase.co; " +
        "media-src 'self' blob: data:; " +

        // --- BẢO MẬT CƠ BẢN ---
        "object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'self'; report-uri /api/csp-violation;";
    await next();
});


app.MapBlazorHub();
app.MapHub<Hubnot>("/friendHub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Trangquantri}/{action=Dangnhap}/{id?}");

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
