using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Views.Shared.Components
{
    public class Footertrangchu : ViewComponent
    {
        private readonly IPageMain _pagemain;
        private readonly IMemoryCache _memoryCache;
        private const string FooterCacheKey = "FooterCacheKey";

        public Footertrangchu(IPageMain pageMain, IMemoryCache memoryCache)
        {
            _pagemain = pageMain;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                if (!_memoryCache.TryGetValue(FooterCacheKey, out var footerData))
                {
                    footerData = await _pagemain.GetFooter();

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                    };

                    _memoryCache.Set(FooterCacheKey, footerData, cacheOptions);
                }

                return View(footerData);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
