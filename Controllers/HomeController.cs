using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Data;
using websitebenhvien.Migrations;
using websitebenhvien.Models;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers;

public class HomeController : Controller
{

    
    private readonly ILogger<HomeController> _logger;
    private readonly IRecruitment _recruitment;
    private readonly MyDbcontext _context;
    private readonly IAllinone _allinone;
    private readonly IMemoryCache _cache;

    public HomeController(ILogger<HomeController> logger,IRecruitment recruitment,MyDbcontext context,IAllinone allinone, IMemoryCache cache)
    {
        _logger = logger;
        _recruitment= recruitment;
        _context = context;
        _allinone = allinone;
        _cache = cache;
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
        try
        {
            var cacheKey = $"PostDetail_{alias_url}";
            if (!_cache.TryGetValue(cacheKey, out News data))
            {
                var alias = "/chi-tiet-tin/" + alias_url;
                data = await _context.News.Where(x => x.Alias_url == alias).AsNoTracking().FirstOrDefaultAsync();
                if (data == null)
                {
                    return NotFound();
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, data, cacheEntryOptions);
            }

            return View(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching post detail");
            return NotFound();
        }
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

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
