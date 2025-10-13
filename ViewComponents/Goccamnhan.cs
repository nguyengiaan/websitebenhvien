using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.ViewComponents
{
    public class GoccamnhanViewComponent : ViewComponent
    {
        private readonly IFeedback _feedbackService;
        private readonly IMemoryCache _cache;
        private const string FEEDBACK_CACHE_KEY = "goccamnhan_feedbacks";
        private const int CACHE_DURATION_MINUTES = 30;

        public GoccamnhanViewComponent(IFeedback feedbackService, IMemoryCache cache)
        {
            _feedbackService = feedbackService;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // Kiểm tra cache trước
                if (!_cache.TryGetValue(FEEDBACK_CACHE_KEY, out List<FeedbackVM>? cachedFeedbacks) || cachedFeedbacks == null)
                {
                    // Nếu không có trong cache, lấy từ database
                    var (feedbacks, _) = await _feedbackService.GetAllFeedbacksAsync("", 1, 20);
                    
                    // Lưu vào cache với thời gian expire 30 phút
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_DURATION_MINUTES),
                        SlidingExpiration = TimeSpan.FromMinutes(10), // Gia hạn cache nếu được truy cập trong 10 phút
                        Priority = CacheItemPriority.Normal
                    };
                    
                    cachedFeedbacks = feedbacks ?? new List<FeedbackVM>();
                    _cache.Set(FEEDBACK_CACHE_KEY, cachedFeedbacks, cacheOptions);
                }

                return View(cachedFeedbacks);
            }
            catch
            {
                // Trả về danh sách rỗng nếu có lỗi
                return View(new List<FeedbackVM>());
            }
        }
    }
}