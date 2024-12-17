using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class SpecialtyController : Controller
    {
        private readonly ISpecialty _specialty;

        public SpecialtyController(ISpecialty specialty)
        {
            _specialty = specialty;

        }
        [HttpPost]
        public async Task<IActionResult> AddSpecialty(SpecialtyVM specialty)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                else
                {
                    var result = await _specialty.AddSpecialty(specialty);
                    return Json(new { status = result, message = result == true ? "Thêm chuyên khoa thành công" : "Thêm chuyên khoa thất bại" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
