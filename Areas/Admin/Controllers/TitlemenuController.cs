using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("admin")]
    public class TitlemenuController : Controller
    {
        private readonly ITitlemenu _titlemenuService;

        public TitlemenuController(ITitlemenu titlemenuService)
        {
            _titlemenuService = titlemenuService;
        }

        [Route("/trang-quan-tri/quan-li-title-menu")]
        public async Task<IActionResult> Index(TitlemenuSearchVM searchModel)
        {
            // Đặt giá trị mặc định nếu cần
            if (searchModel.Page <= 0) searchModel.Page = 1;
            if (searchModel.PageSize <= 0) searchModel.PageSize = 10;
            if (string.IsNullOrEmpty(searchModel.SortBy)) searchModel.SortBy = "Name";
            if (string.IsNullOrEmpty(searchModel.SortDirection)) searchModel.SortDirection = "asc";

            var result = await _titlemenuService.GetPagedTitlemenusAsync(searchModel);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTitlemenu(int id)
        {
            try
            {
                var titlemenu = await _titlemenuService.GetTitlemenuByIdAsync(id);
                if (titlemenu == null)
                {
                    return Json(new { status = false, message = "Không tìm thấy title menu" });
                }

                return Json(new { status = true, data = titlemenu });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitlemenu(TitlemenuVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { status = false, message = "Dữ liệu không hợp lệ" });
                }

                // Kiểm tra tên đã tồn tại
                if (await _titlemenuService.IsNameExistsAsync(model.Name))
                {
                    return Json(new { status = false, message = "Tên title menu đã tồn tại" });
                }

                var result = await _titlemenuService.AddTitlemenuAsync(model);
                if (result)
                {
                    return Json(new { status = true, message = "Thêm title menu thành công" });
                }

                return Json(new { status = false, message = "Thêm title menu thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTitlemenu(TitlemenuVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { status = false, message = "Dữ liệu không hợp lệ" });
                }

                if (!model.Id_titlemenu.HasValue)
                {
                    return Json(new { status = false, message = "ID không hợp lệ" });
                }

                // Kiểm tra tên đã tồn tại (trừ record hiện tại)
                if (await _titlemenuService.IsNameExistsAsync(model.Name, model.Id_titlemenu))
                {
                    return Json(new { status = false, message = "Tên title menu đã tồn tại" });
                }

                var result = await _titlemenuService.UpdateTitlemenuAsync(model);
                if (result)
                {
                    return Json(new { status = true, message = "Cập nhật title menu thành công" });
                }

                return Json(new { status = false, message = "Cập nhật title menu thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTitlemenu(int id)
        {
            try
            {
                if (!await _titlemenuService.TitlemenuExistsAsync(id))
                {
                    return Json(new { status = false, message = "Title menu không tồn tại" });
                }

                var result = await _titlemenuService.DeleteTitlemenuAsync(id);
                if (result)
                {
                    return Json(new { status = true, message = "Xóa title menu thành công" });
                }

                return Json(new { status = false, message = "Xóa title menu thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTitlemenus()
        {
            try
            {
                var titlemenus = await _titlemenuService.GetAllTitlemenusAsync();
                return Json(new { status = true, data = titlemenus });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
