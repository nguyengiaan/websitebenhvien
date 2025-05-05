using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.ViewComponents
{
    public class SharecustomersViewComponent : ViewComponent
    {
        private readonly IAllinone _allinone;
        private readonly IMemoryCache _memoryCache;
        private const string ShareCacheKey = "ShareCustomerData";

        public SharecustomersViewComponent(IAllinone allinone, IMemoryCache memoryCache)
        {
            _allinone = allinone ?? throw new ArgumentNullException(nameof(allinone));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

          public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // Kiểm tra dữ liệu trong cache
                if (!_memoryCache.TryGetValue(ShareCacheKey, out var shareData))
                {
                    // Lấy dữ liệu từ service
                    shareData = await _allinone.ListShareCustomer();
                    if (shareData == null )
                    {
                        return View("Error", "No data available.");
                    }

                    // Cấu hình cache
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    };

                    // Lưu dữ liệu vào cache
                    _memoryCache.Set(ShareCacheKey, shareData, cacheOptions);
                }

                // Trả về View với dữ liệu
                return View(shareData);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu cần) và trả về View lỗi
                return View("Error", ex.Message);
            }
        }
    }
}