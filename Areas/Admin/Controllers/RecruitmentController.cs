using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class RecruitmentController : Controller
    {
        private readonly IRecruitment _recruitment;
        public RecruitmentController(IRecruitment recruitment)
        {
            _recruitment = recruitment;
        }
        [Authorize]
        [HttpPost("/api-tuyen-dung-benh-vien")]
        public async Task<IActionResult> dangtintuyendung( RecruitmentpostVM recruitmentpost)
        {
            try
            {
                var data = await _recruitment.AddRecruitment(recruitmentpost);
                if (data == true)
                {
                    return Json(new { status = true, message = "Đăng tin tuyển dụng thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Đăng tin tuyển dụng thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        
        }
    }
}
