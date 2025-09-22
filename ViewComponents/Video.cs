using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Service.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace websitebenhvien.ViewComponents
{
    public class VideoViewComponent : ViewComponent
    {
    private readonly IRecruitment _recruitment;
    private readonly IMemoryCache _cache;

        public VideoViewComponent(IRecruitment recruitment, IMemoryCache cache)
        {
            _recruitment = recruitment;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                string cacheKey = "VideoViewComponent_Videos";
                if (!_cache.TryGetValue(cacheKey, out var video))
                {
                    video = await _recruitment.GetVideosClient();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    _cache.Set(cacheKey, video, cacheEntryOptions);
                }
                return View(video);
            }
            catch (Exception)
            {
                return View(null);
            }
        }
    }
}