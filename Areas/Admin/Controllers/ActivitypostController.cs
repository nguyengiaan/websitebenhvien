using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="admin,Quanlyhoatdong")]

    public class ActivitypostController : Controller
    {
        private readonly IActivitypost _postActivityService;
        private readonly MyDbcontext _context;

        public ActivitypostController(IActivitypost postActivityService, MyDbcontext context)
        {
            _postActivityService = postActivityService;
            _context = context;
        }

        // GET: Admin/Activitypost

        [Route("/trang-quan-tri/quan-li-hoat-dong")]
        public async Task<IActionResult> Index(PostactivitySearchVM searchModel)
        {
            var result = await _postActivityService.GetPagedPostactivitiesAsync(searchModel);

            // Lấy danh sách categories cho dropdown filter
            var categories = await _context.Activitycategories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id_activitycategory", "Title", searchModel.CategoryId);

            // Truyền thông tin search để hiển thị trong form
            ViewBag.SearchTerm = searchModel.SearchTerm;
            ViewBag.CategoryId = searchModel.CategoryId;
            ViewBag.Status = searchModel.Status;

            return View(result);
        }

        // GET: Admin/Activitypost/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Activitycategories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id_activitycategory", "Title");

            return View(new PostactivityVM());
        }

        // POST: Admin/Activitypost/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostactivityVM model)
        {
            // Xử lý file thumbnail
            if (Request.Form.Files.Count > 0)
            {
                var thumbnailFile = Request.Form.Files.FirstOrDefault(f => f.Name == "ThumbnailFile");
                if (thumbnailFile != null && thumbnailFile.Length > 0)
                {
                    model.ThumbnailFile = thumbnailFile;
                }
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp title
                if (await _postActivityService.IsTitleExistsAsync(model.Title))
                {
                    ModelState.AddModelError("Title", "Tiêu đề đã tồn tại");
                }



                if (ModelState.IsValid)
                {
                    var result = await _postActivityService.AddPostactivityAsync(model);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Tạo hoạt động thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo hoạt động!";
                    }
                }
            }

            // Reload categories if validation fails
            var categories = await _context.Activitycategories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id_activitycategory", "Title", model.Id_Categoryactivity);

            return View(model);
        }

        // GET: Admin/Activitypost/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var postactivity = await _postActivityService.GetPostactivityByIdAsync(id);
            if (postactivity == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hoạt động!";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Activitycategories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id_activitycategory", "Title", postactivity.Id_Categoryactivity);

            return View(postactivity);
        }

        // POST: Admin/Activitypost/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostactivityVM model)
        {
            if (id != model.Id_Postactivity)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp title (loại trừ bản ghi hiện tại)
                if (await _postActivityService.IsTitleExistsAsync(model.Title, model.Id_Postactivity))
                {
                    ModelState.AddModelError("Title", "Tiêu đề đã tồn tại");
                }

                // Kiểm tra trùng lặp URL (loại trừ bản ghi hiện tại)
                if (!string.IsNullOrEmpty(model.Alias_url) && await _postActivityService.IsUrlExistsAsync(model.Alias_url, model.Id_Postactivity))
                {
                    ModelState.AddModelError("Alias_url", "URL đã tồn tại");
                }

                if (ModelState.IsValid)
                {
                    var result = await _postActivityService.UpdatePostactivityAsync(model);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Cập nhật hoạt động thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật hoạt động!";
                    }
                }
            }

            // Reload categories if validation fails
            var categories = await _context.Activitycategories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id_activitycategory", "Title", model.Id_Categoryactivity);

            return View(model);
        }

        // GET: Admin/Activitypost/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var postactivity = await _postActivityService.GetPostactivityByIdAsync(id);
            if (postactivity == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hoạt động!";
                return RedirectToAction(nameof(Index));
            }

            return View(postactivity);
        }

        // GET: Admin/Activitypost/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var postactivity = await _postActivityService.GetPostactivityByIdAsync(id);
            if (postactivity == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hoạt động!";
                return RedirectToAction(nameof(Index));
            }

            return View(postactivity);
        }

        // POST: Admin/Activitypost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _postActivityService.DeletePostactivityAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Xóa hoạt động thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa hoạt động!";
            }

            return RedirectToAction(nameof(Index));
        }

        // AJAX: Generate SEO Keywords based on Description
        [HttpPost]
        public IActionResult GenerateKeywords([FromBody] GenerateKeywordsRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Description))
                {
                    return Json(new { status = false, message = "Mô tả không được để trống" });
                }

                var keywords = Helper.SeoHelper.GenerateSeoKeywords(request.Title ?? "", request.Description);

                return Json(new
                {
                    status = true,
                    message = "Tạo từ khóa thành công",
                    data = new { keywords = keywords }
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra khi tạo từ khóa: " + ex.Message });
            }
        }

        // AJAX: Generate Schema Markup based on Description and other fields
        [HttpPost]
        public IActionResult GenerateSchemaMarkup([FromBody] GenerateSchemaRequest request)
        {
            try
            {
                // Validate all required fields
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return Json(new { status = false, message = "Tiêu đề không được để trống" });
                }

                if (string.IsNullOrWhiteSpace(request.Description))
                {
                    return Json(new { status = false, message = "Mô tả không được để trống" });
                }

                if (string.IsNullOrWhiteSpace(request.Url))
                {
                    return Json(new { status = false, message = "URL không được để trống" });
                }

                // Get the current request's base URL
                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                // Process thumbnail URL if provided
                var thumbnailUrl = request.Thumbnail;
                if (!string.IsNullOrEmpty(thumbnailUrl) && !thumbnailUrl.StartsWith("http"))
                {
                    thumbnailUrl = baseUrl + (thumbnailUrl.StartsWith("/") ? "" : "/") + thumbnailUrl;
                }

                var schemaMarkup = Helper.SeoHelper.GenerateSchemaMarkup(
                    request.Title,
                    request.Description,
                    request.Url,
                    thumbnailUrl,
                    DateTime.Now,
                    baseUrl
                );

                return Json(new
                {
                    status = true,
                    message = "Tạo Schema Markup thành công",
                    data = new { schemaMarkup = schemaMarkup }
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi xảy ra khi tạo Schema Markup: " + ex.Message });
            }
        }
    }

    // Request models for AJAX calls
    public class GenerateKeywordsRequest
    {
        public string? Title { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class GenerateSchemaRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Thumbnail { get; set; }
    }
}
