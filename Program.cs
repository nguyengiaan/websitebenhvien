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
builder.Services.AddRazorPages();
builder.Services.AddScoped<IPageMain,PageMainReponser>();
builder.Services.AddScoped<IAllinone, AllinoneReponser>();  
builder.Services.AddScoped<IUser, UserReponser>();
builder.Services.Configure<FileSystemConfig>(builder.Configuration.GetSection(FileSystemConfig.ConfigName));
builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("user", policy => policy.RequireRole("user"));
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/trang-quan-tri/dang-nhap";
    options.AccessDeniedPath = "/trang-quan-tri/dang-nhap";
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/trang-quan-tri/dang-nhapp";
            options.LogoutPath = "/trang-quan-tri/dang-nhap";
            options.Cookie.HttpOnly = true;
            options.Cookie.Expiration = TimeSpan.FromDays(1);
            options.SlidingExpiration = true;
        });

var app = builder.Build();

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

app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadsPath),
        RequestPath = "/Resources"
    }
);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Route cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Trangquantri}/{action=Dangnhap}/{id?}");

// Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

