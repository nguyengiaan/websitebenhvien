using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,huongdankh")]

    public class CustomerguideController : Controller
    {
        private readonly ICustomeguider _customeguider;

        public CustomerguideController(ICustomeguider customeguider)
        {
            _customeguider = customeguider;
        }
        [Route("/trang-quan-tri/huong-dan-khach-hang")]
        public IActionResult Cusguide()
        {
            return View();
        }
        [Authorize(Roles = "admin,huongdankh")]
        [HttpPost]
        public async Task<IActionResult> AddNews([FromForm] CustomerguideVM news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _customeguider.AddCustomer(news);
                if (data.Item2)
                {
                    return Json(new { status = data.Item2, message = data.Item1 });
                }
                return Json(new { status = data.Item2, message = data.Item1 });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,huongdankh")]
        [HttpPost]
        public async Task<IActionResult> Listnews(string search, int page, int pagesize)
        {
            try
            {
                var data = await _customeguider.Listnews(search, page, pagesize);
                return Json(new { status = true, data = data.ds, pageindex = page, totalpage = data.Totalpages });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,huongdankh")]
        [HttpPost]
        public async Task<IActionResult> DeleteNews(int id)
        {
            try
            {
                var data = await _customeguider.DeleteCustomerguide(id);
                return Json(new { status = true, message = "Xóa tin tức thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,huongdankh")]
        [HttpPost]
        public async Task<IActionResult> GetNewsById(int id)
        {
            try
            {
                var data = await _customeguider.GetCustomerById(id);
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }


    }
}
