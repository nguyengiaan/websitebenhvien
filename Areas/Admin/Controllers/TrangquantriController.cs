using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class TrangquantriController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public TrangquantriController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }

        [Authorize(Roles = "admin,Trangquantri")]
        [Route("trang-quan-tri")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "admin,Dautrang")]
        public IActionResult Cauhinhdautrang()
        {
            return View();
        }
        [Authorize(Roles = "admin,Slidetrangchu")]
        public IActionResult Slidetrang()
        {
            return View();
        }
        [Authorize(Roles = "admin,Chantrang")]
        public IActionResult Chantrang()
        {
            return View();
        }
        [Authorize(Roles = "admin,Logokhachhang")]
        public IActionResult Logokhachhang()
        {
            return View();
        }
        [Authorize(Roles = "admin,Chiasekhachhang")]
        public IActionResult Chiasekhachhang()
        {
            return View();
        }
        [Authorize(Roles = "admin,Thoigianlamviec")]
        public IActionResult Thoigianlamviec()

        {
            return View();
        }
        [Authorize(Roles = "admin,Danhsachmenu")]
        public  IActionResult Trangnoidung()
        {
            return View();
        }
        [Authorize(Roles = "admin,Danhmucbaiviet")]
        public IActionResult Danhmuctintuc()
        {
            return View();
        }
        [Authorize(Roles = "admin,Baiviet")]
        [Route("/trang-quan-tri/quan-ly-tin-tuc")]
        public IActionResult Tintuc()
        {
            return View();
        }
        [Authorize(Roles = "admin,Danhmucsanpham")]
        [Route("/trang-quan-tri/quan-ly-danh-muc-san-pham")]

        public IActionResult Danhmucsanpham()
        {
            return View();
        }
        [Authorize(Roles = "admin,Sanpham")]
        [Route("/trang-quan-tri/quan-ly-san-pham")]
        public IActionResult Sanpham()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [Route("/trang-quan-tri/quan-ly-tai-khoan")]
        public IActionResult Taikhoan()
        {
            return View();
        }
        [Authorize(Roles ="Quanlytuyendung,admin")]
        [Route("/trang-quan-tri/quan-ly-tuyen-dung")]
        public IActionResult Tuyendung()
        {
            return View();
        }
        [Authorize(Roles ="Danhsachtinnhan,admin")]
        [Route("/trang-quan-tri/quan-ly-tin-nhan")]
        public IActionResult TinNhan()
        {
            return View();
        }
        [Authorize(Roles ="Quanlychuyenkhoa,admin")]
        [Route("/trang-quan-tri/quan-ly-chuyen-khoa")]
        public IActionResult Chuyenkhoa()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/dang-nhap")]
        public IActionResult Dangnhap()
        {
            return View();
        }
        [Authorize(Roles ="Quanlybacsi,admin")]
        [Route("/trang-quan-tri/quan-ly-bac-si")]
        public IActionResult Bacsi()
        {
            return View();
        }
        [Authorize(Roles ="Dangkykhambenh,admin")]
        [Route("/trang-quan-tri/dang-ky-kham-benh")]
        public IActionResult Dangkykhambenh()
        {
            return View();
        }
        [Authorize(Roles ="Tinnhanmau,admin")]
        [Route("/trang-quan-tri/quan-ly-tin-mau")]
        public IActionResult Tinmau()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [Route("/trang-quan-tri/quan-ly-phan-quyen")]
        public IActionResult Phanquyen()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [Route("/trang-quan-tri/quan-ly-chuc-nang")]
        public IActionResult Phanquyencn()
        {
            return View();
        }
        [Authorize(Roles ="Quanlyvideo,admin")]
        [Route("/trang-quan-tri/quan-ly-video")]
        public IActionResult Quanlyvideo()
        {
            return View();
        }
        [Authorize(Roles ="Dangkykhamsuckhoe,admin")]
        [Route("/trang-quan-tri/dang-ky-kham-sk")]
        public IActionResult Dangkykhambenhsk()
        {
            return View();
        }

        [Authorize]
        [Route("/trang-loi")]
        public IActionResult Trangloi()
        {
            return View();
        }
        [Authorize(Roles ="Dangtintuyendung,admin")]
        [Route("/trang-quan-tri/dang-tin-tuyen-dung")]
        public IActionResult Dangtintuyendung()
        {
            return View();
        }
        [Authorize(Roles ="Quanlythietbi,admin")]
        [Route("/trang-quan-tri/Quan-ly-thiet-bi")]
        public IActionResult Quanlythietbi()
        {
            return View();
        }
        [Authorize(Roles = "Quanlyemail,admin")]
        [Route("/trang-quan-tri/Quan-ly-email")]
        public IActionResult Quanlyemail()
        {
            return View();
        }
        [Authorize(Roles = "Quanlydoanhnghiep,admin")]
        [Route("/trang-quan-tri/Quan-ly-doanh-nghiep")]
        public IActionResult Quanlydoanhnghiep()
        {
            return View();
        }



    }
}

