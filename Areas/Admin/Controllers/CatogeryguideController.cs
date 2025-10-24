using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,danhmuchuongdan")]
    public class CatogeryguideController : Controller
    {
        private readonly ICatogeryguide _catogeryguide;

        public CatogeryguideController(ICatogeryguide catogeryguide)
        {
            _catogeryguide = catogeryguide;
        }
        [Route("/trang-quan-tri/danh-muc-huong-dan")]
        public IActionResult CatogeryguiderView()
        {
            return View();
        }
    [Authorize(Roles = "admin,danhmuchuongdan")]
        [HttpPost("/api-them-danh-muc-hd")]
        public async Task<IActionResult> AddCatogery(CatogeryguiderVM categorynews)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _catogeryguide.AddCatogery(categorynews);
                if (data.Item2)
                {
                    return Json(new { status = true, message = data.Item1});
                }
                return Json(new { status = false, message = data.Item1 });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // danh sách danh mục tin tức
    [Authorize(Roles = "admin,danhmuchuongdan")]
        [HttpGet("/lay-danh-muc-hd")]
        public async Task<IActionResult> ListCatogeryNews()
        {
            try
            {
                var data = await _catogeryguide.ListCatogery();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // xóa danh mục
    [Authorize(Roles = "admin,danhmuchuongdan")]
        [HttpPost("/api-xoa-danh-muc-huong-dan")]
        public async Task<IActionResult> DeleteCatogery(int id)
        {
            try
            {
                var data = await _catogeryguide.DeleteCatogery(id);
                if (!data.Item2)
                {
                    return Json(new { status = false, message = data.Item1 });
                }
                return Json(new { status = true, message = "Xóa danh mục thành công" });
             
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
