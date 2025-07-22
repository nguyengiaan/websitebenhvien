using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class MenuAdminController : Controller
    {
        private readonly IAdminmenu _adminmenu;
        public MenuAdminController(IAdminmenu adminmenu)
        {
            _adminmenu = adminmenu;
        }

        [Route("/trang-quan-tri/quan-li-menu-admin")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var result = await _adminmenu.Danhsachmenu(page, pageSize, searchTerm);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuAdminVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _adminmenu.Create(model);
            if (result)
            {
                TempData["Success"] = "Thêm menu thành công!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Có lỗi xảy ra khi thêm menu.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var menu = await _adminmenu.MenuId(id);
            if (menu == null)
            {
                TempData["Error"] = "Không tìm thấy menu!";
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuAdminVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _adminmenu.Update(model);
            if (result)
            {
                TempData["Success"] = "Cập nhật menu thành công!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Có lỗi xảy ra khi cập nhật menu.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _adminmenu.Delete(id);
            if (result)
            {
                TempData["Success"] = "Xóa menu thành công!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa menu.";
            }
            return RedirectToAction("Index");
        }

        // API cho tìm kiếm AJAX
        [HttpGet]
        public async Task<IActionResult> Search(string term, int page = 1, int pageSize = 10)
        {
            var result = await _adminmenu.Danhsachmenu(page, pageSize, term);
            return PartialView("_MenuListPartial", result);
        }
    }
}
