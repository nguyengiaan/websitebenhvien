using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Data;
using websitebenhvien.Migrations;
using websitebenhvien.Models;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers;

public class HomeController : Controller
{

    
    private readonly ILogger<HomeController> _logger;
    private readonly IRecruitment _recruitment;
    private readonly MyDbcontext _context;
    private readonly IAllinone _allinone;
    private readonly IMemoryCache _cache;
    private readonly IForbusiness _forbusiness;

    public HomeController(ILogger<HomeController> logger,IRecruitment recruitment,MyDbcontext context,IAllinone allinone, IMemoryCache cache,IForbusiness forbusiness)
    {
        _logger = logger;
        _recruitment= recruitment;
        _context = context;
        _allinone = allinone;
        _cache = cache;
        _forbusiness= forbusiness;
    }
    public IActionResult Index()
    {
        ViewData["Title"] = "Bệnh Viện Đa Khoa Mỹ Phước";
        ViewData["Description"] = "Bệnh Viện Đa Khoa Mỹ Phước cung cấp dịch vụ y tế chất lượng cao với đội ngũ bác sĩ chuyên nghiệp và trang thiết bị hiện đại.";
        ViewData["Keywords"] = "Bệnh Viện Đa Khoa, Mỹ Phước, dịch vụ y tế, bác sĩ chuyên nghiệp, trang thiết bị hiện đại";
        ViewData["Image"] = "https://benhvienmyphuoc.com.vn/Images/Logo-rm.png";
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    [HttpGet("/bai-viet/{catogery}")]
    public IActionResult PostCategory()
    
    {
       
        return View();
    }
    [HttpGet()]
    [Route("/chi-tiet-tin/{alias_url}")]
   public async Task<IActionResult> PostDetail(string alias_url)
{
    // Không nên hard‑code chuỗi “/chi-tiet-tin/” ở nhiều nơi
    const string DetailPrefix = "/chi-tiet-tin/";

    var cacheKey = $"PostDetail_{alias_url}";

    // GetOrCreateAsync ngắn gọn, tránh 2 lần hit cache
    var post = await _cache.GetOrCreateAsync(cacheKey, async entry =>
    {
        entry.SlidingExpiration = TimeSpan.FromMinutes(30);

        // Truy vấn “1 phát ăn ngay”: Include để lấy luôn thể loại
        return await _context.News
            .AsNoTracking()
            .Include(n => n.Categorynews)
            .FirstOrDefaultAsync(
                n => n.Alias_url == DetailPrefix + alias_url);
    });

    if (post is null) return NotFound();

    // post.Categorynews có sẵn nhờ Include phía trên
    if (post.Categorynews is not null)
    {
            ViewBag.breadcrumbs = new List<(string Title, string Url)>
             {
                new("Trang chủ", "/"),
                new(post.Categorynews.Title, post.Categorynews.Alias_url),
                new(post.Title, post.Alias_url)
                };
     }

    return View(post);
}

    [HttpGet("/chuyen-khoa")]
    public IActionResult Specialty()
    {
        return View();
    }
    [HttpGet("/danh-muc-khoa/{chuyenkhoa}")]
    public IActionResult SpecialtyDetail()
    {
        return View();
    }
    [HttpGet("/bac-si/{tenbacsi}")]
    public IActionResult DoctorDetail()
    {
        return View();
    }
    [HttpGet("/tuyen-dung")]
    public async Task<IActionResult> tuyendung()
    {
        try
        {
            ViewData["Title"] = "Tuyển dụng - Bệnh viện Mỹ Phước";
            var data = await _recruitment.Getallpostrecruiment();
            if(data != null)
            {
                return View(data);
            }
            return View(new List<Postpersonnel>());
        }
        catch (Exception ex)
        {
            return Json(new { status = false, message = ex.Message });
        }
    }


    [Route("/danh-cho-doanh-nghiep")]
    public async Task<IActionResult> Danhchodoanhnghiep()
    {
        try
        {
            var cacheKey = "Danhchodoanhnghiep";
            if (!_cache.TryGetValue(cacheKey, out var data))
            {
                data = await _forbusiness.Getbusinesstrue();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, data, cacheEntryOptions);
            }

            return View(data);
        }
        catch (Exception e)
        {
            return Json(new { status = false, message = e.Message });
        }
    }
    [Route("/performance-test")]
    public IActionResult PerformanceTest()
    {
        ViewData["Title"] = "Performance Test - Bệnh viện Mỹ Phước";
        ViewData["Description"] = "Kiểm tra hiệu suất website bệnh viện với các tối ưu hóa render-blocking resources và caching";
        ViewData["Keywords"] = "performance, tối ưu, render-blocking, website bệnh viện, tốc độ tải";
        ViewData["Image"] = "https://benhvienmyphuoc.com.vn/Images/Logo-rm.png";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
