using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models;

namespace websitebenhvien.Controllers;

public class HomeController : Controller
{
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
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
    [HttpGet("/chi-tiet-tin/{alias_url}")]
    public IActionResult PostDetail()
    {
        return View();
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
    public IActionResult tuyendung()
    {
        return View();
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
