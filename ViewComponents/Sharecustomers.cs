using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.EnitityVM;
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
        if (!_memoryCache.TryGetValue(ShareCacheKey, out IEnumerable<LogocustomerVM> shareData))
        {
            // Lấy dữ liệu từ service
            shareData = await _allinone.ListShareCustomer();
            
            // Kiểm tra nếu dữ liệu null
            if (shareData == null)
            {
                shareData = Enumerable.Empty<LogocustomerVM>(); // hoặc xử lý phù hợp với logic của bạn
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
        // Log the exception here if needed
        return View(Enumerable.Empty<LogocustomerVM>()); // Trả về collection rỗng thay vì thông báo lỗi
    }
}
    }
}