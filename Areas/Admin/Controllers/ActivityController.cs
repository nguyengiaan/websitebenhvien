using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("admin")]
    public class ActivityController : Controller
    {
        private readonly IActivity _activityService;

        public ActivityController(IActivity activityService)
        {
            _activityService = activityService;
        }

        [Route("/trang-quan-tri/quan-li-danh-muc-hoat-dong")]
        public async Task<IActionResult> Index(ActivityCategorySearchVM searchModel)
        {
            // Đặt giá trị mặc định nếu cần
            if (searchModel.Page <= 0) searchModel.Page = 1;
            if (searchModel.PageSize <= 0) searchModel.PageSize = 10;
            if (string.IsNullOrEmpty(searchModel.SortBy)) searchModel.SortBy = "title";
            if (string.IsNullOrEmpty(searchModel.SortDirection)) searchModel.SortDirection = "asc";

            var result = await _activityService.GetPagedActivityCategoriesAsync(searchModel);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActivityCategory(int id)
        {
            try
            {
                var activityCategory = await _activityService.GetActivityCategoryByIdAsync(id);
                if (activityCategory == null)
                {
                    return Json(new { status = false, message = "Không tìm thấy danh mục hoạt động" });
                }

                return Json(new { status = true, data = activityCategory });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivityCategory(ActivitycategoryVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value != null)
                                          .SelectMany(x => x.Value!.Errors)
                                          .Select(x => x.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors) });
                }

                // Kiểm tra tiêu đề đã tồn tại
                if (await _activityService.IsTitleExistsAsync(model.Title))
                {
                    return Json(new { status = false, message = "Tiêu đề này đã tồn tại" });
                }

                var result = await _activityService.AddActivityCategoryAsync(model);
                if (result)
                {
                    return Json(new { status = true, message = "Thêm danh mục hoạt động thành công" });
                }

                return Json(new { status = false, message = "Không thể thêm danh mục hoạt động" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivityCategory(ActivitycategoryVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value != null)
                                          .SelectMany(x => x.Value!.Errors)
                                          .Select(x => x.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors) });
                }

                // Kiểm tra danh mục có tồn tại
                if (!await _activityService.ActivityCategoryExistsAsync(model.Id_activitycategory))
                {
                    return Json(new { status = false, message = "Không tìm thấy danh mục hoạt động" });
                }

                // Kiểm tra tiêu đề đã tồn tại (loại trừ bản ghi hiện tại)
                if (await _activityService.IsTitleExistsAsync(model.Title, model.Id_activitycategory))
                {
                    return Json(new { status = false, message = "Tiêu đề này đã tồn tại" });
                }

                var result = await _activityService.UpdateActivityCategoryAsync(model);
                if (result)
                {
                    return Json(new { status = true, message = "Cập nhật danh mục hoạt động thành công" });
                }

                return Json(new { status = false, message = "Không thể cập nhật danh mục hoạt động" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActivityCategory(int id)
        {
            try
            {
                // Kiểm tra danh mục có tồn tại
                if (!await _activityService.ActivityCategoryExistsAsync(id))
                {
                    return Json(new { status = false, message = "Không tìm thấy danh mục hoạt động" });
                }

                var result = await _activityService.DeleteActivityCategoryAsync(id);
                if (result)
                {
                    return Json(new { status = true, message = "Xóa danh mục hoạt động thành công" });
                }

                return Json(new { status = false, message = "Không thể xóa danh mục hoạt động" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivityCategories()
        {
            try
            {
                var categories = await _activityService.GetAllActivityCategoriesAsync();
                return Json(new { status = true, data = categories });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
