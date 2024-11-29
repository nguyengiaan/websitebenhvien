using Microsoft.AspNetCore.Mvc;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangquantriController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public TrangquantriController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        [Route("trang-quan-tri")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cauhinhdautrang()
        {
            return View();
        }
        public IActionResult Slidetrang()
        {
            return View();
        }
        public IActionResult Chantrang()
        {
            return View();
        }
        public IActionResult Logokhachhang()
        {
            return View();
        }
        public IActionResult Chiasekhachhang()
        {
            return View();
        }
        public IActionResult Thoigianlamviec()
        {
            return View();
        }
        public  IActionResult Trangnoidung()
        {
            return View();
        }
        public IActionResult Danhmuctintuc()
        {
            return View();
        }
        [Route("/trang-quan-tri/quan-ly-tin-tuc")]
        public IActionResult Tintuc()
        {
            return View();
        }
        [Route("/trang-quan-tri/quan-ly-danh-muc-san-pham")]
        public IActionResult Danhmucsanpham()
        {
            return View();
        }
    }
}
