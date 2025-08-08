using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReceivingAngleController : Controller
    {
        private readonly IReceivingAngle _receivingAngleService;

        public ReceivingAngleController(IReceivingAngle receivingAngleService)
        {
            _receivingAngleService = receivingAngleService;
        }

        // GET: Admin/ReceivingAngle
        [Route("/trang-quan-tri/goc-cam-nhan")]
        public async Task<IActionResult> Index(string searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                var receivingAngles = await _receivingAngleService.GetAllReceivingAnglesAsync(page, pageSize, searchTerm ?? string.Empty);
                var totalCount = await _receivingAngleService.GetTotalCountAsync(searchTerm);

                var model = new PaginatedReceivingAngleVM
                {
                    Items = receivingAngles,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    SearchTerm = searchTerm
                };

                return View(model);
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách góc tiếp nhận.";
                return View(new PaginatedReceivingAngleVM());
            }
        }

        // GET: Admin/ReceivingAngle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ReceivingAngle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceivingAngleVM receivingAngle)
        {
            if (ModelState.IsValid)
            {
                var result = await _receivingAngleService.CreateReceivingAngleAsync(receivingAngle);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm mới thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Thêm mới không thành công. Vui lòng thử lại.";
                }
            }
            return View(receivingAngle);
        }

        // GET: Admin/ReceivingAngle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID góc tiếp nhận.";
                return RedirectToAction("Index");
            }

            var receivingAngle = await _receivingAngleService.GetReceivingAngleByIdAsync(id.Value);
            if (receivingAngle == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy góc tiếp nhận.";
                return RedirectToAction("Index");
            }

            return View(receivingAngle);
        }

        // POST: Admin/ReceivingAngle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReceivingAngleVM receivingAngle)
        {
            if (id != receivingAngle.id)
            {
                TempData["ErrorMessage"] = "ID không khớp.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var result = await _receivingAngleService.UpdateReceivingAngleAsync(receivingAngle);
                if (result)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật không thành công. Vui lòng thử lại.";
                }
            }
            return View(receivingAngle);
        }

        // GET: Admin/ReceivingAngle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receivingAngle = await _receivingAngleService.GetReceivingAngleByIdAsync(id.Value);
            if (receivingAngle == null)
            {
                return NotFound();
            }

            return View(receivingAngle);
        }

        // GET: Admin/ReceivingAngle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receivingAngle = await _receivingAngleService.GetReceivingAngleByIdAsync(id.Value);
            if (receivingAngle == null)
            {
                return NotFound();
            }

            return View(receivingAngle);
        }

        // POST: Admin/ReceivingAngle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _receivingAngleService.DeleteReceivingAngleAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Xóa không thành công. Vui lòng thử lại.";
            }
            return RedirectToAction("Index");
        }

        // AJAX endpoint for getting details
        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var receivingAngle = await _receivingAngleService.GetReceivingAngleByIdAsync(id);
                if (receivingAngle == null)
                {
                    return NotFound();
                }

                return PartialView("_DetailsPartial", receivingAngle);
            }
            catch
            {
                return StatusCode(500, "Có lỗi xảy ra khi tải chi tiết.");
            }
        }
    }
}