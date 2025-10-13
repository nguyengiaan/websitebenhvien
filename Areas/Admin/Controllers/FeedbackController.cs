using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Helper;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,Quanlygoccamnhan")]
    public class FeedbackController : Controller
    {
        private readonly IFeedback _feedbackService;
        private readonly IMemoryCache _cache;

        public FeedbackController(IFeedback feedbackService, IMemoryCache cache)
        {
            _feedbackService = feedbackService;
            _cache = cache;
        }

        // View: Trang quản lý góc cảm nhận
        [Route("/trang-quan-tri/quan-ly-goc-cam-nhan")]
        public IActionResult goccamnhan()
        {
            return View();
        }

        // API: Lấy danh sách feedbacks với phân trang và tìm kiếm
        [HttpGet("/api-get-feedbacks")]
        public async Task<IActionResult> GetFeedbacks(string search = "", int page = 1, int pagesize = 10)
        {
            try
            {
                var (feedbacks, totalPages) = await _feedbackService.GetAllFeedbacksAsync(search, page, pagesize);
                
                return Json(new { 
                    status = true, 
                    data = new { 
                        ds = feedbacks, 
                        totalPages = totalPages 
                    } 
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        // API: Lấy chi tiết feedback theo ID
        [HttpGet("/api-get-feedback/{id}")]
        public async Task<IActionResult> GetFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
                if (feedback != null)
                {
                    return Json(new { status = true, data = feedback });
                }
                return Json(new { status = false, message = "Không tìm thấy dữ liệu" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        // API: Thêm/Sửa feedback
        [HttpPost("/api-save-feedback")]
        public async Task<IActionResult> SaveFeedback(FeedbackVM model)
        {
            try
            {
                // Sử dụng AddFeedbackAsync cho cả thêm và sửa (method này đã xử lý logic)
                var result = await _feedbackService.AddFeedbackAsync(model);
                
                if (result.Success)
                {
                    // Clear cache khi có thay đổi dữ liệu
                    _cache.ClearFeedbackCache();
                }
                
                return Json(new { status = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        // API: Xóa feedback
        [HttpPost("/api-delete-feedback")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedbackAsync(id);
                if (result)
                {
                    // Clear cache khi có thay đổi dữ liệu
                    _cache.ClearFeedbackCache();
                    return Json(new { status = true, message = "Xóa thành công" });
                }
                return Json(new { status = false, message = "Xóa thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
