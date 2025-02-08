using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Migrations;
using websitebenhvien.Models;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers;

public class HomeController : Controller
{

    
    private readonly ILogger<HomeController> _logger;
    private readonly IRecruitment _recruitment;

    public HomeController(ILogger<HomeController> logger,IRecruitment recruitment)
    {
        _logger = logger;
        _recruitment= recruitment;
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
