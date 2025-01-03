﻿using Microsoft.AspNetCore.Authorization;
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
        [Authorize]

        [Route("trang-quan-tri")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize("admin")]
        public IActionResult Cauhinhdautrang()
        {
            return View();
        }
        [Authorize]
        public IActionResult Slidetrang()
        {
            return View();
        }
        [Authorize]
        public IActionResult Chantrang()
        {
            return View();
        }
        [Authorize]
        public IActionResult Logokhachhang()
        {
            return View();
        }
        [Authorize]
        public IActionResult Chiasekhachhang()
        {
            return View();
        }
        [Authorize]
        public IActionResult Thoigianlamviec()

        {
            return View();
        }
        [Authorize]
        public  IActionResult Trangnoidung()
        {
            return View();
        }
        [Authorize]
        public IActionResult Danhmuctintuc()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-tin-tuc")]
        public IActionResult Tintuc()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-danh-muc-san-pham")]

        public IActionResult Danhmucsanpham()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-san-pham")]
        public IActionResult Sanpham()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-tai-khoan")]
        public IActionResult Taikhoan()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-tuyen-dung")]
        public IActionResult Tuyendung()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-tin-nhan")]
        public IActionResult TinNhan()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-chuyen-khoa")]
        public IActionResult Chuyenkhoa()
        {
            return View();
        }

        [Route("/dang-nhap")]
        public IActionResult Dangnhap()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-bac-si")]
        public IActionResult Bacsi()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/dang-ky-kham-benh")]
        public IActionResult Dangkykhambenh()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-tin-mau")]
        public IActionResult Tinmau()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-phan-quyen")]
        public IActionResult Phanquyen()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-chuc-nang")]
        public IActionResult Phanquyencn()
        {
            return View();
        }
        [Authorize]
        [Route("/trang-quan-tri/quan-ly-video")]
        public IActionResult Quanlyvideo()
        {
            return View();
        }




    }
}

