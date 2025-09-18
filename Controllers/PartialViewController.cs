using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers
{
    public class PartialViewController : Controller
    {
        private readonly IPageMain _pagemain;
        private readonly IMemoryCache _memoryCache;
        private const string HeaderCacheKey = "HeaderCacheKey";

        public PartialViewController(IPageMain pageMain, IMemoryCache memoryCache)
        {
            _pagemain = pageMain;
            _memoryCache = memoryCache;
        }

        public IActionResult ChatComponent()
        {
            return PartialView("_ChatComponent");
        }

        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                // Lấy danh sách thông báo từ database
                var notifications = new List<object>(); // Thay thế bằng logic thực tế
                return Json(notifications);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Có lỗi xảy ra khi tải thông báo" });
            }
        }
    }
}
